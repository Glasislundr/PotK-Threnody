// Decompiled with JetBrains decompiler
// Type: Unit00499Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Unit00499Menu : BackButtonMenuBase
{
  private Dictionary<int, Unit00499Menu.EvolutionSelectMap> dicSelector_;
  private Unit00499Menu.TransMode transMode;
  private Unit00499ReincarnationChange ReinCarnationChangeCtrl;
  [SerializeField]
  protected Unit00499UnitStatus beforeUnit;
  private Unit00499UnitStatus afterUnit;
  private Unit00499UnitStatus recordUnit;
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  protected UILabel TxtTransZeny;
  [SerializeField]
  protected GameObject dirAwake;
  [SerializeField]
  protected GameObject dirEvolutionMaterials;
  [SerializeField]
  protected GameObject DialogBox;
  [SerializeField]
  protected UILabel TxtMaterialName;
  [SerializeField]
  protected UILabel TxtMaterialPlace;
  [SerializeField]
  public UIButton ibtn_Save_Memory;
  public GameObject DirTransmigration;
  public GameObject comShortage;
  public GameObject[] comShortages;
  public GameObject[] linkEvolutionUnits;
  public UILabel[] linkTransUnitsPossessionLabel;
  public GameObject[] linkTransUnits;
  public UIButton evolutionBtn;
  public UIButton transBtn;
  public UIButton recordBtn;
  public bool isLevel;
  public bool isPlusValue;
  public UIButton awakeBtn;
  public Unit00499Evolution evolutionMenu;
  public List<int> materialUnitIds;
  public List<int> materialMaterialUnitIds;
  public int selectEvolutionPatternId;
  protected PlayerUnit baseUnit;
  protected PlayerUnit[] targetPlayerUnits;
  protected bool isUnit;
  protected bool isMoney;
  protected bool isFavorite;
  protected bool fromEarth;
  private Unit00499Scene.Mode sceneMode;
  private GameObject StatusUpPrefab;
  private Unit00499Scene.GvgCostInfo gvgCostInfo;
  private GameObject statusDetailPrefab;
  private Sprite unitLargeSprite;
  public Action exceptionBackScene;
  private GameObject SaveMemorySlotSelectPrefab;
  private GameObject MemorySlotOverwritePrefab;
  private GameObject MemorySlotListPrefab;
  [SerializeField]
  private Transform afterUnitBase;
  protected PlayerUnit playerUnit;
  protected PlayerUnit targetPlayerUnit;
  private PlayerUnit recordPlayerUnit;
  [SerializeField]
  private Transform dir_com_shortage_Transmigration;
  [SerializeField]
  private Transform dir_Com_shortage;

  private static bool isUnitSelectable(UnitUnit unit) => unit.IsNormalUnit;

  private static Unit00499Menu.UnitCondition getUnitCondition(
    PlayerUnit playerUnit,
    PlayerDeck[] decks)
  {
    foreach (PlayerDeck deck in decks)
    {
      if (deck != null)
      {
        foreach (PlayerUnit playerUnit1 in deck.player_units)
        {
          if (!(playerUnit1 == (PlayerUnit) null) && playerUnit1.id == playerUnit.id)
            return Unit00499Menu.UnitCondition.Organized;
        }
      }
    }
    return !playerUnit.favorite ? Unit00499Menu.UnitCondition.Normal : Unit00499Menu.UnitCondition.Favarite;
  }

  public IEnumerator InitEvolutionUnits(
    PlayerUnit[] playerUnits,
    PlayerMaterialUnit[] playerMaterialUnits,
    GameObject unitIconPrefab,
    Unit00499EvolutionIndicator eIndicator,
    int patternId,
    UnitUnit[] evoUnits,
    bool fromEarth = false)
  {
    Unit00499Menu unit00499Menu = this;
    UILabel[] label = eIndicator.linkEvolutionUnitsPossessionLabel;
    GameObject[] objects = eIndicator.linkEvolutionUnits;
    Unit00499EvolutionIndicator.ButtonIcon[] selectIcons = eIndicator.linkSelectableUnits;
    if (unit00499Menu.dicSelector_ == null)
      unit00499Menu.dicSelector_ = new Dictionary<int, Unit00499Menu.EvolutionSelectMap>();
    Unit00499Menu.EvolutionSelectMap selector;
    if (unit00499Menu.dicSelector_.ContainsKey(patternId))
    {
      selector = unit00499Menu.dicSelector_[patternId];
      selector.updateNormalUnit(playerUnits);
    }
    else
    {
      selector = Unit00499Menu.EvolutionSelectMap.create(unit00499Menu.baseUnit, playerUnits, playerMaterialUnits, evoUnits);
      selector.selectAuto(false);
      unit00499Menu.dicSelector_.Add(patternId, selector);
    }
    foreach (GameObject gameObject in objects)
      gameObject.transform.Clear();
    bool canCompleted = ((IEnumerable<bool>) selector.trySelecting()).Where<bool>((Func<bool, bool>) (b => b)).ToArray<bool>().Length == selector.samples_.Length;
    yield return (object) unit00499Menu.StartCoroutine(((IEnumerable<GameObject>) objects).Select<GameObject, IEnumerator>((Func<GameObject, int, IEnumerator>) ((link, n) =>
    {
      UnitUnit evoUnit = n < evoUnits.Length ? evoUnits[n] : (UnitUnit) null;
      Unit00499Menu.SelectCell selectCell = n < selector.selected_.Length ? selector.selected_[n] : (Unit00499Menu.SelectCell) null;
      GameObject top = selectIcons == null || n >= selectIcons.Length ? (GameObject) null : selectIcons[n].top_;
      if (Object.op_Inequality((Object) top, (Object) null))
        top.SetActive(canCompleted && evoUnit != null && selector.isSelectable(evoUnit.ID));
      UnitIcon component = unitIconPrefab.CloneAndGetComponent<UnitIcon>(link.transform);
      IEnumerator enumerator;
      if (evoUnit != null)
      {
        if (selectCell != null)
        {
          PlayerUnit unit = selectCell.unit_;
          enumerator = evoUnit.IsMaterialUnit ? component.SetMaterialUnit(unit, false, (PlayerUnit[]) null) : component.SetPlayerUnit(unit, (PlayerUnit[]) null, (PlayerUnit) null, false, false);
          if (unit.favorite)
            this.isFavorite = false;
        }
        else
        {
          enumerator = component.SetUnit(evoUnit, evoUnit.GetElement(), true);
          this.isUnit = false;
        }
      }
      else
      {
        enumerator = component.SetPlayerUnit((PlayerUnit) null, (PlayerUnit[]) null, (PlayerUnit) null, false, false);
        component.BackgroundModeValue = UnitIcon.BackgroundMode.PlayerShadow;
      }
      return enumerator;
    })).WaitAll());
    int column = 0;
    foreach (GameObject gameObject in objects)
    {
      UnitIcon icon = ((Component) gameObject.GetComponent<UI2DSprite>()).GetComponentInChildren<UnitIcon>();
      if (column < selector.samples_.Length)
      {
        ((Behaviour) icon.Button).enabled = true;
        ((Collider) icon.buttonBoxCollider).enabled = true;
        int num = 0;
        if (icon.unit.IsMaterialUnit)
        {
          PlayerMaterialUnit playerMaterialUnit = ((IEnumerable<PlayerMaterialUnit>) playerMaterialUnits).FirstOrDefault<PlayerMaterialUnit>((Func<PlayerMaterialUnit, bool>) (x => icon.Unit.ID == x.unit.ID));
          if (playerMaterialUnit != null)
            num = playerMaterialUnit.quantity;
          if (icon.Unit.ID == unit00499Menu.baseUnit.unit.ID)
            --num;
          icon.RarityCenter();
          unit00499Menu.setEventMaterialClicked(icon, (LongPressButton) null, fromEarth, selector, column, canCompleted);
        }
        else
        {
          if (icon.PlayerUnit != (PlayerUnit) null)
            icon.setBottom(icon.PlayerUnit);
          else
            icon.setLevelText("1");
          icon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
          num = selector.sources_[icon.Unit.ID].Length;
          unit00499Menu.setEventMaterialClicked(icon, selectIcons == null || column >= selectIcons.Length ? (LongPressButton) null : selectIcons[column].button_, fromEarth, selector, column, canCompleted);
        }
        label[column].SetTextLocalize(Consts.Format(Consts.GetInstance().unit_004_9_9_possession_text, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) num
          }
        }));
        ((Component) label[column]).gameObject.SetActive(true);
      }
      else
      {
        ((Behaviour) icon.Button).enabled = false;
        ((Collider) icon.buttonBoxCollider).enabled = false;
        icon.SetEmpty();
        icon.setSilhouette(fromEarth);
        ((Component) label[column]).gameObject.SetActive(false);
        gameObject.GetComponentInChildren<UIButton>().onClick.Clear();
      }
      ++column;
    }
  }

  private void setEventMaterialClicked(
    UnitIcon icon,
    LongPressButton toSelect,
    bool fromEarth,
    Unit00499Menu.EvolutionSelectMap selector,
    int column,
    bool canCompleted)
  {
    if (Object.op_Equality((Object) toSelect, (Object) null) || !canCompleted)
    {
      icon.onClick = (Action<UnitIconBase>) (x =>
      {
        if (fromEarth)
          return;
        this.ShowMaterialQuestInfo(x.Unit);
      });
      icon.SetButtonDetailEvent(icon.PlayerUnit, ((IEnumerable<Unit00499Menu.SelectCell>) selector.selected_).Where<Unit00499Menu.SelectCell>((Func<Unit00499Menu.SelectCell, bool>) (x => x != null)).Select<Unit00499Menu.SelectCell, PlayerUnit>((Func<Unit00499Menu.SelectCell, PlayerUnit>) (c => c.unit_)).ToArray<PlayerUnit>());
    }
    else
    {
      ((UIButtonColor) icon.Button).isEnabled = false;
      ((Collider) icon.buttonBoxCollider).enabled = false;
      EventDelegate.Set(toSelect.onClick, (EventDelegate.Callback) (() =>
      {
        if (this.IsPushAndSet())
          return;
        this.doUnitSelector(selector, column);
      }));
      if (icon.PlayerUnit != (PlayerUnit) null)
        EventDelegate.Set(toSelect.onLongPress, (EventDelegate.Callback) (() => Unit0042Scene.changeScene(true, icon.PlayerUnit, ((IEnumerable<Unit00499Menu.SelectCell>) selector.selected_).Where<Unit00499Menu.SelectCell>((Func<Unit00499Menu.SelectCell, bool>) (x => x != null)).Select<Unit00499Menu.SelectCell, PlayerUnit>((Func<Unit00499Menu.SelectCell, PlayerUnit>) (c => c.unit_)).ToArray<PlayerUnit>())));
      else
        toSelect.onLongPress.Clear();
    }
  }

  private void doUnitSelector(Unit00499Menu.EvolutionSelectMap selector, int column)
  {
    UnitUnit sample = selector.samples_[column];
    Unit00492Menu.Param param = new Unit00492Menu.Param()
    {
      baseUnit_ = selector.selected_[column] != null ? selector.selected_[column].unit_ : (PlayerUnit) null
    };
    param.selectedUnits_ = ((IEnumerable<Unit00499Menu.SelectCell>) selector.selected_).Where<Unit00499Menu.SelectCell>((Func<Unit00499Menu.SelectCell, bool>) (c => c != null && (param.baseUnit_ == (PlayerUnit) null || param.baseUnit_.id != c.unit_.id) && c.unit_.unit.ID == sample.ID)).Select<Unit00499Menu.SelectCell, PlayerUnit>((Func<Unit00499Menu.SelectCell, PlayerUnit>) (cc => cc.unit_)).ToArray<PlayerUnit>();
    param.units_ = ((IEnumerable<Unit00499Menu.SelectCell>) selector.sources_[sample.ID]).Select<Unit00499Menu.SelectCell, PlayerUnit>((Func<Unit00499Menu.SelectCell, PlayerUnit>) (c => c.unit_)).ToArray<PlayerUnit>();
    param.onUpdate_ = (Unit00492Menu.Param.EventUpdateUnit) (unit =>
    {
      Unit00499Menu.SelectCell selectCell = ((IEnumerable<Unit00499Menu.SelectCell>) selector.selected_).FirstOrDefault<Unit00499Menu.SelectCell>((Func<Unit00499Menu.SelectCell, bool>) (c => c != null && c.unit_.id == unit.id));
      if (selectCell == null)
        return;
      PlayerDeck[] decks = SMManager.Get<PlayerDeck[]>();
      if (Unit00499Menu.getUnitCondition(unit, decks) == Unit00499Menu.UnitCondition.Normal)
        return;
      selector.setUnselected(selectCell.column_);
    });
    param.onResult_ = (Unit00492Menu.Param.EventResult) (result =>
    {
      if (result == (PlayerUnit) null || param.baseUnit_ != (PlayerUnit) null && param.baseUnit_.id == result.id)
        return;
      selector.setSelected(((IEnumerable<Unit00499Menu.SelectCell>) selector.sources_[sample.ID]).First<Unit00499Menu.SelectCell>((Func<Unit00499Menu.SelectCell, bool>) (c => c.unit_.id == result.id)), column);
    });
    Unit00468Scene.changeScene00492EvolutionMaterial(true, param);
  }

  public void updateCheckEnableButton(int patternId)
  {
    this.isUnit = this.dicSelector_ != null && this.dicSelector_.ContainsKey(patternId) && this.dicSelector_[patternId].isCompletedSelect;
    this.isFavorite = true;
  }

  public List<int> getEvolutionMaterialSelectedUnitIds(int patternId)
  {
    return this.dicSelector_ == null || !this.dicSelector_.ContainsKey(patternId) ? new List<int>() : ((IEnumerable<Unit00499Menu.SelectCell>) this.dicSelector_[patternId].selected_).Where<Unit00499Menu.SelectCell>((Func<Unit00499Menu.SelectCell, bool>) (c => c != null && c.unit_.unit.IsNormalUnit)).Select<Unit00499Menu.SelectCell, int>((Func<Unit00499Menu.SelectCell, int>) (cc => cc.unit_.id)).ToList<int>();
  }

  public List<int> getEvolutionMaterialSelectedMaterialIds(int patternId)
  {
    return this.dicSelector_ == null || !this.dicSelector_.ContainsKey(patternId) ? new List<int>() : ((IEnumerable<Unit00499Menu.SelectCell>) this.dicSelector_[patternId].selected_).Where<Unit00499Menu.SelectCell>((Func<Unit00499Menu.SelectCell, bool>) (c => c != null && c.unit_.unit.IsMaterialUnit)).Select<Unit00499Menu.SelectCell, int>((Func<Unit00499Menu.SelectCell, int>) (cc => cc.unit_.id)).ToList<int>();
  }

  public Unit00499Scene.Mode SceneMode => this.sceneMode;

  private IEnumerator ShowTransmigrationPopup()
  {
    Unit00499Menu menu = this;
    Future<GameObject> popupPrefabF = Res.Prefabs.popup.popup_004_13_1__anim_popup01.Load<GameObject>();
    IEnumerator e = popupPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(popupPrefabF.Result).GetComponent<Popup004131Menu>().Init(menu);
  }

  private IEnumerator ShowRecordTransmigrationPopup()
  {
    Unit00499Menu menu = this;
    Future<GameObject> popupPrefabF = Res.Prefabs.popup.popup_004_13_1__anim_popup01.Load<GameObject>();
    IEnumerator e = popupPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(popupPrefabF.Result).GetComponent<Popup004131Menu>().Init(menu, true);
  }

  public IEnumerator Transmigration(bool isRecod)
  {
    if (!Singleton<CommonRoot>.GetInstance().isLoading)
    {
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      Future<WebAPI.Response.UnitTransmigrate> paramF = WebAPI.UnitTransmigrate(this.baseUnit.id, isRecod, this.materialMaterialUnitIds.ToArray(), this.materialUnitIds.ToArray(), this.baseUnit.unit.TransmigratePattern.ID, (Action<WebAPI.Response.UserError>) (error =>
      {
        WebAPI.DefaultUserErrorCallback(error);
        MypageScene.ChangeSceneOnError();
      }));
      IEnumerator e = paramF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (paramF.Result != null)
      {
        Singleton<NGGameDataManager>.GetInstance().corps_player_unit_ids = new HashSet<int>((IEnumerable<int>) paramF.Result.corps_player_unit_ids);
        e = OnDemandDownload.WaitLoadHasUnitResource(false);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        PlayerUnit playerUnit1 = (PlayerUnit) null;
        foreach (PlayerUnit playerUnit2 in SMManager.Get<PlayerUnit[]>())
        {
          if (playerUnit2.id == this.baseUnit.id)
          {
            playerUnit1 = playerUnit2;
            break;
          }
        }
        List<PlayerUnit> playerUnitList = new List<PlayerUnit>();
        foreach (GameObject linkTransUnit in this.linkTransUnits)
        {
          UnitIcon componentInChildren = ((Component) linkTransUnit.GetComponent<UI2DSprite>()).GetComponentInChildren<UnitIcon>();
          if (componentInChildren.PlayerUnit != (PlayerUnit) null)
            playerUnitList.Add(componentInChildren.PlayerUnit);
        }
        unit00497Scene.ChangeScene(false, new PrincesEvolutionParam()
        {
          materiaqlUnits = playerUnitList,
          is_new = false,
          baseUnit = this.baseUnit,
          resultUnit = playerUnit1,
          mode = Unit00499Scene.Mode.Transmigration
        });
        Singleton<NGSceneManager>.GetInstance().destroyScene("unit004_9_9");
        paramF = (Future<WebAPI.Response.UnitTransmigrate>) null;
      }
    }
  }

  protected virtual IEnumerator Evolution()
  {
    if (!Singleton<CommonRoot>.GetInstance().isLoading)
    {
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      Future<WebAPI.Response.UnitEvolution> paramF = WebAPI.UnitEvolution(this.baseUnit.unit.IsMaterialUnit ? this.baseUnit.id : 0, this.baseUnit.unit.IsNormalUnit ? this.baseUnit.id : 0, this.materialMaterialUnitIds.ToArray(), this.materialUnitIds.ToArray(), this.selectEvolutionPatternId, (Action<WebAPI.Response.UserError>) (error =>
      {
        WebAPI.DefaultUserErrorCallback(error);
        MypageScene.ChangeSceneOnError();
      }));
      IEnumerator e = paramF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (paramF.Result != null)
      {
        Singleton<NGGameDataManager>.GetInstance().corps_player_unit_ids = new HashSet<int>((IEnumerable<int>) paramF.Result.corps_player_unit_ids);
        e = OnDemandDownload.WaitLoadHasUnitResource(false);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (paramF.Result.unlock_quests != null)
          Debug.Log((object) ("paramF.Result.unlock_quests:" + (object) paramF.Result.unlock_quests.Length));
        UnitEvolutionResultData.GetInstance().SetData(paramF.Result);
        UnitUnit targetUnit = MasterData.UnitEvolutionPattern[this.selectEvolutionPatternId].target_unit;
        PlayerUnit playerUnit1 = (PlayerUnit) null;
        if (targetUnit.IsMaterialUnit)
        {
          foreach (PlayerMaterialUnit playerMaterialUnit in paramF.Result.player_material_units)
          {
            if (targetUnit.ID == playerMaterialUnit.unit.ID)
            {
              playerUnit1 = PlayerUnit.CreateByPlayerMaterialUnit(playerMaterialUnit);
              break;
            }
          }
        }
        else if (this.baseUnit.unit.IsNormalUnit)
        {
          foreach (PlayerUnit playerUnit2 in paramF.Result.player_units)
          {
            if (playerUnit2.id == this.baseUnit.id)
            {
              playerUnit1 = playerUnit2;
              break;
            }
          }
        }
        else
        {
          foreach (PlayerUnit playerUnit3 in paramF.Result.player_units)
          {
            if (targetUnit.ID == playerUnit3.unit.ID)
            {
              playerUnit1 = playerUnit3;
              break;
            }
          }
        }
        List<PlayerUnit> playerUnitList = new List<PlayerUnit>();
        foreach (GameObject linkEvolutionUnit in this.linkEvolutionUnits)
        {
          UnitIcon componentInChildren = ((Component) linkEvolutionUnit.GetComponent<UI2DSprite>()).GetComponentInChildren<UnitIcon>();
          if (componentInChildren.PlayerUnit != (PlayerUnit) null)
            playerUnitList.Add(componentInChildren.PlayerUnit);
        }
        unit00497Scene.ChangeScene(false, new PrincesEvolutionParam()
        {
          materiaqlUnits = playerUnitList,
          is_new = paramF.Result.is_new,
          baseUnit = this.baseUnit,
          resultUnit = playerUnit1,
          mode = this.baseUnit.unit.CanAwakeUnitFlag ? (this.baseUnit.unit.ID != 101414 ? Unit00499Scene.Mode.CommonAwakeUnit : Unit00499Scene.Mode.AwakeUnit) : Unit00499Scene.Mode.Evolution
        });
        Singleton<NGSceneManager>.GetInstance().destroyScene("unit004_9_9");
        paramF = (Future<WebAPI.Response.UnitEvolution>) null;
      }
    }
  }

  private void DrawTransButton()
  {
    ((Component) this.transBtn).gameObject.SetActive(true);
    ((Component) this.recordBtn).gameObject.SetActive(false);
  }

  private void DrawRecordButton()
  {
    ((Component) this.transBtn).gameObject.SetActive(false);
    ((Component) this.recordBtn).gameObject.SetActive(true);
  }

  protected void InitNumber(
    int v1,
    int v2,
    bool isNormalUnit1,
    bool isNormalUnit2,
    UILabel source,
    UILabel target,
    bool isAwake = false)
  {
    Action<string, UILabel> action = (Action<string, UILabel>) ((v, label) =>
    {
      label.SetTextLocalize(v);
      ((Component) label).gameObject.SetActive(true);
    });
    action(isNormalUnit1 ? v1.ToString() : "---", source);
    if (!isNormalUnit1)
      ((UIWidget) source).color = Color.white;
    action(isNormalUnit2 ? v2.ToString() : "---", target);
    if (!(!isNormalUnit2 | isAwake))
      return;
    ((UIWidget) target).color = Color.white;
  }

  private bool mustCreateAfterUnitObject(Unit00499Scene.Mode mode, Unit00499UnitStatus afterUnit)
  {
    if (Object.op_Equality((Object) afterUnit, (Object) null))
      return true;
    if (mode == afterUnit.mode)
      return false;
    switch (mode)
    {
      case Unit00499Scene.Mode.Evolution:
      case Unit00499Scene.Mode.EarthEvolution:
      case Unit00499Scene.Mode.AwakeUnit:
      case Unit00499Scene.Mode.CommonAwakeUnit:
        return afterUnit.mode == Unit00499Scene.Mode.Transmigration;
      case Unit00499Scene.Mode.Transmigration:
        return true;
      default:
        return true;
    }
  }

  public IEnumerator InitPlayer(
    PlayerUnit playerUnit,
    PlayerUnit targetPlayerUnit,
    GameObject unitIconPrefab,
    bool fromEarth = false)
  {
    Unit00499Menu unit00499Menu = this;
    unit00499Menu.playerUnit = playerUnit;
    unit00499Menu.targetPlayerUnit = targetPlayerUnit;
    unit00499Menu.recordPlayerUnit = (PlayerUnit) null;
    IEnumerator e;
    if (unit00499Menu.mustCreateAfterUnitObject(unit00499Menu.sceneMode, unit00499Menu.afterUnit))
    {
      foreach (Component component in unit00499Menu.afterUnitBase)
        Object.Destroy((Object) component.gameObject);
      Future<GameObject> prefabF = (Future<GameObject>) null;
      switch (unit00499Menu.sceneMode)
      {
        case Unit00499Scene.Mode.Evolution:
        case Unit00499Scene.Mode.EarthEvolution:
        case Unit00499Scene.Mode.AwakeUnit:
        case Unit00499Scene.Mode.CommonAwakeUnit:
          prefabF = Res.Prefabs.unit004_9_9.dir_Status_Evolution_After.Load<GameObject>();
          e = prefabF.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          unit00499Menu.afterUnit = prefabF.Result.Clone(unit00499Menu.afterUnitBase).GetComponent<Unit00499UnitStatus>();
          unit00499Menu.afterUnit.mode = unit00499Menu.sceneMode;
          break;
        case Unit00499Scene.Mode.Transmigration:
          prefabF = Res.Prefabs.unit004_9_9.dir_Reincarnation_Chenge_Status_anim.Load<GameObject>();
          e = prefabF.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          unit00499Menu.ReinCarnationChangeCtrl = prefabF.Result.Clone(unit00499Menu.afterUnitBase).GetComponent<Unit00499ReincarnationChange>();
          unit00499Menu.ReinCarnationChangeCtrl.SetAction(new Action(unit00499Menu.DrawTransButton), new Action(unit00499Menu.DrawRecordButton));
          unit00499Menu.afterUnit = unit00499Menu.ReinCarnationChangeCtrl.AfterUnit;
          unit00499Menu.afterUnit.mode = unit00499Menu.sceneMode;
          unit00499Menu.recordUnit = unit00499Menu.ReinCarnationChangeCtrl.RecordUnit;
          if (PlayerUnitTransMigrateMemoryList.Current == null && Singleton<TutorialRoot>.GetInstance().IsTutorialFinish())
          {
            e = WebAPI.UnitListTransmigrateMemory().Wait();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          }
          break;
      }
      prefabF = (Future<GameObject>) null;
    }
    Func<PlayerUnit, GameObject, bool, bool, IEnumerator> initUnitIcon = (Func<PlayerUnit, GameObject, bool, bool, IEnumerator>) ((pu, go, before, isMemory) =>
    {
      foreach (Component component in go.transform)
        Object.Destroy((Object) component.gameObject);
      UnitIcon component1 = unitIconPrefab.CloneAndGetComponent<UnitIcon>(go.transform);
      component1.RarityCenter();
      ((Collider) component1.buttonBoxCollider).enabled = true;
      ((Behaviour) component1.Button).enabled = true;
      PlayerUnit[] units = new PlayerUnit[1]
      {
        before ? this.playerUnit : this.targetPlayerUnit
      };
      return this.InitUnitIcon(component1, pu, units, before, fromEarth, isMemory);
    });
    e = initUnitIcon(unit00499Menu.playerUnit, unit00499Menu.beforeUnit.linkUnit, true, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = initUnitIcon(unit00499Menu.targetPlayerUnit, unit00499Menu.afterUnit.linkUnit, false, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (unit00499Menu.sceneMode == Unit00499Scene.Mode.Transmigration)
    {
      List<PlayerUnit> self = PlayerTransmigrateMemoryPlayerUnitIds.Current != null ? PlayerTransmigrateMemoryPlayerUnitIds.Current.PlayerUnits() : new List<PlayerUnit>();
      int? nullable = self.FirstIndexOrNull<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null && x.id == this.playerUnit.id));
      unit00499Menu.recordPlayerUnit = nullable.HasValue ? self[nullable.Value] : (PlayerUnit) null;
      if (Object.op_Inequality((Object) unit00499Menu.recordUnit, (Object) null) && unit00499Menu.recordPlayerUnit != (PlayerUnit) null)
      {
        if (playerUnit.level >= playerUnit.unit.rarity.reincarnation_level)
        {
          ((Component) unit00499Menu.afterUnit).gameObject.SetActive(true);
          ((Component) unit00499Menu.recordUnit).gameObject.SetActive(true);
          unit00499Menu.ReinCarnationChangeCtrl.SetActiveChangeButton(true);
          unit00499Menu.DrawTransButton();
          unit00499Menu.ReinCarnationChangeCtrl.AfterFront();
          if (unit00499Menu.transMode == Unit00499Menu.TransMode.Record)
          {
            unit00499Menu.ReinCarnationChangeCtrl.IbtnChangeAfter();
            unit00499Menu.transMode = Unit00499Menu.TransMode.Record;
          }
        }
        else
        {
          ((Component) unit00499Menu.recordUnit).gameObject.SetActive(true);
          ((Component) unit00499Menu.afterUnit).gameObject.SetActive(false);
          unit00499Menu.ReinCarnationChangeCtrl.SetActiveChangeButton(false);
          unit00499Menu.DrawRecordButton();
          unit00499Menu.ReinCarnationChangeCtrl.RecordFront();
          if (unit00499Menu.transMode == Unit00499Menu.TransMode.Trans)
            unit00499Menu.transMode = Unit00499Menu.TransMode.Trans;
        }
        e = initUnitIcon(unit00499Menu.recordPlayerUnit, unit00499Menu.recordUnit.linkUnit, true, true);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
      {
        unit00499Menu.ReinCarnationChangeCtrl.SetActiveChangeButton(false);
        ((Component) unit00499Menu.recordUnit).gameObject.SetActive(false);
        unit00499Menu.DrawTransButton();
        unit00499Menu.ReinCarnationChangeCtrl.AfterFront();
        if (unit00499Menu.transMode == Unit00499Menu.TransMode.Record)
        {
          unit00499Menu.ReinCarnationChangeCtrl.IbtnChangeAfter();
          unit00499Menu.transMode = Unit00499Menu.TransMode.Record;
        }
      }
    }
    unit00499Menu.SetStatusText();
  }

  protected virtual void SetStatusText()
  {
    this.beforeUnit.SetStatusText(this.playerUnit);
    this.afterUnit.SetStatusText(this.targetPlayerUnit, true);
    if (!(this.recordPlayerUnit != (PlayerUnit) null))
      return;
    this.recordUnit.SetStatusTextMemory(this.recordPlayerUnit);
  }

  private IEnumerator InitUnitIcon(
    UnitIcon unitIcon,
    PlayerUnit unit,
    PlayerUnit[] units,
    bool before,
    bool fromEarth,
    bool isMemory = false)
  {
    IEnumerator e;
    if (before)
    {
      e = unitIcon.SetPlayerUnit(unit, units, (PlayerUnit) null, true, isMemory);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = unitIcon.SetPlayerUnitEvolution(unit, units, isMaterial: true, isMemory: isMemory);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    unitIcon.onClick = (Action<UnitIconBase>) (icon =>
    {
      if (before)
      {
        if (fromEarth)
        {
          Unit0542Scene.changeScene(true, icon.PlayerUnit, units);
        }
        else
        {
          if (Singleton<NGGameDataManager>.GetInstance().IsSea)
            Singleton<NGSceneManager>.GetInstance().LastHeaderType = new CommonRoot.HeaderType?(Singleton<CommonRoot>.GetInstance().headerType);
          Unit0042Scene.changeScene(true, icon.PlayerUnit, units, true, isMemory);
        }
      }
      else if (fromEarth)
      {
        Unit0542Scene.changeSceneEvolutionUnit(true, icon.PlayerUnit, units);
      }
      else
      {
        if (Singleton<NGGameDataManager>.GetInstance().IsSea)
          Singleton<NGSceneManager>.GetInstance().LastHeaderType = new CommonRoot.HeaderType?(Singleton<CommonRoot>.GetInstance().headerType);
        Unit0042Scene.changeSceneEvolutionUnit(true, icon.PlayerUnit, units, true, isMemory);
      }
    });
  }

  public IEnumerator InitTransmigrationUnits(
    PlayerUnit[] playerUnits,
    PlayerMaterialUnit[] playerMaterialUnits,
    GameObject unitIconPrefab,
    UILabel[] label,
    GameObject[] objects,
    UnitUnit[] evoUnits,
    bool fromEarth = false)
  {
    Unit00499Menu unit00499Menu = this;
    Dictionary<int, Queue<PlayerUnit>> playerUnitDic = new Dictionary<int, Queue<PlayerUnit>>();
    foreach (PlayerUnit playerUnit in playerUnits)
    {
      if (!playerUnitDic.ContainsKey(playerUnit.unit.ID))
        playerUnitDic.Add(playerUnit.unit.ID, new Queue<PlayerUnit>());
      if (playerUnit.id != unit00499Menu.baseUnit.id)
        playerUnitDic[playerUnit.unit.ID].Enqueue(playerUnit);
    }
    foreach (PlayerMaterialUnit playerMaterialUnit in playerMaterialUnits)
    {
      if (playerMaterialUnit.quantity > 0)
      {
        if (!playerUnitDic.ContainsKey(playerMaterialUnit.unit.ID))
          playerUnitDic.Add(playerMaterialUnit.unit.ID, new Queue<PlayerUnit>());
        int num = playerMaterialUnit.quantity - (playerMaterialUnit.id != unit00499Menu.baseUnit.id || !unit00499Menu.baseUnit.unit.IsMaterialUnit ? 0 : 1);
        for (int count = 0; count < num; ++count)
          playerUnitDic[playerMaterialUnit.unit.ID].Enqueue(PlayerUnit.CreateByPlayerMaterialUnit(playerMaterialUnit, count));
      }
    }
    if (playerUnitDic.ContainsKey(unit00499Menu.baseUnit.unit.ID) && playerUnitDic[unit00499Menu.baseUnit.unit.ID].Count == 0)
      playerUnitDic.Remove(unit00499Menu.baseUnit.unit.ID);
    foreach (GameObject gameObject in objects)
      gameObject.transform.Clear();
    int evoUnitCnt = 0;
    List<PlayerUnit> materialUnitList = new List<PlayerUnit>();
    yield return (object) unit00499Menu.StartCoroutine(((IEnumerable<GameObject>) objects).Select<GameObject, IEnumerator>((Func<GameObject, int, IEnumerator>) ((link, n) =>
    {
      UnitUnit evoUnit = n < evoUnits.Length ? evoUnits[n] : (UnitUnit) null;
      UnitIcon component = unitIconPrefab.CloneAndGetComponent<UnitIcon>(link.transform);
      IEnumerator enumerator;
      if (evoUnit != null)
      {
        if (playerUnitDic.ContainsKey(evoUnit.ID))
        {
          PlayerUnit playerUnit = playerUnitDic[evoUnit.ID].Dequeue();
          if (playerUnitDic[evoUnit.ID].Count == 0)
            playerUnitDic.Remove(evoUnit.ID);
          materialUnitList.Add(playerUnit);
          enumerator = component.SetMaterialUnit(playerUnit, false, (PlayerUnit[]) null);
          if (playerUnit.favorite)
            this.isFavorite = false;
        }
        else
        {
          enumerator = component.SetUnit(evoUnit, evoUnit.GetElement(), true);
          component.setLevelText("1");
          this.isUnit = false;
        }
        ++evoUnitCnt;
      }
      else
      {
        enumerator = component.SetPlayerUnit((PlayerUnit) null, (PlayerUnit[]) null, (PlayerUnit) null, true, false);
        component.BackgroundModeValue = UnitIcon.BackgroundMode.PlayerShadow;
      }
      return enumerator;
    })).WaitAll());
    int index = 0;
    foreach (GameObject gameObject in objects)
    {
      UnitIcon icon = ((Component) gameObject.GetComponent<UI2DSprite>()).GetComponentInChildren<UnitIcon>();
      if (index < evoUnitCnt)
      {
        ((Behaviour) icon.Button).enabled = true;
        ((Collider) icon.buttonBoxCollider).enabled = true;
        int num = 0;
        if (icon.unit.IsMaterialUnit)
        {
          PlayerMaterialUnit playerMaterialUnit = ((IEnumerable<PlayerMaterialUnit>) playerMaterialUnits).FirstOrDefault<PlayerMaterialUnit>((Func<PlayerMaterialUnit, bool>) (x => icon.Unit.ID == x.unit.ID));
          if (playerMaterialUnit != null)
            num = playerMaterialUnit.quantity;
        }
        else
          num = ((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => icon.Unit.ID == x.unit.ID)).ToArray<PlayerUnit>().Length;
        if (icon.Unit.ID == unit00499Menu.baseUnit.unit.ID)
          --num;
        label[index].SetTextLocalize(Consts.Format(Consts.GetInstance().unit_004_9_9_possession_text, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) num
          }
        }));
        ((Component) label[index]).gameObject.SetActive(true);
        icon.RarityCenter();
        icon.onClick = (Action<UnitIconBase>) (x =>
        {
          if (fromEarth)
            return;
          this.ShowMaterialQuestInfo(x.Unit);
        });
        icon.SetButtonDetailEvent(icon.PlayerUnit, materialUnitList.ToArray());
      }
      else
      {
        ((Behaviour) icon.Button).enabled = false;
        ((Collider) icon.buttonBoxCollider).enabled = false;
        icon.SetEmpty();
        icon.setSilhouette(fromEarth);
        ((Component) label[index]).gameObject.SetActive(false);
        gameObject.GetComponentInChildren<UIButton>().onClick.Clear();
      }
      ++index;
    }
  }

  public IEnumerator InitTitle(string titleText)
  {
    this.TxtTitle.SetTextLocalize(Consts.Format(titleText, (IDictionary) new Hashtable()
    {
      {
        (object) "UnitName",
        (object) this.baseUnit.unit.name
      }
    }));
    yield break;
  }

  public List<int> GetMaterialUnitIDS(PlayerUnit baseUnit, PlayerUnit[] units, UnitUnit[] evo)
  {
    List<int> list = new List<int>();
    ((IEnumerable<UnitUnit>) evo).ForEach<UnitUnit>((Action<UnitUnit>) (x =>
    {
      if (!x.IsNormalUnit)
        return;
      foreach (PlayerUnit playerUnit in (IEnumerable<PlayerUnit>) ((IEnumerable<PlayerUnit>) units).Where<PlayerUnit>((Func<PlayerUnit, bool>) (y => x.ID == y.unit.ID)).OrderBy<PlayerUnit, int>((Func<PlayerUnit, int>) (y => y.level)))
      {
        if ((playerUnit.id != baseUnit.id || !baseUnit.unit.IsNormalUnit) && !list.Contains(playerUnit.id))
        {
          list.Add(playerUnit.id);
          break;
        }
      }
    }));
    return list;
  }

  private PlayerUnit[] getMaterialPlayerUnits()
  {
    return ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).Where<PlayerUnit>((Func<PlayerUnit, bool>) (unit => this.materialUnitIds.Any<int>((Func<int, bool>) (id => id == unit.id)))).ToArray<PlayerUnit>();
  }

  public List<int> GetMaterialMaterialUnitIDS(
    PlayerUnit baseUnit,
    PlayerMaterialUnit[] materialUnits,
    UnitUnit[] evo)
  {
    List<int> list = new List<int>();
    ((IEnumerable<UnitUnit>) evo).ForEach<UnitUnit>((Action<UnitUnit>) (x =>
    {
      if (!x.IsMaterialUnit)
        return;
      foreach (PlayerMaterialUnit playerMaterialUnit in ((IEnumerable<PlayerMaterialUnit>) materialUnits).Where<PlayerMaterialUnit>((Func<PlayerMaterialUnit, bool>) (y => x.ID == y.unit.ID)))
      {
        PlayerMaterialUnit z = playerMaterialUnit;
        if (list.Count<int>((Func<int, bool>) (w => w == z.id)) + (z.id != baseUnit.id || !baseUnit.unit.IsMaterialUnit ? 0 : 1) <= z.quantity)
        {
          list.Add(z.id);
          break;
        }
      }
    }));
    return list;
  }

  private PlayerMaterialUnit[] getMaterialMaterialPlayerUnits()
  {
    return ((IEnumerable<PlayerMaterialUnit>) SMManager.Get<PlayerMaterialUnit[]>()).Where<PlayerMaterialUnit>((Func<PlayerMaterialUnit, bool>) (unit => this.materialMaterialUnitIds.Any<int>((Func<int, bool>) (id => id == unit.id)))).ToArray<PlayerMaterialUnit>();
  }

  private void SetTransUpStatus(GameObject dst, GameObject go, int value)
  {
    dst.transform.Clear();
    dst.SetActive(false);
    if (value <= 0)
      return;
    dst.SetActive(true);
    go.CloneAndGetComponent<UnitTransAddStatus>(dst.transform).Init(value);
  }

  public IEnumerator Init(
    PlayerUnit playerUnit,
    PlayerUnit[] targetPlayerUnits,
    Unit00499Scene.Mode sceneMode,
    bool fromEarth = false,
    Unit00499Scene.GvgCostInfo gvgCostInfo = Unit00499Scene.GvgCostInfo.None)
  {
    this.sceneMode = sceneMode;
    this.baseUnit = playerUnit;
    this.targetPlayerUnits = targetPlayerUnits;
    this.isLevel = true;
    this.isUnit = true;
    this.isMoney = true;
    this.isFavorite = true;
    this.isPlusValue = true;
    this.fromEarth = fromEarth;
    if (!fromEarth)
    {
      ((UIButtonColor) this.recordBtn).isEnabled = false;
      ((UIButtonColor) this.transBtn).isEnabled = false;
    }
    ((UIButtonColor) this.evolutionBtn).isEnabled = false;
    ((UIButtonColor) this.awakeBtn).isEnabled = false;
    this.gvgCostInfo = gvgCostInfo;
    PlayerUnit[] playerUnits = SMManager.Get<PlayerUnit[]>();
    PlayerMaterialUnit[] playerMaterialUnits = SMManager.Get<PlayerMaterialUnit[]>();
    this.DialogBox.SetActive(false);
    this.dirAwake.SetActive(false);
    IEnumerator e;
    Future<GameObject> unitIconPrefabF;
    if (this.sceneMode == Unit00499Scene.Mode.Evolution || this.sceneMode == Unit00499Scene.Mode.EarthEvolution)
    {
      this.comShortage.transform.parent = this.dir_Com_shortage;
      this.comShortage.transform.localPosition = Vector3.zero;
      if (!fromEarth)
      {
        this.DirTransmigration.SetActive(false);
        ((Component) this.awakeBtn).gameObject.SetActive(false);
        ((Component) this.evolutionBtn).gameObject.SetActive(true);
        this.dirEvolutionMaterials.SetActive(true);
      }
      e = this.InitTitle(this.sceneMode == Unit00499Scene.Mode.EarthEvolution ? Consts.GetInstance().unit_054_9_9_evolution_title_text : Consts.GetInstance().unit_004_9_9_evolution_title_text);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = this.evolutionMenu.Init(playerUnit, targetPlayerUnits, fromEarth);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else if (this.sceneMode == Unit00499Scene.Mode.AwakeUnit)
    {
      this.DirTransmigration.SetActive(false);
      ((Component) this.evolutionBtn).gameObject.SetActive(false);
      this.dirEvolutionMaterials.SetActive(true);
      ((UIButtonColor) this.awakeBtn).isEnabled = true;
      ((Component) this.awakeBtn).gameObject.SetActive(true);
      this.dirAwake.SetActive(true);
      e = this.InitTitle(Consts.GetInstance().unit_004_9_9_awake_title_text);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = this.evolutionMenu.Init(playerUnit, targetPlayerUnits, fromEarth);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else if (this.sceneMode == Unit00499Scene.Mode.CommonAwakeUnit)
    {
      this.DirTransmigration.SetActive(false);
      ((Component) this.evolutionBtn).gameObject.SetActive(false);
      this.dirEvolutionMaterials.SetActive(true);
      ((UIButtonColor) this.awakeBtn).isEnabled = true;
      ((Component) this.awakeBtn).gameObject.SetActive(true);
      this.dirAwake.SetActive(true);
      e = this.InitTitle(Consts.GetInstance().unit_004_9_9_awake_title_text);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = this.evolutionMenu.Init(playerUnit, targetPlayerUnits, fromEarth);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      this.comShortage.transform.parent = this.dir_com_shortage_Transmigration;
      this.comShortage.transform.localPosition = Vector3.zero;
      if (Object.op_Equality((Object) this.StatusUpPrefab, (Object) null))
      {
        Future<GameObject> statusUpPrefabF = Res.Prefabs.unit004_9_9.dir_Uppt.Load<GameObject>();
        e = statusUpPrefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.StatusUpPrefab = statusUpPrefabF.Result;
        statusUpPrefabF = (Future<GameObject>) null;
      }
      Future<GameObject> prefabF = (Future<GameObject>) null;
      if (Object.op_Equality((Object) this.SaveMemorySlotSelectPrefab, (Object) null))
      {
        prefabF = Res.Prefabs.popup.popup_004_save_memory_slot_select__anim_popup01.Load<GameObject>();
        e = prefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.SaveMemorySlotSelectPrefab = prefabF.Result;
      }
      if (Object.op_Equality((Object) this.MemorySlotOverwritePrefab, (Object) null))
      {
        prefabF = Res.Prefabs.popup.popup_004_memory_slot_overwrite__anim_popup01.Load<GameObject>();
        e = prefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.MemorySlotOverwritePrefab = prefabF.Result;
      }
      this.DirTransmigration.SetActive(true);
      ((Component) this.awakeBtn).gameObject.SetActive(false);
      ((Component) this.evolutionBtn).gameObject.SetActive(false);
      this.dirEvolutionMaterials.SetActive(false);
      this.materialUnitIds = this.GetMaterialUnitIDS(playerUnit, playerUnits, playerUnit.unit.TransmigrateUnits);
      this.materialMaterialUnitIds = this.GetMaterialMaterialUnitIDS(playerUnit, playerMaterialUnits, playerUnit.unit.TransmigrateUnits);
      unitIconPrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      e = unitIconPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject unitIconPrefab = unitIconPrefabF.Result;
      e = this.InitTransmigrationUnits(playerUnits, playerMaterialUnits, unitIconPrefab, this.linkTransUnitsPossessionLabel, this.linkTransUnits, playerUnit.unit.TransmigrateUnits);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Player player = SMManager.Get<Player>();
      NGGameDataManager.Boost boostInfo = Singleton<NGGameDataManager>.GetInstance().BoostInfo;
      int price = (int) ((Decimal) playerUnit.unit.TransmigratePattern.price * (boostInfo == null ? 1.0M : boostInfo.DiscountUnitTransmigrate));
      ((UIWidget) this.TxtTransZeny).color = player.money >= (long) price ? Color.white : Color.red;
      this.TxtTransZeny.SetTextLocalize(price.ToString());
      e = this.InitTitle(Consts.GetInstance().unit_004_9_9_trans_title_text);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = this.InitPlayer(playerUnit, targetPlayerUnits[0], unitIconPrefab, fromEarth);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.SetTransUpStatus(this.afterUnit.TransUpParameter[0], this.StatusUpPrefab, targetPlayerUnits[0].hp.transmigrate - playerUnit.hp.transmigrate);
      this.SetTransUpStatus(this.afterUnit.TransUpParameter[1], this.StatusUpPrefab, targetPlayerUnits[0].strength.transmigrate - playerUnit.strength.transmigrate);
      this.SetTransUpStatus(this.afterUnit.TransUpParameter[3], this.StatusUpPrefab, targetPlayerUnits[0].vitality.transmigrate - playerUnit.vitality.transmigrate);
      this.SetTransUpStatus(this.afterUnit.TransUpParameter[2], this.StatusUpPrefab, targetPlayerUnits[0].intelligence.transmigrate - playerUnit.intelligence.transmigrate);
      this.SetTransUpStatus(this.afterUnit.TransUpParameter[4], this.StatusUpPrefab, targetPlayerUnits[0].mind.transmigrate - playerUnit.mind.transmigrate);
      this.SetTransUpStatus(this.afterUnit.TransUpParameter[5], this.StatusUpPrefab, targetPlayerUnits[0].agility.transmigrate - playerUnit.agility.transmigrate);
      this.SetTransUpStatus(this.afterUnit.TransUpParameter[6], this.StatusUpPrefab, targetPlayerUnits[0].dexterity.transmigrate - playerUnit.dexterity.transmigrate);
      this.SetTransUpStatus(this.afterUnit.TransUpParameter[7], this.StatusUpPrefab, targetPlayerUnits[0].lucky.transmigrate - playerUnit.lucky.transmigrate);
      bool flag = this.CheckEnabledButton(price);
      ((UIButtonColor) this.transBtn).isEnabled = flag;
      ((UIButtonColor) this.recordBtn).isEnabled = flag;
      ((UIButtonColor) this.ibtn_Save_Memory).isEnabled = playerUnit.level == playerUnit.max_level;
      prefabF = (Future<GameObject>) null;
      unitIconPrefabF = (Future<GameObject>) null;
      unitIconPrefab = (GameObject) null;
    }
    if (Object.op_Equality((Object) this.statusDetailPrefab, (Object) null))
    {
      unitIconPrefabF = Res.Prefabs.unit.dir_unit_status_detail.Load<GameObject>();
      e = unitIconPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.statusDetailPrefab = unitIconPrefabF.Result;
      unitIconPrefabF = (Future<GameObject>) null;
    }
    yield return (object) this.LoadUnitLargeSprite(this.baseUnit);
    if (!Singleton<TutorialRoot>.GetInstance().IsTutorialFinish())
      Singleton<TutorialRoot>.GetInstance().CurrentAdvise();
  }

  private IEnumerator LoadUnitLargeSprite(PlayerUnit playerUnit)
  {
    Future<Sprite> futureSprite = playerUnit.unit.LoadSpriteLarge(playerUnit.job_id, 1f);
    IEnumerator e = futureSprite.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.unitLargeSprite = futureSprite.Result;
  }

  public virtual bool CheckEnabledButton(int money)
  {
    this.comShortage.SetActive(false);
    bool flag = false;
    Player player = SMManager.Get<Player>();
    if ((long) money > player.money)
    {
      flag = true;
      ((IEnumerable<GameObject>) this.comShortages).ToggleOnce(0);
      this.isMoney = false;
    }
    if (!this.isUnit)
    {
      flag = true;
      ((IEnumerable<GameObject>) this.comShortages).ToggleOnce(1);
    }
    if (!this.isLevel)
    {
      flag = true;
      ((IEnumerable<GameObject>) this.comShortages).ToggleOnce(2);
    }
    if (!this.isFavorite)
    {
      flag = true;
      ((IEnumerable<GameObject>) this.comShortages).ToggleOnce(3);
    }
    if (!this.isPlusValue)
    {
      flag = true;
      ((IEnumerable<GameObject>) this.comShortages).ToggleOnce(4);
    }
    this.comShortage.SetActive(flag);
    return this.isMoney && this.isUnit && this.isLevel && this.isFavorite && this.isPlusValue;
  }

  private void ShowMaterialQuestInfo(UnitUnit materail)
  {
    int num = !this.DialogBox.activeInHierarchy ? 1 : 0;
    this.DialogBox.SetActive(true);
    if (num != 0)
    {
      UITweener[] tweeners = NGTween.findTweeners(this.DialogBox, true);
      NGTween.playTweens(tweeners, NGTween.Kind.START_END);
      NGTween.playTweens(tweeners, NGTween.Kind.START);
      foreach (UITweener uiTweener in tweeners)
        uiTweener.onFinished.Clear();
    }
    this.TxtMaterialName.SetText(materail.name);
    UnitMaterialQuestInfo materialQuestInfo = ((IEnumerable<UnitMaterialQuestInfo>) MasterData.UnitMaterialQuestInfoList).SingleOrDefault<UnitMaterialQuestInfo>((Func<UnitMaterialQuestInfo, bool>) (x => x.unit_id == materail.ID));
    if (materialQuestInfo == null)
      this.TxtMaterialPlace.SetText("");
    else
      this.TxtMaterialPlace.SetText(materialQuestInfo.long_desc);
  }

  public UITweener[] EndTweensMaterialQuestInfo(bool isForce = false)
  {
    if (!this.DialogBox.activeInHierarchy)
      return (UITweener[]) null;
    UITweener[] tweeners = NGTween.findTweeners(this.DialogBox, true);
    if (!isForce && ((IEnumerable<UITweener>) tweeners).Any<UITweener>((Func<UITweener, bool>) (x => ((Behaviour) x).enabled)))
      return (UITweener[]) null;
    NGTween.playTweens(tweeners, NGTween.Kind.START_END, true);
    NGTween.playTweens(tweeners, NGTween.Kind.END);
    return tweeners;
  }

  public void HideMaterialQuestInfo()
  {
    UITweener[] tweens = this.EndTweensMaterialQuestInfo();
    if (tweens == null)
      return;
    NGTween.setOnTweenFinished(tweens, (MonoBehaviour) this, "HideDialogBox");
  }

  private void HideDialogBox() => this.DialogBox.SetActive(false);

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    if (this.exceptionBackScene != null)
      this.exceptionBackScene();
    else
      this.backScene();
    Singleton<PopupManager>.GetInstance().closeAll();
  }

  public override void onBackButton() => this.IbtnBack();

  public virtual void IbtnCom()
  {
    if (!this.isUnit || !this.isMoney || !this.isLevel || !this.isFavorite || this.evolutionMenu.isMovingIndicator || this.IsPushAndSet())
      return;
    this.StartCoroutine(this.IbtnComAsync());
  }

  private IEnumerator IbtnComAsync()
  {
    Unit00499Menu unit00499Menu = this;
    if (unit00499Menu.baseUnit.tower_is_entry || unit00499Menu.baseUnit.corps_is_entry || ((IEnumerable<PlayerUnit>) unit00499Menu.getMaterialPlayerUnits()).Any<PlayerUnit>((Func<PlayerUnit, bool>) (unit => unit.tower_is_entry || unit.corps_is_entry)))
    {
      bool isRejected = false;
      yield return (object) PopupManager.StartTowerEntryUnitWarningPopupProc((Action<bool>) (selected => isRejected = !selected));
      if (isRejected)
        yield break;
    }
    Consts instance = Consts.GetInstance();
    string message = instance.unit_004_9_9_confirm_evolution_message;
    if (unit00499Menu.gvgCostInfo != Unit00499Scene.GvgCostInfo.None)
    {
      string str = string.Empty;
      if (unit00499Menu.gvgCostInfo == Unit00499Scene.GvgCostInfo.Attack)
        str = instance.unit_004_9_9_confirm_evolution_gvg_cost_over_attack_message;
      else if (unit00499Menu.gvgCostInfo == Unit00499Scene.GvgCostInfo.Defense)
        str = instance.unit_004_9_9_confirm_evolution_gvg_cost_over_defense_message;
      else if (unit00499Menu.gvgCostInfo == Unit00499Scene.GvgCostInfo.Both)
        str = instance.unit_004_9_9_confirm_evolution_gvg_cost_over_both_message;
      message = Consts.Format(instance.unit_004_9_9_confirm_evolution_gvg_cost_over_message, (IDictionary) new Hashtable()
      {
        {
          (object) "deck",
          (object) str
        }
      });
    }
    // ISSUE: reference to a compiler-generated method
    if ((PlayerTransmigrateMemoryPlayerUnitIds.Current != null ? (IEnumerable<int?>) PlayerTransmigrateMemoryPlayerUnitIds.Current.transmigrate_memory_player_unit_ids : (IEnumerable<int?>) new int?[0]).Any<int?>(new Func<int?, bool>(unit00499Menu.\u003CIbtnComAsync\u003Eb__94_1)))
      message += instance.unit_004_9_confirm_memory_alert;
    yield return (object) unit00499Menu.confirmExecute(instance.unit_004_9_9_confirm_evolution_title, message, unit00499Menu.selectEvolutionPatternId, unit00499Menu.Evolution());
  }

  public void IbtnRecord()
  {
    if (PlayerTransmigrateMemoryPlayerUnitIds.Current != null && PlayerTransmigrateMemoryPlayerUnitIds.Current.PlayerUnits().Any<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null && x.id == this.playerUnit.id)))
      Singleton<PopupManager>.GetInstance().open(this.MemorySlotOverwritePrefab).GetComponent<Unit00499SaveMemoryOverwrite>().Initialize(this.playerUnit, new Action(this.RecordEndUpdate));
    else
      this.StartCoroutine(Singleton<PopupManager>.GetInstance().open(this.SaveMemorySlotSelectPrefab).GetComponent<Unit00499SaveMemorySlotSelect>().Initialize(this.playerUnit, new Action(this.RecordEndUpdate)));
  }

  public void RecordEndUpdate()
  {
    this.StartCoroutine(this.Init(this.playerUnit, this.targetPlayerUnits, this.sceneMode, this.fromEarth, this.gvgCostInfo));
  }

  public virtual void IbtnAwake()
  {
    if (!this.isUnit || !this.isMoney || !this.isLevel || !this.isFavorite || this.evolutionMenu.isMovingIndicator || this.IsPushAndSet())
      return;
    Consts instance = Consts.GetInstance();
    string message = instance.unit_004_9_9_confirm_awake_message;
    if (this.gvgCostInfo != Unit00499Scene.GvgCostInfo.None)
    {
      string str = string.Empty;
      if (this.gvgCostInfo == Unit00499Scene.GvgCostInfo.Attack)
        str = instance.unit_004_9_9_confirm_evolution_gvg_cost_over_attack_message;
      else if (this.gvgCostInfo == Unit00499Scene.GvgCostInfo.Defense)
        str = instance.unit_004_9_9_confirm_evolution_gvg_cost_over_defense_message;
      else if (this.gvgCostInfo == Unit00499Scene.GvgCostInfo.Both)
        str = instance.unit_004_9_9_confirm_evolution_gvg_cost_over_both_message;
      message = Consts.Format(instance.unit_004_9_9_confirm_evolution_gvg_cost_over_message, (IDictionary) new Hashtable()
      {
        {
          (object) "deck",
          (object) str
        }
      });
    }
    this.StartCoroutine(this.confirmExecute(instance.unit_004_9_9_confirm_awake_title, message, this.selectEvolutionPatternId, this.Evolution()));
  }

  private IEnumerator confirmExecute(
    string title,
    string message,
    int currentPatternId,
    IEnumerator execute)
  {
    Unit00499Menu unit00499Menu = this;
    bool bDo = false;
    bool bWait = true;
    ModalWindow mv = ModalWindow.ShowYesNo(title, message, (Action) (() =>
    {
      bDo = true;
      bWait = false;
    }), (Action) (() =>
    {
      bWait = false;
      this.IsPush = false;
    }));
    while (bWait)
    {
      if (currentPatternId != unit00499Menu.selectEvolutionPatternId)
      {
        mv.Hide();
        break;
      }
      yield return (object) null;
    }
    if (currentPatternId != unit00499Menu.selectEvolutionPatternId)
      unit00499Menu.IsPush = false;
    else if (bDo)
      unit00499Menu.StartCoroutine(execute);
  }

  public virtual void IbtnTrans()
  {
    if (this.IsPush || !this.isUnit || !this.isMoney || !this.isLevel || !this.isFavorite)
      return;
    this.StartCoroutine(this.IbtnTransAsync());
  }

  private IEnumerator IbtnTransAsync()
  {
    if (this.baseUnit.tower_is_entry || this.baseUnit.corps_is_entry)
    {
      bool isRejected = false;
      yield return (object) PopupManager.StartTowerEntryUnitWarningPopupProc((Action<bool>) (selected => isRejected = !selected), true);
      if (isRejected)
        yield break;
    }
    yield return (object) this.ShowTransmigrationPopup();
  }

  public void IbtnStatusDetail()
  {
    Singleton<PopupManager>.GetInstance().open(this.statusDetailPrefab).GetComponent<Unit004StatusDetailDialog>().Initialize(this.baseUnit, this.unitLargeSprite);
  }

  public void IbtnRecordTrans()
  {
    if (this.IsPush || !this.isUnit || !this.isMoney || !this.isLevel || !this.isFavorite)
      return;
    this.StartCoroutine(this.IbtnRecordTransAsync());
  }

  private IEnumerator IbtnRecordTransAsync()
  {
    Unit00499Menu unit00499Menu = this;
    if (unit00499Menu.playerUnit.tower_is_entry || unit00499Menu.playerUnit.corps_is_entry)
    {
      bool isRejected = false;
      yield return (object) PopupManager.StartTowerEntryUnitWarningPopupProc((Action<bool>) (selected => isRejected = !selected), true);
      if (isRejected)
        yield break;
    }
    if (unit00499Menu.playerUnit.is_memory_over)
    {
      // ISSUE: reference to a compiler-generated method
      ModalWindow.ShowYesNo(Consts.GetInstance().MEMORY_LOAD_ALERT_TITLE, Consts.GetInstance().MEMORY_LOAD_ALERT_DESCRIPTION, new Action(unit00499Menu.\u003CIbtnRecordTransAsync\u003Eb__103_0), (Action) (() => { }));
    }
    else
      yield return (object) unit00499Menu.ShowRecordTransmigrationPopup();
  }

  private enum UnitCondition
  {
    Normal,
    Selected,
    Favarite,
    Organized,
  }

  private class SelectCell
  {
    public PlayerUnit unit_ { get; private set; }

    public void setUnit(PlayerUnit u) => this.unit_ = u;

    public int id_ { get; private set; }

    public Unit00499Menu.UnitCondition condition_ { get; private set; }

    public void setCondition(Unit00499Menu.UnitCondition uc) => this.condition_ = uc;

    public bool isSelectable => this.condition_ == Unit00499Menu.UnitCondition.Normal;

    public int column_ { get; private set; }

    public void setColumn(int column = -1)
    {
      this.column_ = column;
      if (column >= 0)
      {
        this.condition_ = Unit00499Menu.UnitCondition.Selected;
      }
      else
      {
        if (this.condition_ != Unit00499Menu.UnitCondition.Selected)
          return;
        this.condition_ = Unit00499Menu.UnitCondition.Normal;
      }
    }

    public SelectCell(PlayerUnit unit, int id, Unit00499Menu.UnitCondition uc = Unit00499Menu.UnitCondition.Normal)
    {
      this.unit_ = unit;
      this.id_ = id;
      this.condition_ = uc;
      this.column_ = -1;
    }
  }

  private class EvolutionSelectMap
  {
    private Dictionary<int, bool> dicSelectable_;

    public UnitUnit[] samples_ { get; private set; }

    public Dictionary<int, Unit00499Menu.SelectCell[]> sources_ { get; private set; }

    public Unit00499Menu.SelectCell[] selected_ { get; private set; }

    public int selectedCount_ { get; private set; }

    public bool isCompletedSelect => this.selectedCount_ == this.selected_.Length;

    public bool isSelectable(int id) => this.dicSelectable_[id];

    public static Unit00499Menu.EvolutionSelectMap create(
      PlayerUnit baseUnit,
      PlayerUnit[] playerunits,
      PlayerMaterialUnit[] materials,
      UnitUnit[] evosamples)
    {
      if (baseUnit == (PlayerUnit) null || playerunits == null || playerunits.Length == 0 || evosamples == null || evosamples.Length == 0)
        return (Unit00499Menu.EvolutionSelectMap) null;
      Unit00499Menu.EvolutionSelectMap evolutionSelectMap = new Unit00499Menu.EvolutionSelectMap();
      evolutionSelectMap.samples_ = evosamples;
      evolutionSelectMap.selectedCount_ = 0;
      int length = evosamples.Length;
      evolutionSelectMap.selected_ = new Unit00499Menu.SelectCell[length];
      List<int> list = ((IEnumerable<UnitUnit>) evosamples).Select<UnitUnit, int>((Func<UnitUnit, int>) (s => s.ID)).Distinct<int>().ToList<int>();
      Dictionary<int, List<Unit00499Menu.SelectCell>> dictionary = list.ToDictionary<int, int, List<Unit00499Menu.SelectCell>>((Func<int, int>) (k => k), (Func<int, List<Unit00499Menu.SelectCell>>) (k => new List<Unit00499Menu.SelectCell>()));
      evolutionSelectMap.dicSelectable_ = list.ToDictionary<int, int, bool>((Func<int, int>) (k => k), (Func<int, bool>) (k => false));
      int num1 = 1;
      PlayerDeck[] decks = SMManager.Get<PlayerDeck[]>();
      foreach (PlayerUnit playerunit in playerunits)
      {
        List<Unit00499Menu.SelectCell> selectCellList;
        if (baseUnit.id != playerunit.id && dictionary.TryGetValue(playerunit.unit.ID, out selectCellList))
          selectCellList.Add(new Unit00499Menu.SelectCell(playerunit, num1++, Unit00499Menu.getUnitCondition(playerunit, decks)));
      }
      foreach (List<Unit00499Menu.SelectCell> source in dictionary.Values)
      {
        if (source.Any<Unit00499Menu.SelectCell>())
        {
          UnitUnit unit = source.First<Unit00499Menu.SelectCell>().unit_.unit;
          evolutionSelectMap.dicSelectable_[unit.ID] = Unit00499Menu.isUnitSelectable(unit);
        }
      }
      foreach (PlayerMaterialUnit material in materials)
      {
        List<Unit00499Menu.SelectCell> selectCellList;
        if (dictionary.TryGetValue(material.unit.ID, out selectCellList))
        {
          int num2 = material.quantity - (material.id == baseUnit.id ? 1 : 0);
          for (int count = 0; count < num2 && count < length; ++count)
            selectCellList.Add(new Unit00499Menu.SelectCell(PlayerUnit.CreateByPlayerMaterialUnit(material, count), num1++));
        }
      }
      evolutionSelectMap.sources_ = dictionary.ToDictionary<KeyValuePair<int, List<Unit00499Menu.SelectCell>>, int, Unit00499Menu.SelectCell[]>((Func<KeyValuePair<int, List<Unit00499Menu.SelectCell>>, int>) (k => k.Key), (Func<KeyValuePair<int, List<Unit00499Menu.SelectCell>>, Unit00499Menu.SelectCell[]>) (k => k.Value.ToArray()));
      return evolutionSelectMap;
    }

    public void selectAuto(bool ignoreSelectable = true)
    {
      for (int column = 0; column < this.samples_.Length; ++column)
      {
        UnitUnit sample = this.samples_[column];
        if (!ignoreSelectable || !this.isSelectable(sample.ID))
        {
          Unit00499Menu.SelectCell cell = ((IEnumerable<Unit00499Menu.SelectCell>) this.sources_[sample.ID]).FirstOrDefault<Unit00499Menu.SelectCell>((Func<Unit00499Menu.SelectCell, bool>) (s => s.isSelectable));
          if (cell != null)
            this.setSelected(cell, column);
        }
      }
    }

    public void setSelected(Unit00499Menu.SelectCell cell, int column)
    {
      if (cell == null)
      {
        this.setUnselected(column);
      }
      else
      {
        if (cell.column_ == column)
          return;
        this.setUnselected(cell);
        if (column < 0 || column >= this.selected_.Length)
          return;
        this.setUnselected(column);
        this.selected_[column] = cell;
        cell.setColumn(column);
        ++this.selectedCount_;
      }
    }

    public void setUnselected(Unit00499Menu.SelectCell cell) => this.setUnselected(cell.column_);

    public void setUnselected(int column)
    {
      if (column < 0 || column >= this.selected_.Length || this.selected_[column] == null)
        return;
      Unit00499Menu.SelectCell selectCell = this.selected_[column];
      this.selected_[column] = (Unit00499Menu.SelectCell) null;
      selectCell.setColumn();
      --this.selectedCount_;
    }

    public bool[] trySelecting()
    {
      bool[] flagArray = new bool[this.samples_.Length];
      Dictionary<int, Queue<PlayerUnit>> dictionary = new Dictionary<int, Queue<PlayerUnit>>();
      foreach (KeyValuePair<int, Unit00499Menu.SelectCell[]> source in this.sources_)
      {
        Queue<PlayerUnit> playerUnitQueue = new Queue<PlayerUnit>();
        foreach (Unit00499Menu.SelectCell selectCell in source.Value)
          playerUnitQueue.Enqueue(selectCell.unit_);
        dictionary.Add(source.Key, playerUnitQueue);
      }
      for (int index = 0; index < this.samples_.Length; ++index)
      {
        Queue<PlayerUnit> playerUnitQueue = dictionary[this.samples_[index].ID];
        flagArray[index] = playerUnitQueue.Count > 0;
        if (flagArray[index])
          playerUnitQueue.Dequeue();
      }
      return flagArray;
    }

    public bool updateNormalUnit(PlayerUnit[] playerUnits)
    {
      bool flag = false;
      PlayerDeck[] decks = SMManager.Get<PlayerDeck[]>();
      foreach (Unit00499Menu.SelectCell[] source in this.sources_.Values)
      {
        if (source.Length != 0 && ((IEnumerable<Unit00499Menu.SelectCell>) source).First<Unit00499Menu.SelectCell>().unit_.unit.IsNormalUnit)
        {
          foreach (Unit00499Menu.SelectCell selectCell in source)
          {
            Unit00499Menu.SelectCell ic = selectCell;
            ic.setUnit(((IEnumerable<PlayerUnit>) playerUnits).First<PlayerUnit>((Func<PlayerUnit, bool>) (pu => pu.id == ic.unit_.id)));
            Unit00499Menu.UnitCondition unitCondition = Unit00499Menu.getUnitCondition(ic.unit_, decks);
            if (ic.condition_ != unitCondition)
            {
              if (ic.condition_ == Unit00499Menu.UnitCondition.Selected)
              {
                if (unitCondition == Unit00499Menu.UnitCondition.Favarite || unitCondition == Unit00499Menu.UnitCondition.Organized)
                {
                  this.setUnselected(ic.column_);
                  ic.setCondition(unitCondition);
                  flag = true;
                }
              }
              else
              {
                ic.setCondition(unitCondition);
                flag = true;
              }
            }
          }
        }
      }
      return flag;
    }
  }

  private enum TransBonusIndex
  {
    HP,
    ATK,
    MAG,
    DEF,
    MEN,
    SPD,
    TEC,
    LUC,
    MAX,
  }

  private enum TransMode
  {
    Trans,
    Record,
  }
}
