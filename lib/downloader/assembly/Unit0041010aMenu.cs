// Decompiled with JetBrains decompiler
// Type: Unit0041010aMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Unit0041010aMenu : BackButtonMenuBase
{
  [SerializeField]
  public UI2DSprite[] LinkCharacter;
  [SerializeField]
  public UILabel TxtDescription;
  [SerializeField]
  public UILabel TxtPopuptitle;
  private List<PlayerUnit> sellUnitIcons = new List<PlayerUnit>();
  private Unit00410Menu menu;

  private IEnumerator UnitSellAsync()
  {
    Unit0041010aMenu unit0041010aMenu = this;
    if (!Singleton<CommonRoot>.GetInstance().isLoading)
    {
      Singleton<PopupManager>.GetInstance().onDismiss(true);
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      List<int> player_unit_ids = new List<int>();
      List<int> player_material_unit_ids = new List<int>();
      unit0041010aMenu.sellUnitIcons.ForEach((Action<PlayerUnit>) (ic =>
      {
        if (ic.unit.IsNormalUnit)
          player_unit_ids.Add(ic.id);
        else
          player_material_unit_ids.Add(ic.id);
      }));
      Player player = SMManager.Get<Player>();
      Future<WebAPI.Response.UnitSell> ft = WebAPIUtil.UnitSell(player_material_unit_ids.ToArray(), player_unit_ids.ToArray(), (Action<WebAPI.Response.UserError>) (error =>
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        WebAPI.DefaultUserErrorCallback(error);
      }));
      IEnumerator e = ft.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      PlayerUnit[] playerUnits = SMManager.Get<PlayerUnit[]>();
      PlayerMaterialUnit[] playerMaterialUnits = SMManager.Get<PlayerMaterialUnit[]>();
      PlayerDeck[] playerDeck = SMManager.Get<PlayerDeck[]>();
      e = unit0041010aMenu.menu.Init(playerDeck, player, playerUnits, playerMaterialUnits, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      long sendZeny = ft.Result.player.money - player.money;
      int sendMedal = ft.Result.player.medal - player.medal;
      Future<GameObject> prefab0041012F = Res.Prefabs.popup.popup_004_10_12__anim_popup01.Load<GameObject>();
      e = prefab0041012F.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject result = prefab0041012F.Result;
      Singleton<PopupManager>.GetInstance().openAlert(result).GetComponent<Unit0041012Menu>().SetText(sendZeny, sendMedal);
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      player = (Player) null;
      ft = (Future<WebAPI.Response.UnitSell>) null;
      prefab0041012F = (Future<GameObject>) null;
    }
    else
      unit0041010aMenu.IsPush = false;
  }

  public void IbtnPopupYes()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.UnitSellAsync());
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();

  public IEnumerator SetSelectedUnitIcons(List<PlayerUnit> icons, Unit00410Menu menu)
  {
    this.menu = menu;
    this.sellUnitIcons = icons;
    Future<GameObject> prefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject prefab = prefabF.Result;
    int i = 0;
    foreach (PlayerUnit icon1 in icons)
    {
      PlayerUnit icon = icon1;
      if (icon.unit.rarity.index >= 2)
      {
        GameObject unitIconGo = Object.Instantiate<GameObject>(prefab);
        UnitIcon unitIcon = unitIconGo.GetComponent<UnitIcon>();
        unitIcon.setBottom(icon);
        unitIcon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
        e = unitIcon.SetUnit(icon.unit, icon.GetElement(), false);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        unitIcon.setLevelText(icon);
        ((Component) this.LinkCharacter[i]).gameObject.SetActive(false);
        unitIconGo.gameObject.SetParent(((Component) this.LinkCharacter[i]).gameObject, 0.85f);
        ((Component) this.LinkCharacter[i]).gameObject.SetActive(true);
        unitIconGo.SetActive(true);
        ++i;
        unitIconGo = (GameObject) null;
        unitIcon = (UnitIcon) null;
        icon = (PlayerUnit) null;
      }
    }
  }
}
