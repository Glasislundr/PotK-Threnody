// Decompiled with JetBrains decompiler
// Type: Popup023414Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using UnityEngine;

#nullable disable
public class Popup023414Menu : BackButtonMenuBase
{
  public UILabel HimeNum;
  [SerializeField]
  private GameObject slc_Kiseki_Bonus;
  private Modified<CoinBonus[]> coinBonus;

  private void Start()
  {
    this.coinBonus = SMManager.Observe<CoinBonus[]>();
    this.coinBonus.NotifyChanged();
  }

  public void Init() => this.HimeNum.SetTextLocalize(SMManager.Get<Player>().coin);

  public virtual void IbtnRetrun()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public virtual void IbtnBuy()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
    this.StartCoroutine(PopupUtility.BuyKiseki());
  }

  public override void onBackButton() => this.IbtnRetrun();

  protected override void Update()
  {
    base.Update();
    if (!this.coinBonus.IsChangedOnce())
      return;
    this.slc_Kiseki_Bonus.SetActive(this.coinBonus.Value.Length != 0);
  }
}
