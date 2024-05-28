// Decompiled with JetBrains decompiler
// Type: NGxScroll2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class NGxScroll2 : MonoBehaviour
{
  public UIScrollView scrollView;
  public int topSpace = 2;
  public int bottomSpace = 2;
  private const int DEFAULT_ICONS_PER_LINE = 5;
  private int ColumnSize;
  private int iconHeight;
  [SerializeField]
  [Tooltip("scrollView がScrollViewSpecifyBounds の時、スクロールビュー可動範囲計算にTop, Bottomしか見ない")]
  private bool isSpecalMode;
  private List<GameObject> arr = new List<GameObject>();
  private GameObject Top;
  private GameObject Bottom;

  private int TopSpace => (Mathf.Max(2, this.topSpace) + 1) / 2 * 2;

  private int BottomSpace => (Mathf.Max(2, this.bottomSpace) + 1) / 2 * 2;

  public GameObject TopObject => this.Top;

  public GameObject BottomObject => this.Bottom;

  public IEnumerator<GameObject> GetEnumerator()
  {
    return (IEnumerator<GameObject>) this.arr.GetEnumerator();
  }

  private void Awake()
  {
    this.scrollView.contentPivot = (UIWidget.Pivot) 0;
    this.scrollView.disableDragIfFits = true;
    this.Top = (GameObject) null;
    this.Bottom = (GameObject) null;
  }

  private void Start()
  {
    foreach (Transform child in ((Component) this.scrollView.verticalScrollBar).transform.GetChildren())
    {
      if (Object.op_Inequality((Object) ((Component) child).GetComponent<Collider>(), (Object) null))
        ((Component) child).GetComponent<Collider>().enabled = true;
    }
  }

  public IEnumerable<GameObject> GridChildren()
  {
    return ((Component) this.scrollView).transform.GetChildren().Select<Transform, GameObject>((Func<Transform, GameObject>) (t => ((Component) t).gameObject));
  }

  public void Clear()
  {
    if (Object.op_Inequality((Object) this.Top, (Object) null))
      this.Top = (GameObject) null;
    if (Object.op_Inequality((Object) this.Bottom, (Object) null))
      this.Bottom = (GameObject) null;
    this.arr.Clear();
    ((Component) this.scrollView).transform.Clear();
  }

  public void Add(GameObject obj, int width, int height, bool ignoreResizeCollider = false)
  {
    this.iconHeight = height;
    this.ColumnSize = 5;
    obj.transform.parent = ((Component) this.scrollView).transform;
    int num = ((Component) this.scrollView).transform.childCount - 1;
    obj.transform.localPosition = new Vector3((float) (num % this.ColumnSize * width - width * 2), (float) -(num / this.ColumnSize * height), 0.0f);
    obj.transform.localScale = Vector3.one;
    this.arr.Add(obj);
    if (ignoreResizeCollider)
      return;
    BoxCollider componentInChildren = obj.GetComponentInChildren<BoxCollider>();
    if (!Object.op_Inequality((Object) componentInChildren, (Object) null))
      return;
    componentInChildren.size = new Vector3((float) width, (float) height);
  }

  public void AddColumn3(GameObject obj, int width, int height, int startIndex)
  {
    this.iconHeight = height;
    this.ColumnSize = 3;
    obj.transform.parent = ((Component) this.scrollView).transform;
    int num1 = startIndex / this.ColumnSize;
    float num2 = (float) ((startIndex - 1 - num1 * this.ColumnSize) * width);
    float num3 = (float) (-num1 * height);
    Vector3 vector3;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3).\u002Ector(num2, num3, 0.0f);
    obj.transform.localPosition = vector3;
    obj.transform.localScale = Vector3.one;
    this.arr.Add(obj);
    BoxCollider componentInChildren = obj.GetComponentInChildren<BoxCollider>();
    if (!Object.op_Inequality((Object) componentInChildren, (Object) null))
      return;
    componentInChildren.size = new Vector3((float) width, (float) height);
  }

  public Vector3 Add(
    GameObject obj,
    int width,
    int height,
    int startIndex,
    bool ignoreResizeCollider = false)
  {
    this.iconHeight = height;
    this.ColumnSize = 5;
    obj.transform.parent = ((Component) this.scrollView).transform;
    Vector3 vector3 = this.CalcLocalPosition(width, height, startIndex);
    obj.transform.localPosition = vector3;
    obj.transform.localScale = Vector3.one;
    this.arr.Add(obj);
    if (ignoreResizeCollider)
      return vector3;
    BoxCollider componentInChildren = obj.GetComponentInChildren<BoxCollider>();
    if (Object.op_Inequality((Object) componentInChildren, (Object) null))
      componentInChildren.size = new Vector3((float) width, (float) height);
    return vector3;
  }

  public Vector3 CalcLocalPosition(int width, int height, int startIndex)
  {
    return new Vector3((float) (startIndex % this.ColumnSize * width - width * 2), (float) -(startIndex / this.ColumnSize * height), 0.0f);
  }

  public void AddColumn1(GameObject obj, int width, int height, bool ignoreResizeCollider = false)
  {
    this.ColumnSize = 1;
    this.iconHeight = height;
    obj.transform.parent = ((Component) this.scrollView).transform;
    int num = ((Component) this.scrollView).transform.childCount - 1;
    obj.transform.localPosition = new Vector3(0.0f, (float) -(num * height), 0.0f);
    obj.transform.localScale = Vector3.one;
    this.arr.Add(obj);
    if (ignoreResizeCollider)
      return;
    BoxCollider componentInChildren = obj.GetComponentInChildren<BoxCollider>();
    if (!Object.op_Inequality((Object) componentInChildren, (Object) null))
      return;
    componentInChildren.size = new Vector3((float) width, (float) height);
  }

  public void ResetScrollRange(int width, int height, int numObject)
  {
    this.ColumnSize = 5;
    Vector3 vector3_1;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3_1).\u002Ector(float.MaxValue, float.MaxValue, 0.0f);
    Vector3 vector3_2;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3_2).\u002Ector(float.MinValue, float.MinValue, 0.0f);
    Vector3 vector3_3 = this.CalcLocalPosition(width, height, 0);
    Vector3 vector3_4 = Vector3.Min(vector3_3, vector3_1);
    Vector3 vector3_5 = Vector3.Max(vector3_3, vector3_2);
    Vector3 vector3_6 = this.CalcLocalPosition(width, height, this.ColumnSize - 1);
    Vector3 vector3_7 = Vector3.Min(vector3_6, vector3_4);
    Vector3 vector3_8 = Vector3.Max(vector3_6, vector3_5);
    if (numObject > this.ColumnSize)
    {
      Vector3 vector3_9 = this.CalcLocalPosition(width, height, numObject - 1);
      vector3_7 = Vector3.Min(vector3_9, vector3_7);
      vector3_8 = Vector3.Max(vector3_9, vector3_8);
    }
    float num1 = (float) width / 2f;
    float num2 = (float) height / 2f;
    vector3_7.x -= num1;
    vector3_7.y -= num2;
    vector3_8.x += num1;
    vector3_8.y += num2;
    this.resetTopObject(new Vector3(vector3_7.x, vector3_8.y, 0.0f));
    this.resetBottomObject(new Vector3(vector3_8.x, vector3_7.y, 0.0f));
    this.setSpecialMode();
  }

  private void resetTopObject(Vector3 pos)
  {
    if (Object.op_Equality((Object) this.Top, (Object) null))
    {
      this.Top = new GameObject("Top");
      this.Top.layer = ((Component) this.scrollView).gameObject.layer;
      this.Top.transform.parent = ((Component) this.scrollView).transform;
      this.Top.AddComponent<UIWidget>().SetDimensions(2, this.TopSpace);
    }
    this.Top.transform.localPosition = pos;
    this.Top.transform.localScale = Vector3.one;
    this.Top.SetActive(true);
  }

  private void resetBottomObject(Vector3 pos)
  {
    if (Object.op_Equality((Object) this.Bottom, (Object) null))
    {
      this.Bottom = new GameObject("Bottom");
      this.Bottom.layer = ((Component) this.scrollView).gameObject.layer;
      this.Bottom.transform.parent = ((Component) this.scrollView).transform;
      this.Bottom.AddComponent<UIWidget>().SetDimensions(2, this.BottomSpace);
    }
    this.Bottom.transform.localPosition = pos;
    this.Bottom.transform.localScale = Vector3.one;
    this.Bottom.SetActive(true);
  }

  private void clearSpecialMode()
  {
    if (!this.isSpecalMode)
      return;
    ScrollViewSpecifyBounds scrollView = this.scrollView as ScrollViewSpecifyBounds;
    if (!Object.op_Inequality((Object) scrollView, (Object) null))
      return;
    scrollView.ClearBounds();
  }

  private void setSpecialMode()
  {
    if (!this.isSpecalMode)
      return;
    ScrollViewSpecifyBounds scrollView = this.scrollView as ScrollViewSpecifyBounds;
    if (Object.op_Equality((Object) scrollView, (Object) null))
      return;
    scrollView.ClearBounds();
    scrollView.AddBound(this.Top.GetComponent<UIWidget>());
    scrollView.AddBound(this.Bottom.GetComponent<UIWidget>());
  }

  public void CreateScrollPoint(int height, int count)
  {
    if (Object.op_Equality((Object) this.Top, (Object) null))
    {
      this.Top = new GameObject("Top");
      this.Top.layer = ((Component) this.scrollView).gameObject.layer;
      this.Top.transform.parent = ((Component) this.scrollView).transform;
      UIWidget uiWidget = this.Top.AddComponent<UIWidget>();
      uiWidget.SetDimensions(2, this.TopSpace);
      uiWidget.panel = this.scrollView.panel;
    }
    this.Top.transform.localPosition = new Vector3(0.0f, (float) (height / 2), 0.0f);
    this.Top.transform.localScale = Vector3.one;
    this.Top.SetActive(true);
    if (Object.op_Equality((Object) this.Bottom, (Object) null))
    {
      this.Bottom = new GameObject("Bottom");
      this.Bottom.layer = ((Component) this.scrollView).gameObject.layer;
      this.Bottom.transform.parent = ((Component) this.scrollView).transform;
      UIWidget uiWidget = this.Bottom.AddComponent<UIWidget>();
      uiWidget.SetDimensions(2, this.BottomSpace);
      uiWidget.panel = this.scrollView.panel;
    }
    this.Bottom.transform.parent = ((Component) this.scrollView).transform;
    this.Bottom.transform.localPosition = new Vector3(0.0f, (float) (-(Mathf.Max(0, count - 1) / 5 * height) - height / 2), 0.0f);
    this.Bottom.transform.localScale = Vector3.one;
    this.Bottom.SetActive(true);
  }

  public void CreateScrollPointHeight(int height, int count)
  {
    if (Object.op_Equality((Object) this.Top, (Object) null))
    {
      this.Top = new GameObject("Top");
      this.Top.layer = ((Component) this.scrollView).gameObject.layer;
      this.Top.transform.parent = ((Component) this.scrollView).transform;
      UIWidget uiWidget = this.Top.AddComponent<UIWidget>();
      uiWidget.SetDimensions(2, this.TopSpace);
      uiWidget.panel = this.scrollView.panel;
    }
    this.Top.transform.localPosition = new Vector3(0.0f, (float) (height / 2), 0.0f);
    this.Top.transform.localScale = Vector3.one;
    this.Top.SetActive(true);
    if (Object.op_Equality((Object) this.Bottom, (Object) null))
    {
      this.Bottom = new GameObject("Bottom");
      this.Bottom.layer = ((Component) this.scrollView).gameObject.layer;
      this.Bottom.transform.parent = ((Component) this.scrollView).transform;
      UIWidget uiWidget = this.Bottom.AddComponent<UIWidget>();
      uiWidget.SetDimensions(2, this.BottomSpace);
      uiWidget.panel = this.scrollView.panel;
    }
    this.Bottom.transform.localPosition = new Vector3(0.0f, (float) (-((count - 1) * height) - height / 2), 0.0f);
    this.Bottom.transform.localScale = Vector3.one;
    this.Bottom.SetActive(true);
  }

  public void ResolvePosition(Vector2 pos)
  {
    this.scrollView.ResetPosition();
    this.scrollView.SetDragAmount(pos.x, pos.y, false);
    this.scrollView.SetDragAmount(pos.x, pos.y, true);
  }

  public void ResolvePosition(int index, int iconCount)
  {
    this.scrollView.ResetPosition();
    double height = (double) this.scrollView.panel.height;
    Bounds bounds1 = this.scrollView.bounds;
    double y = (double) ((Bounds) ref bounds1).size.y;
    if (height > y)
      return;
    int num1 = this.ColumnSize;
    if (num1 < 1)
      num1 = 1;
    float num2 = (float) (iconCount / num1);
    if ((double) num2 > 0.0)
      ++num2;
    if ((double) num2 < 1.0)
      ;
    Bounds bounds2 = this.scrollView.bounds;
    float num3 = Mathf.Abs(((Bounds) ref bounds2).size.y - this.scrollView.panel.height);
    float num4 = (float) this.iconHeight * (float) (index / num1) / num3;
    if ((double) num4 < 0.0)
      num4 = 0.0f;
    else if ((double) num4 > 1.0)
      num4 = 1f;
    this.ResolvePosition(new Vector2(0.0f, num4));
  }

  public void ResolvePosition() => this.scrollView.ResetPosition();

  public void ResolvePositionFromScrollValue(float pos)
  {
    this.scrollView.ResetPosition();
    double height = (double) this.scrollView.panel.height;
    Bounds bounds1 = this.scrollView.bounds;
    double y = (double) ((Bounds) ref bounds1).size.y;
    if (height > y)
      return;
    Bounds bounds2 = this.scrollView.bounds;
    float num1 = Mathf.Abs(((Bounds) ref bounds2).size.y - this.scrollView.panel.height);
    float num2 = (float) this.iconHeight / 2f / num1;
    float num3 = this.scrollView.panel.height / 2f / num1;
    float num4 = (float) ((double) pos / (double) num1 - ((double) num3 - (double) num2));
    if ((double) num4 < 0.0)
      num4 = 0.0f;
    else if ((double) num4 > 1.0)
      num4 = 1f;
    this.ResolvePosition(new Vector2(0.0f, num4));
  }

  public void Reset()
  {
    this.clearSpecialMode();
    if (Object.op_Inequality((Object) this.Top, (Object) null))
    {
      Object.Destroy((Object) this.Top);
      this.Top = (GameObject) null;
    }
    if (Object.op_Inequality((Object) this.Bottom, (Object) null))
    {
      Object.Destroy((Object) this.Bottom);
      this.Bottom = (GameObject) null;
    }
    ((Component) this.scrollView).transform.DetachChildren();
    this.arr.Clear();
  }
}
