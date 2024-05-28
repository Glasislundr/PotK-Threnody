// Decompiled with JetBrains decompiler
// Type: Quest00215AScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections;

#nullable disable
public class Quest00215AScene : NGSceneBase
{
  public Quest00215AMenu menu;

  public IEnumerator onStartSceneAsync(int episode, UnitUnit unit)
  {
    IEnumerator e = this.menu.SetCharacter(episode, unit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
