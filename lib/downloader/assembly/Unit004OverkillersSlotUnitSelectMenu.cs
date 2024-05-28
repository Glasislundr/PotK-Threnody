// Decompiled with JetBrains decompiler
// Type: Unit004OverkillersSlotUnitSelectMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using CustomDeck;
using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class Unit004OverkillersSlotUnitSelectMenu : UnitMenuBase
{
  private Unit004OverkillersSlotUnitSelectScene.Param param_;
  private EditOverkillersParam editParam_;
  private PlayerUnit selectedUnit_;
  private bool isBaseEquipped_;
  private HashSet<int> excludeAnyEquippedIds_;
  private HashSet<int> excludeSameCharacterIds_;
  private HashSet<int> otherSlotUnitIds_;
  private List<OverkillersGroup> myOverKillersGroupList;
  private GameObject StealAlertPopupPrefab;

  public IEnumerator initialize(
    Unit004OverkillersSlotUnitSelectScene.Param param,
    EditOverkillersParam editParam = null)
  {
    Unit004OverkillersSlotUnitSelectMenu slotUnitSelectMenu1 = this;
    slotUnitSelectMenu1.param_ = param;
    slotUnitSelectMenu1.editParam_ = editParam;
    if (editParam != null)
    {
      slotUnitSelectMenu1.isEditCustomDeck = true;
      slotUnitSelectMenu1.customDeck = Singleton<NGGameDataManager>.GetInstance().currentCustomDeck;
    }
    else
    {
      slotUnitSelectMenu1.isEditCustomDeck = false;
      slotUnitSelectMenu1.customDeck = (PlayerCustomDeck) null;
    }
    EditOverkillersParam overkillersParam = editParam;
    int num = overkillersParam != null ? overkillersParam.slotNo : param.slotNo;
    Unit004OverkillersSlotUnitSelectMenu slotUnitSelectMenu2 = slotUnitSelectMenu1;
    PlayerUnit baseUnit = editParam?.baseUnit;
    if ((object) baseUnit == null)
      baseUnit = param.baseUnit;
    slotUnitSelectMenu2.baseUnit = baseUnit;
    slotUnitSelectMenu1.selectedUnit_ = editParam == null ? param.selectUnit : editParam.selected;
    int id = slotUnitSelectMenu1.baseUnit.id;
    int selected_id = 0;
    UnitUnit unit = slotUnitSelectMenu1.baseUnit.unit;
    PlayerUnit[] playerUnitArray = editParam?.units ?? SMManager.Get<PlayerUnit[]>();
    List<PlayerUnit> lst = new List<PlayerUnit>(playerUnitArray.Length / 5);
    if (editParam != null)
    {
      if (editParam.selected != (PlayerUnit) null)
      {
        lst.Add(editParam.selected);
        selected_id = editParam.selected.id;
      }
    }
    else if (param.selectUnit != (PlayerUnit) null)
    {
      lst.Add(param.selectUnit);
      selected_id = param.selectUnit.id;
    }
    slotUnitSelectMenu1.myOverKillersGroupList = new List<OverkillersGroup>();
    foreach (OverkillersGroup overkillersGroup in MasterData.OverkillersGroupList)
    {
      foreach (int parentUnitId in overkillersGroup.parent_unit_ids)
      {
        if (parentUnitId == unit.ID)
          slotUnitSelectMenu1.myOverKillersGroupList.Add(overkillersGroup);
      }
    }
    slotUnitSelectMenu1.isBaseEquipped_ = editParam == null && slotUnitSelectMenu1.baseUnit.overkillers_base_id > 0;
    slotUnitSelectMenu1.excludeAnyEquippedIds_ = new HashSet<int>();
    if (editParam != null)
    {
      for (int index = 0; index < playerUnitArray.Length; ++index)
      {
        PlayerUnit playerUnit = playerUnitArray[index];
        if (id != playerUnit.id && (selected_id == 0 || selected_id != playerUnit.id) && slotUnitSelectMenu1.isEquipPossible(unit, playerUnit.unit))
        {
          if (!((IEnumerable<int>) slotUnitSelectMenu1.baseUnit.over_killers_player_unit_ids).Contains<int>(playerUnit.id) && editParam.dicReference.ContainsKey(playerUnit.id))
            slotUnitSelectMenu1.excludeAnyEquippedIds_.Add(playerUnit.id);
          lst.Add(playerUnit);
        }
      }
    }
    else
    {
      for (int index = 0; index < playerUnitArray.Length; ++index)
      {
        PlayerUnit playerUnit = playerUnitArray[index];
        if (id != playerUnit.id && (selected_id == 0 || selected_id != playerUnit.id) && slotUnitSelectMenu1.isEquipPossible(unit, playerUnit.unit))
        {
          if (playerUnit.isAnyOverkillersUnits)
            slotUnitSelectMenu1.excludeAnyEquippedIds_.Add(playerUnit.id);
          lst.Add(playerUnit);
        }
      }
    }
    slotUnitSelectMenu1.excludeSameCharacterIds_ = new HashSet<int>();
    slotUnitSelectMenu1.otherSlotUnitIds_ = new HashSet<int>();
    if (slotUnitSelectMenu1.baseUnit.isAnyCacheOverkillersUnits)
    {
      for (int index = 0; index < slotUnitSelectMenu1.baseUnit.cache_overkillers_units.Length; ++index)
      {
        if (index != num && !(slotUnitSelectMenu1.baseUnit.cache_overkillers_units[index] == (PlayerUnit) null))
        {
          slotUnitSelectMenu1.excludeSameCharacterIds_.Add(slotUnitSelectMenu1.baseUnit.cache_overkillers_units[index].unit.same_character_id);
          slotUnitSelectMenu1.otherSlotUnitIds_.Add(slotUnitSelectMenu1.baseUnit.cache_overkillers_units[index].id);
        }
      }
    }
    slotUnitSelectMenu1.SetIconType(UnitMenuBase.IconType.Normal);
    yield return (object) slotUnitSelectMenu1.Initialize();
    slotUnitSelectMenu1.InitializeInfo((IEnumerable<PlayerUnit>) lst, (IEnumerable<PlayerMaterialUnit>) null, Persist.unit004OverkillersSlotUnitSortAndFilter, false, selected_id != 0, true, true, false);
    yield return (object) slotUnitSelectMenu1.CreateUnitIcon();
    slotUnitSelectMenu1.TxtNumber.SetTextLocalize(string.Format("{0}/{1}", (object) lst.Count, (object) Player.Current.max_units));
    slotUnitSelectMenu1.lastReferenceUnitID = -1;
    slotUnitSelectMenu1.InitializeEnd();
  }

  protected bool isEquipPossible(UnitUnit base_unit, UnitUnit target_unit)
  {
    if (base_unit.IsAllEquipUnit)
      return (target_unit.overkillers_parameter != 0 || target_unit.exist_overkillers_skill) && target_unit.character_UnitCharacter != base_unit.character_UnitCharacter && !((IEnumerable<IgnoreOverkillers>) MasterData.IgnoreOverkillersList).Any<IgnoreOverkillers>((Func<IgnoreOverkillers, bool>) (v => v.same_character_id == target_unit.same_character_id));
    if (OverkillersGroup.IsForAllUnits(target_unit.ID))
    {
      if (target_unit.same_character_id == base_unit.same_character_id || target_unit.overkillers_parameter == 0 && !target_unit.exist_overkillers_skill)
        return false;
    }
    else if (target_unit.character_UnitCharacter != base_unit.character_UnitCharacter || target_unit.same_character_id == base_unit.same_character_id || target_unit.overkillers_parameter == 0)
    {
      foreach (OverkillersGroup overKillersGroup in this.myOverKillersGroupList)
      {
        if (overKillersGroup.child_unit_id == target_unit.ID)
          return true;
      }
      return false;
    }
    return true;
  }

  protected override IEnumerator CreateUnitIcon(
    int info_index,
    int unit_index,
    PlayerUnit baseUnit)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Unit004OverkillersSlotUnitSelectMenu slotUnitSelectMenu = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      slotUnitSelectMenu.postCreateUnitIcon(slotUnitSelectMenu.displayUnitInfos[info_index], slotUnitSelectMenu.allUnitIcons[unit_index]);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated method
    this.\u003C\u003E2__current = (object) slotUnitSelectMenu.\u003C\u003En__0(info_index, unit_index, baseUnit);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  protected override void CreateUnitIconCache(int info_index, int unit_index, PlayerUnit baseUnit)
  {
    base.CreateUnitIconCache(info_index, unit_index, baseUnit);
    this.postCreateUnitIcon(this.displayUnitInfos[info_index], this.allUnitIcons[unit_index]);
  }

  private void postCreateUnitIcon(UnitIconInfo dispInfo, UnitIconBase unitIcon)
  {
    if (dispInfo.removeButton)
    {
      unitIcon.onClick = (Action<UnitIconBase>) (x => this.onRemoveUnit());
      unitIcon.Gray = false;
    }
    else
    {
      unitIcon.onLongPress = (Action<UnitIconBase>) (x => Unit0042Scene.changeOverkillersUnitDetail(this.baseUnit, x.PlayerUnit, this.getUnits()));
      unitIcon.CanAwake = false;
      if (this.selectedUnit_ != (PlayerUnit) null && this.selectedUnit_ == unitIcon.PlayerUnit)
      {
        unitIcon.onClick = (Action<UnitIconBase>) (x => this.IbtnBack());
        unitIcon.Gray = false;
        unitIcon.Overkillers = true;
      }
      else if (this.isBaseEquipped_)
      {
        unitIcon.onClick = (Action<UnitIconBase>) (x => { });
        unitIcon.Gray = true;
        unitIcon.Overkillers = false;
      }
      else if (this.isEditCustomDeck)
      {
        if (unitIcon.ForBattle)
        {
          unitIcon.onClick = (Action<UnitIconBase>) (_ => { });
          unitIcon.Gray = true;
          unitIcon.Overkillers = false;
        }
        else if (this.excludeAnyEquippedIds_.Contains(unitIcon.PlayerUnit.id))
        {
          if (!this.excludeSameCharacterIds_.Contains(unitIcon.PlayerUnit.unit.same_character_id))
          {
            unitIcon.onClick = (Action<UnitIconBase>) (x => this.onSetUnit(x));
            unitIcon.Gray = false;
          }
          else
          {
            unitIcon.onClick = (Action<UnitIconBase>) (x => { });
            unitIcon.Gray = true;
          }
          unitIcon.Overkillers = true;
        }
        else if (this.excludeSameCharacterIds_.Contains(unitIcon.PlayerUnit.unit.same_character_id))
        {
          unitIcon.onClick = (Action<UnitIconBase>) (x => { });
          unitIcon.Gray = true;
          unitIcon.Overkillers = this.otherSlotUnitIds_.Contains(unitIcon.PlayerUnit.id);
        }
        else
        {
          unitIcon.onClick = (Action<UnitIconBase>) (x => this.onSetUnit(x));
          unitIcon.Gray = false;
          unitIcon.Overkillers = false;
        }
      }
      else if (this.excludeAnyEquippedIds_.Contains(unitIcon.PlayerUnit.id))
      {
        if (!this.excludeSameCharacterIds_.Contains(unitIcon.PlayerUnit.unit.same_character_id))
        {
          unitIcon.onClick = (Action<UnitIconBase>) (x => this.onSetUnit(x));
          unitIcon.Gray = false;
        }
        else
        {
          unitIcon.onClick = (Action<UnitIconBase>) (x => { });
          unitIcon.Gray = true;
        }
        unitIcon.Overkillers = true;
      }
      else if (this.excludeSameCharacterIds_.Contains(unitIcon.PlayerUnit.unit.same_character_id))
      {
        unitIcon.onClick = (Action<UnitIconBase>) (x => { });
        unitIcon.Gray = true;
        unitIcon.Overkillers = this.otherSlotUnitIds_.Contains(unitIcon.PlayerUnit.id);
      }
      else if (unitIcon.PlayerUnit.overkillers_base_id > 0)
      {
        unitIcon.onClick = (Action<UnitIconBase>) (x => this.onSetUnit(x));
        unitIcon.Gray = false;
        unitIcon.Overkillers = true;
      }
      else
      {
        unitIcon.onClick = (Action<UnitIconBase>) (x => this.onSetUnit(x));
        unitIcon.Gray = false;
        unitIcon.Overkillers = false;
      }
      unitIcon.SetupDeckStatusBlink();
    }
  }

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }

  private void onSetCustomDeckOverkillers(PlayerUnit unit)
  {
    this.editParam_.onSetOverkillers(this.editParam_.index, this.editParam_.slotNo, (object) unit != null ? unit.id : 0);
    this.backScene();
  }

  private void onRemoveUnit()
  {
    if (this.IsPushAndSet())
      return;
    if (this.editParam_ != null)
      this.onSetCustomDeckOverkillers((PlayerUnit) null);
    else
      this.StartCoroutine(this.doRemoveUnit());
  }

  private IEnumerator doRemoveUnit()
  {
    Unit004OverkillersSlotUnitSelectMenu slotUnitSelectMenu = this;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<WebAPI.Response.UnitRemoveOverKillers> future = WebAPI.UnitRemoveOverKillers(slotUnitSelectMenu.param_.baseUnit.id, slotUnitSelectMenu.param_.slotNo, slotUnitSelectMenu.param_.selectUnit.id, new Action<WebAPI.Response.UserError>(slotUnitSelectMenu.errorWebAPI));
    yield return (object) future.Wait();
    if (future.Result != null)
    {
      yield return (object) GuildUtil.UpdateGuildDeck();
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      slotUnitSelectMenu.param_.actSelected((PlayerUnit) null);
      slotUnitSelectMenu.backScene();
    }
  }

  private void onSetUnit(UnitIconBase icon)
  {
    if (this.IsPushAndSet())
      return;
    if (this.editParam_ != null)
      this.onSetCustomDeckOverkillers(icon.PlayerUnit);
    else
      this.StartCoroutine(this.doSetUnit(icon.PlayerUnit));
  }

  private IEnumerator doSetUnit(PlayerUnit unit)
  {
    Unit004OverkillersSlotUnitSelectMenu slotUnitSelectMenu = this;
    if (slotUnitSelectMenu.checkOnceAlertOverkillersUnits())
    {
      int nSelect = 0;
      Consts instance = Consts.GetInstance();
      PopupCommonNoYes.Show(instance.POPUP_004_TITLE_ALERT_TEAMS_OVERKILLERS, instance.POPUP_004_MESSAGE_ALERT_TEAMS_OVERKILLERS, (Action) (() => nSelect = 1), (Action) (() => nSelect = 2), (NGUIText.Alignment) 1);
      while (nSelect == 0)
        yield return (object) null;
      if (nSelect == 2)
      {
        slotUnitSelectMenu.IsPush = false;
        yield break;
      }
    }
    PlayerUnit base_unit = (PlayerUnit) null;
    if (unit.overkillers_base_id > 0)
    {
      foreach (PlayerUnit playerUnit in SMManager.Get<PlayerUnit[]>())
      {
        if (playerUnit.id == unit.overkillers_base_id)
        {
          base_unit = playerUnit;
          break;
        }
      }
    }
    if (base_unit != (PlayerUnit) null)
    {
      if (Object.op_Equality((Object) slotUnitSelectMenu.StealAlertPopupPrefab, (Object) null))
      {
        Future<GameObject> StealAlertPopupPrefabF = new ResourceObject("Prefabs/popup/popup_004_overkillers_steal__anim_popup01").Load<GameObject>();
        IEnumerator e = StealAlertPopupPrefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        slotUnitSelectMenu.StealAlertPopupPrefab = StealAlertPopupPrefabF.Result;
        StealAlertPopupPrefabF = (Future<GameObject>) null;
      }
      int nSelect = 0;
      Singleton<PopupManager>.GetInstance().open(slotUnitSelectMenu.StealAlertPopupPrefab).GetComponent<Unit004OverKillersStealPopup>().Init(base_unit, (Action) (() => nSelect = 1), (Action) (() => nSelect = 2));
      while (nSelect == 0)
        yield return (object) null;
      if (nSelect == 2)
      {
        slotUnitSelectMenu.IsPush = false;
        yield break;
      }
    }
    else if (unit.isAnyOverkillersUnits)
    {
      int nSelect = 0;
      Consts instance = Consts.GetInstance();
      PopupCommonNoYes.Show(instance.POPUP_004_TITLE_ALERT_OVERKILLERS_ALL_REMOVE, instance.POPUP_004_MESSAGE_ALERT_OVERKILLERS_ALL_REMOVE, (Action) (() => nSelect = 1), (Action) (() => nSelect = 2), (NGUIText.Alignment) 1);
      while (nSelect == 0)
        yield return (object) null;
      if (nSelect == 2)
      {
        slotUnitSelectMenu.IsPush = false;
        yield break;
      }
    }
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<WebAPI.Response.UnitSaveOverKillers> future = WebAPI.UnitSaveOverKillers(slotUnitSelectMenu.param_.baseUnit.id, slotUnitSelectMenu.param_.slotNo, unit.id, new Action<WebAPI.Response.UserError>(slotUnitSelectMenu.errorWebAPI));
    yield return (object) future.Wait();
    if (future.Result != null)
    {
      yield return (object) GuildUtil.UpdateGuildDeckAll();
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      slotUnitSelectMenu.param_.actSelected(unit);
      slotUnitSelectMenu.backScene();
    }
  }

  private bool checkOnceAlertOverkillersUnits()
  {
    GuildRegistration guild = PlayerAffiliation.Current.guild;
    if (guild == null || string.IsNullOrEmpty(guild.guild_id))
      return false;
    Persist.GuildOverkillersAlertLog overkillersAlertLog;
    try
    {
      overkillersAlertLog = Persist.guildOverkillersAlertLog.Data;
    }
    catch
    {
      Persist.guildOverkillersAlertLog.Delete();
      overkillersAlertLog = new Persist.GuildOverkillersAlertLog();
      Persist.guildOverkillersAlertLog.Data = overkillersAlertLog;
    }
    string myId = Player.Current.id;
    if (Array.Find<GuildMembership>(guild.memberships, (Predicate<GuildMembership>) (x => x.player.player_id == myId)) == null)
      return false;
    int gvgId = guild.active_gvg_period_id.HasValue ? guild.active_gvg_period_id.Value : 0;
    if (overkillersAlertLog.guildID != guild.guild_id || overkillersAlertLog.gvgID != gvgId)
      overkillersAlertLog.reset(guild.guild_id, gvgId);
    int num = overkillersAlertLog.isAlertOverkillersUnits ? 1 : 0;
    if (num == 0)
      return num != 0;
    overkillersAlertLog.isAlertOverkillersUnits = false;
    Persist.guildOverkillersAlertLog.Flush();
    return num != 0;
  }

  private void errorWebAPI(WebAPI.Response.UserError e)
  {
    WebAPI.DefaultUserErrorCallback(e);
    MypageScene.ChangeSceneOnError();
  }
}
