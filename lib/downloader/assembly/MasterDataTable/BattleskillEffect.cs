// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BattleskillEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections.Generic;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class BattleskillEffect
  {
    private BattleskillEffectLogic _effectLogic;
    private HashSet<BattleskillEffectLogicArgumentEnum> _keysCache;
    private Dictionary<BattleskillEffectLogicArgumentEnum, object> _valueCache;
    private BattleFuncs.PackedSkillEffect _packedSkillEffect;
    [NonSerialized]
    private int? useSkillCountMinCache;
    [NonSerialized]
    private int? useSkillCountMaxCache;
    [NonSerialized]
    private BattleFuncs.CheckInvokeGeneric _checkInvokeGeneric;
    [NonSerialized]
    private bool? _hasExtArg;
    [NonSerialized]
    private bool? _existOnemanChargeExtArg;
    public int ID;
    public int skill_BattleskillSkill;
    public int effect_logic_BattleskillEffectLogic;
    public bool is_targer_enemy;
    public int? use_count;
    public int? use_turn;
    public int? enchant_type;
    public string arg1_value;
    public string arg2_value;
    public string arg3_value;
    public string arg4_value;
    public string arg5_value;
    public string arg6_value;
    public string arg7_value;
    public string arg8_value;
    public string arg9_value;
    public string arg10_value;
    public int min_level;
    public int max_level;

    public BattleskillEffectLogic EffectLogic
    {
      get
      {
        if (this._effectLogic == null)
          this._effectLogic = this.effect_logic;
        return this._effectLogic;
      }
    }

    private string ValueToString(string v) => v;

    private double ValueToDouble(string v)
    {
      double result = 0.0;
      double.TryParse(v, out result);
      return result;
    }

    private float ValueToFloat(string v) => (float) this.ValueToDouble(v);

    private int ValueToInt(string v) => (int) this.ValueToDouble(v);

    private T Get<T>(BattleskillEffectLogicArgumentEnum key, Func<string, T> f)
    {
      if (this._effectLogic == null)
        this._effectLogic = this.effect_logic;
      if (key == this._effectLogic.arg1)
        return f(this.arg1_value);
      if (key == this._effectLogic.arg2)
        return f(this.arg2_value);
      if (key == this._effectLogic.arg3)
        return f(this.arg3_value);
      if (key == this._effectLogic.arg4)
        return f(this.arg4_value);
      if (key == this._effectLogic.arg5)
        return f(this.arg5_value);
      if (key == this._effectLogic.arg6)
        return f(this.arg6_value);
      if (key == this._effectLogic.arg7)
        return f(this.arg7_value);
      if (key == this._effectLogic.arg8)
        return f(this.arg8_value);
      if (key == this._effectLogic.arg9)
        return f(this.arg9_value);
      if (key == this._effectLogic.arg10)
        return f(this.arg10_value);
      throw new Exception(string.Format("key not found: {0} not in {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}", (object) key, (object) this._effectLogic.arg1, (object) this._effectLogic.arg2, (object) this._effectLogic.arg3, (object) this._effectLogic.arg4, (object) this._effectLogic.arg5, (object) this._effectLogic.arg6, (object) this._effectLogic.arg7, (object) this._effectLogic.arg8, (object) this._effectLogic.arg9, (object) this._effectLogic.arg10));
    }

    public bool HasKey(BattleskillEffectLogicArgumentEnum key)
    {
      if (key == BattleskillEffectLogicArgumentEnum.none)
        return false;
      if (this._keysCache == null)
        this._keysCache = this.GetKeys();
      return this._keysCache.Contains(key);
    }

    private HashSet<BattleskillEffectLogicArgumentEnum> GetKeys()
    {
      if (this._effectLogic == null)
        this._effectLogic = this.effect_logic;
      return new HashSet<BattleskillEffectLogicArgumentEnum>()
      {
        this._effectLogic.arg1,
        this._effectLogic.arg2,
        this._effectLogic.arg3,
        this._effectLogic.arg4,
        this._effectLogic.arg5,
        this._effectLogic.arg6,
        this._effectLogic.arg7,
        this._effectLogic.arg8,
        this._effectLogic.arg9,
        this._effectLogic.arg10
      };
    }

    public string GetString(BattleskillEffectLogicArgumentEnum key)
    {
      object obj = (object) null;
      if (this._valueCache != null && this._valueCache.TryGetValue(key, out obj))
        return obj as string;
      if (this._valueCache == null)
        this._valueCache = new Dictionary<BattleskillEffectLogicArgumentEnum, object>();
      string str = this.Get<string>(key, new Func<string, string>(this.ValueToString));
      this._valueCache.Add(key, (object) str);
      return str;
    }

    public float GetFloat(BattleskillEffectLogicArgumentEnum key)
    {
      object obj = (object) null;
      if (this._valueCache != null && this._valueCache.TryGetValue(key, out obj))
        return (float) (double) obj;
      if (this._valueCache == null)
        this._valueCache = new Dictionary<BattleskillEffectLogicArgumentEnum, object>();
      double num = this.Get<double>(key, new Func<string, double>(this.ValueToDouble));
      this._valueCache.Add(key, (object) num);
      return (float) num;
    }

    public int GetInt(BattleskillEffectLogicArgumentEnum key)
    {
      object obj = (object) null;
      if (this._valueCache != null && this._valueCache.TryGetValue(key, out obj))
        return (int) (double) obj;
      if (this._valueCache == null)
        this._valueCache = new Dictionary<BattleskillEffectLogicArgumentEnum, object>();
      double num = this.Get<double>(key, new Func<string, double>(this.ValueToDouble));
      this._valueCache.Add(key, (object) num);
      return (int) num;
    }

    public BattleFuncs.PackedSkillEffect GetPackedSkillEffect()
    {
      if (this._packedSkillEffect == null)
        this._packedSkillEffect = BattleFuncs.PackedSkillEffect.Create(this);
      return this._packedSkillEffect;
    }

    public bool isEnableGameMode(
      BattleskillInvokeGameModeEnum gameMode,
      BL.ISkillEffectListUnit unit)
    {
      return !this.HasKey(BattleskillEffectLogicArgumentEnum.invoke_gamemode) || BattleFuncs.checkInvokeGamemode(this.GetInt(BattleskillEffectLogicArgumentEnum.invoke_gamemode), gameMode == BattleskillInvokeGameModeEnum.colosseum, unit);
    }

    public bool checkLevel(int skillLevel)
    {
      if (this.min_level != 0 && skillLevel < this.min_level)
        return false;
      return this.max_level == 0 || skillLevel <= this.max_level;
    }

    public bool checkUseSkillCount(int nowUseCount)
    {
      ++nowUseCount;
      if (!this.useSkillCountMaxCache.HasValue)
      {
        int num1 = 0;
        int num2 = 0;
        foreach (BattleskillEffect effect in this.skill.Effects)
        {
          if (effect.EffectLogic.Enum == BattleskillEffectLogicEnum.use_skill_count_range_effect)
          {
            num1 = effect.GetInt(BattleskillEffectLogicArgumentEnum.use_skill_count_min);
            num2 = effect.GetInt(BattleskillEffectLogicArgumentEnum.use_skill_count_max);
          }
          else if (effect.ID == this.ID)
          {
            this.useSkillCountMinCache = new int?(num1);
            this.useSkillCountMaxCache = new int?(num2);
            break;
          }
        }
      }
      int? nullable = this.useSkillCountMinCache;
      int num3 = 0;
      if (!(nullable.GetValueOrDefault() == num3 & nullable.HasValue))
      {
        int num4 = nowUseCount;
        nullable = this.useSkillCountMinCache;
        int valueOrDefault = nullable.GetValueOrDefault();
        if (!(num4 >= valueOrDefault & nullable.HasValue))
          return false;
      }
      nullable = this.useSkillCountMaxCache;
      int num5 = 0;
      if (nullable.GetValueOrDefault() == num5 & nullable.HasValue)
        return true;
      int num6 = nowUseCount;
      nullable = this.useSkillCountMaxCache;
      int valueOrDefault1 = nullable.GetValueOrDefault();
      return num6 <= valueOrDefault1 & nullable.HasValue;
    }

    public void ClearCache()
    {
      this._effectLogic = (BattleskillEffectLogic) null;
      this._keysCache = (HashSet<BattleskillEffectLogicArgumentEnum>) null;
      this._valueCache = (Dictionary<BattleskillEffectLogicArgumentEnum, object>) null;
      this._packedSkillEffect = (BattleFuncs.PackedSkillEffect) null;
      this._checkInvokeGeneric = (BattleFuncs.CheckInvokeGeneric) null;
    }

    public static void AllClearCache()
    {
      foreach (BattleskillEffect battleskillEffect in MasterData.BattleskillEffectList)
        battleskillEffect.ClearCache();
    }

    public bool canBuffDebuffSwap()
    {
      if (this.EffectLogic.opt_test3 == 40)
        return false;
      if (!this.use_count.HasValue)
        return true;
      int? useCount = this.use_count;
      int num = 1;
      return !(useCount.GetValueOrDefault() == num & useCount.HasValue);
    }

    public BattleFuncs.CheckInvokeGeneric GetCheckInvokeGeneric()
    {
      if (this._checkInvokeGeneric == null)
        this._checkInvokeGeneric = new BattleFuncs.CheckInvokeGeneric(this.GetPackedSkillEffect());
      return this._checkInvokeGeneric;
    }

    public bool hasExtArg()
    {
      if (!this._hasExtArg.HasValue)
      {
        BattleskillEffect[] effects = this.skill.Effects;
        int length = effects.Length;
        int index1 = 0;
        while (index1 < length && effects[index1].ID != this.ID)
          ++index1;
        int index2 = index1 + 1;
        this._hasExtArg = new bool?(index2 < length && effects[index2].EffectLogic.HasTag(BattleskillEffectTag.ext_arg));
      }
      return this._hasExtArg.Value;
    }

    public bool IsExistOnemanChargeExtArg()
    {
      if (!this._existOnemanChargeExtArg.HasValue)
      {
        if (this.hasExtArg())
        {
          BattleFuncs.PackedSkillEffect packedSkillEffect = this.GetPackedSkillEffect();
          this._existOnemanChargeExtArg = new bool?(packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.oneman_charge_complex_min_range) || packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.oneman_charge_player_min_range) || packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.oneman_charge_enemy_min_range));
        }
        else
          this._existOnemanChargeExtArg = new bool?(false);
      }
      return this._existOnemanChargeExtArg.Value;
    }

    public static BattleskillEffect Parse(MasterDataReader reader)
    {
      return new BattleskillEffect()
      {
        ID = reader.ReadInt(),
        skill_BattleskillSkill = reader.ReadInt(),
        effect_logic_BattleskillEffectLogic = reader.ReadInt(),
        is_targer_enemy = reader.ReadBool(),
        use_count = reader.ReadIntOrNull(),
        use_turn = reader.ReadIntOrNull(),
        enchant_type = reader.ReadIntOrNull(),
        arg1_value = reader.ReadString(true),
        arg2_value = reader.ReadString(true),
        arg3_value = reader.ReadString(true),
        arg4_value = reader.ReadString(true),
        arg5_value = reader.ReadString(true),
        arg6_value = reader.ReadString(true),
        arg7_value = reader.ReadString(true),
        arg8_value = reader.ReadString(true),
        arg9_value = reader.ReadString(true),
        arg10_value = reader.ReadString(true),
        min_level = reader.ReadInt(),
        max_level = reader.ReadInt()
      };
    }

    public BattleskillSkill skill
    {
      get
      {
        BattleskillSkill skill;
        if (!MasterData.BattleskillSkill.TryGetValue(this.skill_BattleskillSkill, out skill))
          Debug.LogError((object) ("Key not Found: MasterData.BattleskillSkill[" + (object) this.skill_BattleskillSkill + "]"));
        return skill;
      }
    }

    public BattleskillEffectLogic effect_logic
    {
      get
      {
        BattleskillEffectLogic effectLogic;
        if (!MasterData.BattleskillEffectLogic.TryGetValue(this.effect_logic_BattleskillEffectLogic, out effectLogic))
          Debug.LogError((object) ("Key not Found: MasterData.BattleskillEffectLogic[" + (object) this.effect_logic_BattleskillEffectLogic + "]"));
        return effectLogic;
      }
    }
  }
}
