// Decompiled with JetBrains decompiler
// Type: Tower029TeamEditMenu
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
public class Tower029TeamEditMenu : UnitSelectMenuBase
{
  [SerializeField]
  private UILabel lblAlibeUnitNum;
  [SerializeField]
  private UILabel lblSelectedUnitNum;
  [SerializeField]
  private UIButton btnDecide;
  private GameObject comfirmPopup;
  private GameObject goHpGauge;
  private List<TowerDeckUnit> selectedUnitInfo;
  private PlayerUnit[] playerUnits;
  private TowerProgress progress;
  private HashSet<int> excludeOverkillersIds;

  private IEnumerator ResourceLoad()
  {
    if (Object.op_Equality((Object) this.comfirmPopup, (Object) null))
    {
      Future<GameObject> f = Res.Prefabs.popup.popup_029_tower_team_confirm__anim_popup01.Load<GameObject>();
      IEnumerator e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.comfirmPopup = f.Result;
      f = (Future<GameObject>) null;
    }
  }

  private IEnumerator EditTeam()
  {
    Tower029TeamEditMenu tower029TeamEditMenu = this;
    Singleton<PopupManager>.GetInstance().dismiss();
    yield return (object) null;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<WebAPI.Response.TowerDeckEdit> f = WebAPI.TowerDeckEdit(tower029TeamEditMenu.selectedUnitIcons.OrderBy<UnitIconInfo, int>((Func<UnitIconInfo, int>) (x => x.select)).Select<UnitIconInfo, int>((Func<UnitIconInfo, int>) (x => x.playerUnit.id)).ToArray<int>(), tower029TeamEditMenu.progress.tower_id, (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
    IEnumerator e1 = f.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (f.Result == null)
    {
      TowerUtil.GotoMypage();
    }
    else
    {
      TowerUtil.towerDeckUnits = ((IEnumerable<TowerDeckUnit>) f.Result.tower_deck.tower_deck_units).OrderBy<TowerDeckUnit, int>((Func<TowerDeckUnit, int>) (u => u.position_id)).ToArray<TowerDeckUnit>();
      tower029TeamEditMenu.backScene();
    }
  }

  private IEnumerator ShowComfirmPopup()
  {
    Tower029TeamEditMenu tower029TeamEditMenu = this;
    GameObject popup = tower029TeamEditMenu.comfirmPopup.Clone();
    popup.SetActive(false);
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = popup.GetComponent<TowerTeamEditComfirmPopup>().InitializeAsync(tower029TeamEditMenu.selectedUnitIcons.OrderBy<UnitIconInfo, int>((Func<UnitIconInfo, int>) (x => x.select)).ToList<UnitIconInfo>(), tower029TeamEditMenu.unitPrefab, tower029TeamEditMenu.goHpGauge, new Action(tower029TeamEditMenu.\u003CShowComfirmPopup\u003Eb__11_1));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
  }

  private void SetIconGray()
  {
    if (this.displayUnitInfos == null)
      return;
    foreach (UnitIconInfo target in this.displayUnitInfos.Where<UnitIconInfo>((Func<UnitIconInfo, bool>) (i => i.select < 0 && this.checkGrayStatus(i))))
      this.setGrayStatus(target);
  }

  private bool checkGrayStatus(UnitIconInfo target) => this.checkGrayStatus(target.playerUnit);

  private bool checkGrayStatus(PlayerUnit target)
  {
    if (!(target != (PlayerUnit) null))
      return false;
    return (double) target.tower_hitpoint_rate <= 0.0 || this.excludeOverkillersIds.Contains(target.id);
  }

  private void setGrayStatus(UnitIconInfo target, bool bGray = true)
  {
    target.gray = bGray;
    if (!Object.op_Inequality((Object) target.icon, (Object) null))
      return;
    target.icon.Gray = bGray;
  }

  private TowerDeckUnit CreateTowerDeckUnit(int player_unit_id, int position_id)
  {
    return new TowerDeckUnit()
    {
      position_id = position_id,
      player_unit_id = player_unit_id
    };
  }

  private void SetUnitIconGray(int unit_index)
  {
    if (unit_index >= this.allUnitIcons.Count || Object.op_Equality((Object) this.allUnitIcons[unit_index], (Object) null))
      return;
    UnitIcon allUnitIcon = (UnitIcon) this.allUnitIcons[unit_index];
    if (!allUnitIcon.Selected && this.checkGrayStatus(allUnitIcon.PlayerUnit))
      allUnitIcon.Gray = true;
    if (Object.op_Equality((Object) allUnitIcon.HpGauge, (Object) null))
      return;
    allUnitIcon.HpGauge.SetGaugeAndDropoutIcon(allUnitIcon.PlayerUnit.TowerHp, allUnitIcon.PlayerUnit.total_hp, false);
  }

  protected bool updateExcludeOverkillers()
  {
    return this.updateExcludeOverkillers(this.selectedUnitIcons.Select<UnitIconInfo, PlayerUnit>((Func<UnitIconInfo, PlayerUnit>) (x => x.playerUnit)).ToArray<PlayerUnit>());
  }

  protected bool updateExcludeOverkillers(PlayerUnit[] selectedUnits)
  {
    HashSet<int> excludeOverkillersIds = this.excludeOverkillersIds;
    this.excludeOverkillersIds = new HashSet<int>();
    if (selectedUnits != null)
    {
      for (int index1 = 0; index1 < selectedUnits.Length; ++index1)
      {
        PlayerUnit selectedUnit = selectedUnits[index1];
        if (!(selectedUnit == (PlayerUnit) null))
        {
          selectedUnit.resetOnceOverkillers();
          if (selectedUnit.isAnyCacheOverkillersUnits)
          {
            for (int index2 = 0; index2 < selectedUnit.cache_overkillers_units.Length; ++index2)
            {
              if (selectedUnit.cache_overkillers_units[index2] != (PlayerUnit) null)
                this.excludeOverkillersIds.Add(selectedUnit.cache_overkillers_units[index2].id);
            }
          }
          else
          {
            int overkillersBaseId;
            if ((overkillersBaseId = selectedUnit.overkillers_base_id) > 0)
              this.excludeOverkillersIds.Add(overkillersBaseId);
          }
        }
      }
    }
    if (excludeOverkillersIds == null)
      return this.excludeOverkillersIds.Count > 0;
    return excludeOverkillersIds.Count != this.excludeOverkillersIds.Count || !excludeOverkillersIds.Equals((object) this.excludeOverkillersIds);
  }

  public IEnumerator InitializeAsync(TowerProgress progress)
  {
    Tower029TeamEditMenu tower029TeamEditMenu = this;
    tower029TeamEditMenu.playerUnits = ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).Where<PlayerUnit>((Func<PlayerUnit, bool>) (u => u.tower_is_entry)).ToArray<PlayerUnit>();
    IEnumerator e;
    if (tower029TeamEditMenu.isInitialize)
    {
      e = tower029TeamEditMenu.UpdateInfoAndScroll(tower029TeamEditMenu.playerUnits);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      tower029TeamEditMenu.SetIconGray();
    }
    else
    {
      tower029TeamEditMenu.progress = progress;
      tower029TeamEditMenu.selectedUnitInfo = new List<TowerDeckUnit>();
      if (TowerUtil.towerDeckUnits != null)
      {
        foreach (TowerDeckUnit towerDeckUnit in TowerUtil.towerDeckUnits)
        {
          TowerDeckUnit deckUnit = towerDeckUnit;
          if (((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).FirstOrDefault<PlayerUnit>((Func<PlayerUnit, bool>) (u => u != (PlayerUnit) null && u.id == deckUnit.player_unit_id)) != (PlayerUnit) null)
            tower029TeamEditMenu.selectedUnitInfo.Add(tower029TeamEditMenu.CreateTowerDeckUnit(deckUnit.player_unit_id, deckUnit.position_id));
        }
      }
      e = tower029TeamEditMenu.ResourceLoad();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = tower029TeamEditMenu.Initialize();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      tower029TeamEditMenu.SetIconType(UnitMenuBase.IconType.NormalWithHpGauge);
      tower029TeamEditMenu.InitializeInfo((IEnumerable<PlayerUnit>) tower029TeamEditMenu.playerUnits, (IEnumerable<PlayerMaterialUnit>) null, Persist.tower029UnitListSortAndFilter, false, false, true, true, true, new Action(tower029TeamEditMenu.InitializeAllUnitInfosExtend));
      e = tower029TeamEditMenu.CreateUnitIcon();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      tower029TeamEditMenu.lblSelectedUnitNum.SetTextLocalize(Consts.GetInstance().TOWER_TEAM_EDIT_SELECTED_UNIT);
      tower029TeamEditMenu.TxtNumber.SetTextLocalize(string.Format("{0}/{1}", (object) tower029TeamEditMenu.selectedUnitIcons.Count, (object) tower029TeamEditMenu.SelectMax));
      tower029TeamEditMenu.lblAlibeUnitNum.SetTextLocalize(Consts.GetInstance().TOWER_TEAM_EDIT_ALIVE_UNIT);
      tower029TeamEditMenu.TxtNumberpossession.SetTextLocalize(string.Format("{0}/{1}", (object) ((IEnumerable<PlayerUnit>) tower029TeamEditMenu.playerUnits).Count<PlayerUnit>((Func<PlayerUnit, bool>) (u => (double) u.TowerHpRate > 0.0)), (object) tower029TeamEditMenu.playerUnits.Length));
      tower029TeamEditMenu.InitializeEnd();
      tower029TeamEditMenu.SetIconGray();
    }
  }

  public void InitializeAllUnitInfosExtend()
  {
    this.selectedUnitIcons.Clear();
    foreach (UnitIconInfo allUnitInfo in this.allUnitInfos)
    {
      UnitIconInfo info = allUnitInfo;
      TowerDeckUnit towerDeckUnit = this.selectedUnitInfo.FirstOrDefault<TowerDeckUnit>((Func<TowerDeckUnit, bool>) (x => x.player_unit_id == info.playerUnit.id));
      if (towerDeckUnit != null)
      {
        info.gray = true;
        if ((double) info.playerUnit.tower_hitpoint_rate > 0.0)
        {
          info.select = towerDeckUnit.position_id - 1;
          this.selectedUnitIcons.Add(info);
        }
      }
    }
    foreach (UnitIconInfo unitIconInfo in this.allUnitInfos.Where<UnitIconInfo>((Func<UnitIconInfo, bool>) (x => x.playerUnit.isAnyOverkillersUnits || x.playerUnit.overkillers_base_id > 0)))
      unitIconInfo.is_overkillers = true;
    this.updateExcludeOverkillers();
    this.updateDecideButton();
  }

  public override void UpdateInfomation()
  {
    this.TxtNumber.SetTextLocalize(string.Format("{0}/{1}", (object) this.selectedUnitIcons.Count, (object) this.SelectMax));
  }

  public override void IbtnYes()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.ShowComfirmPopup());
  }

