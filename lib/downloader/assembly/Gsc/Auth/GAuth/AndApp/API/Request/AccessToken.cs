// Decompiled with JetBrains decompiler
// Type: Gsc.Auth.GAuth.AndApp.API.Request.AccessToken
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Network.Support.MiniJsonHelper;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gsc.Auth.GAuth.AndApp.API.Request
{
  public class AccessToken : Gsc.Network.Request<AccessToken, Gsc.Auth.GAuth.GAuth.API.Response.AccessToken>
  {
    private const string ___path = "{0}/authp-andapp/{1}/get_access_token";

    public string IDToken { get; set; }

    public AccessToken(string idToken) => this.IDToken = idToken;

    public override string GetUrl()
    {
      return string.Format("{0}/authp-andapp/{1}/get_access_token", (object) SDK.Configuration.Env.NativeBaseUrl, (object) SDK.Configuration.AppName);
    }

    public override string GetPath() => "{0}/authp-andapp/{1}/get_access_token";

    public override string GetMethod() => "POST";

    protected override Dictionary<string, object> GetParameters()
    {
      return new Dictionary<string, object>()
      {
        ["andapp_idtoken"] = Serializer.Instance.Add<string>(new Func<string, object>(Serializer.From<string>)).Serialize<string>(this.IDToken),
        ["udid"] = (object) "",
        ["idfa"] = (object) "",
        ["idfv"] = (object) ""
      };
    }
  }
}
