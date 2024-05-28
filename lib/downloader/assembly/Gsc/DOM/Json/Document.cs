// Decompiled with JetBrains decompiler
// Type: Gsc.DOM.Json.Document
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace Gsc.DOM.Json
{
  public class Document : IDocument, IDisposable
  {
    private readonly rapidjson.Document document;
    private readonly Value root;

    public static Document Parse(byte[] bytes) => new Document(rapidjson.Document.Parse(bytes));

    public static Document Parse(string text) => new Document(rapidjson.Document.Parse(text));

    public static Document ParseFromFile(string filepath)
    {
      return new Document(rapidjson.Document.ParseFromFile(filepath));
    }

    public Value Root => this.root;

    IValue IDocument.Root => (IValue) this.root;

    public Document(Document document, ref Value root)
    {
      this.document = document.document;
      this.root = root;
    }

    private Document(rapidjson.Document document)
    {
      this.document = document;
      this.root = new Value(document.Root);
    }

    ~Document() => this.Dispose();

    public void Dispose() => this.document.Dispose();
  }
}
