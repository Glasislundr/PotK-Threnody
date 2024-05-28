// Decompiled with JetBrains decompiler
// Type: BattlePrepareCharacterInfoBase
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
public abstract class BattlePrepareCharacterInfoBase : NGBattleMenuBase
{
  [SerializeField]
  protected UILabel TxtAttack;
  [SerializeField]
  protected UILabel TxtCharaname_element;
  [SerializeField]
  protected GameObject ElementIconParent;
  [SerializeField]
  protected UILabel TxtCritical;
  [SerializeField]
  protected UILabel TxtDexterity;
  [SerializeField]
  protected UILabel TxtHpNumber;
  [SerializeField]
  protected UILabel TxtWeaponName;
  [SerializeField]
  protected UILabel TxtSecondWeaponName;
  [SerializeField]
  protected Transform ImageParent;
  [SerializeField]
  protected int depth;
  [SerializeField]
  protected UI2DSprite iconGear;
  [SerializeField]
  protected UI2DSprite iconSecondGear;
  [SerializeField]
  protected GameObject upCompatibility;
  [SerializeField]
  protected GameObject downCompatibility;
  [SerializeField]
  protected NGTweenGaugeScale hpBar;
  [SerializeField]
  protected UILabel TxtConsume;
  [SerializeField]
  protected GameObject[] attackCount;
  [SerializeField]
  protected UI2DSprite iconDs;
  [SerializeField]
  protected UILabel TxtDsActivationRate;
  [SerializeField]
  protected TweenAlpha tweenAlpha_first_weapon;
  [SerializeField]
  protected TweenAlpha tweenAlpha_second_weapon;
  [SerializeField]
  private UIButton ailmentsDetailBtn;
  [SerializeField]
  private UISprite slcCountry;
  protected AttackStatus[] attacks;
  protected AttackStatus[] originAttacks;
  private AttackStatus currentAttack;
  private BL.UnitPosition currentUnit;
  private GameObject gearIconPrefab;
  private BattleUI04Menu baseMenu;
  private BL.Skill duelSkill;

  protected abstract ResourceObject maskResource();

  protected abstract Tuple<BL.Skill, float, bool> SimulateDuelSkill(
    BL.UnitPosition opponent,
    AttackStatus opponentAS);

  public virtual IEnumerator LoadPrefab(bool isSea)
  {
    Future<GameObject> loader = Res.Icons.GearKindIcon.Load<GameObject>();
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.gearIconPrefab = loader.Result;
  }

