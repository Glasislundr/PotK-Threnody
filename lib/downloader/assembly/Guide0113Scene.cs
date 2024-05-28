// Decompiled with JetBrains decompiler
// Type: Guide0113Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class Guide0113Scene : NGSceneBase
{
  public Guide0113Menu menu;
  private bool isFirstInit = true;

  public static void changeScene(bool stack, Guide0111Scene.UpdateInfo updateInfo)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("guide011_3", (stack ? 1 : 0) != 0, (object) updateInfo);
  }

  public override IEnumerator onInitSceneAsync()
  {
    Guide0113Scene guide0113Scene = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    Future<GameObject> fBG = Res.Prefabs.BackGround.picturebook.Load<GameObject>();
    IEnumerator e = fBG.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    guide0113Scene.backgroundPrefab = fBG.Result;
  }

  public IEnumerator onStartSceneAsync(Guide0111Scene.UpdateInfo updateInfo)
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    if (!updateInfo.version.HasValue)
    {
      Future<WebAPI.Response.ZukanEnemy> enemy = WebAPI.ZukanEnemy((Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      }));
      yield return (object) enemy.Wait();
      if (enemy.Result == null)
      {
        yield break;
      }
      else
      {
        updateInfo.version = new long?(1L);
        List<UnitUnit> unitUnitList = new List<UnitUnit>(enemy.Result.histories.Length);
        foreach (PlayerEnemyHistory history in enemy.Result.histories)
        {
          UnitUnit unitUnit;
          if (MasterData.UnitUnit.TryGetValue(history.unit_id, out unitUnit))
            unitUnitList.Add(unitUnit);
        }
        if (unitUnitList.Any<UnitUnit>())
          yield return (object) OnDemandDownload.WaitLoadUnitResource((IEnumerable<UnitUnit>) unitUnitList, false);
        this.isFirstInit = true;
        enemy = (Future<WebAPI.Response.ZukanEnemy>) null;
      }
    }
    if (this.isFirstInit)
    {
      this.isFirstInit = false;
      IEnumerator e = this.menu.onInitMenuAsync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    this.menu.IbtnUse();
  }

  public void onStartScene(Guide0111Scene.UpdateInfo updateInfo)
  {
  }

  public override void onEndScene()
  {
    this.menu.StopCreateUnitIconImage();
    Singleton<PopupManager>.GetInstance().closeAll();
  }
}
