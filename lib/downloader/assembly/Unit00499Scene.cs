// Decompiled with JetBrains decompiler
// Type: Unit00499Scene
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
public class Unit00499Scene : NGSceneBase
{
  public Unit00499Menu menu;

  public static void changeScene(
    bool stack,
    PlayerUnit playerUnit,
    Unit00499Scene.Mode mode,
    Action exceptionBackScene)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_9_9", (stack ? 1 : 0) != 0, (object) playerUnit, (object) mode, (object) exceptionBackScene);
  }

  public static void changeScene(bool stack, PlayerUnit playerUnit, Unit00499Scene.Mode mode)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_9_9", (stack ? 1 : 0) != 0, (object) playerUnit, (object) mode);
  }

  public IEnumerator onStartSceneAsync()
  {
    IEnumerator e = this.onStartSceneAsync(SMManager.Get<PlayerUnit[]>()[0], Unit00499Scene.Mode.Evolution);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(
    PlayerUnit playerUnit,
    Unit00499Scene.Mode mode,
    Action exceptionBackScene)
  {
    this.menu.exceptionBackScene = exceptionBackScene;
    yield return (object) this.onStartSceneAsync(playerUnit, mode);
  }

  public IEnumerator onStartSceneAsync(PlayerUnit playerUnit, Unit00499Scene.Mode mode)
  {
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    yield return (object) null;
    Singleton<CommonRoot>.GetInstance().setBackground((GameObject) null);
    Unit00499Scene.GvgCostInfo gvgCostInfo = Unit00499Scene.GvgCostInfo.None;
    IEnumerator e1;
    List<PlayerUnit> playerUnitList;
    if (mode == Unit00499Scene.Mode.Evolution || mode == Unit00499Scene.Mode.AwakeUnit || mode == Unit00499Scene.Mode.CommonAwakeUnit)
    {
      int[] array = ((IEnumerable<UnitEvolutionPattern>) playerUnit.unit.EvolutionPattern).ToList<UnitEvolutionPattern>().Select<UnitEvolutionPattern, int>((Func<UnitEvolutionPattern, int>) (x => x.ID)).ToArray<int>();
      Future<WebAPI.Response.UnitEvolutionParameter> paramF = WebAPI.UnitEvolutionParameter(playerUnit.unit.IsMaterialUnit ? playerUnit.id : 0, playerUnit.unit.IsNormalUnit ? playerUnit.id : 0, 0, array, (Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      }));
      e1 = paramF.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (paramF.Result == null)
      {
        yield break;
      }
      else
      {
        gvgCostInfo = (Unit00499Scene.GvgCostInfo) paramF.Result.gvg_deck_cost_over_status;
        List<PlayerUnit> list = ((IEnumerable<PlayerUnit>) paramF.Result.target_player_units).ToList<PlayerUnit>();
        Func<PlayerUnit, PlayerUnit> chagePlayerUnitId = (Func<PlayerUnit, PlayerUnit>) (pu =>
        {
          pu.id = -1;
          return pu;
        });
        playerUnitList = list.Select<PlayerUnit, PlayerUnit>((Func<PlayerUnit, PlayerUnit>) (pu => chagePlayerUnitId(pu))).ToList<PlayerUnit>().Concat<PlayerUnit>(((IEnumerable<PlayerMaterialUnit>) paramF.Result.target_player_material_units).Select<PlayerMaterialUnit, PlayerUnit>((Func<PlayerMaterialUnit, PlayerUnit>) (x => chagePlayerUnitId(PlayerUnit.CreateByPlayerMaterialUnit(x))))).ToList<PlayerUnit>();
        paramF = (Future<WebAPI.Response.UnitEvolutionParameter>) null;
      }
    }
    else if (Singleton<TutorialRoot>.GetInstance().IsTutorialFinish())
    {
      Future<WebAPI.Response.UnitTransmigrateParameter> paramF = WebAPI.UnitTransmigrateParameter(playerUnit.id, playerUnit.unit.TransmigratePattern.ID, (Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      }));
      e1 = paramF.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (paramF.Result == null)
      {
        yield break;
      }
      else
      {
        PlayerUnit targetPlayerUnit = paramF.Result.target_player_unit;
        targetPlayerUnit.id = -1;
        playerUnitList = new List<PlayerUnit>()
        {
          targetPlayerUnit
        };
        paramF = (Future<WebAPI.Response.UnitTransmigrateParameter>) null;
      }
    }
    else
      playerUnitList = new List<PlayerUnit>()
      {
        Singleton<TutorialRoot>.GetInstance().Resume.after_transmigrate_player_unit
      };
    PlayerUnit playerUnit1 = ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).FirstOrDefault<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.id == playerUnit.id));
    if (Singleton<TutorialRoot>.GetInstance().IsTutorialFinish())
    {
      if (playerUnit1 == (PlayerUnit) null)
        playerUnit1 = playerUnit;
    }
    else
      playerUnit1 = Singleton<TutorialRoot>.GetInstance().Resume.after_levelup1_player_unit;
    e1 = this.menu.Init(playerUnit1, playerUnitList.ToArray(), mode, gvgCostInfo: gvgCostInfo);
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    yield return (object) null;
  }

  public void onStartScene()
  {
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public void onStartScene(
    PlayerUnit playerUnit,
    Unit00499Scene.Mode mode,
    Action exceptionBackScene)
  {
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public void onStartScene(PlayerUnit playerUnit, Unit00499Scene.Mode mode)
  {
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public void onBackScene()
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
  }

  public override void onEndScene() => this.menu.EndTweensMaterialQuestInfo(true);

  public enum Mode
  {
    Evolution,
    EarthEvolution,
    Transmigration,
    AwakeUnit,
    CommonAwakeUnit,
    ReincarnationType,
    JobChange,
  }

  public enum GvgCostInfo
  {
    None,
    Attack,
    Defense,
    Both,
  }
}
