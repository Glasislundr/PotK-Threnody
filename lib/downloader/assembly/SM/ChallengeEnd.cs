// Decompiled with JetBrains decompiler
// Type: SM.ChallengeEnd
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace SM
{
  [Serializable]
  public class ChallengeEnd : KeyCompare
  {
    public PlayerItem[] after_player_gears;
    public PlayerUnit[] after_player_units;
    public ChallengeEndUnlock_messages[] unlock_messages;
    public UnlockQuest[] unlock_quests;
    public int[] disappeared_player_gears;
    public ChallengeEndChallenge_win_rewards[] challenge_win_rewards;
    public PlayerUnit[] before_player_units;
    public PlayerItem[] before_player_gears;

    public ChallengeEnd()
    {
    }

    public ChallengeEnd(Dictionary<string, object> json)
    {
      this._hasKey = false;
      List<PlayerItem> playerItemList1 = new List<PlayerItem>();
      foreach (object json1 in (List<object>) json[nameof (after_player_gears)])
        playerItemList1.Add(json1 == null ? (PlayerItem) null : new PlayerItem((Dictionary<string, object>) json1));
      this.after_player_gears = playerItemList1.ToArray();
      List<PlayerUnit> playerUnitList1 = new List<PlayerUnit>();
      foreach (object json2 in (List<object>) json[nameof (after_player_units)])
        playerUnitList1.Add(json2 == null ? (PlayerUnit) null : new PlayerUnit((Dictionary<string, object>) json2));
      this.after_player_units = playerUnitList1.ToArray();
      List<ChallengeEndUnlock_messages> endUnlockMessagesList = new List<ChallengeEndUnlock_messages>();
      foreach (object json3 in (List<object>) json[nameof (unlock_messages)])
        endUnlockMessagesList.Add(json3 == null ? (ChallengeEndUnlock_messages) null : new ChallengeEndUnlock_messages((Dictionary<string, object>) json3));
      this.unlock_messages = endUnlockMessagesList.ToArray();
      List<UnlockQuest> unlockQuestList = new List<UnlockQuest>();
      foreach (object json4 in (List<object>) json[nameof (unlock_quests)])
        unlockQuestList.Add(json4 == null ? (UnlockQuest) null : new UnlockQuest((Dictionary<string, object>) json4));
      this.unlock_quests = unlockQuestList.ToArray();
      this.disappeared_player_gears = ((IEnumerable<object>) json[nameof (disappeared_player_gears)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      List<ChallengeEndChallenge_win_rewards> challengeWinRewardsList = new List<ChallengeEndChallenge_win_rewards>();
      foreach (object json5 in (List<object>) json[nameof (challenge_win_rewards)])
        challengeWinRewardsList.Add(json5 == null ? (ChallengeEndChallenge_win_rewards) null : new ChallengeEndChallenge_win_rewards((Dictionary<string, object>) json5));
      this.challenge_win_rewards = challengeWinRewardsList.ToArray();
      List<PlayerUnit> playerUnitList2 = new List<PlayerUnit>();
      foreach (object json6 in (List<object>) json[nameof (before_player_units)])
        playerUnitList2.Add(json6 == null ? (PlayerUnit) null : new PlayerUnit((Dictionary<string, object>) json6));
      this.before_player_units = playerUnitList2.ToArray();
      List<PlayerItem> playerItemList2 = new List<PlayerItem>();
      foreach (object json7 in (List<object>) json[nameof (before_player_gears)])
        playerItemList2.Add(json7 == null ? (PlayerItem) null : new PlayerItem((Dictionary<string, object>) json7));
      this.before_player_gears = playerItemList2.ToArray();
    }
  }
}
