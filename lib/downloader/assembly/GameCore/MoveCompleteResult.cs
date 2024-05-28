// Decompiled with JetBrains decompiler
// Type: GameCore.MoveCompleteResult
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace GameCore
{
  [Serializable]
  public class MoveCompleteResult : ActionResult
  {
    public MoveCompleteResult(int r, int c)
    {
      this.row = r;
      this.column = c;
      this.isMove = true;
      this.terminate = true;
    }

    public override ActionResultNetwork ToNetworkLocal(BL env) => new ActionResultNetwork();
  }
}
