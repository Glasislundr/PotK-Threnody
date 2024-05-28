// Decompiled with JetBrains decompiler
// Type: Guild0282MemberScroll
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
public class Guild0282MemberScroll : MonoBehaviour
{
  private bool isEnemy;
  private GuildMemberListPopup memberListPopup;
  private Guild0282Menu guild282Menu;
  private Guild0282GuildBaseMenu guildBaseMenu;
  private GuildMembership guildMemberInfo;
  private GuildMemberObject guildMemberObjs;
  private Action actionAfterRoleChange;
  private UnitIcon unitIcon;
  private GameObject unitIconPrefab;
  [SerializeField]
  private GameObject playerImage;
  [SerializeField]
  private UI2DSprite emblemImage;
  [SerializeField]
  private UILabel playerName;
  [SerializeField]
  private UILabel contribution;
  [SerializeField]
  private UILabel contributionValue;
  [SerializeField]
  private UILabel txt_playerlv;
  [SerializeField]
  private UILabel playerLv;
  [SerializeField]
  private GameObject slcListbasePlayer;
  [SerializeField]
  private GameObject slcListbaseMember;
  [SerializeField]
  private GameObject masterIconObj;
  [SerializeField]
  private GameObject subMasterIconObj;
  [SerializeField]
  private UILabel txt_nonparticipation;
  [SerializeField]
  private GameObject dir_remain_battle_count;
  [SerializeField]
  private GameObject dir_stars_get;
  [SerializeField]
  private UILabel txt_stars_get;
  [SerializeField]
  private UILabel lbl_star_acquired;
  [SerializeField]
  private GameObject dir_star_possession;
  [SerializeField]
  private UILabel txtStars_;
  [SerializeField]
  private Guild0282MemberScroll.BattleCountIcon[] battleCountIcon;
  [SerializeField]
  private UILabel txt_last_login_at;
  [SerializeField]
  private GameObject dir_defense_member_select;
  [SerializeField]
  private UILabel txt_num;
  private GuildDefenseMemberListPopup defenseListPopup;

