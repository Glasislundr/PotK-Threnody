// Decompiled with JetBrains decompiler
// Type: SingletonExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class SingletonExtension
{
  public static void SingletonDestory(this GameObject self)
  {
    foreach (SingletonBase componentsInChild in self.GetComponentsInChildren<SingletonBase>(true))
      componentsInChild.forceDestroy();
    Object.Destroy((Object) self);
  }
}
