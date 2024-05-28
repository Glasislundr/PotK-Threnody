// Decompiled with JetBrains decompiler
// Type: ReisouSkillDetail_01
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnitStatusInformation;
using UnityEngine;

#nullable disable
public class ReisouSkillDetail_01 : MonoBehaviour
{
  private bool? isSea_;
  [SerializeField]
  protected UIDragScrollView uiDragScrollView;
  [SerializeField]
  protected UIDragScrollView uiDragScrollViewDetail;
  [SerializeField]
  protected UI2DSprite iconSkill;
  [SerializeField]
  protected UILabel txtSkillName;
  [SerializeField]
  protected UILabel txtSkillLv;
  [SerializeField]
  protected UILabel txtSkillDescription;
  [SerializeField]
  protected UILabel txtReleaseRank;
  [SerializeField]
  protected GameObject slcBlackSheet;
  [SerializeField]
  protected UILabel txtNothingTargetWeapon;
  [SerializeField]
  protected UILabel txtExcludedWeapon;
  [SerializeField]
  protected UIButton btnWeaponList;
  private PopupSkillDetails.Param[] skillParams_;
  private GearReisouSkill skill_;
  private GameObject skillDialogPrefab;
  private Action<GearReisouSkill> awakeWeaponListCallback;
  private BackButtonMenuBase basePopup;

  private bool isSea
  {
    get
    {
      return this.isSea_ ?? (this.isSea_ = new bool?(Singleton<NGGameDataManager>.GetInstance().IsSea)).Value;
    }
  }

  public IEnumerator Init(
    UIScrollView scrollView,
    GearReisouSkill skill,
    int gearLevel,
    PopupSkillDetails.Param[] skillParams,
    bool is_release,
    PlayerItem equipGear,
    Action<GearReisouSkill> awakeWeaponListCallback,
    BackButtonMenuBase basePopup)
  {
    this.uiDragScrollView.scrollView = scrollView;
    this.uiDragScrollViewDetail.scrollView = scrollView;
    this.skill_ = skill;
    this.skillParams_ = skillParams;
    this.awakeWeaponListCallback = awakeWeaponListCallback;
    this.basePopup = basePopup;
    yield return (object) this.doLoadSkill(skill.skill);
    this.txtSkillName.SetTextLocalize(skill.skill.name);
    this.txtSkillLv.SetTextLocalize(Consts.GetInstance().UNIT_0044_REISOU_SKILL_LV.F((object) skill.skill_level, (object) skill.skill.upper_level));
    this.txtSkillDescription.SetTextLocalize(skill.skill.description);
    if (is_release)
      ((Component) this.txtReleaseRank).gameObject.SetActive(false);
    else if (gearLevel < skill.release_rank)
    {
      this.txtReleaseRank.SetTextLocalize(Consts.GetInstance().UNIT_0044_REISOU_RELEASE_RANK.F((object) skill.release_rank));
      ((Component) this.txtExcludedWeapon).gameObject.SetActive(false);
    }
    else if (equipGear == (PlayerItem) null)
    {
      ((Component) this.txtReleaseRank).gameObject.SetActive(false);
    }
    else
    {
      ((Component) this.txtReleaseRank).gameObject.SetActive(false);
      ((Component) this.txtExcludedWeapon).gameObject.SetActive(true);
    }
    if (skill.awake_weapon_group != 0)
    {
      ((Component) this.txtNothingTargetWeapon).gameObject.SetActive(false);
      ((Component) this.btnWeaponList).gameObject.SetActive(true);
    }
    else
    {
      ((Component) this.txtNothingTargetWeapon).gameObject.SetActive(true);
      ((Component) this.btnWeaponList).gameObject.SetActive(false);
    }
  }

  public IEnumerator Init(GearReisouSkill skill, BackButtonMenuBase basePopup)
  {
    this.skill_ = skill;
    this.basePopup = basePopup;
    this.skillParams_ = new List<PopupSkillDetails.Param>()
    {
      new PopupSkillDetails.Param(skill.skill, UnitParameter.SkillGroup.Reisou, new int?(skill.skill_level))
    }.ToArray();
    this.awakeWeaponListCallback = (Action<GearReisouSkill>) null;
    yield return (object) this.doLoadSkill(skill.skill);
    this.txtSkillName.SetTextLocalize(skill.skill.name);
    this.txtSkillLv.SetTextLocalize(Consts.GetInstance().UNIT_0044_REISOU_SKILL_LV.F((object) skill.skill_level, (object) skill.skill.upper_level));
    this.txtSkillDescription.SetTextLocalize(skill.skill.description);
    if (skill.release_rank > 1)
      this.txtReleaseRank.SetTextLocalize(Consts.GetInstance().UNIT_0044_REISOU_RELEASE_RANK.F((object) skill.release_rank));
    else
      ((Component) this.txtReleaseRank).gameObject.SetActive(false);
    this.slcBlackSheet.SetActive(false);
    ((Component) this.txtExcludedWeapon).gameObject.SetActive(false);
    ((Component) this.txtNothingTargetWeapon).gameObject.SetActive(false);
    ((Component) this.btnWeaponList).gameObject.SetActive(false);
  }

  private IEnumerator doLoadSkill(BattleskillSkill s)
  {
    Future<Sprite> ld = s.LoadBattleSkillIcon();
    yield return (object) ld.Wait();
    this.iconSkill.sprite2D = ld.Result;
  }

  public void onBtnDetail()
  {
    if (this.basePopup.IsPushAndSet())
      return;
    this.StartCoroutine(this.openSkillDaialog());
  }

  private IEnumerator openSkillDaialog()
  {
    ReisouSkillDetail_01 reisouSkillDetail01 = this;
    if (Object.op_Equality((Object) reisouSkillDetail01.skillDialogPrefab, (Object) null))
    {
      Future<GameObject> loader = PopupSkillDetails.createPrefabLoader(reisouSkillDetail01.isSea);
      yield return (object) loader.Wait();
      reisouSkillDetail01.skillDialogPrefab = loader.Result;
      loader = (Future<GameObject>) null;
    }
    // ISSUE: reference to a compiler-generated method
    PopupSkillDetails.show(reisouSkillDetail01.skillDialogPrefab, reisouSkillDetail01.skillParams_, ((IEnumerable<PopupSkillDetails.Param>) reisouSkillDetail01.skillParams_).FirstIndexOrNull<PopupSkillDetails.Param>(new Func<PopupSkillDetails.Param, bool>(reisouSkillDetail01.\u003CopenSkillDaialog\u003Eb__23_0)).Value);
    yield return (object) new WaitForSeconds(0.1f);
    reisouSkillDetail01.basePopup.IsPush = false;
  }

  public void onWeaponList()
  {
    Action<GearReisouSkill> weaponListCallback = this.awakeWeaponListCallback;
    if (weaponListCallback == null)
      return;
    weaponListCallback(this.skill_);
  }
}
