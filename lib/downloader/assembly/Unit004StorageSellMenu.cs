// Decompiled with JetBrains decompiler
// Type: Unit004StorageSellMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class Unit004StorageSellMenu : UnitStorageMenuBase
{
  private bool isOverAlert;
  private bool isMemoryAlert;
  private GameObject prefabDialog;
  private GameObject prefab0041014;
  private GameObject prefab0041013;

  public bool isFromAlertUnitOver { get; set; }

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }

  public IEnumerator Init(PlayerUnit[] storageUnits)
  {
    Unit004StorageSellMenu unit004StorageSellMenu = this;
    Future<GameObject> prefab0041014F;
    IEnumerator e;
    if (Object.op_Equality((Object) unit004StorageSellMenu.prefab0041014, (Object) null))
    {
      prefab0041014F = Res.Prefabs.popup.popup_004_10_14__anim_popup01.Load<GameObject>();
      e = prefab0041014F.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unit004StorageSellMenu.prefab0041014 = prefab0041014F.Result;
      prefab0041014F = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) unit004StorageSellMenu.prefab0041013, (Object) null))
    {
      prefab0041014F = Res.Prefabs.popup.popup_004_10_13__anim_popup01.Load<GameObject>();
      e = prefab0041014F.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unit004StorageSellMenu.prefab0041013 = prefab0041014F.Result;
      prefab0041014F = (Future<GameObject>) null;
    }
    foreach (PlayerUnit storageUnit in storageUnits)
      storageUnit.is_storage = true;
    Player player = SMManager.Get<Player>();
    int length = storageUnits.Length;
    int maxUnitReserves = player.max_unit_reserves;
    SMManager.Get<PlayerDeck[]>();
    e = unit004StorageSellMenu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit004StorageSellMenu.selectedUnitIcons.Clear();
    unit004StorageSellMenu.SetIconType(UnitMenuBase.IconType.Normal);
    unit004StorageSellMenu.InitializeInfoEx((IEnumerable<PlayerUnit>) storageUnits, (IEnumerable<PlayerMaterialUnit>) null, Persist.unit004StorageSortAndFilter, false, false, true, true, true, false);
    unit004StorageSellMenu.IgnoreFavoriteUnit();
    e = unit004StorageSellMenu.CreateUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit004StorageSellMenu.UpdateInfomation();
    unit004StorageSellMenu.InitializeEnd();
  }

  protected override IEnumerator CreateUnitIcon(
    int info_index,
    int unit_index,
    PlayerUnit baseUnit = null)
  {
    Unit004StorageSellMenu unit004StorageSellMenu = this;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = unit004StorageSellMenu.\u003C\u003En__0(info_index, unit_index, baseUnit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit004StorageSellMenu.CreateUnitIconAction(info_index, unit_index);
  }

  protected override void CreateUnitIconCache(int info_index, int unit_index, PlayerUnit baseUnit = null)
  {
    base.CreateUnitIconCache(info_index, unit_index);
    this.CreateUnitIconAction(info_index, unit_index);
  }

  public void AllUpdateUnitIcons()
  {
    foreach (UnitIconInfo allUnitInfo in this.allUnitInfos)
    {
      if (Object.op_Inequality((Object) allUnitInfo.icon, (Object) null) && ((Behaviour) allUnitInfo.icon.Button).enabled)
      {
        if (this.SelectedUnitIsMax())
          allUnitInfo.icon.Gray = allUnitInfo.select == -1;
        else
          allUnitInfo.icon.Gray = allUnitInfo.select != -1;
      }
    }
  }

  private int SelectedUnitCount()
  {
    return this.selectedUnitIcons.Select<UnitIconInfo, int>((Func<UnitIconInfo, int>) (u => u.SelectedCount)).Sum();
  }

  public override bool SelectedUnitIsMax() => this.SelectedUnitCount() >= this.SelectMax;

  public override void UpdateInfomation()
  {
    base.UpdateInfomation();
    int num1 = 0;
    long num2 = 0;
    int num3 = 0;
    List<PlayerUnit> playerUnitList = new List<PlayerUnit>();
    int?[] source = PlayerTransmigrateMemoryPlayerUnitIds.Current != null ? PlayerTransmigrateMemoryPlayerUnitIds.Current.transmigrate_memory_player_unit_ids : new int?[0];
    this.isMemoryAlert = false;
    foreach (UnitIconInfo selectedUnitIcon in this.selectedUnitIcons)
    {
      UnitIconInfo unit = selectedUnitIcon;
      if (unit != null && unit.playerUnit != (PlayerUnit) null)
      {
        playerUnitList.Add(unit.playerUnit);
        if (!this.isMemoryAlert)
          this.isMemoryAlert = ((IEnumerable<int?>) source).Any<int?>((Func<int?, bool>) (x =>
          {
            if (!x.HasValue)
              return false;
            int? nullable = x;
            int id = unit.playerUnit.id;
            return nullable.GetValueOrDefault() == id & nullable.HasValue;
          }));
      }
      num2 += (long) unit.playerUnit.unit._base_sell_price * (long) unit.playerUnit.level * (long) unit.SelectedCount;
      num1 += unit.SelectedCount;
      if ((!unit.unit.IsMaterialUnit ? 1 : (unit.unit.is_buildup_only == 1 ? 1 : 0)) != 0)
        num3 += unit.unit.rarity.sell_rarity_medal * unit.SelectedCount;
    }
    Player player = SMManager.Get<Player>();
    this.TxtNumberzeny.SetTextLocalize(string.Format("{0}", (object) num2));
    ((UIWidget) this.TxtNumberzeny).color = num2 + player.money <= Consts.GetInstance().MONEY_MAX ? Color.white : Color.red;
    this.TxtNumberpossession.SetTextLocalize(string.Format("{0}", (object) num3));
    ((UIWidget) this.TxtNumberpossession).color = num3 + player.medal <= Consts.GetInstance().MEDAL_MAX ? Color.white : Color.red;
    ((UIWidget) this.TxtNumberselect).color = num1 >= this.SelectMax ? Color.red : Color.white;
    this.TxtNumberselect.SetTextLocalize(string.Format("{0}/{1}", (object) num1, (object) this.SelectMax));
    this.isOverAlert = num2 + player.money > Consts.GetInstance().MONEY_MAX || num3 + player.medal > Consts.GetInstance().MEDAL_MAX;
  }

  protected override void Select(UnitIconBase unitIconBase)
  {
    if (unitIconBase.PlayerUnit == (PlayerUnit) null)
      base.Select(unitIconBase);
    else if (unitIconBase.PlayerUnit.unit.IsNormalUnit)
    {
      base.Select(unitIconBase);
    }
    else
    {
      UnitIconInfo unitInfoAll = this.GetUnitInfoAll(unitIconBase.PlayerUnit);
      if (unitInfoAll.select == -1 && this.SelectedUnitIsMax())
        return;
      this.StartCoroutine(this.OpenPopup(unitInfoAll));
    }
  }

  private IEnumerator IbtnYesAsync()
  {
    Unit004StorageSellMenu menu = this;
    IEnumerator e;
    if (menu.selectedUnitIcons.Any<UnitIconInfo>((Func<UnitIconInfo, bool>) (ui => ui.playerUnit.unit.rarity.index > 1)))
    {
      Unit004StorageSellMenu unit004StorageSellMenu = menu;
      Popup0041013Menu component = Singleton<PopupManager>.GetInstance().open(menu.prefab0041014).GetComponent<Popup0041013Menu>();
      List<PlayerUnit> ls = new List<PlayerUnit>();
      menu.selectedUnitIcons.ForEach((Action<UnitIconInfo>) (v => ls.AddRange((IEnumerable<PlayerUnit>) unit004StorageSellMenu.ExpandPlayerUnits(v, v.SelectedCount))));
      e = component.Init(ls, menu, menu.isOverAlert, menu.isMemoryAlert);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else if (menu.selectedUnitIcons.Count > 0)
    {
      Unit004StorageSellMenu unit004StorageSellMenu = menu;
      Popup0041013Menu component = Singleton<PopupManager>.GetInstance().open(menu.prefab0041013).GetComponent<Popup0041013Menu>();
      List<PlayerUnit> ls = new List<PlayerUnit>();
      menu.selectedUnitIcons.ForEach((Action<UnitIconInfo>) (v => ls.AddRange((IEnumerable<PlayerUnit>) unit004StorageSellMenu.ExpandPlayerUnits(v, v.SelectedCount))));
      e = component.Init(ls, menu, menu.isOverAlert, menu.isMemoryAlert);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public override void IbtnYes()
  {
    if (this.IsPushAndSet())
      return;
    base.IbtnYes();
    this.StartCoroutine(this.IbtnYesAsync());
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    if (this.isFromAlertUnitOver)
      this.backScene();
    else
      Unit004StorageScene.changeSceneListWithInitialize(false);
  }

  public void OnBtnUnitSellList()
  {
    Unit00468Scene.changeScene00410WithInitialize(false, this.isFromAlertUnitOver ? Unit00410Menu.FromType.AlertUnitOver : Unit00410Menu.FromType.UnitList);
  }

  private List<PlayerUnit> ExpandPlayerUnits(UnitIconInfo iconInfo, int unitCount)
  {
    if ((unitCount > iconInfo.count || unitCount == 0) && Debug.isDebugBuild)
      Debug.LogError((object) ("Illegal unitCount specified. ID: " + (object) iconInfo.playerUnit.unit.ID + ", unitCount: " + (object) unitCount + ", iconInfo.count: " + (object) iconInfo.count));
    if (unitCount < 1)
    {
      Debug.LogError((object) "Unit count is equal at 0");
      return (List<PlayerUnit>) null;
    }
    return new List<PlayerUnit>() { iconInfo.playerUnit };
  }

  private IEnumerator OpenPopup(UnitIconInfo unitIconInfo)
  {
    Unit004StorageSellMenu unit004StorageSellMenu = this;
    if (Object.op_Equality((Object) unit004StorageSellMenu.prefabDialog, (Object) null))
    {
      Future<GameObject> dialogPrefabF = Res.Prefabs.popup.popup_004_14__anim_popup01.Load<GameObject>();
      IEnumerator e = dialogPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unit004StorageSellMenu.prefabDialog = dialogPrefabF.Result;
      dialogPrefabF = (Future<GameObject>) null;
    }
    GameObject gameObject = Singleton<PopupManager>.GetInstance().open(unit004StorageSellMenu.prefabDialog);
    List<Action> callbackList = new List<Action>();
    callbackList.Add(new Action(((UnitSelectMenuBase) unit004StorageSellMenu).CreateSelectUnitList));
    callbackList.Add(new Action(unit004StorageSellMenu.AllUpdateUnitIcons));
    callbackList.Add(new Action(((UnitSelectMenuBase) unit004StorageSellMenu).UpdateInfomation));
    int maxSellableQuantity = unit004StorageSellMenu.SelectMax - (unit004StorageSellMenu.SelectedUnitCount() - unitIconInfo.SelectedCount);
    yield return (object) gameObject.GetComponent<Unit00410Popup>().Show(unitIconInfo, callbackList, maxSellableQuantity);
  }
}
