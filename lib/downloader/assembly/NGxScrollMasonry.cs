// Decompiled with JetBrains decompiler
// Type: NGxScrollMasonry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class NGxScrollMasonry : MonoBehaviour
{
  [SerializeField]
  public UIScrollView Scroll;
  public List<GameObject> Arr = new List<GameObject>();
  private List<GameObject> nArr = new List<GameObject>();

  private void Awake()
  {
    this.Scroll.contentPivot = (UIWidget.Pivot) 0;
    this.Scroll.disableDragIfFits = true;
  }

  public void Add(GameObject obj, bool resize = false)
  {
    if (!resize)
    {
      obj.transform.parent = ((Component) this.Scroll).transform;
      obj.transform.localPosition = this.CalcLocalPosition(obj);
      obj.transform.localScale = Vector3.one;
      this.Arr.Add(obj);
    }
    else
    {
      obj.transform.localPosition = this.CalcLocalPosition(obj, true);
      this.nArr.Add(obj);
    }
    this.SetCollider(obj);
  }

  public void Insert(int index, GameObject obj, bool resize = false)
  {
    if (!resize)
    {
      obj.transform.parent = ((Component) this.Scroll).transform;
      obj.transform.localPosition = this.CalcLocalPosition(obj);
      obj.transform.localScale = Vector3.one;
      this.Arr.Insert(index, obj);
    }
    else
    {
      obj.transform.localPosition = this.CalcLocalPosition(obj, true);
      this.nArr.Insert(index, obj);
    }
    this.SetCollider(obj);
  }

  private Vector3 CalcLocalPosition(GameObject go, bool resize = false)
  {
    float num = resize ? (float) this.GetOffsetTop(this.nArr) : (float) this.GetOffsetTop(this.Arr);
    UIWidget component = go.GetComponent<UIWidget>();
    switch ((int) component.pivot)
    {
      case 3:
      case 4:
      case 5:
        num -= (float) (component.height / 2);
        break;
      case 6:
      case 7:
      case 8:
        num -= (float) component.height;
        break;
    }
    return new Vector3(0.0f, num, 0.0f);
  }

  private void SetCollider(GameObject go)
  {
    UIWidget component1 = go.GetComponent<UIWidget>();
    BoxCollider component2 = go.GetComponent<BoxCollider>();
    if (Object.op_Inequality((Object) component1, (Object) null) && Object.op_Inequality((Object) component2, (Object) null))
    {
      Vector4 drawingDimensions = component1.drawingDimensions;
      component2.center = new Vector3((float) (((double) drawingDimensions.x + (double) drawingDimensions.z) * 0.5), (float) (((double) drawingDimensions.y + (double) drawingDimensions.w) * 0.5));
      component2.size = new Vector3(drawingDimensions.z - drawingDimensions.x, drawingDimensions.w - drawingDimensions.y);
    }
    else
      Debug.LogError((object) "UIWidget or collider is none");
  }

  private int GetOffsetTop(List<GameObject> list)
  {
    return list.Sum<GameObject>((Func<GameObject, int>) (go => go.GetComponent<UIWidget>().height)) * -1;
  }

  public void ResolvePosition() => this.Scroll.ResetPosition();

  public void ResolvePosition(int index)
  {
    if (this.Arr.Count <= 0)
      return;
    index = this.Arr.Count > index ? index : 0;
    float num1 = 0.0f;
    foreach (GameObject gameObject in this.Arr)
      num1 += (float) gameObject.GetComponent<UIWidget>().height;
    Bounds bounds = this.Scroll.bounds;
    float num2 = ((Bounds) ref bounds).size.y - this.Scroll.panel.height;
    float num3 = (float) ((double) this.Arr[index].transform.localPosition.y * -1.0 - (double) num2 / 2.0) / num1;
    if ((double) num3 < 0.0 || index == 0)
      num3 = 0.0f;
    else if ((double) num3 > 1.0)
      num3 = 1f;
    this.ResolvePosition();
    this.Scroll.SetDragAmount(0.0f, num3, false);
    this.Scroll.SetDragAmount(0.0f, num3, true);
  }

  public void Reset()
  {
    ((Component) this.Scroll).transform.DetachChildren();
    this.Arr.Clear();
  }

  public void Resize()
  {
    this.nArr.Clear();
    foreach (GameObject gameObject in this.Arr)
    {
      if (gameObject.activeSelf)
        this.Add(gameObject, true);
    }
  }
}
