using Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App;

public sealed class ProfileManager
{
    public static readonly ProfileManager Instance = new();
    public List<ProfileItem> Profiles { get; private set; } = [];
    private readonly JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true, WriteIndented = true };
    private readonly string profileDir = "profiles";

    public void DiscoverProfiles()
    {
        var info = Directory.CreateDirectory(profileDir);
        var profiles = info.EnumerateFiles().Where(f => Path.GetExtension(f.Name) == ".json").Select(f => FromJsonFile(f.FullName)).Where(p => p is not null).Select(p => p!);
        Profiles.AddRange(profiles);
    }

    private ProfileItem? FromJsonFile(string path)
    {
        var text = File.ReadAllText(path);
        var profile = JsonSerializer.Deserialize<ProfileItem>(text, options);
        return profile;
    }

    public void ImportProfile(string path)
    {
        try
        {
            var text = File.ReadAllText(path);
            var profile = JsonSerializer.Deserialize<Profile>(text, options);
            if (profile is null) { Console.WriteLine("Failed importing {0}!", path); return; }
            var profileItem = new ProfileItem { Name = Path.GetFileNameWithoutExtension(path), SelectedForQuickSwitch = false, Profile = profile };
            Save(profileItem);
            Profiles.Add(profileItem);
        }
        catch(Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    private void Save(ProfileItem item)
    {
        var json = JsonSerializer.Serialize(item, options);
        File.WriteAllText(Path.Combine(profileDir, item.Name + ".json"), json);
    }

    public void ProfileItemChanged(int index, int field, object value)
    {
        if (field == 0 && value is string name)
        {
            Profiles[index].Name = name;
        }

        if (field == 1 && value is bool selectedForQuickSwitch)
        {
            Profiles[index].SelectedForQuickSwitch = selectedForQuickSwitch;
        }

        Save(Profiles[index]);
    }
}
