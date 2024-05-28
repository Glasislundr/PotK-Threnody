// Decompiled with JetBrains decompiler
// Type: Gsc.Purchase.API.Response.ChargeV2Limit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Network.Support.MiniJsonHelper;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gsc.Purchase.API.Response
{
  public class ChargeV2Limit : Gsc.Network.Response<ChargeV2Limit>
  {
    public int Age { get; private set; }

    public bool HasCreditLimit { get; private set; }

    public float CreditLimit { get; private set; }

    public ChargeV2Limit(byte[] payload)
    {
      Dictionary<string, object> result = Gsc.Network.Response<ChargeV2Limit>.GetResult(payload);
      this.Age = Deserializer.Instance.Add<int>(new Func<object, int>(Deserializer.ToIntegerType.int32)).Deserialize<int>(result["age"]);
      this.HasCreditLimit = this.Age < 20;
      if (!this.HasCreditLimit)
        return;
      float[] source = Deserializer.Instance.WithArray<float>().Add<float>(new Func<object, float>(Deserializer.ToNumberType.float32)).Deserialize<float[]>(result["accept_prices"]);
      this.CreditLimit = source == null || source.Length == 0 ? 0.0f : (float) (Math.Ceiling((double) ((IEnumerable<float>) source).Max() * 100.0) / 100.0);
    }
  }
}
