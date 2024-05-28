// Decompiled with JetBrains decompiler
// Type: WebQueue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.App.NetworkHelper;
using Gsc.Auth;
using MiniJSON;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
public static class WebQueue
{
  public static Func<WebError, IEnumerator> FailCallback = new Func<WebError, IEnumerator>(WebQueue.defaultFailCallback);
  public static Func<WebError, IEnumerator> FailAuthTokenCallback = new Func<WebError, IEnumerator>(WebQueue.defaultFailCallback);
  public static Func<WebError, IEnumerator> SafetyFailCallback = new Func<WebError, IEnumerator>(WebQueue.defaultFailCallback);
  public static Action<object> Logger = (Action<object>) (_ => { });
  public const string rpcPath = "api/v2/";

  public static string AuthToken => Session.DefaultSession.AccessToken;

  public static bool IsAuthToken() => false;

  public static void ResetAuthToken()
  {
  }

  public static void Lock() => Gsc.Network.WebQueue.defaultQueue.Pause();

  public static void UnLock() => Gsc.Network.WebQueue.defaultQueue.Start();

  public static void ClearRequestQueue() => Gsc.Network.WebQueue.defaultQueue.Reset();

  private static void addQueue(List<WebRequest> queue, WebRequest request)
  {
    GsccBridge.Send(request, false);
  }

  public static void Add(WebRequest request) => WebQueue.addQueue((List<WebRequest>) null, request);

  public static void Get(string path, Action<WebResponse> callback)
  {
    WebQueue.Add(new WebRequest("api/v2/" + path, WebRequest.RequestMethod.GET, "", callback));
  }

  public static void Post(string path, Action<WebResponse> callback)
  {
    string path1 = !"api/v2/".EndsWith("/") || !path.StartsWith("/") ? "api/v2/" + path : "api/v2/" + path.Substring(1);
    WebQueue.Logger((object) ("Post path : " + path1));
    WebQueue.Add(new WebRequest(path1, WebRequest.RequestMethod.POST, "", callback));
  }

  public static void Post(
    string path,
    Dictionary<string, object> post,
    Action<WebResponse> callback)
  {
    WebQueue.RawPost(!"api/v2/".EndsWith("/") || !path.StartsWith("/") ? "api/v2/" + path : "api/v2/" + path.Substring(1), post, callback);
  }

  public static void RawPost(
    string path,
    Dictionary<string, object> post,
    Action<WebResponse> callback)
  {
    WebQueue.Logger((object) ("Post path : " + path));
    WebQueue.Add(new WebRequest(path, WebRequest.RequestMethod.POST, Json.Serialize((object) post), callback));
  }

  public static void SilentPost(
    string path,
    Dictionary<string, object> post,
    Action<WebResponse> callback)
  {
    WebQueue.Add(new WebRequest(!"api/v2/".EndsWith("/") || !path.StartsWith("/") ? "api/v2/" + path : "api/v2/" + path.Substring(1), WebRequest.RequestMethod.POST, Json.Serialize((object) post), callback, (Func<WebError, IEnumerator>) (_ => (IEnumerator) null))
    {
      Loading = false
    });
  }

  public static void SafetyPost(
    string path,
    Dictionary<string, object> post,
    Action<WebResponse> callback)
  {
    string path1 = !"api/v2/".EndsWith("/") || !path.StartsWith("/") ? "api/v2/" + path : "api/v2/" + path.Substring(1);
    WebQueue.Logger((object) ("Post path : " + path1));
    WebQueue.Add(new WebRequest(path1, WebRequest.RequestMethod.POST, Json.Serialize((object) post), callback, WebQueue.SafetyFailCallback));
  }

  public static int RequestQueueCount() => 0;

  public static int AuthQueueCount() => 0;

  private static void authPost(
    string path,
    Dictionary<string, string> posts,
    Action<WebResponse> successCallback,
    Func<WebError, IEnumerator> failCallback = null)
  {
    WebQueue.Logger((object) ("authPost path : " + path));
    WebQueue.addQueue((List<WebRequest>) null, new WebRequest(path, WebRequest.RequestMethod.POST, Json.Serialize((object) posts), successCallback, failCallback));
  }

  private static void authDeviceInfo(
    Dictionary<string, string> posts,
    Action<WebResponse> successCallback,
    Func<WebError, IEnumerator> failCallback = null)
  {
    string path = "auth/deviceinfo";
    WebQueue.Logger((object) ("authPost path : " + path));
    WebQueue.addQueue((List<WebRequest>) null, new WebRequest(path, WebRequest.RequestMethod.POST, Json.Serialize((object) posts), successCallback, failCallback, isAuthDeviceInfo: true));
  }

  private static void rpcSubmitEnv(
    Dictionary<string, string> posts,
    Action<WebResponse> successCallback,
    Func<WebError, IEnumerator> failCallback = null)
  {
    string path = "rpc/submit/env";
    WebQueue.Logger((object) ("authPost path : " + path));
    WebQueue.addQueue((List<WebRequest>) null, new WebRequest(path, WebRequest.RequestMethod.POST, Json.Serialize((object) posts), successCallback, failCallback, isAuthDeviceInfo: true));
  }

