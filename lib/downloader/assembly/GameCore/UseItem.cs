// Decompiled with JetBrains decompiler
// Type: GameCore.UseItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace GameCore
{
  public class UseItem : INode, IParam<int>
  {
    public readonly Node ID;
    public readonly int iID;

    public UseItem(Node id)
    {
      this.ID = (Node) null;
      if (!int.TryParse(id.Text, out this.iID))
        return;
      this.ID = id;
    }

    public float Eval(Func<string, float> convert) => this.ID == null ? 0.0f : 1f;

    public int getParam(Func<string, float> convert) => this.iID;
  }
}
