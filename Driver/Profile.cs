using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Driver;

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
    private string processTriggers = string.Empty;
    [JsonIgnore]
    private Profile? profile;

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

    public string ProcessTriggers
    {
        get { return processTriggers; }
        set { SetField(ref processTriggers, value, nameof(ProcessTriggers)); }
    }

    public required Profile Profile
    {
        get { return profile; }
        set { SetField(ref profile, value, nameof(Profile)); }
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
