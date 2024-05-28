// Decompiled with JetBrains decompiler
// Type: Gsc.Core.Logger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using Gsc.Network;
using UnityEngine;

#nullable disable
namespace Gsc.Core
{
  public class Logger
  {
    private static bool initialized;

    public static event Application.LogCallback Callback;

    public static void Init()
    {
      if (Logger.initialized)
        return;
      Application.logMessageReceived -= new Application.LogCallback(Logger._HandleLog);
      Application.logMessageReceived += new Application.LogCallback(Logger._HandleLog);
      Logger.initialized = true;
    }

    public static void HandleLog(string logMessage, string stackTrace, LogType logType)
    {
      if (logType > 1 && logType != 4 || PerformanceConfig.GetInstance().IsSendErrorTracker && !SendErrorTracker.isSendError)
        return;
      UnityErrorLogSender.Instance.Send(logMessage, stackTrace, logType);
    }

    private static void _HandleLog(string logMessage, string stackTrace, LogType logType)
    {
      Logger.HandleLog(logMessage, stackTrace, logType);
      if (Logger.Callback == null)
        return;
      Logger.Callback(logMessage, stackTrace, logType);
    }
  }
}
