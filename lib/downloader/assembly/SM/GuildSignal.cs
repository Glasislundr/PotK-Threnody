// Decompiled with JetBrains decompiler
// Type: SM.GuildSignal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace SM
{
  [Serializable]
  public class GuildSignal : KeyCompare
  {
    public GuildEventChat[] chat_events;
    public GuildEventBase[] base_events;
    public GuildEventRelationship[] relationship_events;
    public GuildEventPlayership[] playership_events;
    public GuildEventPayload[] payload_events;
    public GuildEventGift[] gift_events;
    public GuildEvent[] guild_events;
    public GuildEventGvg[] gvg_events;
    public GuildEventEmblem[] emblem_events;
    public GuildEventReward[] reward_events;

    public GuildSignal Clone() => (GuildSignal) this.MemberwiseClone();

    public static GuildSignal Current => SMManager.Get<GuildSignal>();

    public bool existPlayershipEventType(GuildEventType eventType)
    {
      return ((IEnumerable<GuildEventPlayership>) this.playership_events).Any<GuildEventPlayership>((Func<GuildEventPlayership, bool>) (x => x.target_player_id == Player.Current.id && x.event_type == eventType));
    }

    public bool existPlayershipEventTypeWithGuildId(GuildEventType eventType)
    {
      return PlayerAffiliation.Current != null && ((IEnumerable<GuildEventPlayership>) this.playership_events).Any<GuildEventPlayership>((Func<GuildEventPlayership, bool>) (x => x.guild_id == PlayerAffiliation.Current.guild_id && x.event_type == eventType));
    }

    public bool existGuildEvent(GuildEventType eventType)
    {
      return PlayerAffiliation.Current != null && ((IEnumerable<GuildEvent>) this.guild_events).Any<GuildEvent>((Func<GuildEvent, bool>) (x => x.guild_id == PlayerAffiliation.Current.guild_id && x.event_type == eventType));
    }

    public bool existGuildEventRelationship(GuildEventType eventType)
    {
      return ((IEnumerable<GuildEventRelationship>) this.relationship_events).Any<GuildEventRelationship>((Func<GuildEventRelationship, bool>) (x => x.event_type == eventType));
    }

    public bool existRelationshipEventWithoutMyself(GuildEventType eventType)
    {
      return ((IEnumerable<GuildEventRelationship>) this.relationship_events).Any<GuildEventRelationship>((Func<GuildEventRelationship, bool>) (x => x.from_player_id != Player.Current.id && x.event_type == eventType));
    }

    public bool existChatEvent(GuildEventType eventType)
    {
      return ((IEnumerable<GuildEventChat>) this.chat_events).Any<GuildEventChat>((Func<GuildEventChat, bool>) (x => x.guild_id == PlayerAffiliation.Current.guild_id && x.event_type == eventType));
    }

    public bool existBaseEvent(GuildEventType eventType)
    {
      return ((IEnumerable<GuildEventBase>) this.base_events).Any<GuildEventBase>((Func<GuildEventBase, bool>) (x => x.guild_id == PlayerAffiliation.Current.guild_id && x.event_type == eventType));
    }

    public bool existPayloadEvent(GuildEventType eventType)
    {
      return ((IEnumerable<GuildEventPayload>) this.payload_events).Any<GuildEventPayload>((Func<GuildEventPayload, bool>) (x => x.guild_id == PlayerAffiliation.Current.guild_id && x.event_type == eventType));
    }

    public bool existGvgEvent(GuildEventType eventType)
    {
      return ((IEnumerable<GuildEventGvg>) this.gvg_events).Any<GuildEventGvg>((Func<GuildEventGvg, bool>) (x => x.guild_id == PlayerAffiliation.Current.guild_id && x.event_type == eventType));
    }

    public GuildEventGvg getCurrentGvgEvent(GuildEventType eventType)
    {
      return ((IEnumerable<GuildEventGvg>) this.gvg_events).Last<GuildEventGvg>((Func<GuildEventGvg, bool>) (x => x.guild_id == PlayerAffiliation.Current.guild_id && x.event_type == eventType));
    }

    public bool existNewTitle()
    {
      return PlayerAffiliation.Current != null && ((IEnumerable<GuildEventEmblem>) this.emblem_events).Any<GuildEventEmblem>((Func<GuildEventEmblem, bool>) (x => x.guild_id == PlayerAffiliation.Current.guild_id && x.event_type == GuildEventType.assign_emblem));
    }

    public bool existRoleChange()
    {
      int num1 = this.existRelationshipEventWithoutMyself(GuildEventType.change_master) ? 1 : 0;
      bool flag1 = this.existRelationshipEventWithoutMyself(GuildEventType.new_submaster);
      bool flag2 = this.existRelationshipEventWithoutMyself(GuildEventType.leave_submaster);
      int num2 = flag1 ? 1 : 0;
      return (num1 | num2 | (flag2 ? 1 : 0)) != 0;
    }

    public void removePlayershipEvent(GuildEventType eventType)
    {
      List<GuildEventPlayership> list = ((IEnumerable<GuildEventPlayership>) this.playership_events).ToList<GuildEventPlayership>();
      list.RemoveAll((Predicate<GuildEventPlayership>) (x => x.event_type == eventType));
      this.playership_events = list.ToArray();
    }

    public void removeRelationshipEvent(GuildEventType eventType)
    {
      List<GuildEventRelationship> list = ((IEnumerable<GuildEventRelationship>) this.relationship_events).ToList<GuildEventRelationship>();
      list.RemoveAll((Predicate<GuildEventRelationship>) (x => x.event_type == eventType));
      this.relationship_events = list.ToArray();
    }

    public bool existNewApplicant()
    {
      if (!((IEnumerable<GuildEventPlayership>) this.playership_events).Any<GuildEventPlayership>((Func<GuildEventPlayership, bool>) (x => x.event_type == GuildEventType.new_applicant)))
        return false;
      if (!((IEnumerable<GuildEventPlayership>) this.playership_events).Any<GuildEventPlayership>((Func<GuildEventPlayership, bool>) (x => x.event_type == GuildEventType.cancel_applicant)))
        return true;
      List<string> cancelIDs = new List<string>();
      List<string> applicantIDs = new List<string>();
      ((IEnumerable<GuildEventPlayership>) this.playership_events).Where<GuildEventPlayership>((Func<GuildEventPlayership, bool>) (x => x.event_type == GuildEventType.cancel_applicant)).ForEach<GuildEventPlayership>((Action<GuildEventPlayership>) (x => cancelIDs.Add(x.target_player_id)));
      ((IEnumerable<GuildEventPlayership>) this.playership_events).Where<GuildEventPlayership>((Func<GuildEventPlayership, bool>) (x => x.event_type == GuildEventType.new_applicant)).ForEach<GuildEventPlayership>((Action<GuildEventPlayership>) (x => applicantIDs.Add(x.target_player_id)));
      return applicantIDs.Except<string>((IEnumerable<string>) cancelIDs).Any<string>();
    }

    public bool existNewMember()
    {
      return ((IEnumerable<GuildEventPlayership>) this.playership_events).Any<GuildEventPlayership>((Func<GuildEventPlayership, bool>) (x => x.event_type == GuildEventType.apply_applicant));
    }

    public GuildSignal()
    {
    }

    public GuildSignal(Dictionary<string, object> json)
    {
      this._hasKey = false;
      List<GuildEventChat> guildEventChatList = new List<GuildEventChat>();
      foreach (object json1 in (List<object>) json[nameof (chat_events)])
        guildEventChatList.Add(json1 == null ? (GuildEventChat) null : new GuildEventChat((Dictionary<string, object>) json1));
      this.chat_events = guildEventChatList.ToArray();
      List<GuildEventBase> guildEventBaseList = new List<GuildEventBase>();
      foreach (object json2 in (List<object>) json[nameof (base_events)])
        guildEventBaseList.Add(json2 == null ? (GuildEventBase) null : new GuildEventBase((Dictionary<string, object>) json2));
      this.base_events = guildEventBaseList.ToArray();
      List<GuildEventRelationship> eventRelationshipList = new List<GuildEventRelationship>();
      foreach (object json3 in (List<object>) json[nameof (relationship_events)])
        eventRelationshipList.Add(json3 == null ? (GuildEventRelationship) null : new GuildEventRelationship((Dictionary<string, object>) json3));
      this.relationship_events = eventRelationshipList.ToArray();
      List<GuildEventPlayership> guildEventPlayershipList = new List<GuildEventPlayership>();
      foreach (object json4 in (List<object>) json[nameof (playership_events)])
        guildEventPlayershipList.Add(json4 == null ? (GuildEventPlayership) null : new GuildEventPlayership((Dictionary<string, object>) json4));
      this.playership_events = guildEventPlayershipList.ToArray();
      List<GuildEventPayload> guildEventPayloadList = new List<GuildEventPayload>();
      foreach (object json5 in (List<object>) json[nameof (payload_events)])
        guildEventPayloadList.Add(json5 == null ? (GuildEventPayload) null : new GuildEventPayload((Dictionary<string, object>) json5));
      this.payload_events = guildEventPayloadList.ToArray();
      List<GuildEventGift> guildEventGiftList = new List<GuildEventGift>();
      foreach (object json6 in (List<object>) json[nameof (gift_events)])
        guildEventGiftList.Add(json6 == null ? (GuildEventGift) null : new GuildEventGift((Dictionary<string, object>) json6));
      this.gift_events = guildEventGiftList.ToArray();
      List<GuildEvent> guildEventList = new List<GuildEvent>();
      foreach (object json7 in (List<object>) json[nameof (guild_events)])
        guildEventList.Add(json7 == null ? (GuildEvent) null : new GuildEvent((Dictionary<string, object>) json7));
      this.guild_events = guildEventList.ToArray();
      List<GuildEventGvg> guildEventGvgList = new List<GuildEventGvg>();
      foreach (object json8 in (List<object>) json[nameof (gvg_events)])
        guildEventGvgList.Add(json8 == null ? (GuildEventGvg) null : new GuildEventGvg((Dictionary<string, object>) json8));
      this.gvg_events = guildEventGvgList.ToArray();
      List<GuildEventEmblem> guildEventEmblemList = new List<GuildEventEmblem>();
      foreach (object json9 in (List<object>) json[nameof (emblem_events)])
        guildEventEmblemList.Add(json9 == null ? (GuildEventEmblem) null : new GuildEventEmblem((Dictionary<string, object>) json9));
      this.emblem_events = guildEventEmblemList.ToArray();
      List<GuildEventReward> guildEventRewardList = new List<GuildEventReward>();
      foreach (object json10 in (List<object>) json[nameof (reward_events)])
        guildEventRewardList.Add(json10 == null ? (GuildEventReward) null : new GuildEventReward((Dictionary<string, object>) json10));
      this.reward_events = guildEventRewardList.ToArray();
    }
  }
}
