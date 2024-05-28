// Decompiled with JetBrains decompiler
// Type: Unit004_RentalUnit_Edit_Scene
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
public class Unit004_RentalUnit_Edit_Scene : NGSceneBase
{
  private static readonly string DEFAULT_NAME = "unit004_RentalUnit_Edit";
  private Unit004_RentalUnit_Edit_Menu rentalUnitEditMenu;

  public static void ChangeScene(bool bstack = true)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene(Unit004_RentalUnit_Edit_Scene.DEFAULT_NAME, bstack);
  }

  public IEnumerator onStartSceneAsync()
  {
    Unit004_RentalUnit_Edit_Scene rentalUnitEditScene = this;
    if (Object.op_Equality((Object) rentalUnitEditScene.rentalUnitEditMenu, (Object) null))
      rentalUnitEditScene.rentalUnitEditMenu = ((Component) rentalUnitEditScene).gameObject.GetComponent<Unit004_RentalUnit_Edit_Menu>();
    PlayerDeck playerDeck = SMManager.Get<PlayerDeck[]>()[0];
    Player player = SMManager.Get<Player>();
    PlayerUnit[] playerUnits = SMManager.Get<PlayerUnit[]>();
    IEnumerator e;
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      playerUnits = ((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.IsSea)).ToArray<PlayerUnit>();
      e = rentalUnitEditScene.SetSeaBgm();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    if (Singleton<NGGameDataManager>.GetInstance().IsColosseum)
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
    rentalUnitEditScene.rentalUnitEditMenu.SetIconType(UnitMenuBase.IconType.Normal);
    e = rentalUnitEditScene.rentalUnitEditMenu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = rentalUnitEditScene.rentalUnitEditMenu.Initalize(playerDeck, playerUnits, player.max_cost, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene() => Singleton<CommonRoot>.GetInstance().isLoading = false;

  private IEnumerator SetSeaBgm()
  {
    Unit004_RentalUnit_Edit_Scene rentalUnitEditScene = this;
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    SeaHomeMap seaHomeMap = ((IEnumerable<SeaHomeMap>) MasterData.SeaHomeMapList).ActiveSeaHomeMap(ServerTime.NowAppTimeAddDelta());
    if (seaHomeMap != null && !string.IsNullOrEmpty(seaHomeMap.bgm_cuesheet_name) && !string.IsNullOrEmpty(seaHomeMap.bgm_cue_name))
    {
      rentalUnitEditScene.bgmFile = seaHomeMap.bgm_cuesheet_name;
      rentalUnitEditScene.bgmName = seaHomeMap.bgm_cue_name;
    }
  }
}
