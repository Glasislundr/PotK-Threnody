// Decompiled with JetBrains decompiler
// Type: Debug
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public static class Debug
{
  public static readonly bool isDebugBuild;

  public static void Log(object o)
  {
  }

  public static void LogFormat(Object context, string format, params object[] args)
  {
  }

  public static void LogFormat(string format, params object[] args)
  {
  }

  public static void LogWarning(params object[] o)
  {
  }

  public static void LogWarningFormat(string format, params object[] args)
  {
  }

  public static void LogWarningFormat(Object context, string format, params object[] args)
  {
  }

  public static void DrawLine(params object[] o)
  {
  }

  public static void LogError(object o) => Debug.LogError(o);

  public static void LogError(object o, Object context) => Debug.LogError(o, context);

  public static void LogErrorFormat(string format, params object[] args)
  {
    Debug.LogErrorFormat(format, args);
  }

  public static void LogErrorFormat(Object context, string format, params object[] args)
  {
    Debug.LogErrorFormat(context, format, args);
  }

  public static void LogException(Exception ex) => Debug.LogException(ex);

  public static void LogException(Exception ex, Object context) => Debug.LogException(ex, context);
}
