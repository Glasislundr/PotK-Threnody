// Decompiled with JetBrains decompiler
// Type: Mypage00171Scroll
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Mypage00171Scroll : MonoBehaviour
{
  public UILabel PresentName;
  public GameObject[] BtnObject;
  private MasterDataTable.GachaType gachaType;
  private MasterDataTable.CommonRewardType rewardType;
  private bool isApiUpdate;

  private string GetPresentName(PlayerPresent present)
  {
    return CommonRewardType.GetRewardName((MasterDataTable.CommonRewardType) present.reward_type_id.Value, present.reward_id.HasValue ? present.reward_id.Value : 0, present.reward_quantity.HasValue ? present.reward_quantity.Value : 0);
  }

  public IEnumerator Init(PlayerPresent present)
  {
    ((IEnumerable<GameObject>) this.BtnObject).ToggleOnce(0);
    this.PresentName.SetTextLocalize(this.GetPresentName(present));
    this.rewardType = (MasterDataTable.CommonRewardType) present.reward_type_id.Value;
    switch (this.rewardType)
    {
      case MasterDataTable.CommonRewardType.unit:
      case MasterDataTable.CommonRewardType.material_unit:
        UnitUnit unitUnit;
        if (MasterData.UnitUnit.TryGetValue(present.reward_id.Value, out unitUnit))
        {
          if (unitUnit.IsNormalUnit)
          {
            ((IEnumerable<GameObject>) this.BtnObject).ToggleOnce(0);
            break;
          }
          ((IEnumerable<GameObject>) this.BtnObject).ForEach<GameObject>((Action<GameObject>) (x => x.SetActive(false)));
          break;
        }
        ((IEnumerable<GameObject>) this.BtnObject).ForEach<GameObject>((Action<GameObject>) (x => x.SetActive(false)));
        break;
      case MasterDataTable.CommonRewardType.supply:
      case MasterDataTable.CommonRewardType.gear_body:
        ((IEnumerable<GameObject>) this.BtnObject).ToggleOnce(1);
        break;
      case MasterDataTable.CommonRewardType.gear:
      case MasterDataTable.CommonRewardType.material_gear:
        GearGear gearGear;
        if (MasterData.GearGear.TryGetValue(present.reward_id.Value, out gearGear))
        {
          if (!gearGear.isMaterial())
          {
            ((IEnumerable<GameObject>) this.BtnObject).ToggleOnce(1);
            break;
          }
          ((IEnumerable<GameObject>) this.BtnObject).ToggleOnce(-1);
          break;
        }
        ((IEnumerable<GameObject>) this.BtnObject).ToggleOnce(-1);
        break;
      case MasterDataTable.CommonRewardType.money:
      case MasterDataTable.CommonRewardType.medal:
      case MasterDataTable.CommonRewardType.battle_medal:
      case MasterDataTable.CommonRewardType.unit_ticket:
      case MasterDataTable.CommonRewardType.common_ticket:
        ((IEnumerable<GameObject>) this.BtnObject).ToggleOnce(3);
        break;
      case MasterDataTable.CommonRewardType.coin:
      case MasterDataTable.CommonRewardType.friend_point:
      case MasterDataTable.CommonRewardType.gacha_ticket:
        this.isApiUpdate = false;
        this.gachaType = MasterDataTable.GachaType.friend;
        int? rewardTypeId = present.reward_type_id;
        int num1 = 10;
        if (rewardTypeId.GetValueOrDefault() == num1 & rewardTypeId.HasValue)
        {
          this.gachaType = MasterDataTable.GachaType.normal;
        }
        else
        {
          rewardTypeId = present.reward_type_id;
          int num2 = 20;
          if (rewardTypeId.GetValueOrDefault() == num2 & rewardTypeId.HasValue)
          {
            this.isApiUpdate = true;
            this.gachaType = MasterDataTable.GachaType.ticket;
          }
        }
        ((IEnumerable<GameObject>) this.BtnObject).ToggleOnce(2);
        break;
      case MasterDataTable.CommonRewardType.emblem:
        ((IEnumerable<GameObject>) this.BtnObject).ToggleOnce(4);
        break;
      case MasterDataTable.CommonRewardType.quest_key:
        ((IEnumerable<GameObject>) this.BtnObject).ToggleOnce(5);
        break;
      case MasterDataTable.CommonRewardType.season_ticket:
        ((IEnumerable<GameObject>) this.BtnObject).ToggleOnce(6);
        break;
      case MasterDataTable.CommonRewardType.awake_skill:
        ((IEnumerable<GameObject>) this.BtnObject).ToggleOnce(7);
        break;
      default:
        ((IEnumerable<GameObject>) this.BtnObject).ForEach<GameObject>((Action<GameObject>) (x => x.SetActive(false)));
        yield break;
    }
  }

  public void IbtnShop()
  {
    Singleton<PopupManager>.GetInstance().onDismiss();
    ShopTopScene.ChangeScene(false);
  }

  public void IbtnGacha()
  {
    Singleton<PopupManager>.GetInstance().onDismiss();
    CommonRoot instance = Singleton<CommonRoot>.GetInstance();
    instance.isLoading = ((instance.isLoading ? 1 : 0) | (Singleton<NGSceneManager>.GetInstance().changeScene("gacha006_3", true, (object) (int) this.gachaType, (object) this.isApiUpdate) ? 1 : 0)) != 0;
  }

  public void IbtnBugu()
  {
    Singleton<PopupManager>.GetInstance().onDismiss();
    switch (this.rewardType)
    {
      case MasterDataTable.CommonRewardType.supply:
        Bugu005SupplyListScene.ChangeScene(true);
        break;
      case MasterDataTable.CommonRewardType.gear_body:
        Bugu005WeaponStorageScene.ChangeScene(true);
        break;
      default:
        Bugu0052Scene.ChangeScene(true);
        break;
    }
  }

  public void IbtnUnit()
  {
    Singleton<PopupManager>.GetInstance().onDismiss();
    Unit00468Scene.changeScene00411(true);
  }

  public void IbtnEnblem()
  {
    Singleton<PopupManager>.GetInstance().onDismiss();
    Title0241Scene.ChangeScene00241(true, SMManager.Get<Player>().id);
  }

  public void IbtnQuestkey()
  {
    Singleton<PopupManager>.GetInstance().onDismiss();
    Quest00217Scene.ChangeScene(true);
  }

  public void IbtnSeasonTicket()
  {
    Singleton<PopupManager>.GetInstance().onDismiss();
    Versus0261Scene.ChangeScene0261(true);
  }

  public void IbtnAwakeSkill()
  {
    Singleton<PopupManager>.GetInstance().onDismiss();
    Unit004ExtraskillListScene.changeScene(true);
  }
}
