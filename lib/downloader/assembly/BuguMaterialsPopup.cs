// Decompiled with JetBrains decompiler
// Type: BuguMaterialsPopup
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
public class BuguMaterialsPopup : BackButtonMenuBase
{
  private const int MAXCOUNT = 99;
  [SerializeField]
  private UISlider slider;
  public Transform parent;
  public UILabel selectedNum;
  public UILabel min;
  public UILabel max;
  public UILabel titleLabel;
  public UILabel infoLabel;
  public UILabel zeniLabel;
  public UILabel buguRenseiNum;
  public UIButton minBtn;
  public UIButton maxBtn;
  public UIButton minusBtn;
  public UIButton plusBtn;
  private GameObject itemIconPrefab;
  private InventoryItem inventoryItem;
  private GameCore.ItemInfo BaseItem;
  private Dictionary<GearReisouType, int> reisouExp;
  private int selectedItemNum;
  private int canUseItemsValue;
  private Player player;
  private Action<InventoryItem> onClickOKBtn;
  private int residualExp;
  private int currentMaxExp;
  private int residualReisouExp;
  private int currentReisouMaxExp;
  private Decimal rate;
  private int renseiValueByOne;
  private int maxUpNums;

  protected IEnumerator LoadItemIconPrefab()
  {
    if (Object.op_Equality((Object) this.itemIconPrefab, (Object) null))
    {
      Future<GameObject> prefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
      IEnumerator e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.itemIconPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
  }

  public IEnumerator Init(InventoryItem item, GameCore.ItemInfo baseItem, int addRenseiValue, int maxUpNum)
  {
    BuguMaterialsPopup buguMaterialsPopup = this;
    buguMaterialsPopup.minBtn.onClick.Clear();
    buguMaterialsPopup.minBtn.onClick.Add(new EventDelegate((EventDelegate.Callback) (() => ((UIProgressBar) this.slider).value = 0.0f)));
    buguMaterialsPopup.maxBtn.onClick.Clear();
    buguMaterialsPopup.maxBtn.onClick.Add(new EventDelegate((EventDelegate.Callback) (() => ((UIProgressBar) this.slider).value = 1f)));
    buguMaterialsPopup.minusBtn.onClick.Clear();
    buguMaterialsPopup.minusBtn.onClick.Add(new EventDelegate((EventDelegate.Callback) (() =>
    {
      UISlider slider = this.slider;
      ((UIProgressBar) slider).value = ((UIProgressBar) slider).value - 1f / (float) this.canUseItemsValue;
    })));
    buguMaterialsPopup.plusBtn.onClick.Clear();
    buguMaterialsPopup.plusBtn.onClick.Add(new EventDelegate((EventDelegate.Callback) (() =>
    {
      UISlider slider = this.slider;
      ((UIProgressBar) slider).value = ((UIProgressBar) slider).value + 1f / (float) this.canUseItemsValue;
    })));
    buguMaterialsPopup.inventoryItem = item;
    buguMaterialsPopup.BaseItem = baseItem;
    buguMaterialsPopup.maxUpNums = maxUpNum;
    GearRankExp gearRankExp = ((IEnumerable<GearRankExp>) MasterData.GearRankExpList).FirstOrDefault<GearRankExp>((Func<GearRankExp, bool>) (x => x.ID == Mathf.Min(this.BaseItem.gearLevelLimit + maxUpNum, this.BaseItem.playerItem.gear_level_limit_max)));
    if (gearRankExp != null)
    {
      buguMaterialsPopup.currentMaxExp = gearRankExp.from_exp;
      buguMaterialsPopup.residualExp = Math.Max(0, buguMaterialsPopup.currentMaxExp - buguMaterialsPopup.BaseItem.playerItem.gear_total_exp - addRenseiValue);
    }
    IEnumerator e = buguMaterialsPopup.LoadItemIconPrefab();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ItemIcon itemIcon = Object.Instantiate<GameObject>(buguMaterialsPopup.itemIconPrefab, buguMaterialsPopup.parent).GetComponent<ItemIcon>();
    ((Component) itemIcon).transform.localPosition = Vector3.zero;
    e = itemIcon.InitByItemInfo(item.Item);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    itemIcon.SetRenseiIcon();
    if ((item.Item.isSupply || item.Item.isExchangable || item.Item.isCompse ? 1 : (item.Item.isWeaponMaterial ? 1 : 0)) != 0)
      itemIcon.SetRenseiMaterialCount(item.Item.quantity);
    else
      itemIcon.SetRenseiMaterialCount(0);
    itemIcon.SetRenseiMaxUpMark(buguMaterialsPopup.BaseItem != null && !buguMaterialsPopup.BaseItem.playerItem.isLimitMax() && buguMaterialsPopup.inventoryItem.Item.gear != null && GearGear.CanSpecialDrill(buguMaterialsPopup.BaseItem.gear, buguMaterialsPopup.inventoryItem.Item.gear));
    buguMaterialsPopup.titleLabel.SetText(item.Item.Name());
    buguMaterialsPopup.infoLabel.SetText(item.Item.gear.description);
    buguMaterialsPopup.reisouExp = new Dictionary<GearReisouType, int>()
    {
      {
        GearReisouType.holy,
        0
      },
      {
        GearReisouType.chaos,
        0
      }
    };
    int? drillingExpMythologyId;
    if (item.Item.gear.drilling_exp_mythology_id.HasValue)
    {
      drillingExpMythologyId = item.Item.gear.drilling_exp_mythology_id;
      int num = 0;
      if (!(drillingExpMythologyId.GetValueOrDefault() == num & drillingExpMythologyId.HasValue))
        buguMaterialsPopup.calcReisouExp(ref buguMaterialsPopup.reisouExp, item.Item.playerItem, item.Item.gear);
    }
    BoostBonusGearDrilling bonusGearDrilling = (!item.Item.isReisou ? Singleton<NGGameDataManager>.GetInstance().BoostInfo : (NGGameDataManager.Boost) null)?.findBonusGearDrilling(baseItem?.gear);
    buguMaterialsPopup.rate = bonusGearDrilling == null ? 1.0M : (Decimal) bonusGearDrilling.increase_price;
    if (item.Item.gear.drilling_exp_mythology_id.HasValue)
    {
      drillingExpMythologyId = item.Item.gear.drilling_exp_mythology_id;
      int num = 0;
      if (!(drillingExpMythologyId.GetValueOrDefault() == num & drillingExpMythologyId.HasValue))
        buguMaterialsPopup.rate = 1.0M;
    }
    Decimal num1 = bonusGearDrilling == null ? 1.0M : (Decimal) bonusGearDrilling.boot_rate;
    buguMaterialsPopup.renseiValueByOne = (int) (num1 * (item.Item.isReisou ? 0M : Math.Floor((Decimal) GearDrilling.GetGearDrilling(item.Item.gearLevel, item.Item.gear.rarity.index) * (Decimal) item.Item.gear.drilling_rate)));
    if (item.Item.gear.drilling_exp_mythology_id.HasValue)
    {
      drillingExpMythologyId = item.Item.gear.drilling_exp_mythology_id;
      int num2 = 0;
      if (!(drillingExpMythologyId.GetValueOrDefault() == num2 & drillingExpMythologyId.HasValue))
      {
        int num3;
        if (buguMaterialsPopup.BaseItem.reisou != (PlayerItem) null)
        {
          buguMaterialsPopup.currentReisouMaxExp = ((IEnumerable<ReisouRankExp>) MasterData.ReisouRankExpList).FirstOrDefault<ReisouRankExp>((Func<ReisouRankExp, bool>) (x => x.ID == this.BaseItem.reisou.gear_level_limit)).from_exp;
          num3 = baseItem.reisou.gear_total_exp;
          if (baseItem.reisou.isMythologyReisou())
          {
            PlayerMythologyGearStatus mythologyGearStatus = buguMaterialsPopup.BaseItem.reisou.GetPlayerMythologyGearStatus();
            drillingExpMythologyId = item.Item.gear.drilling_exp_mythology_id;
            int num4 = 1;
            if (drillingExpMythologyId.GetValueOrDefault() == num4 & drillingExpMythologyId.HasValue)
              num3 = mythologyGearStatus.holy_gear_total_exp;
            drillingExpMythologyId = item.Item.gear.drilling_exp_mythology_id;
            int num5 = 2;
            if (drillingExpMythologyId.GetValueOrDefault() == num5 & drillingExpMythologyId.HasValue)
              num3 = mythologyGearStatus.chaos_gear_total_exp;
          }
        }
        else
        {
          buguMaterialsPopup.currentReisouMaxExp = ((IEnumerable<ReisouRankExp>) MasterData.ReisouRankExpList).FirstOrDefault<ReisouRankExp>((Func<ReisouRankExp, bool>) (x => x.ID == this.BaseItem.playerItem.gear_level_limit)).from_exp;
          num3 = baseItem.playerItem.gear_total_exp;
          if (baseItem.playerItem.isMythologyReisou())
          {
            PlayerMythologyGearStatus mythologyGearStatus = buguMaterialsPopup.BaseItem.playerItem.GetPlayerMythologyGearStatus();
            drillingExpMythologyId = item.Item.gear.drilling_exp_mythology_id;
            int num6 = 1;
            if (drillingExpMythologyId.GetValueOrDefault() == num6 & drillingExpMythologyId.HasValue)
              num3 = mythologyGearStatus.holy_gear_total_exp;
            drillingExpMythologyId = item.Item.gear.drilling_exp_mythology_id;
            int num7 = 2;
            if (drillingExpMythologyId.GetValueOrDefault() == num7 & drillingExpMythologyId.HasValue)
              num3 = mythologyGearStatus.chaos_gear_total_exp;
          }
        }
        buguMaterialsPopup.residualReisouExp = Math.Max(0, buguMaterialsPopup.currentReisouMaxExp - num3 - addRenseiValue);
        float num8 = 0.0f;
        drillingExpMythologyId = item.Item.gear.drilling_exp_mythology_id;
        int num9 = 1;
        if (drillingExpMythologyId.GetValueOrDefault() == num9 & drillingExpMythologyId.HasValue)
          num8 = (float) buguMaterialsPopup.reisouExp[GearReisouType.holy];
        drillingExpMythologyId = item.Item.gear.drilling_exp_mythology_id;
        int num10 = 2;
        if (drillingExpMythologyId.GetValueOrDefault() == num10 & drillingExpMythologyId.HasValue)
          num8 = (float) buguMaterialsPopup.reisouExp[GearReisouType.chaos];
        if (item.select || item.Item.isTempSelectedCount)
        {
          buguMaterialsPopup.canUseItemsValue = item.Item.tempSelectedCount < (int) Math.Ceiling((double) Math.Max(0.0f, (float) buguMaterialsPopup.residualReisouExp + (float) item.Item.tempSelectedCount * num8) / (double) num8) ? (int) Math.Ceiling((double) Math.Max(0.0f, (float) buguMaterialsPopup.residualReisouExp + (float) item.Item.tempSelectedCount * num8) / (double) num8) : item.Item.tempSelectedCount;
          if (item.Item.quantity <= buguMaterialsPopup.canUseItemsValue)
            buguMaterialsPopup.canUseItemsValue = item.Item.quantity;
        }
        else
        {
          float a = (float) buguMaterialsPopup.residualReisouExp / num8;
          buguMaterialsPopup.canUseItemsValue = (double) item.Item.quantity <= (double) a ? item.Item.quantity : (int) Math.Ceiling((double) a);
        }
        if (buguMaterialsPopup.canUseItemsValue > 99)
        {
          buguMaterialsPopup.canUseItemsValue = 99;
          goto label_47;
        }
        else
          goto label_47;
      }
    }
    if (buguMaterialsPopup.BaseItem != null && !buguMaterialsPopup.BaseItem.playerItem.isLimitMax() && item.Item.gear != null && GearGear.CanSpecialDrill(buguMaterialsPopup.BaseItem.gear, item.Item.gear))
    {
      int num11 = buguMaterialsPopup.BaseItem.playerItem.gear_level_limit_max - Mathf.Min(buguMaterialsPopup.BaseItem.gearLevelLimit + maxUpNum, buguMaterialsPopup.BaseItem.playerItem.gear_level_limit_max);
      buguMaterialsPopup.canUseItemsValue = item.select || item.Item.isTempSelectedCount ? (item.Item.tempSelectedCount < num11 ? num11 : item.Item.tempSelectedCount) : num11;
      if (item.Item.quantity < buguMaterialsPopup.canUseItemsValue)
        buguMaterialsPopup.canUseItemsValue = item.Item.quantity;
    }
    else
    {
      float a = (float) buguMaterialsPopup.residualExp / (float) buguMaterialsPopup.renseiValueByOne;
      buguMaterialsPopup.canUseItemsValue = buguMaterialsPopup.currentMaxExp > addRenseiValue || !item.select && !item.Item.isTempSelectedCount ? (buguMaterialsPopup.currentMaxExp <= addRenseiValue || !item.select && !item.Item.isTempSelectedCount ? (int) Math.Ceiling((double) a) : (int) Math.Ceiling((double) (item.Item.tempSelectedCount * buguMaterialsPopup.renseiValueByOne + buguMaterialsPopup.residualExp) / (double) buguMaterialsPopup.renseiValueByOne)) : item.Item.tempSelectedCount;
      if (buguMaterialsPopup.canUseItemsValue > 99)
        buguMaterialsPopup.canUseItemsValue = 99;
      if (item.Item.quantity < buguMaterialsPopup.canUseItemsValue)
        buguMaterialsPopup.canUseItemsValue = item.Item.quantity;
    }
label_47:
    buguMaterialsPopup.min.SetText("0");
    buguMaterialsPopup.max.SetText(buguMaterialsPopup.canUseItemsValue.ToString());
    if (!item.Item.isTempSelectedCount && item.Item.tempSelectedCount <= 0)
      ((UIProgressBar) buguMaterialsPopup.slider).value = 1f / (float) buguMaterialsPopup.canUseItemsValue;
    else
      ((UIProgressBar) buguMaterialsPopup.slider).value = 1f / (float) buguMaterialsPopup.canUseItemsValue * (float) item.Item.tempSelectedCount;
    buguMaterialsPopup.player = SMManager.Get<Player>();
    buguMaterialsPopup.selectedNum.SetText("");
    buguMaterialsPopup.zeniLabel.SetText("");
    buguMaterialsPopup.buguRenseiNum.SetText("");
  }

  private void calcReisouExp(
    ref Dictionary<GearReisouType, int> outExp,
    PlayerItem item,
    GearGear gear)
  {
    GearReisouType? reisouType = item?.gear?.reisou_type;
    int? drillingExpMythologyId1 = (int?) gear?.drilling_exp_mythology_id;
    if (!reisouType.HasValue && !drillingExpMythologyId1.HasValue || !gear.drilling_exp_mythology_id.HasValue)
      return;
    int? nullable = gear.drilling_exp_mythology_id;
    int num = 0;
    if (nullable.GetValueOrDefault() == num & nullable.HasValue)
      return;
    GearDrillingExpMythology drillingExpMythology = ((IEnumerable<GearDrillingExpMythology>) MasterData.GearDrillingExpMythologyList).FirstOrDefault<GearDrillingExpMythology>((Func<GearDrillingExpMythology, bool>) (x =>
    {
      int id = x.ID;
      int? drillingExpMythologyId2 = gear.drilling_exp_mythology_id;
      int valueOrDefault = drillingExpMythologyId2.GetValueOrDefault();
      return id == valueOrDefault & drillingExpMythologyId2.HasValue;
    }));
    nullable = drillingExpMythology.holy;
    int total_exp1 = nullable ?? 0;
    nullable = drillingExpMythology.chaos;
    int total_exp2 = nullable ?? 0;
    outExp[GearReisouType.holy] += this.reisouValue(gear, total_exp1);
    outExp[GearReisouType.chaos] += this.reisouValue(gear, total_exp2);
  }

  private int reisouValue(GearGear gear, int total_exp, Decimal boostRate = 1.0M)
  {
    int num1 = 0;
    if (!gear.drilling_exp_mythology_id.HasValue)
    {
      int? drillingExpMythologyId = gear.drilling_exp_mythology_id;
      int num2 = 0;
      if (!(drillingExpMythologyId.GetValueOrDefault() == num2 & drillingExpMythologyId.HasValue))
        num1 = ReisouDrilling.GetReisouDrilling(gear.rarity.index);
    }
    return (int) (boostRate * Math.Floor((Decimal) num1 + (Decimal) total_exp) * (Decimal) gear.drilling_rate);
  }

  protected override void Update()
  {
    this.selectedItemNum = Mathf.RoundToInt(((UIProgressBar) this.slider).value * (float) this.canUseItemsValue);
    if (this.canUseItemsValue <= 1 && (double) ((UIProgressBar) this.slider).value < 1.0 && (double) ((UIProgressBar) this.slider).value >= 0.0099999997764825821)
      this.selectedItemNum = 1;
    if (this.selectedItemNum > this.canUseItemsValue)
    {
      this.selectedItemNum = this.canUseItemsValue;
      ((UIProgressBar) this.slider).value = (float) this.selectedItemNum / (float) this.canUseItemsValue;
    }
    this.selectedNum.SetTextLocalize(this.selectedItemNum.ToString());
    int num1 = this.BaseItem == null || this.BaseItem.playerItem.isLimitMax() || this.inventoryItem.Item.gear == null || !GearGear.CanSpecialDrill(this.BaseItem.gear, this.inventoryItem.Item.gear) ? (int) (this.rate * (Decimal) CalcItemCost.GetDrillingCostForOne(this.BaseItem, this.inventoryItem)) * this.selectedItemNum : (int) (this.rate * (Decimal) CalcItemCost.GetDrillingCost(this.BaseItem, Enumerable.Repeat<InventoryItem>(this.inventoryItem, this.selectedItemNum).ToList<InventoryItem>(), this.maxUpNums));
    this.zeniLabel.SetTextLocalize(num1);
    ((UIWidget) this.zeniLabel).color = (long) num1 < this.player.money ? Color.white : Color.red;
    if (this.inventoryItem.Item.gear.drilling_exp_mythology_id.HasValue)
    {
      int? drillingExpMythologyId1 = this.inventoryItem.Item.gear.drilling_exp_mythology_id;
      int num2 = 0;
      if (!(drillingExpMythologyId1.GetValueOrDefault() == num2 & drillingExpMythologyId1.HasValue))
      {
        int? drillingExpMythologyId2 = this.inventoryItem.Item.gear.drilling_exp_mythology_id;
        int num3 = 1;
        if (drillingExpMythologyId2.GetValueOrDefault() == num3 & drillingExpMythologyId2.HasValue)
          this.buguRenseiNum.SetTextLocalize(this.reisouExp[GearReisouType.holy] * this.selectedItemNum);
        int? drillingExpMythologyId3 = this.inventoryItem.Item.gear.drilling_exp_mythology_id;
        int num4 = 2;
        if (!(drillingExpMythologyId3.GetValueOrDefault() == num4 & drillingExpMythologyId3.HasValue))
          return;
        this.buguRenseiNum.SetTextLocalize(this.reisouExp[GearReisouType.chaos] * this.selectedItemNum);
        return;
      }
    }
    this.buguRenseiNum.SetTextLocalize(this.renseiValueByOne * this.selectedItemNum);
  }

  public void SetOnOKClick(Action<InventoryItem> click) => this.onClickOKBtn = click;

  public void OnClickOK()
  {
    this.inventoryItem.Item.isTempSelectedCount = this.selectedItemNum > 0;
    this.inventoryItem.Item.tempSelectedCount = this.selectedItemNum;
    if (this.onClickOKBtn != null && this.inventoryItem != null)
      this.onClickOKBtn(this.inventoryItem);
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnRetrun();

  private void IbtnRetrun()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public void ClearData()
  {
  }
}
