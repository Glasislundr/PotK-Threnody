// Decompiled with JetBrains decompiler
// Type: GameCore.DuelColosseumResult
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;

#nullable disable
namespace GameCore
{
  public class DuelColosseumResult
  {
    public const int DRAW = 0;
    public const int PLAYER_WIN = 1;
    public const int OPPONENT_WIN = 2;
    public bool isPlayerFirstAttacker;
    public BL.Unit player;
    public PlayerItem playerEq;
    public PlayerItem playerEq2;
    public PlayerItem playerEq3;
    public PlayerItem playerReisou;
    public PlayerItem playerReisou2;
    public PlayerItem playerReisou3;
    public ColosseumBeforBonusParam playerBeforeBonusParam;
    public AttackStatus playerAttackStatus;
    public AttackStatus colosseumNewPAS;
    public int colosseumPlayerFirstAttack;
    public Bonus[] playerActiveBonus;
    public IntimateDuelSupport playerDuelSupport;
    public BL.Unit opponent;
    public PlayerItem opponentEq;
    public PlayerItem opponentEq2;
    public PlayerItem opponentEq3;
    public PlayerItem opponentReisou;
    public PlayerItem opponentReisou2;
    public PlayerItem opponentReisou3;
    public ColosseumBeforBonusParam opponentBeforeBonusParam;
    public AttackStatus opponentAttackStatus;
    public AttackStatus colosseumNewOAS;
    public int colosseumOpponentFirstAttack;
    public Bonus[] opponentActiveBonus;
    public IntimateDuelSupport opponentDuelSupport;
    public BL.DuelTurn[] turns;
    public int playerDamage;
    public int playerFromDamage;
    public bool isDiePlayer;
    public int opponentDamage;
    public int opponentFromDamage;
    public bool isDieOpponent;
    public bool isExploreChallenge;

    public int judgment
    {
      get
      {
        if (this.isDiePlayer != this.isDieOpponent)
        {
          if (this.isDiePlayer)
            return 2;
          if (this.isDieOpponent)
            return 1;
        }
        return 0;
      }
    }

    public int firstAttackerDamage
    {
      get => !this.isPlayerFirstAttacker ? this.opponentDamage : this.playerDamage;
      set
      {
        if (this.isPlayerFirstAttacker)
          this.playerDamage = value;
        else
          this.opponentDamage = value;
      }
    }

    public int firstAttackerFromDamage
    {
      get => !this.isPlayerFirstAttacker ? this.opponentFromDamage : this.playerFromDamage;
      set
      {
        if (this.isPlayerFirstAttacker)
          this.playerFromDamage = value;
        else
          this.opponentFromDamage = value;
      }
    }

    public bool isDieFirstAttacker
    {
      get => !this.isPlayerFirstAttacker ? this.isDieOpponent : this.isDiePlayer;
      set
      {
        if (this.isPlayerFirstAttacker)
          this.isDiePlayer = value;
        else
          this.isDieOpponent = value;
      }
    }

    public int secondAttackerDamage
    {
      get => this.isPlayerFirstAttacker ? this.opponentDamage : this.playerDamage;
      set
      {
        if (!this.isPlayerFirstAttacker)
          this.playerDamage = value;
        else
          this.opponentDamage = value;
      }
    }

    public int secondAttackerFromDamage
    {
      get => this.isPlayerFirstAttacker ? this.opponentFromDamage : this.playerFromDamage;
      set
      {
        if (!this.isPlayerFirstAttacker)
          this.playerFromDamage = value;
        else
          this.opponentFromDamage = value;
      }
    }

    public bool isDieSecondAttacker
    {
      get => this.isPlayerFirstAttacker ? this.isDieOpponent : this.isDiePlayer;
      set
      {
        if (!this.isPlayerFirstAttacker)
          this.isDiePlayer = value;
        else
          this.isDieOpponent = value;
      }
    }
  }
}
