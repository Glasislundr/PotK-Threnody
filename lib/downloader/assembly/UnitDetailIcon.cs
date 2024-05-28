// Decompiled with JetBrains decompiler
// Type: UnitDetailIcon
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
public class UnitDetailIcon : UnitIconBase
{
  public GameObject unitIconPrefab;
  public GameObject unitIconParent;
  public UI2DSprite upiconSpriteAtk;
  public UI2DSprite upiconSpriteDef;
  public UI2DSprite upiconSpriteHp;
  public UI2DSprite upiconSpriteMatk;
  public UI2DSprite upiconSpriteMtl;
  public UI2DSprite upiconSpriteSpe;
  public UI2DSprite upiconSpriteTec;
  public UI2DSprite upiconSpriteLuck;
  public UI2DSprite[] upiconSprites;
  public GameObject[] upiconEffect;
  public UILabel txtPossession;
  [SerializeField]
  private GameObject breakThrough;
  [SerializeField]
  private BlinkSync blink;
  [SerializeField]
  private GameObject blinkBreakthrough;
  [SerializeField]
  private GameObject blinkSkillUp;
  [SerializeField]
  private GameObject blinkDeardegreeup;
  [SerializeField]
  private GameObject blinkRelevancedegreeup;
  [SerializeField]
  private GameObject blinkUnityValue;
  [SerializeField]
  private UILabel txtBlinkUnityValue;
  [SerializeField]
  private GameObject blinkBuildupUnityValue;
  [SerializeField]
  private UILabel txtBlinkBuildupUnityValue;
  [SerializeField]
  private GameObject skillUp;
  [SerializeField]
  private GameObject deardegreeup;
  [SerializeField]
  private GameObject relevancedegreeup;
  [SerializeField]
  private GameObject unityValue;
  [SerializeField]
  private UILabel txtUnityValue;
  [SerializeField]
  private GameObject buildupUnityValue;
  [SerializeField]
  private UILabel txtBuildupUnityValue;
  public static readonly int Width = 120;
  public static readonly int Height = 264;
  public static readonly int ColumnValue = 5;
  public static readonly int RowValue = 6;
  public static readonly int RowScreenValue = 3;
  public static readonly int ScreenValue = UnitDetailIcon.ColumnValue * UnitDetailIcon.RowScreenValue;
  public static readonly int MaxValue = UnitDetailIcon.ColumnValue * UnitDetailIcon.RowValue;
  public static readonly int SKILL_ID_ALL = 99;
  private static Dictionary<int, UnitDetailIcon.SpriteCache> spriteCache = new Dictionary<int, UnitDetailIcon.SpriteCache>();

  public bool BreakThrough
  {
    get => this.breakThrough.activeSelf;
    set => this.breakThrough.SetActive(value);
  }

  public static void ClearCache() => UnitDetailIcon.spriteCache.Clear();

  public static bool IsCache(UnitUnit unit) => UnitDetailIcon.spriteCache.ContainsKey(unit.ID);

  public static IEnumerator LoadSprite(UnitUnit unit)
  {
    if (unit != null && !UnitDetailIcon.spriteCache.ContainsKey(unit.ID))
    {
      Future<Sprite> spriteF = unit.LoadSpriteThumbnail();
      IEnumerator e = spriteF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Sprite sprite = spriteF.Result;
      Future<GameObject> SetGearPrefab = Res.Icons.GearKindIcon.Load<GameObject>();
      e = SetGearPrefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject result = SetGearPrefab.Result;
      UnitDetailIcon.spriteCache[unit.ID] = new UnitDetailIcon.SpriteCache(sprite, result);
    }
  }

