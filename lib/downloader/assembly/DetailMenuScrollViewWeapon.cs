// Decompiled with JetBrains decompiler
// Type: DetailMenuScrollViewWeapon
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
using UnityEngine;

#nullable disable
public class DetailMenuScrollViewWeapon : DetailMenuScrollViewBase
{
  [SerializeField]
  protected GameObject dir_PageNormal;
  [SerializeField]
  protected GameObject dir_PageHaveGear2;
  [SerializeField]
  protected UILabel txt_WeaponName;
  [SerializeField]
  protected UILabel txt_WeaponRange;
  [SerializeField]
  protected UI2DSprite dyn_WeaponType;
  [SerializeField]
  protected UILabel[] txt_MagicNames;
  [SerializeField]
  protected UILabel[] txt_MagicRanges;
  [SerializeField]
  protected UILabel[] txt_MagicCosts;
  [SerializeField]
  protected UI2DSprite[] dyn_MagicElementTypes;
  [SerializeField]
  protected UILabel[] txt_WeaponNameHaveGear2;
  [SerializeField]
  protected UILabel[] txt_WeaponRangeHaveGear2;
  [SerializeField]
  protected UI2DSprite[] dyn_WeaponTypeHaveGear2;
  [SerializeField]
  protected UILabel[] txt_MagicNamesHaveGear2;
  [SerializeField]
  protected UILabel[] txt_MagicRangesHaveGear2;
  [SerializeField]
  protected UILabel[] txt_MagicCostsHaveGear2;
  [SerializeField]
  protected UI2DSprite[] dyn_MagicElementTypesHaveGear2;
  private UIButton[] magicButtons;
  [SerializeField]
  private GameObject floatingSkillDialog;
  private Action<BattleskillSkill> showSkillDialog;
  private Action<int, int> showSkillLevel;
  private Battle0171111Event floatingSkillDialogObject;
  private GameObject gearKindIconObject;
  private GameObject gearKindIconObject2;
  private GameObject[] magicElemmentObject;
  private int hp;
  private PlayerUnit playerUnit;

  private void Awake()
  {
    if (this.magicElemmentObject != null)
      return;
    this.magicElemmentObject = new GameObject[this.txt_MagicNames.Length];
  }

