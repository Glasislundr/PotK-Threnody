// Decompiled with JetBrains decompiler
// Type: BattleHealCharacterInfoBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BattleHealCharacterInfoBase : NGBattleMenuBase
{
  [SerializeField]
  protected UILabel TxtCharaname;
  [SerializeField]
  protected UILabel TxtHpNumber;
  [SerializeField]
  protected Transform ImageParent;
  [SerializeField]
  protected int depth;
  [SerializeField]
  protected NGTweenGaugeScale hpBar;
  [SerializeField]
  protected NGTweenGaugeScale consumeBar;
  [SerializeField]
  private UISprite slcCountry;
  protected AttackStatus[] attacks;
  protected BL.UnitPosition currentUnit;
  protected AttackStatus currentAttack;

  public virtual IEnumerator Init(BL.UnitPosition up, AttackStatus[] attacks_)
  {
    BL.Unit unit = up.unit;
    UnitUnit masterUnit = unit.unit;
    this.currentUnit = up;
    this.attacks = attacks_;
    if (Object.op_Inequality((Object) this.slcCountry, (Object) null))
    {
      ((Component) this.slcCountry).gameObject.SetActive(false);
      if (masterUnit.country_attribute.HasValue)
      {
        ((Component) this.slcCountry).gameObject.SetActive(true);
        masterUnit.SetCuntrySpriteName(ref this.slcCountry);
      }
    }
    UILabel txtCharaname = this.TxtCharaname;
    UnitUnit unitUnit = masterUnit;
    SkillMetamorphosis metamorphosis = unit.metamorphosis;
    int metamorphosisId = metamorphosis != null ? metamorphosis.metamorphosis_id : 0;
    string name = unitUnit.getName(metamorphosisId);
    txtCharaname.SetTextLocalize(name);
    this.setHPNumbers(unit.hp.ToString());
    Future<GameObject> future = masterUnit.LoadStory();
    IEnumerator e = future.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.ImageParent.Clear();
    GameObject go = future.Result.Clone(this.ImageParent);
    masterUnit.SetStoryData(go);
    e = masterUnit.SetLargeSpriteWithMask(go, this.maskResource().Load<Texture2D>(), this.depth);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void setHPNumbers(string hp) => this.TxtHpNumber.SetTextLocalize(hp);

  public BL.UnitPosition getCurrent() => this.currentUnit;

  public AttackStatus getCurrentAttack() => this.currentAttack;

  public int getCurrentAttackIndex()
  {
    return ((IEnumerable<AttackStatus>) this.attacks).FirstIndexOrNull<AttackStatus>((Func<AttackStatus, bool>) (x => x == this.currentAttack)).Value;
  }

  protected virtual ResourceObject maskResource() => (ResourceObject) null;
}
