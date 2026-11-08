using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace VisualAcc;

public class VisualAccConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ServerSide;
    
    [ReloadRequired]
    [DefaultValue(true)]
    public bool DeveloperSets { get; set; }
    
    [DefaultValue(true)]
    public bool Debug { get; set; }
}