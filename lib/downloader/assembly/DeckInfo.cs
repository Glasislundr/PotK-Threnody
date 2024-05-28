// Decompiled with JetBrains decompiler
// Type: DeckInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;

#nullable disable
public abstract class DeckInfo
{
  public BattleInfo.DeckType deckType => (BattleInfo.DeckType) this.deck_type_id;

  public abstract bool isNormal { get; }

  public abstract PlayerDeck deck { get; }

  public abstract bool isSea { get; }

  public abstract PlayerSeaDeck seaDeck { get; }

  public abstract bool isCustom { get; }

  public abstract PlayerCustomDeck customDeck { get; }

  public abstract string name { get; }

  public abstract int deck_type_id { get; }

  public abstract int member_limit { get; }

  public abstract int[] player_unit_ids { get; }

  public abstract int cost_limit { get; }

  public abstract int deck_number { get; }

  public abstract PlayerUnit[] player_units { get; }

  public abstract int total_combat { get; }

  public abstract int cost { get; }

  public abstract void resetDeck<T>(T deck) where T : class;

  public bool isNeedsRepair
  {
    get
    {
      PlayerUnit[] playerUnits = this.player_units;
      if (playerUnits == null)
        return false;
      bool isNeedsRepair = false;
      for (int index = 0; index < playerUnits.Length; ++index)
      {
        if (!(playerUnits[index] == (PlayerUnit) null))
        {
          PlayerItem equippedGear = playerUnits[index].equippedGear;
          if (equippedGear != (PlayerItem) null && equippedGear.broken)
          {
            isNeedsRepair = true;
            break;
          }
          PlayerItem equippedGear2 = playerUnits[index].equippedGear2;
          if (equippedGear2 != (PlayerItem) null && equippedGear2.broken)
          {
            isNeedsRepair = true;
            break;
          }
          PlayerItem equippedGear3 = playerUnits[index].equippedGear3;
          if (equippedGear3 != (PlayerItem) null && equippedGear3.broken)
          {
            isNeedsRepair = true;
            break;
          }
        }
      }
      return isNeedsRepair;
    }
  }
}
