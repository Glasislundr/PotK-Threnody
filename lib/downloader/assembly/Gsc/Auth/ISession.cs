// Decompiled with JetBrains decompiler
// Type: Gsc.Auth.ISession
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Network;
using System;

#nullable disable
namespace Gsc.Auth
{
  public interface ISession
  {
    string SecretKey { get; }

    string DeviceID { get; }

    string AccessToken { get; }

    string UserAgent { get; }

    void DeleteAuthKeys();

    void AddDeviceIdCallback(Action<string> callback);

    bool CanRefreshToken(Type requestType);

    IRefreshTokenTask GetRefreshTokenTask();

    IWebTask RegisterEmailAddressAndPassword(
      string email,
      string password,
      bool disableValicationEmail,
      Action<RegisterEmailAddressAndPasswordResult> callback);

    IWebTask AddDeviceWithEmailAddressAndPassword(
      string email,
      string password,
      Action<AddDeviceWithEmailAddressAndPasswordResult> callback);
  }
}
