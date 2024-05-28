// Decompiled with JetBrains decompiler
// Type: UIGridCycle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class UIGridCycle : MonoBehaviour
{
  public int gridSize = 100;
  public bool disableContent = true;
  public int minIndex;
  public int maxIndex;
  public bool hideInactive;
  public UIGridCycle.OnInitializeItem onInitializeItem;
  protected Transform trans;
  protected UIPanel panel;
  protected UIScrollView scroll;
  protected bool horizontal;
  protected bool firstTime = true;
  protected List<Transform> childrenList = new List<Transform>();

  protected virtual void Start()
  {
    this.SortBasedOnScrollMovement();
    this.WrapContent();
    if (Object.op_Inequality((Object) this.scroll, (Object) null))
    {
      UIPanel component = ((Component) this.scroll).GetComponent<UIPanel>();
      UIGridCycle uiGridCycle = this;
      // ISSUE: virtual method pointer
      UIPanel.OnClippingMoved onClippingMoved = new UIPanel.OnClippingMoved((object) uiGridCycle, __vmethodptr(uiGridCycle, OnMove));
      component.onClipMove = onClippingMoved;
    }
    this.firstTime = false;
  }

  protected virtual void OnMove(UIPanel panel) => this.WrapContent();

  [ContextMenu("Sort Based on Scroll Movement")]
  public virtual void SortBasedOnScrollMovement()
  {
    if (!this.CacheScrollView())
      return;
    this.childrenList.Clear();
    for (int index = 0; index < this.trans.childCount; ++index)
    {
      Transform child = this.trans.GetChild(index);
      if (!this.hideInactive || ((Component) child).gameObject.activeInHierarchy)
        this.childrenList.Add(child);
    }
    if (this.horizontal)
      this.childrenList.Sort(new Comparison<Transform>(UIGrid.SortHorizontal));
    else
      this.childrenList.Sort(new Comparison<Transform>(UIGrid.SortVertical));
    this.ResetChildPositions();
  }

  protected bool CacheScrollView()
  {
    this.trans = ((Component) this).transform;
    this.panel = NGUITools.FindInParents<UIPanel>(((Component) this).gameObject);
    this.scroll = ((Component) this.panel).GetComponent<UIScrollView>();
    if (Object.op_Equality((Object) this.scroll, (Object) null))
      return false;
    if (this.scroll.movement == null)
    {
      this.horizontal = true;
    }
    else
    {
      if (this.scroll.movement != 1)
        return false;
      this.horizontal = false;
    }
    return true;
  }

  protected virtual void ResetChildPositions()
  {
    int index = 0;
    for (int count = this.childrenList.Count; index < count; ++index)
    {
      Transform children = this.childrenList[index];
      children.localPosition = this.horizontal ? new Vector3((float) (index * this.gridSize), 0.0f, 0.0f) : new Vector3(0.0f, (float) (-index * this.gridSize), 0.0f);
      this.UpdateItem(children, index);
    }
  }

  public virtual void WrapContent()
  {
    float num1 = (float) (this.gridSize * this.childrenList.Count) * 0.5f;
    Vector3[] worldCorners = ((UIRect) this.panel).worldCorners;
    for (int index = 0; index < 4; ++index)
    {
      Vector3 vector3 = this.trans.InverseTransformPoint(worldCorners[index]);
      worldCorners[index] = vector3;
    }
    Vector3 vector3_1 = Vector3.Lerp(worldCorners[0], worldCorners[2], 0.5f);
    bool flag = true;
    float num2 = num1 * 2f;
    if (this.horizontal)
    {
      float num3 = worldCorners[0].x - (float) this.gridSize;
      float num4 = worldCorners[2].x + (float) this.gridSize;
      int index = 0;
      for (int count = this.childrenList.Count; index < count; ++index)
      {
        Transform children = this.childrenList[index];
        float num5 = children.localPosition.x - vector3_1.x;
        if ((double) num5 < -(double) num1)
        {
          Vector3 localPosition = children.localPosition;
          localPosition.x += num2;
          num5 = localPosition.x - vector3_1.x;
          int num6 = Mathf.RoundToInt(localPosition.x / (float) this.gridSize);
          if (this.minIndex == this.maxIndex || this.minIndex <= num6 && num6 <= this.maxIndex)
          {
            children.localPosition = localPosition;
            this.UpdateItem(children, index);
          }
          else
            flag = false;
        }
        else if ((double) num5 > (double) num1)
        {
          Vector3 localPosition = children.localPosition;
          localPosition.x -= num2;
          num5 = localPosition.x - vector3_1.x;
          int num7 = Mathf.RoundToInt(localPosition.x / (float) this.gridSize);
          if (this.minIndex == this.maxIndex || this.minIndex <= num7 && num7 <= this.maxIndex)
          {
            children.localPosition = localPosition;
            this.UpdateItem(children, index);
          }
          else
            flag = false;
        }
        else if (this.firstTime)
          this.UpdateItem(children, index);
        if (this.disableContent)
        {
          float num8 = num5 + (this.panel.clipOffset.x - this.trans.localPosition.x);
          if (!UICamera.IsPressed(((Component) children).gameObject))
            NGUITools.SetActive(((Component) children).gameObject, (double) num8 > (double) num3 && (double) num8 < (double) num4, false);
        }
      }
    }
    else
    {
      float num9 = worldCorners[0].y - (float) this.gridSize;
      float num10 = worldCorners[2].y + (float) this.gridSize;
      int index = 0;
      for (int count = this.childrenList.Count; index < count; ++index)
      {
        Transform children = this.childrenList[index];
        if (!Object.op_Equality((Object) children, (Object) null))
        {
          float num11 = children.localPosition.y - vector3_1.y;
          if ((double) num11 < -(double) num1)
          {
            Vector3 localPosition = children.localPosition;
            localPosition.y += num2;
            num11 = localPosition.y - vector3_1.y;
            int num12 = Mathf.RoundToInt(localPosition.y / (float) this.gridSize);
            if (this.minIndex == this.maxIndex || this.minIndex <= num12 && num12 <= this.maxIndex)
            {
              children.localPosition = localPosition;
              this.UpdateItem(children, index);
            }
            else
              flag = false;
          }
          else if ((double) num11 > (double) num1)
          {
            Vector3 localPosition = children.localPosition;
            localPosition.y -= num2;
            num11 = localPosition.y - vector3_1.y;
            int num13 = Mathf.RoundToInt(localPosition.y / (float) this.gridSize);
            if (this.minIndex == this.maxIndex || this.minIndex <= num13 && num13 <= this.maxIndex)
            {
              children.localPosition = localPosition;
              this.UpdateItem(children, index);
            }
            else
              flag = false;
          }
          else if (this.firstTime)
            this.UpdateItem(children, index);
          if (this.disableContent)
          {
            float num14 = num11 + (this.panel.clipOffset.y - this.trans.localPosition.y);
            if (!UICamera.IsPressed(((Component) children).gameObject))
              NGUITools.SetActive(((Component) children).gameObject, (double) num14 > (double) num9 && (double) num14 < (double) num10, false);
          }
        }
      }
    }
    this.scroll.restrictWithinPanel = !flag;
    this.scroll.InvalidateBounds();
  }

  private void OnValidate()
  {
    if (this.maxIndex < this.minIndex)
      this.maxIndex = this.minIndex;
    if (this.minIndex <= this.maxIndex)
      return;
    this.maxIndex = this.minIndex;
  }

  protected virtual void UpdateItem(Transform item, int index)
  {
    if (this.onInitializeItem == null)
      return;
    int realIndex = this.scroll.movement == 1 ? Mathf.RoundToInt(item.localPosition.y / (float) this.gridSize) : Mathf.RoundToInt(item.localPosition.x / (float) this.gridSize);
    this.onInitializeItem(((Component) item).gameObject, index, realIndex);
  }

  public delegate void OnInitializeItem(GameObject go, int wrapIndex, int realIndex);
}
