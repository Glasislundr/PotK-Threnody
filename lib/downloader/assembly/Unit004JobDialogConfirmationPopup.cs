// Decompiled with JetBrains decompiler
// Type: Unit004JobDialogConfirmationPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/unit004_Job/Unused/Unit004JobDialogConfirmationPopup")]
public class Unit004JobDialogConfirmationPopup : BackButtonMenuBase
{
  [SerializeField]
  private Transform dirJobAfter;
  private PlayerUnit _unit;
  private PlayerUnitJob_abilities _jobAbility;
  private Action onUpdatedJobAbility_;
  private int _targetLevel;

  public IEnumerator Init(
    PlayerUnit unit,
    PlayerUnitJob_abilities jobAbility,
    Action eventUpdatedJobAbility,
    bool isClassChangeScene,
    int targetLevel)
  {
    this._unit = unit;
    this._jobAbility = jobAbility;
    this.onUpdatedJobAbility_ = eventUpdatedJobAbility;
    this._targetLevel = targetLevel;
    Future<GameObject> JobAfterPanelF = (Future<GameObject>) null;
    JobAfterPanelF = !Singleton<NGGameDataManager>.GetInstance().IsSea || isClassChangeScene ? Res.Prefabs.unit004_Job.Unit_job_after.Load<GameObject>() : Res.Prefabs.unit004_Job.Unit_job_after_sea.Load<GameObject>();
    IEnumerator e = JobAfterPanelF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = JobAfterPanelF.Result.Clone(this.dirJobAfter).GetComponent<Unit004JobAfter>().Init(1, jobAbility, bActiveSkillZoom: true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void IbtnPopupDecide()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().monitorCoroutine(this.openAnimationLevelUp());
  }

  private IEnumerator openAnimationLevelUp()
  {
    Unit004JobDialogConfirmationPopup confirmationPopup = this;
    Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();
    while (Singleton<PopupManager>.GetInstance().isOpenNoFinish)
      yield return (object) null;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    int level = confirmationPopup._jobAbility.level;
    Future<WebAPI.Response.UnitSaveJobAbility> paramF = WebAPI.UnitSaveJobAbility(confirmationPopup._unit.id, confirmationPopup._jobAbility.job_ability_id, confirmationPopup._targetLevel, (Action<WebAPI.Response.UserError>) (error =>
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      WebAPI.DefaultUserErrorCallback(error);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e = paramF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    WebAPI.Response.UnitSaveJobAbility result = paramF.Result;
    if (result != null)
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      // ISSUE: reference to a compiler-generated method
      PlayerUnit unit = Array.Find<PlayerUnit>(result.player_units, new Predicate<PlayerUnit>(confirmationPopup.\u003CopenAnimationLevelUp\u003Eb__7_1));
      if ((object) unit == null)
        unit = confirmationPopup._unit;
      PlayerUnit newUnit = unit;
      // ISSUE: reference to a compiler-generated method
      PlayerUnitJob_abilities newJobAbility = Array.Find<PlayerUnitJob_abilities>(newUnit.job_abilities, new Predicate<PlayerUnitJob_abilities>(confirmationPopup.\u003CopenAnimationLevelUp\u003Eb__7_2));
      Future<GameObject> prefab = (Future<GameObject>) null;
      prefab = Singleton<NGGameDataManager>.GetInstance().IsSea ? Res.Animations.Unit_Level_Job.Unit_JobUP_sea.Load<GameObject>() : Res.Animations.Unit_Level_Job.Unit_JobUP.Load<GameObject>();
      e = prefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject popup = Singleton<PopupManager>.GetInstance().open(prefab.Result);
      popup.SetActive(false);
      e = popup.GetComponent<Unit004JobAnimJobUp>().Init(newUnit, newJobAbility, confirmationPopup._jobAbility, confirmationPopup.onUpdatedJobAbility_);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      popup.SetActive(true);
      Action updatedJobAbility = confirmationPopup.onUpdatedJobAbility_;
      if (updatedJobAbility != null)
        updatedJobAbility();
    }
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();
}
