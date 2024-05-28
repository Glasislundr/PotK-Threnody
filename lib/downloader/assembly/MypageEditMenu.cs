// Decompiled with JetBrains decompiler
// Type: MypageEditMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class MypageEditMenu : UnitSelectMenuBase
{
  [SerializeField]
  protected GameObject[] linkCharactersBac;
  [SerializeField]
  protected GameObject[] linkCharacters;
  [SerializeField]
  protected UILabel txtCostValue;
  protected Promise<int?[]> playerUnitIds;
  protected int deck_type_id;
  protected int deck_number;
  protected int totalCost;
  protected int maxCost;
  protected string IconObjName = "Icon";

  protected void BaseSort(SortInfo info) => base.Sort(info);

  protected override void Sort(SortInfo info)
  {
    this.BaseSort(info);
    this.UpdateInfomation();
    this.UpdateSelectIcon();
  }

  protected virtual void updateTxtCostValue(int cost = 0)
  {
    this.totalCost = cost;
    this.txtCostValue.SetTextLocalize(this.totalCost.ToString() + "/" + (object) this.maxCost);
  }

  private UnitIcon CreateUnitObject(GameObject parent)
  {
    UnitIcon component = Object.Instantiate<GameObject>(this.unitPrefab, new Vector3(10000f, 0.0f, 0.0f), new Quaternion()).GetComponent<UnitIcon>();
    GameObject gameObject = ((Component) component).gameObject;
    ((Object) gameObject).name = this.IconObjName;
    gameObject.SetActive(true);
    parent.transform.Clear();
    gameObject.transform.parent = parent.transform;
    gameObject.transform.localPosition = Vector3.zero;
    UIWidget componentInChildren = parent.GetComponentInChildren<UIWidget>();
    gameObject.GetComponentInChildren<UnitIcon>().SetSize(componentInChildren.width, componentInChildren.height);
    return component;
  }

  protected IEnumerator CreateBottomInformationObject()
  {
    MypageEditMenu mypageEditMenu = this;
    GameObject[] gameObjectArray = mypageEditMenu.linkCharactersBac;
    for (int index = 0; index < gameObjectArray.Length; ++index)
    {
      GameObject parent = gameObjectArray[index];
      UnitIcon unitIcon = mypageEditMenu.CreateUnitObject(parent);
      IEnumerator e = unitIcon.SetPlayerUnit((PlayerUnit) null, mypageEditMenu.getUnits(), (PlayerUnit) null, false, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unitIcon.SetEmpty();
      unitIcon = (UnitIcon) null;
    }
    gameObjectArray = (GameObject[]) null;
    foreach (GameObject linkCharacter in mypageEditMenu.linkCharacters)
      ((Component) mypageEditMenu.CreateUnitObject(linkCharacter)).gameObject.SetActive(false);
  }

  private IEnumerator SetSelectUnit(UnitIconInfo info)
  {
    MypageEditMenu mypageEditMenu = this;
    GameObject gameObject = ((Component) mypageEditMenu.linkCharacters[info.select].transform.GetChildInFind(mypageEditMenu.IconObjName)).gameObject;
    UnitIcon icon = gameObject.GetComponent<UnitIcon>();
    mypageEditMenu.SetWidgetAlpha(false, mypageEditMenu.linkCharactersBac[info.select]);
    mypageEditMenu.SetWidgetAlpha(true, mypageEditMenu.linkCharacters[info.select]);
    gameObject.SetActive(true);
    if (icon.PlayerUnit == (PlayerUnit) null || icon.PlayerUnit.id != info.playerUnit.id)
    {
      IEnumerator e = icon.SetPlayerUnit(info.playerUnit, mypageEditMenu.getUnits(), (PlayerUnit) null, false, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      icon.setBottom(info.playerUnit);
      icon.ShowBottomInfo(mypageEditMenu.sortType);
      icon.ForBattle = false;
      icon.TowerEntry = false;
      icon.CanAwake = false;
      icon.UnitRental = false;
      icon.SetupDeckStatusBlink();
    }
    else if (icon.PlayerUnit != (PlayerUnit) null && icon.PlayerUnit.id == info.playerUnit.id)
      icon.ShowBottomInfo(mypageEditMenu.sortType);
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      if (info != null && info.unit != null)
        icon.SetSeaPiece(info.unit.GetPiece);
      else
        icon.SetSeaPiece(false);
    }
  }

  protected IEnumerator DisplaySelectUnit()
  {
    MypageEditMenu mypageEditMenu = this;
    int num = ((IEnumerable<GameObject>) mypageEditMenu.linkCharacters).Count<GameObject>();
    for (int index = 0; index < num; ++index)
    {
      mypageEditMenu.SetWidgetAlpha(true, mypageEditMenu.linkCharactersBac[index]);
      mypageEditMenu.SetWidgetAlpha(false, mypageEditMenu.linkCharacters[index]);
    }
    foreach (UnitIconInfo selectedUnitIcon in mypageEditMenu.selectedUnitIcons)
    {
      if (selectedUnitIcon.select >= 0)
      {
        IEnumerator e = mypageEditMenu.SetSelectUnit(selectedUnitIcon);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }

  private void SetWidgetAlpha(bool canDisp, GameObject obj)
  {
    UIWidget component = obj.GetComponent<UIWidget>();
    if (Object.op_Inequality((Object) component, (Object) null))
      ((UIRect) component).alpha = canDisp ? 1f : 0.0f;
    else
      obj.SetActive(canDisp);
  }

  public IEnumerator Init(
    PlayerDeck playerDeck,
    PlayerUnit[] playerUnits,
    Promise<int?[]> player_unit_ids,
    int max_cost,
    bool isEquip)
  {
    MypageEditMenu mypageEditMenu = this;
    IEnumerator e = mypageEditMenu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    mypageEditMenu.serverTime = ServerTime.NowAppTime();
    mypageEditMenu.deck_type_id = playerDeck.deck_type_id;
    mypageEditMenu.deck_number = playerDeck.deck_number;
    mypageEditMenu.maxCost = max_cost;
    mypageEditMenu.playerUnitIds = player_unit_ids;
    ((IEnumerable<GameObject>) mypageEditMenu.linkCharacters).ForEach<GameObject>((Action<GameObject>) (v => v.transform.Clear()));
    mypageEditMenu.updateTxtCostValue();
    playerUnits = ((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.IsNormalUnit)).ToArray<PlayerUnit>();
    mypageEditMenu.InitializeInfo((IEnumerable<PlayerUnit>) playerUnits, (IEnumerable<PlayerMaterialUnit>) null, Persist.mypageEditorSortAndFilter, isEquip, false, false, true, true, (Action) (() => { }));
    e = mypageEditMenu.CreateUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = mypageEditMenu.CreateBottomInformationObject();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = mypageEditMenu.DisplaySelectUnit();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    mypageEditMenu.UpdateInfomation();
    mypageEditMenu.InitializeEnd();
  }

  protected virtual int GetUsedCost()
  {
    int cost = 0;
    this.selectedUnitIcons.ForEach((Action<UnitIconInfo>) (x => cost += x.playerUnit.cost));
    return cost;
  }

  public override void UpdateInfomation()
  {
    this.StartCoroutine(this.DisplaySelectUnit());
    this.updateTxtCostValue(this.GetUsedCost());
  }

  protected virtual IEnumerator DeckEditAsync()
  {
    MypageEditMenu mypageEditMenu = this;
    int[] array = mypageEditMenu.selectedUnitIcons.OrderBy<UnitIconInfo, int>((Func<UnitIconInfo, int>) (x => x.select)).Select<UnitIconInfo, int>((Func<UnitIconInfo, int>) (x => x.playerUnit.id)).ToArray<int>();
    IEnumerator e = mypageEditMenu.DeckEditApi(mypageEditMenu.deck_type_id, mypageEditMenu.deck_number, array);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    mypageEditMenu.backScene();
  }

  protected IEnumerator DeckEditApi(int deckTypeID, int deckNumber, int[] playerUnitIds)
  {
    MypageEditMenu mypageEditMenu = this;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    IEnumerator e1;
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      // ISSUE: reference to a compiler-generated method
      e1 = WebAPI.SeaDeckEdit(deckNumber, playerUnitIds, new Action<WebAPI.Response.UserError>(mypageEditMenu.\u003CDeckEditApi\u003Eb__21_0)).Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
    }
    else
    {
      e1 = WebAPI.DeckEdit(deckTypeID, deckNumber, playerUnitIds, (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e))).Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
    }
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
  }

  public override void IbtnYes()
  {
    if (this.IsPush)
      return;
    this.StartCoroutine(this.DeckEditAsync());
  }

  public override void IbtnBack()
  {
    if (this.playerUnitIds != null && !this.playerUnitIds.HasResult)
      this.playerUnitIds.Result = (int?[]) null;
    base.IbtnBack();
  }

  protected virtual void IconGraySetting(UnitIconBase unitIcon, UnitIconInfo info)
  {
    if (this.selectedUnitIcons.Count<UnitIconInfo>() >= this.SelectMax)
      unitIcon.Gray = !info.gray;
    else if (this.totalCost >= this.maxCost)
      unitIcon.Gray = !info.gray;
    else if (info.select >= 0)
      unitIcon.Gray = info.gray;
    else if (info.playerUnit.cost <= this.maxCost - this.totalCost)
      unitIcon.Gray = info.gray;
    else
      unitIcon.Gray = !info.gray;
  }

  protected override void CreateUnitIconAction(int info_index, int unit_index)
  {
    UnitIconBase allUnitIcon = this.allUnitIcons[unit_index];
    UnitIconInfo displayUnitInfo = this.displayUnitInfos[info_index];
    if (displayUnitInfo.removeButton)
    {
      displayUnitInfo.gray = false;
      allUnitIcon.Gray = false;
    }
    else
    {
      displayUnitInfo.gray = false;
      if (displayUnitInfo.select >= 0)
      {
        displayUnitInfo.gray = true;
        if (this.iconSelMode == UnitSelectMenuBase.IconSelMode.Number)
          displayUnitInfo.icon.Select(displayUnitInfo.select);
        else
          displayUnitInfo.icon.SelectByCheckIcon();
      }
      allUnitIcon.onClick = (Action<UnitIconBase>) (ui => this.Select(ui));
      if (displayUnitInfo.button_enable)
      {
        ((Behaviour) allUnitIcon.Button).enabled = true;
      }
      else
      {
        ((Behaviour) allUnitIcon.Button).enabled = false;
        displayUnitInfo.gray = true;
      }
      if (this.selectedUnitIcons.Count >= this.SelectMax)
        allUnitIcon.Gray = !displayUnitInfo.gray;
      else
        this.IconGraySetting(allUnitIcon, displayUnitInfo);
      allUnitIcon.CanAwake = false;
      if (!Singleton<NGGameDataManager>.GetInstance().IsSea)
        return;
      ((UnitIcon) allUnitIcon).SetSeaPiece(displayUnitInfo.unit.GetPiece);
    }
  }

  public override void UpdateSelectIcon()
  {
    this.selectedUnitIcons.ForEachIndex<UnitIconInfo>((Action<UnitIconInfo, int>) ((u, n) =>
    {
      UnitIconInfo unitInfoDisplay = this.GetUnitInfoDisplay(u.playerUnit);
      if (unitInfoDisplay == null || !Object.op_Inequality((Object) unitInfoDisplay.icon, (Object) null))
        return;
      unitInfoDisplay.gray = true;
      if (this.iconSelMode == UnitSelectMenuBase.IconSelMode.Number)
        unitInfoDisplay.icon.Select(unitInfoDisplay.select);
      else
        unitInfoDisplay.icon.SelectByCheckIcon();
    }));
    foreach (UnitIconInfo displayUnitInfo in this.displayUnitInfos)
    {
      if (Object.op_Inequality((Object) displayUnitInfo.icon, (Object) null))
        this.IconGraySetting(displayUnitInfo.icon, displayUnitInfo);
    }
  }

  protected override void Select(UnitIconBase unitIcon)
  {
    MypageUnitUtil.setUnit(unitIcon.PlayerUnit.id);
    this.backScene();
  }

  protected override void CreateUnitIconCache(int info_index, int unit_index, PlayerUnit baseUnit = null)
  {
    base.CreateUnitIconCache(info_index, unit_index);
    this.CreateUnitIconAction(info_index, unit_index);
    UnitIconBase allUnitIcon = this.allUnitIcons[unit_index];
    if (allUnitIcon.PlayerUnit != (PlayerUnit) null && allUnitIcon.PlayerUnit.id == MypageUnitUtil.getUnitId())
    {
      ((Component) allUnitIcon.blinkDeckStatus).gameObject.SetActive(true);
      allUnitIcon.UnitSelected = true;
    }
    this.setLongPressEvent(allUnitIcon, info_index);
  }

  protected void IBtnBack() => this.backScene();

  protected override IEnumerator CreateUnitIcon(
    int info_index,
    int unit_index,
    PlayerUnit baseUnit = null)
  {
    MypageEditMenu mypageEditMenu = this;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = mypageEditMenu.\u003C\u003En__0(info_index, unit_index, baseUnit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UnitIconBase allUnitIcon = mypageEditMenu.allUnitIcons[unit_index];
    if (allUnitIcon.PlayerUnit != (PlayerUnit) null && allUnitIcon.PlayerUnit.id == MypageUnitUtil.getUnitId())
    {
      ((Component) allUnitIcon.blinkDeckStatus).gameObject.SetActive(true);
      allUnitIcon.UnitSelected = true;
    }
    mypageEditMenu.setLongPressEvent(allUnitIcon, info_index);
  }

  private void setLongPressEvent(UnitIconBase unitIcon, int info_index)
  {
    unitIcon.setLongPressEvent((Action) (() => Unit0042Scene.changeSceneMypageEdit(this.displayUnitInfos[info_index].playerUnit, this.getUnits())));
  }
}
