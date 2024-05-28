// Decompiled with JetBrains decompiler
// Type: LispParser
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore.LispCore;

#nullable disable
public class LispParser : IScriptParser
{
  private SExpReader reader;

  public LispParser(SExpReader r) => this.reader = r;

  public Cons parse(string text) => this.reader.parse(text, true) as Cons;
}