  public override IEnumerator initAsync(
    PlayerUnit playerUnit,
    bool limitMode,
    bool isMaterial,
    GameObject[] prefabs)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    DetailMenuScrollViewWeapon scrollViewWeapon = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    scrollViewWeapon.playerUnit = playerUnit;
    Judgement.NonBattleParameter nonBattleParameter = scrollViewWeapon.isMemory ? Judgement.NonBattleParameter.FromPlayerUnitMemory(playerUnit) : Judgement.NonBattleParameter.FromPlayerUnit(playerUnit, scrollViewWeapon.controlFlags.IsOn(Control.SelfAbility));
    scrollViewWeapon.hp = nonBattleParameter.Hp;
    GearGear gear1;
    CommonElement element1;
    if (playerUnit.equippedGear != (PlayerItem) null)
    {
      gear1 = playerUnit.equippedGear.gear;
      element1 = playerUnit.equippedGear.GetElement();
    }
    else
    {
      gear1 = playerUnit.initial_gear;
      element1 = playerUnit.initial_gear.GetElement();
    }
    GearGear gear2 = (GearGear) null;
    CommonElement element2 = CommonElement.none;
    if (playerUnit.equippedGear2 != (PlayerItem) null)
    {
      gear2 = playerUnit.equippedGear2.gear;
      element2 = playerUnit.equippedGear2.GetElement();
    }
    if (scrollViewWeapon.showSkillDialog == null)
    {
      scrollViewWeapon.floatingSkillDialog.transform.Clear();
      if (Object.op_Equality((Object) scrollViewWeapon.floatingSkillDialogObject, (Object) null))
        scrollViewWeapon.floatingSkillDialogObject = prefabs[0].Clone(scrollViewWeapon.floatingSkillDialog.transform).GetComponentInChildren<Battle0171111Event>();
      ((Component) ((Component) scrollViewWeapon.floatingSkillDialogObject).transform.parent).gameObject.SetActive(false);
      // ISSUE: reference to a compiler-generated method
      scrollViewWeapon.showSkillDialog = new Action<BattleskillSkill>(scrollViewWeapon.\u003CinitAsync\u003Eb__27_0);
      // ISSUE: reference to a compiler-generated method
      scrollViewWeapon.showSkillLevel = new Action<int, int>(scrollViewWeapon.\u003CinitAsync\u003Eb__27_1);
    }
    if (gear2 == null)
    {
      scrollViewWeapon.dir_PageNormal.SetActive(true);
      scrollViewWeapon.dir_PageHaveGear2.SetActive(false);
      scrollViewWeapon.setWeapon(prefabs[1], scrollViewWeapon.gearKindIconObject, playerUnit.equippedGearName, scrollViewWeapon.txt_WeaponName, scrollViewWeapon.txt_WeaponRange, scrollViewWeapon.dyn_WeaponType, gear1, element1);
      scrollViewWeapon.setMagics(prefabs[2], scrollViewWeapon.txt_MagicNames, scrollViewWeapon.txt_MagicRanges, scrollViewWeapon.txt_MagicCosts, scrollViewWeapon.dyn_MagicElementTypes);
    }
    else
    {
      scrollViewWeapon.dir_PageNormal.SetActive(false);
      scrollViewWeapon.dir_PageHaveGear2.SetActive(true);
      scrollViewWeapon.setWeapon(prefabs[1], scrollViewWeapon.gearKindIconObject, playerUnit.equippedGearName, scrollViewWeapon.txt_WeaponNameHaveGear2[0], scrollViewWeapon.txt_WeaponRangeHaveGear2[0], scrollViewWeapon.dyn_WeaponTypeHaveGear2[0], gear1, element1);
      scrollViewWeapon.setWeapon(prefabs[1], scrollViewWeapon.gearKindIconObject2, playerUnit.equippedGearName2, scrollViewWeapon.txt_WeaponNameHaveGear2[1], scrollViewWeapon.txt_WeaponRangeHaveGear2[1], scrollViewWeapon.dyn_WeaponTypeHaveGear2[1], gear2, element2);
      scrollViewWeapon.setMagics(prefabs[2], scrollViewWeapon.txt_MagicNamesHaveGear2, scrollViewWeapon.txt_MagicRangesHaveGear2, scrollViewWeapon.txt_MagicCostsHaveGear2, scrollViewWeapon.dyn_MagicElementTypesHaveGear2);
    }
    return false;
  }

  private void setWeapon(
    GameObject _weaponIconPrefab,
    GameObject _gearKindIconObject,
    string _strName,
    UILabel _txtWeaponName,
    UILabel _txtWeaponRange,
    UI2DSprite _dynWeaponType,
    GearGear gear,
    CommonElement element)
  {
    if (Object.op_Equality((Object) _gearKindIconObject, (Object) null))
    {
      _gearKindIconObject = _weaponIconPrefab.Clone(((Component) _dynWeaponType).transform);
      ((UIWidget) _gearKindIconObject.GetComponent<UI2DSprite>()).depth = ((UIWidget) _dynWeaponType).depth + 1;
    }
    _gearKindIconObject.GetComponent<GearKindIcon>().Init(gear.kind, element);
    UIButton component = ((Component) ((Component) _dynWeaponType).transform.parent).gameObject.GetComponent<UIButton>();
    ((UIButtonColor) component).tweenTarget = (GameObject) null;
    component.onClick.Clear();
    ((Component) component).GetComponent<Collider>().enabled = false;
    ((Behaviour) component).enabled = false;
    _txtWeaponName.SetTextLocalize(_strName);
    _txtWeaponRange.SetTextLocalize(gear.min_range.ToString() + "-" + (object) gear.max_range);
  }

  private void setMagics(
    GameObject _magicElementPrefab,
    UILabel[] _txtMagicNames,
    UILabel[] _txtMagicRanges,
    UILabel[] _txtMagicCosts,
    UI2DSprite[] _dynMagicElementTypes)
  {
    PlayerUnitSkills[] skills = this.playerUnit.skills;
    if (this.magicButtons != null)
      ((IEnumerable<UIButton>) this.magicButtons).ForEach<UIButton>((Action<UIButton>) (x =>
      {
        if (!Object.op_Inequality((Object) x, (Object) null))
          return;
        x.onClick.Clear();
      }));
    this.magicButtons = new UIButton[_txtMagicNames.Length];
    int index = 0;
    foreach (GameObject gameObject in this.magicElemmentObject)
    {
      if (Object.op_Inequality((Object) gameObject, (Object) null))
        gameObject.SetActive(false);
    }
    foreach (PlayerUnitSkills playerUnitSkills in skills)
    {
      BattleskillSkill masterSkill = playerUnitSkills.skill;
      if (masterSkill.skill_type == BattleskillSkillType.magic)
      {
        if (index < _txtMagicNames.Length)
        {
          int consumeHp = masterSkill.consume_hp;
          foreach (BattleskillEffect battleskillEffect in ((IEnumerable<BattleskillEffect>) masterSkill.Effects).Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.percentage_hp_consume_magic)))
            consumeHp += Mathf.CeilToInt((float) ((Decimal) this.hp * (Decimal) battleskillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.cost_percentage)));
          _txtMagicNames[index].SetTextLocalize(masterSkill.name);
          _txtMagicRanges[index].SetTextLocalize(masterSkill.min_range.ToString() + "-" + (object) masterSkill.max_range);
          ((Component) _txtMagicRanges[index]).gameObject.SetActive(true);
          _txtMagicCosts[index].SetTextLocalize(consumeHp);
          ((Component) _txtMagicCosts[index]).gameObject.SetActive(true);
          ((Component) _dynMagicElementTypes[index]).gameObject.SetActive(true);
          if (Object.op_Equality((Object) this.magicElemmentObject[index], (Object) null))
            this.magicElemmentObject[index] = this.createIcon(_magicElementPrefab, ((Component) _dynMagicElementTypes[index]).transform);
          this.magicElemmentObject[index].SetActive(true);
          this.magicElemmentObject[index].GetComponent<CommonElementIcon>().Init(masterSkill.element);
          this.magicButtons[index] = ((Component) ((Component) _txtMagicNames[index]).transform.parent).gameObject.GetComponent<UIButton>();
          ((Component) this.magicButtons[index]).GetComponent<Collider>().enabled = true;
          ((Behaviour) this.magicButtons[index]).enabled = true;
          EventDelegate.Set(this.magicButtons[index].onClick, (EventDelegate.Callback) (() =>
          {
            this.showSkillDialog(masterSkill);
            this.showSkillLevel(0, 0);
          }));
        }
        ++index;
      }
    }
    for (; index < _txtMagicNames.Length; ++index)
    {
      _txtMagicNames[index].SetTextLocalize("-");
      ((Component) _txtMagicRanges[index]).gameObject.SetActive(false);
      ((Component) _txtMagicCosts[index]).gameObject.SetActive(false);
      ((Component) _dynMagicElementTypes[index]).gameObject.SetActive(false);
    }
  }

  public override bool Init(PlayerUnit playerUnit, PlayerUnit baseUnit)
  {
    ((Component) this).gameObject.SetActive(true);
    return true;
  }

  private GameObject createIcon(GameObject prefab, Transform trans)
  {
    trans.Clear();
    GameObject icon = prefab.Clone(trans);
    UI2DSprite componentInChildren1 = icon.GetComponentInChildren<UI2DSprite>();
    BoxCollider componentInChildren2 = ((Component) trans).GetComponentInChildren<BoxCollider>();
    UI2DSprite componentInChildren3 = ((Component) trans).GetComponentInChildren<UI2DSprite>();
    ((UIWidget) componentInChildren1).SetDimensions((int) componentInChildren2.size.x, (int) componentInChildren2.size.y);
    ((UIWidget) componentInChildren1).depth = ((UIWidget) componentInChildren3).depth + 1;
    return icon;
  }

  public override IEnumerator initAsyncDiffMode(
    PlayerUnit playerUnit,
    PlayerUnit prevUnit,
    IDetailMenuContainer menuContainer)
  {
    this.playerUnit = playerUnit;
    if (this.magicElemmentObject == null)
      this.magicElemmentObject = new GameObject[this.txt_MagicNames.Length];
    this.hp = Judgement.NonBattleParameter.FromPlayerUnit(playerUnit, true).Hp;
    GearGear gear1;
    CommonElement element1;
    if (playerUnit.equippedGear != (PlayerItem) null)
    {
      gear1 = playerUnit.equippedGear.gear;
      element1 = playerUnit.equippedGear.GetElement();
    }
    else
    {
      gear1 = playerUnit.initial_gear;
      element1 = playerUnit.initial_gear.GetElement();
    }
    GearGear gear2 = (GearGear) null;
    CommonElement element2 = CommonElement.none;
    if (playerUnit.equippedGear2 != (PlayerItem) null)
    {
      gear2 = playerUnit.equippedGear2.gear;
      element2 = playerUnit.equippedGear2.GetElement();
    }
    if (gear2 == null)
    {
      this.dir_PageNormal.SetActive(true);
      this.dir_PageHaveGear2.SetActive(false);
      this.setWeapon(menuContainer.gearKindIconPrefab, this.gearKindIconObject, playerUnit.equippedGearName, this.txt_WeaponName, this.txt_WeaponRange, this.dyn_WeaponType, gear1, element1);
      this.setMagics(menuContainer.commonElementIconPrefab, this.txt_MagicNames, this.txt_MagicRanges, this.txt_MagicCosts, this.dyn_MagicElementTypes);
    }
    else
    {
      this.dir_PageNormal.SetActive(false);
      this.dir_PageHaveGear2.SetActive(true);
      this.setWeapon(menuContainer.gearKindIconPrefab, this.gearKindIconObject, playerUnit.equippedGearName, this.txt_WeaponNameHaveGear2[0], this.txt_WeaponRangeHaveGear2[0], this.dyn_WeaponTypeHaveGear2[0], gear1, element1);
      this.setWeapon(menuContainer.gearKindIconPrefab, this.gearKindIconObject2, playerUnit.equippedGearName2, this.txt_WeaponNameHaveGear2[1], this.txt_WeaponRangeHaveGear2[1], this.dyn_WeaponTypeHaveGear2[1], gear2, element2);
      this.setMagics(menuContainer.commonElementIconPrefab, this.txt_MagicNamesHaveGear2, this.txt_MagicRangesHaveGear2, this.txt_MagicCostsHaveGear2, this.dyn_MagicElementTypesHaveGear2);
      yield break;
    }
  }
}
