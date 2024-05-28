// Decompiled with JetBrains decompiler
// Type: PopupRecommendPartOverkillers
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
[AddComponentMenu("Popup/Recommend/Overkillers")]
public class PopupRecommendPartOverkillers : PopupRecommendPart
{
  [SerializeField]
  private UILabel txtName_;
  [SerializeField]
  private Transform iconUnit_;
  [SerializeField]
  private UIWidget iconSkill_;
  [SerializeField]
  private GameObject notPossessed_;
  private OverkillersSkillRelease overkillersSkill_;

  public override IEnumerator doInitialize(PlayerUnit playerUnit, UnitUnit target)
  {
    PopupRecommendPartOverkillers recommendPartOverkillers = this;
    UnitRecommend unitRecommend;
    if (!MasterData.UnitRecommend.TryGetValue(playerUnit.unit.same_character_id, out unitRecommend) || (recommendPartOverkillers.target_ = unitRecommend.overkillers) == null)
    {
      ((Component) recommendPartOverkillers).gameObject.SetActive(false);
    }
    else
    {
      ((Component) recommendPartOverkillers).gameObject.SetActive(true);
      if (string.IsNullOrEmpty(recommendPartOverkillers.target_.formal_name))
        recommendPartOverkillers.txtName_.SetTextLocalize(recommendPartOverkillers.target_.name);
      else
        recommendPartOverkillers.txtName_.SetTextLocalize(recommendPartOverkillers.target_.formal_name);
      Future<GameObject> ldUnit = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      yield return (object) ldUnit.Wait();
      UnitIcon icon1 = ldUnit.Result.Clone(recommendPartOverkillers.iconUnit_).GetComponent<UnitIcon>();
      yield return (object) icon1.SetUnit(recommendPartOverkillers.target_, recommendPartOverkillers.target_.GetElement(), false);
      icon1.BottomModeValue = UnitIconBase.BottomMode.Level;
      icon1.ShowBottomInfos(UnitSortAndFilter.SORT_TYPES.Level);
      icon1.setLevelText("--");
      ((Behaviour) icon1.Button).enabled = false;
      ((Collider) icon1.buttonBoxCollider).enabled = false;
      icon1 = (UnitIcon) null;
      // ISSUE: reference to a compiler-generated method
      recommendPartOverkillers.notPossessed_.SetActive(!recommendPartOverkillers.menu.isDisabledAccountStatus && Array.Find<PlayerUnit>(SMManager.Get<PlayerUnit[]>(), new Predicate<PlayerUnit>(recommendPartOverkillers.\u003CdoInitialize\u003Eb__5_0)) == (PlayerUnit) null);
      recommendPartOverkillers.overkillersSkill_ = OverkillersSkillRelease.find(recommendPartOverkillers.target_);
      if (recommendPartOverkillers.overkillersSkill_ != null)
      {
        ((Component) recommendPartOverkillers.iconSkill_).gameObject.SetActive(true);
        Future<GameObject> ldSkill = Res.Prefabs.BattleSkillIcon._battleSkillIcon.Load<GameObject>();
        yield return (object) ldSkill.Wait();
        BattleSkillIcon icon2 = ldSkill.Result.Clone(((Component) recommendPartOverkillers.iconSkill_).transform).GetComponent<BattleSkillIcon>();
        yield return (object) icon2.Init(recommendPartOverkillers.overkillersSkill_.skill);
        ((UIWidget) icon2.iconSprite).depth = recommendPartOverkillers.iconSkill_.depth + 1;
        ldSkill = (Future<GameObject>) null;
        icon2 = (BattleSkillIcon) null;
      }
      else
        ((Component) recommendPartOverkillers.iconSkill_).gameObject.SetActive(false);
    }
  }

  public void onClickedSkill()
  {
    if (this.overkillersSkill_ == null || Object.op_Equality((Object) this.menu.skillDetailPrefab, (Object) null))
      return;
    PopupSkillDetails.show(this.menu.skillDetailPrefab, new PopupSkillDetails.Param(this.overkillersSkill_));
  }
}
