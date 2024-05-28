// Decompiled with JetBrains decompiler
// Type: SM.PlayerQuestScoreExtraS
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerQuestScoreExtraS : KeyCompare
  {
    public int quest_extra_s;
    public int score_max;

    public PlayerQuestScoreExtraS()
    {
    }

    public PlayerQuestScoreExtraS(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.quest_extra_s = (int) (long) json[nameof (quest_extra_s)];
      this.score_max = (int) (long) json[nameof (score_max)];
    }
  }
}
