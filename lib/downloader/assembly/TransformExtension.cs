// Decompiled with JetBrains decompiler
// Type: TransformExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class TransformExtension
{
  public static IEnumerable<Transform> GetChildren(this Transform self)
  {
    for (int i = 0; i < self.childCount; ++i)
      yield return self.GetChild(i);
  }

  public static void Clear(this Transform self)
  {
    foreach (Transform child in self.GetChildren())
    {
      ((Component) child).gameObject.SetActive(false);
      Object.Destroy((Object) ((Component) child).gameObject);
    }
    self.DetachChildren();
  }

  public static string GetFullPath(this Transform self)
  {
    return Object.op_Equality((Object) self.parent, (Object) null) ? ((Object) self).name : self.parent.GetFullPath() + "/" + ((Object) self).name;
  }

  public static Transform FindByFullPath(this Transform self, string name)
  {
    string[] strArray = name.Split('/');
    if (strArray.Length == 0)
      return (Transform) null;
    if (strArray[0] != ((Object) self).name)
      return (Transform) null;
    int index = 1;
    while (index < strArray.Length)
    {
      bool flag = false;
      foreach (Transform child in self.GetChildren())
      {
        if (((Object) child).name == strArray[index])
        {
          self = child;
          ++index;
          flag = true;
          break;
        }
      }
      if (!flag)
        return (Transform) null;
    }
    return self;
  }

  public static Transform GetChildInFind(this Transform self, string name)
  {
    if (((Object) ((Component) self).gameObject).name == name)
      return self;
    foreach (Transform self1 in self)
    {
      Transform childInFind = self1.GetChildInFind(name);
      if (Object.op_Inequality((Object) childInFind, (Object) null))
        return childInFind;
    }
    return (Transform) null;
  }

  public static Transform GetParentInFind(this Transform self, string name)
  {
    if (((Object) ((Component) self).gameObject).name == name)
      return self;
    if (Object.op_Equality((Object) self.parent, (Object) null))
      return (Transform) null;
    Transform parentInFind = self.parent.GetParentInFind(name);
    return Object.op_Inequality((Object) parentInFind, (Object) null) ? parentInFind : (Transform) null;
  }
}
