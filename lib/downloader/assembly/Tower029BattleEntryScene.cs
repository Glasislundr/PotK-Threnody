// Decompiled with JetBrains decompiler
// Type: Tower029BattleEntryScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Tower029BattleEntryScene : NGSceneBase
{
  [SerializeField]
  private Tower029BattleEntryMenu menu;
  [SerializeField]
  private GameObject bg;

  private IEnumerator SetBackground(int tower_id, int floor)
  {
    Tower029BattleEntryScene battleEntryScene = this;
    string path = string.Empty;
    TowerStage towerStage = ((IEnumerable<TowerStage>) MasterData.TowerStageList).FirstOrDefault<TowerStage>((Func<TowerStage, bool>) (x => x.tower_id == tower_id && x.floor == floor));
    if (towerStage != null)
      path = towerStage.GetBackgroundPath();
    Future<GameObject> bgF = Res.Prefabs.BackGround.SortieBackGround.Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    battleEntryScene.bg = bgF.Result;
    Future<Sprite> bgSpriteF = Singleton<ResourceManager>.GetInstance().LoadOrNull<Sprite>(path);
    e = bgSpriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) bgSpriteF.Result, (Object) null))
    {
      battleEntryScene.bg.GetComponent<UI2DSprite>().sprite2D = bgSpriteF.Result;
      UI2DSprite component = battleEntryScene.bg.GetComponent<UI2DSprite>();
      component.sprite2D = bgSpriteF.Result;
      Rect textureRect1 = bgSpriteF.Result.textureRect;
      ((UIWidget) component).width = Mathf.FloorToInt(((Rect) ref textureRect1).width);
      Rect textureRect2 = bgSpriteF.Result.textureRect;
      ((UIWidget) component).height = Mathf.FloorToInt(((Rect) ref textureRect2).height);
      battleEntryScene.backgroundPrefab = battleEntryScene.bg;
    }
  }

  public static void ChangeScene(TowerProgress progress, TowerLevelList floor)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("tower029_battle_entry", true, (object) progress, (object) floor);
  }

  public IEnumerator onStartSceneAsync(TowerProgress progress, TowerLevelList floor)
  {
    Tower029BattleEntryScene battleEntryScene = this;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    yield return (object) null;
    IEnumerator e = battleEntryScene.SetBackground(progress.tower_id, floor.floorNum);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = battleEntryScene.menu.InitializeAsync(progress, floor);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    battleEntryScene.bgmFile = TowerUtil.BgmFile;
    battleEntryScene.bgmName = TowerUtil.BgmName;
  }

  public void onStartScene(TowerProgress progress, TowerLevelList floor)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public override void onEndScene()
  {
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Singleton<CommonRoot>.GetInstance().releaseBackground();
  }
}
