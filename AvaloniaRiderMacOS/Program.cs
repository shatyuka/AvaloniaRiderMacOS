using Avalonia;
using System;

#if DEBUG
using System.Diagnostics;
using System.IO;
using System.Reflection;
#endif

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
            var avalonia = Assembly.GetAssembly(typeof(AvaloniaObject))!;
            var v = FileVersionInfo.GetVersionInfo(avalonia.Location);
            var version = $"{v.FileMajorPart}.{v.FileMinorPart}.{v.FileBuildPart}";
            var previewBinary = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                $".nuget/packages/avalonia/{version}/tools/netstandard2.0/designer/Avalonia.Designer.HostApp.dll");
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