  public virtual IEnumerator Init(
    BL.UnitPosition up,
    AttackStatus[] attacks_,
    bool isFacility,
    BattleUI04Menu baseMenu)
  {
    this.baseMenu = baseMenu;
    this.currentUnit = up;
    this.originAttacks = attacks_;
    this.attacks = ((IEnumerable<AttackStatus>) attacks_).OrderBy<AttackStatus, int>((Func<AttackStatus, int>) (x => x.magicBullet == null || !x.magicBullet.isAttack ? 1000 - x.attack : x.magicBullet.cost * 1000 + (1000 - x.attack))).ToArray<AttackStatus>();
    this.upCompatibility.SetActive(false);
    this.downCompatibility.SetActive(false);
    BL.Unit unit = this.currentUnit.unit;
    this.InitHpNumbers(unit);
    this.InitGearIcon(unit);
    this.InitElementIcon(unit);
    this.InitCountryIcon(unit);
    ((UIButtonColor) this.ailmentsDetailBtn).isEnabled = BattleFuncs.getInvestSkills(this.currentUnit).Any<BattleFuncs.InvestSkill>();
    IEnumerator e = this.InitUnitImage(unit, isFacility);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private void InitHpNumbers(BL.Unit unit)
  {
    this.TxtHpNumber.SetTextLocalize(unit.hp);
    this.hpBar.setValue(unit.hp, unit.parameter.Hp, false);
  }

  private void InitGearIcon(BL.Unit unit)
  {
    PlayerItem equippedGear = unit.playerUnit.equippedGear;
    PlayerItem equippedGear2 = unit.playerUnit.equippedGear2;
    this.TxtWeaponName.SetTextLocalize(unit.playerUnit.equippedGearName);
    UILabel secondWeaponName = this.TxtSecondWeaponName;
    if (secondWeaponName != null)
      secondWeaponName.SetTextLocalize(equippedGear2 != (PlayerItem) null ? equippedGear2.name : string.Empty);
    ((Component) this.iconGear).transform.Clear();
    GameObject gameObject1 = this.gearIconPrefab.Clone(((Component) this.iconGear).transform);
    gameObject1.GetComponent<UIWidget>().depth = ((UIWidget) this.iconGear).depth + 1;
    gameObject1.GetComponent<GearKindIcon>().Init(unit.playerUnit.equippedGearOrInitial.kind, unit.playerUnit.equippedGearOrInitial.GetElement());
    if (Object.op_Inequality((Object) this.iconSecondGear, (Object) null))
    {
      ((Component) this.iconSecondGear).transform.Clear();
      if (equippedGear2 != (PlayerItem) null)
      {
        GameObject gameObject2 = this.gearIconPrefab.Clone(((Component) this.iconSecondGear).transform);
        gameObject2.GetComponent<UIWidget>().depth = ((UIWidget) this.iconSecondGear).depth + 1;
        gameObject2.GetComponent<GearKindIcon>().Init(equippedGear2.gear.kind, equippedGear2.GetElement());
      }
    }
    if (!Object.op_Inequality((Object) this.tweenAlpha_first_weapon, (Object) null) || !Object.op_Inequality((Object) this.tweenAlpha_second_weapon, (Object) null))
      return;
    ((Behaviour) this.tweenAlpha_first_weapon).enabled = false;
    ((Behaviour) this.tweenAlpha_second_weapon).enabled = false;
    ((UIRect) ((Component) this.tweenAlpha_first_weapon).GetComponent<UIWidget>()).alpha = 1f;
    ((UIRect) ((Component) this.tweenAlpha_second_weapon).GetComponent<UIWidget>()).alpha = 0.0f;
    if (equippedGear != (PlayerItem) null && equippedGear2 != (PlayerItem) null)
    {
      this.StartCoroutine(this.StartWeaponNameAnim());
    }
    else
    {
      if (!(equippedGear == (PlayerItem) null) || !(equippedGear2 != (PlayerItem) null))
        return;
      ((UIRect) ((Component) this.tweenAlpha_first_weapon).GetComponent<UIWidget>()).alpha = 0.0f;
      ((UIRect) ((Component) this.tweenAlpha_second_weapon).GetComponent<UIWidget>()).alpha = 1f;
    }
  }

  private IEnumerator StartWeaponNameAnim()
  {
    yield return (object) new WaitForSeconds(0.5f);
    ((UITweener) this.tweenAlpha_first_weapon).ResetToBeginning();
    ((UITweener) this.tweenAlpha_second_weapon).ResetToBeginning();
    ((UITweener) this.tweenAlpha_first_weapon).PlayForward();
    ((UITweener) this.tweenAlpha_second_weapon).PlayForward();
  }

  private void InitElementIcon(BL.Unit unit)
  {
    UILabel charanameElement = this.TxtCharaname_element;
    UnitUnit unit1 = unit.unit;
    SkillMetamorphosis metamorphosis = unit.metamorphosis;
    int metamorphosisId = metamorphosis != null ? metamorphosis.metamorphosis_id : 0;
    string name = unit1.getName(metamorphosisId);
    charanameElement.SetTextLocalize(name);
    this.ElementIconParent.transform.Clear();
    GameObject gameObject = this.gearIconPrefab.Clone(this.ElementIconParent.transform);
    gameObject.GetComponent<UIWidget>().depth = this.ElementIconParent.GetComponent<UIWidget>().depth + 1;
    gameObject.GetComponent<GearKindIcon>().Init(unit.unit.kind, unit.playerUnit.GetElement());
  }

  private void InitCountryIcon(BL.Unit unit)
  {
    if (!Object.op_Inequality((Object) this.slcCountry, (Object) null))
      return;
    if (unit.unit.country_attribute.HasValue)
    {
      ((Component) this.slcCountry).gameObject.SetActive(true);
      unit.unit.SetCuntrySpriteName(ref this.slcCountry);
    }
    else
      ((Component) this.slcCountry).gameObject.SetActive(false);
  }

  private IEnumerator InitUnitImage(BL.Unit unit, bool isFacility)
  {
    this.ImageParent.Clear();
    ((Component) this.ImageParent).gameObject.SetActive(!isFacility);
    if (!isFacility)
    {
      Future<GameObject> future = unit.unit.LoadStory();
      IEnumerator e = future.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject go = future.Result.Clone(this.ImageParent);
      SkillMetamorphosis metamorphosis = unit.metamorphosis;
      int jobOrMetamorId = metamorphosis != null ? metamorphosis.metamorphosis_id : unit.playerUnit.job_id;
      unit.unit.SetStoryData(go, ext_id: new int?(jobOrMetamorId));
      e = unit.unit.SetLargeSpriteWithMask(jobOrMetamorId, go, this.maskResource().Load<Texture2D>(), this.depth);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      future = (Future<GameObject>) null;
    }
  }

  public BL.UnitPosition getCurrent() => this.currentUnit;

  public AttackStatus getCurrentAttack() => this.currentAttack;

  public int getCurrentAttackOriginIndex()
  {
    return ((IEnumerable<AttackStatus>) this.originAttacks).FirstIndexOrNull<AttackStatus>((Func<AttackStatus, bool>) (x => x == this.currentAttack)).Value;
  }

  public AttackStatus getAttackStatus(int index) => this.attacks[index];

  public void setCurrentAttack(AttackStatus attackStatus)
  {
    this.currentAttack = attackStatus;
    int count = 0;
    if (this.currentAttack == null)
    {
      this.TxtAttack.SetText("-");
      this.TxtCritical.SetText("-");
      this.TxtDexterity.SetText("-");
    }
    else
    {
      string format = "{0}";
      if ((double) this.currentAttack.elementAttackRate > 1.0)
        format = "[00ff00]{0}[-]";
      else if ((double) this.currentAttack.elementAttackRate < 1.0)
        format = "[ff0000]{0}[-]";
      float damageRate = this.currentAttack.duelParameter.DamageRate;
      this.currentAttack.duelParameter.DamageRate *= this.currentAttack.elementAttackRate * this.currentAttack.attackClassificationRate * this.currentAttack.normalDamageRate;
      this.TxtAttack.SetTextLocalize(format.F((object) Mathf.Max(Mathf.FloorToInt(this.currentAttack.originalAttack), 1)));
      this.currentAttack.duelParameter.DamageRate = damageRate;
      this.TxtCritical.SetTextLocalize(this.currentAttack.criticalDisplay().ToString() + "%");
      this.TxtDexterity.SetTextLocalize(this.currentAttack.dexerityDisplay().ToString() + "%");
      count = this.currentAttack.attackCount;
      if (this.currentUnit.unit.playerUnit.getDualWieldSkillData() != null)
        count /= this.currentUnit.unit.playerUnit.normalAttackCount;
    }
    this.setAttackCount(count);
  }

  private void setAttackCount(int count)
  {
    int num = count - 2;
    for (int index = 0; index < this.attackCount.Length; ++index)
      this.attackCount[index].SetActive(index == num);
  }

  public void SetCompatibility(PlayerUnit opponent)
  {
    Tuple<int, int> gearKindIncr = this.currentUnit.unit.playerUnit.GetGearKindIncr(opponent);
    if (gearKindIncr.Item1 > 0 || gearKindIncr.Item2 > 0)
    {
      this.upCompatibility.SetActive(true);
    }
    else
    {
      if (gearKindIncr.Item1 >= 0 && gearKindIncr.Item2 >= 0)
        return;
      this.downCompatibility.SetActive(true);
    }
  }

  public void SetDsInfo(BL.UnitPosition opponent, AttackStatus opponentAS)
  {
    if (!Object.op_Inequality((Object) this.TxtDsActivationRate, (Object) null))
      return;
    if (this.getCurrentAttack() == null)
    {
      this.TxtDsActivationRate.SetTextLocalize("-");
      ((UIRect) this.iconDs).alpha = 0.0f;
    }
    else
    {
      BL.Skill skill1 = ((IEnumerable<BL.Skill>) this.getCurrent().unit.originalUnit.duelSkills).FirstOrDefault<BL.Skill>((Func<BL.Skill, bool>) (x =>
      {
        BattleskillGenre? genre1_1 = x.genre1;
        BattleskillGenre battleskillGenre1 = BattleskillGenre.attack;
        if (genre1_1.GetValueOrDefault() == battleskillGenre1 & genre1_1.HasValue)
          return true;
        BattleskillGenre? genre1_2 = x.genre1;
        BattleskillGenre battleskillGenre2 = BattleskillGenre.ailment;
        return genre1_2.GetValueOrDefault() == battleskillGenre2 & genre1_2.HasValue;
      }));
      if (skill1 == null)
      {
        this.TxtDsActivationRate.SetTextLocalize("未習得");
        ((UIRect) this.iconDs).alpha = 0.0f;
      }
      else
      {
        Tuple<BL.Skill, float, bool> tuple = this.SimulateDuelSkill(opponent, opponentAS);
        BL.Skill skill2 = tuple.Item1;
        int num = Mathf.Clamp(Mathf.FloorToInt(tuple.Item2), tuple.Item3 ? 0 : 1, 99);
        if (skill2 == null)
        {
          skill2 = skill1;
          this.TxtDsActivationRate.SetTextLocalize("0%");
        }
        else
          this.TxtDsActivationRate.SetTextLocalize(string.Format("{0}%", (object) num));
        this.StartCoroutine(this.LoadDuelSkillIcon(skill2));
      }
    }
  }

  private IEnumerator LoadDuelSkillIcon(BL.Skill skill)
  {
    this.duelSkill = skill;
    ((UIRect) this.iconDs).alpha = 0.0f;
    BattleskillSkill battleskillSkill;
    if (this.duelSkill != null && MasterData.BattleskillSkill.TryGetValue(this.duelSkill.id, out battleskillSkill))
    {
      Future<Sprite> ft = battleskillSkill.LoadBattleSkillIcon();
      IEnumerator e = ft.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (Object.op_Inequality((Object) ft.Result, (Object) null))
      {
        this.iconDs.sprite2D = ft.Result;
        ((UIRect) this.iconDs).alpha = 1f;
      }
      ft = (Future<Sprite>) null;
    }
  }

  public void OpenDuelSkillDetail() => this.baseMenu.OpenSkillDetail(this.duelSkill);

  public void OpenAilmentsDetail() => this.baseMenu.StartOpenAilmentsDetail(this.getCurrent());
}
