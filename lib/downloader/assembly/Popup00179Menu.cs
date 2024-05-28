// Decompiled with JetBrains decompiler
// Type: Popup00179Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;

#nullable disable
public class Popup00179Menu : BackButtonMenuBase
{
  private Mypage0017Menu menu0017;
  private PlayerPresent deletePresent;

  public IEnumerator Init(PlayerPresent present, Mypage0017Menu menu)
  {
    this.menu0017 = menu;
    this.deletePresent = present;
    yield break;
  }

  private IEnumerator DeletePresent()
  {
    Popup00179Menu popup00179Menu = this;
    Singleton<PopupManager>.GetInstance().closeAll();
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<WebAPI.Response.PresentDelete> receive = WebAPI.PresentDelete(new int[1]
    {
      popup00179Menu.deletePresent.id
    }, (Action<WebAPI.Response.UserError>) (e =>
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e1 = receive.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (receive.Result != null)
    {
      popup00179Menu.menu0017.SaveScrollPosition();
      popup00179Menu.StartCoroutine(popup00179Menu.menu0017.UpdateList(SMManager.Get<PlayerPresent[]>()));
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    }
  }

  public void IbtnYes() => this.StartCoroutine(this.DeletePresent());

  public void IbtnNo() => Singleton<PopupManager>.GetInstance().onDismiss();

  public override void onBackButton() => this.IbtnNo();
}
