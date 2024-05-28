// Decompiled with JetBrains decompiler
// Type: DuelCameraManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class DuelCameraManager : MonoBehaviour
{
  public float smoothTime = 0.1f;
  [SerializeField]
  public GameObject duelCamera;
  private GameObject subCamera;
  [SerializeField]
  public GameObject koyuCamera;
  [SerializeField]
  [Tooltip("koyuCameraオリジナル")]
  private GameObject koyuCameraPrefab;
  private float koyuCameraFiledOfView = 40.79f;
  private bool isInitialized;
  private bool camelaLastMove;
  [SerializeField]
  private Transform lookat;
  [SerializeField]
  private Transform lookat_center;
  [SerializeField]
  private Transform lookat_attacker;
  [SerializeField]
  private Transform lookat_defender;
  public Transform lookat_bullet;
  public float MaxFarCameraZ = 20f;
  public float MinNearCameraZ = 2f;
  public bool IsShowCameraName;
  private Vector3 originalPosition;
  private Quaternion originalRotation;
  private Transform originalLookAtTarget;
  private static readonly Quaternion RotationWinAnimator = Quaternion.Euler(0.0f, 90f, 0.0f);
  private static readonly Vector3 PositionWinCam_r1 = new Vector3(0.0f, 0.0f, 2.5f);
  private static readonly Vector3 PositionWinCam_01 = new Vector3(0.0f, 0.0f, 5f);

  public GameObject DuelCamera => this.duelCamera;

  public GameObject currentCamera
  {
    get
    {
      if (!Object.op_Inequality((Object) this.duelCamera, (Object) null))
        return (GameObject) null;
      if (this.duelCamera.activeInHierarchy)
        return this.duelCamera.gameObject;
      return !Object.op_Inequality((Object) this.koyuCamera, (Object) null) ? (GameObject) null : this.koyuCamera.gameObject;
    }
  }

  public void Initialize(bool bFin, bool isSkipped)
  {
    if (this.isInitialized && !bFin)
    {
      GameObject gameObject1 = this.koyuCamera.gameObject;
      string name = ((Object) gameObject1).name;
      int num = 0;
      Transform parent = gameObject1.transform.parent;
      int childCount = parent.childCount;
      for (int index = 0; index < childCount; ++index)
      {
        if (Object.op_Equality((Object) ((Component) parent.GetChild(index)).gameObject, (Object) gameObject1))
        {
          num = index;
          break;
        }
      }
      this.koyuCamera.SetActive(false);
      Object.Destroy((Object) this.koyuCamera);
      this.koyuCamera = this.koyuCameraPrefab.Clone(parent);
      GameObject gameObject2 = this.koyuCamera.gameObject;
      ((Object) gameObject2).name = name;
      gameObject2.transform.SetSiblingIndex(num);
      this.koyuCameraFiledOfView = ((Component) this.koyuCamera.transform.GetChild(0)).gameObject.GetComponent<Camera>().fieldOfView;
    }
    ((Behaviour) this.koyuCamera.GetComponentInChildren<Camera>(true)).enabled = false;
    if (this.isInitialized)
    {
      this.duelCamera.gameObject.SetActive(true);
      this.duelCamera.transform.position = this.originalPosition;
      ((Behaviour) this.duelCamera.GetComponent<Camera>()).enabled = false;
      ((Behaviour) this.subCamera.GetComponent<Camera>()).enabled = false;
      if (!isSkipped)
        return;
      this.duelCamera.transform.rotation = this.originalRotation;
      this.lookat = this.lookat_center;
    }
    else
    {
      this.duelCamera = GameObject.Find("CameraFirst");
      this.duelCamera.gameObject.SetActive(true);
      this.originalPosition = this.duelCamera.transform.position;
      this.originalRotation = this.duelCamera.transform.rotation;
      this.originalLookAtTarget = this.lookat;
      this.subCamera = GameObject.Find("CameraChg");
      this.subCamera.gameObject.transform.parent = this.duelCamera.gameObject.transform;
      this.subCamera.transform.localPosition = Vector3.zero;
      this.subCamera.transform.localRotation = Quaternion.identity;
      ((Behaviour) this.duelCamera.GetComponent<Camera>()).enabled = false;
      ((Behaviour) this.subCamera.GetComponent<Camera>()).enabled = false;
      this.isInitialized = true;
    }
  }

  public void Reset()
  {
    if (!Object.op_Inequality((Object) this.duelCamera, (Object) null))
      return;
    this.duelCamera.transform.parent = ((Component) this).transform;
  }

  public void ActAttackBeginCamera(DuelResult result, NGDuelUnit myUnit)
  {
    if (!result.isBossBattle)
    {
      if (result.isPlayerAttack)
      {
        if (result.distance > 1)
          this.StartCoroutine(this.changeCamera("attackBeginCam_magic"));
        else if (result.attackAttackStatus.isMagic)
          this.StartCoroutine(this.changeCamera("attackBeginCam_magic_r1"));
        else if (result.attack.job.move_type == UnitMoveType.hikou)
          this.StartCoroutine(this.changeCamera("attackBeginCam_fly"));
        else
          this.StartCoroutine(this.changeCamera("attackBeginCam_01"));
      }
      else
        this.StartCoroutine(this.changeCamera(""));
    }
    else if (result.isPlayerAttack)
      this.StartCoroutine(this.changeCamera("BossBattleAttackBeginCam_01"));
    else
      this.StartCoroutine(this.changeCamera("2PBossBattleAttackBeginCam_01"));
  }

  public void ActKoyuCamera(RuntimeAnimatorController rac)
  {
    this.koyuCamera.gameObject.SetActive(true);
    Camera componentInChildren = this.koyuCamera.GetComponentInChildren<Camera>();
    this.subCamera.gameObject.transform.parent = this.koyuCamera.transform;
    this.subCamera.transform.localPosition = ((Component) componentInChildren).gameObject.transform.localPosition;
    this.subCamera.transform.localRotation = ((Component) componentInChildren).gameObject.transform.localRotation;
    this.koyuCamera.GetComponent<Animator>().runtimeAnimatorController = rac;
    this.duelCamera.GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController) null;
    this.duelCamera.gameObject.SetActive(false);
  }

  public void EnableKoyuCamera()
  {
    ((Behaviour) this.koyuCamera.GetComponentInChildren<Camera>()).enabled = true;
  }

  public void ActEndCamera(RuntimeAnimatorController rac)
  {
  }

  public void ActCameraToMe(NGDuelUnit myUnit) => this.lookat = ((Component) myUnit).transform;

  public void ActCameraToCenter()
  {
    this.duelCamera.GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController) null;
    this.lookat = this.lookat_center;
  }

  public void ActCameraToAttacker() => this.lookat = this.lookat_attacker;

  public void EndKoyuCamera()
  {
    this.ActCameraToCenter();
    ((Component) this.koyuCamera.transform.GetChild(0)).gameObject.GetComponent<Camera>().fieldOfView = this.koyuCameraFiledOfView;
    this.koyuCamera.gameObject.SetActive(false);
    this.duelCamera.gameObject.SetActive(true);
    this.subCamera.gameObject.transform.parent = this.duelCamera.gameObject.transform;
    this.subCamera.transform.localPosition = Vector3.zero;
    this.subCamera.transform.localRotation = Quaternion.identity;
  }

  public IEnumerator changeCamera(string cameraName, Transform parent = null, bool animaterWait = false)
  {
    ((Behaviour) this.duelCamera.GetComponent<Camera>()).enabled = true;
    ((Behaviour) this.subCamera.GetComponent<Camera>()).enabled = true;
    ((Behaviour) ((Component) this.koyuCamera.transform.GetChild(0)).gameObject.GetComponent<Camera>()).enabled = true;
    this.koyuCamera.SetActive(false);
    if (cameraName == null || cameraName == "")
    {
      this.duelCamera.GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController) null;
    }
    else
    {
      Future<RuntimeAnimatorController> go = Singleton<NGDuelDataManager>.GetInstance().LoadDuelCamera(cameraName);
      IEnumerator e = go.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Animator component = this.duelCamera.GetComponent<Animator>();
      if (animaterWait)
        ((Behaviour) component).enabled = false;
      this.duelCamera.GetComponent<Animator>().runtimeAnimatorController = go.Result;
      if (Object.op_Inequality((Object) parent, (Object) null))
      {
        GameObject gameObject = new GameObject();
        this.duelCamera.transform.parent = gameObject.transform;
        gameObject.transform.parent = parent;
        gameObject.transform.localScale = Vector3.one;
        gameObject.transform.localRotation = DuelCameraManager.RotationWinAnimator;
        if (cameraName == "WinCam_r1")
          gameObject.transform.localPosition = DuelCameraManager.PositionWinCam_r1;
        if (cameraName == "WinCam_01")
          gameObject.transform.localPosition = DuelCameraManager.PositionWinCam_01;
      }
      if (cameraName.Contains("start") || cameraName.Contains("Start") || cameraName.Contains("Intro"))
        NGDuelManager.actScreen(false);
    }
  }

  public void SetAnimatorEnabled()
  {
    ((Behaviour) this.duelCamera.GetComponent<Animator>()).enabled = true;
  }

  public void updateLookatPosition(
    float distance,
    NGDuelUnit myUnit,
    NGDuelUnit enemyUnit,
    bool isPlayerAttack)
  {
    Vector3 position1 = this.lookat_center.position;
    position1.x = ((Component) myUnit).transform.position.x - distance / 2f;
    this.lookat_center.position = position1;
    Vector3 position2 = this.lookat_attacker.position;
    position2.x = ((Component) myUnit).transform.position.x - 1f;
    this.lookat_attacker.position = position2;
    this.lookat_defender.position.x = ((Component) enemyUnit).transform.position.x + 1f;
    this.lookat_defender.position = position2;
    Vector3 position3 = this.duelCamera.transform.position;
    if (Object.op_Equality((Object) this.lookat, (Object) this.lookat_attacker))
      this.duelCamera.transform.LookAt(this.lookat_attacker);
    else if (Object.op_Equality((Object) this.lookat, (Object) this.lookat_center))
    {
      Vector3 position4 = this.duelCamera.transform.position;
      float num = 3f * distance;
      if ((double) num < (double) this.MinNearCameraZ)
        num = this.MinNearCameraZ;
      else if ((double) num > (double) this.MaxFarCameraZ)
        num = this.MaxFarCameraZ;
      position3.z = num;
      position3.y = distance * 0.5f;
      if ((double) position3.y < 1.5)
        position3.y = 1.5f;
      position3.x = position1.x;
      if (isPlayerAttack)
      {
        if ((double) position3.x < -1.0)
          --position3.x;
      }
      else if ((double) position3.x > 1.0)
        ++position3.x;
      Vector3 zero = Vector3.zero;
      this.duelCamera.transform.position = Vector3.SmoothDamp(position4, position3, ref zero, this.smoothTime);
      this.duelCamera.transform.LookAt(((Component) this.lookat_center).transform.position);
    }
    else if (Object.op_Equality((Object) ((Component) myUnit).transform, (Object) this.lookat) && !this.camelaLastMove)
    {
      this.camelaLastMove = true;
      position3.x = -1f;
      position3.z = 1f;
      position3.y = 0.5f;
      if (Vector3.op_Inequality(this.duelCamera.transform.position, position3))
        this.duelCamera.transform.position = position3;
      this.duelCamera.transform.LookAt(((Component) myUnit).transform);
    }
    else if (Object.op_Equality((Object) this.lookat, (Object) this.lookat_bullet))
      this.duelCamera.transform.LookAt(this.lookat_bullet);
    this.duelCamera.GetComponent<Camera>().fieldOfView = 80f;
  }

  public void ShakeCamera() => this.StartCoroutine(this.coShakeCamera());

  public void InActiveKoyuCamera() => this.koyuCamera.SetActive(false);

  public void DisableKoyuCamera()
  {
    ((Behaviour) this.koyuCamera.GetComponentInChildren<Camera>()).enabled = false;
  }

  private IEnumerator coShakeCamera()
  {
    if (!Object.op_Equality((Object) null, (Object) this.duelCamera))
    {
      yield return (object) new WaitForSeconds(0.2f);
      Transform origTR = this.duelCamera.gameObject.transform;
      this.duelCamera.gameObject.transform.position = new Vector3(origTR.position.x + 0.3f, origTR.position.y + 0.3f, origTR.position.z);
      yield return (object) null;
      this.duelCamera.gameObject.transform.position = new Vector3(origTR.position.x - 0.3f, origTR.position.y - 0.3f, origTR.position.z);
      yield return (object) null;
      this.duelCamera.gameObject.transform.position = new Vector3(origTR.position.x + 0.2f, origTR.position.y - 0.2f, origTR.position.z);
      yield return (object) null;
      this.duelCamera.gameObject.transform.position = new Vector3(origTR.position.x - 0.2f, origTR.position.y + 0.2f, origTR.position.z);
      yield return (object) null;
      this.duelCamera.gameObject.transform.position = new Vector3(origTR.position.x + 0.3f, origTR.position.y + 0.3f, origTR.position.z);
      yield return (object) null;
      this.duelCamera.gameObject.transform.position = new Vector3(origTR.position.x - 0.3f, origTR.position.y - 0.3f, origTR.position.z);
      yield return (object) null;
      this.duelCamera.gameObject.transform.position = new Vector3(origTR.position.x + 0.2f, origTR.position.y - 0.2f, origTR.position.z);
      yield return (object) null;
      this.duelCamera.gameObject.transform.position = new Vector3(origTR.position.x - 0.2f, origTR.position.y + 0.2f, origTR.position.z);
      yield return (object) null;
      this.duelCamera.gameObject.transform.position = new Vector3(origTR.position.x + 0.1f, origTR.position.y + 0.1f, origTR.position.z);
      yield return (object) null;
      this.duelCamera.gameObject.transform.position = new Vector3(origTR.position.x - 0.1f, origTR.position.y - 0.1f, origTR.position.z);
      yield return (object) null;
      this.duelCamera.gameObject.transform.position = origTR.position;
    }
  }

  public void WarpCamera1stAttackPos(bool is_princess, Transform defender)
  {
    Animator component = this.duelCamera.GetComponent<Animator>();
    if (Object.op_Inequality((Object) null, (Object) component))
      component.runtimeAnimatorController = (RuntimeAnimatorController) null;
    this.duelCamera.gameObject.transform.position = !is_princess ? new Vector3(defender.position.x - 3.7f, 2.6f, 3.4f) : new Vector3(defender.position.x + 1f, 2.3f, 5f);
    this.lookat = this.lookat_attacker;
    this.duelCamera.gameObject.transform.LookAt(((Component) this.lookat).transform);
  }

  public IEnumerator WarpCamera1stAttackPosRemoteEnemy(
    Transform defender,
    float delay,
    Transform bullet)
  {
    yield return (object) new WaitForSeconds(delay);
    Animator component = this.duelCamera.GetComponent<Animator>();
    if (Object.op_Inequality((Object) null, (Object) component))
      component.runtimeAnimatorController = (RuntimeAnimatorController) null;
    this.duelCamera.gameObject.transform.position = new Vector3(defender.position.x - 3f, 2.3f, 2f);
    if (Object.op_Equality((Object) null, (Object) bullet))
    {
      this.lookat = this.lookat_attacker;
    }
    else
    {
      this.lookat_bullet = bullet;
      this.lookat = this.lookat_bullet;
    }
    this.duelCamera.gameObject.transform.LookAt(((Component) this.lookat).transform);
  }

  public void LookCameraBullet(Transform bullet) => this.lookat_bullet = bullet;
}
