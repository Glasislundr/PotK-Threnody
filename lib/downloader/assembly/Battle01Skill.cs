// Decompiled with JetBrains decompiler
// Type: Battle01Skill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Battle01Skill : NGBattleMenuBase
{
  [SerializeField]
  private Battle01Skill.Container[] containers_;
  private BL.BattleModified<BL.Skill> modified;
  private BL.BattleModified<BL.Unit> modifiedUnit;
  private BL.BattleModified<BL.UnitPosition> unitPositionModified;
  private GameObject typeIconPrefab;
  private GameObject targetIconPrefab;
  private Battle01Skill.Container current_;
  private bool isCallSkill;

  private bool IsUIActive { get; set; }

  private T clonePrefab<T>(GameObject prefab, UI2DSprite parent) where T : IconPrefabBase
  {
    if (Object.op_Equality((Object) parent, (Object) null))
      return default (T);
    ((Behaviour) parent).enabled = false;
    T component = prefab.CloneAndGetComponent<T>(((Component) parent).transform);
    if (Object.op_Equality((Object) (object) component, (Object) null))
      return default (T);
    NGUITools.AdjustDepth(((Component) (object) component).gameObject, ((UIWidget) parent).depth);
    return component;
  }

  protected override IEnumerator Start_Original()
  {
    Future<GameObject> typeIconPrefabF = Res.Prefabs.BattleSkillIcon._battleSkillIcon.Load<GameObject>();
    IEnumerator e = typeIconPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.typeIconPrefab = typeIconPrefabF.Result;
    Future<GameObject> targetIconPrefabF = Res.Icons.SkillGenreIcon.Load<GameObject>();
    e = targetIconPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.targetIconPrefab = targetIconPrefabF.Result;
    foreach (Battle01Skill.Container container in this.containers_)
      container.dirTop.SetActive(false);
    this.changeContainer(Battle01Skill.Mode.Normal);
  }

  private bool UpdateUIActive()
  {
    bool uiButtonEnable = Singleton<NGBattleManager>.GetInstance().getController<BattleUIController>().uiButtonEnable;
    if (this.IsUIActive == uiButtonEnable)
      return false;
    this.IsUIActive = uiButtonEnable;
    return true;
  }

  protected override void LateUpdate_Battle()
  {
    bool flag1;
    bool flag2;
    if (this.isCallSkill)
    {
      flag1 = this.modified != null && this.modified.isChangedOnce();
      flag2 = flag1;
    }
    else
    {
      flag1 = this.modified != null && this.modifiedUnit != null && this.modified.isChangedOnce() | this.modifiedUnit.isChangedOnce();
      bool flag3 = this.unitPositionModified != null && this.unitPositionModified.isChangedOnce();
      bool flag4 = this.UpdateUIActive();
      flag2 = flag1 | flag3 | flag4;
    }
    if (!flag2)
      return;
    BL.Skill skill = this.modified.value;
    this.changeContainer(skill.skill.IsJobAbility ? Battle01Skill.Mode.JobAbility : Battle01Skill.Mode.Normal);
    this.initializeContainer(this.current_);
    if (flag1)
    {
      if (Object.op_Inequality((Object) this.current_.skillIcon, (Object) null))
        this.StartCoroutine(this.current_.skillIcon.Init(skill.skill));
      this.current_.property01Icon.Init(skill.genre1);
      this.current_.property02Icon.Init(skill.genre2);
      this.setText(this.current_.txt_name, skill.name);
      this.setText(this.current_.txt_remain, "×" + (object) skill.remain);
    }
    if (this.isCallSkill)
    {
      this.current_.dir_Disable.SetActive(false);
    }
    else
    {
      bool flag5 = false;
      if (skill.remain.HasValue)
      {
        int? remain = skill.remain;
        int num = 0;
        flag5 = remain.GetValueOrDefault() <= num & remain.HasValue;
      }
      if (!flag5 && skill.skill.target_type == BattleskillTargetType.myself)
        flag5 = this.modifiedUnit.value.skillEffects.CanUseSkill(skill.skill, skill.level, (BL.ISkillEffectListUnit) this.modifiedUnit.value, this.env.core, (BL.ISkillEffectListUnit) this.modifiedUnit.value, skill.nowUseCount) == 1;
      if (!flag5)
        flag5 = this.modifiedUnit.value.IsDontUseCommand(skill);
      if (!flag5)
        flag5 = !this.IsUIActive;
      this.current_.dir_Disable.SetActive(flag5);
    }
  }

  private void changeContainer(Battle01Skill.Mode mode)
  {
    int index = (int) mode;
    Battle01Skill.Container container = this.containers_.Length > index ? this.containers_[index] : this.containers_[0];
    if (this.current_ == container)
      return;
    if (this.current_ != null)
      this.current_.dirTop.SetActive(false);
    container.dirTop.SetActive(true);
    this.current_ = container;
  }

  private void initializeContainer(Battle01Skill.Container container)
  {
    if (container.isInitialized)
      return;
    container.isInitialized = true;
    container.skillIcon = this.clonePrefab<BattleSkillIcon>(this.typeIconPrefab, container.skill);
    container.property01Icon = this.clonePrefab<SkillGenreIcon>(this.targetIconPrefab, container.property01);
    container.property02Icon = this.clonePrefab<SkillGenreIcon>(this.targetIconPrefab, container.property02);
    this.clonePrefab<SkillGenreIcon>(this.targetIconPrefab, container.property03);
  }

  public void setSkill(BL.Skill skill, BL.Unit unit)
  {
    this.modified = BL.Observe<BL.Skill>(skill);
    this.modifiedUnit = BL.Observe<BL.Unit>(unit);
    this.unitPositionModified = BL.Observe<BL.UnitPosition>(this.env.core.getUnitPosition(unit));
    this.isCallSkill = skill.skill.skill_type == BattleskillSkillType.call;
  }

  public BL.Skill getSkill() => this.modified.value;

  public BL.Unit getUnit() => this.modifiedUnit.value;

  [Serializable]
  private class Container
  {
    public GameObject dirTop;
    public UI2DSprite skill;
    public UI2DSprite property01;
    public UI2DSprite property02;
    public UI2DSprite property03;
    public UILabel txt_name;
    public UILabel txt_remain;
    public GameObject dir_Disable;
    [NonSerialized]
    public bool isInitialized;
    [NonSerialized]
    public BattleSkillIcon skillIcon;
    [NonSerialized]
    public SkillGenreIcon property01Icon;
    [NonSerialized]
    public SkillGenreIcon property02Icon;
  }

  public enum Mode
  {
    Normal,
    JobAbility,
  }
}
