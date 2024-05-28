// Decompiled with JetBrains decompiler
// Type: ShopItemListMenu
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
public class ShopItemListMenu : BackButtonMenuBase
{
  private const int ICON_WIDTH = 206;
  private const int ICON_HEIGHT = 240;
  private const int CHECK_HEIGHT = 480;
  private const int COLUM_NUM = 3;
  private const int ICON_MAX_SCREEN = 21;
  private const int ICON_ROW = 7;
  private const int ICON_SCREEN_VALUE = 12;
  private const float ADD_HEIGHT = 1680f;
  [Header("Top")]
  [SerializeField]
  private UILabel title;
  [SerializeField]
  private NGxScroll2 scroll;
  [Header("Bottom")]
  [SerializeField]
  private GameObject BattleMedalView;
  [SerializeField]
  private UI2DSprite BattleMedalIcon;
  [SerializeField]
  private UILabel HaveBatlleMedal;
  [SerializeField]
  private UILabel HaveLimitBattleMedal;
  private List<ShopItemIconInfo> shopItemIconInfos = new List<ShopItemIconInfo>();
  private List<ShopItemIcon> shopItemIcons = new List<ShopItemIcon>();
  private GameObject ItemIconPrefab;
  private Transform scrollViewTransform;
  private float scrollStartY;
  private bool isInitEnd;
  public int ShopId;
  private int bannerId;
  private string shopName;
  private GameObject battleMedaldetailPopup;
  private List<ShopItemListMenu.BattleMedal> battleMedals = new List<ShopItemListMenu.BattleMedal>();
  private float latestScrollValueY;

  public IEnumerator Init(int shopId, int bannerId, string shopName, string shopTime)
  {
    ShopItemListMenu menu = this;
    if (!menu.isInitEnd)
    {
      menu.ShopId = shopId;
      menu.bannerId = bannerId;
      menu.shopName = shopName;
      Future<GameObject> prefabF = new ResourceObject("Prefabs/shop007_4_1/Shop_vscroll").Load<GameObject>();
      IEnumerator e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      menu.ItemIconPrefab = prefabF.Result;
      menu.scroll.Clear();
      menu.shopItemIcons.Clear();
      menu.scroll.Reset();
      menu.SetTile();
      menu.CreateShopItemIconInfos(shopId, bannerId);
      int iconScreenCount = Mathf.Min(21, menu.shopItemIconInfos.Count);
      for (int i = 0; i < iconScreenCount; ++i)
      {
        ShopItemIcon component = Object.Instantiate<GameObject>(menu.ItemIconPrefab).GetComponent<ShopItemIcon>();
        menu.shopItemIcons.Add(component);
        menu.scroll.AddColumn3(((Component) menu.shopItemIcons[i]).gameObject, 206, 240, i);
        menu.shopItemIconInfos[i].shopItemIcon = component;
        component.info = menu.shopItemIconInfos[i];
        yield return (object) component.Init(menu, menu.scroll.scrollView, shopTime);
      }
      menu.scrollViewTransform = ((Component) menu.scroll.scrollView).transform;
      menu.scrollStartY = ((Component) menu.scroll.scrollView).transform.localPosition.y;
      prefabF = Res.Prefabs.popup.popup_007_4_3__anim_popup01.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      menu.battleMedaldetailPopup = prefabF.Result;
      if (shopId == 4000)
      {
        menu.BattleMedalView.SetActive(true);
        menu.BattleMedalIcon.sprite2D = ShopCommon.PayTypeBattleMedal;
        menu.UpdateHaveBattleMedal();
        menu.scroll.bottomSpace = 200;
      }
      else
        menu.BattleMedalView.SetActive(false);
      menu.CreateScrollPointHeight();
      menu.scroll.ResolvePosition();
      menu.scroll.scrollView.UpdatePosition();
      menu.isInitEnd = true;
    }
  }

  private void SetTile()
  {
    if (this.ShopId == 2000)
      this.title.text = "育成ショップ";
    else if (this.ShopId == 3000)
      this.title.text = "レアメダルショップ";
    else if (this.ShopId == 4000)
      this.title.text = "ファンキルメダルショップ";
    else
      this.title.text = this.shopName;
  }

  private void CreateShopItemIconInfos(int shopId, int bannerId)
  {
    this.shopItemIconInfos.Clear();
    foreach (PlayerShopArticle playerShopArticle in ((IEnumerable<PlayerShopArticle>) ((IEnumerable<Shop>) SMManager.Get<Shop[]>()).First<Shop>((Func<Shop, bool>) (x => x.id == shopId)).articles).Where<PlayerShopArticle>((Func<PlayerShopArticle, bool>) (x =>
    {
      int? bannerId1 = x.banner_id;
      int num = bannerId;
      return bannerId1.GetValueOrDefault() == num & bannerId1.HasValue;
    })).ToArray<PlayerShopArticle>())
      this.shopItemIconInfos.Add(new ShopItemIconInfo(playerShopArticle));
  }

  private void CreateScrollPointHeight()
  {
    this.scroll.CreateScrollPoint(240, this.shopItemIconInfos.Count);
    this.scroll.BottomObject.transform.localPosition = new Vector3(0.0f, (float) (-(Mathf.Max(0, this.shopItemIconInfos.Count - 1) / 3 * 240) - 120), 0.0f);
  }

