// Decompiled with JetBrains decompiler
// Type: Story00983Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Story00983Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtChapter;
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  private NGxScroll ScrollContainer;

  public virtual void Foreground()
  {
  }

  public virtual void IbtnChapter05()
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
    Singleton<NGSceneManager>.GetInstance().changeScene("story009_8", false, (object) false);
  }

  public IEnumerator InitScene(QuestExtraS extra)
  {
    int l_id = extra.quest_l.ID;
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    DateTime serverTime = ServerTime.NowAppTime();
    int[] clearedIDs = Singleton<NGGameDataManager>.GetInstance().clearedExtraQuestSIds;
    List<QuestExtraS> list = ((IEnumerable<StoryPlaybackExtra>) MasterData.StoryPlaybackExtraList.DisplayList(serverTime)).Where<StoryPlaybackExtra>((Func<StoryPlaybackExtra, bool>) (x => x.quest.quest_l.ID == l_id && ((IEnumerable<int>) clearedIDs).Contains<int>(x.quest.ID))).Distinct<StoryPlaybackExtra>((IEqualityComparer<StoryPlaybackExtra>) new LambdaEqualityComparer<StoryPlaybackExtra>((Func<StoryPlaybackExtra, StoryPlaybackExtra, bool>) ((a, b) => a.quest.quest_m.ID == b.quest.quest_m.ID))).OrderBy<StoryPlaybackExtra, int>((Func<StoryPlaybackExtra, int>) (x => x.quest.quest_m.priority)).Select<StoryPlaybackExtra, QuestExtraS>((Func<StoryPlaybackExtra, QuestExtraS>) (x => x.quest)).ToList<QuestExtraS>();
    Future<GameObject> prefabScrollPartsF = Res.Prefabs.story009_8_3.story00983scroll.Load<GameObject>();
    e = prefabScrollPartsF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabScrollPartsF.Result;
    foreach (QuestExtraS extra1 in list)
      this.ScrollInit(extra1, result);
  }

  public void ScrollInit(QuestExtraS extra, GameObject prefab)
  {
    GameObject gameObject = Object.Instantiate<GameObject>(prefab);
    gameObject.GetComponent<Story00983Scroll>().Init(extra, (NGMenuBase) this);
    this.ScrollContainer.Add(gameObject);
  }
}
