// Decompiled with JetBrains decompiler
// Type: Sea030PopUpGiftMixer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Sea030PopUpGiftMixer : BackButtonMenuBase
{
  [SerializeField]
  private GameObject dynMainGift;
  [SerializeField]
  private UILabel txtPledgeName;
  [SerializeField]
  private GameObject[] dynMaterialBugu;
  [SerializeField]
  private GameObject[] txtMaterialNum;
  [SerializeField]
  private GameObject[] slcMaterialNum_txt;
  [SerializeField]
  private GameObject dynMissMaterial;
  [SerializeField]
  private UILabel txtMissNum;
  [SerializeField]
  private UILabel txtMissName;
  [SerializeField]
  private UILabel txtPercentAmount;
  private Sea030GiftDetails giftDetailsMenu;
  private Sea030GiftDetailsScrollList.GiftMixerInfo giftMixerInfo;
  private long playerZenyAmount;
  private GameObject itemIconPrefab;
  [Header("Slider")]
  [SerializeField]
  private UILabel txtCount;
  [SerializeField]
  private UILabel txtCost;
  [SerializeField]
  private UILabel txtSelectMin;
  [SerializeField]
  private UILabel txtSelectMax;
  [SerializeField]
  private UISlider slider;
  [SerializeField]
  private UIButton[] sliderButtons;
  private int maxCount;
  private int selectedCount = 1;
  private int sliderCount = 1;
  [Header("Button")]
  public SpreadColorButton yesButton;

  public IEnumerator Init(
    Sea030GiftDetailsScrollList.GiftMixerInfo gift_mixierInfo,
    long zeny,
    GameObject item_iconPrefab,
    Sea030GiftDetailsScrollContainer scrollContainer)
  {
    ((Collider) ((Component) this.slider).GetComponent<BoxCollider>()).enabled = false;
    ((Behaviour) this.slider).enabled = false;
    this.giftDetailsMenu = scrollContainer.giftDetailsMenu;
    this.giftDetailsMenu.Hide();
    this.giftMixerInfo = gift_mixierInfo;
    this.playerZenyAmount = zeny;
    this.itemIconPrefab = item_iconPrefab;
    IEnumerator e = this.SetCallItemIcon(this.giftMixerInfo.successGear, this.itemIconPrefab, this.dynMainGift.transform, this.giftMixerInfo.playerSuccessGearCount);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.txtPercentAmount.SetTextLocalize(this.giftMixerInfo.successRatio);
    this.txtPledgeName.SetTextLocalize(this.giftMixerInfo.successGear.name);
    int index = 0;
    int materialMaxCount = 0;
    foreach (Sea030GiftDetailsScrollList.RecipeMaterialInfo material in this.giftMixerInfo.materialGear)
    {
      this.txtMaterialNum[index].SetActive(true);
      this.slcMaterialNum_txt[index].SetActive(true);
      this.txtMaterialNum[index].GetComponent<UILabel>().SetTextLocalize(material.neadQuantity);
      e = this.SetCallItemIcon(material.gear, this.itemIconPrefab, this.dynMaterialBugu[index].transform, material.playerQuantity);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      int num = material.playerQuantity / material.neadQuantity;
      if (index == 0)
        materialMaxCount = num;
      else if (materialMaxCount > num)
        materialMaxCount = num;
      ++index;
    }
    if (this.giftMixerInfo.failureGear != null)
    {
      e = this.SetCallItemIcon(this.giftMixerInfo.failureGear, this.itemIconPrefab, this.dynMissMaterial.transform, this.giftMixerInfo.playerFailureGearCount);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.txtMissName.SetTextLocalize(this.giftMixerInfo.failureGear.name);
      this.txtMissNum.SetTextLocalize(this.giftMixerInfo.failureGearGiveCount);
    }
    this.maxCount = Mathf.Min(Mathf.Min(materialMaxCount, (int) Mathf.Min((float) (this.playerZenyAmount / this.giftMixerInfo.zennyAmount), 99f)), 99);
    if (this.maxCount <= 1)
    {
      ((UIProgressBar) this.slider).numberOfSteps = this.maxCount + 1;
      ((Behaviour) this.slider).enabled = false;
      ((Collider) ((Component) this.slider).GetComponent<BoxCollider>()).enabled = false;
      this.txtSelectMin.text = "0";
      foreach (UIButtonColor sliderButton in this.sliderButtons)
        sliderButton.isEnabled = false;
      this.selectedCount = 1;
      this.sliderCount = 1;
    }
    else
    {
      ((Collider) ((Component) this.slider).GetComponent<BoxCollider>()).enabled = true;
      ((Behaviour) this.slider).enabled = true;
      ((UIProgressBar) this.slider).numberOfSteps = this.maxCount;
      this.txtSelectMin.text = "1";
      this.selectedCount = 1;
      this.sliderCount = this.selectedCount - 1;
    }
    this.UpdateInfo();
    this.txtCost.SetTextLocalize((this.giftMixerInfo.zennyAmount * (long) this.selectedCount).ToString());
    this.txtSelectMax.SetTextLocalize(this.maxCount.ToString());
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

  public void OnValueChange()
  {
    this.sliderCount = Mathf.RoundToInt(((UIProgressBar) this.slider).value * ((float) this.maxCount - 1f));
    this.UpdateInfo();
  }

  private void UpdateInfo()
  {
    this.selectedCount = this.maxCount != 1 ? this.sliderCount + 1 : 1;
    ((UIButtonColor) this.yesButton).isEnabled = this.selectedCount > 0;
    this.txtCount.SetTextLocalize(this.selectedCount);
    ((UIProgressBar) this.slider).value = (float) this.sliderCount / ((float) this.maxCount - 1f);
    int index = 0;
    foreach (Sea030GiftDetailsScrollList.RecipeMaterialInfo recipeMaterialInfo in this.giftMixerInfo.materialGear)
    {
      this.txtMaterialNum[index].GetComponent<UILabel>().SetTextLocalize(recipeMaterialInfo.neadQuantity * this.selectedCount);
      ++index;
    }
    this.txtCost.SetTextLocalize((this.giftMixerInfo.zennyAmount * (long) this.selectedCount).ToString());
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
    if (this.sliderCount >= this.maxCount - 1)
      this.sliderCount = this.maxCount - 1;
    this.UpdateInfo();
  }

  public void IbtnSetMin()
  {
    this.sliderCount = 0;
    this.UpdateInfo();
  }

  public void IbtnSetMax()
  {
    this.sliderCount = this.maxCount - 1;
    this.UpdateInfo();
  }

  public void IbtnYes()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(Sea030PopUpGiftMixer.CreateCallItem(this.selectedCount, this.giftMixerInfo.recipeID, this.giftMixerInfo.successGear, this.giftMixerInfo.failureGear, this.itemIconPrefab, this.giftMixerInfo.parentGear, this.giftDetailsMenu));
    this.IbtnNo();
  }

  public static IEnumerator CreateCallItem(
    int quantity,
    int recipe_id,
    GearGear success_gear,
    GearGear failure_gear,
    GameObject iconPrefab,
    GearGear parent_gear,
    Sea030GiftDetails gift_details_menu)
  {
    Singleton<PopupManager>.GetInstance().closeAll(true);
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
    yield return (object) null;
    Future<WebAPI.Response.ItemGearCreateCallGift> handler = WebAPI.ItemGearCreateCallGift(quantity, recipe_id);
    IEnumerator e = handler.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    WebAPI.Response.ItemGearCreateCallGift result = handler.Result;
    if (result == null)
    {
      Singleton<PopupManager>.GetInstance().closeAll();
      yield return (object) null;
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    }
    else
    {
      Future<GameObject> prefab = new ResourceObject("Prefabs/popup/popup_sea030_SuccessGift").Load<GameObject>();
      e = prefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = Singleton<PopupManager>.GetInstance().open(prefab.Result).GetComponent<Sea030PopUpSuccessGift>().Init(success_gear, failure_gear, result.success_call_gift_count, result.failure_call_gift_count, iconPrefab, parent_gear, gift_details_menu);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Sea030GiftListMenu.isCreateCallItem = true;
      yield return (object) null;
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      prefab = (Future<GameObject>) null;
    }
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.giftDetailsMenu.ShowWindow(this.giftMixerInfo.parentGear, false));
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();
}
