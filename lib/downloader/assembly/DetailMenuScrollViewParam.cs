// Decompiled with JetBrains decompiler
// Type: DetailMenuScrollViewParam
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
using UnitDetails;
using UnitStatusInformation;
using UnityEngine;

#nullable disable
public class DetailMenuScrollViewParam : DetailMenuScrollViewBase
{
  [SerializeField]
  protected GameObject unitySkillObject;
  [SerializeField]
  protected UI2DSprite unitySKill;
  [SerializeField]
  protected UIButton unitySKillBtn;
  [SerializeField]
  protected GameObject extraSkillObject;
  [SerializeField]
  protected UI2DSprite extraSKill;
  [SerializeField]
  protected UIButton extraSKillBtn;
  [SerializeField]
  protected UILabel txtDearDegree;
  [SerializeField]
  protected UILabel txtDearDegreeMax;
  [SerializeField]
  protected UILabel txtDearDegreeTitle;
  [SerializeField]
  protected GameObject slc_DearDegreeTitle;
  [SerializeField]
  protected GameObject floatingSkillDialog;
  [SerializeField]
  protected NGTweenGaugeScale lvGauge;
  [SerializeField]
  protected GameObject[] slc_LBicon_None;
  [SerializeField]
  protected GameObject[] slc_LBicon_Blue;
  private int dispStatusCurrent;
  [SerializeField]
  protected GameObject[] dir_statusNum;
  [SerializeField]
  protected GameObject[] dir_statusListText;
  [SerializeField]
  protected UILabel txt_Agility;
  [SerializeField]
  protected UILabel txt_Luck;
  [SerializeField]
  protected UILabel txt_Magic;
  [SerializeField]
  protected UILabel txt_Power;
  [SerializeField]
  protected UILabel txt_Protct;
  [SerializeField]
  protected UILabel txt_Spirit;
  [SerializeField]
  protected UILabel txt_Technique;
  [SerializeField]
  protected GameObject slc_HpMaxStar;
  [SerializeField]
  protected GameObject slc_AgilityMaxStar;
  [SerializeField]
  protected GameObject slc_LuckMaxStar;
  [SerializeField]
  protected GameObject slc_MagicMaxStar;
  [SerializeField]
  protected GameObject slc_PowerMaxStar;
  [SerializeField]
  protected GameObject slc_ProtctMaxStar;
  [SerializeField]
  protected GameObject slc_SpiritMaxStar;
  [SerializeField]
  protected GameObject slc_TechniqueMaxStar;
  [SerializeField]
  protected UILabel txt_Attack;
  [SerializeField]
  protected UILabel txt_Cost;
  [SerializeField]
  protected UILabel txt_Critical;
  [SerializeField]
  protected UILabel txt_Defense;
  [SerializeField]
  protected UILabel txt_Dexterity;
  [SerializeField]
  protected UILabel txt_Evasion;
  [SerializeField]
  protected UILabel txt_Fighting;
  [SerializeField]
  protected UILabel txt_Matk;
  [SerializeField]
  protected UILabel txt_Mdef;
  [SerializeField]
  protected UILabel txt_Movement;
  [SerializeField]
  protected GameObject dir_Hp;
  [SerializeField]
  protected UILabel txt_Lv;
  [SerializeField]
  protected UILabel txt_Lvmax;
  [SerializeField]
  protected UILabel txt_Hp;
  [SerializeField]
  private GameObject dirStatusPlus;
  [SerializeField]
  private UILabel txt_AgilityPlus;
  [SerializeField]
  private UILabel txt_LuckPlus;
  [SerializeField]
  private UILabel txt_MagicPlus;
  [SerializeField]
  private UILabel txt_PowerPlus;
  [SerializeField]
  private UILabel txt_ProtctPlus;
  [SerializeField]
  private UILabel txt_SpiritPlus;
  [SerializeField]
  private UILabel txt_TechniquePlus;
  [SerializeField]
  private UILabel txt_HpPlus;
  [SerializeField]
  protected UILabel txt_Unity;
  [SerializeField]
  protected UILabel txt_Unity_dec;
  [SerializeField]
  protected NGTweenGaugeScale gauge_Unity;
  [SerializeField]
  protected GameObject dir_prencessType;
  [SerializeField]
  protected GameObject[] slc_prencessType;
  [SerializeField]
  protected GameObject dir_maxHp;
  [SerializeField]
  protected UILabel txt_HpNow;
  [SerializeField]
  protected UILabel txt_Hpmax;
  private GameObject skillDetailPrefab;
  private PlayerUnit targetUnit;
  [SerializeField]
  protected UI2DSprite[] dyn_Weapon;
  [SerializeField]
  protected UI2DSprite[] dyn_IconRank;
  private GameObject[] weapon;
  private GearProfiencyIcon[] gearProfiencyIcon;
  [SerializeField]
  protected UIButton IbtnStatusDetail;
  [SerializeField]
  protected UIButton IbtnIntimacy;
  [SerializeField]
  protected UIButton IbtnUnitTraining;
  [SerializeField]
  protected UIButton IbtnUnitQuest;
  [SerializeField]
  private UIButton IbtnStatusChange;
  [SerializeField]
  private GameObject btnUnityDetail;
  [SerializeField]
  [Tooltip("[装備品|オーバーキラーズ]の順でタブメインオブジェクト配置")]
  private GameObject[] tabObjects;
  [SerializeField]
  private GameObject dirLvDetail;
  [SerializeField]
  private DetailMenu detailMenu;
  [SerializeField]
  private EffectBuguSlotLock[] BuguReleaseAnimator;
  private GameObject[] unityDetailPrefabs;
  private Action<GameObject[], NGMenuBase> popupUnityDetail;
  private GameObject levelDetailPrefab;
  private bool isXLevelLimited;
  private DetailMenuOverkillersSlots overkillersSlots_;
  private PlayerUnit playerUnit;
  private Coroutine coroutineUpdater_;

  private DetailMenuOverkillersSlots overkillersSlots
  {
    get
    {
      return !Object.op_Inequality((Object) this.overkillersSlots_, (Object) null) ? (this.overkillersSlots_ = ((Component) this).GetComponent<DetailMenuOverkillersSlots>()) : this.overkillersSlots_;
    }
  }

