// Decompiled with JetBrains decompiler
// Type: PopupRecommendPartJobChange
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnitStatusInformation;
using UnityEngine;

#nullable disable
[AddComponentMenu("Popup/Recommend/JobChange")]
public class PopupRecommendPartJobChange : PopupRecommendPart
{
  [SerializeField]
  private PopupRecommendPartJobChange.TabContainer[] tabs_;
  private bool isWait_;
  private int current_;
  private MasterDataTable.UnitJob[] jobs_;
  private GameObject genrePrefab_;
  private Dictionary<int, List<PopupSkillDetails.Param>> dicSkillParams_ = new Dictionary<int, List<PopupSkillDetails.Param>>();

  public override IEnumerator doInitialize(PlayerUnit playerUnit, UnitUnit target)
  {
    PopupRecommendPartJobChange recommendPartJobChange = this;
    recommendPartJobChange.playerUnit_ = playerUnit;
    recommendPartJobChange.target_ = target;
    recommendPartJobChange.jobs_ = JobChangeUtil.getJobs(target);
    if (recommendPartJobChange.jobs_.Length == 0)
    {
      ((Component) recommendPartJobChange).gameObject.SetActive(false);
    }
    else
    {
      UnitRecommend unitRecommend;
      int num = !MasterData.UnitRecommend.TryGetValue(target.same_character_id, out unitRecommend) ? 0 : (unitRecommend.job_UnitJob.HasValue ? unitRecommend.job_UnitJob.Value : 0);
      int index;
      for (index = 0; index < recommendPartJobChange.jobs_.Length; ++index)
      {
        PopupRecommendPartJobChange.TabContainer tab = recommendPartJobChange.tabs_[index];
        tab.top.SetActive(true);
        recommendPartJobChange.setEventClickedTab(tab.button, index);
        tab.badge.SetActive(recommendPartJobChange.jobs_[index].ID == num);
        tab.topSkills.SetActive(false);
      }
      for (; index < recommendPartJobChange.tabs_.Length; ++index)
      {
        recommendPartJobChange.tabs_[index].top.SetActive(false);
        recommendPartJobChange.tabs_[index].topSkills.SetActive(false);
      }
      recommendPartJobChange.current_ = -1;
      yield return (object) recommendPartJobChange.doChangeTab(0);
    }
  }

  private void setEventClickedTab(UIButton btn, int index)
  {
    EventDelegate.Set(btn.onClick, (EventDelegate.Callback) (() => this.onClickedTab(index)));
  }

  private void onClickedTab(int index)
  {
    if (this.current_ == index || this.isWait_)
      return;
    this.StartCoroutine(this.doChangeTab(index));
  }

  private IEnumerator doChangeTab(int next)
  {
    this.isWait_ = true;
    int old = this.current_;
    PopupRecommendPartJobChange.TabContainer nextTab = this.tabs_[next];
    if (nextTab.jobAblilities == null)
    {
      nextTab.jobAblilities = ((IEnumerable<JobCharacteristics>) this.jobs_[next].JobAbilities).Where<JobCharacteristics>((Func<JobCharacteristics, bool>) (x =>
      {
        BattleskillSkill skill = x.skill;
        return 0 < (skill != null ? skill.upper_level : 0);
      })).ToArray<JobCharacteristics>();
      nextTab.topSkills.GetComponent<UIRect>().alpha = 0.0f;
      nextTab.topSkills.SetActive(true);
      int no = 0;
      int minLength = Mathf.Min(nextTab.jobAblilities.Length, nextTab.skills.Length);
      this.dicSkillParams_[next] = new List<PopupSkillDetails.Param>(minLength);
      for (; no < minLength; ++no)
        yield return (object) this.doInitJobAbility(nextTab.skills[no], nextTab.jobAblilities[no], next, no);
      for (; no < nextTab.skills.Length; ++no)
        nextTab.skills[no].top.SetActive(false);
      nextTab.topSkills.GetComponent<UIRect>().alpha = 1f;
    }
    if (old >= 0 && old != next)
    {
      ((UIButtonColor) this.tabs_[old].button).isEnabled = true;
      this.tabs_[old].topSkills.SetActive(false);
    }
    ((UIButtonColor) nextTab.button).isEnabled = false;
    nextTab.topSkills.SetActive(true);
    this.current_ = next;
    this.isWait_ = false;
  }

