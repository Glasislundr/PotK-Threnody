// Decompiled with JetBrains decompiler
// Type: Shop00716SetText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Shop00716SetText : MonoBehaviour
{
  private SetPopupText spt;

  private void Start()
  {
  }

  private void Update()
  {
  }

  public void SetText(int prev, int next)
  {
    this.spt = ((Component) this).GetComponent<SetPopupText>();
    if (!Object.op_Implicit((Object) this.spt))
      return;
    this.spt.SetText(Consts.Format(Consts.GetInstance().SHOP_00716_SET_TEXT, (IDictionary) new Hashtable()
    {
      {
        (object) "item",
        (object) Consts.GetInstance().UNIQUE_ICON_KISEKI
      },
      {
        (object) "number",
        (object) "１"
      },
      {
        (object) "unit",
        (object) Consts.GetInstance().SHOP_00710_MENU
      }
    }), false);
  }
}
