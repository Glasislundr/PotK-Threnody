// Decompiled with JetBrains decompiler
// Type: MapEditCameraController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class MapEditCameraController : BattleCameraController
{
  public bool initialized_ => Object.op_Inequality((Object) this.angle, (Object) null);

  private bool isControll_
  {
    get => ((Behaviour) this).enabled && this.initialized_ && this.battleManager.initialized;
  }

  protected override IEnumerator Start_Original()
  {
    IEnumerator e = base.Start_Original();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