  public void UpdateHaveBattleMedal()
  {
    int c = 0;
    int num = 0;
    this.battleMedals.Clear();
    foreach (PlayerBattleMedal playerBattleMedal in SMManager.Get<PlayerBattleMedal[]>())
    {
      PlayerBattleMedal battleMedal = playerBattleMedal;
      if (battleMedal.end_at.HasValue && battleMedal.end_at.Value > ShopCommon.LoginTime)
      {
        num += battleMedal.count;
        ShopItemListMenu.BattleMedal battleMedal1 = this.battleMedals.Find((Predicate<ShopItemListMenu.BattleMedal>) (tm => tm.limit == battleMedal.end_at.Value));
        if (battleMedal1 != null)
          battleMedal1.count += battleMedal.count;
        else
          this.battleMedals.Add(new ShopItemListMenu.BattleMedal(battleMedal.end_at.Value, battleMedal.count));
      }
      else
        c += battleMedal.count;
    }
    if (this.battleMedals.Count > 1)
      this.battleMedals = this.battleMedals.OrderBy<ShopItemListMenu.BattleMedal, long>((Func<ShopItemListMenu.BattleMedal, long>) (m => m.limit.Ticks)).ToList<ShopItemListMenu.BattleMedal>();
    this.battleMedals.Add(new ShopItemListMenu.BattleMedal(new DateTime(0L), c));
    this.HaveBatlleMedal.text = SMManager.Get<Player>().battle_medal.ToString();
    this.HaveLimitBattleMedal.text = num.ToString();
  }

  public IEnumerator UpdateIconsHavingNum()
  {
    foreach (ShopItemIcon shopItemIcon in this.shopItemIcons)
      yield return (object) shopItemIcon.UpdateView();
  }

  protected override void Update()
  {
    if (!this.isInitEnd)
      return;
    base.Update();
    this.ScrollUpdate();
  }

  private void ScrollUpdate()
  {
    if (this.shopItemIconInfos.Count <= 12)
      return;
    float num1 = this.scrollViewTransform.localPosition.y - this.scrollStartY;
    if ((double) this.latestScrollValueY == (double) num1)
      return;
    this.latestScrollValueY = num1;
    float num2 = (float) (Mathf.Max(0, this.shopItemIconInfos.Count - 12 - 1) / 3 * 240);
    if ((double) num1 < 0.0)
      num1 = 0.0f;
    if ((double) num1 > (double) num2)
      num1 = num2;
    bool flag;
    do
    {
      flag = false;
      int count = 0;
      foreach (GameObject gameObject in this.scroll)
      {
        GameObject item = gameObject;
        Transform transform = item.transform;
        float num3 = transform.localPosition.y + num1;
        int? nullable = this.shopItemIconInfos.FirstIndexOrNull<ShopItemIconInfo>((Func<ShopItemIconInfo, bool>) (v => Object.op_Inequality((Object) v.shopItemIcon, (Object) null) && Object.op_Equality((Object) ((Component) v.shopItemIcon).gameObject, (Object) item)));
        if ((double) num3 > 480.0)
        {
          transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 1680f, 0.0f);
          if (nullable.HasValue && nullable.Value + 21 < (this.shopItemIconInfos.Count + 4) / 3 * 3)
          {
            if (nullable.Value + 21 >= this.shopItemIconInfos.Count)
              item.SetActive(false);
            else
              this.ScrollIconUpdate(nullable.Value + 21, count);
            flag = true;
          }
        }
        else if ((double) num3 < -1200.0)
        {
          int num4 = 21;
          if (!item.activeSelf)
          {
            item.SetActive(true);
            num4 = 0;
          }
          if (nullable.HasValue && nullable.Value - num4 >= 0)
          {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 1680f, 0.0f);
            this.ScrollIconUpdate(nullable.Value - num4, count);
            flag = true;
          }
        }
        ++count;
      }
    }
    while (flag);
  }

  private void ScrollIconUpdate(int index, int count)
  {
    ShopItemIcon shopItemIcon = this.shopItemIcons[count];
    this.shopItemIconInfos.First<ShopItemIconInfo>((Func<ShopItemIconInfo, bool>) (a => Object.op_Equality((Object) a.shopItemIcon, (Object) shopItemIcon))).shopItemIcon = (ShopItemIcon) null;
    this.shopItemIconInfos[index].shopItemIcon = shopItemIcon;
    shopItemIcon.info = this.shopItemIconInfos[index];
    this.StartCoroutine(shopItemIcon.UpdateView());
  }

  public void OnDetailPressed() => this.StartCoroutine(this.openDetail());

  private IEnumerator openDetail()
  {
    GameObject prefab = Object.Instantiate<GameObject>(this.battleMedaldetailPopup);
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
    IEnumerator initw = prefab.GetComponent<Popup00743Menu>().Init(this.battleMedals);
    while (initw.MoveNext())
      yield return initw.Current;
  }

  public override void onBackButton() => this.OnIbtnBack();

  private void OnIbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public class BattleMedal
  {
    public DateTime limit;
    public int count;

    public BattleMedal()
    {
      this.limit = new DateTime(0L);
      this.count = 0;
    }

    public BattleMedal(DateTime l, int c)
    {
      this.limit = l;
      this.count = c;
    }

    public BattleMedal(ShopItemListMenu.BattleMedal m)
    {
      this.limit = m.limit;
      this.count = m.count;
    }
  }
}
