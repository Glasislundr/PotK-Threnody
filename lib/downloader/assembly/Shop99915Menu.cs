// Decompiled with JetBrains decompiler
// Type: Shop99915Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Shop99915Menu : BackButtonMenuBase
{
  [SerializeField]
  private UILabel TxtTitle;
  [SerializeField]
  private UILabel TxtDescription;

  public void SetText(string name)
  {
    string str = name != null ? name : Consts.GetInstance().UNIQUE_ICON_KEY;
    string text1 = Consts.Format(Consts.GetInstance().SHOP_99915_TXT_TITLE);
    string text2 = Consts.Format(Consts.GetInstance().SHOP_99915_TXT_DESCRIPTION, (IDictionary) new Hashtable()
    {
      {
        (object) nameof (name),
        (object) str
      }
    });
    this.TxtTitle.SetText(text1);
    this.TxtDescription.SetText(text2);
  }

  public void IbtnOK()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnOK();
}
