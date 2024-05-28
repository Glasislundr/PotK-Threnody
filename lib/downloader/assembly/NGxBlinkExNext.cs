// Decompiled with JetBrains decompiler
// Type: NGxBlinkExNext
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class NGxBlinkExNext : NGxBlink
{
  private float timer;
  private bool timing;
  private bool nextElement;
  private bool skipTimer;
  private GameObject[] children;

  public void SetChildren(GameObject[] child) => this.children = child;

  public void ClearChildren()
  {
    if (this.children != null)
    {
      foreach (GameObject child in this.children)
      {
        TweenAlpha component = child.GetComponent<TweenAlpha>();
        if (Object.op_Implicit((Object) component))
          Object.DestroyImmediate((Object) component);
        ((UIRect) child.GetComponent<UIWidget>()).alpha = 1f;
      }
    }
    this.children = (GameObject[]) null;
  }

  protected override NGxBlink.Pair[] GetPairs()
  {
    return this.children == null || this.children.Length == 0 ? (NGxBlink.Pair[]) null : ((IEnumerable<GameObject>) this.children).Select<GameObject, NGxBlink.Pair>((Func<GameObject, int, NGxBlink.Pair>) ((t, n) => new NGxBlink.Pair()
    {
      first = t.gameObject,
      second = this.children[(n + 1) % this.children.Length].gameObject
    })).ToArray<NGxBlink.Pair>();
  }

  protected override IEnumerator RunBlink()
  {
    NGxBlinkExNext ngxBlinkExNext = this;
label_1:
    while (true)
    {
      NGxBlink.Pair[] pairs = ngxBlinkExNext.GetPairs();
      if (pairs != null)
      {
        foreach (NGxBlink.Pair pair in pairs)
        {
          ((UIRect) pair.first.GetComponent<UIWidget>()).alpha = 0.0f;
          ((UIRect) pair.second.GetComponent<UIWidget>()).alpha = 0.0f;
        }
        if (pairs != null)
        {
          if (pairs.Length != 1)
          {
            NGxBlink.Pair[] pairArray = pairs;
            for (int index = 0; index < pairArray.Length; ++index)
            {
              NGxBlink.Pair pair = pairArray[index];
              GameObject first = pair.first;
              GameObject second = pair.second;
              TweenAlpha currentTween = first.GetOrAddComponent<TweenAlpha>();
              TweenAlpha nextTween = second.GetOrAddComponent<TweenAlpha>();
              currentTween.from = 1f;
              currentTween.to = 1f;
              ((UITweener) currentTween).ResetToBeginning();
              nextTween.from = 0.0f;
              nextTween.to = 0.0f;
              ((UITweener) nextTween).ResetToBeginning();
              ngxBlinkExNext.StartTimer();
              while (!ngxBlinkExNext.nextElement && !ngxBlinkExNext.skipTimer)
                yield return (object) null;
              ngxBlinkExNext.StopTimer();
              currentTween.from = 1f;
              currentTween.to = 0.0f;
              nextTween.from = 0.0f;
              nextTween.to = 1f;
              ((UITweener) currentTween).animationCurve = ((UITweener) ngxBlinkExNext.animationApply).animationCurve;
              ((UITweener) nextTween).animationCurve = ((UITweener) ngxBlinkExNext.animationApply).animationCurve;
              ((UITweener) currentTween).duration = ngxBlinkExNext.durationTime;
              ((UITweener) nextTween).duration = ngxBlinkExNext.durationTime;
              ((UITweener) currentTween).PlayForward();
              ((UITweener) nextTween).PlayForward();
              while (((Behaviour) nextTween).enabled)
                yield return (object) null;
              currentTween = (TweenAlpha) null;
              nextTween = (TweenAlpha) null;
            }
            pairArray = (NGxBlink.Pair[]) null;
          }
          else
            goto label_10;
        }
        else
          goto label_7;
      }
      else
        break;
    }
    yield break;
label_7:
    while (ngxBlinkExNext.children.Length == 0)
      yield return (object) null;
    goto label_1;
label_10:
    while (ngxBlinkExNext.children.Length == 1)
      yield return (object) null;
    goto label_1;
  }

  private void Update()
  {
    if (!this.timing)
      return;
    this.timer += Time.deltaTime;
    if ((double) this.timer <= (double) this.waitTime)
      return;
    this.nextElement = true;
  }

  private void StartTimer()
  {
    this.nextElement = false;
    this.timer = 0.0f;
    this.timing = true;
  }

  private void StopTimer()
  {
    this.nextElement = false;
    this.timing = false;
    this.skipTimer = false;
  }

  public void BlinkToNextElement() => this.skipTimer = true;
}