  private void Awake()
  {
    if (this.weapon == null)
      this.weapon = new GameObject[this.dyn_Weapon.Length];
    if (this.gearProfiencyIcon != null)
      return;
    this.gearProfiencyIcon = new GearProfiencyIcon[this.dyn_IconRank.Length];
  }

  private IEnumerator LoadSkillIcon(
    UI2DSprite sprite,
    BattleskillSkill skill,
    PlayerUnit unit,
    bool isGray)
  {
    Future<Sprite> spriteF = skill.LoadBattleSkillIcon();
    IEnumerator e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    sprite.sprite2D = spriteF.Result;
    if (isGray)
      ((UIWidget) sprite).color = Color.gray;
    else
      ((UIWidget) sprite).color = Color.white;
  }

  public override IEnumerator initAsync(
    PlayerUnit playerUnit,
    bool limitMode,
    bool isMaterial,
    GameObject[] prefabs)
  {
    DetailMenuScrollViewParam menuScrollViewParam = this;
    menuScrollViewParam.playerUnit = playerUnit;
    menuScrollViewParam.skillDetailPrefab = prefabs[0];
    GameObject gearKindIconPrefab = prefabs[1];
    GameObject profIconPrefab = prefabs[2];
    menuScrollViewParam.unityDetailPrefabs = new GameObject[2]
    {
      prefabs[3],
      prefabs[6]
    };
    UnitUnit masterUnit = playerUnit.unit;
    if (Object.op_Implicit((Object) menuScrollViewParam.overkillersSlots))
    {
      if (menuScrollViewParam.controlFlags.IsAnyOn(Control.OverkillersSlot | Control.CustomDeck))
      {
        OverkillersSlotRelease.Conditions[] releaseConditions = !masterUnit.exist_overkillers_slot || playerUnit.over_killers_player_unit_ids == null || playerUnit.over_killers_player_unit_ids.Length == 0 ? (OverkillersSlotRelease.Conditions[]) null : Singleton<NGGameDataManager>.GetInstance().getOverkillersSlotReleaseConditions(masterUnit.same_character_id);
        Control controlFlags = !menuScrollViewParam.isMine(playerUnit) || playerUnit.is_storage ? menuScrollViewParam.controlFlags.Clear(Control.OverkillersEdit) : menuScrollViewParam.controlFlags;
        yield return (object) menuScrollViewParam.overkillersSlots.initialize(prefabs[4], prefabs[5], playerUnit, releaseConditions, controlFlags);
      }
      else
        menuScrollViewParam.overkillersSlots.isHide = true;
    }
    if (Object.op_Inequality((Object) menuScrollViewParam.unitySkillObject, (Object) null))
    {
      OverkillersSkillRelease overkillersSkill = menuScrollViewParam.getOverkillersSkill();
      if (overkillersSkill != null)
      {
        menuScrollViewParam.unitySkillObject.SetActive(true);
        yield return (object) menuScrollViewParam.LoadSkillIcon(menuScrollViewParam.unitySKill, overkillersSkill.skill, playerUnit, (double) playerUnit.unityTotal < (double) overkillersSkill.unity_value);
      }
      else
        menuScrollViewParam.unitySkillObject.SetActive(false);
    }
    bool active = ((!menuScrollViewParam.isMine(playerUnit) || playerUnit.is_storage ? (int) menuScrollViewParam.controlFlags.Clear(Control.OverkillersEdit) : (int) menuScrollViewParam.controlFlags) & 33024) == 256;
    if (!menuScrollViewParam.controlFlags.IsOff(Control.SelfAbility))
      active = false;
    GearExtensionUnity gearExtensionUnity = MasterData.GearExtensionUnityList[0];
    for (int index = 0; index < menuScrollViewParam.BuguReleaseAnimator.Length; ++index)
    {
      menuScrollViewParam.BuguReleaseAnimator[index].slotActive(active);
      menuScrollViewParam.BuguReleaseAnimator[index].setUnity(gearExtensionUnity.total_unity_value.ToString());
    }
    if (Object.op_Inequality((Object) menuScrollViewParam.extraSkillObject, (Object) null))
    {
      if (masterUnit.trust_target_flag)
      {
        menuScrollViewParam.extraSkillObject.SetActive(true);
        menuScrollViewParam.slc_DearDegreeTitle.SetActive(true);
        if (masterUnit.IsSea)
          menuScrollViewParam.txtDearDegreeTitle.SetText(Consts.GetInstance().POPUP_004_LIMIT_EXTENDED_TITLE_DEAR);
        else
          menuScrollViewParam.txtDearDegreeTitle.SetText(Consts.GetInstance().POPUP_004_LIMIT_EXTENDED_TITLE_RELEVANCE);
        double num = Math.Round((double) playerUnit.trust_rate * 100.0) / 100.0;
        menuScrollViewParam.txtDearDegree.SetTextLocalize(num.ToString());
        menuScrollViewParam.txtDearDegreeMax.SetTextLocalize(Consts.Format(Consts.GetInstance().UNIT_TRUST_RATE_PERCENT3, (IDictionary) new Hashtable()
        {
          {
            (object) "trust_rate",
            (object) string.Format("{0}", (object) playerUnit.trust_max_rate)
          }
        }));
        UnitSkillAwake[] awakeSkills = playerUnit.GetAwakeSkills();
        if (awakeSkills == null || awakeSkills.Length == 0)
        {
          menuScrollViewParam.extraSkillObject.SetActive(false);
        }
        else
        {
          UnitSkillAwake unitSkillAwake = awakeSkills[0];
          bool isGray = (double) unitSkillAwake.need_affection > (double) playerUnit.trust_rate;
          IEnumerator e = menuScrollViewParam.LoadSkillIcon(menuScrollViewParam.extraSKill, unitSkillAwake.skill, playerUnit, isGray);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
      }
      else
      {
        menuScrollViewParam.extraSkillObject.SetActive(false);
        menuScrollViewParam.slc_DearDegreeTitle.SetActive(false);
        menuScrollViewParam.txtDearDegreeTitle.SetText(string.Empty);
      }
    }
    if (menuScrollViewParam.dyn_Weapon.Length != 0)
    {
      bool isAllEquipUnit = playerUnit.unit.IsAllEquipUnit;
      int[] numArray = new int[2]
      {
        masterUnit.kind_GearKind,
        7
      };
      for (int index = 0; index < numArray.Length; ++index)
      {
        int gearId = numArray[index];
        bool flag = gearId == 8;
        PlayerUnitGearProficiency unitGearProficiency = !flag ? Array.Find<PlayerUnitGearProficiency>(playerUnit.gear_proficiencies, (Predicate<PlayerUnitGearProficiency>) (x => x.gear_kind_id == gearId)) : (PlayerUnitGearProficiency) null;
        if ((flag || unitGearProficiency != null) && menuScrollViewParam.weapon.Length > index)
        {
          if (Object.op_Equality((Object) menuScrollViewParam.weapon[index], (Object) null))
          {
            ((Component) menuScrollViewParam.dyn_Weapon[index]).transform.Clear();
            menuScrollViewParam.weapon[index] = gearKindIconPrefab.Clone(((Component) menuScrollViewParam.dyn_Weapon[index]).transform);
            ((UIWidget) menuScrollViewParam.weapon[index].GetComponent<UI2DSprite>()).depth = ((UIWidget) menuScrollViewParam.dyn_Weapon[index]).depth + 1;
          }
          menuScrollViewParam.weapon[index].GetComponent<GearKindIcon>().Init(gearId);
          if (Object.op_Equality((Object) menuScrollViewParam.gearProfiencyIcon[index], (Object) null))
          {
            ((Component) menuScrollViewParam.dyn_IconRank[index]).transform.Clear();
            menuScrollViewParam.gearProfiencyIcon[index] = profIconPrefab.Clone(((Component) menuScrollViewParam.dyn_IconRank[index]).transform).GetComponent<GearProfiencyIcon>();
          }
          menuScrollViewParam.gearProfiencyIcon[index].Init(unitGearProficiency != null ? unitGearProficiency.level : 0, flag | isAllEquipUnit);
        }
      }
    }
    bool flag1 = menuScrollViewParam.controlFlags.IsOn(Control.CustomDeck);
    bool flag2 = !Singleton<NGGameDataManager>.GetInstance().IsColosseum && !flag1;
    bool flag3 = !flag1;
    bool flag4 = flag2 && menuScrollViewParam.controlFlags.IsOff(Control.MyGvgDeck) && !playerUnit.is_storage;
    if (((isMaterial ? 1 : (playerUnit.is_storage ? 1 : 0)) | (limitMode ? 1 : 0)) != 0)
      flag2 = false;
    else if (flag2 && Singleton<NGSceneManager>.GetInstance().isMatchSceneNameInStack("^(unit004_training|unit004_JobChange)$"))
      flag2 = false;
    if (limitMode)
    {
      flag3 = false;
      flag4 = false;
    }
    else if (flag4)
    {
      string pattern = "^(quest002_14|quest002_8|quest002_8_sea|guild028_2|tower029_battle_entry|CorpsQuest_battle_entry|raid032_battle|explore033_DeckEdit)$";
      if (Singleton<NGSceneManager>.GetInstance().isMatchSceneNameInStack(pattern))
      {
        flag4 = false;
      }
      else
      {
        List<int> list = ((IEnumerable<UnitUnit>) MasterData.UnitUnitList).Where<UnitUnit>((Func<UnitUnit, bool>) (x => x.same_character_id == masterUnit.same_character_id)).Select<UnitUnit, int>((Func<UnitUnit, int>) (x => x.ID)).ToList<int>();
        flag4 = false;
        foreach (QuestCharacterS questCharacterS in MasterData.QuestCharacterSList)
        {
          if (QuestCharacterS.CheckIsReleased(questCharacterS.start_at) && list.Contains(questCharacterS.unit_UnitUnit))
          {
            flag4 = true;
            break;
          }
        }
      }
    }
    if (Object.op_Implicit((Object) menuScrollViewParam.IbtnIntimacy))
      ((UIButtonColor) menuScrollViewParam.IbtnIntimacy).isEnabled = flag3;
    if (Object.op_Implicit((Object) menuScrollViewParam.IbtnUnitTraining))
      ((UIButtonColor) menuScrollViewParam.IbtnUnitTraining).isEnabled = flag2;
    if (Object.op_Implicit((Object) menuScrollViewParam.IbtnUnitQuest))
      ((UIButtonColor) menuScrollViewParam.IbtnUnitQuest).isEnabled = flag4;
    menuScrollViewParam.levelDetailPrefab = prefabs.Length > 7 ? prefabs[7] : (GameObject) null;
    menuScrollViewParam.isXLevelLimited = true;
    if (Object.op_Inequality((Object) menuScrollViewParam.dirLvDetail, (Object) null))
    {
      bool flag5 = menuScrollViewParam.controlFlags.IsOn(Control.OverkillersUnit);
      bool hasXlevel = playerUnit.hasXLevel;
      if (((limitMode || menuScrollViewParam.isMemory ? 1 : (playerUnit.is_storage ? 1 : 0)) | (isMaterial ? 1 : 0) | (flag5 ? 1 : 0) | (flag1 ? 1 : 0)) == 0 & hasXlevel && menuScrollViewParam.isMine(playerUnit))
        menuScrollViewParam.isXLevelLimited = false;
      if (Object.op_Equality((Object) menuScrollViewParam.levelDetailPrefab, (Object) null) | flag5 | flag1)
        menuScrollViewParam.dirLvDetail.SetActive(false);
      else
        menuScrollViewParam.dirLvDetail.SetActive(hasXlevel);
    }
  }

  public override bool Init(PlayerUnit playerUnit, PlayerUnit baseUnit)
  {
    ((Component) this).gameObject.SetActive(true);
    this.dispStatusCurrent = 0;
    this.targetUnit = playerUnit;
    bool flag = this.controlFlags.IsOn(Control.OverkillersUnit);
    if (flag && Object.op_Inequality((Object) this.IbtnStatusChange, (Object) null))
    {
      ((UIButtonColor) this.IbtnStatusDetail).isEnabled = false;
      ((UIButtonColor) this.IbtnStatusChange).isEnabled = false;
      foreach (Collider component in ((Component) this.IbtnStatusChange).gameObject.GetComponents<BoxCollider>())
        component.enabled = false;
    }
    int max = playerUnit.exp_next + playerUnit.exp - 1;
    int n = playerUnit.exp;
    if (n > max)
      n = max;
    if (Object.op_Implicit((Object) this.lvGauge))
      this.lvGauge.setValue(n, max, false);
    Judgement.NonBattleParameter nonBattleParameter;
    if (!this.isMemory)
    {
      nonBattleParameter = Judgement.NonBattleParameter.FromPlayerUnit(playerUnit, this.controlFlags.IsOn(Control.OverkillersUnit | Control.SelfAbility));
      this.setText(this.txt_Lv, playerUnit.total_level);
    }
    else
    {
      nonBattleParameter = Judgement.NonBattleParameter.FromPlayerUnitMemory(playerUnit);
      this.setText(this.txt_Lv, playerUnit.memory_level);
    }
    this.txt_Lvmax.SetTextLocalize(Consts.Format(Consts.GetInstance().UNIT_DETAIL_MAX, (IDictionary) new Hashtable()
    {
      {
        (object) "max",
        (object) playerUnit.total_max_level
      }
    }));
    this.SetPrincessType(playerUnit);
    this.setBicon(playerUnit.breakthrough_count, playerUnit.unit.breakthrough_limit);
    if (playerUnit.tower_is_entry && Singleton<CommonRoot>.GetInstance().headerType == CommonRoot.HeaderType.Tower)
    {
      this.dir_Hp.SetActive(false);
      this.dir_maxHp.SetActive(true);
      this.setText(this.txt_HpNow, TowerUtil.GetHp(nonBattleParameter.Hp, playerUnit.TowerHpRate));
      this.txt_Hpmax.SetTextLocalize(Consts.Format(Consts.GetInstance().UNIT_DETAIL_MAX, (IDictionary) new Hashtable()
      {
        {
          (object) "max",
          (object) nonBattleParameter.Hp
        }
      }));
    }
    else
    {
      this.dir_Hp.SetActive(true);
      this.dir_maxHp.SetActive(false);
      this.setText(this.txt_Hp, nonBattleParameter.Hp);
    }
    this.setText(this.txt_Cost, playerUnit.cost);
    this.setText(this.txt_Movement, nonBattleParameter.Move);
    this.setText(this.txt_Power, nonBattleParameter.Strength);
    this.setText(this.txt_Magic, nonBattleParameter.Intelligence);
    this.setText(this.txt_Protct, nonBattleParameter.Vitality);
    this.setText(this.txt_Spirit, nonBattleParameter.Mind);
    this.setText(this.txt_Agility, nonBattleParameter.Agility);
    this.setText(this.txt_Technique, nonBattleParameter.Dexterity);
    this.setText(this.txt_Luck, nonBattleParameter.Luck);
    if (!this.isMemory)
    {
      this.slc_HpMaxStar.SetActive(playerUnit.hp.is_max);
      this.slc_AgilityMaxStar.SetActive(playerUnit.agility.is_max);
      this.slc_LuckMaxStar.SetActive(playerUnit.lucky.is_max);
      this.slc_MagicMaxStar.SetActive(playerUnit.intelligence.is_max);
      this.slc_PowerMaxStar.SetActive(playerUnit.strength.is_max);
      this.slc_ProtctMaxStar.SetActive(playerUnit.vitality.is_max);
      this.slc_SpiritMaxStar.SetActive(playerUnit.mind.is_max);
      this.slc_TechniqueMaxStar.SetActive(playerUnit.dexterity.is_max);
    }
    else
    {
      this.slc_HpMaxStar.SetActive(playerUnit.is_memory_hp_max);
      this.slc_AgilityMaxStar.SetActive(playerUnit.is_memory_agility_max);
      this.slc_LuckMaxStar.SetActive(playerUnit.is_memory_lucky_max);
      this.slc_MagicMaxStar.SetActive(playerUnit.is_memory_intelligence_max);
      this.slc_PowerMaxStar.SetActive(playerUnit.is_memory_strength_max);
      this.slc_ProtctMaxStar.SetActive(playerUnit.is_memory_vitality_max);
      this.slc_SpiritMaxStar.SetActive(playerUnit.is_memory_mind_max);
      this.slc_TechniqueMaxStar.SetActive(playerUnit.is_memory_dexterity_max);
    }
    if (flag)
    {
      OverkillersParameter overkillersParameter = baseUnit?.getOverkillersParameter(playerUnit);
      if (overkillersParameter != null)
      {
        this.setTextOverkillersParameter(this.txt_PowerPlus, overkillersParameter.strength);
        this.setTextOverkillersParameter(this.txt_MagicPlus, overkillersParameter.intelligence);
        this.setTextOverkillersParameter(this.txt_ProtctPlus, overkillersParameter.vitality);
        this.setTextOverkillersParameter(this.txt_SpiritPlus, overkillersParameter.mind);
        this.setTextOverkillersParameter(this.txt_AgilityPlus, overkillersParameter.agility);
        this.setTextOverkillersParameter(this.txt_TechniquePlus, overkillersParameter.dexterity);
        this.setTextOverkillersParameter(this.txt_LuckPlus, overkillersParameter.lucky);
        this.dirStatusPlus.SetActive(true);
        this.setTextOverkillersParameter(this.txt_HpPlus, overkillersParameter.hp);
        ((Component) this.txt_HpPlus).gameObject.SetActive(true);
        goto label_23;
      }
    }
    else if (this.controlFlags.IsOff(Control.SelfAbility) && this.controlFlags.IsAnyOn(Control.OverkillersSlot | Control.CustomDeck) && playerUnit.isAnyCacheOverkillersUnits)
    {
      this.setTextPlus(this.txt_PowerPlus, playerUnit.strength.overkillersValue);
      this.setTextPlus(this.txt_MagicPlus, playerUnit.intelligence.overkillersValue);
      this.setTextPlus(this.txt_ProtctPlus, playerUnit.vitality.overkillersValue);
      this.setTextPlus(this.txt_SpiritPlus, playerUnit.mind.overkillersValue);
      this.setTextPlus(this.txt_AgilityPlus, playerUnit.agility.overkillersValue);
      this.setTextPlus(this.txt_TechniquePlus, playerUnit.dexterity.overkillersValue);
      this.setTextPlus(this.txt_LuckPlus, playerUnit.lucky.overkillersValue);
      this.dirStatusPlus.SetActive(true);
      this.setTextPlus(this.txt_HpPlus, playerUnit.hp.overkillersValue);
      ((Component) this.txt_HpPlus).gameObject.SetActive(true);
      goto label_23;
    }
    this.dirStatusPlus.SetActive(false);
    ((Component) this.txt_HpPlus).gameObject.SetActive(false);
label_23:
    this.setText(this.txt_Attack, nonBattleParameter.PhysicalAttack);
    this.setText(this.txt_Defense, nonBattleParameter.PhysicalDefense);
    this.setText(this.txt_Matk, nonBattleParameter.MagicAttack);
    this.setText(this.txt_Mdef, nonBattleParameter.MagicDefense);
    this.setText(this.txt_Dexterity, nonBattleParameter.Hit);
    this.setText(this.txt_Critical, nonBattleParameter.Critical);
    this.setText(this.txt_Evasion, nonBattleParameter.Evasion);
    this.setText(this.txt_Fighting, nonBattleParameter.Combat);
    this.setParamUnity(playerUnit);
    return true;
  }

  private void setTextPlus(UILabel label, int value) => label.text = value.ToString("+#;-#;+0;");

  private void setTextOverkillersParameter(UILabel label, int percentage)
  {
    label.text = "x" + OverkillersParameter.toStringPercentage(percentage) + "%";
  }

  private OverkillersSkillRelease getOverkillersSkill()
  {
    return this.isMine(this.playerUnit) ? this.playerUnit.overkillersSkill : (OverkillersSkillRelease) null;
  }

  private void setBicon(int count, int max)
  {
    for (int index = 0; index < this.slc_LBicon_None.Length; ++index)
    {
      if (index < max)
      {
        this.slc_LBicon_None[index].SetActive(index >= count);
        this.slc_LBicon_Blue[index].SetActive(index < count);
      }
      else
      {
        this.slc_LBicon_None[index].SetActive(false);
        this.slc_LBicon_Blue[index].SetActive(false);
      }
    }
  }

  private void setParamUnity(PlayerUnit playerUnit)
  {
    this.txt_Unity.SetTextLocalize(playerUnit.unityInt);
    this.txt_Unity_dec.SetTextLocalize(playerUnit.unityDec);
    if (Object.op_Implicit((Object) this.gauge_Unity))
      this.gauge_Unity.setValue(playerUnit.unityDec, 99, false);
    if (this.isMine(playerUnit) && this.controlFlags.IsOff(Control.OverkillersUnit | Control.CustomDeck))
    {
      bool bUpdateUnit = false;
      bool? bDisabledSelect = new bool?();
      this.popupUnityDetail = (Action<GameObject[], NGMenuBase>) ((prefabs, menu) =>
      {
        if (prefabs == null)
          return;
        if (bUpdateUnit)
        {
          PlayerUnit playerUnit1 = Array.Find<PlayerUnit>(SMManager.Get<PlayerUnit[]>(), (Predicate<PlayerUnit>) (x => x.id == playerUnit.id));
          if ((object) playerUnit1 == null)
            playerUnit1 = playerUnit;
          playerUnit = playerUnit1;
        }
        if (!bDisabledSelect.HasValue)
          bDisabledSelect = new bool?(this.isDisabledQuestSelect);
        PopupUnityValueDetail.show(prefabs[0], prefabs[1], (float) playerUnit.unity_value, playerUnit.buildup_unity_value_f, playerUnit, (Action<Action>) (endWait =>
        {
          bUpdateUnit = true;
          NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
          NGSceneBase sceneBase = instance.sceneBase;
          if (Object.op_Equality((Object) sceneBase, (Object) null))
            return;
          switch (sceneBase.sceneName)
          {
            case "unit004_2":
            case "unit004_2_sea":
              Singleton<NGGameDataManager>.GetInstance().setSceneChangeLog(instance.exportSceneChangeLog());
              Unit0042Menu unit0042Menu = (Unit0042Menu) menu;
              if (Object.op_Inequality((Object) unit0042Menu, (Object) null))
                instance.StartCoroutine(unit0042Menu.doUploadFavorites(endWait));
              else if (endWait != null)
                endWait();
              DetailMenuScrollViewParam.ModifySelectedPlayerUnitInChangeSceneParam(playerUnit);
              Singleton<NGGameDataManager>.GetInstance().OpenPopup = this.popupUnityDetail;
              break;
            case "unit004_JobChange":
              Singleton<NGGameDataManager>.GetInstance().setSceneChangeLog(instance.exportSceneChangeLog());
              Singleton<NGGameDataManager>.GetInstance().OpenPopup = this.popupUnityDetail;
              if (endWait == null)
                break;
              endWait();
              break;
          }
        }), (Action<PopupUtility.SceneTo>) (_ => { }), bDisabledSelect.Value);
      });
    }
    else
      this.popupUnityDetail = (Action<GameObject[], NGMenuBase>) null;
    if (!Object.op_Inequality((Object) this.btnUnityDetail, (Object) null))
      return;
    this.btnUnityDetail.SetActive(this.popupUnityDetail != null);
  }

  private bool isDisabledQuestSelect
  {
    get
    {
      return this.playerUnit.is_storage || this.controlFlags.IsAnyOn(Control.Limited | Control.ToutaPlusNoEnable) || DetailMenuScrollViewParam.checkDisableQuestSelect();
    }
  }

  public static bool checkDisableQuestSelect()
  {
    if (Singleton<NGGameDataManager>.GetInstance().IsColosseum)
      return true;
    NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
    switch (instance.sceneName)
    {
      case "unit004_2":
      case "unit004_JobChange":
        return instance.isMatchSceneNameInStack("^(quest002_8|guild028_2|explore033_DeckEdit|tower029_battle_entry|CorpsQuest_battle_entry|raid032_battle)$");
      case "unit004_2_sea":
        return instance.isMatchSceneNameInStack("^quest002_8_sea$");
      default:
        return true;
    }
  }

  public static void ModifySelectedPlayerUnitInChangeSceneParam(PlayerUnit playerUnit)
  {
    object[] args = Singleton<NGSceneManager>.GetInstance().GetSavedChangeSceneParam().args;
    for (int index = 0; index < args.Length; ++index)
    {
      if (args[index] != null && args[index] is Unit0042Scene.BootParam)
      {
        Unit0042Scene.BootParam orignal = (Unit0042Scene.BootParam) args[index];
        orignal.playerUnit = playerUnit;
        args[index] = (object) new Unit0042Scene.SavedBootParam(orignal);
        break;
      }
    }
  }

  private bool isMine(PlayerUnit unit)
  {
    return !unit.is_enemy && !unit.is_guest && Player.Current.id == unit.player_id;
  }

  private void SetPrincessType(PlayerUnit playerUnit)
  {
    if (playerUnit.unit_type == null || playerUnit.is_enemy)
    {
      this.dir_prencessType.gameObject.SetActive(false);
    }
    else
    {
      this.dir_prencessType.gameObject.SetActive(true);
      if (Object.op_Inequality((Object) this.slc_prencessType[0].gameObject.GetComponent<UILabel>(), (Object) null))
        this.slc_prencessType[0].gameObject.GetComponent<UILabel>().SetText("");
      if (!Singleton<NGGameDataManager>.GetInstance().IsSea)
        ((IEnumerable<GameObject>) this.slc_prencessType).ToggleOnceEx((int) playerUnit.unit_type.Enum);
      else
        this.slc_prencessType[0].gameObject.GetComponent<UILabel>().SetText(Consts.GetInstance().GetUnitTypeText(playerUnit.unit_type.Enum));
    }
  }

  public override void MarkAsChanged()
  {
    if (Object.op_Inequality((Object) this.slc_DearDegreeTitle, (Object) null))
      this.slc_DearDegreeTitle.GetComponent<UIWidget>().MarkAsChanged();
    foreach (GameObject gameObject in this.slc_LBicon_None)
      gameObject.GetComponent<UIWidget>().MarkAsChanged();
    foreach (GameObject gameObject in this.slc_LBicon_Blue)
      gameObject.GetComponent<UIWidget>().MarkAsChanged();
    foreach (UIWidget componentsInChild in this.unitySkillObject.GetComponentsInChildren<UIWidget>())
      componentsInChild.MarkAsChanged();
    foreach (UIWidget componentsInChild in this.extraSkillObject.GetComponentsInChildren<UIWidget>())
      componentsInChild.MarkAsChanged();
  }

  public void onClickStatusChange()
  {
    ++this.dispStatusCurrent;
    this.dispStatusCurrent %= ((IEnumerable<GameObject>) this.dir_statusNum).Count<GameObject>();
    ((IEnumerable<GameObject>) this.dir_statusNum).ToggleOnceEx(this.dispStatusCurrent);
    ((IEnumerable<GameObject>) this.dir_statusListText).ToggleOnceEx(this.dispStatusCurrent);
  }

  public void onClickOverkillersSKill()
  {
    OverkillersSkillRelease overkillersSkill = this.getOverkillersSkill();
    if (overkillersSkill == null)
      return;
    PopupSkillDetails.show(this.skillDetailPrefab, new PopupSkillDetails.Param(overkillersSkill, new int?((int) this.playerUnit.unityTotal)));
  }

  public void onClickExtraSKill()
  {
    UnitSkillAwake[] awakeSkills = this.playerUnit.GetAwakeSkills();
    if (awakeSkills == null || awakeSkills.Length == 0)
      return;
    bool bDisabledStatus = !Object.op_Implicit((Object) this.txtDearDegree) && !Object.op_Implicit((Object) this.txtDearDegreeMax);
    PopupSkillDetails.show(this.skillDetailPrefab, PopupSkillDetails.Param.createByUnitView(awakeSkills[0], this.playerUnit, bDisabledStatus: bDisabledStatus));
  }

  public void onClicedUnity()
  {
    Action<GameObject[], NGMenuBase> popupUnityDetail = this.popupUnityDetail;
    if (popupUnityDetail == null)
      return;
    popupUnityDetail(this.unityDetailPrefabs, (NGMenuBase) NGUITools.FindInParents<Unit0042Menu>(((Component) this).gameObject));
  }

  public void onClickedTabEquipment()
  {
    this.onClickedTab(DetailMenuScrollViewParam.TabMode.Equipment);
  }

  public void onClickedTabOverkillers()
  {
    this.onClickedTab(DetailMenuScrollViewParam.TabMode.Overkillers);
  }

  private void onClickedTab(DetailMenuScrollViewParam.TabMode mode)
  {
    Unit0042Menu inParents = NGUITools.FindInParents<Unit0042Menu>(((Component) this).gameObject);
    if (Object.op_Inequality((Object) inParents, (Object) null))
      inParents.UpdateInfoIndicator(mode);
    else
      this.changeTab(mode);
  }

  public void changeTab(DetailMenuScrollViewParam.TabMode mode)
  {
    for (int index = 0; index < this.tabObjects.Length; ++index)
      this.tabObjects[index].SetActive((DetailMenuScrollViewParam.TabMode) index == mode);
  }

  public void onClickedLvDetail()
  {
    if (Object.op_Equality((Object) this.levelDetailPrefab, (Object) null))
      return;
    PopupXLevelDetail.show(this.levelDetailPrefab, this.isXLevelLimited, this.playerUnit, new Action<PlayerUnit>(this.updateUnit), (Action<PlayerUnit>) (resultReincarnation => { }));
  }

  private void updateUnit(PlayerUnit result)
  {
    if (this.coroutineUpdater_ != null)
      return;
    this.coroutineUpdater_ = Singleton<NGSceneManager>.GetInstance().StartCoroutine(this.doUpdateUnit());
  }

  private IEnumerator doUpdateUnit()
  {
    DetailMenuScrollViewParam menuScrollViewParam = this;
    while (Singleton<PopupManager>.GetInstance().isOpen || Singleton<PopupManager>.GetInstance().isRunningCoroutine)
      yield return (object) null;
    Unit0042Menu menu = NGUITools.FindInParents<Unit0042Menu>(((Component) menuScrollViewParam).gameObject);
    if (Object.op_Inequality((Object) menu, (Object) null))
    {
      Singleton<PopupManager>.GetInstance().open((GameObject) null, isViewBack: false, isNonSe: true);
      yield return (object) null;
      PlayerUnit[] newList = SMManager.Get<PlayerUnit[]>();
      yield return (object) menu.UpdateAllPage(((IEnumerable<PlayerUnit>) menu.UnitList).Select<PlayerUnit, PlayerUnit>((Func<PlayerUnit, PlayerUnit>) (x => Array.Find<PlayerUnit>(newList, (Predicate<PlayerUnit>) (y => y.id == x.id)))).ToArray<PlayerUnit>());
      yield return (object) null;
      Singleton<PopupManager>.GetInstance().dismiss();
    }
    menuScrollViewParam.coroutineUpdater_ = (Coroutine) null;
  }

  public void IbtnBuguLock2()
  {
    Unit0042Menu inParents = NGUITools.FindInParents<Unit0042Menu>(((Component) this).gameObject);
    if (!Object.op_Inequality((Object) inParents, (Object) null))
      return;
    PopupBuguSlotRelease.show(inParents.buguReleaseDialogPrefab, this.targetUnit, this, (Action) (() => this.StartCoroutine(this.doUnlockBugu(0))));
  }

  public void IbtnBuguLock3()
  {
    Unit0042Menu inParents = NGUITools.FindInParents<Unit0042Menu>(((Component) this).gameObject);
    if (!Object.op_Inequality((Object) inParents, (Object) null))
      return;
    PopupBuguSlotRelease.show(inParents.buguReleaseDialogPrefab, this.targetUnit, this, (Action) (() => this.StartCoroutine(this.doUnlockBugu(1))));
  }

  private IEnumerator doUnlockBugu(int no)
  {
    DetailMenuScrollViewParam menuScrollViewParam = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1);
    Unit0042Menu inParents = NGUITools.FindInParents<Unit0042Menu>(((Component) menuScrollViewParam).gameObject);
    if (Object.op_Inequality((Object) inParents, (Object) null))
    {
      bool bWait = true;
      inParents.UploadFavorites((Action) (() => bWait = false));
      while (bWait)
        yield return (object) null;
    }
    IEnumerator e = WebAPI.UnitOpenGearEquipNumber(menuScrollViewParam.targetUnit.id, (Action<WebAPI.Response.UserError>) (error =>
    {
      WebAPI.DefaultUserErrorCallback(error);
      MypageScene.ChangeSceneOnError();
    })).Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<NGGameDataManager>.GetInstance().Add_opened_equip_number_player_unit_ids(menuScrollViewParam.targetUnit.id);
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    Singleton<PopupManager>.GetInstance().open((GameObject) null, isViewBack: false);
    yield return (object) new WaitForSeconds(0.5f);
    menuScrollViewParam.BuguReleaseAnimator[no].startUnlock((Action) (() => this.onFinishedEffectUnlock(no)));
    yield return (object) null;
  }

