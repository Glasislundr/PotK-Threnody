// Decompiled with JetBrains decompiler
// Type: Battle01PVPRespawnCount
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Battle01PVPRespawnCount : NGBattleMenuBase
{
  [SerializeField]
  private UILabel countLabel;

  public void setCount(int n) => this.setText(this.countLabel, string.Concat((object) n));
}
