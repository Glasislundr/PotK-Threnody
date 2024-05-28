// Decompiled with JetBrains decompiler
// Type: GuildDefTeamEditPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnitRegulation;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/Guild/Popup/DefTeamEdit")]
public class GuildDefTeamEditPopup : BackButtonMenuBase
{
  private const float LINK_WIDTH = 92f;
  private const float LINK_DEFWIDTH = 114f;
  private const float scale = 0.807017565f;
  private const int FRIEND_NUM = 5;
  private bool isEnemy;
  private bool myself;
  private bool onGuildBattle;
  [SerializeField]
  private GameObject dyn_leader_unit;
  [SerializeField]
  private UILabel lblPlayerName;
  [SerializeField]
  private UILabel lblTotalPower;
  [SerializeField]
  private UI2DSprite playerTitle;
  [SerializeField]
  private GameObject slc_icon_guildmaseter;
  [SerializeField]
  private GameObject slc_icon_submaseter;
  [SerializeField]
  private UIButton btnTeamEdit;
  [SerializeField]
  private GameObject dir_unavailable_teamformation;
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
  private GvgReinforcement friend;
  private Guild0282Menu guild0282Menu;
  private GuildDefTeamPopup parent;
  private GameObject commonOkPopup;
  private GameObject skillDetailPrefab;
  private PlayerUnitLeader_skills leaderSkill;
  private PlayerUnitLeader_skills friendSkill;

  private int totalCombat
  {
    get
    {
      if (this.deckInfo == null)
        return 0;
      int combat = 0;
      ((IEnumerable<PlayerUnit>) this.deckInfo.player_units).ForEach<PlayerUnit>((Action<PlayerUnit>) (x =>
      {
        if (!(x != (PlayerUnit) null))
          return;
        combat += Judgement.NonBattleParameter.FromPlayerUnit(x).Combat;
      }));
      return combat;
    }
  }

