// Decompiled with JetBrains decompiler
// Type: MasterDataTable.SupplySupply
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class SupplySupply
  {
    public int ID;
    public string name;
    public string description;
    public string flavor;
    public int rarity_GearRarity;
    public int max_count;
    public int battle_stack_limit;
    public int sell_price;
    public int skill_BattleskillSkill;

    public static SupplySupply Parse(MasterDataReader reader)
    {
      return new SupplySupply()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        description = reader.ReadString(true),
        flavor = reader.ReadString(true),
        rarity_GearRarity = reader.ReadInt(),
        max_count = reader.ReadInt(),
        battle_stack_limit = reader.ReadInt(),
        sell_price = reader.ReadInt(),
        skill_BattleskillSkill = reader.ReadInt()
      };
    }

    public GearRarity rarity
    {
      get
      {
        GearRarity rarity;
        if (!MasterData.GearRarity.TryGetValue(this.rarity_GearRarity, out rarity))
          Debug.LogError((object) ("Key not Found: MasterData.GearRarity[" + (object) this.rarity_GearRarity + "]"));
        return rarity;
      }
    }

    public BattleskillSkill skill
    {
      get
      {
        BattleskillSkill skill;
        if (!MasterData.BattleskillSkill.TryGetValue(this.skill_BattleskillSkill, out skill))
          Debug.LogError((object) ("Key not Found: MasterData.BattleskillSkill[" + (object) this.skill_BattleskillSkill + "]"));
        return skill;
      }
    }

    public List<string> ResourcePaths()
    {
      return new List<string>()
      {
        string.Format("AssetBundle/Resources/Supplies/{0}/2D/item_thum", (object) this.ID),
        string.Format("AssetBundle/Resources/Supplies/{0}/2D/item_basic", (object) this.ID)
      };
    }

    public Future<Sprite> LoadSpriteThumbnail()
    {
      return Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format("AssetBundle/Resources/Supplies/{0}/2D/item_thum", (object) this.ID));
    }

    public Future<Sprite> LoadSpriteBasic()
    {
      return Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format("AssetBundle/Resources/Supplies/{0}/2D/item_basic", (object) this.ID));
    }
  }
}
