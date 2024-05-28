// Decompiled with JetBrains decompiler
// Type: Gsc.App.AccountManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Device;
using System;
using System.Collections;

#nullable disable
namespace Gsc.App
{
  public class AccountManager : IAccountManager
  {
    public string GetSecretKey(string name)
    {
      string secretKey = Persist.auth.Data.SecretKey;
      return string.IsNullOrEmpty(secretKey) ? (string) null : secretKey;
    }

    public string GetDeviceId(string name)
    {
      string deviceId = Persist.auth.Data.DeviceID;
      return string.IsNullOrEmpty(deviceId) ? (string) null : deviceId;
    }

    public void SetDeviceId(string name, string deviceId)
    {
      Persist.auth.Data.DeviceID = deviceId;
      Persist.auth.Flush();
    }

    public void SetKeyPair(string name, string secretKey, string deviceId)
    {
      Persist.auth.Data.SecretKey = secretKey;
      Persist.auth.Data.DeviceID = deviceId;
      Persist.auth.Flush();
    }

    public void Remove(string name) => Persist.auth.Data.ResetAllAuthInfo();

    public IEnumerator Salvage()
    {
      if (!Persist.auth.Exists)
      {
        bool isDone = false;
        WebAPI.AuthDeviceInfo((Action) (() =>
        {
          Persist.auth.Flush();
          isDone = true;
        }));
        while (!isDone)
          yield return (object) null;
      }
    }
  }
}
