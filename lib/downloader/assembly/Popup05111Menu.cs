// Decompiled with JetBrains decompiler
// Type: Popup05111Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Earth;
using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Popup05111Menu : BackButtonMenuBase
{
  public override void onBackButton() => this.IbtnNo();

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public void IbtnYes()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.DataReset());
  }

  private IEnumerator DataReset()
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Singleton<PopupManager>.GetInstance().closeAll();
    IEnumerator e = WebAPI.ZeroReset((Action<WebAPI.Response.UserError>) (error =>
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<PopupManager>.GetInstance().closeAll();
      WebAPI.DefaultUserErrorCallback(error);
    })).Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Persist.earthData.Delete();
    Persist.earthBattleEnvironment.Delete();
    MasterDataCache.SetGameMode(MasterDataCache.GameMode.EARTH);
    Singleton<EarthDataManager>.GetInstance().EarthDataReset();
    e = this.ShowDetaResetCompletePopup();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator ShowDetaResetCompletePopup()
  {
    Future<GameObject> prefabF = Res.Prefabs.popup.popup_051_12__anim_popup01.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject prefab = prefabF.Result.Clone();
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
  }
}
