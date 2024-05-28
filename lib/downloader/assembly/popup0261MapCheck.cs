// Decompiled with JetBrains decompiler
// Type: popup0261MapCheck
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
public class popup0261MapCheck : PopupMapCheckBase
{
  [SerializeField]
  private UILabel limit_txt;

  public IEnumerator Init(int stage_id, string remaining_time)
  {
    popup0261MapCheck popup0261MapCheck = this;
    BattleStage stage = MasterData.BattleStage[stage_id];
    popup0261MapCheck.unitFormationList = new List<PopupMapCheckBase.StageFormation>();
    List<PvpStageFormation> list = ((IEnumerable<PvpStageFormation>) MasterData.PvpStageFormationList).Where<PvpStageFormation>((Func<PvpStageFormation, bool>) (x => x.stage == stage)).ToList<PvpStageFormation>();
    for (int index = 0; index < list.Count; ++index)
      popup0261MapCheck.unitFormationList.Add(new PopupMapCheckBase.StageFormation(list[index].formation_x, list[index].formation_y, PopupMapCheckBase.ChipType.OwnArea));
    IEnumerator e = popup0261MapCheck.Init(stage);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup0261MapCheck.limit_txt.SetTextLocalize(remaining_time);
  }

  public void IbtnNo() => Singleton<PopupManager>.GetInstance().onDismiss();

  public override void onBackButton() => this.IbtnNo();

  protected override void SetChipName(PopupMapCheckBase.ChipType type)
  {
    this.formationChipExt = "slc_mapchip_ownarea";
  }
}
