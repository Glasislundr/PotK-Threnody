// Decompiled with JetBrains decompiler
// Type: Unit00468Scene
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
using UniLinq;
using UnityEngine;

#nullable disable
public class Unit00468Scene : NGSceneBase
{
  private readonly int SELL_SCROLLPANEL_BOTTOMANCHOR = 100;
  private readonly int SELL_SCROLLBAR_BOTTOMANCHOR = 162;
  private readonly int FORMATION_SCROLLPANEL_BOTTOMANCHOR = 120;
  private readonly int FORMATION_SCROLLBAR_BOTTOMANCHOR = 140;
  private readonly int POSSESION_SCROLLPANEL_BOTTOMANCHOR;
  private readonly int POSSESION_SCROLLBAR_BOTTOMANCHOR = 60;
  public Unit00468Menu menu00468;
  public Unit004682Menu menu004682;
  public Unit0048Menu menu0048;
  public Unit00481Menu menu00481;
  public Unit00491Menu menu00491;
  public Unit00492Menu menu00492;
  public Unit00410Menu menu00410;
  public Unit00411Menu menu00411;
  public Unit00412Menu menu00412;
  public Unit004431Menu menu004431;
  public Unit00414Menu menu00414;
  public Unit00468GvgMenu menu00468Gvg;
  public Unit00468RaidMenu menu00468Raid;
  public GameObject bottomFromation;
  public GameObject bottomPossesion;
  public GameObject bottomSell;
  public GameObject sortAndFilertObject;
  public UIPanel scrollPanel;
  public UIWidget scrollBarWidget;
  public UIWidget bottomWidget;
  [SerializeField]
  private UIScrollView ScrollView;
  [SerializeField]
  private GameObject objStorage;
  [SerializeField]
  private GameObject objMaterialSell;
  [SerializeField]
  private GameObject objRegression;
  private Unit00410Menu.FromType fromType = Unit00410Menu.FromType.AlertUnitOver;
  public Unit00468Scene.Mode debugInitialMode = Unit00468Scene.Mode.Unit00411;
  [SerializeField]
  private UILabel textTitle;
  [SerializeField]
  private UILabel textTitleShort;
  [SerializeField]
  private GameObject TopObj;
  [SerializeField]
  private GameObject ShortTopObj;
  [SerializeField]
  private GameObject TransmigrateTopObj;
  private UnitMenuBase currentMenu;
  private Unit00468Scene.ModeFlags isInit = new Unit00468Scene.ModeFlags();
  private int? scrollViewAnchorBottomAbsolute_;
  private int? scrollBarAnchorBottomAbsolute_;
  private CommonRoot.HeaderType? backupHeader;

  public override IEnumerator onInitSceneAsync()
  {
    this.isInit.Clear();
    yield break;
  }

  public override IEnumerator Start()
  {
    Unit00468Scene unit00468Scene = this;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = unit00468Scene.\u003C\u003En__0();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    List<NGMenuBase> source = new List<NGMenuBase>()
    {
      (NGMenuBase) unit00468Scene.menu00468,
      (NGMenuBase) unit00468Scene.menu004682,
      (NGMenuBase) unit00468Scene.menu0048,
      (NGMenuBase) unit00468Scene.menu00481,
      (NGMenuBase) unit00468Scene.menu00491,
      (NGMenuBase) unit00468Scene.menu00410,
      (NGMenuBase) unit00468Scene.menu00411,
      (NGMenuBase) unit00468Scene.menu00412,
      (NGMenuBase) unit00468Scene.menu004431,
      (NGMenuBase) unit00468Scene.menu00492,
      (NGMenuBase) unit00468Scene.menu00414,
      (NGMenuBase) unit00468Scene.menu00468Gvg,
      (NGMenuBase) unit00468Scene.menu00468Raid
    };
    unit00468Scene.menuBases = source.Where<NGMenuBase>((Func<NGMenuBase, bool>) (m => Object.op_Inequality((Object) m, (Object) null))).ToArray<NGMenuBase>();
    if (unit00468Scene.menuBases != null)
      ((IEnumerable<NGMenuBase>) unit00468Scene.menuBases).ForEach<NGMenuBase>((Action<NGMenuBase>) (x =>
      {
        if (!Object.op_Inequality((Object) x, (Object) null))
          return;
        ((Behaviour) x).enabled = false;
      }));
    if (Object.op_Inequality((Object) unit00468Scene.currentMenu, (Object) null))
      ((Behaviour) unit00468Scene.currentMenu).enabled = true;
  }

