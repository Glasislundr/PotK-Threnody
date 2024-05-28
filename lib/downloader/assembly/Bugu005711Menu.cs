// Decompiled with JetBrains decompiler
// Type: Bugu005711Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Bugu005711Menu : BackButtonMenuBase
{
  [SerializeField]
  private UISlider slider;
  [SerializeField]
  private UILabel TxtItemName;
  [SerializeField]
  private UILabel TxtItemDescription;
  [SerializeField]
  private UILabel TxtPossessionNum;
  [SerializeField]
  private UILabel TxtSellValue;
  [SerializeField]
  private UILabel TxtSellNum;
  [SerializeField]
  private UILabel TxtTotalSellValue;
  [SerializeField]
  private UILabel TxtSelectMax;
  [SerializeField]
  private UI2DSprite LinkItem;
  private Bugu00525Menu menu;
  private long itemZenny;
  private int itemCount;
  private int totalItem;
  private long totalZenny;
  private InventoryItem item;

  public void IbtnPopupOk()
  {
    if (this.IsPushAndSet())
      return;
    this.menu.SetSellCount(this.item, this.itemCount);
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();

  public IEnumerator InitSceneAsync(InventoryItem item, Bugu00525Menu menu)
  {
    yield return (object) Bugu005711Menu.InitIcon(item, ((Component) this.LinkItem).transform.parent.Find("dir_thum"));
    ((Behaviour) this.LinkItem).enabled = false;
    this.item = item;
    this.menu = menu;
    this.Set(item);
  }

  public static IEnumerator InitIcon(InventoryItem item, Transform dir_thum)
  {
    Future<GameObject> itemIconPrefab = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    IEnumerator e = itemIconPrefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject itemIconObj = Object.Instantiate<GameObject>(itemIconPrefab.Result, dir_thum);
    ItemIcon itemIcon = itemIconObj.GetComponent<ItemIcon>();
    e = itemIcon.InitByItemInfo(item.Item);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    itemIcon.EnableQuantity(0);
    if (item.Item.isSupply)
      itemIcon.BottomModeValue = ItemIcon.BottomMode.Nothing;
    ((Collider) ((Component) itemIcon.supply.button).GetComponent<BoxCollider>()).enabled = false;
    itemIconObj.gameObject.SetActive(true);
  }

  public void Set(InventoryItem item)
  {
    this.totalItem = item.Item.quantity < this.menu.SelectMax ? item.Item.quantity : this.menu.SelectMax;
    this.itemZenny = item.GetSingleSellPrice();
    this.TxtItemName.SetText(item.GetName());
    this.TxtItemDescription.SetText(item.GetDescription());
    this.TxtPossessionNum.SetTextLocalize(item.Item.quantity);
    this.TxtSellNum.SetTextLocalize(this.totalItem);
    this.TxtSelectMax.SetTextLocalize(this.totalItem);
    this.TxtSellValue.SetTextLocalize(this.itemZenny);
    this.TxtTotalSellValue.SetTextLocalize((long) this.totalItem * this.itemZenny);
    ((UIProgressBar) this.slider).value = 1f;
  }

  protected override void Update()
  {
    this.itemCount = (int) ((double) ((UIProgressBar) this.slider).value * (double) this.totalItem);
    if (this.totalItem <= 1 && (double) ((UIProgressBar) this.slider).value < 1.0)
    {
      if ((double) ((UIProgressBar) this.slider).value >= 0.0099999997764825821)
        this.itemCount = 1;
    }
    else if (this.itemCount > this.totalItem)
    {
      this.itemCount = this.totalItem;
      ((UIProgressBar) this.slider).value = (float) this.itemCount / (float) this.totalItem;
    }
    this.TxtSellNum.SetTextLocalize(this.itemCount);
    this.totalZenny = this.itemZenny * (long) this.itemCount;
    this.TxtTotalSellValue.SetTextLocalize(this.totalZenny);
  }
}
