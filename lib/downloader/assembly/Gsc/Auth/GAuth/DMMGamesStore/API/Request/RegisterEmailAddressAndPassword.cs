// Decompiled with JetBrains decompiler
// Type: Gsc.Auth.GAuth.DMMGamesStore.API.Request.RegisterEmailAddressAndPassword
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Network.Support.MiniJsonHelper;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gsc.Auth.GAuth.DMMGamesStore.API.Request
{
  public class RegisterEmailAddressAndPassword : 
    Gsc.Network.Request<RegisterEmailAddressAndPassword, RegisterEmailAddressAndPassword.Response>
  {
    private const string ___path = "{0}/dmm-auth-proxy/{1}/register";

    public int ViewerID { get; set; }

    public string OnetimeToken { get; set; }

    public string EmailAddress { get; set; }

    public string Password { get; set; }

    public bool DisableValidationEmail { get; set; }

    public RegisterEmailAddressAndPassword(
      int viewerId,
      string onetimeToken,
      string emailAddress,
      string password)
    {
      this.ViewerID = viewerId;
      this.OnetimeToken = onetimeToken;
      this.EmailAddress = emailAddress;
      this.Password = password;
    }

    public override string GetUrl()
    {
      return string.Format("{0}/dmm-auth-proxy/{1}/register", (object) SDK.Configuration.Env.NativeBaseUrl, (object) SDK.Configuration.AppName);
    }

    public override string GetPath() => "{0}/dmm-auth-proxy/{1}/register";

    public override string GetMethod() => "POST";

    protected override Dictionary<string, object> GetParameters()
    {
      return new Dictionary<string, object>()
      {
        ["dmm_viewer_id"] = Serializer.Instance.Add<int>(new Func<int, object>(Serializer.From<int>)).Serialize<int>(this.ViewerID),
        ["dmm_onetime_token"] = Serializer.Instance.Add<string>(new Func<string, object>(Serializer.From<string>)).Serialize<string>(this.OnetimeToken),
        ["email"] = Serializer.Instance.Add<string>(new Func<string, object>(Serializer.From<string>)).Serialize<string>(this.EmailAddress),
        ["password"] = Serializer.Instance.Add<string>(new Func<string, object>(Serializer.From<string>)).Serialize<string>(this.Password),
        ["disable_validation_email"] = Serializer.Instance.Add<bool>(new Func<bool, object>(Serializer.From<bool>)).Serialize<bool>(this.DisableValidationEmail)
      };
    }

    public override Type GetErrorResponseType() => typeof (Gsc.Auth.GAuth.GAuth.API.Response.ErrorResponse);

    public class Response : Gsc.Network.Response<RegisterEmailAddressAndPassword.Response>
    {
      public bool IsSucceeded { get; private set; }

      public Response(byte[] payload)
      {
        Dictionary<string, object> result = Gsc.Network.Response<RegisterEmailAddressAndPassword.Response>.GetResult(payload);
        this.IsSucceeded = Deserializer.Instance.Add<bool>(new Func<object, bool>(Deserializer.To<bool>)).Deserialize<bool>(result["is_succeeded"]);
      }
    }
  }
}
