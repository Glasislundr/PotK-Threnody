// Decompiled with JetBrains decompiler
// Type: BattleUI01LayoutAutoLink
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BattleUI01LayoutAutoLink : MonoBehaviour
{
  [SerializeField]
  private BattleUI01LayoutAuto target_;
  private bool isModified_;
  private bool isActive_;
  private bool isModeAuto_;

  public void activate(bool isActive, bool isModeAuto)
  {
    this.isModified_ = true;
    this.isActive_ = isActive;
    this.isModeAuto_ = isModeAuto;
  }

  public void flush(bool isEnabled)
  {
    if (!this.isModified_)
      return;
    this.isModified_ = false;
    this.target_.activate(this.isActive_, this.isModeAuto_, isEnabled);
  }
}
