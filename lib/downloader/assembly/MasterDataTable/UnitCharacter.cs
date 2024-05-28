// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitCharacter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using UnityEngine;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitCharacter
  {
    public int ID;
    public string name;
    public int category_UnitCategory;
    public int gender_UnitGender;
    public int personality_UnitPersonality;
    public int history_number;
    public string height;
    public string weight;
    public string bust;
    public string waist;
    public string hip;
    public string hobby;
    public string birthday;
    public string zodiac_sign;
    public string blood_type;
    public string origin;
    public string favorite;

    public static UnitCharacter Parse(MasterDataReader reader)
    {
      return new UnitCharacter()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        category_UnitCategory = reader.ReadInt(),
        gender_UnitGender = reader.ReadInt(),
        personality_UnitPersonality = reader.ReadInt(),
        history_number = reader.ReadInt(),
        height = reader.ReadString(true),
        weight = reader.ReadString(true),
        bust = reader.ReadString(true),
        waist = reader.ReadString(true),
        hip = reader.ReadString(true),
        hobby = reader.ReadString(true),
        birthday = reader.ReadString(true),
        zodiac_sign = reader.ReadString(true),
        blood_type = reader.ReadString(true),
        origin = reader.ReadString(true),
        favorite = reader.ReadString(true)
      };
    }

    public UnitCategory category => (UnitCategory) this.category_UnitCategory;

    public UnitGender gender => (UnitGender) this.gender_UnitGender;

    public UnitPersonality personality => (UnitPersonality) this.personality_UnitPersonality;

    public UnitUnit GetDefaultUnitUnit()
    {
      return Array.Find<UnitUnit>(MasterData.UnitUnitList, (Predicate<UnitUnit>) (uu => uu.character_UnitCharacter == this.ID));
    }

    public Future<Sprite> LoadCutin()
    {
      return Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format("AssetBundle/Resources/Characters/{0}/battle_cutin", (object) this.ID));
    }
  }
}
