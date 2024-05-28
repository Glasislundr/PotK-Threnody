// Decompiled with JetBrains decompiler
// Type: Story00913ScrollParts
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
public class Story00913ScrollParts : MonoBehaviour
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
  private PlayerSeaQuestS quest;
  private NGMenuBase menu;

  public void onClickChapterButton()
  {
    if (this.menu.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
    Singleton<NGSceneManager>.GetInstance().changeScene("story009_13_2", false, (object) this.quest);
  }

  public void Init(Story00913Menu menu, PlayerSeaQuestS quest)
  {
    this.quest = quest;
    this.menu = (NGMenuBase) menu;
    EventDelegate.Add(this.IbtnChapter.onClick, new EventDelegate.Callback(this.onClickChapterButton));
    ((Component) this.SlcNew).gameObject.SetActive(false);
    this.TxtChapter.SetTextLocalize(quest.quest_sea_s.quest_m.name);
    ((IEnumerable<PlayerSeaQuestS>) SMManager.Get<PlayerSeaQuestS[]>().S(quest.quest_sea_s.quest_xl.ID, quest.quest_sea_s.quest_l.ID, quest.quest_sea_s.quest_m.ID)).OrderBy<PlayerSeaQuestS, int>((Func<PlayerSeaQuestS, int>) (x => x.quest_sea_s.ID)).ForEach<PlayerSeaQuestS>((Action<PlayerSeaQuestS>) (q =>
    {
      if (!q.is_clear || q.quest_sea_s.StoryDetails().Length == 0)
        return;
      foreach (StoryPlaybackSeaDetail storyDetail in q.quest_sea_s.StoryDetails())
      {
        if (PlayerPrefs.GetInt("SeaLastScriptId", -1) == storyDetail.script_id)
          this.SlcLastPlay.SetActive(true);
      }
    }));
  }
}