  public static void changeScene(bool bStack, EditUnitParam param)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_6_8", (bStack ? 1 : 0) != 0, (object) param);
  }

  public static void changeScene00468(bool stack, PlayerDeck playerDeck)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_6_8", (stack ? 1 : 0) != 0, (object) Unit00468Scene.Mode.Unit00468, (object) playerDeck);
  }

  public static void changeScene00468GvgAtk(bool stack, GvgDeck gvgDeck)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_6_8", (stack ? 1 : 0) != 0, (object) Unit00468Scene.Mode.Unit00468GvgAtk, (object) gvgDeck);
  }

  public static void changeScene00468GvgDef(bool stack, GvgDeck gvgDeck)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_6_8", (stack ? 1 : 0) != 0, (object) Unit00468Scene.Mode.Unit00468GvgDef, (object) gvgDeck);
  }

  public static void changeScene00468Raid(bool stack, bool isSimulated)
  {
    Unit00468Scene.Mode mode = isSimulated ? Unit00468Scene.Mode.Unit00468RaidSim : Unit00468Scene.Mode.Unit00468Raid;
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_6_8", (stack ? 1 : 0) != 0, (object) mode);
  }

  public static void changeScene00412Raid(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_6_8", (stack ? 1 : 0) != 0, (object) Unit00468Scene.Mode.Unit00412Raid);
  }

  public static void changeScene004682(bool stack, Unit0046Menu.OneFormationInfo info)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_6_8", (stack ? 1 : 0) != 0, (object) Unit00468Scene.Mode.Unit004682, (object) info);
  }

  public static void changeScene0048(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_UnitTraining_List", stack);
  }

  public static void changeScene00481Reinforce(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_6_8", (stack ? 1 : 0) != 0, (object) Unit00468Scene.Mode.Unit00481);
  }

  public static void changeScene00491Evolution(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_UnitTraining_List", stack);
  }

  public static void changeScene00492EvolutionMaterial(bool stack, Unit00492Menu.Param param)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_6_8_2", (stack ? 1 : 0) != 0, (object) Unit00468Scene.Mode.Unit00492EvoMaterial, (object) param);
  }

  public static void changeScene00491Trans(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_6_8", (stack ? 1 : 0) != 0, (object) Unit00468Scene.Mode.Unit00491Trans);
  }

  public static void changeScene00410(bool stack, Unit00410Menu.FromType fromtype)
  {
    Unit00468Scene.callChangeSale(stack, fromtype, false);
  }

  public static void changeScene00410WithInitialize(bool stack, Unit00410Menu.FromType fromtype)
  {
    Unit00468Scene.callChangeSale(stack, fromtype, true);
  }

  private static void callChangeSale(bool stack, Unit00410Menu.FromType fromtype, bool bInitialize)
  {
    string sceneName = fromtype == Unit00410Menu.FromType.AlertUnitOver || fromtype == Unit00410Menu.FromType.MaterialList ? "unit004_unit_sale" : "unit004_6_8";
    Singleton<NGSceneManager>.GetInstance().changeScene(sceneName, (stack ? 1 : 0) != 0, (object) fromtype, (object) bInitialize);
  }

  public static void changeScene00411(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_unit_list", (stack ? 1 : 0) != 0, (object) Unit00468Scene.Mode.Unit00411);
  }

  public static void changeScene00411WithInitialize(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_unit_list", (stack ? 1 : 0) != 0, (object) Unit00468Scene.Mode.Unit00411, (object) true);
  }

  public static void changeScene00414WithInitialize(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_6_8", (stack ? 1 : 0) != 0, (object) Unit00468Scene.Mode.Unit00414, (object) true);
  }

  public static void changeScene00412(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_6_8", (stack ? 1 : 0) != 0, (object) Unit00468Scene.Mode.Unit00412);
  }

  public static void changeScene00412Gvg(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_6_8", (stack ? 1 : 0) != 0, (object) Unit00468Scene.Mode.Unit00412Gvg);
  }

  public static void changeScene004431(bool stack, Unit004431Menu.Param sendParam)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_6_8", (stack ? 1 : 0) != 0, (object) Unit00468Scene.Mode.Unit004431, (object) sendParam);
  }

  public static void changeScene00414(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_6_8", (stack ? 1 : 0) != 0, (object) Unit00468Scene.Mode.Unit00414);
  }

  public void onStartScene() => this.StartCoroutine(this.hideLoading());

  public void onStartScene(EditUnitParam param) => this.StartCoroutine(this.hideLoading());

  public void onStartScene(Unit00468Scene.Mode mode, PlayerDeck playerDeck)
  {
    this.StartCoroutine(this.hideLoading());
  }

  public void onStartScene(Unit00468Scene.Mode mode, GvgDeck gvgDeck)
  {
    this.StartCoroutine(this.hideLoading());
  }

  public void onStartScene(Unit00410Menu.FromType type, bool clearInitFlg)
  {
    this.StartCoroutine(this.hideLoading());
  }

  public void onStartScene(Unit00468Scene.Mode mode, Unit0046Menu.OneFormationInfo info)
  {
    this.StartCoroutine(this.hideLoading());
  }

  public void onStartScene(Unit00468Scene.Mode mode, Unit004431Menu.Param sendParam)
  {
    this.StartCoroutine(this.hideLoading());
  }

  public void onStartScene(Unit00468Scene.Mode mode, Unit00492Menu.Param param)
  {
    this.StartCoroutine(this.hideLoading());
  }

  public void onStartScene(Unit00468Scene.Mode mode) => this.StartCoroutine(this.hideLoading());

  public IEnumerator onStartSceneAsync()
  {
    IEnumerator e = this.onStartSceneAsync(this.debugInitialMode, SMManager.Get<PlayerDeck[]>()[0]);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(EditUnitParam param)
  {
    Unit00468Scene.Mode mode = param.isMulti ? Unit00468Scene.Mode.Unit00468 : Unit00468Scene.Mode.Unit004682;
    this.SetTitle(mode);
    Player player = Player.Current;
    IEnumerator e;
    switch (mode)
    {
      case Unit00468Scene.Mode.Unit00468:
        if (!this.isInit[mode])
        {
          e = this.menu00468.UpdateInfoAndScroll(param.units);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          this.menu00468.UpdateInfomation();
          yield break;
        }
        else
        {
          e = this.changeMenu((UnitMenuBase) this.menu00468);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          e = this.menu00468.Init(param, player.max_cost);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          break;
        }
      case Unit00468Scene.Mode.Unit004682:
        if (!this.isInit[mode])
        {
          e = this.menu004682.UpdateInfoAndScroll(param.units);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          yield break;
        }
        else
        {
          e = this.changeMenu((UnitMenuBase) this.menu004682);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          e = this.menu004682.Init(param);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          break;
        }
    }
    if (Singleton<NGGameDataManager>.GetInstance().IsColosseum)
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
    this.isInit[mode] = false;
  }

  public IEnumerator onStartSceneAsync(Unit00468Scene.Mode mode, PlayerDeck playerDeck)
  {
    this.SetTitle(mode);
    Player player = SMManager.Get<Player>();
    PlayerUnit[] playerUnits = SMManager.Get<PlayerUnit[]>();
    NGGameDataManager instance = Singleton<NGGameDataManager>.GetInstance();
    if (!instance.IsSea || instance.QuestType.HasValue)
    {
      CommonQuestType? questType = instance.QuestType;
      CommonQuestType commonQuestType = CommonQuestType.Sea;
      if (!(questType.GetValueOrDefault() == commonQuestType & questType.HasValue))
        goto label_6;
    }
    playerUnits = ((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.IsSea)).ToArray<PlayerUnit>();
    IEnumerator e = this.SetSeaBgm();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
label_6:
    if (!this.isInit[mode])
    {
      e = this.menu00468.UpdateInfoAndScroll(((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.IsNormalUnit)).ToArray<PlayerUnit>());
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.menu00468.UpdateInfomation();
    }
    else
    {
      if (Singleton<NGGameDataManager>.GetInstance().IsColosseum)
        Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
      e = this.changeMenu((UnitMenuBase) this.menu00468);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (mode == Unit00468Scene.Mode.Unit00468withFocusLastReference)
        this.menu00468.setLastReference(Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitID);
      e = this.menu00468.Init(playerDeck, playerUnits, player.max_cost, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.isInit[mode] = false;
    }
  }

  public IEnumerator onStartSceneAsync(Unit00468Scene.Mode mode, GvgDeck gvgDeck)
  {
    this.SetTitle(mode);
    Player player = SMManager.Get<Player>();
    PlayerUnit[] playerUnits = SMManager.Get<PlayerUnit[]>();
    GvgDeck deck = gvgDeck;
    IEnumerator e;
    if (GuildUtil.IsCostOver(playerUnits, gvgDeck.player_unit_ids))
    {
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      yield return (object) null;
      e = this.menu00468Gvg.UpdateGvgDeck(mode, player.id, PlayerAffiliation.Current.guild_id);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.isInit[mode] = true;
      this.menu00468Gvg.SelectedUnitIcons.Clear();
      deck = mode == Unit00468Scene.Mode.Unit00468GvgAtk ? GuildUtil.gvgDeckAttack : GuildUtil.gvgDeckDefense;
    }
    if (!this.isInit[mode])
    {
      e = this.menu00468Gvg.UpdateInfoAndScroll(((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.IsNormalUnit)).ToArray<PlayerUnit>());
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.menu00468Gvg.UpdateInfomation();
      Singleton<CommonRoot>.GetInstance().isLoading = false;
    }
    else
    {
      if (Singleton<NGGameDataManager>.GetInstance().IsColosseum)
        Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
      e = this.changeMenu((UnitMenuBase) this.menu00468Gvg);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = this.menu00468Gvg.Init(deck, playerUnits, player.max_cost, false, mode == Unit00468Scene.Mode.Unit00468GvgAtk ? GuildUtil.GvGPopupState.AtkTeam : GuildUtil.GvGPopupState.DefTeam);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      yield return (object) new WaitForEndOfFrame();
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      this.isInit[mode] = false;
    }
  }

  public IEnumerator onStartSceneAsync(Unit00410Menu.FromType type, bool clearInitFlg)
  {
    Unit00468Scene unit00468Scene = this;
    unit00468Scene.fromType = type;
    if (clearInitFlg)
      yield return (object) unit00468Scene.onStartSceneAsync(Unit00468Scene.Mode.Unit00410, true);
    else
      yield return (object) unit00468Scene.onStartSceneAsync(Unit00468Scene.Mode.Unit00410);
    unit00468Scene.StartCoroutine(unit00468Scene.hideLoading());
  }

  public IEnumerator onStartSceneAsync(Unit00468Scene.Mode mode, Unit0046Menu.OneFormationInfo info)
  {
    this.SetTitle(mode);
    Player player = SMManager.Get<Player>();
    PlayerUnit[] playerUnits = SMManager.Get<PlayerUnit[]>();
    NGGameDataManager instance = Singleton<NGGameDataManager>.GetInstance();
    if (!instance.IsSea || instance.QuestType.HasValue)
    {
      CommonQuestType? questType = instance.QuestType;
      CommonQuestType commonQuestType = CommonQuestType.Sea;
      if (!(questType.GetValueOrDefault() == commonQuestType & questType.HasValue))
        goto label_6;
    }
    playerUnits = ((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.IsSea)).ToArray<PlayerUnit>();
    IEnumerator e = this.SetSeaBgm();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
label_6:
    if (!this.isInit[mode])
    {
      e = this.menu004682.UpdateInfoAndScroll(((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.IsNormalUnit)).ToArray<PlayerUnit>());
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      if (Singleton<NGGameDataManager>.GetInstance().IsColosseum)
        Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
      e = this.changeMenu((UnitMenuBase) this.menu004682);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (mode == Unit00468Scene.Mode.Unit004682withFocusLastReference)
        this.menu004682.setLastReference(Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitID);
      e = this.menu004682.Init(player, info.playerDeck, playerUnits, player.max_cost, info.num, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.isInit[mode] = false;
    }
  }

  public IEnumerator onStartSceneAsync(Unit00468Scene.Mode mode, Unit004431Menu.Param sendParam)
  {
    Unit00468Scene unit00468Scene = this;
    unit00468Scene.SetTitle(mode);
    Player player = SMManager.Get<Player>();
    PlayerUnit[] playerUnits = SMManager.Get<PlayerUnit[]>();
    IEnumerator e;
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      unit00468Scene.headerType = CommonRoot.HeaderType.Normal;
      playerUnits = ((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.IsSea)).ToArray<PlayerUnit>();
      e = unit00468Scene.SetSeaBgm();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    if (!unit00468Scene.isInit[mode])
    {
      e = unit00468Scene.menu004431.updateMenu(playerUnits);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = unit00468Scene.changeMenu((UnitMenuBase) unit00468Scene.menu004431);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = unit00468Scene.menu004431.Init(player, playerUnits, sendParam, true);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unit00468Scene.isInit[mode] = false;
    }
  }

  public IEnumerator onStartSceneAsync(Unit00468Scene.Mode mode, Unit00492Menu.Param param)
  {
    IEnumerator e;
    if (mode != Unit00468Scene.Mode.Unit00492EvoMaterial)
    {
      e = this.onStartSceneAsync(mode);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      this.SetTitle(mode);
      if (!this.isInit[mode])
      {
        e = this.menu00492.coUpdateUnits(SMManager.Get<PlayerUnit[]>());
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
      {
        e = this.changeMenu((UnitMenuBase) this.menu00492);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        e = this.menu00492.coInitialize(param);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.isInit[mode] = false;
      }
    }
  }

  public IEnumerator onStartSceneAsync(Unit00468Scene.Mode mode)
  {
    this.SetTitle(mode);
    if (!this.isInit[mode])
    {
      yield return (object) this.reloadMenu(mode);
    }
    else
    {
      yield return (object) this.initMenu(mode);
      this.isInit[mode] = false;
    }
  }

  private IEnumerator initMenu(Unit00468Scene.Mode mode)
  {
    Player player = SMManager.Get<Player>();
    PlayerUnit[] playerUnits = SMManager.Get<PlayerUnit[]>();
    PlayerMaterialUnit[] playerMaterialUnits = SMManager.Get<PlayerMaterialUnit[]>();
    PlayerDeck[] decks;
    PlayerUnit[] array;
    switch (mode)
    {
      case Unit00468Scene.Mode.Unit0048:
        this.menu0048.mode = Unit00468Scene.Mode.Unit0048;
        Singleton<NGGameDataManager>.GetInstance().clearPreviewInheritance();
        yield return (object) this.changeMenu((UnitMenuBase) this.menu0048);
        yield return (object) this.menu0048.Init(player, playerUnits, false);
        break;
      case Unit00468Scene.Mode.Unit00481:
        this.menu00481.mode = Unit00468Scene.Mode.Unit00481;
        yield return (object) this.changeMenu((UnitMenuBase) this.menu00481);
        yield return (object) this.menu00481.Init(player, playerUnits, false);
        break;
      case Unit00468Scene.Mode.Unit00491Evo:
        yield return (object) ServerTime.WaitSync();
        yield return (object) this.changeMenu((UnitMenuBase) this.menu00491);
        yield return (object) this.menu00491.Init(player, playerUnits, playerMaterialUnits, false, Unit00491Menu.Mode.Evolution);
        break;
      case Unit00468Scene.Mode.Unit00491Trans:
        yield return (object) this.changeMenu((UnitMenuBase) this.menu00491);
        if (Object.op_Inequality((Object) this.objRegression, (Object) null))
          this.objRegression.SetActive(false);
        yield return (object) this.menu00491.Init(player, playerUnits, playerMaterialUnits, false, Unit00491Menu.Mode.Trans);
        this.TopObj.SetActive(false);
        this.ShortTopObj.SetActive(false);
        this.TransmigrateTopObj.SetActive(true);
        break;
      case Unit00468Scene.Mode.Unit00410:
        decks = SMManager.Get<PlayerDeck[]>();
        yield return (object) this.changeMenu((UnitMenuBase) this.menu00410);
        yield return (object) this.menu00410.Init(decks, player, playerUnits, playerMaterialUnits, false, this.fromType);
        switch (this.fromType)
        {
          case Unit00410Menu.FromType.UnitList:
            this.TopObj.SetActive(false);
            this.ShortTopObj.SetActive(true);
            break;
          case Unit00410Menu.FromType.MaterialList:
            this.TopObj.SetActive(true);
            this.ShortTopObj.SetActive(false);
            this.sortAndFilertObject.SetActive(false);
            break;
          case Unit00410Menu.FromType.AlertUnitOver:
            this.TopObj.SetActive(false);
            this.ShortTopObj.SetActive(true);
            break;
        }
        break;
      case Unit00468Scene.Mode.Unit00411:
      case Unit00468Scene.Mode.Unit00411withFocusLastReference:
        yield return (object) this.changeMenu((UnitMenuBase) this.menu00411);
        if (mode == Unit00468Scene.Mode.Unit00411withFocusLastReference)
          this.menu00411.setLastReference(Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitID);
        yield return (object) this.menu00411.Init(player, playerUnits, false);
        break;
      case Unit00468Scene.Mode.Unit00412:
        yield return (object) this.changeMenu((UnitMenuBase) this.menu00412);
        yield return (object) this.menu00412.Init(player, playerUnits, true);
        break;
      case Unit00468Scene.Mode.Unit00412Gvg:
        array = (PlayerUnit[]) null;
        switch (GuildUtil.gvgPopupState)
        {
          case GuildUtil.GvGPopupState.AtkTeam:
            array = GuildUtil.gvgDeckAttack.player_units;
            break;
          case GuildUtil.GvGPopupState.DefTeam:
            array = GuildUtil.gvgDeckDefense.player_units;
            break;
        }
        this.menu00412.isBottomViewPossesion = false;
        yield return (object) this.changeMenu((UnitMenuBase) this.menu00412);
        yield return (object) this.menu00412.Init(player, array, true, false);
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        break;
      case Unit00468Scene.Mode.Unit00414:
        ((Behaviour) this.menu00410).enabled = false;
        yield return (object) this.changeMenu((UnitMenuBase) this.menu00414);
        yield return (object) this.menu00414.Init(player, playerMaterialUnits, true);
        break;
      case Unit00468Scene.Mode.Unit00468Raid:
      case Unit00468Scene.Mode.Unit00468RaidSim:
        bool isSim = mode == Unit00468Scene.Mode.Unit00468RaidSim;
        GuildUtil.RaidDeck = ((IEnumerable<PlayerUnit>) GuildUtil.RaidDeck).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => !((IEnumerable<int>) GuildUtil.RaidUsedUnitIds).Contains<int>(x.id))).ToArray<PlayerUnit>();
        yield return (object) this.changeMenu((UnitMenuBase) this.menu00468Raid);
        yield return (object) this.menu00468Raid.Init(GuildUtil.RaidDeck, playerUnits, GuildUtil.RaidUsedUnitIds, isSim);
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        break;
      case Unit00468Scene.Mode.Unit00412Raid:
        this.menu00412.isBottomViewPossesion = false;
        yield return (object) this.changeMenu((UnitMenuBase) this.menu00412);
        yield return (object) this.menu00412.Init(player, GuildUtil.RaidDeck, true, false);
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        break;
      default:
        Debug.LogError((object) ("init " + (object) mode + " is Undefined"));
        break;
    }
    decks = (PlayerDeck[]) null;
    array = (PlayerUnit[]) null;
  }

  private IEnumerator reloadMenu(Unit00468Scene.Mode mode)
  {
    Player player = SMManager.Get<Player>();
    PlayerUnit[] playerUnits = SMManager.Get<PlayerUnit[]>();
    PlayerMaterialUnit[] playerMaterialUnitArray = SMManager.Get<PlayerMaterialUnit[]>();
    switch (mode)
    {
      case Unit00468Scene.Mode.Unit0048:
        yield return (object) this.menu0048.UpdateInfoAndScroll(((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.IsNormalUnit)).ToArray<PlayerUnit>(), (PlayerMaterialUnit[]) null);
        break;
      case Unit00468Scene.Mode.Unit00481:
        yield return (object) this.menu00481.UpdateInfoAndScroll(((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.IsNormalUnit && x.buildup_limit > 0)).ToArray<PlayerUnit>(), (PlayerMaterialUnit[]) null);
        break;
      case Unit00468Scene.Mode.Unit00491Evo:
        yield return (object) this.menu00491.UpdateInfoAndScroll(((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.IsEvolution || x.unit.CanAwakeUnitFlag)).ToArray<PlayerUnit>(), ((IEnumerable<PlayerMaterialUnit>) playerMaterialUnitArray).Where<PlayerMaterialUnit>((Func<PlayerMaterialUnit, bool>) (x => x.unit.IsEvolution)).ToArray<PlayerMaterialUnit>());
        this.menu00491.EnableTouch();
        break;
      case Unit00468Scene.Mode.Unit00491Trans:
        yield return (object) this.menu00491.UpdateInfoAndScroll(((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x =>
        {
          if (x.level >= x.unit.rarity.reincarnation_level)
            return true;
          return PlayerTransmigrateMemoryPlayerUnitIds.Current != null && ((IEnumerable<int?>) PlayerTransmigrateMemoryPlayerUnitIds.Current.transmigrate_memory_player_unit_ids).Any<int?>((Func<int?, bool>) (y =>
          {
            int? nullable = y;
            int id = x.id;
            return nullable.GetValueOrDefault() == id & nullable.HasValue;
          }));
        })).ToArray<PlayerUnit>(), (PlayerMaterialUnit[]) null);
        this.menu00491.EnableTouch();
        break;
      case Unit00468Scene.Mode.Unit00410:
        PlayerUnit[] playerUnits1 = playerUnits;
        PlayerMaterialUnit[] playerMaterialUnits = playerMaterialUnitArray;
        switch (this.fromType)
        {
          case Unit00410Menu.FromType.UnitList:
            playerMaterialUnits = new PlayerMaterialUnit[0];
            break;
          case Unit00410Menu.FromType.MaterialList:
            playerUnits1 = new PlayerUnit[0];
            break;
          case Unit00410Menu.FromType.AlertUnitOver:
            playerMaterialUnits = new PlayerMaterialUnit[0];
            break;
        }
        yield return (object) this.menu00410.UpdateInfoAndScroll(playerUnits1, playerMaterialUnits);
        this.menu00410.AllUpdateUnitIcons();
        this.menu00410.UpdateInfomation();
        break;
      case Unit00468Scene.Mode.Unit00411:
      case Unit00468Scene.Mode.Unit00411withFocusLastReference:
        yield return (object) this.menu00411.UpdateInfoAndScroll(playerUnits, (PlayerMaterialUnit[]) null);
        this.menu00411.SetTextPosession(playerUnits, player);
        break;
      case Unit00468Scene.Mode.Unit00412:
        yield return (object) this.menu00412.UpdateInfoAndScroll(((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.IsNormalUnit)).ToArray<PlayerUnit>());
        break;
      case Unit00468Scene.Mode.Unit00412Gvg:
        PlayerUnit[] playerUnits2 = (PlayerUnit[]) null;
        switch (GuildUtil.gvgPopupState)
        {
          case GuildUtil.GvGPopupState.AtkTeam:
            playerUnits2 = GuildUtil.gvgDeckAttack.player_units;
            break;
          case GuildUtil.GvGPopupState.DefTeam:
            playerUnits2 = GuildUtil.gvgDeckDefense.player_units;
            break;
        }
        yield return (object) this.menu00412.UpdateInfoAndScroll(playerUnits2);
        break;
      case Unit00468Scene.Mode.Unit00414:
        yield return (object) this.menu00414.UpdateInfoAndScroll((PlayerUnit[]) null, playerMaterialUnitArray);
        break;
      case Unit00468Scene.Mode.Unit00468Raid:
      case Unit00468Scene.Mode.Unit00468RaidSim:
        yield return (object) this.menu00468Raid.UpdateInfoAndScroll(((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.IsNormalUnit)).ToArray<PlayerUnit>());
        this.menu00468Raid.UpdateInfomation();
        break;
      case Unit00468Scene.Mode.Unit00412Raid:
        GuildUtil.UpdateRaidDeckInfo();
        yield return (object) this.menu00412.UpdateInfoAndScroll(GuildUtil.RaidDeck);
        break;
      default:
        Debug.LogError((object) ("reload " + (object) mode + " is Undefined"));
        break;
    }
  }

  public IEnumerator onStartSceneAsync(Unit00468Scene.Mode mode, bool clearInitFlg)
  {
    this.isInit.Clear(clearInitFlg);
    IEnumerator e = this.onStartSceneAsync(mode);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public override void onEndScene()
  {
    Persist.sortOrder.Flush();
    UnitIcon.ClearCache();
  }

  public void IbtnSort()
  {
    if (!Object.op_Inequality((Object) this.currentMenu, (Object) null))
      return;
    this.currentMenu.IbtnSort();
  }

  public void IbtnBack()
  {
    if (!Object.op_Inequality((Object) this.currentMenu, (Object) null))
      return;
    this.currentMenu.IbtnBack();
  }

  public void IbtnYes()
  {
    if (!Object.op_Inequality((Object) this.currentMenu, (Object) null))
      return;
    this.currentMenu.IbtnYes();
  }

  public void IbtnClearS()
  {
    if (!Object.op_Inequality((Object) this.currentMenu, (Object) null))
      return;
    this.currentMenu.IbtnClearS();
  }

  private IEnumerator changeMenu(UnitMenuBase menu)
  {
    this.bottomSell.SetActive(menu.isBottomViewSell);
    this.bottomFromation.SetActive(menu.isBottomViewFormation);
    this.bottomPossesion.SetActive(menu.isBottomViewPossesion);
    if (Object.op_Inequality((Object) this.objStorage, (Object) null))
      this.objStorage.SetActive(menu.isStorageButton);
    if (Object.op_Inequality((Object) this.objMaterialSell, (Object) null))
      this.objMaterialSell.SetActive(menu.isMaterialButton);
    if (Object.op_Inequality((Object) this.objRegression, (Object) null))
      this.objRegression.SetActive(menu.isRegressionButton);
    if (Object.op_Inequality((Object) this.sortAndFilertObject, (Object) null))
      this.sortAndFilertObject.SetActive(!menu.isHideSortAndFilterButton);
    if (Object.op_Inequality((Object) this.TopObj, (Object) null) && Object.op_Inequality((Object) this.ShortTopObj, (Object) null))
    {
      this.TopObj.SetActive(!menu.isStorageButton);
      this.ShortTopObj.SetActive(menu.isStorageButton);
    }
    if (menu.isBottomViewSell)
      this.setAnchorBottomAbsolute(this.SELL_SCROLLPANEL_BOTTOMANCHOR, this.SELL_SCROLLBAR_BOTTOMANCHOR);
    else if (menu.isBottomViewFormation)
      this.setAnchorBottomAbsolute(this.FORMATION_SCROLLPANEL_BOTTOMANCHOR, this.FORMATION_SCROLLBAR_BOTTOMANCHOR);
    else if (menu.isBottomViewPossesion)
      this.setAnchorBottomAbsolute(this.POSSESION_SCROLLPANEL_BOTTOMANCHOR, this.POSSESION_SCROLLBAR_BOTTOMANCHOR);
    else if (menu.isMaterialButton)
      this.setAnchorBottomAbsolute(this.POSSESION_SCROLLPANEL_BOTTOMANCHOR, this.POSSESION_SCROLLBAR_BOTTOMANCHOR);
    else
      this.resetAnchorBottomAbsolute();
    this.currentMenu = menu;
    ((Behaviour) this.currentMenu).enabled = true;
    this.currentMenu.SetIconType(UnitMenuBase.IconType.Normal);
    IEnumerator e = this.currentMenu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private void setAnchorBottomAbsolute(int viewAbsolute, int barAbsolute)
  {
    if (!this.scrollViewAnchorBottomAbsolute_.HasValue)
      this.scrollViewAnchorBottomAbsolute_ = new int?(((UIRect) this.scrollPanel).bottomAnchor.absolute);
    ((UIRect) this.scrollPanel).bottomAnchor.absolute = viewAbsolute;
    if (!this.scrollBarAnchorBottomAbsolute_.HasValue)
      this.scrollBarAnchorBottomAbsolute_ = new int?(((UIRect) this.scrollBarWidget).bottomAnchor.absolute);
    ((UIRect) this.scrollBarWidget).bottomAnchor.absolute = barAbsolute;
  }

  private void resetAnchorBottomAbsolute()
  {
    if (this.scrollViewAnchorBottomAbsolute_.HasValue)
      ((UIRect) this.scrollPanel).bottomAnchor.absolute = this.scrollViewAnchorBottomAbsolute_.Value;
    if (!this.scrollBarAnchorBottomAbsolute_.HasValue)
      return;
    ((UIRect) this.scrollBarWidget).bottomAnchor.absolute = this.scrollBarAnchorBottomAbsolute_.Value;
  }

  private void SetTitle(Unit00468Scene.Mode mode)
  {
    if (this.backupHeader.HasValue)
      this.headerType = this.backupHeader.Value;
    else
      this.backupHeader = new CommonRoot.HeaderType?(this.headerType);
    switch (mode)
    {
      case Unit00468Scene.Mode.Unit00468:
      case Unit00468Scene.Mode.Unit00468withFocusLastReference:
        if (Singleton<NGGameDataManager>.GetInstance().IsSea)
          this.headerType = CommonRoot.HeaderType.Normal;
        this.textTitle.SetTextLocalize(Consts.GetInstance().TitleUnitEdit);
        break;
      case Unit00468Scene.Mode.Unit00468GvgAtk:
      case Unit00468Scene.Mode.Unit00468GvgDef:
      case Unit00468Scene.Mode.Unit00468Raid:
      case Unit00468Scene.Mode.Unit00468RaidSim:
        this.textTitle.SetTextLocalize(Consts.GetInstance().TitleUnitEdit);
        break;
      case Unit00468Scene.Mode.Unit004682:
      case Unit00468Scene.Mode.Unit004682withFocusLastReference:
        if (Singleton<NGGameDataManager>.GetInstance().IsSea)
          this.headerType = CommonRoot.HeaderType.Normal;
        this.textTitle.SetTextLocalize(Consts.GetInstance().TitleUnitEdit);
        break;
      case Unit00468Scene.Mode.Unit0048:
        this.textTitle.SetTextLocalize(Consts.GetInstance().TitleComposeBaseSelect);
        break;
      case Unit00468Scene.Mode.Unit00481:
        this.textTitle.SetTextLocalize(Consts.GetInstance().unit_004_8_4_reinforce_title);
        break;
      case Unit00468Scene.Mode.Unit00491Evo:
        this.textTitle.SetTextLocalize(Consts.GetInstance().TitleEvolutionSelect);
        break;
      case Unit00468Scene.Mode.Unit00491Trans:
        this.textTitle.SetTextLocalize(Consts.GetInstance().unit_004_9_9_evolution_title);
        break;
      case Unit00468Scene.Mode.Unit00410:
        this.textTitle.SetTextLocalize(Consts.GetInstance().TitleUnitBuy);
        this.textTitleShort.SetTextLocalize(Consts.GetInstance().TitleUnitBuy);
        break;
      case Unit00468Scene.Mode.Unit00411:
      case Unit00468Scene.Mode.Unit00411withFocusLastReference:
        this.textTitle.SetTextLocalize(Consts.GetInstance().TitleUnitList);
        this.textTitleShort.SetTextLocalize(Consts.GetInstance().TitleUnitList);
        break;
      case Unit00468Scene.Mode.Unit00412:
      case Unit00468Scene.Mode.Unit00412Gvg:
      case Unit00468Scene.Mode.Unit00412Raid:
        this.textTitle.SetTextLocalize(Consts.GetInstance().TitleEquipGear);
        break;
      case Unit00468Scene.Mode.Unit004431:
        this.textTitle.SetTextLocalize(Consts.GetInstance().TitleEquipGear);
        break;
      case Unit00468Scene.Mode.Unit00486:
        this.textTitle.SetTextLocalize(Consts.GetInstance().TitleComposeMaterialSelect);
        break;
      case Unit00468Scene.Mode.Unit00414:
        this.textTitle.SetTextLocalize(Consts.GetInstance().TitleMaterialUnitList);
        break;
      case Unit00468Scene.Mode.Unit00492EvoMaterial:
        this.textTitle.SetTextLocalize(Consts.GetInstance().TitleEvolutionMaterialSelect);
        break;
    }
  }

  public void onBackScene()
  {
    if (!Singleton<NGGameDataManager>.GetInstance().IsSea)
      return;
    Singleton<CommonRoot>.GetInstance().headerType = CommonRoot.HeaderType.Normal;
    Singleton<CommonRoot>.GetInstance().isActiveHeader = true;
  }

  public void onBackScene(EditUnitParam param) => this.onBackScene();

  public void onBackScene(Unit00468Scene.Mode mode, PlayerDeck playerDeck) => this.onBackScene();

  public void onBackScene(Unit00468Scene.Mode mode, GvgDeck gvgDeck) => this.onBackScene();

  public void onBackScene(Unit00468Scene.Mode mode, Unit0046Menu.OneFormationInfo info)
  {
    this.onBackScene();
  }

  public void onBackScene(Unit00468Scene.Mode mode, Unit004431Menu.Param sendParam)
  {
    this.onBackScene();
  }

  public void onBackScene(Unit00468Scene.Mode mode, Unit00492Menu.Param param)
  {
    this.onBackScene();
  }

  public void onBackScene(Unit00468Scene.Mode mode) => this.onBackScene();

  public void onBackScene(Unit00468Scene.Mode mode, bool flg) => this.onBackScene();

  private IEnumerator SetSeaBgm()
  {
    Unit00468Scene unit00468Scene = this;
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    SeaHomeMap seaHomeMap = ((IEnumerable<SeaHomeMap>) MasterData.SeaHomeMapList).ActiveSeaHomeMap(ServerTime.NowAppTimeAddDelta());
    if (seaHomeMap != null && !string.IsNullOrEmpty(seaHomeMap.bgm_cuesheet_name) && !string.IsNullOrEmpty(seaHomeMap.bgm_cue_name))
    {
      unit00468Scene.bgmFile = seaHomeMap.bgm_cuesheet_name;
      unit00468Scene.bgmName = seaHomeMap.bgm_cue_name;
    }
  }

  protected virtual void OnEnable()
  {
    if (!this.ScrollView.isDragging)
      return;
    this.ScrollView.Press(false);
  }

  private IEnumerator hideLoading()
  {
    yield return (object) new WaitForEndOfFrame();
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public enum Mode
  {
    Unit00468,
    Unit00468GvgAtk,
    Unit00468GvgDef,
    Unit004682,
    Unit0048,
    Unit00481,
    Unit00491Evo,
    Unit00491Trans,
    Unit00410,
    Unit00410Unit,
    Unit00410Material,
    Unit00411,
    Unit00412,
    Unit00412Gvg,
    Unit004431,
    Unit00486,
    Unit00414,
    Unit00492EvoMaterial,
    Unit00420,
    Unit00468Raid,
    Unit00468RaidSim,
    Unit00412Raid,
    UnitLumpTouta,
    Unit00411withFocusLastReference,
    Unit00468withFocusLastReference,
    Unit004682withFocusLastReference,
    Max,
  }

  private class ModeFlags
  {
    private Dictionary<Unit00468Scene.Mode, bool> dicFlag_ = new Dictionary<Unit00468Scene.Mode, bool>();
    private bool defFlag_;

    public ModeFlags(bool def = true) => this.defFlag_ = def;

    public bool this[Unit00468Scene.Mode mode]
    {
      get
      {
        bool flag;
        return this.dicFlag_.TryGetValue(mode, out flag) ? flag : this.defFlag_;
      }
      set => this.dicFlag_[mode] = value;
    }

    public void Clear(bool def = true)
    {
      this.dicFlag_.Clear();
      this.defFlag_ = def;
    }
  }
}
