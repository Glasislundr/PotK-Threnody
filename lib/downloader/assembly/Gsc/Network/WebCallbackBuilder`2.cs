﻿// Decompiled with JetBrains decompiler
// Type: Gsc.Network.WebCallbackBuilder`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Core;
using System;
using UnityEngine;

#nullable disable
namespace Gsc.Network
{
  public class WebCallbackBuilder<TRequest, TResponse>
    where TRequest : Request<TRequest, TResponse>
    where TResponse : Response<TResponse>
  {
    public static WebCallbackBuilder<TRequest, TResponse>.WebCallback Build(
      VoidCallback<TResponse> callback)
    {
      return WebCallbackBuilder<TRequest, TResponse>.WebCallback.Create(WebTaskResult.Success, callback.Target as Behaviour, (WebCallbackBuilder<TRequest, TResponse>.Callback) (task => callback(task.Response)));
    }

    public static WebCallbackBuilder<TRequest, TResponse>.WebCallback Build(
      VoidCallback<TRequest, TResponse> callback)
    {
      return WebCallbackBuilder<TRequest, TResponse>.WebCallback.Create(WebTaskResult.Success, callback.Target as Behaviour, (WebCallbackBuilder<TRequest, TResponse>.Callback) (task => callback(task.Request, task.Response)));
    }

    public static WebCallbackBuilder<TRequest, TResponse>.WebCallback Build(
      VoidCallbackWithError<TResponse> callback)
    {
      return WebCallbackBuilder<TRequest, TResponse>.WebCallback.Create(WebTaskResult.Success | WebTaskResult.MustErrorHandle, callback.Target as Behaviour, (WebCallbackBuilder<TRequest, TResponse>.Callback) (task => callback(task.Response, task.error)));
    }

    public static WebCallbackBuilder<TRequest, TResponse>.WebCallback Build(
      VoidCallbackWithError<TRequest, TResponse> callback)
    {
      return WebCallbackBuilder<TRequest, TResponse>.WebCallback.Create(WebTaskResult.Success | WebTaskResult.MustErrorHandle, callback.Target as Behaviour, (WebCallbackBuilder<TRequest, TResponse>.Callback) (task => callback(task.Request, task.Response, task.error)));
    }

    public static WebCallbackBuilder<TRequest, TResponse>.WebCallback Build(
      YieldCallback<TResponse> callback)
    {
      return WebCallbackBuilder<TRequest, TResponse>.WebCallback.Create(WebTaskResult.Success, callback.Target as Behaviour, (WebCallbackBuilder<TRequest, TResponse>.Callback) (task => ImmortalObject.Instance.StartCoroutine(callback(task.Response))));
    }

    public static WebCallbackBuilder<TRequest, TResponse>.WebCallback Build(
      YieldCallback<TRequest, TResponse> callback)
    {
      return WebCallbackBuilder<TRequest, TResponse>.WebCallback.Create(WebTaskResult.Success, callback.Target as Behaviour, (WebCallbackBuilder<TRequest, TResponse>.Callback) (task => ImmortalObject.Instance.StartCoroutine(callback(task.Request, task.Response))));
    }

    public static WebCallbackBuilder<TRequest, TResponse>.WebCallback Build(
      YieldCallbackWithError<TResponse> callback)
    {
      return WebCallbackBuilder<TRequest, TResponse>.WebCallback.Create(WebTaskResult.Success | WebTaskResult.MustErrorHandle, callback.Target as Behaviour, (WebCallbackBuilder<TRequest, TResponse>.Callback) (task => ImmortalObject.Instance.StartCoroutine(callback(task.Response, task.error))));
    }

    public static WebCallbackBuilder<TRequest, TResponse>.WebCallback Build(
      YieldCallbackWithError<TRequest, TResponse> callback)
    {
      return WebCallbackBuilder<TRequest, TResponse>.WebCallback.Create(WebTaskResult.Success | WebTaskResult.MustErrorHandle, callback.Target as Behaviour, (WebCallbackBuilder<TRequest, TResponse>.Callback) (task => ImmortalObject.Instance.StartCoroutine(callback(task.Request, task.Response, task.error))));
    }

    public static WebCallbackBuilder<TRequest, TResponse>.WebCallback Build<TResult>(
      Action<TResult> send,
      ReturnCallback<TResponse, TResult> callback)
    {
      return WebCallbackBuilder<TRequest, TResponse>.WebCallback.Create(WebTaskResult.Success, callback.Target as Behaviour, (WebCallbackBuilder<TRequest, TResponse>.Callback) (task => send(callback(task.Response))));
    }

    public static WebCallbackBuilder<TRequest, TResponse>.WebCallback Build<TResult>(
      Action<TResult> send,
      ReturnCallback<TRequest, TResponse, TResult> callback)
    {
      return WebCallbackBuilder<TRequest, TResponse>.WebCallback.Create(WebTaskResult.Success, callback.Target as Behaviour, (WebCallbackBuilder<TRequest, TResponse>.Callback) (task => send(callback(task.Request, task.Response))));
    }

    public static WebCallbackBuilder<TRequest, TResponse>.WebCallback Build<TResult>(
      Action<TResult> send,
      ReturnCallbackWithError<TResponse, TResult> callback)
    {
      return WebCallbackBuilder<TRequest, TResponse>.WebCallback.Create(WebTaskResult.Success | WebTaskResult.MustErrorHandle, callback.Target as Behaviour, (WebCallbackBuilder<TRequest, TResponse>.Callback) (task => send(callback(task.Response, task.error))));
    }

    public static WebCallbackBuilder<TRequest, TResponse>.WebCallback Build<TResult>(
      Action<TResult> send,
      ReturnCallbackWithError<TRequest, TResponse, TResult> callback)
    {
      return WebCallbackBuilder<TRequest, TResponse>.WebCallback.Create(WebTaskResult.Success | WebTaskResult.MustErrorHandle, callback.Target as Behaviour, (WebCallbackBuilder<TRequest, TResponse>.Callback) (task => send(callback(task.Request, task.Response, task.error))));
    }

    public delegate void Callback(WebTask<TRequest, TResponse> callback)
      where TRequest : Request<TRequest, TResponse>
      where TResponse : Response<TResponse>;

    public class WebCallback : IWebCallback<TRequest, TResponse>
    {
      public bool isBehaviour;
      public Behaviour behaviour;
      public WebTaskResult acceptResults;
      public WebCallbackBuilder<TRequest, TResponse>.Callback invoke;

      public static WebCallbackBuilder<TRequest, TResponse>.WebCallback Create(
        WebTaskResult acceptResults,
        Behaviour behaviour,
        WebCallbackBuilder<TRequest, TResponse>.Callback invoke)
      {
        return new WebCallbackBuilder<TRequest, TResponse>.WebCallback()
        {
          isBehaviour = behaviour != null,
          behaviour = behaviour,
          acceptResults = acceptResults,
          invoke = invoke
        };
      }

      public bool OnCallback(WebTask<TRequest, TResponse> task)
      {
        if ((task.Result & this.acceptResults) <= WebTaskResult.None)
          return false;
        if (!this.isBehaviour || Object.op_Inequality((Object) this.behaviour, (Object) null) && this.behaviour.enabled)
          this.invoke(task);
        return true;
      }
    }
  }
}
