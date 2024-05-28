// Decompiled with JetBrains decompiler
// Type: Bugu0053DirRecipeListPrefabs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Bugu0053DirRecipeListPrefabs
{
  private GameObject dirGearInfo;
  private GameObject dirMaterialInfo;

  public GameObject DirGearInfo => this.dirGearInfo;

  public GameObject DirMaterialInfo => this.dirMaterialInfo;

  public IEnumerator GetPrefabs()
  {
    Future<GameObject> prefabF = Res.Prefabs.popup.popup_000_7_4_2__anim_popup01.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.dirGearInfo = prefabF.Result;
    prefabF = Res.Prefabs.bugu005_3.popup_005_3_14__anim_popup01.Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.dirMaterialInfo = prefabF.Result;
  }
}
