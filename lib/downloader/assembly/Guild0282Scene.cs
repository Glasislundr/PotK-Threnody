// Decompiled with JetBrains decompiler
// Type: Guild0282Scene
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
public class Guild0282Scene : NGSceneBase
{
  [SerializeField]
  private Guild0282Menu guild0282menu;
  private bool completeTestBattleBackProc;

  public static void ChangeScene()
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("guild028_2", Singleton<NGSceneManager>.GetInstance().sceneName != "guild028_2");
  }

  public static void ChangeSceneBattleFinish(string targetPlayerID, int captureStar, bool isTest)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("guild028_2", true, (object) targetPlayerID, (object) captureStar, (object) isTest);
  }

  public static void ChangeSceneOrMemberFocus(GuildMembership member, Guild0282Menu menu)
  {
    bool isStack = Singleton<NGSceneManager>.GetInstance().sceneName != "guild028_2";
    if (member == null)
      Singleton<NGSceneManager>.GetInstance().changeScene("guild028_2", isStack);
    else if (!((IEnumerable<GuildMembership>) PlayerAffiliation.Current.guild.memberships).Any<GuildMembership>((Func<GuildMembership, bool>) (x => x.player.player_id == member.player.player_id)) && (!Object.op_Inequality((Object) menu, (Object) null) || menu.EnGuildUI == null || !menu.EnGuildUI.memberBaseList.Any<Guild0282MemberBase>((Func<Guild0282MemberBase, bool>) (x => x.Member.player.player_id == member.player.player_id))))
    {
      Singleton<NGSceneManager>.GetInstance().changeScene("guild028_2", isStack);
    }
    else
    {
      if (Object.op_Equality((Object) menu, (Object) null))
      {
        menu = ((Component) Singleton<NGSceneManager>.GetInstance().sceneBase).GetComponent<Guild0282Menu>();
        if (Object.op_Equality((Object) menu, (Object) null))
        {
          Singleton<NGSceneManager>.GetInstance().changeScene("guild028_2", (isStack ? 1 : 0) != 0, (object) member);
          return;
        }
      }
      if (!menu.MyGuildUI.memberBaseList.Any<Guild0282MemberBase>((Func<Guild0282MemberBase, bool>) (x => x.Member.player.player_id == member.player.player_id)) && menu.EnGuildUI != null && !menu.EnGuildUI.memberBaseList.Any<Guild0282MemberBase>((Func<Guild0282MemberBase, bool>) (x => x.Member.player.player_id == member.player.player_id)))
        Singleton<NGSceneManager>.GetInstance().changeScene("guild028_2", (isStack ? 1 : 0) != 0, (object) member);
      else if (Singleton<NGSceneManager>.GetInstance().sceneName != "guild028_2")
      {
        Singleton<NGSceneManager>.GetInstance().changeScene("guild028_2", (isStack ? 1 : 0) != 0, (object) member);
      }
      else
      {
        menu.MemberBaseUpdate();
        menu.JumpMember(member);
      }
    }
  }

  public IEnumerator onStartSceneAsync()
  {
    IEnumerator e = this.guild0282menu.InitializeAsync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(GuildMembership member)
  {
    IEnumerator e = this.guild0282menu.InitializeAsync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(string targetPlayerID, int captureStar, bool isTest)
  {
    IEnumerator e = this.guild0282menu.InitializeAsync(true, true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene()
  {
    if (this.guild0282menu.isFailedInit)
      return;
    this.guild0282menu.Initialize();
    this.guild0282menu.InitializeJump();
    if (GuildUtil.gvgPopupState != GuildUtil.GvGPopupState.None && this.guild0282menu.actionForGvgPopup != null)
      this.guild0282menu.actionForGvgPopup();
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
  }

  public void onStartScene(GuildMembership member)
  {
    if (this.guild0282menu.isFailedInit)
      return;
    if (GuildUtil.gvgPopupState != GuildUtil.GvGPopupState.None && this.guild0282menu.actionForGvgPopup != null)
    {
      this.onStartScene();
    }
    else
    {
      this.guild0282menu.Initialize(member);
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    }
  }

  public void onStartScene(string targetPlayerID, int captureStar, bool isTest)
  {
    if (this.guild0282menu.isFailedInit)
      return;
    if (isTest)
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      if (!this.completeTestBattleBackProc)
      {
        if (GuildUtil.gvgPopupState != GuildUtil.GvGPopupState.None && this.guild0282menu.actionForGvgPopup != null)
          this.guild0282menu.actionForGvgPopup();
        int? nullable = ((IEnumerable<GuildMembership>) PlayerAffiliation.Current.guild.memberships).FirstIndexOrNull<GuildMembership>((Func<GuildMembership, bool>) (x => x.player.player_id == targetPlayerID));
        if (nullable.HasValue)
          this.guild0282menu.Initialize(PlayerAffiliation.Current.guild.memberships[nullable.Value]);
        else
          this.guild0282menu.Initialize();
        this.completeTestBattleBackProc = true;
        return;
      }
    }
    if (GuildUtil.gvgPopupState != GuildUtil.GvGPopupState.None && this.guild0282menu.actionForGvgPopup != null)
    {
      this.onStartScene();
    }
    else
    {
      if (isTest)
        return;
      int? nullable = ((IEnumerable<GuildMembership>) this.guild0282menu.EnGuild.memberships).FirstIndexOrNull<GuildMembership>((Func<GuildMembership, bool>) (x => x.player.player_id == targetPlayerID));
      if (nullable.HasValue && GuildUtil.isBattleOrPreparing(PlayerAffiliation.Current.guild.gvg_status))
      {
        this.guild0282menu.InitializeGBResult(this.guild0282menu.EnGuild.memberships[nullable.Value], captureStar);
      }
      else
      {
        this.guild0282menu.Initialize();
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      }
    }
  }

  public override void onEndScene() => this.guild0282menu.onEndScene();

  public override IEnumerator onEndSceneAsync()
  {
    Guild0282Scene guild0282Scene = this;
    float startTime = Time.time;
    while (!guild0282Scene.isTweenFinished && (double) Time.time - (double) startTime < (double) guild0282Scene.tweenTimeoutTime)
      yield return (object) null;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    guild0282Scene.isTweenFinished = true;
    yield return (object) null;
    // ISSUE: reference to a compiler-generated method
    yield return (object) guild0282Scene.\u003C\u003En__0();
  }

  public override IEnumerator onDestroySceneAsync()
  {
    GuildUtil.gvgPopupState = GuildUtil.GvGPopupState.None;
    GuildUtil.gvgDeckAttack = (GvgDeck) null;
    GuildUtil.gvgDeckDefense = (GvgDeck) null;
    GuildUtil.gvgFriendDefense = (GvgReinforcement) null;
    return base.onDestroySceneAsync();
  }
}
