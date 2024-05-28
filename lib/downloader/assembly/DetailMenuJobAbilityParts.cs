// Decompiled with JetBrains decompiler
// Type: DetailMenuJobAbilityParts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

#nullable disable
[AddComponentMenu("PUNK Scripts/Detail/DetailMenuJobAbilityParts")]
public class DetailMenuJobAbilityParts : MonoBehaviour
{
  [SerializeField]
  private UILabel txt_JobAbilityLv;
  [SerializeField]
  private UILabel txt_JobAbilityName;
  [SerializeField]
  private GameObject master_icon;
  [SerializeField]
  private GameObject not_master_icon;
  [SerializeField]
  private UILabel txt_MasterBonus;
  [SerializeField]
  private GameObject dir_LevelUp;

  public bool hasOnClickLevelUp => Object.op_Implicit((Object) this.dir_LevelUp);

  public List<EventDelegate> onClickLevelUp
  {
    get => this.dir_LevelUp.GetComponentInChildren<UIButton>(true).onClick;
  }

  public bool Initialize(PlayerUnitJob_abilities jobAbility, bool diffView = false, bool bActiveLevelUp = false)
  {
    BattleskillSkill skill = jobAbility.skill;
    if (skill == null)
      return false;
    this.txt_JobAbilityName.SetText((diffView ? Consts.GetInstance().JOBCHANGE_CHANGED_VALUE_COLOR : "") + skill.name);
    if (skill.upper_level > 0)
      this.txt_JobAbilityLv.SetTextLocalize(jobAbility.level);
    else
      this.txt_JobAbilityLv.SetTextLocalize(Consts.GetInstance().SKILL_LEVEL_NONE);
    bool flag = jobAbility.level >= skill.upper_level;
    if (Object.op_Inequality((Object) this.master_icon, (Object) null))
      this.master_icon.SetActive(flag);
    if (Object.op_Inequality((Object) this.not_master_icon, (Object) null))
      this.not_master_icon.SetActive(!flag);
    if (Object.op_Inequality((Object) this.txt_MasterBonus, (Object) null))
    {
      ((UIWidget) this.txt_MasterBonus).color = Color32.op_Implicit(flag ? new Color32(byte.MaxValue, byte.MaxValue, (byte) 0, byte.MaxValue) : new Color32((byte) 175, (byte) 175, (byte) 175, byte.MaxValue));
      JobCharacteristics master = jobAbility.master;
      this.txt_MasterBonus.SetTextLocalize(string.Format("{0}{1}{2}", (object) this.GenBonusText(master.levelmax_bonus, master.levelmax_bonus_value, false), (object) this.GenBonusText(master.levelmax_bonus2, master.levelmax_bonus_value2, true), (object) this.GenBonusText(master.levelmax_bonus3, master.levelmax_bonus_value3, true)));
    }
    if (Object.op_Implicit((Object) this.dir_LevelUp))
    {
      if (bActiveLevelUp & flag)
        bActiveLevelUp = false;
      this.dir_LevelUp.SetActive(bActiveLevelUp);
    }
    return true;
  }

  private string GenBonusText(JobCharacteristicsLevelmaxBonus bonus, int value, bool needSpace)
  {
    if (value < 1)
      return string.Empty;
    StringBuilder stringBuilder = new StringBuilder();
    if (needSpace)
      stringBuilder.Append("    ");
    switch (bonus)
    {
      case JobCharacteristicsLevelmaxBonus.none:
        return string.Empty;
      case JobCharacteristicsLevelmaxBonus.hp_add:
        stringBuilder.Append("HP");
        break;
      case JobCharacteristicsLevelmaxBonus.strength_add:
        stringBuilder.Append("力");
        break;
      case JobCharacteristicsLevelmaxBonus.intelligence_add:
        stringBuilder.Append("魔");
        break;
      case JobCharacteristicsLevelmaxBonus.vitality_add:
        stringBuilder.Append("守");
        break;
      case JobCharacteristicsLevelmaxBonus.mind_add:
        stringBuilder.Append("精");
        break;
      case JobCharacteristicsLevelmaxBonus.agility_add:
        stringBuilder.Append("速");
        break;
      case JobCharacteristicsLevelmaxBonus.dexterity_add:
        stringBuilder.Append("技");
        break;
      case JobCharacteristicsLevelmaxBonus.lucky_add:
        stringBuilder.Append("運");
        break;
      case JobCharacteristicsLevelmaxBonus.movement_add:
        stringBuilder.Append("移動");
        break;
    }
    stringBuilder.Append(string.Format("+{0}", (object) value));
    return stringBuilder.ToString();
  }
}
