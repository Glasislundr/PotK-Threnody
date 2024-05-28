// Decompiled with JetBrains decompiler
// Type: GuildRaidLogTeamPopup
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
public class GuildRaidLogTeamPopup : BackButtonMenuBase
{
  private const float LINK_WIDTH = 92f;
  private const float LINK_DEFWIDTH = 114f;
  private const float scale = 0.807017565f;
  private const int FRIEND_NUM = 5;
  [SerializeField]
  private GameObject dir_playerInfo;
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
  private UnitIcon unitIcon;
  private GameObject unitIconPrefab;
  private GvgDeck deckInfo;
  private GvgReinforcement friend;
  private WebAPI.Response.GuildraidHistoryDeck responseHistoryDeck;
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

  public IEnumerator InitializeAsync(GuildChatMessageData data)
  {
    GuildRaidLogTeamPopup raidLogTeamPopup = this;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    ((Component) raidLogTeamPopup).gameObject.SetActive(false);
    string[] strArray = data.messageContent.Split('@');
    string battle_uuid = "";
    if (strArray.Length >= 2)
      battle_uuid = strArray[strArray.Length - 2];
    bool maintenance = false;
    Future<WebAPI.Response.GuildraidHistoryDeck> ft = WebAPI.GuildraidHistoryDeck(battle_uuid, (Action<WebAPI.Response.UserError>) (e =>
    {
      if (e.Code.Equals("GLD014"))
        maintenance = true;
      WebAPI.DefaultUserErrorCallback(e);
    }));
    IEnumerator e1 = ft.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (maintenance)
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      yield return (object) null;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      Singleton<PopupManager>.GetInstance().onDismiss();
      MypageScene.ChangeSceneOnError();
    }
    else if (ft.Result == null)
    {
      ((Component) raidLogTeamPopup).gameObject.SetActive(true);
    }
    else
    {
      raidLogTeamPopup.responseHistoryDeck = ft.Result;
      ft = (Future<WebAPI.Response.GuildraidHistoryDeck>) null;
      Future<GameObject> unitIconPrefabF;
      if (Object.op_Equality((Object) raidLogTeamPopup.unitIconPrefab, (Object) null))
      {
        unitIconPrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
        e1 = unitIconPrefabF.Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        raidLogTeamPopup.unitIconPrefab = unitIconPrefabF.Result;
        unitIconPrefabF = (Future<GameObject>) null;
      }
      if (Object.op_Equality((Object) raidLogTeamPopup.skillDetailPrefab, (Object) null))
      {
        unitIconPrefabF = PopupSkillDetails.createPrefabLoader(false);
        yield return (object) unitIconPrefabF.Wait();
        raidLogTeamPopup.skillDetailPrefab = unitIconPrefabF.Result;
        unitIconPrefabF = (Future<GameObject>) null;
      }
      yield return (object) raidLogTeamPopup.SetPlayerInfo(data.membership);
      yield return (object) raidLogTeamPopup.SetDeck();
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      ((Component) raidLogTeamPopup).gameObject.SetActive(true);
    }
  }

  private IEnumerator SetUnitIcon(GuildMembership info)
  {
    IEnumerator e;
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
  }

  private IEnumerator SetPlayerInfo(GuildMembership info)
  {
    if (info == null)
    {
      this.dir_playerInfo.SetActive(false);
    }
    else
    {
      this.lblPlayerName.SetTextLocalize(info.player.player_name);
      this.slc_icon_guildmaseter.SetActive(info.role == GuildRole.master);
      this.slc_icon_submaseter.SetActive(info.role == GuildRole.sub_master);
      IEnumerator e = this.SetUnitIcon(info);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private IEnumerator SetDeck()
  {
    if (this.responseHistoryDeck != null)
    {
      this.leaderSkill = (PlayerUnitLeader_skills) null;
      this.friendSkill = (PlayerUnitLeader_skills) null;
      GvgDeck deck = new GvgDeck();
      deck.player_awake_skills = this.responseHistoryDeck.player_awake_skills;
      deck.player_gears = this.responseHistoryDeck.player_gears;
      deck.player_units = this.responseHistoryDeck.player_units;
      deck.over_killers = this.responseHistoryDeck.player_over_killers;
      deck.player_reisou_gears = this.responseHistoryDeck.player_reisou_gears;
      GuildUtil.SetEquippedGearAndAwakeSkill(deck);
      GuildUtil.SetOverkillersUnits(deck);
      this.deckInfo = deck;
      foreach (GameObject linkUnabaibleIcon in this.linkUnabaibleIcons)
        linkUnabaibleIcon.SetActive(true);
      IEnumerator e;
      for (int i = 0; i < this.deckInfo.player_units.Length; ++i)
      {
        e = this.LoadUnitPrefab(i, this.deckInfo.player_units[i]);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (i == 0)
          this.leaderSkill = this.deckInfo.player_units[i].leader_skill;
        this.linkUnabaibleIcons[i].SetActive(false);
      }
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
      GvgReinforcement gvgReinforcement = new GvgReinforcement();
      gvgReinforcement.player_awake_skills = this.responseHistoryDeck.helper_awake_skills;
      gvgReinforcement.player_gears = this.responseHistoryDeck.helper_gears;
      gvgReinforcement.player_unit = ((IEnumerable<PlayerUnit>) this.responseHistoryDeck.helper_unit).FirstOrDefault<PlayerUnit>();
      gvgReinforcement.over_killers = this.responseHistoryDeck.helper_over_killers;
      gvgReinforcement.player_reisou_gears = this.responseHistoryDeck.helper_reisou_gears;
      GuildUtil.SetEquippedGearAndAwakeSkill(gvgReinforcement.player_unit, gvgReinforcement.player_gears, gvgReinforcement.player_reisou_gears, gvgReinforcement.player_awake_skills);
      GuildUtil.SetOverkillersUnits(gvgReinforcement.player_unit, gvgReinforcement.over_killers);
      this.friend = gvgReinforcement;
      if (gvgReinforcement.player_unit != (PlayerUnit) null)
      {
        this.slc_NotFriend_Skill.SetActive(false);
        PlayerUnit playerUnit = gvgReinforcement.player_unit;
        this.friendSkill = playerUnit.leader_skill;
        e = this.LoadUnitPrefab(5, playerUnit);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
      {
        this.slc_NotFriend_Skill.SetActive(true);
        this.lblNoFriend.SetTextLocalize(Consts.GetInstance().QUEST_0028_INDICATOR_NOT_RENTAL);
        e = this.LoadUnitPrefab(5, (PlayerUnit) null);
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

  private IEnumerator LoadUnitPrefab(int index, PlayerUnit unit)
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
      unitScript.onClick = (Action<UnitIconBase>) (x => this.StartCoroutine(this.ChangeDetailScene(unit, index)));
      EventDelegate.Set(unitScript.Button.onLongPress, (EventDelegate.Callback) (() => this.StartCoroutine(this.ChangeDetailScene(unit, index))));
      unitScript.BreakWeapon = unit.IsBrokenEquippedGear;
      unitScript.SpecialIcon = false;
    }
    else
      unitScript.SetEmpty();
    unitScript.Favorite = false;
    unitScript.Gray = false;
  }

  private IEnumerator ChangeDetailScene(PlayerUnit unit, int index)
  {
    GuildRaidLogTeamPopup raidLogTeamPopup = this;
    if (!raidLogTeamPopup.IsPushAndSet())
    {
      Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
      yield return (object) null;
      bool stack = true;
      if (Object.op_Inequality((Object) Singleton<ExploreSceneManager>.GetInstanceOrNull(), (Object) null))
      {
        Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
        Singleton<NGSceneManager>.GetInstance().clearStack();
        stack = false;
      }
      if (index == Consts.GetInstance().DECK_POSITION_FRIEND - 1)
        Unit0042Scene.changeSceneGvgUnit((stack ? 1 : 0) != 0, unit, new PlayerUnit[1]
        {
          unit
        }, true);
      else
        Unit0042Scene.changeSceneGvgUnit(stack, unit, raidLogTeamPopup.deckInfo.player_units, true);
      Singleton<PopupManager>.GetInstance().dismiss();
      ((Component) raidLogTeamPopup).gameObject.SetActive(false);
    }
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

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnBack();

  public void onClickedLeaderSkillZoom()
  {
    if (this.leaderSkill == null || Object.op_Equality((Object) this.skillDetailPrefab, (Object) null) || this.IsPushAndSet())
      return;
    PopupSkillDetails.show(this.skillDetailPrefab, new PopupSkillDetails.Param(this.leaderSkill), onClosed: new Action(this.onClosedSkillZoom));
  }

  public void onClickedFriendSkillZoom()
  {
    if (this.friendSkill == null || Object.op_Equality((Object) this.skillDetailPrefab, (Object) null) || this.IsPushAndSet())
      return;
    PopupSkillDetails.show(this.skillDetailPrefab, new PopupSkillDetails.Param(this.friendSkill), onClosed: new Action(this.onClosedSkillZoom));
  }

  private void onClosedSkillZoom() => this.IsPush = false;
}
