// Decompiled with JetBrains decompiler
// Type: Popup005512Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Popup005512Menu : BackButtonMonoBehaiviour
{
  [SerializeField]
  private UILabel description;

  public void IbtnOk() => Singleton<PopupManager>.GetInstance().onDismiss();

  public override void onBackButton() => this.IbtnOk();

  public void SetTextWithMoney(long money)
  {
    string localizeNumberText = money.ToLocalizeNumberText();
    this.description.SetText(Consts.Format(Consts.GetInstance().popup_005_5_12_text, (IDictionary) new Hashtable()
    {
      {
        (object) nameof (money),
        (object) localizeNumberText
      }
    }));
  }
}
