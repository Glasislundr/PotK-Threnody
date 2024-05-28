// Decompiled with JetBrains decompiler
// Type: Guild0283Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Guild0283Menu : BackButtonMenuBase
{
  [SerializeField]
  private NGxScroll ngxScroll;
  [SerializeField]
  private UILabel menuTitle;
  [SerializeField]
  private UIButton buttonTitle;
  [SerializeField]
  private UIButton buttonSetting;
  [SerializeField]
  private UIButton buttonApplicants;
  [SerializeField]
  private UILabel lblUnavailableApplicants;
  [SerializeField]
  private UIButton buttonLeave;
  [SerializeField]
  private UILabel lblUnavailableLeave;
  [SerializeField]
  private UIButton buttonDismiss;
  [SerializeField]
  private UILabel lblUnavailableDismiss;
  [SerializeField]
  private UIButton buttonDefenseMember;
  [SerializeField]
  private UILabel lblUnavailableDefenseMember;
  [SerializeField]
  private GameObject applicantBadge;
  [SerializeField]
  private GameObject titleBadge;
  [SerializeField]
  private UIButton buttonResearch;
  [SerializeField]
  private GameObject buttonResearchInBattleText;
  private GameObject guildSettingPopup;
  private GameObject guildSettingConfirmPopup;
  private GameObject guildBrakeupUnavailablePopup;
  private GameObject guildBrakeupPopup;
  private GameObject guildBrakeupConfirmPopup;
  private GameObject guildResignUnavailablePopup;
  private GameObject guildResignPopup;
  private GameObject guildResignConfirmPopup;
  private GameObject guildNgWordPopup;
  private GameObject commonOkPopup;
  private GameObject defenseMemberSelectPopup;
  private GameObject memberPrefab;

  public GameObject GuildSettingPopup => this.guildSettingPopup;

  public GameObject GuildSettingConfirmPopup => this.guildSettingConfirmPopup;

  public GameObject GuildBreakupUnavailablePopup => this.guildBrakeupUnavailablePopup;

  public GameObject GuildBrakeupPopup => this.guildBrakeupPopup;

  public GameObject GuildBrakeUpConfirmPopup => this.guildBrakeupConfirmPopup;

  public GameObject GuildResignUnavailablePopup => this.guildResignUnavailablePopup;

  public GameObject GuildResignPopup => this.guildResignPopup;

  public GameObject GuildResignConfirmPopup => this.guildResignConfirmPopup;

  public GameObject GuildNgWordPopup => this.guildNgWordPopup;

  public GameObject CommonOkPupup => this.commonOkPopup;

  public GameObject DefenseMemberSelectPopup => this.defenseMemberSelectPopup;

  public GameObject MemberPrefab => this.memberPrefab;

  public IEnumerator InitializeAsync(WebAPI.Response.GuildTop guildTop)
  {
    this.menuTitle.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_028_3_MENU_TITLE));
    this.lblUnavailableApplicants.SetTextLocalize(Consts.GetInstance().POPUP_028_3_UNAVAILABLE);
    this.lblUnavailableLeave.SetTextLocalize(Consts.GetInstance().POPUP_028_3_UNAVAILABLE);
    this.lblUnavailableDismiss.SetTextLocalize(Consts.GetInstance().POPUP_028_3_UNAVAILABLE);
    this.lblUnavailableDefenseMember.SetTextLocalize(Consts.GetInstance().POPUP_028_3_UNAVAILABLE);
    IEnumerator e = this.ResourceLoad();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GuildRole? role = PlayerAffiliation.Current.role;
    GuildRole guildRole1 = GuildRole.master;
    int num1;
    if (!(role.GetValueOrDefault() == guildRole1 & role.HasValue))
    {
      role = PlayerAffiliation.Current.role;
      GuildRole guildRole2 = GuildRole.sub_master;
      num1 = role.GetValueOrDefault() == guildRole2 & role.HasValue ? 1 : 0;
    }
    else
      num1 = 1;
    bool flag1 = num1 != 0;
    role = PlayerAffiliation.Current.role;
    GuildRole guildRole3 = GuildRole.master;
    bool flag2 = role.GetValueOrDefault() == guildRole3 & role.HasValue;
    ((Component) ((Component) this.buttonSetting).transform.parent).gameObject.SetActive(flag1);
    ((Component) ((Component) this.buttonApplicants).transform.parent).gameObject.SetActive(flag1);
    ((Component) ((Component) this.buttonResearch).transform.parent).gameObject.SetActive(!flag1);
    ((Component) ((Component) this.buttonDefenseMember).transform.parent).gameObject.SetActive(flag2);
    if (flag1)
      this.applicantBadge.SetActive(Persist.guildSetting.Exists && GuildUtil.getBadgeState(GuildUtil.GuildBadgeInfoType.newApplicant));
    else
      this.applicantBadge.SetActive(false);
    GameObject gameObject = ((Component) ((Component) this.buttonDismiss).transform.parent).gameObject;
    role = PlayerAffiliation.Current.role;
    GuildRole guildRole4 = GuildRole.master;
    int num2 = role.GetValueOrDefault() == guildRole4 & role.HasValue ? 1 : 0;
    gameObject.SetActive(num2 != 0);
    this.titleBadge.SetActive(Persist.guildSetting.Exists && GuildUtil.getBadgeState(GuildUtil.GuildBadgeInfoType.newTitle));
    if (PlayerAffiliation.Current.onGvgOperation)
    {
      this.SetGuildDontNotMoveButton();
    }
    else
    {
      DateTime dateTime1 = ServerTime.NowAppTime();
      if (guildTop.raid_period != null)
      {
        DateTime? nullable = guildTop.raid_period.entry_end_at;
        DateTime dateTime2 = dateTime1;
        if ((nullable.HasValue ? (nullable.GetValueOrDefault() <= dateTime2 ? 1 : 0) : 0) != 0)
        {
          nullable = guildTop.raid_period.end_at;
          DateTime dateTime3 = dateTime1;
          if ((nullable.HasValue ? (nullable.GetValueOrDefault() >= dateTime3 ? 1 : 0) : 0) != 0)
            goto label_15;
        }
      }
      if (!guildTop.raid_aggregating)
        goto label_16;
label_15:
      this.SetGuildDontNotMoveButton();
    }
