// Decompiled with JetBrains decompiler
// Type: MapEditFacilityCost
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using UnityEngine;

#nullable disable
public class MapEditFacilityCost : MonoBehaviour
{
  [SerializeField]
  private UILabel txtCost_;
  [SerializeField]
  private Transform offset_;
  [SerializeField]
  private Transform localScale_;
  [SerializeField]
  [Tooltip("補間速度0:視点切替n->n+1|1:視点切替2->0")]
  private MapEditFacilityCost.Speed[] tableSpeed_ = new MapEditFacilityCost.Speed[2];
  private bool initalized_;
  private WeakReference wObj3D_;
  private Camera cam3D_;
  private Camera cam2D_;
  private Vector3 posCamera_ = Vector3.zero;
  private Vector3 posTarget_ = Vector3.zero;
  private BL.StructValue<Vector2> refOffset_;
  private BL.StructValue<Vector2> refScale_;
  private float offsetTargetY_;
  private const float TIME_CHANGE_POSITION = 0.3f;

  private GameObject obj3D_
  {
    get
    {
      return this.wObj3D_ == null || !this.wObj3D_.IsAlive ? (GameObject) null : this.wObj3D_.Target as GameObject;
    }
    set
    {
      if (!Object.op_Inequality((Object) this.obj3D_, (Object) value))
        return;
      this.wObj3D_ = new WeakReference((object) value);
    }
  }

  public void initialize(Camera cam3D, Camera cam2D, GameObject go, float offsetY, int cost)
  {
    this.cam3D_ = cam3D;
    this.cam2D_ = cam2D;
    this.obj3D_ = go;
    this.offsetTargetY_ = offsetY;
    this.updatePosition(true);
    this.txtCost_.SetTextLocalize(cost);
    this.initalized_ = true;
  }

  public void setReferenceLocalPosition(
    BL.StructValue<Vector2> offset,
    BL.StructValue<Vector2> scale)
  {
    this.refOffset_ = offset;
    this.refScale_ = scale;
    this.updateLocalPosition(true);
  }

  private void OnEnable()
  {
    this.updateLocalPosition(true);
    if (!this.initalized_)
      return;
    this.updatePosition();
  }

  public void updateLocalPosition(bool immediate, int noSpeed = 0)
  {
    MapEditFacilityCost.Speed speed = !immediate ? (this.tableSpeed_.Length > noSpeed ? this.tableSpeed_[noSpeed] : new MapEditFacilityCost.Speed()) : (MapEditFacilityCost.Speed) null;
    if (this.refOffset_ != null)
    {
      if (immediate)
        this.offset_.localPosition = Vector2.op_Implicit(this.refOffset_.value);
      else if ((double) this.offset_.localPosition.x != (double) this.refOffset_.value.x || (double) this.offset_.localPosition.y != (double) this.refOffset_.value.y)
      {
        TweenPosition orAddComponent = ((Component) this.offset_).gameObject.GetOrAddComponent<TweenPosition>();
        ((UITweener) orAddComponent).animationCurve = speed.animationCurve_;
        ((UITweener) orAddComponent).duration = speed.duration_;
        orAddComponent.worldSpace = false;
        ((UITweener) orAddComponent).SetStartToCurrentValue();
        orAddComponent.to = Vector2.op_Implicit(this.refOffset_.value);
        ((UITweener) orAddComponent).ResetToBeginning();
        ((UITweener) orAddComponent).Play(true);
      }
    }
    if (this.refScale_ == null)
      return;
    if (immediate)
    {
      this.localScale_.localScale = Vector2.op_Implicit(this.refScale_.value);
    }
    else
    {
      if ((double) this.localScale_.localScale.x == (double) this.refScale_.value.x && (double) this.localScale_.localScale.y == (double) this.refScale_.value.y)
        return;
      TweenScale orAddComponent = ((Component) this.localScale_).gameObject.GetOrAddComponent<TweenScale>();
      ((UITweener) orAddComponent).animationCurve = speed.animationCurve_;
      ((UITweener) orAddComponent).duration = speed.duration_;
      ((UITweener) orAddComponent).SetStartToCurrentValue();
      orAddComponent.to = new Vector3(this.refScale_.value.x, this.refScale_.value.y, 1f);
      ((UITweener) orAddComponent).ResetToBeginning();
      ((UITweener) orAddComponent).Play(true);
    }
  }

  private void Update()
  {
    if (!this.initalized_)
      return;
    this.updatePosition();
  }

  private void updatePosition(bool bInit = false)
  {
    GameObject obj3D = this.obj3D_;
    if (Object.op_Equality((Object) obj3D, (Object) null))
    {
      Object.Destroy((Object) ((Component) this).gameObject);
    }
    else
    {
      if (!bInit)
        bInit = Vector3.op_Inequality(this.posCamera_, ((Component) this.cam3D_).transform.position);
      if (!bInit)
        bInit = Vector3.op_Inequality(this.posTarget_, obj3D.transform.position);
      if (!bInit)
        return;
      this.posCamera_ = ((Component) this.cam3D_).transform.position;
      this.posTarget_ = obj3D.transform.position;
      Vector3 worldPoint = this.cam2D_.ViewportToWorldPoint(this.cam3D_.WorldToViewportPoint(new Vector3(this.posTarget_.x, this.posTarget_.y + this.offsetTargetY_, this.posTarget_.z)));
      worldPoint.z = 0.0f;
      ((Component) this).transform.position = worldPoint;
    }
  }

  [Serializable]
  public class Speed
  {
    public AnimationCurve animationCurve_ = AnimationCurve.Linear(0.0f, 0.0f, 1f, 1f);
    public float duration_ = 0.3f;
  }
}
