// Decompiled with JetBrains decompiler
// Type: Gsc.DOM.Json.MiniJSON.Json
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
namespace Gsc.DOM.Json.MiniJSON
{
  public static class Json
  {
    public static object Deserialize(string text)
    {
      using (Document document = Document.Parse(text))
        return Gsc.DOM.MiniJSON.Json.Deserialize((IValue) document.Root);
    }

    public static object Deserialize(byte[] bytes)
    {
      using (Document document = Document.Parse(bytes))
        return Gsc.DOM.MiniJSON.Json.Deserialize((IValue) document.Root);
    }

    public static object Deserialize(Value value) => Gsc.DOM.MiniJSON.Json.Deserialize((IValue) value);
  }
}
