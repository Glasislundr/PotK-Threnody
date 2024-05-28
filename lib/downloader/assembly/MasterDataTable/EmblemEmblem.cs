// Decompiled with JetBrains decompiler
// Type: MasterDataTable.EmblemEmblem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class EmblemEmblem
  {
    public int ID;
    public string name;
    public string description;
    public int rarity_EmblemRarity;
    public int category_id_EmblemCategory;

    public static EmblemEmblem Parse(MasterDataReader reader)
    {
      return new EmblemEmblem()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        description = reader.ReadString(true),
        rarity_EmblemRarity = reader.ReadInt(),
        category_id_EmblemCategory = reader.ReadInt()
      };
    }

    public EmblemRarity rarity
    {
      get
      {
        EmblemRarity rarity;
        if (!MasterData.EmblemRarity.TryGetValue(this.rarity_EmblemRarity, out rarity))
          Debug.LogError((object) ("Key not Found: MasterData.EmblemRarity[" + (object) this.rarity_EmblemRarity + "]"));
        return rarity;
      }
    }

    public EmblemCategory category_id => (EmblemCategory) this.category_id_EmblemCategory;
  }
}
