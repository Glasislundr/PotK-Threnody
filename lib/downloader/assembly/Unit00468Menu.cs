// Decompiled with JetBrains decompiler
// Type: Unit00468Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using CustomDeck;
using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Unit00468Menu : UnitSelectMenuBase
{
  [SerializeField]
  protected UIButton ibtnYes;
  [SerializeField]
  protected GameObject[] linkCharactersBac;
  [SerializeField]
  protected GameObject[] linkCharacters;
  [SerializeField]
  protected UILabel txtCostValue;
  protected int deck_type_id;
  protected int deck_number;
  protected int totalCost;
  protected int maxCost;
  protected HashSet<int> excludeOverkillersIds;
  protected string IconObjName = "Icon";
  private EditUnitParam customEdit_;

  protected bool isNonCheckOverkillers { get; set; }

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
    if (this.isEditCustomDeck)
    {
      ((UIButtonColor) this.ibtnYes).isEnabled = true;
    }
    else
    {
      ((UIButtonColor) this.ibtnYes).isEnabled = this.totalCost > 0;
      if (((UIButtonColor) this.ibtnYes).isEnabled && this.excludeOverkillersIds != null && this.excludeOverkillersIds.Count > 0)
      {
        foreach (UnitIconInfo selectedUnitIcon in this.selectedUnitIcons)
        {
          if (this.excludeOverkillersIds.Contains(selectedUnitIcon.playerUnit.id))
          {
            ((UIButtonColor) this.ibtnYes).isEnabled = false;
            break;
          }
        }
      }
    }
    ((UIWidget) ((Component) ((Component) this.ibtnYes).transform.GetChild(0)).GetComponent<UISprite>()).color = ((UIButtonColor) this.ibtnYes).isEnabled ? Color.white : Color.gray;
  }

  public virtual void InitializeAllUnitInfosExtend(EditUnitParam param)
  {
    UnitIconInfo[] array = this.selectedUnitIcons.ToArray();
    bool updateIndex = array.Length == 0 && !this.isInitialize;
    this.selectedUnitIcons.Clear();
    List<int> list = ((IEnumerable<int>) param.deck.player_unit_ids).ToList<int>();
    foreach (UnitIconInfo allUnitInfo in this.allUnitInfos)
    {
      UnitIconInfo info = allUnitInfo;
      info.select = -1;
      info.for_battle = false;
      info.gray = false;
      if (param.usedUnitIds != null)
        info.is_used = ((IEnumerable<int>) param.usedUnitIds).Contains<int>(info.playerUnit.id);
      if (updateIndex)
      {
        int num = list.IndexOf(info.playerUnit.id);
        if (num != -1)
        {
          info.gray = true;
          info.select = num;
          info.for_battle = true;
          this.selectedUnitIcons.Add(info);
        }
      }
      else
      {
        info.for_battle = list.Contains(info.playerUnit.id);
        UnitIconInfo unitIconInfo = ((IEnumerable<UnitIconInfo>) array).FirstOrDefault<UnitIconInfo>((Func<UnitIconInfo, bool>) (x => x.playerUnit.id == info.playerUnit.id));
        if (unitIconInfo != null)
        {
          info.gray = true;
          info.select = unitIconInfo.select;
          this.selectedUnitIcons.Add(info);
        }
      }
    }
    this.ReflectionSelectUnit();
    this.CreateSelectUnitList(updateIndex);
    this.updateTxtCostValue(this.GetUsedCost());
  }

  public virtual void InitializeAllUnitInfosExtend(PlayerDeck playerDeck)
  {
    bool updateIndex = this.selectedUnitIcons.Count<UnitIconInfo>() == 0 && !this.isInitialize;
    UnitIconInfo[] array = this.selectedUnitIcons.ToArray();
    this.selectedUnitIcons.Clear();
    List<int> intList1 = new List<int>();
    for (int index = 0; index < playerDeck.player_units.Length; ++index)
    {
      if (playerDeck.player_units[index] != (PlayerUnit) null)
        intList1.Add(playerDeck.player_units[index].id);
    }
    List<int> intList2 = new List<int>();
    for (int index = 0; index < array.Length; ++index)
      intList2.Add(array[index].playerUnit.id);
    foreach (UnitIconInfo allUnitInfo in this.allUnitInfos)
    {
      UnitIconInfo info = allUnitInfo;
      info.select = -1;
      info.for_battle = false;
      info.gray = false;
      if (updateIndex)
      {
        int num = intList1.IndexOf(info.playerUnit.id);
        if (num != -1)
        {
          info.gray = true;
          info.select = num;
          info.for_battle = true;
          this.selectedUnitIcons.Add(info);
        }
      }
      else
      {
        info.for_battle = intList1.Contains(info.playerUnit.id);
        UnitIconInfo unitIconInfo = ((IEnumerable<UnitIconInfo>) array).FirstOrDefault<UnitIconInfo>((Func<UnitIconInfo, bool>) (x => x.playerUnit.id == info.playerUnit.id));
        if (unitIconInfo != null)
        {
          info.gray = true;
          info.select = unitIconInfo.select;
          this.selectedUnitIcons.Add(info);
        }
      }
    }
    this.updateExcludeOverkillers();
    this.ReflectionSelectUnit();
    this.CreateSelectUnitList(updateIndex);
    this.updateTxtCostValue(this.GetUsedCost());
  }

  protected void CreateSelectUnitList(bool updateIndex = true)
  {
    this.selectedUnitIcons.Clear();
    this.selectedUnitIcons = this.allUnitInfos.Where<UnitIconInfo>((Func<UnitIconInfo, bool>) (x => x.select >= 0)).OrderBy<UnitIconInfo, int>((Func<UnitIconInfo, int>) (x => x.select)).ToList<UnitIconInfo>();
    this.selectedUnitIcons.ForEachIndex<UnitIconInfo>((Action<UnitIconInfo, int>) ((u, n) =>
    {
      if (updateIndex)
        u.select = n;
      this.UnitInfoUpdate(u, true, u.select);
    }));
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
    Unit00468Menu unit00468Menu = this;
    GameObject[] gameObjectArray = unit00468Menu.linkCharactersBac;
    for (int index = 0; index < gameObjectArray.Length; ++index)
    {
      GameObject parent = gameObjectArray[index];
      UnitIcon unitIcon = unit00468Menu.CreateUnitObject(parent);
      IEnumerator e = unitIcon.SetPlayerUnit((PlayerUnit) null, unit00468Menu.getUnits(), (PlayerUnit) null, false, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unitIcon.SetEmpty();
      unitIcon = (UnitIcon) null;
    }
    gameObjectArray = (GameObject[]) null;
    foreach (GameObject linkCharacter in unit00468Menu.linkCharacters)
      ((Component) unit00468Menu.CreateUnitObject(linkCharacter)).gameObject.SetActive(false);
  }

  private IEnumerator SetSelectUnit(UnitIconInfo info)
  {
    Unit00468Menu unit00468Menu = this;
    GameObject gameObject = ((Component) unit00468Menu.linkCharacters[info.select].transform.GetChildInFind(unit00468Menu.IconObjName)).gameObject;
    UnitIcon icon = gameObject.GetComponent<UnitIcon>();
    unit00468Menu.SetWidgetAlpha(false, unit00468Menu.linkCharactersBac[info.select]);
    unit00468Menu.SetWidgetAlpha(true, unit00468Menu.linkCharacters[info.select]);
    gameObject.SetActive(true);
    if (icon.PlayerUnit == (PlayerUnit) null || icon.PlayerUnit.id != info.playerUnit.id)
    {
      IEnumerator e = icon.SetPlayerUnit(info.playerUnit, unit00468Menu.getUnits(), (PlayerUnit) null, false, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      icon.setBottom(info.playerUnit);
      icon.ShowBottomInfo(unit00468Menu.sortType);
      icon.ForBattle = false;
      icon.TowerEntry = false;
      icon.CanAwake = false;
      icon.UnitRental = false;
      icon.SetupDeckStatusBlink();
    }
    else if (icon.PlayerUnit != (PlayerUnit) null && icon.PlayerUnit.id == info.playerUnit.id)
      icon.ShowBottomInfo(unit00468Menu.sortType);
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
    Unit00468Menu unit00468Menu = this;
    int num = ((IEnumerable<GameObject>) unit00468Menu.linkCharacters).Count<GameObject>();
    for (int index = 0; index < num; ++index)
    {
      unit00468Menu.SetWidgetAlpha(true, unit00468Menu.linkCharactersBac[index]);
      unit00468Menu.SetWidgetAlpha(false, unit00468Menu.linkCharacters[index]);
    }
    foreach (UnitIconInfo selectedUnitIcon in unit00468Menu.selectedUnitIcons)
    {
      if (selectedUnitIcon.select >= 0)
      {
        IEnumerator e = unit00468Menu.SetSelectUnit(selectedUnitIcon);
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

  public IEnumerator Init(EditUnitParam param, int max_cost)
  {
    Unit00468Menu unit00468Menu = this;
    unit00468Menu.customEdit_ = param;
    unit00468Menu.isEditCustomDeck = true;
    unit00468Menu.customDeck = param.deck;
    IEnumerator e = unit00468Menu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00468Menu.serverTime = ServerTime.NowAppTime();
    unit00468Menu.deck_type_id = param.deck.deck_type_id;
    unit00468Menu.deck_number = param.deck.deck_number;
    unit00468Menu.maxCost = max_cost;
    unit00468Menu.excludeOverkillersIds = (HashSet<int>) null;
    unit00468Menu.updateExcludeOverkillers(param.units);
    ((IEnumerable<GameObject>) unit00468Menu.linkCharacters).ForEach<GameObject>((Action<GameObject>) (v => v.transform.Clear()));
    unit00468Menu.updateTxtCostValue();
    unit00468Menu.InitializeInfo((IEnumerable<PlayerUnit>) param.units, (IEnumerable<PlayerMaterialUnit>) null, Persist.unit00468SortAndFilter, false, false, false, true, true, (Action) (() => this.InitializeAllUnitInfosExtend(param)));
    e = unit00468Menu.CreateUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = unit00468Menu.CreateBottomInformationObject();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = unit00468Menu.DisplaySelectUnit();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00468Menu.UpdateInfomation();
    unit00468Menu.InitializeEnd();
    if (unit00468Menu.lastReferenceUnitID != -1)
    {
      yield return (object) null;
      unit00468Menu.resolveScrollPosition(unit00468Menu.lastReferenceUnitID);
      yield return (object) null;
      unit00468Menu.setLastReference();
    }
  }

  public IEnumerator Init(
    PlayerDeck playerDeck,
    PlayerUnit[] playerUnits,
    int max_cost,
    bool isEquip)
  {
    Unit00468Menu unit00468Menu = this;
    IEnumerator e = unit00468Menu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00468Menu.serverTime = ServerTime.NowAppTime();
    unit00468Menu.deck_type_id = playerDeck.deck_type_id;
    unit00468Menu.deck_number = playerDeck.deck_number;
    unit00468Menu.maxCost = max_cost;
    ((IEnumerable<GameObject>) unit00468Menu.linkCharacters).ForEach<GameObject>((Action<GameObject>) (v => v.transform.Clear()));
    unit00468Menu.updateTxtCostValue();
    playerUnits = ((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.IsNormalUnit)).ToArray<PlayerUnit>();
    unit00468Menu.InitializeInfo((IEnumerable<PlayerUnit>) playerUnits, (IEnumerable<PlayerMaterialUnit>) null, Persist.unit00468SortAndFilter, isEquip, false, false, true, true, (Action) (() => this.InitializeAllUnitInfosExtend(playerDeck)));
    e = unit00468Menu.CreateUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = unit00468Menu.CreateBottomInformationObject();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = unit00468Menu.DisplaySelectUnit();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00468Menu.UpdateInfomation();
    unit00468Menu.InitializeEnd();
    if (unit00468Menu.lastReferenceUnitID != -1)
    {
      yield return (object) null;
      unit00468Menu.resolveScrollPosition(unit00468Menu.lastReferenceUnitID);
      yield return (object) null;
      unit00468Menu.setLastReference();
    }
  }

  protected virtual int GetUsedCost()
  {
    int cost = 0;
    this.selectedUnitIcons.ForEach((Action<UnitIconInfo>) (x => cost += x.playerUnit.cost));
    return cost;
  }

  public override void UpdateInfomation()
  {
    this.updateExcludeOverkillers();
    this.StartCoroutine(this.DisplaySelectUnit());
    this.updateTxtCostValue(this.GetUsedCost());
  }

  protected virtual IEnumerator DeckEditAsync()
  {
    Unit00468Menu unit00468Menu = this;
    int[] array = unit00468Menu.selectedUnitIcons.OrderBy<UnitIconInfo, int>((Func<UnitIconInfo, int>) (x => x.select)).Select<UnitIconInfo, int>((Func<UnitIconInfo, int>) (x => x.playerUnit.id)).ToArray<int>();
    IEnumerator e = unit00468Menu.DeckEditApi(unit00468Menu.deck_type_id, unit00468Menu.deck_number, array);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00468Menu.backScene();
  }

  protected IEnumerator DeckEditApi(int deckTypeID, int deckNumber, int[] playerUnitIds)
  {
    Unit00468Menu unit00468Menu = this;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    NGGameDataManager instance = Singleton<NGGameDataManager>.GetInstance();
    IEnumerator e1;
    if (!instance.IsSea || instance.QuestType.HasValue)
    {
      CommonQuestType? questType = instance.QuestType;
      CommonQuestType commonQuestType = CommonQuestType.Sea;
      if (!(questType.GetValueOrDefault() == commonQuestType & questType.HasValue))
      {
        e1 = WebAPI.DeckEdit(deckTypeID, deckNumber, playerUnitIds, (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e))).Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        goto label_10;
      }
    }
    // ISSUE: reference to a compiler-generated method
    e1 = WebAPI.SeaDeckEdit(deckNumber, playerUnitIds, new Action<WebAPI.Response.UserError>(unit00468Menu.\u003CDeckEditApi\u003Eb__31_0)).Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
label_10:
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
  }

  public override void IbtnYes()
  {
    if (this.IsPush)
      return;
    if (this.customEdit_ != null)
    {
      this.IsPush = true;
      if (this.customEdit_.isMulti)
        this.customEdit_.multi.setUnits(this.selectedUnitIcons.OrderBy<UnitIconInfo, int>((Func<UnitIconInfo, int>) (x => x.select)).Select<UnitIconInfo, int>((Func<UnitIconInfo, int>) (x => x.playerUnit.id)).ToArray<int>());
      this.backScene();
    }
    else
      this.StartCoroutine(this.DeckEditAsync());
  }

  protected virtual void IconGraySetting(UnitIconBase unitIcon, UnitIconInfo info)
  {
    bool flag = this.excludeOverkillersIds != null && this.excludeOverkillersIds.Contains(info.playerUnit.id);
    if (this.selectedUnitIcons.Count<UnitIconInfo>() >= this.SelectMax)
      unitIcon.Gray = !info.gray;
    else if (!this.isEditCustomDeck & flag)
      unitIcon.Gray = true;
    else if (this.totalCost >= this.maxCost)
      unitIcon.Gray = !info.gray;
    else if (info.select >= 0)
      unitIcon.Gray = info.gray;
    else if (info.playerUnit.cost <= this.maxCost - this.totalCost)
      unitIcon.Gray = info.gray;
    else
      unitIcon.Gray = !info.gray;
    unitIcon.Overkillers = flag;
    unitIcon.SetupDeckStatusBlink();
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
    if (unitIcon.Selected)
    {
      this.Deselect(unitIcon);
      this.UpdateInfomation();
      this.UpdateSelectIcon();
    }
    else
    {
      if (this.selectedUnitIcons.Count >= this.SelectMax || unitIcon.PlayerUnit.cost > this.maxCost - this.totalCost || !this.isEditCustomDeck && this.excludeOverkillersIds != null && this.excludeOverkillersIds.Contains(unitIcon.PlayerUnit.id))
        return;
      if (this.iconSelMode == UnitSelectMenuBase.IconSelMode.Number)
      {
        if (this.isSelectNumPack)
          unitIcon.Select(this.selectedUnitIcons.Count);
        else
          unitIcon.Select(this.GetMinSelectIndex());
      }
      else
        unitIcon.SelectByCheckIcon();
      UnitIconInfo unitInfoAll = this.GetUnitInfoAll(unitIcon.PlayerUnit);
      if (unitInfoAll != null)
      {
        this.UnitInfoUpdate(unitInfoAll, true, unitIcon.SelIndex);
        this.selectedUnitIcons.Add(unitInfoAll);
      }
      UnitIconInfo unitInfoDisplay = this.GetUnitInfoDisplay(unitIcon.PlayerUnit);
      if (unitInfoDisplay != null)
        this.UnitInfoUpdate(unitInfoDisplay, true, unitIcon.SelIndex);
      this.UpdateInfomation();
      this.UpdateSelectIcon();
    }
  }

  protected override void CreateUnitIconCache(int info_index, int unit_index, PlayerUnit baseUnit = null)
  {
    base.CreateUnitIconCache(info_index, unit_index);
    this.CreateUnitIconAction(info_index, unit_index);
  }

  protected bool updateExcludeOverkillers()
  {
    return this.updateExcludeOverkillers(this.selectedUnitIcons.Select<UnitIconInfo, PlayerUnit>((Func<UnitIconInfo, PlayerUnit>) (x => x.playerUnit)).ToArray<PlayerUnit>());
  }

  protected bool updateExcludeOverkillers(PlayerUnit[] selectedUnits)
  {
    if (this.isNonCheckOverkillers)
    {
      this.excludeOverkillersIds = (HashSet<int>) null;
      return false;
    }
    if (this.isEditCustomDeck)
    {
      if (this.excludeOverkillersIds != null)
        return false;
      this.excludeOverkillersIds = new HashSet<int>();
      for (int index1 = 0; index1 < selectedUnits.Length; ++index1)
      {
        PlayerUnit selectedUnit = selectedUnits[index1];
        if (!(selectedUnit == (PlayerUnit) null) && selectedUnit.isAnyCacheOverkillersUnits)
        {
          for (int index2 = 0; index2 < selectedUnit.cache_overkillers_units.Length; ++index2)
          {
            if (selectedUnit.cache_overkillers_units[index2] != (PlayerUnit) null)
              this.excludeOverkillersIds.Add(selectedUnit.cache_overkillers_units[index2].id);
          }
        }
      }
      return true;
    }
    HashSet<int> excludeOverkillersIds = this.excludeOverkillersIds;
    this.excludeOverkillersIds = new HashSet<int>();
    if (selectedUnits != null)
    {
      for (int index3 = 0; index3 < selectedUnits.Length; ++index3)
      {
        PlayerUnit selectedUnit = selectedUnits[index3];
        if (!(selectedUnit == (PlayerUnit) null))
        {
          selectedUnit.resetOnceOverkillers();
          if (selectedUnit.isAnyCacheOverkillersUnits)
          {
            for (int index4 = 0; index4 < selectedUnit.cache_overkillers_units.Length; ++index4)
            {
              if (selectedUnit.cache_overkillers_units[index4] != (PlayerUnit) null)
                this.excludeOverkillersIds.Add(selectedUnit.cache_overkillers_units[index4].id);
            }
          }
          else
          {
            int overkillersBaseId;
            if ((overkillersBaseId = selectedUnit.overkillers_base_id) > 0)
              this.excludeOverkillersIds.Add(overkillersBaseId);
          }
        }
      }
    }
    if (excludeOverkillersIds == null)
      return this.excludeOverkillersIds.Count > 0;
    return excludeOverkillersIds.Count != this.excludeOverkillersIds.Count || !excludeOverkillersIds.Equals((object) this.excludeOverkillersIds);
  }
}
