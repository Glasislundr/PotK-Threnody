// Decompiled with JetBrains decompiler
// Type: GuildBattleSortiePopup
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
using UnitRegulation;
using UnityEngine;

#nullable disable
public class GuildBattleSortiePopup : BackButtonMenuBase
{
  private const float LINK_WIDTH = 92f;
  private const float LINK_DEFWIDTH = 114f;
  private const float scale = 0.807017565f;
  private const int FRIEND_NUM = 5;
  [SerializeField]
  private UILabel lblLeaderSkillName;
  [SerializeField]
  private UILabel lblLeaderSkillDesc;
  [SerializeField]
  private GameObject objLeaderSkillZoom;
  [SerializeField]
  private GameObject slc_NotFriend_Skill;
  [SerializeField]
  private UILabel lblFriendSkillName;
  [SerializeField]
  private UILabel lblFriendSkillDesc;
  [SerializeField]
  private GameObject objFriendSkillZoom;
  [SerializeField]
  private UILabel lblNoFriend;
  [SerializeField]
  protected GameObject[] linkCharacters;
  [SerializeField]
  protected GameObject[] linkUnabaibleIcons;
  [SerializeField]
  private GameObject[] disabledSkills;
  private UnitIcon unitIcon;
  private GameObject unitIconPrefab;
  private GvgDeck deckInfo;
  private GuildBattlePreparationPopup parent;
  private GameObject commonOkPopup;
  private string targetPlayerID = string.Empty;
  private Guild0282Menu guildMapMenu;
  private GameObject skillDetailPrefab;
  private PlayerUnitLeader_skills leaderSkill;
  private PlayerUnitLeader_skills friendSkill;
  private bool isCompletedOverkillersDeck = true;
  private int testSlotNo = -1;

