// Decompiled with JetBrains decompiler
// Type: Help0155Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;

#nullable disable
public class Help0155Scene : NGSceneBase
{
  public Help0155Menu menu;

  public static void ChangeScene(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("help015_5", stack);
  }

  public override IEnumerator onInitSceneAsync()
  {
    IEnumerator e = this.menu.InitBeginnerNavi();
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
    yield break;
  }
}
