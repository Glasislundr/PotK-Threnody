// Decompiled with JetBrains decompiler
// Type: GuildChatMessageData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class GuildChatMessageData
{
  public long messageID;
  public string senderID;
  public string messageContent;
  public string senderName;
  public DateTime sendTime;
  public int spriteID;
  public int stampID;
  public bool isDeleted;
  public GuildMembership membership;
  public GuildChatMessageData.GuildChatMessageType messageType;
  public int kind_id;
  public GuildLogTransition transition;
  public float topPosition;
  public float bottomPosition;
  public bool isRaidDeckView;

  public GuildChatMessageData(GuildLog data)
  {
    this.messageID = long.Parse(data.log_id);
    this.senderID = data.log_author_id;
    this.senderName = data.log_author_name;
    this.messageContent = data.log_text;
    this.sendTime = Convert.ToDateTime(data.created_at);
    this.kind_id = data.kind_id;
    this.stampID = data.stamp_id;
    this.isDeleted = data.is_deleted;
    this.spriteID = 0;
    this.membership = ((IEnumerable<GuildMembership>) PlayerAffiliation.Current.guild.memberships).FirstOrDefault<GuildMembership>((Func<GuildMembership, bool>) (x => x.player.player_id == data.log_author_id));
    if (this.membership != null)
      this.spriteID = this.membership.player.leader_unit_unit.resource_reference_unit_id.ID;
    if (data.log_type == 1)
      this.messageType = !(data.log_author_id == Player.Current.id) ? GuildChatMessageData.GuildChatMessageType.MemberChat : GuildChatMessageData.GuildChatMessageType.PlayerChat;
    else if (data.log_type == 2)
      this.messageType = !(data.log_author_id == Player.Current.id) ? GuildChatMessageData.GuildChatMessageType.MemberLog : GuildChatMessageData.GuildChatMessageType.PlayerLog;
    else if (data.log_type == 3)
      this.messageType = GuildChatMessageData.GuildChatMessageType.SystemLog;
    else if (data.log_type == 4)
      this.messageType = !(data.log_author_id == Player.Current.id) ? GuildChatMessageData.GuildChatMessageType.MemberStamp : GuildChatMessageData.GuildChatMessageType.PlayerStamp;
    else if (data.log_type == 5)
      this.messageType = GuildChatMessageData.GuildChatMessageType.GuildRaidLog;
    else if (data.log_type == 6)
      this.messageType = GuildChatMessageData.GuildChatMessageType.GuildRaidSystemLog;
    this.transition = data.transition;
  }

  public GuildChatMessageData(
    WebAPI.Response.GuildlogMemberShowLatestGuild_logs data)
  {
    this.messageID = long.Parse(data.log_id);
    this.senderID = data.log_author_id;
    this.senderName = data.log_author_name;
    this.messageContent = data.log_text;
    this.sendTime = Convert.ToDateTime(data.created_at);
    this.isDeleted = data.is_deleted;
    this.spriteID = 0;
    this.membership = ((IEnumerable<GuildMembership>) PlayerAffiliation.Current.guild.memberships).FirstOrDefault<GuildMembership>((Func<GuildMembership, bool>) (x => x.player.player_id == data.log_author_id));
    if (this.membership != null)
      this.spriteID = this.membership.player.leader_unit_unit.resource_reference_unit_id.ID;
    if (data.log_type == 1)
    {
      if (data.log_author_id == Player.Current.id)
        this.messageType = GuildChatMessageData.GuildChatMessageType.PlayerChat;
      else
        this.messageType = GuildChatMessageData.GuildChatMessageType.MemberChat;
    }
    else if (data.log_type == 2)
    {
      if (data.log_author_id == Player.Current.id)
        this.messageType = GuildChatMessageData.GuildChatMessageType.PlayerLog;
      else
        this.messageType = GuildChatMessageData.GuildChatMessageType.MemberLog;
    }
    else if (data.log_type == 3)
      this.messageType = GuildChatMessageData.GuildChatMessageType.SystemLog;
    else if (data.log_type == 5)
    {
      this.messageType = GuildChatMessageData.GuildChatMessageType.GuildRaidLog;
    }
    else
    {
      if (data.log_type != 6)
        return;
      this.messageType = GuildChatMessageData.GuildChatMessageType.GuildRaidSystemLog;
    }
  }

  public GuildChatMessageData(
    WebAPI.Response.GuildlogMemberShowPastGuild_logs data)
  {
    this.messageID = long.Parse(data.log_id);
    this.senderID = data.log_author_id;
    this.senderName = data.log_author_name;
    this.messageContent = data.log_text;
    this.sendTime = Convert.ToDateTime(data.created_at);
    this.isDeleted = data.is_deleted;
    this.spriteID = 0;
    this.membership = ((IEnumerable<GuildMembership>) PlayerAffiliation.Current.guild.memberships).FirstOrDefault<GuildMembership>((Func<GuildMembership, bool>) (x => x.player.player_id == data.log_author_id));
    if (this.membership != null)
      this.spriteID = this.membership.player.leader_unit_unit.resource_reference_unit_id.ID;
    if (data.log_type == 1)
    {
      if (data.log_author_id == Player.Current.id)
        this.messageType = GuildChatMessageData.GuildChatMessageType.PlayerChat;
      else
        this.messageType = GuildChatMessageData.GuildChatMessageType.MemberChat;
    }
    else if (data.log_type == 2)
    {
      if (data.log_author_id == Player.Current.id)
        this.messageType = GuildChatMessageData.GuildChatMessageType.PlayerLog;
      else
        this.messageType = GuildChatMessageData.GuildChatMessageType.MemberLog;
    }
    else if (data.log_type == 3)
      this.messageType = GuildChatMessageData.GuildChatMessageType.SystemLog;
    else if (data.log_type == 5)
    {
      this.messageType = GuildChatMessageData.GuildChatMessageType.GuildRaidLog;
    }
    else
    {
      if (data.log_type != 6)
        return;
      this.messageType = GuildChatMessageData.GuildChatMessageType.GuildRaidSystemLog;
    }
  }

  public string GetFormattedSendTime()
  {
    TimeSpan timeSpan = ServerTime.NowAppTime() + (DateTime.Now - ServerTime.LastSyncLocalTime()) - this.sendTime;
    double totalDays = timeSpan.TotalDays;
    string formattedSendTime;
    if (totalDays > 1.0)
    {
      formattedSendTime = Mathf.FloorToInt((float) totalDays).ToString() + Consts.GetInstance().GUILD_CHAT_SEND_TIME_DAYS_AGO;
    }
    else
    {
      double totalHours = timeSpan.TotalHours;
      if (totalHours > 1.0)
      {
        formattedSendTime = Mathf.FloorToInt((float) totalHours).ToString() + Consts.GetInstance().GUILD_CHAT_SEND_TIME_HOURS_AGO;
      }
      else
      {
        double totalMinutes = timeSpan.TotalMinutes;
        if (totalMinutes > 1.0)
        {
          formattedSendTime = Mathf.FloorToInt((float) totalMinutes).ToString() + Consts.GetInstance().GUILD_CHAT_SEND_TIME_MINUTES_AGO;
        }
        else
        {
          double totalSeconds = timeSpan.TotalSeconds;
          if (totalSeconds < 10.0)
            formattedSendTime = Consts.GetInstance().GUILD_CHAT_SEND_TIME_JUST_NOW;
          else
            formattedSendTime = Consts.Format(Consts.GetInstance().GUILD_CHAT_SEND_TIME_10_SECONDS_AGO, (IDictionary) new Hashtable()
            {
              {
                (object) "seconds",
                (object) Mathf.FloorToInt((float) totalSeconds / 10f)
              }
            });
        }
      }
    }
    return formattedSendTime;
  }

  public enum GuildChatMessageType
  {
    PlayerChat = 1,
    PlayerLog = 2,
    MemberChat = 3,
    MemberLog = 4,
    SystemLog = 5,
    PlayerStamp = 6,
    MemberStamp = 7,
    GuildRaidLog = 8,
    GuildRaidSystemLog = 9,
  }

  public enum ReceivedDataType
  {
    Chat = 1,
    Log = 2,
    SystemLog = 3,
    Stamp = 4,
    GuildRaidLog = 5,
    GuildRaidSystemLog = 6,
  }
}
