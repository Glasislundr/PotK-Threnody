// Decompiled with JetBrains decompiler
// Type: GameObjectExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public static class GameObjectExtension
{
  public static T CloneAndGetComponent<T>(this GameObject self, GameObject parent = null) where T : Component
  {
    return Object.op_Equality((Object) self, (Object) null) ? default (T) : self.CloneAndGetComponent<T>(Object.op_Implicit((Object) parent) ? parent.transform : (Transform) null);
  }

  public static T CloneAndGetComponent<T>(this GameObject self, Transform parent) where T : Component
  {
    return Object.op_Equality((Object) self, (Object) null) ? default (T) : self.Clone(parent).GetComponent<T>();
  }

  public static T CloneAndAddComponent<T>(this GameObject self, GameObject parent = null) where T : Component
  {
    return Object.op_Equality((Object) self, (Object) null) ? default (T) : self.CloneAndAddComponent<T>(Object.op_Implicit((Object) parent) ? parent.transform : (Transform) null);
  }

  public static T CloneAndAddComponent<T>(this GameObject self, Transform parent) where T : Component
  {
    return Object.op_Equality((Object) self, (Object) null) ? default (T) : self.Clone(parent).AddComponent<T>();
  }

  public static GameObject SetParentSafeLocalTransform(this GameObject self, GameObject parent)
  {
    if (Object.op_Equality((Object) self, (Object) null))
      return (GameObject) null;
    Vector3 localScale = self.transform.localScale;
    Vector3 localPosition = self.transform.localPosition;
    Quaternion localRotation = self.transform.localRotation;
    self.layer = parent.layer;
    self.transform.parent = parent.transform;
    self.transform.localScale = localScale;
    self.transform.localPosition = localPosition;
    self.transform.localRotation = localRotation;
    return self;
  }

  public static GameObject SetParent(this GameObject self, GameObject parent)
  {
    if (Object.op_Equality((Object) self, (Object) null))
      return (GameObject) null;
    self.layer = parent.layer;
    self.transform.parent = parent.transform;
    self.transform.localScale = Vector3.one;
    self.transform.localPosition = Vector3.zero;
    self.transform.localRotation = Quaternion.identity;
    return self;
  }

  public static GameObject SetParent(this GameObject self, GameObject parent, float ratio)
  {
    if (Object.op_Equality((Object) self, (Object) null))
      return (GameObject) null;
    self.layer = parent.layer;
    self.transform.parent = parent.transform;
    self.transform.localScale = new Vector3(ratio, ratio, 1f);
    self.transform.localPosition = Vector3.zero;
    self.transform.localRotation = Quaternion.identity;
    return self;
  }

  public static GameObject Clone(this GameObject self, Transform parent = null)
  {
    if (Object.op_Equality((Object) self, (Object) null))
      return (GameObject) null;
    GameObject gameObject = Object.Instantiate<GameObject>(self);
    gameObject.transform.parent = parent ?? gameObject.transform.parent;
    gameObject.transform.localScale = Vector3.one;
    gameObject.transform.localPosition = Vector3.zero;
    gameObject.transform.localRotation = Quaternion.identity;
    return gameObject;
  }

  public static void ToggleOnce(this IEnumerable<GameObject> self, int index)
  {
    self.ForEachIndex<GameObject>((Action<GameObject, int>) ((go, n) => go.SetActive(n == index)));
  }

  public static void ToggleOnceEx(this IEnumerable<GameObject> self, int index)
  {
    self.ForEachIndex<GameObject>((Action<GameObject, int>) ((go, n) =>
    {
      if (!Object.op_Inequality((Object) go, (Object) null))
        return;
      go.SetActive(n == index);
    }));
  }

  public static T GetOrAddComponent<T>(this GameObject self) where T : Component
  {
    if (Object.op_Equality((Object) self, (Object) null))
      return default (T);
    T component = self.GetComponent<T>();
    return Object.op_Inequality((Object) (object) component, (Object) null) ? component : self.AddComponent<T>();
  }

  public static void SetActives(this IEnumerable<GameObject> selfs, bool bAct)
  {
    if (selfs == null)
      return;
    int num = selfs.Count<GameObject>();
    for (int index = 0; index < num; ++index)
      selfs.ElementAt<GameObject>(index).SetActive(bAct);
  }

  public static void SetActiveRange(
    this IEnumerable<GameObject> selfs,
    bool bAct,
    int iStart,
    int count = 0)
  {
    if (selfs == null)
      return;
    int num1 = selfs.Count<GameObject>();
    if (iStart < 0 || iStart >= num1)
      return;
    count = count > 0 ? Mathf.Min(num1, iStart + count) - iStart : num1 - iStart;
    int num2 = iStart + count;
    for (int index = iStart; index < num2; ++index)
      selfs.ElementAt<GameObject>(index).SetActive(bAct);
  }

  public static void SetActives<T>(this IEnumerable<T> selfs, bool bAct) where T : Component
  {
    if (selfs == null)
      return;
    int num = selfs.Count<T>();
    for (int index = 0; index < num; ++index)
      selfs.ElementAt<T>(index).gameObject.SetActive(bAct);
  }

  public static void SetActiveRange<T>(
    this IEnumerable<T> selfs,
    bool bAct,
    int iStart,
    int count = 0)
    where T : Component
  {
    if (selfs == null)
      return;
    int num1 = selfs.Count<T>();
    if (iStart < 0 || iStart >= num1)
      return;
    count = count > 0 ? Mathf.Min(num1, iStart + count) - iStart : num1 - iStart;
    int num2 = iStart + count;
    for (int index = iStart; index < num2; ++index)
      selfs.ElementAt<T>(index).gameObject.SetActive(bAct);
  }
}
