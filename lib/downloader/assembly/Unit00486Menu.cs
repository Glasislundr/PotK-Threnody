// Decompiled with JetBrains decompiler
// Type: Unit00486Menu
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
public class Unit00486Menu : UnitSelectMenuBase
{
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  protected UIButton ibtnEnter;
  [Header("上昇前現在値ラベル")]
  [SerializeField]
  protected UILabel TxtBeforeNowHP;
  [SerializeField]
  protected UILabel TxtBeforeNowPower;
  [SerializeField]
  protected UILabel TxtBeforeNowMagic;
  [SerializeField]
  protected UILabel TxtBeforeNowProtect;
  [SerializeField]
  protected UILabel TxtBeforeNowSprit;
  [SerializeField]
  protected UILabel TxtBeforeNowSpeed;
  [SerializeField]
  protected UILabel TxtBeforeNowTechnique;
  [SerializeField]
  protected UILabel TxtBeforeNowLucky;
  [Header("上昇値ラベル")]
  [SerializeField]
  protected UILabel TxtIncHP;
  [SerializeField]
  protected UILabel TxtIncPower;
  [SerializeField]
  protected UILabel TxtIncMagic;
  [SerializeField]
  protected UILabel TxtIncProtect;
  [SerializeField]
  protected UILabel TxtIncSprit;
  [SerializeField]
  protected UILabel TxtIncSpeed;
  [SerializeField]
  protected UILabel TxtIncTechnique;
  [SerializeField]
  protected UILabel TxtIncLucky;
  private UnitTypeParameter unitTypeParameter;
  private List<UnitIconInfo> firstSelectedUnitIcons;
  protected Player player;
  protected int playerUnitMax;
  private PlayerMaterialUnit[] materials;
  private List<int> lockMaterialIds = new List<int>();
  public float currentMaxTrust;
  private bool singleCancelUpdateInfomation;
  private UnitIconInfo currentUnitInfo;

  public Player Player => this.player;

  public float selectedUnityValue { get; protected set; }

  public float selectedBuildupUnityValue { get; protected set; }

  public virtual IEnumerator Init(
    Player player,
    PlayerUnit basePlayerUnit,
    PlayerUnit[] playerUnits,
    PlayerMaterialUnit[] playerMaterialUnits,
    PlayerUnit[] selectUnits,
    PlayerDeck[] playerDeck,
    bool isEquip,
    int selMax,
    float currentMaxTrust = 0.0f)
  {
    Unit00486Menu unit00486Menu = this;
    unit00486Menu.currentMaxTrust = currentMaxTrust;
    IEnumerator e = unit00486Menu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00486Menu.player = player;
    unit00486Menu.baseUnit = basePlayerUnit;
    unit00486Menu.playerUnitMax = playerUnits.Length;
    unit00486Menu.unitTypeParameter = unit00486Menu.baseUnit.UnitTypeParameter;
    unit00486Menu.SelectMax = selMax;
    unit00486Menu.InitializeInfoEx((IEnumerable<PlayerUnit>) ((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != this.baseUnit && this.isComposeUnit(this.baseUnit, x))).ToArray<PlayerUnit>(), (IEnumerable<PlayerMaterialUnit>) ((IEnumerable<PlayerMaterialUnit>) playerMaterialUnits).Where<PlayerMaterialUnit>((Func<PlayerMaterialUnit, bool>) (x => this.isComposeUnit(this.baseUnit, x))).ToArray<PlayerMaterialUnit>(), Persist.unit00486SortAndFilter, isEquip, false, true, true, true, false, (Action) (() => this.InitializeAllUnitInfosExtend(playerDeck)));
    unit00486Menu.CreateSelectIconInfo(selectUnits, true);
    e = unit00486Menu.CreateUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00486Menu.UpdateInfomation();
    unit00486Menu.InitializeEnd();
  }

