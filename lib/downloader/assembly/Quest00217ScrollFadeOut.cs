// Decompiled with JetBrains decompiler
// Type: Quest00217ScrollFadeOut
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Quest00217ScrollFadeOut : MonoBehaviour
{
  private float duration_;
  private int offsetDepth_;
  private EventDelegate funcFinished_;

  public void init(float duration, int offsetDepth, EventDelegate callbackFinished = null)
  {
    this.duration_ = duration;
    this.offsetDepth_ = offsetDepth;
    this.funcFinished_ = callbackFinished;
  }

  private void Start()
  {
    Quest00217ScrollFadeOut.setWidget(((Component) this).gameObject, this.offsetDepth_);
    ((UITweener) TweenAlpha.Begin(((Component) this).gameObject, this.duration_, 0.0f)).SetOnFinished((EventDelegate.Callback) (() =>
    {
      if (this.funcFinished_ != null)
        this.funcFinished_.Execute();
      else
        Object.Destroy((Object) ((Component) this).gameObject);
    }));
  }

  public static UIWidget setWidget(GameObject go, int offsetDepth)
  {
    if (Object.op_Inequality((Object) go.GetComponent<UIRect>(), (Object) null))
      return (UIWidget) null;
    UIWidget uiWidget1 = go.AddComponent<UIWidget>();
    UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
    bool flag = true;
    int num = 0;
    if (componentsInChildren.Length != 0)
    {
      num = componentsInChildren[0].depth;
      flag = false;
      foreach (UIWidget uiWidget2 in componentsInChildren)
      {
        if (num < uiWidget2.depth)
          num = uiWidget2.depth;
      }
    }
    if (!flag)
      uiWidget1.depth = num + offsetDepth;
    return uiWidget1;
  }
}
