// Decompiled with JetBrains decompiler
// Type: Unit00410Menu
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
[AddComponentMenu("Scenes/Unit/Sale/Menu")]
public class Unit00410Menu : UnitSelectMenuBase
{
  [SerializeField]
  private GameObject ibtnEnter;
  private bool isOverAlert;
  private bool isMemoryAlert;
  private bool isInYesAsync;
  private bool isSold;
  [SerializeField]
  private Unit00411Menu unit00411;
  [SerializeField]
  private Unit00414Menu unit00414;
  [SerializeField]
  [Tooltip("遷移元で切り替えるソートラベルをセット(順番:姫一覧｜素材一覧｜姫所持枠超過ダイアログ")]
  private UISprite[] sortSprites_;

  public Unit00410Menu.FromType fromType_ { get; private set; }

  public IEnumerator Init(
    PlayerDeck[] playerDeck,
    Player player,
    PlayerUnit[] playerUnits,
    PlayerMaterialUnit[] playerMaterialUnits,
    bool isEquip,
    Unit00410Menu.FromType fromType = Unit00410Menu.FromType.AlertUnitOver)
  {
    Unit00410Menu unit00410Menu = this;
    unit00410Menu.setDisable((MonoBehaviour) unit00410Menu.unit00411);
    unit00410Menu.setDisable((MonoBehaviour) unit00410Menu.unit00414);
    unit00410Menu.fromType_ = fromType;
    int index = (int) fromType;
    if (unit00410Menu.sortSprites_ != null && unit00410Menu.sortSprites_.Length > index && Object.op_Inequality((Object) unit00410Menu.sortSprites_[index], (Object) null))
      unit00410Menu.SortSprite = unit00410Menu.sortSprites_[index];
    unit00410Menu.isInYesAsync = false;
    IEnumerator e = unit00410Menu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00410Menu.selectedUnitIcons.Clear();
    Persist<Persist.UnitSortAndFilterInfo> persistData = (Persist<Persist.UnitSortAndFilterInfo>) null;
    switch (fromType)
    {
      case Unit00410Menu.FromType.UnitList:
        playerMaterialUnits = new PlayerMaterialUnit[0];
        persistData = Persist.unit00410SortAndFilter;
        break;
      case Unit00410Menu.FromType.MaterialList:
        playerUnits = new PlayerUnit[0];
        break;
      case Unit00410Menu.FromType.AlertUnitOver:
        playerMaterialUnits = new PlayerMaterialUnit[0];
        persistData = Persist.unit00410SortAndFilter;
        break;
    }
    unit00410Menu.InitializeInfoEx((IEnumerable<PlayerUnit>) playerUnits, (IEnumerable<PlayerMaterialUnit>) playerMaterialUnits, persistData, isEquip, false, true, true, true, false, (Action) (() => this.InitializeAllUnitInfosExtend(playerDeck)));
    e = unit00410Menu.CreateUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00410Menu.UpdateInfomation();
    unit00410Menu.InitializeEnd();
  }

  private void setDisable(MonoBehaviour mono)
  {
    if (!Object.op_Inequality((Object) mono, (Object) null))
      return;
    ((Behaviour) mono).enabled = false;
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
    if (this.selectedUnitIcons.Count > 0 && num1 <= this.SelectMax)
      ((UIButtonColor) this.ibtnEnter.GetComponent<UIButton>()).isEnabled = true;
    else
      ((UIButtonColor) this.ibtnEnter.GetComponent<UIButton>()).isEnabled = false;
    Player player = SMManager.Get<Player>();
    this.TxtNumberzeny.SetTextLocalize(string.Format("{0}", (object) num2));
    ((UIWidget) this.TxtNumberzeny).color = num2 + player.money <= Consts.GetInstance().MONEY_MAX ? Color.white : Color.red;
    this.TxtNumberpossession.SetTextLocalize(string.Format("{0}", (object) num3));
    ((UIWidget) this.TxtNumberpossession).color = num3 + player.medal <= Consts.GetInstance().MEDAL_MAX ? Color.white : Color.red;
    ((UIWidget) this.TxtNumberselect).color = num1 >= this.SelectMax ? Color.red : Color.white;
    this.TxtNumberselect.SetTextLocalize(string.Format("{0}/{1}", (object) num1, (object) this.SelectMax));
    this.isOverAlert = num2 + player.money > Consts.GetInstance().MONEY_MAX || num3 + player.medal > Consts.GetInstance().MEDAL_MAX;
  }

