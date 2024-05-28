// Decompiled with JetBrains decompiler
// Type: Bugu0052Menu
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
public class Bugu0052Menu : Bugu005ItemListMenuBase
{
  [SerializeField]
  private UILabel TxtNumberPattern1;
  private bool needClearCache = true;

  public override Persist<Persist.ItemSortAndFilterInfo> GetPersist()
  {
    return Persist.bugu0052SortAndFilter;
  }

  protected override List<PlayerItem> GetItemList()
  {
    return ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.isWeapon())).ToList<PlayerItem>();
  }

  protected override long GetRevisionItemList() => SMManager.Revision<PlayerItem[]>();

  protected override void BottomInfoUpdate()
  {
    Player player = SMManager.Get<Player>();
    this.TxtNumberPattern1.SetTextLocalize(Consts.Format(Consts.GetInstance().GEAR_0052_POSSESSION, (IDictionary) new Hashtable()
    {
      {
        (object) "now",
        (object) this.InventoryItems.Count<InventoryItem>()
      },
      {
        (object) "max",
        (object) player.max_items
      }
    }));
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

  public void IbtnWeaponStorage()
  {
    if (this.IsPushAndSet())
      return;
    Bugu005WeaponStorageScene.ChangeScene(false);
  }

  public void IbtnSell()
  {
    if (this.IsPushAndSet())
      return;
    this.needClearCache = false;
    Bugu00525Scene.ChangeScene(true, Bugu00525Scene.Mode.Weapon);
  }

  public void IbtnConversion()
  {
    if (this.IsPushAndSet())
      return;
    this.needClearCache = false;
    Bugu005WeaponMaterialConversionScene.ChangeScene(true, Bugu005WeaponMaterialConversionScene.Mode.Weapon);
  }
}
