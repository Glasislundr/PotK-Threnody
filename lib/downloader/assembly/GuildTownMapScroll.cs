// Decompiled with JetBrains decompiler
// Type: GuildTownMapScroll
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class GuildTownMapScroll : PopupMapCheckBase
{
  [SerializeField]
  private UIButton testBattleBtn;
  private Guild0282MemberBaseMenu memberBaseMenu;
  private bool isPush;
  private int slotNo;

  private IEnumerator ShowBattlePreparationPopup()
  {
    IEnumerator e = this.memberBaseMenu.ShowBattlePreparationPopup(this.slotNo, true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

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

  public void OnEnable() => this.isPush = false;

  public IEnumerator InitializeAsync(
    int stage_id,
    bool showTestBattleBtn,
    int slotNo,
    List<Tuple<int, int>> positionList,
    Guild0282MemberBaseMenu memberBaseMenu)
  {
    IEnumerator e = this.CommonInit(stage_id, showTestBattleBtn, slotNo, positionList, memberBaseMenu);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator InitializeAsync(int stage_id, List<Tuple<int, int>> positionList)
  {
    IEnumerator e = this.CommonInit(stage_id, false, -1, positionList, (Guild0282MemberBaseMenu) null);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator CommonInit(
    int stage_id,
    bool showTestBattleBtn,
    int slotNo,
    List<Tuple<int, int>> positionList,
    Guild0282MemberBaseMenu memberBaseMenu)
  {
    GuildTownMapScroll guildTownMapScroll = this;
    BattleStage stage = MasterData.BattleStage[stage_id];
    guildTownMapScroll.memberBaseMenu = memberBaseMenu;
    guildTownMapScroll.slotNo = slotNo;
    guildTownMapScroll.unitFormationList = new List<PopupMapCheckBase.StageFormation>();
    guildTownMapScroll.facilityAreaList = new List<PopupMapCheckBase.StageFormation>();
    GvgStageFormation[] array1 = ((IEnumerable<GvgStageFormation>) MasterData.GvgStageFormationList).Where<GvgStageFormation>((Func<GvgStageFormation, bool>) (x => x.stage_BattleStage == stage_id && x.player_order == 1)).ToArray<GvgStageFormation>();
    for (int index = 0; index < array1.Length; ++index)
      guildTownMapScroll.unitFormationList.Add(new PopupMapCheckBase.StageFormation(array1[index].formation_x, array1[index].formation_y, PopupMapCheckBase.ChipType.Enemy));
    GvgStageFormation[] array2 = ((IEnumerable<GvgStageFormation>) MasterData.GvgStageFormationList).Where<GvgStageFormation>((Func<GvgStageFormation, bool>) (x => x.stage_BattleStage == stage_id && x.player_order == 0)).ToArray<GvgStageFormation>();
    for (int index = 0; index < array2.Length; ++index)
      guildTownMapScroll.unitFormationList.Add(new PopupMapCheckBase.StageFormation(array2[index].formation_x, array2[index].formation_y, PopupMapCheckBase.ChipType.Player));
    BattleMapFacilitySetting[] array3 = ((IEnumerable<BattleMapFacilitySetting>) MasterData.BattleMapFacilitySettingList).Where<BattleMapFacilitySetting>((Func<BattleMapFacilitySetting, bool>) (x => x.map_BattleStage == stage_id)).ToArray<BattleMapFacilitySetting>();
    for (int index = 0; index < array3.Length; ++index)
      guildTownMapScroll.facilityAreaList.Add(new PopupMapCheckBase.StageFormation(array3[index].coordinate_x, array3[index].coordinate_y, PopupMapCheckBase.ChipType.facilityArea));
    guildTownMapScroll.UpdateFacilityPosition(positionList);
    IEnumerator e = guildTownMapScroll.Init(stage);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) guildTownMapScroll.testBattleBtn, (Object) null))
      ((Component) guildTownMapScroll.testBattleBtn).gameObject.SetActive(showTestBattleBtn && PlayerAffiliation.Current.guild.gvg_status != GvgStatus.fighting);
  }

  public void UpdateFacilityPosition(List<Tuple<int, int>> positionList)
  {
    if (this.facilityList == null)
      this.facilityList = new List<PopupMapCheckBase.StageFormation>();
    else
      this.facilityList.Clear();
    if (positionList != null)
    {
      for (int index = 0; index < positionList.Count; ++index)
        this.facilityList.Add(new PopupMapCheckBase.StageFormation(positionList[index].Item1, positionList[index].Item2, PopupMapCheckBase.ChipType.facility));
    }
    this.setupMapChips();
  }

  public void onTestBattleButton()
  {
    if (this.isPush || Object.op_Equality((Object) this.memberBaseMenu, (Object) null))
      return;
    this.isPush = true;
    this.StartCoroutine(this.ShowBattlePreparationPopup());
  }
}
