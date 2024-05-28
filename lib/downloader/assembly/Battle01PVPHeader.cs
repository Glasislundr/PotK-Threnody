// Decompiled with JetBrains decompiler
// Type: Battle01PVPHeader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Battle01PVPHeader : BattleMonoBehaviour
{
  [SerializeField]
  private SpriteNumber remainTurn;
  [SerializeField]
  private SpriteNumber remainTime;
  [SerializeField]
  private Battle01PVPSetPointGauge playerGauge;
  [SerializeField]
  private Battle01PVPSetPointGauge enemyGauge;
  [SerializeField]
  private GameObject remainPlayer;
  [SerializeField]
  private GameObject remainEnemy;
  [SerializeField]
  private Animator playerWipedOutAnime;
  [SerializeField]
  private Animator enemyWipedOutAnime;
  private BL.BattleModified<BL.StructValue<int>> remainTurnModified;
  private BL.BattleModified<BL.StructValue<int>> timeLimitModified;
  private BL.BattleModified<BL.StructValue<int>> playerPointModified;
  private BL.BattleModified<BL.StructValue<int>> enemyPointModified;
  private BL.BattleModified<BL.StructValue<bool>> isPlayerWipedOutModified;
  private BL.BattleModified<BL.StructValue<bool>> isEnemyWipedOutModified;
  private BL.BattleModified<BL.PhaseState> phaseStateModified;
  private SpriteNumberSelectDirect[] numberDirects;
  private BattleTimeManager _btm;

  public Battle01PVPSetPointGauge PlayerGauge => this.playerGauge;

  public Battle01PVPSetPointGauge EnemyGauge => this.enemyGauge;

  private BattleTimeManager btm
  {
    get
    {
      if (Object.op_Equality((Object) this._btm, (Object) null))
        this._btm = this.battleManager.getManager<BattleTimeManager>();
      return this._btm;
    }
  }

  protected override IEnumerator Start_Battle()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Battle01PVPHeader battle01PvpHeader = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    IGameEngine gameEngine = battle01PvpHeader.battleManager.gameEngine;
    if (gameEngine == null)
      return false;
    battle01PvpHeader.numberDirects = ((Component) battle01PvpHeader.remainTime).gameObject.GetComponentsInChildren<SpriteNumberSelectDirect>();
    battle01PvpHeader.remainTurnModified = BL.Observe<BL.StructValue<int>>(gameEngine.remainTurn);
    battle01PvpHeader.timeLimitModified = BL.Observe<BL.StructValue<int>>(gameEngine.timeLimit);
    battle01PvpHeader.playerPointModified = BL.Observe<BL.StructValue<int>>(gameEngine.playerPoint);
    battle01PvpHeader.enemyPointModified = BL.Observe<BL.StructValue<int>>(gameEngine.enemyPoint);
    battle01PvpHeader.isPlayerWipedOutModified = BL.Observe<BL.StructValue<bool>>(gameEngine.isPlayerWipedOut);
    battle01PvpHeader.isEnemyWipedOutModified = BL.Observe<BL.StructValue<bool>>(gameEngine.isEnemyWipedOut);
    battle01PvpHeader.phaseStateModified = BL.Observe<BL.PhaseState>(battle01PvpHeader.env.core.phaseState);
    battle01PvpHeader.playerGauge.initValue(gameEngine.endPoint - battle01PvpHeader.enemyPointModified.value.value, gameEngine.endPoint);
    battle01PvpHeader.enemyGauge.initValue(gameEngine.endPoint - battle01PvpHeader.playerPointModified.value.value, gameEngine.endPoint);
    battle01PvpHeader.env.core.playerPointGauge = gameEngine.endPoint - battle01PvpHeader.enemyPointModified.value.value;
    battle01PvpHeader.env.core.enemyPointGauge = gameEngine.endPoint - battle01PvpHeader.playerPointModified.value.value;
    return false;
  }

  protected override void Update_Battle()
  {
    IGameEngine gameEngine = this.battleManager.gameEngine;
    if (gameEngine == null)
      return;
    if (this.phaseStateModified.isChangedOnce())
    {
      BL.PhaseState phaseState = this.phaseStateModified.value;
      if (phaseState.state == BL.Phase.pvp_enemy_start)
      {
        this.remainPlayer.SetActive(false);
        this.remainEnemy.SetActive(true);
        foreach (Component numberDirect in this.numberDirects)
          numberDirect.gameObject.SetActive(false);
      }
      else if (phaseState.state == BL.Phase.pvp_player_start)
      {
        this.remainPlayer.SetActive(true);
        this.remainEnemy.SetActive(false);
        foreach (Component numberDirect in this.numberDirects)
          numberDirect.gameObject.SetActive(true);
      }
    }
    if (this.remainTurnModified.isChangedOnce())
      this.remainTurn.setNumber(this.remainTurnModified.value.value);
    if (this.timeLimitModified.isChangedOnce())
      this.remainTime.setNumber(this.timeLimitModified.value.value);
    if (this.playerPointModified.isChangedOnce())
    {
      if (this.isPlayerWipedOutModified.isChangedOnce() && this.isPlayerWipedOutModified.value.value)
      {
        this.btm.setScheduleAction((Action) (() =>
        {
          this.enemyWipedOutAnime.SetTrigger("isPlay");
          if (!this.battleManager.isPvp)
            return;
          this.battleManager.gameEngine.deadReserveToPoint(false, true);
        }));
        this.btm.setEnableWait(0.1f);
        this.isPlayerWipedOutModified.value.value = false;
      }
      this.enemyGauge.setValue(gameEngine.endPoint - this.playerPointModified.value.value, gameEngine.endPoint);
      this.env.core.enemyPointGauge = gameEngine.endPoint - this.playerPointModified.value.value;
    }
    if (this.enemyPointModified.isChangedOnce())
    {
      if (this.isEnemyWipedOutModified.isChangedOnce() && this.isEnemyWipedOutModified.value.value)
      {
        this.btm.setScheduleAction((Action) (() =>
        {
          this.playerWipedOutAnime.SetTrigger("isPlay");
          if (!this.battleManager.isPvp)
            return;
          this.battleManager.gameEngine.deadReserveToPoint(true, true);
        }));
        this.btm.setEnableWait(0.1f);
        this.isEnemyWipedOutModified.value.value = false;
      }
      this.playerGauge.setValue(gameEngine.endPoint - this.enemyPointModified.value.value, gameEngine.endPoint);
      this.env.core.playerPointGauge = gameEngine.endPoint - this.enemyPointModified.value.value;
    }
    if (this.battleManager.isPvnpc)
    {
      this.battleManager.pvnpcManager.execNextState((BattleMonoBehaviour) this);
    }
    else
    {
      if (!this.battleManager.isGvg)
        return;
      this.battleManager.gvgManager.execNextState((BattleMonoBehaviour) this);
    }
  }
}
