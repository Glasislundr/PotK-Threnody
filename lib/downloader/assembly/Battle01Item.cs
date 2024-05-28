// Decompiled with JetBrains decompiler
// Type: Battle01Item
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Battle01Item : NGBattleMenuBase
{
  [SerializeField]
  protected UI2DSprite item;
  [SerializeField]
  protected UILabel txt_Item_name;
  [SerializeField]
  protected UILabel txt_Item_amount;
  private BL.BattleModified<BL.Item> modified;
  private ItemIcon itemIcon;

  private void Awake() => ((Behaviour) this.item).enabled = false;

  public override IEnumerator onInitAsync()
  {
    Battle01Item battle01Item = this;
    Future<GameObject> f = !battle01Item.battleManager.isSea ? Res.Prefabs.ItemIcon.prefab.Load<GameObject>() : Res.Prefabs.Sea.ItemIcon.prefab_sea.Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = f.Result;
    battle01Item.itemIcon = result.CloneAndGetComponent<ItemIcon>(((Component) battle01Item.item).gameObject.transform);
    NGUITools.AdjustDepth(((Component) battle01Item.itemIcon).gameObject, ((UIWidget) battle01Item.item).depth);
    battle01Item.itemIcon.SetBasedOnHeight(((UIWidget) battle01Item.item).height);
  }

  private IEnumerator doSetIcon(SupplySupply supply)
  {
    IEnumerator e = this.itemIcon.InitBySupply(supply);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.itemIcon.BottomModeValue = ItemIcon.BottomMode.Nothing;
    this.itemIcon.QuantitySupply = false;
    this.itemIcon.isButtonActive = false;
    this.itemIcon.isBackActive = false;
  }

  protected override void LateUpdate_Battle()
  {
    if (this.modified == null || !this.modified.isChangedOnce())
      return;
    BL.Item obj = this.modified.value;
    this.StartCoroutine(this.doSetIcon(obj.item));
    this.setText(this.txt_Item_name, obj.item.name);
    this.setText(this.txt_Item_amount, "×" + (object) obj.amount);
  }

  public void setItem(BL.Item item) => this.modified = BL.Observe<BL.Item>(item);

  public BL.Item getItem() => this.modified.value;
}
