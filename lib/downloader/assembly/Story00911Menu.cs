// Decompiled with JetBrains decompiler
// Type: Story00911Menu
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
public class Story00911Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  private NGxScroll ScrollContainer;
  [SerializeField]
  private UI2DSprite DynCharacter;
  [SerializeField]
  private UI2DSprite DynCharacter2;
  [SerializeField]
  private GameObject DirNoStory;

  public IEnumerator Init(int id, int id2)
  {
    Story00911Menu story00911Menu = this;
    story00911Menu.TxtTitle.SetTextLocalize(Consts.Format(Consts.GetInstance().COMBI_QUEST_TITLE, (IDictionary) new Hashtable()
    {
      {
        (object) "name",
        (object) MasterData.UnitUnit[id].name
      },
      {
        (object) "name2",
        (object) MasterData.UnitUnit[id2].name
      }
    }));
    IEnumerator e = story00911Menu.SetCharacterLargeImage(id, id2);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    story00911Menu.ScrollContainer.Clear();
    e = story00911Menu.CreateEpisodes(id, id2);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    story00911Menu.ScrollContainer.ResolvePosition();
    // ISSUE: method pointer
    story00911Menu.ScrollContainer.GridReposition(new UIGrid.OnReposition((object) story00911Menu, __methodptr(\u003CInit\u003Eb__5_0)));
  }

  public override void onBackButton() => this.IbtnBack();

  public void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("story009_10", false);
  }

  private IEnumerator SetCharacterLargeImage(int id, int id2)
  {
    IEnumerator e = this.SetCharacterLargeImage(((Component) this.DynCharacter).transform, id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.SetCharacterLargeImage(((Component) this.DynCharacter2).transform, id2);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator SetCharacterLargeImage(Transform trans, int id)
  {
    trans.Clear();
    UnitUnit unit = MasterData.UnitUnit[id];
    Future<GameObject> goFuture = unit.LoadMypage();
    IEnumerator e = goFuture.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = unit.SetLargeSpriteSnap(goFuture.Result.Clone(trans), 5);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator CreateEpisodes(int id, int id2)
  {
    Story00911Menu menu = this;
    Future<GameObject> prefabEpisodeF = Res.Prefabs.story009_6.story0096DirEpisode.Load<GameObject>();
    IEnumerator e = prefabEpisodeF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabEpisodeF.Result;
    IEnumerable<PlayerHarmonyQuestS> source = ((IEnumerable<PlayerHarmonyQuestS>) SMManager.Get<PlayerHarmonyQuestS[]>()).Where<PlayerHarmonyQuestS>((Func<PlayerHarmonyQuestS, bool>) (x => x.is_clear));
    List<PlayerHarmonyQuestS> questsList = new List<PlayerHarmonyQuestS>();
    List<StoryPlaybackHarmonyDetail> playbackList = new List<StoryPlaybackHarmonyDetail>();
    List<Story0093Scene.ContinuousData> continuousDataList = new List<Story0093Scene.ContinuousData>();
    source.OrderBy<PlayerHarmonyQuestS, int>((Func<PlayerHarmonyQuestS, int>) (x => x.quest_harmony_s.ID)).ForEach<PlayerHarmonyQuestS>((Action<PlayerHarmonyQuestS>) (qu =>
    {
      if (qu.quest_harmony_s.unit.ID != id && qu.quest_harmony_s.unit.ID != id2 || qu.quest_harmony_s.target_unit.ID != id && qu.quest_harmony_s.target_unit.ID != id2)
        return;
      ((IEnumerable<StoryPlaybackHarmonyDetail>) MasterData.StoryPlaybackHarmonyDetailList).Where<StoryPlaybackHarmonyDetail>((Func<StoryPlaybackHarmonyDetail, bool>) (x => x.quest.ID == qu.quest_harmony_s.ID && x.timing_StoryPlaybackTiming != 2)).OrderBy<StoryPlaybackHarmonyDetail, int>((Func<StoryPlaybackHarmonyDetail, int>) (x => x.ID)).ForEach<StoryPlaybackHarmonyDetail>((Action<StoryPlaybackHarmonyDetail>) (story =>
      {
        questsList.Add(qu);
        playbackList.Add(story);
        continuousDataList.Add(new Story0093Scene.ContinuousData()
        {
          scriptId_ = story.script_id,
          continuousFlag_ = story.continuous_flag
        });
      }));
    }));
    for (int index = 0; index < questsList.Count; ++index)
    {
      GameObject gameObject = result.Clone();
      menu.ScrollContainer.Add(gameObject);
      gameObject.GetComponent<Story0096EpisodeParts>().setData(questsList[index], playbackList[index], (NGMenuBase) menu, continuousDataList);
    }
    menu.DirNoStory.SetActive(((Component) menu.ScrollContainer.grid).transform.childCount <= 0);
  }
}
