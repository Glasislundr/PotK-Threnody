// Decompiled with JetBrains decompiler
// Type: Gsc.DOM.Json.Member
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
namespace Gsc.DOM.Json
{
  public struct Member : IMember
  {
    private readonly string name;
    private readonly Value value;

    public string Name => this.name;

    public Value Value => this.value;

    IValue IMember.Value => (IValue) this.value;

    public Member(string name, Value value)
    {
      this.name = name;
      this.value = value;
    }
  }
}
