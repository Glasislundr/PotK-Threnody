// Decompiled with JetBrains decompiler
// Type: StoryScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class StoryScene : NGSceneBase
{
  public StoryExecuter executer;

  public static void changeScene009_3(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("story009_3", stack);
  }

  public override IEnumerator onInitSceneAsync()
  {
    if (Object.op_Inequality((Object) this.executer, (Object) null))
    {
      ((Component) this.executer).gameObject.SetActive(true);
      yield return (object) this.executer.initialize((string) null);
    }
  }

  public void onStartScene() => this.executer.render();
}
