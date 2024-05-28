// Decompiled with JetBrains decompiler
// Type: BattleCameraFilter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class BattleCameraFilter
{
  public static string LatestCameraFilterResource;
  public static string LatestCameraFilterResourceDuel;
  private static GameObject latestCreateCameraFilter;
  private string cameraFilterResourceName;
  private Transform parent;
  private bool isDuel;

  public static IEnumerator Create(BattleStage stage, GameObject parentCamera, bool isDuel)
  {
    yield return (object) BattleCameraFilter.CreateCommon(BattleCameraFilter.GetName(stage, isDuel), parentCamera, isDuel);
  }

  private static IEnumerator CreateCommon(
    string resourceName,
    GameObject parentCamera,
    bool isDuel)
  {
    if (!Object.op_Equality((Object) parentCamera, (Object) null))
    {
      if (isDuel)
      {
        if (BattleCameraFilter.LatestCameraFilterResourceDuel == "" || BattleCameraFilter.GetChildCount(parentCamera.transform) <= 0 || BattleCameraFilter.LatestCameraFilterResourceDuel != resourceName)
          yield return (object) new BattleCameraFilter(resourceName, parentCamera.transform, true).CreateFilter();
      }
      else if (BattleCameraFilter.LatestCameraFilterResource == "" || BattleCameraFilter.GetChildCount(parentCamera.transform) <= 0 || BattleCameraFilter.LatestCameraFilterResource != resourceName)
        yield return (object) new BattleCameraFilter(resourceName, parentCamera.transform, false).CreateFilter();
    }
  }

  public static void Active()
  {
    if (!Object.op_Inequality((Object) BattleCameraFilter.latestCreateCameraFilter, (Object) null))
      return;
    BattleCameraFilter.latestCreateCameraFilter.SetActive(true);
  }

  public static void Inactive()
  {
    if (!Object.op_Inequality((Object) BattleCameraFilter.latestCreateCameraFilter, (Object) null))
      return;
    BattleCameraFilter.latestCreateCameraFilter.SetActive(false);
  }

  public static void DesotryBattleWin()
  {
    if (Object.op_Inequality((Object) BattleCameraFilter.latestCreateCameraFilter, (Object) null))
    {
      Object.DestroyImmediate((Object) BattleCameraFilter.latestCreateCameraFilter);
      BattleCameraFilter.latestCreateCameraFilter = (GameObject) null;
    }
    BattleCameraFilter.LatestCameraFilterResource = "";
  }

  private static void Destory(Transform parent, bool isDuel)
  {
    foreach (Transform child in parent.GetChildren())
    {
      if (((Object) child).name == "BattleFilter")
        Object.DestroyImmediate((Object) ((Component) child).gameObject);
    }
    if (isDuel)
    {
      BattleCameraFilter.LatestCameraFilterResourceDuel = "";
    }
    else
    {
      BattleCameraFilter.LatestCameraFilterResource = "";
      BattleCameraFilter.latestCreateCameraFilter = (GameObject) null;
    }
  }

  private BattleCameraFilter(string cameraFilterResourceName, Transform parent, bool isDuel)
  {
    this.cameraFilterResourceName = cameraFilterResourceName;
    this.parent = parent;
    this.isDuel = isDuel;
  }

  private IEnumerator CreateFilter()
  {
    BattleCameraFilter.Destory(this.parent, this.isDuel);
    string path = this.GetPath(this.isDuel);
    if (Singleton<ResourceManager>.GetInstance().Contains(path))
    {
      Future<GameObject> filterF = Singleton<ResourceManager>.GetInstance().Load<GameObject>(path);
      IEnumerator e = filterF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject gameObject = filterF.Result.Clone(this.parent);
      Transform transform = gameObject.transform;
      transform.localPosition = new Vector3(0.0f, 0.0f, 5f);
      transform.LookAt(this.parent.position);
      transform.rotation = Quaternion.op_Multiply(transform.rotation, Quaternion.AngleAxis(90f, Vector3.right));
      ((Object) transform).name = "BattleFilter";
      if (this.isDuel)
      {
        BattleCameraFilter.LatestCameraFilterResourceDuel = this.cameraFilterResourceName;
      }
      else
      {
        BattleCameraFilter.LatestCameraFilterResource = this.cameraFilterResourceName;
        BattleCameraFilter.latestCreateCameraFilter = gameObject;
        gameObject.SetActive(false);
      }
    }
  }

  private static string GetName(BattleStage battleStage, bool isDuel)
  {
    if (Singleton<NGBattleManager>.GetInstance().isEarth)
      return "";
    if (Singleton<NGBattleManager>.GetInstance().isCorps)
    {
      CorpsCameraFilter corpsCameraFilter = ((IEnumerable<CorpsCameraFilter>) MasterData.CorpsCameraFilterList).FirstOrDefault<CorpsCameraFilter>((Func<CorpsCameraFilter, bool>) (x => x.stage_id == battleStage));
      return corpsCameraFilter == null ? "" : corpsCameraFilter.filter_resource_name;
    }
    MasterDataTable.BattleCameraFilter battleCameraFilter = ((IEnumerable<MasterDataTable.BattleCameraFilter>) MasterData.BattleCameraFilterList).FirstOrDefault<MasterDataTable.BattleCameraFilter>((Func<MasterDataTable.BattleCameraFilter, bool>) (x => x.stage_id == battleStage));
    return battleCameraFilter == null ? "" : battleCameraFilter.filter_resource_name;
  }

  private static int GetChildCount(Transform parent)
  {
    int childCount = 0;
    foreach (Object child in parent.GetChildren())
    {
      if (child.name == "BattleFilter")
        ++childCount;
    }
    return childCount;
  }

  private string GetPath(bool isDuel)
  {
    if (isDuel)
    {
      string path = string.Format("BattleFilters/{0}/BattleFilterDuel", (object) this.cameraFilterResourceName);
      if (Singleton<ResourceManager>.GetInstance().Contains(path))
        return path;
    }
    return string.Format("BattleFilters/{0}/BattleFilter", (object) this.cameraFilterResourceName);
  }
}
