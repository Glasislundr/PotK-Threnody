// Decompiled with JetBrains decompiler
// Type: ShopTicketExchangeScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class ShopTicketExchangeScene : NGSceneBase
{
  [SerializeField]
  private ShopTicketExchangeMenu menu;

  public static void ChangeScene()
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("shop007_TicketExchange", true);
  }

  public IEnumerator onStartSceneAsync()
  {
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    this.menu.onStartScene();
    Future<GameObject> prefabF = Res.Prefabs.BackGround.ShopBackground.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().setBackground(prefabF.Result);
    e = this.menu.Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public override void onEndScene() => this.menu.onEndScene();
}