label_16:
    this.ngxScroll.ResolvePosition();
  }

  private void SetGuildDontNotMoveButton()
  {
    if (((Component) ((Component) this.buttonApplicants).transform.parent).gameObject.activeSelf)
    {
      ((UIButtonColor) this.buttonApplicants).isEnabled = false;
      ((Component) ((Component) this.lblUnavailableApplicants).transform.parent).gameObject.SetActive(true);
    }
    ((UIButtonColor) this.buttonLeave).isEnabled = false;
    ((Component) ((Component) this.lblUnavailableLeave).transform.parent).gameObject.SetActive(true);
    if (((Component) ((Component) this.buttonDismiss).transform.parent).gameObject.activeSelf)
    {
      ((UIButtonColor) this.buttonDismiss).isEnabled = false;
      ((Component) ((Component) this.lblUnavailableDismiss).transform.parent).gameObject.SetActive(true);
    }
    if (((Component) ((Component) this.buttonDefenseMember).transform.parent).gameObject.activeSelf)
    {
      ((UIButtonColor) this.buttonDefenseMember).isEnabled = false;
      ((Component) ((Component) this.lblUnavailableDefenseMember).transform.parent).gameObject.SetActive(true);
    }
    if (!((Component) ((Component) this.buttonResearch).transform.parent).gameObject.activeSelf)
      return;
    ((UIButtonColor) this.buttonResearch).isEnabled = false;
    this.buttonResearchInBattleText.SetActive(true);
  }

  public void Initialize()
  {
  }

  private IEnumerator ResourceLoad()
  {
    Future<GameObject> fgObj;
    IEnumerator e;
    if (Object.op_Equality((Object) this.guildSettingPopup, (Object) null))
    {
      fgObj = Res.Prefabs.popup.popup_028_guild_option_edit__anim_popup01.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildSettingPopup = fgObj.Result;
      fgObj = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.guildSettingConfirmPopup, (Object) null))
    {
      fgObj = Res.Prefabs.popup.popup_028_guild_option_edit_confirm__anim_popup01.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildSettingConfirmPopup = fgObj.Result;
      fgObj = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.guildBrakeupUnavailablePopup, (Object) null))
    {
      fgObj = Res.Prefabs.popup.popup_028_3_1__anim_popup01.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildBrakeupUnavailablePopup = fgObj.Result;
      this.InitWidgetAlpha((MonoBehaviour) this.guildBrakeupUnavailablePopup.GetComponent<Guild02831Popup>());
      fgObj = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.guildBrakeupPopup, (Object) null))
    {
      fgObj = Res.Prefabs.popup.popup_028_3_2_1__anim_popup01.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildBrakeupPopup = fgObj.Result;
      this.InitWidgetAlpha((MonoBehaviour) this.guildBrakeupPopup.GetComponent<Guild028321Popup>());
      fgObj = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.guildBrakeupConfirmPopup, (Object) null))
    {
      fgObj = Res.Prefabs.popup.popup_028_3_2_2__anim_popup01.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildBrakeupConfirmPopup = fgObj.Result;
      this.InitWidgetAlpha((MonoBehaviour) this.guildBrakeupConfirmPopup.GetComponent<Guild028322Popup>());
      fgObj = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.guildResignUnavailablePopup, (Object) null))
    {
      fgObj = Res.Prefabs.popup.popup_028_3_3__anim_popup01.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildResignUnavailablePopup = fgObj.Result;
      this.InitWidgetAlpha((MonoBehaviour) this.guildResignUnavailablePopup.GetComponent<Guild02833Popup>());
      fgObj = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.guildResignPopup, (Object) null))
    {
      fgObj = Res.Prefabs.popup.popup_028_3_4_1__anim_popup01.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildResignPopup = fgObj.Result;
      this.InitWidgetAlpha((MonoBehaviour) this.guildResignPopup.GetComponent<Guild028341Popup>());
      fgObj = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.guildResignConfirmPopup, (Object) null))
    {
      fgObj = Res.Prefabs.popup.popup_028_3_4_2__anim_popup01.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildResignConfirmPopup = fgObj.Result;
      this.InitWidgetAlpha((MonoBehaviour) this.guildResignConfirmPopup.GetComponent<Guild028342Popup>());
      fgObj = (Future<GameObject>) null;
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
      fgObj = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.commonOkPopup, (Object) null))
    {
      fgObj = Res.Prefabs.popup.popup_028_guild_common_ok__anim_popup01.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.commonOkPopup = fgObj.Result;
      fgObj = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.defenseMemberSelectPopup, (Object) null))
    {
      fgObj = Res.Prefabs.popup.popup_028_guild_defense_member_select__anim_popup01.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.defenseMemberSelectPopup = fgObj.Result;
      fgObj = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.memberPrefab, (Object) null))
    {
      fgObj = Res.Prefabs.guild.guild_member_list.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.memberPrefab = fgObj.Result;
      fgObj = (Future<GameObject>) null;
    }
  }

  private void InitWidgetAlpha(MonoBehaviour component)
  {
    if (!Object.op_Inequality((Object) ((Component) component).GetComponent<UIWidget>(), (Object) null))
      return;
    ((UIRect) ((Component) component).GetComponent<UIWidget>()).alpha = 0.0f;
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public void onGuildTitleButton() => Guild0284Scene.ChangeScene();

  private IEnumerator ShowGuildOption()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Guild0283Menu menu = this;
    Guild028OptionEditPopup optionEditPopup;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      optionEditPopup.SetPulldownEventCallback();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    GameObject prefab = menu.guildSettingPopup.Clone();
    optionEditPopup = prefab.GetComponent<Guild028OptionEditPopup>();
    prefab.SetActive(false);
    optionEditPopup.Initialize(menu);
    prefab.SetActive(true);
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void onGuildOptionButton() => this.StartCoroutine(this.ShowGuildOption());

  public void onMemberRequestCheckButton() => Guild0285Scene.ChangeScene();

  public void onGuildResignButton()
  {
    GuildRole? role = PlayerAffiliation.Current.role;
    GuildRole guildRole = GuildRole.master;
    if (role.GetValueOrDefault() == guildRole & role.HasValue)
      Singleton<PopupManager>.GetInstance().open(this.guildResignUnavailablePopup).GetComponent<Guild02833Popup>().Initialize();
    else
      Singleton<PopupManager>.GetInstance().open(this.guildResignPopup).GetComponent<Guild028341Popup>().Initialize(this);
  }

  public void onGuildBrakeupButton()
  {
    if (PlayerAffiliation.Current.guild.memberships.Length >= 2)
      Singleton<PopupManager>.GetInstance().open(this.guildBrakeupUnavailablePopup).GetComponent<Guild02831Popup>().Initialize();
    else
      Singleton<PopupManager>.GetInstance().open(this.guildBrakeupPopup).GetComponent<Guild028321Popup>().Initialize(this);
  }

  public void onGuildDefenseMember() => this.StartCoroutine(this.ShowDefenseMemberSelectList());

  private IEnumerator ShowDefenseMemberSelectList()
  {
    GameObject clone = this.DefenseMemberSelectPopup.Clone();
    clone.SetActive(false);
    IEnumerator e = clone.GetComponent<GuildDefenseMemberListPopup>().Initialize(this.memberPrefab, PlayerAffiliation.Current.guild);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject obj = Singleton<PopupManager>.GetInstance().open(clone, isCloned: true);
    while (!Object.op_Equality((Object) obj, (Object) null))
      yield return (object) null;
  }

  public void onGuildResearch()
  {
    if (this.IsPushAndSet())
      return;
    Guild02812Scene.ChangeScene();
  }

  public void showOkPopup(string title, string message, Action ok = null)
  {
    Singleton<PopupManager>.GetInstance().open(this.commonOkPopup).GetComponent<GuildOkPopup>().Initialize(title, message, ok: ok);
  }
}
