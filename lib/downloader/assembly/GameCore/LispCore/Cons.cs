// Decompiled with JetBrains decompiler
// Type: GameCore.LispCore.Cons
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace GameCore.LispCore
{
  [Serializable]
  public class Cons
  {
    public object car;
    public object cdr;

    public override string ToString()
    {
      string str = this.car == null ? "nil" : this.car.ToString();
      if (SExp.consp_(this.cdr))
        return "(" + str + " " + this.cdr.ToString().Substring(1);
      if (this.cdr == null)
        return "(" + str + ")";
      return "(" + str + " . " + this.cdr + ")";
    }
  }
}
