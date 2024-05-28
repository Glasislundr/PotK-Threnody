// Decompiled with JetBrains decompiler
// Type: Gsc.DOM.FastJSON.Json
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace Gsc.DOM.FastJSON
{
  public static class Json
  {
    public static object Deserialize(IValue node)
    {
      if (node.IsObject())
      {
        IObject @object = node.GetObject();
        Dictionary<string, object> dictionary = new Dictionary<string, object>(@object.MemberCount);
        foreach (IMember member in (IEnumerable<IMember>) @object)
          dictionary.Add(member.Name, Json.Deserialize(member.Value));
        return (object) dictionary;
      }
      if (node.IsArray())
      {
        IArray array = node.GetArray();
        List<object> objectList = new List<object>(array.Length);
        foreach (IValue node1 in (IEnumerable<IValue>) array)
          objectList.Add(Json.Deserialize(node1));
        return (object) objectList;
      }
      if (node.IsString())
        return (object) node.ToString();
      if (node.IsLong())
        return (object) node.ToLong();
      if (node.IsDouble())
        return (object) node.ToDouble();
      return node.IsBool() ? (object) node.ToBool() : (object) null;
    }
  }
}
