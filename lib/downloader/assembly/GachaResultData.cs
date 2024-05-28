// Decompiled with JetBrains decompiler
// Type: GachaResultData
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
public class GachaResultData : MonoBehaviour
{
  private static GachaResultData Instance;
  private GachaResultData.ResultData resultData;
  private PlayerCommonTicket[] beforePlayerCommonTicket;

  public static GachaResultData GetInstance()
  {
    if (Object.op_Equality((Object) GachaResultData.Instance, (Object) null))
    {
      GameObject gameObject = GameObject.Find("Gacha Manager");
      if (Object.op_Equality((Object) gameObject, (Object) null))
      {
        gameObject = new GameObject("Gacha Manager");
        Object.DontDestroyOnLoad((Object) gameObject);
      }
      GachaResultData.Instance = gameObject.GetComponent<GachaResultData>();
      if (Object.op_Equality((Object) GachaResultData.Instance, (Object) null))
        GachaResultData.Instance = gameObject.AddComponent<GachaResultData>();
    }
    return GachaResultData.Instance;
  }

  public GachaResultData.ResultData GetData() => this.resultData;

  public void SetData(WebAPI.Response.GachaG001ChargePay data)
  {
    this.resultData = (GachaResultData.ResultData) null;
    this.resultData = new GachaResultData.ResultData();
    this.resultData.resultList = new GachaResultData.Result[data.result.Length];
    this.resultData.playerUnitReserves = data.player_unit_reserves;
    for (int index = 0; index < data.result.Length; ++index)
    {
      GachaResultData.Result result = new GachaResultData.Result();
      result.Set(data.result[index], this.resultData.hasPlayerUnitReserves);
      this.resultData.resultList[index] = result;
    }
    List<GachaResultData.ResultData.AdditionalItem> additionalItemList = new List<GachaResultData.ResultData.AdditionalItem>();
    for (int index = 0; index < data.additional_items.Length; ++index)
      additionalItemList.Add(new GachaResultData.ResultData.AdditionalItem()
      {
        reward_result_id = data.additional_items[index].reward_id,
        reward_type_id = data.additional_items[index].reward_type_id,
        reward_result_quantity = data.additional_items[index].reward_quantity
      });
    this.resultData.unlockQuests = data.unlock_quests;
    this.resultData.additionalItems = additionalItemList.ToArray();
  }

  public void SetData(WebAPI.Response.GachaG001ChargeMultiPay data)
  {
    this.resultData = (GachaResultData.ResultData) null;
    this.resultData = new GachaResultData.ResultData();
    this.resultData.playerUnitReserves = data.player_unit_reserves;
    int length = ((IEnumerable<WebAPI.Response.GachaG001ChargeMultiPayResult>) data.result).Sum<WebAPI.Response.GachaG001ChargeMultiPayResult>((Func<WebAPI.Response.GachaG001ChargeMultiPayResult, int>) (x => x.reward_result_quantity));
    if (length > 10)
    {
      GachaResultData.Result[] source = new GachaResultData.Result[length];
      int index1 = 0;
      for (int index2 = 0; index2 < data.result.Length; ++index2)
      {
        for (int index3 = 0; index3 < data.result[index2].reward_result_quantity; ++index3)
        {
          GachaResultData.Result result = new GachaResultData.Result();
          result.Set(data.result[index2], this.resultData.hasPlayerUnitReserves);
          source[index1] = result;
          ++index1;
        }
      }
      this.resultData.resultList = ((IEnumerable<GachaResultData.Result>) source).OrderBy<GachaResultData.Result, int>((Func<GachaResultData.Result, int>) (x => x.reward_type_id)).ThenByDescending<GachaResultData.Result, bool>((Func<GachaResultData.Result, bool>) (x => x.directionType == GachaDirectionType.pickup)).ThenByDescending<GachaResultData.Result, bool>((Func<GachaResultData.Result, bool>) (x => x.is_new)).ToArray<GachaResultData.Result>();
    }
    else
    {
      this.resultData.resultList = new GachaResultData.Result[data.result.Length];
      for (int index = 0; index < data.result.Length; ++index)
      {
        GachaResultData.Result result = new GachaResultData.Result();
        result.Set(data.result[index], this.resultData.hasPlayerUnitReserves);
        this.resultData.resultList[index] = result;
      }
    }
    List<GachaResultData.ResultData.AdditionalItem> additionalItemList = new List<GachaResultData.ResultData.AdditionalItem>();
    for (int index = 0; index < data.additional_items.Length; ++index)
      additionalItemList.Add(new GachaResultData.ResultData.AdditionalItem()
      {
        reward_result_id = data.additional_items[index].reward_id,
        reward_type_id = data.additional_items[index].reward_type_id,
        reward_result_quantity = data.additional_items[index].reward_quantity
      });
    this.resultData.unlockQuests = data.unlock_quests;
    this.resultData.additionalItems = additionalItemList.ToArray();
  }

