﻿// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GuildLogKind
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
namespace MasterDataTable
{
  public enum GuildLogKind
  {
    GUILD_ESTABLISHED = 1,
    MEMBER_ACCEPTED = 2,
    MEMBER_LEFT = 3,
    MASTER_ASSIGNED = 4,
    SUBMASTER_ASSIGNED = 5,
    PRIVATE_MESSAGE_UPDATED = 6,
    SETTINGS_MODIFIED = 7,
    BROADCAST_MESSAGE_UPDATED = 8,
    EMBLEM_ACQUIRED = 9,
    EMBLEM_CHANGED = 10, // 0x0000000A
    GIFT_SENT = 11, // 0x0000000B
    BATTLE_BEGAN = 12, // 0x0000000C
    BATTLE_ENDED = 13, // 0x0000000D
    GUILD_LEVEL_UP = 14, // 0x0000000E
    BASE_UNLOCKED = 15, // 0x0000000F
    BASE_RANKUP = 16, // 0x00000010
    BANK_INVEST = 17, // 0x00000011
    MEMBER_CAPACITY_UP = 18, // 0x00000012
    BASE_INVEST_SCALE_UNLOCKED = 19, // 0x00000013
    BASE_FEATURE_UNLOCKED = 20, // 0x00000014
    GVG_ENTRIED = 21, // 0x00000015
    GVG_MATCHED = 22, // 0x00000016
    GVG_STARTED = 23, // 0x00000017
    GVG_WIN = 24, // 0x00000018
    GVG_LOSE = 25, // 0x00000019
    GVG_DRAW = 26, // 0x0000001A
    GVG_DEFENSE_START = 27, // 0x0000001B
    GVG_DEFENSE_WIN = 28, // 0x0000001C
    GVG_DEFENSE_LOSE = 29, // 0x0000001D
    GVG_ATTACK_START_WITH_SUPPORT = 30, // 0x0000001E
    GVG_ATTACK_START = 31, // 0x0000001F
    GVG_ATTACK_LOSE = 32, // 0x00000020
    GVG_ATTACK_WIN_CAPTURE_STAR = 33, // 0x00000021
    GVG_ATTACK_WIN = 34, // 0x00000022
    GVG_SET_DECK_REINFORCEMENT = 35, // 0x00000023
    GVG_RESET = 36, // 0x00000024
    MEMBER_AUTO_LEFT = 37, // 0x00000025
    GUILDRAID_START = 38, // 0x00000026
    GUILDRAID_END = 39, // 0x00000027
    GUILDRAID_ATTACK_WITH_SUPPORT = 40, // 0x00000028
    GUILDRAID_ATTACK = 41, // 0x00000029
    GUILDRAID_DEFEAT_WITH_SUPPORT = 42, // 0x0000002A
    GUILDRAID_DEFEAT = 43, // 0x0000002B
    GUILDRAID_END_WITHOUT_RANK = 44, // 0x0000002C
    GUILDRAID_ATTACK_START_WITH_SUPPORT = 45, // 0x0000002D
    GUILDRAID_ATTACK_START = 46, // 0x0000002E
    GUILDRAID_START_ENDLESS = 47, // 0x0000002F
  }
}
