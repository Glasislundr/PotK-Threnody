// Decompiled with JetBrains decompiler
// Type: SM.PlayerChallenge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerChallenge : KeyCompare
  {
    public string player_id;
    public int lose_count;
    public int score;
    public int rank;
    public int win_count;

    public PlayerChallenge()
    {
    }

    public PlayerChallenge(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.player_id = (string) json[nameof (player_id)];
      this.lose_count = (int) (long) json[nameof (lose_count)];
      this.score = (int) (long) json[nameof (score)];
      this.rank = (int) (long) json[nameof (rank)];
      this.win_count = (int) (long) json[nameof (win_count)];
    }
  }
}