  public static void EnqueueAuthPasscode(
    Action<string, int> successCallback,
    Func<WebError, IEnumerator> failCallback)
  {
    WebQueue.authPost("auth/passcode", new Dictionary<string, string>()
    {
      ["device_id"] = Persist.auth.Data.DeviceID,
      ["secret_key"] = Persist.auth.Data.SecretKey
    }, (Action<WebResponse>) (r =>
    {
      object s = r.Json["expires_in"];
      int num2 = s is long num3 ? (int) num3 : int.Parse((string) s);
      successCallback((string) r.Json["passcode"], num2);
    }), (Func<WebError, IEnumerator>) (e => !e.HasResponse() ? new object[0].GetEnumerator() : failCallback(e)));
  }

  public static void EnqueueAuthMigrate(
    string passcode,
    Action successCallback,
    Func<WebError, IEnumerator> failCallback)
  {
    WebQueue.enqueueAuth((Action) (() => WebQueue.authPost("auth/migrate", new Dictionary<string, string>()
    {
      [nameof (passcode)] = passcode,
      ["secret_key"] = Persist.auth.Data.SecretKey,
      ["device_id"] = Persist.auth.Data.DeviceID
    }, (Action<WebResponse>) (r =>
    {
      Persist.auth.Data.DeviceID = (string) r.Json["old_device_id"];
      Persist.auth.Flush();
      successCallback();
    }), (Func<WebError, IEnumerator>) (e => !e.HasResponse() ? new object[0].GetEnumerator() : failCallback(e)))));
  }

  public static void EnqueueAuthFgGIDMigrate(
    string email,
    string passcode,
    Action successCallback,
    Func<WebError, IEnumerator> failCallback)
  {
    WebQueue.enqueueAuth((Action) (() => WebQueue.authPost("api/v2/fggid/device", new Dictionary<string, string>()
    {
      [nameof (email)] = email,
      ["password"] = passcode,
      ["idfv"] = Persist.auth.Data.DeviceID
    }, (Action<WebResponse>) (r =>
    {
      WebQueue.ResetAuthToken();
      Persist.auth.Data.DeviceID = (string) r.Json["device_id"];
      Persist.auth.Data.SecretKey = (string) r.Json["secret_key"];
      Persist.auth.Flush();
      successCallback();
    }), (Func<WebError, IEnumerator>) (e => !e.HasResponse() ? new object[0].GetEnumerator() : failCallback(e)))));
  }

  public static void EnqueueAuthFgGIDAddDevice(
    string email,
    string passcode,
    Action successCallback,
    Func<WebError, IEnumerator> failCallback)
  {
    WebQueue.enqueueAuth((Action) (() => WebQueue.authPost("api/v2/fggid/migrate", new Dictionary<string, string>()
    {
      [nameof (email)] = email,
      ["password"] = passcode,
      ["secret_key"] = Persist.auth.Data.SecretKey,
      ["device_id"] = Persist.auth.Data.DeviceID
    }, (Action<WebResponse>) (r =>
    {
      WebQueue.ResetAuthToken();
      Persist.auth.Data.DeviceID = (string) r.Json["old_device_id"];
      Persist.auth.Flush();
      successCallback();
    }), (Func<WebError, IEnumerator>) (e => !e.HasResponse() ? new object[0].GetEnumerator() : failCallback(e)))));
  }

  public static void EnqueueAuthDeviceInfo(
    Action successCallback,
    Func<WebError, IEnumerator> failCallback)
  {
    WebQueue.authDeviceInfo(new Dictionary<string, string>()
    {
      ["udid"] = Persist.auth.Data.UUID
    }, (Action<WebResponse>) (r =>
    {
      Gsc.Network.WebQueue.defaultQueue.Pause();
      Singleton<NGWeb>.GetInstance().ShowTranferModalWindow((Action) (() =>
      {
        Persist.auth.Data.SecretKey = (string) r.Json["secret_key"];
        Persist.auth.Data.DeviceID = (string) r.Json["device_id"];
        successCallback();
        Gsc.Network.WebQueue.defaultQueue.Start();
      }), (Action) (() =>
      {
        successCallback();
        Gsc.Network.WebQueue.defaultQueue.Start();
      }));
    }), (Func<WebError, IEnumerator>) (e => failCallback(e)));
  }

  public static void EnqueueRpcSubmitEnv(
    Action<bool> successCallback,
    Func<WebError, IEnumerator> failCallback)
  {
    WebQueue.rpcSubmitEnv(new Dictionary<string, string>()
    {
      ["application_version"] = Revision.ApplicationVersion,
      ["platform"] = "windows"
    }, (Action<WebResponse>) (r => successCallback((bool) r.Json["is_submit_env"])), (Func<WebError, IEnumerator>) (e => failCallback(e)));
  }

  private static void enqueueAuth(Action callback = null) => callback();

  private static IEnumerator defaultFailCallback(WebError error)
  {
    WebQueue.Logger((object) error.Show());
    yield break;
  }
}
