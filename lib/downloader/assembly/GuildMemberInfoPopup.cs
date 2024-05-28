// Decompiled with JetBrains decompiler
// Type: GuildMemberInfoPopup
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
public class GuildMemberInfoPopup : BackButtonMenuBase
{
  private const string DefaultDeckPowerValue = "---";
  private bool isEnemy;
  private Guild0282Menu guild282Menu;
  private Guild0282GuildBaseMenu guildBaseMenu;
  private GuildMemberObject guildMemberObjs;
  private GuildMembership memberInfo;
  private UnitIcon unitIcon;
  private GameObject unitIconPrefab;
  [SerializeField]
  private UILabel playerName;
  [SerializeField]
  private UI2DSprite playerImage;
  [SerializeField]
  private UI2DSprite emblemImage;
  [SerializeField]
  private UILabel contribution;
  [SerializeField]
  private UILabel contributionValue;
  [SerializeField]
  private UILabel positionValue;
  [SerializeField]
  private UILabel txt_player_level;
  [SerializeField]
  private UILabel playerLv;
  [SerializeField]
  private UIButton buttonChangeRole;
  [SerializeField]
  private UIButton buttonMap;
  [SerializeField]
  private GameObject dir_position;
  [SerializeField]
  private GameObject slc_master_icon;
  [SerializeField]
  private GameObject slc_submaster_icon;
  [SerializeField]
  private UILabel attackPower;
  [SerializeField]
  private UILabel attackPowerValue;
  [SerializeField]
  private UILabel defensePower;
  [SerializeField]
  private UILabel defensePowerValue;
  [SerializeField]
  private UILabel lastLoginValue;
  [SerializeField]
  private GameObject dir_frame_own;
  [SerializeField]
  private GameObject dir_frame_enemy;
  private Action actionAfterRoleChange;
  private Action actionClose;