  public override void IbtnClearS()
  {
    if (this.isInYesAsync)
      return;
    base.IbtnClearS();
  }

  protected override void Deselect(UnitIconBase unitIcon)
  {
    if (this.isInYesAsync)
      return;
    base.Deselect(unitIcon);
  }

  protected override void Select(UnitIconBase unitIconBase)
  {
    if (this.isInYesAsync)
      return;
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
    Unit00410Menu menu = this;
    menu.isInYesAsync = true;
    menu.isSold = true;
    if (menu.selectedUnitIcons.Any<UnitIconInfo>((Func<UnitIconInfo, bool>) (icon => icon.is_tower_entry)))
    {
      bool isRejected = false;
      yield return (object) PopupManager.StartTowerEntryUnitWarningPopupProc((Action<bool>) (selected => isRejected = !selected));
      if (isRejected)
      {
        menu.isInYesAsync = false;
        yield break;
      }
    }
    menu.isInYesAsync = false;
    menu.IsPush = true;
    Future<GameObject> prefab0041014F;
    IEnumerator e;
    if (menu.selectedUnitIcons.Any<UnitIconInfo>((Func<UnitIconInfo, bool>) (icon => icon.playerUnit.unit.rarity.index > 1)))
    {
      Unit00410Menu unit00410Menu = menu;
      prefab0041014F = Res.Prefabs.popup.popup_004_10_14__anim_popup01.Load<GameObject>();
      e = prefab0041014F.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject result = prefab0041014F.Result;
      Popup0041013Menu component = Singleton<PopupManager>.GetInstance().open(result).GetComponent<Popup0041013Menu>();
      List<PlayerUnit> ls = new List<PlayerUnit>();
      menu.selectedUnitIcons.ForEach((Action<UnitIconInfo>) (v => ls.AddRange((IEnumerable<PlayerUnit>) unit00410Menu.ExpandPlayerUnits(v, v.SelectedCount))));
      e = component.Init(ls, menu, menu.isOverAlert, menu.isMemoryAlert);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      prefab0041014F = (Future<GameObject>) null;
    }
    else if (menu.selectedUnitIcons.Count > 0)
    {
      Unit00410Menu unit00410Menu = menu;
      prefab0041014F = Res.Prefabs.popup.popup_004_10_13__anim_popup01.Load<GameObject>();
      e = prefab0041014F.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject result = prefab0041014F.Result;
      Popup0041013Menu component = Singleton<PopupManager>.GetInstance().open(result).GetComponent<Popup0041013Menu>();
      List<PlayerUnit> ls = new List<PlayerUnit>();
      menu.selectedUnitIcons.ForEach((Action<UnitIconInfo>) (v => ls.AddRange((IEnumerable<PlayerUnit>) unit00410Menu.ExpandPlayerUnits(v, v.SelectedCount))));
      e = component.Init(ls, menu, menu.isOverAlert, menu.isMemoryAlert);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      prefab0041014F = (Future<GameObject>) null;
    }
  }

  public override void IbtnYes()
  {
    if (this.isInYesAsync || this.IsPushAndSet())
      return;
    base.IbtnYes();
    this.StartCoroutine(this.IbtnYesAsync());
  }

  public override void IbtnBack()
  {
    if (this.isInYesAsync)
      return;
    if (this.isSold && this.fromType_ != Unit00410Menu.FromType.AlertUnitOver)
    {
      this.isSold = false;
      Singleton<NGSceneManager>.GetInstance().clearStack();
      Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
      Singleton<NGSceneManager>.GetInstance().changeScene("mypage", false);
    }
    else
    {
      switch (this.fromType_)
      {
        case Unit00410Menu.FromType.UnitList:
          Singleton<NGSceneManager>.GetInstance().clearStack();
          Unit00468Scene.changeScene00411WithInitialize(false);
          break;
        case Unit00410Menu.FromType.MaterialList:
          Unit004UnitMaterialsListScene.changeScene(false);
          break;
        default:
          base.IbtnBack();
          break;
      }
    }
  }

