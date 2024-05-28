// Decompiled with JetBrains decompiler
// Type: Unit004813Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Unit004813Scene : NGSceneBase
{
  [SerializeField]
  private Unit004813Menu menu;

  public static void changeScene(
    bool stack,
    PlayerUnit basePlayerUnit,
    PlayerUnit resultPlayerUnit,
    List<int> otherData,
    Dictionary<string, object> showPopupData)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_8_13", (stack ? 1 : 0) != 0, (object) basePlayerUnit, (object) resultPlayerUnit, (object) otherData, (object) showPopupData);
  }

  public static void changeScene(
    bool stack,
    PlayerUnit basePlayerUnit,
    PlayerUnit resultPlayerUnit,
    List<int> otherData,
    Dictionary<string, object> showPopupData,
    Unit00468Scene.Mode mode)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_8_13", (stack ? 1 : 0) != 0, (object) basePlayerUnit, (object) resultPlayerUnit, (object) otherData, (object) showPopupData, (object) mode);
  }

  public override IEnumerator onInitSceneAsync()
  {
    Unit004813Scene unit004813Scene = this;
    Future<GameObject> bgF = Res.Prefabs.BackGround.UnitBackground_60.Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit004813Scene.backgroundPrefab = bgF.Result;
  }

  public IEnumerator onStartSceneAsync(
    PlayerUnit basePlayerUnit,
    PlayerUnit resultPlayerUnit,
    List<int> otherData,
    Dictionary<string, object> showPopupData)
  {
    this.menu.showPopupData = showPopupData;
    IEnumerator e = this.menu.setCharacter(basePlayerUnit, resultPlayerUnit, otherData);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().closeAll();
    Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
    Singleton<CommonRoot>.GetInstance().setDisableFooterColor(true);
  }

  public IEnumerator onStartSceneAsync(
    PlayerUnit basePlayerUnit,
    PlayerUnit resultPlayerUnit,
    List<int> otherData,
    Dictionary<string, object> showPopupData,
    Unit00468Scene.Mode mode)
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    this.menu.mode = mode;
    IEnumerator e = this.onStartSceneAsync(basePlayerUnit, resultPlayerUnit, otherData, showPopupData);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  public IEnumerator onStartSceneAsync(
    List<List<PlayerUnit>> selectedMaterialPlayerUnits,
    List<Unit004832Menu.ResultPlayerUnit> resultPlayerUnits,
    List<Unit004832Menu.OhterInfo> otherInfos,
    List<Dictionary<string, object>> showPopupDatas)
  {
    if (selectedMaterialPlayerUnits.Count != resultPlayerUnits.Count || resultPlayerUnits.Count != otherInfos.Count || otherInfos.Count != showPopupDatas.Count)
      Debug.LogError((object) "LumpTouta ListError");
    if (selectedMaterialPlayerUnits.Count > 1)
      ((Component) this.menu.skipButton).gameObject.SetActive(true);
    else
      ((Component) this.menu.skipButton).gameObject.SetActive(false);
    this.menu.mode = Unit00468Scene.Mode.UnitLumpTouta;
    this.menu.selectedMaterialPlayerUnits = selectedMaterialPlayerUnits;
    this.menu.resultPlayerUnits = resultPlayerUnits;
    this.menu.otherInfos = otherInfos;
    this.menu.showPopupDatas = showPopupDatas;
    List<PlayerUnit> playerUnitList = new List<PlayerUnit>();
    playerUnitList.Add(resultPlayerUnits[0].beforePlayerUnit);
    playerUnitList.Add(resultPlayerUnits[0].afterPlayerUnit);
    IEnumerator e = this.onStartSceneAsync(playerUnitList[0], playerUnitList[1], new List<int>()
    {
      Convert.ToInt32(otherInfos[0].is_success),
      otherInfos[0].increment_medal,
      otherInfos[0].gain_trust_result.is_equip_awake_skill_release ? 1 : 0,
      otherInfos[0].gain_trust_result.has_new_player_awake_skill ? 1 : 0
    }, showPopupDatas[0]);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public override void onEndScene()
  {
    base.onEndScene();
    Singleton<CommonRoot>.GetInstance().SetFooterEnable(true);
    Singleton<CommonRoot>.GetInstance().setDisableFooterColor(false);
    this.menu.loveGaugeController.StopGaugeAnimation();
  }
}
