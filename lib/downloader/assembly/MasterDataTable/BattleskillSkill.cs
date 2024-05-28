// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BattleskillSkill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UniLinq;
using UnityEngine;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class BattleskillSkill
  {
    public static readonly ReadOnlyCollection<int> InvestElementSkillIds = new List<int>()
    {
      490000010,
      490000013,
      490000015,
      490000017,
      490000019,
      490000021,
      490000173
    }.AsReadOnly();
    [NonSerialized]
    private bool transformationGroupIdCached;
    [NonSerialized]
    private int? transformationGroupIdValue;
    private bool? isJobAbility_;
    public int ID;
    public string name;
    public string description;
    public string shortDescription;
    public string shortDescriptionEnemy;
    public string viewShortDescription;
    public string viewShortDescriptionEnemy;
    public int skill_type_BattleskillSkillType;
    public int element_CommonElement;
    public int? genre1_BattleskillGenre;
    public int? genre2_BattleskillGenre;
    public int target_type_BattleskillTargetType;
    public int min_range;
    public int max_range;
    public int consume_hp;
    public int weight;
    public int power;
    public int use_count;
    public int charge_turn;
    public string duel_magic_bullet_name;
    public bool variable_magic_bullet_flag;
    public string field_effect_name;
    public string field_target_effect_name;
    public int upper_level;
    public int? field_effect_BattleskillFieldEffect;
    public int? duel_effect_BattleskillDuelEffect;
    public int? passive_effect_BattleskillFieldEffect;
    public bool time_of_death_skill_disable;
    public int? ailment_effect_BattleskillAilmentEffect;
    public bool range_effect_passive_skill;
    public int max_use_count;
    public int awake_skill_category_id;
    public int resource_reference_id;

    public BattleskillEffect[] Effects
    {
      get
      {
        NGBattleManager instance = Singleton<NGBattleManager>.GetInstance();
        return instance.initialized ? instance.GetSkillEffect(this) : MasterData.WhereBattleskillEffectBy(this);
      }
    }

    public bool PassiveSkill
    {
      get
      {
        return this.skill_type == BattleskillSkillType.leader || this.skill_type == BattleskillSkillType.duel || this.skill_type == BattleskillSkillType.passive;
      }
    }

    public bool DispSkillList
    {
      get
      {
        return this.skill_type != BattleskillSkillType.leader && this.skill_type != BattleskillSkillType.magic;
      }
    }

    public bool haveKoyuDuelEffect => this.duel_effect != null && this.duel_effect.isKoyuDuelEffect;

    public int[] InvestSkillIds()
    {
      if (((IEnumerable<BattleskillEffect>) this.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.HasKey(BattleskillEffectLogicArgumentEnum.gda_percentage_invocation) || x.HasKey(BattleskillEffectLogicArgumentEnum.gdd_percentage_invocation))))
      {
        List<int> intList = new List<int>();
        int num1 = 1;
        while (true)
        {
          BattleskillEffectLogicArgumentEnum key = BattleskillEffectLogic.GetLogicArgumentEnum("skill_id" + (object) num1);
          IEnumerable<int> ints = ((IEnumerable<BattleskillEffect>) this.Effects).Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.HasKey(key))).Select<BattleskillEffect, int>((Func<BattleskillEffect, int>) (x =>
          {
            int num2 = x.GetInt(key);
            if (num2 < 0)
              num2 = -num2;
            return num2;
          })).Where<int>((Func<int, bool>) (x => x != 0 && MasterData.BattleskillSkill.ContainsKey(x)));
          if (ints.Any<int>())
          {
            intList.AddRange(ints);
            ++num1;
          }
          else
            break;
        }
        return intList.ToArray();
      }
      BattleskillEffectLogicEnum[] Logics = new BattleskillEffectLogicEnum[8]
      {
        BattleskillEffectLogicEnum.mdmg_combi,
        BattleskillEffectLogicEnum.invest_skilleffect,
        BattleskillEffectLogicEnum.invest_passive,
        BattleskillEffectLogicEnum.anohana_trio,
        BattleskillEffectLogicEnum.combi_attack,
        BattleskillEffectLogicEnum.invest_skilleffect_im,
        BattleskillEffectLogicEnum.passive_invest_passive,
        BattleskillEffectLogicEnum.passive_invest_passive2
      };
      return ((IEnumerable<BattleskillEffect>) this.Effects).Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => ((IEnumerable<BattleskillEffectLogicEnum>) Logics).Contains<BattleskillEffectLogicEnum>(x.effect_logic.Enum))).Select<BattleskillEffect, int>((Func<BattleskillEffect, int>) (x =>
      {
        int num = x.GetInt(BattleskillEffectLogicArgumentEnum.skill_id);
        if (num < 0)
          num = -num;
        return num;
      })).Where<int>((Func<int, bool>) (x => x != 0 && MasterData.BattleskillSkill.ContainsKey(x))).ToArray<int>();
    }

    public int? transformationGroupId
    {
      get
      {
        if (!this.transformationGroupIdCached)
        {
          BattleskillEffect battleskillEffect = ((IEnumerable<BattleskillEffect>) this.Effects).FirstOrDefault<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.effect_logic.Enum == BattleskillEffectLogicEnum.transformation_group));
          this.transformationGroupIdValue = battleskillEffect == null ? new int?() : new int?(battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.transformation_group_id));
          this.transformationGroupIdCached = true;
        }
        return this.transformationGroupIdValue;
      }
    }

    public bool checkEnableUnit(BL.ISkillEffectListUnit unit)
    {
      return BattleFuncs.checkEnableUnitSkill(unit, this);
    }

    public BattleskillTargetType GetRangeEffect(out int minRange, out int maxRange)
    {
      minRange = 0;
      maxRange = 0;
      BattleskillEffect[] effects = this.Effects;
      BattleskillEffectLogicEnum[] source1 = new BattleskillEffectLogicEnum[3]
      {
        BattleskillEffectLogicEnum.range_effect_fix_attack,
        BattleskillEffectLogicEnum.range_effect_ratio_attack,
        BattleskillEffectLogicEnum.range_effect_enemy_invest_skilleffect
      };
      BattleskillEffectLogicEnum[] source2 = new BattleskillEffectLogicEnum[3]
      {
        BattleskillEffectLogicEnum.range_effect_fix_heal,
        BattleskillEffectLogicEnum.range_effect_ratio_heal,
        BattleskillEffectLogicEnum.range_effect_player_invest_skilleffect
      };
      foreach (BattleskillEffect battleskillEffect in effects)
      {
        BattleskillEffectLogicEnum battleskillEffectLogicEnum = battleskillEffect.EffectLogic.Enum;
        if (((IEnumerable<BattleskillEffectLogicEnum>) source1).Contains<BattleskillEffectLogicEnum>(battleskillEffectLogicEnum))
        {
          minRange = battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.min_range);
          maxRange = battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.max_range);
          return BattleskillTargetType.enemy_range;
        }
        if (((IEnumerable<BattleskillEffectLogicEnum>) source2).Contains<BattleskillEffectLogicEnum>(battleskillEffectLogicEnum))
        {
          minRange = battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.min_range);
          maxRange = battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.max_range);
          return BattleskillTargetType.player_range;
        }
      }
      return BattleskillTargetType.myself;
    }

    public bool IsJobAbility
    {
      get
      {
        return !this.isJobAbility_.HasValue ? (this.isJobAbility_ = new bool?(Array.Find<JobCharacteristics>(MasterData.JobCharacteristicsList, (Predicate<JobCharacteristics>) (jc =>
        {
          if (jc.skill_BattleskillSkill == this.ID)
            return true;
          if (!jc.skill2_BattleskillSkill.HasValue)
            return false;
          int id = this.ID;
          int? battleskillSkill = jc.skill2_BattleskillSkill;
          int valueOrDefault = battleskillSkill.GetValueOrDefault();
          return id == valueOrDefault & battleskillSkill.HasValue;
        })) != null)).Value : this.isJobAbility_.Value;
      }
    }

    public bool IsLand
    {
      get
      {
        foreach (BattleskillEffect effect in this.Effects)
        {
          if (BattleFuncs.PackedSkillEffect.Create(effect).HasKey(BattleskillEffectLogicArgumentEnum.land_tag1))
            return true;
        }
        return false;
      }
    }

    public bool IsCallTargetPlayer
    {
      get
      {
        return this.target_type == BattleskillTargetType.myself || this.target_type == BattleskillTargetType.player_range || this.target_type == BattleskillTargetType.player_single;
      }
    }

    public bool IsCallTargetEnemy
    {
      get
      {
        return this.target_type == BattleskillTargetType.enemy_range || this.target_type == BattleskillTargetType.enemy_single;
      }
    }

    public bool IsCallTargetComplex
    {
      get
      {
        return this.target_type == BattleskillTargetType.complex_range || this.target_type == BattleskillTargetType.complex_single;
      }
    }

    public static BattleskillSkill Parse(MasterDataReader reader)
    {
      return new BattleskillSkill()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        description = reader.ReadString(true),
        shortDescription = reader.ReadString(true),
        shortDescriptionEnemy = reader.ReadString(true),
        viewShortDescription = reader.ReadString(true),
        viewShortDescriptionEnemy = reader.ReadString(true),
        skill_type_BattleskillSkillType = reader.ReadInt(),
        element_CommonElement = reader.ReadInt(),
        genre1_BattleskillGenre = reader.ReadIntOrNull(),
        genre2_BattleskillGenre = reader.ReadIntOrNull(),
        target_type_BattleskillTargetType = reader.ReadInt(),
        min_range = reader.ReadInt(),
        max_range = reader.ReadInt(),
        consume_hp = reader.ReadInt(),
        weight = reader.ReadInt(),
        power = reader.ReadInt(),
        use_count = reader.ReadInt(),
        charge_turn = reader.ReadInt(),
        duel_magic_bullet_name = reader.ReadString(true),
        variable_magic_bullet_flag = reader.ReadBool(),
        field_effect_name = reader.ReadString(true),
        field_target_effect_name = reader.ReadString(true),
        upper_level = reader.ReadInt(),
        field_effect_BattleskillFieldEffect = reader.ReadIntOrNull(),
        duel_effect_BattleskillDuelEffect = reader.ReadIntOrNull(),
        passive_effect_BattleskillFieldEffect = reader.ReadIntOrNull(),
        time_of_death_skill_disable = reader.ReadBool(),
        ailment_effect_BattleskillAilmentEffect = reader.ReadIntOrNull(),
        range_effect_passive_skill = reader.ReadBool(),
        max_use_count = reader.ReadInt(),
        awake_skill_category_id = reader.ReadInt(),
        resource_reference_id = reader.ReadInt()
      };
    }

    public BattleskillSkillType skill_type
    {
      get => (BattleskillSkillType) this.skill_type_BattleskillSkillType;
    }

    public CommonElement element => (CommonElement) this.element_CommonElement;

    public BattleskillGenre? genre1
    {
      get
      {
        int? battleskillGenre = this.genre1_BattleskillGenre;
        return !battleskillGenre.HasValue ? new BattleskillGenre?() : new BattleskillGenre?((BattleskillGenre) battleskillGenre.GetValueOrDefault());
      }
    }

    public BattleskillGenre? genre2
    {
      get
      {
        int? battleskillGenre = this.genre2_BattleskillGenre;
        return !battleskillGenre.HasValue ? new BattleskillGenre?() : new BattleskillGenre?((BattleskillGenre) battleskillGenre.GetValueOrDefault());
      }
    }

    public BattleskillTargetType target_type
    {
      get => (BattleskillTargetType) this.target_type_BattleskillTargetType;
    }

    public BattleskillFieldEffect field_effect
    {
      get
      {
        if (!this.field_effect_BattleskillFieldEffect.HasValue)
          return (BattleskillFieldEffect) null;
        BattleskillFieldEffect fieldEffect;
        if (!MasterData.BattleskillFieldEffect.TryGetValue(this.field_effect_BattleskillFieldEffect.Value, out fieldEffect))
          Debug.LogError((object) ("Key not Found: MasterData.BattleskillFieldEffect[" + (object) this.field_effect_BattleskillFieldEffect.Value + "]"));
        return fieldEffect;
      }
    }

    public BattleskillDuelEffect duel_effect
    {
      get
      {
        if (!this.duel_effect_BattleskillDuelEffect.HasValue)
          return (BattleskillDuelEffect) null;
        BattleskillDuelEffect duelEffect;
        if (!MasterData.BattleskillDuelEffect.TryGetValue(this.duel_effect_BattleskillDuelEffect.Value, out duelEffect))
          Debug.LogError((object) ("Key not Found: MasterData.BattleskillDuelEffect[" + (object) this.duel_effect_BattleskillDuelEffect.Value + "]"));
        return duelEffect;
      }
    }

    public BattleskillFieldEffect passive_effect
    {
      get
      {
        if (!this.passive_effect_BattleskillFieldEffect.HasValue)
          return (BattleskillFieldEffect) null;
        BattleskillFieldEffect passiveEffect;
        if (!MasterData.BattleskillFieldEffect.TryGetValue(this.passive_effect_BattleskillFieldEffect.Value, out passiveEffect))
          Debug.LogError((object) ("Key not Found: MasterData.BattleskillFieldEffect[" + (object) this.passive_effect_BattleskillFieldEffect.Value + "]"));
        return passiveEffect;
      }
    }

    public BattleskillAilmentEffect ailment_effect
    {
      get
      {
        if (!this.ailment_effect_BattleskillAilmentEffect.HasValue)
          return (BattleskillAilmentEffect) null;
        BattleskillAilmentEffect ailmentEffect;
        if (!MasterData.BattleskillAilmentEffect.TryGetValue(this.ailment_effect_BattleskillAilmentEffect.Value, out ailmentEffect))
          Debug.LogError((object) ("Key not Found: MasterData.BattleskillAilmentEffect[" + (object) this.ailment_effect_BattleskillAilmentEffect.Value + "]"));
        return ailmentEffect;
      }
    }

    public Future<GameObject> LoadDuelMagicBulletPrefab()
    {
      string path = string.Format("BattleEffects/duel/MagicBullets/{0}", (object) this.duel_magic_bullet_name);
      return Singleton<ResourceManager>.GetInstance().LoadOrNull<GameObject>(path);
    }

    public Future<GameObject> LoadFieldEffectPrefab()
    {
      string path = string.Format("BattleEffects/field/{0}", (object) this.field_effect_name);
      return Singleton<ResourceManager>.GetInstance().LoadOrNull<GameObject>(path);
    }

    public Future<GameObject> LoadFieldTargetEffectPrefab()
    {
      string path = string.Format("BattleEffects/field/{0}", (object) this.field_target_effect_name);
      return Singleton<ResourceManager>.GetInstance().LoadOrNull<GameObject>(path);
    }

    public Future<Sprite> LoadBattleSkillIcon(BattleFuncs.InvestSkill s = null)
    {
      string path = this.getSkillIconPath(s);
      if (!Singleton<ResourceManager>.GetInstance().Contains(path))
        path = "BattleSkills/def_skill_icon";
      return Singleton<ResourceManager>.GetInstance().LoadOrNull<Sprite>(path);
    }

    public Future<Sprite> LoadCallSkillIcon()
    {
      return Singleton<ResourceManager>.GetInstance().LoadOrNull<Sprite>("BattleSkills/call_skill_icon");
    }

    public string getSkillIconPath(BattleFuncs.InvestSkill s = null)
    {
      if (s != null)
      {
        switch (s.skill.skill_type)
        {
          case BattleskillSkillType.leader:
            return !s.isEnemyIcon ? "BattleSkills/leader_skill_icon" : "BattleSkills/enemy_skill_icon";
          case BattleskillSkillType.item:
            return "BattleSkills/supply_skill_icon";
          case BattleskillSkillType.call:
            return "BattleSkills/call_skill_icon";
          default:
            if (s.skill.IsJobAbility)
              return "BattleSkills/ability_skill_icon";
            break;
        }
      }
      return this.resource_reference_id != 0 ? "BattleSkills/" + this.resource_reference_id.ToString() + "/skill_icon" : "BattleSkills/" + this.ID.ToString() + "/skill_icon";
    }
  }
}
