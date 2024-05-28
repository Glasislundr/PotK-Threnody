// Decompiled with JetBrains decompiler
// Type: Gsc.Network.CustomHeaders
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Auth;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gsc.Network
{
  public class CustomHeaders
  {
    private readonly string requestId;
    private readonly Dictionary<string, string> headers = new Dictionary<string, string>();
    private readonly List<Dictionary<string, string>> headersList = new List<Dictionary<string, string>>();

    public CustomHeaders(string requestId) => this.requestId = requestId;

    public void SetCustomHeader(string key, string value) => this.headers.Add(key, value);

    public void AddCustomHeaders(Dictionary<string, string> headers)
    {
      this.headersList.Add(headers);
    }

    public void Dispatch(Action<string, string> setter)
    {
      if (!SDK.Initialized)
        return;
      setter("Content-Type", "application/json; charset=utf-8");
      if (Session.DefaultSession.AccessToken != null)
        setter("Authorization", "gauth " + Session.DefaultSession.AccessToken);
      setter("User-Agent", Session.DefaultSession.UserAgent);
      setter("X-GUMI-CLIENT", "gscc ver.0.1");
      setter("X-GUMI-DEVICE-OS", "windows");
      setter("X-GUMI-TRANSACTION", this.requestId);
      setter("X-GUMI-STORE-PLATFORM", Device.Platform);
      if (SDK.Configuration.EnvName != null)
        setter("X-Gumi-Game-Environment", SDK.Configuration.EnvName);
      setter("X-GUMI-REQUEST-ID", this.requestId);
      for (int index = 0; index < this.headersList.Count; ++index)
        this.Dispatch(setter, this.headersList[index]);
      this.Dispatch(setter, this.headers);
    }

    private void Dispatch(Action<string, string> setter, Dictionary<string, string> headers)
    {
      foreach (KeyValuePair<string, string> header in headers)
        setter(header.Key, header.Value);
    }
  }
}
