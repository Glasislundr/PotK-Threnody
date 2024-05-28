// Decompiled with JetBrains decompiler
// Type: Story00983Scroll
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Story00983Scroll : MonoBehaviour
{
  [SerializeField]
  private UISprite IbtnChapterSprite;
  [SerializeField]
  private UIButton IbtnChapter;
  [SerializeField]
  private UISprite SlcNew;
  [SerializeField]
  private UILabel TxtChapter;
  [SerializeField]
  private GameObject SlcLastPlay;
  private QuestExtraS quest;
  private NGMenuBase menu;

  public void onClickChapterButton()
  {
    if (this.menu.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
    Singleton<NGSceneManager>.GetInstance().changeScene("story009_8_5", false, (object) this.quest.ID, (object) true);
  }

  public void Init(QuestExtraS quest, NGMenuBase menu)
  {
    this.quest = quest;
    this.menu = menu;
    EventDelegate.Add(this.IbtnChapter.onClick, new EventDelegate.Callback(this.onClickChapterButton));
    ((Component) this.SlcNew).gameObject.SetActive(false);
    this.TxtChapter.SetTextLocalize(quest.quest_m.name);
    int[] clearedIDs = Singleton<NGGameDataManager>.GetInstance().clearedExtraQuestSIds;
    foreach (QuestExtraS questExtraS in ((IEnumerable<StoryPlaybackExtra>) MasterData.StoryPlaybackExtraList.DisplayList(ServerTime.NowAppTime())).Where<StoryPlaybackExtra>((Func<StoryPlaybackExtra, bool>) (x => x.quest.quest_m.ID == quest.quest_m.ID && ((IEnumerable<int>) clearedIDs).Contains<int>(x.quest.ID))).OrderBy<StoryPlaybackExtra, int>((Func<StoryPlaybackExtra, int>) (x => x.quest.priority)).Select<StoryPlaybackExtra, QuestExtraS>((Func<StoryPlaybackExtra, QuestExtraS>) (x => x.quest)).ToList<QuestExtraS>())
    {
      QuestExtraS extraData = questExtraS;
      ((IEnumerable<StoryPlaybackExtraDetail>) MasterData.StoryPlaybackExtraDetailList).ForEach<StoryPlaybackExtraDetail>((Action<StoryPlaybackExtraDetail>) (detail =>
      {
        if (detail.quest != extraData || detail.timing == StoryPlaybackTiming.located_player_unit || PlayerPrefs.GetInt("ExtraLastScriptId", -1) != detail.script_id)
          return;
        this.SlcLastPlay.SetActive(true);
      }));
    }
  }
}