  public void SetData(WebAPI.Response.GachaG075ChargePay data, PlayerCommonTicket[] commonTicket)
  {
    this.beforePlayerCommonTicket = commonTicket;
    this.resultData = (GachaResultData.ResultData) null;
    this.resultData = new GachaResultData.ResultData();
    this.resultData.resultList = new GachaResultData.Result[data.result.Length];
    this.resultData.playerUnitReserves = data.player_unit_reserves;
    for (int index = 0; index < data.result.Length; ++index)
    {
      GachaResultData.Result result = new GachaResultData.Result();
      result.Set(data.result[index], this.resultData.hasPlayerUnitReserves);
      this.resultData.resultList[index] = result;
    }
    List<GachaResultData.ResultData.AdditionalItem> additionalItemList = new List<GachaResultData.ResultData.AdditionalItem>();
    for (int index = 0; index < data.additional_items.Length; ++index)
      additionalItemList.Add(new GachaResultData.ResultData.AdditionalItem()
      {
        reward_result_id = data.additional_items[index].reward_id,
        reward_type_id = data.additional_items[index].reward_type_id,
        reward_result_quantity = data.additional_items[index].reward_quantity
      });
    this.resultData.unlockQuests = data.unlock_quests;
    this.resultData.additionalItems = additionalItemList.ToArray();
    this.resultData.playerCommonTicket = data.player_common_tickets;
  }

  public void SetData(
    WebAPI.Response.GachaG075ChargeMultiPay data,
    PlayerCommonTicket[] commonTicket)
  {
    this.beforePlayerCommonTicket = commonTicket;
    this.resultData = (GachaResultData.ResultData) null;
    this.resultData = new GachaResultData.ResultData();
    this.resultData.playerUnitReserves = data.player_unit_reserves;
    int length = ((IEnumerable<WebAPI.Response.GachaG075ChargeMultiPayResult>) data.result).Sum<WebAPI.Response.GachaG075ChargeMultiPayResult>((Func<WebAPI.Response.GachaG075ChargeMultiPayResult, int>) (x => x.reward_result_quantity));
    if (length > 10)
    {
      GachaResultData.Result[] source = new GachaResultData.Result[length];
      int index1 = 0;
      for (int index2 = 0; index2 < data.result.Length; ++index2)
      {
        for (int index3 = 0; index3 < data.result[index2].reward_result_quantity; ++index3)
        {
          GachaResultData.Result result = new GachaResultData.Result();
          result.Set(data.result[index2], this.resultData.hasPlayerUnitReserves);
          source[index1] = result;
          ++index1;
        }
      }
      this.resultData.resultList = ((IEnumerable<GachaResultData.Result>) source).OrderBy<GachaResultData.Result, int>((Func<GachaResultData.Result, int>) (x => x.reward_type_id)).ThenByDescending<GachaResultData.Result, bool>((Func<GachaResultData.Result, bool>) (x => x.directionType == GachaDirectionType.pickup)).ThenByDescending<GachaResultData.Result, bool>((Func<GachaResultData.Result, bool>) (x => x.is_new)).ToArray<GachaResultData.Result>();
    }
    else
    {
      this.resultData.resultList = new GachaResultData.Result[data.result.Length];
      for (int index = 0; index < data.result.Length; ++index)
      {
        GachaResultData.Result result = new GachaResultData.Result();
        result.Set(data.result[index], this.resultData.hasPlayerUnitReserves);
        this.resultData.resultList[index] = result;
      }
    }
    List<GachaResultData.ResultData.AdditionalItem> additionalItemList = new List<GachaResultData.ResultData.AdditionalItem>();
    for (int index = 0; index < data.additional_items.Length; ++index)
      additionalItemList.Add(new GachaResultData.ResultData.AdditionalItem()
      {
        reward_result_id = data.additional_items[index].reward_id,
        reward_type_id = data.additional_items[index].reward_type_id,
        reward_result_quantity = data.additional_items[index].reward_quantity
      });
    this.resultData.unlockQuests = data.unlock_quests;
    this.resultData.additionalItems = additionalItemList.ToArray();
    this.resultData.playerCommonTicket = data.player_common_tickets;
  }

