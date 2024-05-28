// Decompiled with JetBrains decompiler
// Type: Bugu0059Scene
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
public class Bugu0059Scene : NGSceneBase
{
  private Bugu0059Menu menu;
  private bool resetFlag;

  public static void changeScene(bool stack, GameCore.ItemInfo itemInfo)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("bugu005_9", (stack ? 1 : 0) != 0, (object) itemInfo, null);
    Singleton<NGGameDataManager>.GetInstance().isReisouScene = false;
  }

  public static void changeScene(bool stack, GameCore.ItemInfo itemInfo, List<InventoryItem> materials)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("bugu005_9", (stack ? 1 : 0) != 0, (object) itemInfo, (object) materials);
    Singleton<NGGameDataManager>.GetInstance().isReisouScene = false;
  }

  public override IEnumerator onInitSceneAsync()
  {
    Bugu0059Scene bugu0059Scene = this;
    bugu0059Scene.menu = bugu0059Scene.menuBase as Bugu0059Menu;
    IEnumerator e = bugu0059Scene.menu.onInitAsync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<GameObject> bgF = Res.Prefabs.BackGround.ComposeBackground.Load<GameObject>();
    e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    bugu0059Scene.backgroundPrefab = bgF.Result;
  }

  public IEnumerator onStartSceneAsync(GameCore.ItemInfo itemInfo, List<InventoryItem> materials)
  {
    Bugu0059Scene scene = this;
    WebAPI.Response.ItemGearBulkDrillingConfirm response = (WebAPI.Response.ItemGearBulkDrillingConfirm) null;
    IEnumerator e;
    if (materials != null && materials.Count > 0)
    {
      int[] array1 = materials.ToMaterialId().ToArray();
      int[] array2 = materials.ToGearId().ToArray();
      int[] array3 = materials.ToMaterialCounts().ToArray();
      Future<WebAPI.Response.ItemGearBulkDrillingConfirm> feature = WebAPI.ItemGearBulkDrillingConfirm(itemInfo.itemID, array2, array1, array3);
      e = feature.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      response = feature.Result;
      feature = (Future<WebAPI.Response.ItemGearBulkDrillingConfirm>) null;
    }
    e = scene.menu.onStartAsync(itemInfo, materials, response, scene, response != null);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onBackSceneAsync(GameCore.ItemInfo itemInfo, List<InventoryItem> materials)
  {
    if (this.resetFlag)
    {
      this.resetFlag = false;
      PlayerItem playerItem = ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).FirstOrDefault<PlayerItem>((Func<PlayerItem, bool>) (x => x.id == itemInfo.itemID));
      itemInfo = new GameCore.ItemInfo(playerItem);
      materials = (List<InventoryItem>) null;
      bool flag = playerItem.isLimitMax() && playerItem.isLevelMax();
      if (playerItem.isReisouSet)
      {
        PlayerItem equipReisou = playerItem.equipReisou;
        flag = ((flag ? 1 : 0) & (!equipReisou.isLimitMax() ? 0 : (equipReisou.isLevelMax() ? 1 : 0))) != 0;
      }
      if (flag)
      {
        this.menu.callBackScene();
        yield break;
      }
    }
    IEnumerator e = this.onStartSceneAsync(itemInfo, materials);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void SetResetFlag() => this.resetFlag = true;
}
