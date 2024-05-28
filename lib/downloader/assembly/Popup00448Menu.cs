// Decompiled with JetBrains decompiler
// Type: Popup00448Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;

#nullable disable
public class Popup00448Menu : NGMenuBase
{
  public UILabel TxtDescription;

  public IEnumerator Init(PlayerItem item)
  {
    this.TxtDescription.SetTextLocalize(Consts.Format(Consts.GetInstance().popup_004_4_8, (IDictionary) new Hashtable()
    {
      {
        (object) "Gear",
        (object) item.gear.kind.name
      },
      {
        (object) "Proficiencies",
        (object) MasterData.UnitProficiency[item.gear.rarity.index].proficiency
      }
    }));
    yield break;
  }

  public virtual void IbtnOk() => Singleton<PopupManager>.GetInstance().onDismiss();
}
