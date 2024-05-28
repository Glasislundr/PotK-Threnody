// Decompiled with JetBrains decompiler
// Type: Shop00720Menu
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
public class Shop00720Menu : BackButtonMenuBase
{
  [SerializeField]
  private GameObject Title;
  [SerializeField]
  private GameObject IbtnBack;
  [SerializeField]
  private GameObject MedalInfo;
  [SerializeField]
  private GameObject IbtnCheckReward;
  [SerializeField]
  private GameObject IbtnSkip;
  [SerializeField]
  private Shop00720Menu.SlotButtonSetting[] SlotButtons;
  private GameObject slot;
  private Shop00720EffectController effect;
  private Shop00720Prefabs prefabs;
  private bool isInitalize;
  private bool isWaitForShot = true;
  private int currentSlot;
  private int currentDeck;
  private List<UITweener> tweeners = new List<UITweener>();
  private List<SlotModuleSlot> slotModuleSlots = new List<SlotModuleSlot>();
  private const int NEED_LOADING = 100;

  public IEnumerator Initialize()
  {
    Shop00720Menu menu = this;
    menu.currentSlot = 0;
    menu.SetEventOnClick();
    Future<GameObject> fPopUp = Res.Animations.Slot_Machines.machines.Load<GameObject>();
    IEnumerator e = fPopUp.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    menu.slot = Object.Instantiate<GameObject>(fPopUp.Result);
    menu.effect = menu.slot.GetComponent<Shop00720EffectController>();
    if (menu.prefabs == null)
    {
      menu.prefabs = new Shop00720Prefabs();
      e = menu.prefabs.GetPrefabs();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    menu.SetTweeners(new List<GameObject>()
    {
      menu.Title,
      menu.IbtnBack,
      menu.MedalInfo
    });
    menu.effect.textureNameList_1 = menu.GetReelPattern(1);
    menu.effect.textureNameList_2 = menu.GetReelPattern(2);
    menu.effect.textureNameList_3 = menu.GetReelPattern(3);
    UnitUnit shopTopUnit = ShopTopUnit.GetShopTopUnit();
    PlayerUnit unit = shopTopUnit == null ? menu.GetDisplayPlayerUnit() : PlayerUnit.create_by_unitunit(shopTopUnit);
    e = menu.effect.CutInInitialize(unit, menu);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private PlayerUnit GetDisplayPlayerUnit()
  {
    int mypage_unit_id = MypageUnitUtil.getUnitId();
    if (mypage_unit_id == 0)
      return this.GetDeckLeaderPlayerUnit();
    PlayerUnit displayPlayerUnit = ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).FirstOrDefault<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.id == mypage_unit_id));
    if (!(displayPlayerUnit == (PlayerUnit) null))
      return displayPlayerUnit;
    MypageUnitUtil.setDefaultUnitNotFound();
    return this.GetDeckLeaderPlayerUnit();
  }

  private PlayerUnit GetDeckLeaderPlayerUnit()
  {
    PlayerDeck[] playerDeckArray = SMManager.Get<PlayerDeck[]>();
    PlayerUnit leaderPlayerUnit = ((IEnumerable<PlayerUnit>) playerDeckArray[Persist.deckOrganized.Data.number].player_units).FirstOrDefault<PlayerUnit>();
    if (leaderPlayerUnit == (PlayerUnit) null)
      leaderPlayerUnit = ((IEnumerable<PlayerUnit>) playerDeckArray[0].player_units).First<PlayerUnit>();
    return leaderPlayerUnit;
  }

  private void SetEventOnClick()
  {
    foreach (Shop00720Menu.SlotButtonSetting slotButton in this.SlotButtons)
    {
      if (!Object.op_Equality((Object) slotButton.Button, (Object) null))
      {
        UIButton component = slotButton.Button.GetComponent<UIButton>();
        if (!Object.op_Equality((Object) component, (Object) null))
        {
          Shop00720Menu.SlotNShot slotNshot = new Shop00720Menu.SlotNShot(this, slotButton.Consecutive);
          EventDelegate.Set(component.onClick, new EventDelegate.Callback(slotNshot.OnClick));
        }
      }
    }
  }

  private string[] GetReelPattern(int row)
  {
    return ((IEnumerable<SlotS001MedalReelDetail>) MasterData.SlotS001MedalReelDetailList).Where<SlotS001MedalReelDetail>((Func<SlotS001MedalReelDetail, bool>) (w => w.reel_detail_id == row)).Select<SlotS001MedalReelDetail, int>((Func<SlotS001MedalReelDetail, int>) (s => s.icon_id)).Select<int, string>((Func<int, string>) (s => this.GetIconFileName(s))).ToArray<string>();
  }

  private string GetIconFileName(int id)
  {
    return ((IEnumerable<SlotS001MedalReelIcon>) MasterData.SlotS001MedalReelIconList).SingleOrDefault<SlotS001MedalReelIcon>((Func<SlotS001MedalReelIcon, bool>) (sd => sd.ID == id)).file_name;
  }

  public void Ready()
  {
    this.setActiveButtonBack(true);
    foreach (UITweener tweener in this.tweeners)
      tweener.PlayForward();
    this.SetMedalInfo(Player.Current.medal);
    this.slotModuleSlots.Clear();
    foreach (SlotModule slotModule in SMManager.Get<SlotModule[]>())
    {
      if (slotModule != null)
      {
        foreach (SlotModuleSlot slotModuleSlot in slotModule.slot)
        {
          if (slotModuleSlot != null)
            this.slotModuleSlots.Add(slotModuleSlot);
        }
      }
    }
    if (this.slotModuleSlots.Count == 0)
    {
      Debug.LogError((object) "NOTHING SLOTMODULES!!!");
      this.effect.loadState = false;
      this.isWaitForShot = true;
      this.isInitalize = true;
      ((Behaviour) this).enabled = true;
    }
    else
    {
      this.currentDeck = this.slotModuleSlots.First<SlotModuleSlot>().deck_id;
      foreach (Shop00720Menu.SlotButtonSetting slotButton in this.SlotButtons)
      {
        Shop00720Menu.SlotButtonSetting sbs = slotButton;
        SlotModuleSlot slotModuleSlot = this.slotModuleSlots.Find((Predicate<SlotModuleSlot>) (s => sbs.Consecutive == s.roll_count));
        if (sbs.Consecutive == 1 && slotModuleSlot == null)
          slotModuleSlot = this.slotModuleSlots.FirstOrDefault<SlotModuleSlot>();
        if (slotModuleSlot == null)
        {
          sbs.enabled = false;
          sbs.slotId = 0;
        }
        else
        {
          sbs.enabled = true;
          sbs.slotId = slotModuleSlot.id;
        }
        int num1 = (int) (Math.Pow(10.0, (double) sbs.Cost.Length) - 1.0);
        int num2 = sbs.enabled ? slotModuleSlot.payment_amount : 0;
        if (num1 < num2)
        {
          sbs.enabled = false;
          num2 = num1;
        }
        int y = sbs.Cost.Length - 1;
        do
        {
          int num3 = (int) Math.Pow(10.0, (double) y);
          sbs.Cost[y].SetSprite(string.Format("num_{0}.png__GUI__007-20_sozai__007-20_sozai_prefab", (object) (num2 / num3)));
          num2 %= num3;
          --y;
        }
        while (y >= 0);
        UIButton component = sbs.Button.GetComponent<UIButton>();
        if (!sbs.enabled || slotModuleSlot.payment_amount > Player.Current.medal)
        {
          ((UIButtonColor) component).isEnabled = false;
          foreach (UIWidget paymentAmount in sbs.PaymentAmounts)
            paymentAmount.color = new Color(0.3f, 0.3f, 0.3f);
        }
        else
        {
          ((UIButtonColor) component).isEnabled = true;
          foreach (UIWidget paymentAmount in sbs.PaymentAmounts)
            paymentAmount.color = new Color(0.5f, 0.5f, 0.5f);
        }
      }
      this.effect.loadState = false;
      this.isWaitForShot = true;
      this.isInitalize = true;
      ((Behaviour) this).enabled = true;
    }
  }

  private void SetTweeners(List<GameObject> list)
  {
    this.tweeners = new List<UITweener>();
    foreach (GameObject gameObject in list)
      this.tweeners = this.tweeners.Concat<UITweener>((IEnumerable<UITweener>) gameObject.GetComponents<UITweener>()).ToList<UITweener>();
  }

  private void SetMedalInfo(int num)
  {
    this.MedalInfo.GetComponentInChildren<UILabel>().SetTextLocalize(num);
  }

  public void OnIbtnBack()
  {
    if (!this.isWaitForShot || this.IsPushAndSet())
      return;
    this.backScene();
  }

  public override void onBackButton() => this.OnIbtnBack();

  public void OnIbtnShotN(int count)
  {
    this.isWaitForShot = false;
    this.currentSlot = 0;
    bool flag = false;
    foreach (Shop00720Menu.SlotButtonSetting slotButton in this.SlotButtons)
    {
      if (slotButton.Consecutive == count)
      {
        this.currentSlot = slotButton.slotId;
        flag = true;
        break;
      }
    }
    if (!flag)
    {
      Debug.LogError((object) string.Format("{0}連スロット情報が準備できていません", (object) count));
      this.isWaitForShot = true;
    }
    else
    {
      this.setActiveButtonBack(false);
      ((Component) Singleton<CommonRoot>.GetInstance().GetNormalHeaderComponent()).GetComponentInChildren<CommonHeaderExp>().SetIsButtonEnable(false);
      this.StartCoroutine(this.MedalPay(count >= 100));
    }
  }

  private void setActiveButtonBack(bool bactive)
  {
    UIButton component = this.IbtnBack.GetComponent<UIButton>();
    if (!Object.op_Inequality((Object) component, (Object) null))
      return;
    ((UIButtonColor) component).isEnabled = bactive;
  }

  private void StartSlotEff()
  {
    foreach (UITweener tweener in this.tweeners)
      tweener.PlayReverse();
    this.effect.Bet();
  }

  private IEnumerator MedalPay(bool bShowLoading)
  {
    Shop00720Menu shop00720Menu = this;
    if (bShowLoading)
      Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1);
    else
      Singleton<CommonRoot>.GetInstance().loadingMode = 2;
    // ISSUE: reference to a compiler-generated method
    Future<WebAPI.Response.SlotS001MedalPay> feature = WebAPI.SlotS001MedalPay(shop00720Menu.currentSlot, new Action<WebAPI.Response.UserError>(shop00720Menu.\u003CMedalPay\u003Eb__32_0));
    IEnumerator e = feature.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    WebAPI.Response.SlotS001MedalPay result = feature.Result;
    if (result != null)
    {
      if (!bShowLoading)
      {
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        shop00720Menu.StartSlotEff();
      }
      e = shop00720Menu.SetResult(result);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      shop00720Menu.effect.loadState = true;
      if (bShowLoading)
      {
        Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
        shop00720Menu.StartSlotEff();
      }
    }
  }

  private IEnumerator SetResult(WebAPI.Response.SlotS001MedalPay result)
  {
    this.effect.transitionPlanList = result.animation_pattern;
    this.effect.stopTextureId_1 = result.result_reel_index[0];
    this.effect.stopTextureId_2 = result.result_reel_index[1];
    this.effect.stopTextureId_3 = result.result_reel_index[2];
    this.effect.rarity = ((IEnumerable<WebAPI.Response.SlotS001MedalPayResult>) result.result).Select<WebAPI.Response.SlotS001MedalPayResult, SlotS001MedalRarity>((Func<WebAPI.Response.SlotS001MedalPayResult, SlotS001MedalRarity>) (s => ((IEnumerable<SlotS001MedalRarity>) MasterData.SlotS001MedalRarityList).SingleOrDefault<SlotS001MedalRarity>((Func<SlotS001MedalRarity, bool>) (sd => sd.ID == s.rarity_id)))).Max<SlotS001MedalRarity>((Func<SlotS001MedalRarity, int>) (m => m.index));
    IEnumerator e = this.effect.Renpatu(result.result);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void OnIbtnSkip()
  {
    this.effect.Bet();
    this.effect.Skip();
    if (!this.effect.Slot_script.isReady)
      return;
    this.Ready();
  }

  public void OnIbtnCheckReward()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.ShowRewards(this.prefabs, this.currentDeck));
  }

  private IEnumerator ShowRewards(Shop00720Prefabs prefabs, int deckID)
  {
    GameObject prefab = prefabs.DirSlotList.Clone();
    prefab.SetActive(false);
    IEnumerator e = prefab.GetComponent<Shop00720RewardList>().Init(prefabs, deckID);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
    prefab.SetActive(true);
  }

  public void OnEndScene()
  {
    if (Object.op_Inequality((Object) this.slot, (Object) null))
      Object.Destroy((Object) this.slot);
    this.slot = (GameObject) null;
    this.effect = (Shop00720EffectController) null;
    this.isInitalize = false;
    ((Behaviour) this).enabled = false;
  }

  protected override void Update()
  {
    if (!this.isInitalize)
      return;
    base.Update();
    if (this.effect.Slot_script.isReady)
    {
      this.IbtnSkip.SetActive(false);
      foreach (Shop00720Menu.SlotButtonSetting slotButton in this.SlotButtons)
        slotButton.Button.SetActive(true);
      this.IbtnCheckReward.SetActive(true);
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(true);
    }
    else if (this.effect.Slot_script.isEnd)
    {
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(true);
      ((Component) Singleton<CommonRoot>.GetInstance().GetNormalHeaderComponent()).GetComponentInChildren<CommonHeaderExp>().SetIsButtonEnable(true);
    }
    else
    {
      this.IbtnSkip.SetActive(true);
      foreach (Shop00720Menu.SlotButtonSetting slotButton in this.SlotButtons)
        slotButton.Button.SetActive(false);
      this.IbtnCheckReward.SetActive(false);
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
    }
  }

  [Serializable]
  private class SlotButtonSetting
  {
    [NonSerialized]
    public bool enabled;
    [NonSerialized]
    public int slotId;
    public int Consecutive;
    public GameObject Button;
    public UISprite[] Cost = new UISprite[0];
    public UISprite[] PaymentAmounts = new UISprite[0];
  }

  private class SlotNShot
  {
    private Shop00720Menu shop_;
    private int count_;

    public SlotNShot(Shop00720Menu shop, int count)
    {
      this.shop_ = shop;
      this.count_ = count;
    }

    public void OnClick() => this.shop_.OnIbtnShotN(this.count_);
  }
}
