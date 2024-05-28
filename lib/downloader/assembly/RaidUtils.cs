// Decompiled with JetBrains decompiler
// Type: RaidUtils
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class RaidUtils
{
  public static GameObject CreateTouchObject(EventDelegate.Callback callback, Transform parent)
  {
    Resolution currentResolution = Screen.currentResolution;
    GameObject touchObject = new GameObject("touch object");
    touchObject.transform.parent = parent;
    UIWidget uiWidget = touchObject.AddComponent<UIWidget>();
    uiWidget.depth = 10000;
    uiWidget.width = ((Resolution) ref currentResolution).height;
    uiWidget.height = ((Resolution) ref currentResolution).width;
    BoxCollider boxCollider = touchObject.AddComponent<BoxCollider>();
    ((Collider) boxCollider).isTrigger = true;
    boxCollider.size = new Vector3()
    {
      x = (float) ((Resolution) ref currentResolution).height,
      y = (float) ((Resolution) ref currentResolution).width,
      z = 1f
    };
    UIButton uiButton = touchObject.AddComponent<UIButton>();
    ((UIButtonColor) uiButton).tweenTarget = (GameObject) null;
    EventDelegate.Add(uiButton.onClick, callback);
    return touchObject;
  }
}