  private IEnumerator ResourceLoad()
  {
    Future<GameObject> fObj;
    if (Object.op_Equality((Object) this.commonOkPopup, (Object) null))
    {
      fObj = Res.Prefabs.popup.popup_028_guild_common_ok__anim_popup01.Load<GameObject>();
      IEnumerator e = fObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.commonOkPopup = fObj.Result;
      fObj = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.skillDetailPrefab, (Object) null))
    {
      fObj = PopupSkillDetails.createPrefabLoader(false);
      yield return (object) fObj.Wait();
      this.skillDetailPrefab = fObj.Result;
      fObj = (Future<GameObject>) null;
    }
  }

  private IEnumerator SetPlayerInfo(GuildMembership info)
  {
    this.lblPlayerName.SetTextLocalize(info.player.player_name);
    this.slc_icon_guildmaseter.SetActive(info.role == GuildRole.master);
    this.slc_icon_submaseter.SetActive(info.role == GuildRole.sub_master);
    IEnumerator e = this.SetUnitIcon(info);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator SetUnitIcon(GuildMembership info)
  {
    IEnumerator e;
    if (Object.op_Equality((Object) this.unitIconPrefab, (Object) null))
    {
      Future<GameObject> unitIconPrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      e = unitIconPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.unitIconPrefab = unitIconPrefabF.Result;
      unitIconPrefabF = (Future<GameObject>) null;
    }
    if (info != null)
    {
      PlayerUnit byUnitunit = PlayerUnit.create_by_unitunit(info.player.leader_unit_unit);
      byUnitunit.level = info.player.leader_unit_level;
      byUnitunit.job_id = info.player.leader_unit_job_id;
      if (Object.op_Equality((Object) this.unitIcon, (Object) null))
        this.unitIcon = this.unitIconPrefab.CloneAndGetComponent<UnitIcon>(this.dyn_leader_unit);
      e = this.unitIcon.setSimpleUnit(byUnitunit);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.unitIcon.BottomBaseObject = false;
    }
    Future<Sprite> image = EmblemUtility.LoadEmblemSprite(info.player.player_emblem_id);
    e = image.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.playerTitle.sprite2D = image.Result;
    this.lblTotalPower.SetTextLocalize(this.totalCombat);
  }

  private IEnumerator SetDeck(string player_id)
  {
    if (this.deckInfo != null)
    {
      IEnumerator e;
      if (player_id != null)
      {
        bool isSuccess = false;
        e = GuildUtil.UpdateGuildDeckDefanse(this.isEnemy ? this.guild0282Menu.EnGuild.guild_id : this.guild0282Menu.MyGuild.guild_id, player_id, (Action) (() => isSuccess = true));
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (!isSuccess)
          yield break;
      }
      Checker checkRules = PlayerAffiliation.Current.gvgCheckRules ?? (Checker) (_ => true);
      this.deckInfo = GuildUtil.gvgDeckDefense;
      bool isFriend = !this.myself;
      if (!isFriend && this.onGuildBattle)
        isFriend = true;
      this.leaderSkill = (PlayerUnitLeader_skills) null;
      this.friendSkill = (PlayerUnitLeader_skills) null;
      foreach (GameObject linkUnabaibleIcon in this.linkUnabaibleIcons)
        linkUnabaibleIcon.SetActive(true);
      for (int i = 0; i < this.deckInfo.player_units.Length; ++i)
      {
        e = this.LoadUnitPrefab(i, this.deckInfo.player_units[i], isFriend);
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
      if (GuildUtil.gvgFriendDefense != null && GuildUtil.gvgFriendDefense.player_unit != (PlayerUnit) null)
      {
        this.slc_NotFriend_Skill.SetActive(false);
        PlayerUnit gUnit = GuildUtil.gvgFriendDefense.player_unit;
        this.friendSkill = gUnit.leader_skill;
        e = this.LoadUnitPrefab(5, gUnit, true);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.disabledSkills[5].SetActive(!checkRules(gUnit));
        gUnit = (PlayerUnit) null;
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
      this.lblTotalPower.SetTextLocalize(this.totalCombat);
    }
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
        if (this.myself && index == Consts.GetInstance().DECK_POSITION_FRIEND - 1)
        {
          if (!this.onGuildBattle)
            this.StartCoroutine(this.parent.ShowGuestSelect());
          else
            this.ShowOkPopup(Consts.GetInstance().POPUP_GUILD_BATTLE_DEF_TEAM_TITLE, Consts.GetInstance().POPUP_GUILD_BATTLE_DEF_TEAM_SELECT_FRIEND_IN_GVG);
        }
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
      unitScript.onClick = (Action<UnitIconBase>) (x =>
      {
        if (!this.myself || index != Consts.GetInstance().DECK_POSITION_FRIEND - 1)
          return;
        if (!this.onGuildBattle)
          this.StartCoroutine(this.parent.ShowGuestSelect());
        else
          this.ShowOkPopup(Consts.GetInstance().POPUP_GUILD_BATTLE_DEF_TEAM_TITLE, Consts.GetInstance().POPUP_GUILD_BATTLE_DEF_TEAM_SELECT_FRIEND_IN_GVG);
      });
    }
    unitScript.Favorite = false;
    unitScript.Gray = false;
  }

  private IEnumerator ChangeDetailScene(PlayerUnit unit, bool isFriend, int index)
  {
    GuildDefTeamEditPopup defTeamEditPopup = this;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    yield return (object) null;
    if (!isFriend)
      Unit0042Scene.changeSceneMyGvgDefDeck(true, unit);
    else if (index == Consts.GetInstance().DECK_POSITION_FRIEND - 1)
      Unit0042Scene.changeSceneGvgUnit(true, unit, new PlayerUnit[1]
      {
        unit
      });
    else
      Unit0042Scene.changeSceneGvgUnit(true, unit, GuildUtil.gvgDeckDefense.player_units);
    defTeamEditPopup.DestroyObject();
    ((Component) defTeamEditPopup).gameObject.SetActive(false);
  }

  private void DestroyObject()
  {
    foreach (GameObject linkCharacter in this.linkCharacters)
    {
      UnitIcon componentInChildren = linkCharacter.GetComponentInChildren<UnitIcon>();
      if (Object.op_Inequality((Object) componentInChildren, (Object) null))
        Object.Destroy((Object) ((Component) componentInChildren).gameObject);
    }
  }

  private IEnumerator ShowDeckEditScene()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    GuildDefTeamEditPopup defTeamEditPopup = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      ((Component) defTeamEditPopup).gameObject.SetActive(false);
      Unit00468Scene.changeScene00468GvgDef(true, defTeamEditPopup.deckInfo);
      defTeamEditPopup.DestroyObject();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private IEnumerator ShowEquipScene()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    GuildDefTeamEditPopup defTeamEditPopup = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      ((Component) defTeamEditPopup).gameObject.SetActive(false);
      Unit00468Scene.changeScene00412Gvg(true);
      defTeamEditPopup.DestroyObject();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private void onClosedSkillZoom()
  {
    this.IsPush = false;
    this.guild0282Menu.isClosePopupByBackBtn = true;
  }

  public void ShowOkPopup(string title, string message, Action ok = null)
  {
    Singleton<PopupManager>.GetInstance().open(this.commonOkPopup).GetComponent<GuildOkPopup>().Initialize(title, message, ok: ok);
  }

  public IEnumerator InitializeAsync(
    Guild0282Menu menu,
    GuildDefTeamPopup parent,
    bool isEnemy,
    GuildMembership info)
  {
    this.isEnemy = isEnemy;
    this.parent = parent;
    this.guild0282Menu = menu;
    this.friend = GuildUtil.gvgFriendDefense;
    this.deckInfo = GuildUtil.gvgDeckDefense;
    IEnumerator e = this.ResourceLoad();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.myself = Player.Current.id.Equals(info.player.player_id);
    this.onGuildBattle = PlayerAffiliation.Current.guild.gvg_status == GvgStatus.fighting;
    e = this.SetPlayerInfo(info);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.SetDeck((string) null);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((Component) this.btnTeamEdit).gameObject.SetActive(this.myself);
    ((UIButtonColor) this.btnTeamEdit).isEnabled = !this.onGuildBattle;
    this.dir_unavailable_teamformation.SetActive(this.myself && this.onGuildBattle);
    this.guild0282Menu.SetGvgPopup(GuildUtil.GvGPopupState.DefTeam, (Action) (() =>
    {
      if (parent.Mode == GuildDefTeamPopup.MODE.Edit)
      {
        ((Component) this).gameObject.SetActive(true);
        this.IsPush = false;
      }
      this.DestroyObject();
      if (this.deckInfo != null && GuildUtil.IsCostOver(SMManager.Get<PlayerUnit[]>(), this.deckInfo.player_unit_ids))
        this.StartCoroutine(this.SetDeck(info.player.player_id));
      else
        this.StartCoroutine(this.SetDeck((string) null));
    }));
  }

  public IEnumerator SetFriendUnit(GvgCandidate candidate)
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    string errCode = string.Empty;
    IEnumerator e1;
    if (candidate != null)
    {
      Future<WebAPI.Response.GvgDeckDefenseReinforcement> ft = WebAPI.GvgDeckDefenseReinforcement(candidate.player_id, (Action<WebAPI.Response.UserError>) (e =>
      {
        errCode = e.Code;
        if (e.Code.Equals("GVG007") || e.Code.Equals("GVG002"))
          return;
        WebAPI.DefaultUserErrorCallback(e);
      }));
      e1 = ft.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (errCode.Equals("GLD014"))
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        yield return (object) null;
        Singleton<CommonRoot>.GetInstance().isLoading = true;
        MypageScene.ChangeSceneOnError();
        yield break;
      }
      else
      {
        if (errCode.Equals("GVG007"))
          this.ShowOkPopup(Consts.GetInstance().POPUP_GUILD_BATTLE_DEF_TEAM_TITLE, Consts.GetInstance().POPUP_GUILD_BATTLE_DEF_TEAM_CANNOT_SELECT_FRIEND);
        else if (errCode.Equals("GVG002"))
          this.ShowOkPopup(Consts.GetInstance().POPUP_GUILD_BATTLE_DEF_TEAM_TITLE, Consts.GetInstance().POPUP_GUILD_BATTLE_DEF_TEAM_SELECT_FRIEND_IN_GVG);
        if (ft.Result != null)
        {
          GuildUtil.SetGvgFriendDefense(ft.Result.reinforcement);
          this.DestroyObject();
          e1 = this.SetDeck((string) null);
          while (e1.MoveNext())
            yield return e1.Current;
          e1 = (IEnumerator) null;
        }
        ft = (Future<WebAPI.Response.GvgDeckDefenseReinforcement>) null;
      }
    }
    else
    {
      errCode = string.Empty;
      Future<WebAPI.Response.GvgDeckDefenseRemoveReinforcement> ft = WebAPI.GvgDeckDefenseRemoveReinforcement((Action<WebAPI.Response.UserError>) (e =>
      {
        errCode = e.Code;
        if (e.Code.Equals("GVG002"))
          return;
        WebAPI.DefaultUserErrorCallback(e);
      }));
      e1 = ft.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (errCode.Equals("GLD014"))
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        yield return (object) null;
        Singleton<CommonRoot>.GetInstance().isLoading = true;
        MypageScene.ChangeSceneOnError();
        yield break;
      }
      else
      {
        if (errCode.Equals("GVG002"))
          this.ShowOkPopup(Consts.GetInstance().POPUP_GUILD_BATTLE_DEF_TEAM_TITLE, Consts.GetInstance().POPUP_GUILD_BATTLE_DEF_TEAM_SELECT_FRIEND_IN_GVG);
        if (ft.Result != null)
        {
          GuildUtil.gvgFriendDefense = (GvgReinforcement) null;
          this.DestroyObject();
          e1 = this.SetDeck((string) null);
          while (e1.MoveNext())
            yield return e1.Current;
          e1 = (IEnumerator) null;
        }
        ft = (Future<WebAPI.Response.GvgDeckDefenseRemoveReinforcement>) null;
      }
    }
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    this.parent.ShowTeamEdit();
  }

  public void onEquipButton()
  {
    if (this.IsPushAndSet())
      return;
    if (PlayerAffiliation.Current.guild.gvg_status == GvgStatus.fighting)
      this.ShowOkPopup(Consts.GetInstance().POPUP_GUILD_BATTLE_DEF_TEAM_TITLE, Consts.GetInstance().POPUP_GUILD_BATTLE_EDIT_GEAR_UNAVAILABLE_IN_GVG, (Action) (() => this.IsPush = false));
    else
      this.StartCoroutine(this.ShowEquipScene());
  }

  public void onTeamEditButton()
  {
    if (this.IsPushAndSet())
      return;
    if (PlayerAffiliation.Current.guild.gvg_status == GvgStatus.fighting)
      this.ShowOkPopup(Consts.GetInstance().POPUP_GUILD_BATTLE_DEF_TEAM_TITLE, Consts.GetInstance().POPUP_GUILD_BATTLE_DEF_TEAM_ERROR, (Action) (() => this.IsPush = false));
    else
      this.StartCoroutine(this.ShowDeckEditScene());
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet() || !((Component) this).gameObject.activeSelf || this.parent.Mode != GuildDefTeamPopup.MODE.Edit || Object.op_Equality((Object) this.guild0282Menu, (Object) null))
      return;
    this.guild0282Menu.closePopup();
  }

  public void onClickedLeaderSkillZoom()
  {
    if (this.leaderSkill == null || Object.op_Equality((Object) this.skillDetailPrefab, (Object) null) || this.IsPushAndSet())
      return;
    this.guild0282Menu.isClosePopupByBackBtn = false;
    PopupSkillDetails.show(this.skillDetailPrefab, new PopupSkillDetails.Param(this.leaderSkill), this.isEnemy, new Action(this.onClosedSkillZoom));
  }

  public void onClickedFriendSkillZoom()
  {
    if (this.friendSkill == null || Object.op_Equality((Object) this.skillDetailPrefab, (Object) null) || this.IsPushAndSet())
      return;
    this.guild0282Menu.isClosePopupByBackBtn = false;
    PopupSkillDetails.show(this.skillDetailPrefab, new PopupSkillDetails.Param(this.friendSkill), this.isEnemy, new Action(this.onClosedSkillZoom));
  }
}
