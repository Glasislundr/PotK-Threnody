// Decompiled with JetBrains decompiler
// Type: Quest0022MissionDescriptions
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
public class Quest0022MissionDescriptions : BackButtonMonoBehaiviour
{
  public Quest0022MissionDescription description;

  public QuestStoryMission[] missionDataS { get; set; }

  public QuestSeaMission[] missionDataSea { get; set; }

  public QuestExtraMission[] missionDataE { get; set; }

  public PlayerMissionHistory[] hists { get; set; }

  public IEnumerator InitMissionDescription(QuestStageMenuBase menu, int MissionCount, int StageID)
  {
    if (this.hists != null && MissionCount > 0 && (this.missionDataS != null || this.missionDataE != null || this.missionDataSea != null))
    {
      MissionCount.ToString();
      IEnumerator e;
      if (this.missionDataS == null || this.missionDataS.Length == 0 || this.missionDataE == null || this.missionDataE.Length == 0 || this.missionDataSea == null || this.missionDataSea.Length == 0)
      {
        if (this.missionDataS != null && this.missionDataS.Length != 0)
        {
          QuestStoryMission[] array = ((IEnumerable<QuestStoryMission>) this.missionDataS).Where<QuestStoryMission>((Func<QuestStoryMission, bool>) (x => StageID == x.quest_s_QuestStoryS)).ToArray<QuestStoryMission>();
          if (((IEnumerable<QuestStoryMission>) array).Count<QuestStoryMission>() > 0)
          {
            e = this.description.InitValue(menu, array, this.hists);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
        }
        else if (this.missionDataE != null && this.missionDataE.Length != 0)
        {
          QuestExtraMission[] array = ((IEnumerable<QuestExtraMission>) this.missionDataE).Where<QuestExtraMission>((Func<QuestExtraMission, bool>) (x => StageID == x.quest_s_QuestExtraS)).ToArray<QuestExtraMission>();
          if (((IEnumerable<QuestExtraMission>) array).Count<QuestExtraMission>() > 0)
          {
            e = this.description.InitValue(menu, array, this.hists);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
        }
        else if (this.missionDataSea != null && this.missionDataSea.Length != 0)
        {
          QuestSeaMission[] array = ((IEnumerable<QuestSeaMission>) this.missionDataSea).Where<QuestSeaMission>((Func<QuestSeaMission, bool>) (x => StageID == x.quest_s_QuestSeaS)).ToArray<QuestSeaMission>();
          if (((IEnumerable<QuestSeaMission>) array).Count<QuestSeaMission>() > 0)
          {
            e = this.description.InitValue(menu, array, this.hists);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
        }
      }
    }
  }

  public void StartTween(bool order)
  {
    foreach (UITweener componentsInChild in ((Component) this).GetComponentsInChildren<UITweener>())
    {
      if (componentsInChild.tweenGroup == 1)
        componentsInChild.Play(order);
    }
  }

  public void StartTweenClick(bool order, EventDelegate.Callback callback = null)
  {
    foreach (UITweener componentsInChild in ((Component) this).GetComponentsInChildren<UITweener>())
    {
      if (componentsInChild.tweenGroup == 2)
      {
        if (callback != null)
        {
          componentsInChild.AddOnFinished(callback);
          callback = (EventDelegate.Callback) null;
        }
        componentsInChild.Play(order);
      }
    }
  }

  public override void onBackButton() => this.showBackKeyToast();
}
