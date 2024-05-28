// Decompiled with JetBrains decompiler
// Type: HotDeal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using Gsc.Purchase;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class HotDeal : BackButtonMenuBase
{
  private UIButton itbn_Buy;
  private UIButton ibtn_Specific;
  private UIButton ibtn_Fonds;
  private UIButton ibtn_Detail;
  private UIButton itbn_Popup_Back;
  private UILabel txt_EndingTime;
  private UILabel txt_AfterDiscount;
  private UILabel txt_Attention;
  private UI2DSprite slc_MainImage;
  private HotDealInfo HotDealInfo;
  private bool isInitialized;
  private Animator animator;
  private int? itemNum;
  private Transform iconRoot;
  private DateTime endDateTime;

  private Type FindComponent<Type>(string path)
  {
    return ((Component) ((Component) this).transform.Find(path)).GetComponent<Type>();
  }

  private static Type FindComponent<Type>(Transform trans, string path)
  {
    Transform transform = trans.Find(path);
    return Object.op_Equality((Object) transform, (Object) null) ? default (Type) : ((Component) transform).GetComponent<Type>();
  }

  public IEnumerator Init(GameObject withLoupeIconPrefab, HotDealInfo hotDealInfo)
  {
    HotDeal hotDeal = this;
    hotDeal.HotDealInfo = hotDealInfo;
    hotDeal.animator = ((Component) hotDeal).GetComponent<Animator>();
    hotDeal.IsPush = true;
    hotDeal.itemNum = hotDeal.HotDealInfo.rewards?.Length;
    if (!hotDeal.itemNum.HasValue)
      hotDeal.itemNum = new int?(0);
    hotDeal.Attach();
    IEnumerator e = hotDeal.ChangeMainImage();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    string productId = hotDeal.HotDealInfo.GetProductId();
    ProductInfo productInfo = PurchaseFlow.ProductList != null ? ((IEnumerable<ProductInfo>) PurchaseFlow.ProductList).FirstOrDefault<ProductInfo>((Func<ProductInfo, bool>) (x => x.ProductId == productId)) : (ProductInfo) null;
    if (productInfo != null)
      hotDeal.SetAfterDiscountText(productInfo.LocalizedPrice);
    else
      hotDeal.SetAfterDiscountText("");
    hotDeal.endDateTime = hotDeal.HotDealInfo.EndDateTime;
    hotDeal.txt_Attention.text = Consts.GetInstance().HOT_DEAL_PAID_TEXT.F((object) hotDeal.HotDealInfo.GetAdditionalPaidCoin());
    e = hotDeal.InitItemIcon(withLoupeIconPrefab);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    hotDeal.IsPush = false;
    hotDeal.isInitialized = true;
  }

  private void Attach()
  {
    this.itbn_Buy = this.FindComponent<UIButton>("dir_parent/itbn_Buy");
    this.ibtn_Specific = this.FindComponent<UIButton>("dir_parent/ibtn_Specific");
    this.ibtn_Fonds = this.FindComponent<UIButton>("dir_parent/ibtn_Fonds");
    this.ibtn_Detail = this.FindComponent<UIButton>("dir_parent/ibtn_Detail");
    this.itbn_Popup_Back = this.FindComponent<UIButton>("dir_parent/itbn_Popup_Back");
    this.txt_EndingTime = this.FindComponent<UILabel>("dir_parent/slc_MainImage/dir_txt/txt_EndingTime");
    this.txt_AfterDiscount = this.FindComponent<UILabel>("dir_parent/itbn_Buy/txt_AfterDiscount");
    this.txt_Attention = this.FindComponent<UILabel>("dir_parent/slc_Base_Himeishi/txt_Attention");
    this.slc_MainImage = this.FindComponent<UI2DSprite>("dir_parent/slc_MainImage");
    this.iconRoot = this.FindComponent<Transform>("dir_parent/dir_RewardImage/slc_Treasure_sum_" + this.itemNum.ToString());
    this.itbn_Buy.onClick.Clear();
    EventDelegate.Add(this.itbn_Buy.onClick, (EventDelegate.Callback) (() => this.StartCoroutine(this.DoBuy())));
    this.ibtn_Specific.onClick.Clear();
    EventDelegate.Add(this.ibtn_Specific.onClick, (EventDelegate.Callback) (() =>
    {
      if (this.IsPushAndSet())
        return;
      this.StartCoroutine(this.IsPushOff());
      this.StartCoroutine(PopupUtility._007_18());
    }));
    this.ibtn_Fonds.onClick.Clear();
    EventDelegate.Add(this.ibtn_Fonds.onClick, (EventDelegate.Callback) (() =>
    {
      if (this.IsPushAndSet())
        return;
      this.StartCoroutine(this.IsPushOff());
      this.StartCoroutine(PopupUtility._007_19());
    }));
    this.ibtn_Detail.onClick.Clear();
    EventDelegate.Add(this.ibtn_Detail.onClick, (EventDelegate.Callback) (() =>
    {
      if (this.IsPushAndSet())
        return;
      this.StartCoroutine(this.IsPushOff());
      this.StartCoroutine(this.ShowItemDetailPopup());
    }));
    this.itbn_Popup_Back.onClick.Clear();
    EventDelegate.Add(this.itbn_Popup_Back.onClick, (EventDelegate.Callback) (() => this.onBackButton()));
  }

  private IEnumerator ChangeMainImage()
  {
    string modalResourceName = this.HotDealInfo.ModalResourceName;
    if (!string.IsNullOrEmpty(modalResourceName))
    {
      Future<Sprite> f = Singleton<ResourceManager>.GetInstance().Load<Sprite>("Prefabs/HotDeal/" + modalResourceName + "/slc_mount");
      IEnumerator e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.slc_MainImage.sprite2D = f.Result;
      ((UIWidget) this.slc_MainImage).MakePixelPerfect();
    }
  }

  private IEnumerator InitItemIcon(GameObject withLoupeIconPrefab)
  {
    int index = 0;
    foreach (Transform parent in this.iconRoot)
    {
      ((Component) parent).gameObject.SetActive(false);
      foreach (Transform transform in parent)
      {
        ((Component) transform).gameObject.SetActive(false);
        EffectSE component = ((Component) transform).GetComponent<EffectSE>();
        if (Object.op_Inequality((Object) component, (Object) null))
        {
          component.playOnStart = false;
          component.playOnEnable = false;
        }
      }
      HotDealRewardSchema reward = this.HotDealInfo.rewards[index];
      IEnumerator e = this.CreateIcon(withLoupeIconPrefab, parent, reward);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ++index;
    }
  }

  private IEnumerator CreateIcon(
    GameObject baseIcon,
    Transform parent,
    HotDealRewardSchema rewards)
  {
    HotDeal hotDeal = this;
    GameObject icon = baseIcon.Clone(parent);
    IEnumerator e = icon.GetComponent<ItemIconDetail>().Init((MasterDataTable.CommonRewardType) rewards.reward_type_id, rewards.reward_id, rewards.reward_quantity, iconInfo: new TransformSizeInfo(0.54f, 46f, -48f), quantityInfo: new TransformSizeInfo(1f, 36f, -90f), quantitySpriteInfo: new SpriteTransformSizeInfo(10f, 4f, 26, 72), stoneInfo: new TransformSizeInfo(1f, 54.5f, -11f), openDetailValidate: new Func<bool>(hotDeal.OpenDetailValidate));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    icon.transform.localPosition = new Vector3(-46f, 48f, 0.0f);
  }

  private IEnumerator WaitForUIAnimationFinish(int layer, Action finishCallback = null)
  {
    if (Object.op_Inequality((Object) this.animator, (Object) null))
    {
      AnimatorStateInfo animatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(layer);
      int shortNameHash = ((AnimatorStateInfo) ref animatorStateInfo).shortNameHash;
      int shortNameHash1;
      while (Object.op_Inequality((Object) this.animator, (Object) null))
      {
        animatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(layer);
        shortNameHash1 = ((AnimatorStateInfo) ref animatorStateInfo).shortNameHash;
        if (shortNameHash1.Equals(shortNameHash))
          yield return (object) null;
        else
          break;
      }
      int num;
      if (!Object.op_Inequality((Object) this.animator, (Object) null))
      {
        num = -1;
      }
      else
      {
        animatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(layer);
        num = ((AnimatorStateInfo) ref animatorStateInfo).shortNameHash;
      }
      shortNameHash = num;
      while (Object.op_Inequality((Object) this.animator, (Object) null))
      {
        animatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(layer);
        if ((double) ((AnimatorStateInfo) ref animatorStateInfo).normalizedTime < 1.0)
        {
          animatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(layer);
          shortNameHash1 = ((AnimatorStateInfo) ref animatorStateInfo).shortNameHash;
          if (!shortNameHash1.Equals(shortNameHash))
            yield return (object) null;
          else
            break;
        }
        else
          break;
      }
    }
    if (finishCallback != null)
      finishCallback();
  }

  public void PlayCloseAnimation(Action finishCallback, bool se = true)
  {
    this.StopParticleEffect();
    if (Object.op_Equality((Object) this.animator, (Object) null))
      return;
    this.animator.SetTrigger("isClose");
    if (se)
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1004");
    if (finishCallback == null)
      return;
    this.StartCoroutine(this.WaitForUIAnimationFinish(0, finishCallback));
  }

  public void PlayItemIconAnimation() => this.DoItemIconAnimation();

  private void DoItemIconAnimation()
  {
    ((Component) this.iconRoot).gameObject.SetActive(true);
    foreach (Transform transform in this.iconRoot)
    {
      ((Component) transform).gameObject.SetActive(true);
      float delay = 0.0f;
      TweenPosition component1 = ((Component) transform).GetComponent<TweenPosition>();
      if (Object.op_Inequality((Object) component1, (Object) null))
      {
        ((UITweener) component1).ResetToBeginning();
        ((UITweener) component1).PlayForward();
      }
      TweenScale component2 = ((Component) transform).GetComponent<TweenScale>();
      if (Object.op_Inequality((Object) component2, (Object) null))
      {
        ((UITweener) component2).ResetToBeginning();
        ((UITweener) component2).PlayForward();
      }
      TweenAlpha component3 = ((Component) transform).GetComponent<TweenAlpha>();
      if (Object.op_Inequality((Object) component3, (Object) null))
      {
        delay = ((UITweener) component3).delay;
        ((UITweener) component3).ResetToBeginning();
        ((UITweener) component3).PlayForward();
      }
      this.StartCoroutine(this.DoItemIconEffectAnimation(delay, transform.GetChild(0)));
    }
  }

  private IEnumerator DoItemIconEffectAnimation(float delay, Transform effTrans)
  {
    yield return (object) new WaitForSeconds(delay);
    ((Component) effTrans).gameObject.SetActive(true);
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.PlayCloseAnimation((Action) (() => Singleton<PopupManager>.GetInstance().onDismiss()));
  }

  private IEnumerator DoBuy()
  {
    HotDeal hotDeal = this;
    if (!hotDeal.IsPushAndSet())
    {
      HotDealPurchaseView hotDealPurchaseView = ((Component) hotDeal).gameObject.GetOrAddComponent<HotDealPurchaseView>();
      IEnumerator e = hotDealPurchaseView.Init();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      while (!hotDealPurchaseView.IsOnProducts)
      {
        if (hotDealPurchaseView.IsInputBirthday)
        {
          hotDeal.IsPush = false;
          yield break;
        }
        else
          yield return (object) null;
      }
      Future<WebAPI.Response.HotdealPackVerifyCheck> handler = WebAPI.HotdealPackVerifyCheck(hotDeal.HotDealInfo.pack_id);
      e = handler.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (handler.Result == null)
      {
        PurchaseBehaviorLoadingLayer.Disable();
        while (Singleton<PopupManager>.GetInstance().ModalWindowIsOpen)
          yield return (object) null;
        hotDeal.IsPush = false;
      }
      else
      {
        Singleton<NGGameDataManager>.GetInstance().PurchaseHotDeal = hotDeal.HotDealInfo;
        hotDeal.StartCoroutine(hotDeal.IsPushOff());
        PurchaseFlow.Instance.Purchase(hotDeal.HotDealInfo.GetProductId());
      }
    }
  }

  protected override void Update()
  {
    if (!this.isInitialized)
      return;
    base.Update();
    this.CheckEndingTimeClose();
    this.UpdateEndingTime();
  }

  private void CheckEndingTimeClose()
  {
    if ((this.endDateTime - DateTime.Now).Seconds >= 0)
      return;
    this.onBackButton();
  }

  private void UpdateEndingTime()
  {
    if (Object.op_Equality((Object) this.txt_EndingTime, (Object) null))
      return;
    TimeSpan timeSpan = this.endDateTime - DateTime.Now;
    string placeholder = Consts.GetInstance().HOT_DEAL_TIME_REMAINING_S;
    if (timeSpan.Days > 0)
      placeholder = Consts.GetInstance().HOT_DEAL_TIME_REMAINING_DHMS;
    else if (timeSpan.Hours > 0)
      placeholder = Consts.GetInstance().HOT_DEAL_TIME_REMAINING_HMS;
    else if (timeSpan.Minutes > 0)
      placeholder = Consts.GetInstance().HOT_DEAL_TIME_REMAINING_MS;
    int num = Mathf.Max(0, timeSpan.Seconds);
    this.txt_EndingTime.text = Consts.Format(placeholder, (IDictionary) new Hashtable()
    {
      {
        (object) "day",
        (object) timeSpan.Days
      },
      {
        (object) "hour",
        (object) timeSpan.Hours
      },
      {
        (object) "minute",
        (object) timeSpan.Minutes
      },
      {
        (object) "second",
        (object) num
      }
    });
  }

  private void SetAfterDiscountText(string text)
  {
    if (Object.op_Equality((Object) this.txt_AfterDiscount, (Object) null))
      return;
    Dictionary<char, string> dictionary = new Dictionary<char, string>()
    {
      {
        '０',
        "0"
      },
      {
        '１',
        "1"
      },
      {
        '２',
        "2"
      },
      {
        '３',
        "3"
      },
      {
        '４',
        "4"
      },
      {
        '５',
        "5"
      },
      {
        '６',
        "6"
      },
      {
        '７',
        "7"
      },
      {
        '８',
        "8"
      },
      {
        '９',
        "9"
      }
    };
    this.txt_AfterDiscount.text = "";
    for (int index = 0; index < text.Length; ++index)
    {
      if (dictionary.ContainsKey(text[index]))
        this.txt_AfterDiscount.text += dictionary[text[index]];
      else
        this.txt_AfterDiscount.text += text[index].ToString();
    }
  }

  private IEnumerator ShowItemDetailPopup()
  {
    Future<GameObject> detailPopupf = Res.Prefabs.popup.popup_006_3_1__anim_popup01.Load<GameObject>();
    IEnumerator e = detailPopupf.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Popup00631Menu component = Singleton<PopupManager>.GetInstance().open(detailPopupf.Result).GetComponent<Popup00631Menu>();
    PackDescription[] bodys = this.HotDealInfo != null ? this.HotDealInfo.descriptions : new List<PackDescription>().ToArray();
    string title = this.HotDealInfo?.GetProductName() ?? "";
    e = component.InitGachaDetail(title, bodys);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private void StopParticleEffect()
  {
    List<ParticleSystem> particleSystemList = new List<ParticleSystem>();
    ParticleSystem component = ((Component) ((Component) this).transform.Find("dir_parent/eff_Particle")).GetComponent<ParticleSystem>();
    if (Object.op_Inequality((Object) component, (Object) null))
    {
      particleSystemList.Add(component);
      foreach (ParticleSystem componentsInChild in ((Component) ((Component) component).transform).GetComponentsInChildren<ParticleSystem>())
        particleSystemList.Add(componentsInChild);
    }
    particleSystemList.ForEach((Action<ParticleSystem>) (p =>
    {
      p.Stop(true, (ParticleSystemStopBehavior) 0);
      ((Component) p).gameObject.SetActive(false);
    }));
  }

  private bool OpenDetailValidate()
  {
    if (this.IsPushAndSet())
      return false;
    this.StartCoroutine(this.IsPushOff());
    return true;
  }
}
