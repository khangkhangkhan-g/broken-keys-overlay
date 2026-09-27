using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace BrokenKeysOverlay;

public sealed class OverlayForm : Form
{
    private const int WS_EX_TOOLWINDOW = 0x00000080;
    private const int WS_EX_NOACTIVATE = 0x08000000;
    private const int WM_MOUSEACTIVATE = 0x0021;
    private const int MA_NOACTIVATE = 3;

    private const int OuterPadding = 5;
    private const int Gap = 3;

    private AppSettings _settings;
    private readonly Label _grip;
    private readonly NotifyIcon _tray;
    private readonly GlobalKeyboardHook _hook;
    private readonly List<KeyDefinition> _latchedModifiers = new();
    private readonly Dictionary<string, KeyTile> _tiles = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<Control> _dynamicControls = new();

    private bool _dragging;
    private Point _dragStartCursor;
    private Point _dragStartForm;

    protected override bool ShowWithoutActivation => true;

    protected override CreateParams CreateParams
    {
        get
        {
            var cp = base.CreateParams;
            cp.ExStyle |= WS_EX_TOOLWINDOW | WS_EX_NOACTIVATE;
            return cp;
        }
    }

    public OverlayForm()
    {
        _settings = SettingsService.Load();

        Text = "Broken Keys Overlay";
        FormBorderStyle = FormBorderStyle.None;
        StartPosition = FormStartPosition.Manual;
        ShowInTaskbar = false;
        TopMost = true;
        BackColor = Color.FromArgb(31, 31, 33);
        AutoScaleMode = AutoScaleMode.Dpi;
        Opacity = Math.Clamp(_settings.Opacity, 0.45, 1.0);
        MaximizeBox = false;
        MinimizeBox = false;

        _grip = new Label
        {
            Text = "⋮⋮",
            ForeColor = Color.FromArgb(145, 145, 151),
            BackColor = Color.Transparent,
            Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
            Width = 15,
            Height = _settings.KeySize,
            TextAlign = ContentAlignment.MiddleCenter,
            Cursor = Cursors.SizeAll,
            TabStop = false
        };
        _grip.MouseDown += Grip_MouseDown;
        _grip.MouseMove += Grip_MouseMove;
        _grip.MouseUp += (_, _) => SavePositionAndStopDragging();
        _grip.DoubleClick += (_, _) => OpenSettings();
        Controls.Add(_grip);

        var gripMenu = new ContextMenuStrip();
        gripMenu.Items.Add("Edit broken keys…", null, (_, _) => OpenSettings());
        gripMenu.Items.Add("Reset pending modifiers", null, (_, _) => ClearLatchedModifiers());
        _grip.ContextMenuStrip = gripMenu;

        _tray = new NotifyIcon
        {
            Text = "Broken Keys Overlay",
            Icon = SystemIcons.Application,
            Visible = true,
            ContextMenuStrip = BuildTrayMenu()
        };
        _tray.DoubleClick += (_, _) => OpenSettings();

        _hook = new GlobalKeyboardHook
        {
            GetLatchedModifiers = () => _latchedModifiers.ToArray(),
            OnChordConsumed = () =>
            {
                if (!IsDisposed && IsHandleCreated)
                    BeginInvoke(ClearLatchedModifiers);
            }
        };

        Load += (_, _) =>
        {
            RebuildOverlay();
            RestorePosition();
            if (_settings.SelectedKeyIds.Count == 0)
                BeginInvoke(OpenSettings);
        };

        FormClosing += (_, _) =>
        {
            SaveCurrentSettings();
            _hook.Dispose();
            _tray.Visible = false;
            _tray.Dispose();
        };
    }

