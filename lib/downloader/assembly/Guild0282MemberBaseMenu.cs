// Decompiled with JetBrains decompiler
// Type: Guild0282MemberBaseMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Guild0282MemberBaseMenu : GuildMapObject
{
  [SerializeField]
  private bool isEnemy;
  private GuildMembership MemberData;
  private GuildMemberObject memberPopup;
  public GameObject unitIconPos;
  private UnitIcon unitIcon;
  private GameObject unitIconPrefab;
  private Guild0282Menu guild0282Menu;
  private GameObject guildBattlePreparationPopup;
  private GameObject guildTownTopPopup;
  private GameObject guildTownTopEnemyPopup;
  [SerializeField]
  private SpreadColorButton ibtn_attack_team;
  [SerializeField]
  private GameObject txt_comeout_before_battled_atk_team;
  [SerializeField]
  private SpreadColorButton ibtn_defense_team;
  [SerializeField]
  private GameObject txt_comeout_before_battled_def_team;
  [SerializeField]
  private SpreadColorButton ibtn_battle;
  [SerializeField]
  private SpreadColorButton ibtn_Town;
  [SerializeField]
  private GameObject txt_comeout_before_battle_map;

  public GuildMembership memberData
  {
    get => this.MemberData;
    set => this.MemberData = value;
  }

  public IEnumerator ResourceLoad()
  {
    Future<GameObject> PrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = PrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.unitIconPrefab = PrefabF.Result;
    if (Object.op_Equality((Object) this.guildBattlePreparationPopup, (Object) null))
    {
      PrefabF = Res.Prefabs.guild028_2.dir_guild_battle_attack_target.Load<GameObject>();
      e = PrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildBattlePreparationPopup = PrefabF.Result;
    }
    if (Object.op_Equality((Object) this.guildTownTopPopup, (Object) null))
    {
      PrefabF = new ResourceObject("Prefabs/guild_town/dir_guild_town_top").Load<GameObject>();
      e = PrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildTownTopPopup = PrefabF.Result;
    }
    if (Object.op_Equality((Object) this.guildTownTopEnemyPopup, (Object) null))
    {
      PrefabF = new ResourceObject("Prefabs/guild_town/dir_guild_town_top_enemy").Load<GameObject>();
      e = PrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildTownTopEnemyPopup = PrefabF.Result;
    }
  }

  public void Initialize(
    GuildMembership data,
    GuildMemberObject popup,
    Guild0282Menu menu,
    bool isEnemy,
    GvgStatus gvgStatus)
  {
    this.isEnemy = isEnemy;
    this.MemberData = data;
    this.memberPopup = popup;
    this.guild0282Menu = menu;
    if (Object.op_Equality((Object) this.unitIcon, (Object) null))
      this.unitIcon = this.unitIconPrefab.Clone(this.unitIconPos.transform).GetComponent<UnitIcon>();
    PlayerUnit byUnitunit = PlayerUnit.create_by_unitunit(data.player.leader_unit_unit);
    byUnitunit.level = data.player.leader_unit_level;
    byUnitunit.job_id = data.player.leader_unit_job_id;
    this.StartCoroutine(this.unitIcon.SetUnit(byUnitunit, data.player.leader_unit_unit.GetElement(), false));
    this.unitIcon.setLevelText(data.player.leader_unit_level.ToString());
    this.unitIcon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    this.unitIcon.Button.onClick.Clear();
    this.unitIcon.Button.onLongPress.Clear();
    if (this.isEnemy)
    {
      int? nullable = ((IEnumerable<GuildMembership>) PlayerAffiliation.Current.guild.memberships).FirstIndexOrNull<GuildMembership>((Func<GuildMembership, bool>) (x => x.player.player_id == Player.Current.id));
      if (nullable.HasValue)
        ((UIButtonColor) this.ibtn_battle).isEnabled = this.memberData.is_defense_membership && GuildUtil.isBattle(gvgStatus) && !this.MemberData.in_battle && PlayerAffiliation.Current.guild.memberships[nullable.Value].action_point != 0 && !menu.IsOpposedPlayer(this.memberData.player.player_id);
      ((UIButtonColor) this.ibtn_attack_team).isEnabled = GuildUtil.isBattle(gvgStatus) && this.memberData.scouted;
      this.txt_comeout_before_battled_atk_team.SetActive(!((UIButtonColor) this.ibtn_attack_team).isEnabled);
      ((UIButtonColor) this.ibtn_defense_team).isEnabled = GuildUtil.isBattle(gvgStatus) && this.memberData.scouted;
      this.txt_comeout_before_battled_def_team.SetActive(!((UIButtonColor) this.ibtn_defense_team).isEnabled);
      ((UIButtonColor) this.ibtn_Town).isEnabled = PlayerAffiliation.Current.guild_map_enabled && GuildUtil.isBattle(gvgStatus) && this.memberData.scouted;
      this.txt_comeout_before_battle_map.SetActive(PlayerAffiliation.Current.guild_map_enabled && !((UIButtonColor) this.ibtn_Town).isEnabled);
    }
    else if (PlayerAffiliation.Current.guild_map_enabled)
    {
      ((UIButtonColor) this.ibtn_attack_team).isEnabled = true;
      ((UIButtonColor) this.ibtn_defense_team).isEnabled = true;
      ((UIButtonColor) this.ibtn_Town).isEnabled = true;
    }
    else
    {
      ((UIButtonColor) this.ibtn_attack_team).isEnabled = GuildUtil.isBattleOrPreparing(gvgStatus);
      ((UIButtonColor) this.ibtn_defense_team).isEnabled = GuildUtil.isBattleOrPreparing(gvgStatus);
      ((UIButtonColor) this.ibtn_Town).isEnabled = false;
    }
  }

  public override void onBackButton()
  {
  }

  public void onButtonMemberLog()
  {
    Singleton<CommonRoot>.GetInstance().guildChatManager.OpenMemberLogDialog(this.MemberData.player.player_id);
  }

  public void onMemberAbout()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.ShowMemberInfo());
  }

  private IEnumerator ShowMemberInfo()
  {
    Guild0282MemberBaseMenu guild0282MemberBaseMenu = this;
    guild0282MemberBaseMenu.guild0282Menu.isClosePopupByBackBtn = false;
    GameObject popup = guild0282MemberBaseMenu.memberPopup.GuildMemberInfoPopup.Clone();
    GuildMemberInfoPopup component = popup.GetComponent<GuildMemberInfoPopup>();
    popup.SetActive(false);
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = component.Initialize(guild0282MemberBaseMenu.isEnemy, guild0282MemberBaseMenu.guild0282Menu, guild0282MemberBaseMenu.MemberData, guild0282MemberBaseMenu.memberPopup, new Action(guild0282MemberBaseMenu.\u003CShowMemberInfo\u003Eb__25_0), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
    guild0282MemberBaseMenu.IsPush = false;
  }

  public void onButtonATKTeam() => this.StartCoroutine(this.showAtkTeamPopup());

  public void onButtonDEFTeam() => this.StartCoroutine(this.showDefTeamPopup());

  public void onButtonBattle()
  {
    if (this.MemberData.own_star > 0)
      this.StartCoroutine(this.ShowBattlePreparationPopup(-1));
    else
      ModalWindow.ShowYesNo(Consts.GetInstance().GUILD_BATTLE_NON_STAR_TITLE, Consts.GetInstance().GUILD_BATTLE_NON_STAR_MESSAGE, (Action) (() => this.StartCoroutine(this.ShowBattlePreparationPopup(-1))), (Action) (() => { }));
  }

  public void onButtonTown()
  {
    if (!this.isEnemy)
      this.StartCoroutine(this.ShowGuildTownPopup());
    else
      this.StartCoroutine(this.ShowGuildTownEnemyPopup());
  }

  public void FriendDetailScene(GuildMembership data)
  {
    GuildPlayerInfo player = PlayerAffiliation.Current.Player;
    if (player.player_id.Equals(Player.Current.id))
      Unit0042Scene.changeScene(true, PlayerUnit.create_by_unitunit(player.leader_unit_unit), SMManager.Get<PlayerUnit[]>());
    else
      Unit0042Scene.changeSceneFriendUnit(true, data.player.player_id, 0);
  }

  private IEnumerator showAtkTeamPopup()
  {
    GameObject popup = this.guild0282Menu.guildAtkTeamPopup.Clone();
    popup.SetActive(false);
    GuildAtkTeamPopup component = popup.GetComponent<GuildAtkTeamPopup>();
    bool success = false;
    IEnumerator e = component.InitializeAsync(this.isEnemy, this.guild0282Menu, this.MemberData, (Action) (() => success = true));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (success)
    {
      popup.SetActive(true);
      this.guild0282Menu.openPopup(popup, GuildUtil.GvGPopupState.AtkTeam, true);
    }
    else
      Object.Destroy((Object) popup);
  }

  private IEnumerator showDefTeamPopup()
  {
    GameObject popup = this.guild0282Menu.guildDefTeamPopup.Clone();
    popup.SetActive(false);
    GuildDefTeamPopup component = popup.GetComponent<GuildDefTeamPopup>();
    bool success = false;
    IEnumerator e = component.InitializeAsync(this.isEnemy, this.guild0282Menu, this.MemberData, (Action) (() => success = true));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (success)
    {
      popup.SetActive(true);
      this.guild0282Menu.openPopup(popup, GuildUtil.GvGPopupState.DefTeam, true);
    }
    else
      Object.Destroy((Object) popup);
  }

  public IEnumerator ShowBattlePreparationPopup(int testSlotNo, bool isPopupStack = false)
  {
    GameObject popup = this.guildBattlePreparationPopup.Clone();
    popup.SetActive(false);
    GuildBattlePreparationPopup script = popup.GetComponent<GuildBattlePreparationPopup>();
    bool success = false;
    IEnumerator e = script.InitializeAsync(this.guild0282Menu, this.MemberData.player.player_id, testSlotNo, (Action) (() => success = true));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (success)
    {
      popup.SetActive(true);
      this.guild0282Menu.openPopup(popup, GuildUtil.GvGPopupState.Sortie, true, isPopupStack);
      yield return (object) null;
      script.ShowGuestSelect();
    }
    else
      Object.Destroy((Object) popup);
  }

  private IEnumerator ShowGuildTownPopup()
  {
    Guild0282MemberBaseMenu memberMenu = this;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    memberMenu.guild0282Menu.SetActiveMenuBlack(true);
    GameObject popup = memberMenu.guildTownTopPopup.Clone();
    popup.SetActive(false);
    GuildTownTopPopup script = popup.GetComponent<GuildTownTopPopup>();
    int defaultSlotNo = 0;
    IEnumerator e1;
    if (PlayerAffiliation.Current.Player.player_id.Equals(memberMenu.memberData.player.player_id))
    {
      e1 = script.InitializeAsync(memberMenu.guild0282Menu, memberMenu);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      defaultSlotNo = PlayerAffiliation.Current.default_town_slot_number;
    }
    else
    {
      Future<WebAPI.Response.GuildtownShow> guildTownShow = WebAPI.GuildtownShow(memberMenu.memberData.player.player_id, (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
      e1 = guildTownShow.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (guildTownShow.Result == null)
      {
        yield break;
      }
      else
      {
        e1 = script.InitializeAsync(memberMenu.guild0282Menu, memberMenu, guildTownShow.Result);
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        defaultSlotNo = guildTownShow.Result.default_town_slot_number;
        guildTownShow = (Future<WebAPI.Response.GuildtownShow>) null;
      }
    }
    popup.SetActive(true);
    yield return (object) 0;
    script.focusCurrentMap(defaultSlotNo, (Action) (() => this.guild0282Menu.openPopup(popup, GuildUtil.GvGPopupState.TownTop, true)));
  }

  private IEnumerator ShowGuildTownEnemyPopup()
  {
    Guild0282MemberBaseMenu memberMenu = this;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    GameObject popup = memberMenu.guildTownTopEnemyPopup.Clone();
    popup.SetActive(false);
    GuildTownTopEnemyPopup script = popup.GetComponent<GuildTownTopEnemyPopup>();
    Future<WebAPI.Response.GuildtownShow> guildTownShow = WebAPI.GuildtownShow(memberMenu.memberData.player.player_id, (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
    IEnumerator e1 = guildTownShow.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (guildTownShow.Result != null)
    {
      e1 = script.InitializeAsync(memberMenu.guild0282Menu, memberMenu, guildTownShow.Result);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      popup.SetActive(true);
      yield return (object) 0;
      memberMenu.guild0282Menu.openPopup(popup, GuildUtil.GvGPopupState.TownTop, true);
    }
  }
}
