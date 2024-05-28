// Decompiled with JetBrains decompiler
// Type: Unit004431Popup
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
public class Unit004431Popup : BackButtonMenuBase
{
  [SerializeField]
  private UILabel Direction1;
  [SerializeField]
  private UILabel Direction2;
  [SerializeField]
  private Transform TakeOffThum;
  private PlayerUnit[] pUnits;
  private PlayerUnit choiceUnit;
  private PlayerItem afterGear;
  private PlayerItem beforeGear;
  private PlayerUnit TakeOffUnit;
  private int index;
  private bool isSwap_;
  private GameObject StatusChangePopupPrefab;
  private GameObject beforeGearIcon;
  private GameObject afterGearIcon;
  private List<GameObject> beforeSkillTypeIcons;
  private List<GameObject> afterSkillTypeIcons;
  private const float UnitIconScale = 0.8f;
  private bool isEarthMode;

  public void Init(
    PlayerUnit[] pUnits,
    PlayerUnit choiceUnit,
    PlayerItem afterGear,
    int index,
    bool isEarthMode = false)
  {
    this.pUnits = pUnits;
    this.choiceUnit = choiceUnit;
    this.afterGear = afterGear;
    this.index = index;
    this.isEarthMode = isEarthMode;
    this.beforeGear = (PlayerItem) null;
    this.TakeOffUnit = (PlayerUnit) null;
    this.StartCoroutine(this.SelectIcon());
    this.StartCoroutine(this.createPopup());
  }

  private IEnumerator createPopup()
  {
    Future<GameObject> popupPrefabF = Res.Prefabs.popup.popup_004_4_1__anim_popup01.Load<GameObject>();
    IEnumerator e = popupPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.StatusChangePopupPrefab = popupPrefabF.Result;
    Future<GameObject> iconPrefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    e = iconPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject iconPrefab = iconPrefabF.Result;
    Future<GameObject> skillTypeIconLoader = Res.Prefabs.BattleSkillIcon._battleSkillIcon.Load<GameObject>();
    e = skillTypeIconLoader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject skillTypeIconPrefab = skillTypeIconLoader.Result;
    this.beforeGearIcon = Object.Instantiate<GameObject>(iconPrefab);
    ItemIcon beforeGearIconScript = this.beforeGearIcon.GetComponent<ItemIcon>();
    if (this.beforeGear != (PlayerItem) null)
    {
      e = beforeGearIconScript.InitByPlayerItem(this.beforeGear);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = beforeGearIconScript.InitByGear((GearGear) null);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      beforeGearIconScript.SetEmpty(true);
    }
    this.afterGearIcon = Object.Instantiate<GameObject>(iconPrefab);
    ItemIcon afterGearIconScript = this.afterGearIcon.GetComponent<ItemIcon>();
    if (this.afterGear != (PlayerItem) null)
    {
      e = afterGearIconScript.InitByPlayerItem(this.afterGear);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = afterGearIconScript.InitByGear((GearGear) null);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      afterGearIconScript.SetEmpty(true);
    }
    this.beforeSkillTypeIcons = new List<GameObject>();
    GearGearSkill[] gearGearSkillArray;
    int index;
    GameObject beforeSkillTypeIcon;
    if (this.beforeGear != (PlayerItem) null && this.beforeGear.gear != null)
    {
      gearGearSkillArray = this.beforeGear.skills;
      for (index = 0; index < gearGearSkillArray.Length; ++index)
      {
        GearGearSkill gearGearSkill = gearGearSkillArray[index];
        beforeSkillTypeIcon = Object.Instantiate<GameObject>(skillTypeIconPrefab);
        e = beforeSkillTypeIcon.GetComponent<BattleSkillIcon>().Init(gearGearSkill.skill);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.beforeSkillTypeIcons.Add(beforeSkillTypeIcon);
        beforeSkillTypeIcon = (GameObject) null;
      }
      gearGearSkillArray = (GearGearSkill[]) null;
    }
    this.afterSkillTypeIcons = new List<GameObject>();
    if (this.afterGear != (PlayerItem) null && this.afterGear.gear != null)
    {
      gearGearSkillArray = this.afterGear.skills;
      for (index = 0; index < gearGearSkillArray.Length; ++index)
      {
        GearGearSkill gearGearSkill = gearGearSkillArray[index];
        beforeSkillTypeIcon = Object.Instantiate<GameObject>(skillTypeIconPrefab);
        e = beforeSkillTypeIcon.GetComponent<BattleSkillIcon>().Init(gearGearSkill.skill);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.afterSkillTypeIcons.Add(beforeSkillTypeIcon);
        beforeSkillTypeIcon = (GameObject) null;
      }
      gearGearSkillArray = (GearGearSkill[]) null;
    }
  }

