// Decompiled with JetBrains decompiler
// Type: Versus0261Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Versus0261Scene : NGSceneBase
{
  [SerializeField]
  private Versus0261Menu menu;

  public static void ChangeScene0261(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("versus026_1", stack);
  }

  public override IEnumerator onInitSceneAsync()
  {
    Versus0261Scene versus0261Scene = this;
    Future<GameObject> bgF = Res.Prefabs.BackGround.MultiBackground.Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    versus0261Scene.backgroundPrefab = bgF.Result;
  }

  public IEnumerator onStartSceneAsync()
  {
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<WebAPI.Response.PvpBoot> futureF = WebAPI.PvpBoot((Action<WebAPI.Response.UserError>) (e =>
    {
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e1 = futureF.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (futureF.Result != null)
    {
      WebAPI.Response.PvpBoot result = futureF.Result;
      Singleton<NGGameDataManager>.GetInstance().IsOpenPvpCampaign = result.pvp_campaigns.Length != 0;
      e1 = this.menu.Initialize(result);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      PVPManager.createPVPManager();
    }
  }

  public override void onSceneInitialized()
  {
    if (this.menu.needTitleBack)
    {
      StartScript.Restart();
    }
    else
    {
      base.onSceneInitialized();
      this.StartCoroutine(this.procLoading());
    }
  }

  private IEnumerator procLoading()
  {
    yield return (object) new WaitForEndOfFrame();
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
  }
}