    protected override void WndProc(ref Message m)
    {
        // Same core model used by floating input surfaces: clicking the palette
        // must not transfer foreground activation away from the app being typed in.
        if (m.Msg == WM_MOUSEACTIVATE)
        {
            m.Result = (IntPtr)MA_NOACTIVATE;
            return;
        }
        base.WndProc(ref m);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using var pen = new Pen(Color.FromArgb(67, 67, 72));
        var rect = new Rectangle(0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
        if (rect.Width > 4 && rect.Height > 4)
        {
            using var path = RoundedRect(rect, 10);
            e.Graphics.DrawPath(pen, path);
        }
    }

    private ContextMenuStrip BuildTrayMenu()
    {
        var menu = new ContextMenuStrip();
        menu.Items.Add("Edit broken keys…", null, (_, _) => OpenSettings());
        menu.Items.Add("Reset pending modifiers", null, (_, _) => ClearLatchedModifiers());
        menu.Items.Add(new ToolStripSeparator());
        menu.Items.Add("Exit", null, (_, _) => Close());
        return menu;
    }

    private void RebuildOverlay()
    {
        SuspendLayout();

        foreach (var control in _dynamicControls)
        {
            Controls.Remove(control);
            control.Dispose();
        }
        _dynamicControls.Clear();
        _tiles.Clear();

        _grip.Height = _settings.KeySize;

        foreach (var id in _settings.SelectedKeyIds)
        {
            var key = KeyCatalog.Find(id);
            if (key is null) continue;

            var tile = new KeyTile(key, _settings.KeySize);
            tile.KeyPressed += (_, _) => HandleKeyClick(key);
            tile.SendAloneRequested += (_, _) => SendModifierAlone(key);
            _tiles[key.Id] = tile;
            _dynamicControls.Add(tile);
            Controls.Add(tile);
        }

        var edit = new KeyDefinition("__edit", "Settings", 0, "Internal", OverlayText: "⚙");
        var editTile = new KeyTile(edit, Math.Max(30, _settings.KeySize - 2))
        {
            Cursor = Cursors.Hand,
            RepeatEnabled = false
        };
        editTile.KeyPressed += (_, _) => OpenSettings();
        _dynamicControls.Add(editTile);
        Controls.Add(editTile);

        LayoutCompactStrip();
        ResumeLayout(false);
        RefreshLatchedVisuals();
        Invalidate();
    }

    private void LayoutCompactStrip()
    {
        var screen = Screen.FromPoint(Location == Point.Empty ? Cursor.Position : Location).WorkingArea;

        // Similar intent to Windows' undocked/small keyboard: stay movable and
        // compact instead of growing across the whole display. Wrap only when the
        // selected replacement keys would exceed roughly half of the work area.
        int maxWidth = Math.Clamp((int)(screen.Width * 0.55), 280, 720);
        int rowHeight = Math.Clamp(_settings.KeySize, 30, 48);

        int x = OuterPadding;
        int y = OuterPadding;
        int maxRight = 0;
        int maxBottom = 0;

        _grip.SetBounds(x, y, 15, rowHeight);
        x += _grip.Width + Gap;
        maxRight = _grip.Right;
        maxBottom = _grip.Bottom;

        foreach (var control in _dynamicControls)
        {
            if (x + control.Width + OuterPadding > maxWidth && x > OuterPadding + _grip.Width + Gap)
            {
                x = OuterPadding;
                y += rowHeight + Gap;
            }

            int top = y + Math.Max(0, (rowHeight - control.Height) / 2);
            control.Location = new Point(x, top);
            x += control.Width + Gap;
            maxRight = Math.Max(maxRight, control.Right);
            maxBottom = Math.Max(maxBottom, control.Bottom);
        }

        ClientSize = new Size(
            Math.Max(58, maxRight + OuterPadding),
            Math.Max(rowHeight + OuterPadding * 2, maxBottom + OuterPadding));

        ApplyRoundedRegion();
    }

    private void ApplyRoundedRegion()
    {
        if (ClientSize.Width < 4 || ClientSize.Height < 4) return;
        using var path = RoundedRect(new Rectangle(0, 0, ClientSize.Width, ClientSize.Height), 10);
        var old = Region;
        Region = new Region(path);
        old?.Dispose();
    }

    private void HandleKeyClick(KeyDefinition key)
    {
        try
        {
            if (key.IsModifier)
            {
                ToggleModifier(key);
                return;
            }

            if (_latchedModifiers.Count > 0)
            {
                InputInjector.SendChord(_latchedModifiers, key);
                ClearLatchedModifiers();
            }
            else
            {
                InputInjector.Press(key);
            }
        }
        catch (Exception ex)
        {
            _tray.ShowBalloonTip(2500, "Key injection failed", ex.Message, ToolTipIcon.Warning);
        }
    }

    private void ToggleModifier(KeyDefinition key)
    {
        var existing = _latchedModifiers.FirstOrDefault(k => k.Id.Equals(key.Id, StringComparison.OrdinalIgnoreCase));
        if (existing is not null)
            _latchedModifiers.Remove(existing);
        else
            _latchedModifiers.Add(key);

        RefreshLatchedVisuals();
    }

    private void SendModifierAlone(KeyDefinition key)
    {
        try
        {
            _latchedModifiers.RemoveAll(k => k.Id.Equals(key.Id, StringComparison.OrdinalIgnoreCase));
            RefreshLatchedVisuals();
            InputInjector.TapModifier(key);
        }
        catch (Exception ex)
        {
            _tray.ShowBalloonTip(2500, "Key injection failed", ex.Message, ToolTipIcon.Warning);
        }
    }

    private void ClearLatchedModifiers()
    {
        _latchedModifiers.Clear();
        RefreshLatchedVisuals();
    }

    private void RefreshLatchedVisuals()
    {
        foreach (var tile in _tiles.Values)
        {
            tile.Latched = _latchedModifiers.Any(k => k.Id.Equals(tile.Key.Id, StringComparison.OrdinalIgnoreCase));
            tile.Invalidate();
        }
    }

    private void OpenSettings()
    {
        ClearLatchedModifiers();
        using var dialog = new SettingsForm(_settings);
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            _settings = dialog.Result;
            Opacity = Math.Clamp(_settings.Opacity, 0.45, 1.0);
            RebuildOverlay();
            RestorePosition(clampOnly: true);
            SettingsService.Save(_settings);
        }
    }

