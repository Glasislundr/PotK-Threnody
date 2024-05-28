// Decompiled with JetBrains decompiler
// Type: Raid032HuntingInfoScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Raid032HuntingInfoScene : NGSceneBase
{
  private Raid032HuntingInfoMenu menu;
  private GameObject dir_raid_HuntingInfo_scrollPrefab;
  private GameObject unitNormalIconPrefab;

  public static void ChangeScene(bool stack, int period_id)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("raid032_HuntingInfo", (stack ? 1 : 0) != 0, (object) period_id);
  }

  public IEnumerator onStartSceneAsync(int period_id)
  {
    Raid032HuntingInfoScene huntingInfoScene = this;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    huntingInfoScene.menu = huntingInfoScene.menuBase as Raid032HuntingInfoMenu;
    huntingInfoScene.menu.ClearScrollView();
    Future<WebAPI.Response.GuildraidSubjugationHistory> ft = WebAPI.GuildraidSubjugationHistory(period_id, (Action<WebAPI.Response.UserError>) (e =>
    {
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e1 = ft.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (ft.Result != null)
    {
      if (Object.op_Equality((Object) huntingInfoScene.dir_raid_HuntingInfo_scrollPrefab, (Object) null))
        yield return (object) huntingInfoScene.LoadHuntingInfoScrollItemPrefab();
      if (Object.op_Equality((Object) huntingInfoScene.unitNormalIconPrefab, (Object) null))
        yield return (object) huntingInfoScene.LoadUnitIconPrefab();
      e1 = huntingInfoScene.menu.InitAsync(huntingInfoScene.dir_raid_HuntingInfo_scrollPrefab, huntingInfoScene.unitNormalIconPrefab, ft.Result);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      Singleton<CommonRoot>.GetInstance().isLoading = false;
    }
  }

  private IEnumerator LoadHuntingInfoScrollItemPrefab()
  {
    Future<GameObject> future = Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/raid032_HuntingInfo/dir_raid_HuntingInfo_scroll");
    IEnumerator e = future.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Equality((Object) future.Result, (Object) null))
      Debug.LogError((object) "failed to load dir_raid_HuntingInfo_scroll.prefab");
    else
      this.dir_raid_HuntingInfo_scrollPrefab = future.Result;
  }

  private IEnumerator LoadUnitIconPrefab()
  {
    Future<GameObject> prefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.unitNormalIconPrefab = prefabF.Result;
  }

  public void onStartScene(int period_id) => Singleton<CommonRoot>.GetInstance().isLoading = false;
}
