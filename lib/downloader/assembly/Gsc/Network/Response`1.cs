// Decompiled with JetBrains decompiler
// Type: Gsc.Network.Response`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.DOM.Json;
using Gsc.Network.Data;
using System.Collections.Generic;

#nullable disable
namespace Gsc.Network
{
  public abstract class Response<T> : IResponse where T : Response<T>
  {
    protected static Dictionary<string, object> GetResult(byte[] payload)
    {
      if (payload == null || payload.Length == 0)
        return (Dictionary<string, object>) null;
      using (Document document = Document.Parse(payload))
      {
        Object @object = document.Root.GetObject();
        Value obj1;
        if (!@object.TryGetValue("response", out obj1))
          return (Dictionary<string, object>) Gsc.DOM.Json.MiniJSON.Json.Deserialize(document.Root);
        Value obj2;
        Value obj3;
        if (@object.TryGetValue("update_models", out obj2) | @object.TryGetValue("remove_models", out obj3))
          EntityRepository.Update((Dictionary<string, object>) Gsc.DOM.Json.MiniJSON.Json.Deserialize(obj2), (Dictionary<string, object>) Gsc.DOM.Json.MiniJSON.Json.Deserialize(obj3));
        return (Dictionary<string, object>) Gsc.DOM.Json.MiniJSON.Json.Deserialize(obj1);
      }
    }
  }
}
