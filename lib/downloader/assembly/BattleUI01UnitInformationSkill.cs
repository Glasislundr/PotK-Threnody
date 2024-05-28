// Decompiled with JetBrains decompiler
// Type: BattleUI01UnitInformationSkill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnitStatusInformation;
using UnityEngine;

#nullable disable
public class BattleUI01UnitInformationSkill : BattleUI01UnitInformationTab
{
  [SerializeField]
  private GameObject[] links_;

  public override IEnumerator initialize()
  {
    BattleUI01UnitInformationSkill informationSkill = this;
    int sIdx = 0;
    UnitParameter unitParameter = new UnitParameter(informationSkill.unit_);
    List<PopupSkillDetails.Param> popupParams = new List<PopupSkillDetails.Param>();
    UnitParameter.SkillSortUnit[] skillSortUnitArray = unitParameter.sortedSkills;
    for (int index = 0; index < skillSortUnitArray.Length; ++index)
    {
      UnitParameter.SkillSortUnit skillSortUnit = skillSortUnitArray[index];
      if (informationSkill.links_.Length > sIdx)
      {
        popupParams.Add(skillSortUnit.toPopupParam);
        switch (skillSortUnit.group)
        {
          case UnitParameter.SkillGroup.Leader:
            yield return (object) informationSkill.main_.LoadLSSkillIcon(informationSkill.links_[sIdx++], skillSortUnit.leaderSkill);
            break;
          case UnitParameter.SkillGroup.Element:
            yield return (object) informationSkill.main_.createBattleSkillIcon(informationSkill.links_[sIdx++], skillSortUnit.elementSkill);
            break;
          case UnitParameter.SkillGroup.Multi:
            yield return (object) informationSkill.main_.createBattleSkillIcon(informationSkill.links_[sIdx++], skillSortUnit.multiSkill);
            break;
          case UnitParameter.SkillGroup.Overkillers:
            yield return (object) informationSkill.main_.createBattleSkillIcon(informationSkill.links_[sIdx++], skillSortUnit.overkillersSkill);
            break;
          case UnitParameter.SkillGroup.Release:
            yield return (object) informationSkill.main_.createBattleSkillIcon(informationSkill.links_[sIdx++], skillSortUnit.releaseSkill);
            break;
          case UnitParameter.SkillGroup.Command:
            yield return (object) informationSkill.main_.createBattleSkillIcon(informationSkill.links_[sIdx++], skillSortUnit.commandSkill);
            break;
          case UnitParameter.SkillGroup.Princess:
            yield return (object) informationSkill.main_.createBattleSkillIcon(informationSkill.links_[sIdx++], skillSortUnit.princessSkill);
            break;
          case UnitParameter.SkillGroup.Grant:
            yield return (object) informationSkill.main_.createBattleSkillIcon(informationSkill.links_[sIdx++], skillSortUnit.grantSkill);
            break;
          case UnitParameter.SkillGroup.Duel:
            yield return (object) informationSkill.main_.createBattleSkillIcon(informationSkill.links_[sIdx++], skillSortUnit.duelSkill);
            break;
          case UnitParameter.SkillGroup.Equip:
            yield return (object) informationSkill.main_.createBattleSkillIcon(informationSkill.links_[sIdx++], skillSortUnit.equipSkill);
            break;
          case UnitParameter.SkillGroup.Extra:
            yield return (object) informationSkill.main_.LoadExtraSkillIcon(informationSkill.links_[sIdx++], skillSortUnit.extraSkill);
            break;
          case UnitParameter.SkillGroup.JobAbility:
            yield return (object) informationSkill.main_.createJobAbilityIcon(informationSkill.links_[sIdx++], skillSortUnit.jobAbility);
            break;
          case UnitParameter.SkillGroup.Reisou:
            yield return (object) informationSkill.main_.createBattleSkillIcon(informationSkill.links_[sIdx++], skillSortUnit.reisouSkill);
            break;
          case UnitParameter.SkillGroup.SEA:
            yield return (object) informationSkill.main_.createAndSetEventBattleSkillIcon(informationSkill.links_[sIdx++], skillSortUnit.SEASkill);
            break;
        }
      }
      else
        break;
    }
    skillSortUnitArray = (UnitParameter.SkillSortUnit[]) null;
    for (; sIdx < informationSkill.links_.Length; ++sIdx)
      informationSkill.links_[sIdx].SetActive(false);
    informationSkill.main_.setPopupSkillParams(popupParams.ToArray());
  }

  public override BattleUI01UnitInformationTab.Type type => BattleUI01UnitInformationTab.Type.Skill;
}
