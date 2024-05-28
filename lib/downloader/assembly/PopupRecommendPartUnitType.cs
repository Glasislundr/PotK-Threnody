// Decompiled with JetBrains decompiler
// Type: PopupRecommendPartUnitType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnitStatusInformation;
using UnityEngine;

#nullable disable
[AddComponentMenu("Popup/Recommend/UnitType")]
public class PopupRecommendPartUnitType : PopupRecommendPart
{
  [SerializeField]
  private PopupRecommendPartUnitType.TabContainer[] tabs_;
  [SerializeField]
  private PopupRecommendPartUnitType.SkillContainer[] skills_;
  private GameObject iconPrefab_;
  private Dictionary<UnitTypeEnum, Tuple<BattleskillSkill, GameObject>[]> dicSkills_ = new Dictionary<UnitTypeEnum, Tuple<BattleskillSkill, GameObject>[]>();
  private Dictionary<UnitTypeEnum, PopupSkillDetails.Param[]> dicSkillParams_ = new Dictionary<UnitTypeEnum, PopupSkillDetails.Param[]>();
  private UnitTypeEnum curretnType_ = UnitTypeEnum.random;
  private Tuple<BattleskillSkill, GameObject>[] current_;

  public override IEnumerator doInitialize(PlayerUnit playerUnit, UnitUnit target)
  {
    PopupRecommendPartUnitType recommendPartUnitType = this;
    if (!((IEnumerable<UnitSkill>) MasterData.UnitSkillList).Any<UnitSkill>((Func<UnitSkill, bool>) (x => x.unit_UnitUnit == target.ID && x.unit_type != 0)))
    {
      ((Component) recommendPartUnitType).gameObject.SetActive(false);
    }
    else
    {
      recommendPartUnitType.playerUnit_ = playerUnit;
      recommendPartUnitType.target_ = target;
      foreach (Object @object in recommendPartUnitType.dicSkills_.Where<KeyValuePair<UnitTypeEnum, Tuple<BattleskillSkill, GameObject>[]>>((Func<KeyValuePair<UnitTypeEnum, Tuple<BattleskillSkill, GameObject>[]>, bool>) (x => x.Value != null && x.Value.Length != 0)).SelectMany<KeyValuePair<UnitTypeEnum, Tuple<BattleskillSkill, GameObject>[]>, GameObject>((Func<KeyValuePair<UnitTypeEnum, Tuple<BattleskillSkill, GameObject>[]>, IEnumerable<GameObject>>) (y => ((IEnumerable<Tuple<BattleskillSkill, GameObject>>) y.Value).Select<Tuple<BattleskillSkill, GameObject>, GameObject>((Func<Tuple<BattleskillSkill, GameObject>, GameObject>) (z => z.Item2)))))
        Object.Destroy(@object);
      int num = 0;
      UnitRecommend unitRecommend;
      if (MasterData.UnitRecommend.TryGetValue(target.same_character_id, out unitRecommend) && unitRecommend.unit_type_UnitType.HasValue)
        num = unitRecommend.unit_type_UnitType.Value;
      recommendPartUnitType.curretnType_ = UnitTypeEnum.random;
      recommendPartUnitType.current_ = (Tuple<BattleskillSkill, GameObject>[]) null;
      UnitTypeEnum[] validUnitTypes = target.validUnitTypes;
      recommendPartUnitType.dicSkills_ = ((IEnumerable<UnitTypeEnum>) validUnitTypes).ToDictionary<UnitTypeEnum, UnitTypeEnum, Tuple<BattleskillSkill, GameObject>[]>((Func<UnitTypeEnum, UnitTypeEnum>) (x => x), (Func<UnitTypeEnum, Tuple<BattleskillSkill, GameObject>[]>) (x => (Tuple<BattleskillSkill, GameObject>[]) null));
      for (int index = 0; index < recommendPartUnitType.tabs_.Length; ++index)
      {
        PopupRecommendPartUnitType.TabContainer tab = recommendPartUnitType.tabs_[index];
        if (((IEnumerable<UnitTypeEnum>) validUnitTypes).Contains<UnitTypeEnum>(tab.type))
        {
          tab.badge.SetActive((UnitTypeEnum) num == tab.type);
          tab.top.SetActive(true);
          tab.objDisabled.SetActive(false);
          EventDelegate.Set(tab.button.onClick, (EventDelegate.Callback) (() => this.onClickedTab(tab.type)));
        }
        else
        {
          tab.top.SetActive(false);
          tab.objDisabled.SetActive(true);
        }
      }
      yield return (object) recommendPartUnitType.doChange(((IEnumerable<UnitTypeEnum>) validUnitTypes).First<UnitTypeEnum>());
    }
  }