  public void SetData(WebAPI.Response.GachaG002FriendpointPay data)
  {
    this.resultData = (GachaResultData.ResultData) null;
    this.resultData = new GachaResultData.ResultData();
    this.resultData.resultList = new GachaResultData.Result[data.result.Length];
    for (int index = 0; index < data.result.Length; ++index)
    {
      GachaResultData.Result result = new GachaResultData.Result();
      result.Set(data.result[index]);
      this.resultData.resultList[index] = result;
    }
    List<GachaResultData.ResultData.AdditionalItem> additionalItemList = new List<GachaResultData.ResultData.AdditionalItem>();
    for (int index = 0; index < data.additional_items.Length; ++index)
      additionalItemList.Add(new GachaResultData.ResultData.AdditionalItem()
      {
        reward_result_id = data.additional_items[index].reward_id,
        reward_type_id = data.additional_items[index].reward_type_id,
        reward_result_quantity = data.additional_items[index].reward_quantity
      });
    this.resultData.additionalItems = additionalItemList.ToArray();
  }

  public void SetData(
    WebAPI.Response.GachaG004TicketPay data,
    GachaResultData.ResultData.GachaTicketData gachaTicketData)
  {
    this.resultData = new GachaResultData.ResultData();
    this.resultData.resultList = new GachaResultData.Result[data.result.Length];
    for (int index = 0; index < data.result.Length; ++index)
    {
      GachaResultData.Result result = new GachaResultData.Result();
      result.Set(data.result[index]);
      this.resultData.resultList[index] = result;
    }
    List<GachaResultData.ResultData.AdditionalItem> additionalItemList = new List<GachaResultData.ResultData.AdditionalItem>();
    for (int index = 0; index < data.additional_items.Length; ++index)
      additionalItemList.Add(new GachaResultData.ResultData.AdditionalItem()
      {
        reward_result_id = data.additional_items[index].reward_id,
        reward_type_id = data.additional_items[index].reward_type_id,
        reward_result_quantity = data.additional_items[index].reward_quantity
      });
    this.resultData.additionalItems = additionalItemList.ToArray();
    this.resultData.gachaTicketData = gachaTicketData;
    this.resultData.is_ticket = true;
  }

  public void SetData(WebAPI.Response.GachaG007PanelPay data, PlayerCommonTicket[] commonTicket)
  {
    this.beforePlayerCommonTicket = commonTicket;
    this.resultData = (GachaResultData.ResultData) null;
    this.resultData = new GachaResultData.ResultData();
    this.resultData.resultList = new GachaResultData.Result[data.result.Length];
    this.resultData.gachaType = MasterDataTable.GachaType.sheet;
    this.resultData.openPanelResult = data.open_panel_result;
    this.resultData.playerUnitReserves = data.player_unit_reserves;
    for (int index = 0; index < data.result.Length; ++index)
    {
      GachaResultData.Result result = new GachaResultData.Result();
      result.Set(data.result[index], this.resultData.hasPlayerUnitReserves);
      this.resultData.resultList[index] = result;
    }
    List<GachaResultData.ResultData.AdditionalItem> additionalItemList = new List<GachaResultData.ResultData.AdditionalItem>();
    for (int index = 0; index < data.additional_items.Length; ++index)
      additionalItemList.Add(new GachaResultData.ResultData.AdditionalItem()
      {
        reward_result_id = data.additional_items[index].reward_id,
        reward_type_id = data.additional_items[index].reward_type_id,
        reward_result_quantity = data.additional_items[index].reward_quantity
      });
    this.resultData.unlockQuests = data.unlock_quests;
    this.resultData.additionalItems = additionalItemList.ToArray();
    this.resultData.playerCommonTicket = data.player_common_tickets;
  }

