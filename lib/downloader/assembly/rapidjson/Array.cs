// Decompiled with JetBrains decompiler
// Type: rapidjson.Array
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace rapidjson
{
  public class Array : IEnumerable<Value>, IEnumerable
  {
    private readonly Document doc;
    private readonly ulong begin;
    private readonly uint elementSize;
    private readonly uint size;

    public int Length => (int) this.size;

    public Array(Document doc, ref IntPtr ptr)
    {
      doc.CheckDisposed();
      IntPtr elementsPointer;
      if (!DLL._rapidjson_get_array_iterator(ptr, out elementsPointer, out this.size, out this.elementSize))
        throw new InvalidOperationException("Not Array Type.");
      this.doc = doc;
      this.begin = (ulong) (long) elementsPointer;
    }

    public IEnumerator<Value> GetEnumerator()
    {
      ulong end = this.begin + (ulong) (this.size * this.elementSize);
      for (ulong itr = this.begin; (long) itr != (long) end; itr += (ulong) this.elementSize)
      {
        IntPtr ptr = (IntPtr) (long) itr;
        yield return new Value(this.doc, ref ptr);
      }
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public Value this[int index]
    {
      get
      {
        if (index < 0 || (uint) index >= this.size)
          throw new IndexOutOfRangeException();
        IntPtr ptr = (IntPtr) ((long) this.begin + (long) (this.elementSize * (uint) index));
        return new Value(this.doc, ref ptr);
      }
    }
  }
}
