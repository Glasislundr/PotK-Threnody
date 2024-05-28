// Decompiled with JetBrains decompiler
// Type: UnitUpdate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class UnitUpdate : BattleMonoBehaviour
{
  private Vector3 aimPos;
  private Vector3 stayRotate;
  public float MoveSpeed = 8f;
  private bool isUnitMove;
  private BattleUnitParts unitParts;
  private Vector3 prevDirection = Vector3.zero;
  private Animator[] anims;
  private Transform rot;
  private Transform _transform;
  private bool isPosStayUpdate;
  private bool isAngleStayUpdate;
  private bool isUpdateAngleOnMove_ = true;
  private bool mIsForceRun;

  private BattleUnitParts UnitParts
  {
    get
    {
      if (Object.op_Inequality((Object) this.unitParts, (Object) null))
        return this.unitParts;
      this.unitParts = ((Component) this).GetComponent<BattleUnitParts>();
      return this.unitParts;
    }
  }

  public bool isUpdateAngleOnMove
  {
    get => this.isUpdateAngleOnMove_;
    set => this.isUpdateAngleOnMove_ = value;
  }

  public bool isForceRun
  {
    get => this.mIsForceRun;
    set => this.mIsForceRun = value;
  }

  protected override IEnumerator Start_Battle()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    UnitUpdate unitUpdate1 = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    unitUpdate1._transform = ((Component) unitUpdate1).transform;
    unitUpdate1.isUnitMove = false;
    unitUpdate1.SetAimPos(unitUpdate1._transform.localPosition);
    unitUpdate1._transform.localPosition = unitUpdate1.aimPos;
    UnitUpdate unitUpdate2 = unitUpdate1;
    Quaternion localRotation = unitUpdate1._transform.localRotation;
    Vector3 eulerAngles = ((Quaternion) ref localRotation).eulerAngles;
    unitUpdate2.stayRotate = eulerAngles;
    unitUpdate1.unitParts = ((Component) unitUpdate1).GetComponent<BattleUnitParts>();
    unitUpdate1.ResetAnimation();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void ResetAnimation()
  {
    this.anims = ((Component) this._transform.GetChildInFind("angle")).GetComponentsInChildren<Animator>(true);
    this.rot = this._transform.GetChildInFind("rot");
  }

  protected override void Update_Battle()
  {
    bool flag = false;
    float l = this.MoveSpeed * Time.deltaTime;
    float num = Vector3.Distance(this.aimPos, this._transform.localPosition);
    if ((double) l > 0.0 && !this.IsArrive(num))
    {
      flag = true;
      Vector3 vector3 = Vector3.op_Subtraction(this.aimPos, this._transform.localPosition);
      Vector3 normalized = ((Vector3) ref vector3).normalized;
      this.prevDirection = normalized;
      this.PosMoveUpdate(normalized, l, num);
      if (this.isUpdateAngleOnMove_)
        this.AngleMoveUpdate(normalized);
      this.setAnimationBool("isRun", true);
    }
    if (this.isUnitMove && !flag && this.isUpdateAngleOnMove_)
    {
      Quaternion quaternion = Quaternion.LookRotation(this.prevDirection);
      float y = ((Quaternion) ref quaternion).eulerAngles.y;
      this.UnitParts.setDirection(y);
      this.SetStayRotateY(y);
    }
    if (!this.isUnitMove && !flag)
    {
      this.PosStayUpdate();
      this.AngleStayUpdate();
    }
    this.setAnimationBool("isRun", flag || this.unitParts.isMoveUnit() || this.mIsForceRun);
    this.isUnitMove = flag;
  }

  private void PosMoveUpdate(Vector3 d, float l, float clamp)
  {
    Transform transform = this._transform;
    transform.localPosition = Vector3.op_Addition(transform.localPosition, Vector3.ClampMagnitude(Vector3.op_Multiply(d, l), clamp));
  }

  private void AngleMoveUpdate(Vector3 d)
  {
    this.rot.localRotation = Quaternion.Slerp(this.rot.localRotation, Quaternion.LookRotation(d), 0.5f);
  }

  public void PosStayUpdate()
  {
    if (this.isPosStayUpdate)
      return;
    this._transform.localPosition = this.aimPos;
    this.isPosStayUpdate = true;
  }

  public void AngleStayUpdate()
  {
    if (this.isAngleStayUpdate)
      return;
    this.rot.localRotation = Quaternion.Euler(this.stayRotate.x, this.stayRotate.y, this.stayRotate.z);
    this.isAngleStayUpdate = true;
  }

  public bool IsMove() => this.isUnitMove;

  public bool IsArrive(float distance) => (double) distance < 0.10000000149011612;

  public void SetAimPos(Vector3 v)
  {
    Vector3 vector3 = Vector3.op_Addition(v, this._transform.parent.position);
    vector3.y += 50f;
    int num = 1 << LayerMask.NameToLayer("Terrain");
    RaycastHit raycastHit;
    this.aimPos = !Physics.Raycast(vector3, Vector3.down, ref raycastHit, 100f, num) ? v : Vector3.op_Subtraction(((RaycastHit) ref raycastHit).point, this._transform.parent.position);
    this.isPosStayUpdate = false;
  }

  public void SetStayRotateY(float y)
  {
    ((Vector3) ref this.stayRotate).Set(this.stayRotate.x, y, this.stayRotate.z);
    this.isAngleStayUpdate = false;
  }

  public void setAnimationTrigger(string name)
  {
    foreach (Animator anim in this.anims)
    {
      if (NGUITools.GetActive((Behaviour) anim) && Object.op_Inequality((Object) anim.runtimeAnimatorController, (Object) null))
        anim.SetTrigger(name);
    }
  }

  public void setAnimationBool(string name, bool v)
  {
    foreach (Animator anim in this.anims)
    {
      if (NGUITools.GetActive((Behaviour) anim) && Object.op_Inequality((Object) anim.runtimeAnimatorController, (Object) null))
        anim.SetBool(name, v);
    }
  }
}
