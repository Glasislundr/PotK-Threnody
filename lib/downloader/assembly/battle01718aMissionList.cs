// Decompiled with JetBrains decompiler
// Type: battle01718aMissionList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class battle01718aMissionList : MonoBehaviour
{
  public List<battle01718aMission> MissionList;
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

  public IEnumerator InitValue(
    PlayerMissionHistory[] hists,
    QuestStoryMission[] missions,
    GameObject missionListBattleItemPrefab)
  {
    this.ClearMissionList();
    this.UpdateScrollViewHeight(missions.Length);
    for (int i = 0; i < missions.Length; ++i)
    {
      bool clearflag = ((IEnumerable<PlayerMissionHistory>) hists).Select<PlayerMissionHistory, int>((Func<PlayerMissionHistory, int>) (x => x.mission_id)).Contains<int>(missions[i].ID);
      battle01718aMission missionItem = missionListBattleItemPrefab.Clone(((Component) this.grid).transform).GetComponent<battle01718aMission>();
      ((Component) missionItem).transform.localPosition = new Vector3(0.0f, (float) -i * this.grid.cellHeight, 0.0f);
      IEnumerator e = missionItem.SetSprite(clearflag, missions[i]);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.MissionList.Add(missionItem);
      missionItem = (battle01718aMission) null;
    }
    this.grid.Reposition();
    this.scrollView.ResetPosition();
  }

  public IEnumerator InitValue(
    PlayerMissionHistory[] hists,
    QuestExtraMission[] missions,
    GameObject missionListBattleItemPrefab)
  {
    this.ClearMissionList();
    this.UpdateScrollViewHeight(missions.Length);
    for (int i = 0; i < missions.Length; ++i)
    {
      bool clearflag = ((IEnumerable<PlayerMissionHistory>) hists).Select<PlayerMissionHistory, int>((Func<PlayerMissionHistory, int>) (x => x.mission_id)).Contains<int>(missions[i].ID);
      battle01718aMission missionItem = missionListBattleItemPrefab.Clone(((Component) this.grid).transform).GetComponent<battle01718aMission>();
      ((Component) missionItem).transform.localPosition = new Vector3(0.0f, (float) -i * this.grid.cellHeight, 0.0f);
      IEnumerator e = missionItem.SetSprite(clearflag, missions[i]);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.MissionList.Add(missionItem);
      missionItem = (battle01718aMission) null;
    }
    this.grid.Reposition();
    this.scrollView.ResetPosition();
  }

  public IEnumerator InitValue(
    PlayerMissionHistory[] hists,
    QuestSeaMission[] missions,
    GameObject missionListBattleItemPrefab)
  {
    this.ClearMissionList();
    this.UpdateScrollViewHeight(missions.Length);
    for (int i = 0; i < missions.Length; ++i)
    {
      bool clearflag = ((IEnumerable<PlayerMissionHistory>) hists).Select<PlayerMissionHistory, int>((Func<PlayerMissionHistory, int>) (x => x.mission_id)).Contains<int>(missions[i].ID);
      battle01718aMission missionItem = missionListBattleItemPrefab.Clone(((Component) this.grid).transform).GetComponent<battle01718aMission>();
      ((Component) missionItem).transform.localPosition = new Vector3(0.0f, (float) -i * this.grid.cellHeight, 0.0f);
      IEnumerator e = missionItem.SetSprite(clearflag, missions[i]);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.MissionList.Add(missionItem);
      missionItem = (battle01718aMission) null;
    }
    this.grid.Reposition();
    this.scrollView.ResetPosition();
  }

  public IEnumerator InitValue(
    int[] clear_ids,
    CorpsMissionReward[] missions,
    GameObject missionListBattleItemPrefab)
  {
    this.ClearMissionList();
    this.UpdateScrollViewHeight(missions.Length);
    for (int i = 0; i < missions.Length; ++i)
    {
      bool clearflag = ((IEnumerable<int>) clear_ids).Contains<int>(missions[i].ID);
      battle01718aMission missionItem = missionListBattleItemPrefab.Clone(((Component) this.grid).transform).GetComponent<battle01718aMission>();
      ((Component) missionItem).transform.localPosition = new Vector3(0.0f, (float) -i * this.grid.cellHeight, 0.0f);
      IEnumerator e = missionItem.SetSprite(clearflag, missions[i]);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.MissionList.Add(missionItem);
      missionItem = (battle01718aMission) null;
    }
    this.grid.Reposition();
    this.scrollView.ResetPosition();
  }

  private void ClearMissionList()
  {
    ((Component) this.grid).transform.Clear();
    this.MissionList.Clear();
  }

  public void UpdateScrollViewHeight(int itemCount)
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
  }

  public void ibtnBack() => Singleton<PopupManager>.GetInstance().onDismiss();
}
