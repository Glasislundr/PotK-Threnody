// Decompiled with JetBrains decompiler
// Type: MissileWeapon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class MissileWeapon : MonoBehaviour
{
  public bool noWaitInitStatusChange;
  public float shootDelay;
  public float initSpeed;
  public float accelSpeed;
  public float deaccelSpeed;
  public Vector3? bulletOffset;
  public float bulletDisappearDelay;
  public Transform damagePoint;
  public GameObject handWeapon;
  public string damageEffect;
  public GameObject myUnit;
  private Vector3 origPosition;
  private Quaternion origRotation;
  public iTween.EaseType easetype = (iTween.EaseType) 21;
  private Hashtable buh = new Hashtable();
  private float internalTime;
  private bool mbFire;
  private Linear mLinearX;
  private Transform firstpos;
  private bool isHit;
  private MissileWeapon.MBStatus status;

  private void Start() => this.internalTime = 0.0f;

  private void Update()
  {
    this.internalTime += Time.deltaTime;
    switch (this.status)
    {
      case MissileWeapon.MBStatus.ST_INIT:
        this.firstpos = this.myUnit.gameObject.transform.GetChildInFind("muzzle");
        if (Object.op_Equality((Object) null, (Object) this.firstpos))
          this.firstpos = this.myUnit.gameObject.transform.GetChildInFind("weaponr");
        ((Component) this).gameObject.transform.localPosition = this.firstpos.position;
        ((Component) this).gameObject.transform.LookAt(this.damagePoint);
        this.status = MissileWeapon.MBStatus.ST_PREPARE;
        if (!this.noWaitInitStatusChange)
          break;
        goto case MissileWeapon.MBStatus.ST_PREPARE;
      case MissileWeapon.MBStatus.ST_PREPARE:
        this.status = MissileWeapon.MBStatus.ST_READY;
        if (!this.noWaitInitStatusChange)
          break;
        goto case MissileWeapon.MBStatus.ST_READY;
      case MissileWeapon.MBStatus.ST_READY:
        if (!this.mbFire)
          break;
        this.status = MissileWeapon.MBStatus.ST_BULLET;
        if (!this.noWaitInitStatusChange)
          break;
        goto case MissileWeapon.MBStatus.ST_BULLET;
      case MissileWeapon.MBStatus.ST_BULLET:
        if ((double) this.shootDelay > (double) this.internalTime)
          break;
        if (this.bulletOffset.HasValue)
          ((Component) this).gameObject.transform.localPosition = this.bulletOffset.Value;
        NGDuelUnit component = this.myUnit.GetComponent<NGDuelUnit>();
        if (Object.op_Inequality((Object) null, (Object) component) && component.mMyUnitData.playerUnit.equippedWeaponGearOrInitial.kind.Enum == GearKindEnum.axe)
        {
          if ((double) this.myUnit.transform.position.x > 0.0)
          {
            ((Component) this).gameObject.transform.rotation = Quaternion.identity;
            ((Component) this).gameObject.transform.localRotation = Quaternion.identity;
          }
          else
          {
            ((Component) this).gameObject.transform.rotation = Quaternion.identity;
            ((Component) this).gameObject.transform.localEulerAngles = new Vector3(180f, 0.0f, 0.0f);
          }
        }
        this.buh.Clear();
        this.buh.Add((object) "position", (object) this.damagePoint);
        this.buh.Add((object) "speed", (object) this.initSpeed);
        this.buh.Add((object) "oncomplete", (object) "onFlyEnd");
        this.buh.Add((object) "easetype", (object) this.easetype);
        this.buh.Add((object) "oncompletetarget", (object) ((Component) this).gameObject);
        iTween.MoveTo(((Component) this).gameObject, this.buh);
        this.status = MissileWeapon.MBStatus.ST_FLYING;
        break;
      case MissileWeapon.MBStatus.ST_FLYEND:
        this.internalTime = 0.0f;
        if (Object.op_Inequality((Object) null, (Object) this.handWeapon) && this.isHit)
        {
          this.handWeapon.transform.parent = this.myUnit.gameObject.transform.GetChildInFind("weaponr");
          this.handWeapon.transform.localPosition = this.origPosition;
          this.handWeapon.transform.localRotation = this.origRotation;
        }
        this.myUnit.GetComponent<clipEffectPlayer>().playEffect(string.Format("{0}:1", (object) this.damageEffect));
        this.mLinearX = (Linear) null;
        if (!this.isHit)
        {
          float x = ((Component) this).gameObject.transform.position.x;
          float num = x;
          float e = (double) x <= 0.0 ? num - 10f : num + 10f;
          this.mLinearX = new Linear(x, e, Mathf.Abs(x - e) / this.initSpeed);
          ++this.bulletDisappearDelay;
        }
        this.status = MissileWeapon.MBStatus.ST_ENDWAIT;
        break;
      case MissileWeapon.MBStatus.ST_ENDWAIT:
        if ((double) this.internalTime > (double) this.bulletDisappearDelay)
        {
          ((Component) this).gameObject.SetActive(false);
          this.status = MissileWeapon.MBStatus.ST_END;
        }
        if (this.mLinearX == null)
          break;
        this.mLinearX.Update(Time.deltaTime);
        if (this.mLinearX.isEnd)
        {
          if (Object.op_Inequality((Object) null, (Object) this.handWeapon))
          {
            this.handWeapon.transform.parent = this.myUnit.gameObject.transform.GetChildInFind("weaponr");
            this.handWeapon.transform.localPosition = this.origPosition;
            this.handWeapon.transform.localRotation = this.origRotation;
          }
          ((Component) this).gameObject.SetActive(false);
          this.status = MissileWeapon.MBStatus.ST_END;
          break;
        }
        Vector3 position = ((Component) this).gameObject.transform.position;
        position.x = this.mLinearX.value;
        ((Component) this).gameObject.transform.position = position;
        break;
      case MissileWeapon.MBStatus.ST_END:
        Object.Destroy((Object) ((Component) this).gameObject);
        break;
    }
  }

  public void Initialize()
  {
  }

  public void Fire(Transform target, GameObject myself, bool is_hit = true)
  {
    this.damagePoint = target;
    this.status = MissileWeapon.MBStatus.ST_INIT;
    this.mbFire = true;
    this.internalTime = 0.0f;
    this.myUnit = myself;
    this.isHit = is_hit;
    ((Component) this).gameObject.transform.rotation = Quaternion.identity;
    ((Component) this).gameObject.transform.LookAt(target);
    if (!Object.op_Inequality((Object) null, (Object) this.handWeapon))
      return;
    this.origRotation = this.handWeapon.transform.localRotation;
    this.origPosition = this.handWeapon.transform.localPosition;
    this.handWeapon.transform.parent = ((Component) this).gameObject.transform.GetChildInFind("attach_point");
    if ((double) this.myUnit.gameObject.transform.position.x > 0.0)
    {
      NGDuelUnit component = this.myUnit.GetComponent<NGDuelUnit>();
      if (!Object.op_Inequality((Object) null, (Object) component))
        return;
      if (component.mMyUnitData.playerUnit.equippedWeaponGearOrInitial.kind.Enum == GearKindEnum.spear)
        this.handWeapon.transform.localEulerAngles = new Vector3(-180f, -180f, 0.0f);
      else
        this.handWeapon.transform.localEulerAngles = new Vector3(-180f, 0.0f, 0.0f);
    }
    else
      this.handWeapon.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
  }

  private void onFlyEnd()
  {
    if (Object.op_Inequality((Object) null, (Object) this.handWeapon))
    {
      this.handWeapon.transform.parent = this.myUnit.gameObject.transform.GetChildInFind("weaponr");
      this.handWeapon.transform.localPosition = this.origPosition;
      this.handWeapon.transform.localRotation = this.origRotation;
    }
    this.myUnit.GetComponent<clipEffectPlayer>().playEffect(string.Format("{0}:1", (object) this.damageEffect));
    this.status = MissileWeapon.MBStatus.ST_ENDWAIT;
  }

  private enum MBStatus
  {
    ST_NONE,
    ST_INIT,
    ST_PREPARE,
    ST_READY,
    ST_BULLET,
    ST_FLYING,
    ST_FLYEND,
    ST_ENDWAIT,
    ST_END,
  }
}
