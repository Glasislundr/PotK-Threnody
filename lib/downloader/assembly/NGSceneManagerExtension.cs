// Decompiled with JetBrains decompiler
// Type: NGSceneManagerExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public static class NGSceneManagerExtension
{
  public static Coroutine Start(this IEnumerator e)
  {
    return Singleton<NGSceneManager>.GetInstance().StartCoroutine(e);
  }
}
