// Decompiled with JetBrains decompiler
// Type: HotDealButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class HotDealButton : MypageEventButton
{
  private UILabel txt_Time;
  private UILabel txt_Time_eff;
  private UILabel txt_Time_Unit;
  private UISprite slc_HotDeal_base;
  private UISprite slc_TitleBase;
  private UISprite slc_Premium;
  private UISprite slc_Premium_out;
  private UISprite slc_Sale;
  private UISprite slc_Sale_out;
  private List<ParticleSystem> effectList;
  private HotDealInfo CurrentHotDealInfo;

  private static Type FindComponent<Type>(Transform trans, string path)
  {
    Transform transform = trans.Find(path);
    return Object.op_Equality((Object) transform, (Object) null) ? default (Type) : ((Component) transform).GetComponent<Type>();
  }

  public void StartEffect()
  {
    if (this.effectList == null)
      return;
    for (int index = 0; index < this.effectList.Count; ++index)
    {
      ParticleSystem.MainModule main = this.effectList[index].main;
      ((ParticleSystem.MainModule) ref main).simulationSpeed = 1f;
    }
  }

  private void StopEffect()
  {
    if (this.effectList == null)
      return;
    for (int index = 0; index < this.effectList.Count; ++index)
    {
      ParticleSystem.MainModule main = this.effectList[index].main;
      ((ParticleSystem.MainModule) ref main).simulationSpeed = 0.0f;
    }
  }

  private void Awake()
  {
    this.Attach();
    this.effectList = new List<ParticleSystem>();
    string[] strArray = new string[3]
    {
      "dir_eff/eff_kirakira",
      "dir_eff/dir_eff_start/eff_kira_Sparke_1",
      "dir_eff/dir_eff_start/eff_kira_Sparke_2"
    };
    foreach (string path in strArray)
    {
      ParticleSystem component = HotDealButton.FindComponent<ParticleSystem>(((Component) this).transform, path);
      if (Object.op_Inequality((Object) component, (Object) null))
        this.effectList.Add(component);
    }
  }

  private void OnEnable() => this.StopEffect();

  private void OnDisable() => this.StopEffect();

  private void Update()
  {
    if (this.CurrentHotDealInfo != null && !this.CurrentHotDealInfo.IsActive)
      this.UpdateButtonState();
    else
      this.UpdateTime();
  }

  private void UpdateCurrentInfo()
  {
    this.CurrentHotDealInfo = ((IEnumerable<HotDealInfo>) Singleton<NGGameDataManager>.GetInstance().HotDealInfo).FirstOrDefault<HotDealInfo>((Func<HotDealInfo, bool>) (x => x.IsActive));
  }

  private void UpdateTime()
  {
    if (Object.op_Equality((Object) this.txt_Time, (Object) null) || Object.op_Equality((Object) this.txt_Time_eff, (Object) null) || Object.op_Equality((Object) this.txt_Time_Unit, (Object) null) || this.CurrentHotDealInfo == null)
      return;
    TimeSpan timeSpan = this.CurrentHotDealInfo.EndDateTime - DateTime.Now;
    if (timeSpan.Days > 0)
    {
      this.txt_Time_Unit.text = Consts.GetInstance().HOT_DEAL_BUTTON_TIME_REMAINING_D;
      this.txt_Time.text = timeSpan.Days.ToString();
      this.txt_Time_eff.text = timeSpan.Days.ToString();
    }
    else if (timeSpan.Hours > 0)
    {
      this.txt_Time_Unit.text = Consts.GetInstance().HOT_DEAL_BUTTON_TIME_REMAINING_H;
      this.txt_Time.text = timeSpan.Hours.ToString();
      this.txt_Time_eff.text = timeSpan.Hours.ToString();
    }
    else if (timeSpan.Minutes > 0)
    {
      this.txt_Time_Unit.text = Consts.GetInstance().HOT_DEAL_BUTTON_TIME_REMAINING_M;
      this.txt_Time.text = timeSpan.Minutes.ToString();
      this.txt_Time_eff.text = timeSpan.Minutes.ToString();
    }
    else
    {
      int num = Mathf.Max(0, timeSpan.Seconds);
      this.txt_Time_Unit.text = Consts.GetInstance().HOT_DEAL_BUTTON_TIME_REMAINING_S;
      this.txt_Time.text = num.ToString();
      this.txt_Time_eff.text = num.ToString();
    }
  }

  private void Attach()
  {
    TweenRotation component1 = HotDealButton.FindComponent<TweenRotation>(((Component) this).transform, "dir_HotDeal/slc_HotDeal_base");
    if (Object.op_Inequality((Object) component1, (Object) null))
      ((Behaviour) component1).enabled = true;
    TweenScale component2 = HotDealButton.FindComponent<TweenScale>(((Component) this).transform, "dir_HotDeal/dir_txt/txt_Time_eff");
    if (Object.op_Inequality((Object) component2, (Object) null))
      ((Behaviour) component2).enabled = true;
    TweenColor component3 = HotDealButton.FindComponent<TweenColor>(((Component) this).transform, "dir_HotDeal/dir_txt/txt_Time_eff");
    if (Object.op_Inequality((Object) component3, (Object) null))
      ((Behaviour) component3).enabled = true;
    this.txt_Time = HotDealButton.FindComponent<UILabel>(((Component) this).transform, "dir_HotDeal/dir_txt/txt_Time");
    this.txt_Time_eff = HotDealButton.FindComponent<UILabel>(((Component) this).transform, "dir_HotDeal/dir_txt/txt_Time_eff");
    this.txt_Time_Unit = HotDealButton.FindComponent<UILabel>(((Component) this).transform, "dir_HotDeal/dir_txt/txt_Time_Unit");
    this.slc_HotDeal_base = HotDealButton.FindComponent<UISprite>(((Component) this).transform, "dir_HotDeal/slc_HotDeal_base");
    this.slc_TitleBase = HotDealButton.FindComponent<UISprite>(((Component) this).transform, "dir_HotDeal/slc_TitleBase");
    this.slc_Premium = HotDealButton.FindComponent<UISprite>(((Component) this).transform, "dir_HotDeal/slc_Premium");
    this.slc_Premium_out = HotDealButton.FindComponent<UISprite>(((Component) this).transform, "dir_HotDeal/slc_Premium/slc_Premium_out");
    this.slc_Sale = HotDealButton.FindComponent<UISprite>(((Component) this).transform, "dir_HotDeal/slc_Sale");
    this.slc_Sale_out = HotDealButton.FindComponent<UISprite>(((Component) this).transform, "dir_HotDeal/slc_Sale/slc_Sale_out");
  }

  private void ChangeIconImage()
  {
    HotdealPack hotdealPack = this.CurrentHotDealInfo.GetHotdealPack();
    if (hotdealPack == null)
      return;
    this.slc_HotDeal_base.spriteName = string.Format("slc_HotDeal_base_{0}", (object) hotdealPack.icon_resource_name);
    ((UIWidget) this.slc_HotDeal_base).MakePixelPerfect();
    this.slc_TitleBase.spriteName = string.Format("slc_TitleBase_{0}", (object) hotdealPack.icon_shadow_text_resource_name);
    ((UIWidget) this.slc_TitleBase).MakePixelPerfect();
    this.slc_Premium.spriteName = string.Format("slc_Premium_{0}", (object) hotdealPack.icon_top_text_resource_name);
    ((UIWidget) this.slc_Premium).MakePixelPerfect();
    this.slc_Premium_out.spriteName = string.Format("slc_Premium_{0}", (object) hotdealPack.icon_top_text_resource_name);
    ((UIWidget) this.slc_Premium_out).MakePixelPerfect();
    this.slc_Sale.spriteName = string.Format("slc_Sale_{0}", (object) hotdealPack.icon_bottom_text_resource_name);
    ((UIWidget) this.slc_Sale).MakePixelPerfect();
    this.slc_Sale_out.spriteName = string.Format("slc_Sale_{0}", (object) hotdealPack.icon_bottom_text_resource_name);
    ((UIWidget) this.slc_Sale_out).MakePixelPerfect();
  }

  public void OnPushButton()
  {
    NGSceneBase sceneBase = Singleton<NGSceneManager>.GetInstance().sceneBase;
    if (sceneBase.IsPush)
      return;
    sceneBase.IsPush = true;
    HotDealGenerator hotDealGenerator = new HotDealGenerator();
    this.StartCoroutine(this.OpenHotdealSelectPopup());
  }

  private IEnumerator OpenHotdealSelectPopup()
  {
    Future<GameObject> loader = new ResourceObject("Prefabs/HotDeal/popup_HotDeal_PremiumSale").Load<GameObject>();
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = loader.Result;
    PopupHotDealSelect popup = Singleton<PopupManager>.GetInstance().open(result, isNonSe: true, isNonOpenAnime: true).GetComponent<PopupHotDealSelect>();
    e = popup.Initialize(((IEnumerable<HotDealInfo>) Singleton<NGGameDataManager>.GetInstance().HotDealInfo).Where<HotDealInfo>((Func<HotDealInfo, bool>) (x => x.IsActive)).ToArray<HotDealInfo>());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().startOpenAnime(((Component) popup).gameObject);
    Singleton<NGSceneManager>.GetInstance().sceneBase.IsPush = false;
  }

  public override void UpdateButtonState()
  {
    HotDealInfo currentHotDealInfo = this.CurrentHotDealInfo;
    if (this.IsActive())
    {
      this.SetActive(true);
      if (currentHotDealInfo != this.CurrentHotDealInfo)
      {
        this.ChangeIconImage();
        this.UpdateTime();
      }
      if (this.IsBadge())
        this.SetBadgeActive(true);
      else
        this.SetBadgeActive(false);
    }
    else
      this.SetActive(false);
  }

  public override bool IsActive()
  {
    this.UpdateCurrentInfo();
    return this.CurrentHotDealInfo != null;
  }

  public override bool IsBadge() => false;
}
