// Decompiled with JetBrains decompiler
// Type: Mypage00113Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
public class Mypage00113Scene : NGSceneBase
{
  public Mypage00113Menu menu;

  public static void changeScene(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("mypage001_13", stack);
  }

  public void onStartScene() => this.menu.Initialize();
}
