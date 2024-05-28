﻿// Decompiled with JetBrains decompiler
// Type: GooglePlayGames.OurUtils.Logger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace GooglePlayGames.OurUtils
{
  public class Logger
  {
    private static bool debugLogEnabled = false;
    private static bool warningLogEnabled = true;

    public static bool DebugLogEnabled
    {
      get => Logger.debugLogEnabled;
      set => Logger.debugLogEnabled = value;
    }

    public static bool WarningLogEnabled
    {
      get => Logger.warningLogEnabled;
      set => Logger.warningLogEnabled = value;
    }

    public static void d(string msg)
    {
      if (!Logger.debugLogEnabled)
        return;
      PlayGamesHelperObject.RunOnGameThread((Action) (() => Debug.Log((object) Logger.ToLogMessage(string.Empty, "DEBUG", msg))));
    }

    public static void w(string msg)
    {
      if (!Logger.warningLogEnabled)
        return;
      PlayGamesHelperObject.RunOnGameThread((Action) (() => Debug.LogWarning((object) Logger.ToLogMessage("!!!", "WARNING", msg))));
    }

    public static void e(string msg)
    {
      if (!Logger.warningLogEnabled)
        return;
      PlayGamesHelperObject.RunOnGameThread((Action) (() => Debug.LogWarning((object) Logger.ToLogMessage("***", "ERROR", msg))));
    }

    public static string describe(byte[] b)
    {
      return b != null ? "byte[" + (object) b.Length + "]" : "(null)";
    }

    private static string ToLogMessage(string prefix, string logType, string msg)
    {
      string empty;
      try
      {
        empty = DateTime.Now.ToString("MM/dd/yy H:mm:ss zzz");
      }
      catch (Exception ex)
      {
        PlayGamesHelperObject.RunOnGameThread((Action) (() => Debug.LogWarning((object) "*** [Play Games Plugin DLL] ERROR: Failed to format DateTime.Now")));
        empty = string.Empty;
      }
      return string.Format("{0} [Play Games Plugin DLL] {1} {2}: {3}", (object) prefix, (object) empty, (object) logType, (object) msg);
    }
  }
}
