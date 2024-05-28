// Decompiled with JetBrains decompiler
// Type: Battle01SkillUse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnitStatusInformation;
using UnityEngine;

#nullable disable
public class Battle01SkillUse : NGBattleMenuBase
{
  [SerializeField]
  private UI2DSprite icon;
  [SerializeField]
  private UI2DSprite property1;
  [SerializeField]
  private UI2DSprite property2;
  [SerializeField]
  private UILabel txt_name;
  [SerializeField]
  private UILabel txt_description;
  [SerializeField]
  private UILabel txt_consume_hp;
  [SerializeField]
  private Battle01CommandSkillUse button;
  [SerializeField]
  private GameObject title_skill;
  [SerializeField]
  private GameObject title_secrets;
  [SerializeField]
  private GameObject notAvailable;
  [SerializeField]
  private UILabel notAvailableTxt;
  [SerializeField]
  private GameObject dirCallSkillDisable;
  [SerializeField]
  private UILabel txtCallSkill;
  [SerializeField]
  private BoxCollider btnSkillAll;
  [SerializeField]
  private GameObject slc_skillselect;
  [SerializeField]
  private GameObject slc_SEAskillselect;
  public GameObject typeIconPrefab;
  public GameObject targetIconPrefab;
  private BattleSkillIcon skillIcon;
  private SkillGenreIcon property1Icon;
  private SkillGenreIcon property2Icon;
  private GameObject skillDetailPrefab;
  private GameObject callSkillDetailPrefab;
  private BL.BattleModified<BL.Skill> modified;
  private BL.Unit unit;
  private int targetsCount;
  private int panelsCount;
  private int hpCost;
  private int hpMust;
  private bool isSelectPanel;
  private bool isPush;

  public Battle01CommandSkillUse SkillUse => this.button;

  private T clonePrefab<T>(GameObject prefab, UI2DSprite parent) where T : IconPrefabBase
  {
    ((Behaviour) parent).enabled = false;
    T component = prefab.CloneAndGetComponent<T>(((Component) parent).transform);
    if (Object.op_Equality((Object) (object) component, (Object) null))
      return default (T);
    NGUITools.AdjustDepth(((Component) (object) component).gameObject, ((UIWidget) parent).depth);
    component.SetBasedOnHeight(((UIWidget) parent).height);
    return component;
  }

  protected override IEnumerator Start_Original()
  {
    this.skillIcon = this.clonePrefab<BattleSkillIcon>(this.typeIconPrefab, this.icon);
    this.property1Icon = this.clonePrefab<SkillGenreIcon>(this.targetIconPrefab, this.property1);
    this.property2Icon = this.clonePrefab<SkillGenreIcon>(this.targetIconPrefab, this.property2);
    Future<GameObject> loader = PopupSkillDetails.createPrefabLoader(Singleton<NGGameDataManager>.GetInstance().IsSea);
    yield return (object) loader.Wait();
    this.skillDetailPrefab = loader.Result;
  }

