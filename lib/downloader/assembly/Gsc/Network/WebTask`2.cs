// Decompiled with JetBrains decompiler
// Type: Gsc.Network.WebTask`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Core;
using Gsc.DOM.Json;
using Gsc.Tasks;
using System;
using System.Collections;
using System.Reflection;
using System.Text;

#nullable disable
namespace Gsc.Network
{
  public class WebTask<TRequest, TResponse> : 
    IWebTask<TResponse>,
    IWebTask,
    IWebTaskBase,
    ITask,
    IEnumerator
    where TRequest : Gsc.Network.Request<TRequest, TResponse>
    where TResponse : Gsc.Network.Response<TResponse>
  {
    private WebInternalTask<TRequest, TResponse> internalTask;
    private WebTaskAttribute attributes;

    public IWebCallback<TRequest, TResponse> callback { get; private set; }

    public WebTaskResult acceptResults { get; private set; }

    public TRequest Request { get; private set; }

    public TResponse Response { get; private set; }

    public IErrorResponse error { get; private set; }

    public bool isBreak { get; private set; }

    public bool isDone { get; private set; }

    public WebTaskResult Result { get; private set; }

    public bool handled { get; private set; }

    public static WebTask<TRequest, TResponse> Send(TRequest request, WebTaskAttribute attributes)
    {
      WebTask<TRequest, TResponse> webTask = new WebTask<TRequest, TResponse>(attributes);
      webTask.Send((Gsc.Network.Request<TRequest, TResponse>) request);
      return webTask;
    }

    private void Send(Gsc.Network.Request<TRequest, TResponse> request)
    {
      if (request == null)
        return;
      this.Request = (TRequest) request;
      WebQueue.defaultQueue.Add((IWebTask) this);
      this.Request.OnTask(this);
    }

    private WebTask(WebTaskAttribute attributes) => this.attributes = attributes;

    public System.Type GetRequestType() => typeof (TRequest);

    public bool IsAcceptResult(WebTaskResult result)
    {
      return (this.acceptResults & result) > WebTaskResult.None;
    }

    public bool HasAttributes(WebTaskAttribute attributes)
    {
      return (this.attributes & attributes) == attributes;
    }

    public void Retry()
    {
      if (this.internalTask != null)
        return;
      this.Reset();
      this.attributes |= WebTaskAttribute.Interrupt;
      WebQueue.defaultQueue.Add((IWebTask) this);
      WebQueue.defaultQueue.Pause(false);
    }

    public void Break()
    {
      if (this.internalTask == null)
        return;
      this.internalTask.Break();
    }

    public WebTask<TRequest, TResponse> SetAcceptResults(WebTaskResult handleResults)
    {
      this.acceptResults = handleResults;
      return this;
    }

    public WebInternalTask GetInternalTask() => (WebInternalTask) this.internalTask;

    public object Current => (object) null;

    public void Reset()
    {
      this.Response = default (TResponse);
      this.error = (IErrorResponse) null;
      this.isBreak = false;
      this.isDone = false;
      this.Result = WebTaskResult.None;
      this.handled = false;
    }

    public bool MoveNext() => !this.isDone;

    public WebTask<TRequest, TResponse> OnResponse(VoidCallback<TResponse> callback)
    {
      this.callback = (IWebCallback<TRequest, TResponse>) WebCallbackBuilder<TRequest, TResponse>.Build(callback);
      return this;
    }

    public WebTask<TRequest, TResponse> OnResponse(VoidCallbackWithError<TResponse> callback)
    {
      this.callback = (IWebCallback<TRequest, TResponse>) WebCallbackBuilder<TRequest, TResponse>.Build(callback);
      return this;
    }

    public WebTask<TRequest, TResponse> OnCoroutineResponse(YieldCallback<TResponse> callback)
    {
      this.callback = (IWebCallback<TRequest, TResponse>) WebCallbackBuilder<TRequest, TResponse>.Build(callback);
      return this;
    }

    public WebTask<TRequest, TResponse> OnCoroutineResponse(
      YieldCallbackWithError<TResponse> callback)
    {
      this.callback = (IWebCallback<TRequest, TResponse>) WebCallbackBuilder<TRequest, TResponse>.Build(callback);
      return this;
    }

    public IEnumerator Run() => (IEnumerator) this.internalTask;

    public void OnStart()
    {
      this.internalTask = WebInternalTask.Create<TRequest, TResponse>((Gsc.Network.Request<TRequest, TResponse>) this.Request);
      this.internalTask.OnStart();
    }

    public void OnFinish()
    {
      this.internalTask.OnFinish();
      this.Response = this.internalTask.Response;
      this.error = this.internalTask.error;
      this.isBreak = this.internalTask.isBreak;
      this.isDone = this.internalTask.isDone;
      this.Result = this.internalTask.Result;
      this.internalTask = (WebInternalTask<TRequest, TResponse>) null;
      if (this.callback == null)
        return;
      this.handled = this.callback.OnCallback(this);
    }

    private static WebTaskResult GetTaskResult(WebInternalResponse response)
    {
      int statusCode = response.StatusCode;
      byte[] payload = response.Payload;
      if (statusCode == 0)
        return WebTaskResult.ServerError;
      if (200 <= statusCode && statusCode <= 299)
        return WebTaskResult.Success;
      if (401 == statusCode)
        return payload != null && payload.Length != 0 && Encoding.UTF8.GetString(payload).ToUpper() == "EXPIRED TOKEN" ? WebTaskResult.InternalExpiredTokenError : WebTaskResult.ExpiredSessionError;
      if (471 == statusCode)
        return WebTaskResult.UpdateApplication;
      if (472 == statusCode)
        return WebTaskResult.UpdateResource;
      if (479 == statusCode)
        return WebTaskResult.Maintenance;
      if (498 == statusCode)
        return WebTaskResult.Interrupt;
      if (499 == statusCode)
        return WebTaskResult.MustErrorHandle;
      return 500 <= statusCode && statusCode <= 599 ? WebTaskResult.ServerError : WebTaskResult.UnknownError;
    }

    public static WebTaskResult TryGetResponse(
      TRequest request,
      WebInternalResponse internalResponse,
      out TResponse response,
      out IErrorResponse error)
    {
      error = (IErrorResponse) null;
      response = default (TResponse);
      byte[] payload = internalResponse.Payload;
      WebTaskResult response1 = request.InquireResult(WebTask<TRequest, TResponse>.GetTaskResult(internalResponse), internalResponse);
      try
      {
        if (response1 == WebTaskResult.Success)
        {
          if (payload != null && payload.Length != 0 && internalResponse.ContentType == ContentType.ApplicationJson)
          {
            using (Document document = Document.Parse(payload))
            {
              if (document.Root.GetValueByPointer("/is_error", false))
              {
                document.Dispose();
                error = AssemblySupport.CreateInstance<IErrorResponse>(request.GetErrorResponseType(), (object) payload);
                return WebTaskResult.MustErrorHandle;
              }
            }
          }
          try
          {
            response = AssemblySupport.CreateInstance<TResponse>((object) payload);
          }
          catch (MissingMethodException ex)
          {
            response = AssemblySupport.CreateInstance<TResponse>((object) internalResponse);
          }
        }
        if (response1 == WebTaskResult.MustErrorHandle)
          error = AssemblySupport.CreateInstance<IErrorResponse>(request.GetErrorResponseType(), (object) payload);
      }
      catch (TargetInvocationException ex)
      {
        Debug.LogException((Exception) new WebTask<TRequest, TResponse>.ParseError(payload, ex));
        return WebTaskResult.ParseError;
      }
      return response1;
    }

    public class ParseError : Exception
    {
      public ParseError(byte[] payload, TargetInvocationException e)
        : base(string.Format("<{0}, {1}>: {2}", (object) typeof (TRequest), (object) typeof (TResponse), (object) Encoding.UTF8.GetString(payload)), (Exception) e)
      {
      }
    }
  }
}
