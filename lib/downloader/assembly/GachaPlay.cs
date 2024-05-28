// Decompiled with JetBrains decompiler
// Type: GachaPlay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GachaPlay : MonoBehaviour
{
  private static GachaPlay Instance;
  private string mError = string.Empty;

  public static GachaPlay GetInstance()
  {
    if (Object.op_Equality((Object) GachaPlay.Instance, (Object) null))
    {
      GameObject gameObject = GameObject.Find("Gacha Manager");
      if (Object.op_Equality((Object) gameObject, (Object) null))
      {
        gameObject = new GameObject("Gacha Manager");
        Object.DontDestroyOnLoad((Object) gameObject);
      }
      GachaPlay.Instance = gameObject.GetComponent<GachaPlay>();
      if (Object.op_Equality((Object) GachaPlay.Instance, (Object) null))
        GachaPlay.Instance = gameObject.AddComponent<GachaPlay>();
    }
    return GachaPlay.Instance;
  }

  public bool isError => !string.IsNullOrEmpty(this.mError);

  private void cleanup() => this.mError = string.Empty;

  public IEnumerator ChargeGacha(
    string name,
    int num,
    int gacha_id,
    MasterDataTable.GachaType gacha_type,
    int payment_amount)
  {
    GachaPlay gachaPlay = this;
    gachaPlay.cleanup();
    PlayerCommonTicket[] beforePlayerCommonTicket = SMManager.Get<PlayerCommonTicket[]>();
    IEnumerator e;
    if (num != 1)
    {
      e = gachaPlay.ChargeGachaMulti(name, gacha_id, gacha_type, payment_amount);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      switch (gacha_type)
      {
        case MasterDataTable.GachaType.sheet:
          // ISSUE: reference to a compiler-generated method
          Future<WebAPI.Response.GachaG007PanelPay> paramF1 = WebAPI.GachaChargePay<WebAPI.Response.GachaG007PanelPay>(name, num, gacha_id, new Action<WebAPI.Response.UserError>(gachaPlay.\u003CChargeGacha\u003Eb__6_0));
          e = paramF1.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          WebAPI.Response.GachaG007PanelPay result_list1 = paramF1.Result;
          if (result_list1 == null)
          {
            yield break;
          }
          else
          {
            e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<PlayerUnit>) result_list1.player_units, false);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<PlayerUnit>) result_list1.player_unit_reserves, false);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            e = OnDemandDownload.WaitLoadMaterialUnitResource((IEnumerable<PlayerMaterialUnit>) result_list1.player_material_units, false);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            GachaResultData.GetInstance().SetData(result_list1, beforePlayerCommonTicket);
            paramF1 = (Future<WebAPI.Response.GachaG007PanelPay>) null;
            result_list1 = (WebAPI.Response.GachaG007PanelPay) null;
            break;
          }
        case MasterDataTable.GachaType.retry:
          // ISSUE: reference to a compiler-generated method
          Future<WebAPI.Response.GachaG101RetryGiftPay> paramF2 = WebAPI.GachaChargePay<WebAPI.Response.GachaG101RetryGiftPay>(name, num, gacha_id, new Action<WebAPI.Response.UserError>(gachaPlay.\u003CChargeGacha\u003Eb__6_1));
          e = paramF2.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          WebAPI.Response.GachaG101RetryGiftPay result_list2 = paramF2.Result;
          if (result_list2 == null)
          {
            yield break;
          }
          else
          {
            e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<PlayerUnit>) result_list2.temp_player_units, false);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<PlayerUnit>) result_list2.player_unit_reserves, false);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            e = OnDemandDownload.WaitLoadMaterialUnitResource((IEnumerable<PlayerMaterialUnit>) result_list2.player_material_units, false);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            GachaResultData.GetInstance().SetData(result_list2, name, gacha_id, num, beforePlayerCommonTicket);
            paramF2 = (Future<WebAPI.Response.GachaG101RetryGiftPay>) null;
            result_list2 = (WebAPI.Response.GachaG101RetryGiftPay) null;
            break;
          }
        default:
          if (name.Contains("newbie") || name.Contains("tutorial") || gacha_type == MasterDataTable.GachaType.tutorial)
          {
            // ISSUE: reference to a compiler-generated method
            Future<WebAPI.Response.GachaG001ChargePay> paramF3 = WebAPI.GachaChargePay<WebAPI.Response.GachaG001ChargePay>(name, num, gacha_id, new Action<WebAPI.Response.UserError>(gachaPlay.\u003CChargeGacha\u003Eb__6_2));
            e = paramF3.Wait();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            WebAPI.Response.GachaG001ChargePay result_list3 = paramF3.Result;
            if (result_list3 == null)
            {
              yield break;
            }
            else
            {
              e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<PlayerUnit>) result_list3.player_units, false);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<PlayerUnit>) result_list3.player_unit_reserves, false);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              e = OnDemandDownload.WaitLoadMaterialUnitResource((IEnumerable<PlayerMaterialUnit>) result_list3.player_material_units, false);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              GachaResultData.GetInstance().SetData(result_list3);
              paramF3 = (Future<WebAPI.Response.GachaG001ChargePay>) null;
              result_list3 = (WebAPI.Response.GachaG001ChargePay) null;
              break;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            Future<WebAPI.Response.GachaG075ChargePay> paramF4 = WebAPI.GachaChargePay<WebAPI.Response.GachaG075ChargePay>(name, num, gacha_id, new Action<WebAPI.Response.UserError>(gachaPlay.\u003CChargeGacha\u003Eb__6_3));
            e = paramF4.Wait();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            WebAPI.Response.GachaG075ChargePay result_list4 = paramF4.Result;
            if (result_list4 == null)
            {
              yield break;
            }
            else
            {
              e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<PlayerUnit>) result_list4.player_units, false);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<PlayerUnit>) result_list4.player_unit_reserves, false);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              e = OnDemandDownload.WaitLoadMaterialUnitResource((IEnumerable<PlayerMaterialUnit>) result_list4.player_material_units, false);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              GachaResultData.GetInstance().SetData(result_list4, beforePlayerCommonTicket);
              paramF4 = (Future<WebAPI.Response.GachaG075ChargePay>) null;
              result_list4 = (WebAPI.Response.GachaG075ChargePay) null;
              break;
            }
          }
      }
      Singleton<NGGameDataManager>.GetInstance().refreshHomeHome = true;
    }
  }

  public IEnumerator ChargeGachaMulti(
    string name,
    int gacha_id,
    MasterDataTable.GachaType gacha_type,
    int payment_amount)
  {
    GachaPlay gachaPlay = this;
    gachaPlay.cleanup();
    PlayerCommonTicket[] beforePlayerCommonTicket = SMManager.Get<PlayerCommonTicket[]>();
    IEnumerator e;
    switch (gacha_type)
    {
      case MasterDataTable.GachaType.sheet:
        // ISSUE: reference to a compiler-generated method
        Future<WebAPI.Response.GachaG007PanelMultiPay> paramF1 = WebAPI.GachaChargeMultiPay<WebAPI.Response.GachaG007PanelMultiPay>(name, gacha_id, new Action<WebAPI.Response.UserError>(gachaPlay.\u003CChargeGachaMulti\u003Eb__7_0));
        e = paramF1.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        WebAPI.Response.GachaG007PanelMultiPay result_list1 = paramF1.Result;
        if (result_list1 == null)
        {
          yield break;
        }
        else
        {
          e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<PlayerUnit>) result_list1.player_units, false);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<PlayerUnit>) result_list1.player_unit_reserves, false);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          e = OnDemandDownload.WaitLoadMaterialUnitResource((IEnumerable<PlayerMaterialUnit>) result_list1.player_material_units, false);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          GachaResultData.GetInstance().SetData(result_list1, beforePlayerCommonTicket);
          paramF1 = (Future<WebAPI.Response.GachaG007PanelMultiPay>) null;
          result_list1 = (WebAPI.Response.GachaG007PanelMultiPay) null;
          break;
        }
      case MasterDataTable.GachaType.retry:
        // ISSUE: reference to a compiler-generated method
        Future<WebAPI.Response.GachaG101RetryGiftMultiPay> paramF2 = WebAPI.GachaChargeMultiPay<WebAPI.Response.GachaG101RetryGiftMultiPay>(name, gacha_id, new Action<WebAPI.Response.UserError>(gachaPlay.\u003CChargeGachaMulti\u003Eb__7_1));
        e = paramF2.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        WebAPI.Response.GachaG101RetryGiftMultiPay result_list2 = paramF2.Result;
        if (result_list2 == null)
        {
          yield break;
        }
        else
        {
          e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<PlayerUnit>) result_list2.temp_player_units, false);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<PlayerUnit>) result_list2.player_unit_reserves, false);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          e = OnDemandDownload.WaitLoadMaterialUnitResource((IEnumerable<PlayerMaterialUnit>) result_list2.player_material_units, false);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          GachaResultData.GetInstance().SetData(result_list2, name, gacha_id, payment_amount, beforePlayerCommonTicket);
          paramF2 = (Future<WebAPI.Response.GachaG101RetryGiftMultiPay>) null;
          result_list2 = (WebAPI.Response.GachaG101RetryGiftMultiPay) null;
          break;
        }
      default:
        if (name.Contains("newbie") || name.Contains("tutorial") || gacha_type == MasterDataTable.GachaType.tutorial)
        {
          // ISSUE: reference to a compiler-generated method
          Future<WebAPI.Response.GachaG001ChargeMultiPay> paramF3 = WebAPI.GachaChargeMultiPay<WebAPI.Response.GachaG001ChargeMultiPay>(name, gacha_id, new Action<WebAPI.Response.UserError>(gachaPlay.\u003CChargeGachaMulti\u003Eb__7_2));
          e = paramF3.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          WebAPI.Response.GachaG001ChargeMultiPay result_list3 = paramF3.Result;
          if (result_list3 == null)
          {
            yield break;
          }
          else
          {
            e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<PlayerUnit>) result_list3.player_units, false);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<PlayerUnit>) result_list3.player_unit_reserves, false);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            e = OnDemandDownload.WaitLoadMaterialUnitResource((IEnumerable<PlayerMaterialUnit>) result_list3.player_material_units, false);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            GachaResultData.GetInstance().SetData(result_list3);
            paramF3 = (Future<WebAPI.Response.GachaG001ChargeMultiPay>) null;
            result_list3 = (WebAPI.Response.GachaG001ChargeMultiPay) null;
            break;
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          Future<WebAPI.Response.GachaG075ChargeMultiPay> paramF4 = WebAPI.GachaChargeMultiPay<WebAPI.Response.GachaG075ChargeMultiPay>(name, gacha_id, new Action<WebAPI.Response.UserError>(gachaPlay.\u003CChargeGachaMulti\u003Eb__7_3));
          e = paramF4.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          WebAPI.Response.GachaG075ChargeMultiPay result_list4 = paramF4.Result;
          if (result_list4 == null)
          {
            yield break;
          }
          else
          {
            e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<PlayerUnit>) result_list4.player_units, false);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<PlayerUnit>) result_list4.player_unit_reserves, false);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            e = OnDemandDownload.WaitLoadMaterialUnitResource((IEnumerable<PlayerMaterialUnit>) result_list4.player_material_units, false);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            GachaResultData.GetInstance().SetData(result_list4, beforePlayerCommonTicket);
            paramF4 = (Future<WebAPI.Response.GachaG075ChargeMultiPay>) null;
            result_list4 = (WebAPI.Response.GachaG075ChargeMultiPay) null;
            break;
          }
        }
    }
    Singleton<NGGameDataManager>.GetInstance().refreshHomeHome = true;
  }

  public IEnumerator FriendGacha(string name, int num, int gacha_id)
  {
    GachaPlay gachaPlay = this;
    gachaPlay.cleanup();
    // ISSUE: reference to a compiler-generated method
    Future<WebAPI.Response.GachaG002FriendpointPay> paramF = WebAPI.GachaFriendPointPay(name, num, gacha_id, new Action<WebAPI.Response.UserError>(gachaPlay.\u003CFriendGacha\u003Eb__8_0));
    IEnumerator e = paramF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    WebAPI.Response.GachaG002FriendpointPay result_list = paramF.Result;
    if (result_list != null)
    {
      e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<PlayerUnit>) result_list.player_units, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = OnDemandDownload.WaitLoadMaterialUnitResource((IEnumerable<PlayerMaterialUnit>) result_list.player_material_units, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GachaResultData.GetInstance().SetData(result_list);
      Singleton<NGGameDataManager>.GetInstance().refreshHomeHome = true;
    }
  }

  public IEnumerator TicketGacha(
    string name,
    int num,
    GachaModuleGacha gachaData,
    GameObject popupPrefab)
  {
    GachaPlay gachaPlay = this;
    gachaPlay.cleanup();
    // ISSUE: reference to a compiler-generated method
    Future<WebAPI.Response.GachaG004TicketPay> paramF = WebAPI.GachaTicketPay<WebAPI.Response.GachaG004TicketPay>(name, num, gachaData.id, new Action<WebAPI.Response.UserError>(gachaPlay.\u003CTicketGacha\u003Eb__9_0));
    IEnumerator e = paramF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    WebAPI.Response.GachaG004TicketPay result_list = paramF.Result;
    if (result_list != null)
    {
      e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<PlayerUnit>) result_list.player_units, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = OnDemandDownload.WaitLoadMaterialUnitResource((IEnumerable<PlayerMaterialUnit>) result_list.player_material_units, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GachaResultData instance = GachaResultData.GetInstance();
      GachaResultData.ResultData.GachaTicketData gachaTicketData1 = new GachaResultData.ResultData.GachaTicketData(gachaData, name, popupPrefab);
      WebAPI.Response.GachaG004TicketPay data = result_list;
      GachaResultData.ResultData.GachaTicketData gachaTicketData2 = gachaTicketData1;
      instance.SetData(data, gachaTicketData2);
      Singleton<NGGameDataManager>.GetInstance().refreshHomeHome = true;
    }
  }

  public IEnumerator TutorialGacha(
    string name,
    int gacha_id,
    MasterDataTable.GachaType gacha_type,
    int payment_amount)
  {
    GachaPlay gachaPlay = this;
    gachaPlay.cleanup();
    if (Singleton<TutorialRoot>.GetInstance().DecryptGachaData())
    {
      GachaResultData.GetInstance().SetTutorialData(Singleton<TutorialRoot>.GetInstance().Tutorial_gacha_unit_ids, Singleton<TutorialRoot>.GetInstance().Tutorial_gacha_unit_types, Singleton<TutorialRoot>.GetInstance().Tutorial_gacha_direction_types);
    }
    else
    {
      // ISSUE: reference to a compiler-generated method
      Future<WebAPI.Response.GachaG001ChargeMultiPay> paramF = WebAPI.GachaChargeMultiPay<WebAPI.Response.GachaG001ChargeMultiPay>(name, gacha_id, new Action<WebAPI.Response.UserError>(gachaPlay.\u003CTutorialGacha\u003Eb__10_0));
      IEnumerator e = paramF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      WebAPI.Response.GachaG001ChargeMultiPay result_list = paramF.Result;
      if (result_list == null)
      {
        yield break;
      }
      else
      {
        Singleton<TutorialRoot>.GetInstance().EncryptAndSaveGachaData(result_list);
        EventTracker.SendNewPlayerId();
        e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<PlayerUnit>) result_list.player_units, true);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<PlayerUnit>) result_list.player_unit_reserves, true);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        e = OnDemandDownload.WaitLoadMaterialUnitResource((IEnumerable<PlayerMaterialUnit>) result_list.player_material_units, true);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        GachaResultData.GetInstance().SetTutorialData(result_list);
        paramF = (Future<WebAPI.Response.GachaG001ChargeMultiPay>) null;
        result_list = (WebAPI.Response.GachaG001ChargeMultiPay) null;
      }
    }
    Singleton<NGGameDataManager>.GetInstance().refreshHomeHome = true;
  }

  private IEnumerator openWebErrorPopup(WebAPI.Response.UserError error)
  {
    yield return (object) PopupCommon.Show(error.Code, error.Reason, (Action) (() => Singleton<NGSceneManager>.GetInstance().changeScene("gacha006_3", false)));
  }
}
