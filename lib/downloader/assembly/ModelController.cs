// Decompiled with JetBrains decompiler
// Type: ModelController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ModelController : MonoBehaviour
{
  [HideInInspector]
  public Transform target;
  [HideInInspector]
  public bool unitWithAnimals;
  [HideInInspector]
  public float distance = 5f;
  public float targetOffsetY = -1.17f;
  public float targetOffsetYAnimals = -1.76f;
  public float maxDistance = 10f;
  public float minDistance = 2.3f;
  public float xSpeed = 350f;
  public float ySpeed = 350f;
  public int yMinLimit = -25;
  public int yMaxLimit = 80;
  public int zoomRate = 200;
  public float zoomDampening = 5f;
  public float startingZoom = 5f;
  private float xDeg;
  private float yDeg;
  private float currentDistance;
  private float desiredDistance;
  private Quaternion currentRotation;
  private Quaternion desiredRotation;
  private Quaternion rotation;
  private Vector3 position;
  private bool firstInit;
  private Vector3 lastMousePosition;
  private bool noRotation;

  public void Init()
  {
    if (!Object.op_Implicit((Object) this.target))
      return;
    if (this.unitWithAnimals)
      ((Component) this).transform.position = Vector3.op_Subtraction(this.target.position, Vector3.op_Addition(Vector3.op_Multiply(Vector3.forward, this.startingZoom), new Vector3(0.0f, this.targetOffsetYAnimals, 0.0f)));
    else
      ((Component) this).transform.position = Vector3.op_Subtraction(this.target.position, Vector3.op_Addition(Vector3.op_Multiply(Vector3.forward, this.startingZoom), new Vector3(0.0f, this.targetOffsetY, 0.0f)));
    this.target.rotation = Quaternion.Euler(0.0f, -180f, 0.0f);
    this.distance = Vector3.Distance(((Component) this).transform.position, this.target.position);
    this.currentDistance = this.distance;
    this.desiredDistance = this.distance;
    this.position = ((Component) this).transform.position;
    this.rotation = ((Component) this).transform.rotation;
    this.currentRotation = ((Component) this).transform.rotation;
    this.desiredRotation = ((Component) this).transform.rotation;
    this.xDeg = Vector3.Angle(Vector3.right, ((Component) this).transform.right);
    this.yDeg = Vector3.Angle(Vector3.up, ((Component) this).transform.up);
    this.firstInit = true;
  }

  private void LateUpdate()
  {
    if (!Object.op_Implicit((Object) this.target))
      return;
    if (!this.firstInit)
      this.Init();
    this.Rotate();
    this.Zoom();
    this.Move();
  }

  private void Rotate()
  {
    if (Input.GetMouseButtonDown(0))
    {
      RaycastHit raycastHit;
      if (Physics.Raycast(GameObject.Find("NGUI Camera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), ref raycastHit) && ((Component) ((RaycastHit) ref raycastHit).transform).gameObject.layer == LayerMask.NameToLayer("2DUnity"))
      {
        this.noRotation = true;
        return;
      }
      this.desiredRotation = ((Component) this).transform.rotation;
      this.lastMousePosition = Input.mousePosition;
    }
    if (Input.GetMouseButton(0) && !this.noRotation)
    {
      if (Vector3.op_Inequality(this.lastMousePosition, Input.mousePosition))
      {
        Vector3 vector3 = Vector3.op_Subtraction(Input.mousePosition, this.lastMousePosition);
        float num = ((Vector3) ref vector3).magnitude / Time.deltaTime;
        this.xDeg += (float) (((double) Input.mousePosition.x - (double) this.lastMousePosition.x) * (double) this.xSpeed * (double) Time.deltaTime * (double) num * 9.9999997473787516E-06);
        this.yDeg -= (float) (((double) Input.mousePosition.y - (double) this.lastMousePosition.y) * (double) this.ySpeed * (double) Time.deltaTime * (double) num * 9.9999997473787516E-06);
        this.yDeg = ModelController.ClampAngle(this.yDeg, (float) this.yMinLimit, (float) this.yMaxLimit);
        this.desiredRotation = Quaternion.Euler(this.yDeg, this.xDeg, 0.0f);
        this.currentRotation = ((Component) this).transform.rotation;
        this.rotation = Quaternion.Lerp(this.currentRotation, this.desiredRotation, Time.deltaTime * this.zoomDampening);
        ((Component) this).transform.rotation = this.rotation;
      }
      this.lastMousePosition = Input.mousePosition;
    }
    if (Input.GetMouseButtonUp(0) && this.noRotation)
      this.noRotation = false;
    if (!Quaternion.op_Inequality(((Component) this).transform.rotation, this.desiredRotation))
      return;
    this.rotation = Quaternion.Lerp(((Component) this).transform.rotation, this.desiredRotation, Time.deltaTime * this.zoomDampening);
    ((Component) this).transform.rotation = this.rotation;
  }

  private void Zoom()
  {
    float num = 0.0f;
    if ((double) Input.GetAxis("Mouse ScrollWheel") > 0.0)
      num = -100f;
    else if ((double) Input.GetAxis("Mouse ScrollWheel") < 0.0)
      num = 100f;
    this.desiredDistance += (float) ((double) num * (double) Time.deltaTime * (double) this.zoomRate * (double) Mathf.Abs(this.desiredDistance) * (1.0 / 1000.0));
    this.desiredDistance = Mathf.Clamp(this.desiredDistance, this.minDistance, this.maxDistance);
    this.currentDistance = Mathf.Lerp(this.currentDistance, this.desiredDistance, Time.deltaTime * this.zoomDampening);
  }

  private void Move()
  {
    this.position = !this.unitWithAnimals ? Vector3.op_Subtraction(this.target.position, Vector3.op_Addition(Vector3.op_Multiply(Quaternion.op_Multiply(this.rotation, Vector3.forward), this.currentDistance), new Vector3(0.0f, this.targetOffsetY, 0.0f))) : Vector3.op_Subtraction(this.target.position, Vector3.op_Addition(Vector3.op_Multiply(Quaternion.op_Multiply(this.rotation, Vector3.forward), this.currentDistance), new Vector3(0.0f, this.targetOffsetYAnimals, 0.0f)));
    ((Component) this).transform.position = this.position;
  }

  private static float ClampAngle(float angle, float min, float max)
  {
    if ((double) angle < -360.0)
      angle += 360f;
    if ((double) angle > 360.0)
      angle -= 360f;
    return Mathf.Clamp(angle, min, max);
  }
}
