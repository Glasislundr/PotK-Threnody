﻿// Decompiled with JetBrains decompiler
// Type: ScheduleEnableFuncWait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
public class ScheduleEnableFuncWait : Schedule
{
  private Func<bool> waitF;

  public ScheduleEnableFuncWait(Func<bool> waitF)
  {
    this.waitF = waitF;
    this.isSetBattleEnable = true;
    this.isBattleEnable = false;
  }

  public override bool completedp() => this.waitF();
}
