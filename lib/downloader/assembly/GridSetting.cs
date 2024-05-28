// Decompiled with JetBrains decompiler
// Type: GridSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class GridSetting
{
  [Tooltip("単位幅")]
  public int width = 123;
  [Tooltip("単位高さ")]
  public int height = 147;
  [Tooltip("列数")]
  public int column = 5;
  [Tooltip("オブジェクト生成行数")]
  public int rowInstance = 8;
  [Tooltip("みなし表示行数")]
  public int rowScreen = 5;

  public int quantityScreen => this.column * this.rowScreen;

  public int quantityInstance => this.column * this.rowInstance;

  public Tuple<GameObject, GameObject> setScrollRange(
    Transform parent,
    string topName,
    string bottomName,
    int numObject)
  {
    Vector3 vector3_1;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3_1).\u002Ector(float.MaxValue, float.MaxValue, 0.0f);
    Vector3 vector3_2;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3_2).\u002Ector(float.MinValue, float.MinValue, 0.0f);
    Vector3 vector3_3 = this.calcLocalPosition(0);
    Vector3 vector3_4 = Vector3.Min(vector3_3, vector3_1);
    Vector3 vector3_5 = Vector3.Max(vector3_3, vector3_2);
    Vector3 vector3_6 = this.calcLocalPosition(this.column - 1);
    Vector3 vector3_7 = Vector3.Min(vector3_6, vector3_4);
    Vector3 vector3_8 = Vector3.Max(vector3_6, vector3_5);
    if (numObject > this.column)
    {
      Vector3 vector3_9 = this.calcLocalPosition(numObject - 1);
      vector3_7 = Vector3.Min(vector3_9, vector3_7);
      vector3_8 = Vector3.Max(vector3_9, vector3_8);
    }
    float num1 = (float) this.width / 2f;
    float num2 = (float) this.height / 2f;
    vector3_7.x -= num1;
    vector3_7.y -= num2;
    vector3_8.x += num1;
    vector3_8.y += num2;
    return Tuple.Create<GameObject, GameObject>(this.createWidgetObject(parent, topName, new Vector3(vector3_7.x, vector3_8.y, 0.0f), (UIWidget.Pivot) 0), this.createWidgetObject(parent, bottomName, new Vector3(vector3_8.x, vector3_7.y, 0.0f), (UIWidget.Pivot) 8));
  }

  private GameObject createWidgetObject(
    Transform parent,
    string objName,
    Vector3 pos,
    UIWidget.Pivot pivot)
  {
    Transform orCreateChild = SA_Extensions_Transform.FindOrCreateChild(parent, objName);
    ((Component) orCreateChild).gameObject.layer = ((Component) parent).gameObject.layer;
    UIWidget orAddComponent = ((Component) orCreateChild).gameObject.GetOrAddComponent<UIWidget>();
    orAddComponent.pivot = pivot;
    orAddComponent.SetDimensions(2, 2);
    orCreateChild.localPosition = pos;
    return ((Component) orCreateChild).gameObject;
  }

  public Vector3 calcLocalPosition(int index)
  {
    return new Vector3((float) (index % this.column * this.width - this.width * 2), (float) -(index / this.column * this.height), 0.0f);
  }
}