  private void onFinishedEffectUnlock(int no)
  {
    Singleton<PopupManager>.GetInstance().dismiss();
    this.StartCoroutine(this.doUpdateUnits(no));
    this.BuguReleaseAnimator[no].ResetAnimation();
  }

  private IEnumerator doUpdateUnits(int no)
  {
    yield return (object) Singleton<NGSceneManager>.GetInstance().StartCoroutine(this.doUpdateUnit());
    yield return (object) null;
  }

  public override IEnumerator initAsyncDiffMode(
    PlayerUnit playerUnit,
    PlayerUnit prevUnit,
    IDetailMenuContainer menuContainer)
  {
    DetailMenuScrollViewParam menuScrollViewParam = this;
    menuScrollViewParam.playerUnit = playerUnit;
    if (menuScrollViewParam.weapon == null)
      menuScrollViewParam.weapon = new GameObject[menuScrollViewParam.dyn_Weapon.Length];
    if (menuScrollViewParam.gearProfiencyIcon == null)
      menuScrollViewParam.gearProfiencyIcon = new GearProfiencyIcon[menuScrollViewParam.dyn_IconRank.Length];
    menuScrollViewParam.skillDetailPrefab = menuContainer.skillDetailDialogPrefab;
    menuScrollViewParam.unityDetailPrefabs = new GameObject[2]
    {
      menuContainer.unityDetailPrefab,
      menuContainer.stageItemPrefab
    };
    menuScrollViewParam.init(playerUnit, prevUnit);
    menuScrollViewParam.overkillersSlots.isHide = true;
    if (Object.op_Inequality((Object) menuScrollViewParam.unitySkillObject, (Object) null))
    {
      OverkillersSkillRelease overkillersSkill = menuScrollViewParam.getOverkillersSkill();
      if (overkillersSkill != null)
      {
        menuScrollViewParam.unitySkillObject.SetActive(true);
        yield return (object) menuScrollViewParam.LoadSkillIcon(menuScrollViewParam.unitySKill, overkillersSkill.skill, playerUnit, (double) playerUnit.unityTotal < (double) overkillersSkill.unity_value);
      }
      else
        menuScrollViewParam.unitySkillObject.SetActive(false);
    }
    if (Object.op_Inequality((Object) menuScrollViewParam.extraSkillObject, (Object) null))
    {
      menuScrollViewParam.extraSkillObject.SetActive(false);
      menuScrollViewParam.slc_DearDegreeTitle.SetActive(false);
      menuScrollViewParam.txtDearDegreeTitle.SetText(string.Empty);
      if (playerUnit.unit.trust_target_flag)
      {
        menuScrollViewParam.extraSkillObject.SetActive(true);
        menuScrollViewParam.slc_DearDegreeTitle.SetActive(true);
        if (playerUnit.unit.IsSea)
          menuScrollViewParam.txtDearDegreeTitle.SetText(Consts.GetInstance().POPUP_004_LIMIT_EXTENDED_TITLE_DEAR);
        else
          menuScrollViewParam.txtDearDegreeTitle.SetText(Consts.GetInstance().POPUP_004_LIMIT_EXTENDED_TITLE_RELEVANCE);
        double num = Math.Round((double) playerUnit.trust_rate * 100.0) / 100.0;
        menuScrollViewParam.txtDearDegree.SetTextLocalize(num.ToString());
        menuScrollViewParam.txtDearDegreeMax.SetTextLocalize(Consts.Format(Consts.GetInstance().UNIT_TRUST_RATE_PERCENT3, (IDictionary) new Hashtable()
        {
          {
            (object) "trust_rate",
            (object) string.Format("{0}", (object) playerUnit.trust_max_rate)
          }
        }));
        UnitSkillAwake[] awakeSkills = playerUnit.GetAwakeSkills();
        if (awakeSkills == null || awakeSkills.Length == 0)
        {
          menuScrollViewParam.extraSkillObject.SetActive(false);
        }
        else
        {
          UnitSkillAwake unitSkillAwake = awakeSkills[0];
          bool isGray = (double) unitSkillAwake.need_affection > (double) playerUnit.trust_rate;
          yield return (object) menuScrollViewParam.LoadSkillIcon(menuScrollViewParam.extraSKill, unitSkillAwake.skill, playerUnit, isGray);
        }
      }
    }
    UnitUnit unit = playerUnit.unit;
    bool isAllEquipUnit = unit.IsAllEquipUnit;
    int[] numArray = new int[2]{ unit.kind_GearKind, 7 };
    for (int index = 0; index < numArray.Length; ++index)
    {
      int gearId = numArray[index];
      bool flag = gearId == 8;
      PlayerUnitGearProficiency unitGearProficiency = !flag ? Array.Find<PlayerUnitGearProficiency>(playerUnit.gear_proficiencies, (Predicate<PlayerUnitGearProficiency>) (x => x.gear_kind_id == gearId)) : (PlayerUnitGearProficiency) null;
      if ((flag || unitGearProficiency != null) && menuScrollViewParam.weapon.Length > index)
      {
        if (Object.op_Equality((Object) menuScrollViewParam.weapon[index], (Object) null))
        {
          ((Component) menuScrollViewParam.dyn_Weapon[index]).transform.Clear();
          menuScrollViewParam.weapon[index] = menuContainer.gearKindIconPrefab.Clone(((Component) menuScrollViewParam.dyn_Weapon[index]).transform);
          ((UIWidget) menuScrollViewParam.weapon[index].GetComponent<UI2DSprite>()).depth = ((UIWidget) menuScrollViewParam.dyn_Weapon[index]).depth + 1;
        }
        menuScrollViewParam.weapon[index].GetComponent<GearKindIcon>().Init(gearId);
        if (Object.op_Equality((Object) menuScrollViewParam.gearProfiencyIcon[index], (Object) null))
        {
          ((Component) menuScrollViewParam.dyn_IconRank[index]).transform.Clear();
          menuScrollViewParam.gearProfiencyIcon[index] = menuContainer.profIconPrefab.Clone(((Component) menuScrollViewParam.dyn_IconRank[index]).transform).GetComponent<GearProfiencyIcon>();
        }
        menuScrollViewParam.gearProfiencyIcon[index].Init(unitGearProficiency != null ? unitGearProficiency.level : 0, flag | isAllEquipUnit);
      }
    }
    menuScrollViewParam.inactivateGameObject<UIButton>(menuScrollViewParam.IbtnStatusDetail);
    menuScrollViewParam.inactivateGameObject<UIButton>(menuScrollViewParam.IbtnUnitTraining);
    menuScrollViewParam.inactivateGameObject<UIButton>(menuScrollViewParam.IbtnIntimacy);
    menuScrollViewParam.inactivateGameObject<UIButton>(menuScrollViewParam.IbtnUnitQuest);
    menuScrollViewParam.inactivateGameObject(menuScrollViewParam.dirStatusPlus);
    menuScrollViewParam.inactivateGameObject<UILabel>(menuScrollViewParam.txt_HpPlus);
    menuScrollViewParam.inactivateGameObject(menuScrollViewParam.dirLvDetail);
  }

