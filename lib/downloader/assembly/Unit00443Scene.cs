// Decompiled with JetBrains decompiler
// Type: Unit00443Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Unit00443Scene : NGSceneBase
{
  public Unit00443Menu menu;
  private bool targetGear_favorite;

  public static void changeScene(bool stack, GameCore.ItemInfo choiceGear, List<InventoryItem> sortGears = null)
  {
    Unit00443Scene.callChangeScene(stack, choiceGear, false, false, sortGears);
  }

  public static void changeSceneLimited(
    bool stack,
    GameCore.ItemInfo choiceGear,
    List<InventoryItem> sortGears = null)
  {
    Unit00443Scene.callChangeScene(stack, choiceGear, true, false, sortGears);
  }

  public static void changeSceneForDrillingMaterial(
    bool stack,
    GameCore.ItemInfo choiceGear,
    List<InventoryItem> sortGears = null)
  {
    Unit00443Scene.callChangeScene(stack, choiceGear, false, true, sortGears);
  }

  public static void changeSceneCustomDeck(
    PlayerItem gear,
    PlayerUnit playerUnit,
    PlayerItem reisou)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_4_3_terminal", true, (object) new CustomDeck.GearInfo(gear, playerUnit, reisou));
  }

  private static void callChangeScene(
    bool bStack,
    GameCore.ItemInfo item,
    bool bLimited,
    bool bForDrilling,
    List<InventoryItem> sortGears = null)
  {
    NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
    string str = "unit004_4_3";
    if (bStack && instance.sceneName == str || instance.isMatchSceneNameInStack(str))
      str += "_terminal";
    instance.changeScene(str, (bStack ? 1 : 0) != 0, (object) item, (object) bLimited, (object) bForDrilling, (object) sortGears);
  }

  public override IEnumerator onInitSceneAsync()
  {
    Unit00443Scene unit00443Scene = this;
    if (Singleton<CommonRoot>.GetInstance().headerType == CommonRoot.HeaderType.Tower)
    {
      unit00443Scene.bgmFile = TowerUtil.BgmFile;
      unit00443Scene.bgmName = TowerUtil.BgmName;
    }
    else if (Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      IEnumerator e = unit00443Scene.SetSeaBgm();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public IEnumerator onStartSceneAsync(CustomDeck.GearInfo gearInfo)
  {
    this.menu.customGearInfo = gearInfo;
    IEnumerator e = this.menu.Init((GameCore.ItemInfo) null, true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(
    GameCore.ItemInfo choiceGear,
    bool bLimited,
    bool bForDrilling,
    List<InventoryItem> sortGears = null)
  {
    if (this.menu.RetentionGear != null)
      choiceGear = this.menu.RetentionGear;
    PlayerItem playerItem = Array.Find<PlayerItem>(SMManager.Get<PlayerItem[]>(), (Predicate<PlayerItem>) (x => x.id == choiceGear.itemID));
    if (playerItem != (PlayerItem) null)
    {
      choiceGear = new GameCore.ItemInfo(playerItem);
      if (sortGears != null)
      {
        for (int i = 0; i < sortGears.Count; i++)
        {
          if (!sortGears[i].removeButton && !Array.Find<PlayerItem>(SMManager.Get<PlayerItem[]>(), (Predicate<PlayerItem>) (x => x.id == sortGears[i].Item.itemID)).isReisouSet && sortGears[i].Item.reisou != (PlayerItem) null)
          {
            sortGears[i].Item.reisou = (PlayerItem) null;
            sortGears[i].Item.isEquipReisou_ = false;
          }
        }
      }
      this.targetGear_favorite = choiceGear.favorite;
      IEnumerator e = this.menu.Init(choiceGear, bLimited, bForDrilling, sortGears);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      Singleton<NGSceneManager>.GetInstance().ChangeErrorPage();
      while (true)
        yield return (object) null;
    }
  }

  public override IEnumerator onEndSceneAsync()
  {
    if (!this.menu.IsTerminal && this.menu.countChangedSetting != 0)
    {
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      IEnumerator e = this.menu.FavoriteAPI();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      this.targetGear_favorite = ((Component) this.menu.nowFavorite).gameObject.activeSelf;
    }
    else
    {
      yield return (object) new WaitForSeconds(0.5f);
      this.menu.EndScene();
    }
  }

  private IEnumerator SetSeaBgm()
  {
    Unit00443Scene unit00443Scene = this;
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    SeaHomeMap seaHomeMap = ((IEnumerable<SeaHomeMap>) MasterData.SeaHomeMapList).ActiveSeaHomeMap(ServerTime.NowAppTimeAddDelta());
    if (seaHomeMap != null && !string.IsNullOrEmpty(seaHomeMap.bgm_cuesheet_name) && !string.IsNullOrEmpty(seaHomeMap.bgm_cue_name))
    {
      unit00443Scene.bgmFile = seaHomeMap.bgm_cuesheet_name;
      unit00443Scene.bgmName = seaHomeMap.bgm_cue_name;
    }
  }
}
