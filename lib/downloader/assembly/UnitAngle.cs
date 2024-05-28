// Decompiled with JetBrains decompiler
// Type: UnitAngle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class UnitAngle : BattleMonoBehaviour
{
  private Quaternion q;
  private Quaternion saveRot = Quaternion.identity;
  private Transform _parent;
  private Transform _transform;

  private void Awake()
  {
    this._transform = ((Component) this).transform;
    this._parent = this._transform.parent;
  }

  protected override IEnumerator Start_Battle()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    UnitAngle unitAngle = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    unitAngle.q = Quaternion.Euler(unitAngle.battleManager.unitAngleValue, 0.0f, 0.0f);
    unitAngle.calcAngle();
    return false;
  }

  private void OnEnable() => this.calcAngle();

  private void calcAngle()
  {
    this._transform.localRotation = Quaternion.op_Multiply(Quaternion.op_Multiply(Quaternion.Inverse(this._parent.rotation), this.q), this._parent.rotation);
    this.saveRot = this._parent.rotation;
  }

  protected override void LateUpdate_Battle()
  {
    if (!Quaternion.op_Inequality(this.saveRot, this._parent.rotation))
      return;
    this.calcAngle();
  }
}
