// Decompiled with JetBrains decompiler
// Type: SM.GuildAppearance
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
  public class GuildAppearance : KeyCompare
  {
    public string master_player_id;
    public int lose_num;
    public int level;
    public int tower_rank;
    public int walls_rank;
    public int experience;
    public int draw_num;
    public int membership_capacity;
    public int scaffold_rank;
    public int experience_next;
    public int ranking_no;
    public string master_player_name;
    public string broadcast_message;
    public int win_num;
    public int _current_emblem;
    public int membership_num;

    public GuildEmblemUnit current_emblem
    {
      get
      {
        if (MasterData.GuildEmblemUnit.ContainsKey(this._current_emblem))
          return MasterData.GuildEmblemUnit[this._current_emblem];
        Debug.LogError((object) ("Key not Found: MasterData.GuildEmblemUnit[" + (object) this._current_emblem + "]"));
        return (GuildEmblemUnit) null;
      }
    }

    public GuildAppearance()
    {
    }

    public GuildAppearance(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.master_player_id = (string) json[nameof (master_player_id)];
      this.lose_num = (int) (long) json[nameof (lose_num)];
      this.level = (int) (long) json[nameof (level)];
      this.tower_rank = (int) (long) json[nameof (tower_rank)];
      this.walls_rank = (int) (long) json[nameof (walls_rank)];
      this.experience = (int) (long) json[nameof (experience)];
      this.draw_num = (int) (long) json[nameof (draw_num)];
      this.membership_capacity = (int) (long) json[nameof (membership_capacity)];
      this.scaffold_rank = (int) (long) json[nameof (scaffold_rank)];
      this.experience_next = (int) (long) json[nameof (experience_next)];
      this.ranking_no = (int) (long) json[nameof (ranking_no)];
      this.master_player_name = (string) json[nameof (master_player_name)];
      this.broadcast_message = (string) json[nameof (broadcast_message)];
      this.win_num = (int) (long) json[nameof (win_num)];
      this._current_emblem = (int) (long) json[nameof (current_emblem)];
      this.membership_num = (int) (long) json[nameof (membership_num)];
    }

    public bool GuildHQOpen() => this.level > 1;
  }
}
