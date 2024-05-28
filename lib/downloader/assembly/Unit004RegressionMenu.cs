// Decompiled with JetBrains decompiler
// Type: Unit004RegressionMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/Unit/RegressionMenu")]
public class Unit004RegressionMenu : UnitMenuBase
{
  private HashSet<int> not_ = new HashSet<int>() { 0 };
  private Dictionary<int, int[]> dicGenealogy_ = new Dictionary<int, int[]>();
  private GameObject prefab_;
  private GameObject prefabConfirm_;
  private GameObject prefabResult_;
  private PlayerUnit currentUnit_;

  public override IEnumerator Initialize()
  {
    Unit004RegressionMenu unit004RegressionMenu = this;
    unit004RegressionMenu.StartCoroutine(unit004RegressionMenu.doLoadPrefabs());
    PlayerUnit[] units = unit004RegressionMenu.getRegressionUnits();
    unit004RegressionMenu.SetIconType(UnitMenuBase.IconType.Normal);
    // ISSUE: reference to a compiler-generated method
    yield return (object) unit004RegressionMenu.\u003C\u003En__0();
    unit004RegressionMenu.InitializeInfo((IEnumerable<PlayerUnit>) units, (IEnumerable<PlayerMaterialUnit>) null, Persist.unit004RegressionSortAndFilter, false, false, true, true, false);
    yield return (object) unit004RegressionMenu.CreateUnitIcon();
    unit004RegressionMenu.TxtNumber.SetTextLocalize(units.Length);
    unit004RegressionMenu.lastReferenceUnitID = -1;
    unit004RegressionMenu.InitializeEnd();
  }

