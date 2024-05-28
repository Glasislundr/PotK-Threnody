// Decompiled with JetBrains decompiler
// Type: GuildDefTeamPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/Guild/Popup/DefTeam")]
public class GuildDefTeamPopup : MonoBehaviour
{
  private bool isEnemy;
  [SerializeField]
  private GameObject dir_guild_DEFteam_teamformation;
  private GuildDefTeamEditPopup teamEditPopup;
  [SerializeField]
  private GameObject dir_guild_battle_guest_select;
  private GuildDefGuestSelectPopup guestSelectPopup;
  private GuildDefTeamPopup.MODE mode;

  public GuildDefTeamPopup.MODE Mode => this.mode;

  public IEnumerator InitializeAsync(
    bool isEnemy,
    Guild0282Menu menu,
    GuildMembership info,
    Action success = null)
  {
    GuildDefTeamPopup parent = this;
    parent.isEnemy = isEnemy;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    bool isSuccess = false;
    IEnumerator e = GuildUtil.UpdateGuildDeckDefanse(isEnemy ? menu.EnGuild.guild_id : menu.MyGuild.guild_id, info.player.player_id, (Action) (() => isSuccess = true));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (isSuccess)
    {
      parent.teamEditPopup = parent.dir_guild_DEFteam_teamformation.GetComponent<GuildDefTeamEditPopup>();
      if (Object.op_Inequality((Object) parent.teamEditPopup, (Object) null))
      {
        e = parent.teamEditPopup.InitializeAsync(menu, parent, isEnemy, info);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      parent.guestSelectPopup = parent.dir_guild_battle_guest_select.GetComponent<GuildDefGuestSelectPopup>();
      if (Object.op_Inequality((Object) parent.guestSelectPopup, (Object) null))
      {
        e = parent.guestSelectPopup.InitializeAsync(parent, parent.teamEditPopup);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      parent.ShowTeamEdit();
      if (success != null)
        success();
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    }
  }

  public IEnumerator ShowGuestSelect()
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    bool maintenance = false;
    Future<WebAPI.Response.GvgDeckReinforcementCandidates> ft = WebAPI.GvgDeckReinforcementCandidates(true, (Action<WebAPI.Response.UserError>) (e =>
    {
      if (e.Code.Equals("GLD014"))
        maintenance = true;
      WebAPI.DefaultUserErrorCallback(e);
    }));
    IEnumerator e1 = ft.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (maintenance)
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      yield return (object) null;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      MypageScene.ChangeSceneOnError();
    }
    else if (ft.Result != null)
    {
      this.mode = GuildDefTeamPopup.MODE.Guest;
      this.dir_guild_DEFteam_teamformation.SetActive(false);
      this.dir_guild_battle_guest_select.SetActive(true);
      if (ft.Result.candidates != null && ft.Result.candidates.Length != 0)
      {
        foreach (GvgCandidate candidate in ft.Result.candidates)
        {
          candidate.player_unit.primary_equipped_gear = candidate.player_unit.FindEquippedGear(candidate.player_gears);
          candidate.player_unit.primary_equipped_gear2 = candidate.player_unit.FindEquippedGear2(candidate.player_gears);
          candidate.player_unit.primary_equipped_gear3 = candidate.player_unit.FindEquippedGear3(candidate.player_gears);
          candidate.player_unit.primary_equipped_reisou = candidate.player_unit.FindEquippedReisou(candidate.player_gears, candidate.player_reisou_gears);
          candidate.player_unit.primary_equipped_reisou2 = candidate.player_unit.FindEquippedReisou2(candidate.player_gears, candidate.player_reisou_gears);
          candidate.player_unit.primary_equipped_reisou3 = candidate.player_unit.FindEquippedReisou3(candidate.player_gears, candidate.player_reisou_gears);
          candidate.player_unit.primary_equipped_awake_skill = candidate.player_unit.FindEquippedExtraSkill(candidate.player_awake_skills);
        }
      }
      e1 = this.guestSelectPopup.CreateFriendList(ft.Result.candidates);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      this.guestSelectPopup.InitScrollPosition();
      this.guestSelectPopup.IsPush = false;
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    }
  }

  public void ShowTeamEdit()
  {
    this.mode = GuildDefTeamPopup.MODE.Edit;
    this.dir_guild_battle_guest_select.SetActive(false);
    this.dir_guild_DEFteam_teamformation.SetActive(true);
    this.teamEditPopup.IsPush = false;
  }

  public enum MODE
  {
    Edit,
    Guest,
  }
}
