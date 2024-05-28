// Decompiled with JetBrains decompiler
// Type: Guild02811Menu
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
public class Guild02811Menu : BackButtonMenuBase
{
  private const int GUILD_LIST_MAX = 5;
  [SerializeField]
  private GameObject buildObj;
  [SerializeField]
  private UIButton buildObjButton;
  [SerializeField]
  private GameObject enoughLevel;
  [SerializeField]
  private UILabel enoughLevelLabel;
  [SerializeField]
  private UILabel enoughLevelShadowLabel;
  [SerializeField]
  private GameObject createLimit;
  [SerializeField]
  private NGxScroll scroll;
  private GameObject scrollObj;
  private ModalWindow nowPopup;
  private GameObject searchSettingPopup;
  private GameObject searchNotFoundPopup;
  private GameObject buildSettingPopup;
  private GameObject guildSerachFriendListPopup;
  private GameObject buildSettingCheckPopup;
  private bool beforeBestSearch;
  private GameObject buildEffectPopup;
  private GuildInfoPopup guildPopup;
  private GuildSetting serchSetting = new GuildSetting();
  private List<GuildDirectory> guildList = new List<GuildDirectory>();
  private GameObject friendPartsObj;
  private GameObject guildNgWordPopup;

  public GameObject BuildSettingCheckPopup => this.buildSettingCheckPopup;

  public GameObject BuildEffectPopup => this.buildEffectPopup;

  public GuildInfoPopup GuildPopup => this.guildPopup;

  public GameObject GuildNgWordPopup => this.guildNgWordPopup;

  public void Setting(GuildSetting data) => this.serchSetting = data;

