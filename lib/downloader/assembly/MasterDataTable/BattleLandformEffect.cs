// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BattleLandformEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class BattleLandformEffect
  {
    private BattleskillEffectLogic _effectLogic;
    private List<BattleskillEffectLogicArgumentEnum> _keysCache;
    public int ID;
    public int group_id;
    public int effect_logic_BattleskillEffectLogic;
    public int landform_effect_phase_BattleLandformEffectPhase;
    public int? use_count;
    public int? use_turn;
    public float arg1_value;
    public float arg2_value;
    public float arg3_value;
    public float arg4_value;
    public float arg5_value;
    public float arg6_value;
    public float arg7_value;
    public float arg8_value;
    public float arg9_value;
    public float arg10_value;

    private string ValueToString(float v) => v.ToString();

    private float ValueToFloat(float v) => v;

    private int ValueToInt(float v) => (int) v;

    private T Get<T>(BattleskillEffectLogicArgumentEnum key, Func<float, T> f)
    {
      this._effectLogic = this._effectLogic ?? this.effect_logic;
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
      throw new Exception(string.Format("key not found: {0} not in {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}", (object) key, (object) this.effect_logic.arg1, (object) this.effect_logic.arg2, (object) this.effect_logic.arg3, (object) this.effect_logic.arg4, (object) this.effect_logic.arg5, (object) this.effect_logic.arg6, (object) this.effect_logic.arg7, (object) this.effect_logic.arg8, (object) this.effect_logic.arg9, (object) this.effect_logic.arg10));
    }

    public bool HasKey(BattleskillEffectLogicArgumentEnum key)
    {
      if (key == BattleskillEffectLogicArgumentEnum.none)
        return false;
      this._keysCache = this._keysCache ?? this.GetKeys();
      return this._keysCache.Contains(key);
    }

    private List<BattleskillEffectLogicArgumentEnum> GetKeys()
    {
      this._effectLogic = this._effectLogic ?? this.effect_logic;
      return new List<BattleskillEffectLogicArgumentEnum>()
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
      return this.Get<string>(key, new Func<float, string>(this.ValueToString));
    }

    public float GetFloat(BattleskillEffectLogicArgumentEnum key)
    {
      return this.Get<float>(key, new Func<float, float>(this.ValueToFloat));
    }

    public int GetInt(BattleskillEffectLogicArgumentEnum key)
    {
      return this.Get<int>(key, new Func<float, int>(this.ValueToInt));
    }

    public void ClearCache()
    {
      this._effectLogic = (BattleskillEffectLogic) null;
      this._keysCache = (List<BattleskillEffectLogicArgumentEnum>) null;
    }

    public static void AllClearCache()
    {
      foreach (BattleskillEffect battleskillEffect in MasterData.BattleskillEffectList)
        battleskillEffect.ClearCache();
    }

    public static BattleLandformEffect Parse(MasterDataReader reader)
    {
      return new BattleLandformEffect()
      {
        ID = reader.ReadInt(),
        group_id = reader.ReadInt(),
        effect_logic_BattleskillEffectLogic = reader.ReadInt(),
        landform_effect_phase_BattleLandformEffectPhase = reader.ReadInt(),
        use_count = reader.ReadIntOrNull(),
        use_turn = reader.ReadIntOrNull(),
        arg1_value = reader.ReadFloat(),
        arg2_value = reader.ReadFloat(),
        arg3_value = reader.ReadFloat(),
        arg4_value = reader.ReadFloat(),
        arg5_value = reader.ReadFloat(),
        arg6_value = reader.ReadFloat(),
        arg7_value = reader.ReadFloat(),
        arg8_value = reader.ReadFloat(),
        arg9_value = reader.ReadFloat(),
        arg10_value = reader.ReadFloat()
      };
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

    public BattleLandformEffectPhase landform_effect_phase
    {
      get => (BattleLandformEffectPhase) this.landform_effect_phase_BattleLandformEffectPhase;
    }
  }
}
