// Decompiled with JetBrains decompiler
// Type: Shop00714Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Shop00714Menu : BackButtonMenuBase
{
  private int price;
  private bool success;
  private int nextMax;
  [SerializeField]
  private UILabel txtDescription2;
  [SerializeField]
  private UILabel txtExpantionNumValue;
  [SerializeField]
  private UILabel txtManaCostValue;
  [SerializeField]
  private PopupSelectSliderController sliderController;
  private int unitPrice;
  private int unitQuantity;

  public void Init(int unitPrice, int unitQuantity, Action<int> onPrice = null)
  {
    this.price = unitPrice;
    this.unitPrice = unitPrice;
    this.unitQuantity = unitQuantity;
    Player player = SMManager.Observe<Player>().Value;
    int num1 = player.max_units_cap - player.max_units;
    int num2 = num1 / unitQuantity * unitPrice;
    if (num1 % unitQuantity > 0)
      num2 += unitPrice;
    int maxPrice = Mathf.Min(num2, player.coin / unitPrice * unitPrice);
    this.InitSliderController(unitPrice, unitPrice, maxPrice, onPrice);
  }

  private void InitSliderController(
    int unitPrice,
    int minPrice,
    int maxPrice,
    Action<int> onPrice)
  {
    if (minPrice < maxPrice)
    {
      this.sliderController.Initialize((float) minPrice, (float) maxPrice, (float) this.price, (PopupSelectSliderController.SliderValueChangeListener) (val =>
      {
        this.price = Mathf.RoundToInt(val);
        this.OnSliderValueChange();
        if (onPrice == null)
          return;
        onPrice(this.price);
      }), (float) unitPrice);
    }
    else
    {
      this.sliderController.Initialize(0.0f, (float) unitPrice, (float) unitPrice, step: (float) unitPrice);
      this.sliderController.LockSlider();
    }
    this.OnSliderValueChange();
    if (onPrice == null)
      return;
    onPrice(this.price);
  }

  private void OnSliderValueChange()
  {
    Player player = SMManager.Observe<Player>().Value;
    this.UpdateNextMax(player);
    this.UpdateText(player);
  }

  private void UpdateNextMax(Player player)
  {
    this.nextMax = player.max_units + this.price / this.unitPrice * this.unitQuantity;
  }

  private void UpdateText(Player player)
  {
    int number = Mathf.Min(this.nextMax, player.max_units_cap);
    this.txtDescription2.SetText(Consts.GetInstance().SHOP_00714_MENU + ":" + player.max_units.ToLocalizeNumberText() + "→[fff000]" + number.ToLocalizeNumberText() + "[-]");
    this.txtExpantionNumValue.SetText((this.price * this.unitQuantity).ToLocalizeNumberText());
    this.txtManaCostValue.SetText("[fff000]" + this.price.ToLocalizeNumberText() + "[-]");
  }

  private IEnumerator UnitBoxPlus()
  {
    Future<WebAPI.Response.ShopBuy> paramF = WebAPI.ShopBuy(10000002, this.price, (Action<WebAPI.Response.UserError>) (e =>
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    })).Then<WebAPI.Response.ShopBuy>((Func<WebAPI.Response.ShopBuy, WebAPI.Response.ShopBuy>) (result =>
    {
      Singleton<NGGameDataManager>.GetInstance().Parse(result);
      return result;
    }));
    IEnumerator e1 = paramF.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (paramF.Result != null)
    {
      e1 = OnDemandDownload.WaitLoadHasUnitResource(false);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      e1 = this.popup00715();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
    }
  }

  public void IbtnPopupYes()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss(true);
    if (SMManager.Get<Player>().CheckKiseki(this.price))
      Singleton<PopupManager>.GetInstance().monitorCoroutine(this.UnitBoxPlusAsync());
    else
      Singleton<PopupManager>.GetInstance().monitorCoroutine(PopupUtility._999_3_1(Consts.GetInstance().SHOP_99931_TXT_DESCRIPTION));
  }

  private IEnumerator UnitBoxPlusAsync()
  {
    Singleton<PopupManager>.GetInstance().closeAll(true);
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    IEnumerator e = this.UnitBoxPlus();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator popup00715()
  {
    Future<GameObject> prefab = Res.Prefabs.popup.popup_007_15__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(prefab.Result).GetComponent<Shop00715SetText>().SetText(this.price, this.nextMax);
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public override void onBackButton() => this.IbtnNo();
}
