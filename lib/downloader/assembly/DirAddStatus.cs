// Decompiled with JetBrains decompiler
// Type: DirAddStatus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DirAddStatus : MonoBehaviour
{
  [SerializeField]
  private GameObject[] AddStatus;
  private List<GameObject> addStatusList = new List<GameObject>();
  private Future<GameObject> addStatusPrefabF;

  public IEnumerator Init(List<IncrementalInfo> list)
  {
    if (this.addStatusPrefabF == null)
    {
      this.addStatusPrefabF = Res.Prefabs.unit004_4_3.Add_Status.Load<GameObject>();
      IEnumerator e = this.addStatusPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    for (int index = this.addStatusList.Count - 1; index > list.Count - 1; --index)
    {
      this.addStatusList[index].transform.Clear();
      this.addStatusList.Remove(this.addStatusList[index]);
    }
    for (int index = 0; index < list.Count; ++index)
    {
      if (this.addStatusList.Count > index)
      {
        this.addStatusList[index].GetComponent<global::AddStatus>().Init(list[index].name, list[index].value);
      }
      else
      {
        GameObject gameObject = this.addStatusPrefabF.Result.Clone(this.AddStatus[index].transform);
        gameObject.GetComponent<global::AddStatus>().Init(list[index].name, list[index].value);
        this.addStatusList.Add(gameObject);
      }
    }
  }
}