  public void SetData(
    WebAPI.Response.GachaG007PanelMultiPay data,
    PlayerCommonTicket[] commonTicket)
  {
    this.beforePlayerCommonTicket = commonTicket;
    this.resultData = (GachaResultData.ResultData) null;
    this.resultData = new GachaResultData.ResultData();
    this.resultData.resultList = new GachaResultData.Result[data.result.Length];
    this.resultData.gachaType = MasterDataTable.GachaType.sheet;
    this.resultData.openPanelResult = data.open_panel_result;
    this.resultData.playerUnitReserves = data.player_unit_reserves;
    for (int index = 0; index < data.result.Length; ++index)
    {
      GachaResultData.Result result = new GachaResultData.Result();
      result.Set(data.result[index], this.resultData.hasPlayerUnitReserves);
      this.resultData.resultList[index] = result;
    }
    this.resultData.unlockQuests = data.unlock_quests;
    List<GachaResultData.ResultData.AdditionalItem> additionalItemList = new List<GachaResultData.ResultData.AdditionalItem>();
    for (int index = 0; index < data.additional_items.Length; ++index)
      additionalItemList.Add(new GachaResultData.ResultData.AdditionalItem()
      {
        reward_result_id = data.additional_items[index].reward_id,
        reward_type_id = data.additional_items[index].reward_type_id,
        reward_result_quantity = data.additional_items[index].reward_quantity
      });
    this.resultData.additionalItems = additionalItemList.ToArray();
    this.resultData.playerCommonTicket = data.player_common_tickets;
  }

  public void SetData(
    WebAPI.Response.GachaG101RetryGiftPay data,
    string gacha_name,
    int gacha_id,
    int roll_count,
    PlayerCommonTicket[] commonTicket)
  {
    this.beforePlayerCommonTicket = commonTicket;
    this.resultData = (GachaResultData.ResultData) null;
    this.resultData = new GachaResultData.ResultData();
    this.resultData.gachaName = gacha_name;
    this.resultData.gachaId = gacha_id;
    this.resultData.rollCount = roll_count;
    this.resultData.gachaType = MasterDataTable.GachaType.retry;
    this.resultData.resultList = new GachaResultData.Result[data.result.Length];
    this.resultData.playerUnitReserves = data.player_unit_reserves;
    this.resultData.remainingRetryCount = new int?(data.remaining_retry_count);
    this.resultData.expiredAt = data.expired_at;
    int index1 = 0;
    int index2 = 0;
    for (; index1 < data.result.Length; ++index1)
    {
      GachaResultData.Result result = new GachaResultData.Result();
      result.Set(data.result[index1]);
      this.resultData.resultList[index1] = result;
      if (data.result[index1].reward_type_id == 1)
      {
        result.reward_id = index1;
        data.temp_player_units[index2].id = index1;
        ++index2;
      }
    }
    this.resultData.is_retry = true;
    this.resultData.playerUnitReserves = data.temp_player_units;
    this.resultData.unlockQuests = data.unlock_quests;
    this.resultData.playerCommonTicket = data.player_common_tickets;
  }

  public void SetData(
    WebAPI.Response.GachaG101RetryGiftMultiPay data,
    string gacha_name,
    int gacha_id,
    int payment_amount,
    PlayerCommonTicket[] commonTicket)
  {
    this.beforePlayerCommonTicket = commonTicket;
    this.resultData = (GachaResultData.ResultData) null;
    this.resultData = new GachaResultData.ResultData();
    this.resultData.gachaName = gacha_name;
    this.resultData.gachaId = gacha_id;
    this.resultData.paymentAmount = payment_amount;
    this.resultData.gachaType = MasterDataTable.GachaType.retry;
    this.resultData.resultList = new GachaResultData.Result[data.result.Length];
    this.resultData.playerUnitReserves = data.player_unit_reserves;
    this.resultData.remainingRetryCount = new int?(data.remaining_retry_count);
    this.resultData.expiredAt = data.expired_at;
    int index1 = 0;
    int index2 = 0;
    for (; index1 < data.result.Length; ++index1)
    {
      GachaResultData.Result result = new GachaResultData.Result();
      result.Set(data.result[index1]);
      this.resultData.resultList[index1] = result;
      if (data.result[index1].reward_type_id == 1)
      {
        result.reward_id = index1;
        data.temp_player_units[index2].id = index1;
        ++index2;
      }
    }
    this.resultData.is_retry = true;
    this.resultData.playerUnitReserves = data.temp_player_units;
    this.resultData.unlockQuests = data.unlock_quests;
    this.resultData.playerCommonTicket = data.player_common_tickets;
  }

