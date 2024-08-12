using IWshRuntimeLibrary;
using System.Diagnostics;
using System.IO;
using File = System.IO.File;

namespace WpfApp;

public static class StartupShortcutHelper
{
    private static readonly string APP_NAME = Path.GetFileNameWithoutExtension(Environment.ProcessPath ?? "Drunk Deer Driver");

    private static string StartupFilePath()
    {
        return Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\" + APP_NAME + ".lnk";
    }

    public static bool StartupFileExists()
    {
        return File.Exists(StartupFilePath());
    }

    private static void AddToStartup()
    {
        WshShell shell = new();
        string shortcutAddress = StartupFilePath();

        if (StartupFileExists()) { File.Delete(shortcutAddress); }

        IWshShortcut shortcut = shell.CreateShortcut(shortcutAddress);
        shortcut.Description = "Drunk Deer Driver - Easy Keyboard Profile Switching";
        shortcut.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var targetPath = Environment.ProcessPath;
        if (targetPath is null || targetPath.Equals(string.Empty))
        {
            targetPath = Process.GetCurrentProcess().MainModule?.FileName;
        }
        shortcut.TargetPath = targetPath;
        shortcut.Arguments = "--start-minimized";
        shortcut.Save();
    }

    private static void RemoveFromStartup()
    {
        if (StartupFileExists()) { File.Delete(StartupFilePath()); }
    }

    public static void OnCheckChanged(bool isChecked)
    {
        if (isChecked) AddToStartup();
        else RemoveFromStartup();
    }
}
