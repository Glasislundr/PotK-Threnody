// Decompiled with JetBrains decompiler
// Type: MissionData.GuildIMission
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;

#nullable disable
namespace MissionData
{
  internal class GuildIMission : IMission
  {
    private GuildMission data_;

    public bool isDaily => false;

    public bool isGuild => true;

    public bool isShow
    {
      get
      {
        DateTime dateTime = ServerTime.NowAppTimeAddDelta();
        if (this.data_.start_at.HasValue && this.data_.start_at.Value > dateTime)
          return false;
        return !this.data_.published_end_at.HasValue || !(this.data_.published_end_at.Value <= dateTime);
      }
    }

    public MissionType missionType => MissionType.guild;

    public object original => (object) this.data_;

    public int ID => this.data_.ID;

    public int priority => this.data_.priority;

    public int progress_max => this.data_.achievement_count;

    public int condition => this.data_.condition;

    public int own_progress_max => this.data_.num;

    public string name => this.data_.name;

    public string detail => this.data_.detail;

    public string scene => this.data_.scene;

    public int point => 0;

    public GuildIMission(GuildMission data) => this.data_ = data;
  }
}
