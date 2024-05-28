// Decompiled with JetBrains decompiler
// Type: GameCore.ColosseumResult
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MiniJSON;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace GameCore
{
  public class ColosseumResult
  {
    public string colosseumTransactionID;
    private string salt;
    public DuelColosseumResult[] duelResult;
    public int win_count;
    public int lose_count;
    public int draw_count;
    public int myTotalDamage;
    public int opponetTotalDamage;

    public ColosseumResult()
    {
      this.colosseumTransactionID = "dummy";
      this.duelResult = new DuelColosseumResult[5];
      this.win_count = 0;
      this.lose_count = 0;
      this.draw_count = 0;
      this.myTotalDamage = 0;
      this.opponetTotalDamage = 0;
    }

    public ColosseumResult(string transactionID, string opponent_id)
    {
      this.colosseumTransactionID = transactionID;
      this.salt = opponent_id;
      this.duelResult = new DuelColosseumResult[5];
      this.win_count = 0;
      this.lose_count = 0;
      this.draw_count = 0;
      this.myTotalDamage = 0;
      this.opponetTotalDamage = 0;
    }

    public bool isWin()
    {
      return this.win_count > this.lose_count || this.win_count == this.lose_count && this.myTotalDamage > this.opponetTotalDamage;
    }

    public void SetData(int index, DuelColosseumResult result)
    {
      if (index >= this.duelResult.Length)
        return;
      this.duelResult[index] = result;
      switch (result.judgment)
      {
        case 0:
          ++this.draw_count;
          break;
        case 1:
          ++this.win_count;
          break;
        case 2:
          ++this.lose_count;
          break;
      }
      this.opponetTotalDamage += result.playerFromDamage;
      this.myTotalDamage += result.opponentFromDamage;
    }

    public string GetResultJsonString()
    {
      Dictionary<string, object> dictionary1 = new Dictionary<string, object>();
      for (int index = 0; index < this.duelResult.Length; ++index)
      {
        DuelColosseumResult result = this.duelResult[index];
        Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
        Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
        Dictionary<string, object> dictionary4 = new Dictionary<string, object>();
        if (result.player != (BL.Unit) null)
        {
          Judgement.BattleParameter battleParameter = Judgement.BattleParameter.FromBeColosseumUnit(result.player, result.playerEq, result.playerEq2, result.playerReisou, result.playerReisou2, result.playerReisou3);
          dictionary3.Add("player_unit_id", (object) result.player.playerUnit.id);
          dictionary3.Add("is_first_attack", (object) result.isPlayerFirstAttacker);
          dictionary3.Add("attack_damage", (object) result.opponentFromDamage);
          if (result.turns != null)
            dictionary3.Add("is_critical", (object) (((IEnumerable<BL.DuelTurn>) result.turns).Count<BL.DuelTurn>((Func<BL.DuelTurn, bool>) (x => x.isCritical && x.isAtackker == result.isPlayerFirstAttacker)) >= 1));
          else
            dictionary3.Add("is_critical", (object) false);
          dictionary3.Add("combat", (object) battleParameter.Combat);
          dictionary3.Add("combat_incr", (object) battleParameter.CombatIncr);
          dictionary3.Add("player_item_ids", (object) this.GetPlayerGearIds(result));
        }
        else
        {
          dictionary3.Add("player_unit_id", (object) null);
          dictionary3.Add("player_item_ids", (object) new int[0]);
          dictionary3.Add("is_first_attack", (object) false);
          dictionary3.Add("attack_damage", (object) 0);
          dictionary3.Add("is_critical", (object) false);
          dictionary3.Add("combat", (object) 0);
          dictionary3.Add("combat_incr", (object) 0);
        }
        if (result.opponent != (BL.Unit) null)
        {
          Judgement.BattleParameter battleParameter = Judgement.BattleParameter.FromBeColosseumUnit(result.opponent, result.opponentEq, result.opponentEq2, result.opponentReisou, result.opponentReisou2, result.opponentReisou3);
          dictionary4.Add("player_unit_id", (object) result.opponent.playerUnit.id);
          dictionary4.Add("is_first_attack", (object) !result.isPlayerFirstAttacker);
          dictionary4.Add("attack_damage", (object) result.playerFromDamage);
          if (result.turns != null)
            dictionary4.Add("is_critical", (object) (((IEnumerable<BL.DuelTurn>) result.turns).Count<BL.DuelTurn>((Func<BL.DuelTurn, bool>) (x => x.isCritical && x.isAtackker == !result.isPlayerFirstAttacker)) >= 1));
          else
            dictionary4.Add("is_critical", (object) false);
          dictionary4.Add("combat", (object) battleParameter.Combat);
          dictionary4.Add("combat_incr", (object) battleParameter.CombatIncr);
          dictionary4.Add("player_item_ids", (object) this.GetOpponentGearIds(result));
        }
        else
        {
          dictionary4.Add("player_unit_id", (object) null);
          dictionary4.Add("player_item_ids", (object) new int[0]);
          dictionary4.Add("is_first_attack", (object) false);
          dictionary4.Add("attack_damage", (object) 0);
          dictionary4.Add("is_critical", (object) false);
          dictionary4.Add("combat", (object) 0);
          dictionary4.Add("combat_incr", (object) 0);
        }
        dictionary2["player"] = (object) dictionary3;
        dictionary2["opponent"] = (object) dictionary4;
        dictionary2["result"] = (object) result.judgment;
        dictionary1.Add((index + 1).ToString(), (object) dictionary2);
      }
      return AES.Encrypt(this.colosseumTransactionID, this.salt, Json.Serialize((object) dictionary1));
    }

    private int[] GetPlayerGearIds(DuelColosseumResult data)
    {
      List<int> intList = new List<int>();
      if (data.playerEq != (PlayerItem) null)
        intList.Add(data.playerEq.id);
      if (data.playerEq2 != (PlayerItem) null)
        intList.Add(data.playerEq2.id);
      if (data.playerReisou != (PlayerItem) null)
        intList.Add(data.playerReisou.id);
      if (data.playerReisou2 != (PlayerItem) null)
        intList.Add(data.playerReisou2.id);
      return intList.ToArray();
    }

    private int[] GetOpponentGearIds(DuelColosseumResult data)
    {
      List<int> intList = new List<int>();
      if (data.opponentEq != (PlayerItem) null)
        intList.Add(data.opponentEq.id);
      if (data.opponentEq2 != (PlayerItem) null)
        intList.Add(data.opponentEq2.id);
      if (data.opponentReisou != (PlayerItem) null)
        intList.Add(data.opponentReisou.id);
      if (data.opponentReisou2 != (PlayerItem) null)
        intList.Add(data.opponentReisou2.id);
      return intList.ToArray();
    }
  }
}
