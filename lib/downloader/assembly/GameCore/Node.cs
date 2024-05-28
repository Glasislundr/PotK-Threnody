// Decompiled with JetBrains decompiler
// Type: GameCore.Node
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace GameCore
{
  public class Node : INode
  {
    public readonly string Text;
    public readonly float Value;
    public readonly bool isFloat;

    public Node(string text)
    {
      this.Text = text;
      this.isFloat = float.TryParse(text, out this.Value);
    }

    public float Eval(Func<string, float> convert)
    {
      return this.isFloat ? this.Value : convert(this.Text);
    }
  }
}
