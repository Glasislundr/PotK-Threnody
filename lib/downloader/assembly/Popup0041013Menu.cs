// Decompiled with JetBrains decompiler
// Type: Popup0041013Menu
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
public class Popup0041013Menu : BackButtonMenuBase
{
  private List<PlayerUnit> sellUnitIcons = new List<PlayerUnit>();
  private Unit00410Menu menuFromHime;
  private Unit004StorageSellMenu menuFromStorage;
  private bool comeFromStorage;
  [SerializeField]
  private Vector3 TextYPos1;
  [SerializeField]
  private Vector3 TextYPos2;
  private bool isPageing;
  [SerializeField]
  private GameObject txtDescription1;
  [SerializeField]
  private GameObject txtDescription2;
  [SerializeField]
  private GameObject txtDescriptionLimit;

  public IEnumerator Init(
    List<PlayerUnit> icons,
    Unit00410Menu menu,
    bool isOverAlert,
    bool isMemoryAlert)
  {
    this.menuFromHime = menu;
    this.sellUnitIcons = icons;
    this.isPageing = isOverAlert & isMemoryAlert;
    this.txtDescription1.SetActive(true);
    this.txtDescription2.SetActive(false);
    this.txtDescriptionLimit.SetActive(false);
    if (this.isPageing)
    {
      this.txtDescriptionLimit.transform.localPosition = this.TextYPos1;
      this.txtDescriptionLimit.SetActive(false);
      this.txtDescription2.transform.localPosition = this.TextYPos2;
      this.txtDescription2.SetActive(true);
    }
    else if (isOverAlert)
    {
      this.txtDescriptionLimit.transform.localPosition = this.TextYPos2;
      this.txtDescriptionLimit.SetActive(true);
    }
    else if (isMemoryAlert)
    {
      this.txtDescription2.transform.localPosition = this.TextYPos2;
      this.txtDescription2.SetActive(true);
      yield break;
    }
  }

  public IEnumerator Init(
    List<PlayerUnit> icons,
    Unit004StorageSellMenu menu,
    bool isOverAlert,
    bool isMemoryAlert)
  {
    this.menuFromStorage = menu;
    this.comeFromStorage = true;
    this.sellUnitIcons = icons;
    this.isPageing = isOverAlert & isMemoryAlert;
    this.txtDescription1.SetActive(true);
    this.txtDescription2.SetActive(false);
    this.txtDescriptionLimit.SetActive(false);
    if (this.isPageing)
    {
      this.txtDescriptionLimit.transform.localPosition = this.TextYPos1;
      this.txtDescriptionLimit.SetActive(false);
      this.txtDescription2.transform.localPosition = this.TextYPos2;
      this.txtDescription2.SetActive(true);
    }
    else if (isOverAlert)
    {
      this.txtDescriptionLimit.transform.localPosition = this.TextYPos2;
      this.txtDescriptionLimit.SetActive(true);
    }
    else if (isMemoryAlert)
    {
      this.txtDescription2.transform.localPosition = this.TextYPos2;
      this.txtDescription2.SetActive(true);
      yield break;
    }
  }

  private IEnumerator UnitSellAsync()
  {
    Popup0041013Menu popup0041013Menu = this;
    if (!Singleton<CommonRoot>.GetInstance().isLoading)
    {
      Singleton<PopupManager>.GetInstance().onDismiss(true);
      Singleton<CommonRoot>.GetInstance().loadingMode = 1;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      List<int> player_unit_ids = new List<int>();
      List<int> player_material_unit_ids = new List<int>();
      popup0041013Menu.sellUnitIcons.ForEach((Action<PlayerUnit>) (ic =>
      {
        if (ic.unit.IsNormalUnit)
          player_unit_ids.Add(ic.id);
        else
          player_material_unit_ids.Add(ic.id);
      }));
      Player player = SMManager.Get<Player>();
      long sendZeny = 0;
      int sendMedal = 0;
      IEnumerator e1;
      if (popup0041013Menu.comeFromStorage)
      {
        // ISSUE: reference to a compiler-generated method
        Future<WebAPI.Response.UnitReservesSell> ft = WebAPI.UnitReservesSell(player_unit_ids.ToArray(), new Action<WebAPI.Response.UserError>(popup0041013Menu.\u003CUnitSellAsync\u003Eb__12_1));
        e1 = ft.Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        if (ft.Result == null)
        {
          yield break;
        }
        else
        {
          Future<WebAPI.Response.UnitReservesIndex> f = WebAPI.UnitReservesIndex((Action<WebAPI.Response.UserError>) (e =>
          {
            Singleton<CommonRoot>.GetInstance().loadingMode = 0;
            WebAPI.DefaultUserErrorCallback(e);
          }));
          e1 = f.Wait();
          while (e1.MoveNext())
            yield return e1.Current;
          e1 = (IEnumerator) null;
          PlayerUnit[] playerUnits = f.Result.player_units;
          e1 = popup0041013Menu.menuFromStorage.Init(playerUnits);
          while (e1.MoveNext())
            yield return e1.Current;
          e1 = (IEnumerator) null;
          sendZeny = ft.Result.player.money - player.money;
          sendMedal = ft.Result.player.medal - player.medal;
          ft = (Future<WebAPI.Response.UnitReservesSell>) null;
          f = (Future<WebAPI.Response.UnitReservesIndex>) null;
        }
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        Future<WebAPI.Response.UnitSell> ft = WebAPIUtil.UnitSell(player_material_unit_ids.ToArray(), player_unit_ids.ToArray(), new Action<WebAPI.Response.UserError>(popup0041013Menu.\u003CUnitSellAsync\u003Eb__12_4));
        e1 = ft.Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        if (ft.Result == null)
        {
          yield break;
        }
        else
        {
          Singleton<NGGameDataManager>.GetInstance().corps_player_unit_ids = new HashSet<int>((IEnumerable<int>) ft.Result.corps_player_unit_ids);
          PlayerUnit[] playerUnits = SMManager.Get<PlayerUnit[]>();
          PlayerMaterialUnit[] playerMaterialUnits = SMManager.Get<PlayerMaterialUnit[]>();
          PlayerDeck[] playerDeck = SMManager.Get<PlayerDeck[]>();
          popup0041013Menu.StartCoroutine(popup0041013Menu.menuFromHime.Init(playerDeck, player, playerUnits, playerMaterialUnits, false, popup0041013Menu.menuFromHime.fromType_));
          sendZeny = ft.Result.player.money - player.money;
          sendMedal = ft.Result.player.medal - player.medal;
          ft = (Future<WebAPI.Response.UnitSell>) null;
        }
      }
      Future<GameObject> prefab0041012F = Res.Prefabs.popup.popup_004_10_12__anim_popup01.Load<GameObject>();
      e1 = prefab0041012F.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      GameObject result = prefab0041012F.Result;
      Singleton<PopupManager>.GetInstance().openAlert(result).GetComponent<Unit0041012Menu>().SetText(sendZeny, sendMedal);
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      player = (Player) null;
      prefab0041012F = (Future<GameObject>) null;
    }
  }

  public void IbtnYes()
  {
    if (this.isPageing)
    {
      this.txtDescription1.SetActive(false);
      this.txtDescription2.SetActive(false);
      this.txtDescriptionLimit.SetActive(true);
      this.isPageing = false;
    }
    else
      this.StartCoroutine(this.UnitSellAsync());
  }

  public void IbtnNo() => Singleton<PopupManager>.GetInstance().onDismiss();

  public override void onBackButton() => this.IbtnNo();
}
