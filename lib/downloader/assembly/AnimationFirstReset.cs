// Decompiled with JetBrains decompiler
// Type: AnimationFirstReset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
[AddComponentMenu("Utility/Tween/AnimationFirstReset")]
public class AnimationFirstReset : MonoBehaviour
{
  [SerializeField]
  [Tooltip("fromの値を取る対象を絞ります")]
  private NGTween.Kind[] tweenGroups = new NGTween.Kind[2]
  {
    NGTween.Kind.START_END,
    NGTween.Kind.START
  };
  [SerializeField]
  [Tooltip("fromの値を取る対象を絞ります")]
  private int[] tweenGroupIDs = new int[0];
  private bool reset_;
  private UITweener[] tweeners_;

  private void Awake()
  {
    this.tweeners_ = ((Component) this).GetComponents<UITweener>();
    this.reset_ = false;
  }

  private void OnEnable()
  {
    if (this.reset_)
      return;
    this.reset_ = true;
    int[] array = ((IEnumerable<NGTween.Kind>) this.tweenGroups).Select<NGTween.Kind, int>((Func<NGTween.Kind, int>) (t => (int) t)).Concat<int>((IEnumerable<int>) this.tweenGroupIDs).ToArray<int>();
    foreach (UITweener tweener in this.tweeners_)
    {
      if (((IEnumerable<int>) array).Contains<int>(tweener.tweenGroup))
      {
        System.Type type = tweener.GetType();
        if (type.Equals(typeof (TweenAlpha)))
        {
          TweenAlpha tweenAlpha = tweener as TweenAlpha;
          tweenAlpha.value = tweenAlpha.from;
        }
        else if (type.Equals(typeof (TweenPosition)))
        {
          TweenPosition tweenPosition = tweener as TweenPosition;
          tweenPosition.value = tweenPosition.from;
        }
        else if (type.Equals(typeof (TweenColor)))
        {
          TweenColor tweenColor = tweener as TweenColor;
          tweenColor.value = tweenColor.from;
        }
        else if (type.Equals(typeof (TweenScale)))
        {
          TweenScale tweenScale = tweener as TweenScale;
          tweenScale.value = tweenScale.from;
        }
        else if (type.Equals(typeof (TweenRotation)))
        {
          TweenRotation tweenRotation = tweener as TweenRotation;
          tweenRotation.value = Quaternion.Euler(tweenRotation.from);
        }
      }
    }
  }
}
