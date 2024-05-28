// Decompiled with JetBrains decompiler
// Type: AddStatus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class AddStatus : MonoBehaviour
{
  [SerializeField]
  private GameObject[] BG;
  [SerializeField]
  private UILabel TxtCategory;
  [SerializeField]
  private UILabel TxtOperator;
  [SerializeField]
  private UILabel[] TxtStatus;

  public void Init(string category, int value)
  {
    AddStatus.Type index = value > 0 ? AddStatus.Type.ADD : AddStatus.Type.SUB;
    ((IEnumerable<GameObject>) this.BG).ToggleOnce((int) index);
    this.TxtCategory.SetText(category);
    foreach (Component txtStatu in this.TxtStatus)
      txtStatu.gameObject.SetActive(false);
    if (index == AddStatus.Type.ADD)
    {
      this.TxtOperator.SetText(Consts.GetInstance().PLUS);
      this.TxtStatus[(int) index].SetTextLocalize(Math.Abs(value));
      ((Component) this.TxtStatus[(int) index]).gameObject.SetActive(true);
    }
    else
    {
      this.TxtOperator.SetText(Consts.GetInstance().MINUS);
      this.TxtStatus[(int) index].SetTextLocalize(Math.Abs(value));
      ((Component) this.TxtStatus[(int) index]).gameObject.SetActive(true);
    }
  }

  private enum Type
  {
    SUB,
    ADD,
  }
}