  public IEnumerator Initialize(
    bool is_enemy,
    Guild0282Menu menu,
    GuildMembership info,
    Guild0282GuildBaseMenu baseMenu,
    GuildMemberObject popupObjs,
    Action action = null)
  {
    GuildMemberInfoPopup guildMemberInfoPopup = this;
    if (Object.op_Inequality((Object) ((Component) guildMemberInfoPopup).GetComponent<UIWidget>(), (Object) null))
      ((UIRect) ((Component) guildMemberInfoPopup).GetComponent<UIWidget>()).alpha = 0.0f;
    guildMemberInfoPopup.isEnemy = is_enemy;
    guildMemberInfoPopup.guild282Menu = menu;
    guildMemberInfoPopup.memberInfo = info;
    guildMemberInfoPopup.guildBaseMenu = baseMenu;
    guildMemberInfoPopup.guildMemberObjs = popupObjs;
    guildMemberInfoPopup.actionAfterRoleChange = action;
    guildMemberInfoPopup.actionClose = (Action) null;
    ((Component) guildMemberInfoPopup.buttonMap).gameObject.SetActive(true);
    guildMemberInfoPopup.SetRoleChangeButton();
    IEnumerator e = guildMemberInfoPopup.SetData(info);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator Initialize(
    string player_id,
    Guild0282Menu menu,
    Guild0282GuildBaseMenu baseMenu,
    GuildMemberObject popupObjs,
    Action actionRoleChange = null,
    bool showMapButton = true,
    bool fromPlayerRankingDamage = false)
  {
    GuildMemberInfoPopup guildMemberInfoPopup = this;
    if (Object.op_Inequality((Object) ((Component) guildMemberInfoPopup).GetComponent<UIWidget>(), (Object) null))
      ((UIRect) ((Component) guildMemberInfoPopup).GetComponent<UIWidget>()).alpha = 0.0f;
    guildMemberInfoPopup.memberInfo = ((IEnumerable<GuildMembership>) PlayerAffiliation.Current.guild.memberships).Where<GuildMembership>((Func<GuildMembership, bool>) (x => x.player.player_id.Equals(player_id))).FirstOrDefault<GuildMembership>();
    guildMemberInfoPopup.isEnemy = false;
    guildMemberInfoPopup.guild282Menu = menu;
    guildMemberInfoPopup.guildBaseMenu = baseMenu;
    guildMemberInfoPopup.guildMemberObjs = popupObjs;
    guildMemberInfoPopup.actionAfterRoleChange = actionRoleChange;
    guildMemberInfoPopup.actionClose = !showMapButton ? (Action) (() =>
    {
      if (!Singleton<NGSceneManager>.GetInstance().sceneName.Equals("guild028_2") || !Object.op_Inequality((Object) Singleton<NGSceneManager>.GetInstance().sceneBase, (Object) null) || !Object.op_Inequality((Object) ((Component) Singleton<NGSceneManager>.GetInstance().sceneBase).GetComponent<Guild0282Menu>(), (Object) null))
        return;
      ((Component) Singleton<NGSceneManager>.GetInstance().sceneBase).GetComponent<Guild0282Menu>().isClosePopupByBackBtn = true;
    }) : (Action) (() =>
    {
      if (fromPlayerRankingDamage)
        return;
      this.StartCoroutine(this.showMemberList());
    });
    ((Component) guildMemberInfoPopup.buttonMap).gameObject.SetActive(showMapButton);
    guildMemberInfoPopup.SetRoleChangeButton();
    IEnumerator e = guildMemberInfoPopup.SetData(guildMemberInfoPopup.memberInfo);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator Initialize(
    bool is_enemy,
    Guild0282Menu menu,
    GuildMembership info,
    GuildMemberObject popupObjs,
    Action action = null,
    bool showMapButton = true)
  {
    GuildMemberInfoPopup guildMemberInfoPopup = this;
    if (Object.op_Inequality((Object) ((Component) guildMemberInfoPopup).GetComponent<UIWidget>(), (Object) null))
      ((UIRect) ((Component) guildMemberInfoPopup).GetComponent<UIWidget>()).alpha = 0.0f;
    guildMemberInfoPopup.isEnemy = is_enemy;
    guildMemberInfoPopup.memberInfo = info;
    guildMemberInfoPopup.guild282Menu = menu;
    guildMemberInfoPopup.guildBaseMenu = (Guild0282GuildBaseMenu) null;
    guildMemberInfoPopup.guildMemberObjs = popupObjs;
    guildMemberInfoPopup.actionAfterRoleChange = action;
    guildMemberInfoPopup.actionClose = (Action) null;
    ((Component) guildMemberInfoPopup.buttonMap).gameObject.SetActive(showMapButton);
    guildMemberInfoPopup.SetRoleChangeButton();
    IEnumerator e = guildMemberInfoPopup.SetData(guildMemberInfoPopup.memberInfo);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private void SetRoleChangeButton()
  {
    if (this.isEnemy)
    {
      ((Component) this.buttonChangeRole).gameObject.SetActive(false);
    }
    else
    {
      bool flag = Player.Current.id.Equals(this.memberInfo.player.player_id);
      GuildRole? role1 = PlayerAffiliation.Current.role;
      GuildRole guildRole1 = GuildRole.general;
      if (!(role1.GetValueOrDefault() == guildRole1 & role1.HasValue))
      {
        GuildRole? role2 = PlayerAffiliation.Current.role;
        GuildRole guildRole2 = GuildRole.sub_master;
        if (!(role2.GetValueOrDefault() == guildRole2 & role2.HasValue) || this.memberInfo.role != GuildRole.master)
        {
          role2 = PlayerAffiliation.Current.role;
          GuildRole guildRole3 = GuildRole.sub_master;
          if (!(role2.GetValueOrDefault() == guildRole3 & role2.HasValue) || this.memberInfo.role != GuildRole.sub_master || flag)
            goto label_6;
        }
      }
      ((Component) this.buttonChangeRole).gameObject.SetActive(false);
label_6:
      if (PlayerAffiliation.Current.onGvgOperation && ((Component) this.buttonChangeRole).gameObject.activeSelf)
        ((UIButtonColor) this.buttonChangeRole).isEnabled = false;
      if (!((Component) this.buttonChangeRole).gameObject.activeSelf || !PlayerAffiliation.Current.IsRaidGuildNotMovePeriod())
        return;
      ((UIButtonColor) this.buttonChangeRole).isEnabled = false;
    }
  }

  private IEnumerator SetData(GuildMembership info)
  {
    this.dir_frame_own.SetActive(!this.isEnemy);
    this.dir_frame_enemy.SetActive(this.isEnemy);
    this.playerName.SetTextLocalize(info.player.player_name);
    this.txt_player_level.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_GUILD_MEMBER_DETAIL_PLAYER_LEVEL));
    this.playerLv.SetTextLocalize(info.player.player_level);
    this.contribution.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_GUILD_MEMBER_DETAIL_CONTRIBUTION));
    this.contributionValue.SetTextLocalize(info.contribution);
    this.slc_master_icon.SetActive(info.role == GuildRole.master);
    this.slc_submaster_icon.SetActive(info.role == GuildRole.sub_master);
    if (info.role == GuildRole.master)
      this.positionValue.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_GUILD_POSITION_MASTER));
    else if (info.role == GuildRole.sub_master)
      this.positionValue.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_GUILD_POSITION_SUB_MASTER));
    else
      this.dir_position.SetActive(false);
    this.attackPower.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_GUILD_ATTACK_DECK_TOTAL_VALUE));
    if (PlayerAffiliation.Current.guild_map_enabled)
      this.attackPowerValue.SetTextLocalize(info.total_attack);
    else if (GuildUtil.isBattleOrPreparing(PlayerAffiliation.Current.guild.gvg_status))
      this.attackPowerValue.SetTextLocalize(info.total_attack);
    else
      this.attackPowerValue.SetTextLocalize("---");
    this.defensePower.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_GUILD_DEFENSE_DECK_TOTAL_VALUE));
    if (PlayerAffiliation.Current.guild_map_enabled)
      this.defensePowerValue.SetTextLocalize(info.total_defense);
    else if (GuildUtil.isBattleOrPreparing(PlayerAffiliation.Current.guild.gvg_status))
      this.defensePowerValue.SetTextLocalize(info.total_defense);
    else
      this.defensePowerValue.SetTextLocalize("---");
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.lastLoginValue.SetTextLocalize((ServerTime.NowAppTime() - info.player.last_signed_in_at).DisplayStringForFriendsGuildMember());
    e = this.SetUnitIcon(info);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator SetUnitIcon(GuildMembership info)
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
      PlayerUnit player = PlayerUnit.create_by_unitunit(info.player.leader_unit_unit);
      player.level = info.player.leader_unit_level;
      player.job_id = info.player.leader_unit_job_id;
      if (Object.op_Equality((Object) this.unitIcon, (Object) null))
        this.unitIcon = this.unitIconPrefab.CloneAndGetComponent<UnitIcon>(((Component) this.playerImage).gameObject);
      e = this.unitIcon.setSimpleUnit(player);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.unitIcon.setLevelText(player);
      this.unitIcon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
      player = (PlayerUnit) null;
      player = (PlayerUnit) null;
    }
    Future<Sprite> image = EmblemUtility.LoadEmblemSprite(info.player.player_emblem_id);
    e = image.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.emblemImage.sprite2D = image.Result;
  }

  private IEnumerator MoveToMap()
  {
    GuildMemberInfoPopup guildMemberInfoPopup = this;
    if (Object.op_Inequality((Object) guildMemberInfoPopup.guildBaseMenu, (Object) null))
      guildMemberInfoPopup.guildBaseMenu.PlayTween(false);
    Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();
    while (Singleton<PopupManager>.GetInstance().isOpen)
      yield return (object) null;
    if (Object.op_Inequality((Object) guildMemberInfoPopup.guild282Menu, (Object) null))
    {
      // ISSUE: reference to a compiler-generated method
      Guild0282MemberBase guild0282MemberBase = guildMemberInfoPopup.guild282Menu.MyGuildUI.memberBaseList.Where<Guild0282MemberBase>(new Func<Guild0282MemberBase, bool>(guildMemberInfoPopup.\u003CMoveToMap\u003Eb__36_0)).FirstOrDefault<Guild0282MemberBase>();
      if (Object.op_Equality((Object) guild0282MemberBase, (Object) null) && guildMemberInfoPopup.guild282Menu.EnGuildUI != null)
      {
        // ISSUE: reference to a compiler-generated method
        guild0282MemberBase = guildMemberInfoPopup.guild282Menu.EnGuildUI.memberBaseList.Where<Guild0282MemberBase>(new Func<Guild0282MemberBase, bool>(guildMemberInfoPopup.\u003CMoveToMap\u003Eb__36_1)).FirstOrDefault<Guild0282MemberBase>();
      }
      if (Object.op_Inequality((Object) guild0282MemberBase, (Object) null))
        Guild0282Scene.ChangeSceneOrMemberFocus(guildMemberInfoPopup.memberInfo, guildMemberInfoPopup.guild282Menu);
    }
    else
      Guild0282Scene.ChangeSceneOrMemberFocus(guildMemberInfoPopup.memberInfo, (Guild0282Menu) null);
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
    if (this.actionClose == null)
      return;
    this.actionClose();
    GuildChatManager.scrollWheel_flg = false;
  }

  public void onMapButton()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.MoveToMap());
  }

  public void onChangePositionButton()
  {
    if (this.IsPushAndSet())
      return;
    this.ShowChangeRolePopup();
  }

  private void ShowChangeRolePopup()
  {
    GameObject prefab = this.guildMemberObjs.GuildPositionManagementPopup.Clone();
    GuildMemberPositionManagementPopup component = prefab.GetComponent<GuildMemberPositionManagementPopup>();
    prefab.SetActive(false);
    component.Initialize(this.guild282Menu, this.memberInfo, this.guildBaseMenu, this.guildMemberObjs, this.actionAfterRoleChange, ((Component) this.buttonMap).gameObject.activeSelf);
    prefab.SetActive(true);
    component.ResetScroll();
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
    this.IsPush = false;
    GuildChatManager.scrollWheel_flg = true;
  }

  private IEnumerator showMemberList()
  {
    if (!Object.op_Equality((Object) this.guildMemberObjs.GuildMemberListPopup, (Object) null))
    {
      GameObject popup = this.guildMemberObjs.GuildMemberListPopup.Clone();
      GuildMemberListPopup component = popup.GetComponent<GuildMemberListPopup>();
      popup.SetActive(false);
      IEnumerator e = component.Initialize(false, this.guild282Menu, this.guildBaseMenu, this.guildMemberObjs, PlayerAffiliation.Current.guild, this.actionAfterRoleChange);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      popup.SetActive(true);
      Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
    }
  }
}