  public override IEnumerator Initialize()
  {
    Unit00486Menu unit00486Menu = this;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    Future<GameObject> prefabF = Res.Prefabs.UnitIcon.detail.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00486Menu.unitPrefab = prefabF.Result;
    unit00486Menu.sortType = UnitSortAndFilter.SORT_TYPES.BranchOfAnArmy;
    unit00486Menu.orderType = SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING;
    unit00486Menu.isInitialize = false;
    unit00486Menu.scroll.Clear();
  }

  public IEnumerator UpdateInfoAndScrollExtend(
    PlayerUnit[] playerUnits,
    PlayerMaterialUnit[] playerMaterialUnits,
    PlayerUnit[] materialUnits)
  {
    Unit00486Menu unit00486Menu = this;
    IEnumerator e = unit00486Menu.UpdateInfoAndScroll(playerUnits, playerMaterialUnits);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00486Menu.CreateSelectIconInfo(materialUnits, false);
  }

  public virtual void SetUpperParamterLabel(
    UILabel label,
    int composeParam,
    int maxParam,
    int incParam)
  {
    if (maxParam > composeParam + incParam)
    {
      if (incParam > 0)
        ((UIWidget) label).color = Color.yellow;
      else
        ((UIWidget) label).color = Color.white;
    }
    else
      ((UIWidget) label).color = Color.red;
    int num = composeParam + incParam;
    if (num == 0)
      label.SetTextLocalize(num);
    else
      label.SetTextLocalize(Consts.Format(Consts.GetInstance().UNIT486_PLUS, (IDictionary) new Hashtable()
      {
        {
          (object) "point",
          (object) num
        }
      }));
  }

  private void SetBeforeUpperNowParameter()
  {
    this.TxtBeforeNowHP.SetTextLocalize(this.baseUnit.self_total_hp);
    this.TxtBeforeNowPower.SetTextLocalize(this.baseUnit.self_total_strength);
    this.TxtBeforeNowMagic.SetTextLocalize(this.baseUnit.self_total_intelligence);
    this.TxtBeforeNowProtect.SetTextLocalize(this.baseUnit.self_total_vitality);
    this.TxtBeforeNowSprit.SetTextLocalize(this.baseUnit.self_total_mind);
    this.TxtBeforeNowSpeed.SetTextLocalize(this.baseUnit.self_total_agility);
    this.TxtBeforeNowTechnique.SetTextLocalize(this.baseUnit.self_total_dexterity);
    this.TxtBeforeNowLucky.SetTextLocalize(this.baseUnit.self_total_lucky);
  }

