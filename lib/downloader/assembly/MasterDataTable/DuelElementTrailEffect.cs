// Decompiled with JetBrains decompiler
// Type: MasterDataTable.DuelElementTrailEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class DuelElementTrailEffect
  {
    public int ID;
    public int kind_GearKind;
    public int element_CommonElement;
    public string trail_effect_name;
    public int? trail_color_r;
    public int? trail_color_g;
    public int? trail_color_b;
    public int? trail_color_a;

    public static DuelElementTrailEffect Parse(MasterDataReader reader)
    {
      return new DuelElementTrailEffect()
      {
        ID = reader.ReadInt(),
        kind_GearKind = reader.ReadInt(),
        element_CommonElement = reader.ReadInt(),
        trail_effect_name = reader.ReadStringOrNull(true),
        trail_color_r = reader.ReadIntOrNull(),
        trail_color_g = reader.ReadIntOrNull(),
        trail_color_b = reader.ReadIntOrNull(),
        trail_color_a = reader.ReadIntOrNull()
      };
    }

    public GearKind kind
    {
      get
      {
        GearKind kind;
        if (!MasterData.GearKind.TryGetValue(this.kind_GearKind, out kind))
          Debug.LogError((object) ("Key not Found: MasterData.GearKind[" + (object) this.kind_GearKind + "]"));
        return kind;
      }
    }

    public CommonElement element => (CommonElement) this.element_CommonElement;
  }
}
