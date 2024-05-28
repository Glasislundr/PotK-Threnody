// Decompiled with JetBrains decompiler
// Type: Unit00498Menu
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
using UnitStatusInformation;
using UnityEngine;

#nullable disable
public class Unit00498Menu : BackButtonMenuBase
{
  public GameObject unitTexture;
  [SerializeField]
  private UILabel TxtCharaName;
  public UILabel jobNameLabel;
  public UILabel jobRankLabel;
  public UILabel maxLevelLabel;
  public GameObject growthParameter;
  [SerializeField]
  private Unit00498Scene sceneBase;
  [SerializeField]
  private GameObject DirAnimEvolution;
  [SerializeField]
  private GameObject DirAnimRevival;
  [SerializeField]
  private GameObject DirAnimEvolutionX;
  [SerializeField]
  private GameObject DirMove;
  [SerializeField]
  private UILabel TxtMoveBefore;
  [SerializeField]
  private UILabel TxtMoveAfter;
  [SerializeField]
  private UILabel TxtNoChange;
  [SerializeField]
  private GameObject MoveBefore;
  [SerializeField]
  private GameObject MoveAfter;
  [SerializeField]
  private GameObject SlcArrow;
  [SerializeField]
  private GameObject NoChange;
  [SerializeField]
  private UI2DSprite[] iconBeforeFamilies;
  [SerializeField]
  private GameObject topAfterFamilies;
  [SerializeField]
  private float localXBeforeSingleFamily = -62f;
  [SerializeField]
  private UI2DSprite[] iconAfterFamilies;
  private float? localXBeforeDoubleFamily;
  private bool jobChangeXDirty;
  private NGSoundManager sm;

  private void OnDisable()
  {
    if (!Object.op_Inequality((Object) this.sm, (Object) null))
      return;
    this.sm.stopVoice();
  }

  public void onTouchPanel()
  {
    if (this.IsPushAndSet())
      return;
    if (!Singleton<TutorialRoot>.GetInstance().IsTutorialFinish())
      this.sceneBase.TutorialAdvice();
    else if (this.jobChangeXDirty)
    {
      this.jobChangeXDirty = false;
      this.StartCoroutine(this.OpenJobChangeXTutorial());
    }
    else
      this.backScene();
  }

  public override void onBackButton() => this.onTouchPanel();

  private IEnumerator OpenJobChangeXTutorial()
  {
    Unit00498Menu unit00498Menu = this;
    Future<GameObject> ft = new ResourceObject("Prefabs/unit004_2/popup_Xjob_tutorial").Load<GameObject>();
    IEnumerator e = ft.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject popup = Singleton<PopupManager>.GetInstance().open(ft.Result, isNonSe: true, isNonOpenAnime: true);
    // ISSUE: reference to a compiler-generated method
    e = popup.GetComponent<SimpleScrollContentsPopup>().Initialize(new Action(unit00498Menu.\u003COpenJobChangeXTutorial\u003Eb__28_0));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().startOpenAnime(popup);
    Persist.jobXInfo.Data.tutorialShow = true;
    Persist.jobXInfo.Flush();
  }

  private void SetMaxLv(int beforeLv, int afterLv)
  {
    this.maxLevelLabel.SetTextLocalize(afterLv);
    ((UIWidget) this.maxLevelLabel).color = beforeLv < afterLv ? Color.yellow : Color.white;
  }

  private void SetMovement(int beforeMovement, int afterMovement)
  {
    if (beforeMovement != afterMovement)
    {
      this.MoveBefore.SetActive(true);
      this.MoveAfter.SetActive(true);
      this.SlcArrow.SetActive(true);
      this.NoChange.SetActive(false);
      this.TxtMoveBefore.SetTextLocalize(beforeMovement);
      Util.SetTextIntegerWithStateColor(this.TxtMoveAfter, afterMovement, beforeMovement);
    }
    else
    {
      this.MoveBefore.SetActive(false);
      this.MoveAfter.SetActive(false);
      this.SlcArrow.SetActive(false);
      this.NoChange.SetActive(true);
      this.TxtNoChange.SetTextLocalize(afterMovement);
    }
  }