  public void SetData(
    WebAPI.Response.GachaResume data,
    string gacha_name,
    int gacha_id,
    int roll_count,
    int payment_amount,
    PlayerCommonTicket[] commonTicket)
  {
    this.beforePlayerCommonTicket = commonTicket;
    this.resultData = (GachaResultData.ResultData) null;
    this.resultData = new GachaResultData.ResultData();
    this.resultData.gachaName = gacha_name;
    this.resultData.gachaId = gacha_id;
    this.resultData.rollCount = roll_count;
    this.resultData.paymentAmount = payment_amount;
    this.resultData.gachaType = MasterDataTable.GachaType.retry;
    this.resultData.resultList = new GachaResultData.Result[data.result.Length];
    this.resultData.playerUnitReserves = data.player_unit_reserves;
    this.resultData.remainingRetryCount = new int?(data.remaining_retry_count);
    this.resultData.expiredAt = data.expired_at;
    int index1 = 0;
    int index2 = 0;
    for (; index1 < data.result.Length; ++index1)
    {
      GachaResultData.Result result = new GachaResultData.Result();
      result.Set(data.result[index1]);
      this.resultData.resultList[index1] = result;
      if (data.result[index1].reward_type_id == 1)
      {
        result.reward_id = index1;
        data.temp_player_units[index2].id = index1;
        ++index2;
      }
    }
    this.resultData.is_retry = true;
    this.resultData.playerUnitReserves = data.temp_player_units;
    this.resultData.unlockQuests = data.unlock_quests;
  }

  public void SetData(WebAPI.Response.SelectticketSpend data)
  {
    this.resultData = (GachaResultData.ResultData) null;
    this.resultData = new GachaResultData.ResultData();
    this.resultData.resultList = new GachaResultData.Result[0];
    this.resultData.unlockQuests = data.unlock_quests;
  }

  public void SetTutorialData(WebAPI.Response.GachaG001ChargeMultiPay data)
  {
    this.SetData(data);
    this.resultData.unlockQuests = new UnlockQuest[0];
    this.resultData.additionalItems = new GachaResultData.ResultData.AdditionalItem[0];
  }

  public void SetTutorialData(int[] unit_ids, int[] unit_types, int[] director_types)
  {
    List<PlayerUnit> playerUnitList = new List<PlayerUnit>();
    for (int ID = 0; ID < unit_ids.Length; ++ID)
      playerUnitList.Add(PlayerUnit.FromUnit(MasterData.UnitUnit[unit_ids[ID]], unit_types[ID], ID));
    this.resultData = (GachaResultData.ResultData) null;
    this.resultData = new GachaResultData.ResultData();
    this.resultData.is_retry = false;
    this.resultData.resultList = new GachaResultData.Result[unit_ids.Length];
    this.resultData.unlockQuests = new UnlockQuest[0];
    this.resultData.additionalItems = new GachaResultData.ResultData.AdditionalItem[0];
    this.resultData.playerUnitReserves = playerUnitList.ToArray();
    for (int index = 0; index < this.resultData.resultList.Length; ++index)
      this.resultData.resultList[index] = new GachaResultData.Result()
      {
        is_new = true,
        reward_result_id = playerUnitList[index].id,
        reward_result_quantity = 1,
        reward_type_id = 1,
        is_reserves = true,
        directionType = (GachaDirectionType) director_types[index]
      };
  }

  public bool IsPopupEffect()
  {
    bool flag = true;
    if ((this.resultData.unlockQuests == null || this.resultData.unlockQuests.Length == 0) && this.resultData.openPanelResult == null && !this.hasCoinResult())
      flag = false;
    return flag;
  }

  public bool hasCoinResult()
  {
    return this.resultData.playerCommonTicket != null && this.resultData.playerCommonTicket.Length != 0;
  }

