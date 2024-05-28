// Decompiled with JetBrains decompiler
// Type: Popup05112Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Earth;
using System;
using System.Collections;

#nullable disable
public class Popup05112Menu : BackButtonMenuBase
{
  public override void onBackButton() => this.IbtnOk();

  public virtual void IbtnOk()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.ChangeEarthPrologue());
  }

  private IEnumerator ChangeEarthPrologue()
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Singleton<PopupManager>.GetInstance().closeAll();
    IEnumerator e = Singleton<EarthDataManager>.GetInstance().EarthDataInit((Action<WebAPI.Response.UserError>) (error =>
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<PopupManager>.GetInstance().closeAll();
    }));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().GetEarthHeaderComponent().Reset();
    Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitID = -1;
    Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitIndex = -1;
    Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
    Singleton<NGSceneManager>.GetInstance().clearStack();
    Prologue0501Scene.ChangeScene(false);
  }
}
