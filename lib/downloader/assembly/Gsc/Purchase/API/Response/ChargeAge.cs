// Decompiled with JetBrains decompiler
// Type: Gsc.Purchase.API.Response.ChargeAge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Network.Support.MiniJsonHelper;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gsc.Purchase.API.Response
{
  public class ChargeAge : Gsc.Network.Response<ChargeAge>
  {
    public int Age { get; private set; }

    public ChargeAge(byte[] payload)
    {
      Dictionary<string, object> result = Gsc.Network.Response<ChargeAge>.GetResult(payload);
      this.Age = Deserializer.Instance.Add<int>(new Func<object, int>(Deserializer.ToIntegerType.int32)).Deserialize<int>(result["age"]);
    }
  }
}
