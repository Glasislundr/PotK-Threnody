// Decompiled with JetBrains decompiler
// Type: Guild0286Menu
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
public class Guild0286Menu : Guild0286Scroll
{
  private const int iconWidth = 620;
  private const int iconHeight = 175;
  private const int iconColumnValue = 1;
  private const int iconRowValue = 12;
  private const int iconScreenValue = 8;
  private const int iconMaxValue = 12;
  private const int MAX_RECEIVE = 60;
  [SerializeField]
  private UIButton ibtnReceiveAll;
  [SerializeField]
  private UIButton ibtnGreedItem;
  private ModalWindow popup;
  [SerializeField]
  private CreateIconObject itemIcon;
  [SerializeField]
  private GameObject giftItemPos;
  [SerializeField]
  private UILabel haveValueLabel;
  [SerializeField]
  private UILabel itemNameLabel;
  private GameObject itemDetailPopup;
  private GameObject receivePrefab;
  [SerializeField]
  private GameObject noGiftTxt;

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public virtual void onButtonRecieveAll()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.ReceiveAll());
  }

  public void onButtonSend()
  {
    if (this.IsPushAndSet())
      return;
    Guild02861Scene.ChangeScene(false);
  }

  public void onButtonHaveItem()
  {
    if (this.IsPushAndSet())
      return;
    Guild02862Scene.ChangeScene();
  }

  private void SetHaveItemValue()
  {
    Player player = SMManager.Get<Player>();
    PlayerUnit[] self1 = SMManager.Get<PlayerUnit[]>();
    PlayerMaterialUnit[] source = SMManager.Get<PlayerMaterialUnit[]>();
    PlayerItem[] self2 = SMManager.Get<PlayerItem[]>();
    PlayerMaterialGear[] self3 = SMManager.Get<PlayerMaterialGear[]>();
    GuildMemberGift gift = new GuildMemberGift();
    gift.gift_reward_id = PlayerAffiliation.Current.Player.gift_reward_id;
    gift.gift_reward_type_id = PlayerAffiliation.Current.Player.gift_reward_type_id;
    gift.gift_reward_quantity = PlayerAffiliation.Current.Player.gift_reward_quantity;
    long num = 0;
    switch (gift.gift_reward_type_id)
    {
      case 1:
        if (MasterData.UnitUnit.ContainsKey(gift.gift_reward_id))
        {
          num = (long) self1.AmountHavingTargetUnit(gift.gift_reward_id, (MasterDataTable.CommonRewardType) gift.gift_reward_type_id);
          break;
        }
        break;
      case 2:
        num = (long) self2.AmountHavingTargetItem(gift.gift_reward_id, (MasterDataTable.CommonRewardType) gift.gift_reward_type_id);
        break;
      case 3:
        num = (long) self2.AmountHavingTargetItem(gift.gift_reward_id, (MasterDataTable.CommonRewardType) gift.gift_reward_type_id);
        break;
      case 4:
        num = player.money;
        break;
      case 10:
        num = (long) player.coin;
        break;
      case 14:
        num = (long) player.medal;
        break;
      case 15:
        num = (long) player.friend_point;
        break;
      case 17:
        num = (long) player.battle_medal;
        break;
      case 19:
        PlayerQuestKey[] self4 = SMManager.Get<PlayerQuestKey[]>();
        int? nullable1 = ((IEnumerable<PlayerQuestKey>) self4).FirstIndexOrNull<PlayerQuestKey>((Func<PlayerQuestKey, bool>) (x => x.quest_key_id == gift.gift_reward_id));
        num = !nullable1.HasValue ? 0L : (long) self4[nullable1.Value].quantity;
        break;
      case 20:
        int? nullable2 = ((IEnumerable<PlayerGachaTicket>) player.gacha_tickets).FirstIndexOrNull<PlayerGachaTicket>((Func<PlayerGachaTicket, bool>) (x => x.ticket_id == gift.gift_reward_id));
        num = !nullable2.HasValue ? 0L : (long) player.gacha_tickets[nullable2.Value].quantity;
        break;
      case 24:
        PlayerMaterialUnit playerMaterialUnit = ((IEnumerable<PlayerMaterialUnit>) source).FirstOrDefault<PlayerMaterialUnit>((Func<PlayerMaterialUnit, bool>) (x => x._unit == gift.gift_reward_id));
        if (playerMaterialUnit != null)
        {
          num = (long) playerMaterialUnit.quantity;
          break;
        }
        break;
      case 26:
      case 35:
        num = (long) self3.AmountHavingTargetItem(gift.gift_reward_id);
        break;
      case 40:
        PlayerCommonTicket[] self5 = SMManager.Get<PlayerCommonTicket[]>();
        int? nullable3 = ((IEnumerable<PlayerCommonTicket>) self5).FirstIndexOrNull<PlayerCommonTicket>((Func<PlayerCommonTicket, bool>) (x => x.ticket_id == gift.gift_reward_id));
        num = !nullable3.HasValue ? 0L : (long) self5[nullable3.Value].quantity;
        break;
    }
    this.haveValueLabel.SetTextLocalize(num);
  }

  private void InitEndAction()
  {
    this.SetHaveItemValue();
    this.CheckAllReceive();
  }

  private void CheckAllReceive()
  {
    if (this.memberGifts.Length == 0)
    {
      ((UIButtonColor) this.ibtnReceiveAll).duration = 0.0f;
      ((UIButtonColor) this.ibtnReceiveAll).isEnabled = false;
      this.noGiftTxt.SetActive(true);
      if (!Persist.guildSetting.Exists || !GuildUtil.getBadgeState(GuildUtil.GuildBadgeInfoType.newGift))
        return;
      GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.newGift, false);
      Persist.guildSetting.Flush();
    }
    else
    {
      ((UIButtonColor) this.ibtnReceiveAll).isEnabled = true;
      this.noGiftTxt.SetActive(false);
    }
  }

  private void onLongPressIcon()
  {
    if (this.IsPushAndSet())
      return;
    switch (PlayerAffiliation.Current.Player.gift_reward_type_id)
    {
      case 1:
      case 2:
      case 3:
      case 19:
      case 21:
      case 24:
      case 26:
      case 35:
        this.StartCoroutine(this.ShowDetailPopup());
        break;
    }
    this.IsPush = false;
  }

  private IEnumerator ShowDetailPopup()
  {
    GameObject popup = Singleton<PopupManager>.GetInstance().open(this.itemDetailPopup);
    popup.SetActive(false);
    IEnumerator e = popup.GetComponent<Shop00742Menu>().Init((MasterDataTable.CommonRewardType) PlayerAffiliation.Current.Player.gift_reward_type_id, PlayerAffiliation.Current.Player.gift_reward_id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
  }

  private IEnumerator SetHaveItemIcon()
  {
    Guild0286Menu guild0286Menu = this;
    GuildPlayerInfo gift = PlayerAffiliation.Current.Player;
    if (Object.op_Inequality((Object) guild0286Menu.itemIcon.GetIcon(), (Object) null))
      Object.Destroy((Object) guild0286Menu.itemIcon.GetIcon());
    IEnumerator e = guild0286Menu.itemIcon.CreateThumbnail((MasterDataTable.CommonRewardType) PlayerAffiliation.Current.Player.gift_reward_type_id, PlayerAffiliation.Current.Player.gift_reward_id, PlayerAffiliation.Current.Player.gift_reward_quantity, isButton: false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    switch (gift.gift_reward_type_id)
    {
      case 1:
      case 24:
        UnitIcon component1 = guild0286Menu.itemIcon.GetIcon().GetComponent<UnitIcon>();
        if (!Object.op_Inequality((Object) component1, (Object) null))
          break;
        component1.setLevelText("1");
        component1.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Rarity);
        guild0286Menu.itemNameLabel.SetTextLocalize(component1.Unit.name);
        // ISSUE: reference to a compiler-generated method
        guild0286Menu.itemIcon.GetIcon().GetComponent<UnitIcon>().onClick = new Action<UnitIconBase>(guild0286Menu.\u003CSetHaveItemIcon\u003Eb__26_0);
        ((UIButtonColor) guild0286Menu.ibtnGreedItem).isEnabled = true;
        break;
      case 2:
        ItemIcon component2 = guild0286Menu.itemIcon.GetIcon().GetComponent<ItemIcon>();
        if (!Object.op_Inequality((Object) component2, (Object) null))
          break;
        component2.QuantitySupply = true;
        component2.EnableQuantity(gift.gift_reward_quantity);
        SupplySupply supplySupply1 = (SupplySupply) null;
        if (MasterData.SupplySupply.TryGetValue(gift.gift_reward_id, out supplySupply1))
          guild0286Menu.itemNameLabel.SetTextLocalize(supplySupply1.name);
        component2.ReleaseClickEvent();
        // ISSUE: reference to a compiler-generated method
        component2.onClick = new Action<ItemIcon>(guild0286Menu.\u003CSetHaveItemIcon\u003Eb__26_3);
        // ISSUE: reference to a compiler-generated method
        component2.supply.button.onLongPress.Add(new EventDelegate(new EventDelegate.Callback(guild0286Menu.\u003CSetHaveItemIcon\u003Eb__26_4)));
        ((UIButtonColor) guild0286Menu.ibtnGreedItem).isEnabled = true;
        break;
      case 3:
      case 26:
      case 35:
        ItemIcon component3 = guild0286Menu.itemIcon.GetIcon().GetComponent<ItemIcon>();
        if (!Object.op_Inequality((Object) component3, (Object) null))
          break;
        component3.QuantitySupply = false;
        GearGear gearGear1 = (GearGear) null;
        if (MasterData.GearGear.TryGetValue(gift.gift_reward_id, out gearGear1))
          guild0286Menu.itemNameLabel.SetTextLocalize(gearGear1.name);
        component3.ReleaseClickEvent();
        // ISSUE: reference to a compiler-generated method
        component3.onClick = new Action<ItemIcon>(guild0286Menu.\u003CSetHaveItemIcon\u003Eb__26_1);
        // ISSUE: reference to a compiler-generated method
        component3.gear.button.onLongPress.Add(new EventDelegate(new EventDelegate.Callback(guild0286Menu.\u003CSetHaveItemIcon\u003Eb__26_2)));
        ((UIButtonColor) guild0286Menu.ibtnGreedItem).isEnabled = true;
        break;
      default:
        if (Object.op_Implicit((Object) guild0286Menu.itemIcon.GetIcon().GetComponent<UnitIcon>()))
        {
          UnitIcon component4 = guild0286Menu.itemIcon.GetIcon().GetComponent<UnitIcon>();
          ((Component) component4).GetComponent<UnitIcon>().ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Rarity);
          guild0286Menu.itemNameLabel.SetTextLocalize(component4.Unit.name);
          // ISSUE: reference to a compiler-generated method
          ((Component) component4).GetComponent<UnitIcon>().onClick = new Action<UnitIconBase>(guild0286Menu.\u003CSetHaveItemIcon\u003Eb__26_5);
          ((UIButtonColor) guild0286Menu.ibtnGreedItem).isEnabled = true;
          break;
        }
        if (Object.op_Implicit((Object) guild0286Menu.itemIcon.GetIcon().GetComponent<ItemIcon>()))
        {
          ItemIcon component5 = guild0286Menu.itemIcon.GetIcon().GetComponent<ItemIcon>();
          component5.QuantitySupply = true;
          component5.EnableQuantity(PlayerAffiliation.Current.Player.gift_reward_quantity);
          GearGear gearGear2 = (GearGear) null;
          if (MasterData.GearGear.TryGetValue(gift.gift_reward_id, out gearGear2))
            guild0286Menu.itemNameLabel.SetTextLocalize(gearGear2.name);
          SupplySupply supplySupply2 = (SupplySupply) null;
          if (MasterData.SupplySupply.TryGetValue(gift.gift_reward_id, out supplySupply2))
            guild0286Menu.itemNameLabel.SetTextLocalize(supplySupply2.name);
          component5.ReleaseClickEvent();
          // ISSUE: reference to a compiler-generated method
          component5.onClick = new Action<ItemIcon>(guild0286Menu.\u003CSetHaveItemIcon\u003Eb__26_6);
          // ISSUE: reference to a compiler-generated method
          component5.supply.button.onLongPress.Add(new EventDelegate(new EventDelegate.Callback(guild0286Menu.\u003CSetHaveItemIcon\u003Eb__26_7)));
          ((UIButtonColor) guild0286Menu.ibtnGreedItem).isEnabled = true;
          break;
        }
        if (!Object.op_Implicit((Object) guild0286Menu.itemIcon.GetIcon().GetComponent<UniqueIcons>()))
          break;
        UniqueIcons component6 = guild0286Menu.itemIcon.GetIcon().GetComponent<UniqueIcons>();
        ((UIButtonColor) guild0286Menu.ibtnGreedItem).isEnabled = true;
        guild0286Menu.itemNameLabel.SetTextLocalize(component6.itemName);
        break;
    }
  }

  private IEnumerator ResourceLoad()
  {
    Future<GameObject> prefabF;
    IEnumerator e;
    if (Object.op_Equality((Object) this.itemDetailPopup, (Object) null))
    {
      prefabF = Res.Prefabs.popup.popup_000_7_4_2__anim_popup01.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.itemDetailPopup = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.receivePrefab, (Object) null))
    {
      prefabF = Res.Prefabs.guild028_6.guild_gift_receive_list.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.receivePrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
  }

  public IEnumerator Init(GuildMemberGift[] gifts)
  {
    Guild0286Menu guild0286Menu = this;
    guild0286Menu.haveValueLabel.text = string.Empty;
    guild0286Menu.itemNameLabel.text = string.Empty;
    IEnumerator e = guild0286Menu.ResourceLoad();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = guild0286Menu.SetHaveItemIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    guild0286Menu.Setting = new ScrollAreaSetting()
    {
      iconColumnValue = 1,
      iconHeight = 175,
      iconMaxValue = 12,
      iconRowValue = 12,
      iconScreenValue = 8,
      iconWidth = 620
    };
    guild0286Menu.SetPrefab(guild0286Menu.receivePrefab);
    guild0286Menu.SetInitEndAction(new Action(guild0286Menu.InitEndAction));
    e = guild0286Menu.Init(gifts, new Action<GuildMemberGift>(guild0286Menu.IbtnReceive));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator ReceiveConnection(GuildMemberGift gift)
  {
    Guild0286Menu guild0286Menu = this;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<WebAPI.Response.GuildGiftReceiveExecute> receive = WebAPI.GuildGiftReceiveExecute(false, new string[1]
    {
      gift.id
    }, (Action<WebAPI.Response.UserError>) (error =>
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      WebAPI.DefaultUserErrorCallback(error);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e = receive.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (receive.Result != null)
    {
      e = OnDemandDownload.WaitLoadHasUnitResource(false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (receive.Result != null)
      {
        // ISSUE: reference to a compiler-generated method
        guild0286Menu.popup = ModalWindow.Show(Consts.GetInstance().GUILD_GIFT_RECEIVE_TITLE, Consts.GetInstance().GUILD_GIFT_RECEIVE_MESSAGE, new Action(guild0286Menu.\u003CReceiveConnection\u003Eb__29_1));
        guild0286Menu.StartCoroutine(guild0286Menu.UpdateList(receive.Result.player_gift));
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      }
      else
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        MypageScene.ChangeScene();
      }
    }
  }

  private void IbtnReceive(GuildMemberGift gift)
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.ReceiveConnection(gift));
  }

  private IEnumerator ReceiveAll()
  {
    Guild0286Menu guild0286Menu = this;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<WebAPI.Response.GuildGiftReceiveExecute> receive = WebAPI.GuildGiftReceiveExecute(false, ((IEnumerable<GuildMemberGift>) guild0286Menu.memberGifts).Select<GuildMemberGift, string>((Func<GuildMemberGift, string>) (x => x.id)).ToArray<string>(), (Action<WebAPI.Response.UserError>) (error =>
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      WebAPI.DefaultUserErrorCallback(error);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e = receive.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (receive.Result != null)
    {
      e = OnDemandDownload.WaitLoadHasUnitResource(false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (receive.Result != null)
      {
        // ISSUE: reference to a compiler-generated method
        guild0286Menu.popup = ModalWindow.Show(Consts.GetInstance().GUILD_GIFT_RECEIVE_ALL_TITLE, Consts.GetInstance().GUILD_GIFT_RECEIVE_ALL_MESSAGE, new Action(guild0286Menu.\u003CReceiveAll\u003Eb__31_2));
        guild0286Menu.StartCoroutine(guild0286Menu.UpdateList(receive.Result.player_gift));
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      }
      else
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        MypageScene.ChangeScene();
      }
    }
  }
}
