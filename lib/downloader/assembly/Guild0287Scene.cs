// Decompiled with JetBrains decompiler
// Type: Guild0287Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Guild0287Scene : NGSceneBase
{
  [SerializeField]
  private Guild0287Menu menu;
  private bool isFaildSync;

  public static void ChangeScene(bool stack = true)
  {
    if (!Persist.guildBankSetting.Exists)
    {
      Persist.guildBankSetting.Data.reset();
      Persist.guildBankSetting.Flush();
    }
    if (Persist.guildBankSetting.Exists && Persist.guildBankSetting.Data.guildBankFirstTime)
    {
      Persist.guildBankSetting.Data.guildBankFirstTime = false;
      Persist.guildBankSetting.Flush();
      Guild02871Scene.ChangeScene(false);
    }
    else
      Singleton<NGSceneManager>.GetInstance().changeScene("guild028_7", stack);
  }

  public IEnumerator onStartSceneAsync()
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    this.isFaildSync = false;
    Future<WebAPI.Response.GuildBankContact> bankContact = WebAPI.GuildBankContact();
    IEnumerator e = bankContact.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (bankContact.Result == null)
    {
      this.isFaildSync = true;
      MypageScene.ChangeSceneOnError();
    }
    else
    {
      e = this.menu.InitializeAsync(bankContact.Result.bank);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public void onStartScene()
  {
    if (this.isFaildSync)
      return;
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  public override IEnumerator onEndSceneAsync()
  {
    Guild0287Scene guild0287Scene = this;
    float startTime = Time.time;
    while (!guild0287Scene.isTweenFinished && (double) Time.time - (double) startTime < (double) guild0287Scene.tweenTimeoutTime)
      yield return (object) null;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    guild0287Scene.isTweenFinished = true;
    yield return (object) null;
    // ISSUE: reference to a compiler-generated method
    yield return (object) guild0287Scene.\u003C\u003En__0();
  }
}
