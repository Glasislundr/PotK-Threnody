// Decompiled with JetBrains decompiler
// Type: Shop00742Unit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Shop00742Unit : MonoBehaviour
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
  protected UILabel TxtJobName;
  [SerializeField]
  protected UILabel TxtLeaderSkillname;
  [SerializeField]
  protected UILabel TxtMatk;
  [SerializeField]
  protected UILabel TxtMdef;
  [SerializeField]
  protected UILabel TxtMove;
  [SerializeField]
  protected UILabel TxtName;
  [SerializeField]
  protected UILabel TxtSkillexplanation;
  [SerializeField]
  protected UI2DSprite SlcTarget;
  [SerializeField]
  protected UI2DSprite rarityStarIcon;
  [SerializeField]
  private UISprite slcCountry;
  private int pA;
  private int pD;
  private int mA;
  private int mD;
  private int hi;
  private int cr;
  private int ev;
  private int mv;
  [SerializeField]
  private UI2DSprite charaMask;
  [SerializeField]
  private UI2DSprite skillIcon;
  [SerializeField]
  private UI2DSprite weaponIcon;
  [SerializeField]
  private GameObject dirSkillNone;
  [SerializeField]
  protected GameObject TargetParent;

  public IEnumerator Init(UnitUnit target)
  {
    IEnumerator e = this.UnitSpriteCreate(target);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<GameObject> loader = Res.Icons.GearKindIcon.Load<GameObject>();
    e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    loader.Result.Clone(((Component) this.weaponIcon).transform).GetComponent<GearKindIcon>().Init(target.kind);
    loader = (Future<GameObject>) null;
    e = this.RarityCreate(target);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) this.slcCountry, (Object) null))
    {
      ((Component) this.slcCountry).gameObject.SetActive(false);
      if (target.country_attribute.HasValue)
      {
        ((Component) this.slcCountry).gameObject.SetActive(true);
        target.SetCuntrySpriteName(ref this.slcCountry);
      }
    }
    this.CalcShopUnitParameter(target, target.initial_gear);
    this.TxtAttack.SetTextLocalize(this.pA);
    this.TxtDefense.SetTextLocalize(this.pD);
    this.TxtMatk.SetTextLocalize(this.mA);
    this.TxtMdef.SetTextLocalize(this.mD);
    this.TxtDexterity.SetTextLocalize(this.hi);
    this.TxtCritical.SetTextLocalize(this.cr);
    this.TxtEvasion.SetTextLocalize(this.ev);
    this.TxtMove.SetTextLocalize(this.mv);
    this.TxtName.SetText(target.name);
    this.TxtJobName.SetText(target.job.name);
    this.dirSkillNone.SetActive(target.RememberLeaderSkill != null);
    if (this.dirSkillNone.activeSelf)
    {
      this.TxtLeaderSkillname.SetText(target.RememberLeaderSkill.name);
      this.TxtSkillexplanation.SetText(target.RememberLeaderSkill.description);
    }
  }

  public void CalcShopUnitParameter(UnitUnit unit, GearGear gear)
  {
    int num1 = unit.strength_initial + gear.strength_incremental + unit.job.strength_initial;
    int num2 = unit.vitality_initial + gear.vitality_incremental + unit.job.vitality_initial;
    int num3 = unit.intelligence_initial + gear.intelligence_incremental + unit.job.intelligence_initial;
    int num4 = unit.mind_initial + gear.mind_incremental + unit.job.mind_initial;
    int num5 = unit.agility_initial + gear.agility_incremental + unit.job.agility_initial;
    int num6 = unit.dexterity_initial + gear.dexterity_incremental + unit.job.dexterity_initial;
    int num7 = unit.lucky_initial + gear.lucky_incremental + unit.job.lucky_initial;
    int movement = unit.job.movement;
    int power = gear.power;
    int physicalDefense = gear.physical_defense;
    int magicDefense = gear.magic_defense;
    int hit1 = gear.hit;
    int critical = gear.critical;
    int evasion1 = gear.evasion;
    int num8 = 0;
    int num9 = 0;
    if (unit.initial_gear.attack_type == GearAttackType.magic)
      num9 = power;
    else
      num8 = power;
    UnitProficiencyIncr unitProficiencyIncr = ((IEnumerable<UnitProficiencyIncr>) MasterData.UnitProficiencyIncrList).Single<UnitProficiencyIncr>((Func<UnitProficiencyIncr, bool>) (x => x.kind.ID == unit.kind.ID && x.proficiency.ID == unit.default_weapon_proficiency.ID));
    int physicalAttack = unitProficiencyIncr.physical_attack;
    int magicAttack = unitProficiencyIncr.magic_attack;
    int hit2 = unitProficiencyIncr.hit;
    int evasion2 = unitProficiencyIncr.evasion;
    this.pA = num1 + num8 + physicalAttack;
    this.pD = num2 + physicalDefense;
    this.mA = num3 + this.MinMagicBulletPower(unit) + num9 + magicAttack;
    this.mD = num4 + magicDefense;
    this.hi = (num6 * 3 + num7) / 2 + hit1 + hit2;
    this.cr = num6 / 2 + critical;
    this.ev = (num5 * 3 + num7) / 2 + evasion1 + evasion2;
    this.mv = movement;
  }

  private int Proficency(GearAttackType kind, int rank) => 0;

  private int MinMagicBulletPower(UnitUnit unit)
  {
    BattleskillSkill[] array = ((IEnumerable<BattleskillSkill>) unit.RememberSkills()).Where<BattleskillSkill>((Func<BattleskillSkill, bool>) (x => x.skill_type == BattleskillSkillType.magic)).ToArray<BattleskillSkill>();
    return array.Length != 0 ? ((IEnumerable<BattleskillSkill>) array).Min<BattleskillSkill>((Func<BattleskillSkill, int>) (x => x.power)) : 0;
  }

  protected virtual IEnumerator UnitSpriteCreate(UnitUnit target)
  {
    IEnumerator e = target.LoadShopContentWithMask(this.TargetParent.transform, this.TargetParent.GetComponent<UIWidget>().depth, Res.GUI._007_4_2_sozai.mask_Chara.Load<Texture2D>());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  protected virtual IEnumerator UnitOtherSpriteCreate(UnitUnit target)
  {
    IEnumerator e = target.LoadShopContentUnitOtherWithMask(this.TargetParent.transform, this.TargetParent.GetComponent<UIWidget>().depth, Res.GUI._007_4_2_sozai.mask_Chara.Load<Texture2D>());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  protected virtual IEnumerator RarityCreate(UnitUnit target)
  {
    RarityIcon.SetRarity(target, this.rarityStarIcon, true);
    yield break;
  }
}
