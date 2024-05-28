// Decompiled with JetBrains decompiler
// Type: Shop00723Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Shop00723Menu : BackButtonMenuBase
{
  [SerializeField]
  private UILabel txtTitle_;
  [SerializeField]
  private UILabel txtDetail_;
  [SerializeField]
  private UILabel txtExpirationDate_;
  [SerializeField]
  private UILabel txtQuantity_;
  [SerializeField]
  private UILabel txtQuantityDesabled_;
  [SerializeField]
  private UILabel txtCost_;
  [SerializeField]
  private NGTweenParts[] firstTweens_;
  [SerializeField]
  private NGTweenParts[] lastTweens_;
  [SerializeField]
  private Transform animeRoot_;
  [SerializeField]
  private NGxScroll scroll_;
  private bool isInitialized_;
  private Shop00723Scene scene_;
  private PlayerSelectTicketSummary playerUnitTicket_;
  private SM.SelectTicket unitTicket_;
  private SelectTicketSelectSample[] unitSamples_;
  private GameObject prefabUnit_;
  private GameObject prefabConfirmation_;
  private GameObject prefabSkillList_;
  private GameObject prefabLeader_;
  private GameObject prefabSkill_;
  private GameObject prefabIconUnit_;
  private GameObject prefabIconSkill_;
  private GameObject prefabIconElement_;
  private GameObject prefabIconGenre_;
  private GameObject prefabExchange_;
  private GameObject prefabExecute_;
  private GameObject charaAnime;
  private Shop00723UnitSelect[] units_;
  private List<Shop00723UnitSelect> selectlist = new List<Shop00723UnitSelect>();
  private SelectTicketSelectSample currentSample_;
  private UnitTypeEnum currentType_;
  private int quantity_;
  private Shop00723Menu.Phase phase_;
  private GameObject objEffect_;
  private const int QUANTITY_DISPLAY_MAX = 999;
  private const int DEFAULT_UNIT_TYPE = 1;
  private const int MARGIN_FULL_SCREEN_COLLISION = 40;
  private const int DEPTH_FULL_SCREEN_BUTTON = 40;

  public GameObject prefabLeader => this.prefabLeader_;

  public GameObject prefabSkill => this.prefabSkill_;

  public GameObject prefabIconUnit => this.prefabIconUnit_;

  public GameObject prefabIconSkill => this.prefabIconSkill_;

  public GameObject prefabIconElement => this.prefabIconElement_;

  public GameObject prefabIconGenre => this.prefabIconGenre_;

  public IEnumerator coInitialize(
    Shop00723Scene scene,
    SM.SelectTicket unitTicket,
    PlayerSelectTicketSummary playerUnitTicket)
  {
    Shop00723Menu shop00723Menu = this;
    shop00723Menu.scene_ = scene;
    shop00723Menu.playerUnitTicket_ = playerUnitTicket;
    shop00723Menu.unitTicket_ = unitTicket;
    shop00723Menu.phase_ = Shop00723Menu.Phase.Normal;
    shop00723Menu.txtTitle_.SetTextLocalize(string.Format(Consts.GetInstance().SHOP_00723_TITLE_NAME, (object) shop00723Menu.unitTicket_.name));
    string text = shop00723Menu.unitTicket_.end_at.HasValue ? string.Format(Consts.GetInstance().SHOP_00723_EXPIRATION_DATE, (object) shop00723Menu.unitTicket_.end_at) : Consts.GetInstance().SHOP_00723_EXPIRATION_DATE_NONE;
    shop00723Menu.txtExpirationDate_.SetTextLocalize(text);
    shop00723Menu.quantity_ = shop00723Menu.playerUnitTicket_.quantity;
    shop00723Menu.txtCost_.SetTextLocalize(shop00723Menu.unitTicket_.cost);
    // ISSUE: reference to a compiler-generated method
    shop00723Menu.unitSamples_ = MasterData.SelectTicketSelectSample.Select<KeyValuePair<int, SelectTicketSelectSample>, SelectTicketSelectSample>((Func<KeyValuePair<int, SelectTicketSelectSample>, SelectTicketSelectSample>) (kv => kv.Value)).Where<SelectTicketSelectSample>(new Func<SelectTicketSelectSample, bool>(shop00723Menu.\u003CcoInitialize\u003Eb__47_1)).ToArray<SelectTicketSelectSample>();
    if (shop00723Menu.unitTicket_.exchange_limit)
    {
      // ISSUE: reference to a compiler-generated method
      shop00723Menu.unitSamples_ = ((IEnumerable<SelectTicketSelectSample>) shop00723Menu.unitSamples_).OrderBy<SelectTicketSelectSample, bool>(new Func<SelectTicketSelectSample, bool>(shop00723Menu.\u003CcoInitialize\u003Eb__47_2)).ToArray<SelectTicketSelectSample>();
    }
    bool iswait;
    Future<GameObject> ldPrefab;
    IEnumerator e;
    if (Object.op_Equality((Object) shop00723Menu.prefabUnit_, (Object) null))
    {
      iswait = true;
      ldPrefab = Res.Prefabs.shop007_23.dir_unit_exchange_list.Load<GameObject>();
      e = ldPrefab.Wait();
      while (e.MoveNext())
      {
        iswait = false;
        yield return e.Current;
      }
      e = (IEnumerator) null;
      shop00723Menu.prefabUnit_ = ldPrefab.Result;
      if (Object.op_Equality((Object) shop00723Menu.prefabUnit_, (Object) null))
      {
        yield break;
      }
      else
      {
        if (iswait)
          yield return (object) null;
        ldPrefab = (Future<GameObject>) null;
      }
    }
    if (Object.op_Equality((Object) shop00723Menu.prefabConfirmation_, (Object) null))
    {
      iswait = true;
      ldPrefab = Res.Prefabs.popup.popup_007_unit_exchange_confirmation__anim_popup01.Load<GameObject>();
      e = ldPrefab.Wait();
      while (e.MoveNext())
      {
        iswait = false;
        yield return e.Current;
      }
      e = (IEnumerator) null;
      shop00723Menu.prefabConfirmation_ = ldPrefab.Result;
      if (Object.op_Equality((Object) shop00723Menu.prefabConfirmation_, (Object) null))
      {
        yield break;
      }
      else
      {
        if (iswait)
          yield return (object) null;
        ldPrefab = (Future<GameObject>) null;
      }
    }
    if (Object.op_Equality((Object) shop00723Menu.prefabSkillList_, (Object) null))
    {
      iswait = true;
      ldPrefab = Res.Prefabs.shop007_unit_exchange.dir_unit_exchange_skill.Load<GameObject>();
      e = ldPrefab.Wait();
      while (e.MoveNext())
      {
        iswait = false;
        yield return e.Current;
      }
      e = (IEnumerator) null;
      shop00723Menu.prefabSkillList_ = ldPrefab.Result;
      if (Object.op_Equality((Object) shop00723Menu.prefabSkillList_, (Object) null))
      {
        yield break;
      }
      else
      {
        if (iswait)
          yield return (object) null;
        ldPrefab = (Future<GameObject>) null;
      }
    }
    if (Object.op_Equality((Object) shop00723Menu.prefabLeader_, (Object) null))
    {
      iswait = true;
      ldPrefab = Res.Prefabs.shop007_unit_exchange.dir_unit_leader_skill.Load<GameObject>();
      e = ldPrefab.Wait();
      while (e.MoveNext())
      {
        iswait = false;
        yield return e.Current;
      }
      e = (IEnumerator) null;
      shop00723Menu.prefabLeader_ = ldPrefab.Result;
      if (Object.op_Equality((Object) shop00723Menu.prefabLeader_, (Object) null))
      {
        yield break;
      }
      else
      {
        if (iswait)
          yield return (object) null;
        ldPrefab = (Future<GameObject>) null;
      }
    }
    if (Object.op_Equality((Object) shop00723Menu.prefabSkill_, (Object) null))
    {
      iswait = true;
      ldPrefab = Res.Prefabs.shop007_unit_exchange.dir_unit_skill_list.Load<GameObject>();
      e = ldPrefab.Wait();
      while (e.MoveNext())
      {
        iswait = false;
        yield return e.Current;
      }
      e = (IEnumerator) null;
      shop00723Menu.prefabSkill_ = ldPrefab.Result;
      if (Object.op_Equality((Object) shop00723Menu.prefabSkill_, (Object) null))
      {
        yield break;
      }
      else
      {
        if (iswait)
          yield return (object) null;
        ldPrefab = (Future<GameObject>) null;
      }
    }
    if (Object.op_Equality((Object) shop00723Menu.prefabIconUnit_, (Object) null))
    {
      iswait = true;
      ldPrefab = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      e = ldPrefab.Wait();
      while (e.MoveNext())
      {
        iswait = false;
        yield return e.Current;
      }
      e = (IEnumerator) null;
      shop00723Menu.prefabIconUnit_ = ldPrefab.Result;
      if (Object.op_Equality((Object) shop00723Menu.prefabUnit_, (Object) null))
      {
        yield break;
      }
      else
      {
        if (iswait)
          yield return (object) null;
        ldPrefab = (Future<GameObject>) null;
      }
    }
    if (Object.op_Equality((Object) shop00723Menu.prefabIconSkill_, (Object) null))
    {
      iswait = true;
      ldPrefab = Res.Prefabs.BattleSkillIcon._battleSkillIcon.Load<GameObject>();
      e = ldPrefab.Wait();
      while (e.MoveNext())
      {
        iswait = false;
        yield return e.Current;
      }
      e = (IEnumerator) null;
      shop00723Menu.prefabIconSkill_ = ldPrefab.Result;
      if (Object.op_Equality((Object) shop00723Menu.prefabSkill_, (Object) null))
      {
        yield break;
      }
      else
      {
        if (iswait)
          yield return (object) null;
        ldPrefab = (Future<GameObject>) null;
      }
    }
    if (Object.op_Equality((Object) shop00723Menu.prefabIconElement_, (Object) null))
    {
      iswait = true;
      ldPrefab = Res.Icons.CommonElementIcon.Load<GameObject>();
      e = ldPrefab.Wait();
      while (e.MoveNext())
      {
        iswait = false;
        yield return e.Current;
      }
      e = (IEnumerator) null;
      shop00723Menu.prefabIconElement_ = ldPrefab.Result;
      if (Object.op_Equality((Object) shop00723Menu.prefabIconElement_, (Object) null))
      {
        yield break;
      }
      else
      {
        if (iswait)
          yield return (object) null;
        ldPrefab = (Future<GameObject>) null;
      }
    }
    if (Object.op_Equality((Object) shop00723Menu.prefabIconGenre_, (Object) null))
    {
      iswait = true;
      ldPrefab = Res.Icons.SkillGenreIcon.Load<GameObject>();
      e = ldPrefab.Wait();
      while (e.MoveNext())
      {
        iswait = false;
        yield return e.Current;
      }
      e = (IEnumerator) null;
      shop00723Menu.prefabIconGenre_ = ldPrefab.Result;
      if (Object.op_Equality((Object) shop00723Menu.prefabIconGenre_, (Object) null))
      {
        yield break;
      }
      else
      {
        if (iswait)
          yield return (object) null;
        ldPrefab = (Future<GameObject>) null;
      }
    }
    if (Object.op_Equality((Object) shop00723Menu.prefabExchange_, (Object) null))
    {
      iswait = true;
      ldPrefab = Res.Prefabs.popup.popup_007_unit_exchange_confirmation__anim_popup01.Load<GameObject>();
      e = ldPrefab.Wait();
      while (e.MoveNext())
      {
        iswait = false;
        yield return e.Current;
      }
      e = (IEnumerator) null;
      shop00723Menu.prefabExchange_ = ldPrefab.Result;
      if (Object.op_Equality((Object) shop00723Menu.prefabExchange_, (Object) null))
      {
        yield break;
      }
      else
      {
        if (iswait)
          yield return (object) null;
        ldPrefab = (Future<GameObject>) null;
      }
    }
    if (Object.op_Equality((Object) shop00723Menu.prefabExecute_, (Object) null))
    {
      iswait = true;
      ldPrefab = Res.Prefabs.gacha006_effect.gacha_rarity.Load<GameObject>();
      e = ldPrefab.Wait();
      while (e.MoveNext())
      {
        iswait = false;
        yield return e.Current;
      }
      e = (IEnumerator) null;
      shop00723Menu.prefabExecute_ = ldPrefab.Result;
      if (Object.op_Equality((Object) shop00723Menu.prefabExecute_, (Object) null))
      {
        yield break;
      }
      else
      {
        if (iswait)
          yield return (object) null;
        ldPrefab = (Future<GameObject>) null;
      }
    }
    if (Object.op_Equality((Object) shop00723Menu.charaAnime, (Object) null))
    {
      iswait = true;
      ldPrefab = Res.Prefabs.popup.popup_007_unit_exchange_confirmation__anim_chara.Load<GameObject>();
      e = ldPrefab.Wait();
      while (e.MoveNext())
      {
        iswait = false;
        yield return e.Current;
      }
      e = (IEnumerator) null;
      shop00723Menu.charaAnime = ldPrefab.Result;
      if (Object.op_Equality((Object) shop00723Menu.charaAnime, (Object) null))
      {
        yield break;
      }
      else
      {
        if (iswait)
          yield return (object) null;
        ldPrefab = (Future<GameObject>) null;
      }
    }
    e = shop00723Menu.coInitializeUnitSelect();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject gameObject = shop00723Menu.charaAnime.Clone(shop00723Menu.animeRoot_);
    gameObject.transform.position = Vector3.zero;
    gameObject.GetComponent<Shop00723AnimeManager>().SetInfo(shop00723Menu.unitTicket_.description);
    shop00723Menu.updateTicketQuantity();
    shop00723Menu.isInitialized_ = true;
  }

  private void UpdateTicketLimit()
  {
    foreach (Shop00723UnitSelect shop00723UnitSelect in this.selectlist)
      shop00723UnitSelect.UpdateInfo(this.playerUnitTicket_, this.currentSample_, this.quantity_, this.unitTicket_);
  }

  private void updateTicketQuantity(bool usedTicket = false)
  {
    if (usedTicket)
      this.quantity_ -= this.unitTicket_.cost;
    int num1 = this.quantity_ >= this.unitTicket_.cost ? 1 : 0;
    int num2 = Mathf.Clamp(this.quantity_, 0, 999);
    if (num1 != 0)
    {
      this.txtQuantity_.SetTextLocalize(num2);
      ((Component) this.txtQuantity_).gameObject.SetActive(true);
      ((Component) this.txtQuantityDesabled_).gameObject.SetActive(false);
    }
    else
    {
      ((Component) this.txtQuantity_).gameObject.SetActive(false);
      this.txtQuantityDesabled_.SetTextLocalize(num2);
      ((Component) this.txtQuantityDesabled_).gameObject.SetActive(true);
    }
  }

  private IEnumerator coInitializeUnitSelect()
  {
    Shop00723Menu menu = this;
    if (menu.unitSamples_ != null && menu.unitSamples_.Length != 0)
    {
      menu.scroll_.Clear();
      menu.selectlist.Clear();
      menu.units_ = (Shop00723UnitSelect[]) null;
      SelectTicketSelectSample[] ticketSelectSampleArray = menu.unitSamples_;
      for (int index = 0; index < ticketSelectSampleArray.Length; ++index)
      {
        SelectTicketSelectSample unitSample = ticketSelectSampleArray[index];
        Shop00723UnitSelect component = menu.prefabUnit_.Clone().GetComponent<Shop00723UnitSelect>();
        menu.scroll_.Add(((Component) component).gameObject, true);
        menu.selectlist.Add(component);
        IEnumerator e = component.coInitialize(menu, unitSample, menu.playerUnitTicket_, menu.unitTicket_);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      ticketSelectSampleArray = (SelectTicketSelectSample[]) null;
      menu.units_ = menu.selectlist.ToArray();
      menu.scroll_.ResolvePosition();
    }
  }

  public void onClickSkill(SelectTicketSelectSample unit)
  {
    if (!this.isInitialized_ || this.currentSample_ != null || this.IsPushAndSet())
      return;
    this.currentSample_ = unit;
    this.popupSkillMenu();
  }

  private void popupSkillMenu()
  {
    GameObject prefab = this.prefabSkillList_.Clone();
    UnitUnit value = (UnitUnit) null;
    if (!MasterData.UnitUnit.TryGetValue(this.currentSample_.reward_id, out value))
      Debug.LogError((object) ("Key Not Found: " + (object) this.currentSample_.reward_id));
    SelectTicketChoices choice = ((IEnumerable<SelectTicketChoices>) this.unitTicket_.choices).FirstOrDefault<SelectTicketChoices>((Func<SelectTicketChoices, bool>) (x => x.reward_id == value.ID));
    prefab.GetComponent<Shop00723PopupSkillMenu>().initialize(this, this.currentSample_, choice);
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true, isNonSe: true, isNonOpenAnime: true);
  }

  public void onClosedSkill() => this.currentSample_ = (SelectTicketSelectSample) null;

  public void onClickSelect(SelectTicketSelectSample unit)
  {
    if (!this.isInitialized_ || this.currentSample_ != null || this.IsPushAndSet())
      return;
    this.currentSample_ = unit;
    this.StartCoroutine(this.coPopupSelect());
  }

  private IEnumerator coPopupSelect()
  {
    Shop00723Menu menu = this;
    GameObject prefab = menu.prefabExchange_.Clone();
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
    IEnumerator e = prefab.GetComponent<Shop00723PopupExchangeMenu>().coInitialize(menu, menu.currentSample_, menu.playerUnitTicket_, menu.unitTicket_);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onClosedSelect() => this.currentSample_ = (SelectTicketSelectSample) null;

  public void doExchangeUnit(UnitTypeEnum unitType)
  {
    this.currentType_ = unitType;
    this.StartCoroutine(this.coExchangeUnit());
  }

  private IEnumerator coExchangeUnit()
  {
    this.phase_ = Shop00723Menu.Phase.EffectGet;
    GameObject bgo = this.createFullScreenButton();
    Singleton<PopupManager>.GetInstance().open(bgo, true, isCloned: true, isNonSe: true);
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
    Future<WebAPI.Response.SelectticketSpend> future = WebAPI.SelectticketSpend(this.currentSample_.ID, this.currentType_ != UnitTypeEnum.random ? (int) this.currentType_ : 1, 1, (Action<WebAPI.Response.UserError>) (e =>
    {
      Singleton<PopupManager>.GetInstance().closeAll();
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e1 = future.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    WebAPI.Response.SelectticketSpend result = future.Result;
    if (result != null)
    {
      e1 = OnDemandDownload.WaitLoadUnitResource(((IEnumerable<PlayerUnit>) result.player_units).Select<PlayerUnit, UnitUnit>((Func<PlayerUnit, UnitUnit>) (x => x.unit)), false);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      PlayerSelectTicketSummary[] source = SMManager.Get<PlayerSelectTicketSummary[]>();
      this.playerUnitTicket_ = this.unitTicket_.id != 0 ? ((IEnumerable<PlayerSelectTicketSummary>) source).FirstOrDefault<PlayerSelectTicketSummary>((Func<PlayerSelectTicketSummary, bool>) (pt => pt.ticket_id == this.unitTicket_.id)) : ((IEnumerable<PlayerSelectTicketSummary>) source).FirstOrDefault<PlayerSelectTicketSummary>();
      if (this.unitTicket_.exchange_limit)
      {
        this.unitSamples_ = MasterData.SelectTicketSelectSample.Select<KeyValuePair<int, SelectTicketSelectSample>, SelectTicketSelectSample>((Func<KeyValuePair<int, SelectTicketSelectSample>, SelectTicketSelectSample>) (kv => kv.Value)).Where<SelectTicketSelectSample>((Func<SelectTicketSelectSample, bool>) (us => us.ticketID == this.unitTicket_.id)).ToArray<SelectTicketSelectSample>();
        this.unitSamples_ = ((IEnumerable<SelectTicketSelectSample>) this.unitSamples_).OrderBy<SelectTicketSelectSample, bool>((Func<SelectTicketSelectSample, bool>) (x => this.GetExchangeCount(x.reward_id) == 0)).ToArray<SelectTicketSelectSample>();
      }
      this.updateTicketQuantity(true);
      if (this.isChangeOver())
      {
        for (int index = 0; index < this.selectlist.Count; ++index)
        {
          if (this.selectlist[index].sample_.ID == this.currentSample_.ID)
          {
            this.selectlist[index].SubtractLimitCount(1);
            break;
          }
        }
      }
      else
      {
        e1 = this.coInitializeUnitSelect();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        this.UpdateTicketLimit();
      }
      e1 = this.coPlayEffect(result);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      UIButton nextButton = bgo.GetComponent<UIButton>();
      EventDelegate.Set(nextButton.onClick, (EventDelegate.Callback) (() =>
      {
        if (!((Behaviour) nextButton).enabled)
          return;
        GachaResultData.GetInstance().SetData(result);
        Singleton<NGSceneManager>.GetInstance().changeScene("gacha006_8", true, (object) ((IEnumerable<PlayerUnit>) result.player_units).FirstOrDefault<PlayerUnit>(), (object) result.is_new, (object) false);
        ((Behaviour) nextButton).enabled = false;
      }));
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      ((Behaviour) nextButton).enabled = true;
      while (((Behaviour) nextButton).enabled)
        yield return (object) null;
      Singleton<PopupManager>.GetInstance().dismiss();
      this.deleteEffect();
      ShopTicketExchangeMenu.IsUpdate = true;
    }
  }

  private void OnEnable()
  {
    if (this.phase_ != Shop00723Menu.Phase.EffectGet)
      return;
    this.StartCoroutine(this.coWaitDateTime());
  }

  private IEnumerator coWaitDateTime()
  {
    Shop00723Menu shop00723Menu = this;
    shop00723Menu.IsPush = true;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    IEnumerator e = shop00723Menu.scene_.coCheckTicketDateTime(shop00723Menu.unitTicket_);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (shop00723Menu.scene_.isErrorTicketDateTime_)
    {
      e = shop00723Menu.scene_.coEndSceneErrorTicketDateTime();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      shop00723Menu.phase_ = Shop00723Menu.Phase.Normal;
      shop00723Menu.IsPush = false;
      shop00723Menu.currentSample_ = (SelectTicketSelectSample) null;
    }
  }

  private void deleteEffect()
  {
    if (!Object.op_Inequality((Object) this.objEffect_, (Object) null))
      return;
    Object.Destroy((Object) this.objEffect_);
    this.objEffect_ = (GameObject) null;
  }

  public void IsChangeOverBackScene()
  {
    if (!this.isChangeOver())
      return;
    this.scene_.shopFinish();
    this.backScene();
  }

  private bool isChangeOver() => this.unitTicket_ != null && this.quantity_ < this.unitTicket_.cost;

  private void OnDestroy() => this.deleteEffect();

  private IEnumerator coPlayEffect(WebAPI.Response.SelectticketSpend result)
  {
    EffectControllerCouponExchange ce = this.prefabExecute_.Clone().GetComponent<EffectControllerCouponExchange>();
    IEnumerator e = ce.coExchangeUnit(result);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    bool bwait = true;
    ce.callbackOnFinishedEffect_ = (EventDelegate.Callback) (() => bwait = false);
    while (bwait)
      yield return (object) null;
    this.objEffect_ = ((Component) ce).gameObject;
  }

  private GameObject createFullScreenButton()
  {
    GameObject fullScreenButton = new GameObject("btnFullScreen");
    fullScreenButton.AddComponent<UIWidget>().depth = 40;
    fullScreenButton.AddComponent<BoxCollider>().size = Vector2.op_Implicit(new Vector2((float) (Screen.width + 40), (float) (Screen.height + 40)));
    ((Behaviour) fullScreenButton.AddComponent<UIButton>()).enabled = false;
    return fullScreenButton;
  }

  private int GetExchangeCount(int rewardID)
  {
    int exchangeCount = 0;
    SelectTicketChoices selectTicketChoices = ((IEnumerable<SelectTicketChoices>) this.unitTicket_.choices).FirstOrDefault<SelectTicketChoices>((Func<SelectTicketChoices, bool>) (u => u.reward_id == rewardID));
    if (selectTicketChoices != null)
    {
      PlayerSelectTicketSummary[] source = SMManager.Get<PlayerSelectTicketSummary[]>();
      PlayerSelectTicketSummaryPlayer_exchange_count_list exchangeCountList = (PlayerSelectTicketSummaryPlayer_exchange_count_list) null;
      this.playerUnitTicket_ = this.unitTicket_.id != 0 ? ((IEnumerable<PlayerSelectTicketSummary>) source).FirstOrDefault<PlayerSelectTicketSummary>((Func<PlayerSelectTicketSummary, bool>) (pt => pt.ticket_id == this.unitTicket_.id)) : ((IEnumerable<PlayerSelectTicketSummary>) source).FirstOrDefault<PlayerSelectTicketSummary>();
      if (this.playerUnitTicket_ != null)
        exchangeCountList = ((IEnumerable<PlayerSelectTicketSummaryPlayer_exchange_count_list>) this.playerUnitTicket_.player_exchange_count_list).FirstOrDefault<PlayerSelectTicketSummaryPlayer_exchange_count_list>((Func<PlayerSelectTicketSummaryPlayer_exchange_count_list, bool>) (u => u.reward_id == rewardID));
      exchangeCount = exchangeCountList == null || !selectTicketChoices.exchangeable_count.HasValue ? (!selectTicketChoices.exchangeable_count.HasValue ? int.MaxValue : selectTicketChoices.exchangeable_count.Value) : selectTicketChoices.exchangeable_count.Value - exchangeCountList.exchange_count;
    }
    return exchangeCount;
  }

  public override void onBackButton() => this.OnIbtnBack();

  public void OnIbtnBack()
  {
    if (this.phase_ != Shop00723Menu.Phase.Normal || this.IsPushAndSet())
      return;
    this.scene_.shopFinish();
    this.backScene();
  }

  private enum Phase
  {
    Normal,
    EffectGet,
  }
}
