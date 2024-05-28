// Decompiled with JetBrains decompiler
// Type: Mypage0017Menu
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
public class Mypage0017Menu : BackButtonMenuBase
{
  public UIButton IBtnGetAll;
  public UIButton IBtnDeleteAll;
  [SerializeField]
  protected UILabel TxtDate18;
  [SerializeField]
  protected UILabel TxtExplanation22;
  [SerializeField]
  protected UILabel TxtPresetname24;
  [SerializeField]
  protected UILabel TxtTime18;
  [SerializeField]
  protected UILabel TxtTitle30;
  [SerializeField]
  protected GameObject DirEmptyMessage;
  private const int iconWidth = 604;
  private const int iconHeight = 135;
  private const int iconColumnValue = 1;
  private const int iconRowValue = 12;
  private const int iconScreenValue = 8;
  private const int iconMaxValue = 12;
  private const int MAX_RECEIVE = 60;
  public NGxScroll2 scroll;
  private PlayerPresent[] Presents;
  private List<Mypage0017Scroll> allScroll = new List<Mypage0017Scroll>();
  private List<PresentInfo> allPresentInfos = new List<PresentInfo>();
  private bool isInitialize;
  private float scrool_start_y;
  private float scrollPositionBeforePresent;

  public virtual void Foreground() => Debug.Log((object) "click default event Foreground");

  public virtual void IbtnReceive() => Debug.Log((object) "click default event IbtnReceive");

  public virtual void VScrollBar() => Debug.Log((object) "click default event VScrollBar");