  private List<PlayerUnit> ExpandPlayerUnits(UnitIconInfo iconInfo, int unitCount)
  {
    if (Debug.isDebugBuild && (unitCount > iconInfo.count || unitCount == 0))
      Debug.LogError((object) ("Illegal unitCount specified. ID: " + (object) iconInfo.playerUnit.unit.ID + ", unitCount: " + (object) unitCount + ", iconInfo.count: " + (object) iconInfo.count));
    if (unitCount < 1)
      return (List<PlayerUnit>) null;
    List<PlayerUnit> playerUnitList1;
    if (unitCount == 1)
    {
      playerUnitList1 = new List<PlayerUnit>();
      playerUnitList1.Add(iconInfo.playerUnit);
    }
    else
      playerUnitList1 = !iconInfo.playerUnit.unit.IsNormalUnit ? ((IEnumerable<PlayerMaterialUnit>) SMManager.Get<PlayerMaterialUnit[]>()).Where<PlayerMaterialUnit>((Func<PlayerMaterialUnit, bool>) (t => t.id == iconInfo.playerUnit.id)).SelectMany<PlayerMaterialUnit, PlayerUnit>((Func<PlayerMaterialUnit, IEnumerable<PlayerUnit>>) (x =>
      {
        List<PlayerUnit> playerUnitList2 = new List<PlayerUnit>();
        for (int count = 0; count < iconInfo.SelectedCount; ++count)
          playerUnitList2.Add(PlayerUnit.CreateByPlayerMaterialUnit(x, count));
        return (IEnumerable<PlayerUnit>) playerUnitList2;
      })).ToList<PlayerUnit>() : ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).Where<PlayerUnit>((Func<PlayerUnit, bool>) (t => t.unit.ID == iconInfo.playerUnit.unit.ID)).Take<PlayerUnit>(unitCount).ToList<PlayerUnit>();
    return playerUnitList1;
  }

  public void AllUpdateUnitIcons()
  {
    foreach (UnitIconInfo allUnitInfo in this.allUnitInfos)
    {
      if (Object.op_Inequality((Object) allUnitInfo.icon, (Object) null) && allUnitInfo.button_enable)
      {
        if (this.SelectedUnitIsMax())
          allUnitInfo.icon.Gray = allUnitInfo.select == -1;
        else
          allUnitInfo.icon.Gray = allUnitInfo.select != -1;
      }
    }
  }

  private IEnumerator OpenPopup(UnitIconInfo unitIconInfo)
  {
    Unit00410Menu unit00410Menu = this;
    Future<GameObject> dialogPrefab = Res.Prefabs.popup.popup_004_14__anim_popup01.Load<GameObject>();
    IEnumerator e = dialogPrefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = dialogPrefab.Result;
    GameObject gameObject = Singleton<PopupManager>.GetInstance().open(result);
    List<Action> callbackList = new List<Action>();
    callbackList.Add(new Action(((UnitSelectMenuBase) unit00410Menu).CreateSelectUnitList));
    callbackList.Add(new Action(unit00410Menu.AllUpdateUnitIcons));
    callbackList.Add(new Action(((UnitSelectMenuBase) unit00410Menu).UpdateInfomation));
    int maxSellableQuantity = unit00410Menu.SelectMax - (unit00410Menu.SelectedUnitCount() - unitIconInfo.SelectedCount);
    yield return (object) gameObject.GetComponent<Unit00410Popup>().Show(unitIconInfo, callbackList, maxSellableQuantity);
  }

  public void onClickedStorageSale()
  {
    if (this.IsPushAndSet())
      return;
    Unit004StorageScene.changeSceneSell(false, this.fromType_ == Unit00410Menu.FromType.AlertUnitOver);
  }

  public enum FromType
  {
    UnitList,
    MaterialList,
    AlertUnitOver,
  }
}
