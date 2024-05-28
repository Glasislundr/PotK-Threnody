// Decompiled with JetBrains decompiler
// Type: DuelTime
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;

#nullable disable
public class DuelTime
{
  public float attackWaitTime1 = 0.3f;
  public float attackWaitTime2 = 3.5f;
  public float criticalCutinWaitTime;
  public float attackWaitTimeCritical = 6f;
  public float koyuSkillWaitTime = 8f;
  public float suiseiAttackSWaitTime = 0.866f;
  public float suiseiFirstAttackWaitTime = 0.3f;
  public float suiseiAttack1WaitTime = 1.4f;
  public float suiseiAttack2WaitTime = 6f;
  public float skillAttackSWaitTime = 0.866f;
  public float skillFirstAttackWaitTime = 0.3f;
  public float skillAttack1WaitTime = 1.4f;
  public float skillAttack2WaitTime = 10f;
  public string runEaseType = "linear";
  public float runSpeed = 12f;
  public float bsSpeed = 0.83f;
  public float bsDelay = 0.1f;
  public float bsDuration = 0.4f;
  public float characterInitOffset;
  public float attackBeginOffsetTime = 0.2f;
  public float attack1ForwardDistance = 1f;
  public string attack1ForwardEaseType = "linear";
  public float attack1ForwardSpeed = 8f;
  public float attack1ForwardKnockBack = 1f;
  public float attack1MoveStartTime;
  public float attack1MoveEndTime = 0.8f;
  public float attack2ForwardDistance = 0.5f;
  public string attack2ForwardEaseType = "linear";
  public float attack2ForwardSpeed = 8f;
  public float attack2ForwardKnockBack = 2f;
  public float attack2MoveStartTime;
  public float attack2MoveEndTime = 1f;
  public float attackSForwardDistance = 1f;
  public string attackSForwardEaseType = "linear";
  public float attackSForwardSpeed = 8f;
  public float attackSForwardKnockBack = 3f;
  public float attackSMoveStartTime;
  public float attackSMoveEndTime = 1f;
  public float missileWeapon1Start = 0.3f;
  public float missileWeapon2Start = 0.3f;
  public float missileWeaponSStart = 0.85f;
  public float waitTimeOfMoveOrigPos = 1.5f;
  public float suiseiAddTime;
  public float skillAddTime;
  public float dodgeTime = 0.8f;
  public float dodgeDistance = 0.5f;

  public DuelTime()
  {
  }

  public DuelTime(DuelDuelConfig config)
  {
    this.attack1ForwardDistance = config.attack1ForwardDistance;
    this.attack1ForwardEaseType = config.attack1ForwardEaseType;
    this.attack1ForwardKnockBack = config.attack1ForwardKnockBack;
    this.attack1ForwardSpeed = config.attack1ForwardSpeed;
    this.attack1MoveStartTime = config.attack1MoveStartTime;
    this.attack2ForwardDistance = config.attack2ForwardDistance;
    this.attack2ForwardEaseType = config.attack2ForwardEaseType;
    this.attack2ForwardKnockBack = config.attack2ForwardKnockBack;
    this.attack2ForwardSpeed = config.attack2ForwardSpeed;
    this.attack2MoveStartTime = config.attack2MoveStartTime;
    this.attackBeginOffsetTime = config.attackBeginOffsetTime;
    this.attackSForwardDistance = config.attackSForwardDistance;
    this.attackSForwardEaseType = config.attackSForwardEaseType;
    this.attackSForwardKnockBack = config.attackSForwardKnockBack;
    this.attackSForwardSpeed = config.attackSForwardSpeed;
    this.attackSMoveStartTime = config.attackSMoveStartTime;
    this.attackWaitTime1 = config.attackWaitTime1;
    this.attackWaitTime2 = config.attackWaitTime2;
    this.attackWaitTimeCritical = config.attackWaitTimeCritical;
    this.bsDelay = config.bsDelay;
    this.bsDuration = config.bsDuration;
    this.criticalCutinWaitTime = config.criticalCutinWaitTime;
    this.dodgeDistance = config.dodgeDistance;
    this.dodgeTime = config.dodgeTime;
    this.runEaseType = config.runEaseType;
    this.runSpeed = config.runSpeed;
    this.skillAddTime = config.skillAddTime;
    this.skillAttack1WaitTime = config.skillAttack1WaitTime;
    this.skillAttackSWaitTime = config.skillAttackSWaitTime;
    this.skillFirstAttackWaitTime = config.skillFirstAttackWaitTime;
    this.suiseiAddTime = config.suiseiAddTime;
    this.suiseiAttack1WaitTime = config.suiseiAttack1WaitTime;
    this.suiseiAttackSWaitTime = config.suiseiAttackSWaitTime;
    this.suiseiFirstAttackWaitTime = config.suiseiFirstAttackWaitTime;
  }
}
