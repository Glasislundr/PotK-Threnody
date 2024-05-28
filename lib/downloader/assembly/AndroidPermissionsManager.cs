// Decompiled with JetBrains decompiler
// Type: AndroidPermissionsManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class AndroidPermissionsManager
{
  private static AndroidJavaObject m_Activity;
  private static AndroidJavaObject m_PermissionService;

  private static AndroidJavaObject GetActivity()
  {
    if (AndroidPermissionsManager.m_Activity == null)
      AndroidPermissionsManager.m_Activity = ((AndroidJavaObject) new AndroidJavaClass("com.unity3d.player.UnityPlayer")).GetStatic<AndroidJavaObject>("currentActivity");
    return AndroidPermissionsManager.m_Activity;
  }

  private static AndroidJavaObject GetPermissionsService()
  {
    return AndroidPermissionsManager.m_PermissionService ?? (AndroidPermissionsManager.m_PermissionService = new AndroidJavaObject("com.unity3d.plugin.UnityAndroidPermissions", Array.Empty<object>()));
  }

  public static bool IsPermissionGranted(string permissionName)
  {
    return AndroidPermissionsManager.GetPermissionsService().Call<bool>(nameof (IsPermissionGranted), new object[2]
    {
      (object) AndroidPermissionsManager.GetActivity(),
      (object) permissionName
    });
  }

  public static void RequestPermission(string[] permissionNames, AndroidPermissionCallback callback)
  {
    AndroidPermissionsManager.GetPermissionsService().Call("RequestPermissionAsync", new object[3]
    {
      (object) AndroidPermissionsManager.GetActivity(),
      (object) permissionNames,
      (object) callback
    });
  }
}
