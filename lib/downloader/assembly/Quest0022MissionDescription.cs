// Decompiled with JetBrains decompiler
// Type: Quest0022MissionDescription
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
public class Quest0022MissionDescription : MonoBehaviour
{
  public List<Quest0022MissionList> MissionList;
  public List<GameObject> ThumbnailList;
  public UIButton btnClose;
  public UIWidget scrollContainerWidget;
  public UIScrollView scrollView;
  public UIGrid grid;
  [SerializeField]
  private int scrollContainerHeightBase;
  [SerializeField]
  private int scrollContainerHeightMin;
  [Tooltip("for 9:16 screen")]
  [SerializeField]
  private int scrollContainerHeightMax9_16;
  [Tooltip("for 3:4 screen")]
  [SerializeField]
  private int scrollContainerHeightMax3_4;
  [SerializeField]
  private Animator completeAnimator;
  [SerializeField]
  public UIButton dragScrollViewButton;

  private void ClearMissionList()
  {
    ((Component) this.grid).transform.Clear();
    this.MissionList.Clear();
    this.ThumbnailList.Clear();
  }

  public virtual IEnumerator InitValue(
    QuestStageMenuBase menu,
    QuestStoryMission[] missions,
    PlayerMissionHistory[] hists)
  {
    this.ClearMissionList();
    this.UpdateScrollViewHeight(missions.Length);
    for (int i = 0; i < missions.Length; ++i)
    {
      bool clearFlag = ((IEnumerable<PlayerMissionHistory>) hists).Select<PlayerMissionHistory, int>((Func<PlayerMissionHistory, int>) (x => x.mission_id)).Contains<int>(missions[i].ID);
      Quest0022MissionList missionItem = menu.missionItem.Clone(((Component) this.grid).transform).GetComponent<Quest0022MissionList>();
      ((Component) missionItem).transform.localPosition = new Vector3(0.0f, (float) -i * this.grid.cellHeight, 0.0f);
      IEnumerator e = missionItem.SetValue(missions[i], clearFlag);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.MissionList.Add(missionItem);
      this.ThumbnailList.Add(((Component) missionItem.LinkParent).gameObject);
      missionItem = (Quest0022MissionList) null;
    }
    this.grid.Reposition();
    this.scrollView.ResetPosition();
  }

  public virtual IEnumerator InitValue(
    QuestStageMenuBase menu,
    QuestExtraMission[] missions,
    PlayerMissionHistory[] hists)
  {
    this.ClearMissionList();
    this.UpdateScrollViewHeight(missions.Length);
    for (int i = 0; i < missions.Length; ++i)
    {
      bool clearFlag = ((IEnumerable<PlayerMissionHistory>) hists).Select<PlayerMissionHistory, int>((Func<PlayerMissionHistory, int>) (x => x.mission_id)).Contains<int>(missions[i].ID);
      Quest0022MissionList missionItem = menu.missionItem.Clone(((Component) this.grid).transform).GetComponent<Quest0022MissionList>();
      ((Component) missionItem).transform.localPosition = new Vector3(0.0f, (float) -i * this.grid.cellHeight, 0.0f);
      IEnumerator e = missionItem.SetValue(missions[i], clearFlag);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.MissionList.Add(missionItem);
      this.ThumbnailList.Add(((Component) missionItem.LinkParent).gameObject);
      missionItem = (Quest0022MissionList) null;
    }
    this.grid.Reposition();
    this.scrollView.ResetPosition();
  }

  public virtual IEnumerator InitValue(
    QuestStageMenuBase menu,
    QuestSeaMission[] missions,
    PlayerMissionHistory[] hists)
  {
    this.ClearMissionList();
    this.UpdateScrollViewHeight(missions.Length);
    for (int i = 0; i < missions.Length; ++i)
    {
      bool clearFlag = ((IEnumerable<PlayerMissionHistory>) hists).Select<PlayerMissionHistory, int>((Func<PlayerMissionHistory, int>) (x => x.mission_id)).Contains<int>(missions[i].ID);
      Quest0022MissionList missionItem = menu.missionItem.Clone(((Component) this.grid).transform).GetComponent<Quest0022MissionList>();
      ((Component) missionItem).transform.localPosition = new Vector3(0.0f, (float) -i * this.grid.cellHeight, 0.0f);
      IEnumerator e = missionItem.SetValue(missions[i], clearFlag);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.MissionList.Add(missionItem);
      this.ThumbnailList.Add(((Component) missionItem.LinkParent).gameObject);
      missionItem = (Quest0022MissionList) null;
    }
    this.grid.Reposition();
    this.scrollView.ResetPosition();
  }

  public virtual IEnumerator LoadAnimation(int maxEffectCount)
  {
    Quest0022MissionDescription missionDescription = this;
    Future<GameObject> h = Res.Animations.mission_clear.Prefab.glitteringly.Load<GameObject>();
    IEnumerator e = h.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Transform[] array = h.Result.Clone(((Component) missionDescription).transform).transform.GetChildren().ToArray<Transform>();
    ((IEnumerable<Transform>) array).ForEach<Transform>((Action<Transform>) (x => ((Component) x).gameObject.SetActive(false)));
    for (int index1 = 0; index1 < missionDescription.MissionList.Count && index1 <= maxEffectCount; ++index1)
    {
      if (index1 < array.Length)
      {
        array[index1].parent = missionDescription.ThumbnailList[index1].transform;
        array[index1].localPosition = Vector3.zero;
        missionDescription.MissionList[index1].Animation = ((Component) array[index1]).gameObject;
      }
      else
      {
        int index2 = index1 % array.Length;
        GameObject gameObject = ((Component) array[index2]).gameObject.Clone(missionDescription.ThumbnailList[index1].transform);
        gameObject.transform.localPosition = Vector3.zero;
        missionDescription.MissionList[index1].Animation = gameObject;
      }
    }
  }

  public int UpdateScrollViewHeight(int itemCount)
  {
    float num1 = (float) this.scrollContainerHeightBase + (float) itemCount * this.grid.cellHeight;
    float num2 = (float) Screen.width / (float) Screen.height;
    float num3 = (float) this.scrollContainerHeightMax3_4;
    if ((double) Mathf.Abs(num2 - 0.75f) > (double) Mathf.Abs(num2 - 0.56f))
      num3 = (float) this.scrollContainerHeightMax9_16;
    if ((double) num1 > (double) num3)
      num1 = num3;
    else if ((double) num1 < (double) this.scrollContainerHeightMin)
      num1 = (float) this.scrollContainerHeightMin;
    this.scrollContainerWidget.height = (int) num1;
    return this.scrollContainerWidget.height;
  }

  public void PlayMissionCompleteTitleAnimation(string trigger = null)
  {
    if (trigger == null)
      ((Component) this.completeAnimator).gameObject.SetActive(true);
    else
      this.completeAnimator.SetTrigger(trigger);
  }
}
