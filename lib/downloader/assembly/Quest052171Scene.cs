// Decompiled with JetBrains decompiler
// Type: Quest052171Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;

#nullable disable
public class Quest052171Scene : NGSceneBase
{
  public Quest052171Menu menu;

  public static void ChangeScene(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest052_17_1", stack);
  }

  public IEnumerator onStartSceneAsync()
  {
    IEnumerator e = this.menu.Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
