// Decompiled with JetBrains decompiler
// Type: AnchorAdjustmentController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class AnchorAdjustmentController : MonoBehaviour
{
  public const float maxRetryTime = 2f;
  public float currentRetryTime;
  public string adjustedObjName;
  public string targetObjName;

  public static void AdjustAnchor(
    UIWidget adjustedObj,
    string targetObjName,
    string rootSearchName = null)
  {
    GameObject gameObject = new GameObject();
    ((Object) gameObject).name = string.Format("AnchorAdjustmentController({0}-{1})", (object) ((Object) ((Component) adjustedObj).gameObject).name, (object) targetObjName);
    AnchorAdjustmentController adjustmentController = gameObject.AddComponent<AnchorAdjustmentController>();
    adjustmentController.adjustedObjName = ((Object) adjustedObj).name;
    adjustmentController.targetObjName = targetObjName;
    adjustmentController.StartCoroutine(adjustmentController.AdjustAnchorCoroutine(adjustedObj, targetObjName, rootSearchName));
  }

  public static void AdjustAnchorByObjects(
    UIWidget adjustedObj,
    string[] objNames,
    string rootSearchName = null)
  {
    GameObject gameObject = new GameObject();
    ((Object) gameObject).name = string.Format("AnchorAdjustmentController({0}-{1})", (object) ((Object) ((Component) adjustedObj).gameObject).name, (object) rootSearchName);
    AnchorAdjustmentController adjustmentController = gameObject.AddComponent<AnchorAdjustmentController>();
    adjustmentController.adjustedObjName = ((Object) adjustedObj).name;
    adjustmentController.ProcAdjustAnchorByObjects(adjustedObj, objNames, rootSearchName);
  }

  private void ProcAdjustAnchorByObjects(
    UIWidget adjustedObj,
    string[] objNames,
    string rootSearchName = null)
  {
    Transform transform1 = (Transform) null;
    Transform transform2 = (Transform) null;
    Transform transform3 = (Transform) null;
    Transform transform4 = (Transform) null;
    for (int index = 0; index < objNames.Length; ++index)
    {
      Transform targetObject = this.GetTargetObject(adjustedObj, objNames[index], rootSearchName);
      if (!Object.op_Equality((Object) targetObject, (Object) null))
      {
        switch (index)
        {
          case 0:
            transform1 = targetObject;
            continue;
          case 1:
            transform2 = targetObject;
            continue;
          case 2:
            transform3 = targetObject;
            continue;
          case 3:
            transform4 = targetObject;
            continue;
          default:
            continue;
        }
      }
    }
    ((UIRect) adjustedObj).leftAnchor.target = transform1;
    ((UIRect) adjustedObj).rightAnchor.target = transform2;
    ((UIRect) adjustedObj).topAnchor.target = transform3;
    ((UIRect) adjustedObj).bottomAnchor.target = transform4;
    ((UIRect) adjustedObj).ResetAnchors();
    ((UIRect) adjustedObj).Update();
    Object.Destroy((Object) ((Component) this).gameObject);
  }

  public IEnumerator AdjustAnchorCoroutine(
    UIWidget adjustedObj,
    string targetObjName,
    string rootSearchName = null)
  {
    AnchorAdjustmentController adjustmentController = this;
    Transform newTarget = adjustmentController.GetTargetObject(adjustedObj, targetObjName, rootSearchName);
    while (Object.op_Equality((Object) newTarget, (Object) null))
    {
      if ((double) adjustmentController.currentRetryTime >= 2.0)
      {
        Debug.LogError((object) ("Failed to find target object: " + ((Object) ((Component) adjustedObj).gameObject).name + "->" + targetObjName));
        Object.Destroy((Object) ((Component) adjustmentController).gameObject);
      }
      Debug.LogWarning((object) string.Format("Target {0} does not exist!!!", (object) targetObjName));
      string str = ((Object) ((Component) adjustmentController).gameObject).name;
      for (Transform parent = ((Component) adjustmentController).transform.parent; Object.op_Inequality((Object) parent, (Object) null); parent = parent.parent)
        str = str + " -> " + ((Object) ((Component) parent).gameObject).name;
      newTarget = adjustmentController.GetTargetObject(adjustedObj, targetObjName, rootSearchName);
      adjustmentController.currentRetryTime += Time.deltaTime;
      yield return (object) null;
    }
    ((UIRect) adjustedObj).leftAnchor.target = newTarget;
    ((UIRect) adjustedObj).rightAnchor.target = newTarget;
    ((UIRect) adjustedObj).topAnchor.target = newTarget;
    ((UIRect) adjustedObj).bottomAnchor.target = newTarget;
    ((UIRect) adjustedObj).ResetAnchors();
    ((UIRect) adjustedObj).Update();
    Object.Destroy((Object) ((Component) adjustmentController).gameObject);
  }

  private Transform GetTargetObject(
    UIWidget adjustedObj,
    string targetObjName,
    string rootSearchName = null)
  {
    Transform targetObject = (Transform) null;
    if (!string.IsNullOrEmpty(rootSearchName))
    {
      GameObject gameObject = GameObject.Find(rootSearchName);
      if (Object.op_Inequality((Object) gameObject, (Object) null))
        targetObject = gameObject.transform.GetChildInFind(targetObjName);
    }
    else
      targetObject = ((Component) adjustedObj).transform.GetParentInFind(targetObjName);
    return targetObject;
  }

  public static void AdjustAnchor(AnchorCustomAdjustment.AnchorSetting[] settings)
  {
    AnchorAdjustmentController adjustmentController = new GameObject().AddComponent<AnchorAdjustmentController>();
    adjustmentController.StartCoroutine(adjustmentController.AdjustAnchorCoroutine(settings));
  }

  public IEnumerator AdjustAnchorCoroutine(AnchorCustomAdjustment.AnchorSetting[] settings)
  {
    AnchorAdjustmentController adjustmentController = this;
    List<AnchorCustomAdjustment.AnchorSetting> listset = ((IEnumerable<AnchorCustomAdjustment.AnchorSetting>) settings).Where<AnchorCustomAdjustment.AnchorSetting>((Func<AnchorCustomAdjustment.AnchorSetting, bool>) (s => Object.op_Inequality((Object) s.widget_, (Object) null) || Object.op_Inequality((Object) s.panel_, (Object) null))).ToList<AnchorCustomAdjustment.AnchorSetting>();
    Dictionary<string, Transform> dictarget = new Dictionary<string, Transform>();
    foreach (AnchorCustomAdjustment.AnchorSetting s in listset)
    {
      Transform tr = (Transform) null;
      while (!string.IsNullOrEmpty(s.targetParentName_) && !dictarget.TryGetValue(s.targetParentName_, out tr))
      {
        yield return (object) null;
        if (!Object.op_Equality((Object) s.widget_, (Object) null) || !Object.op_Equality((Object) s.panel_, (Object) null))
        {
          tr = (Object.op_Inequality((Object) s.widget_, (Object) null) ? ((Component) s.widget_).transform : ((Component) s.panel_).transform).GetParentInFind(s.targetParentName_);
          dictarget.Add(s.targetParentName_, tr);
        }
        else
          break;
      }
      if (Object.op_Inequality((Object) s.widget_, (Object) null))
      {
        if (Object.op_Inequality((Object) tr, (Object) null))
        {
          if (s.isTargetLeft_)
            ((UIRect) s.widget_).leftAnchor.target = tr;
          if (s.isTargetRight_)
            ((UIRect) s.widget_).rightAnchor.target = tr;
          if (s.isTargetTop_)
            ((UIRect) s.widget_).topAnchor.target = tr;
          if (s.isTargetBottom_)
            ((UIRect) s.widget_).bottomAnchor.target = tr;
        }
        ((UIRect) s.widget_).ResetAnchors();
      }
      if (Object.op_Inequality((Object) s.panel_, (Object) null))
      {
        if (Object.op_Inequality((Object) tr, (Object) null))
        {
          if (s.isTargetLeft_)
            ((UIRect) s.panel_).leftAnchor.target = tr;
          if (s.isTargetRight_)
            ((UIRect) s.panel_).rightAnchor.target = tr;
          if (s.isTargetTop_)
            ((UIRect) s.panel_).topAnchor.target = tr;
          if (s.isTargetBottom_)
            ((UIRect) s.panel_).bottomAnchor.target = tr;
        }
        ((UIRect) s.panel_).Update();
      }
      tr = (Transform) null;
    }
    List<AnchorCustomAdjustment.AnchorSetting> anchorSettingList = new List<AnchorCustomAdjustment.AnchorSetting>((IEnumerable<AnchorCustomAdjustment.AnchorSetting>) listset);
    anchorSettingList.Reverse();
    foreach (AnchorCustomAdjustment.AnchorSetting anchorSetting in anchorSettingList)
    {
      if (Object.op_Inequality((Object) anchorSetting.widget_, (Object) null))
        ((UIRect) anchorSetting.widget_).Update();
      if (Object.op_Inequality((Object) anchorSetting.panel_, (Object) null))
        ((UIRect) anchorSetting.panel_).Update();
    }
    Object.Destroy((Object) ((Component) adjustmentController).gameObject);
  }
}
