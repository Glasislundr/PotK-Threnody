// Decompiled with JetBrains decompiler
// Type: Guild02862Menu
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
public class Guild02862Menu : BackButtonMenuBase
{
  private const int ICON_WIDGET_SIZE_X = 112;
  private const int ICON_WIDGET_SIZE_Y = 121;
  private GameObject itemDetailPopup;
  [SerializeField]
  private NGxScroll2 scroll;
  private GameObject giftIcon;
  private List<Guild02862Menu.GiftIconInfo> allGiftItemInfo;
  private List<GameObject> showGiftList;
  private float scrool_start_y;
  private bool isInitialize;

  public IEnumerator InitializeAsync()
  {
    Guild02862Menu guild02862Menu = this;
    guild02862Menu.allGiftItemInfo = new List<Guild02862Menu.GiftIconInfo>();
    guild02862Menu.showGiftList = new List<GameObject>();
    guild02862Menu.allGiftItemInfo.Clear();
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    // ISSUE: reference to a compiler-generated method
    Future<WebAPI.Response.GuildGiftGetWishListMaster> ft = WebAPI.GuildGiftGetWishListMaster(true, new Action<WebAPI.Response.UserError>(guild02862Menu.\u003CInitializeAsync\u003Eb__9_0));
    IEnumerator e = ft.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (ft.Result != null)
    {
      GuildGift[] guildGiftArray = SMManager.Get<GuildGift[]>();
      for (int index = 0; index < guildGiftArray.Length; ++index)
      {
        if (guildGiftArray != null)
          guild02862Menu.allGiftItemInfo.Add(new Guild02862Menu.GiftIconInfo()
          {
            gift = guildGiftArray[index]
          });
      }
      e = guild02862Menu.ResourceLoad();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = guild02862Menu.InitGiftList();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    }
  }

  public void Initialize()
  {
  }

