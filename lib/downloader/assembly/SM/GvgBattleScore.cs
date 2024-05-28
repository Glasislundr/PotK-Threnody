// Decompiled with JetBrains decompiler
// Type: SM.GvgBattleScore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class GvgBattleScore : KeyCompare
  {
    public int total_capture_star;
    public int gain_contribution;
    public int _battle_status;
    public int gain_coin;
    public int opponent_total_capture_star;
    public int total_damage;
    public int gain_experience;
    public DateTime start_dt;
    public DateTime end_dt;
    public int opponent_total_damage;

    public GvgBattleStatus battle_status
    {
      get
      {
        if (!Enum.IsDefined(typeof (GvgBattleStatus), (object) this._battle_status))
          Debug.LogError((object) ("Key not Found: MasterDataTable.GvgBattleStatus[" + (object) this._battle_status + "]"));
        return (GvgBattleStatus) this._battle_status;
      }
    }

    public GvgBattleScore()
    {
    }

    public GvgBattleScore(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.total_capture_star = (int) (long) json[nameof (total_capture_star)];
      this.gain_contribution = (int) (long) json[nameof (gain_contribution)];
      this._battle_status = (int) (long) json[nameof (battle_status)];
      this.gain_coin = (int) (long) json[nameof (gain_coin)];
      this.opponent_total_capture_star = (int) (long) json[nameof (opponent_total_capture_star)];
      this.total_damage = (int) (long) json[nameof (total_damage)];
      this.gain_experience = (int) (long) json[nameof (gain_experience)];
      this.start_dt = DateTime.Parse((string) json[nameof (start_dt)]);
      this.end_dt = DateTime.Parse((string) json[nameof (end_dt)]);
      this.opponent_total_damage = (int) (long) json[nameof (opponent_total_damage)];
    }
  }
}
