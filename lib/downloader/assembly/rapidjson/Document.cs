// Decompiled with JetBrains decompiler
// Type: rapidjson.Document
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace rapidjson
{
  public class Document : IDisposable
  {
    private IntPtr ptr;
    private bool disposed;
    public readonly Value Root;

    public static Document Parse(byte[] bytes)
    {
      IntPtr document;
      if (!DLL._rapidjson_new_document_from_memory_bytes(bytes, (uint) bytes.Length, out document))
        throw new DocumentParseError();
      return new Document(document);
    }

    public static Document Parse(string text)
    {
      IntPtr document;
      if (!DLL._rapidjson_new_document_from_memory_string(text, out document))
        throw new DocumentParseError();
      return new Document(document);
    }

    public static Document ParseFromFile(string filepath)
    {
      IntPtr document;
      if (!DLL._rapidjson_new_document_from_file(filepath, out document))
        throw new DocumentParseError();
      return new Document(document);
    }

    private Document(IntPtr ptr)
    {
      this.ptr = ptr;
      this.Root = new Value(this, ref ptr);
    }

    ~Document() => this.Dispose();

    public void Dispose()
    {
      if (!(this.ptr != IntPtr.Zero))
        return;
      this.disposed = true;
      DLL._rapidjson_delete_document(out this.ptr);
    }

    public void CheckDisposed()
    {
      if (this.disposed)
        throw new AlreadyDisposedDocumentError();
    }
  }
}
