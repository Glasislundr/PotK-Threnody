// Decompiled with JetBrains decompiler
// Type: Popup00441AddEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Popup00441AddEffect : MonoBehaviour
{
  [SerializeField]
  private GameObject[] BG;
  [SerializeField]
  private UILabel[] TxtStatus;
  [SerializeField]
  private UILabel TxtNone;

  public void Init(int value)
  {
    Popup00441AddEffect.Type type = Popup00441AddEffect.Type.NONE;
    if (value > 0)
      type = Popup00441AddEffect.Type.ADD;
    else if (value < 0)
      type = Popup00441AddEffect.Type.SUB;
    ((IEnumerable<GameObject>) this.BG).ToggleOnce(-1);
    foreach (Component txtStatu in this.TxtStatus)
      txtStatu.gameObject.SetActive(false);
    ((Component) this.TxtNone).gameObject.SetActive(false);
    if (type == Popup00441AddEffect.Type.ADD)
    {
      ((IEnumerable<GameObject>) this.BG).ToggleOnce(1);
      this.TxtStatus[1].SetTextLocalize(Math.Abs(value));
      ((Component) this.TxtStatus[1]).gameObject.SetActive(true);
    }
    else if (type == Popup00441AddEffect.Type.SUB)
    {
      ((IEnumerable<GameObject>) this.BG).ToggleOnce(0);
      this.TxtStatus[0].SetTextLocalize(Math.Abs(value));
      ((Component) this.TxtStatus[0]).gameObject.SetActive(true);
    }
    else
      ((Component) this.TxtNone).gameObject.SetActive(true);
  }

  private enum Type
  {
    SUB,
    ADD,
    NONE,
  }
}