    private void Grip_MouseDown(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left) return;
        _dragging = true;
        _dragStartCursor = Cursor.Position;
        _dragStartForm = Location;
    }

    private void Grip_MouseMove(object? sender, MouseEventArgs e)
    {
        if (!_dragging || e.Button != MouseButtons.Left) return;
        var now = Cursor.Position;
        Location = new Point(
            _dragStartForm.X + now.X - _dragStartCursor.X,
            _dragStartForm.Y + now.Y - _dragStartCursor.Y);
    }

    private void SavePositionAndStopDragging()
    {
        if (!_dragging) return;
        _dragging = false;
        ClampToVisibleScreen();
        _settings.OverlayX = Left;
        _settings.OverlayY = Top;
        SettingsService.Save(_settings);
    }

    private void RestorePosition(bool clampOnly = false)
    {
        var working = Screen.FromPoint(Cursor.Position).WorkingArea;

        if (!clampOnly && _settings.OverlayX >= 0 && _settings.OverlayY >= 0)
            Location = new Point(_settings.OverlayX, _settings.OverlayY);
        else if (!clampOnly)
            Location = new Point(working.Right - Width - 24, working.Bottom - Height - 24);

        ClampToVisibleScreen();
    }

    private void ClampToVisibleScreen()
    {
        var screen = Screen.FromRectangle(Bounds).WorkingArea;
        int x = Math.Min(Math.Max(Left, screen.Left), Math.Max(screen.Left, screen.Right - Width));
        int y = Math.Min(Math.Max(Top, screen.Top), Math.Max(screen.Top, screen.Bottom - Height));
        Location = new Point(x, y);
    }

    private void SaveCurrentSettings()
    {
        _settings.OverlayX = Left;
        _settings.OverlayY = Top;
        SettingsService.Save(_settings);
    }

    private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
    {
        int d = radius * 2;
        var path = new GraphicsPath();
        path.AddArc(bounds.Left, bounds.Top, d, d, 180, 90);
        path.AddArc(bounds.Right - d, bounds.Top, d, d, 270, 90);
        path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
        path.AddArc(bounds.Left, bounds.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}
