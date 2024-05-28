// Decompiled with JetBrains decompiler
// Type: NGTween
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class NGTween
{
  public static bool isTweensError;

  public static bool playTweens(UITweener[] tweens, NGTween.Kind group, bool reverse = false)
  {
    NGTween.isTweensError = false;
    bool flag = false;
    foreach (UITweener tween in tweens)
    {
      if ((NGTween.Kind) tween.tweenGroup == group)
      {
        if (!NGTween.playTween(tween, reverse))
          NGTween.isTweensError = true;
        flag = true;
      }
    }
    return flag;
  }

  public static bool playTweens(UITweener[] tweens, int group, bool reverse = false)
  {
    NGTween.isTweensError = false;
    bool flag = false;
    foreach (UITweener tween in tweens)
    {
      if (tween.tweenGroup == group)
      {
        if (!NGTween.playTween(tween, reverse))
          NGTween.isTweensError = true;
        flag = true;
      }
    }
    return flag;
  }

  public static bool playTween(UITweener tween, bool reverse = false)
  {
    try
    {
      if (tween.direction == -1 != reverse)
        tween.Toggle();
      tween.ResetToBeginning();
      tween.Play(!reverse);
    }
    catch (Exception ex)
    {
      return false;
    }
    return true;
  }

  public static bool playTweensNoRestart(UITweener[] tweens, NGTween.Kind group, bool reverse = false)
  {
    NGTween.isTweensError = false;
    bool flag = false;
    foreach (UITweener tween in tweens)
    {
      if ((NGTween.Kind) tween.tweenGroup == group)
      {
        if (!NGTween.playTweenNoRestart(tween, reverse))
          NGTween.isTweensError = true;
        flag = true;
      }
    }
    return flag;
  }

  public static bool playTweenNoRestart(UITweener tween, bool reverse = false)
  {
    if (!Object.op_Implicit((Object) tween))
      return false;
    if (tween.direction == -1 != reverse)
      tween.Toggle();
    tween.Play(!reverse);
    return true;
  }

  public static UITweener[] findTweeners(GameObject o, bool includeChildren)
  {
    UITweener[] uiTweenerArray = includeChildren ? o.GetComponentsInChildren<UITweener>(true) : o.GetComponents<UITweener>();
    List<UITweener> uiTweenerList = new List<UITweener>();
    foreach (UITweener uiTweener in uiTweenerArray)
    {
      if (uiTweener.tweenGroup == 11 || uiTweener.tweenGroup == 12 || uiTweener.tweenGroup == 13)
        uiTweenerList.Add(uiTweener);
    }
    return uiTweenerList.ToArray();
  }

  public static UITweener[] findTweenersAll(GameObject o, bool includeChildren)
  {
    return !includeChildren ? o.GetComponents<UITweener>() : o.GetComponentsInChildren<UITweener>(true);
  }

  public static UITweener[] findTweenersGroup(GameObject o, int group, bool includeChildren)
  {
    UITweener[] uiTweenerArray = includeChildren ? o.GetComponentsInChildren<UITweener>(true) : o.GetComponents<UITweener>();
    List<UITweener> uiTweenerList = new List<UITweener>();
    foreach (UITweener uiTweener in uiTweenerArray)
    {
      if (uiTweener.tweenGroup == group)
        uiTweenerList.Add(uiTweener);
    }
    return uiTweenerList.ToArray();
  }

  public static void setOnTweenFinished(
    UITweener[] tweens,
    MonoBehaviour receiver,
    string function = "onTweenFinished")
  {
    UITweener uiTweener1 = (UITweener) null;
    UITweener uiTweener2 = (UITweener) null;
    EventDelegate eventDelegate = new EventDelegate(receiver, function);
    foreach (UITweener tween in tweens)
    {
      switch (tween.tweenGroup)
      {
        case 11:
          if (Object.op_Equality((Object) uiTweener1, (Object) null) || (double) tween.duration > (double) uiTweener1.duration)
          {
            uiTweener1 = tween;
            break;
          }
          if (Object.op_Equality((Object) uiTweener2, (Object) null) || (double) tween.duration > (double) uiTweener2.duration)
          {
            uiTweener2 = tween;
            break;
          }
          break;
        case 12:
          if (Object.op_Equality((Object) uiTweener1, (Object) null) || (double) tween.duration > (double) uiTweener1.duration)
          {
            uiTweener1 = tween;
            break;
          }
          break;
        case 13:
          if (Object.op_Equality((Object) uiTweener2, (Object) null) || (double) tween.duration > (double) uiTweener2.duration)
          {
            uiTweener2 = tween;
            break;
          }
          break;
        default:
          if (Object.op_Equality((Object) uiTweener1, (Object) null) || (double) tween.duration > (double) uiTweener1.duration)
          {
            uiTweener1 = tween;
            break;
          }
          if (Object.op_Equality((Object) uiTweener2, (Object) null) || (double) tween.duration > (double) uiTweener2.duration)
          {
            uiTweener2 = tween;
            break;
          }
          break;
      }
      if (Object.op_Inequality((Object) uiTweener1, (Object) null))
        uiTweener1.AddOnFinished(eventDelegate);
      else if (Object.op_Inequality((Object) uiTweener2, (Object) null) && Object.op_Inequality((Object) uiTweener2, (Object) uiTweener1))
        uiTweener2.AddOnFinished(eventDelegate);
      uiTweener1 = uiTweener2 = (UITweener) null;
    }
  }

  public static void setOnTweenFinishedIncludeDelay(
    UITweener[] tweens,
    MonoBehaviour receiver,
    string function = "onTweenFinished")
  {
    UITweener uiTweener = (UITweener) null;
    EventDelegate eventDelegate = new EventDelegate(receiver, function);
    foreach (UITweener tween in tweens)
    {
      if (tween.style != 1 && (Object.op_Equality((Object) uiTweener, (Object) null) || (double) tween.delay + (double) tween.duration > (double) uiTweener.delay + (double) uiTweener.duration))
        uiTweener = tween;
    }
    uiTweener.SetOnFinished(eventDelegate);
  }

  public enum Kind
  {
    START_END = 11, // 0x0000000B
    START = 12, // 0x0000000C
    END = 13, // 0x0000000D
    GRAYOUT = 30, // 0x0000001E
  }
}
