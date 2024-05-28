// Decompiled with JetBrains decompiler
// Type: Quest00240723Scene
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
public class Quest00240723Scene : NGSceneBase
{
  public Quest00240723Menu menu;

  public static void ChangeScene0024(bool stack, int L, bool passOriginID, bool mustCreateBG)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_4", (stack ? 1 : 0) != 0, (object) L, (object) passOriginID, (object) mustCreateBG);
  }

  public static void ChangeScene0024(bool stack, int L, bool mustCreateBG)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_4", (stack ? 1 : 0) != 0, (object) L, (object) mustCreateBG);
  }

  public IEnumerator onStartSceneAsync(int L, bool passOriginID, bool mustCreateBG)
  {
    Quest00240723Scene quest00240723Scene = this;
    QuestBG backgroundComponent = Singleton<CommonRoot>.GetInstance().getBackgroundComponent<QuestBG>();
    if (Object.op_Inequality((Object) backgroundComponent, (Object) null))
      backgroundComponent.currentPos = QuestBG.QuestPosition.Story;
    PlayerStoryQuestS[] StoryData = SMManager.Get<PlayerStoryQuestS[]>();
    quest00240723Scene.tweens = (UITweener[]) null;
    if (passOriginID)
      L = quest00240723Scene.SeekChoiceLnum(StoryData, L);
    quest00240723Scene.menu.from2_5 = true;
    IEnumerator e = quest00240723Scene.menu.Init(StoryData, L, mustCreateBG: mustCreateBG);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(int L, bool mustCreateBG)
  {
    Quest00240723Scene quest00240723Scene = this;
    PlayerStoryQuestS[] StoryData = SMManager.Get<PlayerStoryQuestS[]>();
    quest00240723Scene.tweens = (UITweener[]) null;
    quest00240723Scene.menu.from2_5 = false;
    IEnumerator e = quest00240723Scene.menu.Init(StoryData, L, mustCreateBG: mustCreateBG);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private int SeekChoiceLnum(PlayerStoryQuestS[] StoryData, int L)
  {
    return ((IEnumerable<PlayerStoryQuestS>) StoryData).Where<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (x => x.quest_story_s.quest_l_QuestStoryL == L && x.quest_story_s.quest_l.origin_id.HasValue)).Count<PlayerStoryQuestS>() != 0 || !((IEnumerable<PlayerStoryQuestS>) StoryData).Where<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (x => x.quest_story_s.quest_l.origin_id.HasValue)).Select<PlayerStoryQuestS, int>((Func<PlayerStoryQuestS, int>) (x => x.quest_story_s.quest_l.origin_id.Value)).Contains<int>(L) ? L : ((IEnumerable<PlayerStoryQuestS>) StoryData).Where<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (x => x.quest_story_s.quest_l.origin_id.HasValue && x.quest_story_s.quest_l.origin_id.Value == L)).Select<PlayerStoryQuestS, int>((Func<PlayerStoryQuestS, int>) (y => y.quest_story_s.quest_l_QuestStoryL)).First<int>();
  }
}
