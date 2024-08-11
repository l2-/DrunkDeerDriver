using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace App;
public sealed class KeyHandler
{
    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

    [DllImport("user32.dll")]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    private readonly int key;
    private readonly IntPtr hWnd;
    private readonly int id;
    private readonly int modifiers;

    public Action Callback { get; set; } = () => { };

    public KeyHandler(Keys key, Form form, int modifiers = 0)
    {
        this.key = (int)key;
        this.hWnd = form.Handle;
        id = this.GetHashCode();
        this.modifiers = modifiers;
    }

    public override int GetHashCode()
    {
        return key ^ modifiers ^ hWnd.ToInt32();
    }

    public bool Register()
    {
        return RegisterHotKey(hWnd, id, modifiers, key);
    }

    public bool Unregiser()
    {
        return UnregisterHotKey(hWnd, id);
    }
}
