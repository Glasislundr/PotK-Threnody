// Decompiled with JetBrains decompiler
// Type: SM.PlayerSeaDeck
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerSeaDeck : KeyCompare
  {
    public int member_limit;
    public int deck_type_id;
    public int?[] player_unit_ids;
    public int cost_limit;
    public int deck_number;

    public PlayerUnit[] player_units
    {
      get
      {
        Dictionary<int, PlayerUnit> dic = ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).ToDictionary<PlayerUnit, int>((Func<PlayerUnit, int>) (unit => unit.id));
        return ((IEnumerable<int?>) this.player_unit_ids).Select<int?, PlayerUnit>((Func<int?, PlayerUnit>) (id => id.HasValue ? dic[id.Value] : (PlayerUnit) null)).ToArray<PlayerUnit>();
      }
    }

    public int total_combat
    {
      get
      {
        return ((IEnumerable<PlayerUnit>) this.player_units).Where<PlayerUnit>((Func<PlayerUnit, bool>) (unit => unit != (PlayerUnit) null)).Sum<PlayerUnit>((Func<PlayerUnit, int>) (unit => Judgement.NonBattleParameter.FromPlayerUnit(unit).Combat));
      }
    }

    public int cost
    {
      get
      {
        return ((IEnumerable<PlayerUnit>) this.player_units).Where<PlayerUnit>((Func<PlayerUnit, bool>) (unit => unit != (PlayerUnit) null)).Sum<PlayerUnit>((Func<PlayerUnit, int>) (unit => unit.cost));
      }
    }

    public static PlayerDeck[] convertDeckData(PlayerSeaDeck[] decks)
    {
      List<PlayerDeck> playerDeckList = new List<PlayerDeck>();
      foreach (PlayerSeaDeck deck in decks)
        playerDeckList.Add(new PlayerDeck()
        {
          member_limit = deck.member_limit,
          deck_type_id = deck.deck_type_id,
          player_unit_ids = deck.player_unit_ids,
          cost_limit = deck.cost_limit,
          deck_number = deck.deck_number
        });
      return playerDeckList.ToArray();
    }

    public PlayerSeaDeck()
    {
    }

    public PlayerSeaDeck(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.member_limit = (int) (long) json[nameof (member_limit)];
      this.deck_type_id = (int) (long) json[nameof (deck_type_id)];
      this.player_unit_ids = ((IEnumerable<object>) json[nameof (player_unit_ids)]).Select<object, int?>((Func<object, int?>) (s =>
      {
        long? nullable = (long?) s;
        return !nullable.HasValue ? new int?() : new int?((int) nullable.GetValueOrDefault());
      })).ToArray<int?>();
      this.cost_limit = (int) (long) json[nameof (cost_limit)];
      this.deck_number = (int) (long) json[nameof (deck_number)];
    }

    public static DeckInfo createDeckInfo(PlayerSeaDeck deck)
    {
      return (DeckInfo) new PlayerSeaDeck.ImpPlayerSeaDeck(deck);
    }

    public static DeckInfo createDeckInfo(PlayerDeck deck)
    {
      return (DeckInfo) new PlayerSeaDeck.ImpPlayerSeaDeck(deck);
    }

    private class ImpPlayerSeaDeck : DeckInfo
    {
      private PlayerSeaDeck deck_;
      private PlayerUnit[] player_units_;
      private int[] player_unit_ids_;
      private int? total_combat_;
      private int? cost_;

      public ImpPlayerSeaDeck(PlayerSeaDeck d) => this.deck_ = d;

      public ImpPlayerSeaDeck(PlayerDeck d) => this.deck_ = this.convSeaDeck(d);

      private PlayerSeaDeck convSeaDeck(PlayerDeck d)
      {
        return new PlayerSeaDeck()
        {
          member_limit = d.member_limit,
          deck_type_id = d.deck_type_id,
          player_unit_ids = ((IEnumerable<int?>) d.player_unit_ids).ToArray<int?>(),
          cost_limit = d.cost_limit,
          deck_number = d.deck_number
        };
      }

      public override void resetDeck<T>(T d)
      {
        T obj = d;
        if ((object) obj != null)
        {
          switch (obj)
          {
            case PlayerSeaDeck playerSeaDeck:
              this.deck_ = playerSeaDeck;
              break;
            case PlayerDeck d1:
              this.deck_ = this.convSeaDeck(d1);
              break;
            default:
              goto label_4;
          }
          this.player_units_ = (PlayerUnit[]) null;
          this.player_unit_ids_ = (int[]) null;
          this.total_combat_ = new int?();
          this.cost_ = new int?();
          return;
        }
label_4:
        Debug.LogError((object) string.Format("failed! ImpPlayerSeaDeck.resetDeck({0});", (object) typeof (T)));
      }

      public override bool isNormal => false;

      public override PlayerDeck deck => (PlayerDeck) null;

      public override bool isSea => true;

      public override PlayerSeaDeck seaDeck => this.deck_;

      public override bool isCustom => false;

      public override PlayerCustomDeck customDeck => (PlayerCustomDeck) null;

      public override string name
      {
        get
        {
          return Consts.Format(Consts.GetInstance().UNIT_0046_MENU, (IDictionary) new Hashtable()
          {
            {
              (object) "num",
              (object) (this.deck_number + 1)
            }
          });
        }
      }

      public override int deck_type_id => this.deck_.deck_type_id;

      public override int member_limit => this.deck_.member_limit;

      public override int[] player_unit_ids
      {
        get
        {
          return this.player_unit_ids_ ?? (this.player_unit_ids_ = ((IEnumerable<int?>) this.deck_.player_unit_ids).Select<int?, int>((Func<int?, int>) (i => !i.HasValue ? 0 : i.Value)).ToArray<int>());
        }
      }

      public override int cost_limit => this.deck_.cost_limit;

      public override int deck_number => this.deck_.deck_number;

      public override PlayerUnit[] player_units
      {
        get => this.player_units_ ?? (this.player_units_ = this.deck_.player_units);
      }

      public override int total_combat
      {
        get
        {
          return !this.total_combat_.HasValue ? (this.total_combat_ = new int?(this.deck_.total_combat)).Value : this.total_combat_.Value;
        }
      }

      public override int cost
      {
        get
        {
          return !this.cost_.HasValue ? (this.cost_ = new int?(this.deck_.cost)).Value : this.cost_.Value;
        }
      }
    }
  }
}
