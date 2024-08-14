using HidSharp;

namespace Driver;

public sealed record KeyboardFilter
{
    required public int VendorId, ProductId, Usage, UsagePage;
}

public sealed class KeyboardManager : IDisposable
{
    public static readonly KeyboardFilter[] DrunkDeerKeyboards = [
        new KeyboardFilter { VendorId = 0x352d, ProductId = 0x2383, Usage = 0, UsagePage = 0xff00 },
        new KeyboardFilter { VendorId = 0x352d, ProductId = 0x2386, Usage = 0, UsagePage = 0xff00 },
        new KeyboardFilter { VendorId = 0x352d, ProductId = 0x2382, Usage = 0, UsagePage = 0xff00 },
        new KeyboardFilter { VendorId = 0x352d, ProductId = 0x2384, Usage = 0, UsagePage = 0xff00 },
        new KeyboardFilter { VendorId = 0x05ac, ProductId = 0x024f, Usage = 0, UsagePage = 0xff00 },
        new KeyboardFilter { VendorId = 0x352d, ProductId = 0x2391, Usage = 0, UsagePage = 0xff00 }
    ];

    public HidDevice? _keyboard;
    public HidDevice? Keyboard
    {
        get { return _keyboard; }
        set
        {
            if (!EqualityComparer<string?>.Default.Equals(_keyboard?.ToString(), value?.ToString()))
            {
                _keyboard = value;
                ConnectedKeyboardChanged?.Invoke(_keyboard);
            }
        }
    }
    public event Action<HidDevice?>? ConnectedKeyboardChanged;

    public KeyboardManager() { Keyboard = FindKeyboard(); Register(); }

    private void OnDeviceListChanged(object? sender, DeviceListChangedEventArgs e)
    {
        if (Keyboard is not null && Keyboard.CanOpen) return;

        Keyboard = FindKeyboard();
    }

    private static HidDevice? FindKeyboard()
    {
        return DeviceList.Local.GetHidDevices().FirstOrDefault(IsCompatibleKeyboard);
    }

    public static bool IsCompatibleKeyboard(HidDevice device)
        => IsDrunkDeerKeyboard(device) && device.IsCompatible();

    public static bool IsDrunkDeerKeyboard(HidDevice device)
    {
        // The HidSharp lib doesn't allow for easy access to the usage and usage page attributes.
        // Instead we check if the input and output report length are both over 64 bytes.
        // This indicates we probably have a device with read and write stream capability.
        return DrunkDeerKeyboards.Any(ddkbs => ddkbs.ProductId == device.ProductID && ddkbs.VendorId == device.VendorID && device.GetMaxOutputReportLength() >= 64 && device.GetMaxInputReportLength() >= 64);
    }

    public void Register() { DeviceList.Local.Changed += OnDeviceListChanged; }

    public void Unregister() { DeviceList.Local.Changed -= OnDeviceListChanged; }

    public void Dispose()
    {
        Unregister();
        Keyboard = null;
    }

    public bool IsConnected()
    {
        if (Keyboard is null) return false;
        using var stream = Keyboard.Open();
        return stream.Ping();
    }
}
