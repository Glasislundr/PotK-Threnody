// Decompiled with JetBrains decompiler
// Type: Popup001714Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;

#nullable disable
public class Popup001714Menu : NGMenuBase
{
  public UILabel TxtDescription;

  public IEnumerator Init(string str)
  {
    yield break;
  }

  public void SetText(string str)
  {
    this.TxtDescription.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_001714_DESCRIPT_TEXT, (IDictionary) new Hashtable()
    {
      {
        (object) "Item",
        (object) str
      }
    }));
  }

  public void SetMessage(string str) => this.TxtDescription.SetTextLocalize(str);

  public virtual void IbtnOk() => Singleton<PopupManager>.GetInstance().onDismiss();
}
