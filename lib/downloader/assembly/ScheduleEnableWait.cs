﻿// Decompiled with JetBrains decompiler
// Type: ScheduleEnableWait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
public class ScheduleEnableWait : Schedule
{
  private float wait;

  public ScheduleEnableWait(float wait)
  {
    this.wait = wait;
    this.isSetBattleEnable = true;
    this.isBattleEnable = false;
  }

  public override bool completedp() => (double) this.deltaTime > (double) this.wait;
}
