// Decompiled with JetBrains decompiler
// Type: Popup0017aMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public class Popup0017aMenu : BackButtonMenuBase
{
  private Mypage0017Menu menu0017;
  private PlayerPresent[] deletePresentIds;

  public IEnumerator Init(PlayerPresent[] present, Mypage0017Menu menu)
  {
    this.menu0017 = menu;
    this.deletePresentIds = present;
    yield break;
  }

  private IEnumerator DeletePresent()
  {
    Popup0017aMenu popup0017aMenu = this;
    Singleton<PopupManager>.GetInstance().closeAll();
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<WebAPI.Response.PresentDelete> receive = WebAPI.PresentDelete(((IEnumerable<PlayerPresent>) popup0017aMenu.deletePresentIds).Select<PlayerPresent, int>((Func<PlayerPresent, int>) (x => x.id)).ToArray<int>(), (Action<WebAPI.Response.UserError>) (e =>
    {
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e1 = receive.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (receive.Result != null)
    {
      e1 = OnDemandDownload.WaitLoadHasUnitResource(false);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      popup0017aMenu.menu0017.SaveScrollPosition();
      popup0017aMenu.StartCoroutine(popup0017aMenu.menu0017.UpdateList(SMManager.Get<PlayerPresent[]>()));
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    }
  }

  public void IbtnYes() => this.StartCoroutine(this.DeletePresent());

  public void IbtnNo() => Singleton<PopupManager>.GetInstance().onDismiss();

  public override void onBackButton() => this.IbtnNo();
}
