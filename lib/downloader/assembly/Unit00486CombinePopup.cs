// Decompiled with JetBrains decompiler
// Type: Unit00486CombinePopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Unit00486CombinePopup : BackButtonMenuBase
{
  private const int SELECT_NUM_MIN = 0;
  private const int SELECT_NUM_DEFALUT = 1;
  private const int CAN_SELECT_NUM_MAX = 99;
  [SerializeField]
  private Transform dyn_thum;
  [SerializeField]
  private UILabel unitName;
  [SerializeField]
  private UILabel unitFlavor;
  [SerializeField]
  private UILabel selectNumValue;
  [SerializeField]
  private UILabel zenyCostValue;
  [SerializeField]
  private UILabel selectNumMinValue;
  [SerializeField]
  private UILabel selectNumMaxValue;
  private int selectNumMax;
  [SerializeField]
  private UIButton ibtnMin;
  [SerializeField]
  private UIButton ibtnMax;
  [SerializeField]
  private UIButton ibtnPlus;
  [SerializeField]
  private UIButton ibtnMinus;
  [SerializeField]
  private UISlider selectSlider;
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
  private Unit00468Scene.Mode mode;
  private UnitDetailIcon unitDetailIcon;
  private UnitIconInfo unitIconInfo;
  private Unit00486Menu menu;
  private int selectedCount;
  private int beforeSelectNum;
  private const int UNITY_UNIT_CONVERSION = 100;

  public void Init(UnitDetailIcon unitDetailIcon, UnitIconInfo unitIconInfo, Unit00486Menu menu)
  {
    this.unitDetailIcon = unitDetailIcon;
    this.unitIconInfo = unitIconInfo;
    this.menu = menu;
    if (((object) menu).GetType() == typeof (Unit00486Menu))
      this.mode = Unit00468Scene.Mode.Unit0048;
    else if (((object) menu).GetType() == typeof (Unit00487Menu))
      this.mode = Unit00468Scene.Mode.Unit00420;
    this.SetUnitIcon(unitDetailIcon);
    this.unitName.text = unitDetailIcon.unit.name;
    this.unitFlavor.text = unitDetailIcon.unit.description;
    this.TxtBeforeNowHP.SetTextLocalize(menu.baseUnit.self_total_hp);
    this.TxtBeforeNowPower.SetTextLocalize(menu.baseUnit.self_total_strength);
    this.TxtBeforeNowMagic.SetTextLocalize(menu.baseUnit.self_total_intelligence);
    this.TxtBeforeNowProtect.SetTextLocalize(menu.baseUnit.self_total_vitality);
    this.TxtBeforeNowSprit.SetTextLocalize(menu.baseUnit.self_total_mind);
    this.TxtBeforeNowSpeed.SetTextLocalize(menu.baseUnit.self_total_agility);
    this.TxtBeforeNowTechnique.SetTextLocalize(menu.baseUnit.self_total_dexterity);
    this.TxtBeforeNowLucky.SetTextLocalize(menu.baseUnit.self_total_lucky);
    this.selectNumMax = this.CalcSelectMax();
    this.selectNumMinValue.text = 0.ToString();
    this.selectNumMaxValue.text = this.selectNumMax.ToString();
    this.beforeSelectNum = unitIconInfo.tempSelectedCount;
    this.selectedCount = this.beforeSelectNum != 0 || this.selectNumMax <= 0 ? this.beforeSelectNum : 1;
    this.ChangeCurrentValue();
  }

  private int CalcSelectMax()
  {
    int val1 = 0;
    if (this.mode == Unit00468Scene.Mode.Unit0048)
    {
      int paramUpSelectMax = this.GetConsumeParamUpSelectMax();
      int val2_1 = Math.Max(this.GetBreakThroughSelectMax(), paramUpSelectMax);
      int val2_2 = Math.Max(this.GetTrustSelectMax(), val2_1);
      int val2_3 = Math.Max(this.GetSkillUpLevelSelectMax(), val2_2);
      val1 = Math.Max(this.GetBuildupUnitySelectMax(), val2_3);
    }
    else if (this.mode == Unit00468Scene.Mode.Unit00420)
      val1 = this.GetBuildParamUpSelectMax();
    int val2 = Math.Min(val1, 99);
    int unit_id = this.unitIconInfo.unit.ID;
    return Math.Min(Array.Find<PlayerMaterialUnit>(SMManager.Get<PlayerMaterialUnit[]>(), (Predicate<PlayerMaterialUnit>) (x => x._unit == unit_id)).quantity, val2);
  }

  private Dictionary<CalcUnitCompose.ComposeType, int> GetOtherUpperParamDict()
  {
    Dictionary<CalcUnitCompose.ComposeType, int> otherUpperParamDict = new Dictionary<CalcUnitCompose.ComposeType, int>();
    foreach (CalcUnitCompose.ComposeType key in Enum.GetValues(typeof (CalcUnitCompose.ComposeType)))
      otherUpperParamDict.Add(key, 0);
    List<UnitIconInfo> source = new List<UnitIconInfo>();
    foreach (UnitIconInfo selectedUnitIcon in this.menu.SelectedUnitIcons)
    {
      if (selectedUnitIcon != this.unitIconInfo)
      {
        for (int index = 0; index < selectedUnitIcon.tempSelectedCount; ++index)
          source.Add(selectedUnitIcon);
      }
    }
    PlayerUnit[] array = source.Select<UnitIconInfo, PlayerUnit>((Func<UnitIconInfo, PlayerUnit>) (x => x.playerUnit)).ToArray<PlayerUnit>();
    foreach (UnitIconInfo selectedUnitIcon in this.menu.SelectedUnitIcons)
    {
      if (selectedUnitIcon != this.unitIconInfo)
      {
        foreach (CalcUnitCompose.ComposeType composeType in Enum.GetValues(typeof (CalcUnitCompose.ComposeType)))
        {
          switch (this.mode)
          {
            case Unit00468Scene.Mode.Unit0048:
              otherUpperParamDict[composeType] = CalcUnitCompose.getComposeValue(this.menu.baseUnit, array, composeType);
              continue;
            case Unit00468Scene.Mode.Unit00420:
              otherUpperParamDict[composeType] = CalcUnitCompose.getBuildupValue(this.menu.baseUnit, array, composeType);
              continue;
            default:
              continue;
          }
        }
      }
    }
    return otherUpperParamDict;
  }

  private Dictionary<CalcUnitCompose.ComposeType, int> GetOneUpperParamDict()
  {
    Dictionary<CalcUnitCompose.ComposeType, int> oneUpperParamDict = new Dictionary<CalcUnitCompose.ComposeType, int>();
    foreach (CalcUnitCompose.ComposeType key in Enum.GetValues(typeof (CalcUnitCompose.ComposeType)))
      oneUpperParamDict.Add(key, 0);
    PlayerUnit[] material = new PlayerUnit[1]
    {
      this.unitIconInfo.playerUnit
    };
    foreach (CalcUnitCompose.ComposeType composeType in Enum.GetValues(typeof (CalcUnitCompose.ComposeType)))
    {
      switch (this.mode)
      {
        case Unit00468Scene.Mode.Unit0048:
          oneUpperParamDict[composeType] = CalcUnitCompose.getComposeValue(this.menu.baseUnit, material, composeType);
          continue;
        case Unit00468Scene.Mode.Unit00420:
          oneUpperParamDict[composeType] = CalcUnitCompose.getBuildupValue(this.menu.baseUnit, material, composeType);
          continue;
        default:
          continue;
      }
    }
    return oneUpperParamDict;
  }

  private Dictionary<CalcUnitCompose.ComposeType, int> GetcurrentCombineUpDict()
  {
    Dictionary<CalcUnitCompose.ComposeType, int> dictionary = new Dictionary<CalcUnitCompose.ComposeType, int>();
    switch (this.mode)
    {
      case Unit00468Scene.Mode.Unit0048:
        dictionary.Add(CalcUnitCompose.ComposeType.HP, this.menu.baseUnit.hp.compose);
        dictionary.Add(CalcUnitCompose.ComposeType.STRENGTH, this.menu.baseUnit.strength.compose);
        dictionary.Add(CalcUnitCompose.ComposeType.INTELLIGENCE, this.menu.baseUnit.intelligence.compose);
        dictionary.Add(CalcUnitCompose.ComposeType.VITALITY, this.menu.baseUnit.vitality.compose);
        dictionary.Add(CalcUnitCompose.ComposeType.MIND, this.menu.baseUnit.mind.compose);
        dictionary.Add(CalcUnitCompose.ComposeType.AGILITY, this.menu.baseUnit.agility.compose);
        dictionary.Add(CalcUnitCompose.ComposeType.DEXTERITY, this.menu.baseUnit.dexterity.compose);
        dictionary.Add(CalcUnitCompose.ComposeType.LUCKY, this.menu.baseUnit.lucky.compose);
        break;
      case Unit00468Scene.Mode.Unit00420:
        dictionary.Add(CalcUnitCompose.ComposeType.HP, this.menu.baseUnit.hp.buildup);
        dictionary.Add(CalcUnitCompose.ComposeType.STRENGTH, this.menu.baseUnit.strength.buildup);
        dictionary.Add(CalcUnitCompose.ComposeType.INTELLIGENCE, this.menu.baseUnit.intelligence.buildup);
        dictionary.Add(CalcUnitCompose.ComposeType.VITALITY, this.menu.baseUnit.vitality.buildup);
        dictionary.Add(CalcUnitCompose.ComposeType.MIND, this.menu.baseUnit.mind.buildup);
        dictionary.Add(CalcUnitCompose.ComposeType.AGILITY, this.menu.baseUnit.agility.buildup);
        dictionary.Add(CalcUnitCompose.ComposeType.DEXTERITY, this.menu.baseUnit.dexterity.buildup);
        dictionary.Add(CalcUnitCompose.ComposeType.LUCKY, this.menu.baseUnit.lucky.buildup);
        break;
    }
    return dictionary;
  }

  private Dictionary<CalcUnitCompose.ComposeType, int> GetComposeMaxDict()
  {
    Dictionary<CalcUnitCompose.ComposeType, int> composeMaxDict = new Dictionary<CalcUnitCompose.ComposeType, int>();
    switch (this.mode)
    {
      case Unit00468Scene.Mode.Unit0048:
        composeMaxDict.Add(CalcUnitCompose.ComposeType.HP, this.menu.baseUnit.compose_hp_max);
        composeMaxDict.Add(CalcUnitCompose.ComposeType.STRENGTH, this.menu.baseUnit.compose_strength_max);
        composeMaxDict.Add(CalcUnitCompose.ComposeType.INTELLIGENCE, this.menu.baseUnit.compose_intelligence_max);
        composeMaxDict.Add(CalcUnitCompose.ComposeType.VITALITY, this.menu.baseUnit.compose_vitality_max);
        composeMaxDict.Add(CalcUnitCompose.ComposeType.MIND, this.menu.baseUnit.compose_mind_max);
        composeMaxDict.Add(CalcUnitCompose.ComposeType.AGILITY, this.menu.baseUnit.compose_agility_max);
        composeMaxDict.Add(CalcUnitCompose.ComposeType.DEXTERITY, this.menu.baseUnit.compose_dexterity_max);
        composeMaxDict.Add(CalcUnitCompose.ComposeType.LUCKY, this.menu.baseUnit.compose_lucky_max);
        break;
      case Unit00468Scene.Mode.Unit00420:
        composeMaxDict.Add(CalcUnitCompose.ComposeType.HP, this.menu.baseUnit.unit.buildup_limit_release_id.hp_limit_release_cnt);
        composeMaxDict.Add(CalcUnitCompose.ComposeType.STRENGTH, this.menu.baseUnit.unit.buildup_limit_release_id.strength_limit_release_cnt);
        composeMaxDict.Add(CalcUnitCompose.ComposeType.INTELLIGENCE, this.menu.baseUnit.unit.buildup_limit_release_id.intelligence_limit_release_cnt);
        composeMaxDict.Add(CalcUnitCompose.ComposeType.VITALITY, this.menu.baseUnit.unit.buildup_limit_release_id.vitality_limit_release_cnt);
        composeMaxDict.Add(CalcUnitCompose.ComposeType.MIND, this.menu.baseUnit.unit.buildup_limit_release_id.mind_limit_release_cnt);
        composeMaxDict.Add(CalcUnitCompose.ComposeType.AGILITY, this.menu.baseUnit.unit.buildup_limit_release_id.agility_limit_release_cnt);
        composeMaxDict.Add(CalcUnitCompose.ComposeType.DEXTERITY, this.menu.baseUnit.unit.buildup_limit_release_id.dexterity_limit_release_cnt);
        composeMaxDict.Add(CalcUnitCompose.ComposeType.LUCKY, this.menu.baseUnit.unit.buildup_limit_release_id.lucky_limit_release_cnt);
        break;
    }
    return composeMaxDict;
  }

  private int GetConsumeParamUpSelectMax()
  {
    if (this.unitIconInfo.playerUnit.unit.is_consume_only != 1)
      return 0;
    int val1 = 0;
    Dictionary<CalcUnitCompose.ComposeType, int> otherUpperParamDict = this.GetOtherUpperParamDict();
    Dictionary<CalcUnitCompose.ComposeType, int> oneUpperParamDict = this.GetOneUpperParamDict();
    Dictionary<CalcUnitCompose.ComposeType, int> dictionary = this.GetcurrentCombineUpDict();
    Dictionary<CalcUnitCompose.ComposeType, int> composeMaxDict = this.GetComposeMaxDict();
    foreach (CalcUnitCompose.ComposeType key in Enum.GetValues(typeof (CalcUnitCompose.ComposeType)))
    {
      if (oneUpperParamDict[key] > 0)
      {
        int num = composeMaxDict[key] - (dictionary[key] + otherUpperParamDict[key]);
        if (num < 0)
          num = 0;
        int val2 = (int) Math.Ceiling((double) num / (double) oneUpperParamDict[key]);
        val1 = Math.Max(val1, val2);
      }
    }
    return val1;
  }

  private int GetBuildParamUpSelectMax()
  {
    if (this.unitIconInfo.playerUnit.unit.is_buildup_only != 1)
      return 0;
    int val2_1 = 0;
    Dictionary<CalcUnitCompose.ComposeType, int> otherUpperParamDict = this.GetOtherUpperParamDict();
    Dictionary<CalcUnitCompose.ComposeType, int> oneUpperParamDict = this.GetOneUpperParamDict();
    Dictionary<CalcUnitCompose.ComposeType, int> dictionary = this.GetcurrentCombineUpDict();
    Dictionary<CalcUnitCompose.ComposeType, int> composeMaxDict = this.GetComposeMaxDict();
    foreach (CalcUnitCompose.ComposeType key in Enum.GetValues(typeof (CalcUnitCompose.ComposeType)))
    {
      if (oneUpperParamDict[key] > 0)
      {
        int num = composeMaxDict[key] - (dictionary[key] + otherUpperParamDict[key]);
        if (num < 0)
          num = 0;
        val2_1 = Math.Max((int) Math.Ceiling((double) num / (double) oneUpperParamDict[key]), val2_1);
      }
    }
    int num1 = 0;
    foreach (UnitIconInfo selectedUnitIcon in this.menu.SelectedUnitIcons)
    {
      if (selectedUnitIcon != this.unitIconInfo)
        num1 += selectedUnitIcon.tempSelectedCount;
    }
    int val2_2 = Math.Min(this.menu.baseUnit.buildup_limit - (num1 + this.menu.baseUnit.buildup_count), val2_1);
    int num2 = 0;
    if (oneUpperParamDict[CalcUnitCompose.ComposeType.HP] > 0)
    {
      int num3 = this.menu.baseUnit.hp.level_up_max_status - (this.menu.baseUnit.hp.level + this.menu.baseUnit.hp.buildup + otherUpperParamDict[CalcUnitCompose.ComposeType.HP]);
      if (num3 < 0)
        num3 = 0;
      num2 = Math.Max((int) Math.Ceiling((double) num3 / (double) oneUpperParamDict[CalcUnitCompose.ComposeType.HP]), num2);
    }
    if (oneUpperParamDict[CalcUnitCompose.ComposeType.STRENGTH] > 0)
    {
      int num4 = this.menu.baseUnit.strength.level_up_max_status - (this.menu.baseUnit.strength.level + this.menu.baseUnit.strength.buildup + otherUpperParamDict[CalcUnitCompose.ComposeType.STRENGTH]);
      if (num4 < 0)
        num4 = 0;
      num2 = Math.Max((int) Math.Ceiling((double) num4 / (double) oneUpperParamDict[CalcUnitCompose.ComposeType.STRENGTH]), num2);
    }
    if (oneUpperParamDict[CalcUnitCompose.ComposeType.INTELLIGENCE] > 0)
    {
      int num5 = this.menu.baseUnit.intelligence.level_up_max_status - (this.menu.baseUnit.intelligence.level + this.menu.baseUnit.intelligence.buildup + otherUpperParamDict[CalcUnitCompose.ComposeType.INTELLIGENCE]);
      if (num5 < 0)
        num5 = 0;
      num2 = Math.Max((int) Math.Ceiling((double) num5 / (double) oneUpperParamDict[CalcUnitCompose.ComposeType.INTELLIGENCE]), num2);
    }
    if (oneUpperParamDict[CalcUnitCompose.ComposeType.VITALITY] > 0)
    {
      int num6 = this.menu.baseUnit.vitality.level_up_max_status - (this.menu.baseUnit.vitality.level + this.menu.baseUnit.vitality.buildup + otherUpperParamDict[CalcUnitCompose.ComposeType.VITALITY]);
      if (num6 < 0)
        num6 = 0;
      num2 = Math.Max((int) Math.Ceiling((double) num6 / (double) oneUpperParamDict[CalcUnitCompose.ComposeType.VITALITY]), num2);
    }
    if (oneUpperParamDict[CalcUnitCompose.ComposeType.MIND] > 0)
    {
      int num7 = this.menu.baseUnit.mind.level_up_max_status - (this.menu.baseUnit.mind.level + this.menu.baseUnit.mind.buildup + otherUpperParamDict[CalcUnitCompose.ComposeType.MIND]);
      if (num7 < 0)
        num7 = 0;
      num2 = Math.Max((int) Math.Ceiling((double) num7 / (double) oneUpperParamDict[CalcUnitCompose.ComposeType.MIND]), num2);
    }
    if (oneUpperParamDict[CalcUnitCompose.ComposeType.AGILITY] > 0)
    {
      int num8 = this.menu.baseUnit.agility.level_up_max_status - (this.menu.baseUnit.agility.level + this.menu.baseUnit.agility.buildup + otherUpperParamDict[CalcUnitCompose.ComposeType.AGILITY]);
      if (num8 < 0)
        num8 = 0;
      num2 = Math.Max((int) Math.Ceiling((double) num8 / (double) oneUpperParamDict[CalcUnitCompose.ComposeType.AGILITY]), num2);
    }
    if (oneUpperParamDict[CalcUnitCompose.ComposeType.DEXTERITY] > 0)
    {
      int num9 = this.menu.baseUnit.dexterity.level_up_max_status - (this.menu.baseUnit.dexterity.level + this.menu.baseUnit.dexterity.buildup + otherUpperParamDict[CalcUnitCompose.ComposeType.DEXTERITY]);
      if (num9 < 0)
        num9 = 0;
      num2 = Math.Max((int) Math.Ceiling((double) num9 / (double) oneUpperParamDict[CalcUnitCompose.ComposeType.DEXTERITY]), num2);
    }
    if (oneUpperParamDict[CalcUnitCompose.ComposeType.LUCKY] > 0)
    {
      int num10 = this.menu.baseUnit.lucky.level_up_max_status - (this.menu.baseUnit.lucky.level + this.menu.baseUnit.lucky.buildup + otherUpperParamDict[CalcUnitCompose.ComposeType.LUCKY]);
      if (num10 < 0)
        num10 = 0;
      num2 = Math.Max((int) Math.Ceiling((double) num10 / (double) oneUpperParamDict[CalcUnitCompose.ComposeType.LUCKY]), num2);
    }
    return Math.Min(num2, val2_2);
  }

  private int GetBreakThroughSelectMax()
  {
    if (!this.unitIconInfo.playerUnit.unit.IsBreakThrough)
      return 0;
    PlayerUnit baseUnit = this.menu.baseUnit;
    UnitUnit unit1 = baseUnit.unit;
    int num1 = 0;
    foreach (UnitIconInfo selectedUnitIcon in this.menu.SelectedUnitIcons)
    {
      if (selectedUnitIcon != this.unitIconInfo)
      {
        PlayerUnit playerUnit = selectedUnitIcon.playerUnit;
        UnitUnit unit2 = playerUnit.unit;
        if (unit2.same_character_id == unit1.same_character_id || unit2.IsBreakThrough)
        {
          if (unit2.IsBreakThrough)
            num1 += selectedUnitIcon.tempSelectedCount;
          else if (unit2.rarity.index >= unit1.rarity.index)
            num1 += playerUnit.breakthrough_count + 1;
          else
            num1 += playerUnit.unity_value + 1;
        }
      }
    }
    int num2 = baseUnit.breakthrough_count + num1;
    int throughSelectMax = unit1.breakthrough_limit - num2;
    if (throughSelectMax < 0)
      throughSelectMax = 0;
    return throughSelectMax;
  }

  private int GetTrustSelectMax()
  {
    if (!this.unitIconInfo.playerUnit.unit.IsTrustMaterial(this.menu.baseUnit))
      return 0;
    float num1 = 0.0f;
    float trustComposeRate = (float) PlayerUnit.GetTrustComposeRate();
    PlayerUnit.GetTrustRateMax();
    foreach (UnitIconInfo selectedUnitIcon in this.menu.SelectedUnitIcons)
    {
      if (selectedUnitIcon != this.unitIconInfo)
      {
        PlayerUnit playerUnit = selectedUnitIcon.playerUnit;
        if (playerUnit.unit.IsTrustMaterial(this.menu.baseUnit))
        {
          num1 += playerUnit.unit.TrustMaterialUnit(this.menu.baseUnit).increase_value * (float) selectedUnitIcon.tempSelectedCount;
          num1 += playerUnit.trust_rate;
        }
        else if (this.menu.baseUnit.unit.same_character_id == playerUnit.unit.same_character_id)
        {
          num1 += trustComposeRate;
          num1 += playerUnit.trust_rate;
        }
        else if (this.menu.baseUnit.unit.character.ID == playerUnit.unit.character.ID)
          num1 += playerUnit.unit.rarity.trust_rate * (float) (playerUnit.unity_value + 1);
      }
    }
    float num2 = this.menu.currentMaxTrust - num1 - this.menu.baseUnit.trust_rate;
    int trustSelectMax = 0;
    if ((double) num2 > 0.0)
      trustSelectMax = Mathf.CeilToInt(num2 / this.unitIconInfo.unit.TrustMaterialUnit(this.menu.baseUnit).increase_value);
    return trustSelectMax;
  }

  private int GetSkillUpLevelSelectMax()
  {
    if (!this.unitIconInfo.playerUnit.unit.IsMaterialUnitSkillUp)
      return 0;
    int val1 = 0;
    Dictionary<BattleskillSkillType, int> dictionary1 = new Dictionary<BattleskillSkillType, int>();
    Dictionary<BattleskillSkillType, bool> dictionary2 = new Dictionary<BattleskillSkillType, bool>();
    foreach (BattleskillSkillType key in Enum.GetValues(typeof (BattleskillSkillType)))
    {
      dictionary1.Add(key, 0);
      dictionary2.Add(key, false);
    }
    foreach (UnitIconInfo selectedUnitIcon in this.menu.SelectedUnitIcons)
    {
      if (selectedUnitIcon != this.unitIconInfo && selectedUnitIcon.playerUnit.unit.IsMaterialUnitSkillUp)
      {
        if (selectedUnitIcon.playerUnit.unit.skillup_type == 99)
        {
          foreach (BattleskillSkillType battleskillSkillType in Enum.GetValues(typeof (BattleskillSkillType)))
          {
            int key = (int) battleskillSkillType;
            dictionary1[(BattleskillSkillType) key] += selectedUnitIcon.tempSelectedCount;
          }
        }
        else
        {
          int skillupType = selectedUnitIcon.playerUnit.unit.skillup_type;
          dictionary1[(BattleskillSkillType) skillupType] += selectedUnitIcon.tempSelectedCount;
        }
      }
    }
    if (this.unitIconInfo.playerUnit.unit.skillup_type == 99)
    {
      foreach (BattleskillSkillType key in Enum.GetValues(typeof (BattleskillSkillType)))
        dictionary2[key] = true;
    }
    else
    {
      BattleskillSkillType skillupType = (BattleskillSkillType) this.unitIconInfo.playerUnit.unit.skillup_type;
      dictionary2[skillupType] = true;
    }
    foreach (PlayerUnitSkills skill in this.menu.baseUnit.skills)
    {
      foreach (BattleskillSkillType key in Enum.GetValues(typeof (BattleskillSkillType)))
      {
        if (dictionary2[key] && skill.skill.skill_type == key)
        {
          int num = skill.level + dictionary1[key];
          int val2 = skill.skill.upper_level - num;
          if (val2 < 0)
            val2 = 0;
          val1 = Math.Max(val1, val2);
          break;
        }
      }
    }
    return val1;
  }

  private int GetBuildupUnitySelectMax()
  {
    UnitUnit unit1 = this.unitIconInfo.playerUnit.unit;
    if (!unit1.is_unity_value_up)
      return 0;
    UnitUnit unit2 = this.menu.baseUnit.unit;
    UnityValueUpPattern valueUpPattern = unit1.FindValueUpPattern(unit2, (Func<UnitFamily[]>) (() => this.menu.baseUnit.Families));
    UnityPureValueUpPattern pureValueUpPattern = (UnityPureValueUpPattern) null;
    if (valueUpPattern == null)
    {
      pureValueUpPattern = unit1.FindPureValueUpPattern(unit2);
      if (pureValueUpPattern == null)
        return 0;
    }
    int num1 = PlayerUnit.GetUnityValueMax() * 100;
    int num2 = 0;
    if (valueUpPattern != null)
      num2 = Mathf.CeilToInt(this.menu.selectedUnityValue * 100f) + Mathf.CeilToInt(this.menu.selectedBuildupUnityValue * 100f) + Mathf.CeilToInt(this.menu.baseUnit.unityTotal * 100f);
    else if (pureValueUpPattern != null)
      num2 = Mathf.CeilToInt(this.menu.selectedUnityValue * 100f) + Mathf.CeilToInt((float) (this.menu.baseUnit.unity_value * 100));
    int num3 = Mathf.Min(num2, num1);
    int num4 = num1 - num3;
    int num5 = 0;
    if (valueUpPattern != null)
      num5 = Mathf.CeilToInt(valueUpPattern.up_value * 100f);
    else if (pureValueUpPattern != null)
      num5 = Mathf.CeilToInt(100f);
    int num6 = num5;
    return (num4 + num6 - 1) / num5;
  }

  public void sliderChange()
  {
    int num = Mathf.RoundToInt(((UIProgressBar) this.selectSlider).value * (float) this.selectNumMax);
    if (num < 0)
      num = 0;
    this.selectedCount = num;
    this.ChangeCurrentValue(false);
  }

  private void SetUnitIcon(UnitDetailIcon script)
  {
    UnitDetailIcon component1 = ((Component) script).gameObject.Clone(this.dyn_thum).GetComponent<UnitDetailIcon>();
    ((Behaviour) component1.Button).enabled = false;
    if (script.UnitIcon.EnabledExpireDate)
    {
      component1.UnitIcon.PlayerUnit = script.PlayerUnit;
      component1.UnitIcon.EnabledExpireDate = true;
      component1.UnitIcon.ShowBottomInfo(this.menu.CurrentSortType);
      component1.UnitIcon.clearBottomBlinks();
    }
    foreach (Component component2 in component1.unitIconParent.transform)
      ((Behaviour) component2.GetComponent<UnitIcon>().Button).enabled = false;
    component1.Dir_select_check.SetActive(false);
    NGTween.playTweens(((Component) component1).GetComponentsInChildren<UITweener>(true), NGTween.Kind.GRAYOUT, true);
  }

  private void SetUnitUpParamter(
    PlayerUnit baseUnit,
    PlayerUnit[] materialUnits,
    Action<UILabel, int, int, int> SetUpperParamterLabel)
  {
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    int num4 = 0;
    int num5 = 0;
    int num6 = 0;
    int num7 = 0;
    int num8 = 0;
    switch (this.mode)
    {
      case Unit00468Scene.Mode.Unit0048:
        int composeValue1 = CalcUnitCompose.getComposeValue(baseUnit, materialUnits, CalcUnitCompose.ComposeType.HP);
        int composeValue2 = CalcUnitCompose.getComposeValue(baseUnit, materialUnits, CalcUnitCompose.ComposeType.STRENGTH);
        int composeValue3 = CalcUnitCompose.getComposeValue(baseUnit, materialUnits, CalcUnitCompose.ComposeType.INTELLIGENCE);
        int composeValue4 = CalcUnitCompose.getComposeValue(baseUnit, materialUnits, CalcUnitCompose.ComposeType.VITALITY);
        int composeValue5 = CalcUnitCompose.getComposeValue(baseUnit, materialUnits, CalcUnitCompose.ComposeType.MIND);
        int composeValue6 = CalcUnitCompose.getComposeValue(baseUnit, materialUnits, CalcUnitCompose.ComposeType.AGILITY);
        int composeValue7 = CalcUnitCompose.getComposeValue(baseUnit, materialUnits, CalcUnitCompose.ComposeType.DEXTERITY);
        int composeValue8 = CalcUnitCompose.getComposeValue(baseUnit, materialUnits, CalcUnitCompose.ComposeType.LUCKY);
        SetUpperParamterLabel(this.TxtIncHP, baseUnit.hp.compose, baseUnit.compose_hp_max, composeValue1);
        SetUpperParamterLabel(this.TxtIncPower, baseUnit.strength.compose, baseUnit.compose_strength_max, composeValue2);
        SetUpperParamterLabel(this.TxtIncMagic, baseUnit.intelligence.compose, baseUnit.compose_intelligence_max, composeValue3);
        SetUpperParamterLabel(this.TxtIncProtect, baseUnit.vitality.compose, baseUnit.compose_vitality_max, composeValue4);
        SetUpperParamterLabel(this.TxtIncSprit, baseUnit.mind.compose, baseUnit.compose_mind_max, composeValue5);
        SetUpperParamterLabel(this.TxtIncSpeed, baseUnit.agility.compose, baseUnit.compose_agility_max, composeValue6);
        SetUpperParamterLabel(this.TxtIncTechnique, baseUnit.dexterity.compose, baseUnit.compose_dexterity_max, composeValue7);
        SetUpperParamterLabel(this.TxtIncLucky, baseUnit.lucky.compose, baseUnit.compose_lucky_max, composeValue8);
        break;
      case Unit00468Scene.Mode.Unit00420:
        foreach (PlayerUnit materialUnit in materialUnits)
        {
          num1 += materialUnit.unit.hp_buildup;
          num2 += materialUnit.unit.strength_buildup;
          num3 += materialUnit.unit.intelligence_buildup;
          num4 += materialUnit.unit.vitality_buildup;
          num5 += materialUnit.unit.mind_buildup;
          num6 += materialUnit.unit.agility_buildup;
          num7 += materialUnit.unit.dexterity_buildup;
          num8 += materialUnit.unit.lucky_buildup;
        }
        SetUpperParamterLabel(this.TxtIncHP, baseUnit.hp.buildup, baseUnit.hp.possibleBuildupCnt(baseUnit), num1);
        SetUpperParamterLabel(this.TxtIncPower, baseUnit.strength.buildup, baseUnit.strength.possibleBuildupCnt(baseUnit), num2);
        SetUpperParamterLabel(this.TxtIncMagic, baseUnit.intelligence.buildup, baseUnit.intelligence.possibleBuildupCnt(baseUnit), num3);
        SetUpperParamterLabel(this.TxtIncProtect, baseUnit.vitality.buildup, baseUnit.vitality.possibleBuildupCnt(baseUnit), num4);
        SetUpperParamterLabel(this.TxtIncSprit, baseUnit.mind.buildup, baseUnit.mind.possibleBuildupCnt(baseUnit), num5);
        SetUpperParamterLabel(this.TxtIncSpeed, baseUnit.agility.buildup, baseUnit.agility.possibleBuildupCnt(baseUnit), num6);
        SetUpperParamterLabel(this.TxtIncTechnique, baseUnit.dexterity.buildup, baseUnit.dexterity.possibleBuildupCnt(baseUnit), num7);
        SetUpperParamterLabel(this.TxtIncLucky, baseUnit.lucky.buildup, baseUnit.lucky.possibleBuildupCnt(baseUnit), num8);
        break;
    }
  }

  public void OnMinus()
  {
    this.selectedCount = Math.Max(0, this.selectedCount - 1);
    this.ChangeCurrentValue();
  }

  public void OnPlus()
  {
    this.selectedCount = Math.Min(this.selectNumMax, this.selectedCount + 1);
    this.ChangeCurrentValue();
  }

  public void OnResetMin()
  {
    this.selectedCount = 0;
    this.ChangeCurrentValue();
  }

  public void OnResetMax()
  {
    this.selectedCount = this.selectNumMax;
    this.ChangeCurrentValue();
  }

  private void ChangeCurrentValue(bool isSliderValue = true)
  {
    if (isSliderValue)
    {
      if (this.selectNumMax == 0)
        ((UIProgressBar) this.selectSlider).value = 0.0f;
      else
        ((UIProgressBar) this.selectSlider).value = (float) this.selectedCount / (float) this.selectNumMax;
    }
    List<PlayerUnit> playerUnitList = new List<PlayerUnit>();
    foreach (UnitIconInfo selectedUnitIcon in this.menu.SelectedUnitIcons)
    {
      if (selectedUnitIcon != this.unitIconInfo)
      {
        for (int index = 0; index < selectedUnitIcon.tempSelectedCount; ++index)
          playerUnitList.Add(selectedUnitIcon.playerUnit);
      }
    }
    for (int index = 0; index < this.selectedCount; ++index)
      playerUnitList.Add(this.unitIconInfo.playerUnit);
    this.SetUnitUpParamter(this.menu.baseUnit, playerUnitList.ToArray(), new Action<UILabel, int, int, int>(this.menu.SetUpperParamterLabel));
    long num = 0;
    switch (this.mode)
    {
      case Unit00468Scene.Mode.Unit0048:
        num = CalcUnitCompose.priceCompose(this.menu.baseUnit, playerUnitList.ToArray());
        break;
      case Unit00468Scene.Mode.Unit00420:
        num = CalcUnitCompose.priceStringth(this.menu.baseUnit, playerUnitList.ToArray());
        break;
    }
    this.zenyCostValue.SetTextLocalize(num.ToString());
    if (num > this.menu.Player.money)
      ((UIWidget) this.zenyCostValue).color = Color.red;
    else
      ((UIWidget) this.zenyCostValue).color = Color.white;
    this.selectNumValue.text = this.selectedCount.ToString();
    if (this.selectedCount <= 0)
    {
      ((UIButtonColor) this.ibtnMin).isEnabled = false;
      ((UIButtonColor) this.ibtnMinus).isEnabled = false;
    }
    else
    {
      ((UIButtonColor) this.ibtnMin).isEnabled = true;
      ((UIButtonColor) this.ibtnMinus).isEnabled = true;
    }
    if (this.selectedCount >= this.selectNumMax)
    {
      ((UIButtonColor) this.ibtnMax).isEnabled = false;
      ((UIButtonColor) this.ibtnPlus).isEnabled = false;
    }
    else
    {
      ((UIButtonColor) this.ibtnMax).isEnabled = true;
      ((UIButtonColor) this.ibtnPlus).isEnabled = true;
    }
  }

  public void OnOK()
  {
    if (this.IsPushAndSet())
      return;
    if (this.selectedCount <= 0)
    {
      this.unitIconInfo.isTempSelectedCount = true;
      this.unitIconInfo.tempSelectedCount = this.selectedCount;
      this.menu.UnSelect((UnitIconBase) this.unitDetailIcon);
    }
    else if (this.selectedCount != this.beforeSelectNum)
    {
      this.unitIconInfo.isTempSelectedCount = true;
      this.unitIconInfo.tempSelectedCount = this.selectedCount;
      this.menu.OnSelect((UnitIconBase) this.unitDetailIcon);
    }
    this.menu.resetCurrentUnitInfo();
    this.menu.UpdateInfomation();
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnRetrun();

  private void IbtnRetrun()
  {
    if (this.IsPushAndSet())
      return;
    this.menu.resetCurrentUnitInfo();
    this.menu.UpdateInfomation();
    Singleton<PopupManager>.GetInstance().onDismiss();
  }
}
