// Decompiled with JetBrains decompiler
// Type: Versus02610WeekRankPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Versus02610WeekRankPopup : BackButtonMenuBase
{
  [SerializeField]
  private UILabel txt;
  private Versus02610Menu menu;
  private Popup02689Menu rankingDoneMenu;

  public IEnumerator Init(Versus02610Menu menu, int current_rank, Popup02689Menu rankingDoneMenu = null)
  {
    this.menu = menu;
    this.rankingDoneMenu = rankingDoneMenu;
    this.txt.SetText(MasterData.PvpRankingKind[current_rank].name);
    yield return (object) null;
  }

  public override void onBackButton() => this.IbtnTouch();

  public void IbtnTouch()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.close());
  }

  public IEnumerator close()
  {
    if (Object.op_Inequality((Object) this.rankingDoneMenu, (Object) null))
    {
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      this.rankingDoneMenu.updatePvPInfo(false);
      while (Singleton<CommonRoot>.GetInstance().isLoading)
        yield return (object) null;
      this.rankingDoneMenu.close();
    }
    this.menu.dispHowto();
    Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();
  }
}
