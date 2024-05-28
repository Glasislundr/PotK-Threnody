// Decompiled with JetBrains decompiler
// Type: Unit004UnitTrainingListScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/Unit/Training/BaseUnitSelectScene")]
public class Unit004UnitTrainingListScene : NGSceneBase
{
  private static readonly string DefaultName = "unit004_UnitTraining_List";
  private Unit004UnitTrainingListMenu unit004UnitTrainingListMenu;

  public void onStartScene(int playerUnitId) => this.onStartScene();

  public void onStartScene() => Singleton<CommonRoot>.GetInstance().isLoading = false;

  public IEnumerator onStartSceneAsync(int playerUnitId)
  {
    Unit004UnitTrainingListScene trainingListScene = this;
    if (Object.op_Equality((Object) trainingListScene.unit004UnitTrainingListMenu, (Object) null))
      trainingListScene.unit004UnitTrainingListMenu = ((Component) trainingListScene).gameObject.GetComponent<Unit004UnitTrainingListMenu>();
    if (!trainingListScene.unit004UnitTrainingListMenu.isInit)
    {
      if (playerUnitId == -1)
        playerUnitId = Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitID;
      trainingListScene.unit004UnitTrainingListMenu.setLastReference(playerUnitId);
    }
    yield return (object) trainingListScene.onStartSceneAsync();
  }

  public IEnumerator onStartSceneAsync()
  {
    Unit004UnitTrainingListScene trainingListScene = this;
    if (Object.op_Equality((Object) trainingListScene.unit004UnitTrainingListMenu, (Object) null))
      trainingListScene.unit004UnitTrainingListMenu = ((Component) trainingListScene).gameObject.GetComponent<Unit004UnitTrainingListMenu>();
    PlayerDeck playerDeck = SMManager.Get<PlayerDeck[]>()[0];
    Player player = SMManager.Get<Player>();
    PlayerUnit[] playerUnits = SMManager.Get<PlayerUnit[]>();
    IEnumerator e;
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      playerUnits = ((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.IsSea)).ToArray<PlayerUnit>();
      e = trainingListScene.SetSeaBgm();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    if (Singleton<NGGameDataManager>.GetInstance().IsColosseum)
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
    trainingListScene.unit004UnitTrainingListMenu.SetIconType(UnitMenuBase.IconType.Normal);
    if (!trainingListScene.unit004UnitTrainingListMenu.isInit)
    {
      e = trainingListScene.unit004UnitTrainingListMenu.Initialize();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = trainingListScene.unit004UnitTrainingListMenu.Initalize(playerDeck, playerUnits, player.max_cost, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = trainingListScene.unit004UnitTrainingListMenu.UpdateInfoAndScroll(playerUnits);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      trainingListScene.unit004UnitTrainingListMenu.SetTextPosession(playerUnits, player);
    }
  }

  private IEnumerator SetSeaBgm()
  {
    Unit004UnitTrainingListScene trainingListScene = this;
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    SeaHomeMap seaHomeMap = ((IEnumerable<SeaHomeMap>) MasterData.SeaHomeMapList).ActiveSeaHomeMap(ServerTime.NowAppTimeAddDelta());
    if (seaHomeMap != null && !string.IsNullOrEmpty(seaHomeMap.bgm_cuesheet_name) && !string.IsNullOrEmpty(seaHomeMap.bgm_cue_name))
    {
      trainingListScene.bgmFile = seaHomeMap.bgm_cuesheet_name;
      trainingListScene.bgmName = seaHomeMap.bgm_cue_name;
    }
  }

  public static void changeScene(bool isStack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene(Unit004UnitTrainingListScene.DefaultName, isStack);
  }
}