  public new void IbtnClearS()
  {
    if (this.IsPush)
      return;
    foreach (UnitIconBase allUnitIcon in this.allUnitIcons)
    {
      if (Object.op_Inequality((Object) allUnitIcon, (Object) null) && allUnitIcon.PlayerUnit != (PlayerUnit) null)
      {
        UnitIconInfo unitInfoAll = this.GetUnitInfoAll(allUnitIcon.PlayerUnit);
        bool flag = unitInfoAll != null && unitInfoAll.button_enable;
        this.Deselect(allUnitIcon);
        if (((Behaviour) allUnitIcon.Button).enabled && flag)
          allUnitIcon.Gray = this.checkGrayStatus(allUnitIcon.PlayerUnit);
      }
    }
    this.allUnitInfos.ForEach((Action<UnitIconInfo>) (v => v.select = -1));
    this.displayUnitInfos.ForEach((Action<UnitIconInfo>) (v => v.select = -1));
    this.selectedUnitIcons.Clear();
    this.UpdateInfomation();
    ((UIButtonColor) this.btnDecide).isEnabled = false;
    this.selectedUnitInfo.Clear();
    this.updateExcludeOverkillers();
    this.updateGrayStatusAll();
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  protected override IEnumerator CreateUnitIconBase(GameObject prefab)
  {
    Tower029TeamEditMenu tower029TeamEditMenu = this;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = tower029TeamEditMenu.\u003C\u003En__0(prefab);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Equality((Object) tower029TeamEditMenu.goHpGauge, (Object) null))
    {
      Future<GameObject> f = Res.Prefabs.tower.dir_hp_gauge.Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      tower029TeamEditMenu.goHpGauge = f.Result;
      f = (Future<GameObject>) null;
    }
    if (Object.op_Inequality((Object) tower029TeamEditMenu.goHpGauge, (Object) null))
    {
      for (int index = 0; index < tower029TeamEditMenu.allUnitIcons.Count; ++index)
      {
        UnitIcon allUnitIcon = (UnitIcon) tower029TeamEditMenu.allUnitIcons[index];
        tower029TeamEditMenu.goHpGauge.Clone(allUnitIcon.hp_gauge.transform);
        if (allUnitIcon.PlayerUnit != (PlayerUnit) null)
          allUnitIcon.HpGauge.SetGaugeAndDropoutIcon(allUnitIcon.PlayerUnit.TowerHp, allUnitIcon.PlayerUnit.total_hp, false);
      }
    }
  }

