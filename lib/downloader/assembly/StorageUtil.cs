// Decompiled with JetBrains decompiler
// Type: StorageUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.IO;
using UnityEngine;

#nullable disable
public static class StorageUtil
{
  private static readonly string m_persistentDataPath = Application.dataPath + "/../\\Data";
  private static readonly string m_temporaryCachePath = Application.dataPath + "/../\\Cache";

  static StorageUtil()
  {
    if (!Directory.Exists(StorageUtil.m_persistentDataPath))
      Directory.CreateDirectory(StorageUtil.m_persistentDataPath);
    if (Directory.Exists(StorageUtil.m_temporaryCachePath))
      return;
    Directory.CreateDirectory(StorageUtil.m_temporaryCachePath);
  }

  public static string persistentDataPath => StorageUtil.m_persistentDataPath;

  public static string temporaryCachePath => StorageUtil.m_temporaryCachePath;
}