  private IEnumerator SetUnitTexture(PlayerUnit unit)
  {
    UnitUnit unitUnit = unit.unit;
    Future<GameObject> future = unitUnit.LoadMypage();
    IEnumerator e = future.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject texObj = future.Result.Clone(this.unitTexture.transform);
    if (unitUnit.IsNormalUnit)
    {
      e = unitUnit.SetLargeSpriteSnap(unit.job_id, texObj, 4);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = unitUnit.SetLargeSpriteWithMask(unit.job_id, texObj, Res.GUI._004_9_8_sozai.mask_chara.Load<Texture2D>(), 5, -146, 36);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      texObj.transform.localPosition = new Vector3(-10f, -265f, 0.0f);
      e = unitUnit.SetMediumSpriteSnap(texObj, 4);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = unitUnit.SetMediumSpriteWithMask(texObj, Res.GUI._004_9_8_sozai.mask_chara.Load<Texture2D>(), 5, -146, 36);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public IEnumerator setCharacter(
    PlayerUnit beforeUnit,
    PlayerUnit afterUnit,
    Unit00499Scene.Mode mode)
  {
    Future<GameObject> loader = GrowthParameter.LoadPrefab();
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GrowthParameter instance = GrowthParameter.GetInstance(loader.Result, this.growthParameter.transform);
    MasterDataTable.UnitJob afterJob = afterUnit.getJobData();
    this.TxtCharaName.SetTextLocalize(afterUnit.unit.name);
    this.jobNameLabel.SetTextLocalize(afterJob.name);
    if (mode == Unit00499Scene.Mode.Transmigration)
      ((UIWidget) this.jobNameLabel).color = Color.white;
    if (Object.op_Inequality((Object) this.jobRankLabel, (Object) null))
      this.jobRankLabel.SetTextLocalize(MasterData.UnitJobRankName[afterJob.job_rank_UnitJobRank].name);
    this.SetMaxLv(beforeUnit.max_level, afterUnit.max_level);
    Judgement.NonBattleParameter nonBattleParameter1 = Judgement.NonBattleParameter.FromPlayerUnitInitialGear(beforeUnit, true);
    Judgement.NonBattleParameter nonBattleParameter2 = Judgement.NonBattleParameter.FromPlayerUnitInitialGear(afterUnit, true);
    this.SetMovement(nonBattleParameter1.Move, nonBattleParameter2.Move);
    if (afterUnit.unit.IsNormalUnit && beforeUnit.unit.IsNormalUnit)
    {
      switch (mode)
      {
        case Unit00499Scene.Mode.Transmigration:
          instance.SetParameter(GrowthParameter.ParameterType.HP, afterUnit.hp.transmigrate - beforeUnit.hp.transmigrate, afterUnit.self_hp_without_x - afterUnit.hp.transmigrate, afterUnit.self_hp_without_x, false);
          instance.SetParameter(GrowthParameter.ParameterType.STR, afterUnit.strength.transmigrate - beforeUnit.strength.transmigrate, afterUnit.self_strength_without_x - afterUnit.strength.transmigrate, afterUnit.self_strength_without_x, false);
          instance.SetParameter(GrowthParameter.ParameterType.INT, afterUnit.intelligence.transmigrate - beforeUnit.intelligence.transmigrate, afterUnit.self_intelligence_without_x - afterUnit.intelligence.transmigrate, afterUnit.self_intelligence_without_x, false);
          instance.SetParameter(GrowthParameter.ParameterType.VIT, afterUnit.vitality.transmigrate - beforeUnit.vitality.transmigrate, afterUnit.self_vitality_without_x - afterUnit.vitality.transmigrate, afterUnit.self_vitality_without_x, false);
          instance.SetParameter(GrowthParameter.ParameterType.MND, afterUnit.mind.transmigrate - beforeUnit.mind.transmigrate, afterUnit.self_mind_without_x - afterUnit.mind.transmigrate, afterUnit.self_mind_without_x, false);
          instance.SetParameter(GrowthParameter.ParameterType.AGI, afterUnit.agility.transmigrate - beforeUnit.agility.transmigrate, afterUnit.self_agility_without_x - afterUnit.agility.transmigrate, afterUnit.self_agility_without_x, false);
          instance.SetParameter(GrowthParameter.ParameterType.DEX, afterUnit.dexterity.transmigrate - beforeUnit.dexterity.transmigrate, afterUnit.self_dexterity_without_x - afterUnit.dexterity.transmigrate, afterUnit.self_dexterity_without_x, false);
          instance.SetParameter(GrowthParameter.ParameterType.LUK, afterUnit.lucky.transmigrate - beforeUnit.lucky.transmigrate, afterUnit.self_lucky_without_x - afterUnit.lucky.transmigrate, afterUnit.self_lucky_without_x, false);
          break;
        case Unit00499Scene.Mode.JobChange:
          this.setParameterByJobChange(instance, GrowthParameter.ParameterType.HP, nonBattleParameter1.Hp, nonBattleParameter2.Hp);
          this.setParameterByJobChange(instance, GrowthParameter.ParameterType.STR, nonBattleParameter1.Strength, nonBattleParameter2.Strength);
          this.setParameterByJobChange(instance, GrowthParameter.ParameterType.INT, nonBattleParameter1.Intelligence, nonBattleParameter2.Intelligence);
          this.setParameterByJobChange(instance, GrowthParameter.ParameterType.VIT, nonBattleParameter1.Vitality, nonBattleParameter2.Vitality);
          this.setParameterByJobChange(instance, GrowthParameter.ParameterType.MND, nonBattleParameter1.Mind, nonBattleParameter2.Mind);
          this.setParameterByJobChange(instance, GrowthParameter.ParameterType.AGI, nonBattleParameter1.Agility, nonBattleParameter2.Agility);
          this.setParameterByJobChange(instance, GrowthParameter.ParameterType.DEX, nonBattleParameter1.Dexterity, nonBattleParameter2.Dexterity);
          this.setParameterByJobChange(instance, GrowthParameter.ParameterType.LUK, nonBattleParameter1.Luck, nonBattleParameter2.Luck);
          break;
        default:
          instance.SetParameter(GrowthParameter.ParameterType.HP, afterUnit.hp.transmigrate - beforeUnit.hp.transmigrate, afterUnit.self_total_hp - afterUnit.hp.transmigrate, afterUnit.self_total_hp, false);
          instance.SetParameter(GrowthParameter.ParameterType.STR, afterUnit.strength.transmigrate - beforeUnit.strength.transmigrate, afterUnit.self_total_strength - afterUnit.strength.transmigrate, afterUnit.self_total_strength, false);
          instance.SetParameter(GrowthParameter.ParameterType.INT, afterUnit.intelligence.transmigrate - beforeUnit.intelligence.transmigrate, afterUnit.self_total_intelligence - afterUnit.intelligence.transmigrate, afterUnit.self_total_intelligence, false);
          instance.SetParameter(GrowthParameter.ParameterType.VIT, afterUnit.vitality.transmigrate - beforeUnit.vitality.transmigrate, afterUnit.self_total_vitality - afterUnit.vitality.transmigrate, afterUnit.self_total_vitality, false);
          instance.SetParameter(GrowthParameter.ParameterType.MND, afterUnit.mind.transmigrate - beforeUnit.mind.transmigrate, afterUnit.self_total_mind - afterUnit.mind.transmigrate, afterUnit.self_total_mind, false);
          instance.SetParameter(GrowthParameter.ParameterType.AGI, afterUnit.agility.transmigrate - beforeUnit.agility.transmigrate, afterUnit.self_total_agility - afterUnit.agility.transmigrate, afterUnit.self_total_agility, false);
          instance.SetParameter(GrowthParameter.ParameterType.DEX, afterUnit.dexterity.transmigrate - beforeUnit.dexterity.transmigrate, afterUnit.self_total_dexterity - afterUnit.dexterity.transmigrate, afterUnit.self_total_dexterity, false);
          instance.SetParameter(GrowthParameter.ParameterType.LUK, afterUnit.lucky.transmigrate - beforeUnit.lucky.transmigrate, afterUnit.self_total_lucky - afterUnit.lucky.transmigrate, afterUnit.self_total_lucky, false);
          break;
      }
      instance.SetParameterEffects();
      instance.SetParameterMaxStar(GrowthParameter.ParameterType.HP, afterUnit.hp.is_max);
      instance.SetParameterMaxStar(GrowthParameter.ParameterType.STR, afterUnit.strength.is_max);
      instance.SetParameterMaxStar(GrowthParameter.ParameterType.INT, afterUnit.intelligence.is_max);
      instance.SetParameterMaxStar(GrowthParameter.ParameterType.VIT, afterUnit.vitality.is_max);
      instance.SetParameterMaxStar(GrowthParameter.ParameterType.MND, afterUnit.mind.is_max);
      instance.SetParameterMaxStar(GrowthParameter.ParameterType.AGI, afterUnit.agility.is_max);
      instance.SetParameterMaxStar(GrowthParameter.ParameterType.DEX, afterUnit.dexterity.is_max);
      instance.SetParameterMaxStar(GrowthParameter.ParameterType.LUK, afterUnit.lucky.is_max);
    }
    else if (afterUnit.unit.IsNormalUnit && beforeUnit.unit.IsMaterialUnit)
    {
      instance.SetParameter(GrowthParameter.ParameterType.HP, 0, afterUnit.total_hp, afterUnit.total_hp, false);
      instance.SetParameter(GrowthParameter.ParameterType.STR, 0, afterUnit.total_strength, afterUnit.total_strength, false);
      instance.SetParameter(GrowthParameter.ParameterType.INT, 0, afterUnit.total_intelligence, afterUnit.total_intelligence, false);
      instance.SetParameter(GrowthParameter.ParameterType.VIT, 0, afterUnit.total_vitality, afterUnit.total_vitality, false);
      instance.SetParameter(GrowthParameter.ParameterType.MND, 0, afterUnit.total_mind, afterUnit.total_mind, false);
      instance.SetParameter(GrowthParameter.ParameterType.AGI, 0, afterUnit.total_agility, afterUnit.total_agility, false);
      instance.SetParameter(GrowthParameter.ParameterType.DEX, 0, afterUnit.total_dexterity, afterUnit.total_dexterity, false);
      instance.SetParameter(GrowthParameter.ParameterType.LUK, 0, afterUnit.total_lucky, afterUnit.total_lucky, false);
      instance.SetParameterEffects();
      instance.SetParameterMaxStar(GrowthParameter.ParameterType.HP, afterUnit.hp.is_max);
      instance.SetParameterMaxStar(GrowthParameter.ParameterType.STR, afterUnit.strength.is_max);
      instance.SetParameterMaxStar(GrowthParameter.ParameterType.INT, afterUnit.intelligence.is_max);
      instance.SetParameterMaxStar(GrowthParameter.ParameterType.VIT, afterUnit.vitality.is_max);
      instance.SetParameterMaxStar(GrowthParameter.ParameterType.MND, afterUnit.mind.is_max);
      instance.SetParameterMaxStar(GrowthParameter.ParameterType.AGI, afterUnit.agility.is_max);
      instance.SetParameterMaxStar(GrowthParameter.ParameterType.DEX, afterUnit.dexterity.is_max);
      instance.SetParameterMaxStar(GrowthParameter.ParameterType.LUK, afterUnit.lucky.is_max);
    }
    else
    {
      instance.DisableParameter(GrowthParameter.ParameterType.HP);
      instance.DisableParameter(GrowthParameter.ParameterType.STR);
      instance.DisableParameter(GrowthParameter.ParameterType.INT);
      instance.DisableParameter(GrowthParameter.ParameterType.VIT);
      instance.DisableParameter(GrowthParameter.ParameterType.MND);
      instance.DisableParameter(GrowthParameter.ParameterType.AGI);
      instance.DisableParameter(GrowthParameter.ParameterType.DEX);
      instance.DisableParameter(GrowthParameter.ParameterType.LUK);
      instance.SetParameterMaxStar(GrowthParameter.ParameterType.HP, false);
      instance.SetParameterMaxStar(GrowthParameter.ParameterType.STR, false);
      instance.SetParameterMaxStar(GrowthParameter.ParameterType.INT, false);
      instance.SetParameterMaxStar(GrowthParameter.ParameterType.VIT, false);
      instance.SetParameterMaxStar(GrowthParameter.ParameterType.MND, false);
      instance.SetParameterMaxStar(GrowthParameter.ParameterType.AGI, false);
      instance.SetParameterMaxStar(GrowthParameter.ParameterType.DEX, false);
      instance.SetParameterMaxStar(GrowthParameter.ParameterType.LUK, false);
    }
    e = this.SetUnitTexture(afterUnit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.sm = Singleton<NGSoundManager>.GetInstance();
    if (Object.op_Inequality((Object) this.sm, (Object) null))
    {
      this.sm.stopVoice();
      switch (mode)
      {
        case Unit00499Scene.Mode.Evolution:
        case Unit00499Scene.Mode.EarthEvolution:
        case Unit00499Scene.Mode.AwakeUnit:
        case Unit00499Scene.Mode.CommonAwakeUnit:
          if (afterUnit.unit.IsNormalUnit)
          {
            this.sm.playVoiceByID(afterUnit.unit.unitVoicePattern, 55);
            break;
          }
          break;
      }
    }
    this.DirAnimEvolution.SetActive(false);
    if (Object.op_Inequality((Object) this.DirAnimRevival, (Object) null))
      this.DirAnimRevival.SetActive(false);
    if (Object.op_Inequality((Object) this.DirAnimEvolutionX, (Object) null))
      this.DirAnimEvolutionX.SetActive(false);
    switch (mode)
    {
      case Unit00499Scene.Mode.Evolution:
      case Unit00499Scene.Mode.EarthEvolution:
      case Unit00499Scene.Mode.AwakeUnit:
      case Unit00499Scene.Mode.CommonAwakeUnit:
        this.DirAnimEvolution.SetActive(true);
        break;
      case Unit00499Scene.Mode.JobChange:
        if (afterJob.is_vertex_x)
        {
          this.DirAnimEvolutionX.SetActive(true);
          break;
        }
        this.DirAnimEvolution.SetActive(true);
        break;
      default:
        if (Object.op_Inequality((Object) this.DirAnimRevival, (Object) null))
        {
          this.DirAnimRevival.SetActive(true);
          break;
        }
        break;
    }
    if (mode == Unit00499Scene.Mode.JobChange)
    {
      Future<GameObject> ldFamilyIconPrefab = new ResourceObject("Prefabs/SkillFamily/SkillFamilyIcon").Load<GameObject>();
      yield return (object) ldFamilyIconPrefab.Wait();
      SkillfullnessIcon component = ldFamilyIconPrefab.Result.GetComponent<SkillfullnessIcon>();
      UnitFamilyValue[] unitFamilyValues1 = this.getUnitFamilyValues(beforeUnit);
      UnitFamilyValue[] unitFamilyValues2 = this.getUnitFamilyValues(afterUnit);
      if (((IEnumerable<UnitFamilyValue>) unitFamilyValues1).Select<UnitFamilyValue, int>((Func<UnitFamilyValue, int>) (x => x.ID)).SequenceEqual<int>(((IEnumerable<UnitFamilyValue>) unitFamilyValues2).Select<UnitFamilyValue, int>((Func<UnitFamilyValue, int>) (y => y.ID))))
      {
        this.setIconFamilies(this.iconBeforeFamilies, unitFamilyValues1, component);
        this.topAfterFamilies.SetActive(false);
      }
      else
      {
        this.setIconFamilies(this.iconBeforeFamilies, unitFamilyValues1, component);
        this.topAfterFamilies.SetActive(true);
        if (unitFamilyValues1.Length == 1)
        {
          if (!this.localXBeforeDoubleFamily.HasValue)
            this.localXBeforeDoubleFamily = new float?(this.topAfterFamilies.transform.localPosition.x);
          this.topAfterFamilies.transform.localPosition = Vector2.op_Implicit(new Vector2(this.localXBeforeSingleFamily, this.topAfterFamilies.transform.localPosition.y));
        }
        else if (this.localXBeforeDoubleFamily.HasValue)
          this.topAfterFamilies.transform.localPosition = Vector2.op_Implicit(new Vector2(this.localXBeforeDoubleFamily.Value, this.topAfterFamilies.transform.localPosition.y));
        this.setIconFamilies(this.iconAfterFamilies, unitFamilyValues2, component);
      }
      if (!Persist.jobXInfo.Data.tutorialShow && afterUnit.getJobData().is_vertex_x)
        this.jobChangeXDirty = true;
      ldFamilyIconPrefab = (Future<GameObject>) null;
    }
  }

  private void setParameterByJobChange(
    GrowthParameter panel,
    GrowthParameter.ParameterType type,
    int prev,
    int now)
  {
    panel.SetParameter(type, 0, Mathf.Min(now, prev), now, false);
    UILabel resultParameter = panel.resultParameters[(int) type];
    if (!Object.op_Inequality((Object) resultParameter, (Object) null))
      return;
    ((UIWidget) resultParameter).color = now == prev ? Color.white : (now > prev ? Color.yellow : Color.red);
  }

  private void setIconFamilies(
    UI2DSprite[] icons,
    UnitFamilyValue[] familyValues,
    SkillfullnessIcon iconPrefab)
  {
    for (int index = 0; index < icons.Length; ++index)
    {
      if (index >= familyValues.Length || iconPrefab.Icons.Length <= familyValues[index].ID)
      {
        ((Component) icons[index]).gameObject.SetActive(false);
      }
      else
      {
        icons[index].sprite2D = iconPrefab.Icons[familyValues[index].ID];
        ((Component) icons[index]).gameObject.SetActive(true);
      }
    }
  }

  private UnitFamilyValue[] getUnitFamilyValues(PlayerUnit pu)
  {
    UnitFamilyValue unitFamilyValue;
    UnitFamilyValue[] array = ((IEnumerable<UnitFamily>) pu.Families).OrderBy<UnitFamily, int>((Func<UnitFamily, int>) (f => (int) f)).Select<UnitFamily, UnitFamilyValue>((Func<UnitFamily, UnitFamilyValue>) (f => !MasterData.UnitFamilyValue.TryGetValue((int) f, out unitFamilyValue) ? (UnitFamilyValue) null : unitFamilyValue)).Where<UnitFamilyValue>((Func<UnitFamilyValue, bool>) (x => x != null && x.is_disp)).ToArray<UnitFamilyValue>();
    if (array.Length != 0)
      return array;
    return new UnitFamilyValue[1]
    {
      MasterData.UnitFamilyValue[0]
    };
  }
}
