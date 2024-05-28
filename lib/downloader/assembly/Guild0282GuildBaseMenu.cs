// Decompiled with JetBrains decompiler
// Type: Guild0282GuildBaseMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Guild0282GuildBaseMenu : GuildMapObject
{
  private GuildMemberObject guildMemberObj;
  private Guild0282Menu menu;
  [SerializeField]
  private bool isEnemy;
  private GuildRegistration guild;
  private GuildInfoPopup guildInfoPopup;
  private GameObject guildBattleRecordPopup;
  private GameObject guildBattleMemberScorePopup;

  public void Initialize(
    GuildRegistration guildData,
    GuildInfoPopup guildInfoData,
    GuildMemberObject memberObj,
    Guild0282Menu menu)
  {
    this.guild = guildData;
    this.guildInfoPopup = guildInfoData;
    this.guildMemberObj = memberObj;
    this.menu = menu;
  }

  public override void onBackButton()
  {
  }

  public void onButtonBattleLog()
  {
    Debug.Log((object) "OpenBattleLog");
    this.StartCoroutine(this.ShowBattleRecord());
  }

  public void onButtonGuildTop()
  {
    Singleton<PopupManager>.GetInstance().onDismiss();
    this.menu.BackScene();
  }

  public void onButtonMemberList()
  {
    Debug.Log((object) "OpemMemberList");
    this.StartCoroutine(this.ShowMemberList());
  }

  public void onButtonGuildAbout()
  {
    this.menu.isClosePopupByBackBtn = false;
    Singleton<PopupManager>.GetInstance().open(this.guildInfoPopup.guildInfoPopup).GetComponent<Guild028114Popup>().Initialize(this.guildInfoPopup, this.guild, this.isEnemy);
    Debug.Log((object) "OpemGuildAbout");
  }

  private IEnumerator ShowMemberList()
  {
    Guild0282GuildBaseMenu baseMenu = this;
    baseMenu.menu.isClosePopupByBackBtn = false;
    GameObject popup = baseMenu.guildMemberObj.GuildMemberListPopup.Clone();
    GuildMemberListPopup component = popup.GetComponent<GuildMemberListPopup>();
    popup.SetActive(false);
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = component.Initialize(baseMenu.isEnemy, baseMenu.menu, baseMenu, baseMenu.guildMemberObj, baseMenu.guild, new Action(baseMenu.\u003CShowMemberList\u003Eb__13_0));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
  }

  private IEnumerator ShowBattleRecord()
  {
    this.menu.isClosePopupByBackBtn = false;
    Future<GameObject> popupF;
    IEnumerator e;
    if (Object.op_Equality((Object) this.guildBattleRecordPopup, (Object) null))
    {
      popupF = Res.Prefabs.popup.popup_028_guild_battle_records__anim_popup01.Load<GameObject>();
      e = popupF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildBattleRecordPopup = popupF.Result;
      popupF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.guildBattleMemberScorePopup, (Object) null))
    {
      popupF = Res.Prefabs.popup.popup_028_guild_member_battle_records__anim_popup01.Load<GameObject>();
      e = popupF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildBattleMemberScorePopup = popupF.Result;
      popupF = (Future<GameObject>) null;
    }
    GameObject popup = this.guildBattleRecordPopup.Clone();
    popup.SetActive(false);
    GuildBattleRecordPopup script = popup.GetComponent<GuildBattleRecordPopup>();
    bool success = false;
    e = script.InitializeAsync(this.isEnemy, this.guild.guild_id, this.guildBattleMemberScorePopup, (Action) (() => success = true));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (success)
    {
      popup.SetActive(true);
      Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
      yield return (object) null;
      script.InitScrollPosition();
    }
    else
      Object.Destroy((Object) popup);
  }
}
