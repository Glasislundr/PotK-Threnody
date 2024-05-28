// Decompiled with JetBrains decompiler
// Type: SM.PlayerDeck
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
  public class PlayerDeck : KeyCompare
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

    public PlayerDeck()
    {
    }

    public PlayerDeck(Dictionary<string, object> json)
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

    public static DeckInfo createDeckInfo(PlayerDeck deck)
    {
      return (DeckInfo) new PlayerDeck.ImpPlayerDeck(deck);
    }

    public static DeckInfo createGuildRaidDeck(PlayerUnit[] units)
    {
      return (DeckInfo) new PlayerDeck.ImpGuildRaidDeck(units);
    }

    private class ImpPlayerDeck : DeckInfo
    {
      private PlayerDeck deck_;
      private PlayerUnit[] player_units_;
      private int[] player_unit_ids_;
      private int? total_combat_;
      private int? cost_;

      public ImpPlayerDeck(PlayerDeck d) => this.deck_ = d;

      public override void resetDeck<T>(T d)
      {
        if (typeof (T) != typeof (PlayerDeck))
        {
          Debug.LogError((object) string.Format("failed! ImpPlayerDeck.resetDeck({0});", (object) typeof (T)));
        }
        else
        {
          this.deck_ = (object) d as PlayerDeck;
          this.player_units_ = (PlayerUnit[]) null;
          this.player_unit_ids_ = (int[]) null;
          this.total_combat_ = new int?();
          this.cost_ = new int?();
        }
      }

      public override bool isNormal => true;

      public override PlayerDeck deck => this.deck_;

      public override bool isSea => false;

      public override PlayerSeaDeck seaDeck => (PlayerSeaDeck) null;

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

    private class ImpGuildRaidDeck : DeckInfo
    {
      private PlayerDeck deck_;
      private PlayerUnit[] player_units_;
      private int[] player_unit_ids_;
      private int total_combat_;
      private int cost_;

      public ImpGuildRaidDeck(PlayerUnit[] units) => this.resetDeck<PlayerUnit[]>(units);

      public override bool isNormal => true;

      public override PlayerDeck deck => this.deck_;

      public override bool isSea => false;

      public override PlayerSeaDeck seaDeck => (PlayerSeaDeck) null;

      public override bool isCustom => false;

      public override PlayerCustomDeck customDeck => (PlayerCustomDeck) null;

      public override string name => "レイドボス攻撃チーム";

      public override int deck_type_id => this.deck_.deck_type_id;

      public override int member_limit => this.deck_.member_limit;

      public override int[] player_unit_ids => this.player_unit_ids_;

      public override int cost_limit => this.deck_.cost_limit;

      public override int deck_number => this.deck_.deck_number;

      public override PlayerUnit[] player_units => this.player_units_;

      public override int total_combat => this.total_combat_;

      public override int cost => this.cost_;

      public override void resetDeck<T>(T units)
      {
        if (typeof (T) != typeof (PlayerUnit[]))
          Debug.LogError((object) string.Format("failed! ImpRaidDeck.resetDeck({0});", (object) typeof (T)));
        this.player_units_ = ((IEnumerable<PlayerUnit>) ((object) units as PlayerUnit[])).ToArray<PlayerUnit>();
        this.player_unit_ids_ = ((IEnumerable<PlayerUnit>) this.player_units_).Select<PlayerUnit, int>((Func<PlayerUnit, int>) (x => (object) x == null ? 0 : x.id)).ToArray<int>();
        this.total_combat_ = ((IEnumerable<PlayerUnit>) this.player_units_).Sum<PlayerUnit>((Func<PlayerUnit, int>) (x => !(x != (PlayerUnit) null) ? 0 : Judgement.NonBattleParameter.FromPlayerUnit(x).Combat));
        this.cost_ = ((IEnumerable<PlayerUnit>) this.player_units_).Sum<PlayerUnit>((Func<PlayerUnit, int>) (x => (object) x == null ? 0 : x.cost));
        this.deck_ = new PlayerDeck()
        {
          member_limit = 5,
          deck_type_id = 3,
          player_unit_ids = ((IEnumerable<int>) this.player_unit_ids_).Select<int, int?>((Func<int, int?>) (i => i == 0 ? new int?() : new int?(i))).ToArray<int?>(),
          cost_limit = Player.Current.max_cost,
          deck_number = 0
        };
      }
    }
  }
}
