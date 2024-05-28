// Decompiled with JetBrains decompiler
// Type: AnchorCustomAdjustment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[AddComponentMenu("Utility/Behaviour/AnchorCustomAdjustment")]
public class AnchorCustomAdjustment : MonoBehaviour
{
  [SerializeField]
  [Tooltip("自動で実行しない")]
  private bool isManualReset_;
  [SerializeField]
  private AnchorCustomAdjustment.AnchorSetting[] settings_ = new AnchorCustomAdjustment.AnchorSetting[1];

  private void Start()
  {
    if (this.isManualReset_)
      return;
    AnchorAdjustmentController.AdjustAnchor(this.settings_);
  }

  private void OnDisable()
  {
    if (this.isManualReset_)
      return;
    AnchorAdjustmentController.AdjustAnchor(this.settings_);
  }

  public void resetAnchors() => AnchorAdjustmentController.AdjustAnchor(this.settings_);

  public void resetAnchors(Transform[] subTargets)
  {
    Dictionary<string, Transform> dictionary = subTargets != null ? ((IEnumerable<Transform>) subTargets).ToDictionary<Transform, string>((Func<Transform, string>) (x => ((Object) x).name)) : new Dictionary<string, Transform>();
    foreach (AnchorCustomAdjustment.AnchorSetting setting in this.settings_)
    {
      Transform parentInFind;
      if (!dictionary.TryGetValue(setting.targetParentName_, out parentInFind))
      {
        parentInFind = (Object.op_Implicit((Object) setting.widget_) ? ((Component) setting.widget_).transform : ((Component) setting.panel_).transform).GetParentInFind(setting.targetParentName_);
        dictionary.Add(setting.targetParentName_, parentInFind);
      }
      if (Object.op_Inequality((Object) setting.widget_, (Object) null))
      {
        if (setting.isTargetLeft_)
          ((UIRect) setting.widget_).leftAnchor.target = parentInFind;
        if (setting.isTargetRight_)
          ((UIRect) setting.widget_).rightAnchor.target = parentInFind;
        if (setting.isTargetTop_)
          ((UIRect) setting.widget_).topAnchor.target = parentInFind;
        if (setting.isTargetBottom_)
          ((UIRect) setting.widget_).bottomAnchor.target = parentInFind;
      }
      if (Object.op_Inequality((Object) setting.panel_, (Object) null))
      {
        if (setting.isTargetLeft_)
          ((UIRect) setting.panel_).leftAnchor.target = parentInFind;
        if (setting.isTargetRight_)
          ((UIRect) setting.panel_).rightAnchor.target = parentInFind;
        if (setting.isTargetTop_)
          ((UIRect) setting.panel_).topAnchor.target = parentInFind;
        if (setting.isTargetBottom_)
          ((UIRect) setting.panel_).bottomAnchor.target = parentInFind;
      }
    }
    foreach (UIRect uiRect in ((IEnumerable<UIRect>) ((Component) this).GetComponentsInChildren<UIRect>()).Where<UIRect>((Func<UIRect, bool>) (x => x.isAnchored)))
    {
      uiRect.ResetAnchors();
      uiRect.Update();
    }
  }

  [Serializable]
  public class AnchorSetting
  {
    public UIWidget widget_;
    public UIPanel panel_;
    public string targetParentName_;
    public bool isTargetLeft_ = true;
    public bool isTargetRight_ = true;
    public bool isTargetTop_ = true;
    public bool isTargetBottom_ = true;
  }
}
