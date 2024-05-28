// Decompiled with JetBrains decompiler
// Type: SeaHomeUnitAnimeCallback
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class SeaHomeUnitAnimeCallback : MonoBehaviour
{
  [NonSerialized]
  public SeaHomeUnitController controller;

  private void StartMove(float acceleration) => this.controller.StartMove(acceleration);

  private void LoopMove() => this.controller.LoopMove();

  private void CheckStopMove() => this.controller.CheckStopMove();

  private void StopStartMove(float acceleration) => this.controller.StopStartMove(acceleration);

  private void StopMove() => this.controller.StopMove();

  private void EndDance() => this.controller.SetUnitStatus(SeaHomeUnitController.UnitStatus.Stand);
}
