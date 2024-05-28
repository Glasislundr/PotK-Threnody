// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BattleFieldEffectStage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class BattleFieldEffectStage
  {
    public int ID;
    public int stage_BattleStage;
    public int timing_BattleFieldEffectTiming;
    public int field_effect_BattleFieldEffect;

    public static BattleFieldEffectStage Parse(MasterDataReader reader)
    {
      return new BattleFieldEffectStage()
      {
        ID = reader.ReadInt(),
        stage_BattleStage = reader.ReadInt(),
        timing_BattleFieldEffectTiming = reader.ReadInt(),
        field_effect_BattleFieldEffect = reader.ReadInt()
      };
    }

    public BattleStage stage
    {
      get
      {
        BattleStage stage;
        if (!MasterData.BattleStage.TryGetValue(this.stage_BattleStage, out stage))
          Debug.LogError((object) ("Key not Found: MasterData.BattleStage[" + (object) this.stage_BattleStage + "]"));
        return stage;
      }
    }

    public BattleFieldEffectTiming timing
    {
      get => (BattleFieldEffectTiming) this.timing_BattleFieldEffectTiming;
    }

    public BattleFieldEffect field_effect
    {
      get
      {
        BattleFieldEffect fieldEffect;
        if (!MasterData.BattleFieldEffect.TryGetValue(this.field_effect_BattleFieldEffect, out fieldEffect))
          Debug.LogError((object) ("Key not Found: MasterData.BattleFieldEffect[" + (object) this.field_effect_BattleFieldEffect + "]"));
        return fieldEffect;
      }
    }
  }
}
