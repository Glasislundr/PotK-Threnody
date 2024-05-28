// Decompiled with JetBrains decompiler
// Type: Gsc.Auth.GAuth.GAuth.API.Response.Register
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Network.Support.MiniJsonHelper;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gsc.Auth.GAuth.GAuth.API.Response
{
  public class Register : Gsc.Network.Response<Register>
  {
    public string DeviceId { get; private set; }

    public Register(byte[] payload)
    {
      Dictionary<string, object> result = Gsc.Network.Response<Register>.GetResult(payload);
      this.DeviceId = Deserializer.Instance.Add<string>(new Func<object, string>(Deserializer.To<string>)).Deserialize<string>(result["device_id"]);
    }
  }
}