  private IEnumerator doLoadPrefabs()
  {
    Future<GameObject> ld;
    if (Object.op_Equality((Object) this.prefab_, (Object) null))
    {
      ld = new ResourceObject("Prefabs/popup/popup_004_regression").Load<GameObject>();
      yield return (object) ld.Wait();
      this.prefab_ = ld.Result;
      ld = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.prefabConfirm_, (Object) null))
    {
      ld = new ResourceObject("Prefabs/popup/popup_004_regression_confirmation").Load<GameObject>();
      yield return (object) ld.Wait();
      this.prefabConfirm_ = ld.Result;
      ld = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.prefabResult_, (Object) null))
    {
      ld = new ResourceObject("Prefabs/popup/popup_004_regression_result").Load<GameObject>();
      yield return (object) ld.Wait();
      this.prefabResult_ = ld.Result;
      ld = (Future<GameObject>) null;
    }
  }

  protected override IEnumerator CreateUnitIcon(
    int info_index,
    int unit_index,
    PlayerUnit baseUnit)
  {
    Unit004RegressionMenu unit004RegressionMenu = this;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = unit004RegressionMenu.\u003C\u003En__1(info_index, unit_index, baseUnit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit004RegressionMenu.postCreateUnitIcon(unit004RegressionMenu.displayUnitInfos[info_index], unit004RegressionMenu.allUnitIcons[unit_index]);
  }

  protected override void CreateUnitIconCache(int info_index, int unit_index, PlayerUnit baseUnit)
  {
    base.CreateUnitIconCache(info_index, unit_index, baseUnit);
    this.postCreateUnitIcon(this.displayUnitInfos[info_index], this.allUnitIcons[unit_index]);
  }

  private void postCreateUnitIcon(UnitIconInfo dispInfo, UnitIconBase unitIcon)
  {
    unitIcon.onLongPress = (Action<UnitIconBase>) (x => Unit0042Scene.changeSceneEvolutionUnit(true, x.PlayerUnit, this.getUnits()));
    unitIcon.onClick = (Action<UnitIconBase>) (x => this.onSelectUnit(x));
    unitIcon.SetupDeckStatusBlink();
  }

  private void onSelectUnit(UnitIconBase unitIcon)
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine("doRegression", (object) unitIcon.PlayerUnit);
  }

  public void recoverPopup()
  {
    this.currentUnit_ = this.currentUnit_ != (PlayerUnit) null ? Array.Find<PlayerUnit>(this.getUnits(), (Predicate<PlayerUnit>) (x => x.id == this.currentUnit_.id)) : (PlayerUnit) null;
    if (this.currentUnit_ == (PlayerUnit) null)
      return;
    this.StartCoroutine("doRegression", (object) this.currentUnit_);
  }

  private IEnumerator doRegression(PlayerUnit playerUnit)
  {
    Unit004RegressionMenu unit004RegressionMenu = this;
    if (Object.op_Equality((Object) unit004RegressionMenu.prefab_, (Object) null) || Object.op_Equality((Object) unit004RegressionMenu.prefabConfirm_, (Object) null) || Object.op_Equality((Object) unit004RegressionMenu.prefabResult_, (Object) null))
    {
      unit004RegressionMenu.IsPush = false;
    }
    else
    {
      unit004RegressionMenu.currentUnit_ = playerUnit;
      int itemId = Consts.GetInstance().ITEM_REGRESSION_ID;
      PlayerMaterialUnit item = Array.Find<PlayerMaterialUnit>(SMManager.Get<PlayerMaterialUnit[]>(), (Predicate<PlayerMaterialUnit>) (x => x._unit == itemId));
      PopupManager pm = Singleton<PopupManager>.GetInstance();
      pm.open((GameObject) null, isViewBack: false);
      int nWait1;
      while (true)
      {
        GameObject gameObject1 = pm.open(unit004RegressionMenu.prefab_, isNonSe: true, isNonOpenAnime: true);
        nWait1 = 0;
        gameObject1.GetComponent<PopupUnitRegression>().initailize(unit004RegressionMenu.unitPrefab, playerUnit, unit004RegressionMenu.dicGenealogy_[playerUnit._unit][0], (Action) (() => nWait1 = 1), (Action) (() => nWait1 = 2), (Action) (() => nWait1 = 3), (Action) (() => nWait1 = 4), (Action) (() => nWait1 = 5));
        while (nWait1 == 0)
          yield return (object) null;
        if (nWait1 == 1)
        {
          pm.dismiss();
          GameObject gameObject2 = pm.open(unit004RegressionMenu.prefabConfirm_, isNonSe: true, isNonOpenAnime: true);
          int nWait2 = 0;
          gameObject2.GetComponent<PopupUnitRegression>().initailize(unit004RegressionMenu.unitPrefab, playerUnit, unit004RegressionMenu.dicGenealogy_[playerUnit._unit][0], (Action) (() => nWait2 = 1), (Action) (() => nWait2 = 2));
          while (nWait2 == 0)
            yield return (object) null;
          if (nWait2 != 1)
            pm.dismiss();
          else
            goto label_21;
        }
        else
          break;
      }
      switch (nWait1)
      {
        case 2:
          unit004RegressionMenu.currentUnit_ = (PlayerUnit) null;
          break;
        case 3:
          List<HelpHelp> list = ((IEnumerable<HelpHelp>) MasterData.HelpHelpList).Where<HelpHelp>((Func<HelpHelp, bool>) (x => x.title == "姫退行")).ToList<HelpHelp>();
          if (list.Any<HelpHelp>())
          {
            Help0152Scene.ChangeScene(true, list);
            break;
          }
          break;
        case 4:
          PlayerUnit playerMaterialUnit1;
          if (item != null)
            playerMaterialUnit1 = PlayerUnit.CreateByPlayerMaterialUnit(item);
          else
            playerMaterialUnit1 = PlayerUnit.CreateByPlayerMaterialUnit(new PlayerMaterialUnit()
            {
              _unit = itemId
            });
          Unit0042Scene.changeScene(true, playerMaterialUnit1, (PlayerUnit[]) null, true);
          break;
      }
      unit004RegressionMenu.closePopup();
      yield break;
label_21:
      pm.dismiss();
      PlayerMaterialUnit playerMaterialUnit2 = item;
      if ((playerMaterialUnit2 != null ? playerMaterialUnit2.quantity : 0) < Consts.GetInstance().ITEM_REGRESSION_QUANTITY)
      {
        unit004RegressionMenu.closePopup();
      }
      else
      {
        Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
        Future<WebAPI.Response.UnitDegenerate> webAPI = WebAPI.UnitDegenerate(playerUnit.id, new int[1]
        {
          item.id
        }, (Action<WebAPI.Response.UserError>) (e =>
        {
          WebAPI.DefaultUserErrorCallback(e);
          MypageScene.ChangeSceneOnError();
        }));
        yield return (object) webAPI.Wait();
        Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
        if (webAPI.Result == null)
        {
          unit004RegressionMenu.closePopup();
        }
        else
        {
          Singleton<NGGameDataManager>.GetInstance().corps_player_unit_ids = new HashSet<int>((IEnumerable<int>) webAPI.Result.corps_player_unit_ids);
          webAPI = (Future<WebAPI.Response.UnitDegenerate>) null;
          GameObject gameObject = pm.open(unit004RegressionMenu.prefabResult_, isNonSe: true, isNonOpenAnime: true);
          int nWait3 = 0;
          gameObject.GetComponent<PopupUnitRegression>().initailize(unit004RegressionMenu.unitPrefab, playerUnit, unit004RegressionMenu.dicGenealogy_[playerUnit._unit][0], (Action) (() => nWait3 = 1));
          while (nWait3 == 0)
            yield return (object) null;
          pm.dismiss();
          pm.dismiss();
          Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
          yield return (object) null;
          yield return (object) unit004RegressionMenu.Initialize();
          Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
        }
      }
    }
  }

  private void closePopup() => Singleton<PopupManager>.GetInstance().closeAll();

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }

  public override void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public void IbtnEvolution()
  {
    if (this.IsPushAndSet())
      return;
    Unit00468Scene.changeScene00491Evolution(false);
  }

  private PlayerUnit[] getRegressionUnits()
  {
    PlayerUnit[] playerUnitArray = SMManager.Get<PlayerUnit[]>();
    HashSet<int> equippedOverkillers = new HashSet<int>();
    for (int index1 = 0; index1 < playerUnitArray.Length; ++index1)
    {
      PlayerUnit playerUnit = playerUnitArray[index1];
      if (playerUnit.over_killers_player_unit_ids != null && playerUnit.over_killers_player_unit_ids.Length != 0)
      {
        for (int index2 = 0; index2 < playerUnit.over_killers_player_unit_ids.Length; ++index2)
        {
          int killersPlayerUnitId = playerUnit.over_killers_player_unit_ids[index2];
          switch (killersPlayerUnitId)
          {
            case -1:
            case 0:
              continue;
            default:
              equippedOverkillers.Add(killersPlayerUnitId);
              continue;
          }
        }
      }
    }
    int rarityIndex = Consts.GetInstance().UNIT_RARITY_GREATEREQUAL_INDEX;
    return ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => !x.favorite && x.unit.rarity.index >= rarityIndex && !equippedOverkillers.Contains(x.id) && this.checkRegression(x._unit))).ToArray<PlayerUnit>();
  }

  private bool checkRegression(int unitId)
  {
    if (this.not_.Contains(unitId))
      return false;
    int[] genealogyIds;
    if (!this.dicGenealogy_.TryGetValue(unitId, out genealogyIds))
    {
      KeyValuePair<int, int[]> keyValuePair1 = this.dicGenealogy_.FirstOrDefault<KeyValuePair<int, int[]>>((Func<KeyValuePair<int, int[]>, bool>) (x => ((IEnumerable<int>) x.Value).Contains<int>(unitId)));
      if (keyValuePair1.Value == null)
      {
        genealogyIds = UnitEvolutionPattern.getGenealogyIds(unitId);
        if (genealogyIds.Length == 0)
        {
          KeyValuePair<int, int[]> keyValuePair2 = this.dicGenealogy_.FirstOrDefault<KeyValuePair<int, int[]>>((Func<KeyValuePair<int, int[]>, bool>) (x => x.Value.Length == 0));
          if (keyValuePair2.Value != null)
            genealogyIds = keyValuePair2.Value;
        }
      }
      else
        genealogyIds = keyValuePair1.Value;
      this.dicGenealogy_[unitId] = genealogyIds;
    }
    if (genealogyIds.Length != 0 && genealogyIds[0] != unitId)
      return true;
    this.not_.Add(unitId);
    return false;
  }
}
