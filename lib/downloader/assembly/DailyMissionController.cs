// Decompiled with JetBrains decompiler
// Type: DailyMissionController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/DailyMission/DailyMissionController")]
public class DailyMissionController : MonoBehaviour
{
  private GameObject windowPrefab;
  private DailyMissionWindow currentMissionWindow;

  public GameObject panelPrefab { get; set; }

  public GameObject missionPointRewardDetailPopupPrefab { get; set; }

  public GameObject missionPointRewardDetailItemPrefab { get; set; }

  public GameObject weeklyMissionPointRewardPopupPrefab { get; set; }

  public GameObject getPointRewardEffectPopupPrefab { get; set; }

  public GameObject challengeAgainConfirmationPopupPrefab { get; set; }

  public GameObject dailyMissionCollectiveListPrefab { get; set; }

  private void Start() => this.StartCoroutine(this.LoadResource());

  private IEnumerator LoadResource()
  {
    Future<GameObject> window = Res.Prefabs.dailymission027_2.Daily_Mission.Load<GameObject>();
    IEnumerator e = window.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.windowPrefab = window.Result;
    Future<GameObject> panel = Res.Prefabs.dailymission027_2.dir_Daily_Mission.Load<GameObject>();
    e = panel.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.panelPrefab = panel.Result;
    Future<GameObject> missionPointRewardPopupF = Res.Prefabs.dailymission027_2.MissionPointRewardPopup.Load<GameObject>();
    e = missionPointRewardPopupF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.missionPointRewardDetailPopupPrefab = missionPointRewardPopupF.Result;
    ((UIRect) this.missionPointRewardDetailPopupPrefab.GetComponent<UIWidget>()).alpha = 0.0f;
    Future<GameObject> pointRewardDetailItemF = Res.Prefabs.dailymission027_2.MissionPointRewardDetailItem.Load<GameObject>();
    e = pointRewardDetailItemF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.missionPointRewardDetailItemPrefab = pointRewardDetailItemF.Result;
    Future<GameObject> weeklyF = Res.Prefabs.dailymission027_2.WeeklyMissionPointRewardPopup.Load<GameObject>();
    e = weeklyF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.weeklyMissionPointRewardPopupPrefab = weeklyF.Result;
    Future<GameObject> getPointRewardEffectF = Res.Prefabs.dailymission027_2.GetPointRewardEffectPopup.Load<GameObject>();
    e = getPointRewardEffectF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.getPointRewardEffectPopupPrefab = getPointRewardEffectF.Result;
    Future<GameObject> challengeAgainConfirmationPopupF = Res.Prefabs.dailymission027_2.ChallengeAgainConfirmationPopup.Load<GameObject>();
    e = challengeAgainConfirmationPopupF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.challengeAgainConfirmationPopupPrefab = challengeAgainConfirmationPopupF.Result;
    Future<GameObject> collectiveListF = Res.Prefabs.dailymission027_2.DailyMissionCollectiveList.Load<GameObject>();
    e = collectiveListF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.dailyMissionCollectiveListPrefab = collectiveListF.Result;
  }

  public void Show()
  {
    if (Object.op_Inequality((Object) this.currentMissionWindow, (Object) null))
      return;
    this.StartCoroutine(this.ShowWindow());
  }

  public void Hide()
  {
    if (Object.op_Equality((Object) this.currentMissionWindow, (Object) null))
      return;
    Object.Destroy((Object) ((Component) this.currentMissionWindow).gameObject);
    this.currentMissionWindow = (DailyMissionWindow) null;
  }

  public bool IsOpened => Object.op_Inequality((Object) this.currentMissionWindow, (Object) null);

  private IEnumerator ShowWindow()
  {
    DailyMissionController controller = this;
    Singleton<PopupManager>.GetInstance().closeAll();
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
    while (Object.op_Equality((Object) controller.windowPrefab, (Object) null) || Object.op_Equality((Object) controller.panelPrefab, (Object) null))
      yield return (object) null;
    GameObject gameObject = controller.windowPrefab.Clone(((Component) controller).transform);
    controller.currentMissionWindow = gameObject.GetComponent<DailyMissionWindow>();
    IEnumerator e = controller.currentMissionWindow.Init(controller, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    while (Object.op_Equality((Object) controller.missionPointRewardDetailPopupPrefab, (Object) null) || Object.op_Equality((Object) controller.missionPointRewardDetailItemPrefab, (Object) null) || Object.op_Equality((Object) controller.weeklyMissionPointRewardPopupPrefab, (Object) null) || Object.op_Equality((Object) controller.getPointRewardEffectPopupPrefab, (Object) null) || Object.op_Equality((Object) controller.challengeAgainConfirmationPopupPrefab, (Object) null))
      yield return (object) null;
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }
}
