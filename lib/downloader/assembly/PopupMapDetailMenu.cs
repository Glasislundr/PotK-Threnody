// Decompiled with JetBrains decompiler
// Type: PopupMapDetailMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class PopupMapDetailMenu : PopupMapCheckBase
{
  [SerializeField]
  private UILabel lblMapName;
  [SerializeField]
  private UILabel lblDescription;
  [SerializeField]
  private UILabel lblCost;
  private Action actionClose;

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
      case PopupMapCheckBase.ChipType.facilityArea:
        this.formationChipExt = "slc_mapchip_235";
        break;
    }
  }

  public IEnumerator InitializeAsync(int maptown_id, Action actionClose = null)
  {
    yield return (object) this.InitializeAsync(maptown_id, (List<Tuple<int, int>>) null, actionClose);
  }

  public IEnumerator InitializeAsync(
    int maptown_id,
    List<Tuple<int, int>> facilityPositions,
    Action actionClose = null)
  {
    PopupMapDetailMenu popupMapDetailMenu = this;
    MapTown maptown = MasterData.MapTown[maptown_id];
    if (maptown != null)
    {
      int stageId = maptown.stage_id;
      BattleStage stage = MasterData.BattleStage[stageId];
      if (stage != null)
      {
        if (Object.op_Inequality((Object) ((Component) popupMapDetailMenu).GetComponent<UIWidget>(), (Object) null))
          ((UIRect) ((Component) popupMapDetailMenu).GetComponent<UIWidget>()).alpha = 0.0f;
        popupMapDetailMenu.unitFormationList = new List<PopupMapCheckBase.StageFormation>();
        popupMapDetailMenu.facilityAreaList = new List<PopupMapCheckBase.StageFormation>();
        popupMapDetailMenu.actionClose = actionClose;
        GvgStageFormation[] array1 = ((IEnumerable<GvgStageFormation>) MasterData.GvgStageFormationList).Where<GvgStageFormation>((Func<GvgStageFormation, bool>) (x => x.stage_BattleStage == stageId && x.player_order == 1)).ToArray<GvgStageFormation>();
        for (int index = 0; index < array1.Length; ++index)
          popupMapDetailMenu.unitFormationList.Add(new PopupMapCheckBase.StageFormation(array1[index].formation_x, array1[index].formation_y, PopupMapCheckBase.ChipType.Enemy));
        GvgStageFormation[] array2 = ((IEnumerable<GvgStageFormation>) MasterData.GvgStageFormationList).Where<GvgStageFormation>((Func<GvgStageFormation, bool>) (x => x.stage_BattleStage == stageId && x.player_order == 0)).ToArray<GvgStageFormation>();
        for (int index = 0; index < array2.Length; ++index)
          popupMapDetailMenu.unitFormationList.Add(new PopupMapCheckBase.StageFormation(array2[index].formation_x, array2[index].formation_y, PopupMapCheckBase.ChipType.Player));
        BattleMapFacilitySetting[] array3 = ((IEnumerable<BattleMapFacilitySetting>) MasterData.BattleMapFacilitySettingList).Where<BattleMapFacilitySetting>((Func<BattleMapFacilitySetting, bool>) (x => x.map_BattleStage == stageId)).ToArray<BattleMapFacilitySetting>();
        for (int index = 0; index < array3.Length; ++index)
          popupMapDetailMenu.facilityAreaList.Add(new PopupMapCheckBase.StageFormation(array3[index].coordinate_x, array3[index].coordinate_y, PopupMapCheckBase.ChipType.facilityArea));
        if (facilityPositions != null && facilityPositions.Any<Tuple<int, int>>())
        {
          popupMapDetailMenu.facilityList = new List<PopupMapCheckBase.StageFormation>();
          foreach (Tuple<int, int> facilityPosition in facilityPositions)
            popupMapDetailMenu.facilityList.Add(new PopupMapCheckBase.StageFormation(facilityPosition.Item1, facilityPosition.Item2, PopupMapCheckBase.ChipType.facility));
        }
        else
          popupMapDetailMenu.facilityList = (List<PopupMapCheckBase.StageFormation>) null;
        IEnumerator e = popupMapDetailMenu.Init(stage);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (Object.op_Inequality((Object) popupMapDetailMenu.lblMapName, (Object) null))
          popupMapDetailMenu.lblMapName.SetTextLocalize(maptown.name);
        if (Object.op_Inequality((Object) popupMapDetailMenu.lblDescription, (Object) null))
          popupMapDetailMenu.lblDescription.SetTextLocalize(maptown.description);
        if (Object.op_Inequality((Object) popupMapDetailMenu.lblCost, (Object) null))
          popupMapDetailMenu.lblCost.SetTextLocalize(maptown.cost_capacity);
      }
    }
  }

  public IEnumerator InitializeAsync(ShopContent content)
  {
    IEnumerator e = this.InitializeAsync(content.entity_id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public override void onBackButton()
  {
    if (this.actionClose != null)
      this.actionClose();
    Singleton<PopupManager>.GetInstance().dismiss();
  }
}
