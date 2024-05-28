// Decompiled with JetBrains decompiler
// Type: Popup01016Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;

#nullable disable
public class Popup01016Menu : BackButtonMenuBase
{
  public UILabel TextDescription;
  private Setting01013Menu menu01013;

  public IEnumerator Init(Setting01013Menu menu, string name)
  {
    this.menu01013 = menu;
    this.TextDescription.SetText(Consts.Format(Consts.GetInstance().POPUP_01016_DESCRIPTION, (IDictionary) new Hashtable()
    {
      {
        (object) nameof (name),
        (object) name
      }
    }));
    yield break;
  }

  public virtual void IbtnOk()
  {
    this.menu01013.Initialize();
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public void IbtnNo() => this.IbtnOk();

  public override void onBackButton() => this.IbtnNo();
}
