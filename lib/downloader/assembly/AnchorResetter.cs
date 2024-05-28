// Decompiled with JetBrains decompiler
// Type: AnchorResetter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class AnchorResetter : MonoBehaviour
{
  private bool firstEnable = true;

  private void Awake() => this.firstEnable = true;

  private void OnEnable()
  {
    if (!this.firstEnable)
      return;
    this.firstEnable = false;
    UIRect component = ((Component) this).GetComponent<UIRect>();
    if (!Object.op_Inequality((Object) component, (Object) null))
      return;
    component.ResetAnchors();
    component.Update();
  }
}
