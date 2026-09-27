using System.Drawing.Drawing2D;

namespace BrokenKeysOverlay;

/// <summary>
/// Compact, non-focusable keycap used by the floating replacement strip.
/// The visual language intentionally follows a small keyboard keycap rather
/// than a normal desktop command button.
/// </summary>
public sealed class KeyTile : Control
{
    private readonly System.Windows.Forms.Timer _repeatDelayTimer;
    private readonly System.Windows.Forms.Timer _repeatTimer;
    private bool _pressed;

    public KeyDefinition Key { get; }
    public bool Latched { get; set; }
    public bool RepeatEnabled { get; set; } = true;
    public event EventHandler? KeyPressed;
    public event EventHandler? SendAloneRequested;

    public KeyTile(KeyDefinition key, int size)
    {
        Key = key;
        int height = Math.Clamp(size, 30, 48);
        string display = key.OverlayLabel;

        using var font = CreateKeyFont(height);
        var measured = TextRenderer.MeasureText(display, font, new Size(240, height), TextFormatFlags.NoPadding);

        // Keep ordinary keys square-ish. Wider labels only receive as much width
        // as they need; this is what stops Backspace/Ctrl/etc. becoming huge tiles.
        int horizontalPadding = height <= 34 ? 12 : 14;
        Width = Math.Clamp(Math.Max(height, measured.Width + horizontalPadding), height, 88);
        Height = height;
        Margin = new Padding(2);
        Cursor = Cursors.Hand;
        DoubleBuffered = true;
        TabStop = false;
        SetStyle(ControlStyles.Selectable, false);
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);

        _repeatDelayTimer = new System.Windows.Forms.Timer { Interval = 430 };
        _repeatDelayTimer.Tick += (_, _) =>
        {
            _repeatDelayTimer.Stop();
            if (_pressed && RepeatEnabled && !Key.IsModifier)
            {
                KeyPressed?.Invoke(this, EventArgs.Empty);
                _repeatTimer.Start();
            }
        };

        _repeatTimer = new System.Windows.Forms.Timer { Interval = 58 };
        _repeatTimer.Tick += (_, _) =>
        {
            if (_pressed && RepeatEnabled && !Key.IsModifier)
                KeyPressed?.Invoke(this, EventArgs.Empty);
            else
                StopRepeat();
        };

        var tip = new ToolTip
        {
            AutoPopDelay = 6000,
            InitialDelay = 300,
            ReshowDelay = 100
        };
        tip.SetToolTip(this,
            key.IsModifier
                ? $"{key.Label}: click = use with the next key; right-click = send {key.Label} alone"
                : key.Hint ?? $"Send {key.Label} to the currently focused app");
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);

        if (e.Button == MouseButtons.Right && Key.IsModifier)
        {
            SendAloneRequested?.Invoke(this, EventArgs.Empty);
            return;
        }

        if (e.Button != MouseButtons.Left) return;

        _pressed = true;
        Capture = true;
        Invalidate();

        // A keyboard acts on press, not after the pointer leaves the key.
        KeyPressed?.Invoke(this, EventArgs.Empty);

        if (RepeatEnabled && !Key.IsModifier)
            _repeatDelayTimer.Start();
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        if (e.Button != MouseButtons.Left) return;
        _pressed = false;
        Capture = false;
        StopRepeat();
        Invalidate();
    }

    protected override void OnMouseCaptureChanged(EventArgs e)
    {
        base.OnMouseCaptureChanged(e);
        if (!Capture)
        {
            _pressed = false;
            StopRepeat();
            Invalidate();
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _repeatDelayTimer.Dispose();
            _repeatTimer.Dispose();
        }
        base.Dispose(disposing);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        int inset = 1;
        var rect = new Rectangle(inset, inset, Width - inset * 2 - 1, Height - inset * 2 - 1);
        int radius = Math.Clamp(Height / 5, 6, 9);
        using var path = RoundedRect(rect, radius);

        Color fill = Latched
            ? Color.FromArgb(64, 105, 225)
            : _pressed
                ? Color.FromArgb(72, 72, 76)
                : Color.FromArgb(47, 47, 50);

        Color border = Latched
            ? Color.FromArgb(126, 158, 255)
            : _pressed
                ? Color.FromArgb(112, 112, 118)
                : Color.FromArgb(76, 76, 81);

        using var fillBrush = new SolidBrush(fill);
        using var borderPen = new Pen(border);
        e.Graphics.FillPath(fillBrush, path);
        e.Graphics.DrawPath(borderPen, path);

        using var font = CreateKeyFont(Height);
        TextRenderer.DrawText(
            e.Graphics,
            Key.OverlayLabel,
            font,
            rect,
            Color.White,
            TextFormatFlags.HorizontalCenter |
            TextFormatFlags.VerticalCenter |
            TextFormatFlags.EndEllipsis |
            TextFormatFlags.NoPadding);

        if (Key.IsModifier && Latched)
        {
            int dotSize = Height <= 34 ? 4 : 5;
            var dot = new Rectangle(Width - dotSize - 5, 5, dotSize, dotSize);
            using var brush = new SolidBrush(Color.White);
            e.Graphics.FillEllipse(brush, dot);
        }
    }

    private static Font CreateKeyFont(int height)
    {
        float size = height <= 32 ? 8.3f : height <= 36 ? 8.8f : height <= 42 ? 9.2f : 9.6f;
        return new Font("Segoe UI", size, FontStyle.Bold, GraphicsUnit.Point);
    }

    private void StopRepeat()
    {
        _repeatDelayTimer.Stop();
        _repeatTimer.Stop();
    }

    private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
    {
        int diameter = radius * 2;
        var path = new GraphicsPath();
        path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90);
        path.AddArc(bounds.Right - diameter, bounds.Top, diameter, diameter, 270, 90);
        path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
        path.AddArc(bounds.Left, bounds.Bottom - diameter, diameter, diameter, 90, 90);
        path.CloseFigure();
        return path;
    }
}
