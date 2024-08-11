using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

public sealed record ProfileItem
{
    public required string Name { get; set; }
    public required bool SelectedForQuickSwitch { get; set; }
    public required Profile Profile { get; set; }
}
