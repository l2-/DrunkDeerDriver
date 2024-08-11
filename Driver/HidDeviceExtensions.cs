using HidSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver;

public static class HidDeviceExtensions
{
    public static string PacketToString(this byte[] packet)
        => string.Format("[{0}]", string.Join(",", packet.Select(b => string.Format("{0:X2}", b))));

    public static bool WritePacket(this HidStream stream, byte[] packet)
    {
        if (packet.Length < 1) return false;
        Console.WriteLine("Writing packet \t{0}", packet.PacketToString());

        stream.Write([Packets.REPORT_ID, .. packet]);
        var response = stream.Read();
        Console.WriteLine("Received packet {0}", response.Skip(1).ToArray().PacketToString());
        return response[1] == packet[0];
    }

    public static bool Ping(this HidStream stream)
        => stream.WritePacket(Packets.IDENTITY_PACKET);

}
