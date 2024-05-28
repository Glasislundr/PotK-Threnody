// Decompiled with JetBrains decompiler
// Type: Transfer01289Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Transfer01289Menu : MonoBehaviour
{
  [SerializeField]
  protected UILabel TxtDescription;

  public void ChangeDescription(string errorCode)
  {
    string text;
    switch (errorCode)
    {
      case "ASE001":
        text = Consts.GetInstance().POPUP_012_8_9_ASE001_TEXT;
        break;
      case "ASE010":
        text = Consts.GetInstance().POPUP_012_8_9_ASE010_TEXT;
        break;
      case "unknown":
        text = Consts.GetInstance().POPUP_012_8_9_UNKNOWN_TEXT;
        break;
      default:
        text = Consts.Format(Consts.GetInstance().POPUP_012_8_9_OTHER_TEXT, (IDictionary) new Hashtable()
        {
          {
            (object) "error_code",
            (object) errorCode
          }
        });
        break;
    }
    this.TxtDescription.SetTextLocalize(text);
  }

  public string TimeIntToString(int time)
  {
    return time >= 0 && time < 10 ? "０" + time.ToLocalizeNumberText() : time.ToLocalizeNumberText();
  }
}
