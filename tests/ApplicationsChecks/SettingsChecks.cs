using MinimalFirewall;

internal static class SettingsChecks
{
    public static void Run()
    {
        using var form = new Form { ClientSize = new Size(1000, 800) };
        using var settings = new SettingsControl { Dock = DockStyle.Fill };
        form.Controls.Add(settings);
        form.Show();
        Application.DoEvents();

        static IEnumerable<Control> Descendants(Control root) =>
            root.Controls.Cast<Control>().SelectMany(child =>
                new[] { child }.Concat(Descendants(child)));

        var option = Descendants(settings).Single(control =>
            control.Text == "Filter WSL programs (OpenSnitch)");
        if (!option.Visible || option.Parent is null ||
            !option.Parent.ClientRectangle.Contains(option.Bounds))
            throw new InvalidOperationException("WSL option must be visible in the Settings card.");
        form.Close();
    }
}
