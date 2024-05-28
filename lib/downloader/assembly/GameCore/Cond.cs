// Decompiled with JetBrains decompiler
// Type: GameCore.Cond
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace GameCore
{
  public class Cond
  {
    public readonly string Operand;
    public readonly INode Left;
    public readonly INode Right;

    public Cond(string operand, INode left, INode right)
    {
      this.Operand = operand;
      this.Left = left;
      this.Right = right;
    }

    public bool Eval(Func<string, float> convert)
    {
      float num1 = this.Left.Eval(convert);
      float num2 = this.Right.Eval(convert);
      switch (this.Operand)
      {
        case "!=":
          return (double) num1 != (double) num2;
        case "<":
          return (double) num1 < (double) num2;
        case "<=":
          return (double) num1 <= (double) num2;
        case "<>":
          return (double) num1 != (double) num2;
        case "=":
          return (double) num1 == (double) num2;
        case "==":
          return (double) num1 == (double) num2;
        case ">":
          return (double) num1 > (double) num2;
        case ">=":
          return (double) num1 >= (double) num2;
        default:
          throw new FormatException(this.Operand);
      }
    }
  }
}
