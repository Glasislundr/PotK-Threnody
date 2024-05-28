// Decompiled with JetBrains decompiler
// Type: SpriteCash
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
[Serializable]
public class SpriteCash
{
  public Sprite sprite;
  public Future<Sprite> fSprite;
  public int id;
  public bool isLoading;

  public IEnumerator LoadSprite()
  {
    this.sprite = (Sprite) null;
    this.isLoading = true;
    IEnumerator e = this.fSprite.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.sprite = this.fSprite.Result;
    this.isLoading = false;
  }
}
