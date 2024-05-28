// Decompiled with JetBrains decompiler
// Type: BonusDisplay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BonusDisplay
{
  private Bonus[] bonus;
  private int index;
  private bool isBirthDay;
  private bool isPvp;
  private UILabel titleLabel;
  private UILabel limitLabel;

  public void Set(Bonus[] bonus, UILabel title, UILabel limit, bool isBirthDay = true, bool isPvp = false)
  {
    this.index = 0;
    this.bonus = bonus;
    this.titleLabel = title;
    this.limitLabel = limit;
    this.isBirthDay = isBirthDay;
    this.isPvp = isPvp;
    if (Object.op_Inequality((Object) this.titleLabel, (Object) null))
    {
      ((Component) this.titleLabel).gameObject.AddComponent<TweenAlpha>();
      ((Component) this.titleLabel).gameObject.AddComponent<TweenAlpha>();
      this.SetTween(((Component) this.titleLabel).gameObject, true);
    }
    if (Object.op_Inequality((Object) this.limitLabel, (Object) null))
    {
      ((Component) this.limitLabel).gameObject.AddComponent<TweenAlpha>();
      ((Component) this.limitLabel).gameObject.AddComponent<TweenAlpha>();
      this.SetTween(((Component) this.limitLabel).gameObject);
    }
    this.SetInfo();
    this.Start();
  }

  private void SetInfo()
  {
    if (this.bonus.Length == 0 || this.bonus == null)
      return;
    if (Object.op_Inequality((Object) this.titleLabel, (Object) null))
      this.titleLabel.SetTextLocalize(this.bonus[this.index].DisplayName(this.isPvp));
    if (!Object.op_Inequality((Object) this.limitLabel, (Object) null))
      return;
    this.limitLabel.SetTextLocalize(this.bonus[this.index].RemainingTime());
  }

  private void SetTween(GameObject go, bool setCallback = false)
  {
    TweenAlpha[] components = go.GetComponents<TweenAlpha>();
    this.SetStartTween(components[0], setCallback);
    this.SetEndTween(components[1], setCallback);
  }

  private void SetStartTween(TweenAlpha tween, bool setCallback)
  {
    tween.from = 0.0f;
    tween.to = 1f;
    ((UITweener) tween).duration = 1f;
    ((UITweener) tween).tweenGroup = 0;
    if (!setCallback)
      return;
    ((UITweener) tween).SetOnFinished((EventDelegate.Callback) (() => this.End()));
  }

  private void SetEndTween(TweenAlpha tween, bool setCallback)
  {
    tween.from = 1f;
    tween.to = 0.0f;
    ((UITweener) tween).delay = 7f;
    ((UITweener) tween).duration = 1f;
    ((UITweener) tween).tweenGroup = 1;
    if (!setCallback)
      return;
    ((UITweener) tween).SetOnFinished((EventDelegate.Callback) (() => this.Change()));
  }

  private void Start()
  {
    if (Object.op_Inequality((Object) this.titleLabel, (Object) null))
      ((IEnumerable<TweenAlpha>) ((Component) this.titleLabel).gameObject.GetComponents<TweenAlpha>()).ForEach<TweenAlpha>((Action<TweenAlpha>) (x =>
      {
        if (((UITweener) x).tweenGroup != 0)
          return;
        ((UITweener) x).ResetToBeginning();
        ((UITweener) x).PlayForward();
      }));
    if (!Object.op_Inequality((Object) this.limitLabel, (Object) null))
      return;
    ((IEnumerable<TweenAlpha>) ((Component) this.limitLabel).gameObject.GetComponents<TweenAlpha>()).ForEach<TweenAlpha>((Action<TweenAlpha>) (x =>
    {
      if (((UITweener) x).tweenGroup != 0)
        return;
      ((UITweener) x).ResetToBeginning();
      ((UITweener) x).PlayForward();
    }));
  }

  private void End()
  {
    if (Object.op_Inequality((Object) this.titleLabel, (Object) null))
      ((IEnumerable<TweenAlpha>) ((Component) this.titleLabel).gameObject.GetComponents<TweenAlpha>()).ForEach<TweenAlpha>((Action<TweenAlpha>) (x =>
      {
        if (((UITweener) x).tweenGroup != 1)
          return;
        ((UITweener) x).ResetToBeginning();
        ((UITweener) x).PlayForward();
      }));
    if (!Object.op_Inequality((Object) this.limitLabel, (Object) null))
      return;
    ((IEnumerable<TweenAlpha>) ((Component) this.limitLabel).gameObject.GetComponents<TweenAlpha>()).ForEach<TweenAlpha>((Action<TweenAlpha>) (x =>
    {
      if (((UITweener) x).tweenGroup != 1)
        return;
      ((UITweener) x).ResetToBeginning();
      ((UITweener) x).PlayForward();
    }));
  }

  private void Change()
  {
    this.index = this.index + 1 >= this.bonus.Length ? 0 : this.index + 1;
    if (!this.isBirthDay && this.bonus[this.index].category == 12)
      this.index = this.index + 1 >= this.bonus.Length ? 0 : this.index + 1;
    this.SetInfo();
    this.Start();
  }
}
