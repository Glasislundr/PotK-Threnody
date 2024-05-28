// Decompiled with JetBrains decompiler
// Type: Story0092ScrollParts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Story0092ScrollParts : MonoBehaviour
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
  private PlayerStoryQuestS quest;
  private NGMenuBase menu;
  private int XL;

  public void onClickChapterButton()
  {
    if (this.menu.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
    Singleton<NGSceneManager>.GetInstance().changeScene("story009_2_2", false, (object) this.quest, (object) this.XL);
  }

  public void Init(
    Story0092Menu menu,
    PlayerStoryQuestS quest,
    PlayerStoryQuestS backQuest,
    int XL)
  {
    this.quest = quest;
    this.menu = (NGMenuBase) menu;
    this.XL = XL;
    EventDelegate.Add(this.IbtnChapter.onClick, new EventDelegate.Callback(this.onClickChapterButton));
    ((Component) this.SlcNew).gameObject.SetActive(false);
    this.TxtChapter.SetTextLocalize(quest.quest_story_s.quest_l.short_name + " " + quest.quest_story_s.quest_m.short_name + " " + quest.quest_story_s.quest_m.name);
    int lastScriptId = -1;
    switch (XL)
    {
      case 1:
        lastScriptId = PlayerPrefs.GetInt("HeavenLastScriptId", -1);
        break;
      case 4:
        lastScriptId = PlayerPrefs.GetInt("LostLastScriptId", -1);
        break;
      case 6:
        lastScriptId = PlayerPrefs.GetInt("IntegralLastScriptId", -1);
        break;
      case 7:
        lastScriptId = PlayerPrefs.GetInt("EverafterLastScriptId", -1);
        break;
    }
    ((IEnumerable<PlayerStoryQuestS>) SMManager.Get<PlayerStoryQuestS[]>().S(quest.quest_story_s.quest_xl.ID, quest.quest_story_s.quest_l.ID, quest.quest_story_s.quest_m.ID)).OrderBy<PlayerStoryQuestS, int>((Func<PlayerStoryQuestS, int>) (x => x.quest_story_s.ID)).ForEach<PlayerStoryQuestS>((Action<PlayerStoryQuestS>) (q =>
    {
      if (!q.is_clear || q.quest_story_s.StoryDetails().Length == 0)
        return;
      foreach (StoryPlaybackStoryDetail storyDetail in q.quest_story_s.StoryDetails())
      {
        if (lastScriptId == storyDetail.script_id)
          this.SlcLastPlay.SetActive(true);
      }
    }));
  }
}
