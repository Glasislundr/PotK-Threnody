// Decompiled with JetBrains decompiler
// Type: Unit004StorageOutScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Unit004StorageOutScene : NGSceneBase
{
  [SerializeField]
  private Unit004StorageOutMenu menu;

  public static void changeScene(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_storage_out", stack);
  }

  public IEnumerator onStartSceneAsync()
  {
    IEnumerator e1 = this.SetStorageBackground();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    SMManager.Get<PlayerUnit[]>();
    e1 = WebAPI.UnitReservesCount((Action<WebAPI.Response.UserError>) (e =>
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      WebAPI.DefaultUserErrorCallback(e);
    })).Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    int storageCount = SMManager.Get<PlayerUnitReservesCount>().count;
    Future<WebAPI.Response.UnitReservesIndex> reservesIndexF = WebAPI.UnitReservesIndex((Action<WebAPI.Response.UserError>) (e =>
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      WebAPI.DefaultUserErrorCallback(e);
    }));
    e1 = reservesIndexF.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    e1 = this.menu.Init(reservesIndexF.Result.player_units, storageCount, Persist.unit004StorageSortAndFilter);
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
  }

  private IEnumerator SetStorageBackground()
  {
    Unit004StorageOutScene unit004StorageOutScene = this;
    Future<GameObject> bgF = new ResourceObject("Prefabs/BackGround/DefaultBackground_storage").Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) bgF.Result, (Object) null))
      unit004StorageOutScene.backgroundPrefab = bgF.Result;
  }

  public void onStartScene()
  {
  }
}
