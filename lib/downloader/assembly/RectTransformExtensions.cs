// Decompiled with JetBrains decompiler
// Type: RectTransformExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class RectTransformExtensions
{
  public static void SetDefaultScale(this RectTransform trans)
  {
    ((Transform) trans).localScale = new Vector3(1f, 1f, 1f);
  }

  public static void SetPivotAndAnchors(this RectTransform trans, Vector2 aVec)
  {
    trans.pivot = aVec;
    trans.anchorMin = aVec;
    trans.anchorMax = aVec;
  }

  public static Vector2 GetSize(this RectTransform trans)
  {
    Rect rect = trans.rect;
    return ((Rect) ref rect).size;
  }

  public static float GetWidth(this RectTransform trans)
  {
    Rect rect = trans.rect;
    return ((Rect) ref rect).width;
  }

  public static float GetHeight(this RectTransform trans)
  {
    Rect rect = trans.rect;
    return ((Rect) ref rect).height;
  }

  public static void SetPositionOfPivot(this RectTransform trans, Vector2 newPos)
  {
    ((Transform) trans).localPosition = new Vector3(newPos.x, newPos.y, ((Transform) trans).localPosition.z);
  }

  public static void SetLeftBottomPosition(this RectTransform trans, Vector2 newPos)
  {
    RectTransform rectTransform = trans;
    double x1 = (double) newPos.x;
    double x2 = (double) trans.pivot.x;
    Rect rect1 = trans.rect;
    double width = (double) ((Rect) ref rect1).width;
    double num1 = x2 * width;
    double num2 = x1 + num1;
    double y1 = (double) newPos.y;
    double y2 = (double) trans.pivot.y;
    Rect rect2 = trans.rect;
    double height = (double) ((Rect) ref rect2).height;
    double num3 = y2 * height;
    double num4 = y1 + num3;
    double z = (double) ((Transform) trans).localPosition.z;
    Vector3 vector3 = new Vector3((float) num2, (float) num4, (float) z);
    ((Transform) rectTransform).localPosition = vector3;
  }

  public static void SetLeftTopPosition(this RectTransform trans, Vector2 newPos)
  {
    RectTransform rectTransform = trans;
    double x1 = (double) newPos.x;
    double x2 = (double) trans.pivot.x;
    Rect rect1 = trans.rect;
    double width = (double) ((Rect) ref rect1).width;
    double num1 = x2 * width;
    double num2 = x1 + num1;
    double y = (double) newPos.y;
    double num3 = 1.0 - (double) trans.pivot.y;
    Rect rect2 = trans.rect;
    double height = (double) ((Rect) ref rect2).height;
    double num4 = num3 * height;
    double num5 = y - num4;
    double z = (double) ((Transform) trans).localPosition.z;
    Vector3 vector3 = new Vector3((float) num2, (float) num5, (float) z);
    ((Transform) rectTransform).localPosition = vector3;
  }

  public static void SetRightBottomPosition(this RectTransform trans, Vector2 newPos)
  {
    RectTransform rectTransform = trans;
    double x = (double) newPos.x;
    double num1 = 1.0 - (double) trans.pivot.x;
    Rect rect1 = trans.rect;
    double width = (double) ((Rect) ref rect1).width;
    double num2 = num1 * width;
    double num3 = x - num2;
    double y1 = (double) newPos.y;
    double y2 = (double) trans.pivot.y;
    Rect rect2 = trans.rect;
    double height = (double) ((Rect) ref rect2).height;
    double num4 = y2 * height;
    double num5 = y1 + num4;
    double z = (double) ((Transform) trans).localPosition.z;
    Vector3 vector3 = new Vector3((float) num3, (float) num5, (float) z);
    ((Transform) rectTransform).localPosition = vector3;
  }

  public static void SetRightTopPosition(this RectTransform trans, Vector2 newPos)
  {
    RectTransform rectTransform = trans;
    double x = (double) newPos.x;
    double num1 = 1.0 - (double) trans.pivot.x;
    Rect rect1 = trans.rect;
    double width = (double) ((Rect) ref rect1).width;
    double num2 = num1 * width;
    double num3 = x - num2;
    double y = (double) newPos.y;
    double num4 = 1.0 - (double) trans.pivot.y;
    Rect rect2 = trans.rect;
    double height = (double) ((Rect) ref rect2).height;
    double num5 = num4 * height;
    double num6 = y - num5;
    double z = (double) ((Transform) trans).localPosition.z;
    Vector3 vector3 = new Vector3((float) num3, (float) num6, (float) z);
    ((Transform) rectTransform).localPosition = vector3;
  }

  public static void SetSize(this RectTransform trans, Vector2 newSize)
  {
    Rect rect = trans.rect;
    Vector2 size = ((Rect) ref rect).size;
    Vector2 vector2 = Vector2.op_Subtraction(newSize, size);
    trans.offsetMin = Vector2.op_Subtraction(trans.offsetMin, new Vector2(vector2.x * trans.pivot.x, vector2.y * trans.pivot.y));
    trans.offsetMax = Vector2.op_Addition(trans.offsetMax, new Vector2(vector2.x * (1f - trans.pivot.x), vector2.y * (1f - trans.pivot.y)));
  }

  public static void SetWidth(this RectTransform trans, float newSize)
  {
    RectTransform trans1 = trans;
    double num = (double) newSize;
    Rect rect = trans.rect;
    double y = (double) ((Rect) ref rect).size.y;
    Vector2 newSize1 = new Vector2((float) num, (float) y);
    trans1.SetSize(newSize1);
  }

  public static void SetHeight(this RectTransform trans, float newSize)
  {
    RectTransform trans1 = trans;
    Rect rect = trans.rect;
    Vector2 newSize1 = new Vector2(((Rect) ref rect).size.x, newSize);
    trans1.SetSize(newSize1);
  }
}
