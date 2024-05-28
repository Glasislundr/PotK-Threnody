// Decompiled with JetBrains decompiler
// Type: ShopAccumulation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using Gsc.Purchase;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class ShopAccumulation : MonoBehaviour
{
  [SerializeField]
  private UIWidget pageUiWidget;
  [SerializeField]
  private UILabel currentStageName;
  [SerializeField]
  private GameObject kisekiInfo;
  [SerializeField]
  private UILabel kisekiInfoLabel;
  [SerializeField]
  private UIGrid stage1Grid;
  [SerializeField]
  private UIGrid stage2Grid;
  [SerializeField]
  private UIGrid stage3Grid;
  [SerializeField]
  private UICenterOnChild centerOnChild;
  [SerializeField]
  private GameObject leftArrow;
  [SerializeField]
  private GameObject rightArrow;
  [SerializeField]
  private UILabel stageWarningNote;
  [SerializeField]
  private GameObject receiveRestDay;
  [SerializeField]
  private UILabel receiveRestDayText;
  [SerializeField]
  private UIButton priceOrReceiveButton;
  [SerializeField]
  private UILabel priceOrReceiveLabel;
  private WebAPI.Response.CoinbonusHistory coinbonusHistory;
  private StepupPackInfo stepupPackInfo;
  private StepupPackInfoPack_steps current_pack_step;
  private ProductInfo currentProductInfo;
  private bool currentStageIsBuy;
  private DateTime now;
  private const string STAGE1_TEXT = "ステージI";
  private const string STAGE2_TEXT = "ステージII";
  private const string STAGE3_TEXT = "ステージIII";

  public IEnumerator Init(WebAPI.Response.CoinbonusHistory coinbonusHistory)
  {
    ((UIRect) this.pageUiWidget).alpha = 0.0f;
    this.coinbonusHistory = coinbonusHistory;
    this.stepupPackInfo = coinbonusHistory.stepup_packs[0];
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.now = ServerTime.NowAppTime();
    Future<GameObject> prefab = new ResourceObject("Prefabs/common/dir_Reward_IconOnly_Item").Load<GameObject>();
    e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject withLoupeIcon = prefab.Result;
    yield return (object) this.CreateStage(this.stepupPackInfo.pack_steps[0].rewards, withLoupeIcon, ((Component) this.stage1Grid).transform);
    this.stage1Grid.Reposition();
    yield return (object) this.CreateStage(this.stepupPackInfo.pack_steps[1].rewards, withLoupeIcon, ((Component) this.stage2Grid).transform);
    this.stage2Grid.Reposition();
    yield return (object) this.CreateStage(this.stepupPackInfo.pack_steps[2].rewards, withLoupeIcon, ((Component) this.stage3Grid).transform);
    this.stage3Grid.Reposition();
  }

  private void Start()
  {
    int? nullable1 = new int?();
    if (this.stepupPackInfo.player_pack.purchased_at.HasValue)
    {
      DateTime dateTime1 = new DateTime(this.now.Year, this.now.Month, this.now.Day, 23, 59, 59);
      ref int? local = ref nullable1;
      DateTime dateTime2 = dateTime1;
      DateTime? purchasedAt = this.stepupPackInfo.player_pack.purchased_at;
      int days = (purchasedAt.HasValue ? new TimeSpan?(dateTime2 - purchasedAt.GetValueOrDefault()) : new TimeSpan?()).Value.Days;
      local = new int?(days);
    }
    int num1;
    if (nullable1.HasValue)
    {
      int? nullable2 = nullable1;
      int num2 = 6;
      if (nullable2.GetValueOrDefault() == num2 & nullable2.HasValue && this.stepupPackInfo.player_pack.is_received)
      {
        num1 = this.stepupPackInfo.player_pack.next_step;
        goto label_13;
      }
    }
    int? nullable3;
    if (nullable1.HasValue)
    {
      nullable3 = nullable1;
      int num3 = 7;
      if (nullable3.GetValueOrDefault() >= num3 & nullable3.HasValue)
      {
        nullable3 = nullable1;
        int num4 = 9;
        if (nullable3.GetValueOrDefault() <= num4 & nullable3.HasValue)
        {
          num1 = this.stepupPackInfo.player_pack.next_step;
          goto label_13;
        }
      }
    }
    if (nullable1.HasValue)
    {
      nullable3 = nullable1;
      int num5 = 10;
      if (nullable3.GetValueOrDefault() >= num5 & nullable3.HasValue)
      {
        num1 = this.stepupPackInfo.player_pack.next_step;
        goto label_13;
      }
    }
    num1 = this.stepupPackInfo.player_pack.current_step;
label_13:
    Transform transform = (Transform) null;
    switch (num1)
    {
      case 1:
        transform = ((Component) this.stage1Grid).transform.parent;
        break;
      case 2:
        transform = ((Component) this.stage2Grid).transform.parent;
        break;
      case 3:
        transform = ((Component) this.stage3Grid).transform.parent;
        break;
      default:
        Debug.LogError((object) ("想定していないステージです: " + (object) num1));
        break;
    }
    // ISSUE: method pointer
    this.centerOnChild.onFinished = new SpringPanel.OnFinished((object) this, __methodptr(\u003CStart\u003Eb__25_0));
    this.centerOnChild.CenterOn(transform);
  }

  private IEnumerator CreateStage(
    StepupPackReward[] stepup_pack_rewards,
    GameObject withLoupeIcon,
    Transform parent)
  {
    StepupPackReward[] stepupPackRewardArray = stepup_pack_rewards;
    for (int index = 0; index < stepupPackRewardArray.Length; ++index)
    {
      StepupPackReward stepupPackReward = stepupPackRewardArray[index];
      IEnumerator e = withLoupeIcon.Clone(parent).GetComponent<ItemIconDetail>().Init((MasterDataTable.CommonRewardType) stepupPackReward.reward_type_id, stepupPackReward.reward_id, stepupPackReward.reward_quantity);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    stepupPackRewardArray = (StepupPackReward[]) null;
  }

  public void OrLeftArrow()
  {
    if (Object.op_Equality((Object) ((Component) ((Component) this.stage2Grid).transform.parent).gameObject, (Object) this.centerOnChild.centeredObject))
    {
      this.centerOnChild.CenterOn(((Component) this.stage1Grid).transform.parent);
    }
    else
    {
      if (!Object.op_Equality((Object) ((Component) ((Component) this.stage3Grid).transform.parent).gameObject, (Object) this.centerOnChild.centeredObject))
        return;
      this.centerOnChild.CenterOn(((Component) this.stage2Grid).transform.parent);
    }
  }

  public void OrRightArrow()
  {
    if (Object.op_Equality((Object) ((Component) ((Component) this.stage1Grid).transform.parent).gameObject, (Object) this.centerOnChild.centeredObject))
    {
      this.centerOnChild.CenterOn(((Component) this.stage2Grid).transform.parent);
    }
    else
    {
      if (!Object.op_Equality((Object) ((Component) ((Component) this.stage2Grid).transform.parent).gameObject, (Object) this.centerOnChild.centeredObject))
        return;
      this.centerOnChild.CenterOn(((Component) this.stage3Grid).transform.parent);
    }
  }

  private void centerOnFinished()
  {
    int? nullable1;
    if (Object.op_Equality((Object) ((Component) ((Component) this.stage1Grid).transform.parent).gameObject, (Object) this.centerOnChild.centeredObject))
      nullable1 = new int?(0);
    else if (Object.op_Equality((Object) ((Component) ((Component) this.stage2Grid).transform.parent).gameObject, (Object) this.centerOnChild.centeredObject))
      nullable1 = new int?(1);
    else if (Object.op_Equality((Object) ((Component) ((Component) this.stage3Grid).transform.parent).gameObject, (Object) this.centerOnChild.centeredObject))
    {
      nullable1 = new int?(2);
    }
    else
    {
      int? nullable2 = new int?();
      Debug.LogError((object) "想定していない累積特典ステージです");
      return;
    }
    int? nullable3 = nullable1;
    int num1 = 0;
    if (nullable3.GetValueOrDefault() == num1 & nullable3.HasValue)
    {
      this.leftArrow.SetActive(false);
      this.rightArrow.SetActive(true);
      this.currentStageName.text = "ステージI";
    }
    else
    {
      nullable3 = nullable1;
      int num2 = 1;
      if (nullable3.GetValueOrDefault() == num2 & nullable3.HasValue)
      {
        this.leftArrow.SetActive(true);
        this.rightArrow.SetActive(true);
        this.currentStageName.text = "ステージII";
      }
      else
      {
        this.leftArrow.SetActive(true);
        this.rightArrow.SetActive(false);
        this.currentStageName.text = "ステージIII";
      }
    }
    this.current_pack_step = this.stepupPackInfo.pack_steps[nullable1.Value];
    int? nullable4 = new int?();
    int? nullable5 = new int?();
    if (this.stepupPackInfo.player_pack.purchased_at.HasValue)
    {
      DateTime dateTime1 = new DateTime(this.now.Year, this.now.Month, this.now.Day, 23, 59, 59);
      ref int? local = ref nullable5;
      DateTime dateTime2 = dateTime1;
      DateTime? purchasedAt = this.stepupPackInfo.player_pack.purchased_at;
      int days = (purchasedAt.HasValue ? new TimeSpan?(dateTime2 - purchasedAt.GetValueOrDefault()) : new TimeSpan?()).Value.Days;
      local = new int?(days);
    }
    int num3;
    int? nullable6;
    if (nullable5.HasValue)
    {
      nullable3 = nullable5;
      int num4 = 6;
      if (nullable3.GetValueOrDefault() == num4 & nullable3.HasValue && this.stepupPackInfo.player_pack.is_received)
      {
        num3 = this.stepupPackInfo.player_pack.next_step;
        nullable6 = new int?();
        goto label_25;
      }
    }
    if (nullable5.HasValue)
    {
      nullable3 = nullable5;
      int num5 = 7;
      if (nullable3.GetValueOrDefault() >= num5 & nullable3.HasValue)
      {
        nullable3 = nullable5;
        int num6 = 9;
        if (nullable3.GetValueOrDefault() <= num6 & nullable3.HasValue)
        {
          num3 = this.stepupPackInfo.player_pack.next_step;
          nullable6 = new int?();
          goto label_25;
        }
      }
    }
    if (nullable5.HasValue)
    {
      nullable3 = nullable5;
      int num7 = 10;
      if (nullable3.GetValueOrDefault() >= num7 & nullable3.HasValue)
      {
        num3 = this.stepupPackInfo.player_pack.next_step;
        nullable6 = new int?();
        goto label_25;
      }
    }
    num3 = this.stepupPackInfo.player_pack.current_step;
    nullable6 = this.stepupPackInfo.player_pack.rest_receive_day;
label_25:
    if (!nullable6.HasValue)
    {
      this.receiveRestDay.SetActive(false);
    }
    else
    {
      int num8 = num3;
      int? nullable7 = nullable1;
      nullable3 = nullable7.HasValue ? new int?(nullable7.GetValueOrDefault() + 1) : new int?();
      int valueOrDefault = nullable3.GetValueOrDefault();
      if (!(num8 == valueOrDefault & nullable3.HasValue))
      {
        this.receiveRestDay.SetActive(false);
      }
      else
      {
        this.receiveRestDay.SetActive(true);
        nullable3 = nullable6;
        int num9 = 0;
        if (nullable3.GetValueOrDefault() > num9 & nullable3.HasValue)
        {
          this.receiveRestDayText.text = string.Format("残り{0}日", (object) nullable6);
        }
        else
        {
          TimeSpan timeSpan = new DateTime(this.now.Year, this.now.Month, this.now.Day, 23, 59, 59) - this.now;
          this.receiveRestDayText.text = timeSpan.Hours <= 0 ? string.Format("残り{0}分", (object) timeSpan.Minutes) : string.Format("残り{0}時間", (object) timeSpan.Hours);
        }
      }
    }
    CoinGroup coinGroup = ((IEnumerable<CoinGroup>) this.coinbonusHistory.coin_groups).First<CoinGroup>((Func<CoinGroup, bool>) (x => x.id == this.current_pack_step.pack_set.coin_group_id));
    ProductInfo productInfo = ((IEnumerable<ProductInfo>) PurchaseHandler.Instance.ProductList).First<ProductInfo>((Func<ProductInfo, bool>) (x => x.ProductId == coinGroup.GetProductId()));
    this.currentProductInfo = productInfo;
    int? nullable8;
    if (this.stepupPackInfo.player_pack.is_purchasable)
    {
      this.currentStageIsBuy = true;
      this.kisekiInfo.SetActive(true);
      this.kisekiInfoLabel.text = string.Format("購入時、有償石{0}個と特典付与", (object) MasterData.CoinProductList.GetActiveProductData(productInfo.ProductId).additional_paid_coin);
      this.priceOrReceiveLabel.SetTextLocalize(productInfo.LocalizedPrice.Replace("\\", "￥"));
    }
    else
    {
      if (!this.stepupPackInfo.player_pack.is_received)
      {
        int num10 = num3;
        int? nullable9 = nullable1;
        nullable3 = nullable9.HasValue ? new int?(nullable9.GetValueOrDefault() + 1) : new int?();
        int valueOrDefault = nullable3.GetValueOrDefault();
        if (num10 == valueOrDefault & nullable3.HasValue)
        {
          this.currentStageIsBuy = false;
          this.kisekiInfo.SetActive(false);
          this.priceOrReceiveLabel.text = "受取";
          goto label_40;
        }
      }
      this.currentStageIsBuy = false;
      int num11 = num3;
      nullable8 = nullable1;
      nullable3 = nullable8.HasValue ? new int?(nullable8.GetValueOrDefault() + 1) : new int?();
      int valueOrDefault1 = nullable3.GetValueOrDefault();
      if (num11 == valueOrDefault1 & nullable3.HasValue)
      {
        this.kisekiInfo.SetActive(false);
        this.priceOrReceiveLabel.text = "受取済";
      }
      else
      {
        this.kisekiInfo.SetActive(true);
        this.kisekiInfoLabel.text = string.Format("購入時、有償石{0}個と特典付与", (object) MasterData.CoinProductList.GetActiveProductData(productInfo.ProductId).additional_paid_coin);
        this.priceOrReceiveLabel.SetTextLocalize(productInfo.LocalizedPrice.Replace("\\", "￥"));
      }
    }
label_40:
    if (this.stepupPackInfo.player_pack.is_purchasable || !this.stepupPackInfo.player_pack.is_received)
    {
      int num12 = num3;
      nullable8 = nullable1;
      nullable3 = nullable8.HasValue ? new int?(nullable8.GetValueOrDefault() + 1) : new int?();
      int valueOrDefault = nullable3.GetValueOrDefault();
      if (num12 == valueOrDefault & nullable3.HasValue)
      {
        ((UIButtonColor) this.priceOrReceiveButton).isEnabled = true;
        goto label_44;
      }
    }
    ((UIButtonColor) this.priceOrReceiveButton).isEnabled = false;
label_44:
    if (num3 != 1)
    {
      int num13 = num3;
      nullable8 = nullable1;
      nullable3 = nullable8.HasValue ? new int?(nullable8.GetValueOrDefault() + 1) : new int?();
      int valueOrDefault = nullable3.GetValueOrDefault();
      if (num13 == valueOrDefault & nullable3.HasValue && !nullable6.HasValue)
      {
        this.stageWarningNote.text = "現在のステージを継続購入しない場合、ステージIに戻ります。";
        goto label_63;
      }
    }
    int num14 = num3;
    nullable8 = nullable1;
    nullable3 = nullable8.HasValue ? new int?(nullable8.GetValueOrDefault() + 1) : new int?();
    int valueOrDefault2 = nullable3.GetValueOrDefault();
    if (!(num14 == valueOrDefault2 & nullable3.HasValue))
    {
      int num15 = num3;
      nullable8 = nullable1;
      nullable3 = nullable8.HasValue ? new int?(nullable8.GetValueOrDefault() + 1) : new int?();
      int valueOrDefault3 = nullable3.GetValueOrDefault();
      if (num15 < valueOrDefault3 & nullable3.HasValue)
      {
        this.stageWarningNote.text = "前のステージを購入すると本ステージに昇格可能となります。";
        goto label_63;
      }
    }
    int num16 = num3;
    nullable8 = nullable1;
    nullable3 = nullable8.HasValue ? new int?(nullable8.GetValueOrDefault() + 1) : new int?();
    int valueOrDefault4 = nullable3.GetValueOrDefault();
    if (!(num16 == valueOrDefault4 & nullable3.HasValue))
    {
      int num17 = num3;
      nullable8 = nullable1;
      nullable3 = nullable8.HasValue ? new int?(nullable8.GetValueOrDefault() + 1) : new int?();
      int valueOrDefault5 = nullable3.GetValueOrDefault();
      if (num17 > valueOrDefault5 & nullable3.HasValue)
      {
        this.stageWarningNote.text = "現在のステージを継続購入しない場合、ステージIに戻ります";
        goto label_63;
      }
    }
    if (num3 == 1)
    {
      nullable8 = nullable1;
      nullable3 = nullable8.HasValue ? new int?(nullable8.GetValueOrDefault() + 1) : new int?();
      int num18 = 3;
      if (nullable3.GetValueOrDefault() == num18 & nullable3.HasValue)
      {
        this.stageWarningNote.text = "前のステージを購入すると本ステージに昇格可能となります。";
        goto label_63;
      }
    }
    nullable8 = nullable1;
    nullable3 = nullable8.HasValue ? new int?(nullable8.GetValueOrDefault() + 1) : new int?();
    int num19 = 3;
    if (nullable3.GetValueOrDefault() == num19 & nullable3.HasValue)
    {
      this.stageWarningNote.text = "7日目の報酬を受け取る時点（受け取らない場合は8日目0時）から10日目23:59までの保留時間内、再度購入可能です。";
    }
    else
    {
      nullable8 = nullable1;
      nullable3 = nullable8.HasValue ? new int?(nullable8.GetValueOrDefault() + 1) : new int?();
      int num20 = 1;
      if (nullable3.GetValueOrDefault() == num20 & nullable3.HasValue)
      {
        this.stageWarningNote.text = "7日目の報酬を受け取る時点（受け取らない場合は8日目0時）から10日目23:59までの保留時間内、ステージIIに昇格可能です。";
      }
      else
      {
        nullable8 = nullable1;
        nullable3 = nullable8.HasValue ? new int?(nullable8.GetValueOrDefault() + 1) : new int?();
        int num21 = 2;
        if (nullable3.GetValueOrDefault() == num21 & nullable3.HasValue)
          this.stageWarningNote.text = "7日目の報酬を受け取る時点（受け取らない場合は8日目0時）から10日目23:59までの保留時間内、ステージIIIに昇格可能です。";
        else
          Debug.LogError((object) ("想定していないステージです: " + (object) nullable1 + (object) 1));
      }
    }
label_63:
    ((UIRect) this.pageUiWidget).alpha = 1f;
  }

  public void OnDetailButton()
  {
    Singleton<NGSceneManager>.GetInstance().StartCoroutine(this.ShowOnDetailButton());
  }

  private IEnumerator ShowOnDetailButton()
  {
    Future<GameObject> detailPopupf = Res.Prefabs.popup.popup_006_3_1__anim_popup01.Load<GameObject>();
    IEnumerator e = detailPopupf.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = Singleton<PopupManager>.GetInstance().open(detailPopupf.Result).GetComponent<Popup00631Menu>().InitGachaDetail("累積パック - " + this.currentStageName.text, this.current_pack_step.descriptions);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void OnBuyOrReceiveButton()
  {
    if (this.currentStageIsBuy)
      this.StartCoroutine(this.DoBuy());
    else
      this.StartCoroutine(this.DoReceive(MasterData.CoinProductList.GetActiveProductData(this.currentProductInfo.ProductId)));
  }

  private IEnumerator DoBuy()
  {
    ShopAccumulation shopAccumulation = this;
    Future<WebAPI.Response.CoinbonusPackVerifyCheck> handler = WebAPI.CoinbonusPackVerifyCheck(2, shopAccumulation.stepupPackInfo.pack.id, shopAccumulation.current_pack_step.pack_set.step);
    IEnumerator e = handler.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (handler.Result != null && PurchaseFlow.Instance.Purchase(shopAccumulation.currentProductInfo.ProductId))
      shopAccumulation.StartCoroutine(PurchaseBehaviorLoadingLayer.Enable());
  }

  private IEnumerator DoReceive(CoinProduct coinProduct)
  {
    PurchaseBehavior.PopupDismiss();
    yield return (object) Singleton<NGSceneManager>.GetInstance().StartCoroutine(Shop0079Menu.OnPurchaseOrReciveSucceeded(false, coinProduct, 0));
  }
}
