// Decompiled with JetBrains decompiler
// Type: Gsc.App.NetworkHelper.BinaryRequest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Network;
using System;
using System.Collections;
using System.Text;
using UnityEngine;

#nullable disable
namespace Gsc.App.NetworkHelper
{
  public class BinaryRequest : Request<BinaryRequest, BinaryResponse>
  {
    private readonly WebRequest ___request;
    private readonly byte[] ___payload;
    private WebInternalResponse ___response;

    public BinaryRequest(WebRequest request)
    {
      this.___request = request;
      this.___payload = request.PostData != null ? Encoding.UTF8.GetBytes(request.PostData) : (byte[]) null;
    }

    public override string GetPath() => "/" + this.___request.Path;

    public override string GetMethod()
    {
      if (this.___request.Method == WebRequest.RequestMethod.GET)
        return "GET";
      return this.___request.Method == WebRequest.RequestMethod.POST ? "POST" : (string) null;
    }

    public override byte[] GetPayload() => this.___payload;

    public override WebTaskResult InquireResult(WebTaskResult result, WebInternalResponse response)
    {
      this.___response = response;
      return result;
    }

    public IEnumerator OnFailed()
    {
      WebResponse response = new WebResponse();
      if (this.___response != null)
      {
        response.Bytes = this.___response.Payload;
        response.Status = this.___response.StatusCode;
      }
      return BinaryRequest.OnFailed(this.___task.Result, this.___request, response, this.___request.FailCallbackOrNull);
    }

    public static IEnumerator OnFailed(WebTaskResult result)
    {
      return BinaryRequest.OnFailed(result, new WebRequest("auth", WebRequest.RequestMethod.GET, (string) null, (Action<WebResponse>) null), new WebResponse(), (Func<WebError, IEnumerator>) null);
    }

    public static IEnumerator OnFailed(
      WebTaskResult result,
      WebRequest request,
      WebResponse response,
      Func<WebError, IEnumerator> callback)
    {
      callback = callback ?? WebQueue.FailCallback;
      if (callback != null)
      {
        WebError webError;
        switch (result)
        {
          case WebTaskResult.ServerError:
            webError = WebError.ServerError5xx(request, response);
            break;
          case WebTaskResult.ExpiredSessionError:
            ModalWindow.Show("セッションエラー", "アプリを再起動して下さい", (Action) (() => Application.Quit()));
            yield break;
          case WebTaskResult.InvalidDeviceError:
            webError = WebError.ClientError4xx(request, response);
            callback = WebQueue.FailAuthTokenCallback;
            break;
          case WebTaskResult.UnknownError:
            webError = WebError.ClientError4xx(request, response);
            break;
          default:
            webError = WebError.Timeout(request);
            break;
        }
        yield return (object) callback(webError);
      }
    }
  }
}