  private void Awake()
  {
    this.UnitIcon = this.unitIconParent.GetComponentInChildren<UnitIcon>(true);
    if (Object.op_Equality((Object) this.UnitIcon, (Object) null))
      this.UnitIcon = this.unitIconPrefab.CloneAndGetComponent<UnitIcon>(this.unitIconParent);
    this.UnitIcon.BottomBaseObject = false;
    this.UnitIcon.isViewBackObject = false;
    ((Collider) this.UnitIcon.buttonBoxCollider).enabled = false;
    this.txtLabel = this.UnitIcon.TxtLabel;
    this.for_battle = this.UnitIcon.for_battle;
    this.tower_entry = this.UnitIcon.tower_entry;
    this.can_awake = this.UnitIcon.can_awake;
    this.unitRental = this.UnitIcon.unitRental;
    this.overkillers = this.UnitIcon.overkillers;
    this.blinkDeckStatus = this.UnitIcon.blinkDeckStatus;
    this.equip = this.UnitIcon.equip;
    this.SelectNumberSprites = this.UnitIcon.SelectNumberSprites;
    this.SelectNumberBase = this.UnitIcon.SelectNumberBase;
    this.SelectNumber = this.UnitIcon.SelectNumber;
  }

  public bool SkillUp
  {
    get => this.skillUp.activeSelf;
    set => this.skillUp.SetActive(value);
  }

  public bool Deardegreeup
  {
    get => this.deardegreeup.activeSelf;
    set => this.deardegreeup.SetActive(value);
  }

  public bool Relevancedegreeup
  {
    get => this.relevancedegreeup.activeSelf;
    set => this.relevancedegreeup.SetActive(value);
  }

  public bool UnityValue
  {
    get => this.unityValue.activeSelf;
    set => this.unityValue.SetActive(value);
  }

  public void SetSprite(UnitUnit unit)
  {
    ((IEnumerable<UI2DSprite>) this.upiconSprites).ForEach<UI2DSprite>((Action<UI2DSprite>) (x => ((Component) x).gameObject.SetActive(true)));
    ((IEnumerable<GameObject>) this.upiconEffect).ForEach<GameObject>((Action<GameObject>) (x => x.SetActive(false)));
    int num = 0;
    if (!unit.IsBuildup)
    {
      if (num < 4 && unit.hp_compose != 0)
        this.upiconSprites[num++].sprite2D = this.upiconSpriteHp.sprite2D;
      if (num < 4 && unit.strength_compose != 0)
        this.upiconSprites[num++].sprite2D = this.upiconSpriteAtk.sprite2D;
      if (num < 4 && unit.vitality_compose != 0)
        this.upiconSprites[num++].sprite2D = this.upiconSpriteDef.sprite2D;
      if (num < 4 && unit.intelligence_compose != 0)
        this.upiconSprites[num++].sprite2D = this.upiconSpriteMatk.sprite2D;
      if (num < 4 && unit.mind_compose != 0)
        this.upiconSprites[num++].sprite2D = this.upiconSpriteMtl.sprite2D;
      if (num < 4 && unit.agility_compose != 0)
        this.upiconSprites[num++].sprite2D = this.upiconSpriteSpe.sprite2D;
      if (num < 4 && unit.dexterity_compose != 0)
        this.upiconSprites[num++].sprite2D = this.upiconSpriteTec.sprite2D;
      if (num < 4 && unit.lucky_compose != 0)
        this.upiconSprites[num++].sprite2D = this.upiconSpriteLuck.sprite2D;
    }
    else
    {
      if (num < 4 && unit.hp_buildup != 0)
        this.upiconSprites[num++].sprite2D = this.upiconSpriteHp.sprite2D;
      if (num < 4 && unit.strength_buildup != 0)
        this.upiconSprites[num++].sprite2D = this.upiconSpriteAtk.sprite2D;
      if (num < 4 && unit.vitality_buildup != 0)
        this.upiconSprites[num++].sprite2D = this.upiconSpriteDef.sprite2D;
      if (num < 4 && unit.intelligence_buildup != 0)
        this.upiconSprites[num++].sprite2D = this.upiconSpriteMatk.sprite2D;
      if (num < 4 && unit.mind_buildup != 0)
        this.upiconSprites[num++].sprite2D = this.upiconSpriteMtl.sprite2D;
      if (num < 4 && unit.agility_buildup != 0)
        this.upiconSprites[num++].sprite2D = this.upiconSpriteSpe.sprite2D;
      if (num < 4 && unit.dexterity_buildup != 0)
        this.upiconSprites[num++].sprite2D = this.upiconSpriteTec.sprite2D;
      if (num < 4 && unit.lucky_buildup != 0)
        this.upiconSprites[num++].sprite2D = this.upiconSpriteLuck.sprite2D;
      for (int index = 0; index < num; ++index)
      {
        this.upiconEffect[index].SetActive(true);
        TweenColor component = this.upiconEffect[index].GetComponent<TweenColor>();
        ((UITweener) component).ResetToBeginning();
        ((UITweener) component).PlayForward();
      }
    }
    while (num < 4)
      ((Component) this.upiconSprites[num++]).gameObject.SetActive(false);
  }

