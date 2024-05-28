// Decompiled with JetBrains decompiler
// Type: Gsc.Auth.GAuth.GAuth.API.Request.Register
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Network.Support.MiniJsonHelper;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gsc.Auth.GAuth.GAuth.API.Request
{
  public class Register : Gsc.Network.Request<Register, Gsc.Auth.GAuth.GAuth.API.Response.Register>
  {
    private const string ___path = "/register";

    public string Udid { get; set; }

    public string SecretKey { get; set; }

    public string Idfv { get; set; }

    public string Idfa { get; set; }

    public Register(string secretKey) => this.SecretKey = secretKey;

    public override string GetPath() => SDK.Configuration.Env.AuthApiPrefix + "/register";

    public override string GetMethod() => "POST";

    protected override Dictionary<string, object> GetParameters()
    {
      return new Dictionary<string, object>()
      {
        ["udid"] = Serializer.Instance.Add<string>(new Func<string, object>(Serializer.From<string>)).Serialize<string>(this.Udid),
        ["secret_key"] = Serializer.Instance.Add<string>(new Func<string, object>(Serializer.From<string>)).Serialize<string>(this.SecretKey),
        ["idfv"] = Serializer.Instance.Add<string>(new Func<string, object>(Serializer.From<string>)).Serialize<string>(this.Idfv),
        ["idfa"] = Serializer.Instance.Add<string>(new Func<string, object>(Serializer.From<string>)).Serialize<string>(this.Idfa)
      };
    }
  }
}
