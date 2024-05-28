// Decompiled with JetBrains decompiler
// Type: Shop00720Prefabs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Shop00720Prefabs
{
  private GameObject dirSlotReward;
  private GameObject dirSlotList;
  private GameObject dirSlotPattern;
  private GameObject dirList;

  public GameObject DirSlotReward => this.dirSlotReward;

  public GameObject DirSlotList => this.dirSlotList;

  public GameObject DirSlotPattern => this.dirSlotPattern;

  public GameObject DirList => this.dirList;

  public IEnumerator GetPrefabs()
  {
    Future<GameObject> prefabF = Res.Prefabs.shop007_20.dir_slot_reward.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.dirSlotReward = prefabF.Result;
    prefabF = Res.Prefabs.shop007_20.dir_slotList.Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.dirSlotList = prefabF.Result;
    prefabF = Res.Prefabs.shop007_20.dir_slot_pattern.Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.dirSlotPattern = prefabF.Result;
    prefabF = Res.Prefabs.shop007_20.dir_List.Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.dirList = prefabF.Result;
  }
}
