// Decompiled with JetBrains decompiler
// Type: MypageEditScene
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
public class MypageEditScene : NGSceneBase
{
  private readonly int SELL_SCROLLPANEL_BOTTOMANCHOR = 100;
  private readonly int SELL_SCROLLBAR_BOTTOMANCHOR = 162;
  private readonly int FORMATION_SCROLLPANEL_BOTTOMANCHOR = 120;
  private readonly int FORMATION_SCROLLBAR_BOTTOMANCHOR = 140;
  private readonly int POSSESION_SCROLLPANEL_BOTTOMANCHOR;
  private readonly int POSSESION_SCROLLBAR_BOTTOMANCHOR = 60;
  public MypageEditMenu _menu;
  public GameObject bottomPossesion;
  public GameObject sortAndFilertObject;
  public UIPanel scrollPanel;
  public UIWidget scrollBarWidget;
  public UIWidget bottomWidget;
  [SerializeField]
  private UIScrollView ScrollView;
  private static bool sellUnitPanel;
  private static bool sellMaterialPanel;
  [SerializeField]
  private UILabel textTitle;
  [SerializeField]
  private GameObject TopObj;
  private UnitMenuBase currentMenu;
  private bool[] isInit = new bool[1];

  public override IEnumerator onInitSceneAsync()
  {
    for (int index = 0; index < 1; ++index)
      this.isInit[index] = true;
    yield break;
  }

  public override IEnumerator Start()
  {
    MypageEditScene mypageEditScene = this;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = mypageEditScene.\u003C\u003En__0();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    List<NGMenuBase> source = new List<NGMenuBase>()
    {
      (NGMenuBase) mypageEditScene._menu
    };
    mypageEditScene.menuBases = source.Where<NGMenuBase>((Func<NGMenuBase, bool>) (m => Object.op_Inequality((Object) m, (Object) null))).ToArray<NGMenuBase>();
    if (mypageEditScene.menuBases != null)
      ((IEnumerable<NGMenuBase>) mypageEditScene.menuBases).ForEach<NGMenuBase>((Action<NGMenuBase>) (x =>
      {
        if (!Object.op_Inequality((Object) x, (Object) null))
          return;
        ((Behaviour) x).enabled = false;
      }));
    if (Object.op_Inequality((Object) mypageEditScene.currentMenu, (Object) null))
      ((Behaviour) mypageEditScene.currentMenu).enabled = true;
  }

