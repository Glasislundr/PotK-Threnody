// Decompiled with JetBrains decompiler
// Type: Guild0281FacilityInfoController
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
public class Guild0281FacilityInfoController : MonoBehaviour
{
  [SerializeField]
  private UILabel scaffoldRankLabel;
  [SerializeField]
  private UILabel scaffoldStatus1ValueLabel;
  [SerializeField]
  private UILabel scaffoldStatus2ValueLabel;
  [SerializeField]
  private UILabel wallsRankLabel;
  [SerializeField]
  private UILabel wallsStatus1ValueLabel;
  [SerializeField]
  private UILabel towerRankLabel;
  [SerializeField]
  private UILabel towerStatus1ValueLabel;
  [SerializeField]
  private UILabel towerStatus2ValueLabel;
  [SerializeField]
  private GameObject scaffoldInvestmentIcon;
  [SerializeField]
  private GameObject wallsInvestmentIcon;
  [SerializeField]
  private GameObject towerInvestmentIcon;
  [SerializeField]
  private UIButton scaffoldInvestmentButton;
  [SerializeField]
  private UIButton wallsInvestmentButton;
  [SerializeField]
  private UIButton towerInvestmentButton;
  [SerializeField]
  private Transform scaffoldLevelUpEffectContainer;
  [SerializeField]
  private Transform wallsLevelUpEffectContainer;
  [SerializeField]
  private Transform towerLevelUpEffectContainer;
  private GameObject buildingInvestmentConfirmDialogPrefab;
  private MyPageGuildMenu parentMenu;

