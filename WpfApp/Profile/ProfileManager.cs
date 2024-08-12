﻿using Driver;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text.Json;
using MessageBox = System.Windows.Forms.MessageBox;

namespace WpfApp.Profile;

public sealed class ProfileManager(KeyboardManager keyboardManager)
{
    public List<ProfileItem> Profiles { get; private set; } = [];
    public List<Tuple<ProfileItem, string>> ProfileFileNames { get; private set; } = [];
    private readonly JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true, WriteIndented = true };
    private readonly string profileDir = "profiles";
    private readonly KeyboardManager keyboardManager = keyboardManager;
    private int currentIndex = -1;
    public int CurrentIndex
    {
        get { return currentIndex; }
        set
        {
            if (!EqualityComparer<int>.Default.Equals(currentIndex, value))
            {
                currentIndex = value;
                CurrentProfileChanged?.Invoke(currentIndex, Profiles[currentIndex]);
            }
        }
    }

    public event Action<int, ProfileItem>? CurrentProfileChanged;
    public event Action<int, ProfileItem>? ProfileCollectionChanged;

    public void DiscoverProfiles()
    {
        var info = Directory.CreateDirectory(profileDir);
        var profiles = info.EnumerateFiles().Where(f => Path.GetExtension(f.Name) == ".json").Select(f => FromJsonFile(f.FullName)).Where(p => p is not null).Select(p => p!);
        foreach (var profile in profiles)
        {
            profile.PropertyChanged += ProfileItemChanged;
            Profiles.Add(profile);
            ProfileCollectionChanged?.Invoke(Profiles.Count - 1, profile);
        }
        ProfileFileNames = Profiles.Select(p => Tuple.Create(p, p.Name)).ToList();
        var current = Math.Max(Profiles.FindIndex(p => p.IsDefault), 0);
        if (current != CurrentIndex && current < Profiles.Count)
        {
            CurrentIndex = current;
        }
    }

    private ProfileItem? FromJsonFile(string path)
    {
        var text = File.ReadAllText(path);
        var profile = JsonSerializer.Deserialize<ProfileItem>(text, options);
        if (profile != null)
        {
            profile.Name = Path.GetFileNameWithoutExtension(path);
            profile.IsDirty = false;
        }
        return profile;
    }

    public void ImportProfile(string path)
    {
        try
        {
            var text = File.ReadAllText(path);
            var profile = JsonSerializer.Deserialize<Driver.Profile>(text, options);
            if (profile is null) { Console.WriteLine("Failed importing {0}!", path); return; }
            var profileItem = new ProfileItem
            {
                Name = Path.GetFileNameWithoutExtension(path),
                Profile = profile,
                IsDirty = false
            };
            Save(profileItem);
            Profiles.Add(profileItem);
            ProfileCollectionChanged?.Invoke(Profiles.Count - 1, profileItem);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    private void Save(ProfileItem item)
    {
        var json = JsonSerializer.Serialize(item, options);
        var indexOld = ProfileFileNames.FindIndex(t => t.Item1 == item);
        if (indexOld >= 0 && !ProfileFileNames[indexOld].Item2.Equals(item.Name))
        {
            var old = ProfileFileNames[indexOld];
            // changed profile name, remove old one
            File.Delete(Path.Combine(profileDir, old.Item2 + ".json"));
            Console.WriteLine("Removing {0}", old.Item2);
            ProfileFileNames.RemoveAt(indexOld);
        }
        File.WriteAllText(Path.Combine(profileDir, item.Name + ".json"), json);
        Console.WriteLine("Saving {0}", item.Name);
        ProfileFileNames.Add(Tuple.Create(item, item.Name));
    }

    public void ProfileItemChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is ProfileItem item && item.IsDirty)
        {
            if (nameof(ProfileItem.IsDefault).Equals(e.PropertyName) && item.IsDefault)
            {
                foreach (var profile in Profiles.Where(p => p != item))
                {
                    profile.IsDefault = false;
                }
            }
            foreach (var profile in Profiles.Where(p => p.IsDirty))
            {
                Save(profile);
                profile.IsDirty = false;
            }
        }
    }

    public void PushCurrentProfile()
    {
        if (CurrentIndex >= Profiles.Count)
        {
            Console.WriteLine("Current profile out of range!");
            return;
        }
        var current = Profiles[CurrentIndex];
        Console.WriteLine("Pushing profile {0} to keyboard", current.Name);
        var packets = current.Profile.BuildPackets();
        keyboardManager.Keyboard?.Open().WritePacket(packets);
    }

    public void QuickSwitchProfile()
    {
        var quickSwitchProfiles = Profiles.Where(p => p.SelectedForQuickSwitch).ToList();
        if (quickSwitchProfiles.Count < 2) return;
        var current = Profiles[CurrentIndex];
        var currentIndex = quickSwitchProfiles.IndexOf(current);
        var next = quickSwitchProfiles[(currentIndex + 1) % quickSwitchProfiles.Count];
        Console.WriteLine("Switching from {0} to profile {1}", current.Name, next.Name);
        SwitchTo(next);
    }

    public bool IsSelected(ProfileItem profileItem)
    {
        return Profiles.IndexOf(profileItem) == CurrentIndex;
    }

    public void SwitchTo(ProfileItem profileItem)
    {
        var i = Profiles.IndexOf(profileItem);
        if (i >= 0 && i < Profiles.Count)
        {
            CurrentIndex = i;
        }
    }
}
