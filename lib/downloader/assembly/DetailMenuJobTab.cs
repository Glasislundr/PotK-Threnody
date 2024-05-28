// Decompiled with JetBrains decompiler
// Type: DetailMenuJobTab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[AddComponentMenu("PUNK Scripts/Detail/DetailMenuJobTab")]
public class DetailMenuJobTab : MonoBehaviour
{
  [SerializeField]
  [Tooltip("クラスチェンジ画面用パネル(初期表示)")]
  private GameObject defaultPanel_;
  [SerializeField]
  [Tooltip("「クラスチェンジ無し」用パネル")]
  private GameObject noneJobChange_;
  [SerializeField]
  [Tooltip("「クラスチェンジ」/「バーテックスファクター」タブ表示")]
  private GameObject topTab_;
  [Header("「クラスチェンジ」設定")]
  [SerializeField]
  private DetailMenuJobTab.JobChangePanel jobChange_;
  [Header("「バーテックスファクター」設定")]
  [SerializeField]
  private DetailMenuJobTab.JobAbilityPanel jobAbility_;
  private DetailMenuJobTab.TabMode tabMode_ = DetailMenuJobTab.TabMode.JobChange;

  public bool initialize(
    PlayerUnit playerUnit,
    bool bLimit,
    Action<DetailMenuJobAbilityParts, PlayerUnitJob_abilities, bool, bool> eventClicked,
    Action<UIButton, int> eventChanged)
  {
    if (Object.op_Implicit((Object) this.defaultPanel_))
      this.defaultPanel_.SetActive(false);
    bool isExist;
    PlayerUnit[] jobChangePattern = JobChangeUtil.createJobChangePattern(playerUnit, out isExist);
    if (!isExist)
    {
      this.topTab_.SetActive(false);
      this.noneJobChange_.SetActive(true);
      return false;
    }
    this.noneJobChange_.SetActive(false);
    this.topTab_.SetActive(true);
    bLimit |= eventChanged == null;
    PlayerUnitJob_abilities[] jobAbilities1 = playerUnit.job_abilities;
    if (jobAbilities1 != null && jobAbilities1.Length != 0)
    {
      int iStart = 0;
      for (int index = 0; index < jobAbilities1.Length && iStart < this.jobAbility_.abilities.Length; ++index)
      {
        if (!jobAbilities1[index].skill.IsLand)
          eventClicked(this.jobAbility_.abilities[iStart++], jobAbilities1[index], false, bLimit);
      }
      if (this.jobAbility_.abilities.Length > iStart)
        ((IEnumerable<DetailMenuJobAbilityParts>) this.jobAbility_.abilities).SetActiveRange<DetailMenuJobAbilityParts>(false, iStart);
    }
    else
      ((IEnumerable<DetailMenuJobAbilityParts>) this.jobAbility_.abilities).SetActives<DetailMenuJobAbilityParts>(false);
    int num1 = playerUnit.hasXLevel ? 1 : 0;
    PlayerUnit[] array1 = ((IEnumerable<PlayerUnit>) jobChangePattern).Skip<PlayerUnit>(1).Take<PlayerUnit>(3).ToArray<PlayerUnit>();
    HashSet<int> intSet = playerUnit.changed_job_ids == null || playerUnit.changed_job_ids.Length == 0 ? new HashSet<int>() : new HashSet<int>(((IEnumerable<int?>) playerUnit.changed_job_ids).Where<int?>((Func<int?, bool>) (a => a.HasValue)).Select<int?, int>((Func<int?, int>) (b => b.Value)));
    if (num1 != 0)
    {
      int index1 = 0;
      int index2 = 4;
      while (index1 < array1.Length)
      {
        if (jobChangePattern.Length > index2 && jobChangePattern[index2] != (PlayerUnit) null && intSet.Contains(jobChangePattern[index2].job_id))
          array1[index1] = jobChangePattern[index2];
        ++index1;
        ++index2;
      }
    }
    Tuple<PlayerUnit, int>[] array2 = ((IEnumerable<PlayerUnit>) array1).Select<PlayerUnit, Tuple<PlayerUnit, int>>((Func<PlayerUnit, int, Tuple<PlayerUnit, int>>) ((x, i) => Tuple.Create<PlayerUnit, int>(x, i))).Where<Tuple<PlayerUnit, int>>((Func<Tuple<PlayerUnit, int>, bool>) (y => y.Item1 != (PlayerUnit) null)).ToArray<Tuple<PlayerUnit, int>>();
    for (int index3 = 0; index3 < array2.Length; ++index3)
    {
      PlayerUnit playerUnit1 = array2[index3].Item1;
      int num2 = array2[index3].Item2;
      MasterDataTable.UnitJob job = playerUnit1.getJobData();
      DetailMenuJobTab.JobParts part = this.jobChange_.parts[index3];
      bool isVertexX = job.is_vertex_x;
      bool flag = intSet.Contains(job.ID);
      if (Object.op_Inequality((Object) part.topNormal, (Object) null))
        part.topNormal.SetActive(!isVertexX);
      if (Object.op_Inequality((Object) part.topX, (Object) null))
        part.topX.SetActive(isVertexX);
      UnitJobRankName unitJobRankName;
      string text1 = (isVertexX ? "[FFED22]" : string.Empty) + (MasterData.UnitJobRankName.TryGetValue(job.job_rank_UnitJobRank, out unitJobRankName) ? unitJobRankName.name : string.Empty);
      part.txtRank.SetTextLocalize(text1);
      part.txtName.SetTextLocalize(job.name);
      PlayerUnitJob_abilities[] jobAbilities2 = playerUnit1.job_abilities;
      int num3 = 0;
      int count = 0;
      for (int index4 = 0; index4 < jobAbilities2.Length; ++index4)
      {
        BattleskillSkill skill = jobAbilities2[index4].skill;
        if (!skill.IsLand)
        {
          if (part.abilityTags.Length > count)
          {
            DetailMenuJobTab.AbilityTag abilityTag = part.abilityTags[count++];
            string text2 = jobAbilities2[index4].level.ToString();
            if (text2 == "0" && !flag)
              text2 = "-";
            abilityTag.txtLevel.SetTextLocalize(text2);
            abilityTag.txtName.SetTextLocalize(skill.name);
            abilityTag.top.SetActive(true);
            JobCharacteristics jobCharacteristics;
            if (MasterData.JobCharacteristics.TryGetValue(jobAbilities2[index4].job_ability_id, out jobCharacteristics))
            {
              XLevelLimits xlevelLimits = jobCharacteristics.xlevel_limits;
              if (xlevelLimits != null)
                num3 += xlevelLimits.getLimit(jobAbilities2[index4].level);
            }
          }
          else
            break;
        }
      }
      if (part.abilityTags.Length > count)
        ((IEnumerable<DetailMenuJobTab.AbilityTag>) part.abilityTags).Skip<DetailMenuJobTab.AbilityTag>(count).Select<DetailMenuJobTab.AbilityTag, GameObject>((Func<DetailMenuJobTab.AbilityTag, GameObject>) (x => x.top)).SetActives(false);
      if (isVertexX)
      {
        if (flag)
        {
          part.txtMaxLevel.SetTextLocalize(string.Format("+{0}", (object) num3));
          UILabel txtProficiency = part.txtProficiency;
          PlayerUnitX_job_proficiencies jobProficiencies = ((IEnumerable<PlayerUnitX_job_proficiencies>) playerUnit.x_job_proficiencies).FirstOrDefault<PlayerUnitX_job_proficiencies>((Func<PlayerUnitX_job_proficiencies, bool>) (x => x.job_id == job.ID));
          int proficiency = jobProficiencies != null ? jobProficiencies.proficiency : 0;
          txtProficiency.SetTextLocalize(proficiency);
        }
        else
        {
          part.txtMaxLevel.SetTextLocalize("-");
          part.txtProficiency.SetTextLocalize("-");
        }
      }
      if (Object.op_Implicit((Object) part.button))
      {
        if (bLimit || !((IEnumerable<int?>) playerUnit.changed_job_ids).Contains<int?>(new int?(job.ID)))
        {
          ((Component) part.button).gameObject.SetActive(false);
        }
        else
        {
          ((Component) part.button).gameObject.SetActive(true);
          eventChanged(part.button, job.ID);
        }
      }
      part.top.SetActive(true);
    }
    if (this.jobChange_.parts.Length > array2.Length)
      ((IEnumerable<DetailMenuJobTab.JobParts>) this.jobChange_.parts).Skip<DetailMenuJobTab.JobParts>(array2.Length).Select<DetailMenuJobTab.JobParts, GameObject>((Func<DetailMenuJobTab.JobParts, GameObject>) (x => x.top)).SetActives(false);
    this.resetTab();
    return true;
  }

