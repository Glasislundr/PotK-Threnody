// Decompiled with JetBrains decompiler
// Type: Popup0228Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;

#nullable disable
public class Popup0228Menu : NGBattleMenuBase
{
  private void rebirth()
  {
    BattleTimeManager manager = this.battleManager.getManager<BattleTimeManager>();
    this.env.rebirthUnits(this.env.core.playerUnits.value, manager);
    manager.setPhaseState(BL.Phase.turn_initialize);
    manager.setScheduleAction((Action) (() =>
    {
      ++this.env.core.continueCount;
      this.battleManager.saveEnvironment();
      this.battleManager.StartCoroutine(this.SendContinueCount(this.env.core.continueCount));
    }));
  }

  public void IbtnYes()
  {
    if (this.env.core.continueCount < this.env.core.battleInfo.Coin)
    {
      this.rebirth();
      this.battleManager.popupDismiss();
    }
    else
    {
      if (this.IsPushAndSet())
        return;
      this.StopCoroutine(this.OpenItemList());
      this.StartCoroutine(this.OpenItemList());
    }
  }

  private IEnumerator OpenItemList()
  {
    Popup0228Menu popup0228Menu = this;
    IEnumerator e = PurchaseBehavior.OpenItemList(true);
    while (e.MoveNext())
    {
      if (PurchaseBehavior.IsOpen)
        popup0228Menu.IsPush = false;
      yield return e.Current;
    }
    e = (IEnumerator) null;
    popup0228Menu.IsPush = false;
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    this.battleManager.getManager<BattleTimeManager>().setPhaseState(BL.Phase.gameover, true);
    this.battleManager.popupDismiss();
    this.StopCoroutine(this.OpenItemList());
  }

  private IEnumerator SendContinueCount(int count)
  {
    IEnumerator e = WebAPI.SilentBattleContinueCount(count).Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
