// Decompiled with JetBrains decompiler
// Type: Gsc.Network.WebInternalTask
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Tasks;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

#nullable disable
namespace Gsc.Network
{
  public abstract class WebInternalTask : IWebTaskBase, ITask, IEnumerator
  {
    public static bool EnableAutoRetry = true;
    private int retryCount;
    private bool completed;
    private UnityWebRequest webRequest;
    private readonly string method;
    private readonly string uri;
    private readonly byte[] payload;
    private readonly CustomHeaders customHeaders;
    private WebInternalTask.WaitTask waitTask;
    private object subroutine;
    private static readonly byte[] wwwZeroBuffer = Encoding.ASCII.GetBytes("{}");

    public WebTaskResult Result { get; protected set; }

    public bool isBreak { get; private set; }

    public bool isDone => this.completed || this.isBreak;

    public static WebInternalTask<TRequest, TResponse> Create<TRequest, TResponse>(
      Request<TRequest, TResponse> request)
      where TRequest : Request<TRequest, TResponse>
      where TResponse : Response<TResponse>
    {
      return new WebInternalTask<TRequest, TResponse>(request);
    }

    public WebInternalTask(string method, string uri, byte[] payload, CustomHeaders customHeaders)
    {
      this.method = method;
      this.uri = uri;
      this.payload = payload;
      this.customHeaders = customHeaders;
    }

    private void Update()
    {
      if (this.webRequest == null)
      {
        this.webRequest = WebInternalTask.CreateRequest(this.method, this.uri, this.payload, this.customHeaders);
        this.subroutine = (object) this.webRequest.Send();
      }
      else
      {
        if (!this.webRequest.isDone)
          return;
        WebInternalResponse response = new WebInternalResponse(this.webRequest);
        int statusCode = response.StatusCode;
        try
        {
          if (statusCode != 0 && 500 <= statusCode && statusCode <= 599 && 503 != statusCode && 504 != statusCode && WebInternalTask.EnableAutoRetry && ++this.retryCount < 3)
          {
            this.waitTask = new WebInternalTask.WaitTask();
          }
          else
          {
            this.Result = this.ProcessResponse(response);
            this.completed = true;
          }
        }
        finally
        {
          this.InternalDispose();
        }
      }
    }

    protected abstract WebTaskResult ProcessResponse(WebInternalResponse response);

    public void Break() => this.isBreak = true;

    public void Reset()
    {
      if (this.isBreak)
        return;
      this.retryCount = 0;
      this.completed = false;
    }

    public void OnStart() => this.Reset();

    public void OnFinish()
    {
    }

    public IEnumerator Run() => (IEnumerator) this;

    public object Current => this.subroutine;

    public bool MoveNext()
    {
      this.subroutine = (object) null;
      if (this.waitTask != null && this.waitTask.Wait())
        return true;
      this.waitTask = (WebInternalTask.WaitTask) null;
      if (!this.isDone)
        this.Update();
      if (!this.isDone)
        return true;
      this.InternalDispose();
      return false;
    }

    private void InternalDispose()
    {
      if (this.webRequest == null)
        return;
      this.webRequest.Dispose();
      this.webRequest = (UnityWebRequest) null;
    }

    private static UnityWebRequest CreateRequest(
      string method,
      string uri,
      byte[] payload,
      CustomHeaders customHeaders)
    {
      UnityWebRequest request = new UnityWebRequest(uri);
      request.method = method;
      if (method != "GET")
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(payload ?? WebInternalTask.wwwZeroBuffer);
      request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
      customHeaders.Dispatch(new Action<string, string>(request.SetRequestHeader));
      return request;
    }

    private class WaitTask
    {
      private readonly float time;

      public WaitTask() => this.time = Time.unscaledTime;

      public bool Wait() => (double) Time.unscaledTime - (double) this.time < 1.0;
    }
  }
}