  public void changeTab(DetailMenuJobTab.TabMode mode)
  {
    this.tabMode_ = mode;
    this.resetTab();
  }

  private void resetTab()
  {
    ((IEnumerable<GameObject>) new GameObject[2]
    {
      this.jobAbility_.top,
      this.jobChange_.top
    }).ToggleOnceEx((int) this.tabMode_);
  }

  public void onClickedJobAbility() => this.onClickedTab(DetailMenuJobTab.TabMode.JobAbility);

  public void onClickedJobChange() => this.onClickedTab(DetailMenuJobTab.TabMode.JobChange);

  private void onClickedTab(DetailMenuJobTab.TabMode mode)
  {
    Unit0042Menu inParents = NGUITools.FindInParents<Unit0042Menu>(((Component) this).gameObject);
    if (Object.op_Inequality((Object) inParents, (Object) null))
      inParents.UpdateInfoIndicator(mode);
    else
      this.changeTab(mode);
  }

  public enum TabMode
  {
    JobAbility,
    JobChange,
  }

  [Serializable]
  private class AbilityTag
  {
    public GameObject top;
    public UILabel txtLevel;
    public UILabel txtName;
  }

  [Serializable]
  private class JobParts
  {
    public GameObject top;
    public GameObject topNormal;
    public GameObject topX;
    public UILabel txtRank;
    public UILabel txtName;
    public DetailMenuJobTab.AbilityTag[] abilityTags;
    public UILabel txtMaxLevel;
    public UILabel txtProficiency;
    public UIButton button;
  }

  [Serializable]
  private class JobChangePanel
  {
    public GameObject top;
    public DetailMenuJobTab.JobParts[] parts;
  }

  [Serializable]
  private class JobAbilityPanel
  {
    public GameObject top;
    public DetailMenuJobAbilityParts[] abilities;
  }
}
