// Decompiled with JetBrains decompiler
// Type: Story0592Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Story0592Scene : NGSceneBase
{
  private Story0592Menu menu;
  private bool isInit;

  public static void ChangeScene(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("story059_2", stack);
  }

  public override IEnumerator onInitSceneAsync()
  {
    Story0592Scene story0592Scene = this;
    story0592Scene.menu = story0592Scene.menuBase as Story0592Menu;
    if (!Object.op_Equality((Object) story0592Scene.menu, (Object) null))
    {
      story0592Scene.isInit = false;
      IEnumerator e = story0592Scene.menu.InitSceneAsync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public IEnumerator onStartSceneAsync()
  {
    if (!this.isInit && !Object.op_Equality((Object) this.menu, (Object) null))
    {
      IEnumerator e = this.menu.StartSceneAsync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.isInit = true;
    }
  }
}