  public IEnumerator InitializeAsync()
  {
    Guild02811Menu guild02811Menu = this;
    guild02811Menu.guildPopup = new GuildInfoPopup();
    // ISSUE: reference to a compiler-generated method
    guild02811Menu.guildPopup.SetSendRequestCallback(new Action(guild02811Menu.\u003CInitializeAsync\u003Eb__31_0));
    // ISSUE: reference to a compiler-generated method
    guild02811Menu.guildPopup.SetCancelRequestCallback(new Action(guild02811Menu.\u003CInitializeAsync\u003Eb__31_1));
    IEnumerator e = guild02811Menu.guildPopup.ResourceLoad();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = guild02811Menu.ResourceLoad();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator OnStartAsync()
  {
    this.serchSetting = new GuildSetting();
    this.guildList = new List<GuildDirectory>();
    IEnumerator e = this.SearchBestGuild();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (SM.GuildSignal.Current.existGuildEventRelationship(GuildEventType.leave_membership))
    {
      e = this.UpdateHelpers();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public void Initialize() => this.DrawGuildList();

  private void CheckStatus()
  {
    int num = 40;
    int? nullable = ((IEnumerable<MasterDataTable.GuildSetting>) MasterData.GuildSettingList).FirstIndexOrNull<MasterDataTable.GuildSetting>((Func<MasterDataTable.GuildSetting, bool>) (x => x.ID == 1));
    if (nullable.HasValue)
    {
      int? intValue = MasterData.GuildSettingList[nullable.Value].GetIntValue();
      if (intValue.HasValue)
      {
        intValue = MasterData.GuildSettingList[nullable.Value].GetIntValue();
        num = intValue.Value;
      }
    }
    this.enoughLevelLabel.SetTextLocalize(Consts.Format(Consts.GetInstance().Guild0281MENU_ENOUGH_LEVEL, (IDictionary) new Hashtable()
    {
      {
        (object) "enoughLevel",
        (object) num
      }
    }));
    this.enoughLevelShadowLabel.SetTextLocalize(Consts.Format(Consts.GetInstance().Guild0281MENU_ENOUGH_LEVEL, (IDictionary) new Hashtable()
    {
      {
        (object) "enoughLevel",
        (object) num
      }
    }));
    ((UIButtonColor) this.buildObjButton).isEnabled = Player.Current.level >= num;
    this.enoughLevel.SetActive(Player.Current.level < num);
    this.createLimit.SetActive(false);
    switch (PlayerAffiliation.Current.status)
    {
      case GuildMembershipStatus.applicant:
        ((UIButtonColor) this.buildObjButton).isEnabled = false;
        if (!SM.GuildSignal.Current.existGuildEventRelationship(GuildEventType.leave_membership))
          break;
        this.nowPopup = ModalWindow.Show(Consts.GetInstance().GUILD_LEAVE_TITLE, Consts.GetInstance().GUILD_LEAVE_MESSAGE, (Action) (() =>
        {
          Object.DestroyObject((Object) this.nowPopup);
          this.nowPopup = (ModalWindow) null;
          SM.GuildSignal.Current.removeRelationshipEvent(GuildEventType.leave_membership);
        }));
        break;
      case GuildMembershipStatus.membership:
        ((UIButtonColor) this.buildObjButton).isEnabled = false;
        if (!SM.GuildSignal.Current.existPlayershipEventType(GuildEventType.apply_applicant) || !Object.op_Equality((Object) this.nowPopup, (Object) null))
          break;
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        this.nowPopup = ModalWindow.Show(Consts.GetInstance().GUILD_APPLY_APPLICANT_TITLE, Consts.GetInstance().GUILD_APPLY_APPLICANT_MESSAGE, (Action) (() =>
        {
          Object.DestroyObject((Object) this.nowPopup);
          this.nowPopup = (ModalWindow) null;
          MypageScene.ChangeScene(MypageRootMenu.Mode.GUILD, true);
        }));
        break;
      case GuildMembershipStatus.withdraw:
        ((UIButtonColor) this.buildObjButton).isEnabled = false;
        this.createLimit.SetActive(!this.enoughLevel.activeSelf);
        if (!SM.GuildSignal.Current.existGuildEventRelationship(GuildEventType.leave_membership))
          break;
        this.nowPopup = ModalWindow.Show(Consts.GetInstance().GUILD_LEAVE_TITLE, Consts.GetInstance().GUILD_LEAVE_MESSAGE, (Action) (() =>
        {
          Object.DestroyObject((Object) this.nowPopup);
          this.nowPopup = (ModalWindow) null;
          SM.GuildSignal.Current.removeRelationshipEvent(GuildEventType.leave_membership);
        }));
        break;
    }
  }

  private IEnumerator ResourceLoad()
  {
    Future<GameObject> fgObj = (Future<GameObject>) null;
    IEnumerator e;
    if (Object.op_Equality((Object) this.scrollObj, (Object) null))
    {
      fgObj = Res.Prefabs.guild028_1_1.dir_guild_list.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.scrollObj = fgObj.Result;
    }
    if (Object.op_Equality((Object) this.searchSettingPopup, (Object) null))
    {
      fgObj = Res.Prefabs.popup.popup_028_1_1__anim_popup01.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.searchSettingPopup = fgObj.Result;
    }
    if (Object.op_Equality((Object) this.searchNotFoundPopup, (Object) null))
    {
      fgObj = Res.Prefabs.popup.popup_028_1_2__anim_popup01.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.searchNotFoundPopup = fgObj.Result;
    }
    if (Object.op_Equality((Object) this.buildSettingPopup, (Object) null))
    {
      fgObj = Res.Prefabs.popup.popup_028_1_3__anim_popup01.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.buildSettingPopup = fgObj.Result;
    }
    if (Object.op_Equality((Object) this.buildSettingCheckPopup, (Object) null))
    {
      fgObj = Res.Prefabs.popup.popup_028_1_4__anim_popup01.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.buildSettingCheckPopup = fgObj.Result;
    }
    if (Object.op_Equality((Object) this.buildEffectPopup, (Object) null))
    {
      fgObj = Res.Prefabs.popup.guild_establishment_anim.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.buildEffectPopup = fgObj.Result;
    }
    if (Object.op_Equality((Object) this.guildSerachFriendListPopup, (Object) null))
    {
      fgObj = Res.Prefabs.popup.popup_028_guild_search_friend_list__anim_popup01.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildSerachFriendListPopup = fgObj.Result;
    }
    if (Object.op_Equality((Object) this.guildNgWordPopup, (Object) null))
    {
      fgObj = Res.Prefabs.popup.popup_028_guild_ng_word__anim_popup01.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildNgWordPopup = fgObj.Result;
      this.InitWidgetAlpha((MonoBehaviour) this.guildNgWordPopup.GetComponent<Guild028NgWordPopup>());
    }
  }

  public IEnumerator SearchGuild(Action callback = null)
  {
    Guild02811Menu guild02811menu = this;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    guild02811menu.beforeBestSearch = false;
    // ISSUE: reference to a compiler-generated method
    Future<WebAPI.Response.GuildSearch> searchGuild = WebAPI.GuildSearch(guild02811menu.serchSetting.approvalID, guild02811menu.serchSetting.atmosphereID, guild02811menu.serchSetting.autoApprovalID, guild02811menu.serchSetting.autokickID, guild02811menu.serchSetting.availabilityID, guild02811menu.serchSetting.guildName, 5, new Action<WebAPI.Response.UserError>(guild02811menu.\u003CSearchGuild\u003Eb__36_0));
    IEnumerator e = searchGuild.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (searchGuild.Result != null)
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      guild02811menu.guildList.Clear();
      guild02811menu.guildList = (List<GuildDirectory>) null;
      guild02811menu.guildList = ((IEnumerable<GuildDirectory>) searchGuild.Result.guilds).ToList<GuildDirectory>();
      if (guild02811menu.guildList.Count == 0)
        Singleton<PopupManager>.GetInstance().open(guild02811menu.searchNotFoundPopup).GetComponent<Guild028201Popup>().Initialize(guild02811menu);
      if (callback != null)
        callback();
    }
  }

  public IEnumerator SearchBestGuild(Action callback = null)
  {
    Guild02811Menu guild02811menu = this;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    guild02811menu.beforeBestSearch = true;
    // ISSUE: reference to a compiler-generated method
    Future<WebAPI.Response.GuildRecommends> searchGuild = WebAPI.GuildRecommends(true, 5, false, new Action<WebAPI.Response.UserError>(guild02811menu.\u003CSearchBestGuild\u003Eb__37_0));
    IEnumerator e = searchGuild.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (searchGuild.Result != null)
    {
      guild02811menu.guildList.Clear();
      guild02811menu.guildList = (List<GuildDirectory>) null;
      guild02811menu.guildList = ((IEnumerable<GuildDirectory>) searchGuild.Result.guilds).ToList<GuildDirectory>();
      if (guild02811menu.guildList.Count == 0)
        Singleton<PopupManager>.GetInstance().open(guild02811menu.searchNotFoundPopup).GetComponent<Guild028201Popup>().Initialize(guild02811menu);
      if (callback != null)
        callback();
    }
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
  }

  public void DrawGuildList()
  {
    this.CheckStatus();
    this.scroll.Clear();
    this.scroll.Reset();
    GameObject obj = Object.Instantiate<GameObject>(this.buildObj);
    obj.SetActive(true);
    this.scroll.Add(obj);
    int? nullable = this.guildList.FirstIndexOrNull<GuildDirectory>((Func<GuildDirectory, bool>) (x => x.guild_id == PlayerAffiliation.Current.guild_id));
    if (nullable.HasValue)
    {
      obj = Object.Instantiate<GameObject>(this.scrollObj);
      Guild02811Scroll component = obj.GetComponent<Guild02811Scroll>();
      if (Object.op_Inequality((Object) component, (Object) null))
      {
        GuildDirectory guild = this.guildList[nullable.Value];
        this.StartCoroutine(component.Initialize(guild, this.guildPopup, true));
      }
      this.scroll.Add(obj);
    }
    this.guildList.ForEach((Action<GuildDirectory>) (x =>
    {
      if (x.guild_id == PlayerAffiliation.Current.guild_id)
        return;
      bool IsApply = x.guild_id == PlayerAffiliation.Current.applicant_guild_id;
      obj = Object.Instantiate<GameObject>(this.scrollObj);
      Guild02811Scroll component = obj.GetComponent<Guild02811Scroll>();
      if (Object.op_Inequality((Object) component, (Object) null))
        this.StartCoroutine(component.Initialize(x, this.guildPopup, IsApply));
      this.scroll.Add(obj);
    }));
    this.scroll.ResolvePosition();
  }

  public void UpdateApplyGuildLst()
  {
    this.scroll.GridChildren().ForEach<GameObject>((Action<GameObject>) (x =>
    {
      Guild02811Scroll component = x.GetComponent<Guild02811Scroll>();
      if (!Object.op_Inequality((Object) component, (Object) null))
        return;
      component.UpdateApply(PlayerAffiliation.Current.guild.guild_id);
    }));
  }

  public IEnumerator FriendList()
  {
    Guild02811Menu menu = this;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    IEnumerator e;
    if (Object.op_Equality((Object) menu.friendPartsObj, (Object) null))
    {
      Future<GameObject> fgObj = Res.Prefabs.guild.guild_search_friend_list.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      menu.friendPartsObj = fgObj.Result;
      fgObj = (Future<GameObject>) null;
    }
    Future<WebAPI.Response.GuildFriendAffiliations> api = WebAPI.GuildFriendAffiliations();
    e = api.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (SMManager.Get<PlayerFriend[]>() == null)
    {
      e = WebAPI.FriendFriends().Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(menu.guildSerachFriendListPopup).GetComponent<Guild02811FriendListPopup>().Initialize(menu, menu.friendPartsObj, api.Result, ServerTime.NowAppTime());
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
  }

  public void BuildGuildPopupOpen()
  {
    Singleton<PopupManager>.GetInstance().open(this.buildSettingPopup).GetComponent<Guild028301Popup>().Initialize(this);
  }

  public void BuildGuildPopupOpen(GuildSetting setting)
  {
    this.StartCoroutine(Singleton<PopupManager>.GetInstance().open(this.buildSettingPopup).GetComponent<Guild028301Popup>().Initialize(this, setting));
  }

  public void onButtonSetting()
  {
    Singleton<PopupManager>.GetInstance().open(this.searchSettingPopup).GetComponent<Guild028101Popup>().Initialize(this, this.serchSetting);
  }

  public void onButtonUpdate()
  {
    if (this.beforeBestSearch)
      this.StartCoroutine(this.SearchBestGuild(new Action(this.DrawGuildList)));
    else
      this.StartCoroutine(this.GuildListUpdate());
  }

  public void onButtonBuild() => this.BuildGuildPopupOpen();

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
    Singleton<NGSceneManager>.GetInstance().clearStack();
    this.backScene();
  }

  private IEnumerator GuildListUpdate()
  {
    IEnumerator e = this.SearchGuild();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.DrawGuildList();
  }

  private IEnumerator UpdateHelpers()
  {
    IEnumerator e1 = WebAPI.PlayerHelpers(0, (Action<WebAPI.Response.UserError>) (e =>
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    })).Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
  }

  private void InitWidgetAlpha(MonoBehaviour component)
  {
    if (!Object.op_Inequality((Object) ((Component) component).GetComponent<UIWidget>(), (Object) null))
      return;
    ((UIRect) ((Component) component).GetComponent<UIWidget>()).alpha = 0.0f;
  }
}
