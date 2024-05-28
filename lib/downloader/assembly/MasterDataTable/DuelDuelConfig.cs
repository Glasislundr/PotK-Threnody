// Decompiled with JetBrains decompiler
// Type: MasterDataTable.DuelDuelConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class DuelDuelConfig
  {
    public int ID;
    public string controller_name;
    public int move_type_DuelMoveTypeEnum;
    public float myReach;
    public float attackWaitTime1;
    public float attackWaitTime2;
    public float criticalCutinWaitTime;
    public float attackWaitTimeCritical;
    public float suiseiAttackSWaitTime;
    public float suiseiFirstAttackWaitTime;
    public float suiseiAttack1WaitTime;
    public float suiseiAddTime;
    public float skillAttackSWaitTime;
    public float skillFirstAttackWaitTime;
    public float skillAttack1WaitTime;
    public float skillAddTime;
    public string runEaseType;
    public float runSpeed;
    public float bsDelay;
    public float bsDuration;
    public float attackBeginOffsetTime;
    public float attack1ForwardDistance;
    public string attack1ForwardEaseType;
    public float attack1ForwardSpeed;
    public float attack1MoveStartTime;
    public float attack1ForwardKnockBack;
    public float attack2ForwardDistance;
    public string attack2ForwardEaseType;
    public float attack2ForwardSpeed;
    public float attack2MoveStartTime;
    public float attack2ForwardKnockBack;
    public float attackSForwardDistance;
    public string attackSForwardEaseType;
    public float attackSForwardSpeed;
    public float attackSMoveStartTime;
    public float attackSForwardKnockBack;
    public float dodgeTime;
    public float dodgeDistance;
    public int noRunAttack;

    public List<string> preloadEffectFileNameList
    {
      get
      {
        return ((IEnumerable<DuelEffectPreload>) MasterData.DuelEffectPreloadList).Where<DuelEffectPreload>((Func<DuelEffectPreload, bool>) (x => this.ID == x.duel_controller_id)).Select<DuelEffectPreload, string>((Func<DuelEffectPreload, string>) (x => x.effect_file_name)).ToList<string>();
      }
    }

    public static DuelDuelConfig Parse(MasterDataReader reader)
    {
      return new DuelDuelConfig()
      {
        ID = reader.ReadInt(),
        controller_name = reader.ReadString(true),
        move_type_DuelMoveTypeEnum = reader.ReadInt(),
        myReach = reader.ReadFloat(),
        attackWaitTime1 = reader.ReadFloat(),
        attackWaitTime2 = reader.ReadFloat(),
        criticalCutinWaitTime = reader.ReadFloat(),
        attackWaitTimeCritical = reader.ReadFloat(),
        suiseiAttackSWaitTime = reader.ReadFloat(),
        suiseiFirstAttackWaitTime = reader.ReadFloat(),
        suiseiAttack1WaitTime = reader.ReadFloat(),
        suiseiAddTime = reader.ReadFloat(),
        skillAttackSWaitTime = reader.ReadFloat(),
        skillFirstAttackWaitTime = reader.ReadFloat(),
        skillAttack1WaitTime = reader.ReadFloat(),
        skillAddTime = reader.ReadFloat(),
        runEaseType = reader.ReadString(true),
        runSpeed = reader.ReadFloat(),
        bsDelay = reader.ReadFloat(),
        bsDuration = reader.ReadFloat(),
        attackBeginOffsetTime = reader.ReadFloat(),
        attack1ForwardDistance = reader.ReadFloat(),
        attack1ForwardEaseType = reader.ReadString(true),
        attack1ForwardSpeed = reader.ReadFloat(),
        attack1MoveStartTime = reader.ReadFloat(),
        attack1ForwardKnockBack = reader.ReadFloat(),
        attack2ForwardDistance = reader.ReadFloat(),
        attack2ForwardEaseType = reader.ReadString(true),
        attack2ForwardSpeed = reader.ReadFloat(),
        attack2MoveStartTime = reader.ReadFloat(),
        attack2ForwardKnockBack = reader.ReadFloat(),
        attackSForwardDistance = reader.ReadFloat(),
        attackSForwardEaseType = reader.ReadString(true),
        attackSForwardSpeed = reader.ReadFloat(),
        attackSMoveStartTime = reader.ReadFloat(),
        attackSForwardKnockBack = reader.ReadFloat(),
        dodgeTime = reader.ReadFloat(),
        dodgeDistance = reader.ReadFloat(),
        noRunAttack = reader.ReadInt()
      };
    }

    public DuelMoveTypeEnum move_type => (DuelMoveTypeEnum) this.move_type_DuelMoveTypeEnum;
  }
}