  private IEnumerator ResourceLoad()
  {
    Future<GameObject> fgObj;
    IEnumerator e;
    if (Object.op_Equality((Object) this.giftIcon, (Object) null))
    {
      fgObj = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.giftIcon = fgObj.Result;
      fgObj = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.itemDetailPopup, (Object) null))
    {
      fgObj = Res.Prefabs.popup.popup_000_7_4_2__anim_popup01.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.itemDetailPopup = fgObj.Result;
      fgObj = (Future<GameObject>) null;
    }
  }

  private void CreateItemIcon(int info_index, int item_index)
  {
    this.StartCoroutine(this.loadThumb(this.allGiftItemInfo[info_index], this.showGiftList[item_index]));
  }

  private IEnumerator CreateItemIconForInitialize(int info_index, int item_index)
  {
    if (info_index < this.allGiftItemInfo.Count || item_index < this.showGiftList.Count)
    {
      IEnumerator e = this.loadThumb(this.allGiftItemInfo[info_index], this.showGiftList[item_index]);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private IEnumerator loadThumb(Guild02862Menu.GiftIconInfo itemIcon, GameObject parent)
  {
    int rewardQuantity = itemIcon.gift.reward_quantity;
    int rewardTypeID = itemIcon.gift.reward_type_id;
    int rewardId = itemIcon.gift.reward_id;
    CreateIconObject target = parent.GetOrAddComponent<CreateIconObject>();
    IEnumerator e = target.CreateThumbnail((MasterDataTable.CommonRewardType) rewardTypeID, rewardId, rewardQuantity, isButton: false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject icon = target.GetIcon();
    LongPressButton component1 = parent.GetComponent<LongPressButton>();
    switch (rewardTypeID)
    {
      case 1:
      case 24:
        ((Collider) ((Component) component1).GetComponent<BoxCollider>()).enabled = false;
        UnitIcon component2 = icon.GetComponent<UnitIcon>();
        if (!Object.op_Equality((Object) component2, (Object) null))
        {
          component2.setLevelText("1");
          component2.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
          ((Collider) component2.buttonBoxCollider).enabled = false;
          break;
        }
        break;
      case 2:
        ItemIcon component3 = icon.GetComponent<ItemIcon>();
        if (Object.op_Inequality((Object) component3, (Object) null))
        {
          component3.QuantitySupply = true;
          component3.EnableQuantity(rewardQuantity);
          component3.ReleaseClickEvent();
          this.initButtonEvent(component1, itemIcon, icon);
          break;
        }
        break;
      case 3:
      case 26:
      case 35:
        ItemIcon component4 = icon.GetComponent<ItemIcon>();
        if (Object.op_Inequality((Object) component4, (Object) null))
        {
          component4.QuantitySupply = false;
          component4.ReleaseClickEvent();
          this.initButtonEvent(component1, itemIcon, icon);
          break;
        }
        break;
      default:
        this.initButtonEvent(component1, itemIcon, icon);
        break;
    }
    itemIcon.iconObj = parent.GetComponent<CreateIconObject>();
    parent.SetActive(true);
  }

  private void initButtonEvent(
    LongPressButton btn,
    Guild02862Menu.GiftIconInfo info,
    GameObject icon)
  {
    foreach (Collider componentsInChild in icon.GetComponentsInChildren<BoxCollider>())
      componentsInChild.enabled = false;
    ((Behaviour) btn).enabled = true;
    EventDelegate.Set(btn.onClick, (EventDelegate.Callback) (() => this.onClickIcon(info)));
    EventDelegate.Set(btn.onLongPress, (EventDelegate.Callback) (() => this.onLongPressIcon(info)));
  }

  public IEnumerator InitGiftList()
  {
    this.isInitialize = false;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    this.scroll.Clear();
    this.scroll.Reset();
    for (int index = 0; index < Mathf.Min(ItemIcon.MaxValue, this.allGiftItemInfo.Count); ++index)
    {
      GameObject gameObject = new GameObject("ibtn_GiftIcon");
      UIWidget uiWidget = gameObject.AddComponent<UIWidget>();
      gameObject.AddComponent<LongPressButton>();
      gameObject.AddComponent<BoxCollider>();
      gameObject.AddComponent<UIDragScrollView>();
      uiWidget.autoResizeBoxCollider = true;
      uiWidget.SetDimensions(112, 121);
      ((UIRect) uiWidget).alpha = 0.0f;
      this.showGiftList.Add(gameObject);
    }
    yield return (object) null;
    for (int index = 0; index < this.showGiftList.Count; ++index)
    {
      if (!Object.op_Equality((Object) this.showGiftList[index], (Object) null))
        this.scroll.Add(this.showGiftList[index], ItemIcon.Width, ItemIcon.Height);
    }
    this.scroll.CreateScrollPoint(ItemIcon.Height, this.allGiftItemInfo.Count);
    this.scroll.ResolvePosition();
    for (int i = 0; i < Mathf.Min(ItemIcon.MaxValue, this.allGiftItemInfo.Count); ++i)
    {
      IEnumerator e = this.CreateItemIconForInitialize(i, i);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.showGiftList[i].gameObject.SetActive(false);
      ((UIRect) this.showGiftList[i].GetComponent<UIWidget>()).alpha = 1f;
    }
    for (int index = 0; index < Mathf.Min(ItemIcon.MaxValue, this.allGiftItemInfo.Count); ++index)
      this.showGiftList[index].SetActive(true);
    this.scrool_start_y = ((Component) this.scroll.scrollView).transform.localPosition.y;
    this.isInitialize = true;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
  }

  private IEnumerator SetItem(Guild02862Menu.GiftIconInfo iconInfo)
  {
    Guild02862Menu guild02862Menu = this;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    // ISSUE: reference to a compiler-generated method
    Future<WebAPI.Response.GuildGiftUpdateWishList> ft = WebAPI.GuildGiftUpdateWishList(iconInfo.gift.id, false, new Action<WebAPI.Response.UserError>(guild02862Menu.\u003CSetItem\u003Eb__17_0));
    IEnumerator e = ft.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (ft.Result != null)
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      guild02862Menu.backScene();
    }
  }

  private IEnumerator ShowDetailPopup(Guild02862Menu.GiftIconInfo giftInfo)
  {
    GameObject popup = Singleton<PopupManager>.GetInstance().open(this.itemDetailPopup);
    popup.SetActive(false);
    IEnumerator e = popup.GetComponent<Shop00742Menu>().Init((MasterDataTable.CommonRewardType) giftInfo.gift.reward_type_id, giftInfo.gift.reward_id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  private void onClickIcon(Guild02862Menu.GiftIconInfo giftInfo)
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.SetItem(giftInfo));
  }

  private void onLongPressIcon(Guild02862Menu.GiftIconInfo giftInfo)
  {
    if (this.IsPushAndSet())
      return;
    if (giftInfo.gift.reward_type_id == 1 || giftInfo.gift.reward_type_id == 24 || giftInfo.gift.reward_type_id == 3 || giftInfo.gift.reward_type_id == 26 || giftInfo.gift.reward_type_id == 35 || giftInfo.gift.reward_type_id == 2 || giftInfo.gift.reward_type_id == 19 || giftInfo.gift.reward_type_id == 21)
      this.StartCoroutine(this.ShowDetailPopup(giftInfo));
    else
      this.IsPush = false;
  }

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }

  private void ScrollUpdate()
  {
    if (!this.isInitialize || this.showGiftList.Count <= ItemIcon.ScreenValue)
      return;
    int num1 = ItemIcon.Height * 2;
    float num2 = ((Component) this.scroll.scrollView).transform.localPosition.y - this.scrool_start_y;
    float num3 = (float) (Mathf.Max(0, this.allGiftItemInfo.Count - ItemIcon.ScreenValue - 1) / ItemIcon.ColumnValue * ItemIcon.Height);
    float num4 = (float) (ItemIcon.Height * ItemIcon.RowValue);
    if ((double) num2 < 0.0)
      num2 = 0.0f;
    if ((double) num2 > (double) num3)
      num2 = num3;
    bool flag;
    do
    {
      flag = false;
      int num5 = 0;
      foreach (GameObject gameObject in this.scroll)
      {
        GameObject item = gameObject;
        float num6 = item.transform.localPosition.y + num2;
        int? nullable = this.allGiftItemInfo.FirstIndexOrNull<Guild02862Menu.GiftIconInfo>((Func<Guild02862Menu.GiftIconInfo, bool>) (v => Object.op_Inequality((Object) v.iconObj, (Object) null) && Object.op_Equality((Object) ((Component) v.iconObj).gameObject, (Object) item)));
        if ((double) num6 > (double) num1)
        {
          int info_index = nullable.HasValue ? nullable.Value + ItemIcon.MaxValue : this.allGiftItemInfo.Count;
          if (nullable.HasValue && info_index < this.allGiftItemInfo.Count)
          {
            item.transform.localPosition = new Vector3(item.transform.localPosition.x, item.transform.localPosition.y - num4, 0.0f);
            this.ResetScroll(num5);
            this.CreateItemIcon(info_index, num5);
            flag = true;
          }
        }
        else if ((double) num6 < -((double) num4 - (double) num1))
        {
          int num7 = ItemIcon.MaxValue;
          if (!item.activeSelf)
          {
            item.SetActive(true);
            num7 = 0;
          }
          int info_index = nullable.HasValue ? nullable.Value - num7 : -1;
          if (nullable.HasValue && info_index >= 0)
          {
            item.transform.localPosition = new Vector3(item.transform.localPosition.x, item.transform.localPosition.y + num4, 0.0f);
            this.ResetScroll(num5);
            this.CreateItemIcon(info_index, num5);
            flag = true;
          }
        }
        ++num5;
      }
    }
    while (flag);
  }

  protected void ResetScroll(int index)
  {
    GameObject item = this.showGiftList[index];
    for (int index1 = 0; index1 < item.transform.childCount; ++index1)
      Object.Destroy((Object) ((Component) item.transform.GetChild(index1)).gameObject);
    item.SetActive(false);
    this.allGiftItemInfo.Where<Guild02862Menu.GiftIconInfo>((Func<Guild02862Menu.GiftIconInfo, bool>) (a => Object.op_Equality((Object) a.iconObj, (Object) item.GetComponent<CreateIconObject>()))).ForEach<Guild02862Menu.GiftIconInfo>((Action<Guild02862Menu.GiftIconInfo>) (b => b.iconObj = (CreateIconObject) null));
  }

  public class GiftIconInfo
  {
    public GuildGift gift;
    public CreateIconObject iconObj;
    public bool select;
    public int index;
  }
}
