// Decompiled with JetBrains decompiler
// Type: Battle01UnitCounter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Battle01UnitCounter : NGBattleMenuBase
{
  [SerializeField]
  private SelectParts selectParts;
  [SerializeField]
  private UILabel count_txt;

  public void setCount(int c)
  {
    if (c <= 0)
    {
      this.selectParts.setValue(1);
    }
    else
    {
      this.selectParts.setValue(0);
      this.setText(this.count_txt, c);
    }
  }
}