  private void setTab(UnitTypeEnum type)
  {
    for (int index = 0; index < this.tabs_.Length; ++index)
    {
      PopupRecommendPartUnitType.TabContainer tab = this.tabs_[index];
      if (tab.type == type)
        ((UIButtonColor) tab.button).isEnabled = false;
      else if (((Component) tab.button).gameObject.activeSelf)
        ((UIButtonColor) tab.button).isEnabled = true;
    }
  }

  private void onClickedTab(UnitTypeEnum type) => this.StartCoroutine("doChange", (object) type);

  private IEnumerator doChange(UnitTypeEnum type)
  {
    if (this.curretnType_ != type)
    {
      if (Object.op_Equality((Object) this.iconPrefab_, (Object) null))
      {
        Future<GameObject> ld = Res.Prefabs.BattleSkillIcon._battleSkillIcon.Load<GameObject>();
        yield return (object) ld.Wait();
        this.iconPrefab_ = ld.Result;
        ld = (Future<GameObject>) null;
      }
      if (this.current_ != null && this.current_.Length != 0)
      {
        for (int index = 0; index < this.current_.Length; ++index)
          this.current_[index].Item2.SetActive(false);
      }
      this.setTab(type);
      this.curretnType_ = type;
      this.current_ = this.dicSkills_[type];
      if (this.current_ == null)
      {
        BattleskillSkill[] skills = ((IEnumerable<UnitSkill>) MasterData.UnitSkillList).Where<UnitSkill>((Func<UnitSkill, bool>) (x => x.unit_UnitUnit == this.target_.ID && (UnitTypeEnum) x.unit_type == type)).OrderBy<UnitSkill, int>((Func<UnitSkill, int>) (y => y.skill_BattleskillSkill)).Take<UnitSkill>(this.skills_.Length).Select<UnitSkill, BattleskillSkill>((Func<UnitSkill, BattleskillSkill>) (z => z.skill)).ToArray<BattleskillSkill>();
        this.dicSkillParams_[type] = ((IEnumerable<BattleskillSkill>) skills).Select<BattleskillSkill, PopupSkillDetails.Param>((Func<BattleskillSkill, PopupSkillDetails.Param>) (x => new PopupSkillDetails.Param(x, UnitParameter.SkillGroup.Princess))).ToArray<PopupSkillDetails.Param>();
        this.current_ = new Tuple<BattleskillSkill, GameObject>[skills.Length];
        this.dicSkills_[type] = this.current_;
        for (int n = 0; n < this.current_.Length; ++n)
        {
          GameObject go = this.iconPrefab_.Clone(this.skills_[n].lnkIcon);
          BattleSkillIcon icon = go.GetComponent<BattleSkillIcon>();
          yield return (object) icon.Init(skills[n]);
          icon.SetDepth(this.skills_[n].depth);
          this.current_[n] = Tuple.Create<BattleskillSkill, GameObject>(skills[n], go);
          go = (GameObject) null;
          icon = (BattleSkillIcon) null;
        }
        skills = (BattleskillSkill[]) null;
      }
      int index1;
      for (index1 = 0; index1 < this.current_.Length; ++index1)
      {
        Tuple<BattleskillSkill, GameObject> tuple = this.current_[index1];
        tuple.Item2.SetActive(true);
        PopupRecommendPartUnitType.SkillContainer skill = this.skills_[index1];
        skill.txtName.SetTextLocalize(tuple.Item1.name);
        skill.txtDescription.SetHeadline(tuple.Item1.description);
        skill.top.SetActive(true);
        this.setEventClickedSkill(skill.btnDetail, type, index1);
      }
      for (; index1 < this.skills_.Length; ++index1)
        this.skills_[index1].top.SetActive(false);
    }
  }

  private void setEventClickedSkill(UIButton btn, UnitTypeEnum unitType, int index)
  {
    EventDelegate.Set(btn.onClick, (EventDelegate.Callback) (() =>
    {
      if (Object.op_Equality((Object) this.menu.skillDetailPrefab, (Object) null))
        return;
      PopupSkillDetails.show(this.menu.skillDetailPrefab, this.dicSkillParams_[unitType], index);
    }));
  }

  [Serializable]
  private class SkillContainer
  {
    public GameObject top;
    public Transform lnkIcon;
    public int depth;
    public UILabel txtName;
    public HeadlineLabel txtDescription;
    public UIButton btnDetail;
  }

  [Serializable]
  private class TabContainer
  {
    public UnitTypeEnum type;
    public GameObject top;
    public GameObject objDisabled;
    public UIButton button;
    public GameObject badge;
  }
}
