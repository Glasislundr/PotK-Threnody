// Decompiled with JetBrains decompiler
// Type: PanelInit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PanelInit : MonoBehaviour
{
  private Vector3[] vertex;
  private BoxTransform bxT;
  public float mPanelHeight;
  public float panelFloat;

  public float panelHeight => this.mPanelHeight * ((Component) this).transform.localScale.y;

  public float panelHeightNonScale => this.mPanelHeight;

  private void Awake()
  {
    this.bxT = ((Component) this).GetComponent<BoxTransform>();
    this.mPanelHeight = 0.0f;
  }

  private void Update()
  {
  }

  private Vector3 initVertex(PanelInit.VertexKind kind)
  {
    Vector3 vector3 = new Vector3();
    vector3.y = 0.0f;
    switch (kind)
    {
      case PanelInit.VertexKind.RightUp:
        vector3.x = 0.5f;
        vector3.z = 0.5f;
        break;
      case PanelInit.VertexKind.LeftUp:
        vector3.x = -0.5f;
        vector3.z = 0.5f;
        break;
      case PanelInit.VertexKind.LeftDown:
        vector3.x = -0.5f;
        vector3.z = -0.5f;
        break;
      case PanelInit.VertexKind.RightDown:
        vector3.x = 0.5f;
        vector3.z = -0.5f;
        break;
    }
    return vector3;
  }

  public void Init()
  {
    float x = ((Component) this).transform.localScale.x;
    float num1 = 0.0f;
    Matrix4x4 identity1 = Matrix4x4.identity;
    identity1.m00 = x;
    identity1.m11 = x;
    identity1.m22 = x;
    Matrix4x4 identity2 = Matrix4x4.identity;
    identity2.m00 = 1f / x;
    identity2.m11 = 1f / x;
    identity2.m22 = 1f / x;
    for (int index = 0; index < 4; ++index)
    {
      Vector3 vector3_1 = this.initVertex((PanelInit.VertexKind) index);
      Vector3 vector3_2 = ((Matrix4x4) ref identity1).MultiplyPoint3x4(vector3_1);
      vector3_2 = Vector3.op_Addition(vector3_2, ((Component) this).transform.position);
      vector3_2.y += 50f;
      int num2 = 1 << LayerMask.NameToLayer("Terrain");
      RaycastHit raycastHit;
      if (Physics.Raycast(vector3_2, Vector3.down, ref raycastHit, 100f, num2))
      {
        Vector3 vector3_3 = Vector3.op_Subtraction(((RaycastHit) ref raycastHit).point, ((Component) this).transform.position);
        Vector3 pos = ((Matrix4x4) ref identity2).MultiplyPoint3x4(vector3_3);
        num1 += pos.y;
        this.SetSquare((PanelInit.VertexKind) index, pos);
      }
      else
      {
        Vector3 vector3_4 = Vector3.op_Subtraction(vector3_2, ((Component) this).transform.position);
        Vector3 pos = ((Matrix4x4) ref identity2).MultiplyPoint3x4(vector3_4);
        pos.y = 0.0f;
        this.SetSquare((PanelInit.VertexKind) index, pos);
      }
    }
    float num3 = num1 / 4f;
    Vector3 vector3_5;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3_5).\u002Ector(((Component) this).transform.position.x, 0.0f, ((Component) this).transform.position.z);
    ((Component) this).transform.position = vector3_5;
    this.mPanelHeight = num3;
    this.bxT.SetMesh();
    BoxCollider component = ((Component) this).GetComponent<BoxCollider>();
    Vector3 vector3_6;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3_6).\u002Ector(component.center.x, this.mPanelHeight, component.center.z);
    component.center = vector3_6;
  }

  private void SetSquare(PanelInit.VertexKind iType, Vector3 pos)
  {
    pos.y += this.panelFloat;
    this.bxT.SetVertices((int) iType, pos);
  }

  public enum VertexKind
  {
    RightUp,
    LeftUp,
    LeftDown,
    RightDown,
    Max,
  }
}
