// Decompiled with JetBrains decompiler
// Type: MissionData.GuildIMissionReward
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;

#nullable disable
namespace MissionData
{
  internal class GuildIMissionReward : IMissionReward
  {
    private GuildMissionReward data_;

    public bool isDaily => false;

    public bool isGuild => true;

    public object original => (object) this.data_;

    public int reward_quantity => this.data_.reward_quantity;

    public string reward_message => this.data_.reward_message;

    public int reward_type_id => this.data_.reward_type_id;

    public int reward_id => this.data_.reward_id;

    public GuildIMissionReward(GuildMissionReward dat) => this.data_ = dat;
  }
}
