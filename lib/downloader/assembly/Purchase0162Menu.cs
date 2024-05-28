// Decompiled with JetBrains decompiler
// Type: Purchase0162Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using UnityEngine;

#nullable disable
public class Purchase0162Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtNumberCharge;
  [SerializeField]
  protected UILabel TxtNumberFree;
  [SerializeField]
  protected UILabel TxtNumberCommon;
  [SerializeField]
  protected UIScrollView uiScroll;
  private Player player;
  private long? revPlayer;

  public virtual void Foreground()
  {
  }

  public virtual void VScrollBar()
  {
  }

  protected override void Update()
  {
    base.Update();
    if (this.revPlayer.HasValue)
    {
      long num = SMManager.Revision<Player>();
      long? revPlayer = this.revPlayer;
      long valueOrDefault = revPlayer.GetValueOrDefault();
      if (num == valueOrDefault & revPlayer.HasValue)
        return;
    }
    this.player = SMManager.Get<Player>();
    this.revPlayer = new long?(SMManager.Revision<Player>());
    this.TxtNumberCharge.SetTextLocalize(this.player.paid_coin);
    this.TxtNumberFree.SetTextLocalize(this.player.free_coin);
    this.TxtNumberCommon.SetTextLocalize(this.player.common_coin);
  }

  public void InitPurchase(Player player)
  {
    this.player = player;
    this.revPlayer = new long?(SMManager.Revision<Player>());
    this.TxtNumberCharge.SetTextLocalize(player.paid_coin);
    this.TxtNumberFree.SetTextLocalize(player.free_coin);
    this.TxtNumberCommon.SetTextLocalize(player.common_coin);
    this.uiScroll.ResetPosition();
  }

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public override void onBackButton() => this.IbtnBack();
}
