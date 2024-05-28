// Decompiled with JetBrains decompiler
// Type: AndroidPermissionCallback
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class AndroidPermissionCallback : AndroidJavaProxy
{
  private event Action<string> OnPermissionGrantedAction;

  private event Action<string> OnPermissionDeniedAction;

  public AndroidPermissionCallback(
    Action<string> onGrantedCallback,
    Action<string> onDeniedCallback)
    : base("com.unity3d.plugin.UnityAndroidPermissions$IPermissionRequestResult")
  {
    if (onGrantedCallback != null)
      this.OnPermissionGrantedAction += onGrantedCallback;
    if (onDeniedCallback == null)
      return;
    this.OnPermissionDeniedAction += onDeniedCallback;
  }

  public virtual void OnPermissionGranted(string permissionName)
  {
    if (this.OnPermissionGrantedAction == null)
      return;
    this.OnPermissionGrantedAction(permissionName);
  }

  public virtual void OnPermissionDenied(string permissionName)
  {
    if (this.OnPermissionDeniedAction == null)
      return;
    this.OnPermissionDeniedAction(permissionName);
  }
}
