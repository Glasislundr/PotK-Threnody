﻿// Decompiled with JetBrains decompiler
// Type: Gsc.Purchase.ListenerSupport
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Gsc.Purchase
{
  public static class ListenerSupport
  {
    private static bool IsAliveMethod(bool enabledInactiveCallback, Delegate method)
    {
      return ListenerSupport.IsAliveObject<object>(enabledInactiveCallback, method.Target);
    }

    public static bool IsAliveObject<T>(bool enabledInactiveCallback, T obj)
    {
      if ((object) obj == null)
        return false;
      if (!((object) obj is MonoBehaviour))
        return true;
      MonoBehaviour monoBehaviour = (object) obj as MonoBehaviour;
      if (Object.op_Equality((Object) monoBehaviour, (Object) null) || !((Behaviour) monoBehaviour).enabled)
        return false;
      return enabledInactiveCallback || ((Component) monoBehaviour).gameObject.activeInHierarchy;
    }

    public static bool Call(bool enabledInactiveCallback, Action method)
    {
      if (!ListenerSupport.IsAliveMethod(enabledInactiveCallback, (Delegate) method))
        return false;
      method();
      return true;
    }

    public static bool Call<T1>(bool enabledInactiveCallback, Action<T1> method, T1 arg1)
    {
      if (!ListenerSupport.IsAliveMethod(enabledInactiveCallback, (Delegate) method))
        return false;
      method(arg1);
      return true;
    }

    public static bool Call<T1, T2>(
      bool enabledInactiveCallback,
      Action<T1, T2> method,
      T1 arg1,
      T2 arg2)
    {
      if (!ListenerSupport.IsAliveMethod(enabledInactiveCallback, (Delegate) method))
        return false;
      method(arg1, arg2);
      return true;
    }

    public static bool CallResult(
      bool enabledInactiveCallback,
      IPurchaseResultListener listener,
      ResultCode resultCode,
      FulfillmentResult result)
    {
      switch (resultCode)
      {
        case ResultCode.Succeeded:
          return ListenerSupport.Call<FulfillmentResult>(enabledInactiveCallback, new Action<FulfillmentResult>(listener.OnPurchaseSucceeded), result);
        case ResultCode.Canceled:
          return ListenerSupport.Call(enabledInactiveCallback, new Action(listener.OnPurchaseCanceled));
        case ResultCode.AlreadyOwned:
          return ListenerSupport.Call(enabledInactiveCallback, new Action(listener.OnPurchaseAlreadyOwned));
        case ResultCode.Deferred:
          return ListenerSupport.Call(enabledInactiveCallback, new Action(listener.OnPurchaseDeferred));
        case ResultCode.Pending:
          return ListenerSupport.Call(enabledInactiveCallback, new Action(listener.OnPurchasePending));
        case ResultCode.PendingExists:
          return ListenerSupport.Call(enabledInactiveCallback, new Action(listener.OnPurchasePendingExists));
        case ResultCode.OverCreditLimit:
          return ListenerSupport.Call(enabledInactiveCallback, new Action(listener.OnOverCreditLimited));
        case ResultCode.InsufficientBalances:
          return ListenerSupport.Call(enabledInactiveCallback, new Action(listener.OnInsufficientBalances));
        default:
          return ListenerSupport.Call(enabledInactiveCallback, new Action(listener.OnPurchaseFailed));
      }
    }
  }
}
