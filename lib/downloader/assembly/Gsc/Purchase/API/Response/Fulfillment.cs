// Decompiled with JetBrains decompiler
// Type: Gsc.Purchase.API.Response.Fulfillment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Network.Support.MiniJsonHelper;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gsc.Purchase.API.Response
{
  public class Fulfillment : Gsc.Network.Response<Fulfillment>
  {
    public FulfillmentResult Result { get; private set; }

    public Fulfillment(byte[] payload)
    {
      Dictionary<string, object> result = Gsc.Network.Response<Fulfillment>.GetResult(payload);
      this.Result = Deserializer.Instance.Add<FulfillmentResult>(new Func<object, FulfillmentResult>(Deserializer.ToObject<FulfillmentResult>)).Deserialize<FulfillmentResult>((object) result);
    }
  }
}
