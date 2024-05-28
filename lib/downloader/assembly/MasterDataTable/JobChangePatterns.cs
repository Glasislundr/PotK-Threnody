// Decompiled with JetBrains decompiler
// Type: MasterDataTable.JobChangePatterns
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using LocaleTimeZone;
using System;
using System.Collections.Generic;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class JobChangePatterns
  {
    public int ID;
    public int unit_UnitUnit;
    public int job_UnitJob;
    public int job1_UnitJob;
    public int? materials1_JobChangeMaterials;
    public int job2_UnitJob;
    public int materials2_JobChangeMaterials;
    public int? job3_UnitJob;
    public int? materials3_JobChangeMaterials;
    public int? job4_UnitJob;
    public int? materials4_JobChangeMaterials;
    public int? job5_UnitJob;
    public int? materials5_JobChangeMaterials;
    public int? job6_UnitJob;
    public int? materials6_JobChangeMaterials;
    public int? job7_UnitJob;
    public int? materials7_JobChangeMaterials;

    public static JobChangePatterns Parse(MasterDataReader reader)
    {
      return new JobChangePatterns()
      {
        ID = reader.ReadInt(),
        unit_UnitUnit = reader.ReadInt(),
        job_UnitJob = reader.ReadInt(),
        job1_UnitJob = reader.ReadInt(),
        materials1_JobChangeMaterials = reader.ReadIntOrNull(),
        job2_UnitJob = reader.ReadInt(),
        materials2_JobChangeMaterials = reader.ReadInt(),
        job3_UnitJob = reader.ReadIntOrNull(),
        materials3_JobChangeMaterials = reader.ReadIntOrNull(),
        job4_UnitJob = reader.ReadIntOrNull(),
        materials4_JobChangeMaterials = reader.ReadIntOrNull(),
        job5_UnitJob = reader.ReadIntOrNull(),
        materials5_JobChangeMaterials = reader.ReadIntOrNull(),
        job6_UnitJob = reader.ReadIntOrNull(),
        materials6_JobChangeMaterials = reader.ReadIntOrNull(),
        job7_UnitJob = reader.ReadIntOrNull(),
        materials7_JobChangeMaterials = reader.ReadIntOrNull()
      };
    }

    public UnitUnit unit
    {
      get
      {
        UnitUnit unit;
        if (!MasterData.UnitUnit.TryGetValue(this.unit_UnitUnit, out unit))
          Debug.LogError((object) ("Key not Found: MasterData.UnitUnit[" + (object) this.unit_UnitUnit + "]"));
        return unit;
      }
    }

    public UnitJob job
    {
      get
      {
        UnitJob job;
        if (!MasterData.UnitJob.TryGetValue(this.job_UnitJob, out job))
          Debug.LogError((object) ("Key not Found: MasterData.UnitJob[" + (object) this.job_UnitJob + "]"));
        return job;
      }
    }

    public UnitJob job1
    {
      get
      {
        UnitJob job1;
        if (!MasterData.UnitJob.TryGetValue(this.job1_UnitJob, out job1))
          Debug.LogError((object) ("Key not Found: MasterData.UnitJob[" + (object) this.job1_UnitJob + "]"));
        return job1;
      }
    }

    public JobChangeMaterials materials1
    {
      get
      {
        if (!this.materials1_JobChangeMaterials.HasValue)
          return (JobChangeMaterials) null;
        JobChangeMaterials materials1;
        if (!MasterData.JobChangeMaterials.TryGetValue(this.materials1_JobChangeMaterials.Value, out materials1))
          Debug.LogError((object) ("Key not Found: MasterData.JobChangeMaterials[" + (object) this.materials1_JobChangeMaterials.Value + "]"));
        return materials1;
      }
    }

    public UnitJob job2
    {
      get
      {
        UnitJob job2;
        if (!MasterData.UnitJob.TryGetValue(this.job2_UnitJob, out job2))
          Debug.LogError((object) ("Key not Found: MasterData.UnitJob[" + (object) this.job2_UnitJob + "]"));
        return job2;
      }
    }

    public JobChangeMaterials materials2
    {
      get
      {
        JobChangeMaterials materials2;
        if (!MasterData.JobChangeMaterials.TryGetValue(this.materials2_JobChangeMaterials, out materials2))
          Debug.LogError((object) ("Key not Found: MasterData.JobChangeMaterials[" + (object) this.materials2_JobChangeMaterials + "]"));
        return materials2;
      }
    }

    public UnitJob job3
    {
      get
      {
        if (!this.job3_UnitJob.HasValue)
          return (UnitJob) null;
        UnitJob job3;
        if (!MasterData.UnitJob.TryGetValue(this.job3_UnitJob.Value, out job3))
          Debug.LogError((object) ("Key not Found: MasterData.UnitJob[" + (object) this.job3_UnitJob.Value + "]"));
        return job3;
      }
    }

    public JobChangeMaterials materials3
    {
      get
      {
        if (!this.materials3_JobChangeMaterials.HasValue)
          return (JobChangeMaterials) null;
        JobChangeMaterials materials3;
        if (!MasterData.JobChangeMaterials.TryGetValue(this.materials3_JobChangeMaterials.Value, out materials3))
          Debug.LogError((object) ("Key not Found: MasterData.JobChangeMaterials[" + (object) this.materials3_JobChangeMaterials.Value + "]"));
        return materials3;
      }
    }

    public UnitJob job4
    {
      get
      {
        if (!this.job4_UnitJob.HasValue)
          return (UnitJob) null;
        UnitJob job4;
        if (!MasterData.UnitJob.TryGetValue(this.job4_UnitJob.Value, out job4))
          Debug.LogError((object) ("Key not Found: MasterData.UnitJob[" + (object) this.job4_UnitJob.Value + "]"));
        return job4;
      }
    }

    public JobChangeMaterials materials4
    {
      get
      {
        if (!this.materials4_JobChangeMaterials.HasValue)
          return (JobChangeMaterials) null;
        JobChangeMaterials materials4;
        if (!MasterData.JobChangeMaterials.TryGetValue(this.materials4_JobChangeMaterials.Value, out materials4))
          Debug.LogError((object) ("Key not Found: MasterData.JobChangeMaterials[" + (object) this.materials4_JobChangeMaterials.Value + "]"));
        return materials4;
      }
    }

    public UnitJob job5
    {
      get
      {
        if (!this.job5_UnitJob.HasValue)
          return (UnitJob) null;
        UnitJob job5;
        if (!MasterData.UnitJob.TryGetValue(this.job5_UnitJob.Value, out job5))
          Debug.LogError((object) ("Key not Found: MasterData.UnitJob[" + (object) this.job5_UnitJob.Value + "]"));
        return job5;
      }
    }

    public JobChangeMaterials materials5
    {
      get
      {
        if (!this.materials5_JobChangeMaterials.HasValue)
          return (JobChangeMaterials) null;
        JobChangeMaterials materials5;
        if (!MasterData.JobChangeMaterials.TryGetValue(this.materials5_JobChangeMaterials.Value, out materials5))
          Debug.LogError((object) ("Key not Found: MasterData.JobChangeMaterials[" + (object) this.materials5_JobChangeMaterials.Value + "]"));
        return materials5;
      }
    }

    public UnitJob job6
    {
      get
      {
        if (!this.job6_UnitJob.HasValue)
          return (UnitJob) null;
        UnitJob job6;
        if (!MasterData.UnitJob.TryGetValue(this.job6_UnitJob.Value, out job6))
          Debug.LogError((object) ("Key not Found: MasterData.UnitJob[" + (object) this.job6_UnitJob.Value + "]"));
        return job6;
      }
    }

    public JobChangeMaterials materials6
    {
      get
      {
        if (!this.materials6_JobChangeMaterials.HasValue)
          return (JobChangeMaterials) null;
        JobChangeMaterials materials6;
        if (!MasterData.JobChangeMaterials.TryGetValue(this.materials6_JobChangeMaterials.Value, out materials6))
          Debug.LogError((object) ("Key not Found: MasterData.JobChangeMaterials[" + (object) this.materials6_JobChangeMaterials.Value + "]"));
        return materials6;
      }
    }

    public UnitJob job7
    {
      get
      {
        if (!this.job7_UnitJob.HasValue)
          return (UnitJob) null;
        UnitJob job7;
        if (!MasterData.UnitJob.TryGetValue(this.job7_UnitJob.Value, out job7))
          Debug.LogError((object) ("Key not Found: MasterData.UnitJob[" + (object) this.job7_UnitJob.Value + "]"));
        return job7;
      }
    }

    public JobChangeMaterials materials7
    {
      get
      {
        if (!this.materials7_JobChangeMaterials.HasValue)
          return (JobChangeMaterials) null;
        JobChangeMaterials materials7;
        if (!MasterData.JobChangeMaterials.TryGetValue(this.materials7_JobChangeMaterials.Value, out materials7))
          Debug.LogError((object) ("Key not Found: MasterData.JobChangeMaterials[" + (object) this.materials7_JobChangeMaterials.Value + "]"));
        return materials7;
      }
    }

    public JobChangePatterns GetEnablePatterns()
    {
      JobChangePatterns enablePatterns = new JobChangePatterns();
      enablePatterns.ID = this.ID;
      enablePatterns.unit_UnitUnit = this.unit_UnitUnit;
      enablePatterns.job_UnitJob = this.job_UnitJob;
      TimeZoneInfo timeZone = Japan.CreateTimeZone();
      DateTime dateTime = ServerTime.NowAppTimeAddDelta();
      UnitJob unitJob;
      if (!MasterData.UnitJob.TryGetValue(this.job1_UnitJob, out unitJob) || unitJob.start_at.HasValue && !(TimeZoneInfo.ConvertTime(unitJob.start_at.Value, timeZone, TimeZoneInfo.Local) <= dateTime))
        return (JobChangePatterns) null;
      enablePatterns.job1_UnitJob = this.job1_UnitJob;
      enablePatterns.materials1_JobChangeMaterials = this.materials1_JobChangeMaterials;
      if (!MasterData.UnitJob.TryGetValue(this.job2_UnitJob, out unitJob) || unitJob.start_at.HasValue && !(TimeZoneInfo.ConvertTime(unitJob.start_at.Value, timeZone, TimeZoneInfo.Local) <= dateTime))
        return (JobChangePatterns) null;
      enablePatterns.job2_UnitJob = this.job2_UnitJob;
      enablePatterns.materials2_JobChangeMaterials = this.materials2_JobChangeMaterials;
      if (this.job3_UnitJob.HasValue && MasterData.UnitJob.TryGetValue(this.job3_UnitJob.Value, out unitJob) && (!unitJob.start_at.HasValue || TimeZoneInfo.ConvertTime(unitJob.start_at.Value, timeZone, TimeZoneInfo.Local) <= dateTime))
      {
        enablePatterns.job3_UnitJob = this.job3_UnitJob;
        enablePatterns.materials3_JobChangeMaterials = this.materials3_JobChangeMaterials;
      }
      if (enablePatterns.job3_UnitJob.HasValue && this.job4_UnitJob.HasValue && MasterData.UnitJob.TryGetValue(this.job4_UnitJob.Value, out unitJob) && (!unitJob.start_at.HasValue || TimeZoneInfo.ConvertTime(unitJob.start_at.Value, timeZone, TimeZoneInfo.Local) <= dateTime))
      {
        enablePatterns.job4_UnitJob = this.job4_UnitJob;
        enablePatterns.materials4_JobChangeMaterials = this.materials4_JobChangeMaterials;
      }
      if (this.job5_UnitJob.HasValue && MasterData.UnitJob.TryGetValue(this.job5_UnitJob.Value, out unitJob) && (!unitJob.start_at.HasValue || TimeZoneInfo.ConvertTime(unitJob.start_at.Value, timeZone, TimeZoneInfo.Local) <= dateTime))
      {
        enablePatterns.job5_UnitJob = this.job5_UnitJob;
        enablePatterns.materials5_JobChangeMaterials = this.materials5_JobChangeMaterials;
      }
      if (enablePatterns.job3_UnitJob.HasValue && this.job6_UnitJob.HasValue && MasterData.UnitJob.TryGetValue(this.job6_UnitJob.Value, out unitJob) && (!unitJob.start_at.HasValue || TimeZoneInfo.ConvertTime(unitJob.start_at.Value, timeZone, TimeZoneInfo.Local) <= dateTime))
      {
        enablePatterns.job6_UnitJob = this.job6_UnitJob;
        enablePatterns.materials6_JobChangeMaterials = this.materials6_JobChangeMaterials;
      }
      if (enablePatterns.job4_UnitJob.HasValue && this.job7_UnitJob.HasValue && MasterData.UnitJob.TryGetValue(this.job7_UnitJob.Value, out unitJob) && (!unitJob.start_at.HasValue || TimeZoneInfo.ConvertTime(unitJob.start_at.Value, timeZone, TimeZoneInfo.Local) <= dateTime))
      {
        enablePatterns.job7_UnitJob = this.job7_UnitJob;
        enablePatterns.materials7_JobChangeMaterials = this.materials7_JobChangeMaterials;
      }
      return enablePatterns;
    }

    public UnitJob[] getJobs(int maxJobs = 4)
    {
      List<UnitJob> unitJobList = new List<UnitJob>(maxJobs)
      {
        this.job1,
        this.job2
      };
      for (int index = 2; index < maxJobs; ++index)
      {
        UnitJob unitJob;
        switch (index)
        {
          case 2:
            unitJob = this.job3;
            break;
          case 3:
            unitJob = this.job4;
            break;
          case 4:
            unitJob = this.job5;
            break;
          case 5:
            unitJob = this.job6;
            break;
          case 6:
            unitJob = this.job7;
            break;
          default:
            unitJob = (UnitJob) null;
            break;
        }
        if (unitJob != null)
          unitJobList.Add(unitJob);
        else
          break;
      }
      return unitJobList.ToArray();
    }
  }
}
