// Decompiled with JetBrains decompiler
// Type: Guide0112Scene
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
public class Guide0112Scene : NGSceneBase
{
  public Guide0112Menu menu;
  private bool isFirstInit = true;

  public static void changeScene(bool stack, Guide0111Scene.UpdateInfo updateInfo)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("guide011_2", (stack ? 1 : 0) != 0, (object) updateInfo);
  }

  public override IEnumerator onInitSceneAsync()
  {
    Guide0112Scene guide0112Scene = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    Future<GameObject> fBG = Res.Prefabs.BackGround.picturebook.Load<GameObject>();
    IEnumerator e = fBG.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    guide0112Scene.backgroundPrefab = fBG.Result;
  }

  public IEnumerator onStartSceneAsync(Guide0111Scene.UpdateInfo updateInfo)
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    long ver = SMManager.Revision<PlayerUnit[]>();
    long sub = SMManager.Revision<PlayerItem[]>();
    if (updateInfo.version.HasValue)
    {
      long num1 = ver;
      long? nullable = updateInfo.version;
      long valueOrDefault1 = nullable.GetValueOrDefault();
      if (num1 == valueOrDefault1 & nullable.HasValue)
      {
        long num2 = sub;
        nullable = updateInfo.verSub;
        long valueOrDefault2 = nullable.GetValueOrDefault();
        if (num2 == valueOrDefault2 & nullable.HasValue)
          goto label_13;
      }
    }
    Future<WebAPI.Response.ZukanUnit> unit = WebAPI.ZukanUnit((Action<WebAPI.Response.UserError>) (e =>
    {
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    }));
    yield return (object) unit.Wait();
    if (unit.Result == null)
    {
      yield break;
    }
    else
    {
      updateInfo.version = new long?(ver);
      updateInfo.verSub = new long?(sub);
      List<UnitUnit> unitUnitList = new List<UnitUnit>(unit.Result.histories.Length);
      foreach (PlayerUnitHistory history in unit.Result.histories)
      {
        UnitUnit unitUnit;
        if (MasterData.UnitUnit.TryGetValue(history.unit_id, out unitUnit))
          unitUnitList.Add(unitUnit);
      }
      if (unitUnitList.Any<UnitUnit>())
        yield return (object) OnDemandDownload.WaitLoadUnitResource((IEnumerable<UnitUnit>) unitUnitList, false);
      this.isFirstInit = true;
      unit = (Future<WebAPI.Response.ZukanUnit>) null;
    }
label_13:
    if (this.isFirstInit)
    {
      this.isFirstInit = false;
      IEnumerator e = this.menu.onInitMenuAsync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    this.menu.IbtnUse();
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
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