  protected override void Update_Battle()
  {
    if (this.modified == null || !this.modified.isChangedOnce())
      return;
    BL.Skill skill = this.modified.value;
    this.StartCoroutine(this.skillIcon.Init(skill.skill));
    this.property1Icon.Init(skill.genre1);
    this.property2Icon.Init(skill.genre2);
    this.setText(this.txt_name, skill.name);
    this.setText(this.txt_description, skill.description);
    if (Object.op_Inequality((Object) this.slc_skillselect, (Object) null) && Object.op_Inequality((Object) this.slc_SEAskillselect, (Object) null))
    {
      if (skill.skill.skill_type == BattleskillSkillType.SEA)
      {
        this.slc_skillselect.SetActive(false);
        this.slc_SEAskillselect.SetActive(true);
      }
      else
      {
        this.slc_skillselect.SetActive(true);
        this.slc_SEAskillselect.SetActive(false);
      }
    }
    if (Object.op_Inequality((Object) this.txt_consume_hp, (Object) null))
    {
      if (this.hpCost > 0)
      {
        ((Component) this.txt_consume_hp).gameObject.SetActive(true);
        string v = Consts.Format(Consts.GetInstance().BATTLE_UI_CONSUME_HP, (IDictionary) new Hashtable()
        {
          {
            (object) "hp",
            (object) this.hpCost
          }
        });
        if (this.hpCost < this.unit.hp)
          this.setText(this.txt_consume_hp, v);
        else
          this.setText(this.txt_consume_hp, "[ff0000]" + v);
      }
      else
        ((Component) this.txt_consume_hp).gameObject.SetActive(false);
    }
    if (!Object.op_Inequality((Object) this.notAvailable, (Object) null))
      return;
    bool o_isEnoughHp;
    bool o_isEnoughTarget;
    if (this.CanUseSkill(out o_isEnoughHp, out o_isEnoughTarget))
    {
      this.notAvailable.SetActive(false);
      ((Component) this.button).gameObject.SetActive(true);
      this.button.setEnable(true);
      this.dirCallSkillDisable.SetActive(false);
    }
    else if (skill.skill.skill_type == BattleskillSkillType.call)
    {
      this.notAvailable.SetActive(false);
      ((Component) this.button).gameObject.SetActive(false);
      this.dirCallSkillDisable.SetActive(true);
      if (this.env.core.playerCallSkillState.isUsedCallSkill)
        this.txtCallSkill.SetTextLocalize(Consts.GetInstance().BATTLE_UI_USED_CALL_SKILL);
      else
        this.txtCallSkill.SetTextLocalize(Consts.GetInstance().BATTLE_UI_NOT_CHARGED_CALL_SKILL);
    }
    else
    {
      this.notAvailable.SetActive(true);
      ((Component) this.button).gameObject.SetActive(true);
      this.button.setEnable(false);
      if (!o_isEnoughHp)
        this.notAvailableTxt.SetTextLocalize(Consts.GetInstance().BATTLE_UI_NOT_ENOUGH_HP);
      else if (!o_isEnoughTarget)
      {
        if (!this.isSelectPanel)
          this.notAvailableTxt.SetTextLocalize(Consts.GetInstance().BATTLE_UI_NOT_ENOUGH_TARGET);
        else
          this.notAvailableTxt.SetTextLocalize(Consts.GetInstance().BATTLE_UI_NOT_ENOUGH_PANEL);
      }
      else
        this.notAvailableTxt.SetTextLocalize(string.Empty);
      this.dirCallSkillDisable.SetActive(false);
    }
  }

  public bool CanUseSkill(out bool o_isEnoughHp, out bool o_isEnoughTarget)
  {
    if (this.modified.value.skill.skill_type == BattleskillSkillType.call)
    {
      o_isEnoughHp = o_isEnoughTarget = true;
      return this.env.core.playerCallSkillState.isCanUseCallSkill;
    }
    bool flag1 = this.hpMust < this.unit.hp;
    bool flag2 = this.targetsCount > 0 || this.panelsCount > 0;
    o_isEnoughHp = flag1;
    o_isEnoughTarget = flag2;
    return flag1 & flag2;
  }

  public bool CanUseSkill() => this.CanUseSkill(out bool _, out bool _);

