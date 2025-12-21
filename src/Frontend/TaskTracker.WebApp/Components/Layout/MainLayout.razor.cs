using MudBlazor;

namespace TaskTracker.WebApp.Components.Layout;

public partial class MainLayout
{
    private MudTheme _currentTheme = new()
    {
        PaletteLight = new PaletteLight()
        {
            Primary = "#5A7863",       // Основний колір (кнопки, активні посилання)
            Secondary = "#90AB8B",     // Другорядний колір
            AppbarBackground = "#3B4953", // Колір верхньої панелі (Header)
            AppbarText = "#EBF4DD",    // Колір тексту на верхній панелі

            Background = "#EBF4DD",    // Загальний фон сторінки
            Surface = "#FFFFFF",       // Фон карток (MudCard, поверхні) - залишив білим для контрасту
            
            TextPrimary = "#3B4953",   // Основний колір тексту
            TextSecondary = "#5A7863", // Колір другорядного тексту

            DrawerBackground = "#FFFFFF", // Фон бокового меню
            DrawerText = "#3B4953",       // Текст у меню
            DrawerIcon = "#5A7863"        // Іконки у меню
        }
    };
}
