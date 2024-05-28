// Decompiled with JetBrains decompiler
// Type: Colosseum0236Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Colosseum0236Scene : NGSceneBase
{
  [SerializeField]
  private Colosseum0236Menu menu;

  public static void ChangeScene(ColosseumUtility.Info info)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("colosseum023_6", false, (object) info);
  }

  public override IEnumerator onInitSceneAsync()
  {
    Colosseum0236Scene colosseum0236Scene = this;
    Future<GameObject> bgF = Res.Prefabs.BackGround.ColosseumBackground.Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    colosseum0236Scene.backgroundPrefab = bgF.Result;
  }

  public IEnumerator onStartSceneAsync()
  {
    Future<WebAPI.Response.ColosseumBoot> futureF = WebAPI.ColosseumBoot((Action<WebAPI.Response.UserError>) (e =>
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
      e1 = this.onStartSceneAsync(new ColosseumUtility.Info(false, futureF.Result));
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
    }
  }

  public IEnumerator onStartSceneAsync(ColosseumUtility.Info info)
  {
    IEnumerator e = this.menu.Initialize(info);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
