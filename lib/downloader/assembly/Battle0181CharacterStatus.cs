// Decompiled with JetBrains decompiler
// Type: Battle0181CharacterStatus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Battle0181CharacterStatus : NGBattleMenuBase
{
  private const int SKILL_ICON_SLIDER_NUM = 4;
  [SerializeField]
  protected UI2DSprite[] link_skillIcons = new UI2DSprite[4];
  [SerializeField]
  protected UILabel[] txt_skillNames = new UILabel[4];
  [SerializeField]
  protected GameObject[] go_skillBases = new GameObject[4];
  protected Coroutine[] skill_slide_anims = new Coroutine[4];
  [SerializeField]
  protected NGTweenGaugeScale hpGauge;
  [SerializeField]
  protected UISprite link_spattackUp;
  [SerializeField]
  protected UISprite link_spattackDown;
  [SerializeField]
  protected UILabel txt_weaponName;
  [SerializeField]
  protected UI2DSprite link_weaponIcon;
  [SerializeField]
  protected UILabel txt_hpMax;
  [SerializeField]
  protected UILabel txt_hp;
  [SerializeField]
  protected UILabel txt_characterName_ElementOn;
  [SerializeField]
  protected UILabel txt_consumeHp;
  [SerializeField]
  protected GameObject go_weaponTypeIcon;
  [SerializeField]
  protected UILabel txt_attack;
  [SerializeField]
  protected UILabel txt_dex;
  [SerializeField]
  protected UILabel txt_critical;
  [SerializeField]
  protected UI2DSprite link_characterIcon;
  [SerializeField]
  protected GameObject link_elementIcon;
  [SerializeField]
  protected GameObject go_commonElementIcon;
  [SerializeField]
  protected float flSkillDispTime;
  [SerializeField]
  protected UILabel TxtHpNumber;
  [SerializeField]
  protected GameObject attackCountOwner;
  [SerializeField]
  protected GameObject[] attackCount;
  [SerializeField]
  private UISprite slcCountry;
  private GameObject gearTypeIcon;
  private GameObject elementIcon;
  protected BL.UnitPosition current;
  private UnitIcon unitIcon;
  protected int currentHp;
  protected int maxHp;
  public bool isHpDamaged;
  private bool isDemoMode;
  private static readonly string StrDemoParam = "---";
  private const int DEF_DEMO_VALUE = 100;
  private List<int> severalSkillIds = new List<int>(4);
  private List<ParticleSystem> particleList = new List<ParticleSystem>();
  private int tmpAttackCount;

  protected GameObject createIcon(GameObject prefab, Transform trans)
  {
    GameObject icon = prefab.Clone(trans);
    UI2DSprite componentInChildren1 = icon.GetComponentInChildren<UI2DSprite>();
    UI2DSprite componentInChildren2 = ((Component) trans).GetComponentInChildren<UI2DSprite>();
    ((UIWidget) componentInChildren1).SetDimensions(((UIWidget) componentInChildren2).width, ((UIWidget) componentInChildren2).height);
    ((UIWidget) componentInChildren1).depth = 100;
    return icon;
  }

  public virtual IEnumerator Init(
    BL.UnitPosition up,
    AttackStatus attackStatus,
    int firstAttack,
    bool isColosseum,
    bool isDemo)
  {
    for (int index = 0; index < 4; ++index)
      this.skill_slide_anims[index] = (Coroutine) null;
    this.current = up;
    BL.Unit unit = up.unit;
    this.currentHp = unit.hp;
    this.maxHp = unit?.parameter?.Hp ?? this.currentHp;
    this.isDemoMode = isDemo;
    if (isDemo)
    {
      this.currentHp = this.maxHp = 100;
      ((IEnumerable<GameObject>) new GameObject[2]
      {
        Object.op_Inequality((Object) this.link_spattackUp, (Object) null) ? ((Component) this.link_spattackUp).gameObject : (GameObject) null,
        Object.op_Inequality((Object) this.link_spattackDown, (Object) null) ? ((Component) this.link_spattackDown).gameObject : (GameObject) null
      }).ToggleOnceEx(-1);
      this.TxtHpNumber.applyGradient = false;
      ((UIWidget) this.TxtHpNumber).color = ((UIWidget) this.txt_attack).color = ((UIWidget) this.txt_critical).color = ((UIWidget) this.txt_dex).color = Color.grey;
    }
    if (unit.skills.Length != 0)
      this.txt_skillNames[0].SetTextLocalize(unit.skills[0].name);
    this.hpGauge.setValue(this.currentHp, this.maxHp);
    this.txt_weaponName.SetTextLocalize(this.GetWeaponName(unit.playerUnit));
    if (Object.op_Equality((Object) this.gearTypeIcon, (Object) null))
      this.gearTypeIcon = this.createIcon(this.go_weaponTypeIcon, ((Component) this.link_weaponIcon).transform);
    if (Object.op_Inequality((Object) null, (Object) this.gearTypeIcon))
    {
      GearKindIcon component = this.gearTypeIcon.GetComponent<GearKindIcon>();
      if (Object.op_Inequality((Object) component, (Object) null))
        component.Init(this.GetWeaponKind(unit.playerUnit), this.GetWeaponElement(unit.playerUnit));
    }
    this.SetUnitGearIcon(unit);
    if (Object.op_Implicit((Object) this.txt_consumeHp))
      this.txt_consumeHp.SetTextLocalize("");
    this.setHPNumbers(!isDemo ? this.currentHp.ToString() : Battle0181CharacterStatus.StrDemoParam);
    ((IEnumerable<GameObject>) this.attackCount).ToggleOnceEx(-1);
    if (!isDemo && attackStatus != null)
    {
      this.setAttackCount(attackStatus.attackCount);
      float damageRate = attackStatus.duelParameter.DamageRate;
      attackStatus.duelParameter.DamageRate *= attackStatus.elementAttackRate * attackStatus.attackClassificationRate * attackStatus.normalDamageRate;
      int num = Mathf.Max(Mathf.FloorToInt(attackStatus.originalAttack), 1);
      attackStatus.duelParameter.DamageRate = damageRate;
      string text = num.ToString();
      if (!isColosseum)
      {
        if ((double) attackStatus.elementAttackRate > 1.0)
          text = "[00ff00]" + text;
        else if ((double) attackStatus.elementAttackRate < 1.0)
          text = "[ff0000]" + text;
      }
      else if (num > firstAttack)
        text = "[00ff00]" + text;
      else if (num < firstAttack)
        text = "[ff0000]" + text;
      this.txt_attack.SetTextLocalize(text);
      this.txt_critical.SetTextLocalize(attackStatus.criticalDisplay().ToString() + "%");
      this.txt_dex.SetTextLocalize(attackStatus.dexerityDisplay().ToString() + "%");
    }
    else
    {
      this.txt_attack.SetText(Battle0181CharacterStatus.StrDemoParam);
      this.txt_critical.SetText(Battle0181CharacterStatus.StrDemoParam);
      this.txt_dex.SetText(Battle0181CharacterStatus.StrDemoParam);
    }
    ((IEnumerable<GameObject>) this.go_skillBases).ToggleOnceEx(-1);
    this.isHpDamaged = false;
    if (Object.op_Inequality((Object) this.slcCountry, (Object) null))
    {
      ((Component) this.slcCountry).gameObject.SetActive(false);
      if (up.unit.unit.country_attribute.HasValue)
      {
        ((Component) this.slcCountry).gameObject.SetActive(true);
        up.unit.unit.SetCuntrySpriteName(ref this.slcCountry);
      }
    }
    yield return (object) this.LoadCharaIcon();
  }

  public virtual void ChangeStatus(BL.UnitPosition up, AttackStatus attackStatus, int firstAttack)
  {
    if (this.isDemoMode || attackStatus.duelParameter == null)
      return;
    this.txt_weaponName.SetTextLocalize(this.GetWeaponName(up.unit.playerUnit));
    this.attackCount[0].SetActive(false);
    this.attackCount[1].SetActive(false);
    this.attackCount[2].SetActive(false);
    if (attackStatus != null)
      this.setAttackCount(attackStatus.attackCount);
    if (attackStatus != null)
    {
      float damageRate = attackStatus.duelParameter.DamageRate;
      attackStatus.duelParameter.DamageRate *= attackStatus.elementAttackRate * attackStatus.attackClassificationRate * attackStatus.normalDamageRate;
      int num = Mathf.Max(Mathf.FloorToInt(attackStatus.originalAttack), 1);
      attackStatus.duelParameter.DamageRate = damageRate;
      string text = num.ToString();
      if (num > firstAttack)
        text = "[00ff00]" + text;
      else if (num < firstAttack)
        text = "[ff0000]" + text;
      this.txt_attack.SetTextLocalize(text);
      this.txt_critical.SetTextLocalize(attackStatus.criticalDisplay().ToString() + "%");
      this.txt_dex.SetTextLocalize(attackStatus.dexerityDisplay().ToString() + "%");
    }
    else
    {
      this.txt_attack.SetText("-");
      this.txt_critical.SetText("-");
      this.txt_dex.SetText("-");
    }
    foreach (GameObject goSkillBase in this.go_skillBases)
      goSkillBase.SetActive(false);
  }

  protected void SetUnitGearIcon(BL.Unit unit)
  {
    if (Object.op_Equality((Object) this.elementIcon, (Object) null))
      this.elementIcon = this.createIcon(this.go_weaponTypeIcon, this.link_elementIcon.transform);
    UILabel characterNameElementOn = this.txt_characterName_ElementOn;
    UnitUnit unit1 = unit.unit;
    SkillMetamorphosis metamorphosis = unit.metamorphosis;
    int metamorphosisId = metamorphosis != null ? metamorphosis.metamorphosis_id : 0;
    string name = unit1.getName(metamorphosisId);
    characterNameElementOn.SetTextLocalize(name);
    if (!Object.op_Inequality((Object) this.elementIcon, (Object) null))
      return;
    this.elementIcon.GetComponent<GearKindIcon>().Init(unit.unit.kind, unit.playerUnit.GetElement());
  }

  private void setAttackCount(int count)
  {
    if (this.current.unit.playerUnit.getDualWieldSkillData() != null)
      count /= this.current.unit.playerUnit.normalAttackCount;
    int num = count - 2;
    int index = 2 != count ? (3 != count ? (4 != count ? -1 : 2) : 1) : 0;
    if (index < 0)
      return;
    this.attackCount[index].SetActive(true);
    foreach (UITweener uiTweener in NGTween.findTweenersAll(this.attackCountOwner, false))
    {
      if (uiTweener.tweenGroup == 101)
        uiTweener.ResetToBeginning();
    }
  }

  private IEnumerator LoadCharaIcon()
  {
    IEnumerator e;
    if (Object.op_Equality((Object) this.unitIcon, (Object) null))
    {
      Future<GameObject> f = !Singleton<NGGameDataManager>.GetInstance().IsSea ? Res.Prefabs.UnitIcon.normal.Load<GameObject>() : Res.Prefabs.Sea.UnitIcon.normal_sea.Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject gameObject = Object.Instantiate<GameObject>(f.Result);
      gameObject.transform.parent = ((Component) this.link_characterIcon).transform;
      gameObject.transform.localPosition = Vector3.zero;
      gameObject.transform.localScale = Vector3.one;
      this.unitIcon = gameObject.GetComponent<UnitIcon>();
      f = (Future<GameObject>) null;
    }
    e = this.unitIcon.SetUnit(this.current.unit.playerUnit, this.current.unit.metamorphosis, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!this.isDemoMode)
      this.unitIcon.setLevelText(this.current.unit.playerUnit);
    else
      this.unitIcon.setLevelText("--");
    this.unitIcon.BottomModeValue = UnitIconBase.GetBottomModeLevel(this.current.unit.unit, this.current.unit.playerUnit);
    ((Component) this.unitIcon.RarityStar).gameObject.SetActive(false);
  }

  public BL.UnitPosition getCurrent() => this.current;

  public void Damaged(int damage, int? heal = null)
  {
    if (this.isDemoMode)
      return;
    this.currentHp -= Mathf.Max(0, damage);
    this.txt_consumeHp.SetTextLocalize(damage);
    ((IEnumerable<UITweener>) ((Component) this.txt_consumeHp).GetComponentsInChildren<UITweener>()).ForEach<UITweener>((Action<UITweener>) (x =>
    {
      x.ResetToBeginning();
      x.PlayForward();
    }));
    if (heal.HasValue)
      this.currentHp += Mathf.Max(0, heal.Value);
    this.currentHp = Mathf.Clamp(this.currentHp, 0, this.maxHp);
    this.setHPNumbers(this.currentHp.ToString());
    this.hpGauge.setValue(this.currentHp, this.maxHp);
  }

  public virtual void Healed(int heal)
  {
    if (this.isDemoMode)
      return;
    this.currentHp += Math.Min(this.maxHp - this.currentHp, heal);
    if (this.maxHp < this.currentHp)
      this.currentHp = this.maxHp;
    this.hpGauge.setValue(this.currentHp, this.current.unit.parameter.Hp);
    this.setHPNumbers(this.currentHp.ToString());
  }

  public void skillInvoke(BL.Skill[] skills, bool bResetEntry)
  {
    this.severalSkillsInvoke(skills, bResetEntry);
  }

  private void severalSkillsInvoke(BL.Skill[] skills, bool bResetEntry)
  {
    if (bResetEntry)
      this.severalSkillIds.Clear();
    if (skills == null)
      return;
    for (int index = 0; index < skills.Length && index < 4; ++index)
    {
      if (!this.severalSkillIds.Contains(skills[index].id))
      {
        int count = this.severalSkillIds.Count;
        if (count >= 4)
          break;
        this.severalSkillIds.Add(skills[index].id);
        this.startDisplaySkill(count, skills[index]);
      }
    }
  }

  public void skillInvoke(BL.Skill skill) => this.startDisplaySkill(0, skill);

  private void startDisplaySkill(int posIndex, BL.Skill skill)
  {
    if (this.skill_slide_anims[posIndex] != null)
    {
      this.StopCoroutine(this.skill_slide_anims[posIndex]);
      if (Object.op_Inequality((Object) this.go_skillBases[posIndex], (Object) null))
        this.go_skillBases[posIndex].SetActive(false);
    }
    this.skill_slide_anims[posIndex] = this.StartCoroutine(this.displaySkill(posIndex, skill));
  }

  private IEnumerator displaySkill(int posIndex, BL.Skill iskill)
  {
    Battle0181CharacterStatus battle0181CharacterStatus = this;
    BattleskillSkill skill = MasterData.BattleskillSkill[iskill.id];
    Future<Sprite> fsi = skill.LoadBattleSkillIcon();
    IEnumerator e = fsi.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    battle0181CharacterStatus.link_skillIcons[posIndex].sprite2D = fsi.Result;
    battle0181CharacterStatus.setText(battle0181CharacterStatus.txt_skillNames[posIndex], skill.name);
    battle0181CharacterStatus.go_skillBases[posIndex].SetActive(true);
    foreach (UITweener component in battle0181CharacterStatus.go_skillBases[posIndex].GetComponents<UITweener>())
    {
      component.ResetToBeginning();
      component.PlayForward();
    }
    yield return (object) new WaitForSeconds(battle0181CharacterStatus.flSkillDispTime);
    if (Object.op_Inequality((Object) null, (Object) battle0181CharacterStatus.go_skillBases[posIndex]))
      battle0181CharacterStatus.go_skillBases[posIndex].SetActive(false);
    battle0181CharacterStatus.skill_slide_anims[posIndex] = (Coroutine) null;
  }

  protected void setHPNumbers(string hp) => this.TxtHpNumber.SetTextLocalize(hp);

  public void colosseumParameterChangeEffect(
    AttackStatus attackStatus,
    AttackStatus newAttackStatus,
    int firstAttack,
    GameObject upEffect,
    GameObject downEffect)
  {
    if (this.isDemoMode || newAttackStatus == null)
      return;
    float damageRate1 = attackStatus.duelParameter.DamageRate;
    attackStatus.duelParameter.DamageRate *= attackStatus.elementAttackRate * attackStatus.attackClassificationRate * attackStatus.normalDamageRate;
    int num1 = Mathf.Max(Mathf.FloorToInt(attackStatus.originalAttack), 1);
    attackStatus.duelParameter.DamageRate = damageRate1;
    float damageRate2 = newAttackStatus.duelParameter.DamageRate;
    newAttackStatus.duelParameter.DamageRate *= newAttackStatus.elementAttackRate * newAttackStatus.attackClassificationRate * newAttackStatus.normalDamageRate;
    int num2 = Mathf.Max(Mathf.FloorToInt(newAttackStatus.originalAttack), 1);
    newAttackStatus.duelParameter.DamageRate = damageRate2;
    if (num1 != num2)
    {
      string text = num2.ToString();
      if (num2 > firstAttack)
        text = "[00ff00]" + text;
      else if (num2 < firstAttack)
        text = "[ff0000]" + text;
      this.txt_attack.SetTextLocalize(text);
      if (num1 < num2)
      {
        ParticleSystem component = upEffect.CloneAndGetComponent<ParticleSystem>(((Component) this.txt_attack).gameObject.transform);
        if (Object.op_Inequality((Object) component, (Object) null))
          this.particleList.Add(component);
      }
      else if (num1 > num2)
      {
        ParticleSystem component = downEffect.CloneAndGetComponent<ParticleSystem>(((Component) this.txt_attack).gameObject.transform);
        if (Object.op_Inequality((Object) component, (Object) null))
          this.particleList.Add(component);
      }
    }
    if (attackStatus.dexerityDisplay() != newAttackStatus.dexerityDisplay())
    {
      this.txt_dex.SetTextLocalize(newAttackStatus.dexerityDisplay().ToString() + "%");
      if (attackStatus.dexerityDisplay() < newAttackStatus.dexerityDisplay())
      {
        ParticleSystem component = upEffect.CloneAndGetComponent<ParticleSystem>(((Component) this.txt_dex).gameObject.transform);
        if (Object.op_Inequality((Object) component, (Object) null))
          this.particleList.Add(component);
      }
      else if (attackStatus.dexerityDisplay() > newAttackStatus.dexerityDisplay())
      {
        ParticleSystem component = downEffect.CloneAndGetComponent<ParticleSystem>(((Component) this.txt_dex).gameObject.transform);
        if (Object.op_Inequality((Object) component, (Object) null))
          this.particleList.Add(component);
      }
    }
    if (attackStatus.criticalDisplay() == newAttackStatus.criticalDisplay())
      return;
    this.txt_critical.SetTextLocalize(newAttackStatus.criticalDisplay().ToString() + "%");
    if (attackStatus.criticalDisplay() < newAttackStatus.criticalDisplay())
    {
      ParticleSystem component = upEffect.CloneAndGetComponent<ParticleSystem>(((Component) this.txt_critical).gameObject.transform);
      if (!Object.op_Inequality((Object) component, (Object) null))
        return;
      this.particleList.Add(component);
    }
    else
    {
      if (attackStatus.criticalDisplay() <= newAttackStatus.criticalDisplay())
        return;
      ParticleSystem component = downEffect.CloneAndGetComponent<ParticleSystem>(((Component) this.txt_critical).gameObject.transform);
      if (!Object.op_Inequality((Object) component, (Object) null))
        return;
      this.particleList.Add(component);
    }
  }

  public void startColosseumParameterChangeEffect(
    AttackStatus attackStatus,
    AttackStatus newAttackStatus)
  {
    if (this.isDemoMode || newAttackStatus == null)
      return;
    foreach (ParticleSystem particle in this.particleList)
      particle.Play();
    this.particleList.Clear();
    if (attackStatus.attackCount < newAttackStatus.attackCount)
    {
      UITweener[] tweenersAll = NGTween.findTweenersAll(this.attackCountOwner, false);
      this.tmpAttackCount = newAttackStatus.attackCount;
      if (attackStatus.attackCount > 1)
      {
        NGTween.setOnTweenFinished(tweenersAll, (MonoBehaviour) this, "AfterAttackCountUpAnime");
        NGTween.playTweens(tweenersAll, 100, true);
      }
      else
        this.AfterAttackCountUpAnime();
    }
    else
    {
      if (attackStatus.attackCount <= newAttackStatus.attackCount)
        return;
      UITweener[] tweenersAll = NGTween.findTweenersAll(this.attackCountOwner, false);
      this.tmpAttackCount = newAttackStatus.attackCount;
      if (attackStatus.attackCount > 2)
      {
        NGTween.setOnTweenFinished(tweenersAll, (MonoBehaviour) this, "AfterAttackCountDownAnime");
        NGTween.playTweens(tweenersAll, 101, true);
      }
      else
        this.AfterAttackCountDownAnime();
    }
  }

  private void AfterAttackCountUpAnime()
  {
    this.attackCount[0].SetActive(false);
    this.attackCount[1].SetActive(false);
    this.attackCount[2].SetActive(false);
    this.setAttackCount(this.tmpAttackCount);
    NGTween.playTweens(NGTween.findTweenersAll(this.attackCountOwner, false), 100);
  }

  private void AfterAttackCountDownAnime()
  {
    UITweener[] tweenersAll = NGTween.findTweenersAll(this.attackCountOwner, false);
    NGTween.playTweens(tweenersAll, 101);
    NGTween.setOnTweenFinished(tweenersAll, (MonoBehaviour) this, "SetAttackCount");
  }

  private void SetAttackCount()
  {
    this.attackCount[0].SetActive(false);
    this.attackCount[1].SetActive(false);
    this.attackCount[2].SetActive(false);
    this.setAttackCount(this.tmpAttackCount);
  }

  private string GetWeaponName(PlayerUnit unit)
  {
    string empty = string.Empty;
    return !(unit.equippedGear == (PlayerItem) null) || !(unit.equippedGear2 != (PlayerItem) null) ? unit.equippedGearName : unit.equippedGearName2;
  }

  private CommonElement GetWeaponElement(PlayerUnit unit)
  {
    return !(unit.equippedGear == (PlayerItem) null) || !(unit.equippedGear2 != (PlayerItem) null) ? (unit.equippedGear != (PlayerItem) null ? unit.equippedGear.GetElement() : unit.equippedGearOrInitial.GetElement()) : unit.equippedGear2.GetElement();
  }

  private GearKind GetWeaponKind(PlayerUnit unit)
  {
    return !(unit.equippedGear == (PlayerItem) null) || !(unit.equippedGear2 != (PlayerItem) null) ? unit.equippedGearOrInitial.kind : unit.equippedGear2OrInitial.kind;
  }
}
