// Decompiled with JetBrains decompiler
// Type: ReisouRenseiScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class ReisouRenseiScene : NGSceneBase
{
  private ReisouRenseiMenu menu;
  private bool resetFlag;
  private bool isDrillingMax;

  public bool IsDrillingMax => this.isDrillingMax;

  public static void changeScene(bool stack, PlayerItem item)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("reisourensei_1", (stack ? 1 : 0) != 0, (object) item, null);
    Singleton<NGGameDataManager>.GetInstance().isReisouScene = false;
  }

  public static void changeScene(bool stack, PlayerItem item, List<InventoryItem> materials)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("reisourensei_1", (stack ? 1 : 0) != 0, (object) item, (object) materials);
    Singleton<NGGameDataManager>.GetInstance().isReisouScene = false;
  }

  public override IEnumerator onInitSceneAsync()
  {
    ReisouRenseiScene reisouRenseiScene = this;
    reisouRenseiScene.menu = reisouRenseiScene.menuBase as ReisouRenseiMenu;
    IEnumerator e = reisouRenseiScene.menu.onInitAsync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<GameObject> bgF = Res.Prefabs.BackGround.ComposeBackground.Load<GameObject>();
    e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    reisouRenseiScene.backgroundPrefab = bgF.Result;
  }

  public IEnumerator onStartSceneAsync(PlayerItem item, List<InventoryItem> materials)
  {
    ReisouRenseiScene scene = this;
    WebAPI.Response.ItemGearBulkDrillingConfirm response = (WebAPI.Response.ItemGearBulkDrillingConfirm) null;
    IEnumerator e;
    if (materials != null && materials.Count > 0)
    {
      int[] array1 = materials.ToMaterialId().ToArray();
      int[] array2 = materials.ToGearId().ToArray();
      int[] array3 = materials.ToMaterialCounts().ToArray();
      Future<WebAPI.Response.ItemGearBulkDrillingConfirm> feature = WebAPI.ItemGearBulkDrillingConfirm(item.id, array2, array1, array3);
      e = feature.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      response = feature.Result;
      feature = (Future<WebAPI.Response.ItemGearBulkDrillingConfirm>) null;
    }
    e = scene.menu.onStartAsync(item, materials, response, scene, response != null);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onBackSceneAsync(PlayerItem item, List<InventoryItem> materials)
  {
    this.isDrillingMax = false;
    if (this.resetFlag)
    {
      this.resetFlag = false;
      PlayerItem playerItem = ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).FirstOrDefault<PlayerItem>((Func<PlayerItem, bool>) (x => x.id == item.id));
      materials = (List<InventoryItem>) null;
      if (playerItem.isMythologyReisou())
      {
        PlayerMythologyGearStatus mythologyGearStatus = playerItem.GetPlayerMythologyGearStatus();
        bool flag = mythologyGearStatus.holy_gear_level >= mythologyGearStatus.holy_gear_level_limit && mythologyGearStatus.chaos_gear_level >= mythologyGearStatus.chaos_gear_level_limit;
        this.isDrillingMax = playerItem.isLimitMax() & flag;
      }
      else
        this.isDrillingMax = playerItem.isLimitMax() && playerItem.isLevelMax();
    }
    IEnumerator e = this.onStartSceneAsync(item, materials);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void SetResetFlag() => this.resetFlag = true;
}
