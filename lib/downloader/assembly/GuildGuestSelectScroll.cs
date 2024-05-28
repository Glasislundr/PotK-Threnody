// Decompiled with JetBrains decompiler
// Type: GuildGuestSelectScroll
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
using UnitStatusInformation;
using UnityEngine;

#nullable disable
public class GuildGuestSelectScroll : MonoBehaviour
{
  [SerializeField]
  private GameObject slc_icon_guildmaseter;
  [SerializeField]
  private GameObject slc_icon_submaseter;
  [SerializeField]
  private GameObject link_Character;
  [SerializeField]
  private UILabel lblFriendName;
  [SerializeField]
  private UILabel comment;
  [SerializeField]
  private GameObject dir_player_info;
  [SerializeField]
  private GameObject dir_player_info_master;
  [SerializeField]
  private GameObject slc_icon_guildmaster;
  [SerializeField]
  private GameObject slc_icon_submaster;
  [SerializeField]
  private UILabel lblFriendName_master;
  [SerializeField]
  private UILabel lblFriendSkillName;
  [SerializeField]
  private UILabel lblFriendSkillDesc;
  [SerializeField]
  private UI2DSprite playerTitle;
  [SerializeField]
  private UI2DSprite playerTitle_master;
  [SerializeField]
  private GameObject dir_unit_rented;
  [SerializeField]
  private UIButton btnDecide;
  [SerializeField]
  private GameObject dir_Friendlist_None;
  [SerializeField]
  private GameObject dir_Friendlist;
  private GameObject unitIconPrefab;
  [SerializeField]
  private UIButton skillAllBtn;
  private UnitIcon unitIcon;
  private Action<GvgCandidate> decideButtonAction;
  private GvgCandidate friend;
  private GuildGuestSelectScroll.MODE mode;

  private IEnumerator SkillAllBtnOnClick(BattleskillSkill skill)
  {
    Future<GameObject> loader = PopupSkillDetails.createPrefabLoader(Singleton<NGGameDataManager>.GetInstance().IsSea);
    yield return (object) loader.Wait();
    PopupSkillDetails.show(loader.Result, new PopupSkillDetails.Param(skill, UnitParameter.SkillGroup.Leader));
  }

  private IEnumerator SetUnitIcon(GvgCandidate friend, bool isRaidHelper = false)
  {
    PlayerUnit friendUnit = (PlayerUnit) null;
    friendUnit = friend.player_unit;
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
    if (friend != null)
    {
      if (Object.op_Equality((Object) this.unitIcon, (Object) null))
        this.unitIcon = this.unitIconPrefab.CloneAndGetComponent<UnitIcon>(this.link_Character);
      e = this.unitIcon.setSimpleUnit(friendUnit);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.unitIcon.setLevelText(friendUnit);
      this.unitIcon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
      if (isRaidHelper)
        this.unitIcon.onClick = (Action<UnitIconBase>) (x =>
        {
          Singleton<CommonRoot>.GetInstance().loadingMode = 0;
          Singleton<CommonRoot>.GetInstance().isLoading = true;
          Unit0042Scene.changeSceneFriendUnit(true, friend.player_id, friendUnit.id);
        });
      else
        this.unitIcon.onClick = (Action<UnitIconBase>) (x => this.StartCoroutine(this.onClickIcon(friend.player_id)));
    }
    this.skillAllBtn.onClick.Clear();
    this.skillAllBtn.onClick.Add(new EventDelegate((EventDelegate.Callback) (() => this.StartCoroutine(this.SkillAllBtnOnClick(friendUnit.leader_skill.skill)))));
    Future<Sprite> image = EmblemUtility.LoadEmblemSprite(friend.player_emblem_id);
    e = image.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.playerTitle.sprite2D = image.Result;
    this.playerTitle_master.sprite2D = image.Result;
  }

  private IEnumerator onClickIcon(string member_id)
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<WebAPI.Response.GvgDeckReinforcementStatus> futureF = WebAPI.GvgDeckReinforcementStatus(this.mode == GuildGuestSelectScroll.MODE.Atk, member_id);
    IEnumerator e = futureF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    WebAPI.Response.GvgDeckReinforcementStatus result = futureF.Result;
    if (result != null)
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
    }
  }

  private void SetMemberPositionIcon(string member_id)
  {
    int? nullable = ((IEnumerable<GuildMembership>) PlayerAffiliation.Current.guild.memberships).FirstIndexOrNull<GuildMembership>((Func<GuildMembership, bool>) (x => x.player.player_id.Equals(member_id)));
    if (!nullable.HasValue)
      return;
    GuildMembership membership = PlayerAffiliation.Current.guild.memberships[nullable.Value];
    bool flag = membership.role == GuildRole.general;
    this.dir_player_info.SetActive(flag);
    this.dir_player_info_master.SetActive(!flag);
    if (flag)
      return;
    this.slc_icon_guildmaster.SetActive(membership.role == GuildRole.master);
    this.slc_icon_submaster.SetActive(membership.role == GuildRole.sub_master);
  }

  public IEnumerator InitializeAsync(
    GvgCandidate friend,
    GuildGuestSelectScroll.MODE mode,
    Action<GvgCandidate> action,
    bool isRaidHelper = false)
  {
    this.friend = friend;
    this.mode = mode;
    this.dir_Friendlist_None.SetActive(friend == null);
    this.dir_Friendlist.SetActive(friend != null);
    if (friend != null)
    {
      IEnumerator e = this.SetUnitIcon(friend, isRaidHelper);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.lblFriendName.SetTextLocalize(friend.player_name);
      this.lblFriendName_master.SetTextLocalize(friend.player_name);
      this.SetMemberPositionIcon(friend.player_id);
      if (friend.player_unit != (PlayerUnit) null && friend.player_unit.leader_skill != null)
      {
        this.lblFriendSkillName.SetTextLocalize(friend.player_unit.leader_skill.skill.name);
        this.lblFriendSkillDesc.SetTextLocalize(friend.player_unit.leader_skill.skill.description);
      }
      else
      {
        this.lblFriendSkillName.SetText("---");
        this.lblFriendSkillDesc.SetText("-----");
      }
      if (mode == GuildGuestSelectScroll.MODE.Def)
      {
        this.dir_unit_rented.SetActive(friend.using_player_id != null);
        ((UIButtonColor) this.btnDecide).isEnabled = friend.using_player_id == null;
      }
      else
      {
        this.dir_unit_rented.SetActive(false);
        ((UIButtonColor) this.btnDecide).isEnabled = true;
      }
    }
    ((Component) this.btnDecide).GetComponent<BoxCollider>().size = Vector2.op_Implicit(((UIWidget) ((Component) this.btnDecide).GetComponent<UISprite>()).localSize);
    this.decideButtonAction = action;
  }

  public void DecideButton()
  {
    if (this.decideButtonAction == null)
      return;
    this.decideButtonAction(this.friend);
  }

  public enum MODE
  {
    Atk,
    Def,
  }
}
