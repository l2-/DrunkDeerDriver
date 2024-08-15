using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Driver.Packets;

namespace Driver;

public static class Packets
{
    public enum KeyPointType
    {
        ActuationPoint = 0x01,
        Downstroke = 0x04,
        Upstroke = 0x05
    }

    public readonly static byte REPORT_ID = 0x04;
    public readonly static byte PACKET_SIZE = 63;
    public readonly static byte[] IDENTITY_PACKET = [0xa0, 0x02, .. new byte[61]];
    public readonly static byte[] CLEAR_UP_RTP_PACKET = [0xaa, 0x00, 0x01, .. new byte[60]];
    public readonly static byte[] COMMON_SWITCH_PACKET_BASE = [0xb5, 0x00, 0x1e, 0x01, 0x00, 0x00, 0x01, .. new byte[56]];

    public static byte Clamp(this byte value, byte a, byte b)
        => Math.Max(a, Math.Min(value, b));

    public static byte GetActuationPoint(this Profile profile, int index)
        => ((byte)(profile.Keys_Array[index].Action_Point * 10)).Clamp(2, 38);

    public static byte GetDownstrokePoint(this Profile profile, int index)
        => ((byte)(profile.Keys_Array[index].Downstroke * 10)).Clamp(0, 36);

    public static byte GetUpstrokePoint(this Profile profile, int index)
        => ((byte)(profile.Keys_Array[index].Upstroke * 10)).Clamp(0, 36);

    // correct order -->
    // remap packets
    // clear packet
    // rtp packets (authorityPacket -> downLoadPacket)
    // common switch packet

    // common switch packet = 0xb5, 0x00, 0x1e, 0x01, 0x00, 0x00, 0x01, valueT (0x00), valueR (0x01), 0x00, if valueT == 1 then 0x00 otherwise rtp_lw, 0x00...x62

    // clear up rtp packet = 0xAA, 0x00, 0x01, 0x00...x60

    // Part of remap commands - buildPkt_remap_key_array

    // TODO
    private static byte[][] BuildPacketsRemapping(this Profile profile)
    {
        List<byte[]> packets = [];
        return [.. packets];
    }

    // TODO
    private static byte[] BuildPacketRTPAuthority(this ReleaseDoubleTriggerRapidTriggerPlusSetting setting)
    {
        return [];
    }

    // TODO
    private static byte[] BuildPacketRTPAuthorityDownload(this ReleaseDoubleTriggerRapidTriggerPlusSetting setting)
    {
        return [];
    }

    // TODO
    private static byte[][] BuildPacketsRapidTriggerPlusSettings(this Profile profile)
    {
        List<byte[]> packets = [];
        foreach (var _rtpSetting in profile.RTP?.Rdt_RtpSettings ?? [])
        {
            if (_rtpSetting is not { } rtpSetting) continue;
            packets.Add(rtpSetting.BuildPacketRTPAuthority());
            packets.Add(rtpSetting.BuildPacketRTPAuthorityDownload());
        }
        return [.. packets];
    }

    private static byte[] BuildCommonSwitchPacket(this Profile profile)
    {
        byte[] packet = [..COMMON_SWITCH_PACKET_BASE];
        packet[7] = 0x00;
        packet[8] = 0x01;
        packet[10] = 0x00; // not sure. should be 'if valueT == 1 then 0x00 otherwise rtp_lw' or just rtp_lw
        return packet;
    }

    public static byte[][] BuildPacketsRapidTriggerPlus(this Profile profile)
    {
        List<byte[]> packets = [];
        packets.AddRange(profile.BuildPacketsRemapping());
        packets.Add(CLEAR_UP_RTP_PACKET);
        packets.AddRange(profile.BuildPacketsRapidTriggerPlusSettings());
        packets.Add(profile.BuildCommonSwitchPacket());
        return [.. packets];
    }

    public static byte[] BuildPacketKeyPoint(this Profile profile, byte packetNumber, KeyPointType keyPointType)
    {
        var packet = new byte[PACKET_SIZE];
        packet[0] = 0xb6;
        packet[1] = ((byte)keyPointType);
        packet[2] = 0x00;
        packet[3] = packetNumber; // 0,1,2

        var offset = packetNumber switch
        {
            0 => 0,
            1 => 59,
            _ => 118,
        };

        var max_x = packetNumber switch
        {
            2 => 8,
            _ => 59
        };

        Func<int, byte> getValue = keyPointType switch
        {
            KeyPointType.ActuationPoint => profile.GetActuationPoint,
            KeyPointType.Downstroke => profile.GetDownstrokePoint,
            KeyPointType.Upstroke => profile.GetUpstrokePoint,
            _ => throw new NotImplementedException(),
        };

        for (int x = 0; x < max_x; x++)
        {
            var value = getValue(x + offset);
            packet[4 + x] = value;
        }

        return packet;
    }

    public static byte[][] BuildPackets(this Profile profile)
    {
        List<byte[]> packets = [];
        packets.Add(profile.BuildPacketKeyPoint(0, KeyPointType.ActuationPoint));
        packets.Add(profile.BuildPacketKeyPoint(1, KeyPointType.ActuationPoint));
        packets.Add(profile.BuildPacketKeyPoint(2, KeyPointType.ActuationPoint));
        packets.Add(profile.BuildPacketKeyPoint(0, KeyPointType.Downstroke));
        packets.Add(profile.BuildPacketKeyPoint(1, KeyPointType.Downstroke));
        packets.Add(profile.BuildPacketKeyPoint(2, KeyPointType.Downstroke));
        packets.Add(profile.BuildPacketKeyPoint(0, KeyPointType.Upstroke));
        packets.Add(profile.BuildPacketKeyPoint(1, KeyPointType.Upstroke));
        packets.Add(profile.BuildPacketKeyPoint(2, KeyPointType.Upstroke));
        return [.. packets];
    }
}
