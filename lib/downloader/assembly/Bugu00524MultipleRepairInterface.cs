// Decompiled with JetBrains decompiler
// Type: Bugu00524MultipleRepairInterface
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Bugu00524MultipleRepairInterface : MonoBehaviour
{
  private const float ICON_SCALE = 0.8f;
  private const int BED_MAX_PERCENT = 90;
  private const int BED_UP_PERCENT = 10;
  private const int MEDAL_PRICE = 10;
  [SerializeField]
  private Transform[] repairTargetIcon;
  [SerializeField]
  private UILabel[] txtRepairPercent;
  [SerializeField]
  private UILabel txtZeny;
  [SerializeField]
  private UIButton ibtnRepair;
  [SerializeField]
  private GameObject repairCaution;
  [SerializeField]
  private UIButton ibtnMedalRepair;
  [SerializeField]
  private UISprite[] spriteRequestMedal;
  private Bugu00524Menu menuInstance;
  [SerializeField]
  private UIButton ibtnPlus;
  [SerializeField]
  private UIButton ibtnMinus;
  private ItemIcon[] icons;
  private int repairPrice;
  private int repairMedal;
  private int zenyBoostCnt;

  public void Init(Bugu00524Menu menu)
  {
    this.menuInstance = menu;
    GameObject itemIconPrefab = this.menuInstance.GetItemIconPrefab();
    int length = this.repairTargetIcon.Length;
    this.icons = new ItemIcon[length];
    for (int index = 0; index < length; ++index)
    {
      this.repairTargetIcon[index].Clear();
      GameObject gameObject = itemIconPrefab.Clone(this.repairTargetIcon[index]);
      gameObject.transform.localScale = new Vector3(0.8f, 0.8f);
      ItemIcon component = gameObject.GetComponent<ItemIcon>();
      this.icons[index] = component;
    }
    this.SetSelectItem((List<InventoryItem>) null);
  }

  public void SetSelectItem(List<InventoryItem> items)
  {
    Player player = SMManager.Get<Player>();
    this.repairPrice = 0;
    this.repairMedal = 0;
    int count = items != null ? items.Count : 0;
    bool flag1 = false;
    foreach (Component component in this.spriteRequestMedal)
      component.gameObject.SetActive(false);
    this.repairMedal = 10 * count;
    ((UIButtonColor) this.ibtnMedalRepair).isEnabled = player.medal >= this.repairMedal && count > 0;
    if (count > 0)
    {
      if (this.spriteRequestMedal.Length >= count)
        ((Component) this.spriteRequestMedal[count - 1]).gameObject.SetActive(true);
    }
    else
      this.zenyBoostCnt = 0;
    for (int index = 0; index < this.menuInstance.SelectMax; ++index)
    {
      if (index < count)
      {
        this.icons[index].SetEmpty(false);
        if (ItemIcon.IsCache(items[index].Item))
          this.icons[index].InitByItemInfoCache(items[index].Item);
        else
          this.StartCoroutine(this.icons[index].InitByItemInfo(items[index].Item));
        this.icons[index].onClick = (Action<ItemIcon>) (playeritem => { });
        this.icons[index].Gray = false;
        this.icons[index].Deselect();
        this.icons[index].HideCounter();
        flag1 |= items[index].Item.favorite;
        int num = (int) Math.Round((double) items[index].Item.gear.repair_success_ratio * 100.0) + 10 * this.zenyBoostCnt;
        this.txtRepairPercent[index].SetTextLocalize(num.ToString() + Consts.GetInstance().PERCENT);
        this.repairPrice += items[index].GetRepairPrice(this.zenyBoostCnt);
      }
      else
      {
        this.icons[index].SetEmpty(true);
        this.icons[index].onClick = (Action<ItemIcon>) (playeritem => { });
        this.txtRepairPercent[index].SetTextLocalize(Consts.GetInstance().GEAR_0052_REPAIR_DEFAULT_PROBABILITY_TEXT);
      }
    }
    this.txtZeny.SetTextLocalize(this.repairPrice);
    ((UIWidget) this.txtZeny).color = player.money >= (long) this.repairPrice ? Color.white : Color.red;
    ((UIButtonColor) this.ibtnRepair).isEnabled = count > 0 && player.money >= (long) this.repairPrice && !flag1;
    bool flag2 = !flag1 && count > 0;
    if (Object.op_Inequality((Object) this.ibtnPlus, (Object) null))
      ((UIButtonColor) this.ibtnPlus).isEnabled = flag2;
    if (Object.op_Inequality((Object) this.ibtnMinus, (Object) null))
      ((UIButtonColor) this.ibtnMinus).isEnabled = flag2;
    if (!Object.op_Inequality((Object) this.repairCaution, (Object) null))
      return;
    this.repairCaution.SetActive(flag1);
  }

  public void onIbtnMinus()
  {
    if (this.zenyBoostCnt == 0)
      return;
    --this.zenyBoostCnt;
    this.SetSelectItem(this.menuInstance.GetSelectItem());
  }

  public void onIbtnPlus()
  {
    if (this.menuInstance.GetSelectItem() == null || this.menuInstance.GetSelectItem().Count == 0 || (int) Math.Round((double) this.menuInstance.GetSelectItem()[0].Item.gear.repair_success_ratio * 100.0) + 10 * (this.zenyBoostCnt + 1) > 90)
      return;
    ++this.zenyBoostCnt;
    this.SetSelectItem(this.menuInstance.GetSelectItem());
  }

  public void onIbtnClear() => this.menuInstance.ClearSelectItem();

  public void onIbtnRepair() => this.menuInstance.ibtnRepair(this.repairPrice, this.zenyBoostCnt);

  public void onIbtnRepairRareMedal() => this.menuInstance.ibtnRepairRareMedal(this.repairMedal);

  public void onIbtnIcon1()
  {
    this.menuInstance.RemoveSelectItem(1);
    this.menuInstance.UpdateSelectItemIndexWithInfo();
  }

  public void onIbtnIcon2()
  {
    this.menuInstance.RemoveSelectItem(2);
    this.menuInstance.UpdateSelectItemIndexWithInfo();
  }

  public void onIbtnIcon3()
  {
    this.menuInstance.RemoveSelectItem(3);
    this.menuInstance.UpdateSelectItemIndexWithInfo();
  }

  public void onIbtnIcon4()
  {
    this.menuInstance.RemoveSelectItem(4);
    this.menuInstance.UpdateSelectItemIndexWithInfo();
  }

  public void onIbtnIcon5()
  {
    this.menuInstance.RemoveSelectItem(5);
    this.menuInstance.UpdateSelectItemIndexWithInfo();
  }
}
