// Decompiled with JetBrains decompiler
// Type: SM.PlayerMissionHistory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerMissionHistory : KeyCompare
  {
    public int mission_id;
    public int story_category;
    public string reward_title;
    public bool is_clear;

    public PlayerMissionHistory()
    {
    }

    public PlayerMissionHistory(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.mission_id = (int) (long) json[nameof (mission_id)];
      this.story_category = (int) (long) json[nameof (story_category)];
      this.reward_title = json[nameof (reward_title)] == null ? (string) null : (string) json[nameof (reward_title)];
      this.is_clear = (bool) json[nameof (is_clear)];
    }
  }
}
