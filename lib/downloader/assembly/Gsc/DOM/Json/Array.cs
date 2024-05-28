// Decompiled with JetBrains decompiler
// Type: Gsc.DOM.Json.Array
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Gsc.DOM.Json
{
  public struct Array : IArray, IEnumerable<IValue>, IEnumerable, IEnumerable<Value>
  {
    private readonly rapidjson.Array value;

    public int Length => this.value.Length;

    public Array(rapidjson.Array value) => this.value = value;

    public IEnumerator<Value> GetEnumerator()
    {
      foreach (rapidjson.Value obj in this.value)
        yield return new Value(obj);
    }

    IEnumerator<IValue> IEnumerable<IValue>.GetEnumerator()
    {
      foreach (rapidjson.Value obj in this.value)
        yield return (IValue) new Value(obj);
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public Value this[int index] => new Value(this.value[index]);

    IValue IArray.this[int index] => (IValue) this[index];
  }
}