  private IEnumerator doInitJobAbility(
    PopupRecommendPartJobChange.SkillContainer container,
    JobCharacteristics skill,
    int tabNo,
    int index)
  {
    PopupRecommendPartJobChange recommendPartJobChange = this;
    BattleskillSkill s = skill.skill;
    if (Object.op_Equality((Object) recommendPartJobChange.genrePrefab_, (Object) null))
    {
      Future<GameObject> ld = Res.Icons.SkillGenreIcon.Load<GameObject>();
      yield return (object) ld.Wait();
      recommendPartJobChange.genrePrefab_ = ld.Result;
      ld = (Future<GameObject>) null;
    }
    int? level = Array.Find<PlayerUnitJob_abilities>(recommendPartJobChange.playerUnit_.job_abilities ?? new PlayerUnitJob_abilities[0], (Predicate<PlayerUnitJob_abilities>) (x => x.job_ability_id == skill.ID))?.level;
    if (!level.HasValue)
      level = Array.Find<PlayerUnitAll_saved_job_abilities>(recommendPartJobChange.playerUnit_.all_saved_job_abilities ?? new PlayerUnitAll_saved_job_abilities[0], (Predicate<PlayerUnitAll_saved_job_abilities>) (x => x.job_ability_id == skill.ID))?.level;
    container.txtName.SetTextLocalize(s.name);
    container.txtDescription.SetHeadline(s.description);
    container.txtLevel.SetTextLocalize(string.Format("{0}/{1}", (object) (level.HasValue ? level.Value : 0), (object) s.upper_level));
    BattleskillGenre? genre1 = s.genre1;
    BattleskillGenre? genre2 = s.genre2;
    ((Component) container.genre1).gameObject.SetActive(genre1.HasValue);
    if (genre1.HasValue)
    {
      SkillGenreIcon component = recommendPartJobChange.genrePrefab_.Clone(container.genre1).GetComponent<SkillGenreIcon>();
      component.Init(genre1);
      ((UIWidget) component.iconSprite).depth = container.depth + 1;
    }
    ((Component) container.genre2).gameObject.SetActive(genre2.HasValue);
    if (genre2.HasValue)
    {
      SkillGenreIcon component = recommendPartJobChange.genrePrefab_.Clone(container.genre2).GetComponent<SkillGenreIcon>();
      component.Init(genre2);
      ((UIWidget) component.iconSprite).depth = container.depth + 1;
    }
    recommendPartJobChange.dicSkillParams_[tabNo].Add(new PopupSkillDetails.Param(s, UnitParameter.SkillGroup.JobAbility, level));
    EventDelegate.Set(container.button.onClick, (EventDelegate.Callback) (() =>
    {
      if (Object.op_Equality((Object) this.menu.skillDetailPrefab, (Object) null))
        return;
      PopupSkillDetails.show(this.menu.skillDetailPrefab, this.dicSkillParams_[tabNo].ToArray(), index);
    }));
  }

  [Serializable]
  private class SkillContainer
  {
    public GameObject top;
    public int depth;
    public UILabel txtName;
    public HeadlineLabel txtDescription;
    public UILabel txtLevel;
    public UIButton button;
    public Transform genre1;
    public Transform genre2;
  }

  [Serializable]
  private class TabContainer
  {
    public GameObject top;
    public UIButton button;
    public GameObject badge;
    public GameObject topSkills;
    public PopupRecommendPartJobChange.SkillContainer[] skills;

    public JobCharacteristics[] jobAblilities { get; set; }
  }
}
