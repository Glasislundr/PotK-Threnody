// Decompiled with JetBrains decompiler
// Type: Tower029UnitSelectionMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using DeckOrganization;
using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Tower029UnitSelectionMenu : UnitSelectMenuBase
{
  [SerializeField]
  private UILabel lblTitle;
  [SerializeField]
  private UIButton btnDecide;
  private GameObject goHpGauge;
  private TowerProgress progress;
  private PlayerUnit[] playerUnits;
  private List<PlayerUnit> selectedUnits;
  private GameObject selectionOrderPopup;
  private bool IsAutoSelectProcess;
  private TowerUtil.SequenceType sequenceType;
  private bool isCheckSelectionMode;
  [SerializeField]
  private List<SupplyItem> SupplyItems = new List<SupplyItem>();
  [SerializeField]
  private List<SupplyItem> SaveDeck = new List<SupplyItem>();

  private IEnumerator ResourceLoad()
  {
    if (Object.op_Equality((Object) this.selectionOrderPopup, (Object) null))
    {
      Future<GameObject> f = Res.Prefabs.popup.popup_029_tower_unit_selection_auto__anim_popup01.Load<GameObject>();
      IEnumerator e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.selectionOrderPopup = f.Result;
      f = (Future<GameObject>) null;
    }
  }

  private void ShowUnitSelectionOrderPopup()
  {
    if (!Object.op_Inequality((Object) this.selectionOrderPopup, (Object) null))
      return;
    GameObject prefab = this.selectionOrderPopup.Clone();
    prefab.SetActive(false);
    prefab.GetComponent<Tower029UnitSelectionOrderPopup>().Initialize(new Action<TowerUtil.UnitSelectionOrder>(this.AutoSelect));
    prefab.SetActive(true);
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
  }

  private TowerCreator.Selection GetUnitSelectionOrder(TowerUtil.UnitSelectionOrder order)
  {
    TowerCreator.Selection unitSelectionOrder = TowerCreator.Selection.Element;
    switch (order)
    {
      case TowerUtil.UnitSelectionOrder.LEVEL:
        unitSelectionOrder = TowerCreator.Selection.Level;
        break;
      case TowerUtil.UnitSelectionOrder.ATTRIBUTE:
        unitSelectionOrder = TowerCreator.Selection.Element;
        break;
      case TowerUtil.UnitSelectionOrder.WEAPON:
        unitSelectionOrder = TowerCreator.Selection.GearKind;
        break;
      case TowerUtil.UnitSelectionOrder.FAVORITE:
        unitSelectionOrder = TowerCreator.Selection.Favorite;
        break;
    }
    return unitSelectionOrder;
  }

  private IEnumerator AutoSelectAsync(TowerUtil.UnitSelectionOrder order)
  {
    Tower029UnitSelectionMenu unitSelectionMenu = this;
    unitSelectionMenu.IsAutoSelectProcess = true;
    TowerCreator deckCreator_ = new TowerCreator(((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).Where<PlayerUnit>((Func<PlayerUnit, bool>) (u => u.unit.IsNormalUnit && u.level >= TowerUtil.BorderLevel)).ToArray<PlayerUnit>(), unitSelectionMenu.GetUnitSelectionOrder(order));
    IEnumerator e = deckCreator_.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unitSelectionMenu.selectedUnits = deckCreator_.result_;
    e = unitSelectionMenu.UpdateInfoAndScroll(((IEnumerable<PlayerUnit>) unitSelectionMenu.playerUnits).ToArray<PlayerUnit>());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unitSelectionMenu.UpdateInfomation();
  }

  private void AutoSelect(TowerUtil.UnitSelectionOrder order)
  {
    this.StartCoroutine(this.AutoSelectAsync(order));
  }

  private IEnumerator ExecUpdateInfoAndScroll()
  {
    Tower029UnitSelectionMenu unitSelectionMenu = this;
    IEnumerator e = unitSelectionMenu.UpdateInfoAndScroll(((IEnumerable<PlayerUnit>) unitSelectionMenu.playerUnits).ToArray<PlayerUnit>());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unitSelectionMenu.UpdateInfomation();
  }

  public IEnumerator InitializeAsync(
    TowerUtil.UnitSelectionMode mode,
    TowerProgress progress,
    TowerUtil.SequenceType type)
  {
    Tower029UnitSelectionMenu unitSelectionMenu = this;
    IEnumerator e = unitSelectionMenu.ResourceLoad();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unitSelectionMenu.playerUnits = ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).Where<PlayerUnit>((Func<PlayerUnit, bool>) (u => u.unit.IsNormalUnit && u.level >= TowerUtil.BorderLevel)).ToArray<PlayerUnit>();
    if (unitSelectionMenu.isInitialize)
    {
      e = unitSelectionMenu.UpdateInfoAndScroll(((IEnumerable<PlayerUnit>) unitSelectionMenu.playerUnits).ToArray<PlayerUnit>());
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      unitSelectionMenu.selectedUnits = new List<PlayerUnit>();
      unitSelectionMenu.progress = progress;
      unitSelectionMenu.sequenceType = type;
      e = unitSelectionMenu.Initialize();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unitSelectionMenu.SetIconType(UnitMenuBase.IconType.NormalWithHpGauge);
      unitSelectionMenu.InitializeInfo((IEnumerable<PlayerUnit>) unitSelectionMenu.playerUnits, (IEnumerable<PlayerMaterialUnit>) null, Persist.tower029UnitListSortAndFilter, false, false, false, true, false, new Action(unitSelectionMenu.InitializeAllUnitInfosExtend));
      e = unitSelectionMenu.CreateUnitIcon();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unitSelectionMenu.TxtNumber.SetTextLocalize(string.Format("{0}/{1}", (object) 0, (object) TowerUtil.MaxUnitNum));
      unitSelectionMenu.lblTitle.SetTextLocalize(Consts.GetInstance().TOWER_SELECTION_TITLE);
      if (unitSelectionMenu.lastReferenceUnitID != -1)
      {
        unitSelectionMenu.InitializeEnd();
        yield return (object) null;
        unitSelectionMenu.resolveScrollPosition(unitSelectionMenu.lastReferenceUnitID);
        yield return (object) null;
        unitSelectionMenu.setLastReference();
      }
      ((UIButtonColor) unitSelectionMenu.btnDecide).isEnabled = unitSelectionMenu.selectedUnits.Count > 0;
      unitSelectionMenu.isCheckSelectionMode = true;
    }
  }

  public void onStartScene(TowerUtil.UnitSelectionMode mode)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    if (this.isCheckSelectionMode)
    {
      this.isCheckSelectionMode = false;
      if (mode == TowerUtil.UnitSelectionMode.Auto)
        this.ShowUnitSelectionOrderPopup();
    }
    if (this.isInitialize)
      return;
    this.InitializeEnd();
  }

  public override void IbtnYes()
  {
    if (this.IsPushAndSet())
      return;
    this.SupplyItems = ((IEnumerable<SupplyItem>) SupplyItem.Merge(((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>().AllSupplies()).ToList<PlayerItem>().ToArray(), ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>().AllTowerSupplies()).ToArray<PlayerItem>())).ToList<SupplyItem>();
    this.SaveDeck = this.SupplyItems.Copy();
    Quest00210aScene.ChangeScene(true, new Quest00210Menu.Param()
    {
      SupplyItems = this.SupplyItems,
      SaveDeck = this.SaveDeck,
      removeButton = false,
      limitedOnly = true,
      mode = Quest00210Scene.Mode.Tower
    }, this.selectedUnitIcons.Select<UnitIconInfo, int>((Func<UnitIconInfo, int>) (x => x.playerUnit.id)).ToArray<int>(), this.progress, this.sequenceType);
  }

  public override void IbtnClearS()
  {
    this.selectedUnits.Clear();
    base.IbtnClearS();
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public void onClickUnitlIcon(PlayerUnit unit)
  {
    Unit0042Scene.changeScene(true, unit, this.getUnits());
  }

  public override void UpdateInfomation()
  {
    this.TxtNumber.SetTextLocalize(string.Format("{0}/{1}", (object) this.selectedUnitIcons.Count, (object) TowerUtil.MaxUnitNum));
    ((UIButtonColor) this.btnDecide).isEnabled = this.selectedUnitIcons.Count > 0;
  }

  public void InitializeAllUnitInfosExtend()
  {
    if (!this.IsAutoSelectProcess)
    {
      this.selectedUnitIcons.Clear();
      foreach (UnitIconInfo allUnitInfo in this.allUnitInfos)
      {
        UnitIconInfo info = allUnitInfo;
        int? nullable = this.selectedUnits.FirstIndexOrNull<PlayerUnit>((Func<PlayerUnit, bool>) (u => u != (PlayerUnit) null && u.id == info.playerUnit.id));
        if (nullable.HasValue)
        {
          info.gray = true;
          info.select = nullable.Value;
          this.selectedUnitIcons.Add(info);
        }
      }
    }
    else
    {
      foreach (UnitIconInfo allUnitInfo in this.allUnitInfos)
      {
        UnitIconInfo info = allUnitInfo;
        int? nullable = this.selectedUnits.FirstIndexOrNull<PlayerUnit>((Func<PlayerUnit, bool>) (u => u != (PlayerUnit) null && u.id == info.playerUnit.id));
        if (nullable.HasValue)
        {
          info.gray = true;
          info.select = nullable.Value;
          this.selectedUnitIcons.Add(info);
        }
      }
      this.IsAutoSelectProcess = false;
    }
  }

  protected override void CreateUnitIconAction(int info_index, int unit_index)
  {
    base.CreateUnitIconAction(info_index, unit_index);
    UnitIconBase allUnitIcon = this.allUnitIcons[unit_index];
    UnitIconInfo info = this.displayUnitInfos[info_index];
    if (info.select >= 0 || !this.selectedUnits.FirstIndexOrNull<PlayerUnit>((Func<PlayerUnit, bool>) (u => u != (PlayerUnit) null && u.unit.ID == info.unit.ID)).HasValue)
      return;
    info.gray = true;
    allUnitIcon.Gray = true;
  }

  protected override IEnumerator CreateUnitIconBase(GameObject prefab)
  {
    Tower029UnitSelectionMenu unitSelectionMenu = this;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = unitSelectionMenu.\u003C\u003En__0(prefab);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Equality((Object) unitSelectionMenu.goHpGauge, (Object) null))
    {
      Future<GameObject> f = Res.Prefabs.tower.dir_hp_gauge.Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unitSelectionMenu.goHpGauge = f.Result;
      f = (Future<GameObject>) null;
    }
    if (Object.op_Inequality((Object) unitSelectionMenu.goHpGauge, (Object) null))
    {
      for (int index = 0; index < unitSelectionMenu.allUnitIcons.Count; ++index)
      {
        UnitIcon allUnitIcon = (UnitIcon) unitSelectionMenu.allUnitIcons[index];
        unitSelectionMenu.goHpGauge.Clone(allUnitIcon.hp_gauge.transform);
        allUnitIcon.HpGauge.TweenHpGauge.setValue(100, 100, false);
      }
    }
  }

  protected override void Select(UnitIconBase unitIcon)
  {
    if (unitIcon.Gray && !unitIcon.Selected)
      return;
    base.Select(unitIcon);
    if (unitIcon.Selected)
    {
      this.selectedUnits.Add(unitIcon.PlayerUnit);
      this.allUnitIcons.Where<UnitIconBase>((Func<UnitIconBase, bool>) (i => !i.PlayerUnit.Equals(unitIcon.PlayerUnit) && i.Unit.ID == unitIcon.Unit.ID)).ForEach<UnitIconBase>((Action<UnitIconBase>) (u =>
      {
        u.Gray = true;
        UnitIconInfo unitInfoAll = this.GetUnitInfoAll(u.PlayerUnit);
        if (unitInfoAll == null)
          return;
        unitInfoAll.gray = true;
      }));
    }
    else
    {
      this.selectedUnits.Remove(unitIcon.PlayerUnit);
      this.allUnitIcons.Where<UnitIconBase>((Func<UnitIconBase, bool>) (i => !i.PlayerUnit.Equals(unitIcon.PlayerUnit) && i.Unit.ID == unitIcon.Unit.ID)).ForEach<UnitIconBase>((Action<UnitIconBase>) (u =>
      {
        u.Gray = false;
        UnitIconInfo unitInfoAll = this.GetUnitInfoAll(u.PlayerUnit);
        if (unitInfoAll == null)
          return;
        unitInfoAll.gray = false;
      }));
    }
    ((UIButtonColor) this.btnDecide).isEnabled = this.selectedUnits.Count > 0;
    if (this.selectedUnitIcons.Count < this.SelectMax)
      return;
    for (int i = 0; i < this.selectedUnitIcons.Count; i++)
      this.allUnitIcons.Where<UnitIconBase>((Func<UnitIconBase, bool>) (x => !x.PlayerUnit.Equals(this.selectedUnitIcons[i].playerUnit) && x.Unit.ID == this.selectedUnitIcons[i].unit.ID)).ForEach<UnitIconBase>((Action<UnitIconBase>) (u =>
      {
        u.Gray = true;
        UnitIconInfo unitInfoAll = this.GetUnitInfoAll(u.PlayerUnit);
        if (unitInfoAll == null)
          return;
        unitInfoAll.gray = true;
      }));
  }

  protected override void Sort(SortInfo info)
  {
    base.Sort(info);
    this.StartCoroutine(this.ExecUpdateInfoAndScroll());
  }
}
