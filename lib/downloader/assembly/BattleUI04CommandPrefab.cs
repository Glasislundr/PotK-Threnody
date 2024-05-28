// Decompiled with JetBrains decompiler
// Type: BattleUI04CommandPrefab
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
public class BattleUI04CommandPrefab : MonoBehaviour
{
  [SerializeField]
  [Tooltip("名前")]
  private UILabel label;
  [SerializeField]
  [Tooltip("消費HP")]
  private UILabel decHP;
  [SerializeField]
  [Tooltip("魔弾(消費HP表記有り)用下地")]
  private GameObject baseMagicBullet;
  [SerializeField]
  [Tooltip("消費HP無し用下地")]
  private GameObject baseGear;
  [SerializeField]
  [Tooltip("攻撃区分表示")]
  private BattleUI04CommandPrefab.IconControl cntlClassification_;
  [SerializeField]
  [Tooltip("属性表示")]
  private BattleUI04CommandPrefab.IconControl cntlElement_;
  private GameObject elementIcon;

  public IEnumerator Init(AttackStatus attack, BL.Unit unit)
  {
    if (attack.magicBullet != null)
      yield return (object) this.initializeIcons(attack, attack.magicBullet);
    else if (attack.weapon != null)
    {
      CommonElement? nullable = attack.GetOverwriteElement();
      if (!nullable.HasValue)
        nullable = new CommonElement?(attack.weapon.attackMethod.element);
      yield return (object) this.initializeIcons(attack, attack.weapon.attackMethod, nullable.Value);
    }
    else
    {
      GearGear weaponGearOrInitial = unit.playerUnit.equippedWeaponGearOrInitial;
      CommonElement? nullable = attack.GetOverwriteElement();
      if (!nullable.HasValue)
        nullable = new CommonElement?(weaponGearOrInitial.attachedElement);
      yield return (object) this.initializeIcons(attack, weaponGearOrInitial, unit.playerUnit.initial_gear, nullable.Value);
    }
  }

  public IEnumerator InitOppoSide(AttackStatus attack, BL.Unit unit)
  {
    if (attack.magicBullet != null)
      yield return (object) this.initializeIcons(attack, attack.magicBullet);
    else if (attack.weapon != null)
    {
      CommonElement? nullable = attack.GetOverwriteElement();
      if (!nullable.HasValue)
        nullable = new CommonElement?(attack.weapon.attackMethod.element);
      yield return (object) this.initializeIcons(attack, attack.weapon.attackMethod, nullable.Value);
    }
    else
    {
      GearGear weaponGearOrInitial = unit.playerUnit.equippedWeaponGearOrInitial;
      CommonElement? nullable = attack.GetOverwriteElement();
      if (!nullable.HasValue)
        nullable = new CommonElement?(weaponGearOrInitial.attachedElement);
      yield return (object) this.initializeIcons(attack, weaponGearOrInitial, unit.playerUnit.initial_gear, nullable.Value);
    }
  }

  private IEnumerator initializeIcons(
    AttackStatus attack,
    GearGear gear,
    GearGear initGear,
    CommonElement element)
  {
    this.baseMagicBullet.SetActive(false);
    this.baseGear.gameObject.SetActive(true);
    ((Component) this.decHP).gameObject.SetActive(false);
    this.label.SetTextLocalize(gear.name);
    this.initializeAttackClass(gear.hasAttackClass ? gear.gearClassification.attack_classification : initGear.gearClassification.attack_classification, this.toArrow(attack.attackClassificationRate));
    yield return (object) this.initializeElement(element, this.toArrow(attack.attackElementDamageRate));
  }

  private IEnumerator initializeIcons(AttackStatus attack, BL.MagicBullet magicBullet)
  {
    this.baseMagicBullet.SetActive(true);
    this.baseGear.SetActive(false);
    this.label.SetTextLocalize(magicBullet.name);
    ((Component) this.decHP).gameObject.SetActive(true);
    this.decHP.SetTextLocalize(magicBullet.cost);
    this.initializeAttackClass(GearAttackClassification.magic, this.toArrow(attack.attackClassificationRate));
    yield return (object) this.initializeElement(magicBullet.element, this.toArrow(attack.attackElementDamageRate));
  }

