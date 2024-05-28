// Decompiled with JetBrains decompiler
// Type: PopupJobChangeConditions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class PopupJobChangeConditions : BackButtonPopupBase
{
  [SerializeField]
  [Tooltip("解放条件表示")]
  private PopupJobChangeConditions.UnitJob[] jobs_;
  [SerializeField]
  [Tooltip("土台画像")]
  private UISprite baseSprite_;
  [SerializeField]
  private int contentHeight_ = 150;
  private int baseOffsetHeight_ = 157;

  public static Future<GameObject> createLoader()
  {
    return new ResourceObject("Prefabs/unit004_Job/popup_JobReleasedClass_Conditions__anim_popup01").Load<GameObject>();
  }

  public static void show(GameObject prefab, PlayerUnit playerUnit, int jobIndex)
  {
    if (playerUnit == (PlayerUnit) null)
      Debug.LogError((object) "playerUnit is null.");
    else
      Singleton<PopupManager>.GetInstance().open(prefab, isNonSe: true).GetComponent<PopupJobChangeConditions>().initialize(playerUnit, jobIndex);
  }

  private void initialize(PlayerUnit playerUnit, int jobIndex)
  {
    this.setTopObject(((Component) this).gameObject);
    foreach (PopupJobChangeConditions.UnitJob job in this.jobs_)
      job.setConditions(playerUnit, 0);
    List<int> classChangeJobIdList = playerUnit.unit.getClassChangeJobIdList();
    switch (jobIndex)
    {
      case 3:
        this.jobs_[0].setConditions(playerUnit, classChangeJobIdList[1]);
        this.jobs_[1].setConditions(playerUnit, classChangeJobIdList[2]);
        break;
      case 4:
      case 5:
      case 6:
        this.jobs_[0].setConditions(playerUnit, classChangeJobIdList[jobIndex - 1]);
        break;
      default:
        Debug.LogError((object) ("jobIndex " + (object) jobIndex + " can't have change conditions."));
        break;
    }
    this.adjustBaseHeight();
  }

  private void adjustBaseHeight()
  {
    ((UIWidget) this.baseSprite_).height = this.contentHeight_ * ((IEnumerable<PopupJobChangeConditions.UnitJob>) this.jobs_).Count<PopupJobChangeConditions.UnitJob>((Func<PopupJobChangeConditions.UnitJob, bool>) (x => x.isActive)) + this.baseOffsetHeight_;
  }

  public override void onBackButton() => this.onClickedClose();

  public void onClickedClose()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  [Serializable]
  private class UnitCondition
  {
    [SerializeField]
    [Tooltip("先頭位置")]
    private GameObject objTop_;
    [SerializeField]
    [Tooltip("条件表示")]
    private UILabel txtCondition_;
    [SerializeField]
    [Tooltip("現レベル表示")]
    private UILabel txtStatus_;

    public void setCondition(JobCharacteristics jobAbility, int level)
    {
      if (jobAbility == null)
      {
        this.objTop_.SetActive(false);
      }
      else
      {
        BattleskillSkill skill = jobAbility.skill;
        string placeholder1;
        string placeholder2;
        if (skill.upper_level > level)
        {
          placeholder1 = Consts.GetInstance().POPUP_JOBCHANGE_CONDITION;
          placeholder2 = Consts.GetInstance().POPUP_JOBCHANGE_STATUS;
        }
        else
        {
          placeholder1 = Consts.GetInstance().POPUP_JOBCHANGE_CONDITION_COMPLETED;
          placeholder2 = Consts.GetInstance().POPUP_JOBCHANGE_STATUS_COMPLETED;
        }
        string text1 = Consts.Format(placeholder1, (IDictionary) new Hashtable()
        {
          {
            (object) "name",
            (object) skill.name
          },
          {
            (object) nameof (level),
            (object) skill.upper_level
          }
        });
        string text2 = Consts.Format(placeholder2, (IDictionary) new Hashtable()
        {
          {
            (object) "now",
            (object) level
          },
          {
            (object) "max",
            (object) skill.upper_level
          }
        });
        this.txtCondition_.SetTextLocalize(text1);
        this.txtStatus_.SetTextLocalize(text2);
        this.objTop_.SetActive(true);
      }
    }
  }

  [Serializable]
  private class UnitJob
  {
    [SerializeField]
    [Tooltip("先頭位置")]
    private GameObject objTop_;
    [SerializeField]
    [Tooltip("ジョブ名")]
    private UILabel txtName_;
    [SerializeField]
    [Tooltip("解放条件表示")]
    private PopupJobChangeConditions.UnitCondition[] conditions_;

    public bool isActive => this.objTop_.activeSelf;

    public void setConditions(PlayerUnit playerUnit, int jobId)
    {
      MasterDataTable.UnitJob unitJob;
      if (jobId != 0 && MasterData.UnitJob.TryGetValue(jobId, out unitJob) && unitJob.JobAbilities.Length != 0)
      {
        PlayerUnitAll_saved_job_abilities[] allJobAbilities = playerUnit.all_saved_job_abilities ?? new PlayerUnitAll_saved_job_abilities[0];
        \u003C\u003Ef__AnonymousType11<JobCharacteristics, PlayerUnitAll_saved_job_abilities>[] array = ((IEnumerable<JobCharacteristics>) unitJob.JobAbilities).Where<JobCharacteristics>((Func<JobCharacteristics, bool>) (x => x.skill.upper_level > 0)).Select(y => new
        {
          m = y,
          v = Array.Find<PlayerUnitAll_saved_job_abilities>(allJobAbilities, (Predicate<PlayerUnitAll_saved_job_abilities>) (z => z.job_ability_id == y.ID))
        }).ToArray();
        this.txtName_.SetTextLocalize(unitJob.name);
        for (int index = 0; index < this.conditions_.Length; ++index)
        {
          if (array.Length <= index)
          {
            this.conditions_[index].setCondition((JobCharacteristics) null, 0);
          }
          else
          {
            PopupJobChangeConditions.UnitCondition condition = this.conditions_[index];
            JobCharacteristics m = array[index].m;
            PlayerUnitAll_saved_job_abilities v = array[index].v;
            int level = v != null ? v.level : 0;
            condition.setCondition(m, level);
          }
        }
        this.objTop_.SetActive(true);
      }
      else
        this.objTop_.SetActive(false);
    }
  }
}
