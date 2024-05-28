// Decompiled with JetBrains decompiler
// Type: Quest00215Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Quest00215Scene : NGSceneBase
{
  [SerializeField]
  private Quest00215Menu menu;
  private bool doneDisplay;

  public IEnumerator onStartSceneAsync()
  {
    if (!this.doneDisplay)
    {
      IEnumerator e = this.menu.Init(100111);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.menu.ScrollContainerResolvePosition();
      this.doneDisplay = true;
    }
  }

  public IEnumerator onStartSceneAsync(int id)
  {
    if (!this.doneDisplay)
    {
      IEnumerator e = this.menu.Init(id);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.menu.ScrollContainerResolvePosition();
      this.doneDisplay = true;
    }
  }
}
