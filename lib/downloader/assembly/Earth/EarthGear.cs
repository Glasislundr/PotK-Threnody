// Decompiled with JetBrains decompiler
// Type: Earth.EarthGear
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
namespace Earth
{
  [Serializable]
  public class EarthGear : BL.ModelBase
  {
    private int mID;
    private int mItemID;
    private int mExperience;
    private bool mIsFavorite;
    private bool mIsLost;
    [NonSerialized]
    private PlayerItem mPlayerItem;
    [NonSerialized]
    private BL.BattleModified<EarthGear> mModified;
    private static readonly string serverDataFormat = "{{\"id\":{0},\"gear_id\":{1},\"experience\":{2},\"favorite\":{3},\"is_lost\":{4}}}";

    public int ID => this.mID;

    public int experience => this.mExperience;

    public bool favorite
    {
      get => this.mIsFavorite;
      set
      {
        if (this.mIsFavorite == value)
          return;
        this.mIsFavorite = value;
        this.commit();
      }
    }

    public bool isLost => this.mIsLost;

    public int gearID => this.mItemID;

    public GearGear gear => MasterData.GearGear[this.mItemID];

    public static EarthGear Create(int gearID)
    {
      return new EarthGear()
      {
        mID = EarthDataManager.GetAutoIndex(),
        mItemID = gearID,
        mExperience = 0,
        mIsLost = false
      };
    }

    public void AddExperience(int experience)
    {
      this.mExperience = Mathf.Max(0, this.mExperience + experience);
      if (((IEnumerable<GearRankExp>) MasterData.GearRankExpList).FirstOrDefault<GearRankExp>((Func<GearRankExp, bool>) (x => x.from_exp <= this.mExperience && x.to_exp >= this.mExperience)) == null)
        this.mExperience = ((IEnumerable<GearRankExp>) MasterData.GearRankExpList).OrderByDescending<GearRankExp, int>((Func<GearRankExp, int>) (x => x.level)).First<GearRankExp>().from_exp;
      this.commit();
    }

    public void SetLost() => this.mIsLost = true;

    public PlayerItem GetPlayerItem(bool isCopy = false)
    {
      if (this.mPlayerItem != (PlayerItem) null && !this.mModified.isChangedOnce())
        return isCopy ? CopyUtil.DeepCopy<PlayerItem>(this.mPlayerItem) : this.mPlayerItem;
      if (this.mPlayerItem == (PlayerItem) null)
      {
        this.mModified = new BL.BattleModified<EarthGear>(this);
        this.mPlayerItem = PlayerItem.CreateForKey(this.mID);
        this.mPlayerItem.player_id = SMManager.Get<Player>().id;
        this.mPlayerItem.id = this.mID;
        this.mPlayerItem._entity_type = 3;
        this.mPlayerItem.entity_id = this.mItemID;
        this.mPlayerItem.broken = false;
        this.mPlayerItem.is_new = false;
        this.mPlayerItem.for_battle = false;
        this.mPlayerItem.box_type_id = 0;
        this.mPlayerItem.quantity = 1;
        this.mPlayerItem.gear_level_limit = 5;
        this.mPlayerItem.gear_level_limit_max = 5;
      }
      this.mPlayerItem.favorite = this.mIsFavorite;
      GearRankExp gearRankExp = ((IEnumerable<GearRankExp>) MasterData.GearRankExpList).FirstOrDefault<GearRankExp>((Func<GearRankExp, bool>) (x => x.from_exp <= this.mExperience && x.to_exp >= this.mExperience));
      this.mPlayerItem.gear_total_exp = this.mExperience;
      this.mPlayerItem.gear_exp = this.mExperience - gearRankExp.from_exp;
      this.mPlayerItem.gear_exp_next = gearRankExp.to_exp - this.mExperience;
      this.mPlayerItem.gear_level = gearRankExp.level;
      return isCopy ? CopyUtil.DeepCopy<PlayerItem>(this.mPlayerItem) : this.mPlayerItem;
    }

    public string GetSeverString()
    {
      return string.Format(EarthGear.serverDataFormat, (object) this.mID, (object) this.mItemID, (object) this.mExperience, (object) (this.mIsFavorite ? 1 : 0), (object) (this.mIsLost ? 1 : 0));
    }

    public static EarthGear JsonLoad(Dictionary<string, object> json)
    {
      return new EarthGear()
      {
        mID = (int) (long) json["id"],
        mItemID = (int) (long) json["gear_id"],
        mExperience = (int) (long) json["experience"],
        mIsFavorite = (int) (long) json["favorite"] != 0,
        mIsLost = json.ContainsKey("is_lost") && (int) (long) json["is_lost"] != 0
      };
    }
  }
}
