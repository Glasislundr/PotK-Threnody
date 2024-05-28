// Decompiled with JetBrains decompiler
// Type: Story00922Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Story00922Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  private NGxScroll ScrollContainer;
  private PlayerStoryQuestS quest;
  private int XL;

  public virtual void Foreground()
  {
  }

  public virtual void VScrollBar()
  {
  }

  public override void onBackButton() => this.IbtnBack();

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("story009_2", false, (object) this.quest, (object) this.XL);
  }

  public IEnumerator InitEpisodeButton(PlayerStoryQuestS[] quests, PlayerStoryQuestS quest, int XL)
  {
    Story00922Menu menu = this;
    menu.quest = quest;
    menu.XL = XL;
    Array.Reverse((Array) quests);
    Future<GameObject> prefabScrollPartsF = Res.Prefabs.story009_2_2.story009_2_2_button.Load<GameObject>();
    IEnumerator e = prefabScrollPartsF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabScrollPartsF.Result;
    List<Story0093Scene.ContinuousData> continuousDataList = new List<Story0093Scene.ContinuousData>();
    List<StoryPlaybackStoryDetail> storyList = new List<StoryPlaybackStoryDetail>();
    ((IEnumerable<PlayerStoryQuestS>) quests).OrderBy<PlayerStoryQuestS, int>((Func<PlayerStoryQuestS, int>) (x => x.quest_story_s.ID)).ForEach<PlayerStoryQuestS>((Action<PlayerStoryQuestS>) (q =>
    {
      if (!q.is_clear || q.quest_story_s.StoryDetails().Length == 0)
        return;
      foreach (StoryPlaybackStoryDetail storyDetail in q.quest_story_s.StoryDetails())
      {
        storyList.Add(storyDetail);
        continuousDataList.Add(new Story0093Scene.ContinuousData()
        {
          scriptId_ = storyDetail.script_id,
          continuousFlag_ = storyDetail.continuous_flag
        });
      }
    }));
    foreach (StoryPlaybackStoryDetail story in storyList)
    {
      Story00922ScrollParts component = result.CloneAndGetComponent<Story00922ScrollParts>();
      menu.ScrollContainer.Add(((Component) component).gameObject);
      component.Init(menu, story, continuousDataList, XL);
    }
    menu.ScrollContainer.ResolvePosition();
  }
}
