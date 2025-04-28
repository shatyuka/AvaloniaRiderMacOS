using Avalonia;
using System;
using System.IO;
using System.Reflection;

namespace AvaloniaRiderMacOS;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
#if DEBUG
        if (Environment.GetEnvironmentVariable("AVALONIA_DESIGNER_PREVIEW") != null)
        {
            var previewBinary = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                ".nuget/packages/avalonia/11.2.8/tools/netstandard2.0/designer/Avalonia.Designer.HostApp.dll");
            var assembly = Assembly.LoadFrom(previewBinary);
            var module = assembly.GetModule("Avalonia.Designer.HostApp.dll");
            var program = module?.GetType("Avalonia.Designer.HostApp.Program");
            var main = program?.GetMethod("Main");
            main?.Invoke(null, [args]);
            return;
        }
#endif
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
