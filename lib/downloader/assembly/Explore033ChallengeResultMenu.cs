// Decompiled with JetBrains decompiler
// Type: Explore033ChallengeResultMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Explore033ChallengeResultMenu : ResultMenuBase
{
  private ResultMenuBase.Param result;

  public override IEnumerator Init(ResultMenuBase.Param param, Gladiator gladiator)
  {
    Explore033ChallengeResultMenu challengeResultMenu = this;
    challengeResultMenu.result = param;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = challengeResultMenu.\u003C\u003En__0(param, gladiator);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    challengeResultMenu.DirUnitExp.SetActive(false);
  }

  public override IEnumerator Run()
  {
    Explore033ChallengeResultMenu challengeResultMenu = this;
    Explore033ChallengeResultMenu.Runner[] runnerArray = new Explore033ChallengeResultMenu.Runner[7]
    {
      new Explore033ChallengeResultMenu.Runner(challengeResultMenu.InitObjects),
      new Explore033ChallengeResultMenu.Runner(((ResultMenuBase) challengeResultMenu).ShowUnitEXP),
      new Explore033ChallengeResultMenu.Runner(((ResultMenuBase) challengeResultMenu).SkipPopupPlay),
      new Explore033ChallengeResultMenu.Runner(((ResultMenuBase) challengeResultMenu).CharacterIntimatesPopup),
      new Explore033ChallengeResultMenu.Runner(((ResultMenuBase) challengeResultMenu).CharacterStoryPopup),
      new Explore033ChallengeResultMenu.Runner(((ResultMenuBase) challengeResultMenu).HarmonyStoryPopup),
      new Explore033ChallengeResultMenu.Runner(((ResultMenuBase) challengeResultMenu).SkipPopupPlay)
    };
    for (int index = 0; index < runnerArray.Length; ++index)
    {
      IEnumerator e = runnerArray[index]();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    runnerArray = (Explore033ChallengeResultMenu.Runner[]) null;
  }

  public override IEnumerator OnFinish()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Explore033ChallengeResultMenu challengeResultMenu = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    challengeResultMenu.DirUnitExp.SetActive(false);
    return false;
  }

  private IEnumerator InitObjects()
  {
    Explore033ChallengeResultMenu challengeResultMenu = this;
    challengeResultMenu.beforeUnits = ((IEnumerable<PlayerUnit>) challengeResultMenu.result.challenge_finish.before_player_units).ToDictionary<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.id));
    challengeResultMenu.afterUnits = ((IEnumerable<PlayerUnit>) challengeResultMenu.result.challenge_finish.after_player_units).ToDictionary<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.id));
    challengeResultMenu.beforeGears = ((IEnumerable<PlayerItem>) challengeResultMenu.result.challenge_finish.before_player_gears).ToDictionary<PlayerItem, int>((Func<PlayerItem, int>) (x => x.id));
    challengeResultMenu.afterGears = ((IEnumerable<PlayerItem>) challengeResultMenu.result.challenge_finish.after_player_gears).ToDictionary<PlayerItem, int>((Func<PlayerItem, int>) (x => x.id));
    challengeResultMenu.characterIntimates.Clear();
    challengeResultMenu.unlockCharacterQuestIDS.Clear();
    if (challengeResultMenu.result.challenge_finish.unlock_quests != null && challengeResultMenu.result.challenge_finish.unlock_quests.Length != 0)
      challengeResultMenu.unlockCharacterQuestIDS = ((IEnumerable<UnlockQuest>) challengeResultMenu.result.challenge_finish.unlock_quests).Where<UnlockQuest>((Func<UnlockQuest, bool>) (x => x.quest_type == 2)).Select<UnlockQuest, int>((Func<UnlockQuest, int>) (x => x.quest_s_id)).ToList<int>();
    challengeResultMenu.disappearedPlayerGears.Clear();
    if (challengeResultMenu.result.challenge_finish.disappeared_player_gears != null && challengeResultMenu.result.challenge_finish.disappeared_player_gears.Length != 0)
    {
      foreach (int disappearedPlayerGear in challengeResultMenu.result.challenge_finish.disappeared_player_gears)
      {
        if (challengeResultMenu.beforeGears.ContainsKey(disappearedPlayerGear))
          challengeResultMenu.disappearedPlayerGears.Add(challengeResultMenu.beforeGears[disappearedPlayerGear]);
      }
    }
    challengeResultMenu.DirUnitExp.SetActive(false);
    yield return (object) new WaitForSeconds(0.1f);
  }

  private delegate IEnumerator Runner();
}
