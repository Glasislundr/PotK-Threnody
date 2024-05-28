// Decompiled with JetBrains decompiler
// Type: Tower029ShopMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Tower029ShopMenu : ShopArticleListMenu
{
  public override IEnumerator Init(Future<GameObject> cellPrefab)
  {
    Singleton<CommonRoot>.GetInstance().GetTowerHeaderComponent().SetHeaderTowerMedal(TowerUtil.TowerPlayer != null ? TowerUtil.TowerPlayer.tower_medal : 0);
    IEnumerator e = base.Init(cellPrefab);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  protected override void UpdatePurchasedHolding(long nextholding)
  {
    if (TowerUtil.TowerPlayer != null)
      TowerUtil.TowerPlayer.tower_medal = (int) nextholding;
    Singleton<CommonRoot>.GetInstance().GetTowerHeaderComponent().SetHeaderTowerMedal((int) nextholding);
  }

  public override void onBackButton() => this.onClickedBack();

  public void onClickedBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }
}