  private IEnumerator SelectIcon()
  {
    Unit004431Popup unit004431Popup = this;
    ((UIRect) ((Component) unit004431Popup).GetComponent<UIWidget>()).alpha = 0.0f;
    switch (unit004431Popup.index)
    {
      case 1:
        unit004431Popup.beforeGear = unit004431Popup.choiceUnit.equippedGear;
        break;
      case 2:
        unit004431Popup.beforeGear = unit004431Popup.choiceUnit.equippedGear2;
        break;
      case 3:
        unit004431Popup.beforeGear = unit004431Popup.choiceUnit.equippedGear3;
        break;
      default:
        Debug.LogError((object) string.Format("Unit004431Popup.index={0} はNG!! 1～3 の値を指定!!", (object) unit004431Popup.index));
        break;
    }
    // ISSUE: reference to a compiler-generated method
    unit004431Popup.TakeOffUnit = ((IEnumerable<PlayerUnit>) unit004431Popup.pUnits).FirstOrDefault<PlayerUnit>(new Func<PlayerUnit, bool>(unit004431Popup.\u003CSelectIcon\u003Eb__19_0));
    unit004431Popup.isSwap_ = false;
    if (unit004431Popup.TakeOffUnit != (PlayerUnit) null)
    {
      IEnumerator e = unit004431Popup.SetSprite();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ((UIRect) ((Component) unit004431Popup).GetComponent<UIWidget>()).alpha = 1f;
      if (unit004431Popup.choiceUnit != unit004431Popup.TakeOffUnit)
      {
        unit004431Popup.Direction1.SetText(Consts.GetInstance().UNIT_00431_POPUP_DIRECTIONS1);
        unit004431Popup.Direction2.SetText("[ffff00]" + unit004431Popup.TakeOffUnit.unit.name + "[-]\n" + Consts.GetInstance().UNIT_00431_POPUP_DIRECTIONS2);
      }
      else
      {
        unit004431Popup.Direction1.SetText(Consts.GetInstance().UNIT_00431_POPUP_SWAP1);
        unit004431Popup.Direction2.SetText(Consts.GetInstance().UNIT_00431_POPUP_SWAP2);
        unit004431Popup.isSwap_ = true;
      }
    }
    else
    {
      ((UIRect) ((Component) unit004431Popup).GetComponent<UIWidget>()).alpha = 0.0f;
      unit004431Popup.StartCoroutine(unit004431Popup.StatusPopup());
    }
  }

  private IEnumerator SetSprite()
  {
    Future<GameObject> prefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject prefab = prefabF.Result.Clone(this.TakeOffThum);
    UnitIcon uniticon = prefab.GetComponent<UnitIcon>();
    uniticon.setBottom(this.TakeOffUnit);
    uniticon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    e = uniticon.SetUnit(this.TakeOffUnit, this.TakeOffUnit.GetElement(), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    uniticon.BottomModeValue = UnitIconBase.GetBottomModeLevel(this.TakeOffUnit.unit, this.TakeOffUnit);
    uniticon.setLevelText(this.TakeOffUnit);
    prefab.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    if (Object.op_Inequality((Object) this.beforeGearIcon, (Object) null))
      Object.Destroy((Object) this.beforeGearIcon);
    if (Object.op_Inequality((Object) this.afterGearIcon, (Object) null))
      Object.Destroy((Object) this.afterGearIcon);
    if (this.beforeSkillTypeIcons != null)
    {
      foreach (Object beforeSkillTypeIcon in this.beforeSkillTypeIcons)
        Object.Destroy(beforeSkillTypeIcon);
    }
    if (this.afterSkillTypeIcons != null)
    {
      foreach (Object afterSkillTypeIcon in this.afterSkillTypeIcons)
        Object.Destroy(afterSkillTypeIcon);
    }
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();

  public void ibtnOK()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.StatusPopup());
  }

  private IEnumerator StatusPopup()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Unit004431Popup unit004431Popup = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    Singleton<PopupManager>.GetInstance().dismiss(true);
    GameObject gameObject = Singleton<PopupManager>.GetInstance().open(unit004431Popup.StatusChangePopupPrefab);
    PlayerUnit removeUnit = (PlayerUnit) null;
    if (unit004431Popup.TakeOffUnit != (PlayerUnit) null)
      removeUnit = unit004431Popup.TakeOffUnit;
    Unit00441Menu component = gameObject.GetComponent<Unit00441Menu>();
    unit004431Popup.StartCoroutine(component.SetGear(removeUnit, unit004431Popup.choiceUnit, unit004431Popup.beforeGear, unit004431Popup.afterGear, unit004431Popup.beforeGearIcon, unit004431Popup.afterGearIcon, unit004431Popup.beforeSkillTypeIcons.ToArray(), unit004431Popup.afterSkillTypeIcons.ToArray(), unit004431Popup.index, unit004431Popup.isEarthMode, isSwap: unit004431Popup.isSwap_));
    return false;
  }
}
