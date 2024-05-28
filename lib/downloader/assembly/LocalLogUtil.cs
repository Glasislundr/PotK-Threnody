// Decompiled with JetBrains decompiler
// Type: LocalLogUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.IO;
using System.Text;
using UnityEngine;

#nullable disable
public class LocalLogUtil
{
  private static string m_logFilePath = StorageUtil.persistentDataPath + "/error.log";

  public static void Init()
  {
    File.Delete(LocalLogUtil.m_logFilePath);
    LocalLogUtil.Append(DateTime.Now.ToString() + ": Initialize log file.\r\n");
  }

  public static void LogFatalError(string logMessage, string stackTrace, LogType logType)
  {
    if (logType > 1 && logType != 4)
      return;
    LocalLogUtil.Append("\r\n" + logMessage + "\r\n" + stackTrace);
  }

  private static void Append(string message)
  {
    byte[] bytes = Encoding.UTF8.GetBytes(message);
    using (FileStream fileStream = new FileStream(LocalLogUtil.m_logFilePath, FileMode.Append))
    {
      fileStream.Write(bytes, 0, bytes.Length);
      fileStream.Close();
    }
  }
}
