// Decompiled with JetBrains decompiler
// Type: GuideRaritySelectDialog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class GuideRaritySelectDialog : BackButtonMenuBase
{
  [SerializeField]
  private UISprite SlcBase;
  [SerializeField]
  private Vector2[] sclBaseDemision;
  [SerializeField]
  private GuideUnitEvolution[] guideUnitEvolutions;
  [SerializeField]
  private GameObject dirJobChange;
  [SerializeField]
  private GuideUnitJob[] guideUnitJobs;
  public UIGrid gridJob;
  private Guide01122Menu menu;
  private UnitUnit[] commonUnitList;
  private PlayerUnitHistory[] histories;
  private int dispEvolutionCnt;
  private UnitUnit firstSelectUnit;
  private UnitUnit selectUnit;
  private int dispJobCnt;
  private int firstSelectJobID;
  private int selectJobID;
  private bool isExJobHave;

  public void Init(
    Guide01122Menu menu,
    UnitUnit unit,
    UnitUnit[] commonUnitList,
    PlayerUnitHistory[] histories,
    int select_job_id)
  {
    this.menu = menu;
    int length = this.guideUnitEvolutions.Length;
    this.commonUnitList = ((IEnumerable<UnitUnit>) commonUnitList).OrderBy<UnitUnit, int>((Func<UnitUnit, int>) (x => x.ID)).ToArray<UnitUnit>();
    this.dispEvolutionCnt = Math.Min(length, this.commonUnitList.Length);
    this.histories = histories;
    this.firstSelectUnit = this.selectUnit = unit;
    this.firstSelectJobID = this.selectJobID = select_job_id;
    foreach (UnitUnit commonUnit in this.commonUnitList)
    {
      List<int> source = commonUnit.getClassChangeJobIdList();
      if (source.Count > 4)
        source = source.GetRange(0, 4);
      if (source.Any<int>((Func<int, bool>) (x => x != 0)))
      {
        this.isExJobHave = true;
        break;
      }
    }
    if (this.isExJobHave)
    {
      this.dirJobChange.SetActive(true);
      ((UIWidget) this.SlcBase).SetDimensions((int) this.sclBaseDemision[0].x, (int) this.sclBaseDemision[0].y);
      this.setExJob(unit, new int?(select_job_id));
    }
    else
    {
      this.dirJobChange.SetActive(false);
      ((UIWidget) this.SlcBase).SetDimensions((int) this.sclBaseDemision[1].x, (int) this.sclBaseDemision[1].y);
    }
    ((IEnumerable<GuideUnitEvolution>) this.guideUnitEvolutions).ForEach<GuideUnitEvolution>((Action<GuideUnitEvolution>) (x => ((Component) x).gameObject.SetActive(false)));
    this.setEvolution();
  }

  private void setEvolution()
  {
    List<int> intList = this.selectUnit.getClassChangeJobIdList();
    if (intList.Count > 4)
      intList = intList.GetRange(0, 4);
    for (int i = 0; i < this.dispEvolutionCnt; i++)
    {
      int? nullable = ((IEnumerable<PlayerUnitHistory>) this.histories).FirstIndexOrNull<PlayerUnitHistory>((Func<PlayerUnitHistory, bool>) (x => x.unit_id == this.commonUnitList[i].ID));
      UnitUnit commonUnit = this.commonUnitList[i];
      PlayerUnit byUnitunit = PlayerUnit.create_by_unitunit(commonUnit);
      if (i == this.dispEvolutionCnt - 1 && intList.Contains(this.selectJobID) && ((IEnumerable<UnitUnit>) this.commonUnitList).Any<UnitUnit>((Func<UnitUnit, bool>) (x => x.job_UnitJob != this.selectJobID)))
        byUnitunit.job_id = this.selectJobID;
      if (!nullable.HasValue)
        this.guideUnitEvolutions[i].Set(byUnitunit, this.menu, this, false, false, commonUnit.awake_unit_flag);
      else if (this.selectUnit.ID == commonUnit.ID)
        this.guideUnitEvolutions[i].Set(byUnitunit, this.menu, this, true, true, commonUnit.awake_unit_flag);
      else
        this.guideUnitEvolutions[i].Set(byUnitunit, this.menu, this, false, true, commonUnit.awake_unit_flag);
      ((Component) this.guideUnitEvolutions[i]).gameObject.SetActive(true);
    }
  }

  private void setExJob(UnitUnit unit, int? select_job_id = null)
  {
    List<int> source = this.selectUnit.getClassChangeJobIdList();
    if (source.Count > 4)
      source = source.GetRange(0, 4);
    if (!source.Any<int>((Func<int, bool>) (x => x != 0)))
    {
      source.Add(unit.job_UnitJob);
      this.selectJobID = unit.job_UnitJob;
    }
    this.dispJobCnt = Math.Min(this.guideUnitJobs.Length, source.Count<int>());
    PlayerUnitHistory playerUnitHistory = ((IEnumerable<PlayerUnitHistory>) this.histories).FirstOrDefault<PlayerUnitHistory>((Func<PlayerUnitHistory, bool>) (x => x.unit_id == unit.ID));
    ((IEnumerable<GuideUnitJob>) this.guideUnitJobs).ForEach<GuideUnitJob>((Action<GuideUnitJob>) (x => ((Component) x).gameObject.SetActive(false)));
    int jobUnitJob = unit.job_UnitJob;
    for (int index = 0; index < this.dispJobCnt; ++index)
    {
      bool isEnable = source[index] == unit.job_UnitJob || ((IEnumerable<int?>) playerUnitHistory.job_ids).Contains<int?>(new int?(source[index]));
      this.guideUnitJobs[index].Set(this.menu, this, isEnable, source[index]);
      ((Component) this.guideUnitJobs[index]).gameObject.SetActive(true);
      if (isEnable)
        jobUnitJob = source[index];
    }
    if (select_job_id.HasValue)
      jobUnitJob = select_job_id.Value;
    this.setSelectJobID(jobUnitJob);
    this.gridJob.Reposition();
  }

  private void setSelectJobID(int job_id)
  {
    this.selectJobID = job_id;
    for (int index = 0; index < this.dispJobCnt; ++index)
      this.guideUnitJobs[index].SetSelect(this.selectJobID);
  }

  public void IbtnYes()
  {
    if (this.firstSelectUnit.ID != this.selectUnit.ID || this.firstSelectJobID != this.selectJobID)
      this.menu.updateMenu(this.selectUnit, this.selectJobID);
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public void IbtnNo() => Singleton<PopupManager>.GetInstance().onDismiss();

  public override void onBackButton() => this.IbtnNo();

  public void onEvolutionButton(PlayerUnit playerUnit)
  {
    this.selectUnit = playerUnit.unit;
    for (int index = 0; index < this.dispEvolutionCnt; ++index)
      this.guideUnitEvolutions[index].SetSelect(this.selectUnit.ID == this.commonUnitList[index].ID);
    this.selectJobID = playerUnit.job_id;
    this.setEvolution();
    this.setExJob(this.selectUnit, new int?(this.selectJobID));
  }

  public void onJobButton(int job_id)
  {
    this.setSelectJobID(job_id);
    this.selectJobID = job_id;
    this.setEvolution();
  }
}
