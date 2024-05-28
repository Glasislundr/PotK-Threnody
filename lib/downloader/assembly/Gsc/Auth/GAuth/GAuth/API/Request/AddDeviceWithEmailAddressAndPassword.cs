// Decompiled with JetBrains decompiler
// Type: Gsc.Auth.GAuth.GAuth.API.Request.AddDeviceWithEmailAddressAndPassword
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Network;
using Gsc.Network.Support.MiniJsonHelper;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gsc.Auth.GAuth.GAuth.API.Request
{
  public class AddDeviceWithEmailAddressAndPassword : 
    Gsc.Network.Request<AddDeviceWithEmailAddressAndPassword, Gsc.Auth.GAuth.GAuth.API.Response.AddDeviceWithEmailAddressAndPassword>
  {
    private const string ___path = "/email/device";

    public string EmailAddress { get; set; }

    public string Password { get; set; }

    public string Idfv { get; set; }

    public AddDeviceWithEmailAddressAndPassword(string emailAddress, string password)
    {
      this.EmailAddress = emailAddress;
      this.Password = password;
    }

    public override string GetPath() => SDK.Configuration.Env.AuthApiPrefix + "/email/device";

    public override string GetMethod() => "POST";

    protected override Dictionary<string, object> GetParameters()
    {
      return new Dictionary<string, object>()
      {
        ["email"] = Serializer.Instance.Add<string>(new Func<string, object>(Serializer.From<string>)).Serialize<string>(this.EmailAddress),
        ["password"] = Serializer.Instance.Add<string>(new Func<string, object>(Serializer.From<string>)).Serialize<string>(this.Password),
        ["idfv"] = Serializer.Instance.Add<string>(new Func<string, object>(Serializer.From<string>)).Serialize<string>(this.Idfv)
      };
    }

    public override Type GetErrorResponseType() => typeof (Gsc.Auth.GAuth.GAuth.API.Response.ErrorResponse);

    public override WebTaskResult InquireResult(WebTaskResult result, WebInternalResponse response)
    {
      return response.StatusCode == 400 && response.Payload != null && response.Payload.Length != 0 && response.ContentType == ContentType.ApplicationJson ? WebTaskResult.MustErrorHandle : result;
    }
  }
}
