// Decompiled with JetBrains decompiler
// Type: Battle01SkillSubject
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
public class Battle01SkillSubject : NGBattleMenuBase
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
  private GameObject notAvailable;
  [SerializeField]
  private List<GameObject> notAvailableReason;
  [SerializeField]
  private GameObject slc_skillselect;
  [SerializeField]
  private GameObject slc_SEAskillselect;
  public GameObject typeIconPrefab;
  public GameObject targetIconPrefab;
  private BattleSkillIcon skillIcon;
  private SkillGenreIcon property1Icon;
  private SkillGenreIcon property2Icon;
  private BL.Unit unit;
  private BL.BattleModified<BL.Skill> modified;
  private List<BL.Unit> skillTargets;
  private List<BL.Unit> skillGrayTargets;
  private List<BL.Panel> skillPanels;
  private int hpCost;
  private int hpMust;
  private bool isSelectPanel;
  private GameObject skillDetailPrefab;
  public NGHorizontalScrollParts _scroll;

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

  private void Awake()
  {
    this.skillIcon = this.clonePrefab<BattleSkillIcon>(this.typeIconPrefab, this.icon);
    this.property1Icon = this.clonePrefab<SkillGenreIcon>(this.targetIconPrefab, this.property1);
    this.property2Icon = this.clonePrefab<SkillGenreIcon>(this.targetIconPrefab, this.property2);
  }

  private void OnEnable()
  {
    if (this.modified == null || this.skillTargets == null)
      return;
    BL.Skill skill = this.modified.value;
    Battle01SkillUseSelect[] componentsInChildren = ((Component) this).GetComponentsInChildren<Battle01SkillUseSelect>(true);
    if (componentsInChildren.Length == 0)
      return;
    componentsInChildren[0].setTargets(this.unit, skill, this.skillTargets, this.skillGrayTargets, this.skillPanels);
  }

  protected override IEnumerator Start_Original()
  {
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
    if (!Object.op_Inequality((Object) this.notAvailable, (Object) null) || this.notAvailableReason == null || this.notAvailableReason.Count <= 0)
      return;
    if (this.hpMust > 0 && this.hpMust >= this.unit.hp)
    {
      this.notAvailable.SetActive(true);
      this.notAvailableReason.ToggleOnce(0);
    }
    else if (this.skillTargets.Count <= 0 && this.skillPanels.Count<BL.Panel>() <= 0)
    {
      this.notAvailable.SetActive(true);
      if (!this.isSelectPanel)
        this.notAvailableReason.ToggleOnce(1);
      else
        this.notAvailableReason.ToggleOnce(2);
    }
    else
      this.notAvailable.SetActive(false);
  }

  public void setSkillTargets(
    BL.Unit unit,
    BL.Skill skill,
    List<BL.Unit> targets,
    List<BL.Panel> panels,
    bool isSelectPanel)
  {
    this.unit = unit;
    this.modified = BL.Observe<BL.Skill>(skill);
    this.isSelectPanel = isSelectPanel;
    Tuple<int, int> hpCost = skill.getHpCost(unit);
    this.hpCost = hpCost.Item1;
    this.hpMust = hpCost.Item2;
    if (unit.hp <= this.hpMust)
    {
      targets = new List<BL.Unit>();
      panels = new List<BL.Panel>();
    }
    this.skillTargets = targets.Where<BL.Unit>((Func<BL.Unit, bool>) (x => x.skillEffects.CanUseSkill(skill.skill, skill.level, (BL.ISkillEffectListUnit) x, this.env.core, (BL.ISkillEffectListUnit) unit, skill.nowUseCount) == 0)).ToList<BL.Unit>();
    this.skillGrayTargets = targets.Where<BL.Unit>((Func<BL.Unit, bool>) (x => x.skillEffects.CanUseSkill(skill.skill, skill.level, (BL.ISkillEffectListUnit) x, this.env.core, (BL.ISkillEffectListUnit) unit, skill.nowUseCount) == 1)).ToList<BL.Unit>();
    this.skillPanels = panels.Where<BL.Panel>((Func<BL.Panel, bool>) (x => BattleFuncs.canUseSkillToPanel(skill.skill, skill.level, x, (BL.ISkillEffectListUnit) unit, skill.nowUseCount))).ToList<BL.Panel>();
    Battle01SkillUseSelect[] componentsInChildren1 = ((Component) this).GetComponentsInChildren<Battle01SkillUseSelect>(true);
    if (componentsInChildren1.Length != 0)
      componentsInChildren1[0].setTargets(unit, skill, this.skillTargets, this.skillGrayTargets, this.skillPanels);
    Battle01SkillTitle[] componentsInChildren2 = ((Component) this).GetComponentsInChildren<Battle01SkillTitle>(true);
    if (componentsInChildren2.Length == 0)
      return;
    componentsInChildren2[0].setSkill(skill);
  }

  public void resetScrollView()
  {
    if (Object.op_Equality((Object) this._scroll, (Object) null))
    {
      NGHorizontalScrollParts[] componentsInChildren = ((Component) this).GetComponentsInChildren<NGHorizontalScrollParts>(true);
      if (componentsInChildren.Length != 0)
        this._scroll = componentsInChildren[0];
    }
    this._scroll.resetScrollView();
  }

  public void useUnit(BL.Unit unit, BL.Panel panel)
  {
    if (this.modified == null)
      return;
    Battle01SelectNode inParents = NGUITools.FindInParents<Battle01SelectNode>(((Component) this).transform);
    List<BL.Unit> unitList = new List<BL.Unit>();
    if (unit != (BL.Unit) null)
      unitList.Add(unit);
    List<BL.Panel> panelList = new List<BL.Panel>();
    if (panel != null)
      panelList.Add(panel);
    BL.Skill skill = this.modified.value;
    List<BL.Unit> targets = unitList;
    List<BL.Panel> panels = panelList;
    int num = this.isSelectPanel ? 1 : 0;
    inParents.useSkillUse(skill, targets, panels, num != 0);
  }

  public void onClickedZoom()
  {
    BL.Skill s;
    if (Object.op_Equality((Object) this.skillDetailPrefab, (Object) null) || this.modified == null || (s = this.modified.value) == null)
      return;
    BattleskillSkill skill = s.skill;
    if (skill.skill_type == BattleskillSkillType.SEA)
      PopupSkillDetails.show(this.skillDetailPrefab, PopupSkillDetails.Param.createBySEASkillView(s, this.unit.playerUnit.countEquippedOverkillers), isAutoClose: true);
    else
      PopupSkillDetails.show(this.skillDetailPrefab, new PopupSkillDetails.Param(skill, UnitParameter.SkillGroup.Command, new int?(s.level)), isAutoClose: true);
  }
}
