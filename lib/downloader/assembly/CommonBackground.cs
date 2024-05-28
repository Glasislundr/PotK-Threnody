// Decompiled with JetBrains decompiler
// Type: CommonBackground
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using UniLinq;
using UnityEngine;

#nullable disable
public class CommonBackground : NGMenuBase
{
  public UIWidget bgContainer;
  private GameObject current;
  private string BgPrefabName;

  public GameObject Current => this.current;

  public string GetBgPrefabName()
  {
    return Object.op_Equality((Object) this.current, (Object) null) ? "" : this.BgPrefabName;
  }

  private IEnumerator LateRemoveAll(GameObject[] objs)
  {
    yield return (object) null;
    foreach (Object @object in objs)
      Object.DestroyImmediate(@object);
  }

  public void releaseBackground()
  {
    GameObject[] array = ((Component) this.bgContainer).transform.GetChildren().Select<Transform, GameObject>((Func<Transform, GameObject>) (x => ((Component) x).gameObject)).ToArray<GameObject>();
    foreach (GameObject gameObject in array)
      gameObject.SetActive(false);
    this.StartCoroutine(this.LateRemoveAll(array));
    this.current = (GameObject) null;
  }

  public void setBackground(GameObject prefab)
  {
    this.releaseBackground();
    this.current = NGUITools.AddChild(((Component) this.bgContainer).gameObject, prefab);
    foreach (UI2DSprite componentsInChild in this.current.GetComponentsInChildren<UI2DSprite>())
    {
      if (this.bgContainer.height >= ((UIWidget) componentsInChild).height)
      {
        ((UIWidget) componentsInChild).keepAspectRatio = (UIWidget.AspectRatioSource) 2;
        ((UIRect) componentsInChild).topAnchor.target = ((Component) this.bgContainer).transform;
        ((UIRect) componentsInChild).topAnchor.absolute = 0;
        ((UIRect) componentsInChild).bottomAnchor.target = ((Component) this.bgContainer).transform;
        ((UIRect) componentsInChild).bottomAnchor.absolute = 0;
      }
    }
    this.BgPrefabName = ((Object) prefab).name;
  }

  public bool ComparisonBackground(GameObject prefab)
  {
    return this.hasBackground() || ((Component) this.bgContainer).transform.childCount <= 0 || !(this.current.GetComponent<QuestBG>().namePrefab == prefab.GetComponent<QuestBG>().namePrefab);
  }

  public bool hasBackground()
  {
    return Object.op_Equality((Object) this.current, (Object) null) || Object.op_Equality((Object) this.current.GetComponent<QuestBG>(), (Object) null);
  }
}
