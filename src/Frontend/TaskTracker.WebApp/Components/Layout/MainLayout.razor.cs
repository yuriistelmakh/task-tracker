using MudBlazor;

namespace TaskTracker.WebApp.Components.Layout;

public partial class MainLayout
{
    private MudTheme _currentTheme = new()
    {
        PaletteLight = new PaletteLight()
        {
            Primary = "#5A7863",       // Primary color (buttons, active links)
            Secondary = "#90AB8B",     // Secondary color
            AppbarBackground = "#3B4953", // Top panel color (Header)
            AppbarText = "#EBF4DD",    // Text color on the top panel

            Background = "#EBF4DD",    // General page background
            Surface = "#FFFFFF",       // Card background (MudCard, surfaces) - white for contrast

            TextPrimary = "#3B4953",   // Primary text color
            TextSecondary = "#A0A8AD", // Secondary text color

            DrawerBackground = "#FFFFFF", // Sidebar menu background
            DrawerText = "#3B4953",       // Menu text
            DrawerIcon = "#5A7863",        // Menu icons

            Success = "#00A344"
        }
    };
}
