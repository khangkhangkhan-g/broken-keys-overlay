namespace BrokenKeysOverlay;

public sealed class SettingsForm : Form
{
    private const string AllKeysCategory = "All keys";

    private readonly HashSet<string> _selected;
    private readonly AppSettings _source;
    private readonly TextBox _search;
    private readonly ListBox _categories;
    private readonly FlowLayoutPanel _keyPanel;
    private readonly TrackBar _sizeSlider;
    private readonly TrackBar _opacitySlider;
    private readonly Label _countLabel;

    public AppSettings Result { get; private set; }

    public SettingsForm(AppSettings source)
    {
        _source = source;
        _selected = new HashSet<string>(source.SelectedKeyIds, StringComparer.OrdinalIgnoreCase);
        Result = Clone(source);

        Text = "Broken Keys Overlay — choose replacement keys";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(860, 620);
        ClientSize = new Size(980, 680);
        BackColor = Color.FromArgb(246, 247, 249);
        ForeColor = Color.FromArgb(28, 30, 34);
        Font = new Font("Segoe UI", 9.5f);
        AutoScaleMode = AutoScaleMode.Dpi;

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 4,
            Padding = new Padding(24, 20, 24, 20),
            BackColor = BackColor
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 92));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 54));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
        Controls.Add(root);

        // Header
        var header = new Panel { Dock = DockStyle.Fill, BackColor = BackColor };
        var heading = new Label
        {
            Text = "Choose only the keys that are broken",
            AutoSize = true,
            Font = new Font("Segoe UI", 18f, FontStyle.Bold),
            ForeColor = Color.FromArgb(18, 20, 24),
            Location = new Point(0, 4)
        };
        var subheading = new Label
        {
            Text = "The overlay behaves like a tiny undocked keyboard: only your selected keys are shown, and clicking it does not take typing focus from the current app.",
            AutoSize = false,
            Height = 38,
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            ForeColor = Color.FromArgb(92, 96, 104),
            Location = new Point(2, 46)
        };
        subheading.Width = Math.Max(400, ClientSize.Width - 80);
        header.Controls.Add(heading);
        header.Controls.Add(subheading);
        root.Controls.Add(header, 0, 0);

        // Search / actions bar
        var toolbar = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 4,
            RowCount = 1,
            Margin = new Padding(0),
            BackColor = BackColor
        };
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        _search = new TextBox
        {
            PlaceholderText = "Search a key — e.g. W, Backspace, Ctrl, F5, Volume",
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 7, 12, 7),
            Font = new Font("Segoe UI", 10f)
        };
        _search.TextChanged += (_, _) => RenderKeys();
        toolbar.Controls.Add(_search, 0, 0);

        var clear = MakeToolbarButton("Clear selected");
        clear.Click += (_, _) =>
        {
            _selected.Clear();
            RenderKeys();
        };
        toolbar.Controls.Add(clear, 1, 0);

        var selectCommon = MakeToolbarButton("Common keys");
        selectCommon.Click += (_, _) =>
        {
            foreach (var id in new[] { "Backspace", "Enter", "Space", "LCtrl", "LShift", "LAlt", "Delete" })
                _selected.Add(id);
            RenderKeys();
        };
        toolbar.Controls.Add(selectCommon, 2, 0);

        _countLabel = new Label
        {
            AutoSize = true,
            ForeColor = Color.FromArgb(82, 86, 94),
            TextAlign = ContentAlignment.MiddleRight,
            Margin = new Padding(14, 14, 0, 0)
        };
        toolbar.Controls.Add(_countLabel, 3, 0);
        root.Controls.Add(toolbar, 0, 1);

        // Main content: category list + key grid. This intentionally avoids nested AutoSize/Dock
        // combinations, which are fragile under Windows DPI scaling.
        var split = new SplitContainer
        {
            Dock = DockStyle.Fill,
            FixedPanel = FixedPanel.Panel1,
            IsSplitterFixed = false,
            SplitterDistance = 190,
            SplitterWidth = 8,
            BackColor = BackColor,
            Margin = new Padding(0, 4, 0, 4)
        };

        var categoryHeader = new Label
        {
            Text = "CATEGORIES",
            Dock = DockStyle.Top,
            Height = 30,
            Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
            ForeColor = Color.FromArgb(106, 110, 118),
            Padding = new Padding(6, 7, 0, 0)
        };
        _categories = new ListBox
        {
            Dock = DockStyle.Fill,
            BorderStyle = BorderStyle.None,
            IntegralHeight = false,
            Font = new Font("Segoe UI", 10f),
            BackColor = Color.FromArgb(238, 240, 244),
            ForeColor = Color.FromArgb(38, 40, 46),
            ItemHeight = 30
        };
        _categories.SelectedIndexChanged += (_, _) => RenderKeys();
        split.Panel1.BackColor = Color.FromArgb(238, 240, 244);
        split.Panel1.Padding = new Padding(6);
        var categoryLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            Margin = new Padding(0),
            Padding = new Padding(0),
            BackColor = split.Panel1.BackColor
        };
        categoryLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        categoryLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
        categoryLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        categoryLayout.Controls.Add(categoryHeader, 0, 0);
        categoryLayout.Controls.Add(_categories, 0, 1);
        split.Panel1.Controls.Add(categoryLayout);

        var keysHeader = new Label
        {
            Text = "Click a key to add/remove it from the floating strip",
            Dock = DockStyle.Top,
            Height = 34,
            ForeColor = Color.FromArgb(92, 96, 104),
            Padding = new Padding(8, 7, 0, 0)
        };
        _keyPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            WrapContents = true,
            FlowDirection = FlowDirection.LeftToRight,
            BackColor = Color.White,
            Padding = new Padding(10),
            Margin = new Padding(0)
        };
        split.Panel2.BackColor = Color.White;
        split.Panel2.Padding = new Padding(0);
        var keysLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            Margin = new Padding(0),
            Padding = new Padding(0),
            BackColor = Color.White
        };
        keysLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        keysLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        keysLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        keysLayout.Controls.Add(keysHeader, 0, 0);
        keysLayout.Controls.Add(_keyPanel, 0, 1);
        split.Panel2.Controls.Add(keysLayout);
        root.Controls.Add(split, 0, 2);

        // Footer
        var footer = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            Margin = new Padding(0),
            Padding = new Padding(0, 12, 0, 0),
            BackColor = BackColor
        };
        footer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        footer.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        var appearance = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            Margin = new Padding(0)
        };
        appearance.Controls.Add(new Label
        {
            Text = "Overlay size",
            AutoSize = true,
            Padding = new Padding(0, 12, 2, 0)
        });
        _sizeSlider = new TrackBar
        {
            Minimum = 30,
            Maximum = 48,
            Value = Math.Clamp(source.KeySize, 30, 48),
            TickStyle = TickStyle.None,
            Width = 120,
            Margin = new Padding(4, 5, 16, 0)
        };
        appearance.Controls.Add(_sizeSlider);
        appearance.Controls.Add(new Label
        {
            Text = "Opacity",
            AutoSize = true,
            Padding = new Padding(0, 12, 2, 0)
        });
        _opacitySlider = new TrackBar
        {
            Minimum = 45,
            Maximum = 100,
            Value = (int)Math.Round(Math.Clamp(source.Opacity, 0.45, 1.0) * 100),
            TickStyle = TickStyle.None,
            Width = 120,
            Margin = new Padding(4, 5, 0, 0)
        };
        appearance.Controls.Add(_opacitySlider);
        footer.Controls.Add(appearance, 0, 0);

        var actions = new FlowLayoutPanel
        {
            AutoSize = true,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            Anchor = AnchorStyles.Right | AnchorStyles.Top,
            Margin = new Padding(0)
        };
        var cancel = new Button
        {
            Text = "Cancel",
            AutoSize = true,
            MinimumSize = new Size(86, 38),
            Margin = new Padding(0, 3, 8, 0),
            DialogResult = DialogResult.Cancel
        };
        var save = new Button
        {
            Text = "Save overlay",
            AutoSize = true,
            MinimumSize = new Size(118, 38),
            Margin = new Padding(0, 3, 0, 0),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(42, 102, 232),
            ForeColor = Color.White,
            UseVisualStyleBackColor = false
        };
        save.FlatAppearance.BorderSize = 0;
        save.Click += (_, _) => SaveAndClose();
        actions.Controls.Add(cancel);
        actions.Controls.Add(save);
        footer.Controls.Add(actions, 1, 0);
        root.Controls.Add(footer, 0, 3);

        AcceptButton = save;
        CancelButton = cancel;

        PopulateCategories();
        UpdateCount();
        Shown += (_, _) =>
        {
            _categories.SelectedIndex = 0;
            _search.Focus();
        };
    }

    private static Button MakeToolbarButton(string text)
    {
        var button = new Button
        {
            Text = text,
            AutoSize = true,
            MinimumSize = new Size(0, 34),
            Margin = new Padding(0, 5, 8, 5),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.White,
            ForeColor = Color.FromArgb(48, 52, 60),
            UseVisualStyleBackColor = false
        };
        button.FlatAppearance.BorderColor = Color.FromArgb(205, 208, 214);
        return button;
    }

    private void PopulateCategories()
    {
        _categories.BeginUpdate();
        _categories.Items.Clear();
        _categories.Items.Add(AllKeysCategory);
        foreach (var category in KeyCatalog.All.Select(k => k.Category).Distinct())
            _categories.Items.Add(category);
        _categories.EndUpdate();
    }

    private void RenderKeys()
    {
        if (_keyPanel is null || _categories is null || _search is null) return;

        string query = _search.Text.Trim();
        string category = _categories.SelectedItem?.ToString() ?? AllKeysCategory;

        IEnumerable<KeyDefinition> keys = KeyCatalog.All;
        if (!string.Equals(category, AllKeysCategory, StringComparison.OrdinalIgnoreCase) && string.IsNullOrWhiteSpace(query))
            keys = keys.Where(k => string.Equals(k.Category, category, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(query))
        {
            keys = keys.Where(k =>
                k.Label.Contains(query, StringComparison.OrdinalIgnoreCase)
                || k.Id.Contains(query, StringComparison.OrdinalIgnoreCase)
                || k.Category.Contains(query, StringComparison.OrdinalIgnoreCase)
                || (k.Hint?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false));
        }

        var keyList = keys.ToList();

        _keyPanel.SuspendLayout();
        _keyPanel.Controls.Clear();

        if (keyList.Count == 0)
        {
            _keyPanel.Controls.Add(new Label
            {
                Text = "No matching keys.",
                AutoSize = true,
                ForeColor = Color.FromArgb(100, 104, 112),
                Font = new Font("Segoe UI", 10f),
                Margin = new Padding(10)
            });
        }
        else
        {
            foreach (var key in keyList)
                _keyPanel.Controls.Add(CreateKeyButton(key));
        }

        _keyPanel.ResumeLayout(true);
        UpdateCount();
    }

    private Button CreateKeyButton(KeyDefinition key)
    {
        bool selected = _selected.Contains(key.Id);
        int width = Math.Clamp(28 + key.Label.Length * 9, 58, 142);

        var button = new Button
        {
            Text = key.Label,
            Tag = key,
            Width = width,
            Height = 44,
            Margin = new Padding(5),
            Padding = new Padding(7, 0, 7, 0),
            FlatStyle = FlatStyle.Flat,
            BackColor = selected ? Color.FromArgb(42, 102, 232) : Color.FromArgb(244, 245, 248),
            ForeColor = selected ? Color.White : Color.FromArgb(35, 38, 44),
            UseVisualStyleBackColor = false,
            TabStop = false,
            Cursor = Cursors.Hand
        };
        button.FlatAppearance.BorderSize = 1;
        button.FlatAppearance.BorderColor = selected ? Color.FromArgb(42, 102, 232) : Color.FromArgb(210, 214, 220);
        button.FlatAppearance.MouseOverBackColor = selected ? Color.FromArgb(35, 88, 205) : Color.FromArgb(233, 236, 241);
        button.Click += (_, _) =>
        {
            if (_selected.Contains(key.Id))
                _selected.Remove(key.Id);
            else
                _selected.Add(key.Id);

            StyleKeyButton(button, _selected.Contains(key.Id));
            UpdateCount();
        };

        var tip = new ToolTip();
        tip.SetToolTip(button, key.Hint ?? $"Add/remove {key.Label} from the overlay");
        return button;
    }

    private static void StyleKeyButton(Button button, bool selected)
    {
        button.BackColor = selected ? Color.FromArgb(42, 102, 232) : Color.FromArgb(244, 245, 248);
        button.ForeColor = selected ? Color.White : Color.FromArgb(35, 38, 44);
        button.FlatAppearance.BorderColor = selected ? Color.FromArgb(42, 102, 232) : Color.FromArgb(210, 214, 220);
        button.FlatAppearance.MouseOverBackColor = selected ? Color.FromArgb(35, 88, 205) : Color.FromArgb(233, 236, 241);
    }

    private void UpdateCount() => _countLabel.Text = $"{_selected.Count} selected";

    private void SaveAndClose()
    {
        Result = Clone(_source);
        Result.SelectedKeyIds = KeyCatalog.All
            .Where(k => _selected.Contains(k.Id))
            .Select(k => k.Id)
            .ToList();
        Result.KeySize = Math.Clamp(_sizeSlider.Value, 30, 48);
        Result.Opacity = _opacitySlider.Value / 100.0;
        DialogResult = DialogResult.OK;
        Close();
    }

    private static AppSettings Clone(AppSettings source) => new()
    {
        SchemaVersion = AppSettings.CurrentSchemaVersion,
        SelectedKeyIds = source.SelectedKeyIds.ToList(),
        OverlayX = source.OverlayX,
        OverlayY = source.OverlayY,
        KeySize = source.KeySize,
        Opacity = source.Opacity
    };
}
