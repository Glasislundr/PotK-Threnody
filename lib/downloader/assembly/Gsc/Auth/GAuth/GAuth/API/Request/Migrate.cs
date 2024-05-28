// Decompiled with JetBrains decompiler
// Type: Gsc.Auth.GAuth.GAuth.API.Request.Migrate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Network.Support.MiniJsonHelper;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gsc.Auth.GAuth.GAuth.API.Request
{
  public class Migrate : Gsc.Network.Request<Migrate, Gsc.Auth.GAuth.GAuth.API.Response.Migrate>
  {
    private const string ___path = "/migrate";

    public string Passcode { get; set; }

    public string SecretKey { get; set; }

    public string DeviceId { get; set; }

    public Migrate(string passcode, string secretKey, string deviceId)
    {
      this.Passcode = passcode;
      this.SecretKey = secretKey;
      this.DeviceId = deviceId;
    }

    public override string GetPath() => SDK.Configuration.Env.AuthApiPrefix + "/migrate";

    public override string GetMethod() => "POST";

    protected override Dictionary<string, object> GetParameters()
    {
      return new Dictionary<string, object>()
      {
        ["passcode"] = Serializer.Instance.Add<string>(new Func<string, object>(Serializer.From<string>)).Serialize<string>(this.Passcode),
        ["secret_key"] = Serializer.Instance.Add<string>(new Func<string, object>(Serializer.From<string>)).Serialize<string>(this.SecretKey),
        ["device_id"] = Serializer.Instance.Add<string>(new Func<string, object>(Serializer.From<string>)).Serialize<string>(this.DeviceId)
      };
    }
  }
}