  protected override IEnumerator CreateUnitIcon(
    int info_index,
    int unit_index,
    PlayerUnit baseUnit = null)
  {
    IEnumerator e = base.CreateUnitIcon(info_index, unit_index, baseUnit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.resetIconCommon(info_index, unit_index);
  }

  protected override void CreateUnitIconCache(int info_index, int unit_index, PlayerUnit baseUnit = null)
  {
    base.CreateUnitIconCache(info_index, unit_index);
    this.resetIconCommon(info_index, unit_index);
  }

  private void resetIconCommon(int info_index, int unit_index)
  {
    this.CreateUnitIconAction(info_index, unit_index);
    UnitIconInfo displayUnitInfo = this.displayUnitInfos[info_index];
    displayUnitInfo.icon.Overkillers = displayUnitInfo.is_overkillers;
    displayUnitInfo.icon.SetupDeckStatusBlink();
    this.SetUnitIconGray(unit_index);
  }

  protected override void Select(UnitIconBase unitIcon)
  {
    if (!unitIcon.Selected && this.checkGrayStatus(unitIcon.PlayerUnit))
      return;
    base.Select(unitIcon);
    if (unitIcon.Selected)
    {
      this.selectedUnitInfo.Add(this.CreateTowerDeckUnit(unitIcon.PlayerUnit.id, unitIcon.SelIndex + 1));
    }
    else
    {
      int? nullable = this.selectedUnitInfo.FirstIndexOrNull<TowerDeckUnit>((Func<TowerDeckUnit, bool>) (i => i.player_unit_id == unitIcon.PlayerUnit.id));
      if (nullable.HasValue)
        this.selectedUnitInfo.RemoveAt(nullable.Value);
    }
    this.updateExcludeOverkillers();
    this.updateDecideButton();
    this.updateGrayStatusAll();
  }

  private void updateDecideButton()
  {
    if (this.selectedUnitIcons.Count > 0)
    {
      bool flag = true;
      if (this.excludeOverkillersIds.Count > 0)
      {
        foreach (UnitIconInfo selectedUnitIcon in this.selectedUnitIcons)
        {
          if (this.excludeOverkillersIds.Contains(selectedUnitIcon.playerUnit.id))
          {
            flag = false;
            break;
          }
        }
      }
      ((UIButtonColor) this.btnDecide).isEnabled = flag;
    }
    else
      ((UIButtonColor) this.btnDecide).isEnabled = false;
  }

  private void updateGrayStatusAll()
  {
    List<UnitIconInfo> displayUnitInfos = this.displayUnitInfos;
    if (displayUnitInfos == null)
      return;
    if (this.SelectedUnitIsMax())
    {
      foreach (UnitIconInfo target in displayUnitInfos)
      {
        if (target.select >= 0 && !this.checkGrayStatus(target))
          this.setGrayStatus(target, false);
        else
          this.setGrayStatus(target);
      }
    }
    else
    {
      foreach (UnitIconInfo target in displayUnitInfos)
      {
        if (target.select < 0)
          this.setGrayStatus(target, this.checkGrayStatus(target));
      }
    }
  }

  public override void ForBattle(Func<UnitIconInfo, PlayerUnit, bool> func)
  {
    if (TowerUtil.towerDeckUnits == null)
      return;
    List<PlayerUnit> playerUnitList = new List<PlayerUnit>();
    PlayerUnit[] source = SMManager.Get<PlayerUnit[]>();
    for (int i = 0; i < TowerUtil.towerDeckUnits.Length; i++)
    {
      PlayerUnit playerUnit = ((IEnumerable<PlayerUnit>) source).Where<PlayerUnit>((Func<PlayerUnit, bool>) (u => u.id == TowerUtil.towerDeckUnits[i].player_unit_id)).FirstOrDefault<PlayerUnit>();
      if (playerUnit != (PlayerUnit) null)
        playerUnitList.Add(playerUnit);
    }
    foreach (PlayerUnit playerUnit in playerUnitList)
    {
      PlayerUnit unit = playerUnit;
      if (!(unit == (PlayerUnit) null))
      {
        UnitIconInfo unitIconInfo = this.allUnitInfos.FirstOrDefault<UnitIconInfo>((Func<UnitIconInfo, bool>) (x => func(x, unit)));
        if (unitIconInfo != null)
          unitIconInfo.for_battle = true;
      }
    }
  }

  public override void UpdateAllUnitTowerEntryView()
  {
  }

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }
}
