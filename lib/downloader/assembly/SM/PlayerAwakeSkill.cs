// Decompiled with JetBrains decompiler
// Type: SM.PlayerAwakeSkill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerAwakeSkill : KeyCompare
  {
    public int skill_id;
    public bool favorite;
    public int id;
    public int level;

    public PlayerAwakeSkill()
    {
    }

    public PlayerAwakeSkill(Dictionary<string, object> json)
    {
      this._hasKey = true;
      this.skill_id = (int) (long) json[nameof (skill_id)];
      this.favorite = (bool) json[nameof (favorite)];
      this._key = (object) (this.id = (int) (long) json[nameof (id)]);
      this.level = (int) (long) json[nameof (level)];
    }

    public BattleskillSkill masterData
    {
      get
      {
        BattleskillSkill masterData = (BattleskillSkill) null;
        MasterData.BattleskillSkill.TryGetValue(this.skill_id, out masterData);
        return masterData;
      }
    }

    public PlayerUnit EqupmentUnit
    {
      get
      {
        PlayerUnit[] source = SMManager.Get<PlayerUnit[]>();
        if (source == null)
          return (PlayerUnit) null;
        PlayerUnit playerUnit = ((IEnumerable<PlayerUnit>) source).FirstOrDefault<PlayerUnit>((Func<PlayerUnit, bool>) (x => ((IEnumerable<int?>) x.equip_awake_skill_ids).Contains<int?>(new int?(this.id))));
        return playerUnit == (PlayerUnit) null ? (PlayerUnit) null : playerUnit;
      }
    }
  }
}
