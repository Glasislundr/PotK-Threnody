// Decompiled with JetBrains decompiler
// Type: PledgeHimeNum
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PledgeHimeNum : MonoBehaviour
{
  [SerializeField]
  private UILabel himeNumLabel;

  public void SetPledgeNum(int num) => this.himeNumLabel.SetTextLocalize(num.ToString());
}
