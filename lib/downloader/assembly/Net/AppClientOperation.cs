// Decompiled with JetBrains decompiler
// Type: Net.AppClientOperation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
namespace Net
{
  public enum AppClientOperation
  {
    StatRequest,
    JoinRoom,
    ReadyCompleted,
    LocateUnitsCompleted,
    TurnInitializeCompleted,
    MoveUnitTimeout,
    MoveUnit,
    MoveUnitWithAttack,
    MoveUnitWithSkill,
    ActionUnitCompleted,
    WipedOutCompleted,
    RecoveryRequest,
    AutoOnRequest,
    UseCallSkill,
  }
}
