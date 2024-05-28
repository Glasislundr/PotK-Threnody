// Decompiled with JetBrains decompiler
// Type: Sea030DateScene
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
public class Sea030DateScene : NGSceneBase
{
  [SerializeField]
  public Sea030DateMenu menu;
  private readonly string ImageBgNameBase = "Prefabs/BackGround/SeaDate/DateBackground_{0}";

  public static void changeScene(
    bool stack,
    Sea030HomeMenu seaHomeMenu,
    SeaHomeManager.UnitConrtolleData current2DUnitData)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("sea030_date", (stack ? 1 : 0) != 0, (object) seaHomeMenu, (object) current2DUnitData);
  }

  public override IEnumerator onInitSceneAsync()
  {
    yield break;
  }

  public virtual IEnumerator onStartSceneAsync(
    Sea030HomeMenu seaHomeMenu,
    SeaHomeManager.UnitConrtolleData current2DUnitData)
  {
    Sea030DateScene sea030DateScene = this;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    DateTime now = ServerTime.NowAppTimeAddDelta();
    SeaHomeTimeZone timeZone = ((IEnumerable<SeaHomeTimeZone>) MasterData.SeaHomeTimeZoneList).FirstOrDefault<SeaHomeTimeZone>((Func<SeaHomeTimeZone, bool>) (x => x.WithIn(now)));
    Future<GameObject> bgF = new ResourceObject(string.Format(sea030DateScene.ImageBgNameBase, (object) timeZone.image_pattern)).Load<GameObject>();
    e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    sea030DateScene.backgroundPrefab = bgF.Result;
    Singleton<CommonRoot>.GetInstance().setBackground(sea030DateScene.backgroundPrefab);
    e = sea030DateScene.menu.Init(seaHomeMenu, current2DUnitData, now, timeZone);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }
}
