// Decompiled with JetBrains decompiler
// Type: Gsc.App.NetworkHelper.GsccBridge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using DeviceKit;
using Gsc.DOM.MiniJSON;
using Gsc.Network;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Gsc.App.NetworkHelper
{
  public static class GsccBridge
  {
    public static readonly Dictionary<string, string> defaultHeaders = new Dictionary<string, string>();

    static GsccBridge() => GsccBridge.MakeDefaultHeaders();

    public static void Send(WebRequest request, bool interrupt)
    {
      BinaryRequest binaryRequest = new BinaryRequest(request);
      binaryRequest.CustomHeaders.AddCustomHeaders(GsccBridge.defaultHeaders);
      if (!string.IsNullOrEmpty(Revision.DLCVersion))
        binaryRequest.CustomHeaders.SetCustomHeader("X-Device-DLCVersion", Revision.DLCVersion);
      WebTaskAttribute attributes = WebTaskAttribute.Reliable | WebTaskAttribute.Parallel;
      if (interrupt)
        attributes |= WebTaskAttribute.Interrupt;
      if (!request.Loading)
        attributes |= WebTaskAttribute.Silent;
      WebTask<BinaryRequest, BinaryResponse> task = binaryRequest.GetTask(attributes);
      request.task = (IWebTask) task;
      task.OnResponse((VoidCallbackWithError<BinaryResponse>) ((response, error) =>
      {
        if (error != null)
          GsccBridge.OnError(GsccBridge.MakeResponse(error), request.SuccessCallback);
        else
          GsccBridge.OnResponse(GsccBridge.MakeResponse(response), request.SuccessCallback);
      }));
    }

    private static WebResponse MakeResponse(BinaryResponse br)
    {
      return new WebResponse()
      {
        Json = br.jsonData,
        Status = br.statusCode,
        Body = br.text
      };
    }

    private static WebResponse MakeResponse(IErrorResponse er)
    {
      return new WebResponse()
      {
        Json = (Dictionary<string, object>) Json.Deserialize(er.data.Root),
        Status = 200,
        Body = (string) null
      };
    }

    private static void OnResponse(WebResponse response, Action<WebResponse> callback)
    {
      if (response.Json.ContainsKey("response_for_the_request"))
      {
        object obj;
        if (response.Json.TryGetValue("gamekit", out obj))
          GameKit.SyncAchievements(((Dictionary<string, object>) obj)["achievements"]);
        object s;
        if (response.Json.TryGetValue("server_time", out s))
          ServerTime.SetSyncServerTime(DateTime.Parse((string) s));
        response.Json = (Dictionary<string, object>) response.Json["response_for_the_request"];
      }
      callback(response);
    }

    private static void OnError(WebResponse response, Action<WebResponse> callback)
    {
      callback(response);
    }

    public static IEnumerator OnFailed(WebTaskResult result, IWebTask task)
    {
      if (task == null)
        yield return (object) BinaryRequest.OnFailed(result);
      else if (task is WebTask<BinaryRequest, BinaryResponse> webTask)
      {
        yield return (object) webTask.Request.OnFailed();
      }
      else
      {
        WebRequest request = new WebRequest((string) null, WebRequest.RequestMethod.GET, (string) null, (Action<WebResponse>) null);
        WebResponse response = new WebResponse();
        request.task = task;
        yield return (object) BinaryRequest.OnFailed(task.Result, request, response, (Func<WebError, IEnumerator>) null);
      }
    }

    private static void MakeDefaultHeaders()
    {
      GsccBridge.defaultHeaders.Add("X-Device-API-Version", "2");
      GsccBridge.defaultHeaders.Add("X-Device-ApplicationVersion", Revision.ApplicationVersion);
      string str = DeviceKit.App.GetBundleVersion();
      if (string.IsNullOrEmpty(str))
      {
        Debug.LogWarning((object) "WebQueue.GetDefaultHeaders: BundleVersion is null. Set default BundleVersion string for debug build.");
        str = "4.0.0";
      }
      GsccBridge.defaultHeaders.Add("X-Device-Version", str);
      string languageLocale = Sys.GetLanguageLocale();
      if (string.IsNullOrEmpty(languageLocale))
        Debug.LogWarning((object) "WebQueue.GetDefaultHeaders: Language/Locale is null or empty. Skip this header.");
      else
        GsccBridge.defaultHeaders.Add("X-Device-Language-Locale", languageLocale);
    }

    public static void UnityErrorLogCallback(
      CustomHeaders customHeaders,
      Dictionary<string, object> user,
      Dictionary<string, object> tags,
      Dictionary<string, object> extra)
    {
      customHeaders.AddCustomHeaders(GsccBridge.defaultHeaders);
      Player player = SMManager.Get<Player>();
      if (player != null)
      {
        user.Add("username", (object) player.name);
        extra.Add("id", (object) player.id);
        extra.Add("short_id", (object) player.short_id);
        extra.Add("level", (object) player.level);
        extra.Add("money", (object) player.money);
        extra.Add("medal", (object) player.medal);
        extra.Add("paid_coin", (object) player.paid_coin);
        extra.Add("free_coin", (object) player.free_coin);
      }
      extra.Add("application_version", (object) Revision.ApplicationVersion);
      extra.Add("dlc_version", (object) Revision.DLCVersion);
    }
  }
}
