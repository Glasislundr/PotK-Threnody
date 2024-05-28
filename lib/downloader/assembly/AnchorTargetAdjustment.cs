// Decompiled with JetBrains decompiler
// Type: AnchorTargetAdjustment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class AnchorTargetAdjustment : MonoBehaviour
{
  public string targetParentName = "MainPanel";

  private void Start()
  {
    UIWidget component = ((Component) this).GetComponent<UIWidget>();
    if (Object.op_Equality((Object) component, (Object) null))
      return;
    AnchorAdjustmentController.AdjustAnchor(component, this.targetParentName);
  }

  private void OnDisable()
  {
    UIWidget component = ((Component) this).GetComponent<UIWidget>();
    if (Object.op_Equality((Object) component, (Object) null))
      return;
    Transform parentInFind = ((Component) this).transform.GetParentInFind(this.targetParentName);
    if (Object.op_Equality((Object) parentInFind, (Object) null))
    {
      Debug.LogError((object) ("AnchorTargetAdjustment Not Parent Error. Parent Name is " + this.targetParentName));
    }
    else
    {
      ((UIRect) component).leftAnchor.target = parentInFind;
      ((UIRect) component).rightAnchor.target = parentInFind;
      ((UIRect) component).topAnchor.target = parentInFind;
      ((UIRect) component).bottomAnchor.target = parentInFind;
      ((UIRect) component).ResetAnchors();
      ((UIRect) component).Update();
    }
  }
}