  protected virtual void SetUpperParameter(PlayerUnit[] materialPlayerUnits)
  {
    int composeValue1 = CalcUnitCompose.getComposeValue(this.baseUnit, materialPlayerUnits, CalcUnitCompose.ComposeType.HP);
    int composeValue2 = CalcUnitCompose.getComposeValue(this.baseUnit, materialPlayerUnits, CalcUnitCompose.ComposeType.STRENGTH);
    int composeValue3 = CalcUnitCompose.getComposeValue(this.baseUnit, materialPlayerUnits, CalcUnitCompose.ComposeType.INTELLIGENCE);
    int composeValue4 = CalcUnitCompose.getComposeValue(this.baseUnit, materialPlayerUnits, CalcUnitCompose.ComposeType.VITALITY);
    int composeValue5 = CalcUnitCompose.getComposeValue(this.baseUnit, materialPlayerUnits, CalcUnitCompose.ComposeType.MIND);
    int composeValue6 = CalcUnitCompose.getComposeValue(this.baseUnit, materialPlayerUnits, CalcUnitCompose.ComposeType.AGILITY);
    int composeValue7 = CalcUnitCompose.getComposeValue(this.baseUnit, materialPlayerUnits, CalcUnitCompose.ComposeType.DEXTERITY);
    int composeValue8 = CalcUnitCompose.getComposeValue(this.baseUnit, materialPlayerUnits, CalcUnitCompose.ComposeType.LUCKY);
    this.selectedUnityValue = Mathf.Min(CalcUnitCompose.getComposeUnity(this.baseUnit, materialPlayerUnits, true), (float) PlayerUnit.GetUnityValueMax());
    this.selectedBuildupUnityValue = Mathf.Min(CalcUnitCompose.getComposeUnity(this.baseUnit, materialPlayerUnits, false), (float) PlayerUnit.GetUnityValueMax());
    this.SetUpperParamterLabel(this.TxtIncHP, this.baseUnit.hp.compose, this.baseUnit.compose_hp_max, composeValue1);
    this.SetUpperParamterLabel(this.TxtIncPower, this.baseUnit.strength.compose, this.baseUnit.compose_strength_max, composeValue2);
    this.SetUpperParamterLabel(this.TxtIncMagic, this.baseUnit.intelligence.compose, this.baseUnit.compose_intelligence_max, composeValue3);
    this.SetUpperParamterLabel(this.TxtIncProtect, this.baseUnit.vitality.compose, this.baseUnit.compose_vitality_max, composeValue4);
    this.SetUpperParamterLabel(this.TxtIncSprit, this.baseUnit.mind.compose, this.baseUnit.compose_mind_max, composeValue5);
    this.SetUpperParamterLabel(this.TxtIncSpeed, this.baseUnit.agility.compose, this.baseUnit.compose_agility_max, composeValue6);
    this.SetUpperParamterLabel(this.TxtIncTechnique, this.baseUnit.dexterity.compose, this.baseUnit.compose_dexterity_max, composeValue7);
    this.SetUpperParamterLabel(this.TxtIncLucky, this.baseUnit.lucky.compose, this.baseUnit.compose_lucky_max, composeValue8);
  }

  protected virtual void SetPrice(PlayerUnit[] materialPlayerUnits)
  {
    long num = CalcUnitCompose.priceCompose(this.baseUnit, materialPlayerUnits);
    this.TxtNumberzeny.SetTextLocalize(num.ToString());
    if (num > this.player.money)
    {
      ((UIWidget) this.TxtNumberzeny).color = Color.red;
      ((UIButtonColor) this.ibtnEnter).isEnabled = false;
    }
    else
    {
      ((UIWidget) this.TxtNumberzeny).color = Color.white;
      if (materialPlayerUnits.Length == 0)
        return;
      ((UIButtonColor) this.ibtnEnter).isEnabled = true;
    }
  }

  protected override void UnitInfoUpdate(UnitIconInfo info, bool enable, int cnt)
  {
    info.gray = enable;
    if (info.unit.IsNormalUnit)
      info.tempSelectedCount = cnt != -1 ? 1 : 0;
    if (cnt == -1)
      info.ComposeUnSelect();
    else
      info.ComposeSelect();
  }

  public void resetCurrentUnitInfo()
  {
    this.currentUnitInfo = (UnitIconInfo) null;
    this.singleCancelUpdateInfomation = false;
  }

  public override void UpdateInfomation()
  {
    if (this.singleCancelUpdateInfomation)
    {
      this.singleCancelUpdateInfomation = false;
    }
    else
    {
      List<PlayerUnit> playerUnitList = new List<PlayerUnit>();
      foreach (UnitIconInfo selectedUnitIcon in this.selectedUnitIcons)
      {
        if (this.currentUnitInfo != selectedUnitIcon)
        {
          for (int index = 0; index < selectedUnitIcon.tempSelectedCount; ++index)
            playerUnitList.Add(selectedUnitIcon.playerUnit);
        }
      }
      PlayerUnit[] array = playerUnitList.ToArray();
      this.SetPrice(array);
      this.SetBeforeUpperNowParameter();
      this.SetUpperParameter(array);
      this.TxtNumberselect.SetTextLocalize(string.Format("{0}/{1}", (object) this.selectedUnitIcons.Count, (object) this.SelectMax));
      ((UIWidget) this.TxtNumberselect).color = this.selectedUnitIcons.Count > this.SelectMax - 1 ? Color.red : Color.white;
      this.TxtNumberpossession.SetTextLocalize(string.Format("{0}/{1}", (object) this.playerUnitMax, (object) this.player.max_units));
    }
  }