  public IEnumerator Initialize(
    bool is_enemy,
    GuildMemberListPopup popup,
    GuildMembership info,
    Guild0282Menu guildMenu,
    Guild0282GuildBaseMenu baseMenu,
    GuildMemberObject popupObjs,
    Action action = null)
  {
    this.isEnemy = is_enemy;
    this.memberListPopup = popup;
    this.guildMemberInfo = info;
    this.guild282Menu = guildMenu;
    this.guildBaseMenu = baseMenu;
    this.guildMemberObjs = popupObjs;
    this.actionAfterRoleChange = action;
    IEnumerator e = this.SetData(info);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator SetData()
  {
    IEnumerator e = this.SetData(this.guildMemberInfo);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator SetData(GuildMembership info)
  {
    this.playerName.SetTextLocalize(info.player.player_name);
    this.txt_playerlv.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_GUILD_MEMBER_MASTER_LEVEL));
    this.playerLv.SetTextLocalize(info.player.player_level);
    this.contribution.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_GUILD_MEMBER_CONTRIBUTION));
    this.contributionValue.SetTextLocalize(info.contribution);
    this.txt_nonparticipation.SetTextLocalize(Consts.GetInstance().POPUP_GUILD_BATTLE_MEMBER_SCORE_NOT_DEFENSE);
    bool onGvg = GuildUtil.isBattleOrPreparing(PlayerAffiliation.Current.guild.gvg_status);
    IEnumerator e = this.SetLastLoginText(onGvg, info);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    bool flag1 = info.player.player_id.Equals(Player.Current.id);
    this.slcListbaseMember.SetActive(!flag1);
    this.slcListbasePlayer.SetActive(flag1);
    this.masterIconObj.SetActive(info.role == GuildRole.master);
    this.subMasterIconObj.SetActive(info.role == GuildRole.sub_master);
    this.dir_stars_get.SetActive(onGvg);
    ((Component) this.txt_nonparticipation).gameObject.SetActive(onGvg && !info.is_defense_membership);
    this.dir_star_possession.SetActive(onGvg && info.is_defense_membership);
    this.dir_remain_battle_count.SetActive(onGvg);
    if (onGvg)
    {
      this.txt_stars_get.SetTextLocalize(Consts.GetInstance().POPUP_GUILD_BATTLE_MEMBER_SCORE_ACQUIRED_STAR);
      this.lbl_star_acquired.SetTextLocalize(info.capture_star);
      this.txtStars_.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_GUILD_BATTLE_MEMBER_SCORE_STAR_VALUE, (IDictionary) new Hashtable()
      {
        {
          (object) "numerator",
          (object) info.own_star
        },
        {
          (object) "denominator",
          (object) PlayerAffiliation.Current.guild.gvg_max_star_possession
        }
      }));
      for (int index = 0; index < this.battleCountIcon.Length; ++index)
      {
        Guild0282MemberScroll.BattleCountIcon battleCountIcon = this.battleCountIcon[index];
        battleCountIcon.inBattle.SetActive(false);
        bool flag2 = index < info.action_point;
        if (this.isEnemy)
        {
          battleCountIcon.own.SetActive(false);
          battleCountIcon.enemy.SetActive(flag2);
        }
        else
        {
          battleCountIcon.own.SetActive(flag2);
          battleCountIcon.enemy.SetActive(false);
        }
        if (Object.op_Inequality((Object) battleCountIcon.objOff, (Object) null))
          battleCountIcon.objOff.SetActive(!flag2);
      }
      if (info.in_attack && info.action_point < this.battleCountIcon.Length)
      {
        Guild0282MemberScroll.BattleCountIcon battleCountIcon = this.battleCountIcon[info.action_point];
        battleCountIcon.inBattle.SetActive(true);
        if (Object.op_Inequality((Object) battleCountIcon.objOff, (Object) null))
          battleCountIcon.objOff.SetActive(false);
      }
    }
    e = this.SetUnitIcon(info);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.SetDefenseNumber();
  }

  private IEnumerator SetLastLoginText(bool onGvg, GuildMembership info)
  {
    if (onGvg)
    {
      ((Component) this.txt_last_login_at).gameObject.SetActive(false);
    }
    else
    {
      IEnumerator e = ServerTime.WaitSync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      TimeSpan self = ServerTime.NowAppTime() - info.player.last_signed_in_at;
      ((Component) this.txt_last_login_at).gameObject.SetActive(true);
      this.txt_last_login_at.SetTextLocalize(Consts.Format(Consts.GetInstance().LAST_PLAY_FRIEND_DETAIL, (IDictionary) new Hashtable()
      {
        {
          (object) "time",
          (object) self.DisplayStringForFriendsGuildMember()
        }
      }));
    }
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
        this.unitIcon = this.unitIconPrefab.CloneAndGetComponent<UnitIcon>(this.playerImage);
      e = this.unitIcon.setSimpleUnit(player);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.unitIcon.setLevelText(player);
      this.unitIcon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
      player = (PlayerUnit) null;
    }
    Future<Sprite> image = EmblemUtility.LoadEmblemSprite(info.player.player_emblem_id);
    e = image.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.emblemImage.sprite2D = image.Result;
  }

  public void FriendDetailScene(GuildPlayerInfo playerInfo)
  {
    Singleton<PopupManager>.GetInstance().dismiss();
    if (playerInfo.player_id.Equals(Player.Current.id))
      Unit0042Scene.changeScene(true, PlayerUnit.create_by_unitunit(playerInfo.leader_unit_unit), SMManager.Get<PlayerUnit[]>());
    else
      Unit0042Scene.changeSceneFriendUnit(true, playerInfo.player_id, 0);
  }

  public void onMemberButton()
  {
    if (this.memberListPopup.IsPushAndSet())
      return;
    this.StartCoroutine(this.ShowMemberInfo());
  }

  public void onDefenseMemberButton()
  {
    if (this.guildMemberInfo.defense_priority == 0)
    {
      this.defenseListPopup.SelectedKeyPriorityValuePlayerIDDic.ForEach<KeyValuePair<int, string>>((Action<KeyValuePair<int, string>>) (x =>
      {
        if (this.guildMemberInfo.defense_priority != 0 || x.Value != null)
          return;
        this.guildMemberInfo.defense_priority = x.Key;
      }));
      if (this.defenseListPopup.SelectedKeyPriorityValuePlayerIDDic.ContainsKey(this.guildMemberInfo.defense_priority))
        this.defenseListPopup.SelectedKeyPriorityValuePlayerIDDic[this.guildMemberInfo.defense_priority] = this.guildMemberInfo.player.player_id;
    }
    else
    {
      if (this.defenseListPopup.SelectedKeyPriorityValuePlayerIDDic.ContainsKey(this.guildMemberInfo.defense_priority))
        this.defenseListPopup.SelectedKeyPriorityValuePlayerIDDic[this.guildMemberInfo.defense_priority] = (string) null;
      this.defenseListPopup.SelectedKeyPriorityValuePlayerIDDic.ForEach<KeyValuePair<int, string>>((Action<KeyValuePair<int, string>>) (x =>
      {
        if (this.guildMemberInfo.defense_priority == 0 || x.Key != this.guildMemberInfo.defense_priority)
          return;
        this.guildMemberInfo.defense_priority = 0;
      }));
    }
    this.SetDefenseNumber();
  }

  private void SetDefenseNumber()
  {
    this.dir_defense_member_select.SetActive(this.guildMemberInfo.defense_priority != 0);
    if (this.guildMemberInfo.defense_priority == 0)
      return;
    this.txt_num.SetTextLocalize(this.guildMemberInfo.defense_priority);
  }

  private IEnumerator ShowMemberInfo()
  {
    GameObject popup = this.guildMemberObjs.GuildMemberInfoPopup.Clone();
    this.memberListPopup.SetScrollValue();
    GuildMemberInfoPopup component = popup.GetComponent<GuildMemberInfoPopup>();
    popup.SetActive(false);
    IEnumerator e = component.Initialize(this.isEnemy, this.guild282Menu, this.guildMemberInfo, this.guildBaseMenu, this.guildMemberObjs, this.actionAfterRoleChange);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
    this.memberListPopup.IsPush = false;
  }

  public void DefenseMemberSelectInitialize(GuildDefenseMemberListPopup popup)
  {
    this.defenseListPopup = popup;
    UIButton component1 = this.slcListbaseMember.GetComponent<UIButton>();
    component1.onClick.Clear();
    component1.onClick.Add(new EventDelegate(new EventDelegate.Callback(this.onDefenseMemberButton)));
    UIButton component2 = this.slcListbasePlayer.GetComponent<UIButton>();
    component2.onClick.Clear();
    component2.onClick.Add(new EventDelegate(new EventDelegate.Callback(this.onDefenseMemberButton)));
  }

  [Serializable]
  private class BattleCountIcon
  {
    [SerializeField]
    public GameObject inBattle;
    [SerializeField]
    public GameObject own;
    [SerializeField]
    public GameObject enemy;
    public GameObject objOff;
  }
}
