// Decompiled with JetBrains decompiler
// Type: Bugu0551Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;

#nullable disable
public class Bugu0551Menu : BackButtonMenuBase
{
  public IEnumerator InitSceneAsync()
  {
    yield break;
  }

  public IEnumerator StartSceneAsync()
  {
    yield break;
  }

  public override void onBackButton()
  {
    Singleton<NGSceneManager>.GetInstance().clearStack();
    Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
    Singleton<NGSceneManager>.GetInstance().changeScene("mypage051", false);
  }

  public void onListClick() => Bugu0552Scene.ChangeScene(true);

  public void onSaleClick() => Bugu055SellScene.ChangeScene(true);
}
