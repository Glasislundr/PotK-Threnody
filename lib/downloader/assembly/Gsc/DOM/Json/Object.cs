// Decompiled with JetBrains decompiler
// Type: Gsc.DOM.Json.Object
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Gsc.DOM.Json
{
  public struct Object : IObject, IEnumerable<IMember>, IEnumerable, IEnumerable<Member>
  {
    private readonly rapidjson.Object value;

    public int MemberCount => this.value.MemberCount;

    public Object(rapidjson.Object value) => this.value = value;

    public bool HasMember(string name) => this.value.HasMember(name);

    public bool TryGetValue(string name, out Value value)
    {
      rapidjson.Value obj;
      int num = this.value.TryGetValue(name, out obj) ? 1 : 0;
      value = new Value(obj);
      return num != 0;
    }

    bool IObject.TryGetValue(string name, out IValue value)
    {
      rapidjson.Value obj;
      int num = this.value.TryGetValue(name, out obj) ? 1 : 0;
      value = (IValue) new Value(obj);
      return num != 0;
    }

    public IEnumerator<Member> GetEnumerator()
    {
      foreach (KeyValuePair<string, rapidjson.Value> keyValuePair in this.value)
        yield return new Member(keyValuePair.Key, new Value(keyValuePair.Value));
    }

    IEnumerator<IMember> IEnumerable<IMember>.GetEnumerator()
    {
      foreach (KeyValuePair<string, rapidjson.Value> keyValuePair in this.value)
        yield return (IMember) new Member(keyValuePair.Key, new Value(keyValuePair.Value));
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public Value this[string name] => new Value(this.value[name]);

    IValue IObject.this[string name] => (IValue) this[name];
  }
}
