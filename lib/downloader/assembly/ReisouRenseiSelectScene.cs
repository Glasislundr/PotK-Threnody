// Decompiled with JetBrains decompiler
// Type: ReisouRenseiSelectScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ReisouRenseiSelectScene : NGSceneBase
{
  public ReisouRenseiSelectMenu menu;

  public override IEnumerator onInitSceneAsync()
  {
    yield break;
  }

  public static void ChangeScene(
    bool stack,
    List<InventoryItem> select,
    GameCore.ItemInfo target,
    bool isSpecial)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("reisou_rensei_drilling_material", (stack ? 1 : 0) != 0, (object) select, (object) target, (object) isSpecial);
  }

  public IEnumerator onStartSceneAsync(List<InventoryItem> select, GameCore.ItemInfo target, bool isSpecial)
  {
    if (Singleton<NGGameDataManager>.GetInstance().IsColosseum)
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
    if (!Array.Find<PlayerItem>(SMManager.Get<PlayerItem[]>(), (Predicate<PlayerItem>) (x => x.id == target.itemID)).isReisouSet)
    {
      target.reisou = (PlayerItem) null;
      target.isEquipReisou_ = false;
    }
    this.menu.SetFirstSelectItem(isSpecial ? ReisouRenseiSelectMenu.DrillingType.Special : ReisouRenseiSelectMenu.DrillingType.Normal, select, target);
    IEnumerator e = this.menu.Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual void onStartScene(List<InventoryItem> select, GameCore.ItemInfo target, bool isSpecial)
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    Singleton<CommonRoot>.GetInstance().isActiveHeader = true;
    Singleton<CommonRoot>.GetInstance().isActiveFooter = true;
  }

  public override void onEndScene()
  {
    Persist.sortOrder.Flush();
    this.menu.onEndScene();
    ItemIcon.ClearCache();
    ((Component) this).GetComponentInChildren<NGxScroll2>().scrollView.Press(false);
  }
}
