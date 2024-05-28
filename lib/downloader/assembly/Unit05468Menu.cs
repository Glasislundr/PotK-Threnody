// Decompiled with JetBrains decompiler
// Type: Unit05468Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Earth;
using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Unit05468Menu : Unit00411Menu
{
  private readonly int MARKER_FLASH_TWEEN = 50;
  [SerializeField]
  private UILabel PossessionUnit;
  private int beforeInfoIndex;
  private int beforeIconIndex;
  private int afterInfoIndex;
  private int afterIconIndex;
  private GameObject supplyIcon;
  [SerializeField]
  private Transform[] SuppleIconPositions;
  private PlayerUnit[] cacheUnits;
  [SerializeField]
  private List<SupplyItem> SupplyItems = new List<SupplyItem>();
  [SerializeField]
  private List<SupplyItem> SaveDeck = new List<SupplyItem>();
  private Unit05468Menu.state selectState;

  private void ChangeState(Unit05468Menu.state state) => this.selectState = state;

  protected override void Update()
  {
    base.Update();
    if (this.selectState == Unit05468Menu.state.WAIT)
      return;
    switch (this.selectState)
    {
      case Unit05468Menu.state.INIT:
        Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
        this.InitSelectIndex();
        break;
      case Unit05468Menu.state.SELECT_ANIMATION:
        Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
        this.SelectAnimation();
        break;
      case Unit05468Menu.state.SELECT:
        Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
        this.Select();
        break;
      case Unit05468Menu.state.SWAP_ANIMATION:
        Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
        this.PlaySwapAnimation();
        break;
      case Unit05468Menu.state.SWAP:
        this.StartCoroutine(this.Swap());
        break;
    }
  }

  public IEnumerator Initialize(PlayerUnit[] playerUnits)
  {
    Unit05468Menu unit05468Menu = this;
    unit05468Menu.selectState = Unit05468Menu.state.INIT;
    unit05468Menu.cacheUnits = playerUnits;
    unit05468Menu.SetIconType(UnitMenuBase.IconType.EarthNormal);
    IEnumerator e = unit05468Menu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit05468Menu.InitializeInfo((IEnumerable<PlayerUnit>) playerUnits, (IEnumerable<PlayerMaterialUnit>) null, (Persist<Persist.UnitSortAndFilterInfo>) null, false, false, false, false, false);
    e = unit05468Menu.CreateUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit05468Menu.InitializeEnd();
    unit05468Menu.PossessionUnit.SetTextLocalize(playerUnits.Length.ToString());
  }

  public IEnumerator LoadResources()
  {
    if (Object.op_Equality((Object) this.supplyIcon, (Object) null))
    {
      Future<GameObject> supplyIconF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
      IEnumerator e = supplyIconF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.supplyIcon = supplyIconF.Result;
      supplyIconF = (Future<GameObject>) null;
    }
  }

  protected override IEnumerator CreateUnitIcon(
    int info_index,
    int unit_index,
    PlayerUnit baseUnit = null)
  {
    Unit05468Menu unit05468Menu = this;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = unit05468Menu.\u003C\u003En__0(info_index, unit_index, (PlayerUnit) null);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit05468Menu.displayUnitInfos[info_index].alignmentSequence = info_index;
    unit05468Menu.DispIndex(unit05468Menu.allUnitIcons[unit_index], unit05468Menu.displayUnitInfos[info_index].alignmentSequence + 1);
  }

  protected override void CreateUnitIconCache(int info_index, int unit_index, PlayerUnit baseUnit = null)
  {
    base.CreateUnitIconCache(info_index, unit_index);
    this.displayUnitInfos[info_index].alignmentSequence = info_index;
    this.DispIndex(this.allUnitIcons[unit_index], this.displayUnitInfos[info_index].alignmentSequence + 1);
  }

  protected override void CreateUnitIconAction(int info_index, int unit_index)
  {
    UnitIconBase allUnitIcon = this.allUnitIcons[unit_index];
    allUnitIcon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    ((UnitIcon) allUnitIcon).SetEarthButtonDetalEvent(this.allUnitInfos[info_index].playerUnit, this.getUnits());
    allUnitIcon.onClick = (Action<UnitIconBase>) (ui =>
    {
      if (this.IsPush)
        return;
      if (this.beforeInfoIndex == 0)
      {
        if (info_index == 0)
          return;
        this.beforeInfoIndex = info_index;
        this.beforeIconIndex = unit_index;
        this.ChangeState(Unit05468Menu.state.SELECT_ANIMATION);
      }
      else
      {
        if (info_index == 0)
          return;
        if (this.beforeInfoIndex == info_index)
        {
          this.ChangeState(Unit05468Menu.state.INIT);
        }
        else
        {
          this.afterInfoIndex = info_index;
          this.afterIconIndex = unit_index;
          this.ChangeState(Unit05468Menu.state.SELECT_ANIMATION);
        }
      }
    });
    allUnitIcon.markerDisplayFinished = (Action) (() => this.ChangeState(Unit05468Menu.state.SELECT));
  }

  public void IbtnOk()
  {
    if (this.IsPushAndSet())
      return;
    this.ApplyIndex();
    Singleton<EarthDataManager>.GetInstance().ClearCharacterBattleIndex();
    Singleton<EarthDataManager>.GetInstance().Save();
    this.backScene();
  }

  public void IbtnClear()
  {
    if (this.IsPush)
      return;
    this.StartCoroutine(this.UpdateInfoAndScroll(this.cacheUnits, (PlayerMaterialUnit[]) null));
    this.ChangeState(Unit05468Menu.state.INIT);
  }

  public void IbtnChange()
  {
    if (this.IsPushAndSet())
      return;
    this.SupplyItems = ((IEnumerable<SupplyItem>) SupplyItem.Merge(((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>().AllSupplies()).ToList<PlayerItem>().ToArray(), ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>().AllBattleSupplies()).ToArray<PlayerItem>())).ToList<SupplyItem>();
    this.SaveDeck = this.SupplyItems.Copy();
    Quest00210aScene.ChangeScene(true, new Quest00210Menu.Param()
    {
      SupplyItems = this.SupplyItems,
      SaveDeck = this.SaveDeck,
      removeButton = false,
      limitedOnly = false,
      mode = Quest00210Scene.Mode.Earth
    });
  }

  private void DispMarker(int info_index, int unit_index, bool disp)
  {
    if (this.displayUnitInfos.Count > 0)
      this.displayUnitInfos[info_index].selectMarker = disp;
    if (this.allUnitInfos.Count > 0)
      this.allUnitInfos[info_index].selectMarker = disp;
    if (this.allUnitIcons.Count <= 0)
      return;
    this.allUnitIcons[unit_index].SelectMarker = disp;
  }

  private void SelectAnimation()
  {
    this.ChangeState(Unit05468Menu.state.WAIT);
    if (this.afterIconIndex == 0)
      this.DispMarker(this.beforeInfoIndex, this.beforeIconIndex, true);
    else
      this.DispMarker(this.afterInfoIndex, this.afterIconIndex, true);
  }

  private void Select()
  {
    if (this.afterIconIndex == 0)
      this.ChangeState(Unit05468Menu.state.WAIT);
    else
      this.ChangeState(Unit05468Menu.state.SWAP_ANIMATION);
  }

  private void PlaySwapAnimation()
  {
    this.ChangeState(Unit05468Menu.state.WAIT);
    UITweener[] iconTweens1 = this.GetIconTweens(this.beforeIconIndex, this.MARKER_FLASH_TWEEN);
    UITweener[] iconTweens2 = this.GetIconTweens(this.afterIconIndex, this.MARKER_FLASH_TWEEN);
    if (iconTweens1.Length == 0 || iconTweens2.Length == 0)
    {
      this.ChangeState(Unit05468Menu.state.SWAP);
    }
    else
    {
      iconTweens1[0].SetOnFinished((EventDelegate.Callback) (() => this.ChangeState(Unit05468Menu.state.SWAP)));
      foreach (UITweener uiTweener in iconTweens1)
      {
        uiTweener.ResetToBeginning();
        uiTweener.PlayForward();
      }
      foreach (UITweener uiTweener in iconTweens2)
      {
        uiTweener.ResetToBeginning();
        uiTweener.PlayForward();
      }
    }
  }

  private UITweener[] GetIconTweens(int iconIndex, int groupId)
  {
    return NGTween.findTweenersGroup(this.allUnitIcons[iconIndex].selectMarker, groupId, false);
  }

  private IEnumerator Swap()
  {
    Unit05468Menu unit05468Menu = this;
    unit05468Menu.ChangeState(Unit05468Menu.state.WAIT);
    int num = unit05468Menu.displayUnitInfos[unit05468Menu.beforeInfoIndex].playerUnit.id == unit05468Menu.allUnitIcons[unit05468Menu.beforeIconIndex].PlayerUnit.id ? 1 : 0;
    UnitIconInfo displayUnitInfo = unit05468Menu.displayUnitInfos[unit05468Menu.beforeInfoIndex];
    unit05468Menu.displayUnitInfos[unit05468Menu.beforeInfoIndex] = unit05468Menu.displayUnitInfos[unit05468Menu.afterInfoIndex];
    unit05468Menu.displayUnitInfos[unit05468Menu.afterInfoIndex] = displayUnitInfo;
    UnitIconInfo allUnitInfo = unit05468Menu.allUnitInfos[unit05468Menu.beforeInfoIndex];
    unit05468Menu.allUnitInfos[unit05468Menu.beforeInfoIndex] = unit05468Menu.allUnitInfos[unit05468Menu.afterInfoIndex];
    unit05468Menu.allUnitInfos[unit05468Menu.afterInfoIndex] = allUnitInfo;
    IEnumerator e;
    if (num != 0)
    {
      e = unit05468Menu.CreateUnitIcon(unit05468Menu.beforeInfoIndex, unit05468Menu.beforeIconIndex, (PlayerUnit) null);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    e = unit05468Menu.CreateUnitIcon(unit05468Menu.afterInfoIndex, unit05468Menu.afterIconIndex, (PlayerUnit) null);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
    unit05468Menu.ChangeState(Unit05468Menu.state.INIT);
  }

  private void InitSelectIndex()
  {
    this.ChangeState(Unit05468Menu.state.WAIT);
    this.DispMarker(this.beforeInfoIndex, this.beforeIconIndex, false);
    this.DispMarker(this.afterInfoIndex, this.afterIconIndex, false);
    this.beforeInfoIndex = 0;
    this.beforeIconIndex = 0;
    this.afterInfoIndex = 0;
    this.afterIconIndex = 0;
  }

  protected void DispIndex(UnitIconBase iconBase, int index)
  {
    bool isGorgeous = index == 1;
    iconBase.DispEarthUnitNumberIcon(index, isGorgeous, false);
  }

  protected void HiddenIndex(UnitIconBase iconBase) => iconBase.HiddenEarthUnitNumberIcon(false);

  private void ApplyIndex()
  {
    foreach (UnitIconInfo allUnitInfo in this.allUnitInfos)
    {
      int index = allUnitInfo.alignmentSequence + 1;
      Singleton<EarthDataManager>.GetInstance().SetCharacterIndex(allUnitInfo.playerUnit.id, index);
    }
  }

  public IEnumerator DispSupplyDeck(PlayerItem[] supplys)
  {
    IEnumerator e;
    for (int i = 0; i < Consts.GetInstance().ITEM_EXTEND_VALUE; ++i)
    {
      if (i < supplys.Length)
      {
        e = this.CreateSupplyIcon(supplys[i], i);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
      {
        e = this.CreateSupplyIcon((PlayerItem) null, i);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }

  private IEnumerator CreateSupplyIcon(PlayerItem supply, int pos)
  {
    ItemIcon itemIcon = ((Component) this.SuppleIconPositions[pos]).GetComponentInChildren<ItemIcon>();
    if (Object.op_Equality((Object) itemIcon, (Object) null))
      itemIcon = this.supplyIcon.Clone(this.SuppleIconPositions[pos]).GetComponent<ItemIcon>();
    if (supply == (PlayerItem) null)
    {
      itemIcon.SetEmpty(true);
    }
    else
    {
      itemIcon.SetEmpty(false);
      IEnumerator e = itemIcon.InitByPlayerItem(supply);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private enum state
  {
    INIT,
    SELECT_ANIMATION,
    SELECT,
    SWAP_ANIMATION,
    SWAP,
    WAIT,
  }
}
