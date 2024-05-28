// Decompiled with JetBrains decompiler
// Type: Unit004JobAnimJobUp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Unit004JobAnimJobUp : BackButtonMenuBase
{
  [SerializeField]
  private Transform dirJobAfter;
  [SerializeField]
  private Transform dirJobAfter2;
  [SerializeField]
  private GameObject dynCharacter;
  [SerializeField]
  private GameObject Vertex_Lv5_logo;
  [SerializeField]
  private GameObject txt_top;
  [SerializeField]
  private GameObject txt_top2;
  private PlayerUnit _playerUnit;
  private PlayerUnitJob_abilities _jobAbility;
  private float clickTimer;
  private bool isOpening;
  private Action onUpdatedJobAbility_;
  private bool isJobAbilityMax;
  private bool isOpeningMasterBonus;

  public IEnumerator Init(
    PlayerUnit unit,
    PlayerUnitJob_abilities jobAbility,
    PlayerUnitJob_abilities beforeAbility,
    Action eventUpdatedJobAbility)
  {
    this._playerUnit = unit;
    this._jobAbility = jobAbility;
    this.onUpdatedJobAbility_ = eventUpdatedJobAbility;
    this.clickTimer = Time.time + 0.5f;
    this.isJobAbilityMax = jobAbility.level == jobAbility.master.levelup_patterns.Length;
    this.dynCharacter.SetActive(false);
    Future<GameObject> JobAfterPanelF = Singleton<NGGameDataManager>.GetInstance().IsSea ? Res.Prefabs.unit004_Job.Unit_job_after_sea.Load<GameObject>() : Res.Prefabs.unit004_Job.Unit_job_after.Load<GameObject>();
    IEnumerator e = JobAfterPanelF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = JobAfterPanelF.Result.Clone(this.dirJobAfter).GetComponent<Unit004JobAfter>().Init(1, jobAbility, beforeAbility);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  protected override void Update()
  {
    if ((double) this.clickTimer >= (double) Time.time || !Input.GetMouseButtonDown(0))
      return;
    if (this.isJobAbilityMax && !this.isOpeningMasterBonus)
    {
      this.DisplayMasterBonus();
      this.isOpeningMasterBonus = true;
      this.clickTimer = Time.time + 0.5f;
    }
    else
    {
      if (this.isOpening)
        return;
      this.OpenDialogAndQuitAnim();
      this.isOpening = true;
    }
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.OpenDialogAndQuitAnim();
  }

  private void OpenDialogAndQuitAnim()
  {
    Singleton<PopupManager>.GetInstance().monitorCoroutine(this.OpenJobDialogPanel());
  }

  private IEnumerator OpenJobDialogPanel()
  {
    Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();
    while (Singleton<PopupManager>.GetInstance().isOpenNoFinish)
      yield return (object) null;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    yield return (object) new WaitForSeconds(0.5f);
    GameObject popup;
    Future<GameObject> prefab;
    IEnumerator e;
    if (this._jobAbility.current_levelup_pattern != null)
    {
      prefab = (Future<GameObject>) null;
      if (!Singleton<NGGameDataManager>.GetInstance().IsSea)
      {
        JobCharacteristicsLevelupPattern currentLevelupPattern = this._jobAbility.current_levelup_pattern;
        prefab = new ResourceObject("Prefabs/unit004_Job/" + (currentLevelupPattern == null || !currentLevelupPattern.proficiency.HasValue ? "Unit_JobCharacteristic_UP_Dialog" : "Unit_X_JobCharacteristic_UP_Dialog")).Load<GameObject>();
      }
      else
        prefab = Res.Prefabs.unit004_Job.Unit_JobCharacteristic_UP_Dialog_sea.Load<GameObject>();
      e = prefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      popup = Singleton<PopupManager>.GetInstance().open(prefab.Result, isNonSe: true, isNonOpenAnime: true);
      e = popup.GetComponent<Unit004JobDialogUp>().Init(this._playerUnit, this._jobAbility, this.onUpdatedJobAbility_);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<PopupManager>.GetInstance().startOpenAnime(popup);
      prefab = (Future<GameObject>) null;
    }
    else
    {
      prefab = Res.Prefabs.unit004_Job.Unit_JobCharacteristic_Dialog.Load<GameObject>();
      e = prefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      popup = Singleton<PopupManager>.GetInstance().open(prefab.Result);
      popup.SetActive(false);
      e = popup.GetComponent<Unit004JobDialog>().Init(this._jobAbility);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      popup.SetActive(true);
      prefab = (Future<GameObject>) null;
    }
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
  }

  private void DisplayMasterBonus()
  {
    Singleton<PopupManager>.GetInstance().monitorCoroutine(this.SwitchMasterBonusPanel());
  }

  private IEnumerator SwitchMasterBonusPanel()
  {
    ((Component) this.dirJobAfter).gameObject.SetActive(false);
    if (!Singleton<NGGameDataManager>.GetInstance().IsSea)
      this.Vertex_Lv5_logo.SetActive(true);
    this.txt_top2.SetActive(true);
    this.txt_top.SetActive(false);
    Future<Sprite> CharacterF = MasterData.UnitUnit[this._playerUnit._unit].LoadSpriteLarge(this._playerUnit.job_id, 1f);
    IEnumerator e = CharacterF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.dynCharacter.GetComponent<UI2DSprite>().sprite2D = CharacterF.Result;
    this.dynCharacter.SetActive(true);
    Future<GameObject> JobAfterPanelF = !Singleton<NGGameDataManager>.GetInstance().IsSea ? new ResourceObject("Prefabs/unit004_Job/Unit_job_after_2").Load<GameObject>() : new ResourceObject("Prefabs/unit004_2_sea/Unit_job_after_2_sea").Load<GameObject>();
    e = JobAfterPanelF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = JobAfterPanelF.Result.Clone(this.dirJobAfter2).GetComponent<Unit004JobAfter2>().Init(this._jobAbility);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((Component) this.dirJobAfter2).gameObject.SetActive(true);
  }
}
