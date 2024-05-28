// Decompiled with JetBrains decompiler
// Type: Shop00720Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Shop00720Scene : NGSceneBase
{
  [SerializeField]
  private Shop00720Menu Menu;

  public IEnumerator onStartSceneAsync()
  {
    IEnumerator e1 = this.Menu.Initialize();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (DateTime.Now > Singleton<NGGameDataManager>.GetInstance().lastSlotTime.AddMinutes(10.0) || DateTime.Now.Hour != Singleton<NGGameDataManager>.GetInstance().lastSlotTime.Hour)
    {
      Future<WebAPI.Response.Slot> future = WebAPI.Slot((Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      }));
      e1 = future.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (future.Result != null)
      {
        Singleton<NGGameDataManager>.GetInstance().lastSlotTime = DateTime.Now;
        future = (Future<WebAPI.Response.Slot>) null;
      }
    }
  }

  public void onStartScene() => this.Menu.Ready();

  public override void onEndScene() => this.Menu.OnEndScene();
}
