// Decompiled with JetBrains decompiler
// Type: Explore033DistantMap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Explore033DistantMap : MonoBehaviour
{
  private const int NUM_ANCHOR_POINT = 2;
  public GameObject[] RandomObjects;
  public GameObject[] ObjectAnchors = new GameObject[2];
  private Animator mAnimator;
  private GameObject[] mActiveObjects = new GameObject[2];
  private int?[] mActiveObjectIndex = new int?[2];

  public float speed
  {
    set => this.mAnimator.speed = value;
  }

  private void Awake() => this.mAnimator = ((Component) this).GetComponent<Animator>();

  private void Start()
  {
    foreach (GameObject randomObject in this.RandomObjects)
      randomObject.SetActive(false);
    for (int index = 0; index < 2; ++index)
      this.SetRandomObject(index);
  }

  public void SetRandomObject(int index)
  {
    if (index >= 2)
    {
      Debug.LogError((object) ("Can't Target Index = " + (object) index + ". Index Max = " + (object) 2));
    }
    else
    {
      if (Object.op_Inequality((Object) this.mActiveObjects[index], (Object) null))
        this.mActiveObjects[index].SetActive(false);
      int index1;
      do
      {
        index1 = Random.Range(0, this.RandomObjects.Length);
      }
      while (!this.CanUseIndex(index1));
      this.mActiveObjectIndex[index] = new int?(index1);
      this.mActiveObjects[index] = this.RandomObjects[index1];
      Vector3 localPosition = this.mActiveObjects[index].transform.localPosition;
      this.mActiveObjects[index].transform.parent = this.ObjectAnchors[index].transform;
      this.mActiveObjects[index].transform.localPosition = localPosition;
      this.mActiveObjects[index].SetActive(true);
    }
  }

  private bool CanUseIndex(int index)
  {
    foreach (int? nullable in this.mActiveObjectIndex)
    {
      if (nullable.HasValue && nullable.Value == index)
        return false;
    }
    return true;
  }
}