  private void InitializeLabel()
  {
    ((Component) this.blink).gameObject.SetActive(false);
    this.BreakThrough = false;
    this.SkillUp = false;
    this.Deardegreeup = false;
    this.Relevancedegreeup = false;
    this.UnityValue = false;
  }

  private void SetLabel(PlayerUnit playerUnit, PlayerUnit basePlayerUnit, bool isTrustMaterial = false)
  {
    UnitUnit unit1 = playerUnit.unit;
    UnitUnit unit2 = basePlayerUnit.unit;
    this.InitializeLabel();
    int num = 0;
    float unity = 0.0f;
    bool flag1 = false;
    bool flag2 = false;
    if (basePlayerUnit.unity_value < PlayerUnit.GetUnityValueMax())
    {
      if (unit1.IsNormalUnit)
      {
        if (unit2.same_character_id == unit1.same_character_id)
        {
          num = Mathf.Min(playerUnit.unity_value + PlayerUnit.GetUnityValue(), PlayerUnit.GetUnityValueMax());
          unity = Mathf.Min(playerUnit.buildup_unity_value_f, (float) PlayerUnit.GetUnityValueMax());
        }
      }
      else if (unit1.is_unity_value_up)
      {
        if ((double) basePlayerUnit.unityTotal < (double) PlayerUnit.GetUnityValueMax())
        {
          UnityValueUpPattern valueUpPattern = unit1.FindValueUpPattern(unit2, (Func<UnitFamily[]>) (() => basePlayerUnit.Families));
          if (valueUpPattern != null)
            unity = valueUpPattern.up_value;
        }
        if (unit1.FindPureValueUpPattern(unit2) != null)
          num = 1;
      }
      if (num > 0)
        flag1 = true;
      if ((double) unity > 0.0)
        flag2 = true;
    }
    bool flag3 = !unit1.IsBreakThrough ? (!unit1.IsPureValueUp ? unit2.same_character_id == unit1.same_character_id && basePlayerUnit.breakthrough_count < unit2.breakthrough_limit : unit1.IsBreakThrougPureValueUp(basePlayerUnit)) : unit1.CheckBreakThroughMaterial(basePlayerUnit);
    IEnumerable<PlayerUnitSkills> source1 = ((IEnumerable<PlayerUnitSkills>) basePlayerUnit.skills).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => x.level < x.skill.upper_level));
    bool flag4 = (unit1.IsMaterialUnitSkillUp || unit1.same_character_id == unit2.same_character_id) && source1.Count<PlayerUnitSkills>() > 0 || source1.Any<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (unitBase => playerUnit.skills != null && ((IEnumerable<PlayerUnitSkills>) playerUnit.skills).Count<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (unit => unitBase.skill_id == unit.skill_id)) > 0));
    bool flag5 = false;
    if (!flag4)
      flag5 = UnitDetailIcon.IsSkillUpMaterial(unit1, basePlayerUnit) | unit1.IsSkillLevelUpPureValueUp(basePlayerUnit);
    bool flag6 = false;
    if (unit2.trust_target_flag && (double) basePlayerUnit.trust_rate < (double) basePlayerUnit.trust_max_rate && (unit1.same_character_id == unit2.same_character_id || unit1.IsPureValueUp))
      flag6 = true;
    bool flag7 = unit2.trust_target_flag && (double) basePlayerUnit.trust_rate < (double) basePlayerUnit.trust_max_rate && unit1.character.ID == unit2.character.ID;
    this.BreakThrough = false;
    this.SkillUp = false;
    this.Deardegreeup = false;
    this.Relevancedegreeup = false;
    this.UnityValue = false;
    bool[] source2 = new bool[5]
    {
      flag3,
      flag4 | flag5,
      flag6 | isTrustMaterial | flag7,
      flag1,
      flag2
    };
    if (((IEnumerable<bool>) source2).Count<bool>((Func<bool, bool>) (b => b)) > 1)
    {
      GameObject[] gameObjectArray = new GameObject[5]
      {
        this.blinkBreakthrough,
        this.blinkSkillUp,
        this.getBlinkDegreeup(unit2),
        this.blinkUnityValue,
        this.blinkBuildupUnityValue
      };
      foreach (GameObject gameObject in gameObjectArray)
        gameObject.SetActive(false);
      List<GameObject> blinks = new List<GameObject>();
      for (int index = 0; index < source2.Length; ++index)
      {
        if (source2[index])
        {
          GameObject gameObject = gameObjectArray[index];
          blinks.Add(gameObject);
          gameObject.SetActive(true);
        }
      }
      this.blink.resetBlinks((IEnumerable<GameObject>) blinks);
      ((Component) this.blink).gameObject.SetActive(true);
      this.txtBlinkUnityValue.SetTextLocalize(string.Format(Consts.GetInstance().unit_004_8_4_plus_unity_value, (object) num));
      this.txtBlinkBuildupUnityValue.SetTextLocalize(string.Format(Consts.GetInstance().unit_004_8_4_plus_buildup_unity_value, (object) PlayerUnit.UnityToPercent(unity)));
    }
    else
    {
      GameObject[] gameObjectArray = new GameObject[5]
      {
        this.breakThrough,
        this.skillUp,
        unit2.IsSea ? this.deardegreeup : (unit2.IsResonanceUnit ? this.relevancedegreeup : this.deardegreeup),
        this.unityValue,
        this.buildupUnityValue
      };
      for (int index = 0; index < source2.Length; ++index)
        gameObjectArray[index].SetActive(source2[index]);
      this.txtUnityValue.SetTextLocalize(string.Format(Consts.GetInstance().unit_004_8_4_plus_unity_value, (object) num));
      this.txtBuildupUnityValue.SetTextLocalize(string.Format(Consts.GetInstance().unit_004_8_4_plus_buildup_unity_value, (object) PlayerUnit.UnityToPercent(unity)));
    }
  }

  private GameObject getBlinkDegreeup(UnitUnit baseU)
  {
    if (baseU.IsSea)
    {
      this.blinkRelevancedegreeup.SetActive(false);
      return this.blinkDeardegreeup;
    }
    if (baseU.IsResonanceUnit)
    {
      this.blinkDeardegreeup.SetActive(false);
      return this.blinkRelevancedegreeup;
    }
    this.blinkRelevancedegreeup.SetActive(false);
    return this.blinkDeardegreeup;
  }

  public override IEnumerator SetMaterialUnit(
    PlayerUnit playerUnit,
    bool isNew,
    PlayerUnit[] playerUnits,
    bool isTrust)
  {
    UnitDetailIcon unitDetailIcon = this;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = unitDetailIcon.\u003C\u003En__0(playerUnit, isNew, playerUnits);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (playerUnit != (PlayerUnit) null)
      EventDelegate.Set(unitDetailIcon.Button.onLongPress, (EventDelegate.Callback) (() =>
      {
        if (this.pressEvent != null)
          this.pressEvent();
        Unit0042Scene.changeScene(true, playerUnit, playerUnits, true);
      }));
    unitDetailIcon.InitializeLabel();
    e = unitDetailIcon.SetUnit(unitDetailIcon.playerUnit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unitDetailIcon.SetSprite(playerUnit.unit);
    if (Object.op_Inequality((Object) unitDetailIcon.txtPossession, (Object) null))
    {
      PlayerMaterialUnit playerMaterialUnit = ((IEnumerable<PlayerMaterialUnit>) SMManager.Get<PlayerMaterialUnit[]>()).FirstOrDefault<PlayerMaterialUnit>((Func<PlayerMaterialUnit, bool>) (x => x.id == playerUnit.id));
      if (playerMaterialUnit != null)
      {
        ((Component) unitDetailIcon.txtPossession).gameObject.SetActive(true);
        unitDetailIcon.txtPossession.SetTextLocalize(Consts.Format(Consts.GetInstance().unit_004_9_9_possession_text, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) playerMaterialUnit.quantity
          }
        }));
      }
      else
        ((Component) unitDetailIcon.txtPossession).gameObject.SetActive(false);
    }
  }

  public override IEnumerator SetMaterialUnit(
    PlayerUnit playerUnit,
    PlayerUnit basePlayerUnit,
    bool isNew,
    PlayerUnit[] playerUnits,
    bool isTrust)
  {
    UnitDetailIcon unitDetailIcon = this;
    unitDetailIcon.playerUnit = unitDetailIcon.UnitIcon.PlayerUnit = playerUnit;
    IEnumerator e = unitDetailIcon.SetMaterialUnit(playerUnit, isNew, playerUnits, isTrust);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unitDetailIcon.SetLabel(playerUnit, basePlayerUnit, isTrust);
  }

  public static bool IsSkillUpMaterial(UnitUnit unit, PlayerUnit baseUnit)
  {
    bool flag = false;
    if (unit.skillup_type != 0)
    {
      PlayerUnitSkills[] array = ((IEnumerable<PlayerUnitSkills>) baseUnit.skills).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => x.level < x.skill.upper_level)).ToArray<PlayerUnitSkills>();
      if (array.Length != 0)
      {
        int materialSkillUpType = unit.skillup_type;
        flag = materialSkillUpType == UnitDetailIcon.SKILL_ID_ALL || ((IEnumerable<PlayerUnitSkills>) array).Any<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (target => target.skill.skill_type == (BattleskillSkillType) materialSkillUpType));
        UnitSkillupSetting skillupSetting = ((IEnumerable<UnitSkillupSetting>) MasterData.UnitSkillupSettingList).FirstOrDefault<UnitSkillupSetting>((Func<UnitSkillupSetting, bool>) (x => x.material_unit_id == unit.ID));
        if (skillupSetting != null && skillupSetting.skill_group.HasValue)
        {
          IEnumerable<int> skillIDList = ((IEnumerable<UnitSkillupSkillGroupSetting>) MasterData.UnitSkillupSkillGroupSettingList).Where<UnitSkillupSkillGroupSetting>((Func<UnitSkillupSkillGroupSetting, bool>) (x => x.group_id == skillupSetting.skill_group.Value)).Select<UnitSkillupSkillGroupSetting, int>((Func<UnitSkillupSkillGroupSetting, int>) (x => x.skill_id));
          flag = flag && ((IEnumerable<PlayerUnitSkills>) array).Any<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => skillIDList.Contains<int>(x.skill.ID)));
        }
      }
    }
    return flag;
  }

  public override IEnumerator SetPlayerUnit(
    PlayerUnit playerUnit,
    PlayerUnit[] playerUnits,
    PlayerUnit basePlayerUnit = null,
    bool isMaterial = false,
    bool isMemory = false)
  {
    UnitDetailIcon unitDetailIcon = this;
    unitDetailIcon.playerUnit = unitDetailIcon.UnitIcon.PlayerUnit = playerUnit;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = unitDetailIcon.\u003C\u003En__1(playerUnit, playerUnits, basePlayerUnit, isMaterial, isMemory);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (playerUnit != (PlayerUnit) null)
      EventDelegate.Set(unitDetailIcon.Button.onLongPress, (EventDelegate.Callback) (() =>
      {
        if (this.pressEvent != null)
          this.pressEvent();
        if (Singleton<NGGameDataManager>.GetInstance().IsSea)
          Singleton<NGSceneManager>.GetInstance().LastHeaderType = new CommonRoot.HeaderType?(Singleton<CommonRoot>.GetInstance().headerType);
        Unit0042Scene.changeScene(true, playerUnit, playerUnits, isMaterial, isMemory);
      }));
    unitDetailIcon.SetSprite(playerUnit.unit);
    unitDetailIcon.UnitIcon.princessType.SetPrincessType(playerUnit);
    e = unitDetailIcon.SetUnit(unitDetailIcon.playerUnit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (basePlayerUnit != (PlayerUnit) null)
    {
      unitDetailIcon.SetLabel(playerUnit, basePlayerUnit);
      NGTween.playTweens(((Component) unitDetailIcon).GetComponentsInChildren<UITweener>(true), NGTween.Kind.GRAYOUT, true);
    }
    if (Object.op_Inequality((Object) unitDetailIcon.txtPossession, (Object) null))
      ((Component) unitDetailIcon.txtPossession).gameObject.SetActive(false);
  }

  public IEnumerator SetPlayerUnitEvolution(
    PlayerUnit playerUnit,
    PlayerUnit[] playerUnits,
    PlayerUnit basePlayerUnit = null)
  {
    UnitDetailIcon unitDetailIcon = this;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = unitDetailIcon.\u003C\u003En__1(playerUnit, playerUnits, basePlayerUnit, false, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (playerUnit != (PlayerUnit) null)
      EventDelegate.Set(unitDetailIcon.Button.onLongPress, (EventDelegate.Callback) (() =>
      {
        if (this.pressEvent != null)
          this.pressEvent();
        Unit0042Scene.changeSceneEvolutionUnit(true, playerUnit, playerUnits);
      }));
    unitDetailIcon.SetSprite(playerUnit.unit);
    e = unitDetailIcon.SetUnit(unitDetailIcon.playerUnit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (basePlayerUnit != (PlayerUnit) null)
    {
      unitDetailIcon.SetLabel(playerUnit, basePlayerUnit);
      NGTween.playTweens(((Component) unitDetailIcon).GetComponentsInChildren<UITweener>(true), NGTween.Kind.GRAYOUT, true);
    }
    if (Object.op_Inequality((Object) unitDetailIcon.txtPossession, (Object) null))
      ((Component) unitDetailIcon.txtPossession).gameObject.SetActive(false);
  }

  public override IEnumerator SetUnit(UnitUnit unit, CommonElement element, bool isGray = false)
  {
    UnitDetailIcon unitDetailIcon = this;
    IEnumerator e = unitDetailIcon.UnitIcon.SetUnit(unit, unit.GetElement(), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    // ISSUE: reference to a compiler-generated method
    e = unitDetailIcon.\u003C\u003En__2(unit, unit.GetElement(), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unitDetailIcon.UnitIcon.setLevelText(unitDetailIcon.playerUnit);
    unitDetailIcon.UnitIcon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.BranchOfAnArmy);
    unitDetailIcon.UnitIcon.setCostText(unitDetailIcon.playerUnit);
    unitDetailIcon.UnitIcon.Favorite = unitDetailIcon.playerUnit.favorite;
    unitDetailIcon.UnitIcon.SpecialIcon = false;
    unitDetailIcon.UnitIcon.SpecialIconType = -1;
    unitDetailIcon.BreakThrough |= unitDetailIcon.playerUnit.unit.IsBreakThrough;
    unitDetailIcon.SkillUp |= unitDetailIcon.playerUnit.unit.IsMaterialUnitSkillUp;
    unitDetailIcon.Deardegreeup = !unitDetailIcon.playerUnit.unit.IsBreakThrough && !unitDetailIcon.playerUnit.unit.IsMaterialUnitSkillUp && unitDetailIcon.playerUnit.unit.trust_target_flag && (double) unitDetailIcon.playerUnit.trust_rate < (double) unitDetailIcon.playerUnit.trust_max_rate && unitDetailIcon.playerUnit.unit.IsSea;
    unitDetailIcon.Relevancedegreeup = !unitDetailIcon.playerUnit.unit.IsBreakThrough && !unitDetailIcon.playerUnit.unit.IsMaterialUnitSkillUp && unitDetailIcon.playerUnit.unit.trust_target_flag && (double) unitDetailIcon.playerUnit.trust_rate < (double) unitDetailIcon.playerUnit.trust_max_rate && unitDetailIcon.playerUnit.unit.IsResonanceUnit;
    unitDetailIcon.SetSprite(unit);
  }

  public IEnumerator SetUnit(PlayerUnit playerUnit, bool isGray = false)
  {
    UnitDetailIcon unitDetailIcon = this;
    IEnumerator e = unitDetailIcon.UnitIcon.SetUnit(playerUnit, playerUnit.GetElement(), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    // ISSUE: reference to a compiler-generated method
    e = unitDetailIcon.\u003C\u003En__2(playerUnit.unit, playerUnit.GetElement(), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unitDetailIcon.UnitIcon.setLevelText(unitDetailIcon.playerUnit);
    unitDetailIcon.UnitIcon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.BranchOfAnArmy);
    unitDetailIcon.UnitIcon.setCostText(unitDetailIcon.playerUnit);
    unitDetailIcon.UnitIcon.Favorite = unitDetailIcon.playerUnit.favorite;
    unitDetailIcon.UnitIcon.princessType.SetPrincessType(playerUnit);
    unitDetailIcon.UnitIcon.SpecialIcon = false;
    unitDetailIcon.UnitIcon.SpecialIconType = -1;
    unitDetailIcon.BreakThrough |= unitDetailIcon.playerUnit.unit.IsBreakThrough;
    unitDetailIcon.SkillUp |= unitDetailIcon.playerUnit.unit.IsMaterialUnitSkillUp;
    unitDetailIcon.Deardegreeup = ((unitDetailIcon.Deardegreeup ? 1 : 0) | (unitDetailIcon.playerUnit.unit.IsBreakThrough || unitDetailIcon.playerUnit.unit.IsMaterialUnitSkillUp || !unitDetailIcon.playerUnit.unit.trust_target_flag || (double) unitDetailIcon.playerUnit.trust_rate >= (double) unitDetailIcon.playerUnit.trust_max_rate ? 0 : (unitDetailIcon.playerUnit.unit.IsSea ? 1 : 0))) != 0;
    unitDetailIcon.Relevancedegreeup = ((unitDetailIcon.Relevancedegreeup ? 1 : 0) | (unitDetailIcon.playerUnit.unit.IsBreakThrough || unitDetailIcon.playerUnit.unit.IsMaterialUnitSkillUp || !unitDetailIcon.playerUnit.unit.trust_target_flag || (double) unitDetailIcon.playerUnit.trust_rate >= (double) unitDetailIcon.playerUnit.trust_max_rate ? 0 : (unitDetailIcon.playerUnit.unit.IsResonanceUnit ? 1 : 0))) != 0;
    unitDetailIcon.SetSprite(playerUnit.unit);
  }

  public void ResetUnit()
  {
    this.Deselect();
    if (Object.op_Inequality((Object) this.defaultIconSprite, (Object) null))
    {
      this.icon.sprite2D = this.defaultIconSprite;
      ((UIWidget) this.icon).width = ((Texture) this.defaultIconSprite.texture).width;
      ((UIWidget) this.icon).height = ((Texture) this.defaultIconSprite.texture).height;
    }
    this.UnitIcon.BackgroundModeValue = UnitIcon.BackgroundMode.PlayerShadow;
    ((Component) this.UnitIcon.RarityStar).gameObject.SetActive(false);
    ((Component) this.UnitIcon.type).gameObject.SetActive(false);
    this.UnitIcon.BottomModeValue = UnitIconBase.BottomMode.Nothing;
  }

  public IEnumerator SetSelectUnit()
  {
    int num = 0;
    while (num < 4)
      ((Component) this.upiconSprites[num++]).gameObject.SetActive(false);
    this.UnitIcon.SetEmpty();
    this.UnitIcon.SelectUnit = true;
    ((Component) this.txtPossession).gameObject.SetActive(false);
    return this.UnitIcon.SetSelectUnit();
  }

  public void SetMaterialUnitCache(
    PlayerUnit playerUnit,
    bool isNew,
    PlayerUnit[] playerUnits,
    PlayerUnit basePlayerUnit = null,
    bool isTrust = false)
  {
    this.playerUnit = this.UnitIcon.PlayerUnit = playerUnit;
    EventDelegate.Set(this.Button.onLongPress, (EventDelegate.Callback) (() =>
    {
      if (this.pressEvent != null)
        this.pressEvent();
      Unit0042Scene.changeScene(true, playerUnit, playerUnits, true);
    }));
    this.UnitIcon.princessType.DispPrincessType(false);
    this.InitializeLabel();
    if (basePlayerUnit != (PlayerUnit) null)
    {
      this.SetLabel(playerUnit, basePlayerUnit, isTrust);
      NGTween.playTweens(((Component) this).GetComponentsInChildren<UITweener>(true), NGTween.Kind.GRAYOUT, true);
    }
    this.SetUnitCache();
    this.BreakThrough |= playerUnit.unit.IsBreakThrough;
    this.SkillUp |= playerUnit.unit.IsMaterialUnitSkillUp;
    if (!Object.op_Inequality((Object) this.txtPossession, (Object) null))
      return;
    PlayerMaterialUnit playerMaterialUnit = ((IEnumerable<PlayerMaterialUnit>) SMManager.Get<PlayerMaterialUnit[]>()).FirstOrDefault<PlayerMaterialUnit>((Func<PlayerMaterialUnit, bool>) (x => x.id == playerUnit.id));
    if (playerMaterialUnit != null)
    {
      ((Component) this.txtPossession).gameObject.SetActive(true);
      this.txtPossession.SetTextLocalize(Consts.Format(Consts.GetInstance().unit_004_9_9_possession_text, (IDictionary) new Hashtable()
      {
        {
          (object) "Count",
          (object) playerMaterialUnit.quantity
        }
      }));
    }
    else
      ((Component) this.txtPossession).gameObject.SetActive(false);
  }

  public void SetPlayerUnitCache(
    PlayerUnit playerUnit,
    PlayerUnit[] playerUnits,
    PlayerUnit basePlayerUnit = null,
    bool isMaterial = false,
    bool isMemory = false)
  {
    this.playerUnit = this.UnitIcon.PlayerUnit = playerUnit;
    EventDelegate.Set(this.Button.onLongPress, (EventDelegate.Callback) (() =>
    {
      if (this.pressEvent != null)
        this.pressEvent();
      if (Singleton<NGGameDataManager>.GetInstance().IsSea)
        Singleton<NGSceneManager>.GetInstance().LastHeaderType = new CommonRoot.HeaderType?(Singleton<CommonRoot>.GetInstance().headerType);
      Unit0042Scene.changeScene(true, playerUnit, playerUnits, isMaterial, isMemory);
    }));
    this.SetSprite(playerUnit.unit);
    this.UnitIcon.princessType.SetPrincessType(playerUnit);
    if (basePlayerUnit != (PlayerUnit) null)
    {
      this.SetLabel(playerUnit, basePlayerUnit);
      NGTween.playTweens(((Component) this).GetComponentsInChildren<UITweener>(true), NGTween.Kind.GRAYOUT, true);
    }
    this.SetUnitCache();
    if (!Object.op_Inequality((Object) this.txtPossession, (Object) null))
      return;
    ((Component) this.txtPossession).gameObject.SetActive(false);
  }

  private void SetUnitCache()
  {
    this.unit = this.playerUnit.unit;
    this.UnitIcon.SetUnitCache(this.unit, this.playerUnit.GetElement(), this.playerUnit.getJobData().ID);
    this.UnitIcon.setLevelText(this.playerUnit);
    this.UnitIcon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.BranchOfAnArmy);
    this.UnitIcon.setCostText(this.playerUnit);
    this.UnitIcon.Favorite = this.playerUnit.favorite;
    this.UnitIcon.SpecialIcon = false;
    this.UnitIcon.SpecialIconType = -1;
    this.SetSprite(this.unit);
  }

  protected override void showBottomInfoEx(UnitSortAndFilter.SORT_TYPES sort, GameObject target)
  {
    this.UnitIcon.ShowBottomInfo(sort);
  }

  private class SpriteCache
  {
    public Sprite thumbnail;
    public GameObject gear;

    public SpriteCache(Sprite s, GameObject o)
    {
      this.thumbnail = s;
      this.gear = o;
    }
  }
}
