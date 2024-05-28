// Decompiled with JetBrains decompiler
// Type: Unit004OverkillersSlotUnitSelectScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using CustomDeck;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Unit004OverkillersSlotUnitSelectScene : NGSceneBase
{
  public static readonly string DefaultName = "unit004_UnitEquip_List";

  public static void changeScene(
    PlayerUnit base_unit,
    int slot_no,
    PlayerUnit select_unit,
    Action<PlayerUnit> act_selected)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene(Unit004OverkillersSlotUnitSelectScene.DefaultName, true, (object) new Unit004OverkillersSlotUnitSelectScene.Param(base_unit, slot_no, select_unit, act_selected));
  }

  public static void changeScene(EditOverkillersParam param)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene(Unit004OverkillersSlotUnitSelectScene.DefaultName, true, (object) param);
  }

  private Unit004OverkillersSlotUnitSelectMenu menu
  {
    get => this.menuBase as Unit004OverkillersSlotUnitSelectMenu;
  }

  public IEnumerator onStartSceneAsync(Unit004OverkillersSlotUnitSelectScene.Param param)
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    yield return (object) null;
    yield return (object) this.menu.initialize(param);
  }

  public void onStartScene(Unit004OverkillersSlotUnitSelectScene.Param param)
  {
    this.StartCoroutine(this.doDelayLoadingOff());
  }

  public IEnumerator onStartSceneAsync(EditOverkillersParam param)
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    yield return (object) null;
    yield return (object) this.menu.initialize((Unit004OverkillersSlotUnitSelectScene.Param) null, param);
  }

  public void onStartScene(EditOverkillersParam param)
  {
    this.onStartScene((Unit004OverkillersSlotUnitSelectScene.Param) null);
  }

  private IEnumerator doDelayLoadingOff()
  {
    yield return (object) new WaitForSeconds(0.5f);
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public IEnumerator onBackSceneAsync(Unit004OverkillersSlotUnitSelectScene.Param param)
  {
    yield break;
  }

  public void onBackScene(Unit004OverkillersSlotUnitSelectScene.Param param)
  {
  }

  public class Param
  {
    public PlayerUnit baseUnit { get; private set; }

    public int slotNo { get; private set; }

    public PlayerUnit selectUnit { get; private set; }

    public Action<PlayerUnit> actSelected { get; private set; }

    public Param(
      PlayerUnit base_unit,
      int slot_no,
      PlayerUnit select_unit,
      Action<PlayerUnit> act_selected)
    {
      this.baseUnit = base_unit;
      this.slotNo = slot_no;
      this.selectUnit = select_unit;
      this.actSelected = act_selected;
    }
  }
}
