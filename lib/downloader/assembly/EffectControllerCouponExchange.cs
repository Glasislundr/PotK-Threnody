// Decompiled with JetBrains decompiler
// Type: EffectControllerCouponExchange
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
public class EffectControllerCouponExchange : EffectController
{
  [SerializeField]
  private CommonRarityAnim rarityAnim_;
  [SerializeField]
  private GameObject effNew_;
  [SerializeField]
  private GachaSEAfterUser SE_;
  [NonSerialized]
  public EventDelegate.Callback callbackOnFinishedEffect_;

  private void initializeLocal()
  {
    this.SE_.result = false;
    this.back_button_.SetActive(false);
  }

  public IEnumerator coExchangeUnit(WebAPI.Response.SelectticketSpend unitticketSpend)
  {
    EffectControllerCouponExchange controllerCouponExchange = this;
    controllerCouponExchange.initializeLocal();
    PlayerUnit unit = ((IEnumerable<PlayerUnit>) unitticketSpend.player_units).FirstOrDefault<PlayerUnit>();
    IEnumerator e;
    if (unit.unit.IsMaterialUnit)
    {
      e = controllerCouponExchange.SetTextureUnit(unit.unit.ID, controllerCouponExchange.rarityAnim_.image400_);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = controllerCouponExchange.SetTextureUnit(unit.unit.ID, controllerCouponExchange.rarityAnim_.image400_blur_);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ((Component) controllerCouponExchange.rarityAnim_.image_).gameObject.SetActive(false);
      ((Component) controllerCouponExchange.rarityAnim_.image400_).gameObject.SetActive(true);
    }
    else
    {
      e = controllerCouponExchange.SetTextureUnit(unit.unit.ID, controllerCouponExchange.rarityAnim_.image_);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = controllerCouponExchange.SetTextureUnit(unit.unit.ID, controllerCouponExchange.rarityAnim_.image_blur_);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ((Component) controllerCouponExchange.rarityAnim_.image_).gameObject.SetActive(true);
      ((Component) controllerCouponExchange.rarityAnim_.image400_).gameObject.SetActive(false);
    }
    controllerCouponExchange.SE_.setResult(unit.unit.rarity.index);
    controllerCouponExchange.SetCommonRarity(controllerCouponExchange.rarityAnim_.rarity_obj_list_, unit.unit.rarity.index);
    if (Object.op_Inequality((Object) controllerCouponExchange.effNew_, (Object) null))
      controllerCouponExchange.effNew_.SetActive(unitticketSpend.is_new);
    yield return (object) controllerCouponExchange.CreateRarityStartObjects(unit.unit);
  }

  private IEnumerator CreateRarityStartObjects(UnitUnit unit)
  {
    Future<GameObject> rarityStarObj = (Future<GameObject>) null;
    int num = unit.rarity.index + 1;
    switch (num)
    {
      case 1:
      case 2:
      case 3:
      case 4:
        rarityStarObj = Res.Prefabs.common_animation.common_rarity.raritystar1.Load<GameObject>();
        break;
      case 5:
        rarityStarObj = Res.Prefabs.common_animation.common_rarity.raritystar2.Load<GameObject>();
        break;
      case 6:
        rarityStarObj = Res.Prefabs.common_animation.common_rarity.raritystar3.Load<GameObject>();
        break;
      default:
        Debug.LogError((object) ("想定していないレアリティのため、星Prefabを取得できません: " + (object) num));
        break;
    }
    IEnumerator e = rarityStarObj.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = rarityStarObj.Result;
    for (int index = 0; index <= unit.rarity.index; ++index)
      result.Clone(this.rarityAnim_.rarity_list[unit.rarity.index].rarity_list[index].transform);
  }

  public void onFinishedEffect()
  {
    if (this.callbackOnFinishedEffect_ == null)
      return;
    this.callbackOnFinishedEffect_();
  }

  private void Start() => this.SE_.OnPlayResult();
}
