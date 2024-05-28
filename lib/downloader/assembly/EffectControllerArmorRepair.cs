// Decompiled with JetBrains decompiler
// Type: EffectControllerArmorRepair
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class EffectControllerArmorRepair : EffectController
{
  [SerializeField]
  private GameObject animation_root_;
  [SerializeField]
  private List<RepairObjectController> animationObject;
  public RepairSoundEffect sound_effect_;
  private GameObject AnimationItemIconPrefab;
  private int useList;

  public void EndSE() => this.sound_effect_.OnSE0020();

  public void EndEffect()
  {
    for (int index = 0; index < this.animationObject[this.useList].animationObject.Count; ++index)
      this.animationObject[this.useList].animationObject[index].SkipEffect();
  }

  public IEnumerator initialize()
  {
    EffectControllerArmorRepair controllerArmorRepair = this;
    controllerArmorRepair.sound_effect_.ef = (EffectController) controllerArmorRepair;
    controllerArmorRepair.isAnimation = true;
    controllerArmorRepair.animation_root_.SetActive(false);
    if (Object.op_Equality((Object) controllerArmorRepair.AnimationItemIconPrefab, (Object) null))
    {
      Future<GameObject> fAnimationItemIcon = Res.Prefabs.ArmorRepair.AnimationItemIcon.Load<GameObject>();
      IEnumerator e = fAnimationItemIcon.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      controllerArmorRepair.AnimationItemIconPrefab = fAnimationItemIcon.Result;
      fAnimationItemIcon = (Future<GameObject>) null;
    }
  }

  public IEnumerator Set(
    List<GameCore.ItemInfo> thum_list,
    GameObject back_button,
    List<WebAPI.Response.ItemGearRepairRepair_results> result_list = null,
    List<WebAPI.Response.ItemGearRepairListRepair_results> result_powered_list = null)
  {
    EffectControllerArmorRepair controllerArmorRepair = this;
    Future<GameObject> get = Res.Prefabs.ArmorRepair.get_sum.Load<GameObject>();
    Future<GameObject> lost = Res.Prefabs.ArmorRepair.lost_sum.Load<GameObject>();
    Future<GameObject> normal = Res.Prefabs.ArmorRepair.normal_sum.Load<GameObject>();
    IEnumerator e = get.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = lost.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = normal.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    int numItem = thum_list.Count;
    controllerArmorRepair.useList = numItem - 1;
    Debug.Log((object) ("Repair useList = " + (object) numItem));
    bool powered = result_list == null;
    controllerArmorRepair.back_button_ = back_button;
    controllerArmorRepair.back_button_.SetActive(true);
    controllerArmorRepair.sound_effect_.result = false;
    e = controllerArmorRepair.initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    RepairObjectController objectController = controllerArmorRepair.animationObject[controllerArmorRepair.useList];
    ((Component) objectController).gameObject.SetActive(true);
    for (int index = 0; index < objectController.animationObject.Count; ++index)
    {
      RepairObject repairObject = objectController.animationObject[index];
      repairObject.ObjectOff();
      if (index < numItem)
      {
        GameCore.ItemInfo item = thum_list[index];
        repairObject.itemInfo = item;
        if (powered)
          repairObject.result_powered = result_powered_list.FirstOrDefault<WebAPI.Response.ItemGearRepairListRepair_results>((Func<WebAPI.Response.ItemGearRepairListRepair_results, bool>) (x => x.player_gear_id == item.itemID));
        else
          repairObject.result = result_list.FirstOrDefault<WebAPI.Response.ItemGearRepairRepair_results>((Func<WebAPI.Response.ItemGearRepairRepair_results, bool>) (x => x.player_gear_id == item.itemID));
      }
      else
      {
        repairObject.itemInfo = (GameCore.ItemInfo) null;
        repairObject.result = (WebAPI.Response.ItemGearRepairRepair_results) null;
      }
    }
    foreach (RepairObject repairObject in objectController.animationObject)
    {
      RepairObject repair = repairObject;
      if (repair.itemInfo != null)
      {
        GearGear gear = repair.itemInfo.gear;
        if (gear != null)
        {
          Future<Sprite> spritef = gear.LoadSpriteThumbnail();
          e = spritef.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          if (powered)
          {
            switch (repair.result_powered.status)
            {
              case 1:
                repair.Success(controllerArmorRepair.AnimationItemIconPrefab, get.Result);
                repair.SetTexture((Texture) spritef.Result.texture);
                break;
              case 2:
                repair.Normal(controllerArmorRepair.AnimationItemIconPrefab, normal.Result);
                repair.SetTexture((Texture) spritef.Result.texture);
                break;
              case 3:
                controllerArmorRepair.sound_effect_.SetLost(true);
                repair.Lost(controllerArmorRepair.AnimationItemIconPrefab, lost.Result);
                repair.SetTexture((Texture) spritef.Result.texture);
                break;
            }
          }
          else
          {
            switch (repair.result.status)
            {
              case 1:
                repair.Success(controllerArmorRepair.AnimationItemIconPrefab, get.Result);
                repair.SetTexture((Texture) spritef.Result.texture);
                break;
              case 2:
                repair.Normal(controllerArmorRepair.AnimationItemIconPrefab, normal.Result);
                repair.SetTexture((Texture) spritef.Result.texture);
                break;
              case 3:
                controllerArmorRepair.sound_effect_.SetLost(true);
                repair.Lost(controllerArmorRepair.AnimationItemIconPrefab, lost.Result);
                repair.SetTexture((Texture) spritef.Result.texture);
                break;
            }
          }
          spritef = (Future<Sprite>) null;
          repair = (RepairObject) null;
        }
        else
          break;
      }
      else
        break;
    }
    controllerArmorRepair.animation_root_.SetActive(true);
  }
}
