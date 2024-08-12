using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = System.IO.File;

namespace WpfApp;

public static class StartupShortcutHelper
{
    private static readonly string app_name = "DrunkDeerDriver";

    private static string StartupFilePath()
    {
        return Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\" + app_name + ".lnk";
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

        System.Reflection.Assembly curAssembly = System.Reflection.Assembly.GetExecutingAssembly();
        IWshShortcut shortcut = shell.CreateShortcut(shortcutAddress);
        shortcut.Description = "Logitech Battery Indicator";
        shortcut.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var targetPath = curAssembly.Location.Replace(".dll", ".exe");
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
