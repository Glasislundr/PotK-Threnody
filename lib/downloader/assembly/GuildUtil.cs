// Decompiled with JetBrains decompiler
// Type: GuildUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class GuildUtil
{
  public const string ERR_NG_WORD = "GLD011";
  public const string ERR_NO_APPLICANT = "GLD006";
  public const string ERR_APPLIED = "GLD001";
  public const string ERR_NO_AUTOLIZATION = "GLD002";
  public const string ERR_GUILD_MAINTENANCE = "GLD014";
  public const string ERR_GUILD_CHAT_MAINTENANCE = "GLD015";
  public const string ERR_GUILD_FACILITY_CONSISTENCY = "GLD016";
  public const string ERR_RAID_MEMBER_CHANGE_NOT_AVAILABLE = "GLD020";
  public const string ERR_NOT_GUILD_MEMBER = "GLD003";
  public const string ERR_GVG_BATTLE_ALREADY_FINISHED = "GVG001";
  public const string ERR_GVG_CANNOT_USE_ON_GVG = "GVG002";
  public const string ERR_GVG_INVALID_GVG_RESULT = "GVG003";
  public const string ERR_GVG_BATTLE_TARGET_IS_IN_BATTLE = "GVG005";
  public const string ERR_GVG_BATTLE_EXPIRED = "GVG006";
  public const string ERR_GVG_DEF_FRIEND_ALREADY_SELECTED = "GVG007";
  public const string slcTextGLVNumber = "slc_text_glv_number{0}.png__GUI__guild_common_other__guild_common_other_prefab";
  public const int GvgExcellentWinStarNum = 3;
  public const int GvgGreatWinStarNum = 2;
  public const int GvgWinStarNum = 1;
  public const int RP_MAX_NUM = 3;
  public static GvgDeck gvgDeckAttack = (GvgDeck) null;
  public static GvgDeck gvgDeckDefense = (GvgDeck) null;
  public static GvgReinforcement gvgFriendDefense = (GvgReinforcement) null;
  public static GvgCandidate gvgFriendAttack = (GvgCandidate) null;
  public static GuildUtil.GvGPopupState gvgPopupState = GuildUtil.GvGPopupState.None;
  public static string gvgBattleIDServer = string.Empty;
  public static int last_edit_slot_no;
  public static PlayerUnit[] RaidDeck = new PlayerUnit[0];
  public static int[] RaidUsedUnitIds = new int[0];
  public static GvgCandidate RaidFriend = (GvgCandidate) null;
  public static int rp;
  public static int rp_max;
  public static float GuildMemberListPopupSlideValue;

  public static void UpdateRaidDeckInfo()
  {
    PlayerUnit[] playerUnits = SMManager.Get<PlayerUnit[]>();
    GuildUtil.RaidDeck = ((IEnumerable<PlayerUnit>) GuildUtil.RaidDeck).Select<PlayerUnit, PlayerUnit>((Func<PlayerUnit, PlayerUnit>) (x => ((IEnumerable<PlayerUnit>) playerUnits).FirstOrDefault<PlayerUnit>((Func<PlayerUnit, bool>) (y => x.id == y.id)))).Where<PlayerUnit>((Func<PlayerUnit, bool>) (z => z != (PlayerUnit) null)).ToArray<PlayerUnit>();
  }

  public static void ClearCache()
  {
    GuildUtil.gvgDeckAttack = (GvgDeck) null;
    GuildUtil.gvgDeckDefense = (GvgDeck) null;
    GuildUtil.gvgFriendDefense = (GvgReinforcement) null;
    GuildUtil.gvgFriendAttack = (GvgCandidate) null;
    GuildUtil.gvgPopupState = GuildUtil.GvGPopupState.None;
    GuildUtil.gvgBattleIDServer = string.Empty;
    GuildUtil.last_edit_slot_no = 0;
    GuildUtil.RaidDeck = new PlayerUnit[0];
    GuildUtil.RaidUsedUnitIds = new int[0];
    GuildUtil.RaidFriend = (GvgCandidate) null;
    GuildUtil.rp = 0;
    GuildUtil.rp_max = 0;
  }

  public static void resetSettingPersist()
  {
    if (Persist.guildSetting.Exists)
    {
      Persist.guildSetting.Data.reset();
      Persist.guildSetting.Flush();
    }
    if (Persist.guildHeaderChat.Exists)
    {
      Persist.guildHeaderChat.Data.reset();
      Persist.guildHeaderChat.Flush();
    }
    if (Persist.guildTopLevel.Exists)
    {
      Persist.guildTopLevel.Data.reset();
      Persist.guildTopLevel.Flush();
    }
    if (Persist.gvgBattleEnvironment.Exists)
      Persist.gvgBattleEnvironment.Delete();
    if (Persist.guildBattleUser.Exists)
      Persist.guildBattleUser.Delete();
    PlayerAffiliation current = PlayerAffiliation.Current;
    if (current == null || !current.isGuildMember())
      return;
    Persist.guildTopLevel.Data.guildID = current.guild.guild_id;
    Persist.guildTopLevel.Data.level = current.guild.appearance.level;
    Persist.guildTopLevel.Flush();
  }

  public static void setBadgeState(GuildUtil.GuildBadgeInfoType key, bool state)
  {
    try
    {
      Persist.guildSetting.Data.setBadgeState(key, state);
    }
    catch (Exception ex)
    {
      Persist.guildSetting.Delete();
      Persist.guildSetting.Data.reset();
    }
  }

  public static bool getBadgeState(GuildUtil.GuildBadgeInfoType key)
  {
    try
    {
      return Persist.guildSetting.Data.getBadgeState(key);
    }
    catch (Exception ex)
    {
      Persist.guildSetting.Delete();
      Persist.guildSetting.Data.reset();
    }
    return false;
  }

  public static int getGuildMemberNum()
  {
    try
    {
      return Persist.guildSetting.Data.memberNum;
    }
    catch (Exception ex)
    {
      Persist.guildSetting.Delete();
      Persist.guildSetting.Data.reset();
    }
    return -1;
  }

  public static void setGuildMemberNum(int num)
  {
    try
    {
      Persist.guildSetting.Data.memberNum = num;
    }
    catch (Exception ex)
    {
      Persist.guildSetting.Delete();
      Persist.guildSetting.Data.reset();
    }
  }

  public static void setTitleSortCategory(int val)
  {
    try
    {
      Persist.guildSetting.Data.titleSortCategory = val;
    }
    catch (Exception ex)
    {
      Persist.guildSetting.Delete();
      Persist.guildSetting.Data.reset();
    }
  }

  public static int getTitleSortCategory()
  {
    try
    {
      return Persist.guildSetting.Data.titleSortCategory;
    }
    catch (Exception ex)
    {
      Persist.guildSetting.Delete();
      Persist.guildSetting.Data.reset();
    }
    return 0;
  }

  public static void setTimeTitleAppear(DateTime time)
  {
    try
    {
      Persist.guildSetting.Data.timeTitleAppear = time;
    }
    catch (Exception ex)
    {
      Persist.guildSetting.Delete();
      Persist.guildSetting.Data.reset();
    }
  }

  public static DateTime getTimeTitleAppear()
  {
    try
    {
      return Persist.guildSetting.Data.timeTitleAppear;
    }
    catch (Exception ex)
    {
      Persist.guildSetting.Delete();
      Persist.guildSetting.Data.reset();
    }
    return new DateTime(2000, 1, 1);
  }

  public static string gvgBattleIDLocal
  {
    get
    {
      try
      {
        return Persist.gvgBattleEnvironment.Data.core.battleInfo.battleId;
      }
      catch (Exception ex)
      {
      }
      return string.Empty;
    }
  }

  public static IEnumerator EditGuildDeckAttack(int[] playerUnitIds, Action action = null)
  {
    string errCode = string.Empty;
    Future<WebAPI.Response.GvgDeckAttackEdit> atkTeam = WebAPI.GvgDeckAttackEdit(playerUnitIds, (Action<WebAPI.Response.UserError>) (e =>
    {
      errCode = e.Code;
      if (e.Code.Equals("GVG002"))
        return;
      WebAPI.DefaultUserErrorCallback(e);
    }));
    IEnumerator e1 = atkTeam.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (errCode.Equals("GLD014"))
    {
      yield return (object) null;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      MypageScene.ChangeSceneOnError();
    }
    else if (errCode.Equals("GVG002"))
    {
      ModalWindow.Show(Consts.GetInstance().POPUP_GUILD_BATTLE_DEF_TEAM_TITLE, Consts.GetInstance().POPUP_GUILD_BATTLE_ATK_TEAM_ERROR, (Action) (() => { }));
    }
    else
    {
      if (atkTeam.Result != null)
      {
        GuildUtil.gvgDeckAttack = atkTeam.Result.deck;
        GuildUtil.SetEquippedGearAndAwakeSkill(GuildUtil.gvgDeckAttack);
        GuildUtil.SetOverkillersUnits(GuildUtil.gvgDeckAttack);
        GuildUtil.UpdateMembershipDeckValue(PlayerAffiliation.Current.guild.memberships, atkTeam.Result.deck, Player.Current.id, true);
      }
      if (action != null)
        action();
    }
  }

  public static IEnumerator EditGuildDeckDefense(int[] playerUnitIds, Action action = null)
  {
    string errCode = string.Empty;
    Future<WebAPI.Response.GvgDeckDefenseEdit> defTeam = WebAPI.GvgDeckDefenseEdit(playerUnitIds, (Action<WebAPI.Response.UserError>) (e =>
    {
      errCode = e.Code;
      if (e.Code.Equals("GVG002"))
        return;
      WebAPI.DefaultUserErrorCallback(e);
    }));
    IEnumerator e1 = defTeam.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (errCode.Equals("GLD014"))
    {
      yield return (object) null;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      MypageScene.ChangeSceneOnError();
    }
    else if (errCode.Equals("GVG002"))
    {
      ModalWindow.Show(Consts.GetInstance().POPUP_GUILD_BATTLE_DEF_TEAM_TITLE, Consts.GetInstance().POPUP_GUILD_BATTLE_DEF_TEAM_ERROR, (Action) (() => { }));
    }
    else
    {
      if (defTeam.Result != null)
      {
        GuildUtil.gvgDeckDefense = defTeam.Result.deck;
        GuildUtil.SetEquippedGearAndAwakeSkill(GuildUtil.gvgDeckDefense);
        GuildUtil.SetOverkillersUnits(GuildUtil.gvgDeckDefense);
        GuildUtil.UpdateMembershipDeckValue(PlayerAffiliation.Current.guild.memberships, defTeam.Result.deck, Player.Current.id, false);
      }
      if (action != null)
        action();
    }
  }

  public static IEnumerator UpdateGuildDeckAttack(string guild_id, string player_id, Action action = null)
  {
    bool maintenance = false;
    Future<WebAPI.Response.GvgDeckAttackShow> atkTeam = WebAPI.GvgDeckAttackShow(guild_id, player_id, (Action<WebAPI.Response.UserError>) (e =>
    {
      if (e.Code.Equals("GLD014"))
        maintenance = true;
      WebAPI.DefaultUserErrorCallback(e);
    }));
    IEnumerator e1 = atkTeam.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (maintenance)
    {
      yield return (object) null;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      MypageScene.ChangeSceneOnError();
    }
    else
    {
      if (atkTeam.Result != null)
      {
        GuildUtil.gvgDeckAttack = atkTeam.Result.deck;
        GuildUtil.SetEquippedGearAndAwakeSkill(GuildUtil.gvgDeckAttack);
        GuildUtil.SetOverkillersUnits(GuildUtil.gvgDeckAttack);
        GuildUtil.UpdateMembershipDeckValue(PlayerAffiliation.Current.guild.memberships, atkTeam.Result.deck, player_id, true);
      }
      if (action != null)
        action();
    }
  }

  public static IEnumerator UpdateGuildDeckDefanse(
    string guild_id,
    string player_id,
    Action action = null)
  {
    bool maintenance = false;
    Future<WebAPI.Response.GvgDeckDefenseShow> defTeam = WebAPI.GvgDeckDefenseShow(guild_id, player_id, (Action<WebAPI.Response.UserError>) (e =>
    {
      if (e.Code.Equals("GLD014"))
        maintenance = true;
      WebAPI.DefaultUserErrorCallback(e);
    }));
    IEnumerator e1 = defTeam.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (maintenance)
    {
      yield return (object) null;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      MypageScene.ChangeSceneOnError();
    }
    else
    {
      if (defTeam.Result != null)
      {
        GuildUtil.gvgDeckDefense = defTeam.Result.deck;
        GuildUtil.SetEquippedGearAndAwakeSkill(GuildUtil.gvgDeckDefense);
        GuildUtil.SetOverkillersUnits(GuildUtil.gvgDeckDefense);
        GuildUtil.SetGvgFriendDefense(defTeam.Result.reinforcement);
        GuildUtil.UpdateMembershipDeckValue(PlayerAffiliation.Current.guild.memberships, defTeam.Result.deck, player_id, false);
      }
      if (action != null)
        action();
    }
  }

  public static void SetEquippedGearAndAwakeSkill(GvgDeck deck)
  {
    if (deck == null || deck.player_units == null)
      return;
    foreach (PlayerUnit playerUnit in deck.player_units)
      GuildUtil.SetEquippedGearAndAwakeSkill(playerUnit, deck.player_gears, deck.player_reisou_gears, deck.player_awake_skills);
  }

  public static void SetEquippedGearAndAwakeSkill(
    PlayerUnit unit,
    PlayerItem[] gears,
    PlayerGearReisouSchema[] reisou_gears,
    PlayerAwakeSkill[] awakeSkills)
  {
    if (unit == (PlayerUnit) null)
      return;
    unit.primary_equipped_gear = unit.FindEquippedGear(gears);
    unit.primary_equipped_gear2 = unit.FindEquippedGear2(gears);
    unit.primary_equipped_gear3 = unit.FindEquippedGear3(gears);
    unit.primary_equipped_reisou = unit.FindEquippedReisou(gears, reisou_gears);
    unit.primary_equipped_reisou2 = unit.FindEquippedReisou2(gears, reisou_gears);
    unit.primary_equipped_reisou3 = unit.FindEquippedReisou3(gears, reisou_gears);
    unit.primary_equipped_awake_skill = unit.FindEquippedExtraSkill(awakeSkills);
    unit.usedPrimary = PlayerUnit.UsedPrimary.All;
  }

  public static void SetOverkillersUnits(GvgDeck deck)
  {
    if (deck == null || deck.player_units == null)
      return;
    foreach (PlayerUnit playerUnit in deck.player_units)
      GuildUtil.SetOverkillersUnits(playerUnit, deck.over_killers);
  }

  public static void SetOverkillersUnits(PlayerUnit target, PlayerUnit[] overkillersUnits)
  {
    if (!(target != (PlayerUnit) null) || !target.isDirtyOverkillersSlots)
      return;
    target.importOverkillersUnits(overkillersUnits);
    target.resetOverkillersParameter();
  }

  public static void SetGvgFriendDefense(GvgReinforcement friend)
  {
    GuildUtil.gvgFriendDefense = friend;
    if (GuildUtil.gvgFriendDefense == null || !(GuildUtil.gvgFriendDefense.player_unit != (PlayerUnit) null))
      return;
    GuildUtil.SetEquippedGearAndAwakeSkill(GuildUtil.gvgFriendDefense.player_unit, GuildUtil.gvgFriendDefense.player_gears, GuildUtil.gvgFriendDefense.player_reisou_gears, GuildUtil.gvgFriendDefense.player_awake_skills);
    GuildUtil.SetOverkillersUnits(GuildUtil.gvgFriendDefense.player_unit, friend.over_killers);
  }

  public static void UpdateMembershipDeckValue(
    GuildMembership[] members,
    GvgDeck deck,
    string player_id,
    bool isAttack)
  {
    if (PlayerAffiliation.Current == null)
      return;
    for (int index = 0; index < members.Length; ++index)
    {
      GuildMembership member = members[index];
      if (member.player.player_id.Equals(player_id))
      {
        int combat = 0;
        ((IEnumerable<PlayerUnit>) deck.player_units).ForEach<PlayerUnit>((Action<PlayerUnit>) (x =>
        {
          if (!(x != (PlayerUnit) null))
            return;
          combat += Judgement.NonBattleParameter.FromPlayerUnit(x).Combat;
        }));
        if (isAttack)
        {
          member.total_attack = combat;
          break;
        }
        member.total_defense = combat;
        break;
      }
    }
  }

  public static float GVGStartHour() => GuildUtil.GVGKeyGetHour("GVG_START_HOUR");

  public static float GVGEndHour() => GuildUtil.GVGKeyGetHour("GVG_END_HOUR");

  public static float GVGReleaseEntryHour() => GuildUtil.GVGKeyGetHour("GVG_RELEASE_ENTRY_HOUR");

  public static float GVGEntryExpiredHour() => GuildUtil.GVGKeyGetHour("GVG_ENTRY_EXPIRED_HOUR");

  public static int GetGuildSettingInt(string key)
  {
    float result = 0.0f;
    float.TryParse(((IEnumerable<MasterDataTable.GuildSetting>) MasterData.GuildSettingList).Where<MasterDataTable.GuildSetting>((Func<MasterDataTable.GuildSetting, bool>) (x => x.key == key)).FirstOrDefault<MasterDataTable.GuildSetting>().value, out result);
    return (int) result;
  }

  private static float GVGKeyGetHour(string key)
  {
    float result = 0.0f;
    float.TryParse(((IEnumerable<MasterDataTable.GuildSetting>) MasterData.GuildSettingList).Where<MasterDataTable.GuildSetting>((Func<MasterDataTable.GuildSetting, bool>) (x => x.key == key)).FirstOrDefault<MasterDataTable.GuildSetting>().value, out result);
    return result;
  }

  public static IEnumerator TimeCountSprite(
    List<SpriteNumberSelectDirect> hour,
    List<SpriteNumberSelectDirect> minute,
    double actionTime,
    Action action,
    NGMenuBase menu)
  {
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    DateTime now = ServerTime.NowAppTimeAddDelta().ToUniversalTime().AddHours(9.0);
    DateTime targetTime = GuildUtil.TargetTime(now, actionTime);
    while (!Object.op_Equality((Object) menu, (Object) null))
    {
      TimeSpan timeSpan = targetTime.Subtract(now);
      if (Object.op_Inequality((Object) hour[0], (Object) null))
        hour[0].setNumber(Mathf.Abs(timeSpan.Hours % 10), Color.white);
      if (Object.op_Inequality((Object) hour[1], (Object) null))
        hour[1].setNumber(Mathf.Abs(timeSpan.Hours / 10), Color.white);
      if (Object.op_Inequality((Object) minute[0], (Object) null))
        minute[0].setNumber(Mathf.Abs(timeSpan.Minutes % 10), Color.white);
      if (Object.op_Inequality((Object) minute[1], (Object) null))
        minute[1].setNumber(Mathf.Abs(timeSpan.Minutes / 10), Color.white);
      if (timeSpan.TotalSeconds > 0.0)
      {
        yield return (object) new WaitForSeconds(1f);
        now = ServerTime.NowAppTimeAddDelta().ToUniversalTime().AddHours(9.0);
      }
      else
      {
        if (Object.op_Equality((Object) menu, (Object) null))
          break;
        action();
        break;
      }
    }
  }

  public static IEnumerator TimeCountText(
    UILabel label,
    string text,
    double actionTime,
    Action action,
    MonoBehaviour menu)
  {
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    DateTime now = ServerTime.NowAppTimeAddDelta().ToUniversalTime().AddHours(9.0);
    DateTime targetTime = GuildUtil.TargetTime(now, actionTime);
    while (!Object.op_Equality((Object) menu, (Object) null))
    {
      TimeSpan timeSpan = targetTime.Subtract(now);
      label.SetTextLocalize(Consts.Format(text, (IDictionary) new Hashtable()
      {
        {
          (object) "hour",
          (object) timeSpan.Hours
        },
        {
          (object) "minute",
          (object) string.Format("{0:D2}", (object) timeSpan.Minutes)
        }
      }));
      if (timeSpan.TotalSeconds > 0.0)
      {
        yield return (object) new WaitForSeconds(1f);
        now = ServerTime.NowAppTimeAddDelta().ToUniversalTime().AddHours(9.0);
      }
      else
      {
        if (Object.op_Equality((Object) menu, (Object) null))
          break;
        action();
        break;
      }
    }
  }

  private static DateTime TargetTime(DateTime now, double actionTime)
  {
    int hour = now.Hour;
    DateTime dateTime = new DateTime(now.Year, now.Month, now.Day);
    if ((double) hour >= actionTime)
      dateTime = dateTime.AddDays(1.0);
    return dateTime.AddHours(actionTime);
  }

  public static bool isBattleOrPreparing(GvgStatus gvgStatus)
  {
    return gvgStatus == GvgStatus.preparing || gvgStatus == GvgStatus.fighting;
  }

  public static bool isBattle(GvgStatus gvgStatus) => gvgStatus == GvgStatus.fighting;

  public static Tuple<bool, bool, GuildUtil.GuildBadgeLabelType> getBadgeInfo()
  {
    bool flag1 = false;
    bool flag2 = false;
    GuildUtil.GuildBadgeLabelType guildBadgeLabelType = GuildUtil.GuildBadgeLabelType.none;
    bool flag3 = false;
    if (PlayerAffiliation.Current == null)
      return new Tuple<bool, bool, GuildUtil.GuildBadgeLabelType>(flag1, flag2, guildBadgeLabelType);
    if (!Persist.guildSetting.Exists)
    {
      Persist.guildSetting.Data.reset();
      Persist.guildSetting.Flush();
    }
    if (!Persist.guildSetting.Exists)
      return new Tuple<bool, bool, GuildUtil.GuildBadgeLabelType>(flag1, flag2, guildBadgeLabelType);
    if (!PlayerAffiliation.Current.isGuildMember())
    {
      if (SM.GuildSignal.Current.existRelationshipEventWithoutMyself(GuildEventType.leave_membership))
        flag1 = true;
      if (Persist.gvgBattleEnvironment.Exists)
        Persist.gvgBattleEnvironment.Delete();
      return new Tuple<bool, bool, GuildUtil.GuildBadgeLabelType>(flag1, flag2, guildBadgeLabelType);
    }
    bool flag4 = Singleton<NGSceneManager>.GetInstance().sceneName.Equals("guild028_1") || Singleton<NGSceneManager>.GetInstance().sceneName.Equals("guild028_3") || Singleton<NGSceneManager>.GetInstance().sceneName.Equals("raid_top");
    int num = !Object.op_Inequality((Object) Singleton<NGSceneManager>.GetInstance().sceneBase, (Object) null) ? 0 : (Singleton<NGSceneManager>.GetInstance().sceneBase.currentSceneGuildChatDisplayingStatus == NGSceneBase.GuildChatDisplayingStatus.Opened ? 1 : 0);
    GuildRole? role = PlayerAffiliation.Current.role;
    GuildRole guildRole1 = GuildRole.master;
    if (!(role.GetValueOrDefault() == guildRole1 & role.HasValue))
    {
      role = PlayerAffiliation.Current.role;
      GuildRole guildRole2 = GuildRole.sub_master;
      if (!(role.GetValueOrDefault() == guildRole2 & role.HasValue))
      {
        GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.newApplicant, false);
        GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.newMember, false);
        goto label_19;
      }
    }
    if (SM.GuildSignal.Current.existNewApplicant())
    {
      GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.newApplicant, true);
      flag3 = true;
    }
    if (PlayerAffiliation.Current.guild.auto_approval.auto_approval && SM.GuildSignal.Current.existNewMember())
    {
      GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.newMember, true);
      flag3 = true;
    }
