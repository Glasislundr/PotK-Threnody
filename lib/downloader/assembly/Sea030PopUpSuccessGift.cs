// Decompiled with JetBrains decompiler
// Type: Sea030PopUpSuccessGift
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Sea030PopUpSuccessGift : BackButtonMenuBase
{
  [SerializeField]
  private UILabel txtSuccessGearName;
  [SerializeField]
  private UILabel txtFailureGearName;
  [SerializeField]
  private UILabel txtSuccessGearCount;
  [SerializeField]
  private UILabel txtFailureGearCount;
  [SerializeField]
  private GameObject dynSuccessGiftIcon;
  [SerializeField]
  private GameObject dynFailureGiftIcon;
  [SerializeField]
  private GameObject dirTitle;
  private GearGear parentGear;
  private Sea030GiftDetails giftDetails;

  public IEnumerator Init(
    GearGear success_gear,
    GearGear failure_gear,
    int success_gear_count,
    int failure_gear_count,
    GameObject itemIconPrefab,
    GearGear parent_gear,
    Sea030GiftDetails giftDetailsMenu)
  {
    this.txtSuccessGearName.SetTextLocalize(success_gear.name);
    this.txtSuccessGearCount.SetTextLocalize(success_gear_count);
    if (failure_gear != null)
    {
      this.txtFailureGearName.SetTextLocalize(failure_gear.name);
      this.txtFailureGearCount.SetTextLocalize(failure_gear_count);
    }
    else
    {
      this.txtFailureGearName.SetTextLocalize("統合失敗アイテム");
      this.txtFailureGearCount.SetTextLocalize(0);
    }
    this.parentGear = parent_gear;
    this.giftDetails = giftDetailsMenu;
    IEnumerator e;
    if (success_gear_count > 0)
    {
      e = this.SetCallItemIcon(success_gear, itemIconPrefab, this.dynSuccessGiftIcon.transform, 0);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    if (failure_gear_count > 0)
    {
      e = this.SetCallItemIcon(failure_gear, itemIconPrefab, this.dynFailureGiftIcon.transform, 0);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    if (failure_gear_count == 0 || success_gear_count == 0)
    {
      float num = (float) (((double) this.dynSuccessGiftIcon.transform.position.x + (double) this.dynFailureGiftIcon.transform.position.x) / 2.0);
      Vector3 position1 = this.dynSuccessGiftIcon.transform.position;
      Vector3 position2 = this.dynFailureGiftIcon.transform.position;
      position1.x = num;
      position2.x = num;
      this.dynSuccessGiftIcon.transform.position = position1;
      this.dynFailureGiftIcon.transform.position = position2;
    }
    if (success_gear_count == 0)
      this.dirTitle.SetActive(false);
    yield return (object) null;
  }

  private IEnumerator SetCallItemIcon(
    GearGear gear,
    GameObject ItemIconPrefab,
    Transform iconTransform,
    int quantity)
  {
    ItemIcon gearIcon = ItemIconPrefab.Clone(iconTransform).GetComponent<ItemIcon>();
    IEnumerator e = gearIcon.InitByGear(gear, gear.GetElement());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    gearIcon.gear.rank.SetActive(false);
    gearIcon.gear.unlimit.SetActive(false);
    gearIcon.Gray = false;
    gearIcon.EnableQuantity(quantity);
  }

  public void IbtnOk()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.giftDetails.ShowWindow(this.parentGear, false));
    Singleton<PopupManager>.GetInstance().closeAll();
  }

  public override void onBackButton() => this.IbtnOk();
}
