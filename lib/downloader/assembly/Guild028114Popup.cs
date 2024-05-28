// Decompiled with JetBrains decompiler
// Type: Guild028114Popup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Guild028114Popup : BackButtonMenuBase
{
  [SerializeField]
  private Transform guildBasePos;
  private GameObject guildBasePrefab;
  private Guild0282GuildBase guildBase;
  private GuildInfoPopup guildPopup;
  private GuildRegistration guildRegistration;
  private GuildDirectory guildDirectory;
  [SerializeField]
  private UILabel guildStatement;
  [SerializeField]
  private UILabel guildMasterLabel;
  [SerializeField]
  private UILabel guildMasterName;
  [SerializeField]
  private UILabel guildJoinCondition1;
  [SerializeField]
  private UILabel guildJoinCondition2;
  [SerializeField]
  private UILabel guildJoinCondition3;
  [SerializeField]
  private UILabel guildJoinCondition4;
  [SerializeField]
  private UI2DSprite guildTitleImage;
  [SerializeField]
  private UILabel guildName;
  [SerializeField]
  private UILabel guildRank;
  [SerializeField]
  private UILabel nextExp;
  [SerializeField]
  private UILabel currentMember;
  [SerializeField]
  private UILabel maxMember;
  [SerializeField]
  private UILabel txt_guild_results_win;
  [SerializeField]
  private UILabel win;
  [SerializeField]
  private UILabel txt_guild_results;
  [SerializeField]
  private UILabel battleCount;
  [SerializeField]
  private UIButton btnSendRequest;
  [SerializeField]
  private UIButton btnCancelRequest;
  [SerializeField]
  private UIButton btnStatement;
  [SerializeField]
  private UISprite gaugeExp;
  [SerializeField]
  private UISprite guildLv_10;
  [SerializeField]
  private UISprite guildLv_1;
  [SerializeField]
  private UIScrollView scrollView;
  [SerializeField]
  private GameObject dir_frame_own;
  [SerializeField]
  private GameObject dir_frame_enemy;
  private GuildImageCache guildImageCache;
  private bool isEnemy;
  private bool fromChangeGuild;

  public void Initialize(GuildInfoPopup popup, GuildRegistration registration = null, bool isEnemy = false)
  {
    this.guildPopup = popup;
    this.guildDirectory = (GuildDirectory) null;
    this.isEnemy = isEnemy;
    this.StartCoroutine(this.SetGuildData(registration == null ? PlayerAffiliation.Current.guild : registration));
    this.SetButton(popup);
    this.initPanelAlpha();
  }

  public void Initialize(GuildDirectory guildData, GuildInfoPopup popup, bool changeGuild = false)
  {
    this.fromChangeGuild = changeGuild;
    this.guildPopup = popup;
    this.guildRegistration = (GuildRegistration) null;
    this.StartCoroutine(this.SetGuildData(guildData));
    this.SetButton(popup);
    this.initPanelAlpha();
  }

  private void initPanelAlpha()
  {
    UIWidget component = ((Component) this).GetComponent<UIWidget>();
    if (!Object.op_Inequality((Object) component, (Object) null))
      return;
    ((UIRect) component).alpha = 0.0f;
  }

  private void SetButton(GuildInfoPopup popup)
  {
    if (this.fromChangeGuild)
    {
      if (PlayerAffiliation.Current.status == GuildMembershipStatus.membership && PlayerAffiliation.Current.applicant_guild_id == this.guildDirectory.guild_id)
      {
        ((Component) this.btnSendRequest).gameObject.SetActive(false);
        ((Component) this.btnCancelRequest).gameObject.SetActive(true);
      }
      else
      {
        ((Component) this.btnSendRequest).gameObject.SetActive(true);
        ((Component) this.btnCancelRequest).gameObject.SetActive(false);
      }
      if (PlayerAffiliation.Current.guild_id == this.guildDirectory.guild_id)
        ((UIButtonColor) this.btnSendRequest).isEnabled = false;
      else
        ((UIButtonColor) this.btnSendRequest).isEnabled = true;
      ((Component) this.btnStatement).gameObject.SetActive(false);
    }
    else
    {
      if (this.guildDirectory != null)
      {
        if (PlayerAffiliation.Current.isGuildMember())
        {
          ((Component) this.btnSendRequest).gameObject.SetActive(false);
          ((Component) this.btnCancelRequest).gameObject.SetActive(false);
        }
        else if (PlayerAffiliation.Current.status == GuildMembershipStatus.applicant && (PlayerAffiliation.Current.guild_id == this.guildDirectory.guild_id || PlayerAffiliation.Current.applicant_guild_id == this.guildDirectory.guild_id))
        {
          ((Component) this.btnSendRequest).gameObject.SetActive(false);
          ((Component) this.btnCancelRequest).gameObject.SetActive(true);
        }
        else
        {
          ((Component) this.btnSendRequest).gameObject.SetActive(true);
          ((Component) this.btnCancelRequest).gameObject.SetActive(false);
        }
      }
      else
      {
        ((Component) this.btnSendRequest).gameObject.SetActive(false);
        ((Component) this.btnCancelRequest).gameObject.SetActive(false);
      }
      if (this.guildDirectory == null && !this.isEnemy)
      {
        GuildRole? role1 = PlayerAffiliation.Current.role;
        GuildRole guildRole1 = GuildRole.master;
        if (!(role1.GetValueOrDefault() == guildRole1 & role1.HasValue))
        {
          GuildRole? role2 = PlayerAffiliation.Current.role;
          GuildRole guildRole2 = GuildRole.sub_master;
          if (!(role2.GetValueOrDefault() == guildRole2 & role2.HasValue))
          {
            ((Component) this.btnStatement).gameObject.SetActive(false);
            return;
          }
        }
        ((Component) this.btnStatement).gameObject.SetActive(true);
      }
      else
        ((Component) this.btnStatement).gameObject.SetActive(false);
    }
  }

  private IEnumerator SetGuildData(GuildRegistration data)
  {
    this.guildImageCache = new GuildImageCache();
    this.guildRegistration = data;
    Future<Sprite> futureGuildTitleImage = EmblemUtility.LoadGuildEmblemSprite(this.guildRegistration.appearance._current_emblem);
    IEnumerator e = futureGuildTitleImage.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.guildTitleImage.sprite2D = futureGuildTitleImage.Result;
    if (Object.op_Equality((Object) this.guildBasePrefab, (Object) null))
    {
      Future<GameObject> fgObj = Res.Prefabs.guild028_2.GuildBase.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildBasePrefab = fgObj.Result;
      e = this.guildImageCache.ResourceLoad(data.appearance);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      fgObj = (Future<GameObject>) null;
    }
    this.dir_frame_own.SetActive(!this.isEnemy);
    this.dir_frame_enemy.SetActive(this.isEnemy);
    this.guildName.SetTextLocalize(this.guildRegistration.guild_name);
    this.SetCondition(this.guildRegistration.approval_policy, this.guildRegistration.atmosphere, this.guildRegistration.auto_approval, this.guildRegistration.auto_kick);
    this.SetGuildData(this.guildRegistration.appearance, this.guildRegistration.guild_id);
    yield return (object) null;
    this.scrollView.ResetPosition();
    if (Object.op_Inequality((Object) ((Component) this.guildStatement).GetComponent<BoxCollider>(), (Object) null))
    {
      ((Component) this.guildStatement).GetComponent<BoxCollider>().size = new Vector3((float) ((UIWidget) this.guildStatement).width, (float) ((UIWidget) this.guildStatement).height, 0.0f);
      ((Component) this.guildStatement).GetComponent<BoxCollider>().center = new Vector3(0.0f, (float) -((UIWidget) this.guildStatement).height * 0.5f, 0.0f);
    }
  }

  private IEnumerator SetGuildData(GuildDirectory data)
  {
    this.guildImageCache = new GuildImageCache();
    this.guildDirectory = data;
    Future<Sprite> futureGuildTitleImage = EmblemUtility.LoadGuildEmblemSprite(this.guildDirectory.appearance._current_emblem);
    IEnumerator e = futureGuildTitleImage.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.guildTitleImage.sprite2D = futureGuildTitleImage.Result;
    if (Object.op_Equality((Object) this.guildBasePrefab, (Object) null))
    {
      Future<GameObject> fgObj = Res.Prefabs.guild028_2.GuildBase.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildBasePrefab = fgObj.Result;
      e = this.guildImageCache.ResourceLoad(data.appearance);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      fgObj = (Future<GameObject>) null;
    }
    this.guildName.SetTextLocalize(this.guildDirectory.guild_name);
    this.SetCondition(this.guildDirectory.approval_policy, this.guildDirectory.atmosphere, this.guildDirectory.auto_approval, this.guildDirectory.auto_kick);
    this.SetGuildData(this.guildDirectory.appearance, this.guildDirectory.guild_id);
    yield return (object) null;
    this.scrollView.ResetPosition();
    if (Object.op_Inequality((Object) ((Component) this.guildStatement).GetComponent<BoxCollider>(), (Object) null))
    {
      ((Component) this.guildStatement).GetComponent<BoxCollider>().size = new Vector3((float) ((UIWidget) this.guildStatement).width, (float) ((UIWidget) this.guildStatement).height, 0.0f);
      ((Component) this.guildStatement).GetComponent<BoxCollider>().center = new Vector3(0.0f, (float) -((UIWidget) this.guildStatement).height * 0.5f, 0.0f);
    }
  }

  public IEnumerator ResetScrollPosition()
  {
    yield return (object) null;
    this.scrollView.ResetPosition();
  }

  private void SetGuildData(GuildAppearance data, string guildID)
  {
    if (Object.op_Inequality((Object) this.guildBase, (Object) null))
      Object.Destroy((Object) this.guildBase);
    this.guildBase = this.guildBasePrefab.Clone(((Component) this.guildBasePos).transform).GetComponent<Guild0282GuildBase>();
    ((Collider) ((Component) this.guildBase).GetComponent<BoxCollider>()).enabled = false;
    this.guildBase.SetEnemy(this.isEnemy);
    this.guildBase.SetSprite(data, this.guildImageCache);
    this.guildMasterName.SetTextLocalize(data.master_player_name);
    this.guildMasterLabel.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_028_1_1_4_GUILDMASTER_LABEL));
    this.guildRank.SetTextLocalize(Consts.Format(Consts.GetInstance().Guild0281MENU_RANKING, (IDictionary) new Hashtable()
    {
      {
        (object) "ranking",
        (object) data.ranking_no
      }
    }));
    this.nextExp.SetTextLocalize(Consts.Format(Consts.GetInstance().Guild0281MENU_NEXTEXP, (IDictionary) new Hashtable()
    {
      {
        (object) "nextExp",
        (object) data.experience_next
      }
    }));
    if (data.level < 10)
    {
      ((Component) this.guildLv_1).gameObject.SetActive(false);
      ((Component) this.guildLv_10).gameObject.SetActive(true);
      this.guildLv_10.SetSprite(string.Format("slc_text_glv_number{0}.png__GUI__guild_common_other__guild_common_other_prefab", (object) data.level));
    }
    else
    {
      ((Component) this.guildLv_1).gameObject.SetActive(true);
      ((Component) this.guildLv_10).gameObject.SetActive(true);
      this.guildLv_1.SetSprite(string.Format("slc_text_glv_number{0}.png__GUI__guild_common_other__guild_common_other_prefab", (object) (data.level % 10)));
      this.guildLv_10.SetSprite(string.Format("slc_text_glv_number{0}.png__GUI__guild_common_other__guild_common_other_prefab", (object) (data.level / 10)));
    }
    float num = 0.0f;
    if (data.experience_next > 0)
      num = (float) data.experience / (float) (data.experience + data.experience_next);
    ((Component) this.gaugeExp).transform.localScale = new Vector3(num, 1f, 1f);
    this.currentMember.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_028_1_1_4_CURRENT_MEMBER, (IDictionary) new Hashtable()
    {
      {
        (object) "currentMember",
        (object) data.membership_num
      }
    }));
    this.maxMember.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_028_1_1_4_MAX_MEMBER, (IDictionary) new Hashtable()
    {
      {
        (object) "maxMember",
        (object) data.membership_capacity
      }
    }));
    this.battleCount.SetTextLocalize(data.win_num + data.lose_num + data.draw_num);
    this.win.SetTextLocalize(data.win_num);
    this.txt_guild_results_win.SetTextLocalize(Consts.GetInstance().POPUP_028_1_1_4_TXT_WIN_NUM);
    this.txt_guild_results.SetTextLocalize(Consts.GetInstance().POPUP_028_1_1_4_TXT_MATCH_NUM);
    this.guildStatement.SetTextLocalize(data.broadcast_message);
  }

  private void SetCondition(
    GuildApprovalPolicy app_pol,
    GuildAtmosphere atmos,
    GuildAutoApproval auto_app,
    GuildAutokick auto_kick)
  {
    string text1 = app_pol == null ? Consts.GetInstance().GUILD_SETTING_CONDITIONS_NULL : app_pol.name;
    string text2 = atmos == null ? Consts.GetInstance().GUILD_SETTING_CONDITIONS_NULL : atmos.name;
    string text3 = auto_app == null ? Consts.GetInstance().GUILD_SETTING_CONDITIONS_NULL : auto_app.name;
    string str = auto_kick == null ? Consts.GetInstance().GUILD_SETTING_CONDITIONS_NULL : auto_kick.name;
    this.guildJoinCondition1.SetTextLocalize(text1);
    this.guildJoinCondition2.SetTextLocalize(text2);
    this.guildJoinCondition3.SetTextLocalize(text3);
    this.guildJoinCondition4.SetTextLocalize(Consts.GetInstance().POPUP_GUILD_OPTION_EDIT_AUTO_KICK + " : " + str);
  }

  public override void onBackButton()
  {
    if (Singleton<NGSceneManager>.GetInstance().sceneName.Equals("guild028_2") && Object.op_Inequality((Object) Singleton<NGSceneManager>.GetInstance().sceneBase, (Object) null) && Object.op_Inequality((Object) ((Component) Singleton<NGSceneManager>.GetInstance().sceneBase).GetComponent<Guild0282Menu>(), (Object) null))
      ((Component) Singleton<NGSceneManager>.GetInstance().sceneBase).GetComponent<Guild0282Menu>().isClosePopupByBackBtn = true;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public void onButtonSendRequest()
  {
    GameObject gameObject = Singleton<PopupManager>.GetInstance().open(this.guildPopup.guildSendRequestPopup);
    if (Object.op_Inequality((Object) ((Component) gameObject.GetComponent<Guild028115Popup>()).GetComponent<UIWidget>(), (Object) null))
      ((UIRect) ((Component) gameObject.GetComponent<Guild028115Popup>()).GetComponent<UIWidget>()).alpha = 0.0f;
    if (this.guildDirectory == null)
      gameObject.GetComponent<Guild028115Popup>().Initialize(this.guildRegistration, this.guildPopup, this.fromChangeGuild);
    else
      gameObject.GetComponent<Guild028115Popup>().Initialize(this.guildDirectory, this.guildPopup, this.fromChangeGuild);
  }

  public void onButtonCancelRequest()
  {
    GameObject gameObject = Singleton<PopupManager>.GetInstance().open(this.guildPopup.guildCancelRequestPopup);
    if (Object.op_Inequality((Object) ((Component) gameObject.GetComponent<Guild028116Popup>()).GetComponent<UIWidget>(), (Object) null))
      ((UIRect) ((Component) gameObject.GetComponent<Guild028116Popup>()).GetComponent<UIWidget>()).alpha = 0.0f;
    if (this.guildDirectory == null)
      gameObject.GetComponent<Guild028116Popup>().Initialize(this.guildRegistration, this.guildPopup, this.fromChangeGuild);
    else
      gameObject.GetComponent<Guild028116Popup>().Initialize(this.guildDirectory, this.guildPopup, this.fromChangeGuild);
  }

  public void onButtonEditStatement() => this.StartCoroutine(this.ShowGuildStatementPopup());

  private IEnumerator ShowGuildStatementPopup()
  {
    if (this.guildDirectory == null)
    {
      GameObject prefab = this.guildPopup.guildStatementPopup.Clone();
      Guild028117Popup popupScript = prefab.GetComponent<Guild028117Popup>();
      prefab.SetActive(false);
      popupScript.Initialize(this.guildPopup);
      prefab.SetActive(true);
      Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
      yield return (object) null;
      popupScript.ResetScrollPosition();
      popupScript.UpdateDrawScrollViewAnchor();
      popupScript.UpdateDragScrollViewBoxCollider();
      popupScript = (Guild028117Popup) null;
    }
    else
      Singleton<PopupManager>.GetInstance().open(this.guildPopup.guildStatementPopup).GetComponent<Guild028117Popup>().Initialize(this.guildDirectory, this.guildPopup);
  }
}
