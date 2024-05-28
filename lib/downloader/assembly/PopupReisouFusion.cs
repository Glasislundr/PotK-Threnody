// Decompiled with JetBrains decompiler
// Type: PopupReisouFusion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class PopupReisouFusion : BackButtonPopupBase
{
  [SerializeField]
  protected GameObject dirHolyIcon;
  [SerializeField]
  protected GameObject dirChaosIcon;
  [SerializeField]
  protected UILabel txtZeny;
  [SerializeField]
  protected UILabel txtMedalCost;
  [SerializeField]
  protected UILabel txtMedalPosession;
  [SerializeField]
  protected SpreadColorButton btnYes;
  protected Action callBack;
  protected PlayerItem holyItem;
  protected PlayerItem chaosItem;

  public IEnumerator Init(
    Action callBack,
    GearReisouFusion recipe,
    PlayerItem baseItem,
    PlayerItem targetItem)
  {
    this.callBack = callBack;
    this.holyItem = baseItem.isHolyReisou() ? baseItem : targetItem;
    this.chaosItem = baseItem.isChaosReisou() ? baseItem : targetItem;
    Future<GameObject> prefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject itemIconPrefab = prefabF.Result;
    ItemIcon itemIcon = itemIconPrefab.Clone(this.dirHolyIcon.transform).GetComponent<ItemIcon>();
    e = itemIcon.InitByPlayerItem(this.holyItem);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    itemIcon.EnableLongPressEventReisou();
    itemIcon = (ItemIcon) null;
    itemIcon = itemIconPrefab.Clone(this.dirChaosIcon.transform).GetComponent<ItemIcon>();
    e = itemIcon.InitByPlayerItem(this.chaosItem);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    itemIcon.EnableLongPressEventReisou();
    itemIcon = (ItemIcon) null;
    Player player = SMManager.Get<Player>();
    string format = "[ff0000]{0}[-]";
    if (player.money < (long) recipe.cost_zeny)
      this.txtZeny.SetTextLocalize(format.F((object) recipe.cost_zeny));
    else
      this.txtZeny.SetTextLocalize(recipe.cost_zeny);
    if (player.battle_medal < recipe.cost_medal)
      this.txtMedalCost.SetTextLocalize(format.F((object) recipe.cost_medal));
    else
      this.txtMedalCost.SetTextLocalize(recipe.cost_medal);
    this.txtMedalPosession.SetTextLocalize(player.battle_medal);
    if (player.money < (long) recipe.cost_zeny || player.battle_medal < recipe.cost_medal)
      ((UIButtonColor) this.btnYes).isEnabled = false;
  }

  public void onBtnYes()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.CallFusionAPI());
  }

  protected IEnumerator CallFusionAPI()
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
    yield return (object) null;
    Future<WebAPI.Response.ItemGearReisouFusion> paramF = WebAPI.ItemGearReisouFusion(this.chaosItem.id, this.holyItem.id, (Action<WebAPI.Response.UserError>) (error =>
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
      yield return (object) null;
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      PlayerItem mythologyItem = paramF.Result.player_items[0];
      Future<GameObject> popupPrefabF = new ResourceObject("Prefabs/popup/popup_Reisou_fusion_result").Load<GameObject>();
      e = popupPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject popup = popupPrefabF.Result.Clone();
      PopupReisouFusionResult component = popup.GetComponent<PopupReisouFusionResult>();
      popup.SetActive(false);
      e = component.Init(this.callBack, mythologyItem);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      popup.SetActive(true);
      Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
    }
  }

  public void onBtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public override void onBackButton() => this.onBtnNo();
}
