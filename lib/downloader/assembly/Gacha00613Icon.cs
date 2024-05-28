// Decompiled with JetBrains decompiler
// Type: Gacha00613Icon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using UnityEngine;

#nullable disable
public class Gacha00613Icon : MonoBehaviour
{
  public Gacha00613Scene Scene;
  public int Number;
  public bool is_new;

  public void IbtnIcon()
  {
    if (!this.Scene.Menu.IsBtnAction)
      return;
    GachaResultData.Result result = GachaResultData.GetInstance().GetData().GetResultData()[this.Number];
    bool isRetry = GachaResultData.GetInstance().GetData().is_retry;
    this.is_new = result.is_new;
    int id = isRetry ? result.reward_id : result.reward_result_id;
    CommonRewardType commonRewardType = new CommonRewardType(result.reward_type_id, id, result.reward_result_quantity, result.is_new, result.is_reserves, isRetry);
    commonRewardType.ThenUnit((Action<PlayerUnit>) (unit => this.ChangeSceneUnit(unit)));
    commonRewardType.ThenMaterialUnit((Action<PlayerMaterialUnit>) (unit => this.ChangeSceneMaterialUnit(unit)));
    commonRewardType.ThenGear((Action<PlayerItem>) (gear => this.ChangeSceneGear(gear)));
    commonRewardType.ThenMaterialGear((Action<PlayerMaterialGear>) (materialGear => this.ChangeSceneGear(materialGear)));
  }

  private bool checkCancelAction()
  {
    Gacha00613Menu menu = this.Scene.Menu;
    return menu.isWaitPopup || menu.IsPushAndSet();
  }

  private void ChangeSceneUnit(PlayerUnit PU)
  {
    if (this.checkCancelAction())
      return;
    Singleton<PopupManager>.GetInstance().open((GameObject) null);
    if (PU.unit.IsMaterialUnit)
    {
      Unit00493Scene.changeScene(true, PU.unit, this.is_new, true);
    }
    else
    {
      bool flag = GachaResultData.GetInstance().GetData() == null || GachaResultData.GetInstance().GetData().additionalItems == null || GachaResultData.GetInstance().GetData().additionalItems.Length == 0;
      Singleton<NGSceneManager>.GetInstance().changeScene("gacha006_8", (flag ? 1 : 0) != 0, (object) PU, (object) this.is_new);
    }
  }

  private void ChangeSceneMaterialUnit(PlayerMaterialUnit PU)
  {
    if (this.checkCancelAction())
      return;
    Singleton<PopupManager>.GetInstance().open((GameObject) null);
    if (PU.unit.IsMaterialUnit)
    {
      Unit00493Scene.changeScene(true, PU.unit, this.is_new, true);
    }
    else
    {
      bool flag = GachaResultData.GetInstance().GetData() == null || GachaResultData.GetInstance().GetData().additionalItems == null || GachaResultData.GetInstance().GetData().additionalItems.Length == 0;
      Singleton<NGSceneManager>.GetInstance().changeScene("gacha006_8", (flag ? 1 : 0) != 0, (object) PU, (object) this.is_new);
    }
  }

  private void ChangeSceneGear(PlayerItem PI)
  {
    if (this.checkCancelAction())
      return;
    Singleton<PopupManager>.GetInstance().open((GameObject) null);
    if (!PI.gear.kind.isEquip)
      Bugu00561Scene.changeScene(true, new GameCore.ItemInfo(PI), this.is_new, true, true);
    else
      Singleton<NGSceneManager>.GetInstance().changeScene("gacha006_11", true, (object) this.is_new, (object) new GameCore.ItemInfo(PI));
  }

  private void ChangeSceneGear(PlayerMaterialGear PI)
  {
    if (this.checkCancelAction())
      return;
    Singleton<PopupManager>.GetInstance().open((GameObject) null);
    if (!PI.gear.kind.isEquip)
      Bugu00561Scene.changeScene(true, new GameCore.ItemInfo(PI), this.is_new, true, true);
    else
      Singleton<NGSceneManager>.GetInstance().changeScene("gacha006_11", true, (object) this.is_new, (object) new GameCore.ItemInfo(PI));
  }
}
