// Decompiled with JetBrains decompiler
// Type: Mypage0017Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;

#nullable disable
public class Mypage0017Scene : NGSceneBase
{
  public Mypage0017Menu menu;

  public static void changeScene(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("mypage001_7", stack);
  }

  public IEnumerator onStartSceneAsync()
  {
    IEnumerator e = this.menu.Init(SMManager.Get<PlayerPresent[]>());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
