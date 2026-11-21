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

        // Card layout lists controls explicitly; an option left out of every card is never shown.
        foreach (var text in new[] { "Filter WSL programs (OpenSnitch)", "Clean up orphaned rules at startup" })
        {
            var option = Descendants(settings).Single(control => control.Text == text);
            // Left in the legacy fixed panel, an option is "visible" but covered by the cards.
            if (!option.Visible || option.Parent is null || option.Parent.Name == "mainSettingsPanel" ||
                !option.Parent.ClientRectangle.Contains(option.Bounds))
                throw new InvalidOperationException($"\"{text}\" must be visible in a Settings card.");
        }
        form.Close();
    }
}
