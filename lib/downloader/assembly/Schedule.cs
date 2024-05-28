// Decompiled with JetBrains decompiler
// Type: Schedule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using UnityEngine;

#nullable disable
public class Schedule
{
  public bool isSetBattleEnable;
  public bool isBattleEnable;
  public float startTime;
  public BL.Phase state = BL.Phase.unset;
  public Action endAction;
  public bool isInsertMode;

  public virtual bool body() => true;

  public virtual bool completedp() => true;

  public float time => Time.time;

  public float deltaTime => Time.time - this.startTime;

  public bool execBody()
  {
    BattleTimeManager manager = Singleton<NGBattleManager>.GetInstance().getManager<BattleTimeManager>();
    bool insertMode = manager.insertMode;
    manager.insertMode = this.isInsertMode;
    int num = this.body() ? 1 : 0;
    manager.insertMode = insertMode;
    return num != 0;
  }
}
