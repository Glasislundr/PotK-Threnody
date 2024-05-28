// Decompiled with JetBrains decompiler
// Type: WWWUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class WWWUtil
{
  public static IEnumerator RequestAndCache(string url, Action<Dictionary<string, object>> callback)
  {
    Texture2D texture = new Texture2D(0, 0, (TextureFormat) 5, false);
    WWW www = new WWW(url);
    while (!www.isDone)
      yield return (object) null;
    if (string.IsNullOrEmpty(www.error))
      www.LoadImageIntoTexture(texture);
    callback(new Dictionary<string, object>()
    {
      {
        "www",
        (object) www
      },
      {
        "texture",
        (object) texture
      }
    });
  }
}
