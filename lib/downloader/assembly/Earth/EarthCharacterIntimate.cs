// Decompiled with JetBrains decompiler
// Type: Earth.EarthCharacterIntimate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace Earth
{
  [Serializable]
  public class EarthCharacterIntimate : BL.ModelBase
  {
    private const int Breakthrough_Maxlevel = 10;
    private const int Maxlevel = 5;
    private int mCharacterID1;
    private int mCharacterID2;
    private int mExperience;
    [NonSerialized]
    private Tuple<int, int> mKey;
    [NonSerialized]
    private PlayerCharacterIntimate playerCharacterInimate;
    [NonSerialized]
    private BL.BattleModified<EarthCharacterIntimate> mModified;
    private static readonly string serverDataFormat = "{{\"character_id_1\":{0},\"character_id_2\":{1},\"experience\":{2}}}";

    public Tuple<int, int> key
    {
      get
      {
        if (this.mKey == null)
          this.mKey = new Tuple<int, int>(this.mCharacterID1, this.mCharacterID2);
        return this.mKey;
      }
    }

    public int experience => this.mExperience;

    public static EarthCharacterIntimate Create(int characterID1, int characterID2)
    {
      if (characterID1 == characterID2)
        return (EarthCharacterIntimate) null;
      EarthCharacterIntimate characterIntimate = new EarthCharacterIntimate()
      {
        mCharacterID1 = characterID1 < characterID2 ? characterID1 : characterID2,
        mCharacterID2 = characterID1 > characterID2 ? characterID1 : characterID2,
        mExperience = 0
      };
      characterIntimate.mKey = new Tuple<int, int>(characterIntimate.mCharacterID1, characterIntimate.mCharacterID2);
      return characterIntimate;
    }

    public Tuple<int, int> AddExperience(int exp)
    {
      int num = ((IEnumerable<InitimateBreakthrough>) MasterData.InitimateBreakthroughList).Any<InitimateBreakthrough>((Func<InitimateBreakthrough, bool>) (x =>
      {
        if (x.character_id == this.mCharacterID1 && x.target_character_id == this.mCharacterID2)
          return true;
        return x.character_id == this.mCharacterID2 && x.target_character_id == this.mCharacterID1;
      })) ? 10 : 5;
      InitimateLevel initimateLevel1 = ((IEnumerable<InitimateLevel>) MasterData.InitimateLevelList).FirstOrDefault<InitimateLevel>((Func<InitimateLevel, bool>) (x => x.from_exp <= this.mExperience && x.to_exp >= this.mExperience));
      if (initimateLevel1 == null || initimateLevel1.level > num)
        initimateLevel1 = ((IEnumerable<InitimateLevel>) MasterData.InitimateLevelList).FirstOrDefault<InitimateLevel>((Func<InitimateLevel, bool>) (x => x.level == this.playerCharacterInimate.max_level));
      this.mExperience += exp;
      InitimateLevel initimateLevel2 = ((IEnumerable<InitimateLevel>) MasterData.InitimateLevelList).FirstOrDefault<InitimateLevel>((Func<InitimateLevel, bool>) (x => x.from_exp <= this.mExperience && x.to_exp >= this.mExperience));
      if (initimateLevel2 == null || initimateLevel2.level > num)
      {
        initimateLevel2 = ((IEnumerable<InitimateLevel>) MasterData.InitimateLevelList).FirstOrDefault<InitimateLevel>((Func<InitimateLevel, bool>) (x => x.level == this.playerCharacterInimate.max_level));
        this.mExperience = initimateLevel2.from_exp;
      }
      return new Tuple<int, int>(initimateLevel1.level, initimateLevel2.level);
    }

    public PlayerCharacterIntimate GetPlayerCharacterIntimate()
    {
      if (this.playerCharacterInimate != null && !this.mModified.isChangedOnce())
        return this.playerCharacterInimate;
      if (this.playerCharacterInimate == null)
      {
        this.mModified = BL.Observe<EarthCharacterIntimate>(this);
        this.playerCharacterInimate = PlayerCharacterIntimate.CreateForKey(EarthDataManager.GetAutoIndex());
      }
      this.playerCharacterInimate._character = this.mCharacterID1;
      this.playerCharacterInimate._target_character = this.mCharacterID2;
      this.playerCharacterInimate.max_level = ((IEnumerable<InitimateBreakthrough>) MasterData.InitimateBreakthroughList).Any<InitimateBreakthrough>((Func<InitimateBreakthrough, bool>) (x =>
      {
        if (x.character_id == this.mCharacterID1 && x.target_character_id == this.mCharacterID2)
          return true;
        return x.character_id == this.mCharacterID2 && x.target_character_id == this.mCharacterID1;
      })) ? 10 : 5;
      InitimateLevel initimateLevel = ((IEnumerable<InitimateLevel>) MasterData.InitimateLevelList).FirstOrDefault<InitimateLevel>((Func<InitimateLevel, bool>) (x => x.from_exp <= this.mExperience && x.to_exp >= this.mExperience));
      if (initimateLevel == null || initimateLevel.level > this.playerCharacterInimate.max_level)
        initimateLevel = ((IEnumerable<InitimateLevel>) MasterData.InitimateLevelList).FirstOrDefault<InitimateLevel>((Func<InitimateLevel, bool>) (x => x.level == this.playerCharacterInimate.max_level));
      this.playerCharacterInimate.total_exp = this.mExperience;
      this.playerCharacterInimate.exp = this.mExperience - initimateLevel.from_exp;
      this.playerCharacterInimate.exp_next = initimateLevel.to_exp - this.mExperience;
      this.playerCharacterInimate.level = initimateLevel.level;
      return this.playerCharacterInimate;
    }

    public string GetSeverString()
    {
      return string.Format(EarthCharacterIntimate.serverDataFormat, (object) this.mCharacterID1, (object) this.mCharacterID2, (object) this.mExperience);
    }

    public static EarthCharacterIntimate JsonLoad(Dictionary<string, object> json)
    {
      return new EarthCharacterIntimate()
      {
        mCharacterID1 = (int) (long) json["character_id_1"],
        mCharacterID2 = (int) (long) json["character_id_2"],
        mExperience = (int) (long) json["experience"]
      };
    }
  }
}
