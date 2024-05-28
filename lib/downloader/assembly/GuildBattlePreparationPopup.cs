// Decompiled with JetBrains decompiler
// Type: GuildBattlePreparationPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class GuildBattlePreparationPopup : MonoBehaviour
{
  [SerializeField]
  private GameObject dir_guild_battle_stage_entry;
  private GuildBattleSortiePopup sortiePopup;
  [SerializeField]
  private GameObject dir_guild_battle_guest_select;
  private GuildAtkGuestSelectPopup guestSelectPopup;
  private GvgDeck deckInfo;
  private Guild0282Menu guild0282Menu;
  private GuildBattlePreparationPopup.MODE mode;

  public GuildBattlePreparationPopup.MODE Mode => this.mode;

  public IEnumerator InitializeAsync(
    Guild0282Menu menu,
    string targetPlayerID,
    int testSlotNo,
    Action success = null)
  {
    GuildBattlePreparationPopup parent = this;
    parent.guild0282Menu = menu;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    bool maintenance = false;
    Future<WebAPI.Response.GvgDeckReinforcementCandidates> ft = WebAPI.GvgDeckReinforcementCandidates(false, (Action<WebAPI.Response.UserError>) (e =>
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
      bool isSuccess = false;
      e1 = GuildUtil.UpdateGuildDeckAttack(PlayerAffiliation.Current.guild_id, Player.Current.id, (Action) (() => isSuccess = true));
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (isSuccess)
      {
        parent.deckInfo = GuildUtil.gvgDeckAttack;
        parent.sortiePopup = parent.dir_guild_battle_stage_entry.GetComponent<GuildBattleSortiePopup>();
        if (Object.op_Inequality((Object) parent.sortiePopup, (Object) null))
        {
          e1 = parent.sortiePopup.InitializeAsync(menu, parent, targetPlayerID, testSlotNo);
          while (e1.MoveNext())
            yield return e1.Current;
          e1 = (IEnumerator) null;
        }
        parent.guestSelectPopup = parent.dir_guild_battle_guest_select.GetComponent<GuildAtkGuestSelectPopup>();
        if (Object.op_Inequality((Object) parent.guestSelectPopup, (Object) null))
        {
          e1 = parent.guestSelectPopup.InitializeAsync(menu, parent, parent.sortiePopup, ft.Result.candidates);
          while (e1.MoveNext())
            yield return e1.Current;
          e1 = (IEnumerator) null;
        }
        if (success != null)
          success();
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      }
    }
  }

  public void ShowGuestSelect()
  {
    this.dir_guild_battle_stage_entry.SetActive(false);
    this.dir_guild_battle_guest_select.SetActive(true);
    this.guestSelectPopup.InitScrollPosition();
    this.guestSelectPopup.IsPush = false;
    this.guestSelectPopup.SetGvgPopup();
    this.mode = GuildBattlePreparationPopup.MODE.Guest;
  }

  public void ShowSortie()
  {
    this.dir_guild_battle_guest_select.SetActive(false);
    this.dir_guild_battle_stage_entry.SetActive(true);
    this.sortiePopup.IsPush = false;
    this.sortiePopup.SetGvgPopup();
    this.mode = GuildBattlePreparationPopup.MODE.Sortie;
  }

  public enum MODE
  {
    Sortie,
    Guest,
  }
}
