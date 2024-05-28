// Decompiled with JetBrains decompiler
// Type: Friend0086Menu
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
public class Friend0086Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtCharaname;
  [SerializeField]
  protected UILabel TxtFighting;
  [SerializeField]
  protected UILabel TxtHp;
  [SerializeField]
  protected UILabel TxtLastplay;
  [SerializeField]
  protected UILabel TxtLv;
  [SerializeField]
  protected UILabel TxtPlayername;
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  protected GameObject LinkCharacter;
  [SerializeField]
  protected GameObject LinkBugu;
  [SerializeField]
  protected UILabel TxtComment;
  [SerializeField]
  protected UILabel TxtPower;
  [SerializeField]
  protected GameObject GuildInfoRoot;
  [SerializeField]
  protected GameObject GuildThumb;
  [SerializeField]
  protected UILabel TxtGuildName;
  [SerializeField]
  protected UISprite GuildLevel;
  [SerializeField]
  protected UISprite GuildLevel10;
  [SerializeField]
  protected UILabel TxtGuildMember;
  [SerializeField]
  protected UILabel TxtGuildMemberCapacity;
  [SerializeField]
  protected GameObject LeaderUnit;
  [SerializeField]
  protected GameObject[] DeckUnits;
  [SerializeField]
  protected UILabel TxtLeader;
  [SerializeField]
  protected UILabel TxtVictoryPoint;
  [SerializeField]
  protected UILabel TxtLeaderRank;
  [SerializeField]
  protected UILabel TxtTotalPower;
  [SerializeField]
  protected UILabel TxtBadgeRanking;
  [SerializeField]
  protected UISprite slcFriendBadge;
  [SerializeField]
  protected UISprite slcGuildBadge;
  public UIButton BtnBattleRecoed;
  public UIButton BtnFavoritOn;
  public UIButton BtnFavoritOff;
  public UIButton BtnCancellation;
  public UIButton BtnApplication;
  private string target_player_id;
  private GuildDirectory guildData;
  private Guild0282GuildBase guildBase;
  private GuildInfoPopup guildInfoPopup;
  private bool favorite;
  [SerializeField]
  private UI2DSprite Emblem;
  private bool isInit;
  private int friendStatus;
  private bool isGuildMember;
  private Action callback;
  private bool isCommError;
  private List<string> target_friend_ids = new List<string>();

  public Versus02613Scene.BootParam bootParam { get; set; }

  public bool isContinueBackground { get; set; }

  private IEnumerator openPopup0087()
  {
    Friend0086Menu friend0086Menu = this;
    Future<GameObject> prefabF = Res.Prefabs.popup.popup_008_7__anim_popup01.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabF.Result;
    Singleton<PopupManager>.GetInstance().open(result).GetComponent<Friend0087Menu>().Init0087PopUp(friend0086Menu.target_player_id, new Action(friend0086Menu.backToScene));
  }

  public virtual void IbtnApproval()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.openPopup0087());
  }

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGGameDataManager>.GetInstance().refreshHomeHome = true;
    this.backToScene();
  }

  private void backToScene()
  {
    if (this.bootParam != null)
      this.bootParam.pop();
    this.backScene();
  }

  public override void onBackButton() => this.IbtnBack();

  private IEnumerator Cancellation()
  {
    Friend0086Menu friend0086Menu = this;
    Future<GameObject> prefab0083F = Res.Prefabs.popup.popup_008_3__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab0083F.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefab0083F.Result;
    Friend0083Menu component = Singleton<PopupManager>.GetInstance().openAlert(result).GetComponent<Friend0083Menu>();
    component.AddTargetFriendId(friend0086Menu.target_player_id);
    component.SetCallback(new Action(friend0086Menu.callbackFriendCancellation));
  }

  public virtual void IbtnCancellation()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.Cancellation());
  }

  private IEnumerator FriendFavoriteAsync(
    string[] target_player_ids,
    string[] unlock_target_player_ids)
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Future<WebAPI.Response.FriendFavorite> ft = WebAPI.FriendFavorite(target_player_ids, unlock_target_player_ids, (Action<WebAPI.Response.UserError>) (e =>
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      WebAPI.DefaultUserErrorCallback(e);
    }));
    IEnumerator e1 = ft.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (ft.Result != null)
    {
      Singleton<NGGameDataManager>.GetInstance().favoriteFriends = ft.Result.favorite_friend_list;
      this.changeFavorite(target_player_ids.Length != 0);
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    }
  }

  public virtual void IbtnFavoriteiOff()
  {
    this.StartCoroutine(this.FriendFavoriteAsync(new string[1]
    {
      this.target_player_id
    }, new string[0]));
  }

  public virtual void IbtnFavoriteiOn()
  {
    this.StartCoroutine(this.FriendFavoriteAsync(new string[0], new string[1]
    {
      this.target_player_id
    }));
  }

  private IEnumerator openPopup0089()
  {
    Friend0086Menu friend0086Menu = this;
    Future<GameObject> prefabF = Res.Prefabs.popup.popup_8_9__anim_popup01.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabF.Result;
    Singleton<PopupManager>.GetInstance().open(result).GetComponent<Friend0089Menu>().InitPopup(friend0086Menu.target_player_id, new Action(friend0086Menu.backToScene));
  }

  public virtual void IbtnRefusal()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.openPopup0089());
  }

  public void changeFavorite(bool favorite)
  {
    if (this.target_player_id == Player.Current.id)
    {
      this.favorite = false;
      ((Component) this.BtnFavoritOn).gameObject.SetActive(false);
      ((Component) this.BtnFavoritOff).gameObject.SetActive(false);
      ((Component) this.BtnCancellation).gameObject.SetActive(false);
      ((Component) this.BtnApplication).gameObject.SetActive(false);
      ((Component) this.slcFriendBadge).gameObject.SetActive(false);
      ((Component) this.slcGuildBadge).gameObject.SetActive(false);
    }
    else
    {
      this.favorite = favorite;
      ((Component) this.BtnFavoritOn).gameObject.SetActive(this.favorite);
      ((Component) this.BtnFavoritOff).gameObject.SetActive(!this.favorite);
      ((UIButtonColor) this.BtnCancellation).isEnabled = !this.favorite;
      this.setFriendStatusButton();
    }
  }

  public void setFriendStatusButton()
  {
    GameObject gameObject = ((Component) ((Component) this.BtnApplication).gameObject.transform.Find("slc_applying")).gameObject;
    switch (this.friendStatus)
    {
      case 1:
        ((Component) this.BtnFavoritOn).gameObject.SetActive(false);
        ((Component) this.BtnFavoritOff).gameObject.SetActive(true);
        ((UIButtonColor) this.BtnFavoritOff).isEnabled = false;
        ((Component) this.BtnCancellation).gameObject.SetActive(false);
        ((Component) this.BtnApplication).gameObject.SetActive(true);
        gameObject.gameObject.SetActive(true);
        ((Component) this.slcFriendBadge).gameObject.SetActive(false);
        ((Component) this.slcGuildBadge).gameObject.SetActive(this.isGuildMember);
        break;
      case 2:
        ((UIButtonColor) this.BtnFavoritOff).isEnabled = true;
        ((Component) this.BtnCancellation).gameObject.SetActive(true);
        ((Component) this.BtnApplication).gameObject.SetActive(false);
        gameObject.gameObject.SetActive(false);
        ((Component) this.slcFriendBadge).gameObject.SetActive(!this.isGuildMember);
        ((Component) this.slcGuildBadge).gameObject.SetActive(this.isGuildMember);
        break;
      default:
        ((Component) this.BtnFavoritOn).gameObject.SetActive(false);
        ((Component) this.BtnFavoritOff).gameObject.SetActive(true);
        ((UIButtonColor) this.BtnFavoritOff).isEnabled = false;
        ((Component) this.BtnCancellation).gameObject.SetActive(false);
        ((Component) this.BtnApplication).gameObject.SetActive(true);
        gameObject.gameObject.SetActive(false);
        ((Component) this.slcFriendBadge).gameObject.SetActive(false);
        ((Component) this.slcGuildBadge).gameObject.SetActive(this.isGuildMember);
        break;
    }
  }

  public void callbackFriendCancellation()
  {
    if (this.friendStatus == 2)
      this.friendStatus = 0;
    this.changeFavorite(this.favorite);
    this.StartCoroutine(PopupCommon.Show(Consts.GetInstance().FRIEND_0086_MENU_CANCELLATION_TITLE, Consts.GetInstance().FRIEND_0086_MENU_CANCELLATION_MESSAGE));
  }

  private IEnumerator SetGuildInfo(GuildDirectory guild)
  {
    if (guild == null)
    {
      this.GuildInfoRoot.SetActive(false);
    }
    else
    {
      GuildAppearance data = guild.appearance;
      this.TxtGuildName.SetTextLocalize(guild.guild_name);
      if (data.level < 10)
      {
        ((Component) this.GuildLevel).gameObject.SetActive(false);
        ((Component) this.GuildLevel10).gameObject.SetActive(true);
        this.GuildLevel10.SetSprite(string.Format("slc_text_glv_number{0}.png__GUI__guild_common_other__guild_common_other_prefab", (object) data.level));
      }
      else
      {
        ((Component) this.GuildLevel).gameObject.SetActive(true);
        ((Component) this.GuildLevel10).gameObject.SetActive(true);
        this.GuildLevel.SetSprite(string.Format("slc_text_glv_number{0}.png__GUI__guild_common_other__guild_common_other_prefab", (object) (data.level % 10)));
        this.GuildLevel10.SetSprite(string.Format("slc_text_glv_number{0}.png__GUI__guild_common_other__guild_common_other_prefab", (object) (data.level / 10)));
      }
      this.TxtGuildMember.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_028_1_1_4_CURRENT_MEMBER, (IDictionary) new Hashtable()
      {
        {
          (object) "currentMember",
          (object) data.membership_num
        }
      }));
      this.TxtGuildMemberCapacity.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_028_1_1_4_MAX_MEMBER, (IDictionary) new Hashtable()
      {
        {
          (object) "maxMember",
          (object) data.membership_capacity
        }
      }));
      Future<GameObject> fgObj = Res.Prefabs.guild028_2.GuildBase.Load<GameObject>();
      IEnumerator e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject guildBasePrefab = fgObj.Result;
      GuildImageCache guildImageCache = new GuildImageCache();
      e = guildImageCache.ResourceLoad(data);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (Object.op_Inequality((Object) this.guildBase, (Object) null))
        Object.Destroy((Object) this.guildBase);
      this.guildBase = guildBasePrefab.Clone(this.GuildThumb.transform).GetComponent<Guild0282GuildBase>();
      ((Collider) ((Component) this.guildBase).GetComponent<BoxCollider>()).enabled = false;
      this.guildBase.SetSprite(data, guildImageCache);
    }
  }

  public void FriendDetailScene(PlayerUnit playerUnit)
  {
    Unit0042Scene.changeSceneFriendUnit(true, this.target_player_id, playerUnit);
  }

  private void SetIconEvent(UIButton button, PlayerUnit unit)
  {
    EventDelegate.Add(button.onClick, (EventDelegate.Callback) (() => this.FriendDetailScene(unit)));
  }

  public IEnumerator setData(
    string in_target_player_id,
    SM.FriendDetail friendDetail,
    int in_friendStatus,
    bool in_is_favorite)
  {
    Friend0086Menu friend0086Menu = this;
    if (!friend0086Menu.isInit)
    {
      friend0086Menu.isInit = true;
      friend0086Menu.target_player_id = in_target_player_id;
      friend0086Menu.friendStatus = in_friendStatus;
      friend0086Menu.guildInfoPopup = new GuildInfoPopup();
      IEnumerator e = friend0086Menu.guildInfoPopup.ResourceLoad();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Future<Sprite> sprF = EmblemUtility.LoadEmblemSprite(friendDetail.player_current_emblem_id);
      e = sprF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      friend0086Menu.Emblem.sprite2D = sprF.Result;
      e = ServerTime.WaitSync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      TimeSpan self = ServerTime.NowAppTime() - friendDetail.target_player_last_signed_in_at;
      friend0086Menu.TxtLastplay.SetTextLocalize(Consts.Format(Consts.GetInstance().LAST_PLAY_FRIEND_DETAIL, (IDictionary) new Hashtable()
      {
        {
          (object) "time",
          (object) self.DisplayStringForFriendsGuildMember()
        }
      }));
      friend0086Menu.TxtPlayername.SetTextLocalize(friendDetail.player_name);
      friend0086Menu.TxtLv.SetTextLocalize(friendDetail.player_level);
      friend0086Menu.SetTextPlayerComment(friendDetail.player_comment);
      friend0086Menu.SetTextRecord(friendDetail);
      friend0086Menu.guildData = friendDetail.guild;
      friend0086Menu.StartCoroutine(friend0086Menu.SetGuildInfo(friend0086Menu.guildData));
      PlayerUnit[] deck = friendDetail.player_units;
      foreach (PlayerUnit playerUnit in deck)
      {
        playerUnit.primary_equipped_gear = playerUnit.FindEquippedGear(friendDetail.player_items);
        playerUnit.primary_equipped_gear2 = playerUnit.FindEquippedGear2(friendDetail.player_items);
        playerUnit.primary_equipped_gear3 = playerUnit.FindEquippedGear3(friendDetail.player_items);
        playerUnit.primary_equipped_reisou = playerUnit.FindEquippedReisou(friendDetail.player_items, friendDetail.player_reisou_items);
        playerUnit.primary_equipped_reisou2 = playerUnit.FindEquippedReisou2(friendDetail.player_items, friendDetail.player_reisou_items);
        playerUnit.primary_equipped_reisou3 = playerUnit.FindEquippedReisou3(friendDetail.player_items, friendDetail.player_reisou_items);
        PlayerAwakeSkill playerAwakeSkill = (PlayerAwakeSkill) null;
        foreach (int? equipAwakeSkillId in playerUnit.equip_awake_skill_ids)
        {
          int? awakeSkillID = equipAwakeSkillId;
          playerAwakeSkill = ((IEnumerable<PlayerAwakeSkill>) friendDetail.player_awake_skills).FirstOrDefault<PlayerAwakeSkill>((Func<PlayerAwakeSkill, bool>) (x =>
          {
            int id = x.id;
            int? nullable = awakeSkillID;
            int valueOrDefault = nullable.GetValueOrDefault();
            return id == valueOrDefault & nullable.HasValue;
          }));
        }
        playerUnit.primary_equipped_awake_skill = playerAwakeSkill;
      }
      e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<PlayerUnit>) deck, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      int combat = 0;
      ((IEnumerable<PlayerUnit>) deck).ForEach<PlayerUnit>((Action<PlayerUnit>) (x =>
      {
        if (!(x != (PlayerUnit) null))
          return;
        combat += Judgement.NonBattleParameter.FromPlayerUnit(x).Combat;
      }));
      friend0086Menu.TxtPower.SetTextLocalize(combat.ToString());
      e = friend0086Menu.LoadCharacterSprite(deck[0].unit.ID, deck[0].job_id, friend0086Menu.LeaderUnit);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Future<GameObject> unitPrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      e = unitPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      for (int i = 0; i < friend0086Menu.DeckUnits.Length && deck.Length > i; ++i)
      {
        if (!(deck[i] == (PlayerUnit) null))
        {
          UnitIcon icon = unitPrefabF.Result.Clone(friend0086Menu.DeckUnits[i].transform).GetComponent<UnitIcon>();
          e = icon.setSimpleUnit(deck[i]);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          icon.setLevelText(deck[i]);
          if (i == 0)
          {
            icon.isViewBackObject = false;
            ((Component) icon.icon).gameObject.SetActive(false);
            UIButton component = friend0086Menu.LeaderUnit.GetComponent<UIButton>();
            friend0086Menu.SetIconEvent(component, deck[i]);
          }
          icon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
          friend0086Menu.SetIconEvent((UIButton) icon.Button, deck[i]);
          icon = (UnitIcon) null;
        }
      }
      friend0086Menu.isGuildMember = false;
      if (PlayerAffiliation.Current != null && friend0086Menu.guildData != null && PlayerAffiliation.Current.isGuildMember() && PlayerAffiliation.Current.guild_id == friend0086Menu.guildData.guild_id)
        friend0086Menu.isGuildMember = true;
      friend0086Menu.changeFavorite(in_is_favorite);
    }
  }

  private void SetTextPlayerComment(string comment)
  {
    if (comment == string.Empty)
      this.TxtComment.SetTextLocalize(Consts.GetInstance().FRIEND_COMMENT_DEFAULT);
    else
      this.TxtComment.SetTextLocalize(comment);
  }

  private void SetTextRecord(SM.FriendDetail friendDetail)
  {
    if (friendDetail.is_join_class_match)
    {
      this.TxtLeader.SetTextLocalize(friendDetail.pvp_current_class_name);
      this.TxtVictoryPoint.SetTextLocalize(Consts.Format(Consts.GetInstance().FRIEND_0086_MENU_VICTORY_POINT_OPEN, (IDictionary) new Hashtable()
      {
        {
          (object) "num",
          (object) friendDetail.pvp_total_win
        }
      }));
    }
    else
    {
      this.TxtLeader.SetTextLocalize(Consts.GetInstance().FRIEND_0086_MENU_CLASS_NAME_CLOSED);
      this.TxtVictoryPoint.SetTextLocalize(Consts.GetInstance().FRIEND_0086_MENU_VICTORY_POINT_CLOSED);
    }
    if (friendDetail.is_join_class_match && friendDetail.is_join_pvp_weekly_class)
    {
      this.TxtTotalPower.SetTextLocalize(Consts.Format(Consts.GetInstance().FRIEND_0086_MENU_TOTAL_POWER_OPEN, (IDictionary) new Hashtable()
      {
        {
          (object) "num",
          (object) friendDetail.pvp_weekly_class_point
        }
      }));
      this.TxtLeaderRank.SetTextLocalize(Consts.Format(Consts.GetInstance().FRIEND_0086_MENU_LEADER_RANK_OPEN, (IDictionary) new Hashtable()
      {
        {
          (object) "num",
          (object) friendDetail.pvp_weekly_class_rank
        }
      }));
      this.TxtBadgeRanking.SetTextLocalize(friendDetail.pvp_weekly_class_name);
    }
    else
    {
      this.TxtTotalPower.SetTextLocalize(Consts.GetInstance().FRIEND_0086_MENU_TOTAL_POWER_CLOSED);
      this.TxtLeaderRank.SetTextLocalize(Consts.GetInstance().FRIEND_0086_MENU_LEADER_RANK_CLOSED);
      this.TxtBadgeRanking.SetTextLocalize(Consts.GetInstance().FRIEND_0086_MENU_BADGE_RANKING_CLOSED);
      ((UIButtonColor) this.BtnBattleRecoed).isEnabled = false;
    }
    ((Component) this.BtnBattleRecoed).gameObject.SetActive(friendDetail.is_display_pvp_history);
  }

  public void onButtonBattleRecord()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.commRecordData());
  }

  public IEnumerator commRecordData()
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Future<WebAPI.Response.FriendPvpClassMatchHistory> futureF = WebAPI.FriendPvpClassMatchHistory(this.target_player_id, (Action<WebAPI.Response.UserError>) (e =>
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      WebAPI.DefaultUserErrorCallback(e);
    }));
    IEnumerator e1 = futureF.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (futureF.Result != null)
    {
      WebAPI.Response.FriendPvpClassMatchHistory result = futureF.Result;
      if (this.bootParam != null)
        Versus02613Scene.ChangeScene(true, this.bootParam, result.pvp_class_record, result.best_class, this.target_player_id, this.isContinueBackground);
      else
        Versus02613Scene.ChangeTopScene(true, result.pvp_class_record, result.best_class, this.target_player_id, this.isContinueBackground);
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    }
  }

  public void onButtonGuildInfo() => this.StartCoroutine(this.ShowGuildInfo());

  private IEnumerator ShowGuildInfo()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Friend0086Menu friend0086Menu = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    Singleton<PopupManager>.GetInstance().open(friend0086Menu.guildInfoPopup.guildInfoPopup).GetComponent<Guild028114Popup>().Initialize(friend0086Menu.guildData, friend0086Menu.guildInfoPopup);
    friend0086Menu.guildInfoPopup.SetRequestMaintenanceCallback(new Action(friend0086Menu.SetActionGuildMaintenance));
    friend0086Menu.guildInfoPopup.SetRequestAlreadyGuildCallback(new Action(friend0086Menu.SetActionAlreadyGuild));
    return false;
  }

  public void SetActionGuildMaintenance()
  {
  }

  public void SetActionAlreadyGuild()
  {
    this.StartCoroutine(PopupCommon.Show(Consts.GetInstance().POPUP_028_1_1_5_TITLE, Consts.GetInstance().POPUP_028_1_1_6_NO_APPLICANT));
  }

  private IEnumerator LoadCharacterSprite(int id, int job_id, GameObject locationObject)
  {
    if (MasterData.UnitUnit.ContainsKey(id))
    {
      Future<Sprite> mainFuture = MasterData.UnitUnit[id].LoadSpriteLarge(job_id, 1f);
      IEnumerator e = mainFuture.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Future<Texture2D> loadMask = new ResourceObject("GUI/008-6_sozai/slc_leader_character_Mask").Load<Texture2D>();
      e = loadMask.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      NGxMaskSpriteWithScale component = locationObject.GetComponent<NGxMaskSpriteWithScale>();
      component.MainUI2DSprite.sprite2D = mainFuture.Result;
      component.maskTexture = loadMask.Result;
      component.FitMask();
      mainFuture = (Future<Sprite>) null;
      loadMask = (Future<Texture2D>) null;
    }
  }

  public void setTxtTitle(string str) => this.TxtTitle.SetTextLocalize(str);

  public void onButtonFriendApplication()
  {
    if (this.IsPushAndSet())
      return;
    if (this.friendStatus == 1)
      this.StartCoroutine(PopupCommon.Show(Consts.GetInstance().FRIEND_0086_MENU_APPLYING_TITLE, Consts.GetInstance().FRIEND_0086_MENU_APPLYING_MESSAGE));
    else
      PopupCommonNoYes.Show(Consts.GetInstance().FRIEND_0086_MENU_APPLICATION_TITLE, Consts.GetInstance().FRIEND_0086_MENU_APPLICATION_MESSAGE, (Action) (() =>
      {
        Singleton<PopupManager>.GetInstance().dismiss();
        this.StartCoroutine(this.FriendApplyAsync());
      }), (Action) (() => { }));
  }

  private IEnumerator FriendApplyAsync()
  {
    Friend0086Menu friend0086Menu = this;
    friend0086Menu.isCommError = false;
    if (SMManager.Get<Player>().friends_count >= SMManager.Get<Player>().max_friends)
    {
      Singleton<CommonRoot>.GetInstance().StartCoroutine(friend0086Menu.openPopup00813());
    }
    else
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 1;
      friend0086Menu.target_friend_ids.Clear();
      friend0086Menu.target_friend_ids.Add(friend0086Menu.target_player_id);
      // ISSUE: reference to a compiler-generated method
      Future<WebAPI.Response.FriendApply> future = WebAPI.FriendApply(friend0086Menu.target_friend_ids.ToArray(), new Action<WebAPI.Response.UserError>(friend0086Menu.\u003CFriendApplyAsync\u003Eb__82_0));
      IEnumerator e = future.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (future.HasResult && !friend0086Menu.isCommError)
      {
        Singleton<CommonRoot>.GetInstance().StartCoroutine(friend0086Menu.openPopup00815());
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        if (friend0086Menu.friendStatus == 0)
          friend0086Menu.friendStatus = 1;
        friend0086Menu.changeFavorite(friend0086Menu.favorite);
      }
      else
      {
        friend0086Menu.callback();
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      }
    }
  }

  private IEnumerator openPopup00813()
  {
    Friend0086Menu friend0086Menu = this;
    Future<GameObject> prefab = Res.Prefabs.popup.popup_008_13__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    // ISSUE: reference to a compiler-generated method
    EventDelegate.Add(Singleton<PopupManager>.GetInstance().openAlert(prefab.Result).GetComponent<FriendApplicationResultPopup>().OK.onClick, new EventDelegate.Callback(friend0086Menu.\u003CopenPopup00813\u003Eb__83_0));
  }

  private IEnumerator openPopup00814()
  {
    Friend0086Menu friend0086Menu = this;
    Future<GameObject> prefab = Res.Prefabs.popup.popup_008_14__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    // ISSUE: reference to a compiler-generated method
    EventDelegate.Add(Singleton<PopupManager>.GetInstance().openAlert(prefab.Result).GetComponent<FriendApplicationResultPopup>().OK.onClick, new EventDelegate.Callback(friend0086Menu.\u003CopenPopup00814\u003Eb__84_0));
  }

  private IEnumerator openPopup00815()
  {
    Friend0086Menu friend0086Menu = this;
    Future<GameObject> prefab = Res.Prefabs.popup.popup_008_15__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    // ISSUE: reference to a compiler-generated method
    EventDelegate.Add(Singleton<PopupManager>.GetInstance().openAlert(prefab.Result).GetComponent<FriendApplicationResultPopup>().OK.onClick, new EventDelegate.Callback(friend0086Menu.\u003CopenPopup00815\u003Eb__85_0));
  }
}
