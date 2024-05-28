// Decompiled with JetBrains decompiler
// Type: Help0154Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;

#nullable disable
public class Help0154Scene : NGSceneBase
{
  public Help0154Menu menu;

  public static void ChangeScene(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("help015_4", stack);
  }

  public IEnumerator onStartSceneAsync()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Help0154Scene help0154Scene = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (Singleton<NGGameDataManager>.GetInstance().IsEarth)
      help0154Scene.bgmName = "bgm104";
    return false;
  }

  public void onStartScene()
  {
    this.menu.InitContact(SMManager.Get<Player>());
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }
}
