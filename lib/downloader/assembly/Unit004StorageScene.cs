// Decompiled with JetBrains decompiler
// Type: Unit004StorageScene
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
public class Unit004StorageScene : NGSceneBase
{
  private readonly int SELL_SCROLLPANEL_BOTTOMANCHOR = 100;
  private readonly int SELL_SCROLLBAR_BOTTOMANCHOR = 162;
  private readonly int FORMATION_SCROLLPANEL_BOTTOMANCHOR = 120;
  private readonly int FORMATION_SCROLLBAR_BOTTOMANCHOR = 140;
  private readonly int POSSESION_SCROLLPANEL_BOTTOMANCHOR;
  private readonly int POSSESION_SCROLLBAR_BOTTOMANCHOR = 60;
  [SerializeField]
  private Unit004StorageMenu menuList;
  [SerializeField]
  private Unit004StorageSellMenu menuSell;
  private UnitMenuBase currentMenu;
  [SerializeField]
  private GameObject bottomStorage;
  [SerializeField]
  private GameObject bottomPossession;
  [SerializeField]
  private GameObject bottomSell;
  [SerializeField]
  private UIPanel scrollPanel;
  [SerializeField]
  private UIWidget scrollBarWidget;
  [SerializeField]
  private UILabel textTitle;
  [SerializeField]
  private UIButton ibtn_storage_out;
  [SerializeField]
  private UIButton ibtn_back;

  public override IEnumerator Start()
  {
    Unit004StorageScene unit004StorageScene = this;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = unit004StorageScene.\u003C\u003En__0();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    List<NGMenuBase> source = new List<NGMenuBase>()
    {
      (NGMenuBase) unit004StorageScene.menuList,
      (NGMenuBase) unit004StorageScene.menuSell
    };
    unit004StorageScene.menuBases = source.Where<NGMenuBase>((Func<NGMenuBase, bool>) (m => Object.op_Inequality((Object) m, (Object) null))).ToArray<NGMenuBase>();
    if (unit004StorageScene.menuBases != null)
      ((IEnumerable<NGMenuBase>) unit004StorageScene.menuBases).ForEach<NGMenuBase>((Action<NGMenuBase>) (x =>
      {
        if (!Object.op_Inequality((Object) x, (Object) null))
          return;
        ((Behaviour) x).enabled = false;
      }));
    if (Object.op_Inequality((Object) unit004StorageScene.currentMenu, (Object) null))
      ((Behaviour) unit004StorageScene.currentMenu).enabled = true;
  }

  public static void changeSceneList(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_storage", (stack ? 1 : 0) != 0, (object) Unit004StorageScene.Mode.List);
  }

  public static void changeSceneListWithInitialize(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_storage", (stack ? 1 : 0) != 0, (object) Unit004StorageScene.Mode.List, (object) true);
  }

  public static void changeSceneSell(bool stack, bool bFromAlertUnitOver)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_storage", (stack ? 1 : 0) != 0, (object) Unit004StorageScene.Mode.Sell, (object) false, (object) bFromAlertUnitOver);
  }

  private void changeMenu(UnitMenuBase menu)
  {
    this.currentMenu = menu;
    this.textTitle.SetText(this.currentMenu.Title);
    if (Object.op_Equality((Object) this.currentMenu, (Object) this.menuList))
    {
      EventDelegate.Set(this.ibtn_storage_out.onClick, new EventDelegate((MonoBehaviour) this.menuList, "OnBtnUnitList"));
      EventDelegate.Set(this.ibtn_back.onClick, new EventDelegate((MonoBehaviour) this.menuList, "onBackButton"));
      ((Behaviour) this.menuSell).enabled = false;
      ((Behaviour) this.menuList).enabled = true;
      this.bottomStorage.SetActive(true);
      this.bottomPossession.SetActive(true);
      this.bottomSell.SetActive(false);
      ((UIRect) this.scrollPanel).bottomAnchor.absolute = this.POSSESION_SCROLLPANEL_BOTTOMANCHOR;
      ((UIRect) this.scrollBarWidget).bottomAnchor.absolute = this.POSSESION_SCROLLBAR_BOTTOMANCHOR;
    }
    else
    {
      if (!Object.op_Equality((Object) this.currentMenu, (Object) this.menuSell))
        return;
      EventDelegate.Set(this.ibtn_storage_out.onClick, new EventDelegate((MonoBehaviour) this.menuSell, "OnBtnUnitSellList"));
      EventDelegate.Set(this.ibtn_back.onClick, new EventDelegate((MonoBehaviour) this.menuSell, "onBackButton"));
      ((Behaviour) this.menuSell).enabled = true;
      ((Behaviour) this.menuList).enabled = false;
      this.bottomStorage.SetActive(false);
      this.bottomPossession.SetActive(false);
      this.bottomSell.SetActive(true);
      ((UIRect) this.scrollPanel).bottomAnchor.absolute = this.SELL_SCROLLPANEL_BOTTOMANCHOR;
      ((UIRect) this.scrollBarWidget).bottomAnchor.absolute = this.SELL_SCROLLBAR_BOTTOMANCHOR;
    }
  }

  public IEnumerator onStartSceneAsync(Unit004StorageScene.Mode mode)
  {
    IEnumerator e1 = this.SetStorageBackground();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    Future<WebAPI.Response.UnitReservesIndex> f = WebAPI.UnitReservesIndex((Action<WebAPI.Response.UserError>) (e =>
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      WebAPI.DefaultUserErrorCallback(e);
    }));
    e1 = f.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    PlayerUnit[] playerUnits = f.Result.player_units;
    switch (mode)
    {
      case Unit004StorageScene.Mode.List:
        this.changeMenu((UnitMenuBase) this.menuList);
        e1 = this.menuList.Init(playerUnits);
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        break;
      case Unit004StorageScene.Mode.Sell:
        this.changeMenu((UnitMenuBase) this.menuSell);
        e1 = this.menuSell.Init(playerUnits);
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        break;
    }
  }

  public IEnumerator onStartSceneAsync(Unit004StorageScene.Mode mode, bool clearInitFlag)
  {
    this.menuList.isInit = false;
    IEnumerator e = this.onStartSceneAsync(mode);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(
    Unit004StorageScene.Mode mode,
    bool clearInitFlag,
    bool bFromAlertUnitOver)
  {
    this.menuSell.isFromAlertUnitOver = bFromAlertUnitOver;
    yield return (object) this.onStartSceneAsync(mode);
  }

  private IEnumerator SetStorageBackground()
  {
    Unit004StorageScene unit004StorageScene = this;
    Future<GameObject> bgF = new ResourceObject("Prefabs/BackGround/DefaultBackground_storage").Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) bgF.Result, (Object) null))
      unit004StorageScene.backgroundPrefab = bgF.Result;
  }

  public void IbtnSort()
  {
    if (!Object.op_Inequality((Object) this.currentMenu, (Object) null))
      return;
    this.currentMenu.IbtnSort();
  }

  public enum Mode
  {
    List,
    Sell,
  }
}
