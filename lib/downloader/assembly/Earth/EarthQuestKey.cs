// Decompiled with JetBrains decompiler
// Type: Earth.EarthQuestKey
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Earth
{
  [Serializable]
  public class EarthQuestKey : BL.ModelBase
  {
    private int mKeyID;
    private int mQuantity;
    [NonSerialized]
    private PlayerQuestKey mQuestKey;
    [NonSerialized]
    private BL.BattleModified<EarthQuestKey> mModified;
    private static readonly string serverDataFormat = "{{\"key_id\":{0},\"quantity\":{1}}}";

    public int ID => this.mKeyID;

    public int quantity
    {
      get => this.mQuantity;
      set
      {
        if (this.mQuantity == value)
          return;
        this.mQuantity = Mathf.Max(0, value);
        this.commit();
      }
    }

    public int keyID => this.mKeyID;

    public QuestkeyQuestkey questKey => MasterData.QuestkeyQuestkey[this.mKeyID];

    public static EarthQuestKey Create(int keyID, int quantity)
    {
      return new EarthQuestKey()
      {
        mKeyID = keyID,
        mQuantity = quantity
      };
    }

    public void UseItem(int count) => this.quantity = Mathf.Max(0, this.quantity - count);

    private PlayerQuestKey CreatePlayerQuestKey()
    {
      PlayerQuestKey forKey = PlayerQuestKey.CreateForKey(EarthDataManager.GetAutoIndex());
      forKey.player_id = SMManager.Get<Player>().id;
      forKey.quest_key_id = this.mKeyID;
      forKey.max_quantity = MasterData.EarthQuestKey.ContainsKey(this.mKeyID) ? MasterData.EarthQuestKey[this.keyID].max_stack : 0;
      forKey.quantity = 0;
      return forKey;
    }

    public PlayerQuestKey GetQuestKey(bool isCopy = false)
    {
      if (this.mQuestKey != null && !this.mModified.isChangedOnce())
        return isCopy ? CopyUtil.DeepCopy<PlayerQuestKey>(this.mQuestKey) : this.mQuestKey;
      if (this.mQuestKey == null)
      {
        this.mModified = BL.Observe<EarthQuestKey>(this);
        this.mQuestKey = PlayerQuestKey.CreateForKey(this.mKeyID);
      }
      this.mQuestKey.max_quantity = MasterData.EarthQuestKey.ContainsKey(this.mKeyID) ? MasterData.EarthQuestKey[this.keyID].max_stack : 0;
      this.mQuestKey.quest_key_id = this.mKeyID;
      this.mQuestKey.quantity = this.mQuantity;
      return this.mQuestKey;
    }

    public string GetSeverString()
    {
      return string.Format(EarthQuestKey.serverDataFormat, (object) this.mKeyID, (object) this.mQuantity);
    }

    public static EarthQuestKey JsonLoad(Dictionary<string, object> json)
    {
      return new EarthQuestKey()
      {
        mKeyID = (int) (long) json["key_id"],
        mQuantity = (int) (long) json["quantity"]
      };
    }
  }
}
