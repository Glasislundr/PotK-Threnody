// Decompiled with JetBrains decompiler
// Type: GameCore.IGameEngine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace GameCore
{
  public interface IGameEngine
  {
    bool isDisposition { get; }

    bool isWaitAction { get; }

    int endPoint { get; }

    string playerName { get; }

    string enemyName { get; }

    int playerEmblem { get; }

    int enemyEmblem { get; }

    BL.StructValue<int> remainTurn { get; }

    BL.StructValue<int> timeLimit { get; }

    BL.StructValue<int> playerPoint { get; }

    BL.StructValue<int> enemyPoint { get; }

    BL.StructValue<bool> isPlayerWipedOut { get; }

    BL.StructValue<bool> isEnemyWipedOut { get; }

    HashSet<BL.Panel> formationPanel { get; }

    void startMain();

    void locateUnitsCompleted();

    void turnInitializeCompleted();

    void actionUnitCompleted();

    void wipedOutCompleted();

    void moveUnit(BL.UnitPosition up);

    void moveUnitWithAttack(
      BL.UnitPosition attack,
      BL.UnitPosition defense,
      bool isHeal,
      int attackStatusIndex);

    void moveUnitWithAttack(BL.Unit attack, BL.Unit defense, bool isHeal, int attackStatusIndex);

    void moveUnitWithSkill(
      BL.Unit unit,
      BL.Skill skill,
      List<BL.Unit> targets,
      List<BL.Panel> penels);

    void readyComplited();

    void autoOnRequest();

    void useCallSkill(BL.Skill skill, List<BL.Unit> targets, bool isPlayer);

    void applyDeadUnit(BL.Unit attack, BL.Unit defense);

    void deadReserveToPoint(bool isEnemy, bool needAnnihilateCheck);
  }
}
