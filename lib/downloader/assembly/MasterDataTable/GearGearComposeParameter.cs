// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GearGearComposeParameter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GearGearComposeParameter
  {
    public int ID;
    public int group_id;
    public int compose_level;
    public int compose_kind_GearModelKind;
    public int sell_price;
    public int repair_price;
    public float repair_success_ratio;
    public int hp_buildup_limit;
    public int strength_buildup_limit;
    public int vitality_buildup_limit;
    public int intelligence_buildup_limit;
    public int mind_buildup_limit;
    public int agility_buildup_limit;
    public int dexterity_buildup_limit;
    public int lucky_buildup_limit;

    public static GearGearComposeParameter Parse(MasterDataReader reader)
    {
      return new GearGearComposeParameter()
      {
        ID = reader.ReadInt(),
        group_id = reader.ReadInt(),
        compose_level = reader.ReadInt(),
        compose_kind_GearModelKind = reader.ReadInt(),
        sell_price = reader.ReadInt(),
        repair_price = reader.ReadInt(),
        repair_success_ratio = reader.ReadFloat(),
        hp_buildup_limit = reader.ReadInt(),
        strength_buildup_limit = reader.ReadInt(),
        vitality_buildup_limit = reader.ReadInt(),
        intelligence_buildup_limit = reader.ReadInt(),
        mind_buildup_limit = reader.ReadInt(),
        agility_buildup_limit = reader.ReadInt(),
        dexterity_buildup_limit = reader.ReadInt(),
        lucky_buildup_limit = reader.ReadInt()
      };
    }

    public GearModelKind compose_kind
    {
      get
      {
        GearModelKind composeKind;
        if (!MasterData.GearModelKind.TryGetValue(this.compose_kind_GearModelKind, out composeKind))
          Debug.LogError((object) ("Key not Found: MasterData.GearModelKind[" + (object) this.compose_kind_GearModelKind + "]"));
        return composeKind;
      }
    }
  }
}
