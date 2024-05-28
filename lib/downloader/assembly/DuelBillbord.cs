// Decompiled with JetBrains decompiler
// Type: DuelBillbord
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DuelBillbord : MonoBehaviour
{
  private Vector3 savePos;
  private Vector3 saveCameraPos;
  private Quaternion saveCameraRot;
  private NGDuelManager mng;

  private void Start()
  {
    this.savePos = Vector3.zero;
    this.saveCameraPos = Vector3.zero;
    this.saveCameraRot = Quaternion.identity;
    GameObject gameObject = GameObject.Find("Duel3DRoot");
    if (!Object.op_Inequality((Object) null, (Object) gameObject))
      return;
    this.mng = gameObject.GetComponent<NGDuelManager>();
    this.calc(this.mng.currentCamera);
  }

  private void Update()
  {
    if (Object.op_Equality((Object) null, (Object) this.mng) || Object.op_Equality((Object) null, (Object) this.mng.currentCamera))
      return;
    Transform transform = this.mng.currentCamera.transform;
    if (!Vector3.op_Inequality(this.savePos, ((Component) this).transform.position) && !Vector3.op_Inequality(this.saveCameraPos, transform.position) && !Quaternion.op_Inequality(this.saveCameraRot, transform.rotation))
      return;
    this.calc(this.mng.currentCamera);
  }

  private void calc(GameObject co)
  {
    if (Object.op_Equality((Object) co, (Object) null))
      return;
    Vector3 vector3 = Vector3.back;
    if (((Object) co).name.Contains("maya"))
      vector3 = Vector3.forward;
    Camera[] componentsInChildren = co.GetComponentsInChildren<Camera>(true);
    if (componentsInChildren.Length == 0)
      return;
    Camera camera = componentsInChildren[0];
    Transform transform = co.transform;
    Vector3 forward = transform.forward;
    ((Vector3) ref forward).Normalize();
    Vector3 position = ((Component) this).transform.position;
    ((Component) this).transform.position = Vector3.op_Addition(transform.position, Vector3.op_Multiply(forward, camera.nearClipPlane + 1f / 1000f));
    ((Component) this).transform.LookAt(Vector3.op_Addition(((Component) this).transform.position, Quaternion.op_Multiply(transform.rotation, vector3)), Quaternion.op_Multiply(transform.rotation, Vector3.up));
    ((Component) this).transform.position = position;
    this.saveCameraRot = transform.rotation;
    this.saveCameraPos = transform.position;
    this.savePos = position;
  }
}