  public virtual bool isComposeUnit(PlayerUnit baseUnit, PlayerUnit unit)
  {
    UnitUnit unit_unit = unit.unit;
    if (unit_unit.IsBuildup)
      return false;
    UnitUnit base_unit = baseUnit.unit;
    if (unit_unit.same_character_id == base_unit.same_character_id || unit_unit.character_UnitCharacter == base_unit.character_UnitCharacter && (double) baseUnit.trust_rate < (double) baseUnit.trust_max_rate)
      return true;
    IEnumerable<PlayerUnitSkills> source = ((IEnumerable<PlayerUnitSkills>) baseUnit.skills).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => x.level < x.skill.upper_level));
    bool isSkill = source.Any<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (unitBase => ((IEnumerable<PlayerUnitSkills>) unit.skills).Count<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => unitBase.skill_id == x.skill_id)) > 0));
    if (unit_unit.same_character_id == base_unit.same_character_id && source.Count<PlayerUnitSkills>() > 0)
      isSkill = true;
    else
      ((IEnumerable<PlayerUnitSkills>) baseUnit.skills).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => x.level < x.skill.upper_level)).ForEach<PlayerUnitSkills>((Action<PlayerUnitSkills>) (y =>
      {
        if (base_unit.same_character_id == unit_unit.same_character_id)
          return;
        int[] array1 = ((IEnumerable<PlayerUnitSkills>) unit.skills).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (z => y.skill_id == z.skill_id)).Select<PlayerUnitSkills, int>((Func<PlayerUnitSkills, int>) (x => x.skill_id)).ToArray<int>();
        if (array1.Length == 0)
          return;
        int[] array2 = ((IEnumerable<UnitSkill>) base_unit.RememberUnitSkills(baseUnit._unit_type)).Select<UnitSkill, int>((Func<UnitSkill, int>) (x => x.ID)).ToArray<int>();
        for (int index = 0; index < array1.Length; ++index)
        {
          if (((IEnumerable<int>) array2).Contains<int>(array1[index]))
          {
            isSkill = true;
            break;
          }
        }
      }));
    bool flag1 = false;
    if (unit_unit.hp_compose != 0 && baseUnit.hp.compose < baseUnit.compose_hp_max)
      flag1 = true;
    else if (unit_unit.strength_compose != 0 && baseUnit.strength.compose < baseUnit.compose_strength_max)
      flag1 = true;
    else if (unit_unit.vitality_compose != 0 && baseUnit.vitality.compose < baseUnit.compose_vitality_max)
      flag1 = true;
    else if (unit_unit.intelligence_compose != 0 && baseUnit.intelligence.compose < baseUnit.compose_intelligence_max)
      flag1 = true;
    else if (unit_unit.mind_compose != 0 && baseUnit.mind.compose < baseUnit.compose_mind_max)
      flag1 = true;
    else if (unit_unit.agility_compose != 0 && baseUnit.agility.compose < baseUnit.compose_agility_max)
      flag1 = true;
    else if (unit_unit.dexterity_compose != 0 && baseUnit.dexterity.compose < baseUnit.compose_dexterity_max)
      flag1 = true;
    else if ((base_unit.same_character_id == unit_unit.same_character_id || unit_unit.lucky_compose != 0) && baseUnit.lucky.compose < baseUnit.compose_lucky_max)
      flag1 = true;
    bool flag2 = false;
    bool flag3 = false;
    if (baseUnit.breakthrough_count < base_unit.breakthrough_limit)
    {
      if (unit_unit.IsBreakThrough)
        flag2 = unit_unit.CheckBreakThroughMaterial(baseUnit);
      else
        flag3 = base_unit.same_character_id == unit_unit.same_character_id;
    }
    bool flag4 = UnitDetailIcon.IsSkillUpMaterial(unit_unit, baseUnit);
    bool flag5 = false;
    bool flag6 = false;
    bool flag7 = false;
    if (unit_unit.same_character_id == base_unit.same_character_id)
    {
      if (base_unit.trust_target_flag && (double) baseUnit.trust_rate < (double) baseUnit.trust_max_rate)
        flag5 = true;
      if (baseUnit.unity_value < PlayerUnit.GetUnityValueMax())
        flag6 = true;
      if (baseUnit.lucky.compose < this.unitTypeParameter.lucky_compose_max)
        flag7 = true;
    }
    return isSkill | flag1 | flag2 | flag4 | flag3 | flag5 | flag6 | flag7;
  }

  public virtual bool isComposeUnit(PlayerUnit baseUnit, PlayerMaterialUnit materialUnit)
  {
    UnitUnit unit1 = materialUnit.unit;
    if (unit1.IsBuildup)
      return false;
    bool flag1 = false;
    UnitUnit unit2 = baseUnit.unit;
    if (unit1.hp_compose != 0 && baseUnit.hp.compose < baseUnit.compose_hp_max)
      flag1 = true;
    else if (unit1.strength_compose != 0 && baseUnit.strength.compose < baseUnit.compose_strength_max)
      flag1 = true;
    else if (unit1.vitality_compose != 0 && baseUnit.vitality.compose < baseUnit.compose_vitality_max)
      flag1 = true;
    else if (unit1.intelligence_compose != 0 && baseUnit.intelligence.compose < baseUnit.compose_intelligence_max)
      flag1 = true;
    else if (unit1.mind_compose != 0 && baseUnit.mind.compose < baseUnit.compose_mind_max)
      flag1 = true;
    else if (unit1.agility_compose != 0 && baseUnit.agility.compose < baseUnit.compose_agility_max)
      flag1 = true;
    else if (unit1.dexterity_compose != 0 && baseUnit.dexterity.compose < baseUnit.compose_dexterity_max)
      flag1 = true;
    else if ((unit2.same_character_id == unit1.same_character_id || unit1.lucky_compose != 0) && baseUnit.lucky.compose < baseUnit.compose_lucky_max)
      flag1 = true;
    bool flag2 = false;
    if (unit1.IsBreakThrough && baseUnit.breakthrough_count < unit2.breakthrough_limit)
      flag2 = unit1.CheckBreakThroughMaterial(baseUnit);
    bool flag3 = false;
    if (unit2.trust_target_flag && unit1.IsTrustMaterial(baseUnit) && (double) baseUnit.trust_rate < (double) baseUnit.trust_max_rate)
      flag3 = true;
    bool flag4 = UnitDetailIcon.IsSkillUpMaterial(unit1, baseUnit);
    bool flag5 = false;
    if (unit1.IsBuildUpMaterial(baseUnit))
      flag5 = true;
    bool flag6 = (double) baseUnit.unityTotal + (double) this.selectedUnityValue + (double) this.selectedBuildupUnityValue < (double) PlayerUnit.GetUnityValueMax();
    bool flag7 = unit1.is_unity_value_up & flag6 && unit1.FindValueUpPattern(unit2, (Func<UnitFamily[]>) (() => baseUnit.Families)) != null;
    bool flag8 = (double) baseUnit.unity_value + (double) this.selectedUnityValue < (double) PlayerUnit.GetUnityValueMax();
    bool flag9 = unit1.is_unity_value_up & flag8 && unit1.FindPureValueUpPattern(unit2) != null;
    return flag1 | flag2 | flag4 | flag3 | flag5 | flag7 | flag9;
  }

  protected void CreateSelectIconInfo(PlayerUnit[] selectUnits, bool updateFirst)
  {
    bool flag = this.SelectedUnitIsMax();
    this.selectedUnitIcons.Clear();
    foreach (PlayerUnit selectUnit1 in selectUnits)
    {
      PlayerUnit selectUnit = selectUnit1;
      UnitIconInfo unitIconInfo = this.allUnitInfos.FirstOrDefault<UnitIconInfo>((Func<UnitIconInfo, bool>) (y =>
      {
        if (!(y.playerUnit == selectUnit) || y.playerUnit.favorite)
          return false;
        return !y.playerUnit.unit.IsNormalUnit || OverkillersUtil.checkDelete(y.playerUnit);
      }));
      if (unitIconInfo != null)
      {
        if (unitIconInfo.playerUnit.UnitIconInfo == null)
          unitIconInfo.playerUnit.UnitIconInfo = selectUnit.UnitIconInfo;
        int num = !updateFirst ? unitIconInfo.SelectedCount : selectUnit.UnitIconInfo.SelectedCount;
        unitIconInfo.SelectedCount = num;
        unitIconInfo.tempSelectedCount = num;
        unitIconInfo.gray = true;
        if (flag && Object.op_Inequality((Object) unitIconInfo.icon, (Object) null))
          unitIconInfo.icon.Gray = false;
        this.selectedUnitIcons.Add(unitIconInfo);
      }
    }
    this.selectedUnitIcons = this.selectedUnitIcons.OrderBy<UnitIconInfo, int>((Func<UnitIconInfo, int>) (ui => ui.select)).ToList<UnitIconInfo>();
    if (!updateFirst)
      return;
    this.firstSelectedUnitIcons = new List<UnitIconInfo>((IEnumerable<UnitIconInfo>) this.selectedUnitIcons);
  }

  public override void InitializeAllUnitInfosExtend(PlayerDeck[] playerDeck)
  {
    base.InitializeAllUnitInfosExtend(playerDeck);
    this.allUnitInfos.Where<UnitIconInfo>((Func<UnitIconInfo, bool>) (x => x.unit.IsMaterialUnit)).ForEach<UnitIconInfo>((Action<UnitIconInfo>) (x => x.isTrustMaterial = x.unit.IsTrustMaterial(this.baseUnit)));
  }

  protected override bool IsSelectEx(UnitIconBase unitIconBase)
  {
    UnitIconInfo unitInfoAll = this.GetUnitInfoAll(unitIconBase.PlayerUnit);
    return (!this.SelectedUnitIsMax() || this.selectedUnitIcons.Contains(unitInfoAll)) && !(unitIconBase as UnitDetailIcon).unit.IsNormalUnit;
  }

  protected override void SelectEx(UnitIconBase unitIconBase, UnitIconInfo unitIconInfo)
  {
    this.currentUnitInfo = unitIconInfo;
    this.UpdateInfomation();
    this.singleCancelUpdateInfomation = true;
    this.StartCoroutine(this.ShowUnit00468CombinePopup(unitIconBase as UnitDetailIcon, unitIconInfo));
  }

  private IEnumerator ShowUnit00468CombinePopup(
    UnitDetailIcon unitDetailIcon,
    UnitIconInfo unitIconInfo)
  {
    Unit00486Menu menu = this;
    Future<GameObject> f = new ResourceObject("Prefabs/Popup_Common/popup_BundleIntegration_Base").Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(f.Result).GetComponent<Unit00486CombinePopup>().Init(unitDetailIcon, unitIconInfo, menu);
  }

  protected virtual void returnScene(
    List<UnitIconInfo> list,
    PlayerUnit _basePlayerUnit,
    bool isEnter)
  {
    if (!isEnter)
    {
      this.backScene();
    }
    else
    {
      List<PlayerUnit> playerUnitList = new List<PlayerUnit>();
      foreach (UnitIconInfo unitIconInfo in list)
      {
        if (!unitIconInfo.playerUnit.favorite && (!unitIconInfo.playerUnit.unit.IsNormalUnit || OverkillersUtil.checkDelete(unitIconInfo.playerUnit)))
        {
          unitIconInfo.SelectedCount = unitIconInfo.tempSelectedCount;
          unitIconInfo.isTempSelectedCount = false;
          unitIconInfo.tempSelectedCount = 0;
          playerUnitList.Add(unitIconInfo.playerUnit);
        }
      }
      Singleton<NGSceneManager>.GetInstance().clearStack(Unit004TrainingScene.DefSceneName);
      Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
      if (this.exceptionBackScene == null)
        Unit004TrainingScene.changeCombine(false, _basePlayerUnit, playerUnitList.ToArray());
      else
        Unit004TrainingScene.changeCombine(false, _basePlayerUnit, playerUnitList.ToArray(), exceptionBackScene: this.exceptionBackScene);
    }
  }

  public override void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().closeAll();
    this.returnScene(this.firstSelectedUnitIcons, this.baseUnit, false);
  }

  public virtual void IbtnClear()
  {
    this.IbtnClearS();
    this.allUnitInfos.ForEach((Action<UnitIconInfo>) (x => x.tempSelectedCount = 0));
    this.displayUnitInfos.ForEach((Action<UnitIconInfo>) (x => x.tempSelectedCount = 0));
    this.firstSelectedUnitIcons.Clear();
  }

  public virtual void IbtnEnter()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.doReturnScene());
  }

  private IEnumerator doReturnScene()
  {
    Unit00486Menu unit00486Menu = this;
    bool flag1 = false;
    int sameCharacterId1 = unit00486Menu.baseUnit.unit.same_character_id;
    foreach (UnitIconInfo selectedUnitIcon in unit00486Menu.selectedUnitIcons)
    {
      UnitUnit unit;
      if ((unit = selectedUnitIcon.playerUnit.unit).same_character_id == sameCharacterId1 && (unit.exist_overkillers_slot || selectedUnitIcon.playerUnit.hasOverkillersState))
      {
        flag1 = true;
        break;
      }
    }
    if (flag1)
    {
      int nCommand = 0;
      Consts instance = Consts.GetInstance();
      PopupCommonNoYes.Show(instance.POPUP_ALERT_TITLE_COMBINE_OVERKILLERS_SLOTS, instance.POPUP_ALERT_MESSAGE_COMINE_OVERKILLERS_SLOTS, (Action) (() => nCommand = 1), (Action) (() => nCommand = -1));
      while (nCommand == 0)
        yield return (object) null;
      if (nCommand < 0)
        yield break;
    }
    bool flag2 = false;
    foreach (UnitIconInfo selectedUnitIcon in unit00486Menu.selectedUnitIcons)
    {
      PlayerUnit playerUnit = selectedUnitIcon.playerUnit;
      if (playerUnit.all_saved_job_abilities != null && playerUnit.all_saved_job_abilities.Length != 0 || playerUnit.job_abilities != null && playerUnit.job_abilities.Length != 0)
      {
        flag2 = true;
        break;
      }
    }
    if (flag2)
    {
      int nCommand = 0;
      Consts instance = Consts.GetInstance();
      PopupCommonNoYes.Show(instance.POPUP_ALERT_TITLE_COMBINE_JOBCHANGE_UNITS, instance.POPUP_ALERT_MESSAGE_COMBINE_JOBCHANGE_UNITS, (Action) (() => nCommand = 1), (Action) (() => nCommand = -1), (NGUIText.Alignment) 1);
      while (nCommand == 0)
        yield return (object) null;
      if (nCommand < 0)
        yield break;
    }
    bool flag3 = false;
    int sameCharacterId2 = unit00486Menu.baseUnit.unit.same_character_id;
    if (PerformanceConfig.GetInstance().IsGear3 && !MasterData.GearExtensionExclusion.ContainsKey(sameCharacterId2))
    {
      foreach (UnitIconInfo selectedUnitIcon in unit00486Menu.selectedUnitIcons)
      {
        if (selectedUnitIcon.playerUnit.unit.same_character_id == sameCharacterId2 && selectedUnitIcon.playerUnit.unit.rarity.index + 1 >= 6)
        {
          flag3 = true;
          break;
        }
      }
    }
    if (flag3)
    {
      int nCommand = 0;
      Consts instance = Consts.GetInstance();
      PopupCommonNoYes.Show(instance.POPUP_ALERT_TITLE_COMBINE_BUGU3OPENED_SLOTS, instance.POPUP_ALERT_MESSAGE_COMINE_BUGU3OPENED_SLOTS, (Action) (() => nCommand = 1), (Action) (() => nCommand = -1));
      while (nCommand == 0)
        yield return (object) null;
      if (nCommand < 0)
        yield break;
    }
    int same_char = unit00486Menu.baseUnit.unit.same_character_id;
    Consts instance1 = Consts.GetInstance();
    UnitUnit unit1;
    UnitUnit unit2;
    UnitUnit unit3;
    UnitUnit unit4;
    (Func<PlayerUnit, bool>, (string, string))[] valueTupleArray = new (Func<PlayerUnit, bool>, (string, string))[4]
    {
      ((Func<PlayerUnit, bool>) (material => (unit1 = material.unit).same_character_id == same_char && unit1.awake_unit_flag), (instance1.POPUP_ALERT_TITLE_COMBINE_AWAKE_UNITS_11, instance1.POPUP_ALERT_MESSAGE_COMINE_AWAKE_UNITS_11)),
      ((Func<PlayerUnit, bool>) (material => (unit2 = material.unit).same_character_id == same_char && !unit2.awake_unit_flag && material.hasAwakeState), (instance1.POPUP_ALERT_TITLE_COMBINE_AWAKE_UNITS_10, instance1.POPUP_ALERT_MESSAGE_COMINE_AWAKE_UNITS_10)),
      ((Func<PlayerUnit, bool>) (material => (unit3 = material.unit).same_character_id != same_char && unit3.awake_unit_flag), (instance1.POPUP_ALERT_TITLE_COMBINE_AWAKE_UNITS_01, instance1.POPUP_ALERT_MESSAGE_COMINE_AWAKE_UNITS_01)),
      ((Func<PlayerUnit, bool>) (material => (unit4 = material.unit).same_character_id != same_char && !unit4.awake_unit_flag && material.hasAwakeState), (instance1.POPUP_ALERT_TITLE_COMBINE_AWAKE_UNITS_00, instance1.POPUP_ALERT_MESSAGE_COMINE_AWAKE_UNITS_00))
    };
    for (int index = 0; index < valueTupleArray.Length; ++index)
    {
      (Func<PlayerUnit, bool>, (string, string)) valueTuple = valueTupleArray[index];
      bool flag4 = false;
      foreach (UnitIconInfo selectedUnitIcon in unit00486Menu.selectedUnitIcons)
      {
        if (valueTuple.Item1(selectedUnitIcon.playerUnit))
        {
          flag4 = true;
          break;
        }
      }
      if (flag4)
      {
        int nCommand = 0;
        PopupCommonNoYes.Show(valueTuple.Item2.Item1, valueTuple.Item2.Item2, (Action) (() => nCommand = 1), (Action) (() => nCommand = -1));
        while (nCommand == 0)
          yield return (object) null;
        if (nCommand < 0)
          yield break;
      }
    }
    valueTupleArray = ((Func<PlayerUnit, bool>, (string, string))[]) null;
    unit00486Menu.returnScene(unit00486Menu.selectedUnitIcons, unit00486Menu.baseUnit, true);
  }

  public override void onBackButton() => this.IbtnBack();
}
