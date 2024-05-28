// Decompiled with JetBrains decompiler
// Type: FloatingDialogBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FloatingDialogBase : MonoBehaviour
{
  protected TweenAlpha tweenAlpha;
  protected TweenScale tweenScale;
  protected bool show;

  public bool IsShow => this.show;

  protected void Awake()
  {
    this.tweenAlpha = ((Component) this).gameObject.GetComponent<TweenAlpha>();
    this.tweenScale = ((Component) this).gameObject.GetComponent<TweenScale>();
  }

  private void Start() => ((Component) this).gameObject.SetActive(false);

  protected virtual void Update()
  {
    if (!Input.GetMouseButtonDown(0) || !this.show)
      return;
    this.Hide();
  }

  public virtual void Show()
  {
    ((Component) this).gameObject.SetActive(true);
    ((IEnumerable<UITweener>) ((Component) this).gameObject.GetComponentsInChildren<UITweener>()).ForEach<UITweener>((Action<UITweener>) (c =>
    {
      ((Behaviour) c).enabled = true;
      c.onFinished.Clear();
      c.PlayForward();
    }));
    this.show = true;
  }

  public virtual void Hide()
  {
    this.show = false;
    UITweener[] tweens = ((Component) this).gameObject.GetComponentsInChildren<UITweener>();
    if (tweens.Length == 0)
      return;
    int finishCount = 0;
    EventDelegate.Callback onFinish = (EventDelegate.Callback) (() =>
    {
      if (++finishCount < tweens.Length)
        return;
      ((Component) this).gameObject.SetActive(false);
    });
    ((IEnumerable<UITweener>) tweens).ForEach<UITweener>((Action<UITweener>) (c =>
    {
      c.onFinished.Clear();
      c.AddOnFinished(onFinish);
      c.PlayReverse();
    }));
  }
}