label_19:
    if (!flag4 && SM.GuildSignal.Current.existPlayershipEventType(GuildEventType.apply_applicant))
      flag1 = true;
    if (((IEnumerable<GuildEventGift>) SM.GuildSignal.Current.gift_events).Where<GuildEventGift>((Func<GuildEventGift, bool>) (x => x.event_type == GuildEventType.incoming_gift)).FirstOrDefault<GuildEventGift>() != null)
    {
      GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.newGift, true);
      flag3 = true;
    }
    if (!flag4 && SM.GuildSignal.Current.existPayloadEvent(GuildEventType.level_up))
    {
      GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.guildLevelup, true);
      flag3 = true;
    }
    if (!flag4 && SM.GuildSignal.Current.existBaseEvent(GuildEventType.base_rank_up))
    {
      GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.baseRankUp, true);
      flag3 = true;
    }
    if (num == 0 && SM.GuildSignal.Current.existChatEvent(GuildEventType.post_new_chat))
    {
      GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.postNewChat, true);
      flag3 = true;
    }
    if (GuildUtil.getBadgeState(GuildUtil.GuildBadgeInfoType.newApplicant) || !flag4 && GuildUtil.getBadgeState(GuildUtil.GuildBadgeInfoType.startHuntingEvent) || !flag4 && GuildUtil.getBadgeState(GuildUtil.GuildBadgeInfoType.receiveHuntingReward) || !flag4 && GuildUtil.getBadgeState(GuildUtil.GuildBadgeInfoType.newMember) || GuildUtil.getBadgeState(GuildUtil.GuildBadgeInfoType.newGift) || GuildUtil.getBadgeState(GuildUtil.GuildBadgeInfoType.guildLevelup) || GuildUtil.getBadgeState(GuildUtil.GuildBadgeInfoType.baseRankUp))
      flag1 = true;
    if (GuildUtil.getBadgeState(GuildUtil.GuildBadgeInfoType.postNewChat))
    {
      flag2 = true;
      flag1 = false;
    }
    if (!GuildUtil.getBadgeState(GuildUtil.GuildBadgeInfoType.changeRole) && SM.GuildSignal.Current.existRoleChange())
    {
      GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.changeRole, true);
      flag3 = true;
    }
    if (!GuildUtil.getBadgeState(GuildUtil.GuildBadgeInfoType.newTitle) && SM.GuildSignal.Current.existNewTitle())
    {
      GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.newTitle, true);
      flag3 = true;
    }
    GuildEvent guildEvent = ((IEnumerable<GuildEvent>) SM.GuildSignal.Current.guild_events).Where<GuildEvent>((Func<GuildEvent, bool>) (x => x.event_type == GuildEventType.gvg_entry || x.event_type == GuildEventType.gvg_matched || x.event_type == GuildEventType.gvg_started || x.event_type == GuildEventType.gvg_finished || x.event_type == GuildEventType.gvg_entry_expired || x.event_type == GuildEventType.gvg_entry_cancel || x.event_type == GuildEventType.guild_raid_started || x.event_type == GuildEventType.guild_raid_end)).OrderByDescending<GuildEvent, DateTime?>((Func<GuildEvent, DateTime?>) (x => x.created_at)).FirstOrDefault<GuildEvent>();
    if (guildEvent != null)
    {
      GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.gvg_entry, false);
      GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.gvg_matched, false);
      GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.gvg_started, false);
      GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.guild_raid, false);
      if (guildEvent.event_type == GuildEventType.gvg_entry)
        GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.gvg_entry, true);
      else if (guildEvent.event_type == GuildEventType.gvg_entry_cancel)
        GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.gvg_entry, false);
      else if (guildEvent.event_type == GuildEventType.gvg_matched)
        GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.gvg_matched, true);
      else if (guildEvent.event_type == GuildEventType.gvg_started)
        GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.gvg_started, true);
      else if (guildEvent.event_type == GuildEventType.guild_raid_started)
        GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.guild_raid, true);
      else if (guildEvent.event_type == GuildEventType.guild_raid_end)
        GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.guild_raid, false);
      flag3 = true;
      if (Persist.gvgBattleEnvironment.Exists)
        Persist.gvgBattleEnvironment.Delete();
    }
    if (GuildUtil.getBadgeState(GuildUtil.GuildBadgeInfoType.gvg_entry))
      guildBadgeLabelType = GuildUtil.GuildBadgeLabelType.entry;
    else if (GuildUtil.getBadgeState(GuildUtil.GuildBadgeInfoType.gvg_matched))
      guildBadgeLabelType = GuildUtil.GuildBadgeLabelType.prepare;
    else if (GuildUtil.getBadgeState(GuildUtil.GuildBadgeInfoType.gvg_started))
      guildBadgeLabelType = GuildUtil.GuildBadgeLabelType.battle;
    else if (GuildUtil.getBadgeState(GuildUtil.GuildBadgeInfoType.guild_raid))
      guildBadgeLabelType = GuildUtil.GuildBadgeLabelType.raidBoss;
    if (flag3)
      Persist.guildSetting.Flush();
    return new Tuple<bool, bool, GuildUtil.GuildBadgeLabelType>(flag1, flag2, guildBadgeLabelType);
  }

  public static Sprite LoadKindSprite(FacilityCategory category, CommonElement element)
  {
    string str = string.Empty;
    switch (category)
    {
      case FacilityCategory.wall:
        str = "wall";
        break;
      case FacilityCategory.range:
      case FacilityCategory.all_range:
        str = "scope";
        break;
      case FacilityCategory.trap:
        str = "trap";
        break;
    }
    return str.Equals(string.Empty) ? (Sprite) null : Resources.Load<Sprite>(string.Format("Icons/Materials/FacilityKindIcon/slc_{0}_{1}_34_30", (object) str, (object) element.ToString()));
  }

  public static MasterDataTable.CommonRewardType getCommonRewardType(CommonGuildRewardType type)
  {
    return type != CommonGuildRewardType.emblem ? MasterDataTable.CommonRewardType.coin : MasterDataTable.CommonRewardType.emblem;
  }

  public static bool IsCostOver(PlayerUnit[] playerUnits, int?[] playerUnitIDs)
  {
    if (playerUnits == null || playerUnits.Length == 0 || playerUnitIDs == null || playerUnitIDs.Length == 0)
      return false;
    int cost = 0;
    IEnumerable<PlayerUnit> playerUnits1 = ((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => ((IEnumerable<int?>) playerUnitIDs).Any<int?>((Func<int?, bool>) (y => y.HasValue && y.Value == x.id))));
    if (playerUnits1.Count<PlayerUnit>() <= 0)
      return false;
    playerUnits1.ForEach<PlayerUnit>((Action<PlayerUnit>) (x => cost += x.cost));
    return cost > SMManager.Get<Player>().max_cost;
  }

  public static IEnumerator UpdateGuildDeck()
  {
    IEnumerator e;
    switch (GuildUtil.gvgPopupState)
    {
      case GuildUtil.GvGPopupState.AtkTeam:
        e = GuildUtil.UpdateGuildDeckAttack(PlayerAffiliation.Current.guild_id, Player.Current.id, (Action) (() => { }));
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
      case GuildUtil.GvGPopupState.DefTeam:
        e = GuildUtil.UpdateGuildDeckDefanse(PlayerAffiliation.Current.guild_id, Player.Current.id, (Action) (() => { }));
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
    }
  }

  public static IEnumerator UpdateGuildDeckAll()
  {
    if (GuildUtil.gvgPopupState == GuildUtil.GvGPopupState.AtkTeam || GuildUtil.gvgPopupState == GuildUtil.GvGPopupState.DefTeam)
    {
      IEnumerator e = GuildUtil.UpdateGuildDeckAttack(PlayerAffiliation.Current.guild_id, Player.Current.id, (Action) (() => { }));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = GuildUtil.UpdateGuildDeckDefanse(PlayerAffiliation.Current.guild_id, Player.Current.id, (Action) (() => { }));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public enum GvGPopupState
  {
    None,
    AtkTeam,
    DefTeam,
    Sortie,
    TownTop,
    MapList,
    FacilityList,
    FacilityDetail,
  }

  public enum FooterGuildBadge
  {
    bikkuri,
    label,
    chat,
  }

  public enum GuildBadgeLabelType
  {
    none,
    entry,
    prepare,
    battle,
    raidBoss,
    matching,
  }

  public enum GuildBadgeInfoType
  {
    newApplicant,
    newTitle,
    newGift,
    changeRole,
    startHuntingEvent,
    receiveHuntingReward,
    newMember,
    guildLevelup,
    baseRankUp,
    postNewChat,
    gvg_entry,
    gvg_matched,
    gvg_started,
    guild_raid,
  }

  public enum MenuType
  {
    menu2811,
    menu2812,
  }
}