  private void init(PlayerUnit playerUnit, PlayerUnit prevUnit)
  {
    ((Component) this).gameObject.SetActive(true);
    this.dispStatusCurrent = 0;
    this.targetUnit = playerUnit;
    int max = playerUnit.exp_next + playerUnit.exp - 1;
    int n = playerUnit.exp;
    if (n > max)
      n = max;
    this.lvGauge.setValue(n, max, false);
    Judgement.NonBattleParameter nonBattleParameter = Judgement.NonBattleParameter.FromPlayerUnit(playerUnit, true);
    Judgement.NonBattleParameter prev = prevUnit != (PlayerUnit) null ? Judgement.NonBattleParameter.FromPlayerUnit(prevUnit, true) : (Judgement.NonBattleParameter) null;
    this.setText(this.txt_Lv, playerUnit.total_level);
    this.txt_Lvmax.SetTextLocalize(Consts.Format(Consts.GetInstance().UNIT_DETAIL_MAX, (IDictionary) new Hashtable()
    {
      {
        (object) "max",
        (object) playerUnit.total_max_level
      }
    }));
    this.SetPrincessType(playerUnit);
    this.setBicon(playerUnit.breakthrough_count, playerUnit.unit.breakthrough_limit);
    this.dir_Hp.SetActive(true);
    this.dir_maxHp.SetActive(false);
    this.slc_HpMaxStar.SetActive(playerUnit.hp.is_max);
    this.slc_AgilityMaxStar.SetActive(playerUnit.agility.is_max);
    this.slc_LuckMaxStar.SetActive(playerUnit.lucky.is_max);
    this.slc_MagicMaxStar.SetActive(playerUnit.intelligence.is_max);
    this.slc_PowerMaxStar.SetActive(playerUnit.strength.is_max);
    this.slc_ProtctMaxStar.SetActive(playerUnit.vitality.is_max);
    this.slc_SpiritMaxStar.SetActive(playerUnit.mind.is_max);
    this.slc_TechniqueMaxStar.SetActive(playerUnit.dexterity.is_max);
    this.setParamUnity(playerUnit);
    Dictionary<string, UILabel> dest = new Dictionary<string, UILabel>()
    {
      {
        "Hp",
        this.txt_Hp
      },
      {
        "Move",
        this.txt_Movement
      },
      {
        "Strength",
        this.txt_Power
      },
      {
        "Intelligence",
        this.txt_Magic
      },
      {
        "Vitality",
        this.txt_Protct
      },
      {
        "Mind",
        this.txt_Spirit
      },
      {
        "Agility",
        this.txt_Agility
      },
      {
        "Dexterity",
        this.txt_Technique
      },
      {
        "Luck",
        this.txt_Luck
      },
      {
        "PhysicalAttack",
        this.txt_Attack
      },
      {
        "PhysicalDefense",
        this.txt_Defense
      },
      {
        "MagicAttack",
        this.txt_Matk
      },
      {
        "MagicDefense",
        this.txt_Mdef
      },
      {
        "Hit",
        this.txt_Dexterity
      },
      {
        "Critical",
        this.txt_Critical
      },
      {
        "Evasion",
        this.txt_Evasion
      },
      {
        "Combat",
        this.txt_Fighting
      },
      {
        "Cost",
        this.txt_Cost
      }
    };
    if (prev != null)
      Util.SetTextIntegersWithStateColor<Judgement.NonBattleParameter>(dest, nonBattleParameter, prev);
    else
      Util.SetTextIntegers<Judgement.NonBattleParameter>(dest, nonBattleParameter, Color.white);
  }

  public enum TabMode
  {
    Equipment,
    Overkillers,
  }
}