  public IEnumerator CharacterStoryPopup()
  {
    Future<GameObject> prefab = Res.Prefabs.battle.popup_020_11_2__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (this.resultData.unlockQuests != null && this.resultData.unlockQuests.Length != 0)
    {
      UnlockQuest[] unlockQuestArray = this.resultData.unlockQuests;
      for (int index = 0; index < unlockQuestArray.Length; ++index)
      {
        QuestCharacterS quest = MasterData.QuestCharacterS[unlockQuestArray[index].quest_s_id];
        Singleton<NGSoundManager>.GetInstance().playSE("SE_1028", delay: 0.8f);
        Battle020112Menu o = this.OpenPopup(prefab.Result).GetComponent<Battle020112Menu>();
        e = o.Init(quest);
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
      this.resultData.unlockQuests = (UnlockQuest[]) null;
    }
  }

  public IEnumerator SheetGachaResultPopup()
  {
    if (this.resultData.openPanelResult != null)
    {
      yield return (object) new WaitForSeconds(1f);
      Future<GameObject> prefabCursolF = Res.Prefabs.gacha006_effect.SheetGacha.PsCursol.Load<GameObject>();
      IEnumerator e = prefabCursolF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Future<GameObject> prefabHitF = Res.Prefabs.gacha006_effect.SheetGacha.PsCursolHit.Load<GameObject>();
      e = prefabHitF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Future<GameObject> prefabF = Res.Prefabs.gacha006_3.dir_SheetGacha.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject sheedPopup = prefabF.Result.Clone();
      sheedPopup.SetActive(false);
      Popup0063SheetMenu script = sheedPopup.GetComponent<Popup0063SheetMenu>();
      GachaG007PlayerSheet[] sheet = SMManager.Get<GachaG007PlayerSheet[]>();
      GachaG007PlayerPanel[] panels = ((IEnumerable<GachaG007PlayerPanel>) this.resultData.openPanelResult.player_sheet.player_panels).OrderBy<GachaG007PlayerPanel, int>((Func<GachaG007PlayerPanel, int>) (x => x.position)).ToArray<GachaG007PlayerPanel>();
      e = script.Init(panels, this.resultData.openPanelResult.player_sheet, this.resultData.openPanelResult.open_panel_position, true, prefabCursolF.Result, prefabHitF.Result);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<PopupManager>.GetInstance().open(sheedPopup, isCloned: true);
      sheedPopup.SetActive(true);
      e = script.StartSelEffect(panels, this.resultData.openPanelResult.open_panel_position);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      yield return (object) new WaitForSeconds(0.5f);
      script.HitEffect(this.resultData.openPanelResult.open_panel_position - 1);
      yield return (object) new WaitForSeconds(2f);
      GachaG007PlayerPanel playerPanel1 = ((IEnumerable<GachaG007PlayerPanel>) this.resultData.openPanelResult.player_sheet.player_panels).FirstOrDefault<GachaG007PlayerPanel>((Func<GachaG007PlayerPanel, bool>) (x => x.position == this.resultData.openPanelResult.open_panel_position));
      if (playerPanel1 != null)
      {
        e = script.GetItemEffect(playerPanel1);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      bool flag = true;
      foreach (GachaG007PlayerPanel playerPanel2 in this.resultData.openPanelResult.player_sheet.player_panels)
      {
        if (!playerPanel2.is_opened && playerPanel2.position != this.resultData.openPanelResult.open_panel_position)
        {
          flag = false;
          break;
        }
      }
      if (flag)
      {
        e = script.SheetCompleteEffect();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      bool isFinished = false;
      script.SetCallback((Action) (() => isFinished = true));
      if (sheet != null && sheet.Length != 0)
      {
        e = script.SheetResetPopup(sheet[0]);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      script.SetBackBtnEnable(true);
      while (!isFinished)
        yield return (object) null;
      yield return (object) new WaitForSeconds(0.6f);
      this.resultData.openPanelResult = (GachaG007OpenPanelResult) null;
    }
  }

  public IEnumerator StartSheetEffect()
  {
    GachaResultData gachaResultData = this;
    Resolution windowSize = Screen.currentResolution;
    Future<Sprite> textureLoader = Res.Prefabs.BackGround.black.Load<Sprite>();
    IEnumerator e = textureLoader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject colorLayer = new GameObject("Color Layer")
    {
      transform = {
        parent = ((Component) gachaResultData).gameObject.transform
      },
      layer = ((Component) gachaResultData).gameObject.layer
    };
    colorLayer.transform.localScale = new Vector3(1f, 1f, 1f);
    UIPanel uiPanel = colorLayer.AddComponent<UIPanel>();
    UI2DSprite ui2Dsprite = colorLayer.AddComponent<UI2DSprite>();
    uiPanel.depth = 300;
    ui2Dsprite.sprite2D = textureLoader.Result;
    ((UIRect) ui2Dsprite).alpha = 0.75f;
    ((UIWidget) ui2Dsprite).height = ((Resolution) ref windowSize).height;
    ((UIWidget) ui2Dsprite).width = ((Resolution) ref windowSize).width;
    Future<GameObject> loader = Res.Prefabs.gacha006_3.SheetGacha_Start_eff.Load<GameObject>();
    e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject gameObject = loader.Result.Clone(colorLayer.transform);
    bool isFinished = false;
    e = gameObject.GetComponent<SheetGachaStart>().Init((Action) (() => isFinished = true));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    while (!isFinished)
      yield return (object) null;
    Object.DestroyObject((Object) colorLayer);
  }

  public GameObject OpenPopup(GameObject original)
  {
    GameObject gameObject = Singleton<PopupManager>.GetInstance().open(original);
    ((Component) gameObject.transform.parent.Find("Popup Mask")).gameObject.GetComponent<TweenAlpha>().to = 0.75f;
    return gameObject;
  }

  public IEnumerator CoinAcquisitionPopup()
  {
    if (this.hasCoinResult())
    {
      int common_ticket_id = this.resultData.playerCommonTicket[0].ticket_id;
      int quantity1 = this.resultData.playerCommonTicket[0].quantity;
      PlayerCommonTicket playerCommonTicket = ((IEnumerable<PlayerCommonTicket>) this.beforePlayerCommonTicket).FirstOrDefault<PlayerCommonTicket>((Func<PlayerCommonTicket, bool>) (x => x.ticket_id == common_ticket_id));
      int quantity2 = playerCommonTicket == null ? 0 : playerCommonTicket.quantity;
      int acquisitionValue = quantity1 - quantity2;
      if (acquisitionValue != 0)
      {
        Future<GameObject> prefab = new ResourceObject("Prefabs/gacha006_3/popup_Coin_Acquisition").Load<GameObject>();
        IEnumerator e = prefab.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        e = Singleton<PopupManager>.GetInstance().open(prefab.Result).GetComponent<PopupCoinAcquisition>().Init(common_ticket_id, acquisitionValue);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.resultData.playerCommonTicket = (PlayerCommonTicket[]) null;
      }
    }
  }

  public class Result
  {
    public bool is_new;
    public int reward_id;
    public int reward_result_id;
    public int reward_result_quantity;
    public int reward_type_id;
    public bool is_reserves;
    public GachaDirectionType directionType;
    public bool isChangeEffect;
    public GachaDirectionType? changeDirectionType;

    public UnlockQuest unlock_quest { get; set; }

    public void Set(
      WebAPI.Response.GachaG001ChargeMultiPayResult result,
      bool isReserves)
    {
      this.is_new = result.is_new;
      this.reward_result_id = result.reward_result_id;
      this.reward_result_quantity = result.reward_result_quantity;
      this.reward_type_id = result.reward_type_id;
      this.is_reserves = isReserves;
      this.directionType = (GachaDirectionType) result.direction_type_id;
      this.SetChangeEffectData();
    }

    public void Set(WebAPI.Response.GachaG001ChargePayResult result, bool isReserves)
    {
      this.is_new = result.is_new;
      this.reward_result_id = result.reward_result_id;
      this.reward_result_quantity = result.reward_result_quantity;
      this.reward_type_id = result.reward_type_id;
      this.is_reserves = isReserves;
      this.directionType = (GachaDirectionType) result.direction_type_id;
      this.SetChangeEffectData();
    }

    public void Set(
      WebAPI.Response.GachaG075ChargeMultiPayResult result,
      bool isReserves)
    {
      this.is_new = result.is_new;
      this.reward_result_id = result.reward_result_id;
      this.reward_result_quantity = result.reward_result_quantity;
      this.reward_type_id = result.reward_type_id;
      this.is_reserves = isReserves;
      this.directionType = (GachaDirectionType) result.direction_type_id;
      this.SetChangeEffectData();
    }

    public void Set(WebAPI.Response.GachaG075ChargePayResult result, bool isReserves)
    {
      this.is_new = result.is_new;
      this.reward_result_id = result.reward_result_id;
      this.reward_result_quantity = result.reward_result_quantity;
      this.reward_type_id = result.reward_type_id;
      this.is_reserves = isReserves;
      this.directionType = (GachaDirectionType) result.direction_type_id;
      this.SetChangeEffectData();
    }

    public void Set(
      WebAPI.Response.GachaG002FriendpointPayResult result)
    {
      this.is_new = result.is_new;
      this.reward_result_id = result.reward_result_id;
      this.reward_result_quantity = result.reward_result_quantity;
      this.reward_type_id = result.reward_type_id;
      this.directionType = (GachaDirectionType) result.direction_type_id;
      this.SetChangeEffectData();
    }

    public void Set(WebAPI.Response.GachaG004TicketPayResult result)
    {
      this.is_new = result.is_new;
      this.reward_result_id = result.reward_result_id;
      this.reward_result_quantity = result.reward_result_quantity;
      this.reward_type_id = result.reward_type_id;
      this.directionType = (GachaDirectionType) result.direction_type_id;
      this.SetChangeEffectData();
    }

    public void Set(WebAPI.Response.GachaG007PanelPayResult result, bool isReserves)
    {
      this.is_new = result.is_new;
      this.reward_result_id = result.reward_result_id;
      this.reward_result_quantity = result.reward_result_quantity;
      this.reward_type_id = result.reward_type_id;
      this.is_reserves = isReserves;
      this.directionType = (GachaDirectionType) result.direction_type_id;
      this.SetChangeEffectData();
    }

    public void Set(
      WebAPI.Response.GachaG007PanelMultiPayResult result,
      bool isReserves)
    {
      this.is_new = result.is_new;
      this.reward_result_id = result.reward_result_id;
      this.reward_result_quantity = result.reward_result_quantity;
      this.reward_type_id = result.reward_type_id;
      this.is_reserves = isReserves;
      this.directionType = (GachaDirectionType) result.direction_type_id;
      this.SetChangeEffectData();
    }

    public void Set(WebAPI.Response.GachaG101RetryGiftPayResult result)
    {
      this.is_new = result.is_new;
      this.reward_id = result.reward_id;
      this.reward_result_id = result.reward_result_id;
      this.reward_result_quantity = result.reward_result_quantity;
      this.reward_type_id = result.reward_type_id;
      this.is_reserves = true;
      this.directionType = (GachaDirectionType) result.direction_type_id;
      this.SetChangeEffectData();
    }

    public void Set(
      WebAPI.Response.GachaG101RetryGiftMultiPayResult result)
    {
      this.is_new = result.is_new;
      this.reward_id = result.reward_id;
      this.reward_result_id = result.reward_result_id;
      this.reward_result_quantity = result.reward_result_quantity;
      this.reward_type_id = result.reward_type_id;
      this.is_reserves = true;
      this.directionType = (GachaDirectionType) result.direction_type_id;
      this.SetChangeEffectData();
    }

    public void Set(WebAPI.Response.GachaResumeResult result)
    {
      this.is_new = result.is_new;
      this.reward_id = result.reward_id;
      this.reward_type_id = result.reward_type_id;
      this.reward_result_id = result.reward_result_id;
      this.reward_result_quantity = result.reward_result_quantity;
      this.is_reserves = true;
    }

    private void SetChangeEffectData()
    {
      this.isChangeEffect = (this.directionType == GachaDirectionType.pickup || this.directionType == GachaDirectionType.high) && this.ProbabilityEffectShow(Consts.GetInstance().GACHA_CHANGE_EFFECT_VALUE);
      if (!this.isChangeEffect)
        return;
      this.changeDirectionType = new GachaDirectionType?((GachaDirectionType) this.ProbabilityEffectStartColor(this.directionType == GachaDirectionType.pickup));
    }

    private bool ProbabilityEffectShow(float probability)
    {
      return (double) Random.Range(0.0f, 1f) <= (double) probability;
    }

    private int ProbabilityEffectStartColor(bool isPickUp)
    {
      return isPickUp ? Random.Range(1, 4) : Random.Range(1, 3);
    }
  }

  public class ResultData
  {
    public string gachaName;
    public int gachaId;
    public int rollCount;
    public int paymentAmount;
    public int? remainingRetryCount;
    public DateTime? expiredAt;
    public MasterDataTable.GachaType gachaType;
    public GachaResultData.Result[] resultList;
    public GachaResultData.ResultData.AdditionalItem[] additionalItems;
    public bool is_retry;
    public bool is_ticket;
    public GachaResultData.ResultData.GachaTicketData gachaTicketData;

    public UnlockQuest[] unlockQuests { get; set; }

    public GachaG007OpenPanelResult openPanelResult { get; set; }

    public PlayerUnit[] playerUnitReserves { get; set; }

    public PlayerCommonTicket[] playerCommonTicket { get; set; }

    public GachaResultData.Result[] GetResultData() => this.resultList;

    public GachaResultData.ResultData.AdditionalItem[] GetAdditionalData() => this.additionalItems;

    public UnlockQuest[] GetUnlockQuestData() => this.unlockQuests;

    public PlayerUnit[] GetPlayerUnitReserves() => this.playerUnitReserves;

    public bool hasPlayerUnitReserves
    {
      get => this.playerUnitReserves != null && this.playerUnitReserves.Length != 0;
    }

    public class AdditionalItem
    {
      public GameObject unknownObject;
      public GameObject gameObject;
      public int reward_result_quantity;
      public int reward_type_id;
      public int reward_result_id;
      public bool is_new;
      public bool is_reserves;
    }

    public class GachaTicketData
    {
      public GachaModuleGacha gachaData;
      public string gachaName;
      public GameObject popupPrefab;

      public GachaTicketData(GachaModuleGacha gachaData, string gachaName, GameObject popupPrefab)
      {
        this.gachaData = gachaData;
        this.gachaName = gachaName;
        this.popupPrefab = popupPrefab;
      }
    }
  }
}
