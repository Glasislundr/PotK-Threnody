// Decompiled with JetBrains decompiler
// Type: SM.PlayerCallMission
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerCallMission : KeyCompare
  {
    public int count;
    public int mission_id;
    public PlayerCallMissionReward[] rewards;
    public int player_mission_id;
    public int mission_status;
    public int sequentialNumber;

    public PlayerCallMission()
    {
    }

    public PlayerCallMission(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.count = (int) (long) json[nameof (count)];
      this.mission_id = (int) (long) json[nameof (mission_id)];
      List<PlayerCallMissionReward> callMissionRewardList = new List<PlayerCallMissionReward>();
      foreach (object json1 in (List<object>) json[nameof (rewards)])
        callMissionRewardList.Add(json1 == null ? (PlayerCallMissionReward) null : new PlayerCallMissionReward((Dictionary<string, object>) json1));
      this.rewards = callMissionRewardList.ToArray();
      this.player_mission_id = (int) (long) json[nameof (player_mission_id)];
      this.mission_status = (int) (long) json[nameof (mission_status)];
    }
  }
}
