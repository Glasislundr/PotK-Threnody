// Decompiled with JetBrains decompiler
// Type: Popup004UnitTransformConfirm
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Popup004UnitTransformConfirm : BackButtonMenuBase
{
  private UnitStorageMenuBase menu;
  private List<PlayerUnit> storageUnitIcons = new List<PlayerUnit>();
  private Popup004UnitTransformConfirm.StorageConfirmMode confirmMode;
  [SerializeField]
  private UILabel txtDescription;
  [SerializeField]
  private GameObject txtDescription2;
  private bool isMemoryAlert;

  public void Init(
    UnitStorageMenuBase menu,
    List<PlayerUnit> storageUnitIcons,
    Popup004UnitTransformConfirm.StorageConfirmMode confirmMode)
  {
    this.menu = menu;
    this.storageUnitIcons = storageUnitIcons;
    this.confirmMode = confirmMode;
    string text = Consts.GetInstance().popup_004_Unit_Transform_Confirm_Out;
    if (this.confirmMode == Popup004UnitTransformConfirm.StorageConfirmMode.StorageIn)
    {
      this.isMemoryAlert = false;
      int?[] source = PlayerTransmigrateMemoryPlayerUnitIds.Current != null ? PlayerTransmigrateMemoryPlayerUnitIds.Current.transmigrate_memory_player_unit_ids : new int?[0];
      foreach (PlayerUnit storageUnitIcon in storageUnitIcons)
      {
        PlayerUnit unit = storageUnitIcon;
        if (unit != (PlayerUnit) null && !this.isMemoryAlert)
        {
          this.isMemoryAlert = ((IEnumerable<int?>) source).Any<int?>((Func<int?, bool>) (x =>
          {
            if (!x.HasValue)
              return false;
            int? nullable = x;
            int id = unit.id;
            return nullable.GetValueOrDefault() == id & nullable.HasValue;
          }));
          if (this.isMemoryAlert)
            break;
        }
      }
      text = Consts.GetInstance().popup_004_Unit_Transform_Confirm_In;
    }
    this.txtDescription.SetTextLocalize(text);
    this.txtDescription2.SetActive(false);
  }

  private IEnumerator StorageIn()
  {
    Popup004UnitTransformConfirm transformConfirm = this;
    if (!Singleton<CommonRoot>.GetInstance().isLoading)
    {
      Singleton<PopupManager>.GetInstance().onDismiss(true);
      Singleton<CommonRoot>.GetInstance().loadingMode = 1;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      List<int> player_unit_ids = new List<int>();
      transformConfirm.storageUnitIcons.ForEach((Action<PlayerUnit>) (ic =>
      {
        if (!ic.unit.IsNormalUnit)
          return;
        player_unit_ids.Add(ic.id);
      }));
      // ISSUE: reference to a compiler-generated method
      Future<WebAPI.Response.UnitReservesAdd> ft = WebAPIUtil.UnitReservesAdd(player_unit_ids.ToArray(), new Action<WebAPI.Response.UserError>(transformConfirm.\u003CStorageIn\u003Eb__8_1));
      IEnumerator e1 = ft.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (ft.Result != null)
      {
        PlayerUnit[] playerUnits = SMManager.Get<PlayerUnit[]>();
        Player player = SMManager.Get<Player>();
        e1 = WebAPI.UnitReservesCount((Action<WebAPI.Response.UserError>) (e =>
        {
          Singleton<CommonRoot>.GetInstance().loadingMode = 0;
          WebAPI.DefaultUserErrorCallback(e);
        })).Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        int storageCount = SMManager.Get<PlayerUnitReservesCount>().count;
        e1 = transformConfirm.menu.Init(playerUnits, storageCount, transformConfirm.menu.StorageMenuSortAndFilterInfo);
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        Future<GameObject> prefab004UnitTransformResultF = Res.Prefabs.popup.popup_004_unit_transform_result__anim_popup01.Load<GameObject>();
        e1 = prefab004UnitTransformResultF.Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        GameObject result = prefab004UnitTransformResultF.Result;
        Singleton<PopupManager>.GetInstance().openAlert(result).GetComponent<Popup004UnitTransformResult>().Init(transformConfirm.storageUnitIcons.Count, playerUnits.Length, storageCount, player);
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        ft = (Future<WebAPI.Response.UnitReservesAdd>) null;
        playerUnits = (PlayerUnit[]) null;
        player = (Player) null;
        prefab004UnitTransformResultF = (Future<GameObject>) null;
      }
    }
    else
      transformConfirm.IsPush = false;
  }

  private IEnumerator StorageOut()
  {
    Popup004UnitTransformConfirm transformConfirm = this;
    if (!Singleton<CommonRoot>.GetInstance().isLoading)
    {
      Singleton<PopupManager>.GetInstance().onDismiss(true);
      Singleton<CommonRoot>.GetInstance().loadingMode = 1;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      List<int> player_unit_ids = new List<int>();
      transformConfirm.storageUnitIcons.ForEach((Action<PlayerUnit>) (ic =>
      {
        if (!ic.unit.IsNormalUnit)
          return;
        player_unit_ids.Add(ic.id);
      }));
      // ISSUE: reference to a compiler-generated method
      Future<WebAPI.Response.UnitReservesRestore> ft = WebAPI.UnitReservesRestore(player_unit_ids.ToArray(), new Action<WebAPI.Response.UserError>(transformConfirm.\u003CStorageOut\u003Eb__9_1));
      IEnumerator e1 = ft.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (ft.Result != null)
      {
        Future<WebAPI.Response.UnitReservesIndex> reservesIndexF = WebAPI.UnitReservesIndex((Action<WebAPI.Response.UserError>) (e =>
        {
          Singleton<CommonRoot>.GetInstance().loadingMode = 0;
          WebAPI.DefaultUserErrorCallback(e);
        }));
        e1 = reservesIndexF.Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        PlayerUnit[] storageUnits = reservesIndexF.Result.player_units;
        PlayerUnit[] playerUnits = SMManager.Get<PlayerUnit[]>();
        Player player = SMManager.Get<Player>();
        e1 = WebAPI.UnitReservesCount().Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        int storageCount = SMManager.Get<PlayerUnitReservesCount>().count;
        transformConfirm.StartCoroutine(transformConfirm.menu.Init(storageUnits, storageCount, transformConfirm.menu.StorageMenuSortAndFilterInfo));
        Future<GameObject> prefab004UnitTransformResultF = Res.Prefabs.popup.popup_004_unit_transform_result__anim_popup01.Load<GameObject>();
        e1 = prefab004UnitTransformResultF.Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        GameObject result = prefab004UnitTransformResultF.Result;
        Singleton<PopupManager>.GetInstance().openAlert(result).GetComponent<Popup004UnitTransformResult>().Init(transformConfirm.storageUnitIcons.Count, playerUnits.Length, storageCount, player);
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        ft = (Future<WebAPI.Response.UnitReservesRestore>) null;
        reservesIndexF = (Future<WebAPI.Response.UnitReservesIndex>) null;
        storageUnits = (PlayerUnit[]) null;
        playerUnits = (PlayerUnit[]) null;
        player = (Player) null;
        prefab004UnitTransformResultF = (Future<GameObject>) null;
      }
    }
    else
      transformConfirm.IsPush = false;
  }

  public void IbtnYes()
  {
    switch (this.confirmMode)
    {
      case Popup004UnitTransformConfirm.StorageConfirmMode.StorageIn:
        if (this.isMemoryAlert)
        {
          ((Component) this.txtDescription).gameObject.SetActive(false);
          this.txtDescription2.SetActive(true);
          this.isMemoryAlert = false;
          break;
        }
        this.StartCoroutine(this.StorageIn());
        break;
      case Popup004UnitTransformConfirm.StorageConfirmMode.StorageOut:
        this.StartCoroutine(this.StorageOut());
        break;
    }
  }

  public IEnumerator UpdateList()
  {
    Popup004UnitTransformConfirm transformConfirm = this;
    PlayerUnit[] playerUnits = SMManager.Get<PlayerUnit[]>();
    IEnumerator e1 = WebAPI.UnitReservesCount((Action<WebAPI.Response.UserError>) (e =>
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      WebAPI.DefaultUserErrorCallback(e);
    })).Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    int count = SMManager.Get<PlayerUnitReservesCount>().count;
    transformConfirm.StartCoroutine(transformConfirm.menu.Init(playerUnits, count, transformConfirm.menu.StorageMenuSortAndFilterInfo));
  }

  public void IbtnNo() => Singleton<PopupManager>.GetInstance().onDismiss();

  public override void onBackButton() => this.IbtnNo();

  public enum StorageConfirmMode
  {
    StorageIn,
    StorageOut,
  }
}
