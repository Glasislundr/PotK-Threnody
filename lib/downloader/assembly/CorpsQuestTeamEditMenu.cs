// Decompiled with JetBrains decompiler
// Type: CorpsQuestTeamEditMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/CorpsQuest/TeamEditMenu")]
public class CorpsQuestTeamEditMenu : UnitSelectMenuBase
{
  [SerializeField]
  private UIButton btnDecide;
  private GameObject comfirmPopup;
  private GameObject goHpGauge;
  private List<PlayerUnit> selectedUnitInfo;
  private PlayerUnit[] playerUnits;
  private PlayerCorps playerCorps;
  private PlayerCorpsDeck playerDeck;
  private HashSet<int> usedUnitIds;
  private HashSet<int> excludeOverkillersIds;

  public IEnumerator ResourceLoad()
  {
    Future<GameObject> f = new ResourceObject("Prefabs/popup/popup_corpsquest_team_confirm__anim_popup01").Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.comfirmPopup = f.Result;
    f = (Future<GameObject>) null;
    f = Res.Prefabs.tower.dir_hp_gauge.Load<GameObject>();
    e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.goHpGauge = f.Result;
    f = (Future<GameObject>) null;
  }

  private IEnumerator EditTeam()
  {
    CorpsQuestTeamEditMenu questTeamEditMenu = this;
    Singleton<PopupManager>.GetInstance().dismiss();
    yield return (object) null;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    Future<WebAPI.Response.QuestCorpsDeckEdit> f = WebAPI.QuestCorpsDeckEdit(questTeamEditMenu.playerCorps.period_id, questTeamEditMenu.selectedUnitIcons.OrderBy<UnitIconInfo, int>((Func<UnitIconInfo, int>) (x => x.select)).Select<UnitIconInfo, int>((Func<UnitIconInfo, int>) (x => x.playerUnit.id)).ToArray<int>(), (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
    IEnumerator e1 = f.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (f.Result == null)
      MypageScene.ChangeSceneOnError();
    else
      questTeamEditMenu.backScene();
  }

  private IEnumerator ShowComfirmPopup()
  {
    CorpsQuestTeamEditMenu questTeamEditMenu = this;
    GameObject popup = questTeamEditMenu.comfirmPopup.Clone();
    popup.SetActive(false);
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = popup.GetComponent<CorpsQuestTeamEditComfirmPopup>().InitializeAsync(questTeamEditMenu.selectedUnitIcons.OrderBy<UnitIconInfo, int>((Func<UnitIconInfo, int>) (x => x.select)).ToList<UnitIconInfo>(), questTeamEditMenu.unitPrefab, questTeamEditMenu.goHpGauge, new Action(questTeamEditMenu.\u003CShowComfirmPopup\u003Eb__11_1));
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
    return this.usedUnitIds.Contains(target.id) || this.excludeOverkillersIds.Contains(target.id);
  }

  private void setGrayStatus(UnitIconInfo target, bool bGray = true)
  {
    target.gray = bGray;
    if (!Object.op_Inequality((Object) target.icon, (Object) null))
      return;
    target.icon.Gray = bGray;
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
    allUnitIcon.HpGauge.SetGauge(this.usedUnitIds.Contains(allUnitIcon.PlayerUnit.id) ? 0 : 1, 1, false);
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

  public IEnumerator InitializeAsync(PlayerCorps corps)
  {
    CorpsQuestTeamEditMenu questTeamEditMenu = this;
    PlayerUnit[] myUnits = SMManager.Get<PlayerUnit[]>();
    questTeamEditMenu.playerUnits = ((IEnumerable<PlayerUnit>) myUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (u => ((IEnumerable<int>) corps.entry_player_unit_ids).Contains<int>(u.id))).ToArray<PlayerUnit>();
    questTeamEditMenu.playerCorps = corps;
    questTeamEditMenu.playerDeck = Array.Find<PlayerCorpsDeck>(SMManager.Get<PlayerCorpsDeck[]>(), (Predicate<PlayerCorpsDeck>) (x => x.corps_id == corps.corps_id));
    questTeamEditMenu.usedUnitIds = new HashSet<int>((IEnumerable<int>) corps.used_player_unit_ids);
    questTeamEditMenu.selectedUnitInfo = questTeamEditMenu.playerDeck != null ? ((IEnumerable<int>) questTeamEditMenu.playerDeck.deck_player_unit_ids).Select<int, PlayerUnit>((Func<int, PlayerUnit>) (i => Array.Find<PlayerUnit>(myUnits, (Predicate<PlayerUnit>) (x => x.id == i)))).ToList<PlayerUnit>() : new List<PlayerUnit>();
    IEnumerator e;
    if (questTeamEditMenu.isInitialize)
    {
      e = questTeamEditMenu.UpdateInfoAndScroll(questTeamEditMenu.playerUnits);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      questTeamEditMenu.SetIconGray();
    }
    else
    {
      e = questTeamEditMenu.Initialize();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      questTeamEditMenu.SetIconType(UnitMenuBase.IconType.NormalWithHpGauge);
      questTeamEditMenu.InitializeInfo((IEnumerable<PlayerUnit>) questTeamEditMenu.playerUnits, (IEnumerable<PlayerMaterialUnit>) null, Persist.corpsUnitListSortAndFilter, false, false, true, true, true, new Action(questTeamEditMenu.InitializeAllUnitInfosExtend));
      e = questTeamEditMenu.CreateUnitIcon();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      questTeamEditMenu.TxtNumber.SetTextLocalize(string.Format("{0}/{1}", (object) questTeamEditMenu.selectedUnitIcons.Count, (object) questTeamEditMenu.SelectMax));
      questTeamEditMenu.TxtNumberpossession.SetTextLocalize(string.Format("{0}/{1}", (object) ((IEnumerable<PlayerUnit>) questTeamEditMenu.playerUnits).Count<PlayerUnit>((Func<PlayerUnit, bool>) (u => !this.usedUnitIds.Contains(u.id))), (object) questTeamEditMenu.playerUnits.Length));
      questTeamEditMenu.InitializeEnd();
      questTeamEditMenu.SetIconGray();
    }
  }

  public void InitializeAllUnitInfosExtend()
  {
    this.selectedUnitIcons.Clear();
    foreach (UnitIconInfo allUnitInfo in this.allUnitInfos)
    {
      UnitIconInfo info = allUnitInfo;
      int? nullable = this.selectedUnitInfo.FirstIndexOrNull<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.id == info.playerUnit.id));
      if (nullable.HasValue)
      {
        info.gray = true;
        if (!this.usedUnitIds.Contains(info.playerUnit.id))
        {
          info.select = nullable.Value;
          this.selectedUnitIcons.Add(info);
        }
      }
      info.is_used = this.usedUnitIds.Contains(info.playerUnit.id);
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
    foreach (UnitIconInfo allUnitInfo in this.allUnitInfos)
      allUnitInfo.select = -1;
    foreach (UnitIconInfo displayUnitInfo in this.displayUnitInfos)
      displayUnitInfo.select = -1;
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
    CorpsQuestTeamEditMenu questTeamEditMenu = this;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = questTeamEditMenu.\u003C\u003En__0(prefab);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) questTeamEditMenu.goHpGauge, (Object) null))
    {
      for (int index = 0; index < questTeamEditMenu.allUnitIcons.Count; ++index)
      {
        UnitIcon allUnitIcon = (UnitIcon) questTeamEditMenu.allUnitIcons[index];
        questTeamEditMenu.goHpGauge.Clone(allUnitIcon.hp_gauge.transform);
        if (allUnitIcon.PlayerUnit != (PlayerUnit) null)
          allUnitIcon.HpGauge.SetGauge(questTeamEditMenu.usedUnitIds.Contains(allUnitIcon.PlayerUnit.id) ? 0 : 1, 1, false);
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
      this.selectedUnitInfo.Add(unitIcon.PlayerUnit);
    }
    else
    {
      int? nullable = this.selectedUnitInfo.FirstIndexOrNull<PlayerUnit>((Func<PlayerUnit, bool>) (i => i.id == unitIcon.PlayerUnit.id));
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
    if (this.playerDeck == null)
      return;
    PlayerUnit[] units = SMManager.Get<PlayerUnit[]>();
    foreach (PlayerUnit playerUnit in ((IEnumerable<int>) this.playerDeck.deck_player_unit_ids).Select<int, PlayerUnit>((Func<int, PlayerUnit>) (i => Array.Find<PlayerUnit>(units, (Predicate<PlayerUnit>) (x => x.id == i)))))
    {
      PlayerUnit unit = playerUnit;
      UnitIconInfo unitIconInfo = this.allUnitInfos.FirstOrDefault<UnitIconInfo>((Func<UnitIconInfo, bool>) (x => func(x, unit)));
      if (unitIconInfo != null)
        unitIconInfo.for_battle = true;
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
