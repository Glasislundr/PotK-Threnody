// Decompiled with JetBrains decompiler
// Type: BEUnitExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;

#nullable disable
public static class BEUnitExtension
{
  public static void spawn(this BL.Unit self, BE env, bool resetDangerAria)
  {
    env.core.getUnitPosition(self).resetSpawnPosition(env.core, resetDangerAria: resetDangerAria);
    self.isEnable = true;
    if (self.isView)
      env.unitResource[self].unitParts_.spawn();
    foreach (BL.UnitPosition unitPosition in env.core.unitPositions.value)
    {
      if (unitPosition.hasPanelsCache)
        unitPosition.clearMovePanelCache();
    }
  }

  public static void rebirthBE(this BL.Unit self, BE env, bool intoEffectMode = false)
  {
    if (!self.isView)
      return;
    env.unitResource[self].unitParts_.rebirth(intoEffectMode);
    env.unitResource[self].unitParts_.resetTerrainEffect();
  }

  public static UnitVoicePattern getVoicePattern(this BL.Unit self)
  {
    SkillMetamorphosis metamorphosis = self.metamorphosis;
    return metamorphosis == null ? self.unit.unitVoicePattern : self.unit.getVoicePattern(metamorphosis.metamorphosis_id);
  }
}
