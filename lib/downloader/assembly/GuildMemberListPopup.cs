// Decompiled with JetBrains decompiler
// Type: GuildMemberListPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class GuildMemberListPopup : GuildMemberListBase
{
  protected const int Width = 518;
  protected const int Height = 157;
  protected const int ColumnValue = 1;
  protected const int RowValue = 15;
  protected const int ScreenValue = 8;
  private bool isEnemy;
  [SerializeField]
  private UILabel popupTitle;
  [SerializeField]
  protected UILabel guildMemberNum;
  [SerializeField]
  private GameObject slc_title_base_one;
  [SerializeField]
  private GameObject slc_title_base_enemy;
  private Guild0282Menu guild282Menu;
  private Guild0282GuildBaseMenu guildBaseMenu;
  private GuildRegistration guildInfo;
  private GuildMemberObject guildMemberObjs;
  private Action actionAfterRoleChange;

  public IEnumerator Initialize(
    bool is_enemy,
    Guild0282Menu menu,
    Guild0282GuildBaseMenu baseMenu,
    GuildMemberObject popup,
    GuildRegistration guild,
    Action action = null)
  {
    IEnumerator e = this.CommonInit(is_enemy, menu, baseMenu, popup, guild, action);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator Initialize(GuildMemberObject popup, Action action = null)
  {
    IEnumerator e = this.CommonInit(false, (Guild0282Menu) null, (Guild0282GuildBaseMenu) null, popup, (GuildRegistration) null, action);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator CommonInit(
    bool is_enemy,
    Guild0282Menu menu,
    Guild0282GuildBaseMenu baseMenu,
    GuildMemberObject popup,
    GuildRegistration guild,
    Action action = null)
  {
    GuildMemberListPopup guildMemberListPopup = this;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    yield return (object) null;
    if (Object.op_Inequality((Object) ((Component) guildMemberListPopup).GetComponent<UIWidget>(), (Object) null))
      ((UIRect) ((Component) guildMemberListPopup).GetComponent<UIWidget>()).alpha = 0.0f;
    guildMemberListPopup.isEnemy = is_enemy;
    guildMemberListPopup.guildInfo = is_enemy ? guild : PlayerAffiliation.Current.guild;
    guildMemberListPopup.guild282Menu = menu;
    guildMemberListPopup.guildBaseMenu = baseMenu;
    guildMemberListPopup.guildMemberObjs = popup;
    guildMemberListPopup.actionAfterRoleChange = action;
    guildMemberListPopup.slc_title_base_one.SetActive(!guildMemberListPopup.isEnemy);
    guildMemberListPopup.slc_title_base_enemy.SetActive(guildMemberListPopup.isEnemy);
    guildMemberListPopup.popupTitle.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_GUILD_MEMBER_TITLE));
    IEnumerator e = guildMemberListPopup.InitMemberScroll(guildMemberListPopup.guildInfo.memberships);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = guildMemberListPopup.CreateSortPopup();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
  }

  protected virtual IEnumerator InitMemberScroll(GuildMembership[] members)
  {
    GuildMemberListPopup guildMemberListPopup = this;
    guildMemberListPopup.allMemberInfo.Clear();
    guildMemberListPopup.allMemberBar.Clear();
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    DateTime nowTime = ServerTime.NowAppTime();
    guildMemberListPopup.Initialize(nowTime, 518, 157, 15, 8);
    guildMemberListPopup.CreateMemberInfo(members);
    if (guildMemberListPopup.allMemberInfo.Count > 0)
    {
      e = guildMemberListPopup.CreateScrollBase(guildMemberListPopup.guildMemberObjs.GuildMemberListPrefab);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    guildMemberListPopup.scroll.ResolvePosition();
    guildMemberListPopup.scroll.scrollView.UpdatePosition();
    guildMemberListPopup.guildMemberNum.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_GUILD_MEMBER_NUMBER, (IDictionary) new Hashtable()
    {
      {
        (object) "now",
        (object) guildMemberListPopup.guildInfo.memberships.Length
      },
      {
        (object) "max",
        (object) PlayerAffiliation.Current.guild.appearance.membership_capacity
      }
    }));
    guildMemberListPopup.InitializeEnd();
    if ((double) GuildUtil.GuildMemberListPopupSlideValue > 0.0)
    {
      guildMemberListPopup.scroll.ResolvePosition(new Vector2(0.0f, GuildUtil.GuildMemberListPopupSlideValue));
      GuildUtil.GuildMemberListPopupSlideValue = 0.0f;
    }
  }

  public void SetScrollValue()
  {
    GuildUtil.GuildMemberListPopupSlideValue = this.scroll.scrollView.verticalScrollBar.value;
  }

  public override void onBackButton()
  {
    if (this.IsOpenSortPopup)
      return;
    if (Singleton<NGSceneManager>.GetInstance().sceneName.Equals("guild028_2") && Object.op_Inequality((Object) Singleton<NGSceneManager>.GetInstance().sceneBase, (Object) null) && Object.op_Inequality((Object) ((Component) Singleton<NGSceneManager>.GetInstance().sceneBase).GetComponent<Guild0282Menu>(), (Object) null))
      ((Component) Singleton<NGSceneManager>.GetInstance().sceneBase).GetComponent<Guild0282Menu>().isClosePopupByBackBtn = true;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }

  protected override IEnumerator CreateScroll(int info_index, int bar_index)
  {
    GuildMemberListPopup popup = this;
    Guild0282MemberScroll scroll = popup.allMemberBar[bar_index];
    MemberBarInfo memberBarInfo = popup.allMemberInfo[info_index];
    memberBarInfo.scroll = scroll;
    ((Component) scroll).gameObject.SetActive(true);
    IEnumerator e = scroll.Initialize(popup.isEnemy, popup, memberBarInfo.member, popup.guild282Menu, popup.guildBaseMenu, popup.guildMemberObjs, popup.actionAfterRoleChange);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((Component) scroll).gameObject.SetActive(true);
  }
}
