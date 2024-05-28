// Decompiled with JetBrains decompiler
// Type: SM.PvPClassRecord
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PvPClassRecord : KeyCompare
  {
    public int current_season_win_count;
    public int top3;
    public int consecutive_top100;
    public int top1000;
    public int season_consecutive_title;
    public int consecutive_top3;
    public int season_class_up;
    public int current_season_loss_count;
    public int top;
    public int top10;
    public int top30;
    public int top100;
    public int current_season_draw_count;
    public int season_title_count;
    public int best_ranking;
    public int season_class_stay;
    public int consecutive_top;
    public PvPRecord pvp_record;
    public bool has_pvp_battle_history;
    public int season_class_down;
    public int consecutive_top30;
    public int consecutive_top10;

    public PvPClassRecord()
    {
    }

    public PvPClassRecord(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.current_season_win_count = (int) (long) json[nameof (current_season_win_count)];
      this.top3 = (int) (long) json[nameof (top3)];
      this.consecutive_top100 = (int) (long) json[nameof (consecutive_top100)];
      this.top1000 = (int) (long) json[nameof (top1000)];
      this.season_consecutive_title = (int) (long) json[nameof (season_consecutive_title)];
      this.consecutive_top3 = (int) (long) json[nameof (consecutive_top3)];
      this.season_class_up = (int) (long) json[nameof (season_class_up)];
      this.current_season_loss_count = (int) (long) json[nameof (current_season_loss_count)];
      this.top = (int) (long) json[nameof (top)];
      this.top10 = (int) (long) json[nameof (top10)];
      this.top30 = (int) (long) json[nameof (top30)];
      this.top100 = (int) (long) json[nameof (top100)];
      this.current_season_draw_count = (int) (long) json[nameof (current_season_draw_count)];
      this.season_title_count = (int) (long) json[nameof (season_title_count)];
      this.best_ranking = (int) (long) json[nameof (best_ranking)];
      this.season_class_stay = (int) (long) json[nameof (season_class_stay)];
      this.consecutive_top = (int) (long) json[nameof (consecutive_top)];
      this.pvp_record = json[nameof (pvp_record)] == null ? (PvPRecord) null : new PvPRecord((Dictionary<string, object>) json[nameof (pvp_record)]);
      this.has_pvp_battle_history = (bool) json[nameof (has_pvp_battle_history)];
      this.season_class_down = (int) (long) json[nameof (season_class_down)];
      this.consecutive_top30 = (int) (long) json[nameof (consecutive_top30)];
      this.consecutive_top10 = (int) (long) json[nameof (consecutive_top10)];
    }
  }
}
