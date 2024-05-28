// Decompiled with JetBrains decompiler
// Type: Gsc.Auth.GAuth.GAuth.API.Response.RegisterEmailAddressAndPassword
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Network.Support.MiniJsonHelper;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gsc.Auth.GAuth.GAuth.API.Response
{
  public class RegisterEmailAddressAndPassword : Gsc.Network.Response<RegisterEmailAddressAndPassword>
  {
    public bool IsSuccess { get; private set; }

    public RegisterEmailAddressAndPassword(byte[] payload)
    {
      Dictionary<string, object> result = Gsc.Network.Response<RegisterEmailAddressAndPassword>.GetResult(payload);
      this.IsSuccess = Deserializer.Instance.Add<bool>(new Func<object, bool>(Deserializer.To<bool>)).Deserialize<bool>(result["is_success"]);
    }
  }
}
