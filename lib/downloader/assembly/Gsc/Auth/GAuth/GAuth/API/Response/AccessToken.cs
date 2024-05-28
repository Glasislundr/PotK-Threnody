// Decompiled with JetBrains decompiler
// Type: Gsc.Auth.GAuth.GAuth.API.Response.AccessToken
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Network.Support.MiniJsonHelper;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gsc.Auth.GAuth.GAuth.API.Response
{
  public class AccessToken : Gsc.Network.Response<AccessToken>
  {
    public string Token { get; private set; }

    public int ExpiresIn { get; private set; }

    public AccessToken(byte[] payload)
    {
      Dictionary<string, object> result = Gsc.Network.Response<AccessToken>.GetResult(payload);
      this.Token = Deserializer.Instance.Add<string>(new Func<object, string>(Deserializer.To<string>)).Deserialize<string>(result["access_token"]);
      this.ExpiresIn = Deserializer.Instance.Add<int>(new Func<object, int>(Deserializer.ToIntegerType.int32)).Deserialize<int>(result["expires_in"]);
    }
  }
}
