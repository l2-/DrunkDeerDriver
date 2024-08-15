using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Driver;

/*
 * {
        className: string;
        value: number;
        name: string;
        isBorder: boolean;
        height: number;
        keyname: string;
        action_point: number;
        downstroke: number;
        upstroke: number;
        rdt: boolean;
    }
 */
public abstract record RapidTriggerPlusKey
{
    // Dont save bloat
    //public string? ClassName;
    //public int? Value;
    //public string? Name;
    //public bool? IsBorder;
    //public int? Height;
    public required string Keyname;
    public required int Action_Point;
    public required int Downstroke;
    public required int Upstroke;
}

public sealed record ReleaseDoubleTriggerKey : RapidTriggerPlusKey
{
    public required bool Rdt;
}

public sealed record LastWinTriggerKey : RapidTriggerPlusKey
{
    public bool? Rdt;
    public object? Rdt_; // ReleaseDoubleTriggerKey? property loop?
    public required int LwIndex;
}
/*
 * currentRDT: {
    isRdtEnabled: boolean;
    mainKey: {
        className: string;
        value: number;
        name: string;
        isBorder: boolean;
        height: number;
        keyname: string;
        action_point: number;
        downstroke: number;
        upstroke: number;
        rdt: boolean;
    };
    triggerKey: {
        className: string;
        value: number;
        name: string;
        isBorder: boolean;
        height: number;
        keyname: string;
        action_point: number;
        downstroke: number;
        upstroke: number;
        rdt: boolean;
    }
    x2Reset: number;
    x2Active: number;
} 
 */
public sealed record ReleaseDoubleTriggerRapidTriggerPlusSetting
{
    public required bool IsRdtEnabled;
    public required ReleaseDoubleTriggerKey MainKey;
    public required ReleaseDoubleTriggerKey TriggerKey;
    public required int X2Reset;
    public required int Y2Active;
}

public sealed record LastWinRapidTriggerPlusSetting
{
    public required bool IsRdtEnabled;
    public required ReleaseDoubleTriggerKey MainKey;
    public required ReleaseDoubleTriggerKey TriggerKey;
}

public sealed record RapidTriggerPlus
{
    public required ReleaseDoubleTriggerRapidTriggerPlusSetting[] Rdt_RtpSettings;
    public required bool Rdt_Watch_Change;
    public required LastWinTriggerKey[][] Lw_Temp_list;
    public required LastWinRapidTriggerPlusSetting[] Lw_RtpSettings;
    public required bool LW_Watch_Change;
    public required string Rtp_Model;

    //public bool Rdt_Open = false; // dont care
    //public bool Lw_Open = false; // dont care
}

public sealed record KeySetting
{
    public required string KeyName { get; set; }
    public required decimal Action_Point { get; set; }
    public required decimal Downstroke { get; set; }
    public required decimal Upstroke { get; set; }
}

public sealed record Profile
{
    public required string Storagename { get; set; }
    public required string Showname { get; set; }
    public required KeySetting[] Keys_Array { get; set; }
    public RapidTriggerPlus? RTP { get; set; }
}

public sealed record KeyRemapSetting
{
    public required int KeyIndex { get; set; }
    public required string KeyText { get; set; }
    public required int KeyCmd { get; set; }
    //public required int KeyType { get; set; } // Probably not needed because the array this setting is in also shows the type of setting
    public required int KeyCode { get; set; }
}

public sealed record RemapProfile
{
    public required string Storagename { get; set; }
    public required string Showname { get; set; }
    public required KeySetting[] KeyCodeDefault { get; set; }
    public required Dictionary<string, int> HotKeyMap { get; set; }
    public required KeySetting[] KeyCodeFn1 { get; set; }
    public required KeySetting[] KeyCodeFn2 { get; set; }
}

public record ProfileItem : INotifyPropertyChanged
{
    [JsonIgnore]
    private string name = string.Empty;
    [JsonIgnore]
    private bool selectedForQuickSwitch = false;
    [JsonIgnore]
    private bool isDefault = false;
    [JsonIgnore]
    private string[] processTriggers = [];
    [JsonIgnore]
    private Profile? profile;
    [JsonIgnore]
    private RemapProfile? remapProfile;

    [JsonIgnore]
    public bool IsDirty { get; set; }
    public event PropertyChangedEventHandler? PropertyChanged;


    [JsonIgnore]
    public string Name
    {
        get { return name; }
        set { SetField(ref name, value, nameof(Name)); }
    }

    public bool SelectedForQuickSwitch
    {
        get { return selectedForQuickSwitch; }
        set { SetField(ref selectedForQuickSwitch, value, nameof(SelectedForQuickSwitch)); }
    }

    public bool IsDefault
    {
        get { return isDefault; }
        set { SetField(ref isDefault, value, nameof(IsDefault)); }
    }

    public string[] ProcessTriggers
    {
        get { return processTriggers; }
        set { SetField(ref processTriggers, value, nameof(ProcessTriggers)); }
    }

    public required Profile Profile
    {
        get
        {
            if (profile is null)
            {
                throw new Exception("Profile is null");
            }
            return profile;
        }
        set { SetField(ref profile, value, nameof(Profile)); }
    }

    public RemapProfile? RemapProfile
    {
        get { return remapProfile; }
        set { SetField(ref remapProfile, value, nameof(RemapProfile)); }
    }

    protected void SetField<T>(ref T field, T value, string propertyName)
    {
        if (!EqualityComparer<T>.Default.Equals(field, value))
        {
            field = value;
            IsDirty = true;
            OnPropertyChanged(propertyName);
        }
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
