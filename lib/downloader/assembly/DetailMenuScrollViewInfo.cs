// Decompiled with JetBrains decompiler
// Type: DetailMenuScrollViewInfo
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
using UnityEngine;

#nullable disable
public class DetailMenuScrollViewInfo : DetailMenuScrollViewBase
{
  private const string MAGIC_BULLET_NONE_NAME = "-";
  [SerializeField]
  private DetailMenuScrollViewInfoWeapon infoWeapon;
  [SerializeField]
  private DetailMenuScrollViewInfoOption infoOption;
  private bool isInfoWeapon;
  private GameObject gearKindIconPrefab;
  private GameObject attackMethodDialog;
  private GameObject attackMethodDialogPrefab;
  [SerializeField]
  private GameObject attackMethodDialogRoot;
  private GameObject elementTypeIconPrefab;
  private PlayerUnit playerUnit;
  private Action<BattleskillSkill> showSkillDialog;
  private Action<int, int> showSkillLevel;

  public override bool Init(PlayerUnit playerUnit, PlayerUnit baseUnit)
  {
    ((Component) this).gameObject.SetActive(true);
    return true;
  }

  public override IEnumerator initAsync(
    PlayerUnit pUnit,
    bool limitMode,
    bool isMaterial,
    GameObject[] prefabs)
  {
    this.playerUnit = pUnit;
    Future<GameObject> elementTypeIconPrefabF = Res.Icons.CommonElementIcon.Load<GameObject>();
    IEnumerator e = elementTypeIconPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.elementTypeIconPrefab = elementTypeIconPrefabF.Result;
    elementTypeIconPrefabF = (Future<GameObject>) null;
    elementTypeIconPrefabF = Singleton<NGGameDataManager>.GetInstance().IsSea ? Res.Prefabs.unit004_2_sea.AttackMethodDialog_sea.Load<GameObject>() : Res.Prefabs.unit004_2.AttackMethodDialog.Load<GameObject>();
    e = elementTypeIconPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.attackMethodDialogPrefab = elementTypeIconPrefabF.Result;
    elementTypeIconPrefabF = (Future<GameObject>) null;
    e = this.setWeapon(this.playerUnit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.isInfoWeapon = false;
    this.ShowOtherWeapons();
  }

  public void ShowOtherWeapons()
  {
    this.isInfoWeapon = !this.isInfoWeapon;
    ((Component) this.infoWeapon).gameObject.SetActive(this.isInfoWeapon);
    ((Component) this.infoOption).gameObject.SetActive(!this.isInfoWeapon);
  }

  public IEnumerator initializeWeapon(
    DetailMenuScrollViewInfo.WeaponRow row,
    GearGear gear,
    GearGear assist,
    bool bAwaked)
  {
    row.top_.SetActive(true);
    if (gear.hasAttackClass)
      row.iconClass_.Initialize(gear.gearClassification.attack_classification);
    else
      row.iconClass_.Initialize(this.playerUnit.initial_gear.gearClassification.attack_classification);
    row.spriteElement_.sprite2D = this.loadElementIcon(gear.attachedElement);
    row.spriteKind_.sprite2D = this.loadGearKindIcon((GearKindEnum) gear.kind_GearKind);
    row.txtName_.SetTextLocalize(gear.name);
    int minRange = gear.min_range;
    int maxRange = gear.max_range;
    if (assist != null && gear != assist)
    {
      if (!bAwaked && minRange < assist.min_range)
        minRange = assist.min_range;
      if (maxRange < assist.max_range)
        maxRange = assist.max_range;
    }
    row.txtRange_.SetTextLocalize(minRange.ToString() + "-" + (object) maxRange);
    this.setTextCost(row.txtCost_, new int?());
    yield return (object) new WaitForEndOfFrame();
  }

  public IEnumerator initializeMagic(DetailMenuScrollViewInfo.WeaponRow row, PlayerUnitSkills magic)
  {
    DetailMenuScrollViewInfo menuScrollViewInfo = this;
    BattleskillSkill skill1 = magic.skill;
    GearAttackClassification attackClass = GearAttackClassification.magic;
    row.top_.SetActive(true);
    row.iconClass_.Initialize(attackClass);
    row.spriteElement_.sprite2D = menuScrollViewInfo.loadElementIcon(skill1.element);
    row.spriteKind_.sprite2D = menuScrollViewInfo.loadGearKindIcon(GearKindEnum.magic);
    row.txtName_.SetTextLocalize(skill1.name);
    int hp = (menuScrollViewInfo.isMemory ? Judgement.NonBattleParameter.FromPlayerUnitMemory(menuScrollViewInfo.playerUnit) : Judgement.NonBattleParameter.FromPlayerUnit(menuScrollViewInfo.playerUnit)).Hp;
    BattleskillSkill skill2 = magic.skill;
    int consumeHp = skill2.consume_hp;
    foreach (BattleskillEffect battleskillEffect in ((IEnumerable<BattleskillEffect>) skill2.Effects).Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.percentage_hp_consume_magic)))
      consumeHp += Mathf.CeilToInt((float) ((Decimal) hp * (Decimal) battleskillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.cost_percentage)));
    row.txtRange_.SetTextLocalize(skill2.min_range.ToString() + "-" + (object) skill2.max_range);
    menuScrollViewInfo.setTextCost(row.txtCost_, new int?(consumeHp));
    menuScrollViewInfo.setEventPopupSkill(row.button_, skill1, magic.level, attackClass);
    yield return (object) new WaitForEndOfFrame();
  }

  public IEnumerator initializeOptionAttack(
    DetailMenuScrollViewInfo.WeaponRow row,
    IAttackMethod attack)
  {
    DetailMenuScrollViewInfo menuScrollViewInfo = this;
    row.top_.SetActive(true);
    row.iconClass_.Initialize(attack.attackClass);
    row.spriteElement_.sprite2D = menuScrollViewInfo.loadElementIcon(attack.skill.element);
    row.spriteKind_.sprite2D = menuScrollViewInfo.loadGearKindIcon(attack.kind.Enum);
    row.txtName_.SetTextLocalize(attack.skill.name);
    int hp = (menuScrollViewInfo.isMemory ? Judgement.NonBattleParameter.FromPlayerUnitMemory(menuScrollViewInfo.playerUnit) : Judgement.NonBattleParameter.FromPlayerUnit(menuScrollViewInfo.playerUnit)).Hp;
    BattleskillSkill skill = attack.skill;
    int consumeHp = skill.consume_hp;
    foreach (BattleskillEffect battleskillEffect in ((IEnumerable<BattleskillEffect>) skill.Effects).Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.percentage_hp_consume_magic)))
      consumeHp += Mathf.CeilToInt((float) ((Decimal) hp * (Decimal) battleskillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.cost_percentage)));
    row.txtRange_.SetTextLocalize(skill.min_range.ToString() + "-" + (object) skill.max_range);
    menuScrollViewInfo.setTextCost(row.txtCost_, new int?(consumeHp));
    menuScrollViewInfo.setEventPopupSkill(row.button_, attack.skill, attack.skill.upper_level, attack.attackClass);
    yield return (object) new WaitForEndOfFrame();
  }

  private Sprite loadElementIcon(CommonElement element)
  {
    return this.elementTypeIconPrefab.GetComponent<CommonElementIcon>().getIcon(element);
  }

  private Sprite loadGearKindIcon(GearKindEnum kind)
  {
    return GearKindIcon.LoadSprite(kind, CommonElement.none);
  }

  private string toRangeString(int min, int max)
  {
    return string.Format("{0} - {1}", (object) min, (object) max);
  }

  private void setTextCost(UILabel label, int? cost)
  {
    if (cost.HasValue)
      label.SetTextLocalize(cost.Value.ToString());
    else
      label.SetTextLocalize("-");
  }

  private PlayerUnitSkills getBulletByGear(PlayerUnitSkills[] bullets, PlayerItem gear)
  {
    if (bullets == null || bullets.Length == 0 || gear == (PlayerItem) null)
      return (PlayerUnitSkills) null;
    BattleskillSkill gs = Array.Find<GearGearSkill>(gear.skills, (Predicate<GearGearSkill>) (s => s.skill.skill_type == BattleskillSkillType.magic))?.skill;
    return gs == null ? (PlayerUnitSkills) null : Array.Find<PlayerUnitSkills>(bullets, (Predicate<PlayerUnitSkills>) (s => s.skill_id == gs.ID));
  }

  private BL.MagicBullet getBulletByGear(BL.MagicBullet[] bullets, PlayerItem gear)
  {
    if (bullets == null || bullets.Length == 0 || gear == (PlayerItem) null)
      return (BL.MagicBullet) null;
    BattleskillSkill gs = Array.Find<GearGearSkill>(gear.skills, (Predicate<GearGearSkill>) (s => s.skill.skill_type == BattleskillSkillType.magic))?.skill;
    return gs == null ? (BL.MagicBullet) null : Array.Find<BL.MagicBullet>(bullets, (Predicate<BL.MagicBullet>) (s => s.skillId == gs.ID));
  }

  private void setEventPopupSkill(
    UIButton btn,
    BattleskillSkill skill,
    int level,
    GearAttackClassification attackClass)
  {
    btn.onClick.Clear();
    EventDelegate.Add(btn.onClick, (EventDelegate.Callback) (() => this.popupSkill(skill, level, attackClass)));
  }

  private void popupSkill(BattleskillSkill skill, int level, GearAttackClassification attackClass)
  {
    if (Object.op_Equality((Object) this.attackMethodDialog, (Object) null))
    {
      this.attackMethodDialog = this.attackMethodDialogPrefab.Clone(this.attackMethodDialogRoot.transform);
      this.attackMethodDialog.GetComponentInChildren<UIPanel>().depth += 30;
      this.attackMethodDialog.SetActive(false);
    }
    DetailAttackMenuDialog componentInChildren = this.attackMethodDialog.GetComponentInChildren<DetailAttackMenuDialog>();
    if (Object.op_Equality((Object) componentInChildren, (Object) null))
      return;
    componentInChildren.setSkillProperty(true);
    componentInChildren.setData(skill, "", attackClass);
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1006");
    componentInChildren.Show();
    this.attackMethodDialog.SetActive(true);
  }

  private IEnumerator setWeapon(PlayerUnit pU)
  {
    IEnumerator e = this.infoWeapon.initAsync(pU, false, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.infoOption.initAsync(pU, false, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public override IEnumerator initAsyncDiffMode(
    PlayerUnit playerUnit,
    PlayerUnit prevUnit,
    IDetailMenuContainer menuContainer)
  {
    IEnumerator e = this.initAsync(playerUnit, false, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  [Serializable]
  public class WeaponRow
  {
    [SerializeField]
    [Tooltip("先頭オブジェクト")]
    public GameObject top_;
    [SerializeField]
    [Tooltip("攻撃区分")]
    public AttackClassIcon iconClass_;
    [SerializeField]
    [Tooltip("武具種")]
    public UI2DSprite spriteKind_;
    [SerializeField]
    [Tooltip("攻撃属性")]
    public UI2DSprite spriteElement_;
    [SerializeField]
    [Tooltip("ボタン")]
    public UIButton button_;
    [SerializeField]
    [Tooltip("名前")]
    public UILabel txtName_;
    [SerializeField]
    [Tooltip("範囲")]
    public UILabel txtRange_;
    [SerializeField]
    [Tooltip("コスト")]
    public UILabel txtCost_;
  }
}
