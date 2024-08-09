using HidSharp;

namespace DrunkDeerDriver;

internal class Program
{
    private static byte[] identityPacket = new byte[] { 0x04, 160, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    private static byte[] p11 = new byte[] { 0x04, 182, 1, 0, 0, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 20, 20, 20, 20, 20, 20, 10, 10, 2, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 20, 20 };
    private static byte[] p12 = new byte[] { 0x04, 182, 1, 0, 1, 20, 20, 20, 20, 10, 2, 2, 2, 10, 10, 10, 10, 10, 10, 10, 10, 20, 10, 10, 20, 20, 20, 20, 20, 20, 10, 20, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 20, 20, 20, 20, 20, 20, 10, 10, 10, 20, 20, 20, 10, 20, 20, 10, 10, 10, 10 };
    private static byte[] p13 = new byte[] { 0x04, 182, 1, 0, 2, 10, 10, 20, 20, 20, 20, 20, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    private static byte[] p21 = new byte[] { 0x04, 182, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 0, 0, 0, 0, 0, 0, 10, 10, 1, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 0, 0 };
    private static byte[] p22 = new byte[] { 0x04, 182, 4, 0, 1, 0, 0, 0, 0, 10, 1, 1, 1, 10, 10, 10, 10, 10, 10, 10, 10, 0, 10, 10, 0, 0, 0, 0, 0, 0, 10, 0, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 0, 0, 0, 0, 0, 0, 10, 10, 10, 0, 0, 0, 10, 0, 0, 10, 10, 10, 10 };
    private static byte[] p23 = new byte[] { 0x04, 182, 4, 0, 2, 10, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    private static byte[] p31 = new byte[] { 0x04, 182, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 0, 0, 0, 0, 0, 0, 10, 10, 1, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 0, 0 };
    private static byte[] p32 = new byte[] { 0x04, 182, 5, 0, 1, 0, 0, 0, 0, 10, 1, 1, 1, 10, 10, 10, 10, 10, 10, 10, 10, 0, 10, 10, 0, 0, 0, 0, 0, 0, 10, 0, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 0, 0, 0, 0, 0, 0, 10, 10, 10, 0, 0, 0, 10, 0, 0, 10, 10, 10, 10 };
    private static byte[] p33 = new byte[] { 0x04, 182, 5, 0, 2, 10, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    private static byte[][] bs = new byte[9][] { p11, p12, p13, p21, p22, p23, p31, p32, p33 };

    static void Main(string[] args)
    {
        var device = KeyboardManager.Instance.Keyboard;
        if (device is null)
        {
            Console.WriteLine("No keyboard"); return;
        }
        var stream = device.Open();
        stream.ReadTimeout = 20000;
        stream.WriteTimeout = 20000;
        using (stream)
        {
            Write(stream, identityPacket);

            //foreach (var b in bs)
            //{
            //    Write(stream, b);
            //}

            Write(stream, identityPacket);
        }
        Console.WriteLine("Hello, World!");



        // TODO
        // keep app running in tray with little icon? (profile first letter or index in the icon?)
        // read profiles from the export
        // listen to hotkey and change profile based on it
        // change profile based on app in foreground
        // change RT and turbo based on profile
        // change lighting based on active profile? Need to link profiles together somehow?

    }

    private static void Write(HidStream stream, byte[] data)
    {
        stream.Write(data);
        Console.WriteLine(string.Join(",", stream.Read()));
    }
}
