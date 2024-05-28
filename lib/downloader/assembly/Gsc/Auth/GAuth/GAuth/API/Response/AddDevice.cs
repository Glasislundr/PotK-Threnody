// Decompiled with JetBrains decompiler
// Type: Gsc.Auth.GAuth.GAuth.API.Response.AddDevice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Network.Support.MiniJsonHelper;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gsc.Auth.GAuth.GAuth.API.Response
{
  public class AddDevice : Gsc.Network.Response<AddDevice>
  {
    public bool IsSucceeded { get; private set; }

    public AddDevice(byte[] payload)
    {
      Dictionary<string, object> result = Gsc.Network.Response<AddDevice>.GetResult(payload);
      this.IsSucceeded = Deserializer.Instance.Add<bool>(new Func<object, bool>(Deserializer.To<bool>)).Deserialize<bool>(result["is_succeeded"]);
    }
  }
}
