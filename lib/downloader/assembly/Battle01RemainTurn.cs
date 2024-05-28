// Decompiled with JetBrains decompiler
// Type: Battle01RemainTurn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Battle01RemainTurn : NGBattleMenuBase
{
  private BL.BattleModified<BL.PhaseState> stateModified;
  private BL.BattleModified<BL.Condition> conditionModified;
  [SerializeField]
  private UILabel txt_turn;
  [SerializeField]
  private UILabel txt_3turn;
  [SerializeField]
  private UILabel txt_3turn_survive;
  [SerializeField]
  private GameObject effect;
  [SerializeField]
  private GameObject effect_3turn;
  [SerializeField]
  private GameObject effect_3turn_survive;
  [SerializeField]
  private int redLimit = 3;
  private int turn;
  private Color survive_color = new Color(0.0f, 1f, 1f);

  private int remainTurn()
  {
    if (this.env.core.condition.isTurn)
      return this.env.core.condition.turn - this.env.core.phaseState.turnCount;
    return this.env.core.condition.isElapsedTurn ? Mathf.Max(0, this.env.core.condition.elapsedTurn - this.env.core.phaseState.turnCount) : 0;
  }

  private void setTurn(int rt)
  {
    if (rt > this.redLimit)
    {
      this.effect.SetActive(true);
      this.effect_3turn.SetActive(false);
      this.effect_3turn_survive.SetActive(false);
      ((UIWidget) this.txt_turn).color = Color.white;
    }
    else if (this.env.core.condition.isElapsedTurn)
    {
      this.effect.SetActive(false);
      this.effect_3turn.SetActive(false);
      this.effect_3turn_survive.SetActive(true);
      ((UIWidget) this.txt_turn).color = this.survive_color;
      ((Behaviour) ((Component) this.txt_turn).GetComponent<UITweener>()).enabled = true;
    }
    else
    {
      this.effect.SetActive(false);
      this.effect_3turn.SetActive(true);
      this.effect_3turn_survive.SetActive(false);
      ((UIWidget) this.txt_turn).color = Color.red;
      ((Behaviour) ((Component) this.txt_turn).GetComponent<UITweener>()).enabled = true;
    }
    this.setText(this.txt_turn, rt);
    this.setText(this.txt_3turn, rt);
    this.setText(this.txt_3turn_survive, rt);
    this.turn = rt;
  }

  protected override IEnumerator Start_Battle()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Battle01RemainTurn battle01RemainTurn = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    battle01RemainTurn.stateModified = BL.Observe<BL.PhaseState>(battle01RemainTurn.env.core.phaseState);
    battle01RemainTurn.conditionModified = BL.Observe<BL.Condition>(battle01RemainTurn.env.core.condition);
    battle01RemainTurn.turn = battle01RemainTurn.remainTurn();
    battle01RemainTurn.setTurn(battle01RemainTurn.turn);
    return false;
  }

  protected override void Update_Battle()
  {
    if (this.conditionModified.isChangedOnce())
    {
      this.turn = this.remainTurn();
      this.setTurn(this.turn);
    }
    if (!this.stateModified.isChangedOnce())
      return;
    int rt = this.remainTurn();
    if (rt == this.turn)
      return;
    this.setTurn(rt);
    this.turn = rt;
  }
}
