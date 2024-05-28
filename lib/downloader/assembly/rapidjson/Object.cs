// Decompiled with JetBrains decompiler
// Type: rapidjson.Object
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace rapidjson
{
  public class Object : IEnumerable<KeyValuePair<string, Value>>, IEnumerable
  {
    private IntPtr root;
    private readonly Document doc;
    private readonly uint size;

    public int MemberCount => (int) this.size;

    public Object(Document doc, ref IntPtr ptr)
    {
      doc.CheckDisposed();
      if (!DLL._rapidjson_get_object_member_count(ptr, out this.size))
        throw new InvalidOperationException("Not Object Type.");
      this.doc = doc;
      this.root = ptr;
    }

    public IEnumerator<KeyValuePair<string, Value>> GetEnumerator()
    {
      for (uint i = 0; i < this.size; ++i)
      {
        this.doc.CheckDisposed();
        string key;
        IntPtr ptr;
        DLL._rapidjson_get_key_value_pair_by_object_index(this.root, i, out key, out ptr);
        yield return new KeyValuePair<string, Value>(key, new Value(this.doc, ref ptr));
      }
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public Value this[string name]
    {
      get
      {
        Value obj;
        if (!this.TryGetValue(name, out obj))
          throw new KeyNotFoundException();
        return obj;
      }
    }

    public bool TryGetValue(string name, out Value value)
    {
      this.doc.CheckDisposed();
      IntPtr dst = IntPtr.Zero;
      bool flag = DLL.TryGet(ref this.root, name, out dst);
      value = new Value(flag ? this.doc : (Document) null, ref dst);
      return flag;
    }

    public bool HasMember(string name)
    {
      this.doc.CheckDisposed();
      IntPtr dst = IntPtr.Zero;
      return DLL.TryGet(ref this.root, name, out dst);
    }
  }
}
