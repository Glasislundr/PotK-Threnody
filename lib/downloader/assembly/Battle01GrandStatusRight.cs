// Decompiled with JetBrains decompiler
// Type: Battle01GrandStatusRight
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
public class Battle01GrandStatusRight : NGBattleMenuBase
{
  [SerializeField]
  private UILabel unit;
  [SerializeField]
  private UILabel item;
  [SerializeField]
  private UILabel zeny;
  private BL.BattleModified<BL.StructValue<int>> dropUnitModified;
  private BL.BattleModified<BL.StructValue<int>> dropItemModified;
  private BL.BattleModified<BL.StructValue<long>> dropMoneyModified;

  public override IEnumerator onInitAsync()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Battle01GrandStatusRight grandStatusRight = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    grandStatusRight.dropMoneyModified = BL.Observe<BL.StructValue<long>>(grandStatusRight.env.core.dropMoney);
    grandStatusRight.dropItemModified = BL.Observe<BL.StructValue<int>>(grandStatusRight.env.core.dropItem);
    grandStatusRight.dropUnitModified = BL.Observe<BL.StructValue<int>>(grandStatusRight.env.core.dropUnit);
    return false;
  }

  protected override void LateUpdate_Battle()
  {
    if (this.dropMoneyModified.isChangedOnce())
    {
      Decimal num1 = (Decimal) this.dropMoneyModified.value.value;
      Decimal num2 = 1.0M;
      Decimal num3 = 0M;
      foreach (var data in this.env.core.playerUnits.value.SelectMany(u =>
      {
        List<Tuple<BattleskillSkill, int, int>> second = new List<Tuple<BattleskillSkill, int, int>>();
        if (u.is_leader || u.friend || u.playerUnit.is_enemy_leader || u.playerUnit.is_guest && u.is_helper)
          second.AddRange(((IEnumerable<PlayerUnitLeader_skills>) u.playerUnit.leader_skills).Select<PlayerUnitLeader_skills, Tuple<BattleskillSkill, int, int>>((Func<PlayerUnitLeader_skills, Tuple<BattleskillSkill, int, int>>) (skill => new Tuple<BattleskillSkill, int, int>(skill.skill, skill.level, 0))));
        return ((IEnumerable<Tuple<BattleskillSkill, int, int>>) u.unitAndGearSkills).Concat<Tuple<BattleskillSkill, int, int>>((IEnumerable<Tuple<BattleskillSkill, int, int>>) second).Where<Tuple<BattleskillSkill, int, int>>((Func<Tuple<BattleskillSkill, int, int>, bool>) (x => x.Item1.skill_type == BattleskillSkillType.passive || x.Item1.skill_type == BattleskillSkillType.leader)).SelectMany(x => ((IEnumerable<BattleskillEffect>) x.Item1.Effects).Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (effect => effect.EffectLogic.Enum == BattleskillEffectLogicEnum.ratio_money && effect.checkLevel(x.Item2))).Select(effect => new
        {
          effect = effect,
          level = x.Item2
        }));
      }))
      {
        num2 *= (Decimal) data.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) / 100M;
        Decimal num4 = (Decimal) data.effect.GetFloat(BattleskillEffectLogicArgumentEnum.limit);
        if (num4 > num3)
          num3 = num4;
      }
      foreach (var data in this.env.core.playerUnits.value.SelectMany(u =>
      {
        List<Tuple<BattleskillSkill, int, int>> second = new List<Tuple<BattleskillSkill, int, int>>();
        if (u.is_leader || u.friend || u.playerUnit.is_enemy_leader || u.playerUnit.is_guest && u.is_helper)
          second.AddRange(((IEnumerable<PlayerUnitLeader_skills>) u.playerUnit.leader_skills).Select<PlayerUnitLeader_skills, Tuple<BattleskillSkill, int, int>>((Func<PlayerUnitLeader_skills, Tuple<BattleskillSkill, int, int>>) (skill => new Tuple<BattleskillSkill, int, int>(skill.skill, skill.level, 0))));
        List<\u003C\u003Ef__AnonymousType19<BattleskillEffect, int, int>> list = ((IEnumerable<Tuple<BattleskillSkill, int, int>>) u.unitAndGearSkills).Concat<Tuple<BattleskillSkill, int, int>>((IEnumerable<Tuple<BattleskillSkill, int, int>>) second).Where<Tuple<BattleskillSkill, int, int>>((Func<Tuple<BattleskillSkill, int, int>, bool>) (x => x.Item1.skill_type == BattleskillSkillType.passive || x.Item1.skill_type == BattleskillSkillType.leader)).SelectMany(x => ((IEnumerable<BattleskillEffect>) x.Item1.Effects).Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (effect =>
        {
          if (effect.EffectLogic.Enum != BattleskillEffectLogicEnum.ratio_money2 || !effect.checkLevel(x.Item2) || effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != u.playerUnit.GetElement() || effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) != 0 && !u.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id)) || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != u.unit.kind.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id) != 0 && !u.originalUnit.unit.HasSkillGroupId(effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id)))
            return false;
          return effect.GetInt(BattleskillEffectLogicArgumentEnum.exclude_skill_group_id) == 0 || !u.originalUnit.unit.HasSkillGroupId(effect.GetInt(BattleskillEffectLogicArgumentEnum.exclude_skill_group_id));
        })).Select(effect => new
        {
          effect = effect,
          level = x.Item2,
          gearIndex = x.Item3
        })).ToList();
        if (list.Any(x => x.gearIndex == 1))
          list.RemoveAll(x => x.gearIndex == 2);
        return list;
      }))
      {
        num2 += ((Decimal) data.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) + (Decimal) data.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio) * (Decimal) data.level - 100M) / 100M;
        Decimal num5 = (Decimal) data.effect.GetFloat(BattleskillEffectLogicArgumentEnum.limit);
        if (num5 > num3)
          num3 = num5;
      }
      if (num3 != 0M)
      {
        Decimal num6 = num3 / 100M;
        if (num2 > num6)
          num2 = num6;
      }
      this.setText(this.zeny, (long) (num1 * num2));
    }
    if (this.dropItemModified.isChangedOnce())
      this.setText(this.item, this.dropItemModified.value.value);
    if (!this.dropUnitModified.isChangedOnce())
      return;
    this.setText(this.unit, this.dropUnitModified.value.value);
  }

  public Transform getLabelTransform(BL.DropData drop)
  {
    switch (drop.reward.Type)
    {
      case MasterDataTable.CommonRewardType.unit:
      case MasterDataTable.CommonRewardType.material_unit:
        return ((Component) this.unit).transform;
      case MasterDataTable.CommonRewardType.supply:
      case MasterDataTable.CommonRewardType.gear:
      case MasterDataTable.CommonRewardType.quest_key:
      case MasterDataTable.CommonRewardType.gacha_ticket:
      case MasterDataTable.CommonRewardType.material_gear:
      case MasterDataTable.CommonRewardType.reincarnation_type_ticket:
      case MasterDataTable.CommonRewardType.gear_body:
        return ((Component) this.item).transform;
      case MasterDataTable.CommonRewardType.money:
        return ((Component) this.zeny).transform;
      default:
        return (Transform) null;
    }
  }
}
