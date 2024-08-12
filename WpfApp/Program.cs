using System.Runtime.InteropServices;

namespace WpfApp;

public partial class Program
{
    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool AllocConsole();

    [STAThread]
    public static void Main(string[] args)
    {
        if (args.Contains("--console"))
        {
            AllocConsole();
            Console.SetWindowSize(220, 32);
        }
        if (args.Contains("--start-minimized"))
        {
            MainWindow.ShouldStartMinimized = true;
        }

        App app = new();
        app.InitializeComponent();
        app.Run();
        App.Application_Exit();
    }
}