  public IEnumerator InitializeAsync(MyPageGuildMenu parent)
  {
    this.parentMenu = parent;
    IEnumerator e = this.LoadFacilityInvestDialogPrefab();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator LoadFacilityInvestDialogPrefab()
  {
    Future<GameObject> prefab = Res.Prefabs.popup.popup_028_guild_HQ_BuildingInvestmentConfirm__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.buildingInvestmentConfirmDialogPrefab = prefab.Result;
  }

  public void RefreshGuildFacilityStatus()
  {
    GuildLevelBonus levelBonus = PlayerAffiliation.Current.guild.level_bonus;
    GuildMembership guildMembership = ((IEnumerable<GuildMembership>) PlayerAffiliation.Current.guild.memberships).FirstOrDefault<GuildMembership>((Func<GuildMembership, bool>) (x => x.player.player_id == Player.Current.id));
    if (guildMembership != null && (guildMembership.role == GuildRole.master || guildMembership.role == GuildRole.sub_master))
    {
      this.scaffoldInvestmentIcon.SetActive(true);
      this.wallsInvestmentIcon.SetActive(true);
      this.towerInvestmentIcon.SetActive(true);
      ((UIButtonColor) this.scaffoldInvestmentButton).isEnabled = true;
      ((UIButtonColor) this.wallsInvestmentButton).isEnabled = true;
      ((UIButtonColor) this.towerInvestmentButton).isEnabled = true;
    }
    else
    {
      this.scaffoldInvestmentIcon.SetActive(false);
      this.wallsInvestmentIcon.SetActive(false);
      this.towerInvestmentIcon.SetActive(false);
      ((UIButtonColor) this.scaffoldInvestmentButton).isEnabled = false;
      ((UIButtonColor) this.wallsInvestmentButton).isEnabled = false;
      ((UIButtonColor) this.towerInvestmentButton).isEnabled = false;
    }
    foreach (GuildHq hq in PlayerAffiliation.Current.guild.hqs)
    {
      switch (hq.base_type)
      {
        case GuildBaseType.walls:
          this.wallsRankLabel.SetTextLocalize(hq.rank);
          foreach (GuildBaseBonus bonuse in hq.bonuses)
          {
            if (bonuse.bonus_type == GuildBaseBonusType.hit_point)
              this.wallsStatus1ValueLabel.SetTextLocalize(this.GetFormattedStatusValue(bonuse.bonus_amount));
          }
          break;
        case GuildBaseType.tower:
          this.towerRankLabel.SetTextLocalize(hq.rank);
          foreach (GuildBaseBonus bonuse in hq.bonuses)
          {
            if (bonuse.bonus_type == GuildBaseBonusType.physical_attack)
              this.towerStatus1ValueLabel.SetTextLocalize(this.GetFormattedStatusValue(bonuse.bonus_amount));
            else if (bonuse.bonus_type == GuildBaseBonusType.magic_attack)
              this.towerStatus2ValueLabel.SetTextLocalize(this.GetFormattedStatusValue(bonuse.bonus_amount));
          }
          break;
        case GuildBaseType.scaffold:
          this.scaffoldRankLabel.SetTextLocalize(hq.rank);
          foreach (GuildBaseBonus bonuse in hq.bonuses)
          {
            if (bonuse.bonus_type == GuildBaseBonusType.accuracy_rate)
              this.scaffoldStatus1ValueLabel.SetTextLocalize(this.GetFormattedStatusValue(bonuse.bonus_amount));
            else if (bonuse.bonus_type == GuildBaseBonusType.avoidance)
              this.scaffoldStatus2ValueLabel.SetTextLocalize(this.GetFormattedStatusValue(bonuse.bonus_amount));
          }
          break;
        default:
          Debug.LogError((object) "The type of facility does not exist!");
          return;
      }
    }
  }

  public void OnScaffoldInvestmentButtonClicked()
  {
    this.OpenFacilityInvestmentDialog(GuildBaseType.scaffold);
  }

  public void OnWallsInvestmentButtonClicked()
  {
    this.OpenFacilityInvestmentDialog(GuildBaseType.walls);
  }

  public void OnTowerInvestmentButtonClicked()
  {
    this.OpenFacilityInvestmentDialog(GuildBaseType.tower);
  }

  public void OpenFacilityInvestmentDialog(GuildBaseType investmentFacilityType)
  {
    if (((Component) Singleton<NGSceneManager>.GetInstance().sceneBase).GetComponent<NGMenuBase>().IsPushAndSet())
      return;
    GuildHq selectedFacility = (GuildHq) null;
    foreach (GuildHq hq in PlayerAffiliation.Current.guild.hqs)
    {
      if (hq.base_type == investmentFacilityType)
      {
        selectedFacility = hq;
        break;
      }
    }
    if (selectedFacility.rank < selectedFacility.max_rank && PlayerAffiliation.Current.guild.appearance.level >= selectedFacility.guild_level_cap)
      this.StartCoroutine(this.OpenFacilityInvestmentDialog(selectedFacility));
    else
      ModalWindow.Show(Consts.GetInstance().Guild0281MENU_FACILITY_RANK_MAX_DIALOG_TITLE, Consts.GetInstance().Guild0281MENU_FACILITY_RANK_MAX_DIALOG_CONTENT, (Action) (() =>
      {
        if (!Object.op_Inequality((Object) Singleton<NGSceneManager>.GetInstance().sceneBase, (Object) null) || !Object.op_Inequality((Object) ((Component) Singleton<NGSceneManager>.GetInstance().sceneBase).GetComponent<NGMenuBase>(), (Object) null))
          return;
        ((Component) Singleton<NGSceneManager>.GetInstance().sceneBase).GetComponent<NGMenuBase>().IsPush = false;
      }));
  }

  private IEnumerator OpenFacilityInvestmentDialog(GuildHq selectedFacility)
  {
    Guild0281FacilityInfoController facilityInfoController = this;
    if (Object.op_Equality((Object) facilityInfoController.buildingInvestmentConfirmDialogPrefab, (Object) null))
    {
      IEnumerator e = facilityInfoController.LoadFacilityInvestDialogPrefab();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    GameObject prefab = facilityInfoController.buildingInvestmentConfirmDialogPrefab.Clone();
    prefab.GetComponent<Guild0281BuildingInvestmentConfirmDialogController>().Initialize(selectedFacility, facilityInfoController);
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
  }

  public IEnumerator OnInvestmentFinished(GuildHq selectedFacility)
  {
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    IEnumerator e = this.parentMenu.OnFacilityLvup(selectedFacility.base_type);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.ShowFacilityLevelUpEffect(selectedFacility.base_type);
    this.RefreshGuildFacilityStatus();
  }

  private void ShowFacilityLevelUpEffect(GuildBaseType facilityType)
  {
    Transform upEffectContainer;
    switch (facilityType)
    {
      case GuildBaseType.walls:
        upEffectContainer = this.wallsLevelUpEffectContainer;
        break;
      case GuildBaseType.tower:
        upEffectContainer = this.towerLevelUpEffectContainer;
        break;
      case GuildBaseType.scaffold:
        upEffectContainer = this.scaffoldLevelUpEffectContainer;
        break;
      default:
        Debug.LogError((object) "The type of facility does not exist!");
        Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
        if (!Object.op_Inequality((Object) Singleton<NGSceneManager>.GetInstance().sceneBase, (Object) null))
          return;
        Singleton<NGSceneManager>.GetInstance().sceneBase.IsPush = false;
        return;
    }
    this.StartCoroutine(this.StartLvupEffect(upEffectContainer));
  }

  private IEnumerator StartLvupEffect(Transform target)
  {
    ((Component) target).gameObject.SetActive(true);
    Animator animator = ((Component) target).GetComponentInChildren<Animator>();
    AnimatorStateInfo animatorStateInfo;
    do
    {
      yield return (object) null;
      animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
    }
    while ((double) ((AnimatorStateInfo) ref animatorStateInfo).normalizedTime < 1.0);
    ((Component) target).gameObject.SetActive(false);
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
    if (Object.op_Inequality((Object) Singleton<NGSceneManager>.GetInstance().sceneBase, (Object) null))
      Singleton<NGSceneManager>.GetInstance().sceneBase.IsPush = false;
  }

  private string GetFormattedStatusValue(int value, bool isValueOnly = false)
  {
    return !isValueOnly ? string.Format("{0}%UP", (object) value) : value.ToString();
  }

  public void InvestFacility(GuildHq selectedFacility)
  {
    this.StartCoroutine(this.InvestFacilityCoroutine(selectedFacility));
  }

  private IEnumerator InvestFacilityCoroutine(GuildHq selectedFacility)
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Future<WebAPI.Response.GuildBaseInvest> future = WebAPI.GuildBaseInvest((int) selectedFacility.base_type, selectedFacility.rank, (Action<WebAPI.Response.UserError>) (e =>
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      ModalWindow.Show(Consts.GetInstance().Guild0281MENU_FACILITY_INVESTMENT_ERROR_POPUP_TITLE, Consts.GetInstance().Guild0281MENU_FACILITY_INVESTMENT_ERROR_POPUP_CONTENT, (Action) (() =>
      {
        Singleton<NGGameDataManager>.GetInstance().refreshHomeHome = true;
        Singleton<NGGameDataManager>.GetInstance().isCallHomeUpdateAllData = true;
        MypageScene.ChangeScene(MypageRootMenu.Mode.GUILD);
      }));
      Singleton<PopupManager>.GetInstance().dismiss();
    }));
    IEnumerator e1 = future.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (future.Result != null)
    {
      Singleton<PopupManager>.GetInstance().dismiss(true);
      e1 = this.OnInvestmentFinished(selectedFacility);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
    }
  }
}