  public static void changeScene00468(
    bool stack,
    PlayerDeck playerDeck,
    Promise<int?[]> player_unit_ids)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_6_8", (stack ? 1 : 0) != 0, (object) MypageEditScene.Mode.Unit00468, (object) playerDeck, (object) player_unit_ids);
  }

  public IEnumerator onStartSceneAsync()
  {
    IEnumerator e = this.onStartSceneAsync(MypageEditScene.Mode.Unit00468, SMManager.Get<PlayerDeck[]>()[0], new Promise<int?[]>());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(
    MypageEditScene.Mode mode,
    PlayerDeck playerDeck,
    Promise<int?[]> player_unit_ids)
  {
    this.SetTitle(mode);
    Player player = SMManager.Get<Player>();
    PlayerUnit[] playerUnits = SMManager.Get<PlayerUnit[]>();
    IEnumerator e;
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      playerUnits = ((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.IsSea)).ToArray<PlayerUnit>();
      e = this.SetSeaBgm();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    if (!this.isInit[(int) mode])
    {
      e = this._menu.UpdateInfoAndScroll(((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.IsNormalUnit)).ToArray<PlayerUnit>());
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this._menu.UpdateInfomation();
    }
    else
    {
      if (Singleton<NGGameDataManager>.GetInstance().IsColosseum)
        Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
      e = this.changeMenu((UnitMenuBase) this._menu);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = this._menu.Init(playerDeck, playerUnits, player_unit_ids, player.max_cost, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.isInit[(int) mode] = false;
    }
  }

  public IEnumerator onStartSceneAsync(MypageEditScene.Mode mode)
  {
    this.SetTitle(mode);
    if (!this.isInit[(int) mode])
    {
      yield return (object) this.reloadMenu(mode);
    }
    else
    {
      yield return (object) this.initMenu(mode);
      this.isInit[(int) mode] = false;
    }
  }

  private IEnumerator initMenu(MypageEditScene.Mode mode)
  {
    SMManager.Get<Player>();
    SMManager.Get<PlayerUnit[]>();
    SMManager.Get<PlayerMaterialUnit[]>();
    yield break;
  }

  private IEnumerator reloadMenu(MypageEditScene.Mode mode)
  {
    SMManager.Get<Player>();
    SMManager.Get<PlayerUnit[]>();
    SMManager.Get<PlayerMaterialUnit[]>();
    yield break;
  }

  public IEnumerator onStartSceneAsync(MypageEditScene.Mode mode, bool clearInitFlg)
  {
    for (int index = 0; index < 1; ++index)
      this.isInit[index] = clearInitFlg;
    IEnumerator e = this.onStartSceneAsync(mode);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public override void onSceneInitialized()
  {
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
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

  private IEnumerator changeMenu(UnitMenuBase menu, string title = null)
  {
    this.bottomPossesion.SetActive(menu.isBottomViewPossesion);
    this.sortAndFilertObject.SetActive(!menu.isHideSortAndFilterButton);
    if (Object.op_Inequality((Object) this.TopObj, (Object) null))
      this.TopObj.SetActive(!menu.isStorageButton);
    if (menu.isBottomViewSell)
    {
      ((UIRect) this.scrollPanel).bottomAnchor.absolute = this.SELL_SCROLLPANEL_BOTTOMANCHOR;
      ((UIRect) this.scrollBarWidget).bottomAnchor.absolute = this.SELL_SCROLLBAR_BOTTOMANCHOR;
    }
    else if (menu.isBottomViewFormation)
    {
      ((UIRect) this.scrollPanel).bottomAnchor.absolute = this.FORMATION_SCROLLPANEL_BOTTOMANCHOR;
      ((UIRect) this.scrollBarWidget).bottomAnchor.absolute = this.FORMATION_SCROLLBAR_BOTTOMANCHOR;
    }
    else if (menu.isBottomViewPossesion)
    {
      ((UIRect) this.scrollPanel).bottomAnchor.absolute = this.POSSESION_SCROLLPANEL_BOTTOMANCHOR;
      ((UIRect) this.scrollBarWidget).bottomAnchor.absolute = this.POSSESION_SCROLLBAR_BOTTOMANCHOR;
    }
    this.currentMenu = menu;
    ((Behaviour) this.currentMenu).enabled = true;
    this.currentMenu.SetIconType(UnitMenuBase.IconType.Normal);
    IEnumerator e = this.currentMenu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private void SetTitle(MypageEditScene.Mode mode)
  {
    this.textTitle.SetTextLocalize(Consts.GetInstance().MYPAGE_EDIT_UNIT_SELECT_TITLE);
  }

  public void onBackScene()
  {
    if (!Singleton<NGGameDataManager>.GetInstance().IsSea)
      return;
    Singleton<CommonRoot>.GetInstance().headerType = CommonRoot.HeaderType.Normal;
    Singleton<CommonRoot>.GetInstance().isActiveHeader = true;
  }

  public void onBackScene(
    MypageEditScene.Mode mode,
    PlayerDeck playerDeck,
    Promise<int?[]> player_unit_ids)
  {
    this.onBackScene();
  }

  public void onBackScene(
    MypageEditScene.Mode mode,
    GvgDeck gvgDeck,
    Promise<int?[]> player_unit_ids)
  {
    this.onBackScene();
  }

  public void onBackScene(MypageEditScene.Mode mode, Unit0046Menu.OneFormationInfo info)
  {
    this.onBackScene();
  }

  public void onBackScene(MypageEditScene.Mode mode, Unit004431Menu.Param sendParam)
  {
    this.onBackScene();
  }

  public void onBackScene(MypageEditScene.Mode mode, Unit00492Menu.Param param)
  {
    this.onBackScene();
  }

  public void onBackScene(MypageEditScene.Mode mode) => this.onBackScene();

  public void onBackScene(MypageEditScene.Mode mode, bool flg) => this.onBackScene();

  private IEnumerator SetSeaBgm()
  {
    MypageEditScene mypageEditScene = this;
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    SeaHomeMap seaHomeMap = ((IEnumerable<SeaHomeMap>) MasterData.SeaHomeMapList).ActiveSeaHomeMap(ServerTime.NowAppTimeAddDelta());
    if (seaHomeMap != null && !string.IsNullOrEmpty(seaHomeMap.bgm_cuesheet_name) && !string.IsNullOrEmpty(seaHomeMap.bgm_cue_name))
    {
      mypageEditScene.bgmFile = seaHomeMap.bgm_cuesheet_name;
      mypageEditScene.bgmName = seaHomeMap.bgm_cue_name;
    }
  }

  protected virtual void OnEnable()
  {
    if (!this.ScrollView.isDragging)
      return;
    this.ScrollView.Press(false);
  }

  public enum Mode
  {
    Unit00468,
    Max,
  }
}