  private IEnumerator SetDeck()
  {
    this.leaderSkill = (PlayerUnitLeader_skills) null;
    this.friendSkill = (PlayerUnitLeader_skills) null;
    foreach (GameObject linkUnabaibleIcon in this.linkUnabaibleIcons)
      linkUnabaibleIcon.SetActive(true);
    Checker checkRules = PlayerAffiliation.Current.gvgCheckRules ?? (Checker) (_ => true);
    OverkillersUtil.checkCompletedDeck(this.deckInfo.player_units, out this.isCompletedOverkillersDeck);
    IEnumerator e;
    for (int i = 0; i < this.deckInfo.player_units.Length; ++i)
    {
      e = this.LoadUnitPrefab(i, this.deckInfo.player_units[i], false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (i == 0)
        this.leaderSkill = this.deckInfo.player_units[i].leader_skill;
      this.linkUnabaibleIcons[i].SetActive(false);
      this.disabledSkills[i].SetActive(!checkRules(this.deckInfo.player_units[i]));
    }
    ((IEnumerable<GameObject>) this.disabledSkills).SetActiveRange(false, this.deckInfo.player_units.Length);
    if (this.leaderSkill != null)
    {
      BattleskillSkill skill = this.leaderSkill.skill;
      this.lblLeaderSkillName.SetTextLocalize(skill.name);
      this.lblLeaderSkillDesc.SetTextLocalize(skill.description);
      this.objLeaderSkillZoom.SetActive(true);
    }
    else
    {
      this.lblLeaderSkillName.SetText("---");
      this.lblLeaderSkillDesc.SetText("-----");
      this.objLeaderSkillZoom.SetActive(false);
    }
    if (GuildUtil.gvgFriendAttack != null)
    {
      this.slc_NotFriend_Skill.SetActive(false);
      PlayerUnit unit = GuildUtil.gvgFriendAttack.player_unit;
      this.friendSkill = unit.leader_skill;
      e = this.LoadUnitPrefab(5, unit, true);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.disabledSkills[5].SetActive(!checkRules(unit));
      unit = (PlayerUnit) null;
    }
    else
    {
      this.slc_NotFriend_Skill.SetActive(true);
      this.lblNoFriend.SetTextLocalize(Consts.GetInstance().QUEST_0028_INDICATOR_NOT_RENTAL);
      e = this.LoadUnitPrefab(5, (PlayerUnit) null, true);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    this.resetFriendSkill();
    if (!this.isCompletedOverkillersDeck)
      this.setErrorOverkillersDeck();
  }

  public IEnumerator LoadUnitPrefab(int index, PlayerUnit unit, bool isFriend)
  {
    GameObject gameObject = this.unitIconPrefab.Clone(this.linkCharacters[index].transform);
    gameObject.transform.localScale = new Vector3(0.807017565f, 0.807017565f);
    UnitIcon unitScript = gameObject.GetComponent<UnitIcon>();
    IEnumerator e = unitScript.setSimpleUnit(unit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unitScript.setLevelText(unit);
    unitScript.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    if (unit != (PlayerUnit) null)
    {
      unitScript.onClick = (Action<UnitIconBase>) (x =>
      {
        if (index == Consts.GetInstance().DECK_POSITION_FRIEND - 1)
          this.parent.ShowGuestSelect();
        else
          this.StartCoroutine(this.ChangeDetailScene(unit, isFriend, index));
      });
      EventDelegate.Set(unitScript.Button.onLongPress, (EventDelegate.Callback) (() => this.StartCoroutine(this.ChangeDetailScene(unit, isFriend, index))));
      unitScript.BreakWeapon = !isFriend && unit.IsBrokenEquippedGear;
      unitScript.SpecialIcon = false;
    }
    else
    {
      unitScript.SetEmpty();
      unitScript.onClick = (Action<UnitIconBase>) (x => this.parent.ShowGuestSelect());
    }
    unitScript.Favorite = false;
    unitScript.Gray = false;
  }

  private IEnumerator ChangeDetailScene(PlayerUnit unit, bool isFriend, int index)
  {
    GuildBattleSortiePopup battleSortiePopup = this;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    yield return (object) null;
    if (!isFriend)
      Unit0042Scene.changeSceneGvgUnit(true, unit, GuildUtil.gvgDeckAttack.player_units);
    else if (index == Consts.GetInstance().DECK_POSITION_FRIEND - 1)
    {
      Future<WebAPI.Response.GvgDeckReinforcementStatus> futureF = WebAPI.GvgDeckReinforcementStatus(true, GuildUtil.gvgFriendAttack.player_id);
      IEnumerator e = futureF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      WebAPI.Response.GvgDeckReinforcementStatus result = futureF.Result;
      if (result == null)
      {
        yield break;
      }
      else
      {
        GvgReinforcement reinforcement = result.reinforcement;
        PlayerUnit playerUnit = reinforcement.player_unit;
        playerUnit.primary_equipped_gear = playerUnit.FindEquippedGear(reinforcement.player_gears);
        playerUnit.primary_equipped_gear2 = playerUnit.FindEquippedGear2(reinforcement.player_gears);
        playerUnit.primary_equipped_gear3 = playerUnit.FindEquippedGear3(reinforcement.player_gears);
        playerUnit.primary_equipped_reisou = playerUnit.FindEquippedReisou(reinforcement.player_gears, reinforcement.player_reisou_gears);
        playerUnit.primary_equipped_reisou2 = playerUnit.FindEquippedReisou2(reinforcement.player_gears, reinforcement.player_reisou_gears);
        playerUnit.primary_equipped_reisou3 = playerUnit.FindEquippedReisou3(reinforcement.player_gears, reinforcement.player_reisou_gears);
        PlayerAwakeSkill playerAwakeSkill = (PlayerAwakeSkill) null;
        foreach (int? equipAwakeSkillId in playerUnit.equip_awake_skill_ids)
        {
          int? awakeSkillID = equipAwakeSkillId;
          playerAwakeSkill = ((IEnumerable<PlayerAwakeSkill>) reinforcement.player_awake_skills).FirstOrDefault<PlayerAwakeSkill>((Func<PlayerAwakeSkill, bool>) (x =>
          {
            int id = x.id;
            int? nullable = awakeSkillID;
            int valueOrDefault = nullable.GetValueOrDefault();
            return id == valueOrDefault & nullable.HasValue;
          }));
        }
        playerUnit.primary_equipped_awake_skill = playerAwakeSkill;
        playerUnit.importOverkillersUnits(reinforcement.over_killers);
        playerUnit.resetOverkillersParameter();
        Unit0042Scene.changeSceneGvgUnit(true, playerUnit, new PlayerUnit[1]
        {
          playerUnit
        });
        futureF = (Future<WebAPI.Response.GvgDeckReinforcementStatus>) null;
      }
    }
    battleSortiePopup.DestroyObject();
    ((Component) battleSortiePopup).gameObject.SetActive(false);
  }

  private void DestroyObject()
  {
    if (this.linkCharacters == null)
      return;
    foreach (GameObject linkCharacter in this.linkCharacters)
    {
      UnitIcon componentInChildren = linkCharacter.GetComponentInChildren<UnitIcon>();
      if (Object.op_Inequality((Object) componentInChildren, (Object) null))
        Object.Destroy((Object) ((Component) componentInChildren).gameObject);
    }
  }

  private IEnumerator ResourceLoad()
  {
    Future<GameObject> unitIconPrefabF;
    if (Object.op_Equality((Object) this.unitIconPrefab, (Object) null))
    {
      unitIconPrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      IEnumerator e = unitIconPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.unitIconPrefab = unitIconPrefabF.Result;
      unitIconPrefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.skillDetailPrefab, (Object) null))
    {
      unitIconPrefabF = PopupSkillDetails.createPrefabLoader(false);
      yield return (object) unitIconPrefabF.Wait();
      this.skillDetailPrefab = unitIconPrefabF.Result;
      unitIconPrefabF = (Future<GameObject>) null;
    }
  }

  private IEnumerator sortie()
  {
    GuildBattleSortiePopup battleSortiePopup = this;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    bool maintenance = false;
    GuildBattleSortiePopup.NotSortieType sortieType = GuildBattleSortiePopup.NotSortieType.None;
    Future<WebAPI.Response.GvgBattleStart> ft = WebAPI.GvgBattleStart(GuildUtil.gvgFriendAttack == null ? string.Empty : GuildUtil.gvgFriendAttack.player_id, battleSortiePopup.targetPlayerID, (Action<WebAPI.Response.UserError>) (e =>
    {
      if (e.Code.Equals("GLD014"))
      {
        WebAPI.DefaultUserErrorCallback(e);
        maintenance = true;
      }
      else if (e.Code.Equals("GVG001"))
        sortieType = GuildBattleSortiePopup.NotSortieType.EndGuildBattle;
      else if (e.Code.Equals("GVG005"))
        sortieType = GuildBattleSortiePopup.NotSortieType.Fighting;
      else if (e.Code.Equals("GLD002"))
        sortieType = GuildBattleSortiePopup.NotSortieType.OtherReason;
      else
        WebAPI.DefaultUserErrorCallback(e);
    }));
    IEnumerator e1 = ft.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 4;
    if (maintenance)
    {
      yield return (object) null;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      MypageScene.ChangeSceneOnError();
    }
    if (sortieType != GuildBattleSortiePopup.NotSortieType.None)
    {
      e1 = battleSortiePopup.showSortieUnAvailablePopup(sortieType);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
    }
    if (ft.Result == null)
    {
      battleSortiePopup.IsPush = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    }
    else
    {
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      yield return (object) null;
      battleSortiePopup.guildMapMenu.closePopup();
      while (GuildUtil.gvgPopupState != GuildUtil.GvGPopupState.None)
        yield return (object) null;
      PlayerGuildTownSlotPosition[] facility_data = ft.Result.target_town_slot.facilities_data;
      PlayerUnit[] facility_units = new PlayerUnit[facility_data.Length];
      Tuple<int, int>[] facility_coordinates = new Tuple<int, int>[facility_data.Length];
      for (int i = 0; i < facility_data.Length; i++)
      {
        PlayerGuildFacility playerGuildFacility = ((IEnumerable<PlayerGuildFacility>) ft.Result.target_facilities).FirstOrDefault<PlayerGuildFacility>((Func<PlayerGuildFacility, bool>) (x => x._master == facility_data[i].master_id));
        if (playerGuildFacility != null)
        {
          facility_units[i] = PlayerUnit.FromFacility(playerGuildFacility.unit, -(i + 1));
          facility_coordinates[i] = new Tuple<int, int>(facility_data[i].x, facility_data[i].y);
        }
      }
      WebAPI.Response.GvgBattleStart result = ft.Result;
      battleSortiePopup.attachOverkillers(result.own_deck_units, result.own_over_killers);
      battleSortiePopup.attachOverkillers(result.own_support_units, result.own_support_over_killers);
      battleSortiePopup.attachOverkillers(result.target_deck_units, result.target_over_killers);
      battleSortiePopup.attachOverkillers(result.target_support_units, result.target_support_over_killers);
      BattleInfo battleInfo = BattleInfo.MakeGvgBattleInfo(result.battle_uuid, PlayerAffiliation.Current.gvg_period_id, result.stage.stage_id, result.own_deck_units, result.target_deck_units, result.own_support_units, result.target_support_units, result.own_deck_gears, result.target_deck_gears, result.own_support_gears, result.target_support_gears, result.own_deck_reisou_gears, result.target_deck_reisou_gears, result.own_support_reisou_gears, result.target_support_reisou_gears, result.bonus_effects, result.target_bonus_effects, result.own_character_intimates, result.target_character_intimates, result.own_deck_awake_skills, result.own_support_awake_skills, result.target_deck_awake_skills, result.target_support_awake_skills, result.battle_start_time, facility_units, facility_coordinates);
      battleInfo.gvgSetting = new GVGSetting();
      battleSortiePopup.setGvgSetting(battleInfo.gvgSetting, result.stage.turns, result.stage.point, result.stage.annihilation_point);
      int? nullable = ((IEnumerable<GuildMembership>) battleSortiePopup.guildMapMenu.EnGuild.memberships).FirstIndexOrNull<GuildMembership>((Func<GuildMembership, bool>) (x => x.player.player_id == this.targetPlayerID));
      if (nullable.HasValue)
      {
        GuildMembership membership = battleSortiePopup.guildMapMenu.EnGuild.memberships[nullable.Value];
        battleInfo.gvgSetting.enemyID = membership.player.player_id;
        battleInfo.gvgSetting.enemyPlayerName = membership.player.player_name;
        battleInfo.gvgSetting.enemyEmblemID = membership.player.player_emblem_id;
        battleInfo.gvgSetting.enemyGuildPosition = membership._role;
        battleInfo.gvgSetting.enemyLevel = membership.player.player_level;
        battleInfo.gvgSetting.enemyContribution = membership.contribution;
        battleInfo.gvgSetting.enemyTownLevel = 1;
        battleInfo.gvgSetting.enemyKeepStar = ft.Result.target_star;
        if (ft.Result.target_call_skill_same_character_ids.Length != 0 && ft.Result.target_call_skill_values.Length != 0)
        {
          battleInfo.enemyCallSkillParam.player_rank = membership.player.player_level;
          battleInfo.enemyCallSkillParam.same_character_id = ft.Result.target_call_skill_same_character_ids[0];
          battleInfo.enemyCallSkillParam.player_rank = ft.Result.target_call_skill_values[0];
        }
      }
      battleInfo.gvgSetting.enemyGuildname = battleSortiePopup.guildMapMenu.EnGuild.guild_name;
      battleInfo.gvgSetting.myGuildName = battleSortiePopup.guildMapMenu.MyGuild.guild_name;
      Singleton<NGBattleManager>.GetInstance().startBattle(battleInfo);
    }
  }

  private IEnumerator testBattle()
  {
    GuildBattleSortiePopup battleSortiePopup = this;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    IEnumerator e1 = ServerTime.WaitSync();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    bool maintenance = false;
    Future<WebAPI.Response.GuildtownTest> ft = WebAPI.GuildtownTest(battleSortiePopup.testSlotNo, GuildUtil.gvgFriendAttack == null ? string.Empty : GuildUtil.gvgFriendAttack.player_id, battleSortiePopup.targetPlayerID, (Action<WebAPI.Response.UserError>) (e =>
    {
      if (e.Code.Equals("GLD014"))
      {
        WebAPI.DefaultUserErrorCallback(e);
        maintenance = true;
      }
      else
        WebAPI.DefaultUserErrorCallback(e);
    }));
    e1 = ft.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 4;
    if (maintenance)
    {
      yield return (object) null;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      MypageScene.ChangeSceneOnError();
    }
    if (ft.Result == null)
    {
      battleSortiePopup.IsPush = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    }
    else
    {
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      yield return (object) null;
      battleSortiePopup.guildMapMenu.closeAllPopup();
      while (GuildUtil.gvgPopupState != GuildUtil.GvGPopupState.None)
        yield return (object) null;
      PlayerGuildTownSlotPosition[] facility_data = ft.Result.target_town_slot.facilities_data;
      PlayerUnit[] facility_units = new PlayerUnit[facility_data.Length];
      Tuple<int, int>[] facility_coordinates = new Tuple<int, int>[facility_data.Length];
      for (int i = 0; i < facility_data.Length; i++)
      {
        PlayerGuildFacility playerGuildFacility = ((IEnumerable<PlayerGuildFacility>) ft.Result.target_facilities).FirstOrDefault<PlayerGuildFacility>((Func<PlayerGuildFacility, bool>) (x => x._master == facility_data[i].master_id));
        if (playerGuildFacility != null)
        {
          facility_units[i] = PlayerUnit.FromFacility(playerGuildFacility.unit, -(i + 1));
          facility_coordinates[i] = new Tuple<int, int>(facility_data[i].x, facility_data[i].y);
        }
      }
      WebAPI.Response.GuildtownTest result = ft.Result;
      battleSortiePopup.attachOverkillers(result.own_deck_units, result.own_over_killers);
      battleSortiePopup.attachOverkillers(result.own_support_units, result.own_support_over_killers);
      battleSortiePopup.attachOverkillers(result.target_deck_units, result.target_over_killers);
      battleSortiePopup.attachOverkillers(result.target_support_units, result.target_support_over_killers);
      BattleInfo battleInfo = BattleInfo.MakeGvgBattleInfo(string.Empty, PlayerAffiliation.Current.gvg_period_id, ft.Result.stage.stage_id, result.own_deck_units, result.target_deck_units, result.own_support_units, result.target_support_units, result.own_deck_gears, result.target_deck_gears, result.own_support_gears, result.target_support_gears, result.own_deck_reisou_gears, result.target_deck_reisou_gears, result.own_support_reisou_gears, result.target_support_reisou_gears, result.bonus_effects, result.target_bonus_effects, result.own_character_intimates, result.target_character_intimates, result.own_deck_awake_skills, result.own_support_awake_skills, result.target_deck_awake_skills, result.target_support_awake_skills, ServerTime.NowAppTime(), facility_units, facility_coordinates);
      battleInfo.gvgSetting = new GVGSetting();
      battleSortiePopup.setGvgSetting(battleInfo.gvgSetting, ft.Result.stage.turns, ft.Result.stage.point, ft.Result.stage.annihilation_point);
      int? nullable = ((IEnumerable<GuildMembership>) PlayerAffiliation.Current.guild.memberships).FirstIndexOrNull<GuildMembership>((Func<GuildMembership, bool>) (x => x.player.player_id == this.targetPlayerID));
      if (nullable.HasValue)
      {
        GuildMembership membership = PlayerAffiliation.Current.guild.memberships[nullable.Value];
        battleInfo.gvgSetting.enemyID = membership.player.player_id;
        battleInfo.gvgSetting.enemyPlayerName = membership.player.player_name;
        battleInfo.gvgSetting.enemyEmblemID = membership.player.player_emblem_id;
        battleInfo.gvgSetting.enemyGuildPosition = membership._role;
        battleInfo.gvgSetting.enemyLevel = membership.player.player_level;
        battleInfo.gvgSetting.enemyContribution = membership.contribution;
        battleInfo.gvgSetting.enemyTownLevel = 1;
        battleInfo.gvgSetting.enemyKeepStar = 0;
        if (ft.Result.target_call_skill_same_character_ids.Length != 0 && ft.Result.target_call_skill_values.Length != 0)
        {
          battleInfo.enemyCallSkillParam.player_rank = membership.player.player_level;
          battleInfo.enemyCallSkillParam.same_character_id = ft.Result.target_call_skill_same_character_ids[0];
          battleInfo.enemyCallSkillParam.intimate_rank = ft.Result.target_call_skill_values[0];
        }
      }
      battleInfo.gvgSetting.enemyGuildname = PlayerAffiliation.Current.guild.guild_name;
      battleInfo.gvgSetting.myGuildName = PlayerAffiliation.Current.guild.guild_name;
      battleInfo.gvgSetting.isTestBattle = true;
      Singleton<NGBattleManager>.GetInstance().startBattle(battleInfo);
    }
  }

  private void attachOverkillers(PlayerUnit[] playerUnits, PlayerUnit[] overkillers)
  {
    if (playerUnits == null)
      return;
    for (int index = 0; index < playerUnits.Length; ++index)
    {
      if (playerUnits[index] != (PlayerUnit) null)
        playerUnits[index].importOverkillersUnits(overkillers);
    }
  }

  private IEnumerator showSortieUnAvailablePopup(GuildBattleSortiePopup.NotSortieType type)
  {
    if (Object.op_Equality((Object) this.commonOkPopup, (Object) null))
    {
      Future<GameObject> popup = Res.Prefabs.popup.popup_028_guild_common_ok__anim_popup01.Load<GameObject>();
      IEnumerator e = popup.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.commonOkPopup = popup.Result;
      popup = (Future<GameObject>) null;
    }
    string message = string.Empty;
    if (type == GuildBattleSortiePopup.NotSortieType.EndGuildBattle)
      message = Consts.GetInstance().POPUP_GUILD_BATTLE_CANNOT_SROTIE_END_BATTLE;
    else if (type == GuildBattleSortiePopup.NotSortieType.Fighting)
      message = Consts.GetInstance().POPUP_GUILD_BATTLE_CANNOT_SORTIE_DESC;
    else if (type == GuildBattleSortiePopup.NotSortieType.OtherReason)
      message = Consts.GetInstance().POPUP_GUILD_BATTLE_CANNOT_SROTIE_OTHER_REASON;
    Singleton<PopupManager>.GetInstance().open(this.commonOkPopup).GetComponent<GuildOkPopup>().Initialize(Consts.GetInstance().POPUP_GUILD_BATTLE_CANNOT_SORTIE_TITLE, message, ok: (Action) (() =>
    {
      if (type != GuildBattleSortiePopup.NotSortieType.EndGuildBattle)
        return;
      this.guildMapMenu.closePopup();
      MypageScene.ChangeScene(MypageRootMenu.Mode.GUILD, true);
    }));
  }

  private void setGvgSetting(GVGSetting setting, int turns, int point, int annihilation_point)
  {
    float? gvgFactor = this.getGvgFactor("POINT_LEADER_FACTOR");
    setting.point_leader_factor = gvgFactor.HasValue ? gvgFactor.Value : 1f;
    gvgFactor = this.getGvgFactor("POINT_NO_LEADER_FACTOR");
    setting.point_no_leader_factor = gvgFactor.HasValue ? gvgFactor.Value : 1f;
    gvgFactor = this.getGvgFactor("POINT_COST_FACTOR");
    setting.point_cost_factor = gvgFactor.HasValue ? gvgFactor.Value : 1f;
    gvgFactor = this.getGvgFactor("POINT_RARITY_FACTOR");
    setting.point_rarity_factor = gvgFactor.HasValue ? gvgFactor.Value : 1f;
    gvgFactor = this.getGvgFactor("POINT_BASE_FACTOR");
    setting.point_base_factor = gvgFactor.HasValue ? gvgFactor.Value : 1f;
    gvgFactor = this.getGvgFactor("RESPAWN_BASE_FACTOR");
    setting.respawn_base_factor = gvgFactor.HasValue ? gvgFactor.Value : 1f;
    gvgFactor = this.getGvgFactor("RESPAWN_RARITY_FACTOR");
    setting.respawn_rarity_factor = gvgFactor.HasValue ? gvgFactor.Value : 1f;
    gvgFactor = this.getGvgFactor("RESPAWN_COST_FACTOR");
    setting.respawn_cost_factor = gvgFactor.HasValue ? gvgFactor.Value : 1f;
    gvgFactor = this.getGvgFactor("TURNS_FACTOR");
    setting.turns_factor = gvgFactor.HasValue ? gvgFactor.Value : 1f;
    setting.turns = turns;
    setting.point = point;
    setting.annihilation_point = annihilation_point;
  }

  private float? getGvgFactor(string key)
  {
    int? nullable = MasterData.GvgSettings.FirstIndexOrNull<KeyValuePair<int, GvgSettings>>((Func<KeyValuePair<int, GvgSettings>, bool>) (x => x.Value.key.Equals(key)));
    return !nullable.HasValue ? new float?() : new float?(MasterData.GvgSettingsList[nullable.Value].value);
  }

  private void onClosedSkillDetail()
  {
    this.IsPush = false;
    this.guildMapMenu.isClosePopupByBackBtn = true;
  }

  private void resetFriendSkill()
  {
    if (this.friendSkill != null)
    {
      BattleskillSkill skill = this.friendSkill.skill;
      this.lblFriendSkillDesc.SetText(skill.description);
      this.lblFriendSkillName.SetText(skill.name);
      this.objFriendSkillZoom.SetActive(true);
    }
    else
    {
      this.lblFriendSkillName.SetText("---");
      this.lblFriendSkillDesc.SetText("-----");
      this.objFriendSkillZoom.SetActive(false);
    }
  }

  private void setErrorOverkillersDeck()
  {
    this.slc_NotFriend_Skill.gameObject.SetActive(true);
    this.lblNoFriend.SetTextLocalize(Consts.GetInstance().QUEST_0028_INDICATOR_LIMITED_OVERKILLERS);
  }

  public IEnumerator InitializeAsync(
    Guild0282Menu menu,
    GuildBattlePreparationPopup parent,
    string targetPlayerID,
    int testSlotNo)
  {
    this.parent = parent;
    this.deckInfo = GuildUtil.gvgDeckAttack;
    this.targetPlayerID = targetPlayerID;
    this.guildMapMenu = menu;
    this.testSlotNo = testSlotNo;
    IEnumerator e = this.ResourceLoad();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.SetDeck();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void SetGvgPopup()
  {
    this.guildMapMenu.SetGvgPopup(GuildUtil.GvGPopupState.Sortie, (Action) (() =>
    {
      ((Component) this).gameObject.SetActive(true);
      this.IsPush = false;
      this.StartCoroutine(this.SetDeck());
    }));
  }

  public IEnumerator SetFriendUnit(GvgCandidate friend)
  {
    if (this.linkCharacters != null)
    {
      UnitIcon componentInChildren = this.linkCharacters[5].GetComponentInChildren<UnitIcon>();
      if (Object.op_Inequality((Object) componentInChildren, (Object) null))
        Object.Destroy((Object) ((Component) componentInChildren).gameObject);
      this.friendSkill = (PlayerUnitLeader_skills) null;
      bool bDisableSkills = false;
      IEnumerator e;
      if (friend == null)
      {
        GuildUtil.gvgFriendAttack = (GvgCandidate) null;
        this.slc_NotFriend_Skill.SetActive(true);
        this.lblNoFriend.SetTextLocalize(Consts.GetInstance().QUEST_0028_INDICATOR_NOT_RENTAL);
        e = this.LoadUnitPrefab(5, (PlayerUnit) null, true);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
      {
        GuildUtil.gvgFriendAttack = friend;
        this.slc_NotFriend_Skill.SetActive(false);
        PlayerUnit gUnit = friend.player_unit;
        if (gUnit.leader_skills != null)
          this.friendSkill = gUnit.leader_skill;
        e = this.LoadUnitPrefab(5, gUnit, true);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        Checker gvgCheckRules = PlayerAffiliation.Current.gvgCheckRules;
        if (gvgCheckRules != null)
          bDisableSkills = !gvgCheckRules(gUnit);
        gUnit = (PlayerUnit) null;
      }
      this.disabledSkills[5].SetActive(bDisableSkills);
      this.resetFriendSkill();
      this.parent.ShowSortie();
    }
  }

  public void onSortieButton()
  {
    if (this.IsPushAndSet())
      return;
    if (!this.isCompletedOverkillersDeck)
    {
      Consts instance = Consts.GetInstance();
      this.StartCoroutine(PopupCommon.Show(instance.QUEST_0028_ERROR_TITLE_OVERKILLERS, instance.QUEST_0028_ERROR_MESSAGE_OVERKILLERS, (Action) (() => this.IsPush = false)));
    }
    else if (this.testSlotNo >= 0)
      this.StartCoroutine(this.testBattle());
    else
      this.StartCoroutine(this.sortie());
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet() || !((Component) this).gameObject.activeSelf || this.parent.Mode != GuildBattlePreparationPopup.MODE.Sortie)
      return;
    this.parent.ShowGuestSelect();
  }

  public void onClickedLeaderSkillZoom()
  {
    if (this.leaderSkill == null || Object.op_Equality((Object) this.skillDetailPrefab, (Object) null) || this.IsPushAndSet())
      return;
    this.guildMapMenu.isClosePopupByBackBtn = false;
    PopupSkillDetails.show(this.skillDetailPrefab, new PopupSkillDetails.Param(this.leaderSkill), onClosed: new Action(this.onClosedSkillDetail));
  }

  public void onClickedFriendSkillZoom()
  {
    if (this.friendSkill == null || Object.op_Equality((Object) this.skillDetailPrefab, (Object) null) || this.IsPushAndSet())
      return;
    this.guildMapMenu.isClosePopupByBackBtn = false;
    PopupSkillDetails.show(this.skillDetailPrefab, new PopupSkillDetails.Param(this.friendSkill), onClosed: new Action(this.onClosedSkillDetail));
  }

  public enum NotSortieType
  {
    None = -1, // 0xFFFFFFFF
    EndGuildBattle = 1,
    Fighting = 2,
    OtherReason = 3,
  }
}
