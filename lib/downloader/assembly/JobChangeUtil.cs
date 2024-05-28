// Decompiled with JetBrains decompiler
// Type: JobChangeUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using EquipmentRules;
using JobChangeData;
using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UniLinq;

#nullable disable
public static class JobChangeUtil
{
  public static PlayerUnit[] getTargets()
  {
    return ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).Where<PlayerUnit>((Func<PlayerUnit, bool>) (pu => JobChangeUtil.getJobChangePatterns(pu) != null)).ToArray<PlayerUnit>();
  }

  public static PlayerUnit[] createJobChangePattern(PlayerUnit playerUnit, out bool isExist)
  {
    JobChangePatterns jobChangePatterns = JobChangeUtil.getJobChangePatterns(playerUnit);
    if (jobChangePatterns == null)
    {
      isExist = false;
      return Enumerable.Repeat<PlayerUnit>(JobChangeUtil.createPlayerUnit(playerUnit, playerUnit.job_id), DefValues.NUM_CHANGETYPE).ToArray<PlayerUnit>();
    }
    isExist = true;
    List<PlayerUnit> playerUnitList = new List<PlayerUnit>(DefValues.NUM_CHANGETYPE)
    {
      JobChangeUtil.createPlayerUnit(playerUnit, jobChangePatterns.job1_UnitJob),
      JobChangeUtil.createPlayerUnit(playerUnit, jobChangePatterns.job2_UnitJob)
    };
    if (jobChangePatterns.job3_UnitJob.HasValue)
      playerUnitList.Add(JobChangeUtil.createPlayerUnit(playerUnit, jobChangePatterns.job3_UnitJob.Value));
    else
      playerUnitList.Add((PlayerUnit) null);
    if (jobChangePatterns.job4_UnitJob.HasValue)
      playerUnitList.Add(JobChangeUtil.createPlayerUnit(playerUnit, jobChangePatterns.job4_UnitJob.Value));
    else
      playerUnitList.Add((PlayerUnit) null);
    if (jobChangePatterns.job5_UnitJob.HasValue)
      playerUnitList.Add(JobChangeUtil.createPlayerUnit(playerUnit, jobChangePatterns.job5_UnitJob.Value));
    else
      playerUnitList.Add((PlayerUnit) null);
    if (jobChangePatterns.job6_UnitJob.HasValue)
      playerUnitList.Add(JobChangeUtil.createPlayerUnit(playerUnit, jobChangePatterns.job6_UnitJob.Value));
    else
      playerUnitList.Add((PlayerUnit) null);
    if (jobChangePatterns.job7_UnitJob.HasValue)
      playerUnitList.Add(JobChangeUtil.createPlayerUnit(playerUnit, jobChangePatterns.job7_UnitJob.Value));
    else
      playerUnitList.Add((PlayerUnit) null);
    return playerUnitList.ToArray();
  }

  public static JobCharacteristics[] getAllJobAbilities(PlayerUnit playerUnit)
  {
    JobCharacteristics[] first = new JobCharacteristics[0];
    JobChangePatterns jobChangePatterns = JobChangeUtil.getJobChangePatterns(playerUnit);
    return jobChangePatterns == null ? first : ((IEnumerable<JobCharacteristics>) first).Concat<JobCharacteristics>((IEnumerable<JobCharacteristics>) (jobChangePatterns.job1?.JobAbilities ?? new JobCharacteristics[0])).Concat<JobCharacteristics>((IEnumerable<JobCharacteristics>) (jobChangePatterns.job2?.JobAbilities ?? new JobCharacteristics[0])).Concat<JobCharacteristics>((IEnumerable<JobCharacteristics>) (jobChangePatterns.job3?.JobAbilities ?? new JobCharacteristics[0])).Concat<JobCharacteristics>((IEnumerable<JobCharacteristics>) (jobChangePatterns.job4?.JobAbilities ?? new JobCharacteristics[0])).Concat<JobCharacteristics>((IEnumerable<JobCharacteristics>) (jobChangePatterns.job5?.JobAbilities ?? new JobCharacteristics[0])).Concat<JobCharacteristics>((IEnumerable<JobCharacteristics>) (jobChangePatterns.job6?.JobAbilities ?? new JobCharacteristics[0])).Concat<JobCharacteristics>((IEnumerable<JobCharacteristics>) (jobChangePatterns.job7?.JobAbilities ?? new JobCharacteristics[0])).ToArray<JobCharacteristics>();
  }

  public static JobChangePatterns getJobChangePatterns(PlayerUnit playerUnit)
  {
    return JobChangeUtil.getJobChangePatterns(playerUnit._unit, playerUnit.job_id);
  }

  public static JobChangePatterns getJobChangePatterns(int unitId, int jobId)
  {
    JobChangePatterns jobChangePatterns = Array.Find<JobChangePatterns>(MasterData.JobChangePatternsList, (Predicate<JobChangePatterns>) (p => p.unit_UnitUnit == unitId && p.job_UnitJob == jobId));
    return jobChangePatterns == null ? jobChangePatterns : jobChangePatterns.GetEnablePatterns();
  }

  public static PlayerMaterialUnit[] createPlayerMaterialUnits(JobChangeMaterials materials)
  {
    List<PlayerMaterialUnit> playerMaterialUnitList = new List<PlayerMaterialUnit>(DefValues.NUM_MATERIALSLOT);
    int id = 1;
    if (materials.material1_UnitUnit.HasValue)
      playerMaterialUnitList.Add(JobChangeUtil.createPlayerMaterialUnit(id++, materials.material1_UnitUnit.Value, materials.quantity1));
    if (materials.material2_UnitUnit.HasValue)
      playerMaterialUnitList.Add(JobChangeUtil.createPlayerMaterialUnit(id++, materials.material2_UnitUnit.Value, materials.quantity2));
    if (materials.material3_UnitUnit.HasValue)
      playerMaterialUnitList.Add(JobChangeUtil.createPlayerMaterialUnit(id++, materials.material3_UnitUnit.Value, materials.quantity3));
    if (materials.material4_UnitUnit.HasValue)
      playerMaterialUnitList.Add(JobChangeUtil.createPlayerMaterialUnit(id++, materials.material4_UnitUnit.Value, materials.quantity4));
    if (materials.material5_UnitUnit.HasValue)
      playerMaterialUnitList.Add(JobChangeUtil.createPlayerMaterialUnit(id, materials.material5_UnitUnit.Value, materials.quantity5));
    return playerMaterialUnitList.ToArray();
  }

  private static PlayerMaterialUnit createPlayerMaterialUnit(int id, int materialId, int quantity)
  {
    PlayerMaterialUnit playerMaterialUnit1 = new PlayerMaterialUnit();
    PlayerMaterialUnit playerMaterialUnit2 = Array.Find<PlayerMaterialUnit>(SMManager.Get<PlayerMaterialUnit[]>(), (Predicate<PlayerMaterialUnit>) (x => x._unit == materialId));
    playerMaterialUnit1.id = playerMaterialUnit2 != null ? playerMaterialUnit2.id : id;
    playerMaterialUnit1._unit = materialId;
    playerMaterialUnit1.quantity = quantity;
    return playerMaterialUnit1;
  }

  public static bool checkCompletedMaterials(
    PlayerMaterialUnit[] playerMaterials,
    PlayerMaterialUnit[] materials)
  {
    foreach (PlayerMaterialUnit material in materials)
    {
      PlayerMaterialUnit m = material;
      if (m == null || m.quantity <= 0)
        return false;
      PlayerMaterialUnit playerMaterialUnit = Array.Find<PlayerMaterialUnit>(playerMaterials, (Predicate<PlayerMaterialUnit>) (p => p._unit == m._unit));
      if (playerMaterialUnit == null || playerMaterialUnit.quantity < m.quantity)
        return false;
    }
    return true;
  }

  public static PlayerUnit createPlayerUnit(
    PlayerUnit original,
    int jobId,
    Func<PlayerUnit, PlayerUnit> funcClone = null,
    bool bCloneParam = false)
  {
    PlayerUnit playerUnit = funcClone != null ? funcClone(original) : original.Clone();
    if (playerUnit.job_id != jobId)
    {
      playerUnit.clearInitialGear();
      JobChangeUtil.setJobParam(playerUnit.all_saved_job_abilities, playerUnit, jobId);
      PlayerItem[] equippedPlayerGears = new PlayerItem[3]
      {
        original.equippedGear,
        original.equippedGear2,
        original.equippedGear3
      };
      int slotIndex1 = 0;
      if (equippedPlayerGears[slotIndex1] != (PlayerItem) null)
      {
        PlayerItem playerItem = equippedPlayerGears[slotIndex1];
        equippedPlayerGears[slotIndex1] = (PlayerItem) null;
        if (!Gears.createFuncPossibleEquipped(playerUnit, slotIndex1, equippedPlayerGears, true)(playerItem))
          playerUnit.primary_equipped_gear = new PlayerItem();
        equippedPlayerGears[slotIndex1] = playerItem;
      }
      int slotIndex2 = 2;
      if (equippedPlayerGears[slotIndex2] != (PlayerItem) null)
      {
        PlayerItem playerItem = equippedPlayerGears[slotIndex2];
        equippedPlayerGears[slotIndex2] = (PlayerItem) null;
        if (!Gears.createFuncPossibleEquipped(playerUnit, slotIndex2, equippedPlayerGears, true)(playerItem))
          playerUnit.primary_equipped_gear3 = new PlayerItem();
      }
    }
    else if (bCloneParam)
      JobChangeUtil.cloneParameter(playerUnit);
    return playerUnit;
  }

  public static void cloneParameter(PlayerUnit target)
  {
    target.hp = JobChangeUtil.deepClone<PlayerUnitHp>(target.hp);
    target.strength = JobChangeUtil.deepClone<PlayerUnitStrength>(target.strength);
    target.vitality = JobChangeUtil.deepClone<PlayerUnitVitality>(target.vitality);
    target.intelligence = JobChangeUtil.deepClone<PlayerUnitIntelligence>(target.intelligence);
    target.mind = JobChangeUtil.deepClone<PlayerUnitMind>(target.mind);
    target.agility = JobChangeUtil.deepClone<PlayerUnitAgility>(target.agility);
    target.dexterity = JobChangeUtil.deepClone<PlayerUnitDexterity>(target.dexterity);
    target.lucky = JobChangeUtil.deepClone<PlayerUnitLucky>(target.lucky);
  }

  private static void setJobParam(
    PlayerUnitAll_saved_job_abilities[] abilities,
    PlayerUnit target,
    int jobId)
  {
    target.job_id = jobId;
    MasterDataTable.UnitJob jobData = target.getJobData();
    UnitUnit unit = target.unit;
    JobChangeUtil.cloneParameter(target);
    target.hp.initial = unit.hp_initial + jobData.hp_initial;
    target.strength.initial = unit.strength_initial + jobData.strength_initial;
    target.vitality.initial = unit.vitality_initial + jobData.vitality_initial;
    target.intelligence.initial = unit.intelligence_initial + jobData.intelligence_initial;
    target.mind.initial = unit.mind_initial + jobData.mind_initial;
    target.agility.initial = unit.agility_initial + jobData.agility_initial;
    target.dexterity.initial = unit.dexterity_initial + jobData.dexterity_initial;
    target.lucky.initial = unit.lucky_initial + jobData.lucky_initial;
    target.job_abilities = ((IEnumerable<JobCharacteristics>) jobData.JobAbilities).Select<JobCharacteristics, PlayerUnitJob_abilities>((Func<JobCharacteristics, PlayerUnitJob_abilities>) (j => JobChangeUtil.createUnitJobAbility(j))).ToArray<PlayerUnitJob_abilities>();
    if (abilities != null && abilities.Length != 0)
    {
      for (int index = 0; index < target.job_abilities.Length; ++index)
      {
        int jaid = target.job_abilities[index].job_ability_id;
        PlayerUnitAll_saved_job_abilities savedJobAbilities = Array.Find<PlayerUnitAll_saved_job_abilities>(abilities, (Predicate<PlayerUnitAll_saved_job_abilities>) (x => x.job_ability_id == jaid));
        if (savedJobAbilities != null)
          target.job_abilities[index].level = savedJobAbilities.level;
      }
    }
    target.move = jobData.movement;
  }

  private static T deepClone<T>(T src) where T : class
  {
    using (MemoryStream serializationStream = new MemoryStream())
    {
      BinaryFormatter binaryFormatter = new BinaryFormatter();
      binaryFormatter.Serialize((Stream) serializationStream, (object) src);
      serializationStream.Seek(0L, SeekOrigin.Begin);
      return (T) binaryFormatter.Deserialize((Stream) serializationStream);
    }
  }

  private static PlayerUnitJob_abilities createUnitJobAbility(JobCharacteristics dat)
  {
    return new PlayerUnitJob_abilities()
    {
      job_ability_id = dat.ID,
      skill_id = dat.skill_BattleskillSkill,
      level = 0
    };
  }

  public static bool checkDiff3DModel(PlayerUnit a, PlayerUnit b)
  {
    return a._unit != b._unit || a.job_id != b.job_id;
  }

  public static int[] GetJobIdChangePatternsConditions(
    PlayerUnit playerUnit,
    WebAPI.Response.UnitPreviewJob previewJobResponse)
  {
    List<int> intList = new List<int>();
    JobChangePatterns jobChangePatterns = JobChangeUtil.getJobChangePatterns(playerUnit);
    if (jobChangePatterns == null)
      return intList.ToArray();
    int job2UnitJob = jobChangePatterns.job2_UnitJob;
    int jobId1 = jobChangePatterns.job3_UnitJob.HasValue ? jobChangePatterns.job3_UnitJob.Value : 0;
    int jobId2 = jobChangePatterns.job4_UnitJob.HasValue ? jobChangePatterns.job4_UnitJob.Value : 0;
    if (jobId2 > 0 && !((IEnumerable<int>) previewJobResponse.changed_job_ids).Contains<int>(jobId2) && (!JobChangeUtil.isJobAbilitiesMax(job2UnitJob, previewJobResponse) || !JobChangeUtil.isJobAbilitiesMax(jobId1, previewJobResponse)))
      intList.Add(jobId2);
    int jobId3 = jobChangePatterns.job5_UnitJob.HasValue ? jobChangePatterns.job5_UnitJob.Value : 0;
    if (jobId3 > 0 && !((IEnumerable<int>) previewJobResponse.changed_job_ids).Contains<int>(jobId3) && !JobChangeUtil.isJobAbilitiesMax(jobId2, previewJobResponse))
      intList.Add(jobId3);
    int jobId4 = jobChangePatterns.job6_UnitJob.HasValue ? jobChangePatterns.job6_UnitJob.Value : 0;
    if (jobId4 > 0 && !((IEnumerable<int>) previewJobResponse.changed_job_ids).Contains<int>(jobId4) && !JobChangeUtil.isJobAbilitiesMax(jobId3, previewJobResponse))
      intList.Add(jobId4);
    if (jobChangePatterns.job7_UnitJob.HasValue)
    {
      int num = jobChangePatterns.job7_UnitJob.Value;
      if (!((IEnumerable<int>) previewJobResponse.changed_job_ids).Contains<int>(num) && !JobChangeUtil.isJobAbilitiesMax(jobId4, previewJobResponse))
        intList.Add(num);
    }
    return intList.ToArray();
  }

  private static bool isJobAbilitiesMax(
    int jobId,
    WebAPI.Response.UnitPreviewJob previewJobResponse)
  {
    MasterDataTable.UnitJob unitJob;
    if (!MasterData.UnitJob.TryGetValue(jobId, out unitJob))
      return false;
    foreach (JobCharacteristics jobAbility1 in unitJob.JobAbilities)
    {
      JobCharacteristics jobAbility = jobAbility1;
      WebAPI.Response.UnitPreviewJobJob_abilities previewJobJobAbilities = Array.Find<WebAPI.Response.UnitPreviewJobJob_abilities>(previewJobResponse.job_abilities, (Predicate<WebAPI.Response.UnitPreviewJobJob_abilities>) (x => x.job_ability_id == jobAbility.ID));
      if (previewJobJobAbilities == null || previewJobJobAbilities.level < MasterData.BattleskillSkill[previewJobJobAbilities.skill_id].upper_level)
        return false;
    }
    return true;
  }

  public static MasterDataTable.UnitJob[] getJobs(UnitUnit unit)
  {
    List<MasterDataTable.UnitJob> unitJobList = new List<MasterDataTable.UnitJob>(DefValues.NUM_CHANGETYPE - 1);
    JobChangePatterns jobChangePatterns = Array.Find<JobChangePatterns>(MasterData.JobChangePatternsList, (Predicate<JobChangePatterns>) (x => x.unit_UnitUnit == unit.ID && x.job1_UnitJob == unit.job_UnitJob));
    if (jobChangePatterns != null)
    {
      unitJobList.Add(jobChangePatterns.job2);
      MasterDataTable.UnitJob job3 = jobChangePatterns.job3;
      if (job3 != null)
      {
        unitJobList.Add(job3);
        MasterDataTable.UnitJob job4 = jobChangePatterns.job4;
        if (job4 != null)
          unitJobList.Add(job4);
      }
    }
    return unitJobList.ToArray();
  }
}
