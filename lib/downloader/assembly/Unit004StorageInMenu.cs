// Decompiled with JetBrains decompiler
// Type: Unit004StorageInMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Unit004StorageInMenu : UnitStorageMenuBase
{
  public void IbtnDecide()
  {
    this.IbtnYes();
    this.StartCoroutine(this.UnitSendAsync());
  }

  public override void onBackButton()
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    this.OnBtnBack();
  }

  public override int SetSelectMax(int storageCount, Player player)
  {
    return Mathf.Min(Mathf.Max(0, player.max_unit_reserves - storageCount), this.selectMaxInitialValue);
  }

  private IEnumerator UnitSendAsync()
  {
    Unit004StorageInMenu unit004StorageInMenu = this;
    if (!Singleton<CommonRoot>.GetInstance().isLoading)
    {
      Singleton<PopupManager>.GetInstance().onDismiss(true);
      Singleton<CommonRoot>.GetInstance().loadingMode = 1;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      List<int> player_unit_ids = new List<int>();
      unit004StorageInMenu.selectedUnitIcons.ForEach((Action<UnitIconInfo>) (ic =>
      {
        if (!ic.unit.IsNormalUnit)
          return;
        player_unit_ids.Add(ic.playerUnit.id);
      }));
      SMManager.Get<Player>();
      Future<GameObject> prefab004unitTransformConfirmF = Res.Prefabs.popup.popup_004_unit_transform_confirm__anim_popup01.Load<GameObject>();
      IEnumerator e = prefab004unitTransformConfirmF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject result = prefab004unitTransformConfirmF.Result;
      Popup004UnitTransformConfirm component = Singleton<PopupManager>.GetInstance().open(result).GetComponent<Popup004UnitTransformConfirm>();
      List<PlayerUnit> list = unit004StorageInMenu.selectedUnitIcons.Select<UnitIconInfo, PlayerUnit>((Func<UnitIconInfo, PlayerUnit>) (x => x.playerUnit)).ToList<PlayerUnit>();
      Unit004StorageInMenu menu = unit004StorageInMenu;
      List<PlayerUnit> storageUnitIcons = list;
      component.Init((UnitStorageMenuBase) menu, storageUnitIcons, Popup004UnitTransformConfirm.StorageConfirmMode.StorageIn);
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      prefab004unitTransformConfirmF = (Future<GameObject>) null;
    }
  }
}
