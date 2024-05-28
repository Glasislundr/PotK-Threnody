// Decompiled with JetBrains decompiler
// Type: Quest002152popup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Quest002152popup : BackButtonMenuBase
{
  [SerializeField]
  private UI2DSprite LeftChara;
  [SerializeField]
  private UI2DSprite RightChara;
  [SerializeField]
  private UILabel TxtDescription;
  [SerializeField]
  private UILabel TxtLiberation;
  public int lvv;

  public void PopupSetiing()
  {
    this.TxtDescription.SetTextLocalize("ディランダル\nと\nエクスカリバー\nの[ffff00]親密Lv" + this.lvv.ToString() + "[-]以上");
    this.TxtLiberation.SetTextLocalize("エピソード解放条件");
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();
}
