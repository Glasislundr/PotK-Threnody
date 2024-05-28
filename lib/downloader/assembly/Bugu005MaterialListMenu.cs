// Decompiled with JetBrains decompiler
// Type: Bugu005MaterialListMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Bugu005MaterialListMenu : Bugu005ItemListMenuBase
{
  private bool needClearCache = true;
  private bool isIngotSold_;
  private bool isEndPopupSellIngot_;
  private bool showPopupAutoSell = true;

  public override Persist<Persist.ItemSortAndFilterInfo> GetPersist()
  {
    return Persist.bugu005MaterialListSortAndFilter;
  }

  protected override List<PlayerMaterialGear> GetMaterialList()
  {
    return ((IEnumerable<PlayerMaterialGear>) SMManager.Get<PlayerMaterialGear[]>()).Where<PlayerMaterialGear>((Func<PlayerMaterialGear, bool>) (x => !x.isWeapon() && !x.isSupply() && !x.isWeaponMaterial())).ToList<PlayerMaterialGear>();
  }

  public override IEnumerator Init()
  {
    yield return (object) base.Init();
  }

  public IEnumerator GoldSell()
  {
    Bugu005MaterialListMenu materialListMenu = this;
    if (materialListMenu.showPopupAutoSell)
    {
      materialListMenu.showPopupAutoSell = false;
      materialListMenu.isIngotSold_ = false;
      materialListMenu.isEndPopupSellIngot_ = false;
      yield return (object) materialListMenu.StartCoroutine(Popup0071SellIngotMenu.show(new Action<bool>(materialListMenu.onIngotSellResult), actionReload: new Action(materialListMenu.Reload)));
    }
  }

  private void onIngotSellResult(bool bSold)
  {
    this.isIngotSold_ = bSold;
    this.isEndPopupSellIngot_ = true;
  }

  private void Reload() => this.StartCoroutine(this.ReloadList());

  private IEnumerator ReloadList()
  {
    Bugu005MaterialListMenu materialListMenu = this;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    ItemIcon.ClearCache();
    materialListMenu.UpdateInventoryItemList();
    IEnumerator e = materialListMenu.Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  protected virtual void OnEnable()
  {
    if (!this.scroll.scrollView.isDragging)
      return;
    this.scroll.scrollView.Press(false);
  }

  public void onBackScene()
  {
    if (Object.op_Inequality((Object) this.SortPopupPrefab, (Object) null))
      this.SortPopupPrefab.GetComponent<ItemSortAndFilter>().Initialize((Bugu005ItemListMenuBase) this);
    float num = this.scroll.scrollView.verticalScrollBar.value;
    this.Sort(this.SortCategory, this.OrderBuySort, this.isEquipFirst);
    this.scroll.ResolvePosition(new Vector2(0.0f, num));
    this.needClearCache = true;
  }

  public override void onEndScene()
  {
    base.onEndScene();
    Persist.sortOrder.Flush();
    if (!this.needClearCache)
      return;
    ItemIcon.ClearCache();
  }

  public void IbtnSell()
  {
    if (this.IsPushAndSet())
      return;
    this.needClearCache = false;
    Bugu00525Scene.ChangeScene(true, Bugu00525Scene.Mode.Material);
  }
}
