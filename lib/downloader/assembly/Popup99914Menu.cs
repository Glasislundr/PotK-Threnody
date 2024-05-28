// Decompiled with JetBrains decompiler
// Type: Popup99914Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Popup99914Menu : NGMenuBase
{
  [SerializeField]
  private UI2DSprite Emblem;
  protected Action onCallback;

  public void SetCallback(Action callback) => this.onCallback = callback;

  public IEnumerator Init(PlayerEmblem emblem)
  {
    Future<Sprite> sprF = EmblemUtility.LoadEmblemSprite(emblem.emblem_id);
    IEnumerator e = sprF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.Emblem.sprite2D = sprF.Result;
  }

  public virtual void IbtnOk()
  {
    if (this.IsPushAndSet())
      return;
    if (this.onCallback != null)
      this.onCallback();
    Singleton<PopupManager>.GetInstance().onDismiss();
  }
}
