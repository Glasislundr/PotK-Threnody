// Decompiled with JetBrains decompiler
// Type: Purchase0162Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;

#nullable disable
public class Purchase0162Scene : NGSceneBase
{
  public Purchase0162Menu menu;

  public static void ChangeScene(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("purchase016_2", stack);
  }

  public IEnumerator onStartSceneAsync()
  {
    this.menu.InitPurchase(SMManager.Get<Player>());
    yield break;
  }
}
