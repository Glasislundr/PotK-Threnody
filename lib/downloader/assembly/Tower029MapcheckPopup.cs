// Decompiled with JetBrains decompiler
// Type: Tower029MapcheckPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Tower029MapcheckPopup : PopupMapCheckBase
{
  [SerializeField]
  private UILabel lblPopupTitle;

  protected override void SetChipName(PopupMapCheckBase.ChipType type)
  {
    switch (type)
    {
      case PopupMapCheckBase.ChipType.Player:
        this.formationChipExt = "slc_mapchip_50";
        break;
      case PopupMapCheckBase.ChipType.Enemy:
        this.formationChipExt = "slc_mapchip_51";
        break;
      case PopupMapCheckBase.ChipType.facility:
        this.formationChipExt = "slc_mapchip_238";
        break;
    }
  }

  public IEnumerator InitializeAsync(int stage_id)
  {
    Tower029MapcheckPopup tower029MapcheckPopup = this;
    if (Object.op_Inequality((Object) ((Component) tower029MapcheckPopup).GetComponent<UIWidget>(), (Object) null))
      ((UIRect) ((Component) tower029MapcheckPopup).GetComponent<UIWidget>()).alpha = 0.0f;
    BattleStage stage = MasterData.BattleStage[stage_id];
    tower029MapcheckPopup.unitFormationList = new List<PopupMapCheckBase.StageFormation>();
    IEnumerator e = MasterData.LoadBattleStageEnemy(stage);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    BattleStageEnemy enemy = (BattleStageEnemy) null;
    List<PopupMapCheckBase.StageFormation> source = new List<PopupMapCheckBase.StageFormation>();
    for (int index = 0; index < MasterData.BattleStageEnemyList.Length; ++index)
    {
      enemy = MasterData.BattleStageEnemyList[index];
      if (!enemy.reinforcement_BattleReinforcement.HasValue)
      {
        FacilityLevel facilityLevel = Array.Find<FacilityLevel>(MasterData.FacilityLevelList, (Predicate<FacilityLevel>) (fl => fl.unit_UnitUnit == enemy.unit_UnitUnit));
        if (facilityLevel == null)
          tower029MapcheckPopup.unitFormationList.Add(new PopupMapCheckBase.StageFormation(enemy.initial_coordinate_x, enemy.initial_coordinate_y, PopupMapCheckBase.ChipType.Enemy));
        else if (facilityLevel.facility.is_view)
          source.Add(new PopupMapCheckBase.StageFormation(enemy.initial_coordinate_x, enemy.initial_coordinate_y, PopupMapCheckBase.ChipType.facility));
      }
    }
    if (source.Any<PopupMapCheckBase.StageFormation>())
      tower029MapcheckPopup.facilityList = source;
    for (int index = 0; index < stage.Players.Length; ++index)
    {
      BattleStagePlayer player = stage.Players[index];
      tower029MapcheckPopup.unitFormationList.Add(new PopupMapCheckBase.StageFormation(player.initial_coordinate_x, player.initial_coordinate_y, PopupMapCheckBase.ChipType.Player));
    }
    e = tower029MapcheckPopup.Init(stage);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    tower029MapcheckPopup.lblPopupTitle.SetTextLocalize(Consts.GetInstance().POPUP_TOWER_MAPCHECK_TITLE);
  }

  public override void onBackButton() => Singleton<PopupManager>.GetInstance().dismiss();
}
