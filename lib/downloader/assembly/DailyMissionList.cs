// Decompiled with JetBrains decompiler
// Type: DailyMissionList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MissionData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class DailyMissionList : MonoBehaviour
{
  [SerializeField]
  public UIGrid grid;
  [SerializeField]
  private GameObject dirMissionCleared;
  [SerializeField]
  public UIScrollView scrollView;
  [SerializeField]
  public UIScrollBar scrollBar;
  private DailyMissionController controller;
  private GameObject missionPrefab;
  private IMissionAchievement[] missions;
  private int listTypeIndex;
  private DailyMissionWindow targetWindow;
  private List<Coroutine> loadingCoroutineList = new List<Coroutine>();
  private const int defaultSetupMissionMax = 5;
  private List<IMissionAchievement> loadMissionList = new List<IMissionAchievement>();
  private List<GameObject> loadScrollList = new List<GameObject>();

  public bool IsCreated { get; private set; }

  private void OnDestroy() => this.Clear();

  public void Initialize(
    DailyMissionWindow targetWindow,
    int listType,
    GameObject prefab,
    IMissionAchievement[] missions)
  {
    this.targetWindow = targetWindow;
    this.listTypeIndex = listType;
    this.missions = missions;
    this.missionPrefab = prefab;
    this.SetVisible(false);
    this.IsCreated = false;
  }

  public void Clear()
  {
    foreach (Coroutine loadingCoroutine in this.loadingCoroutineList)
    {
      if (loadingCoroutine != null)
        this.StopCoroutine(loadingCoroutine);
    }
    this.loadingCoroutineList.Clear();
    foreach (Component component in ((Component) this.grid).transform)
      Object.Destroy((Object) component.gameObject);
    ((Component) this.grid).transform.Clear();
    this.loadMissionList.Clear();
    this.loadScrollList.Clear();
  }

  public void ResetPosition() => this.scrollView.ResetPosition();

  public bool BulkReceiveFlag()
  {
    if (this.missions != null)
    {
      if (this.listTypeIndex == 3)
      {
        if (((IEnumerable<IMissionAchievement>) this.missions).Any<IMissionAchievement>((Func<IMissionAchievement, bool>) (x => x.isCleared && !x.isReceived)))
          return true;
      }
      else if (((IEnumerable<IMissionAchievement>) this.missions).Any<IMissionAchievement>((Func<IMissionAchievement, bool>) (x => x.isCleared)))
        return true;
    }
    return false;
  }

  public void SetVisible(bool isVisible)
  {
    ((Component) this.scrollBar).gameObject.SetActive(isVisible);
    ((Component) this.scrollView).gameObject.SetActive(isVisible);
    if (this.missions == null)
      return;
    this.dirMissionCleared.SetActive(isVisible && ((IEnumerable<IMissionAchievement>) this.missions).Count<IMissionAchievement>() == 0);
  }

  public IEnumerator Create(DailyMissionController controller)
  {
    DailyMissionList dailyMissionList = this;
    dailyMissionList.controller = controller;
    dailyMissionList.Clear();
    int count = 0;
    IMissionAchievement[] missionAchievementArray = dailyMissionList.missions;
    for (int index = 0; index < missionAchievementArray.Length; ++index)
    {
      IMissionAchievement pdm = missionAchievementArray[index];
      GameObject gameObject = dailyMissionList.missionPrefab.Clone(((Component) dailyMissionList.grid).transform);
      if (count < 5 || !PerformanceConfig.GetInstance().IsTuningMissionInitialize)
      {
        IEnumerator e = gameObject.GetComponent<DailyMission0272Panel>().Init(controller, pdm, dailyMissionList.targetWindow, dailyMissionList.listTypeIndex);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
      {
        dailyMissionList.loadMissionList.Add(pdm);
        dailyMissionList.loadScrollList.Add(gameObject);
      }
      ++count;
    }
    missionAchievementArray = (IMissionAchievement[]) null;
    if (dailyMissionList.loadMissionList.Count > 0)
      dailyMissionList.loadingCoroutineList.Add(dailyMissionList.StartCoroutine(dailyMissionList.CreatePanels()));
    // ISSUE: method pointer
    dailyMissionList.grid.onReposition = new UIGrid.OnReposition((object) dailyMissionList, __methodptr(\u003CCreate\u003Eb__23_0));
    dailyMissionList.grid.Reposition();
    dailyMissionList.IsCreated = true;
  }

  private IEnumerator CreatePanels()
  {
    for (int i = 0; i < this.loadMissionList.Count; ++i)
    {
      IEnumerator e = this.loadScrollList[i].GetComponent<DailyMission0272Panel>().Init(this.controller, this.loadMissionList[i], this.targetWindow, this.listTypeIndex);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      yield return (object) null;
    }
  }
}
