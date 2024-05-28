// Decompiled with JetBrains decompiler
// Type: Gacha00611Menu
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
public class Gacha00611Menu : EquipmentDetailMenuBase
{
  [SerializeField]
  protected UILabel TxtAttack;
  [SerializeField]
  protected UILabel TxtCritical;
  [SerializeField]
  protected UILabel TxtDefense;
  [SerializeField]
  protected UILabel TxtDexterity;
  [SerializeField]
  protected UILabel TxtEvasion;
  [SerializeField]
  protected UILabel TxtExp;
  [SerializeField]
  protected UILabel TxtNextExp;
  [SerializeField]
  protected UILabel TxtMagicAttack;
  [SerializeField]
  protected UILabel TxtMagicDefense;
  [SerializeField]
  protected UILabel TxtRange;
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  protected UILabel TxtWait;
  [SerializeField]
  protected UILabel TxtWeapontype;
  [SerializeField]
  public GearKindIcon Weapon;
  [SerializeField]
  private SprGearAttack WeaponSpAttack;
  [SerializeField]
  private UILabel TxtWeaponAttack;
  [SerializeField]
  private SprGearElement WeaponSpElement;
  [SerializeField]
  private UILabel TxtWeaponElement;
  [SerializeField]
  protected UI2DSprite DynWeaponIll;
  [SerializeField]
  protected Transform DynWeaponModel;
  [SerializeField]
  protected GameObject NewIcon;
  [SerializeField]
  protected UISprite SlcGauge;
  [SerializeField]
  private Transform TopStarPos;
  [SerializeField]
  private GameObject charaThum;
  [SerializeField]
  private UIButton btnBack;
  [SerializeField]
  private GameObject DirAddStauts;
  [SerializeField]
  public GameObject SkillArrowObject;
  [SerializeField]
  public GameObject WeaponSkillOneRoot;
  [SerializeField]
  public UIButton[] WeaponSkillButtonOne;
  [SerializeField]
  public BattleSkillIcon[] WeaponSkillOne;
  [SerializeField]
  public GameObject WeaponSkillTwoRoot;
  [SerializeField]
  public UIButton[] WeaponSkillButtonTwo;
  [SerializeField]
  public BattleSkillIcon[] WeaponSkillTwo;
  public GameObject unitPrefab;
  public GameObject SkillDialog;
  [SerializeField]
  public UI2DSprite rarityStarsIcon;
  [SerializeField]
  private GameObject floatingSkillDialog;
  private Action<GearGearSkill, bool> showSkillDialog;
  [SerializeField]
  private GameObject rankUpObject;
  [SerializeField]
  private GameObject maxRankUpObject;
  [SerializeField]
  private GameObject dirEquipNone;
  [SerializeField]
  private GameObject dirEquipUnit;
  [SerializeField]
  private GameObject remainingManaSeedContainer;
  [SerializeField]
  private UILabel remainingManaSeedLabel;
  private Unit004431Menu.Param sendParam = new Unit004431Menu.Param();
  private bool isRankup;
  private bool isMaxRankup;
  private int maxWidth;
  private Decimal startGauge;
  private Decimal addGauge;
  private int base_gear_level;
  private bool isStopSe;
  private bool isEndGaugeAnim;
  private GameCore.ItemInfo target;
  private List<GearGearSkill> evoSkill;
  private List<GearGearSkill> addSkill;
  private List<GearReisouSkill> addReisouSkill;
  private GameObject skillGetPrefab;
  private GameObject commonElementIconPrefab;
  private GameObject popupAttackClassPrefab;
  private GameObject popupAttachedElementPrefab;
  protected string seGaugeUp = "SE_1053";
  protected string seRankUp = "SE_1054";
  protected string seSkillAwaked = "SE_1055";
  [SerializeField]
  protected GameObject DirReisou;
  [SerializeField]
  protected GameObject DynReisouIcon;
  [SerializeField]
  protected GameObject rankUpReisouObject;
  [SerializeField]
  protected GameObject dynNotificationGetItem;
  protected int? addReisouJewel;
  [SerializeField]
  protected Gacha00611Menu.ReisouExpGauge HolyReisouExpGauge;
  [SerializeField]
  protected Gacha00611Menu.ReisouExpGauge ChaosReisouExpGauge;
  protected PlayerItem baseReisouInfo;
  protected PlayerItem targetReisouInfo;
  protected ItemIcon reisouIcon;
  [SerializeField]
  protected GameObject dirBottom;
  private const int defaultIllustPosY = 123;
  private const int reisouIllustPosY = 0;
  private Action cahngeSceneCallback;
  private static readonly string COLOR_TAG_GREEN = "[00ff00]{0}[-]";
  private static readonly string COLOR_TAG_RED = "[ff0000]{0}[-]";
  private static readonly string COLOR_TAG_YELLOW = "[ffff00]{0}[-]";
  private bool isPlaying;
  private bool isPlaySkip;

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet() || !((Component) this.btnBack).GetComponent<Collider>().enabled)
      return;
    this.doBackScene();
  }

  private void doBackScene()
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    if (this.cahngeSceneCallback != null)
      this.cahngeSceneCallback();
    else
      this.backScene();
    ((Component) this.btnBack).GetComponent<Collider>().enabled = false;
  }

  public UIButton BackSceneButton => this.btnBack;

  public IEnumerator Initialize(
    GameCore.ItemInfo TargetInfo,
    bool NewFlag,
    int countWeapon,
    bool select = true,
    GameCore.ItemInfo baseInfo = null,
    PlayerItem TargetReisouInfo = null,
    PlayerItem baseReisouInfo = null,
    bool showEquipUnit = false,
    int? addReisouJewel = null)
  {
    Gacha00611Menu gacha00611Menu = this;
    gacha00611Menu.target = TargetInfo;
    gacha00611Menu.targetReisouInfo = TargetReisouInfo;
    gacha00611Menu.baseReisouInfo = baseReisouInfo;
    PlayerUnit[] equiptargets = ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => ((IEnumerable<int?>) x.equip_gear_ids).Contains<int?>(new int?(TargetInfo.itemID)))).ToArray<PlayerUnit>();
    gacha00611Menu.addReisouJewel = addReisouJewel;
    gacha00611Menu.isStopSe = false;
    UnitIcon[] componentsInChildren = gacha00611Menu.charaThum.GetComponentsInChildren<UnitIcon>();
    if (componentsInChildren != null)
      ((IEnumerable<UnitIcon>) componentsInChildren).ForEach<UnitIcon>((Action<UnitIcon>) (x => Object.Destroy((Object) ((Component) x).gameObject)));
    Future<GameObject> iconPrefabF;
    IEnumerator e;
    if (Object.op_Equality((Object) gacha00611Menu.unitPrefab, (Object) null))
    {
      iconPrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      e = iconPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      gacha00611Menu.unitPrefab = iconPrefabF.Result;
      iconPrefabF = (Future<GameObject>) null;
    }
    UnitIcon unitScript = gacha00611Menu.unitPrefab.Clone(gacha00611Menu.charaThum.transform).GetComponent<UnitIcon>();
    if (((IEnumerable<PlayerUnit>) equiptargets).Count<PlayerUnit>() <= 0)
    {
      unitScript.SetEmpty();
      unitScript.SelectUnit = !showEquipUnit && select;
    }
    else
    {
      unitScript.setBottom(equiptargets[0]);
      unitScript.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
      e = unitScript.SetUnit(equiptargets[0].unit, equiptargets[0].GetElement(), false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unitScript.setLevelText(equiptargets[0]);
    }
    if (showEquipUnit)
    {
      ((UIButtonColor) unitScript.Button).isEnabled = false;
      ((UIButtonColor) gacha00611Menu.charaThum.GetComponent<UIButton>()).isEnabled = false;
    }
    gacha00611Menu.sendParam.gearId = TargetInfo.itemID;
    GearGear targetGear = TargetInfo.gear;
    gacha00611Menu.sendParam.gearKindId = targetGear.kind_GearKind;
    Future<Sprite> spriteF = targetGear.LoadSpriteBasic();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    gacha00611Menu.DynWeaponIll.sprite2D = spriteF.Result;
    UI2DSprite dynWeaponIll1 = gacha00611Menu.DynWeaponIll;
    Rect textureRect1 = spriteF.Result.textureRect;
    int num1 = Mathf.FloorToInt(((Rect) ref textureRect1).width);
    ((UIWidget) dynWeaponIll1).width = num1;
    UI2DSprite dynWeaponIll2 = gacha00611Menu.DynWeaponIll;
    Rect textureRect2 = spriteF.Result.textureRect;
    int num2 = Mathf.FloorToInt(((Rect) ref textureRect2).height);
    ((UIWidget) dynWeaponIll2).height = num2;
    ((Component) gacha00611Menu.DynWeaponIll).transform.localScale = Vector2.op_Implicit(new Vector2(0.8f, 0.8f));
    gacha00611Menu.Weapon.Init(targetGear.kind, TargetInfo.GetElement());
    if (Object.op_Equality((Object) gacha00611Menu.SkillDialog, (Object) null))
    {
      iconPrefabF = Res.Prefabs.battle017_11_1_1.SkillDetailDialog.Load<GameObject>();
      e = iconPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      gacha00611Menu.SkillDialog = iconPrefabF.Result;
      iconPrefabF = (Future<GameObject>) null;
    }
    gacha00611Menu.SetSkillDeteilEvent(TargetInfo);
    if (targetGear.kind.is_attack)
    {
      if (targetGear.hasAttackClass)
      {
        gacha00611Menu.WeaponSpAttack.InitGearAttackID((int) targetGear.gearClassification.attack_classification);
        gacha00611Menu.TxtWeaponAttack.SetText(targetGear.attackClassificationName);
      }
      else
      {
        ((Component) gacha00611Menu.WeaponSpAttack).gameObject.SetActive(false);
        ((Component) gacha00611Menu.TxtWeaponAttack).gameObject.SetActive(false);
      }
      if (Object.op_Equality((Object) gacha00611Menu.commonElementIconPrefab, (Object) null))
      {
        iconPrefabF = Res.Icons.CommonElementIcon.Load<GameObject>();
        yield return (object) iconPrefabF.Wait();
        gacha00611Menu.commonElementIconPrefab = iconPrefabF.Result;
        iconPrefabF = (Future<GameObject>) null;
      }
      if (Object.op_Equality((Object) gacha00611Menu.popupAttackClassPrefab, (Object) null))
      {
        iconPrefabF = PopupAttackClassDetail.createPrefabLoader();
        yield return (object) iconPrefabF.Wait();
        gacha00611Menu.popupAttackClassPrefab = iconPrefabF.Result;
        iconPrefabF = (Future<GameObject>) null;
      }
      if (Object.op_Equality((Object) gacha00611Menu.popupAttachedElementPrefab, (Object) null))
      {
        iconPrefabF = PopupGearAttachedElementDetail.createPrefabLoader();
        yield return (object) iconPrefabF.Wait();
        gacha00611Menu.popupAttachedElementPrefab = iconPrefabF.Result;
        iconPrefabF = (Future<GameObject>) null;
      }
      gacha00611Menu.WeaponSpElement.Initialize(gacha00611Menu.commonElementIconPrefab.GetComponent<CommonElementIcon>(), targetGear.attachedElement);
      gacha00611Menu.TxtWeaponElement.SetText(CommonElementName.GetName((int) targetGear.attachedElement));
    }
    else
    {
      ((Component) gacha00611Menu.WeaponSpAttack).gameObject.SetActive(false);
      ((Component) gacha00611Menu.TxtWeaponAttack).gameObject.SetActive(false);
      ((Component) gacha00611Menu.WeaponSpElement).gameObject.SetActive(false);
      ((Component) gacha00611Menu.TxtWeaponElement).gameObject.SetActive(false);
    }
    gacha00611Menu.dirEquipNone.SetActive(!showEquipUnit);
    gacha00611Menu.dirEquipUnit.SetActive(showEquipUnit);
    Judgement.GearParameter lhs1 = TargetInfo.isWeaponMaterial ? Judgement.GearParameter.FromGearGear(targetGear) : Judgement.GearParameter.FromPlayerGear(TargetInfo);
    Judgement.GearParameter lhs2 = (Judgement.GearParameter) null;
    if (baseInfo != null)
      lhs2 = Judgement.GearParameter.FromPlayerGear(baseInfo);
    if (gacha00611Menu.targetReisouInfo != (PlayerItem) null)
    {
      Judgement.GearParameter rhs = Judgement.GearParameter.FromPlayerGear(gacha00611Menu.targetReisouInfo);
      lhs1 = Judgement.GearParameter.AddReisou(lhs1, rhs);
    }
    if (baseReisouInfo != (PlayerItem) null)
    {
      Judgement.GearParameter rhs = Judgement.GearParameter.FromPlayerGear(baseReisouInfo);
      lhs2 = Judgement.GearParameter.AddReisou(lhs2, rhs);
    }
    if (targetGear.attack_type == GearAttackType.physical)
    {
      gacha00611Menu.SetParameter(gacha00611Menu.TxtAttack, lhs1.Power, lhs2 != null ? lhs1.Power - lhs2.Power : 0);
      gacha00611Menu.TxtMagicAttack.SetTextLocalize(0);
    }
    else
    {
      gacha00611Menu.TxtAttack.SetTextLocalize(0);
      gacha00611Menu.SetParameter(gacha00611Menu.TxtMagicAttack, lhs1.Power, lhs2 != null ? lhs1.Power - lhs2.Power : 0);
    }
    gacha00611Menu.SetParameter(gacha00611Menu.TxtCritical, lhs1.Critical, lhs2 != null ? lhs1.Critical - lhs2.Critical : 0);
    gacha00611Menu.SetParameter(gacha00611Menu.TxtDefense, lhs1.PhysicalDefense, lhs2 != null ? lhs1.PhysicalDefense - lhs2.PhysicalDefense : 0);
    gacha00611Menu.SetParameter(gacha00611Menu.TxtMagicDefense, lhs1.MagicDefense, lhs2 != null ? lhs1.MagicDefense - lhs2.MagicDefense : 0);
    gacha00611Menu.SetParameter(gacha00611Menu.TxtEvasion, lhs1.Evasion, lhs2 != null ? lhs1.Evasion - lhs2.Evasion : 0);
    gacha00611Menu.SetParameter(gacha00611Menu.TxtDexterity, lhs1.Hit, lhs2 != null ? lhs1.Hit - lhs2.Hit : 0);
    gacha00611Menu.TxtRange.SetTextLocalize(targetGear.min_range.ToString() + "-" + (object) targetGear.max_range);
    gacha00611Menu.TxtWait.SetTextLocalize(targetGear.weight);
    gacha00611Menu.TxtExp.SetTextLocalize(Consts.Format(Consts.GetInstance().BUGU_0059_RANK, (IDictionary) new Hashtable()
    {
      {
        (object) "now",
        (object) TargetInfo.gearLevel
      },
      {
        (object) "max",
        (object) TargetInfo.gearLevelLimit
      }
    }));
    gacha00611Menu.TxtNextExp.SetTextLocalize(TargetInfo.gearExpNext);
    gacha00611Menu.TxtTitle.SetText(TargetInfo.Name());
    string text1 = targetGear.gearClassification?.name ?? "-";
    gacha00611Menu.TxtWeapontype.SetText(text1);
    gacha00611Menu.maxWidth = ((UIWidget) gacha00611Menu.SlcGauge).width;
    Decimal num3 = 0M;
    if (TargetInfo.gearExpNext + TargetInfo.gearExp != 0)
      num3 = (Decimal) TargetInfo.gearExp / (Decimal) (TargetInfo.gearExpNext + TargetInfo.gearExp);
    if (baseInfo != null && baseInfo.gearExpNext + baseInfo.gearExp != 0)
      num3 = (Decimal) baseInfo.gearExp / (Decimal) (baseInfo.gearExpNext + baseInfo.gearExp);
    Decimal num4 = (Decimal) gacha00611Menu.maxWidth * num3;
    if (num4 == 0M)
    {
      ((Component) gacha00611Menu.SlcGauge).gameObject.SetActive(false);
    }
    else
    {
      ((Component) gacha00611Menu.SlcGauge).gameObject.SetActive(true);
      ((UIWidget) gacha00611Menu.SlcGauge).width = (int) num4;
    }
    RarityIcon.SetRarity(targetGear, gacha00611Menu.rarityStarsIcon);
    gacha00611Menu.NewIcon.SetActive(NewFlag);
    e = gacha00611Menu.SetIncrementalParameter(TargetInfo, gacha00611Menu.DirAddStauts);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    gacha00611Menu.isEndGaugeAnim = false;
    gacha00611Menu.HolyReisouExpGauge.isEndGaugeAnim = false;
    gacha00611Menu.ChaosReisouExpGauge.isEndGaugeAnim = false;
    if (baseInfo != null)
    {
      Decimal num5 = baseInfo.gearExpNext + baseInfo.gearExp == 0 ? 0M : (Decimal) baseInfo.gearExp / (Decimal) (baseInfo.gearExpNext + baseInfo.gearExp);
      Decimal num6 = TargetInfo.gearExpNext + TargetInfo.gearExp == 0 ? 0M : (Decimal) TargetInfo.gearExp / (Decimal) (TargetInfo.gearExpNext + TargetInfo.gearExp);
      gacha00611Menu.isRankup = TargetInfo.gearLevel > baseInfo.gearLevel;
      gacha00611Menu.isMaxRankup = TargetInfo.gearLevelLimit > baseInfo.gearLevelLimit;
      gacha00611Menu.startGauge = num5;
      gacha00611Menu.addGauge = (Decimal) (TargetInfo.gearLevel - baseInfo.gearLevel) - num5 + num6;
      gacha00611Menu.base_gear_level = baseInfo.gearLevel;
      if (gacha00611Menu.isRankup)
        gacha00611Menu.TxtExp.SetTextLocalize(Consts.Format(Consts.GetInstance().BUGU_0059_RANK, (IDictionary) new Hashtable()
        {
          {
            (object) "now",
            (object) baseInfo.gearLevel
          },
          {
            (object) "max",
            (object) baseInfo.gearLevelLimit
          }
        }));
      else if (gacha00611Menu.isMaxRankup)
      {
        UILabel txtExp = gacha00611Menu.TxtExp;
        string bugu0059Rank = Consts.GetInstance().BUGU_0059_RANK;
        Hashtable hashtable1 = new Hashtable();
        Hashtable hashtable2 = hashtable1;
        string str;
        if (TargetInfo.gearLevel >= baseInfo.gearLevel)
        {
          if (TargetInfo.gearLevel <= baseInfo.gearLevel)
            str = TargetInfo.gearLevel.ToString();
          else
            str = Gacha00611Menu.COLOR_TAG_GREEN.F((object) TargetInfo.gearLevel);
        }
        else
          str = Gacha00611Menu.COLOR_TAG_RED.F((object) TargetInfo.gearLevel);
        hashtable2.Add((object) "now", (object) str);
        hashtable1.Add((object) "max", (object) Gacha00611Menu.COLOR_TAG_GREEN.F((object) TargetInfo.gearLevelLimit));
        Hashtable args = hashtable1;
        string text2 = Consts.Format(bugu0059Rank, (IDictionary) args);
        txtExp.SetTextLocalize(text2);
      }
      if (baseInfo.skills.Length < TargetInfo.skills.Length)
      {
        gacha00611Menu.addSkill = new List<GearGearSkill>();
        for (int length = baseInfo.skills.Length; length < TargetInfo.skills.Length; ++length)
          gacha00611Menu.addSkill.Add(TargetInfo.skills[length]);
      }
      if (baseInfo.skills.Length != 0)
      {
        gacha00611Menu.evoSkill = new List<GearGearSkill>();
        for (int index = 0; index < baseInfo.skills.Length; ++index)
        {
          if (baseInfo.skills[index].ID != TargetInfo.skills[index].ID && baseInfo.skills[index].release_rank < TargetInfo.skills[index].release_rank)
            gacha00611Menu.evoSkill.Add(TargetInfo.skills[index]);
        }
      }
    }
    if (countWeapon > 1)
      ModalWindow.Show(Consts.Format(Consts.GetInstance().GACHA_0061MULTIPLE_WEAPONS_TITLE), Consts.Format(Consts.GetInstance().GACHA_0061MULTIPLE_WEAPONS_NUM, (IDictionary) new Hashtable()
      {
        {
          (object) "weapon",
          (object) TargetInfo.name.ToString()
        },
        {
          (object) "num",
          (object) countWeapon.ToString()
        }
      }), (Action) (() => { }));
    gacha00611Menu.remainingManaSeedContainer.SetActive(false);
    if (TargetInfo.gear != null && TargetInfo.gear.kind.Enum == GearKindEnum.accessories && TargetInfo.gear.disappearance_type_GearDisappearanceType == 1)
    {
      gacha00611Menu.remainingManaSeedContainer.SetActive(true);
      gacha00611Menu.remainingManaSeedLabel.SetTextLocalize(TargetInfo.gearAccessoryRemainingAmount);
    }
    if (gacha00611Menu.targetReisouInfo == (PlayerItem) null)
    {
      gacha00611Menu.DirReisou.SetActive(false);
    }
    else
    {
      gacha00611Menu.DirReisou.SetActive(true);
      iconPrefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
      e = iconPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      gacha00611Menu.reisouIcon = iconPrefabF.Result.CloneAndGetComponent<ItemIcon>(gacha00611Menu.DynReisouIcon.transform);
      e = gacha00611Menu.reisouIcon.InitByPlayerItem(gacha00611Menu.targetReisouInfo);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      gacha00611Menu.reisouIcon.setEquipReisouDisp();
      gacha00611Menu.reisouIcon.onClick = (Action<ItemIcon>) (_ => { });
      gacha00611Menu.reisouIcon.DisableLongPressEvent();
      if (baseReisouInfo.gear.isHolyReisou())
      {
        gacha00611Menu.HolyReisouExpGauge = gacha00611Menu.setReisouGaugeExp(gacha00611Menu.HolyReisouExpGauge, gacha00611Menu.targetReisouInfo, baseReisouInfo);
        gacha00611Menu.ChaosReisouExpGauge.DirReisou.SetActive(false);
      }
      else if (baseReisouInfo.gear.isChaosReisou())
      {
        gacha00611Menu.HolyReisouExpGauge.DirReisou.SetActive(false);
        gacha00611Menu.ChaosReisouExpGauge = gacha00611Menu.setReisouGaugeExp(gacha00611Menu.ChaosReisouExpGauge, gacha00611Menu.targetReisouInfo, baseReisouInfo);
      }
      else
      {
        GearReisouFusion fusionMineRecipe = baseReisouInfo.GetReisouFusionMineRecipe();
        PlayerMythologyGearStatus mythologyGearStatus1 = gacha00611Menu.targetReisouInfo.GetPlayerMythologyGearStatus();
        PlayerMythologyGearStatus mythologyGearStatus2 = baseReisouInfo.GetPlayerMythologyGearStatus();
        PlayerItem targetItem1 = new PlayerItem(fusionMineRecipe.holy_ID, mythologyGearStatus1);
        PlayerItem targetItem2 = new PlayerItem(fusionMineRecipe.chaos_ID, mythologyGearStatus1);
        PlayerItem baseItem1 = new PlayerItem(fusionMineRecipe.holy_ID, mythologyGearStatus2);
        PlayerItem baseItem2 = new PlayerItem(fusionMineRecipe.chaos_ID, mythologyGearStatus2);
        gacha00611Menu.HolyReisouExpGauge = gacha00611Menu.setReisouGaugeExp(gacha00611Menu.HolyReisouExpGauge, targetItem1, baseItem1);
        gacha00611Menu.ChaosReisouExpGauge = gacha00611Menu.setReisouGaugeExp(gacha00611Menu.ChaosReisouExpGauge, targetItem2, baseItem2);
      }
      iconPrefabF = (Future<GameObject>) null;
    }
    if (gacha00611Menu.evoSkill != null && gacha00611Menu.evoSkill.Count > 0 || gacha00611Menu.addSkill != null && gacha00611Menu.addSkill.Count > 0 || gacha00611Menu.addReisouSkill != null && gacha00611Menu.addReisouSkill.Count > 0)
    {
      iconPrefabF = Res.Prefabs.battle.BuguSkillGetPrefab.Load<GameObject>();
      e = iconPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      gacha00611Menu.skillGetPrefab = iconPrefabF.Result;
      iconPrefabF = (Future<GameObject>) null;
    }
    if (TargetInfo.isReisou)
    {
      gacha00611Menu.dirBottom.SetActive(false);
      ((Component) gacha00611Menu.DynWeaponIll).transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
    }
    else
    {
      gacha00611Menu.dirBottom.SetActive(true);
      ((Component) gacha00611Menu.DynWeaponIll).transform.localPosition = new Vector3(0.0f, 123f, 0.0f);
    }
  }

  private Gacha00611Menu.ReisouExpGauge setReisouGaugeExp(
    Gacha00611Menu.ReisouExpGauge reisouExpGauge,
    PlayerItem targetItem,
    PlayerItem baseItem)
  {
    reisouExpGauge.DirReisou.SetActive(true);
    reisouExpGauge.TxtReisouRank.SetTextLocalize(Consts.GetInstance().UNIT_00443_REISOU_RANK.F((object) targetItem.gear_level, (object) targetItem.gear_level_limit));
    if (targetItem.gear_level > baseItem.gear_level)
      reisouExpGauge.TxtReisouNextRank.SetTextLocalize(Consts.Format(Consts.GetInstance().BUGU_0059_REMAIN, (IDictionary) new Hashtable()
      {
        {
          (object) "remain",
          (object) 0
        }
      }));
    else
      reisouExpGauge.TxtReisouNextRank.SetTextLocalize(Consts.Format(Consts.GetInstance().BUGU_0059_REMAIN, (IDictionary) new Hashtable()
      {
        {
          (object) "remain",
          (object) targetItem.gear_exp_next
        }
      }));
    if (baseItem != (PlayerItem) null)
    {
      Decimal num1 = baseItem.gear_exp_next + baseItem.gear_exp == 0 ? 0M : (Decimal) baseItem.gear_exp / (Decimal) (baseItem.gear_exp_next + baseItem.gear_exp);
      Decimal num2 = targetItem.gear_exp_next + targetItem.gear_exp == 0 ? 0M : (Decimal) targetItem.gear_exp / (Decimal) (targetItem.gear_exp_next + targetItem.gear_exp);
      reisouExpGauge.startReisouGauge = num1;
      reisouExpGauge.addReisouGauge = (Decimal) (targetItem.gear_level - baseItem.gear_level) - num1 + num2;
      reisouExpGauge.isReisouRankup = targetItem.gear_level > baseItem.gear_level;
      reisouExpGauge.base_reisou_level = baseItem.gear_level;
      if (reisouExpGauge.isReisouRankup)
      {
        reisouExpGauge.TxtReisouRank.SetTextLocalize(Consts.GetInstance().UNIT_00443_REISOU_RANK.F((object) baseItem.gear_level, (object) baseItem.gear_level_limit));
        GearReisouSkill[] reisouSkills1 = baseItem.getReisouSkills(this.target.gear.ID, true);
        GearReisouSkill[] reisouSkills2 = targetItem.getReisouSkills(this.target.gear.ID, true);
        if (reisouSkills1.Length < reisouSkills2.Length)
        {
          if (this.addReisouSkill == null)
            this.addReisouSkill = new List<GearReisouSkill>();
          for (int length = reisouSkills1.Length; length < reisouSkills2.Length; ++length)
            this.addReisouSkill.Add(reisouSkills2[length]);
        }
      }
    }
    PlayerItem playerItem = targetItem;
    if (baseItem != (PlayerItem) null)
      playerItem = baseItem;
    reisouExpGauge.maxReisouWidth = ((UIWidget) reisouExpGauge.SlcReisouGauge).width;
    float num3 = (float) playerItem.gear_exp / (float) (playerItem.gear_exp + playerItem.gear_exp_next);
    float num4 = (float) reisouExpGauge.maxReisouWidth * num3;
    if ((double) num4 == 0.0 || playerItem.gear_exp + playerItem.gear_exp_next == 0)
    {
      ((Component) reisouExpGauge.SlcReisouGauge).gameObject.SetActive(false);
    }
    else
    {
      ((Component) reisouExpGauge.SlcReisouGauge).gameObject.SetActive(true);
      ((UIWidget) reisouExpGauge.SlcReisouGauge).width = (int) num4;
    }
    return reisouExpGauge;
  }

  private void SetParameter(UILabel label, int param, int up)
  {
    if (up > 0)
      label.SetTextLocalize(Gacha00611Menu.COLOR_TAG_YELLOW.F((object) param));
    else if (up < 0)
      label.SetTextLocalize(Gacha00611Menu.COLOR_TAG_RED.F((object) param));
    else
      label.SetTextLocalize(param);
  }

  public void OpenPopupWeaponAttack()
  {
    if (this.target == null || this.target.gear == null || !Object.op_Inequality((Object) this.popupAttackClassPrefab, (Object) null))
      return;
    this.StartCoroutine(this.doPopupAttackClassDetail());
  }

  private IEnumerator doPopupAttackClassDetail()
  {
    Gacha00611Menu gacha00611Menu = this;
    if (!gacha00611Menu.IsPushAndSet())
    {
      yield return (object) PopupAttackClassDetail.show(gacha00611Menu.popupAttackClassPrefab, gacha00611Menu.target.gear);
      gacha00611Menu.IsPush = false;
    }
  }

  public void OpenPopupWeaponElement()
  {
    if (this.target == null || this.target.gear == null || !Object.op_Inequality((Object) this.popupAttachedElementPrefab, (Object) null))
      return;
    this.StartCoroutine(this.doPopupAttachedElementDetail());
  }

  private IEnumerator doPopupAttachedElementDetail()
  {
    Gacha00611Menu gacha00611Menu = this;
    if (!gacha00611Menu.IsPushAndSet())
    {
      yield return (object) PopupGearAttachedElementDetail.show(gacha00611Menu.popupAttachedElementPrefab, gacha00611Menu.target.gear);
      gacha00611Menu.IsPush = false;
    }
  }

  public void StartAnime()
  {
    this.isPlaying = true;
    this.isPlaySkip = false;
    Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
    Singleton<PopupManager>.GetInstance().open((GameObject) null, isViewBack: false, isNonSe: true);
    this.StartCoroutine("Play");
  }

  private void playFinish()
  {
    if (!this.isPlaying)
      return;
    this.isPlaying = false;
    Singleton<PopupManager>.GetInstance().dismiss();
    Singleton<CommonRoot>.GetInstance().SetFooterEnable(true);
  }

  public void StopAnime()
  {
    this.isStopSe = true;
    this.StopCoroutine("Play");
    Singleton<NGSoundManager>.GetInstance().stopSE();
    this.playFinish();
  }

  protected override void Update()
  {
    base.Update();
    if (!this.isPlaying || this.isPlaySkip || !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2) && (double) Input.GetAxis("Mouse ScrollWheel") == 0.0)
      return;
    this.isPlaySkip = true;
    this.IsPush = true;
    this.doBackScene();
  }

  private IEnumerator Play()
  {
    Gacha00611Menu gacha00611Menu = this;
    gacha00611Menu.StartCoroutine(gacha00611Menu.PlayGaugeAnim());
    gacha00611Menu.StartCoroutine(gacha00611Menu.PlayReisou(true));
    gacha00611Menu.StartCoroutine(gacha00611Menu.PlayReisou(false));
    while (!gacha00611Menu.isEndGaugeAnim || !gacha00611Menu.HolyReisouExpGauge.isEndGaugeAnim || !gacha00611Menu.ChaosReisouExpGauge.isEndGaugeAnim)
      yield return (object) null;
    GameObject popup;
    BattleResultBuguSkillGet o;
    IEnumerator e;
    if (gacha00611Menu.addSkill != null)
    {
      foreach (GearGearSkill skill in gacha00611Menu.addSkill)
      {
        popup = Singleton<PopupManager>.GetInstance().open(gacha00611Menu.skillGetPrefab);
        popup.SetActive(false);
        o = popup.GetComponent<BattleResultBuguSkillGet>();
        e = o.Init(true, gacha00611Menu.target, skill);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        popup.SetActive(true);
        Singleton<NGSoundManager>.GetInstance().playSE(gacha00611Menu.seSkillAwaked);
        o.doStart();
        bool isFinished = false;
        o.SetCallback((Action) (() => isFinished = true));
        while (!isFinished)
          yield return (object) null;
        yield return (object) new WaitForSeconds(0.6f);
        popup = (GameObject) null;
        o = (BattleResultBuguSkillGet) null;
      }
    }
    if (gacha00611Menu.addReisouSkill != null)
    {
      foreach (GearReisouSkill skill in gacha00611Menu.addReisouSkill)
      {
        popup = Singleton<PopupManager>.GetInstance().open(gacha00611Menu.skillGetPrefab);
        popup.SetActive(false);
        o = popup.GetComponent<BattleResultBuguSkillGet>();
        e = o.InitReisou(true, gacha00611Menu.targetReisouInfo, skill);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        popup.SetActive(true);
        Singleton<NGSoundManager>.GetInstance().playSE(gacha00611Menu.seSkillAwaked);
        o.doStart();
        bool isFinished = false;
        o.SetCallback((Action) (() => isFinished = true));
        while (!isFinished)
          yield return (object) null;
        yield return (object) new WaitForSeconds(0.6f);
        popup = (GameObject) null;
        o = (BattleResultBuguSkillGet) null;
      }
    }
    gacha00611Menu.playFinish();
  }

  private IEnumerator PlayGaugeAnim()
  {
    if (this.addGauge > 0M)
    {
      int seChannel = -1;
      Decimal addValue = this.addGauge / 30.0M > 0.033M ? 0.033M : this.addGauge / 30.0M;
      Decimal now = 0M;
      if (!this.isStopSe)
        seChannel = Singleton<NGSoundManager>.GetInstance().playSE(this.seGaugeUp, true);
      while (now < this.addGauge)
      {
        Decimal d1 = this.startGauge + now;
        now += addValue;
        if (now > this.addGauge)
          now = this.addGauge;
        int int32_1 = Decimal.ToInt32(Decimal.Floor(d1));
        Decimal d2 = this.startGauge + now;
        Decimal moveTone = d2 - Decimal.Floor(d2);
        int int32_2 = Decimal.ToInt32(Decimal.Floor(d2));
        if (this.isRankup && int32_1 < int32_2)
        {
          this.TxtExp.SetTextLocalize(Consts.Format(Consts.GetInstance().BUGU_0059_RANK, (IDictionary) new Hashtable()
          {
            {
              (object) "now",
              (object) Gacha00611Menu.COLOR_TAG_GREEN.F((object) (int32_2 + this.base_gear_level))
            },
            {
              (object) "max",
              (object) this.target.gearLevelLimit
            }
          }));
          if (Object.op_Inequality((Object) this.rankUpObject, (Object) null))
          {
            if (!this.isStopSe)
            {
              Singleton<NGSoundManager>.GetInstance().stopSE(seChannel);
              seChannel = -1;
              Singleton<NGSoundManager>.GetInstance().playSE(this.seRankUp);
            }
            else
              Singleton<NGSoundManager>.GetInstance().stopSE();
            this.rankUpObject.SetActive(true);
            ((IEnumerable<UITweener>) this.rankUpObject.GetComponentsInChildren<UITweener>()).ForEach<UITweener>((Action<UITweener>) (x =>
            {
              ((Behaviour) x).enabled = true;
              x.PlayForward();
            }));
            ((UIWidget) this.SlcGauge).width = this.maxWidth;
          }
          yield return (object) new WaitForSeconds(0.6f);
          if (!this.isStopSe)
            seChannel = Singleton<NGSoundManager>.GetInstance().playSE(this.seGaugeUp, true);
        }
        if (Object.op_Inequality((Object) this.SlcGauge, (Object) null))
        {
          Decimal num = (Decimal) this.maxWidth * moveTone;
          if (num == 0M)
          {
            ((Component) this.SlcGauge).gameObject.SetActive(false);
          }
          else
          {
            ((Component) this.SlcGauge).gameObject.SetActive(true);
            ((UIWidget) this.SlcGauge).width = (int) num;
          }
        }
        yield return (object) null;
      }
      yield return (object) new WaitForSeconds(0.3f);
      if (!this.isStopSe)
        Singleton<NGSoundManager>.GetInstance().stopSE(seChannel);
      else
        Singleton<NGSoundManager>.GetInstance().stopSE();
    }
    if (this.isMaxRankup && Object.op_Inequality((Object) this.maxRankUpObject, (Object) null))
    {
      this.maxRankUpObject.SetActive(true);
      ((IEnumerable<UITweener>) this.maxRankUpObject.GetComponentsInChildren<UITweener>()).ForEach<UITweener>((Action<UITweener>) (x =>
      {
        ((Behaviour) x).enabled = true;
        x.PlayForward();
      }));
      yield return (object) new WaitForSeconds(0.6f);
    }
    this.isEndGaugeAnim = true;
  }

  private IEnumerator PlayReisou(bool isHoly)
  {
    Gacha00611Menu.ReisouExpGauge ReisouExpGauge = isHoly ? this.HolyReisouExpGauge : this.ChaosReisouExpGauge;
    if (ReisouExpGauge.addReisouGauge > 0M)
    {
      int seChannel = -1;
      Decimal addValue = ReisouExpGauge.addReisouGauge / 30.0M > 0.033M ? 0.033M : ReisouExpGauge.addReisouGauge / 30.0M;
      Decimal now = 0M;
      if (!this.isStopSe)
        seChannel = Singleton<NGSoundManager>.GetInstance().playSE(this.seGaugeUp, true);
      while (now < ReisouExpGauge.addReisouGauge)
      {
        Decimal d1 = ReisouExpGauge.startReisouGauge + now;
        now += addValue;
        if (now > ReisouExpGauge.addReisouGauge)
          now = ReisouExpGauge.addReisouGauge;
        int int32_1 = Decimal.ToInt32(Decimal.Floor(d1));
        Decimal d2 = ReisouExpGauge.startReisouGauge + now;
        Decimal moveTone = d2 - Decimal.Floor(d2);
        int int32_2 = Decimal.ToInt32(Decimal.Floor(d2));
        if (ReisouExpGauge.isReisouRankup && int32_1 < int32_2)
        {
          ReisouExpGauge.TxtReisouRank.SetTextLocalize(Consts.GetInstance().UNIT_00443_REISOU_RANK.F((object) Gacha00611Menu.COLOR_TAG_GREEN.F((object) (int32_2 + ReisouExpGauge.base_reisou_level)), (object) this.targetReisouInfo.gear_level_limit));
          if (Object.op_Inequality((Object) this.rankUpReisouObject, (Object) null))
          {
            if (!this.isStopSe)
            {
              Singleton<NGSoundManager>.GetInstance().stopSE(seChannel);
              seChannel = -1;
              Singleton<NGSoundManager>.GetInstance().playSE(this.seRankUp);
            }
            else
              Singleton<NGSoundManager>.GetInstance().stopSE();
            this.rankUpReisouObject.SetActive(true);
            ((IEnumerable<UITweener>) this.rankUpReisouObject.GetComponentsInChildren<UITweener>()).ForEach<UITweener>((Action<UITweener>) (x =>
            {
              ((Behaviour) x).enabled = true;
              x.PlayForward();
            }));
            ((UIWidget) ReisouExpGauge.SlcReisouGauge).width = ReisouExpGauge.maxReisouWidth;
          }
          yield return (object) new WaitForSeconds(0.6f);
          if (!this.isStopSe)
            seChannel = Singleton<NGSoundManager>.GetInstance().playSE(this.seGaugeUp, true);
        }
        if (Object.op_Inequality((Object) ReisouExpGauge.SlcReisouGauge, (Object) null))
        {
          Decimal num = (Decimal) ReisouExpGauge.maxReisouWidth * moveTone;
          if (num == 0M)
          {
            ((Component) ReisouExpGauge.SlcReisouGauge).gameObject.SetActive(false);
          }
          else
          {
            ((Component) ReisouExpGauge.SlcReisouGauge).gameObject.SetActive(true);
            ((UIWidget) ReisouExpGauge.SlcReisouGauge).width = (int) num;
          }
        }
        yield return (object) null;
      }
      if (!this.isStopSe)
        Singleton<NGSoundManager>.GetInstance().stopSE(seChannel);
      else
        Singleton<NGSoundManager>.GetInstance().stopSE();
      yield return (object) new WaitForSeconds(0.3f);
    }
    if (isHoly)
      this.HolyReisouExpGauge.isEndGaugeAnim = true;
    else
      this.ChaosReisouExpGauge.isEndGaugeAnim = true;
  }

  protected void SetSkillDialog(UIButton button, GearGearSkill skill_data, bool isRelease)
  {
    if (this.showSkillDialog == null)
    {
      Battle0171111Event dialog = this.SkillDialog.Clone(this.floatingSkillDialog.transform).GetComponentInChildren<Battle0171111Event>();
      ((Component) ((Component) dialog).transform.parent).gameObject.SetActive(false);
      this.showSkillDialog = (Action<GearGearSkill, bool>) ((skill, release) =>
      {
        dialog.setData(skill.skill);
        if (release)
          dialog.setSkillLv(skill.skill_level, skill.skill.upper_level);
        else
          dialog.setSkillLv(0, skill.skill.upper_level);
        dialog.Show();
      });
    }
    EventDelegate.Set(button.onClick, (EventDelegate.Callback) (() => this.showSkillDialog(skill_data, isRelease)));
  }

  protected void SetSkillDeteilEvent(GameCore.ItemInfo gear)
  {
    BattleSkillIcon[] battleSkillIconArray = this.WeaponSkillOne;
    UIButton[] uiButtonArray = this.WeaponSkillButtonOne;
    this.WeaponSkillOneRoot.SetActive(true);
    this.WeaponSkillTwoRoot.SetActive(false);
    GearGear gear1 = gear.gear;
    if (gear1.rememberSkills.Count <= 0)
      return;
    GearGearSkill[] skills = gear.skills;
    int num = gear1.rememberSkills.Count > skills.Length ? 1 : 0;
    bool flag = num == 0 && gear1.rememberSkills[0].Count > 0 && !gear1.rememberSkills[0].All<GearGearSkill>((Func<GearGearSkill, bool>) (x => x.isReleased(gear)));
    if (((num | (flag ? 1 : 0)) != 0 || gear1.skills.Length > 1) && (gear1.rememberSkills.Count > 1 || gear1.rememberSkills.Count == 1 && gear1.rememberSkills[0].Count > 1))
    {
      battleSkillIconArray = this.WeaponSkillTwo;
      uiButtonArray = this.WeaponSkillButtonTwo;
      this.WeaponSkillOneRoot.SetActive(false);
      this.WeaponSkillTwoRoot.SetActive(true);
    }
    if (num != 0)
    {
      for (int index = 0; index < gear1.rememberSkills.Count && battleSkillIconArray.Length > index; ++index)
      {
        BattleSkillIcon battleSkillIcon = battleSkillIconArray[index];
        if (gear1.rememberSkills[index][0].release_rank > gear.gearLevel)
          battleSkillIcon.EnableNeedRankIcon(gear1.rememberSkills[index][0].release_rank);
        else
          battleSkillIcon.EnableNeedRankIcon(0);
        this.StartCoroutine(battleSkillIcon.Init(gear1.rememberSkills[index][0].skill));
        this.SetSkillDialog(uiButtonArray[index], gear1.rememberSkills[index][0], gear1.rememberSkills[index][0].isReleased(gear));
      }
    }
    else if (flag)
    {
      for (int index = 0; index < gear1.rememberSkills[0].Count && battleSkillIconArray.Length > index; ++index)
      {
        BattleSkillIcon battleSkillIcon = battleSkillIconArray[index];
        if (gear1.rememberSkills[0][index].release_rank > gear.gearLevel)
          battleSkillIcon.EnableNeedRankIcon(gear1.rememberSkills[0][index].release_rank);
        else
          battleSkillIcon.EnableNeedRankIcon(0);
        this.StartCoroutine(battleSkillIcon.Init(gear1.rememberSkills[0][index].skill));
        this.SetSkillDialog(uiButtonArray[index], gear1.rememberSkills[0][index], gear1.rememberSkills[0][index].isReleased(gear));
      }
    }
    else
    {
      for (int index = 0; index < gear.skills.Length && battleSkillIconArray.Length > index; ++index)
      {
        this.StartCoroutine(battleSkillIconArray[index].Init(gear.skills[index].skill));
        this.SetSkillDialog(uiButtonArray[index], gear.skills[index], true);
      }
    }
    this.SkillArrowObject.SetActive(flag);
  }

  public IEnumerator PlayGetReisouJewel()
  {
    if (this.addReisouJewel.HasValue && this.addReisouJewel.Value > 0)
    {
      Future<GameObject> popupPrefabF = new ResourceObject("Prefabs/notification_get_item/notification_get_item").Load<GameObject>();
      IEnumerator e = popupPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject popup = popupPrefabF.Result.Clone(this.dynNotificationGetItem.transform);
      PopupNotificationGetItem script = popup.GetComponent<PopupNotificationGetItem>();
      popup.SetActive(false);
      e = script.Init(MasterDataTable.CommonRewardType.reisou_jewel, this.addReisouJewel.Value);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      popup.SetActive(true);
      e = script.Play();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Object.Destroy((Object) popup);
    }
  }

  public void SetChangeScene(Action changeScene) => this.cahngeSceneCallback = changeScene;

  public void changeScene()
  {
    if (this.cahngeSceneCallback != null)
      this.cahngeSceneCallback();
    else
      this.backScene();
  }

  public void choiceUnitChangeScene() => Unit00468Scene.changeScene004431(true, this.sendParam);

  public override void onBackButton() => this.IbtnBack();

  [Serializable]
  public struct ReisouExpGauge
  {
    [SerializeField]
    public GameObject DirReisou;
    [SerializeField]
    public UILabel TxtReisouRank;
    [SerializeField]
    public UILabel TxtReisouNextRank;
    [SerializeField]
    public UISprite SlcReisouGauge;
    [SerializeField]
    public Decimal startReisouGauge;
    [SerializeField]
    public Decimal addReisouGauge;
    [NonSerialized]
    public int base_reisou_level;
    [NonSerialized]
    public bool isEndGaugeAnim;
    [NonSerialized]
    public bool isReisouRankup;
    [NonSerialized]
    public int maxReisouWidth;
  }
}
