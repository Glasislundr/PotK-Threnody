// Decompiled with JetBrains decompiler
// Type: Unit004StatusDetailXDialog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using UnityEngine;

#nullable disable
public class Unit004StatusDetailXDialog : Unit004StatusDetailDialog
{
  public override void Initialize(PlayerUnit playerUnit, Sprite unitLargeSprite, bool isMemory = false)
  {
    this.SetUnitSprite(playerUnit, unitLargeSprite);
    this.SetUnitStatus(playerUnit, isMemory);
    this.StatusDetailList.Add(this.dir_status_hp);
    this.StatusDetailList.Add(this.dir_status_attack);
    this.StatusDetailList.Add(this.dir_status_magic_attack);
    this.StatusDetailList.Add(this.dir_status_defense);
    this.StatusDetailList.Add(this.dir_status_mental);
    this.StatusDetailList.Add(this.dir_status_speed);
    this.StatusDetailList.Add(this.dir_status_technique);
    this.StatusDetailList.Add(this.dir_status_lucky);
    this.SetComposeAddValue(playerUnit);
    this.SetJobAbilityMasterBonusValue(playerUnit);
    this.SetXLevelUpParametarValue(playerUnit);
  }

  protected override void SetComposeAddValue(PlayerUnit playerUnit)
  {
    if (playerUnit.unit.compose_max_unity_value_setting_id_ComposeMaxUnityValueSetting <= 0)
    {
      for (int index = 0; index < this.StatusDetailList.Count; ++index)
      {
        this.StatusDetailList[index].Txt_touta.SetTextLocalize("(-");
        this.StatusDetailList[index].Txt_touta_max.SetTextLocalize("/-)");
      }
    }
    else
    {
      int[] numArray1 = new int[8]
      {
        playerUnit.UnitTypeParameter.hp_compose_max,
        playerUnit.UnitTypeParameter.strength_compose_max,
        playerUnit.UnitTypeParameter.intelligence_compose_max,
        playerUnit.UnitTypeParameter.vitality_compose_max,
        playerUnit.UnitTypeParameter.mind_compose_max,
        playerUnit.UnitTypeParameter.agility_compose_max,
        playerUnit.UnitTypeParameter.dexterity_compose_max,
        playerUnit.UnitTypeParameter.lucky_compose_max
      };
      int[] numArray2 = new int[8]
      {
        playerUnit.getComposeAddValue(PlayerUnit.ParamType.HP),
        playerUnit.getComposeAddValue(PlayerUnit.ParamType.STRENGTH),
        playerUnit.getComposeAddValue(PlayerUnit.ParamType.INTELLIGENCE),
        playerUnit.getComposeAddValue(PlayerUnit.ParamType.VITALITY),
        playerUnit.getComposeAddValue(PlayerUnit.ParamType.MIND),
        playerUnit.getComposeAddValue(PlayerUnit.ParamType.AGILITY),
        playerUnit.getComposeAddValue(PlayerUnit.ParamType.DEXTERITY),
        playerUnit.getComposeAddValue(PlayerUnit.ParamType.LUCKY)
      };
      int[] numArray3 = new int[8]
      {
        playerUnit.getComposeAddMax(PlayerUnit.ParamType.HP),
        playerUnit.getComposeAddMax(PlayerUnit.ParamType.STRENGTH),
        playerUnit.getComposeAddMax(PlayerUnit.ParamType.INTELLIGENCE),
        playerUnit.getComposeAddMax(PlayerUnit.ParamType.VITALITY),
        playerUnit.getComposeAddMax(PlayerUnit.ParamType.MIND),
        playerUnit.getComposeAddMax(PlayerUnit.ParamType.AGILITY),
        playerUnit.getComposeAddMax(PlayerUnit.ParamType.DEXTERITY),
        playerUnit.getComposeAddMax(PlayerUnit.ParamType.LUCKY)
      };
      for (int index = 0; index < this.StatusDetailList.Count; ++index)
      {
        this.StatusDetailList[index].Txt_combine_max.SetTextLocalize(string.Format("/{0}", (object) (numArray1[index] + numArray2[index])));
        this.StatusDetailList[index].Txt_touta.SetTextLocalize(string.Format("({0}", (object) numArray2[index]));
        this.StatusDetailList[index].Txt_touta_max.SetTextLocalize(string.Format("/{0})", (object) numArray3[index]));
      }
    }
  }

