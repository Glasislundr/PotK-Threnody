// Decompiled with JetBrains decompiler
// Type: SM.Bonus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace SM
{
  [Serializable]
  public class Bonus : KeyCompare
  {
    public int category;
    public int value;
    public int remaining_time;
    public int logic;
    public int _skill;
    public int id;

    private static bool CompFromString(Bonus.CompFunc func, string compValue)
    {
      int result = 0;
      return int.TryParse(compValue, out result) && func(result);
    }

    public static bool IsEnableBonus(BL.Unit unit, Bonus bonus, string today)
    {
      Bonus.CompFunc func = (Bonus.CompFunc) null;
      if (bonus.logic == 1)
        func = (Bonus.CompFunc) (value => bonus.value == value);
      else if (bonus.logic == 2)
        func = (Bonus.CompFunc) (value => bonus.value >= value);
      else if (bonus.logic == 3)
        func = (Bonus.CompFunc) (value => bonus.value <= value);
      if (func == null)
        return false;
      switch (bonus.category)
      {
        case 1:
          return func(unit.playerUnit.unit_type.ID);
        case 2:
          return func(unit.unit.job.ID);
        case 3:
          return ((IEnumerable<UnitFamily>) unit.playerUnit.Families).Any<UnitFamily>((Func<UnitFamily, bool>) (x => func((int) x)));
        case 4:
          return func(unit.unit.kind.ID);
        case 5:
          ColosseumBonusBloodType colosseumBonusBloodType = ((IEnumerable<ColosseumBonusBloodType>) MasterData.ColosseumBonusBloodTypeList).Where<ColosseumBonusBloodType>((Func<ColosseumBonusBloodType, bool>) (x => x.name == unit.unit.character.blood_type)).FirstOrDefault<ColosseumBonusBloodType>();
          return colosseumBonusBloodType != null && func(colosseumBonusBloodType.value);
        case 6:
          ColosseumBonusZodiacType colosseumBonusZodiacType = ((IEnumerable<ColosseumBonusZodiacType>) MasterData.ColosseumBonusZodiacTypeList).Where<ColosseumBonusZodiacType>((Func<ColosseumBonusZodiacType, bool>) (x => x.name == unit.unit.character.zodiac_sign)).FirstOrDefault<ColosseumBonusZodiacType>();
          return colosseumBonusZodiacType != null && func(colosseumBonusZodiacType.value);
        case 7:
          return Bonus.CompFromString(func, unit.unit.character.height);
        case 8:
          return Bonus.CompFromString(func, unit.unit.character.weight);
        case 9:
          return Bonus.CompFromString(func, unit.unit.character.bust);
        case 10:
          return Bonus.CompFromString(func, unit.unit.character.waist);
        case 11:
          return Bonus.CompFromString(func, unit.unit.character.hip);
        case 12:
          return unit.unit.character.birthday == today;
        case 13:
          return func((int) unit.playerUnit.GetElement());
        case 14:
          UnitGroup unitGroup1 = ((IEnumerable<UnitGroup>) MasterData.UnitGroupList).FirstOrDefault<UnitGroup>((Func<UnitGroup, bool>) (x => x.unit_id == unit.unit.ID));
          return unitGroup1 != null && func(unitGroup1.group_large_category_id.ID);
        case 15:
          UnitGroup unitGroup2 = ((IEnumerable<UnitGroup>) MasterData.UnitGroupList).FirstOrDefault<UnitGroup>((Func<UnitGroup, bool>) (x => x.unit_id == unit.unit.ID));
          return unitGroup2 != null && func(unitGroup2.group_small_category_id.ID);
        case 16:
          UnitGroup unitGroup3 = ((IEnumerable<UnitGroup>) MasterData.UnitGroupList).FirstOrDefault<UnitGroup>((Func<UnitGroup, bool>) (x => x.unit_id == unit.unit.ID));
          if (unitGroup3 == null)
            return false;
          return func(unitGroup3.group_clothing_category_id.ID) || func(unitGroup3.group_clothing_category_id_2.ID);
        case 17:
          UnitGroup unitGroup4 = ((IEnumerable<UnitGroup>) MasterData.UnitGroupList).FirstOrDefault<UnitGroup>((Func<UnitGroup, bool>) (x => x.unit_id == unit.unit.ID));
          return unitGroup4 != null && func(unitGroup4.group_generation_category_id.ID);
        default:
          return false;
      }
    }

    public string DisplayName(bool isPvp = false)
    {
      if (isPvp)
      {
        PvpBonus pvpBonus = ((IEnumerable<PvpBonus>) MasterData.PvpBonusList).FirstOrDefault<PvpBonus>((Func<PvpBonus, bool>) (x => x.ID == this.id));
        if (pvpBonus != null)
          return pvpBonus.disp_text + this.skill.name;
      }
      else
      {
        ColosseumBonus colosseumBonus = ((IEnumerable<ColosseumBonus>) MasterData.ColosseumBonusList).FirstOrDefault<ColosseumBonus>((Func<ColosseumBonus, bool>) (x => x.ID == this.id));
        if (colosseumBonus != null)
          return colosseumBonus.disp_text + this.skill.name;
      }
      return "";
    }

    public string RemainingTime() => Bonus.ToRemainingTime(this.remaining_time);

    public static string ToRemainingTime(int remaining_time)
    {
      int num1 = remaining_time / 1440;
      int num2 = remaining_time / 60;
      int num3 = remaining_time % 60;
      string str = "";
      string remainingTime;
      if (num1 > 0)
        remainingTime = str + Consts.Format(Consts.GetInstance().COLOSSEUM_BONUS_DAY, (IDictionary) new Hashtable()
        {
          {
            (object) "day",
            (object) num1
          }
        });
      else if (num2 > 0)
        remainingTime = str + Consts.Format(Consts.GetInstance().COLOSSEUM_BONUS_HOUR, (IDictionary) new Hashtable()
        {
          {
            (object) "hour",
            (object) num2
          }
        });
      else
        remainingTime = str + Consts.Format(Consts.GetInstance().COLOSSEUM_BONUS_MINUE, (IDictionary) new Hashtable()
        {
          {
            (object) "min",
            (object) num3
          }
        });
      return remainingTime;
    }

    public BattleskillSkill skill
    {
      get
      {
        if (MasterData.BattleskillSkill.ContainsKey(this._skill))
          return MasterData.BattleskillSkill[this._skill];
        Debug.LogError((object) ("Key not Found: MasterData.BattleskillSkill[" + (object) this._skill + "]"));
        return (BattleskillSkill) null;
      }
    }

    public Bonus()
    {
    }

    public Bonus(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.category = (int) (long) json[nameof (category)];
      this.value = (int) (long) json[nameof (value)];
      this.remaining_time = (int) (long) json[nameof (remaining_time)];
      this.logic = (int) (long) json[nameof (logic)];
      this._skill = (int) (long) json[nameof (skill)];
      this.id = (int) (long) json[nameof (id)];
    }

    private delegate bool CompFunc(int value);
  }
}
