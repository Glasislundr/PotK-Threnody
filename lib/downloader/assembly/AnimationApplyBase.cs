// Decompiled with JetBrains decompiler
// Type: AnimationApplyBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (UIRect))]
[RequireComponent(typeof (NGTweenParts))]
public class AnimationApplyBase : MonoBehaviour
{
  private bool isInit;
  private Dictionary<int, Vector3> originalTweenPositionFromDict = new Dictionary<int, Vector3>();
  private Dictionary<int, Vector3> originalTweenPositionToDict = new Dictionary<int, Vector3>();
  public string AnimationName;

  private void Awake() => this.SetTweens();

  public static void SetTweens(GameObject go)
  {
    AnimationApplyBase component = go.GetComponent<AnimationApplyBase>();
    if (!Object.op_Implicit((Object) component))
      return;
    component.SetTweens();
  }

  public void SetTweens()
  {
    if (this.isInit)
      return;
    this.isInit = true;
    string resourcePathByName = AnimationApplyBase.GetResourcePathByName(this.AnimationName);
    GameObject from = Resources.Load(resourcePathByName.Replace(".prefab", ""), typeof (GameObject)) as GameObject;
    if (Object.op_Implicit((Object) from))
    {
      GameObject gameObject = ((Component) this).gameObject;
      List<UITweener> uiTweenerList = AnimationApplyBase.copyUITweeners(from, gameObject);
      foreach (UITweener uiTweener in uiTweenerList)
      {
        if (uiTweener.GetType().Equals(typeof (TweenPosition)))
        {
          TweenPosition tweenPosition = uiTweener as TweenPosition;
          int instanceId = ((Object) tweenPosition).GetInstanceID();
          this.originalTweenPositionFromDict[instanceId] = tweenPosition.from;
          this.originalTweenPositionToDict[instanceId] = tweenPosition.to;
        }
      }
      this.resetTweenParameters(uiTweenerList.ToArray(), gameObject);
      NGTweenParts component1 = gameObject.GetComponent<NGTweenParts>();
      if (Object.op_Inequality((Object) component1, (Object) null))
      {
        NGTween.setOnTweenFinished(uiTweenerList.ToArray(), (MonoBehaviour) component1);
      }
      else
      {
        NGTweenParts component2 = from.GetComponent<NGTweenParts>();
        if (!Object.op_Inequality((Object) component2, (Object) null))
          return;
        NGTweenParts receiver = gameObject.AddComponent<NGTweenParts>();
        receiver.includeTweenChildren = component2.includeTweenChildren;
        receiver.autoSetOnTweenFinished = component2.autoSetOnTweenFinished;
        receiver.findTweensTimeOutFrame = component2.findTweensTimeOutFrame;
        receiver.timeOutTime = component2.timeOutTime;
        NGTween.setOnTweenFinished(uiTweenerList.ToArray(), (MonoBehaviour) receiver);
      }
    }
    else
      Debug.LogWarning((object) ("animation not found. name = " + resourcePathByName));
  }

  public static string GetResourceDir() => "AnimationApply/";

  public static string GetResourcePathByName(string name)
  {
    return AnimationApplyBase.GetResourceDir() + name + ".prefab";
  }

  private static List<UITweener> copyUITweeners(GameObject from, GameObject to)
  {
    UITweener[] components = from.GetComponents<UITweener>();
    bool flag1 = false;
    bool flag2 = false;
    foreach (UITweener uiTweener in components)
    {
      if (uiTweener is TweenColor)
      {
        flag1 = true;
        break;
      }
      if (!flag2 && uiTweener is TweenAlpha)
        flag2 = true;
    }
    if (flag1 | flag2 && (flag1 ? (!Object.op_Implicit((Object) to.GetComponent<UIWidget>()) ? 1 : 0) : (!Object.op_Implicit((Object) to.GetComponent<UIRect>()) ? 1 : 0)) != 0)
    {
      UIWidget uiWidget = to.AddComponent<UIWidget>();
      uiWidget.width = 2;
      uiWidget.height = 2;
    }
    List<UITweener> uiTweenerList = new List<UITweener>();
    foreach (UITweener uiTweener1 in components)
    {
      System.Type type = uiTweener1.GetType();
      UITweener uiTweener2 = to.AddComponent(type) as UITweener;
      foreach (FieldInfo field in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
      {
        if (!(field.Name == "onFinished"))
          field.SetValue((object) uiTweener2, field.GetValue((object) uiTweener1));
      }
      ((Behaviour) uiTweener2).enabled = false;
      uiTweenerList.Add(uiTweener2);
    }
    return uiTweenerList;
  }

  public static void CopyUITweenerComponent(GameObject from, GameObject to)
  {
    foreach (UITweener copyUiTweener in AnimationApplyBase.copyUITweeners(from, to))
    {
      if (copyUiTweener.GetType().Equals(typeof (TweenPosition)))
        AnimationApplyBase.adjustCopy(copyUiTweener as TweenPosition);
    }
  }

  public void Reset()
  {
    this.resetTweenParameters(((Component) this).gameObject.GetComponents<UITweener>(), ((Component) this).gameObject);
  }

  private void resetTweenParameters(UITweener[] tweens, GameObject go)
  {
    foreach (UITweener tween in tweens)
    {
      if (tween.GetType().Equals(typeof (TweenPosition)))
        this.adjustPaste(tween as TweenPosition, go);
    }
  }

  private static void adjustCopy(TweenPosition tween)
  {
    tween.from = Vector3.op_Subtraction(tween.from, tween.to);
    tween.to = Vector3.zero;
  }

  private void adjustPaste(TweenPosition tween, GameObject go)
  {
    Vector3 localPosition = go.transform.localPosition;
    Vector3 zero1 = Vector3.zero;
    Vector3 zero2 = Vector3.zero;
    this.originalTweenPositionFromDict.TryGetValue(((Object) tween).GetInstanceID(), out zero1);
    this.originalTweenPositionToDict.TryGetValue(((Object) tween).GetInstanceID(), out zero2);
    tween.from = Vector3.op_Addition(zero1, localPosition);
    tween.to = Vector3.op_Addition(zero2, localPosition);
  }
}
