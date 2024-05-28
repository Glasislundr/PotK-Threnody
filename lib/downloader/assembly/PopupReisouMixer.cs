// Decompiled with JetBrains decompiler
// Type: PopupReisouMixer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PopupReisouMixer : BackButtonMenuBase
{
  [Header("Icon")]
  [SerializeField]
  private GameObject dynThum;
  private ItemIcon itemIcon;
  [Header("Label")]
  [SerializeField]
  private UILabel txtName;
  [SerializeField]
  private UILabel txtFlavor;
  [SerializeField]
  private UILabel txtSelectNumValue;
  [SerializeField]
  private UILabel txtZenyCostValue;
  [SerializeField]
  private UILabel txtAcquisitionsValue;
  [Header("Slider")]
  [SerializeField]
  private UILabel txtSelectMin;
  [SerializeField]
  private UILabel txtSelectMax;
  [SerializeField]
  private UISlider slider;
  private int maxCount;
  private int sliderCount = 1;
  private int zenyAmount;
  private int getReisouJewel;
  [Header("Button")]
  public SpreadColorButton btnOK;
  private InventoryItem selectItem;
  private Action<InventoryItem, int> cbOK;

  public IEnumerator Init(
    InventoryItem selectItem,
    int totalCost,
    int maxCountLimit,
    int selectedCount,
    Action<InventoryItem, int> cbOK)
  {
    this.selectItem = selectItem;
    this.cbOK = cbOK;
    GameCore.ItemInfo item = selectItem.Item;
    ((Collider) ((Component) this.slider).GetComponent<BoxCollider>()).enabled = false;
    ((Behaviour) this.slider).enabled = false;
    List<InventoryItem> inventoryItemList = new List<InventoryItem>();
    this.zenyAmount = CalcItemCost.GetReisouMixingCostSingle(item);
    this.getReisouJewel = item.gear.rarity.combine_reisou_jewel;
    Future<GameObject> prefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabF.Result;
    this.maxCount = maxCountLimit;
    if (this.maxCount > item.quantity)
      this.maxCount = item.quantity;
    int num = totalCost - this.zenyAmount * selectedCount;
    long pMoney = SMManager.Get<Player>().money - (long) num;
    while (pMoney < (long) (this.maxCount * this.zenyAmount))
      --this.maxCount;
    this.itemIcon = result.Clone(this.dynThum.transform).GetComponent<ItemIcon>();
    e = this.itemIcon.InitByItemInfo(item);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.itemIcon.SetRenseiIcon();
    this.itemIcon.SetRenseiMaterialCount(item.quantity);
    ((Collider) ((Component) this.slider).GetComponent<BoxCollider>()).enabled = true;
    ((Behaviour) this.slider).enabled = true;
    ((UIProgressBar) this.slider).numberOfSteps = this.maxCount + 1;
    this.txtSelectMin.text = "0";
    if (selectedCount == 0)
      selectedCount = 1;
    this.sliderCount = selectedCount;
    this.UpdateInfo();
    this.txtName.SetTextLocalize(item.gear.name);
    this.txtFlavor.SetTextLocalize(item.gear.description);
    string format = "[ff0000]{0}[-]";
    if (pMoney < (long) (this.zenyAmount * this.sliderCount))
      this.txtZenyCostValue.SetTextLocalize(format.F((object) (this.zenyAmount * this.sliderCount)));
    else
      this.txtZenyCostValue.SetTextLocalize(this.zenyAmount * this.sliderCount);
    this.txtSelectMax.SetTextLocalize(this.maxCount.ToString());
  }

  public void OnValueChange()
  {
    this.sliderCount = Mathf.RoundToInt(((UIProgressBar) this.slider).value * (float) this.maxCount);
    this.UpdateInfo();
  }

  private void UpdateInfo()
  {
    this.itemIcon.SetRenseiMaterialNum(this.sliderCount);
    this.txtSelectNumValue.SetTextLocalize(this.sliderCount.ToString());
    ((UIProgressBar) this.slider).value = (float) this.sliderCount / (float) this.maxCount;
    this.txtZenyCostValue.SetTextLocalize(this.zenyAmount * this.sliderCount);
    this.txtAcquisitionsValue.SetTextLocalize(this.getReisouJewel * this.sliderCount);
  }

  public void IbtnDecrease()
  {
    --this.sliderCount;
    if (this.sliderCount <= 0)
      this.sliderCount = 0;
    this.UpdateInfo();
  }

  public void IbtnIncrease()
  {
    ++this.sliderCount;
    if (this.sliderCount >= this.maxCount)
      this.sliderCount = this.maxCount;
    this.UpdateInfo();
  }

  public void IbtnSetMin()
  {
    this.sliderCount = 0;
    this.UpdateInfo();
  }

  public void IbtnSetMax()
  {
    this.sliderCount = this.maxCount;
    this.UpdateInfo();
  }

  public void IbtnOK()
  {
    if (this.IsPushAndSet())
      return;
    Action<InventoryItem, int> cbOk = this.cbOK;
    if (cbOk != null)
      cbOk(this.selectItem, this.sliderCount);
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnBack();
}
