// Decompiled with JetBrains decompiler
// Type: GrowthDegrees
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;
using UnitStatusInformation;
using UnityEngine;

#nullable disable
[AddComponentMenu("Prefabs/GrowthDegrees")]
public class GrowthDegrees : MonoBehaviour
{
  [SerializeField]
  [Tooltip("成長度昇順にセット")]
  private Sprite[] sprites_;
  [SerializeField]
  [Tooltip("表示の実体(X|XX|XXX)表示順に並べる")]
  private GameObject[] bodies_;
  [SerializeField]
  [Tooltip("情報無し表示")]
  private GameObject objNothing_;

  public void Show(GrowthDegree growthDegree)
  {
    int index1 = (int) growthDegree;
    int index2 = growthDegree.ToString().Length - 1;
    if (this.sprites_.Length <= index1 || this.bodies_.Length <= index2)
    {
      this.Hide();
    }
    else
    {
      Sprite sprite = this.sprites_[index1];
      foreach (UI2DSprite componentsInChild in this.bodies_[index2].GetComponentsInChildren<UI2DSprite>(true))
        componentsInChild.sprite2D = sprite;
      ((IEnumerable<GameObject>) this.bodies_).ToggleOnce(index2);
      this.objNothing_.SetActive(false);
    }
  }

  public void Hide()
  {
    ((IEnumerable<GameObject>) this.bodies_).SetActives(false);
    this.objNothing_.SetActive(true);
  }
}
