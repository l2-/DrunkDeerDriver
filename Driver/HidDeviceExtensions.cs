using HidSharp;

namespace Driver;

public static class HidDeviceExtensions
{
    public enum ErrorType
    {
        ERR_MSG_RETRY, ERR_MSG_REPORT, ERR_MSG_MATCH, ERR_MSG_MATCH2, ERR_MSG_SELECT, ERR_MSG_SELECT2, ERR_MSG_RESPONSE,
    }
    private static readonly Dictionary<ErrorType, string> DrunkDeerErrorMessageMap = new()
    {
        { ErrorType.ERR_MSG_RETRY, "Failed to connect keyboard, please try it again(0}!" },
        { ErrorType.ERR_MSG_REPORT, "transmit report to keyboard failed(0.1}." },
        { ErrorType.ERR_MSG_MATCH, "keyboard firmware and driver does not match(1}." },
        { ErrorType.ERR_MSG_MATCH2, "keyboard firmware and driver does not match(2}." },
        { ErrorType.ERR_MSG_SELECT, "does not select one keyboard(4}." },
        { ErrorType.ERR_MSG_SELECT2, "select one keyboard failed(5}." },
        { ErrorType.ERR_MSG_RESPONSE, "failed to receive response message(6}." },
    };

    public static TResult Using<TResult, T>(
        this T factory,
        Func<T, TResult> use) where T : IDisposable
    {
        using var disposable = factory;
        return use(disposable);
    }

    public static string PacketToString(this byte[] packet)
        => string.Format("[{0}]", string.Join(",", packet.Select(b => string.Format("{0:X2}", b))));

    public static bool WritePacket(this HidStream stream, byte[][] packets)
    {
        foreach (var p in packets)
        {
            if (!stream.TryWritePacket(p))
            {
                return false;
            }
        }
        return true;
    }

    public static bool TryWritePacket(this HidStream stream, byte[] packet)
        => stream.WritePacket(packet) is { } response && response.Length > 0 && response.First() == packet[0];


    public static byte[] WritePacket(this HidStream stream, byte[] packet)
    {
        if (packet.Length < 1) return [];
        Console.WriteLine("Writing packet \t{0}", packet.PacketToString());

        stream.Write([Packets.REPORT_ID, .. packet]);
        var response = stream.Read();
        Console.WriteLine("Received packet {0}", response.Skip(1).ToArray().PacketToString());
        return response.Skip(1).ToArray();
    }


    public static bool Ping(this HidStream stream)
        => stream.TryWritePacket(Packets.IDENTITY_PACKET);

    public static KeyboardSpecs GetKeyboardSpecs(this HidStream stream)
        => new(stream.WritePacket(Packets.IDENTITY_PACKET));

    public static bool IsCompatible(this HidDevice device)
        => device.Open().Using(s => s.GetKeyboardSpecs()).KeyboardType is not null;
}