  private IEnumerator initializeIcons(
    AttackStatus attack,
    IAttackMethod attackMethod,
    CommonElement element)
  {
    this.baseMagicBullet.SetActive(false);
    this.baseGear.gameObject.SetActive(true);
    ((Component) this.decHP).gameObject.SetActive(false);
    this.label.SetTextLocalize(attackMethod.skill.name);
    this.initializeAttackClass(attackMethod.attackClass, this.toArrow(attack.attackClassificationRate));
    yield return (object) this.initializeElement(element, this.toArrow(attack.attackElementDamageRate));
  }

  private BattleUI04CommandPrefab.Arrow toArrow(float rate)
  {
    if ((double) rate == 1.0)
      return BattleUI04CommandPrefab.Arrow.None;
    return (double) rate <= 1.0 ? BattleUI04CommandPrefab.Arrow.Down : BattleUI04CommandPrefab.Arrow.Up;
  }

  private void initializeAttackClass(
    GearAttackClassification attackClass,
    BattleUI04CommandPrefab.Arrow dir)
  {
    if (this.cntlClassification_ == null)
      return;
    this.cntlClassification_.enabled = true;
    this.cntlClassification_.initializeAttackClassIcon(attackClass);
    this.cntlClassification_.arrowDirection = dir;
  }

  private IEnumerator initializeElement(
    CommonElement element,
    BattleUI04CommandPrefab.Arrow arrowDir)
  {
    if (this.cntlElement_ != null)
    {
      if (Object.op_Equality((Object) this.elementIcon, (Object) null))
      {
        Future<GameObject> loader = Res.Icons.CommonElementIcon.Load<GameObject>();
        IEnumerator e = loader.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.elementIcon = loader.Result;
        loader = (Future<GameObject>) null;
      }
      this.cntlElement_.enabled = true;
      this.cntlElement_.iconSprite = this.elementIcon.GetComponent<CommonElementIcon>().getIcon(element);
      this.cntlElement_.arrowDirection = arrowDir;
    }
  }

  private void initializeBlank()
  {
    if (this.cntlClassification_ != null)
      this.cntlClassification_.enabled = false;
    if (this.cntlElement_ != null)
      this.cntlElement_.enabled = false;
    this.baseMagicBullet.SetActive(false);
    this.baseGear.SetActive(false);
  }

  private enum Arrow
  {
    None = -1, // 0xFFFFFFFF
    Up = 0,
    Down = 1,
  }

  [Serializable]
  private class IconControl
  {
    public UI2DSprite icon;
    [SerializeField]
    [Tooltip("{↑,↓}順でセット")]
    private GameObject[] arrows;

    public void initializeAttackClassIcon(GearAttackClassification attackClass)
    {
      AttackClassIcon component = Object.op_Inequality((Object) this.icon, (Object) null) ? ((Component) this.icon).GetComponent<AttackClassIcon>() : (AttackClassIcon) null;
      if (!Object.op_Inequality((Object) component, (Object) null))
        return;
      component.Initialize(attackClass);
    }

    public Sprite iconSprite
    {
      set
      {
        if (!Object.op_Inequality((Object) this.icon, (Object) null))
          return;
        this.icon.sprite2D = value;
      }
    }

    public BattleUI04CommandPrefab.Arrow arrowDirection
    {
      set
      {
        GameObject[] arrows = this.arrows;
        if (arrows == null)
          return;
        ((IEnumerable<GameObject>) arrows).ToggleOnce((int) value);
      }
    }

    public bool enabled
    {
      set
      {
        if (!Object.op_Inequality((Object) this.icon, (Object) null))
          return;
        ((Component) this.icon).gameObject.SetActive(value);
      }
    }
  }
}