  public void setSkillTargets(
    BL.Unit unit,
    BL.Skill skill,
    List<BL.Unit> targets,
    List<BL.Panel> panels,
    bool isSelectPanel)
  {
    this.modified = BL.Observe<BL.Skill>(skill);
    this.unit = unit;
    if (skill.isOugi)
    {
      this.title_skill.SetActive(false);
      this.title_secrets.SetActive(true);
    }
    else
    {
      this.title_skill.SetActive(true);
      this.title_secrets.SetActive(false);
    }
    this.isSelectPanel = isSelectPanel;
    Tuple<int, int> hpCost = skill.getHpCost(unit);
    this.hpCost = hpCost.Item1;
    this.hpMust = hpCost.Item2;
    if (unit.hp <= this.hpMust)
    {
      targets = new List<BL.Unit>();
      panels = new List<BL.Panel>();
    }
    List<BL.Unit> list1 = targets.Where<BL.Unit>((Func<BL.Unit, bool>) (x => x.skillEffects.CanUseSkill(skill.skill, skill.level, (BL.ISkillEffectListUnit) x, this.env.core, (BL.ISkillEffectListUnit) unit, skill.nowUseCount) == 0)).ToList<BL.Unit>();
    BattleskillEffect battleskillEffect = ((IEnumerable<BattleskillEffect>) skill.skill.Effects).FirstOrDefault<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.random_choice && x.checkLevel(skill.level)));
    if (battleskillEffect != null && list1.Count < battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.min_unit_count))
      list1.Clear();
    this.targetsCount = list1.Count;
    List<BL.Unit> list2 = targets.Where<BL.Unit>((Func<BL.Unit, bool>) (x => x.skillEffects.CanUseSkill(skill.skill, skill.level, (BL.ISkillEffectListUnit) x, this.env.core, (BL.ISkillEffectListUnit) unit, skill.nowUseCount) == 1)).ToList<BL.Unit>();
    List<BL.Panel> list3 = panels.Where<BL.Panel>((Func<BL.Panel, bool>) (x => BattleFuncs.canUseSkillToPanel(skill.skill, skill.level, x, (BL.ISkillEffectListUnit) unit, skill.nowUseCount))).ToList<BL.Panel>();
    this.panelsCount = list3.Count<BL.Panel>();
    this.button.setData(unit, skill, list1, list3);
    List<BL.Unit> list4 = list1.Concat<BL.Unit>((IEnumerable<BL.Unit>) list2).Distinct<BL.Unit>().ToList<BL.Unit>();
    List<BL.Unit> attackTargets;
    List<BL.Unit> healTargets;
    if (skill.targetType == BattleskillTargetType.complex_range || skill.targetType == BattleskillTargetType.complex_single)
    {
      BL.ForceID forceId = this.env.core.getForceID(unit);
      attackTargets = new List<BL.Unit>();
      healTargets = new List<BL.Unit>();
      foreach (BL.Unit unit1 in list4)
      {
        if (this.env.core.getForceID(unit1) == forceId)
          healTargets.Add(unit1);
        else
          attackTargets.Add(unit1);
      }
    }
    else if (skill.isOwn)
    {
      attackTargets = new List<BL.Unit>();
      healTargets = list4;
    }
    else
    {
      attackTargets = list4;
      healTargets = new List<BL.Unit>();
    }
    Singleton<NGBattleManager>.GetInstance().getController<BattleInputObserver>().setTargetSelectMode(attackTargets, healTargets, list2, list3, (Action<BL.Unit, BL.Panel>) ((u, p) =>
    {
      Battle01CommandSkillUse[] componentsInChildren = ((Component) this).GetComponentsInChildren<Battle01CommandSkillUse>();
      if (componentsInChildren.Length == 0)
        return;
      componentsInChildren[0].onClick();
    }));
    this.SetDetailCollider(true);
  }

  public void setCallSkillTargets(BL.Skill skill, List<BL.Unit> targets)
  {
    this.modified = BL.Observe<BL.Skill>(skill);
    this.unit = (BL.Unit) null;
    if (skill.isOugi)
    {
      this.title_skill.SetActive(false);
      this.title_secrets.SetActive(true);
    }
    else
    {
      this.title_skill.SetActive(true);
      this.title_secrets.SetActive(false);
    }
    this.isSelectPanel = false;
    this.hpCost = 0;
    this.hpMust = 0;
    List<BL.Unit> list1 = targets.Where<BL.Unit>((Func<BL.Unit, bool>) (x => x.skillEffects.CanUseSkill(skill.skill, skill.level, (BL.ISkillEffectListUnit) x, this.env.core, (BL.ISkillEffectListUnit) this.unit, 1, new bool?(true)) == 0)).ToList<BL.Unit>();
    this.targetsCount = list1.Count;
    List<BL.Unit> list2 = targets.Where<BL.Unit>((Func<BL.Unit, bool>) (x => x.skillEffects.CanUseSkill(skill.skill, skill.level, (BL.ISkillEffectListUnit) x, this.env.core, (BL.ISkillEffectListUnit) this.unit, 1, new bool?(true)) == 1)).ToList<BL.Unit>();
    List<BL.Panel> panels = new List<BL.Panel>();
    this.panelsCount = 0;
    this.button.setData(this.unit, skill, list1, panels);
    list1.Concat<BL.Unit>((IEnumerable<BL.Unit>) list2).Distinct<BL.Unit>().ToList<BL.Unit>();
    List<BL.Unit> attackTargets = new List<BL.Unit>();
    List<BL.Unit> healTargets = new List<BL.Unit>();
    foreach (BL.Unit target in targets)
    {
      if (target.isPlayerForce)
        healTargets.Add(target);
      else
        attackTargets.Add(target);
    }
    Singleton<NGBattleManager>.GetInstance().getController<BattleInputObserver>().setTargetSelectMode(attackTargets, healTargets, list2, panels, (Action<BL.Unit, BL.Panel>) ((u, p) =>
    {
      Battle01CommandSkillUse[] componentsInChildren = ((Component) this).GetComponentsInChildren<Battle01CommandSkillUse>();
      if (componentsInChildren.Length == 0)
        return;
      componentsInChildren[0].onClick();
    }));
    this.SetDetailCollider(true);
  }

  public void onClickedZoom()
  {
    BL.Skill s;
    if (this.isPush || this.modified == null || (s = this.modified.value) == null)
      return;
    BattleskillSkill skill = s.skill;
    this.isPush = true;
    PopupSkillDetails.Param obj;
    switch (skill.skill_type)
    {
      case BattleskillSkillType.call:
        this.StartCoroutine(this.OpenCallSkillPopup(skill));
        return;
      case BattleskillSkillType.SEA:
        obj = PopupSkillDetails.Param.createBySEASkillView(s, this.unit.playerUnit.countEquippedOverkillers);
        break;
      default:
        obj = new PopupSkillDetails.Param(skill, UnitParameter.SkillGroup.Command, new int?(s.level));
        this.isPush = false;
        break;
    }
    if (!Object.op_Inequality((Object) this.skillDetailPrefab, (Object) null) || obj == null)
      return;
    PopupSkillDetails.show(this.skillDetailPrefab, obj, isAutoClose: true);
    this.isPush = false;
  }

  private IEnumerator OpenCallSkillPopup(BattleskillSkill s)
  {
    Battle01SkillUse battle01SkillUse = this;
    if (Object.op_Equality((Object) battle01SkillUse.callSkillDetailPrefab, (Object) null))
    {
      Future<GameObject> prefabF = new ResourceObject(Singleton<NGGameDataManager>.GetInstance().IsSea ? "Prefabs/UnitGUIs/Popup_CallSkillDetails_Sea" : "Prefabs/UnitGUIs/Popup_CallSkillDetails").Load<GameObject>();
      IEnumerator e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      battle01SkillUse.callSkillDetailPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    UnitUnit unit = (UnitUnit) null;
    foreach (UnitUnit unitUnit in MasterData.UnitUnitList)
    {
      if (unitUnit.same_character_id == battle01SkillUse.env.core.playerCallSkillState.sameCharacterID)
      {
        unit = unitUnit;
        break;
      }
    }
    yield return (object) Singleton<PopupManager>.GetInstance().open(battle01SkillUse.callSkillDetailPrefab).GetComponent<PopupCallSkill>().initialize(unit, s, true, battle01SkillUse.env.core.playerCallSkillState.callGaugeRate);
    battle01SkillUse.isPush = false;
  }

  public void SetDetailCollider(bool value) => ((Collider) this.btnSkillAll).enabled = value;

  public void clearModified() => this.modified = (BL.BattleModified<BL.Skill>) null;
}