  protected void SetJobAbilityMasterBonusValue(PlayerUnit playerUnit)
  {
    JobCharacteristics[] allJobAbilities = JobChangeUtil.getAllJobAbilities(playerUnit);
    if (allJobAbilities.Length < 1)
    {
      for (int index = 0; index < this.StatusDetailList.Count; ++index)
      {
        this.StatusDetailList[index].Txt_master_bonus.SetTextLocalize("-");
        this.StatusDetailList[index].Txt_master_bonus_max.SetTextLocalize("/-");
      }
    }
    else
    {
      int[] numArray1 = new int[Enum.GetNames(typeof (JobCharacteristicsLevelmaxBonus)).Length];
      foreach (JobCharacteristics jobCharacteristics in allJobAbilities)
      {
        numArray1[(int) jobCharacteristics.levelmax_bonus] += jobCharacteristics.levelmax_bonus_value;
        numArray1[(int) jobCharacteristics.levelmax_bonus2] += jobCharacteristics.levelmax_bonus_value2;
        numArray1[(int) jobCharacteristics.levelmax_bonus3] += jobCharacteristics.levelmax_bonus_value3;
      }
      int[] numArray2 = new int[8]
      {
        playerUnit.bonus_hp,
        playerUnit.bonus_strength,
        playerUnit.bonus_intelligence,
        playerUnit.bonus_vitality,
        playerUnit.bonus_mind,
        playerUnit.bonus_agility,
        playerUnit.bonus_dexterity,
        playerUnit.bonus_lucky
      };
      int[] numArray3 = new int[8]
      {
        numArray1[1],
        numArray1[2],
        numArray1[3],
        numArray1[4],
        numArray1[5],
        numArray1[6],
        numArray1[7],
        numArray1[8]
      };
      for (int index = 0; index < this.StatusDetailList.Count; ++index)
      {
        this.StatusDetailList[index].Txt_master_bonus.SetTextLocalize(string.Format("{0}", (object) numArray2[index]));
        this.StatusDetailList[index].Txt_master_bonus_max.SetTextLocalize(string.Format("/{0}", (object) numArray3[index]));
        this.StatusDetailList[index].Max_value += numArray3[index];
      }
    }
  }

  protected void SetXLevelUpParametarValue(PlayerUnit playerUnit)
  {
    XLevelStatus xlevelStatus;
    if (!playerUnit.hasXLevel || !MasterData.XLevelStatus.TryGetValue(playerUnit.unit.ID, out xlevelStatus))
    {
      for (int index = 0; index < this.StatusDetailList.Count; ++index)
      {
        this.StatusDetailList[index].Txt_X_levelup.SetTextLocalize("-");
        this.StatusDetailList[index].Txt_X_levelup_max.SetTextLocalize("/-");
      }
    }
    else
    {
      int[] numArray1 = new int[8]
      {
        playerUnit.hp.x_level,
        playerUnit.strength.x_level,
        playerUnit.intelligence.x_level,
        playerUnit.vitality.x_level,
        playerUnit.mind.x_level,
        playerUnit.agility.x_level,
        playerUnit.dexterity.x_level,
        playerUnit.lucky.x_level
      };
      int[] numArray2 = new int[8]
      {
        xlevelStatus.hp_level_up_max,
        xlevelStatus.strength_level_up_max,
        xlevelStatus.intelligence_level_up_max,
        xlevelStatus.vitality_level_up_max,
        xlevelStatus.mind_level_up_max,
        xlevelStatus.agility_level_up_max,
        xlevelStatus.dexterity_level_up_max,
        xlevelStatus.lucky_level_up_max
      };
      for (int index = 0; index < this.StatusDetailList.Count; ++index)
      {
        this.StatusDetailList[index].Txt_X_levelup.SetTextLocalize(string.Format("{0}", (object) numArray1[index]));
        this.StatusDetailList[index].Txt_X_levelup_max.SetTextLocalize(string.Format("/{0}", (object) numArray2[index]));
        this.StatusDetailList[index].Max_value += numArray2[index];
      }
    }
  }
}