  public static Tuple<List<PlayerPresent>, bool> createReceiveList(PlayerPresent[] presents)
  {
    List<PlayerPresent> playerPresentList = new List<PlayerPresent>();
    Player player = SMManager.Get<Player>();
    PlayerUnit[] playerUnitArray = SMManager.Get<PlayerUnit[]>();
    PlayerItem[] source1 = SMManager.Get<PlayerItem[]>();
    SeaPlayer seaPlayer = SMManager.Get<SeaPlayer>();
    PlayerSeaDeck[] playerSeaDeckArray = SMManager.Get<PlayerSeaDeck[]>();
    int num1 = ((IEnumerable<PlayerItem>) ((IEnumerable<PlayerItem>) source1).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.entity_type == MasterDataTable.CommonRewardType.gear && !x.isReisou())).ToArray<PlayerItem>()).Count<PlayerItem>();
    int num2 = ((IEnumerable<PlayerItem>) ((IEnumerable<PlayerItem>) source1).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.entity_type == MasterDataTable.CommonRewardType.gear && x.isReisou())).ToArray<PlayerItem>()).Count<PlayerItem>();
    int num3 = 0;
    int num4 = 0;
    int num5 = 0;
    long num6 = 0;
    int num7 = 0;
    int num8 = 0;
    int num9 = 0;
    int num10 = 0;
    int num11 = 0;
    int num12 = 0;
    int[] numArray = new int[MasterData.SupplySupply.Count<KeyValuePair<int, SupplySupply>>() + 1];
    Dictionary<int, int> dictionary1 = new Dictionary<int, int>();
    Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
    PlayerItem[] source2 = SMManager.Get<PlayerItem[]>().AllSupplies();
    int num13 = 0;
    int idx = 1;
    while (num13 < MasterData.SupplySupply.Count<KeyValuePair<int, SupplySupply>>())
    {
      int cnt = 0;
      ((IEnumerable<PlayerItem>) source2).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.supply.ID == MasterData.SupplySupply[idx].ID)).ForEach<PlayerItem>((Action<PlayerItem>) (x => cnt += x.quantity));
      numArray[MasterData.SupplySupply[idx].ID] = cnt;
      ++num13;
      idx++;
    }
    PlayerQuestKey[] source3 = SMManager.Get<PlayerQuestKey[]>();
    foreach (PlayerQuestKey playerQuestKey in source3)
      dictionary1.Add(playerQuestKey.quest_key_id, playerQuestKey.quantity);
    PlayerRecoveryItem[] source4 = SMManager.Get<PlayerRecoveryItem[]>();
    foreach (PlayerRecoveryItem playerRecoveryItem in source4)
      dictionary2.Add(playerRecoveryItem.recovery_item_id, playerRecoveryItem.quantity);
    bool flag1 = false;
    Dictionary<int, bool> dictionary3 = new Dictionary<int, bool>();
    for (int index = presents.Length - 1; index >= 0; --index)
    {
      PlayerPresent present = presents[index];
      int? rewardQuantity;
      int? nullable;
      if (present.received_at.HasValue)
      {
        present.isReceivable = new bool?(false);
      }
      else
      {
        switch (present.reward_type_id.Value)
        {
          case 1:
          case 24:
            UnitUnit unitUnit;
            if (present.reward_id.HasValue && MasterData.UnitUnit.TryGetValue(present.reward_id.Value, out unitUnit))
            {
              if (unitUnit.IsNormalUnit)
              {
                if (playerUnitArray.Length + num3 < player.max_units)
                {
                  present.isReceivable = new bool?(true);
                  dictionary3[present.reward_type_id.Value] = true;
                  ++num3;
                  continue;
                }
                present.isReceivable = new bool?(false);
                dictionary3[present.reward_type_id.Value] = false;
                continue;
              }
              present.isReceivable = new bool?(true);
              dictionary3[present.reward_type_id.Value] = true;
              continue;
            }
            present.isReceivable = new bool?(false);
            dictionary3[present.reward_type_id.Value] = false;
            continue;
          case 3:
            GearGear gearGear;
            if (present.reward_id.HasValue && MasterData.GearGear.TryGetValue(present.reward_id.Value, out gearGear))
            {
              if (gearGear.isMaterial())
              {
                present.isReceivable = new bool?(true);
                dictionary3[present.reward_type_id.Value] = true;
                continue;
              }
              if (gearGear.isReisou())
              {
                if (num2 + num5 < player.max_reisou_items)
                {
                  ++num5;
                  present.isReceivable = new bool?(true);
                  dictionary3[present.reward_type_id.Value] = true;
                  continue;
                }
                present.isReceivable = new bool?(false);
                dictionary3[present.reward_type_id.Value] = false;
                continue;
              }
              if (num1 + num4 < player.max_items)
              {
                ++num4;
                present.isReceivable = new bool?(true);
                dictionary3[present.reward_type_id.Value] = true;
                continue;
              }
              present.isReceivable = new bool?(false);
              dictionary3[present.reward_type_id.Value] = false;
              continue;
            }
            continue;
          case 4:
            if (player.money + (num6 + (long) present.reward_quantity.Value) <= Consts.GetInstance().MONEY_MAX)
            {
              num6 += (long) present.reward_quantity.Value;
              present.isReceivable = new bool?(true);
              dictionary3[present.reward_type_id.Value] = true;
              continue;
            }
            present.isReceivable = new bool?(false);
            dictionary3[present.reward_type_id.Value] = false;
            continue;
          case 11:
            if (player.ap + player.ap_overflow + num9 < Player.GetApOverChargeLimit())
            {
              num9 += present.reward_quantity.Value;
              present.isReceivable = new bool?(true);
              dictionary3[present.reward_type_id.Value] = true;
              continue;
            }
            present.isReceivable = new bool?(false);
            dictionary3[present.reward_type_id.Value] = false;
            continue;
          case 14:
            if (player.medal + (num8 + present.reward_quantity.Value) <= Consts.GetInstance().MEDAL_MAX)
            {
              num8 += present.reward_quantity.Value;
              present.isReceivable = new bool?(true);
              dictionary3[present.reward_type_id.Value] = true;
              continue;
            }
            present.isReceivable = new bool?(false);
            dictionary3[present.reward_type_id.Value] = false;
            continue;
          case 15:
            if (player.friend_point + (num7 + present.reward_quantity.Value) <= Consts.GetInstance().FRIEND_POINT_MAX)
            {
              num7 += present.reward_quantity.Value;
              present.isReceivable = new bool?(true);
              dictionary3[present.reward_type_id.Value] = true;
              continue;
            }
            present.isReceivable = new bool?(false);
            dictionary3[present.reward_type_id.Value] = false;
            continue;
          case 18:
            if (player.bp + num10 < player.bp_max)
            {
              num10 += present.reward_quantity.Value;
              present.isReceivable = new bool?(true);
              dictionary3[present.reward_type_id.Value] = true;
              continue;
            }
            present.isReceivable = new bool?(false);
            dictionary3[present.reward_type_id.Value] = false;
            continue;
          case 19:
            PlayerQuestKey playerQuestKey = ((IEnumerable<PlayerQuestKey>) source3).Where<PlayerQuestKey>((Func<PlayerQuestKey, bool>) (x =>
            {
              int questKeyId = x.quest_key_id;
              int? rewardId = present.reward_id;
              int valueOrDefault = rewardId.GetValueOrDefault();
              return questKeyId == valueOrDefault & rewardId.HasValue;
            })).FirstOrDefault<PlayerQuestKey>();
            int num14;
            if (playerQuestKey != null)
            {
              rewardQuantity = present.reward_quantity;
              int num15 = dictionary1[playerQuestKey.quest_key_id];
              nullable = rewardQuantity.HasValue ? new int?(rewardQuantity.GetValueOrDefault() + num15) : new int?();
              int maxQuantity = playerQuestKey.max_quantity;
              num14 = nullable.GetValueOrDefault() <= maxQuantity & nullable.HasValue ? 1 : 0;
            }
            else
              num14 = 1;
            if (num14 != 0)
            {
              dictionary1[present.reward_id.Value] += present.reward_quantity.Value;
              present.isReceivable = new bool?(true);
              dictionary3[present.reward_type_id.Value] = true;
              continue;
            }
            present.isReceivable = new bool?(false);
            dictionary3[present.reward_type_id.Value] = false;
            continue;
          case 22:
            if (player.mp + num11 < player.mp_max)
            {
              num11 += present.reward_quantity.Value;
              present.isReceivable = new bool?(true);
              dictionary3[present.reward_type_id.Value] = true;
              continue;
            }
            present.isReceivable = new bool?(false);
            dictionary3[present.reward_type_id.Value] = false;
            continue;
          case 30:
            if (seaPlayer != null && playerSeaDeckArray != null && playerSeaDeckArray.Length != 0 && seaPlayer.dp + num12 < seaPlayer.dp_max)
            {
              num12 += present.reward_quantity.Value;
              present.isReceivable = new bool?(true);
              dictionary3[present.reward_type_id.Value] = true;
              continue;
            }
            present.isReceivable = new bool?(false);
            dictionary3[present.reward_type_id.Value] = false;
            continue;
          case 34:
            int num16 = 0;
            UnitTypeTicket ticketData = MasterData.UnitTypeTicket[present.reward_id.Value];
            PlayerUnitTypeTicket playerUnitTypeTicket = ((IEnumerable<PlayerUnitTypeTicket>) SMManager.Get<PlayerUnitTypeTicket[]>()).FirstOrDefault<PlayerUnitTypeTicket>((Func<PlayerUnitTypeTicket, bool>) (x => x.ticket_id == ticketData.ID));
            if (playerUnitTypeTicket != null)
              num16 = playerUnitTypeTicket.quantity;
            int num17 = num16;
            rewardQuantity = present.reward_quantity;
            nullable = rewardQuantity.HasValue ? new int?(num17 + rewardQuantity.GetValueOrDefault()) : new int?();
            int maxTicket = ticketData.max_ticket;
            if (nullable.GetValueOrDefault() <= maxTicket & nullable.HasValue)
            {
              present.isReceivable = new bool?(true);
              dictionary3[present.reward_type_id.Value] = true;
              continue;
            }
            present.isReceivable = new bool?(false);
            dictionary3[present.reward_type_id.Value] = false;
            continue;
          case 39:
            if (!dictionary2.ContainsKey(present.reward_id.Value))
              dictionary2.Add(present.reward_id.Value, 0);
            PlayerRecoveryItem playerRecoveryItem = ((IEnumerable<PlayerRecoveryItem>) source4).Where<PlayerRecoveryItem>((Func<PlayerRecoveryItem, bool>) (x =>
            {
              int recoveryItemId = x.recovery_item_id;
              int? rewardId = present.reward_id;
              int valueOrDefault = rewardId.GetValueOrDefault();
              return recoveryItemId == valueOrDefault & rewardId.HasValue;
            })).FirstOrDefault<PlayerRecoveryItem>();
            int num18;
            if (playerRecoveryItem != null)
            {
              rewardQuantity = present.reward_quantity;
              int num19 = dictionary2[playerRecoveryItem.recovery_item_id];
              nullable = rewardQuantity.HasValue ? new int?(rewardQuantity.GetValueOrDefault() + num19) : new int?();
              int apRecoveryItemMax = Consts.GetInstance().AP_RECOVERY_ITEM_MAX;
              num18 = nullable.GetValueOrDefault() <= apRecoveryItemMax & nullable.HasValue ? 1 : 0;
            }
            else
              num18 = 1;
            if (num18 != 0)
            {
              dictionary2[present.reward_id.Value] += present.reward_quantity.Value;
              present.isReceivable = new bool?(true);
              dictionary3[present.reward_type_id.Value] = true;
              continue;
            }
            present.isReceivable = new bool?(false);
            dictionary3[present.reward_type_id.Value] = false;
            continue;
          default:
            present.isReceivable = new bool?(true);
            dictionary3[present.reward_type_id.Value] = true;
            continue;
        }
      }
    }
    foreach (PlayerPresent present in presents)
    {
      if (!present.received_at.HasValue && playerPresentList.Count < 60)
      {
        bool? isReceivable = present.isReceivable;
        bool flag2 = false;
        if (!(isReceivable.GetValueOrDefault() == flag2 & isReceivable.HasValue))
          playerPresentList.Add(present);
        else
          flag1 = true;
      }
    }
    return new Tuple<List<PlayerPresent>, bool>(playerPresentList, flag1);
  }

  public IEnumerator Init(PlayerPresent[] presents)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    this.Presents = ((IEnumerable<PlayerPresent>) presents).ToList<PlayerPresent>().OrderByDescending<PlayerPresent, int>((Func<PlayerPresent, int>) (x => x.id)).ToArray<PlayerPresent>();
    if (Mypage0017Menu.createReceiveList(presents).Item1.Count == 0)
    {
      ((UIButtonColor) this.IBtnGetAll).duration = 0.0f;
      ((UIButtonColor) this.IBtnGetAll).isEnabled = false;
    }
    else
      ((UIButtonColor) this.IBtnGetAll).isEnabled = true;
    if (((IEnumerable<PlayerPresent>) presents).Where<PlayerPresent>((Func<PlayerPresent, bool>) (x => x.received_at.HasValue)).ToList<PlayerPresent>().Count > 0)
    {
      ((UIButtonColor) this.IBtnDeleteAll).isEnabled = true;
    }
    else
    {
      ((UIButtonColor) this.IBtnDeleteAll).duration = 0.0f;
      ((UIButtonColor) this.IBtnDeleteAll).isEnabled = false;
    }
    Future<GameObject> prefabF = Res.Prefabs.mypage001_7.vscroll_630_2.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabF.Result;
    this.Initialize();
    this.InitializePresentInfo(this.Presents);
    if (this.allPresentInfos.Count > 0)
    {
      e = this.CreateScrollBase(result);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    this.DirEmptyMessage.SetActive(this.allPresentInfos.Count < 1);
    this.scroll.ResolvePosition();
    this.InitializeEnd();
    if ((double) this.scrollPositionBeforePresent != 0.0)
    {
      this.scroll.ResolvePositionFromScrollValue(this.scrollPositionBeforePresent);
      this.scrollPositionBeforePresent = 0.0f;
    }
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public IEnumerator UpdateList(PlayerPresent[] presents)
  {
    IEnumerator e = this.Init(presents);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator UnitOverPopup()
  {
    IEnumerator e = PopupUtility._999_5_1();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator ItemOverPopup()
  {
    IEnumerator e = PopupUtility._999_6_1(true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator ReisouOverPopup()
  {
    IEnumerator e = PopupUtility.popupMaxReisou();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator LimitOverPopup(string str)
  {
    Future<GameObject> popupPrefabF = Res.Prefabs.popup.popup_001_7_14__anim_popup01.Load<GameObject>();
    IEnumerator e = popupPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = popupPrefabF.Result;
    Singleton<PopupManager>.GetInstance().open(result).GetComponent<Popup001714Menu>().SetText(str);
  }

  private IEnumerator LimitOverPopupMessage(string itemName)
  {
    Future<GameObject> popupPrefabF = Res.Prefabs.popup.popup_001_7_14__anim_popup01.Load<GameObject>();
    IEnumerator e = popupPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = popupPrefabF.Result;
    Singleton<PopupManager>.GetInstance().open(result).GetComponent<Popup001714Menu>().SetMessage(Consts.Format(Consts.GetInstance().POPUP_001714_DESCRIPT_TEXT2, (IDictionary) new Hashtable()
    {
      {
        (object) "Item",
        (object) itemName
      }
    }));
  }

  private IEnumerator ReceiveConnection(PlayerPresent present)
  {
    Mypage0017Menu menu = this;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<WebAPI.Response.PresentRead> receive = WebAPI.PresentRead(new int[1]
    {
      present.id
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
      if (receive.Result != null && receive.Result.is_success)
      {
        e = menu.CharacterStoryPopup(receive.Result.unlock_quests);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        Future<GameObject> popupPrefabF = Res.Prefabs.popup.popup_001_7_6__anim_popup01.Load<GameObject>();
        e = popupPrefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        GameObject result = popupPrefabF.Result;
        Mypage00176Menu component = Singleton<PopupManager>.GetInstance().open(result).GetComponent<Mypage00176Menu>();
        menu.StartCoroutine(component.Init(present, menu));
        menu.SaveScrollPosition();
        menu.StartCoroutine(menu.UpdateList(SMManager.Get<PlayerPresent[]>()));
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        popupPrefabF = (Future<GameObject>) null;
      }
      else
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        MypageScene.ChangeScene();
      }
    }
  }

  public IEnumerator CharacterStoryPopup(UnlockQuest[] unlockQuests)
  {
    if (unlockQuests != null && unlockQuests.Length != 0)
    {
      Future<GameObject> prefab = Res.Prefabs.battle.popup_020_11_2__anim_popup01.Load<GameObject>();
      IEnumerator e = prefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      UnlockQuest[] unlockQuestArray = unlockQuests;
      for (int index = 0; index < unlockQuestArray.Length; ++index)
      {
        QuestCharacterS quest = MasterData.QuestCharacterS[unlockQuestArray[index].quest_s_id];
        Singleton<NGSoundManager>.GetInstance().playSE("SE_1028", delay: 0.8f);
        Battle020112Menu o = this.OpenPopup(prefab.Result).GetComponent<Battle020112Menu>();
        e = o.Init(quest, true);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        bool isFinished = false;
        o.SetCallback((Action) (() => isFinished = true));
        while (!isFinished)
          yield return (object) null;
        yield return (object) new WaitForSeconds(0.6f);
        o = (Battle020112Menu) null;
      }
      unlockQuestArray = (UnlockQuest[]) null;
      unlockQuests = (UnlockQuest[]) null;
    }
  }

  public GameObject OpenPopup(GameObject original)
  {
    GameObject gameObject = Singleton<PopupManager>.GetInstance().open(original);
    ((Component) gameObject.transform.parent.Find("Popup Mask")).gameObject.GetComponent<TweenAlpha>().to = 0.75f;
    return gameObject;
  }

  public void SaveScrollPosition()
  {
    this.scrollPositionBeforePresent = ((Component) this.scroll.scrollView).transform.localPosition.y;
  }

  private void IbtnReceive(PlayerPresent present)
  {
    if (this.IsPushAndSet())
      return;
    Player player = SMManager.Get<Player>();
    PlayerUnit[] playerUnitArray = SMManager.Get<PlayerUnit[]>();
    PlayerItem[] source1 = SMManager.Get<PlayerItem[]>();
    PlayerQuestKey[] source2 = SMManager.Get<PlayerQuestKey[]>();
    PlayerRecoveryItem[] source3 = SMManager.Get<PlayerRecoveryItem[]>();
    int num1 = ((IEnumerable<PlayerItem>) ((IEnumerable<PlayerItem>) source1).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.entity_type == MasterDataTable.CommonRewardType.gear && !x.isReisou())).ToArray<PlayerItem>()).Count<PlayerItem>();
    int num2 = ((IEnumerable<PlayerItem>) ((IEnumerable<PlayerItem>) source1).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.entity_type == MasterDataTable.CommonRewardType.gear && x.isReisou())).ToArray<PlayerItem>()).Count<PlayerItem>();
    if (present.reward_type_id.Value == 1 || present.reward_type_id.Value == 24)
    {
      UnitUnit unitUnit;
      if (!present.reward_id.HasValue || !MasterData.UnitUnit.TryGetValue(present.reward_id.Value, out unitUnit))
        return;
      if (unitUnit.IsNormalUnit)
      {
        if (playerUnitArray.Length >= player.max_units)
          this.StartCoroutine(this.UnitOverPopup());
        else
          this.StartCoroutine(this.ReceiveConnection(present));
      }
      else
        this.StartCoroutine(this.ReceiveConnection(present));
    }
    else if (present.reward_type_id.Value == 3)
    {
      GearGear gearGear;
      if (!present.reward_id.HasValue || !MasterData.GearGear.TryGetValue(present.reward_id.Value, out gearGear))
        return;
      if (gearGear.isMaterial())
        this.StartCoroutine(this.ReceiveConnection(present));
      else if (gearGear.isReisou())
      {
        if (num2 >= player.max_reisou_items)
          this.StartCoroutine(this.ReisouOverPopup());
        else
          this.StartCoroutine(this.ReceiveConnection(present));
      }
      else if (num1 >= player.max_items)
        this.StartCoroutine(this.ItemOverPopup());
      else
        this.StartCoroutine(this.ReceiveConnection(present));
    }
    else if (present.reward_type_id.Value == 19)
    {
      PlayerQuestKey playerQuestKey = ((IEnumerable<PlayerQuestKey>) source2).Where<PlayerQuestKey>((Func<PlayerQuestKey, bool>) (x =>
      {
        int questKeyId = x.quest_key_id;
        int? rewardId = present.reward_id;
        int valueOrDefault = rewardId.GetValueOrDefault();
        return questKeyId == valueOrDefault & rewardId.HasValue;
      })).FirstOrDefault<PlayerQuestKey>();
      if (playerQuestKey != null)
      {
        int quantity = playerQuestKey.quantity;
        int? rewardQuantity = present.reward_quantity;
        int? nullable = rewardQuantity.HasValue ? new int?(quantity + rewardQuantity.GetValueOrDefault()) : new int?();
        int maxQuantity = playerQuestKey.max_quantity;
        if (!(nullable.GetValueOrDefault() <= maxQuantity & nullable.HasValue))
        {
          if (playerQuestKey != null)
          {
            this.StartCoroutine(this.LimitOverPopup(MasterData.QuestkeyQuestkey[playerQuestKey.quest_key_id].name));
            return;
          }
          this.StartCoroutine(this.LimitOverPopup(Consts.GetInstance().UNIQUE_ICON_KEY));
          return;
        }
      }
      this.StartCoroutine(this.ReceiveConnection(present));
    }
    else if (present.reward_type_id.Value == 4)
    {
      if (player.money + (long) present.reward_quantity.Value <= Consts.GetInstance().MONEY_MAX)
        this.StartCoroutine(this.ReceiveConnection(present));
      else
        this.StartCoroutine(this.LimitOverPopup(Consts.GetInstance().UNIQUE_ICON_ZENY));
    }
    else if (present.reward_type_id.Value == 15)
    {
      if (player.friend_point + present.reward_quantity.Value <= Consts.GetInstance().FRIEND_POINT_MAX)
        this.StartCoroutine(this.ReceiveConnection(present));
      else
        this.StartCoroutine(this.LimitOverPopup(Consts.GetInstance().UNIQUE_ICON_POINT));
    }
    else if (present.reward_type_id.Value == 14)
    {
      if (player.medal + present.reward_quantity.Value <= Consts.GetInstance().MEDAL_MAX)
        this.StartCoroutine(this.ReceiveConnection(present));
      else
        this.StartCoroutine(this.LimitOverPopup(Consts.GetInstance().UNIQUE_ICON_MEDAL));
    }
    else if (present.reward_type_id.Value == 11)
    {
      if (player.ap + player.ap_overflow < Player.GetApOverChargeLimit())
        this.StartCoroutine(this.ReceiveConnection(present));
      else
        this.StartCoroutine(this.LimitOverPopupMessage(Consts.GetInstance().AP_NAME));
    }
    else if (present.reward_type_id.Value == 18)
    {
      if (player.bp < player.bp_max)
        this.StartCoroutine(this.ReceiveConnection(present));
      else
        this.StartCoroutine(this.LimitOverPopupMessage(Consts.GetInstance().CP_NAME));
    }
    else if (present.reward_type_id.Value == 22)
    {
      if (player.mp < player.mp_max)
        this.StartCoroutine(this.ReceiveConnection(present));
      else
        this.StartCoroutine(this.LimitOverPopupMessage(Consts.GetInstance().MP_NAME));
    }
    else if (present.reward_type_id.Value == 30)
    {
      SeaPlayer seaPlayer = SMManager.Get<SeaPlayer>();
      PlayerSeaDeck[] playerSeaDeckArray = SMManager.Get<PlayerSeaDeck[]>();
      if (seaPlayer == null || playerSeaDeckArray == null || playerSeaDeckArray.Length == 0)
        ModalWindow.Show("海上編未プレイ", "海上編をはじめていないため、DPは受け取れません。", (Action) (() => this.IsPush = false));
      else if (seaPlayer.dp >= seaPlayer.dp_max)
        this.StartCoroutine(this.LimitOverPopupMessage(Consts.GetInstance().DP_NAME));
      else
        this.StartCoroutine(this.ReceiveConnection(present));
    }
    else if (present.reward_type_id.Value == 29)
      this.StartCoroutine(this.ReceiveConnection(present));
    else if (present.reward_type_id.Value == 34)
    {
      int num3 = 0;
      UnitTypeTicket ticketData = MasterData.UnitTypeTicket[present.reward_id.Value];
      PlayerUnitTypeTicket playerUnitTypeTicket = ((IEnumerable<PlayerUnitTypeTicket>) SMManager.Get<PlayerUnitTypeTicket[]>()).FirstOrDefault<PlayerUnitTypeTicket>((Func<PlayerUnitTypeTicket, bool>) (x => x.ticket_id == ticketData.ID));
      if (playerUnitTypeTicket != null)
        num3 = playerUnitTypeTicket.quantity;
      int num4 = num3;
      int? rewardQuantity = present.reward_quantity;
      int? nullable = rewardQuantity.HasValue ? new int?(num4 + rewardQuantity.GetValueOrDefault()) : new int?();
      int maxTicket = ticketData.max_ticket;
      if (nullable.GetValueOrDefault() > maxTicket & nullable.HasValue)
        this.StartCoroutine(this.LimitOverPopup(Consts.GetInstance().UNIQUE_ICON_UNIT_TYPE_TICKET));
      else
        this.StartCoroutine(this.ReceiveConnection(present));
    }
    else if (present.reward_type_id.Value == 39)
    {
      PlayerRecoveryItem playerRecoveryItem = ((IEnumerable<PlayerRecoveryItem>) source3).Where<PlayerRecoveryItem>((Func<PlayerRecoveryItem, bool>) (x =>
      {
        int recoveryItemId = x.recovery_item_id;
        int? rewardId = present.reward_id;
        int valueOrDefault = rewardId.GetValueOrDefault();
        return recoveryItemId == valueOrDefault & rewardId.HasValue;
      })).FirstOrDefault<PlayerRecoveryItem>();
      if (playerRecoveryItem != null)
      {
        int quantity = playerRecoveryItem.quantity;
        int? rewardQuantity = present.reward_quantity;
        int? nullable = rewardQuantity.HasValue ? new int?(quantity + rewardQuantity.GetValueOrDefault()) : new int?();
        int apRecoveryItemMax = Consts.GetInstance().AP_RECOVERY_ITEM_MAX;
        if (!(nullable.GetValueOrDefault() <= apRecoveryItemMax & nullable.HasValue))
        {
          if (playerRecoveryItem != null)
          {
            this.StartCoroutine(this.LimitOverPopup(MasterData.RecoveryItemAPHeal[playerRecoveryItem.recovery_item_id].name));
            return;
          }
          this.StartCoroutine(this.LimitOverPopup(Consts.GetInstance().UNIQUE_ICON_AP_RECOVERY_ITEM));
          return;
        }
      }
      this.StartCoroutine(this.ReceiveConnection(present));
    }
    else
      this.StartCoroutine(this.ReceiveConnection(present));
  }

  private IEnumerator HavePresentShow(PlayerPresent present)
  {
    Mypage0017Menu menu = this;
    Future<GameObject> popupPrefabF = Res.Prefabs.popup.popup_001_7_6__anim_popup01.Load<GameObject>();
    IEnumerator e = popupPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = popupPrefabF.Result;
    Mypage00176Menu component = Singleton<PopupManager>.GetInstance().open(result).GetComponent<Mypage00176Menu>();
    menu.StartCoroutine(component.Init(present, menu));
  }

  private void IbtnHavePresent(PlayerPresent present)
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.HavePresentShow(present));
  }

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().closeAll();
    Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
    MypageScene.ChangeScene();
  }

  public override void onBackButton() => this.IbtnBack();

  private IEnumerator ReceiveAll()
  {
    Mypage0017Menu menu = this;
    Future<GameObject> popupPrefabF = Res.Prefabs.popup.popup_001_7_13__anim_popup01.Load<GameObject>();
    IEnumerator e = popupPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = popupPrefabF.Result;
    Popup001713Menu component = Singleton<PopupManager>.GetInstance().open(result).GetComponent<Popup001713Menu>();
    menu.StartCoroutine(component.Init(menu, menu.Presents));
  }

  public virtual void IbtnGetall()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.ReceiveAll());
  }

  private IEnumerator DeleteAll()
  {
    Mypage0017Menu menu = this;
    Future<GameObject> popupPrefabF = Res.Prefabs.popup.popup_001_7_a__anim_popup01.Load<GameObject>();
    IEnumerator e = popupPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = popupPrefabF.Result;
    Popup0017aMenu component = Singleton<PopupManager>.GetInstance().open(result).GetComponent<Popup0017aMenu>();
    menu.StartCoroutine(component.Init(menu.Presents, menu));
  }

  public virtual void IbtnDeleteall()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.DeleteAll());
  }

  public void Initialize()
  {
    this.isInitialize = false;
    this.scroll.Clear();
  }

  private void InitializeEnd()
  {
    this.scrool_start_y = ((Component) this.scroll.scrollView).transform.localPosition.y;
    this.isInitialize = true;
  }

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }

  protected void ScrollUpdate()
  {
    if (!this.isInitialize || this.allPresentInfos.Count <= 8)
      return;
    int num1 = 270;
    float num2 = ((Component) this.scroll.scrollView).transform.localPosition.y - this.scrool_start_y;
    float num3 = (float) (Mathf.Max(0, this.allPresentInfos.Count - 8 - 1) / 1 * 135);
    float num4 = 1620f;
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
        GameObject unit = gameObject;
        float num6 = unit.transform.localPosition.y + num2;
        if ((double) num6 > (double) num1)
        {
          int? nullable = this.allPresentInfos.FirstIndexOrNull<PresentInfo>((Func<PresentInfo, bool>) (v => Object.op_Inequality((Object) v.scroll, (Object) null) && Object.op_Equality((Object) ((Component) v.scroll).gameObject, (Object) unit)));
          int info_index = nullable.HasValue ? nullable.Value + 12 : this.allPresentInfos.Count;
          if (nullable.HasValue && info_index < this.allPresentInfos.Count)
          {
            unit.transform.localPosition = new Vector3(unit.transform.localPosition.x, unit.transform.localPosition.y - num4, 0.0f);
            if (info_index >= this.allPresentInfos.Count)
            {
              unit.SetActive(false);
            }
            else
            {
              this.ResetScroll(num5);
              this.StartCoroutine(this.CreateScroll(info_index, num5));
            }
            flag = true;
          }
        }
        else if ((double) num6 < -((double) num4 - (double) num1))
        {
          int num7 = 12;
          if (!unit.activeSelf)
          {
            unit.SetActive(true);
            num7 = 0;
          }
          int? nullable = this.allPresentInfos.FirstIndexOrNull<PresentInfo>((Func<PresentInfo, bool>) (v => Object.op_Inequality((Object) v.scroll, (Object) null) && Object.op_Equality((Object) ((Component) v.scroll).gameObject, (Object) unit)));
          int info_index = nullable.HasValue ? nullable.Value - num7 : -1;
          if (nullable.HasValue && info_index >= 0)
          {
            unit.transform.localPosition = new Vector3(unit.transform.localPosition.x, unit.transform.localPosition.y + num4, 0.0f);
            this.ResetScroll(num5);
            this.StartCoroutine(this.CreateScroll(info_index, num5));
            flag = true;
          }
        }
        ++num5;
      }
    }
    while (flag);
  }

  private void InitializePresentInfo(PlayerPresent[] presents)
  {
    this.allPresentInfos.Clear();
    foreach (PlayerPresent present in presents)
      this.allPresentInfos.Add(new PresentInfo()
      {
        present = present
      });
  }

  private void ResetScroll(int index)
  {
    Mypage0017Scroll scroll = this.allScroll[index];
    ((Component) scroll).gameObject.SetActive(false);
    this.allPresentInfos.Where<PresentInfo>((Func<PresentInfo, bool>) (a => Object.op_Equality((Object) a.scroll, (Object) scroll))).ForEach<PresentInfo>((Action<PresentInfo>) (b => b.scroll = (Mypage0017Scroll) null));
  }

  private IEnumerator CreateScrollBase(GameObject prefab)
  {
    this.allScroll.Clear();
    for (int index = 0; index < Mathf.Min(12, this.allPresentInfos.Count); ++index)
      this.allScroll.Add(Object.Instantiate<GameObject>(prefab).GetComponent<Mypage0017Scroll>());
    this.scroll.Reset();
    for (int index = 0; index < Mathf.Min(12, this.allScroll.Count); ++index)
      this.scroll.AddColumn1(((Component) this.allScroll[index]).gameObject, 604, 135);
    this.scroll.CreateScrollPointHeight(135, this.allPresentInfos.Count);
    this.scroll.ResolvePosition();
    for (int index = 0; index < Mathf.Min(12, this.allPresentInfos.Count); ++index)
      this.ResetScroll(index);
    for (int i = 0; i < Mathf.Min(12, this.allPresentInfos.Count); ++i)
    {
      IEnumerator e = this.CreateScroll(i, i);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private IEnumerator CreateScroll(int info_index, int unit_index)
  {
    Mypage0017Scroll scroll = this.allScroll[unit_index];
    this.allPresentInfos.Where<PresentInfo>((Func<PresentInfo, bool>) (a => Object.op_Equality((Object) a.scroll, (Object) scroll))).ForEach<PresentInfo>((Action<PresentInfo>) (b => b.scroll = (Mypage0017Scroll) null));
    this.allPresentInfos[info_index].scroll = scroll;
    IEnumerator e = scroll.Init(this.allPresentInfos[info_index].present);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((Component) scroll).gameObject.SetActive(true);
    EventDelegate.Set(scroll.GetReceive().onClick, (EventDelegate.Callback) (() => this.IbtnReceive(this.allPresentInfos[info_index].present)));
    EventDelegate.Set(scroll.GetHaveReceive().onClick, (EventDelegate.Callback) (() => this.IbtnHavePresent(this.allPresentInfos[info_index].present)));
  }
}
