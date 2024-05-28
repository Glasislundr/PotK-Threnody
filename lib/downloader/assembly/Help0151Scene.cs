// Decompiled with JetBrains decompiler
// Type: Help0151Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;

#nullable disable
public class Help0151Scene : NGSceneBase
{
  public Help0151Menu menu;

  public static void ChangeScene(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("help015_1", stack);
  }

  public override IEnumerator onInitSceneAsync()
  {
    IEnumerator e = this.menu.InitHelp();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Singleton<CommonRoot>.GetInstance().isLoading)
    {
      yield return (object) null;
      Singleton<CommonRoot>.GetInstance().isLoading = false;
    }
  }

  public IEnumerator onStartSceneAsync()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Help0151Scene help0151Scene = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (Singleton<NGGameDataManager>.GetInstance().IsEarth)
      help0151Scene.bgmName = "bgm104";
    return false;
  }
}
