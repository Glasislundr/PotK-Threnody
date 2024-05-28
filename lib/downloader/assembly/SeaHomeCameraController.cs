// Decompiled with JetBrains decompiler
// Type: SeaHomeCameraController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class SeaHomeCameraController : MonoBehaviour
{
  [SerializeField]
  private Camera target;
  [SerializeField]
  private float ratationXUnit = 0.1f;
  [SerializeField]
  private float maxRatationX = 10f;
  [SerializeField]
  private float minRatationX = -10f;
  [SerializeField]
  private float ratationYUnit = 0.1f;
  [SerializeField]
  private float maxRatationY = 10f;
  [SerializeField]
  private float minRatationY = -10f;
  [SerializeField]
  private float positionZUnit = 0.03f;
  [SerializeField]
  private float maxPositionZ = 1f;
  [SerializeField]
  private float minPositionZ = -3f;
  [SerializeField]
  private Vector3 LookupCameraOffset = new Vector3(0.0f, 0.0f, 3f);
  [SerializeField]
  private float addFov = 15f;
  [SerializeField]
  private float fovPerSec = 10f;
  [SerializeField]
  private float minFov = 55f;
  [SerializeField]
  private float maxFov = 70f;
  private SeaHomeCameraController.CameraMode _cameraMode;
  private Transform m_transform;
  private Vector3 originalPosition;
  private Quaternion originalRotation;
  private Vector3 changeRotation;
  private float moveZ;
  private bool isGyroscope;
  private bool isLookuped;
  private bool isAnimeted;
  private bool isAutoFocus = true;
  private Transform targetTransform;
  private SeaHomeManager owner;

  public Camera mainCamera => this.target;

  public SeaHomeCameraController.CameraMode cameraMode => this._cameraMode;

  public bool IsLookuped => this.isLookuped;

  public bool IsAutoFocus
  {
    get => this.isAutoFocus;
    set => this.isAutoFocus = value;
  }

  private void SetOwner(SeaHomeManager manager) => this.owner = manager;

  public void Init(SeaHomeManager manager)
  {
    this.SetOwner(manager);
    this.mainCamera.fieldOfView = this.maxFov;
  }

  public bool SetMode(SeaHomeCameraController.CameraMode mode)
  {
    if (this._cameraMode == mode)
      return true;
    this._cameraMode = mode;
    return true;
  }

  public bool SetLookupUnit(Transform target)
  {
    if (Object.op_Inequality((Object) this.targetTransform, (Object) null) || this.isLookuped || this.isAnimeted)
      return false;
    this.targetTransform = target;
    this.isLookuped = true;
    Hashtable hashtable = new Hashtable();
    hashtable.Add((object) "position", (object) new Vector3(target.position.x + this.LookupCameraOffset.x, this.originalPosition.y + this.LookupCameraOffset.y, target.position.z + this.LookupCameraOffset.z));
    hashtable.Add((object) "islocal", (object) false);
    hashtable.Add((object) "speed", (object) 5f);
    if (this._cameraMode != SeaHomeCameraController.CameraMode.OPERATION)
    {
      this.isAnimeted = true;
      hashtable.Add((object) "easetype", (object) "easeInOutExpo");
      hashtable.Add((object) "oncomplete", (object) "OnSetMoveCompleteHandler");
      hashtable.Add((object) "oncompletetarget", (object) ((Component) this).gameObject);
      iTween.MoveTo(((Component) this).gameObject, hashtable);
    }
    else
    {
      hashtable.Add((object) "easetype", (object) "linear");
      iTween.MoveUpdate(((Component) this).gameObject, hashtable);
    }
    return true;
  }

  public bool ResetLookupUnit(Transform target)
  {
    if (Object.op_Inequality((Object) this.targetTransform, (Object) target) || !this.isLookuped || this.isAnimeted)
      return false;
    this.isAnimeted = true;
    iTween.MoveTo(((Component) this).gameObject, new Hashtable()
    {
      {
        (object) "position",
        (object) this.originalPosition
      },
      {
        (object) "islocal",
        (object) false
      },
      {
        (object) "speed",
        (object) 5f
      },
      {
        (object) "easetype",
        (object) "easeInOutExpo"
      },
      {
        (object) "oncomplete",
        (object) "OnResetMoveCompleteHandler"
      },
      {
        (object) "oncompletetarget",
        (object) ((Component) this).gameObject
      }
    });
    return true;
  }

  public void Reset()
  {
    this.changeRotation = new Vector3();
    this.moveZ = 0.0f;
    this.isAnimeted = true;
    iTween.MoveTo(((Component) this).gameObject, new Hashtable()
    {
      {
        (object) "position",
        (object) this.originalPosition
      },
      {
        (object) "islocal",
        (object) false
      },
      {
        (object) "time",
        (object) 0.5f
      },
      {
        (object) "easetype",
        (object) "easeInOutExpo"
      },
      {
        (object) "oncomplete",
        (object) "OnResetMoveCompleteHandler"
      },
      {
        (object) "oncompletetarget",
        (object) ((Component) this).gameObject
      }
    });
    iTween.RotateTo(((Component) this).gameObject, new Hashtable()
    {
      {
        (object) "rotation",
        (object) ((Quaternion) ref this.originalRotation).eulerAngles
      },
      {
        (object) "islocal",
        (object) true
      },
      {
        (object) "time",
        (object) 0.5f
      },
      {
        (object) "easetype",
        (object) "linear"
      }
    });
  }

  private void Start()
  {
    this.isGyroscope = SystemInfo.supportsGyroscope;
    this.isLookuped = false;
    this.isAnimeted = false;
    this.m_transform = ((Component) this).transform;
    this.originalPosition = new Vector3(this.m_transform.position.x, this.m_transform.position.y, this.m_transform.position.z);
    this.originalRotation = new Quaternion(this.m_transform.localRotation.x, this.m_transform.localRotation.y, this.m_transform.localRotation.z, ((Component) this).transform.localRotation.w);
    this.changeRotation = new Vector3();
    if (!this.isGyroscope)
      return;
    Input.gyro.enabled = true;
  }

  private void Update()
  {
    if (this._cameraMode == SeaHomeCameraController.CameraMode.OPERATION)
    {
      if (this.isGyroscope && Input.gyro.enabled)
      {
        Vector3 rotationRate = Input.gyro.rotationRate;
        this.changeRotation.x = Mathf.Clamp(this.changeRotation.x - rotationRate.x, this.minRatationX, this.maxRatationX);
        this.changeRotation.y = Mathf.Clamp(this.changeRotation.y - rotationRate.y, this.minRatationY, this.maxRatationY);
        this.m_transform.localRotation = Quaternion.op_Multiply(this.originalRotation, Quaternion.Euler(this.changeRotation));
      }
      else
      {
        if (Input.GetKey((KeyCode) 273))
          this.changeRotation.x = Mathf.Clamp(this.changeRotation.x - this.ratationXUnit, this.minRatationX, this.maxRatationX);
        if (Input.GetKey((KeyCode) 274))
          this.changeRotation.x = Mathf.Clamp(this.changeRotation.x + this.ratationXUnit, this.minRatationX, this.maxRatationX);
        if (Input.GetKey((KeyCode) 276))
          this.changeRotation.y = Mathf.Clamp(this.changeRotation.y - this.ratationYUnit, this.minRatationY, this.maxRatationY);
        if (Input.GetKey((KeyCode) 275))
          this.changeRotation.y = Mathf.Clamp(this.changeRotation.y + this.ratationYUnit, this.minRatationY, this.maxRatationY);
        if (Input.GetKey((KeyCode) 119))
          this.moveZ = Mathf.Clamp(this.moveZ - this.positionZUnit, this.minPositionZ, this.maxPositionZ);
        if (Input.GetKey((KeyCode) 115))
          this.moveZ = Mathf.Clamp(this.moveZ + this.positionZUnit, this.minPositionZ, this.maxPositionZ);
        this.m_transform.localRotation = Quaternion.op_Multiply(this.originalRotation, Quaternion.Euler(this.changeRotation));
      }
      if (!this.isLookuped || !Object.op_Inequality((Object) this.targetTransform, (Object) null) || this.isAnimeted)
        return;
      iTween.MoveUpdate(((Component) this).gameObject, new Hashtable()
      {
        {
          (object) "position",
          (object) new Vector3(this.targetTransform.position.x + this.LookupCameraOffset.x, this.originalPosition.y + this.LookupCameraOffset.y, this.targetTransform.position.z + this.LookupCameraOffset.z)
        },
        {
          (object) "islocal",
          (object) false
        },
        {
          (object) "speed",
          (object) 5f
        },
        {
          (object) "easetype",
          (object) "linear"
        }
      });
    }
    else
    {
      if (this._cameraMode != SeaHomeCameraController.CameraMode.NORMAL || !this.isAutoFocus || this.isLookuped || this.isAnimeted)
        return;
      float num1 = 0.0f;
      if (!Object.op_Inequality((Object) this.owner, (Object) null))
        return;
      Matrix4x4 localToWorldMatrix = this.m_transform.localToWorldMatrix;
      Vector3 vector3_1 = Vector3.op_Subtraction(((Matrix4x4) ref localToWorldMatrix).MultiplyPoint3x4(new Vector3(0.0f, 0.0f, 1f)), this.m_transform.position);
      ((Vector3) ref vector3_1).Normalize();
      foreach (SeaHomeUnitController unitControler in this.owner.UnitControlers)
      {
        if (!Object.op_Equality((Object) unitControler.UnitTransform, (Object) null))
        {
          Vector3 vector3_2 = Vector3.op_Subtraction(unitControler.UnitTransform.position, this.m_transform.position);
          ((Vector3) ref vector3_2).Normalize();
          float num2 = Mathf.Acos(Vector2.Dot(new Vector2(vector3_1.x, vector3_1.z), new Vector2(vector3_2.x, vector3_2.z))) * 57.29578f;
          if ((double) num1 < (double) num2)
            num1 = num2;
        }
      }
      float num3 = Mathf.Clamp((float) (((double) num1 + (double) this.addFov) * 2.0), this.minFov, this.maxFov);
      float fieldOfView = this.mainCamera.fieldOfView;
      if (Mathf.Approximately(fieldOfView, num3))
        return;
      float num4 = Time.deltaTime * this.fovPerSec;
      float num5;
      if ((double) fieldOfView > (double) num3)
      {
        num5 = fieldOfView - num4;
        if ((double) num5 < (double) num3)
          num5 = num3;
      }
      else
      {
        num5 = fieldOfView + num4;
        if ((double) num5 > (double) num3)
          num5 = num3;
      }
      this.mainCamera.fieldOfView = num5;
    }
  }

  private void OnSetMoveCompleteHandler() => this.isAnimeted = false;

  private void OnResetMoveCompleteHandler()
  {
    this.isAnimeted = false;
    this.isLookuped = false;
    this.targetTransform = (Transform) null;
  }

  public enum CameraMode
  {
    NORMAL,
    OPERATION,
  }
}
