// Decompiled with JetBrains decompiler
// Type: SM.PlayerBingoPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerBingoPanel : KeyCompare
  {
    public int count;
    public int panel_id;
    public BingoRewardGroup[] rewards;
    public int bingo_id;
    public bool is_open;
    public int bingo_panel_id;
    public int id;
    public bool is_reward_get;

    public PlayerBingoPanel()
    {
    }

    public PlayerBingoPanel(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.count = (int) (long) json[nameof (count)];
      this.panel_id = (int) (long) json[nameof (panel_id)];
      List<BingoRewardGroup> bingoRewardGroupList = new List<BingoRewardGroup>();
      foreach (object json1 in (List<object>) json[nameof (rewards)])
        bingoRewardGroupList.Add(json1 == null ? (BingoRewardGroup) null : new BingoRewardGroup((Dictionary<string, object>) json1));
      this.rewards = bingoRewardGroupList.ToArray();
      this.bingo_id = (int) (long) json[nameof (bingo_id)];
      this.is_open = (bool) json[nameof (is_open)];
      this.bingo_panel_id = (int) (long) json[nameof (bingo_panel_id)];
      this.id = (int) (long) json[nameof (id)];
      this.is_reward_get = (bool) json[nameof (is_reward_get)];
    }
  }
}
