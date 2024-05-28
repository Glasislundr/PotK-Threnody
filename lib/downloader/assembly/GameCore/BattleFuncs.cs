// Decompiled with JetBrains decompiler
// Type: GameCore.BattleFuncs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
namespace GameCore
{
  public static class BattleFuncs
  {
    private const int CriticalDamageRate = 3;
    private const int EDGE_MAX_COST = 10000;
    public static GameGlobalVariable<BL> environment = GameGlobalVariable<BL>.Null();
    public static BL.Unit[] ZeroArrayUnit = new BL.Unit[0];
    public static BL.ForceID[] ForceIDArrayNone = new BL.ForceID[1]
    {
      BL.ForceID.none
    };
    public static BL.ForceID[] ForceIDArrayPlayer = new BL.ForceID[1];
    public static BL.ForceID[] ForceIDArrayEnemy = new BL.ForceID[1]
    {
      BL.ForceID.enemy
    };
    public static BL.ForceID[] ForceIDArrayNeutral = new BL.ForceID[1]
    {
      BL.ForceID.neutral
    };
    public static BL.ForceID[] ForceIDArrayPlayerTarget = new BL.ForceID[1]
    {
      BL.ForceID.enemy
    };
    public static BL.ForceID[] ForceIDArrayEnemyTarget = new BL.ForceID[1];
    public static BL.ForceID[] ForceIDArrayNeutralTarget = new BL.ForceID[2]
    {
      BL.ForceID.player,
      BL.ForceID.enemy
    };
    private const int costMax = 1000000000;
    private static BattleskillEffectLogicEnum[] DeckEveryGeSkillAddEnum = new BattleskillEffectLogicEnum[17]
    {
      BattleskillEffectLogicEnum.deck_every_ge_fix_strength,
      BattleskillEffectLogicEnum.deck_every_ge_fix_vitality,
      BattleskillEffectLogicEnum.deck_every_ge_fix_intelligence,
      BattleskillEffectLogicEnum.deck_every_ge_fix_mind,
      BattleskillEffectLogicEnum.deck_every_ge_fix_agility,
      BattleskillEffectLogicEnum.deck_every_ge_fix_dexterity,
      BattleskillEffectLogicEnum.deck_every_ge_fix_luck,
      BattleskillEffectLogicEnum.deck_every_ge_fix_move,
      BattleskillEffectLogicEnum.deck_every_ge_fix_physical_attack,
      BattleskillEffectLogicEnum.deck_every_ge_fix_magic_attack,
      BattleskillEffectLogicEnum.deck_every_ge_fix_physical_defense,
      BattleskillEffectLogicEnum.deck_every_ge_fix_magic_defense,
      BattleskillEffectLogicEnum.deck_every_ge_fix_hit,
      BattleskillEffectLogicEnum.deck_every_ge_fix_evasion,
      BattleskillEffectLogicEnum.deck_every_ge_fix_critical,
      BattleskillEffectLogicEnum.deck_every_ge_fix_critical_evasion,
      BattleskillEffectLogicEnum.deck_every_ge_fix_attack_speed
    };
    private static BattleskillEffectLogicEnum[] DeckEveryGeSkillMulEnum = new BattleskillEffectLogicEnum[17]
    {
      BattleskillEffectLogicEnum.deck_every_ge_ratio_strength,
      BattleskillEffectLogicEnum.deck_every_ge_ratio_vitality,
      BattleskillEffectLogicEnum.deck_every_ge_ratio_intelligence,
      BattleskillEffectLogicEnum.deck_every_ge_ratio_mind,
      BattleskillEffectLogicEnum.deck_every_ge_ratio_agility,
      BattleskillEffectLogicEnum.deck_every_ge_ratio_dexterity,
      BattleskillEffectLogicEnum.deck_every_ge_ratio_luck,
      BattleskillEffectLogicEnum.deck_every_ge_ratio_move,
      BattleskillEffectLogicEnum.deck_every_ge_ratio_physical_attack,
      BattleskillEffectLogicEnum.deck_every_ge_ratio_magic_attack,
      BattleskillEffectLogicEnum.deck_every_ge_ratio_physical_defense,
      BattleskillEffectLogicEnum.deck_every_ge_ratio_magic_defense,
      BattleskillEffectLogicEnum.deck_every_ge_ratio_hit,
      BattleskillEffectLogicEnum.deck_every_ge_ratio_evasion,
      BattleskillEffectLogicEnum.deck_every_ge_ratio_critical,
      BattleskillEffectLogicEnum.deck_every_ge_ratio_critical_evasion,
      BattleskillEffectLogicEnum.deck_every_ge_ratio_attack_speed
    };
    private static BattleskillEffectLogicEnum[] DeckEveryLeSkillAddEnum = new BattleskillEffectLogicEnum[17]
    {
      BattleskillEffectLogicEnum.deck_every_le_fix_strength,
      BattleskillEffectLogicEnum.deck_every_le_fix_vitality,
      BattleskillEffectLogicEnum.deck_every_le_fix_intelligence,
      BattleskillEffectLogicEnum.deck_every_le_fix_mind,
      BattleskillEffectLogicEnum.deck_every_le_fix_agility,
      BattleskillEffectLogicEnum.deck_every_le_fix_dexterity,
      BattleskillEffectLogicEnum.deck_every_le_fix_luck,
      BattleskillEffectLogicEnum.deck_every_le_fix_move,
      BattleskillEffectLogicEnum.deck_every_le_fix_physical_attack,
      BattleskillEffectLogicEnum.deck_every_le_fix_magic_attack,
      BattleskillEffectLogicEnum.deck_every_le_fix_physical_defense,
      BattleskillEffectLogicEnum.deck_every_le_fix_magic_defense,
      BattleskillEffectLogicEnum.deck_every_le_fix_hit,
      BattleskillEffectLogicEnum.deck_every_le_fix_evasion,
      BattleskillEffectLogicEnum.deck_every_le_fix_critical,
      BattleskillEffectLogicEnum.deck_every_le_fix_critical_evasion,
      BattleskillEffectLogicEnum.deck_every_le_fix_attack_speed
    };
    private static BattleskillEffectLogicEnum[] DeckEveryLeSkillMulEnum = new BattleskillEffectLogicEnum[17]
    {
      BattleskillEffectLogicEnum.deck_every_le_ratio_strength,
      BattleskillEffectLogicEnum.deck_every_le_ratio_vitality,
      BattleskillEffectLogicEnum.deck_every_le_ratio_intelligence,
      BattleskillEffectLogicEnum.deck_every_le_ratio_mind,
      BattleskillEffectLogicEnum.deck_every_le_ratio_agility,
      BattleskillEffectLogicEnum.deck_every_le_ratio_dexterity,
      BattleskillEffectLogicEnum.deck_every_le_ratio_luck,
      BattleskillEffectLogicEnum.deck_every_le_ratio_move,
      BattleskillEffectLogicEnum.deck_every_le_ratio_physical_attack,
      BattleskillEffectLogicEnum.deck_every_le_ratio_magic_attack,
      BattleskillEffectLogicEnum.deck_every_le_ratio_physical_defense,
      BattleskillEffectLogicEnum.deck_every_le_ratio_magic_defense,
      BattleskillEffectLogicEnum.deck_every_le_ratio_hit,
      BattleskillEffectLogicEnum.deck_every_le_ratio_evasion,
      BattleskillEffectLogicEnum.deck_every_le_ratio_critical,
      BattleskillEffectLogicEnum.deck_every_le_ratio_critical_evasion,
      BattleskillEffectLogicEnum.deck_every_le_ratio_attack_speed
    };
    private static BattleskillEffectLogicEnum[] DeckAnotherGeSkillAddEnum = new BattleskillEffectLogicEnum[17]
    {
      BattleskillEffectLogicEnum.deck_another_ge_fix_strength,
      BattleskillEffectLogicEnum.deck_another_ge_fix_vitality,
      BattleskillEffectLogicEnum.deck_another_ge_fix_intelligence,
      BattleskillEffectLogicEnum.deck_another_ge_fix_mind,
      BattleskillEffectLogicEnum.deck_another_ge_fix_agility,
      BattleskillEffectLogicEnum.deck_another_ge_fix_dexterity,
      BattleskillEffectLogicEnum.deck_another_ge_fix_luck,
      BattleskillEffectLogicEnum.deck_another_ge_fix_move,
      BattleskillEffectLogicEnum.deck_another_ge_fix_physical_attack,
      BattleskillEffectLogicEnum.deck_another_ge_fix_magic_attack,
      BattleskillEffectLogicEnum.deck_another_ge_fix_physical_defense,
      BattleskillEffectLogicEnum.deck_another_ge_fix_magic_defense,
      BattleskillEffectLogicEnum.deck_another_ge_fix_hit,
      BattleskillEffectLogicEnum.deck_another_ge_fix_evasion,
      BattleskillEffectLogicEnum.deck_another_ge_fix_critical,
      BattleskillEffectLogicEnum.deck_another_ge_fix_critical_evasion,
      BattleskillEffectLogicEnum.deck_another_ge_fix_attack_speed
    };
    private static BattleskillEffectLogicEnum[] DeckAnotherGeSkillMulEnum = new BattleskillEffectLogicEnum[17]
    {
      BattleskillEffectLogicEnum.deck_another_ge_ratio_strength,
      BattleskillEffectLogicEnum.deck_another_ge_ratio_vitality,
      BattleskillEffectLogicEnum.deck_another_ge_ratio_intelligence,
      BattleskillEffectLogicEnum.deck_another_ge_ratio_mind,
      BattleskillEffectLogicEnum.deck_another_ge_ratio_agility,
      BattleskillEffectLogicEnum.deck_another_ge_ratio_dexterity,
      BattleskillEffectLogicEnum.deck_another_ge_ratio_luck,
      BattleskillEffectLogicEnum.deck_another_ge_ratio_move,
      BattleskillEffectLogicEnum.deck_another_ge_ratio_physical_attack,
      BattleskillEffectLogicEnum.deck_another_ge_ratio_magic_attack,
      BattleskillEffectLogicEnum.deck_another_ge_ratio_physical_defense,
      BattleskillEffectLogicEnum.deck_another_ge_ratio_magic_defense,
      BattleskillEffectLogicEnum.deck_another_ge_ratio_hit,
      BattleskillEffectLogicEnum.deck_another_ge_ratio_evasion,
      BattleskillEffectLogicEnum.deck_another_ge_ratio_critical,
      BattleskillEffectLogicEnum.deck_another_ge_ratio_critical_evasion,
      BattleskillEffectLogicEnum.deck_another_ge_ratio_attack_speed
    };
    private static BattleskillEffectLogicEnum[] DeckAnotherLeSkillAddEnum = new BattleskillEffectLogicEnum[17]
    {
      BattleskillEffectLogicEnum.deck_another_le_fix_strength,
      BattleskillEffectLogicEnum.deck_another_le_fix_vitality,
      BattleskillEffectLogicEnum.deck_another_le_fix_intelligence,
      BattleskillEffectLogicEnum.deck_another_le_fix_mind,
      BattleskillEffectLogicEnum.deck_another_le_fix_agility,
      BattleskillEffectLogicEnum.deck_another_le_fix_dexterity,
      BattleskillEffectLogicEnum.deck_another_le_fix_luck,
      BattleskillEffectLogicEnum.deck_another_le_fix_move,
      BattleskillEffectLogicEnum.deck_another_le_fix_physical_attack,
      BattleskillEffectLogicEnum.deck_another_le_fix_magic_attack,
      BattleskillEffectLogicEnum.deck_another_le_fix_physical_defense,
      BattleskillEffectLogicEnum.deck_another_le_fix_magic_defense,
      BattleskillEffectLogicEnum.deck_another_le_fix_hit,
      BattleskillEffectLogicEnum.deck_another_le_fix_evasion,
      BattleskillEffectLogicEnum.deck_another_le_fix_critical,
      BattleskillEffectLogicEnum.deck_another_le_fix_critical_evasion,
      BattleskillEffectLogicEnum.deck_another_le_fix_attack_speed
    };
    private static BattleskillEffectLogicEnum[] DeckAnotherLeSkillMulEnum = new BattleskillEffectLogicEnum[17]
    {
      BattleskillEffectLogicEnum.deck_another_le_ratio_strength,
      BattleskillEffectLogicEnum.deck_another_le_ratio_vitality,
      BattleskillEffectLogicEnum.deck_another_le_ratio_intelligence,
      BattleskillEffectLogicEnum.deck_another_le_ratio_mind,
      BattleskillEffectLogicEnum.deck_another_le_ratio_agility,
      BattleskillEffectLogicEnum.deck_another_le_ratio_dexterity,
      BattleskillEffectLogicEnum.deck_another_le_ratio_luck,
      BattleskillEffectLogicEnum.deck_another_le_ratio_move,
      BattleskillEffectLogicEnum.deck_another_le_ratio_physical_attack,
      BattleskillEffectLogicEnum.deck_another_le_ratio_magic_attack,
      BattleskillEffectLogicEnum.deck_another_le_ratio_physical_defense,
      BattleskillEffectLogicEnum.deck_another_le_ratio_magic_defense,
      BattleskillEffectLogicEnum.deck_another_le_ratio_hit,
      BattleskillEffectLogicEnum.deck_another_le_ratio_evasion,
      BattleskillEffectLogicEnum.deck_another_le_ratio_critical,
      BattleskillEffectLogicEnum.deck_another_le_ratio_critical_evasion,
      BattleskillEffectLogicEnum.deck_another_le_ratio_attack_speed
    };
    private static BattleskillEffectLogicEnum[] DeckSameGeSkillAddEnum = new BattleskillEffectLogicEnum[17]
    {
      BattleskillEffectLogicEnum.deck_same_ge_fix_strength,
      BattleskillEffectLogicEnum.deck_same_ge_fix_vitality,
      BattleskillEffectLogicEnum.deck_same_ge_fix_intelligence,
      BattleskillEffectLogicEnum.deck_same_ge_fix_mind,
      BattleskillEffectLogicEnum.deck_same_ge_fix_agility,
      BattleskillEffectLogicEnum.deck_same_ge_fix_dexterity,
      BattleskillEffectLogicEnum.deck_same_ge_fix_luck,
      BattleskillEffectLogicEnum.deck_same_ge_fix_move,
      BattleskillEffectLogicEnum.deck_same_ge_fix_physical_attack,
      BattleskillEffectLogicEnum.deck_same_ge_fix_magic_attack,
      BattleskillEffectLogicEnum.deck_same_ge_fix_physical_defense,
      BattleskillEffectLogicEnum.deck_same_ge_fix_magic_defense,
      BattleskillEffectLogicEnum.deck_same_ge_fix_hit,
      BattleskillEffectLogicEnum.deck_same_ge_fix_evasion,
      BattleskillEffectLogicEnum.deck_same_ge_fix_critical,
      BattleskillEffectLogicEnum.deck_same_ge_fix_critical_evasion,
      BattleskillEffectLogicEnum.deck_same_ge_fix_attack_speed
    };
    private static BattleskillEffectLogicEnum[] DeckSameGeSkillMulEnum = new BattleskillEffectLogicEnum[17]
    {
      BattleskillEffectLogicEnum.deck_same_ge_ratio_strength,
      BattleskillEffectLogicEnum.deck_same_ge_ratio_vitality,
      BattleskillEffectLogicEnum.deck_same_ge_ratio_intelligence,
      BattleskillEffectLogicEnum.deck_same_ge_ratio_mind,
      BattleskillEffectLogicEnum.deck_same_ge_ratio_agility,
      BattleskillEffectLogicEnum.deck_same_ge_ratio_dexterity,
      BattleskillEffectLogicEnum.deck_same_ge_ratio_luck,
      BattleskillEffectLogicEnum.deck_same_ge_ratio_move,
      BattleskillEffectLogicEnum.deck_same_ge_ratio_physical_attack,
      BattleskillEffectLogicEnum.deck_same_ge_ratio_magic_attack,
      BattleskillEffectLogicEnum.deck_same_ge_ratio_physical_defense,
      BattleskillEffectLogicEnum.deck_same_ge_ratio_magic_defense,
      BattleskillEffectLogicEnum.deck_same_ge_ratio_hit,
      BattleskillEffectLogicEnum.deck_same_ge_ratio_evasion,
      BattleskillEffectLogicEnum.deck_same_ge_ratio_critical,
      BattleskillEffectLogicEnum.deck_same_ge_ratio_critical_evasion,
      BattleskillEffectLogicEnum.deck_same_ge_ratio_attack_speed
    };
    private static BattleskillEffectLogicEnum[] DeckSameLeSkillAddEnum = new BattleskillEffectLogicEnum[17]
    {
      BattleskillEffectLogicEnum.deck_same_le_fix_strength,
      BattleskillEffectLogicEnum.deck_same_le_fix_vitality,
      BattleskillEffectLogicEnum.deck_same_le_fix_intelligence,
      BattleskillEffectLogicEnum.deck_same_le_fix_mind,
      BattleskillEffectLogicEnum.deck_same_le_fix_agility,
      BattleskillEffectLogicEnum.deck_same_le_fix_dexterity,
      BattleskillEffectLogicEnum.deck_same_le_fix_luck,
      BattleskillEffectLogicEnum.deck_same_le_fix_move,
      BattleskillEffectLogicEnum.deck_same_le_fix_physical_attack,
      BattleskillEffectLogicEnum.deck_same_le_fix_magic_attack,
      BattleskillEffectLogicEnum.deck_same_le_fix_physical_defense,
      BattleskillEffectLogicEnum.deck_same_le_fix_magic_defense,
      BattleskillEffectLogicEnum.deck_same_le_fix_hit,
      BattleskillEffectLogicEnum.deck_same_le_fix_evasion,
      BattleskillEffectLogicEnum.deck_same_le_fix_critical,
      BattleskillEffectLogicEnum.deck_same_le_fix_critical_evasion,
      BattleskillEffectLogicEnum.deck_same_le_fix_attack_speed
    };
    private static BattleskillEffectLogicEnum[] DeckSameLeSkillMulEnum = new BattleskillEffectLogicEnum[17]
    {
      BattleskillEffectLogicEnum.deck_same_le_ratio_strength,
      BattleskillEffectLogicEnum.deck_same_le_ratio_vitality,
      BattleskillEffectLogicEnum.deck_same_le_ratio_intelligence,
      BattleskillEffectLogicEnum.deck_same_le_ratio_mind,
      BattleskillEffectLogicEnum.deck_same_le_ratio_agility,
      BattleskillEffectLogicEnum.deck_same_le_ratio_dexterity,
      BattleskillEffectLogicEnum.deck_same_le_ratio_luck,
      BattleskillEffectLogicEnum.deck_same_le_ratio_move,
      BattleskillEffectLogicEnum.deck_same_le_ratio_physical_attack,
      BattleskillEffectLogicEnum.deck_same_le_ratio_magic_attack,
      BattleskillEffectLogicEnum.deck_same_le_ratio_physical_defense,
      BattleskillEffectLogicEnum.deck_same_le_ratio_magic_defense,
      BattleskillEffectLogicEnum.deck_same_le_ratio_hit,
      BattleskillEffectLogicEnum.deck_same_le_ratio_evasion,
      BattleskillEffectLogicEnum.deck_same_le_ratio_critical,
      BattleskillEffectLogicEnum.deck_same_le_ratio_critical_evasion,
      BattleskillEffectLogicEnum.deck_same_le_ratio_attack_speed
    };
    private static BattleskillEffectLogicEnum[] DeckOpponentEveryGeSkillAddEnum = new BattleskillEffectLogicEnum[17]
    {
      BattleskillEffectLogicEnum.deck_opponent_every_ge_fix_strength,
      BattleskillEffectLogicEnum.deck_opponent_every_ge_fix_vitality,
      BattleskillEffectLogicEnum.deck_opponent_every_ge_fix_intelligence,
      BattleskillEffectLogicEnum.deck_opponent_every_ge_fix_mind,
      BattleskillEffectLogicEnum.deck_opponent_every_ge_fix_agility,
      BattleskillEffectLogicEnum.deck_opponent_every_ge_fix_dexterity,
      BattleskillEffectLogicEnum.deck_opponent_every_ge_fix_luck,
      BattleskillEffectLogicEnum.deck_opponent_every_ge_fix_move,
      BattleskillEffectLogicEnum.deck_opponent_every_ge_fix_physical_attack,
      BattleskillEffectLogicEnum.deck_opponent_every_ge_fix_magic_attack,
      BattleskillEffectLogicEnum.deck_opponent_every_ge_fix_physical_defense,
      BattleskillEffectLogicEnum.deck_opponent_every_ge_fix_magic_defense,
      BattleskillEffectLogicEnum.deck_opponent_every_ge_fix_hit,
      BattleskillEffectLogicEnum.deck_opponent_every_ge_fix_evasion,
      BattleskillEffectLogicEnum.deck_opponent_every_ge_fix_critical,
      BattleskillEffectLogicEnum.deck_opponent_every_ge_fix_critical_evasion,
      BattleskillEffectLogicEnum.deck_opponent_every_ge_fix_attack_speed
    };
    private static BattleskillEffectLogicEnum[] DeckOpponentEveryGeSkillMulEnum = new BattleskillEffectLogicEnum[17]
    {
      BattleskillEffectLogicEnum.deck_opponent_every_ge_ratio_strength,
      BattleskillEffectLogicEnum.deck_opponent_every_ge_ratio_vitality,
      BattleskillEffectLogicEnum.deck_opponent_every_ge_ratio_intelligence,
      BattleskillEffectLogicEnum.deck_opponent_every_ge_ratio_mind,
      BattleskillEffectLogicEnum.deck_opponent_every_ge_ratio_agility,
      BattleskillEffectLogicEnum.deck_opponent_every_ge_ratio_dexterity,
      BattleskillEffectLogicEnum.deck_opponent_every_ge_ratio_luck,
      BattleskillEffectLogicEnum.deck_opponent_every_ge_ratio_move,
      BattleskillEffectLogicEnum.deck_opponent_every_ge_ratio_physical_attack,
      BattleskillEffectLogicEnum.deck_opponent_every_ge_ratio_magic_attack,
      BattleskillEffectLogicEnum.deck_opponent_every_ge_ratio_physical_defense,
      BattleskillEffectLogicEnum.deck_opponent_every_ge_ratio_magic_defense,
      BattleskillEffectLogicEnum.deck_opponent_every_ge_ratio_hit,
      BattleskillEffectLogicEnum.deck_opponent_every_ge_ratio_evasion,
      BattleskillEffectLogicEnum.deck_opponent_every_ge_ratio_critical,
      BattleskillEffectLogicEnum.deck_opponent_every_ge_ratio_critical_evasion,
      BattleskillEffectLogicEnum.deck_opponent_every_ge_ratio_attack_speed
    };
    private static BattleskillEffectLogicEnum[] DeckOpponentEveryLeSkillAddEnum = new BattleskillEffectLogicEnum[17]
    {
      BattleskillEffectLogicEnum.deck_opponent_every_le_fix_strength,
      BattleskillEffectLogicEnum.deck_opponent_every_le_fix_vitality,
      BattleskillEffectLogicEnum.deck_opponent_every_le_fix_intelligence,
      BattleskillEffectLogicEnum.deck_opponent_every_le_fix_mind,
      BattleskillEffectLogicEnum.deck_opponent_every_le_fix_agility,
      BattleskillEffectLogicEnum.deck_opponent_every_le_fix_dexterity,
      BattleskillEffectLogicEnum.deck_opponent_every_le_fix_luck,
      BattleskillEffectLogicEnum.deck_opponent_every_le_fix_move,
      BattleskillEffectLogicEnum.deck_opponent_every_le_fix_physical_attack,
      BattleskillEffectLogicEnum.deck_opponent_every_le_fix_magic_attack,
      BattleskillEffectLogicEnum.deck_opponent_every_le_fix_physical_defense,
      BattleskillEffectLogicEnum.deck_opponent_every_le_fix_magic_defense,
      BattleskillEffectLogicEnum.deck_opponent_every_le_fix_hit,
      BattleskillEffectLogicEnum.deck_opponent_every_le_fix_evasion,
      BattleskillEffectLogicEnum.deck_opponent_every_le_fix_critical,
      BattleskillEffectLogicEnum.deck_opponent_every_le_fix_critical_evasion,
      BattleskillEffectLogicEnum.deck_opponent_every_le_fix_attack_speed
    };
    private static BattleskillEffectLogicEnum[] DeckOpponentEveryLeSkillMulEnum = new BattleskillEffectLogicEnum[17]
    {
      BattleskillEffectLogicEnum.deck_opponent_every_le_ratio_strength,
      BattleskillEffectLogicEnum.deck_opponent_every_le_ratio_vitality,
      BattleskillEffectLogicEnum.deck_opponent_every_le_ratio_intelligence,
      BattleskillEffectLogicEnum.deck_opponent_every_le_ratio_mind,
      BattleskillEffectLogicEnum.deck_opponent_every_le_ratio_agility,
      BattleskillEffectLogicEnum.deck_opponent_every_le_ratio_dexterity,
      BattleskillEffectLogicEnum.deck_opponent_every_le_ratio_luck,
      BattleskillEffectLogicEnum.deck_opponent_every_le_ratio_move,
      BattleskillEffectLogicEnum.deck_opponent_every_le_ratio_physical_attack,
      BattleskillEffectLogicEnum.deck_opponent_every_le_ratio_magic_attack,
      BattleskillEffectLogicEnum.deck_opponent_every_le_ratio_physical_defense,
      BattleskillEffectLogicEnum.deck_opponent_every_le_ratio_magic_defense,
      BattleskillEffectLogicEnum.deck_opponent_every_le_ratio_hit,
      BattleskillEffectLogicEnum.deck_opponent_every_le_ratio_evasion,
      BattleskillEffectLogicEnum.deck_opponent_every_le_ratio_critical,
      BattleskillEffectLogicEnum.deck_opponent_every_le_ratio_critical_evasion,
      BattleskillEffectLogicEnum.deck_opponent_every_le_ratio_attack_speed
    };
    private static BattleskillEffectLogicEnum[] DeckOpponentAnotherGeSkillAddEnum = new BattleskillEffectLogicEnum[17]
    {
      BattleskillEffectLogicEnum.deck_opponent_another_ge_fix_strength,
      BattleskillEffectLogicEnum.deck_opponent_another_ge_fix_vitality,
      BattleskillEffectLogicEnum.deck_opponent_another_ge_fix_intelligence,
      BattleskillEffectLogicEnum.deck_opponent_another_ge_fix_mind,
      BattleskillEffectLogicEnum.deck_opponent_another_ge_fix_agility,
      BattleskillEffectLogicEnum.deck_opponent_another_ge_fix_dexterity,
      BattleskillEffectLogicEnum.deck_opponent_another_ge_fix_luck,
      BattleskillEffectLogicEnum.deck_opponent_another_ge_fix_move,
      BattleskillEffectLogicEnum.deck_opponent_another_ge_fix_physical_attack,
      BattleskillEffectLogicEnum.deck_opponent_another_ge_fix_magic_attack,
      BattleskillEffectLogicEnum.deck_opponent_another_ge_fix_physical_defense,
      BattleskillEffectLogicEnum.deck_opponent_another_ge_fix_magic_defense,
      BattleskillEffectLogicEnum.deck_opponent_another_ge_fix_hit,
      BattleskillEffectLogicEnum.deck_opponent_another_ge_fix_evasion,
      BattleskillEffectLogicEnum.deck_opponent_another_ge_fix_critical,
      BattleskillEffectLogicEnum.deck_opponent_another_ge_fix_critical_evasion,
      BattleskillEffectLogicEnum.deck_opponent_another_ge_fix_attack_speed
    };
    private static BattleskillEffectLogicEnum[] DeckOpponentAnotherGeSkillMulEnum = new BattleskillEffectLogicEnum[17]
    {
      BattleskillEffectLogicEnum.deck_opponent_another_ge_ratio_strength,
      BattleskillEffectLogicEnum.deck_opponent_another_ge_ratio_vitality,
      BattleskillEffectLogicEnum.deck_opponent_another_ge_ratio_intelligence,
      BattleskillEffectLogicEnum.deck_opponent_another_ge_ratio_mind,
      BattleskillEffectLogicEnum.deck_opponent_another_ge_ratio_agility,
      BattleskillEffectLogicEnum.deck_opponent_another_ge_ratio_dexterity,
      BattleskillEffectLogicEnum.deck_opponent_another_ge_ratio_luck,
      BattleskillEffectLogicEnum.deck_opponent_another_ge_ratio_move,
      BattleskillEffectLogicEnum.deck_opponent_another_ge_ratio_physical_attack,
      BattleskillEffectLogicEnum.deck_opponent_another_ge_ratio_magic_attack,
      BattleskillEffectLogicEnum.deck_opponent_another_ge_ratio_physical_defense,
      BattleskillEffectLogicEnum.deck_opponent_another_ge_ratio_magic_defense,
      BattleskillEffectLogicEnum.deck_opponent_another_ge_ratio_hit,
      BattleskillEffectLogicEnum.deck_opponent_another_ge_ratio_evasion,
      BattleskillEffectLogicEnum.deck_opponent_another_ge_ratio_critical,
      BattleskillEffectLogicEnum.deck_opponent_another_ge_ratio_critical_evasion,
      BattleskillEffectLogicEnum.deck_opponent_another_ge_ratio_attack_speed
    };
    private static BattleskillEffectLogicEnum[] DeckOpponentAnotherLeSkillAddEnum = new BattleskillEffectLogicEnum[17]
    {
      BattleskillEffectLogicEnum.deck_opponent_another_le_fix_strength,
      BattleskillEffectLogicEnum.deck_opponent_another_le_fix_vitality,
      BattleskillEffectLogicEnum.deck_opponent_another_le_fix_intelligence,
      BattleskillEffectLogicEnum.deck_opponent_another_le_fix_mind,
      BattleskillEffectLogicEnum.deck_opponent_another_le_fix_agility,
      BattleskillEffectLogicEnum.deck_opponent_another_le_fix_dexterity,
      BattleskillEffectLogicEnum.deck_opponent_another_le_fix_luck,
      BattleskillEffectLogicEnum.deck_opponent_another_le_fix_move,
      BattleskillEffectLogicEnum.deck_opponent_another_le_fix_physical_attack,
      BattleskillEffectLogicEnum.deck_opponent_another_le_fix_magic_attack,
      BattleskillEffectLogicEnum.deck_opponent_another_le_fix_physical_defense,
      BattleskillEffectLogicEnum.deck_opponent_another_le_fix_magic_defense,
      BattleskillEffectLogicEnum.deck_opponent_another_le_fix_hit,
      BattleskillEffectLogicEnum.deck_opponent_another_le_fix_evasion,
      BattleskillEffectLogicEnum.deck_opponent_another_le_fix_critical,
      BattleskillEffectLogicEnum.deck_opponent_another_le_fix_critical_evasion,
      BattleskillEffectLogicEnum.deck_opponent_another_le_fix_attack_speed
    };
    private static BattleskillEffectLogicEnum[] DeckOpponentAnotherLeSkillMulEnum = new BattleskillEffectLogicEnum[17]
    {
      BattleskillEffectLogicEnum.deck_opponent_another_le_ratio_strength,
      BattleskillEffectLogicEnum.deck_opponent_another_le_ratio_vitality,
      BattleskillEffectLogicEnum.deck_opponent_another_le_ratio_intelligence,
      BattleskillEffectLogicEnum.deck_opponent_another_le_ratio_mind,
      BattleskillEffectLogicEnum.deck_opponent_another_le_ratio_agility,
      BattleskillEffectLogicEnum.deck_opponent_another_le_ratio_dexterity,
      BattleskillEffectLogicEnum.deck_opponent_another_le_ratio_luck,
      BattleskillEffectLogicEnum.deck_opponent_another_le_ratio_move,
      BattleskillEffectLogicEnum.deck_opponent_another_le_ratio_physical_attack,
      BattleskillEffectLogicEnum.deck_opponent_another_le_ratio_magic_attack,
      BattleskillEffectLogicEnum.deck_opponent_another_le_ratio_physical_defense,
      BattleskillEffectLogicEnum.deck_opponent_another_le_ratio_magic_defense,
      BattleskillEffectLogicEnum.deck_opponent_another_le_ratio_hit,
      BattleskillEffectLogicEnum.deck_opponent_another_le_ratio_evasion,
      BattleskillEffectLogicEnum.deck_opponent_another_le_ratio_critical,
      BattleskillEffectLogicEnum.deck_opponent_another_le_ratio_critical_evasion,
      BattleskillEffectLogicEnum.deck_opponent_another_le_ratio_attack_speed
    };
    private static BattleskillEffectLogicEnum[] DeckOpponentSameGeSkillAddEnum = new BattleskillEffectLogicEnum[17]
    {
      BattleskillEffectLogicEnum.deck_opponent_same_ge_fix_strength,
      BattleskillEffectLogicEnum.deck_opponent_same_ge_fix_vitality,
      BattleskillEffectLogicEnum.deck_opponent_same_ge_fix_intelligence,
      BattleskillEffectLogicEnum.deck_opponent_same_ge_fix_mind,
      BattleskillEffectLogicEnum.deck_opponent_same_ge_fix_agility,
      BattleskillEffectLogicEnum.deck_opponent_same_ge_fix_dexterity,
      BattleskillEffectLogicEnum.deck_opponent_same_ge_fix_luck,
      BattleskillEffectLogicEnum.deck_opponent_same_ge_fix_move,
      BattleskillEffectLogicEnum.deck_opponent_same_ge_fix_physical_attack,
      BattleskillEffectLogicEnum.deck_opponent_same_ge_fix_magic_attack,
      BattleskillEffectLogicEnum.deck_opponent_same_ge_fix_physical_defense,
      BattleskillEffectLogicEnum.deck_opponent_same_ge_fix_magic_defense,
      BattleskillEffectLogicEnum.deck_opponent_same_ge_fix_hit,
      BattleskillEffectLogicEnum.deck_opponent_same_ge_fix_evasion,
      BattleskillEffectLogicEnum.deck_opponent_same_ge_fix_critical,
      BattleskillEffectLogicEnum.deck_opponent_same_ge_fix_critical_evasion,
      BattleskillEffectLogicEnum.deck_opponent_same_ge_fix_attack_speed
    };
    private static BattleskillEffectLogicEnum[] DeckOpponentSameGeSkillMulEnum = new BattleskillEffectLogicEnum[17]
    {
      BattleskillEffectLogicEnum.deck_opponent_same_ge_ratio_strength,
      BattleskillEffectLogicEnum.deck_opponent_same_ge_ratio_vitality,
      BattleskillEffectLogicEnum.deck_opponent_same_ge_ratio_intelligence,
      BattleskillEffectLogicEnum.deck_opponent_same_ge_ratio_mind,
      BattleskillEffectLogicEnum.deck_opponent_same_ge_ratio_agility,
      BattleskillEffectLogicEnum.deck_opponent_same_ge_ratio_dexterity,
      BattleskillEffectLogicEnum.deck_opponent_same_ge_ratio_luck,
      BattleskillEffectLogicEnum.deck_opponent_same_ge_ratio_move,
      BattleskillEffectLogicEnum.deck_opponent_same_ge_ratio_physical_attack,
      BattleskillEffectLogicEnum.deck_opponent_same_ge_ratio_magic_attack,
      BattleskillEffectLogicEnum.deck_opponent_same_ge_ratio_physical_defense,
      BattleskillEffectLogicEnum.deck_opponent_same_ge_ratio_magic_defense,
      BattleskillEffectLogicEnum.deck_opponent_same_ge_ratio_hit,
      BattleskillEffectLogicEnum.deck_opponent_same_ge_ratio_evasion,
      BattleskillEffectLogicEnum.deck_opponent_same_ge_ratio_critical,
      BattleskillEffectLogicEnum.deck_opponent_same_ge_ratio_critical_evasion,
      BattleskillEffectLogicEnum.deck_opponent_same_ge_ratio_attack_speed
    };
    private static BattleskillEffectLogicEnum[] DeckOpponentSameLeSkillAddEnum = new BattleskillEffectLogicEnum[17]
    {
      BattleskillEffectLogicEnum.deck_opponent_same_le_fix_strength,
      BattleskillEffectLogicEnum.deck_opponent_same_le_fix_vitality,
      BattleskillEffectLogicEnum.deck_opponent_same_le_fix_intelligence,
      BattleskillEffectLogicEnum.deck_opponent_same_le_fix_mind,
      BattleskillEffectLogicEnum.deck_opponent_same_le_fix_agility,
      BattleskillEffectLogicEnum.deck_opponent_same_le_fix_dexterity,
      BattleskillEffectLogicEnum.deck_opponent_same_le_fix_luck,
      BattleskillEffectLogicEnum.deck_opponent_same_le_fix_move,
      BattleskillEffectLogicEnum.deck_opponent_same_le_fix_physical_attack,
      BattleskillEffectLogicEnum.deck_opponent_same_le_fix_magic_attack,
      BattleskillEffectLogicEnum.deck_opponent_same_le_fix_physical_defense,
      BattleskillEffectLogicEnum.deck_opponent_same_le_fix_magic_defense,
      BattleskillEffectLogicEnum.deck_opponent_same_le_fix_hit,
      BattleskillEffectLogicEnum.deck_opponent_same_le_fix_evasion,
      BattleskillEffectLogicEnum.deck_opponent_same_le_fix_critical,
      BattleskillEffectLogicEnum.deck_opponent_same_le_fix_critical_evasion,
      BattleskillEffectLogicEnum.deck_opponent_same_le_fix_attack_speed
    };
    private static BattleskillEffectLogicEnum[] DeckOpponentSameLeSkillMulEnum = new BattleskillEffectLogicEnum[17]
    {
      BattleskillEffectLogicEnum.deck_opponent_same_le_ratio_strength,
      BattleskillEffectLogicEnum.deck_opponent_same_le_ratio_vitality,
      BattleskillEffectLogicEnum.deck_opponent_same_le_ratio_intelligence,
      BattleskillEffectLogicEnum.deck_opponent_same_le_ratio_mind,
      BattleskillEffectLogicEnum.deck_opponent_same_le_ratio_agility,
      BattleskillEffectLogicEnum.deck_opponent_same_le_ratio_dexterity,
      BattleskillEffectLogicEnum.deck_opponent_same_le_ratio_luck,
      BattleskillEffectLogicEnum.deck_opponent_same_le_ratio_move,
      BattleskillEffectLogicEnum.deck_opponent_same_le_ratio_physical_attack,
      BattleskillEffectLogicEnum.deck_opponent_same_le_ratio_magic_attack,
      BattleskillEffectLogicEnum.deck_opponent_same_le_ratio_physical_defense,
      BattleskillEffectLogicEnum.deck_opponent_same_le_ratio_magic_defense,
      BattleskillEffectLogicEnum.deck_opponent_same_le_ratio_hit,
      BattleskillEffectLogicEnum.deck_opponent_same_le_ratio_evasion,
      BattleskillEffectLogicEnum.deck_opponent_same_le_ratio_critical,
      BattleskillEffectLogicEnum.deck_opponent_same_le_ratio_critical_evasion,
      BattleskillEffectLogicEnum.deck_opponent_same_le_ratio_attack_speed
    };
    private static BattleskillEffectLogicEnum[] DamageCutFixEnum = new BattleskillEffectLogicEnum[8]
    {
      BattleskillEffectLogicEnum.damage_cut_fix_duel,
      BattleskillEffectLogicEnum.damage_cut_fix_after_duel,
      BattleskillEffectLogicEnum.damage_cut_fix_ailment,
      BattleskillEffectLogicEnum.damage_cut_fix_landform,
      BattleskillEffectLogicEnum.damage_cut_fix_command,
      BattleskillEffectLogicEnum.damage_cut_fix_release,
      (BattleskillEffectLogicEnum) 0,
      (BattleskillEffectLogicEnum) 0
    };
    private static BattleskillEffectLogicEnum[] DamageCutRatioEnum = new BattleskillEffectLogicEnum[8]
    {
      BattleskillEffectLogicEnum.damage_cut_ratio_duel,
      BattleskillEffectLogicEnum.damage_cut_ratio_after_duel,
      BattleskillEffectLogicEnum.damage_cut_ratio_ailment,
      BattleskillEffectLogicEnum.damage_cut_ratio_landform,
      BattleskillEffectLogicEnum.damage_cut_ratio_command,
      BattleskillEffectLogicEnum.damage_cut_ratio_release,
      (BattleskillEffectLogicEnum) 0,
      (BattleskillEffectLogicEnum) 0
    };
    private static BattleskillEffectLogicEnum[] DamageCut2FixEnum = new BattleskillEffectLogicEnum[8]
    {
      BattleskillEffectLogicEnum.damage_cut2_fix_duel,
      BattleskillEffectLogicEnum.damage_cut2_fix_after_duel,
      BattleskillEffectLogicEnum.damage_cut2_fix_ailment,
      BattleskillEffectLogicEnum.damage_cut2_fix_landform,
      BattleskillEffectLogicEnum.damage_cut2_fix_command,
      BattleskillEffectLogicEnum.damage_cut2_fix_release,
      (BattleskillEffectLogicEnum) 0,
      (BattleskillEffectLogicEnum) 0
    };
    private static BattleskillEffectLogicEnum[] DamageCut2RatioEnum = new BattleskillEffectLogicEnum[8]
    {
      BattleskillEffectLogicEnum.damage_cut2_ratio_duel,
      BattleskillEffectLogicEnum.damage_cut2_ratio_after_duel,
      BattleskillEffectLogicEnum.damage_cut2_ratio_ailment,
      BattleskillEffectLogicEnum.damage_cut2_ratio_landform,
      BattleskillEffectLogicEnum.damage_cut2_ratio_command,
      BattleskillEffectLogicEnum.damage_cut2_ratio_release,
      (BattleskillEffectLogicEnum) 0,
      (BattleskillEffectLogicEnum) 0
    };
    private static BattleskillEffectLogicEnum[] DamageCut4RatioEnum = new BattleskillEffectLogicEnum[8]
    {
      (BattleskillEffectLogicEnum) 0,
      BattleskillEffectLogicEnum.damage_cut4_ratio_after_duel,
      BattleskillEffectLogicEnum.damage_cut4_ratio_ailment,
      BattleskillEffectLogicEnum.damage_cut4_ratio_landform,
      BattleskillEffectLogicEnum.damage_cut4_ratio_command,
      BattleskillEffectLogicEnum.damage_cut4_ratio_release,
      BattleskillEffectLogicEnum.damage_cut4_ratio_range_effect,
      BattleskillEffectLogicEnum.damage_cut4_ratio_trap
    };
    private const float MulBaseValue = 10000f;
    private static BattleskillEffectLogicEnum[] CantInvokeDuelSkillEnum = new BattleskillEffectLogicEnum[3]
    {
      BattleskillEffectLogicEnum.cant_invoke_duel_skill_attack,
      BattleskillEffectLogicEnum.cant_invoke_duel_skill_defense,
      BattleskillEffectLogicEnum.cant_invoke_duel_skill_buff
    };
    private static BattleskillEffectLogicEnum[] charismaEffectsEnum = new BattleskillEffectLogicEnum[39]
    {
      BattleskillEffectLogicEnum.charisma_fix_strength,
      BattleskillEffectLogicEnum.charisma_fix_vitality,
      BattleskillEffectLogicEnum.charisma_fix_intelligence,
      BattleskillEffectLogicEnum.charisma_fix_mind,
      BattleskillEffectLogicEnum.charisma_fix_agility,
      BattleskillEffectLogicEnum.charisma_fix_dexterity,
      BattleskillEffectLogicEnum.charisma_fix_luck,
      BattleskillEffectLogicEnum.charisma_fix_move,
      BattleskillEffectLogicEnum.charisma_fix_physical_attack,
      BattleskillEffectLogicEnum.charisma_fix_magic_attack,
      BattleskillEffectLogicEnum.charisma_fix_physical_defense,
      BattleskillEffectLogicEnum.charisma_fix_magic_defense,
      BattleskillEffectLogicEnum.charisma_fix_hit,
      BattleskillEffectLogicEnum.charisma_fix_evasion,
      BattleskillEffectLogicEnum.charisma_fix_critical,
      BattleskillEffectLogicEnum.charisma_fix_critical_evasion,
      BattleskillEffectLogicEnum.charisma_fix_attack_speed,
      BattleskillEffectLogicEnum.charisma_ratio_strength,
      BattleskillEffectLogicEnum.charisma_ratio_vitality,
      BattleskillEffectLogicEnum.charisma_ratio_intelligence,
      BattleskillEffectLogicEnum.charisma_ratio_mind,
      BattleskillEffectLogicEnum.charisma_ratio_agility,
      BattleskillEffectLogicEnum.charisma_ratio_dexterity,
      BattleskillEffectLogicEnum.charisma_ratio_luck,
      BattleskillEffectLogicEnum.charisma_ratio_move,
      BattleskillEffectLogicEnum.charisma_ratio_physical_attack,
      BattleskillEffectLogicEnum.charisma_ratio_magic_attack,
      BattleskillEffectLogicEnum.charisma_ratio_physical_defense,
      BattleskillEffectLogicEnum.charisma_ratio_magic_defense,
      BattleskillEffectLogicEnum.charisma_ratio_hit,
      BattleskillEffectLogicEnum.charisma_ratio_evasion,
      BattleskillEffectLogicEnum.charisma_ratio_critical,
      BattleskillEffectLogicEnum.charisma_ratio_critical_evasion,
      BattleskillEffectLogicEnum.charisma_ratio_attack_speed,
      BattleskillEffectLogicEnum.charisma_clamp_hit,
      BattleskillEffectLogicEnum.charisma_damage_rate,
      BattleskillEffectLogicEnum.charisma_enemy_damage_rate,
      BattleskillEffectLogicEnum.charisma_provoke,
      BattleskillEffectLogicEnum.cant_warp_area
    };
    private static BattleskillEffectLogicEnum[] moveDistanceSkillEffectsEnum = new BattleskillEffectLogicEnum[34]
    {
      BattleskillEffectLogicEnum.move_distance_fix_strength,
      BattleskillEffectLogicEnum.move_distance_fix_vitality,
      BattleskillEffectLogicEnum.move_distance_fix_intelligence,
      BattleskillEffectLogicEnum.move_distance_fix_mind,
      BattleskillEffectLogicEnum.move_distance_fix_agility,
      BattleskillEffectLogicEnum.move_distance_fix_dexterity,
      BattleskillEffectLogicEnum.move_distance_fix_luck,
      BattleskillEffectLogicEnum.move_distance_fix_move,
      BattleskillEffectLogicEnum.move_distance_fix_physical_attack,
      BattleskillEffectLogicEnum.move_distance_fix_magic_attack,
      BattleskillEffectLogicEnum.move_distance_fix_physical_defense,
      BattleskillEffectLogicEnum.move_distance_fix_magic_defense,
      BattleskillEffectLogicEnum.move_distance_fix_hit,
      BattleskillEffectLogicEnum.move_distance_fix_evasion,
      BattleskillEffectLogicEnum.move_distance_fix_critical,
      BattleskillEffectLogicEnum.move_distance_fix_critical_evasion,
      BattleskillEffectLogicEnum.move_distance_fix_attack_speed,
      BattleskillEffectLogicEnum.move_distance_ratio_strength,
      BattleskillEffectLogicEnum.move_distance_ratio_vitality,
      BattleskillEffectLogicEnum.move_distance_ratio_intelligence,
      BattleskillEffectLogicEnum.move_distance_ratio_mind,
      BattleskillEffectLogicEnum.move_distance_ratio_agility,
      BattleskillEffectLogicEnum.move_distance_ratio_dexterity,
      BattleskillEffectLogicEnum.move_distance_ratio_luck,
      BattleskillEffectLogicEnum.move_distance_ratio_move,
      BattleskillEffectLogicEnum.move_distance_ratio_physical_attack,
      BattleskillEffectLogicEnum.move_distance_ratio_magic_attack,
      BattleskillEffectLogicEnum.move_distance_ratio_physical_defense,
      BattleskillEffectLogicEnum.move_distance_ratio_magic_defense,
      BattleskillEffectLogicEnum.move_distance_ratio_hit,
      BattleskillEffectLogicEnum.move_distance_ratio_evasion,
      BattleskillEffectLogicEnum.move_distance_ratio_critical,
      BattleskillEffectLogicEnum.move_distance_ratio_critical_evasion,
      BattleskillEffectLogicEnum.move_distance_ratio_attack_speed
    };
    private static BattleskillEffectLogicEnum[] rangeSkillEffectsEnum = new BattleskillEffectLogicEnum[34]
    {
      BattleskillEffectLogicEnum.cavalry_rush_fix_strength,
      BattleskillEffectLogicEnum.cavalry_rush_fix_vitality,
      BattleskillEffectLogicEnum.cavalry_rush_fix_intelligence,
      BattleskillEffectLogicEnum.cavalry_rush_fix_mind,
      BattleskillEffectLogicEnum.cavalry_rush_fix_agility,
      BattleskillEffectLogicEnum.cavalry_rush_fix_dexterity,
      BattleskillEffectLogicEnum.cavalry_rush_fix_luck,
      BattleskillEffectLogicEnum.cavalry_rush_fix_move,
      BattleskillEffectLogicEnum.cavalry_rush_fix_physical_attack,
      BattleskillEffectLogicEnum.cavalry_rush_fix_magic_attack,
      BattleskillEffectLogicEnum.cavalry_rush_fix_physical_defense,
      BattleskillEffectLogicEnum.cavalry_rush_fix_magic_defense,
      BattleskillEffectLogicEnum.cavalry_rush_fix_hit,
      BattleskillEffectLogicEnum.cavalry_rush_fix_evasion,
      BattleskillEffectLogicEnum.cavalry_rush_fix_critical,
      BattleskillEffectLogicEnum.cavalry_rush_fix_critical_evasion,
      BattleskillEffectLogicEnum.cavalry_rush_fix_attack_speed,
      BattleskillEffectLogicEnum.cavalry_rush_ratio_strength,
      BattleskillEffectLogicEnum.cavalry_rush_ratio_vitality,
      BattleskillEffectLogicEnum.cavalry_rush_ratio_intelligence,
      BattleskillEffectLogicEnum.cavalry_rush_ratio_mind,
      BattleskillEffectLogicEnum.cavalry_rush_ratio_agility,
      BattleskillEffectLogicEnum.cavalry_rush_ratio_dexterity,
      BattleskillEffectLogicEnum.cavalry_rush_ratio_luck,
      BattleskillEffectLogicEnum.cavalry_rush_ratio_move,
      BattleskillEffectLogicEnum.cavalry_rush_ratio_physical_attack,
      BattleskillEffectLogicEnum.cavalry_rush_ratio_magic_attack,
      BattleskillEffectLogicEnum.cavalry_rush_ratio_physical_defense,
      BattleskillEffectLogicEnum.cavalry_rush_ratio_magic_defense,
      BattleskillEffectLogicEnum.cavalry_rush_ratio_hit,
      BattleskillEffectLogicEnum.cavalry_rush_ratio_evasion,
      BattleskillEffectLogicEnum.cavalry_rush_ratio_critical,
      BattleskillEffectLogicEnum.cavalry_rush_ratio_critical_evasion,
      BattleskillEffectLogicEnum.cavalry_rush_ratio_attack_speed
    };
    private static BattleskillEffectLogicEnum[] deadCountSkillEffectsEnumPlayerForce = new BattleskillEffectLogicEnum[68]
    {
      BattleskillEffectLogicEnum.dead_count_player_fix_strength,
      BattleskillEffectLogicEnum.dead_count_player_fix_vitality,
      BattleskillEffectLogicEnum.dead_count_player_fix_intelligence,
      BattleskillEffectLogicEnum.dead_count_player_fix_mind,
      BattleskillEffectLogicEnum.dead_count_player_fix_agility,
      BattleskillEffectLogicEnum.dead_count_player_fix_dexterity,
      BattleskillEffectLogicEnum.dead_count_player_fix_luck,
      BattleskillEffectLogicEnum.dead_count_player_fix_move,
      BattleskillEffectLogicEnum.dead_count_player_fix_physical_attack,
      BattleskillEffectLogicEnum.dead_count_player_fix_magic_attack,
      BattleskillEffectLogicEnum.dead_count_player_fix_physical_defense,
      BattleskillEffectLogicEnum.dead_count_player_fix_magic_defense,
      BattleskillEffectLogicEnum.dead_count_player_fix_hit,
      BattleskillEffectLogicEnum.dead_count_player_fix_evasion,
      BattleskillEffectLogicEnum.dead_count_player_fix_critical,
      BattleskillEffectLogicEnum.dead_count_player_fix_critical_evasion,
      BattleskillEffectLogicEnum.dead_count_player_fix_attack_speed,
      BattleskillEffectLogicEnum.dead_count_player_ratio_strength,
      BattleskillEffectLogicEnum.dead_count_player_ratio_vitality,
      BattleskillEffectLogicEnum.dead_count_player_ratio_intelligence,
      BattleskillEffectLogicEnum.dead_count_player_ratio_mind,
      BattleskillEffectLogicEnum.dead_count_player_ratio_agility,
      BattleskillEffectLogicEnum.dead_count_player_ratio_dexterity,
      BattleskillEffectLogicEnum.dead_count_player_ratio_luck,
      BattleskillEffectLogicEnum.dead_count_player_ratio_move,
      BattleskillEffectLogicEnum.dead_count_player_ratio_physical_attack,
      BattleskillEffectLogicEnum.dead_count_player_ratio_magic_attack,
      BattleskillEffectLogicEnum.dead_count_player_ratio_physical_defense,
      BattleskillEffectLogicEnum.dead_count_player_ratio_magic_defense,
      BattleskillEffectLogicEnum.dead_count_player_ratio_hit,
      BattleskillEffectLogicEnum.dead_count_player_ratio_evasion,
      BattleskillEffectLogicEnum.dead_count_player_ratio_critical,
      BattleskillEffectLogicEnum.dead_count_player_ratio_critical_evasion,
      BattleskillEffectLogicEnum.dead_count_player_ratio_attack_speed,
      BattleskillEffectLogicEnum.dead_count_complex_fix_strength,
      BattleskillEffectLogicEnum.dead_count_complex_fix_vitality,
      BattleskillEffectLogicEnum.dead_count_complex_fix_intelligence,
      BattleskillEffectLogicEnum.dead_count_complex_fix_mind,
      BattleskillEffectLogicEnum.dead_count_complex_fix_agility,
      BattleskillEffectLogicEnum.dead_count_complex_fix_dexterity,
      BattleskillEffectLogicEnum.dead_count_complex_fix_luck,
      BattleskillEffectLogicEnum.dead_count_complex_fix_move,
      BattleskillEffectLogicEnum.dead_count_complex_fix_physical_attack,
      BattleskillEffectLogicEnum.dead_count_complex_fix_magic_attack,
      BattleskillEffectLogicEnum.dead_count_complex_fix_physical_defense,
      BattleskillEffectLogicEnum.dead_count_complex_fix_magic_defense,
      BattleskillEffectLogicEnum.dead_count_complex_fix_hit,
      BattleskillEffectLogicEnum.dead_count_complex_fix_evasion,
      BattleskillEffectLogicEnum.dead_count_complex_fix_critical,
      BattleskillEffectLogicEnum.dead_count_complex_fix_critical_evasion,
      BattleskillEffectLogicEnum.dead_count_complex_fix_attack_speed,
      BattleskillEffectLogicEnum.dead_count_complex_ratio_strength,
      BattleskillEffectLogicEnum.dead_count_complex_ratio_vitality,
      BattleskillEffectLogicEnum.dead_count_complex_ratio_intelligence,
      BattleskillEffectLogicEnum.dead_count_complex_ratio_mind,
      BattleskillEffectLogicEnum.dead_count_complex_ratio_agility,
      BattleskillEffectLogicEnum.dead_count_complex_ratio_dexterity,
      BattleskillEffectLogicEnum.dead_count_complex_ratio_luck,
      BattleskillEffectLogicEnum.dead_count_complex_ratio_move,
      BattleskillEffectLogicEnum.dead_count_complex_ratio_physical_attack,
      BattleskillEffectLogicEnum.dead_count_complex_ratio_magic_attack,
      BattleskillEffectLogicEnum.dead_count_complex_ratio_physical_defense,
      BattleskillEffectLogicEnum.dead_count_complex_ratio_magic_defense,
      BattleskillEffectLogicEnum.dead_count_complex_ratio_hit,
      BattleskillEffectLogicEnum.dead_count_complex_ratio_evasion,
      BattleskillEffectLogicEnum.dead_count_complex_ratio_critical,
      BattleskillEffectLogicEnum.dead_count_complex_ratio_critical_evasion,
      BattleskillEffectLogicEnum.dead_count_complex_ratio_attack_speed
    };
    private static BattleskillEffectLogicEnum[] deadCountSkillEffectsEnumEnemyForce = new BattleskillEffectLogicEnum[68]
    {
      BattleskillEffectLogicEnum.dead_count_enemy_fix_strength,
      BattleskillEffectLogicEnum.dead_count_enemy_fix_vitality,
      BattleskillEffectLogicEnum.dead_count_enemy_fix_intelligence,
      BattleskillEffectLogicEnum.dead_count_enemy_fix_mind,
      BattleskillEffectLogicEnum.dead_count_enemy_fix_agility,
      BattleskillEffectLogicEnum.dead_count_enemy_fix_dexterity,
      BattleskillEffectLogicEnum.dead_count_enemy_fix_luck,
      BattleskillEffectLogicEnum.dead_count_enemy_fix_move,
      BattleskillEffectLogicEnum.dead_count_enemy_fix_physical_attack,
      BattleskillEffectLogicEnum.dead_count_enemy_fix_magic_attack,
      BattleskillEffectLogicEnum.dead_count_enemy_fix_physical_defense,
      BattleskillEffectLogicEnum.dead_count_enemy_fix_magic_defense,
      BattleskillEffectLogicEnum.dead_count_enemy_fix_hit,
      BattleskillEffectLogicEnum.dead_count_enemy_fix_evasion,
      BattleskillEffectLogicEnum.dead_count_enemy_fix_critical,
      BattleskillEffectLogicEnum.dead_count_enemy_fix_critical_evasion,
      BattleskillEffectLogicEnum.dead_count_enemy_fix_attack_speed,
      BattleskillEffectLogicEnum.dead_count_enemy_ratio_strength,
      BattleskillEffectLogicEnum.dead_count_enemy_ratio_vitality,
      BattleskillEffectLogicEnum.dead_count_enemy_ratio_intelligence,
      BattleskillEffectLogicEnum.dead_count_enemy_ratio_mind,
      BattleskillEffectLogicEnum.dead_count_enemy_ratio_agility,
      BattleskillEffectLogicEnum.dead_count_enemy_ratio_dexterity,
      BattleskillEffectLogicEnum.dead_count_enemy_ratio_luck,
      BattleskillEffectLogicEnum.dead_count_enemy_ratio_move,
      BattleskillEffectLogicEnum.dead_count_enemy_ratio_physical_attack,
      BattleskillEffectLogicEnum.dead_count_enemy_ratio_magic_attack,
      BattleskillEffectLogicEnum.dead_count_enemy_ratio_physical_defense,
      BattleskillEffectLogicEnum.dead_count_enemy_ratio_magic_defense,
      BattleskillEffectLogicEnum.dead_count_enemy_ratio_hit,
      BattleskillEffectLogicEnum.dead_count_enemy_ratio_evasion,
      BattleskillEffectLogicEnum.dead_count_enemy_ratio_critical,
      BattleskillEffectLogicEnum.dead_count_enemy_ratio_critical_evasion,
      BattleskillEffectLogicEnum.dead_count_enemy_ratio_attack_speed,
      BattleskillEffectLogicEnum.dead_count_complex_fix_strength,
      BattleskillEffectLogicEnum.dead_count_complex_fix_vitality,
      BattleskillEffectLogicEnum.dead_count_complex_fix_intelligence,
      BattleskillEffectLogicEnum.dead_count_complex_fix_mind,
      BattleskillEffectLogicEnum.dead_count_complex_fix_agility,
      BattleskillEffectLogicEnum.dead_count_complex_fix_dexterity,
      BattleskillEffectLogicEnum.dead_count_complex_fix_luck,
      BattleskillEffectLogicEnum.dead_count_complex_fix_move,
      BattleskillEffectLogicEnum.dead_count_complex_fix_physical_attack,
      BattleskillEffectLogicEnum.dead_count_complex_fix_magic_attack,
      BattleskillEffectLogicEnum.dead_count_complex_fix_physical_defense,
      BattleskillEffectLogicEnum.dead_count_complex_fix_magic_defense,
      BattleskillEffectLogicEnum.dead_count_complex_fix_hit,
      BattleskillEffectLogicEnum.dead_count_complex_fix_evasion,
      BattleskillEffectLogicEnum.dead_count_complex_fix_critical,
      BattleskillEffectLogicEnum.dead_count_complex_fix_critical_evasion,
      BattleskillEffectLogicEnum.dead_count_complex_fix_attack_speed,
      BattleskillEffectLogicEnum.dead_count_complex_ratio_strength,
      BattleskillEffectLogicEnum.dead_count_complex_ratio_vitality,
      BattleskillEffectLogicEnum.dead_count_complex_ratio_intelligence,
      BattleskillEffectLogicEnum.dead_count_complex_ratio_mind,
      BattleskillEffectLogicEnum.dead_count_complex_ratio_agility,
      BattleskillEffectLogicEnum.dead_count_complex_ratio_dexterity,
      BattleskillEffectLogicEnum.dead_count_complex_ratio_luck,
      BattleskillEffectLogicEnum.dead_count_complex_ratio_move,
      BattleskillEffectLogicEnum.dead_count_complex_ratio_physical_attack,
      BattleskillEffectLogicEnum.dead_count_complex_ratio_magic_attack,
      BattleskillEffectLogicEnum.dead_count_complex_ratio_physical_defense,
      BattleskillEffectLogicEnum.dead_count_complex_ratio_magic_defense,
      BattleskillEffectLogicEnum.dead_count_complex_ratio_hit,
      BattleskillEffectLogicEnum.dead_count_complex_ratio_evasion,
      BattleskillEffectLogicEnum.dead_count_complex_ratio_critical,
      BattleskillEffectLogicEnum.dead_count_complex_ratio_critical_evasion,
      BattleskillEffectLogicEnum.dead_count_complex_ratio_attack_speed
    };
    private static BattleskillEffectLogicEnum[] onemanChargeEffectsEnum = new BattleskillEffectLogicEnum[34]
    {
      BattleskillEffectLogicEnum.oneman_charge_fix_strength,
      BattleskillEffectLogicEnum.oneman_charge_fix_vitality,
      BattleskillEffectLogicEnum.oneman_charge_fix_intelligence,
      BattleskillEffectLogicEnum.oneman_charge_fix_mind,
      BattleskillEffectLogicEnum.oneman_charge_fix_agility,
      BattleskillEffectLogicEnum.oneman_charge_fix_dexterity,
      BattleskillEffectLogicEnum.oneman_charge_fix_luck,
      BattleskillEffectLogicEnum.oneman_charge_fix_move,
      BattleskillEffectLogicEnum.oneman_charge_fix_physical_attack,
      BattleskillEffectLogicEnum.oneman_charge_fix_magic_attack,
      BattleskillEffectLogicEnum.oneman_charge_fix_physical_defense,
      BattleskillEffectLogicEnum.oneman_charge_fix_magic_defense,
      BattleskillEffectLogicEnum.oneman_charge_fix_hit,
      BattleskillEffectLogicEnum.oneman_charge_fix_evasion,
      BattleskillEffectLogicEnum.oneman_charge_fix_critical,
      BattleskillEffectLogicEnum.oneman_charge_fix_critical_evasion,
      BattleskillEffectLogicEnum.oneman_charge_ratio_strength,
      BattleskillEffectLogicEnum.oneman_charge_ratio_vitality,
      BattleskillEffectLogicEnum.oneman_charge_ratio_intelligence,
      BattleskillEffectLogicEnum.oneman_charge_ratio_mind,
      BattleskillEffectLogicEnum.oneman_charge_ratio_agility,
      BattleskillEffectLogicEnum.oneman_charge_ratio_dexterity,
      BattleskillEffectLogicEnum.oneman_charge_ratio_luck,
      BattleskillEffectLogicEnum.oneman_charge_ratio_move,
      BattleskillEffectLogicEnum.oneman_charge_ratio_physical_attack,
      BattleskillEffectLogicEnum.oneman_charge_ratio_magic_attack,
      BattleskillEffectLogicEnum.oneman_charge_ratio_physical_defense,
      BattleskillEffectLogicEnum.oneman_charge_ratio_magic_defense,
      BattleskillEffectLogicEnum.oneman_charge_ratio_hit,
      BattleskillEffectLogicEnum.oneman_charge_ratio_evasion,
      BattleskillEffectLogicEnum.oneman_charge_ratio_critical,
      BattleskillEffectLogicEnum.oneman_charge_ratio_critical_evasion,
      BattleskillEffectLogicEnum.oneman_charge_fix_attack_speed,
      BattleskillEffectLogicEnum.oneman_charge_ratio_attack_speed
    };
    private static BattleskillEffectLogicEnum[] targetCountEffectsEnum = new BattleskillEffectLogicEnum[34]
    {
      BattleskillEffectLogicEnum.target_count_fix_strength,
      BattleskillEffectLogicEnum.target_count_fix_vitality,
      BattleskillEffectLogicEnum.target_count_fix_intelligence,
      BattleskillEffectLogicEnum.target_count_fix_mind,
      BattleskillEffectLogicEnum.target_count_fix_agility,
      BattleskillEffectLogicEnum.target_count_fix_dexterity,
      BattleskillEffectLogicEnum.target_count_fix_luck,
      BattleskillEffectLogicEnum.target_count_fix_move,
      BattleskillEffectLogicEnum.target_count_fix_physical_attack,
      BattleskillEffectLogicEnum.target_count_fix_magic_attack,
      BattleskillEffectLogicEnum.target_count_fix_physical_defense,
      BattleskillEffectLogicEnum.target_count_fix_magic_defense,
      BattleskillEffectLogicEnum.target_count_fix_hit,
      BattleskillEffectLogicEnum.target_count_fix_evasion,
      BattleskillEffectLogicEnum.target_count_fix_critical,
      BattleskillEffectLogicEnum.target_count_fix_critical_evasion,
      BattleskillEffectLogicEnum.target_count_ratio_strength,
      BattleskillEffectLogicEnum.target_count_ratio_vitality,
      BattleskillEffectLogicEnum.target_count_ratio_intelligence,
      BattleskillEffectLogicEnum.target_count_ratio_mind,
      BattleskillEffectLogicEnum.target_count_ratio_agility,
      BattleskillEffectLogicEnum.target_count_ratio_dexterity,
      BattleskillEffectLogicEnum.target_count_ratio_luck,
      BattleskillEffectLogicEnum.target_count_ratio_move,
      BattleskillEffectLogicEnum.target_count_ratio_physical_attack,
      BattleskillEffectLogicEnum.target_count_ratio_magic_attack,
      BattleskillEffectLogicEnum.target_count_ratio_physical_defense,
      BattleskillEffectLogicEnum.target_count_ratio_magic_defense,
      BattleskillEffectLogicEnum.target_count_ratio_hit,
      BattleskillEffectLogicEnum.target_count_ratio_evasion,
      BattleskillEffectLogicEnum.target_count_ratio_critical,
      BattleskillEffectLogicEnum.target_count_ratio_critical_evasion,
      BattleskillEffectLogicEnum.target_count_fix_attack_speed,
      BattleskillEffectLogicEnum.target_count_ratio_attack_speed
    };

    public static bool useNeighbors(BL.Unit u, BL env_ = null)
    {
      if (env_ == null)
        env_ = BattleFuncs.env;
      return env_.battleInfo.pvp || env_.battleInfo.gvg || env_.battleInfo.isEarthMode || env_.playerUnits.value.Contains(u);
    }

    public static AttackStatus selectCounterAttackStatus(AttackStatus[] attacks)
    {
      return attacks.Length == 0 ? (AttackStatus) null : ((IEnumerable<AttackStatus>) attacks).Where<AttackStatus>((Func<AttackStatus, bool>) (x => x.magicBullet == null || x.magicBullet.isAttack)).OrderBy<AttackStatus, int>((Func<AttackStatus, int>) (x => x.magicBullet != null ? x.magicBullet.cost : 0)).ThenByDescending<AttackStatus, int>((Func<AttackStatus, int>) (x =>
      {
        float damageRate = x.duelParameter.DamageRate;
        x.duelParameter.DamageRate *= x.elementAttackRate * x.attackClassificationRate * x.normalDamageRate;
        int num = (int) Judgement.CalcMaximumFloatValue((Decimal) Mathf.Max(Mathf.FloorToInt(x.originalAttack), 1) * (Decimal) x.normalAttackCount);
        x.duelParameter.DamageRate = damageRate;
        return num;
      })).FirstOrDefault<AttackStatus>();
    }

    public static AttackStatus getCounterAttack(
      BL.ISkillEffectListUnit attack,
      BL.Panel attackPanel,
      BL.Unit[] attackNeighbors,
      BL.ISkillEffectListUnit defense,
      BL.Panel defensePanel,
      BL.Unit[] defenseNeighbors,
      bool isAttack,
      bool isHeal,
      int move_distance,
      int move_range,
      bool isAI = false,
      bool checkInvokeAbsoluteCounterAttack = false)
    {
      return BattleFuncs.selectCounterAttackStatus(BattleFuncs.getAttackStatusArray(defense, defensePanel, defenseNeighbors, attack, attackPanel, attackNeighbors, defense.hp, isAttack, isHeal, move_distance, move_range, isAI, checkInvokeAbsoluteCounterAttack: checkInvokeAbsoluteCounterAttack));
    }

    public static bool isCounterAttack(
      BL.ISkillEffectListUnit attack,
      BL.Panel attackPanel,
      BL.Unit[] attackNeighbors,
      BL.ISkillEffectListUnit defense,
      BL.Panel defensePanel,
      BL.Unit[] defenseNeighbors,
      int defenseHp,
      bool isAI)
    {
      return BattleFuncs.getAttackStatusArray(defense, defensePanel, defenseNeighbors, attack, attackPanel, attackNeighbors, defenseHp, false, false, isAI: isAI, makeEmptyArray: true).Length != 0;
    }

    public static AttackStatus[] getAttackStatusArray(
      BL.ISkillEffectListUnit attack,
      BL.Panel attackPanel,
      BL.Unit[] attackNeighbors,
      BL.ISkillEffectListUnit defense,
      BL.Panel defensePanel,
      BL.Unit[] defenseNeighbors,
      int attackHp,
      bool isAttack,
      bool isHeal,
      int move_distance = 0,
      int move_range = -1,
      bool isAI = false,
      bool makeEmptyArray = false,
      bool checkInvokeAbsoluteCounterAttack = false)
    {
      if (attack.IsDontAction)
        return new AttackStatus[0];
      AttackStatus[] ret;
      if (BattleFuncs.env.getAttackStatusCache(attack, attackPanel, attackNeighbors, defense, defensePanel, defenseNeighbors, attackHp, isAttack, isHeal, move_distance, move_range, isAI, out ret))
        return ret;
      int distance = BL.fieldDistance(attackPanel, defensePanel);
      BL.Unit originalUnit = attack.originalUnit;
      BL.Unit.GearRange gearRange = attack.gearRange();
      Tuple<int, int> addRange = attackPanel.getEffectsAddRange(originalUnit);
      GearKindEnum gearKindEnum = originalUnit.weapon.gear.kind.Enum;
      bool flag = false;
      bool magicWarriorFlag = originalUnit.unit.magic_warrior_flag;
      AttackStatus[] array1 = new AttackStatus[0];
      AttackStatus[] array2 = new AttackStatus[0];
      List<AttackStatus> first = new List<AttackStatus>(originalUnit.optionWeapons.Length);
      bool checkAbsolute = checkInvokeAbsoluteCounterAttack;
      while (true)
      {
        if (originalUnit.magicBullets.Length != 0)
        {
          flag = true;
          IEnumerable<BL.MagicBullet> source = ((IEnumerable<BL.MagicBullet>) originalUnit.magicBullets).Where<BL.MagicBullet>((Func<BL.MagicBullet, bool>) (x =>
          {
            if (x != null && x.isHeal == isHeal && attackHp > x.cost)
            {
              BL.Unit.MagicRange magicRange = attack.magicRange(x);
              if (checkAbsolute || NC.IsReach(magicRange.Min + addRange.Item1, magicRange.Max + addRange.Item2, distance))
                return true;
            }
            return false;
          }));
          if (!makeEmptyArray)
            array1 = source.Select<BL.MagicBullet, AttackStatus>((Func<BL.MagicBullet, AttackStatus>) (bullet => BattleFuncs.getAttackStatus(bullet, (BL.Weapon) null, attack, attackPanel, attackNeighbors, defense, defensePanel, defenseNeighbors, isAttack, isHeal, move_distance, move_range, isAI, checkInvokeAbsoluteCounterAttack))).ToArray<AttackStatus>();
          else
            Array.Resize<AttackStatus>(ref array1, source.Count<BL.MagicBullet>());
        }
        if (!isHeal && !flag | magicWarriorFlag && (!originalUnit.IsAllEquipUnit || gearKindEnum != GearKindEnum.gun && gearKindEnum != GearKindEnum.staff) && (checkAbsolute || NC.IsReach(gearRange.Min + addRange.Item1, gearRange.Max + addRange.Item2, distance)))
        {
          if (!makeEmptyArray)
            array2 = new AttackStatus[1]
            {
              BattleFuncs.getAttackStatus((BL.MagicBullet) null, (BL.Weapon) null, attack, attackPanel, attackNeighbors, defense, defensePanel, defenseNeighbors, isAttack, isHeal, move_distance, move_range, isAI, checkInvokeAbsoluteCounterAttack)
            };
          else
            Array.Resize<AttackStatus>(ref array2, 1);
        }
        if (!isHeal && originalUnit.optionWeapons.Length != 0)
        {
          for (int index = 0; index < originalUnit.optionWeapons.Length; ++index)
          {
            BL.Weapon optionWeapon = originalUnit.optionWeapons[index];
            IAttackMethod attackMethod = optionWeapon.attackMethod;
            int minRange = attackMethod.skill.min_range;
            int maxRange = attackMethod.skill.max_range;
            if (checkAbsolute || NC.IsReach(minRange + addRange.Item1, maxRange + addRange.Item2, distance))
              first.Add(BattleFuncs.getAttackStatus((BL.MagicBullet) null, optionWeapon, attack, attackPanel, attackNeighbors, defense, defensePanel, defenseNeighbors, isAttack, isHeal, move_distance, move_range, isAI, checkInvokeAbsoluteCounterAttack));
          }
        }
        ret = ((IEnumerable<AttackStatus>) array2).Concat<AttackStatus>(first.Concat<AttackStatus>((IEnumerable<AttackStatus>) array1)).ToArray<AttackStatus>();
        if (!isAttack)
        {
          if (!checkAbsolute)
          {
            if (ret.Length < 1 && ((IEnumerable<BattleskillEffect>) attack.originalUnit.absoluteCounterAttackEffects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (effect =>
            {
              BattleFuncs.PackedSkillEffect packedSkillEffect = effect.GetPackedSkillEffect();
              return BattleFuncs.checkInvokeSkillEffect(packedSkillEffect, attack, defense) && !attack.IsDontUseSkill(effect.skill.ID) && packedSkillEffect.CheckLandTag(attackPanel, isAI);
            })) && !BattleFuncs.isSkillsAndEffectsInvalid(attack, defense) && !BattleFuncs.cantInvokeDuelSkill(2, attack, defense, attackPanel, defensePanel))
              checkAbsolute = true;
            else
              goto label_27;
          }
          else
            break;
        }
        else
          goto label_27;
      }
      if (!makeEmptyArray)
      {
        foreach (AttackStatus attackStatus in ret)
          attackStatus.isAbsoluteCounterAttack = true;
      }
label_27:
      if (makeEmptyArray || ret.Length < 1)
        return ret;
      ret = !(isAttack & isHeal) ? ((IEnumerable<AttackStatus>) ret).OrderByDescending<AttackStatus, int>((Func<AttackStatus, int>) (status => status.attack)).ToArray<AttackStatus>() : ((IEnumerable<AttackStatus>) ret).OrderBy<AttackStatus, int>((Func<AttackStatus, int>) (status => status.magicBullet == null ? 0 : status.magicBullet.cost)).ToArray<AttackStatus>();
      return checkInvokeAbsoluteCounterAttack ? ret : BattleFuncs.env.setAttackStatusCache(attack, attackPanel, attackNeighbors, defense, defensePanel, defenseNeighbors, attackHp, isAttack, isHeal, move_distance, move_range, isAI, ret);
    }

    private static AttackStatus getAttackStatus(
      BL.MagicBullet magicBullet,
      BL.Weapon weapon,
      BL.ISkillEffectListUnit attack,
      BL.Panel attackPanel,
      BL.Unit[] attackNeighbors,
      BL.ISkillEffectListUnit defense,
      BL.Panel defensePanel,
      BL.Unit[] defenseNeighbors,
      bool isAttack,
      bool isHeal,
      int move_distance,
      int move_range,
      bool isAI,
      bool checkInvokeAbsoluteCounterAttack)
    {
      BL.ForceID forceId1 = BattleFuncs.env.getForceID(attack.originalUnit);
      BL.ForceID forceId2 = BattleFuncs.env.getForceID(defense.originalUnit);
      BL.ForceID[] targetForce1 = BattleFuncs.env.getTargetForce(attack.originalUnit, attack.IsCharm);
      BL.ForceID[] targetForce2 = BattleFuncs.env.getTargetForce(defense.originalUnit, defense.IsCharm);
      IEnumerable<BL.Unit> beAttackDeckUnits = (IEnumerable<BL.Unit>) BattleFuncs.env.forceUnits(forceId1).value;
      IEnumerable<BL.Unit> beAttackTargetDeckUnits = ((IEnumerable<BL.ForceID>) targetForce1).SelectMany<BL.ForceID, BL.Unit>((Func<BL.ForceID, IEnumerable<BL.Unit>>) (x => (IEnumerable<BL.Unit>) BattleFuncs.env.forceUnits(x).value));
      IEnumerable<BL.Unit> beDefenseDeckUnits;
      IEnumerable<BL.Unit> beDefenseTargetDeckUnits;
      if (forceId1 == forceId2)
      {
        beDefenseDeckUnits = beAttackDeckUnits;
        beDefenseTargetDeckUnits = beAttackTargetDeckUnits;
      }
      else
      {
        beDefenseDeckUnits = (IEnumerable<BL.Unit>) BattleFuncs.env.forceUnits(forceId2).value;
        beDefenseTargetDeckUnits = ((IEnumerable<BL.ForceID>) targetForce2).SelectMany<BL.ForceID, BL.Unit>((Func<BL.ForceID, IEnumerable<BL.Unit>>) (x => (IEnumerable<BL.Unit>) BattleFuncs.env.forceUnits(x).value));
      }
      bool flag;
      if (weapon != null)
        flag = false;
      else if (attack.originalUnit.IsAllEquipUnit)
      {
        if (magicBullet != null)
        {
          flag = true;
        }
        else
        {
          GearGear gear = attack.originalUnit.weapon.gear;
          flag = (gear.attack_type == GearAttackType.none ? (int) attack.originalUnit.playerUnit.initial_gear.attack_type : (int) gear.attack_type) == 6;
        }
      }
      else if (attack.originalUnit.unit.magic_warrior_flag)
      {
        flag = magicBullet != null;
      }
      else
      {
        GearGear gear = attack.originalUnit.weapon.gear;
        flag = (gear.attack_type == GearAttackType.none ? (int) attack.originalUnit.playerUnit.initial_gear.attack_type : (int) gear.attack_type) == 6;
      }
      Judgement.BeforeDuelParameter single = Judgement.BeforeDuelParameter.CreateSingle(attack, magicBullet, attackPanel.landform, attackNeighbors, defense, (BL.MagicBullet) null, defensePanel.landform, defenseNeighbors, isAttack, BL.fieldDistance(attackPanel, defensePanel), move_distance, move_range, attack.hp, defense.hp, targetForce1, targetForce2, attackPanel, defensePanel, beAttackDeckUnits, beDefenseDeckUnits, beAttackTargetDeckUnits, beDefenseTargetDeckUnits, isHeal, isAI, new bool?(flag), checkInvokeAbsoluteCounterAttack, weapon);
      float num = 1f;
      float attackDamageRate = attack.originalUnit.playerUnit.normalAttackDamageRate;
      AttackStatus attackStatus = new AttackStatus();
      attackStatus.duelParameter = single;
      attackStatus.isMagic = flag;
      attackStatus.magicBullet = magicBullet;
      attackStatus.weapon = weapon;
      attackStatus.attackRate = num;
      attackStatus.normalDamageRate = attackDamageRate;
      attackStatus.normalAttackCount = attack.originalUnit.playerUnit.normalAttackCount;
      attackStatus.calcElementAttackRate(attack, defense);
      attackStatus.calcAttackClassificationRate(attack, defense, attackPanel, defensePanel);
      return attackStatus;
    }

    public static void makeAttackStatusArgs(
      BL.UnitPosition attack,
      BL.UnitPosition defense,
      bool isAttack,
      bool isHeal,
      out BL.Unit[] attackNeighbors,
      out BL.Unit[] defenseNeighbors,
      out int move_distance,
      out int move_range,
      BL.Panel originalPanel = null,
      BL.Panel movePanel = null,
      bool isAI = false)
    {
      if (movePanel == null)
        movePanel = !isAttack ? BattleFuncs.env.getFieldPanel(defense) : BattleFuncs.env.getFieldPanel(attack);
      int row1;
      int column1;
      int row2;
      int column2;
      if (isAttack)
      {
        row1 = movePanel.row;
        column1 = movePanel.column;
        row2 = defense.row;
        column2 = defense.column;
      }
      else
      {
        row1 = attack.row;
        column1 = attack.column;
        row2 = movePanel.row;
        column2 = movePanel.column;
      }
      attackNeighbors = BattleFuncs.useNeighbors(attack.unit) ? BattleFuncs.getFourForceUnits(row1, column1, BattleFuncs.getForceIDArray(attack.unit, BattleFuncs.env), isAI).Select<BL.UnitPosition, BL.Unit>((Func<BL.UnitPosition, BL.Unit>) (x => x.unit)).ToArray<BL.Unit>() : BattleFuncs.ZeroArrayUnit;
      defenseNeighbors = BattleFuncs.useNeighbors(defense.unit) ? BattleFuncs.getFourForceUnits(row2, column2, BattleFuncs.getForceIDArray(defense.unit, BattleFuncs.env), isAI).Select<BL.UnitPosition, BL.Unit>((Func<BL.UnitPosition, BL.Unit>) (x => x.unit)).ToArray<BL.Unit>() : BattleFuncs.ZeroArrayUnit;
      if (isHeal)
      {
        move_distance = 0;
        move_range = -1;
      }
      else if (isAttack)
      {
        if (originalPanel == null)
          originalPanel = BattleFuncs.env.getFieldPanel(attack, true);
        move_distance = !BattleFuncs.hasEnabledMoveDistanceEffects((BL.ISkillEffectListUnit) attack.unit) ? 0 : BL.fieldDistance(originalPanel, BattleFuncs.env.getFieldPanel(defense)) - 1;
        if (BattleFuncs.hasEnabledRangeEffects((BL.ISkillEffectListUnit) attack.unit) || BattleFuncs.hasEnabledRangeEffects((BL.ISkillEffectListUnit) defense.unit))
          move_range = BL.fieldDistance(originalPanel, movePanel);
        else
          move_range = -1;
      }
      else
      {
        if (originalPanel == null)
          originalPanel = BattleFuncs.env.getFieldPanel(defense, true);
        move_distance = !BattleFuncs.hasEnabledMoveDistanceEffects((BL.ISkillEffectListUnit) defense.unit) ? 0 : BL.fieldDistance(originalPanel, BattleFuncs.env.getFieldPanel(attack)) - 1;
        if (BattleFuncs.hasEnabledRangeEffects((BL.ISkillEffectListUnit) defense.unit) || BattleFuncs.hasEnabledRangeEffects((BL.ISkillEffectListUnit) attack.unit))
          move_range = BL.fieldDistance(originalPanel, movePanel);
        else
          move_range = -1;
      }
    }

    public static AttackStatus[] getAttackStatusArray(
      BL.UnitPosition attack,
      BL.UnitPosition defense,
      bool isAttack,
      bool isHeal,
      bool isAI = false)
    {
      BL.Unit[] attackNeighbors;
      BL.Unit[] defenseNeighbors;
      int move_distance;
      int move_range;
      BattleFuncs.makeAttackStatusArgs(attack, defense, isAttack, isHeal, out attackNeighbors, out defenseNeighbors, out move_distance, out move_range, isAI: isAI);
      BL.ISkillEffectListUnit attack1 = attack is BL.ISkillEffectListUnit ? attack as BL.ISkillEffectListUnit : (BL.ISkillEffectListUnit) attack.unit;
      BL.ISkillEffectListUnit defense1 = defense is BL.ISkillEffectListUnit ? defense as BL.ISkillEffectListUnit : (BL.ISkillEffectListUnit) defense.unit;
      return BattleFuncs.getAttackStatusArray(attack1, BattleFuncs.env.getFieldPanel(attack), attackNeighbors, defense1, BattleFuncs.env.getFieldPanel(defense), defenseNeighbors, attack1.hp, isAttack, isHeal, move_distance, move_range, isAI);
    }

    public static Tuple<BL.Skill, float, bool> SimulateDuelSkill(
      BL.UnitPosition attack,
      AttackStatus attackAS,
      BL.UnitPosition defense,
      AttackStatus defenseAS,
      bool isAttacker)
    {
      BL.ISkillEffectListUnit attack1 = attack is BL.ISkillEffectListUnit ? attack as BL.ISkillEffectListUnit : (BL.ISkillEffectListUnit) attack.unit;
      BL.ISkillEffectListUnit skillEffectListUnit = defense is BL.ISkillEffectListUnit ? defense as BL.ISkillEffectListUnit : (BL.ISkillEffectListUnit) defense.unit;
      BL.Panel fieldPanel1 = BattleFuncs.env.getFieldPanel(attack);
      BL.Panel fieldPanel2 = BattleFuncs.env.getFieldPanel(defense);
      AttackStatus attackAS1 = attackAS;
      BL.Panel attackPanel = fieldPanel1;
      BL.ISkillEffectListUnit defense1 = skillEffectListUnit;
      AttackStatus defenseAS1 = defenseAS;
      BL.Panel defensePanel = fieldPanel2;
      int distance = BL.fieldDistance(fieldPanel1, fieldPanel2);
      int num = isAttacker ? 1 : 0;
      return Judgement.BeforeDuelParameter.SimulateDS(attack1, attackAS1, attackPanel, defense1, defenseAS1, defensePanel, distance, num != 0);
    }

    public static DuelResult calcDuel(
      AttackStatus attackStatus,
      BL.ISkillEffectListUnit attack,
      BL.Panel attackPanel,
      BL.ISkillEffectListUnit defense,
      BL.Panel defensePanel,
      int move_distance,
      int move_range,
      bool isAI = false)
    {
      return attackStatus.isHeal ? BattleFuncs.calcDuelHeal(attackStatus, attack, attackPanel, defense, defensePanel, isAI) : BattleFuncs.calcDuelAttack(attackStatus, attack, attackPanel, defense, defensePanel, move_distance, move_range, isAI);
    }

    public static DuelResult calcDuelHeal(
      AttackStatus attackStatus,
      BL.ISkillEffectListUnit attack,
      BL.Panel attackPanel,
      BL.ISkillEffectListUnit defense,
      BL.Panel defensePanel,
      bool isAI = false)
    {
      DuelResult duelResult = new DuelResult()
      {
        moveUnit = attack.originalUnit,
        moveUnitIsCharm = attack.IsCharm,
        isColosseum = false,
        isPlayerAttack = attack.originalUnit.isPlayerControl,
        attack = attack.originalUnit,
        defense = defense.originalUnit,
        attackAttackStatus = attackStatus,
        defenseAttackStatus = (AttackStatus) null,
        turns = new BL.DuelTurn[0],
        attackDamage = attackStatus.magicBullet.cost,
        defenseDamage = -attackStatus.healAttack(attack, defense),
        attackFromDamage = 0,
        defenseFromDamage = 0
      };
      duelResult.isDieAttack = duelResult.attackDamage >= attack.hp;
      duelResult.isDieDefense = duelResult.defenseDamage >= defense.hp;
      return duelResult;
    }

    public static DuelResult calcDuelAttack(
      AttackStatus attackStatus,
      BL.ISkillEffectListUnit attack,
      BL.Panel attackPanel,
      BL.ISkillEffectListUnit defense,
      BL.Panel defensePanel,
      int move_distance,
      int move_range,
      bool isAI = false)
    {
      DuelResult duelResult = new DuelResult();
      duelResult.isColosseum = false;
      duelResult.moveUnit = attack.originalUnit;
      duelResult.moveUnitIsCharm = attack.IsCharm;
      BL.Unit[] array1 = BattleFuncs.getFourForceUnits(attackPanel.row, attackPanel.column, BattleFuncs.getForceIDArray(attack.originalUnit, BattleFuncs.env), isAI).Select<BL.UnitPosition, BL.Unit>((Func<BL.UnitPosition, BL.Unit>) (x => x.unit)).ToArray<BL.Unit>();
      BL.Unit[] array2 = BattleFuncs.getFourForceUnits(defensePanel.row, defensePanel.column, BattleFuncs.getForceIDArray(defense.originalUnit, BattleFuncs.env), isAI).Select<BL.UnitPosition, BL.Unit>((Func<BL.UnitPosition, BL.Unit>) (x => x.unit)).ToArray<BL.Unit>();
      AttackStatus defenseAS = BattleFuncs.getCounterAttack(attack, attackPanel, array1, defense, defensePanel, array2, false, false, move_distance, move_range, isAI);
      BattleskillSkill battleskillSkill = (BattleskillSkill) null;
      if (defenseAS == null && !defense.IsDontAction && !BattleFuncs.isSkillsAndEffectsInvalid(defense, attack))
      {
        List<Tuple<BattleskillEffect, int, BattleFuncs.PackedSkillEffect>> tupleList = new List<Tuple<BattleskillEffect, int, BattleFuncs.PackedSkillEffect>>();
        foreach (Tuple<BattleskillSkill, int, int> unitAndGearSkill in defense.originalUnit.unitAndGearSkills)
        {
          bool flag = false;
          foreach (BattleskillEffect effect in unitAndGearSkill.Item1.Effects)
          {
            if (effect.EffectLogic.Enum == BattleskillEffectLogicEnum.absolute_counter_attack && (double) effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invocation) < 200.0 && effect.checkLevel(unitAndGearSkill.Item2))
            {
              BattleFuncs.PackedSkillEffect pse = BattleFuncs.PackedSkillEffect.Create(effect);
              if (BattleFuncs.checkInvokeSkillEffect(pse, defense, attack) && pse.CheckLandTag(defensePanel, isAI))
              {
                if (!flag)
                {
                  if (!defense.IsDontUseSkill(unitAndGearSkill.Item1.ID) && !BattleFuncs.cantInvokeDuelSkill(2, defense, attack, defensePanel, attackPanel))
                    flag = true;
                  else
                    break;
                }
                tupleList.Add(Tuple.Create<BattleskillEffect, int, BattleFuncs.PackedSkillEffect>(effect, unitAndGearSkill.Item2, pse));
              }
            }
          }
        }
        if (tupleList.Count > 0)
        {
          AttackStatus counterAttack = BattleFuncs.getCounterAttack(attack, attackPanel, array1, defense, defensePanel, array2, false, false, move_distance, move_range, isAI, true);
          if (counterAttack != null)
          {
            foreach (Tuple<BattleskillEffect, int, BattleFuncs.PackedSkillEffect> tuple in tupleList)
            {
              BattleFuncs.PackedSkillEffect packedSkillEffect = tuple.Item3;
              if (BattleFuncs.isInvoke(defense, attack, attackStatus.duelParameter.defenderUnitParameter, attackStatus.duelParameter.attackerUnitParameter, counterAttack, attackStatus, tuple.Item2, tuple.Item1.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invocation), BattleFuncs.env.random, true, defense.hp, attack.hp, new int?(), base_invocation: packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.base_invocation) ? packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.base_invocation) : 0.0f, invocation_skill_ratio: packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.invocation_skill_ratio) ? packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.invocation_skill_ratio) : 1f, invocation_luck_ratio: packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.invocation_luck_ratio) ? packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.invocation_luck_ratio) : 1f))
              {
                defenseAS = counterAttack;
                battleskillSkill = tuple.Item1.skill;
                break;
              }
            }
          }
        }
      }
      bool flag1 = defenseAS != null && defenseAS.isAbsoluteCounterAttack;
      Tuple<BattleskillSkill, int, int> tuple1 = (Tuple<BattleskillSkill, int, int>) null;
      if (!defense.IsDontAction && defenseAS != null && !BattleFuncs.isSkillsAndEffectsInvalid(defense, attack) && !BattleFuncs.cantInvokeDuelSkill(2, defense, attack, defensePanel, attackPanel))
      {
        tuple1 = ((IEnumerable<Tuple<BattleskillSkill, int, int>>) defense.originalUnit.unitAndGearSkills).Where<Tuple<BattleskillSkill, int, int>>((Func<Tuple<BattleskillSkill, int, int>, bool>) (x => !defense.IsDontUseSkill(x.Item1.ID) && BattleFuncs.CreatePackedSkillEffects(x.Item1, x.Item2).Any<BattleFuncs.PackedSkillEffect>((Func<BattleFuncs.PackedSkillEffect, bool>) (effect => effect.LogicEnum() == BattleskillEffectLogicEnum.ambush && effect.GetHasKeyEffect(BattleskillEffectLogicArgumentEnum.invoke_gamemode).isEnableGameMode(BattleskillInvokeGameModeEnum.quest, defense) && (!effect.HasKey(BattleskillEffectLogicArgumentEnum.gear_kind_id) || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == defense.originalUnit.unit.kind.ID) && (!effect.HasKey(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == attack.originalUnit.unit.kind.ID) && (!effect.HasKey(BattleskillEffectLogicArgumentEnum.element) || effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == defense.originalUnit.playerUnit.GetElement()) && (!effect.HasKey(BattleskillEffectLogicArgumentEnum.target_element) || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == attack.originalUnit.playerUnit.GetElement()) && (!effect.HasKey(BattleskillEffectLogicArgumentEnum.job_id) || effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == defense.originalUnit.job.ID) && (!effect.HasKey(BattleskillEffectLogicArgumentEnum.target_job_id) || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == attack.originalUnit.job.ID) && (!effect.HasKey(BattleskillEffectLogicArgumentEnum.family_id) || effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || defense.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id))) && (!effect.HasKey(BattleskillEffectLogicArgumentEnum.target_family_id) || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) == 0 || attack.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id))) && (!effect.HasKey(BattleskillEffectLogicArgumentEnum.attacker_attack_type) || effect.GetInt(BattleskillEffectLogicArgumentEnum.attacker_attack_type) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.attacker_attack_type) == 1 && !attackStatus.isMagic || effect.GetInt(BattleskillEffectLogicArgumentEnum.attacker_attack_type) == 2 && attackStatus.isMagic) && effect.CheckLandTag(defensePanel, isAI) && (Decimal) defense.hp <= ((Decimal) effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) * 100M + (Decimal) (x.Item2 * 2)) / 100.0M * (Decimal) defense.originalUnit.parameter.Hp)))).FirstOrDefault<Tuple<BattleskillSkill, int, int>>();
        if (tuple1 != null)
        {
          BL.ISkillEffectListUnit skillEffectListUnit = attack;
          attack = defense;
          defense = skillEffectListUnit;
          AttackStatus attackStatus1 = attackStatus;
          attackStatus = defenseAS;
          defenseAS = attackStatus1;
          BL.Panel panel = attackPanel;
          attackPanel = defensePanel;
          defensePanel = panel;
        }
      }
      duelResult.beforeAttakerAilmentEffectIDs = attack.skillEffects.Where(BattleskillSkillType.ailment).Select<BattleskillSkill, int>((Func<BattleskillSkill, int>) (x => x.ailment_effect.ID)).ToArray<int>();
      duelResult.beforeDefenderAilmentEffectIDs = defense.skillEffects.Where(BattleskillSkillType.ailment).Select<BattleskillSkill, int>((Func<BattleskillSkill, int>) (x => x.ailment_effect.ID)).ToArray<int>();
      duelResult.isPlayerAttack = attack.originalUnit.isPlayerControl;
      duelResult.attack = attack.originalUnit;
      duelResult.defense = defense.originalUnit;
      duelResult.attackAttackStatus = attackStatus;
      duelResult.defenseAttackStatus = defenseAS;
      duelResult.distance = BL.fieldDistance(attackPanel, defensePanel);
      BL.SkillEffectList skillEffects1 = attack.skillEffects;
      BL.SkillEffectList skillEffects2 = defense.skillEffects;
      if (PerformanceConfig.GetInstance().IsNotUseDeepCopy)
      {
        attack.setSkillEffects(attack.skillEffects.Clone());
        defense.setSkillEffects(defense.skillEffects.Clone());
      }
      else
      {
        attack.setSkillEffects(CopyUtil.DeepCopy<BL.SkillEffectList>(skillEffects1));
        defense.setSkillEffects(CopyUtil.DeepCopy<BL.SkillEffectList>(skillEffects2));
      }
      duelResult.turns = BattleFuncs.calcTurns(attack, attackStatus, attackPanel, defense, defenseAS, defensePanel, duelResult.distance, isAI, tuple1 != null);
      attack.setSkillEffects(skillEffects1);
      defense.setSkillEffects(skillEffects2);
      if (battleskillSkill != null)
      {
        BL.Skill[] skillArray1 = tuple1 == null ? duelResult.turns[0].invokeDefenderDuelSkills : duelResult.turns[0].invokeDuelSkills;
        BL.Skill[] skillArray2 = new BL.Skill[skillArray1.Length + 1];
        skillArray2[0] = new BL.Skill()
        {
          id = battleskillSkill.ID
        };
        for (int index = 0; index < skillArray1.Length; ++index)
          skillArray2[index + 1] = skillArray1[index];
        if (tuple1 == null)
          duelResult.turns[0].invokeDefenderDuelSkills = skillArray2;
        else
          duelResult.turns[0].invokeDuelSkills = skillArray2;
      }
      if (tuple1 != null)
      {
        BL.Skill[] invokeDuelSkills = duelResult.turns[0].invokeDuelSkills;
        BL.Skill[] skillArray = new BL.Skill[invokeDuelSkills.Length + 1];
        skillArray[0] = new BL.Skill()
        {
          id = tuple1.Item1.ID
        };
        for (int index = 0; index < invokeDuelSkills.Length; ++index)
          skillArray[index + 1] = invokeDuelSkills[index];
        duelResult.turns[0].invokeDuelSkills = skillArray;
      }
      foreach (BL.DuelTurn turn in duelResult.turns)
      {
        if (turn.isAtackker)
        {
          duelResult.defenseDamage += turn.damage;
          duelResult.defenseDamage -= turn.defenderDrainDamage;
          duelResult.attackDamage += turn.counterDamage;
          duelResult.attackDamage -= turn.drainDamage;
        }
        else
        {
          duelResult.attackDamage += turn.damage;
          duelResult.attackDamage -= turn.defenderDrainDamage;
          duelResult.defenseDamage += turn.counterDamage;
          duelResult.defenseDamage -= turn.drainDamage;
        }
      }
      duelResult.attackFromDamage = ((IEnumerable<BL.DuelTurn>) duelResult.turns).Sum<BL.DuelTurn>((Func<BL.DuelTurn, int>) (x => x.isAtackker ? 0 : x.damage));
      duelResult.defenseFromDamage = ((IEnumerable<BL.DuelTurn>) duelResult.turns).Sum<BL.DuelTurn>((Func<BL.DuelTurn, int>) (x => !x.isAtackker ? 0 : x.damage));
      duelResult.isDieAttack = duelResult.attackDamage >= attack.hp;
      duelResult.isDieDefense = duelResult.defenseDamage >= defense.hp;
      duelResult.distance = flag1 ? 1 : BL.fieldDistance(attackPanel, defensePanel);
      BL.Unit[] array3 = BattleFuncs.getFourForceUnits(attackPanel.row, attackPanel.column, BattleFuncs.getForceIDArray(attack.originalUnit, BattleFuncs.env), isAI).Select<BL.UnitPosition, BL.Unit>((Func<BL.UnitPosition, BL.Unit>) (x => x.unit)).ToArray<BL.Unit>();
      BattleFuncs.BeforeDuelDuelSupport beforeDuelDuelSupport1 = BattleFuncs.GetBeforeDuelDuelSupport(attack, defense, array3);
      duelResult.attackDuelSupport = beforeDuelDuelSupport1.duelSupport;
      duelResult.attackDuelSupportHitIncr = beforeDuelDuelSupport1.hitIncr;
      duelResult.attackDuelSupportEvasionIncr = beforeDuelDuelSupport1.evasionIncr;
      duelResult.attackDuelSupportCriticalIncr = beforeDuelDuelSupport1.criticalIncr;
      duelResult.attackDuelSupportCriticalEvasionIncr = beforeDuelDuelSupport1.criticalEvasionIncr;
      BL.Unit[] array4 = BattleFuncs.getFourForceUnits(defensePanel.row, defensePanel.column, BattleFuncs.getForceIDArray(defense.originalUnit, BattleFuncs.env), isAI).Select<BL.UnitPosition, BL.Unit>((Func<BL.UnitPosition, BL.Unit>) (x => x.unit)).ToArray<BL.Unit>();
      BattleFuncs.BeforeDuelDuelSupport beforeDuelDuelSupport2 = BattleFuncs.GetBeforeDuelDuelSupport(defense, attack, array4);
      duelResult.defenseDuelSupport = beforeDuelDuelSupport2.duelSupport;
      duelResult.defenseDuelSupportHitIncr = beforeDuelDuelSupport2.hitIncr;
      duelResult.defenseDuelSupportEvasionIncr = beforeDuelDuelSupport2.evasionIncr;
      duelResult.defenseDuelSupportCriticalIncr = beforeDuelDuelSupport2.criticalIncr;
      duelResult.defenseDuelSupportCriticalEvasionIncr = beforeDuelDuelSupport2.criticalEvasionIncr;
      GearKindCorrelations kindCorrelations1 = MasterData.UniqueGearKindCorrelationsBy(attack.originalUnit.unit.kind, defense.originalUnit.unit.kind);
      BL.DuelHistory duelHistory1 = new BL.DuelHistory()
      {
        inflictTotalDamage = duelResult.defenseFromDamage,
        sufferTotalDamage = duelResult.attackFromDamage,
        criticalCount = ((IEnumerable<BL.DuelTurn>) duelResult.turns).Count<BL.DuelTurn>((Func<BL.DuelTurn, bool>) (x => x.isAtackker && x.isCritical)),
        inflictMaxDamage = ((IEnumerable<BL.DuelTurn>) duelResult.turns).Max<BL.DuelTurn>((Func<BL.DuelTurn, int>) (x => !x.isAtackker ? 0 : x.damage)),
        weekElementAttackCount = ((IEnumerable<BL.DuelTurn>) duelResult.turns).Count<BL.DuelTurn>((Func<BL.DuelTurn, bool>) (x => x.isAtackker && (double) x.attackStatus.elementAttackRate > 1.0)),
        weekKindAttackCount = kindCorrelations1 == null || !kindCorrelations1.is_advantage ? 0 : ((IEnumerable<BL.DuelTurn>) duelResult.turns).Count<BL.DuelTurn>((Func<BL.DuelTurn, bool>) (x => x.isAtackker)),
        targetUnitID = defense.originalUnit.playerUnit.id
      };
      GearKindCorrelations kindCorrelations2 = MasterData.UniqueGearKindCorrelationsBy(defense.originalUnit.unit.kind, attack.originalUnit.unit.kind);
      BL.DuelHistory duelHistory2 = new BL.DuelHistory()
      {
        inflictTotalDamage = duelResult.attackFromDamage,
        sufferTotalDamage = duelResult.defenseFromDamage,
        criticalCount = ((IEnumerable<BL.DuelTurn>) duelResult.turns).Count<BL.DuelTurn>((Func<BL.DuelTurn, bool>) (x => !x.isAtackker && x.isCritical)),
        inflictMaxDamage = ((IEnumerable<BL.DuelTurn>) duelResult.turns).Max<BL.DuelTurn>((Func<BL.DuelTurn, int>) (x => x.isAtackker ? 0 : x.damage)),
        weekElementAttackCount = ((IEnumerable<BL.DuelTurn>) duelResult.turns).Count<BL.DuelTurn>((Func<BL.DuelTurn, bool>) (x => !x.isAtackker && (double) x.attackStatus.elementAttackRate > 1.0)),
        weekKindAttackCount = kindCorrelations2 == null || !kindCorrelations2.is_advantage ? 0 : ((IEnumerable<BL.DuelTurn>) duelResult.turns).Count<BL.DuelTurn>((Func<BL.DuelTurn, bool>) (x => !x.isAtackker)),
        targetUnitID = attack.originalUnit.playerUnit.id
      };
      List<BL.DuelHistory> duelHistoryList = new List<BL.DuelHistory>();
      if (attack.originalUnit.duelHistory != null)
        duelHistoryList.AddRange((IEnumerable<BL.DuelHistory>) attack.originalUnit.duelHistory);
      duelHistoryList.Add(duelHistory1);
      attack.originalUnit.duelHistory = duelHistoryList.ToArray();
      duelHistoryList.Clear();
      if (defense.originalUnit.duelHistory != null)
        duelHistoryList.AddRange((IEnumerable<BL.DuelHistory>) defense.originalUnit.duelHistory);
      duelHistoryList.Add(duelHistory2);
      defense.originalUnit.duelHistory = duelHistoryList.ToArray();
      return duelResult;
    }

    public static DuelResult calcDuel(
      AttackStatus attackStatus,
      BL.UnitPosition attack,
      BL.UnitPosition defense,
      bool isAI = false)
    {
      return BattleFuncs.calcDuel(attackStatus, attack is BL.ISkillEffectListUnit ? attack as BL.ISkillEffectListUnit : (BL.ISkillEffectListUnit) attack.unit, BattleFuncs.env.getFieldPanel(attack), defense is BL.ISkillEffectListUnit ? defense as BL.ISkillEffectListUnit : (BL.ISkillEffectListUnit) defense.unit, BattleFuncs.env.getFieldPanel(defense), BL.fieldDistance(BattleFuncs.env.getFieldPanel(attack, true), BattleFuncs.env.getFieldPanel(defense)) - 1, BL.fieldDistance(BattleFuncs.env.getFieldPanel(attack, true), BattleFuncs.env.getFieldPanel(attack)), isAI);
    }

    private static BL.DuelTurn[] calcTurns(
      BL.ISkillEffectListUnit attack,
      AttackStatus attackAS,
      BL.Panel attackPanel,
      BL.ISkillEffectListUnit defense,
      AttackStatus defenseAS,
      BL.Panel defensePanel,
      int distance,
      bool isAI,
      bool invokedAmbush)
    {
      List<BL.DuelTurn> turns = new List<BL.DuelTurn>();
      TurnHp hp = new TurnHp();
      hp.attackerHp = attack.hp;
      hp.defenderHp = defense.hp;
      hp.attackerIsDontAction = attack.IsDontAction;
      hp.defenderIsDontAction = defense.IsDontAction;
      hp.attackerIsDontEvasion = attack.IsDontEvasion;
      hp.defenderIsDontEvasion = defense.IsDontEvasion;
      hp.attackerIsDontUseSkill = attack.IsDontAction;
      hp.defenderIsDontUseSkill = defense.IsDontAction;
      hp.attackerCantOneMore = false;
      hp.defenderCantOneMore = false;
      hp.otherHp = new Dictionary<BL.ISkillEffectListUnit, TurnOtherHp>();
      if (BattleFuncs.isSkillsAndEffectsInvalid(attack, defense))
        hp.attackerIsDontUseSkill = true;
      if (BattleFuncs.isSkillsAndEffectsInvalid(defense, attack))
        hp.defenderIsDontUseSkill = true;
      int num = BattleFuncs.checkRushInvoke(attack, defense, attackAS, defenseAS, attack.hp, defense.hp, BattleFuncs.env.random, panel: attackPanel) ? 1 : 0;
      bool flag = BattleFuncs.checkRushInvoke(defense, attack, defenseAS, attackAS, defense.hp, attack.hp, BattleFuncs.env.random, panel: defensePanel);
      Action action1 = (Action) (() =>
      {
        if (hp.attackerCantOneMore || !BattleFuncs.canOneMore(attackAS.duelParameter.attackerUnitParameter, attackAS.duelParameter.defenderUnitParameter, attack, defense, true, invokedAmbush, BattleFuncs.env.random, attackAS, defenseAS, hp.attackerHp, hp.defenderHp, attackPanel: attackPanel, defensePanel: defensePanel))
          return;
        BattleFuncs.calcMultiAttack(turns, hp, true, attack, attackAS, attackPanel, defense, defenseAS, defensePanel, distance, isAI, invokedAmbush, true);
      });
      Action action2 = (Action) (() =>
      {
        if (defenseAS == null || hp.defenderCantOneMore || !BattleFuncs.canOneMore(defenseAS.duelParameter.attackerUnitParameter, defenseAS.duelParameter.defenderUnitParameter, defense, attack, false, invokedAmbush, BattleFuncs.env.random, defenseAS, attackAS, hp.defenderHp, hp.attackerHp, attackPanel: defensePanel, defensePanel: attackPanel))
          return;
        BattleFuncs.calcMultiAttack(turns, hp, false, defense, defenseAS, defensePanel, attack, attackAS, attackPanel, distance, isAI, invokedAmbush, true);
      });
      BattleFuncs.calcMultiAttack(turns, hp, true, attack, attackAS, attackPanel, defense, defenseAS, defensePanel, distance, isAI, invokedAmbush, false);
      if (num != 0)
      {
        action1();
        action1 = (Action) null;
      }
      BattleFuncs.calcMultiAttack(turns, hp, false, defense, defenseAS, defensePanel, attack, attackAS, attackPanel, distance, isAI, invokedAmbush, false);
      if (flag)
      {
        action2();
        action2 = (Action) null;
      }
      if (action1 != null)
        action1();
      if (action2 != null)
        action2();
      return turns.ToArray();
    }

    private static void calcMultiAttack(
      List<BL.DuelTurn> turns,
      TurnHp hp,
      bool isAttacker,
      BL.ISkillEffectListUnit attack,
      AttackStatus attackStatus,
      BL.Panel attackPanel,
      BL.ISkillEffectListUnit defense,
      AttackStatus defenseStatus,
      BL.Panel defensePanel,
      int distance,
      bool isAI,
      bool invokedAmbush,
      bool isOneMoreAttack)
    {
      if (attackStatus == null || hp.isDieAttackerOrDefender() || (isAttacker ? (hp.attackerIsDontAction ? 1 : 0) : (hp.defenderIsDontAction ? 1 : 0)) != 0)
        return;
      List<BL.DuelTurn> duelTurnList = new List<BL.DuelTurn>();
      int num = BattleFuncs.attackCount(attack, defense);
      int normalAttackCount = attack.originalUnit.playerUnit.normalAttackCount;
      int attackedCount = 0;
      bool isInvalidAttackDuelSkill = false;
      for (; attackedCount < num; ++attackedCount)
        BattleFuncs.calcSingleAttack(turns.Concat<BL.DuelTurn>((IEnumerable<BL.DuelTurn>) duelTurnList).ToList<BL.DuelTurn>(), duelTurnList, hp, isAttacker, attack, attackStatus, attackPanel, defense, defenseStatus, defensePanel, distance, BattleFuncs.env.random, isAI, new int?(), invokedAmbush, false, defense.hp, isOneMoreAttack, attackedCount, isInvalidAttackDuelSkill, false);
      bool flag1 = BattleFuncs.IsInvokeDuelSkill(duelTurnList);
      if (normalAttackCount >= 2 && duelTurnList.Count > 0 && !(hp.isDieAttackerOrDefender() & flag1))
      {
        if (hp.isDieAttackerOrDefender())
          isInvalidAttackDuelSkill = true;
        for (int index = num * normalAttackCount; attackedCount < index; ++attackedCount)
          BattleFuncs.calcSingleAttack(turns.Concat<BL.DuelTurn>((IEnumerable<BL.DuelTurn>) duelTurnList).ToList<BL.DuelTurn>(), duelTurnList, hp, isAttacker, attack, attackStatus, attackPanel, defense, defenseStatus, defensePanel, distance, BattleFuncs.env.random, isAI, new int?(), invokedAmbush, false, defense.hp, isOneMoreAttack, attackedCount, isInvalidAttackDuelSkill, true);
      }
      bool flag2 = BattleFuncs.IsInvokeDuelSkill(duelTurnList);
      if (duelTurnList.Count <= 0)
        return;
      int count = duelTurnList.Count;
      foreach (BL.DuelTurn duelTurn in duelTurnList)
      {
        if (flag2)
        {
          duelTurn.attackCount = 1;
          duelTurn.isDualSingleAttack = true;
        }
        else
          duelTurn.attackCount = count;
        turns.Add(duelTurn);
        --count;
      }
    }

    private static bool IsInvokeDuelSkill(List<BL.DuelTurn> turns)
    {
      foreach (BL.DuelTurn turn in turns)
      {
        if (((IEnumerable<BL.Skill>) turn.invokeDuelSkills).Any<BL.Skill>((Func<BL.Skill, bool>) (x =>
        {
          BattleskillGenre? genre1 = x.genre1;
          BattleskillGenre battleskillGenre = BattleskillGenre.attack;
          return genre1.GetValueOrDefault() == battleskillGenre & genre1.HasValue;
        })))
          return true;
      }
      return false;
    }

    private static BattleFuncs.AttackDamageData calcAttackDamage(
      XorShift random,
      bool isAttacker,
      TurnHp hp,
      BL.ISkillEffectListUnit attack,
      AttackStatus attackStatus,
      BL.Panel attackPanel,
      BL.ISkillEffectListUnit defense,
      AttackStatus defenseStatus,
      BL.Panel defensePanel,
      int distance,
      bool isAI,
      int? colosseumTurn,
      BattleDuelSkill baseDuelSkill,
      bool invokedAmbush,
      bool invokedPrayer,
      float? invokeRate,
      float? criticalRate,
      int defenderDuelBeginHp,
      bool isOneMoreAttack,
      bool isBiattack,
      int biAttackNo = 0,
      bool isInvalidAttackDuelSkill = false)
    {
      bool isColosseum = colosseumTurn.HasValue;
      BattleFuncs.AttackDamageData ret = new BattleFuncs.AttackDamageData();
      int? nullable1 = new int?();
      float? nullable2 = new float?();
      float? nullable3 = new float?();
      float? nullable4 = new float?();
      List<BattleFuncs.InvalidSpecificSkillLogic> invalidSkillLogics = BattleFuncs.GetInvalidSkillsAndLogics((IEnumerable<BattleskillEffect>) baseDuelSkill.invokeAttackerSkillEffects);
      if (!colosseumTurn.HasValue)
      {
        foreach (BL.SkillEffect skillEffect in BattleFuncs.gearSkillEffectFilter(attack.originalUnit, attack.skillEffects.Where(BattleskillEffectLogicEnum.invalid_specific_skills_and_logics, (Func<BL.SkillEffect, bool>) (x =>
        {
          BattleskillEffect effect = x.effect;
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack_nc);
          bool flag = invokedAmbush ? !isAttacker : isAttacker;
          if (num == 1 && !flag || num == 2 & flag)
            return false;
          BattleFuncs.PackedSkillEffect pse = BattleFuncs.PackedSkillEffect.Create(x);
          return !BattleFuncs.isSealedSkillEffect(attack, x) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.range) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.range) == distance) && BattleFuncs.checkInvokeSkillEffect(pse, attack, defense, colosseumTurn, unitHp: new int?(isAttacker ? hp.attackerHp : hp.defenderHp), targetHp: new int?(isAttacker ? hp.defenderHp : hp.attackerHp)) && pse.CheckLandTag(attackPanel, isAI) && !BattleFuncs.isEffectEnemyRangeAndInvalid(x, attack, defense) && !BattleFuncs.isSkillsAndEffectsInvalid(attack, defense);
        }))))
          invalidSkillLogics.Add(BattleFuncs.InvalidSpecificSkillLogic.Create(skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.invalid_skill_id), skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.invalid_logic_id), (object) skillEffect));
      }
      float num1 = baseDuelSkill.damageRate;
      if (baseDuelSkill.biAttackDamageRate != null)
        num1 *= baseDuelSkill.biAttackDamageRate[biAttackNo];
      float attackRate1 = baseDuelSkill.attackRate;
      float damageValue = baseDuelSkill.damageValue;
      int? fixDamage1 = baseDuelSkill.FixDamage;
      float drainRate = baseDuelSkill.drainRate;
      float downPhysicalRate = baseDuelSkill.defenseDownPhysicalRate;
      float defenseDownMagicRate = baseDuelSkill.defenseDownMagicRate;
      float? fixHit = baseDuelSkill.FixHit;
      int fixHitPriority = baseDuelSkill.FixHitPriority;
      float? fixCritical = baseDuelSkill.FixCritical;
      bool isChageAttackType = baseDuelSkill.isChageAttackType;
      float? percentageDamageRate = baseDuelSkill.PercentageDamageRate;
      int percentageDamageMax = baseDuelSkill.PercentageDamageMax;
      float drainRateRatio = baseDuelSkill.drainRateRatio;
      float physicalRateRatio = baseDuelSkill.defenseDownPhysicalRateRatio;
      float downMagicRateRatio = baseDuelSkill.defenseDownMagicRateRatio;
      if (isBiattack)
      {
        Func<BL.SkillEffect, float> getValue = (Func<BL.SkillEffect, float>) (effect =>
        {
          float num2 = 1f;
          for (int index = biAttackNo >= 5 ? 5 : biAttackNo + 1; index >= 1; --index)
          {
            BattleskillEffectLogicArgumentEnum logicArgumentEnum = BattleskillEffectLogic.GetLogicArgumentEnum("percentage_damage_s" + (object) index);
            if (logicArgumentEnum != BattleskillEffectLogicArgumentEnum.none)
            {
              float num3 = effect.effect.GetFloat(logicArgumentEnum);
              if ((double) num3 != 0.0)
              {
                num2 = num3;
                break;
              }
            }
          }
          return num2;
        });
        foreach (BL.SkillEffect skillEffect in BattleFuncs.gearSkillEffectFilter(defense.originalUnit, defense.skillEffects.Where(BattleskillEffectLogicEnum.resist_suisei, (Func<BL.SkillEffect, bool>) (x =>
        {
          BattleskillEffect effect = x.effect;
          BattleFuncs.PackedSkillEffect pse = BattleFuncs.PackedSkillEffect.Create(x);
          pse.SetIgnoreHeader(true);
          if (BattleFuncs.isSealedSkillEffect(defense, x) || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != defense.originalUnit.unit.kind.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != attack.originalUnit.unit.kind.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != defense.originalUnit.playerUnit.GetElement() || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != attack.originalUnit.playerUnit.GetElement() || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) != attack.originalUnit.job.ID || !BattleFuncs.checkInvokeSkillEffect(pse, defense, attack, colosseumTurn, unitHp: new int?(isAttacker ? hp.defenderHp : hp.attackerHp), targetHp: new int?(isAttacker ? hp.attackerHp : hp.defenderHp)) || !pse.CheckLandTag(defensePanel, isAI) || BattleFuncs.isEffectEnemyRangeAndInvalid(x, defense, attack) || BattleFuncs.isSkillsAndEffectsInvalid(defense, attack))
            return false;
          Func<BattleFuncs.InvalidSpecificSkillLogic, bool> funcExtraCheck = (Func<BattleFuncs.InvalidSpecificSkillLogic, bool>) (issl =>
          {
            BattleskillEffect battleskillEffect = (BattleskillEffect) null;
            if (issl.param is BL.SkillEffect)
              battleskillEffect = (issl.param as BL.SkillEffect).effect;
            else if (issl.param is BattleskillEffect)
              battleskillEffect = issl.param as BattleskillEffect;
            if (battleskillEffect == null)
              return true;
            int num6 = battleskillEffect.HasKey(BattleskillEffectLogicArgumentEnum.condition) ? battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.condition) : 0;
            if (num6 == 0)
              return true;
            float num7 = getValue(x);
            if (num6 == 1 && (double) num7 > 1.0)
              return true;
            return num6 == 2 && (double) num7 < 1.0;
          });
          return !BattleFuncs.checkInvalidEffect(x, invalidSkillLogics, funcExtraCheck);
        }))))
          num1 = (float) ((Decimal) num1 * (Decimal) getValue(skillEffect));
      }
      ret.invokeSkillExtraInfo = new List<string>();
      ret.damageShareUnit = new List<BL.ISkillEffectListUnit>();
      ret.damageShareDamage = new List<int>();
      ret.damageShareSkillEffect = new List<BL.UseSkillEffect>();
      ret.attackerUseSkillEffects = new List<BL.UseSkillEffect>();
      ret.defenderUseSkillEffects = new List<BL.UseSkillEffect>();
      ret.duelSkillProc = BattleDuelSkill.invokeDuelSkills(attack, attackStatus, attackPanel, defense, defenseStatus, defensePanel, distance, isAttacker ? hp.attackerHp : hp.defenderHp, isAttacker ? hp.defenderHp : hp.attackerHp, isAttacker ? hp.attackerIsDontUseSkill : hp.defenderIsDontUseSkill, isAttacker ? hp.defenderIsDontUseSkill : hp.attackerIsDontUseSkill, random, isAI, colosseumTurn, isBiattack, isAttacker, invokedAmbush, invokedPrayer, baseDuelSkill, invokeRate, defenderDuelBeginHp, isOneMoreAttack, isInvalidAttackDuelSkill, invalidSkillLogics, hp);
      float? nullable5 = new float?();
      int maxDamage1 = 0;
      float? nullable6 = ret.duelSkillProc.PercentageDamageRate;
      if (nullable6.HasValue)
      {
        nullable5 = ret.duelSkillProc.PercentageDamageRate;
        maxDamage1 = ret.duelSkillProc.PercentageDamageMax;
      }
      else if (percentageDamageRate.HasValue)
      {
        nullable5 = percentageDamageRate;
        maxDamage1 = percentageDamageMax;
      }
      BL.MagicBullet magicBullet = attackStatus.magicBullet;
      if (magicBullet != null && !nullable5.HasValue)
      {
        BattleskillEffect percentageDamage = magicBullet.percentageDamage;
        if (percentageDamage != null)
        {
          nullable5 = new float?(percentageDamage.GetFloat(BattleskillEffectLogicArgumentEnum.percentage));
          maxDamage1 = percentageDamage.HasKey(BattleskillEffectLogicArgumentEnum.max_value) ? percentageDamage.GetInt(BattleskillEffectLogicArgumentEnum.max_value) : 0;
        }
      }
      ret.hp = hp;
      nullable6 = ret.duelSkillProc.FixHit;
      float? nullable7;
      if (nullable6.HasValue || fixHit.HasValue)
      {
        int? hitMin = attackStatus.duelParameter.HitMin;
        float? nullable8;
        if (!hitMin.HasValue)
        {
          nullable6 = new float?();
          nullable8 = nullable6;
        }
        else
          nullable8 = new float?((float) hitMin.GetValueOrDefault() / 100f);
        float? nullable9 = nullable8;
        int? hitMax = attackStatus.duelParameter.HitMax;
        float? nullable10;
        if (!hitMax.HasValue)
        {
          nullable6 = new float?();
          nullable10 = nullable6;
        }
        else
          nullable10 = new float?((float) hitMax.GetValueOrDefault() / 100f);
        float? nullable11 = nullable10;
        nullable6 = ret.duelSkillProc.FixHit;
        float? nullable12 = nullable6.HasValue ? ret.duelSkillProc.FixHit : fixHit;
        if (fixHitPriority == 0 && nullable11.HasValue)
        {
          nullable6 = nullable12;
          float? nullable13 = nullable11;
          if ((double) nullable6.GetValueOrDefault() > (double) nullable13.GetValueOrDefault() & nullable6.HasValue & nullable13.HasValue)
            nullable12 = nullable11;
        }
        if (nullable9.HasValue)
        {
          nullable7 = nullable12;
          nullable6 = nullable9;
          if ((double) nullable7.GetValueOrDefault() < (double) nullable6.GetValueOrDefault() & nullable7.HasValue & nullable6.HasValue)
            nullable12 = nullable9;
        }
        ref BattleFuncs.AttackDamageData local = ref ret;
        nullable6 = nullable12;
        float num8 = random.NextFloat();
        int num9 = (double) nullable6.GetValueOrDefault() >= (double) num8 & nullable6.HasValue ? 1 : 0;
        local.isHit = num9 != 0;
      }
      else
        ret.isHit = attackStatus.calcHit(random.NextFloat());
      if (defense.IsDontEvasion || (isAttacker ? (hp.defenderIsDontEvasion ? 1 : 0) : (hp.attackerIsDontEvasion ? 1 : 0)) != 0)
        ret.isHit = true;
      float? nullable14 = criticalRate;
      if (fixCritical.HasValue)
      {
        if (nullable14.HasValue)
        {
          nullable6 = fixCritical;
          nullable7 = nullable14;
          if (!((double) nullable6.GetValueOrDefault() > (double) nullable7.GetValueOrDefault() & nullable6.HasValue & nullable7.HasValue))
            goto label_46;
        }
        nullable14 = fixCritical;
      }
label_46:
      if (attackStatus.criticalDisplay() > 0)
      {
        if (nullable14.HasValue)
        {
          double critical = (double) attackStatus.critical;
          nullable7 = nullable14;
          double valueOrDefault = (double) nullable7.GetValueOrDefault();
          if (!(critical > valueOrDefault & nullable7.HasValue))
            goto label_50;
        }
        nullable14 = new float?(attackStatus.critical);
      }
label_50:
      if (nullable14.HasValue)
      {
        int? criticalMin = attackStatus.duelParameter.CriticalMin;
        float? nullable15;
        if (!criticalMin.HasValue)
        {
          nullable7 = new float?();
          nullable15 = nullable7;
        }
        else
          nullable15 = new float?((float) criticalMin.GetValueOrDefault() / 100f);
        float? nullable16 = nullable15;
        int? criticalMax = attackStatus.duelParameter.CriticalMax;
        float? nullable17;
        if (!criticalMax.HasValue)
        {
          nullable7 = new float?();
          nullable17 = nullable7;
        }
        else
          nullable17 = new float?((float) criticalMax.GetValueOrDefault() / 100f);
        float? nullable18 = nullable17;
        if (nullable18.HasValue)
        {
          nullable7 = nullable14;
          nullable6 = nullable18;
          if ((double) nullable7.GetValueOrDefault() > (double) nullable6.GetValueOrDefault() & nullable7.HasValue & nullable6.HasValue)
            nullable14 = nullable18;
        }
        if (nullable16.HasValue)
        {
          nullable6 = nullable14;
          nullable7 = nullable16;
          if ((double) nullable6.GetValueOrDefault() < (double) nullable7.GetValueOrDefault() & nullable6.HasValue & nullable7.HasValue)
            nullable14 = nullable16;
        }
      }
      ref BattleFuncs.AttackDamageData local1 = ref ret;
      int num10;
      if (ret.isHit && nullable14.HasValue)
      {
        nullable7 = nullable14;
        float num11 = random.NextFloat();
        num10 = (double) nullable7.GetValueOrDefault() >= (double) num11 & nullable7.HasValue ? 1 : 0;
      }
      else
        num10 = 0;
      local1.isCritical = num10 != 0;
      ret.damage = 0;
      ret.dispDamage = 0;
      ret.realDamage = 0;
      ret.dispDrainDamage = 0;
      ret.drainDamage = 0;
      ret.counterDamage = 0;
      ret.defenderDispDrainDamage = 0;
      ret.defenderDrainDamage = 0;
      ret.attackerSwapHealDamage = 0;
      ret.defenderSwapHealDamage = 0;
      ret.duelSkillProc.invokeDefenderSkill(ret.isCritical);
      if (ret.duelSkillProc.isSuppressCritical || baseDuelSkill.isSuppressCritical)
        ret.isCritical = false;
      if (ret.duelSkillProc.attackerCantOneMore || baseDuelSkill.attackerCantOneMore)
      {
        if (isAttacker)
          hp.attackerCantOneMore = true;
        else
          hp.defenderCantOneMore = true;
      }
      if (biAttackNo == 0)
      {
        int hp1 = attack.originalUnit.parameter.Hp;
        ret.counterDamage += Mathf.CeilToInt((float) hp1 * baseDuelSkill.counterDamageHpPercentage) + Mathf.CeilToInt((float) hp1 * ret.duelSkillProc.counterDamageHpPercentage);
        ret.counterDamage += ret.duelSkillProc.counterDamageValue + baseDuelSkill.counterDamageValue;
      }
      Action action1 = (Action) (() =>
      {
        if (isAttacker)
        {
          ret.counterDamage = NC.Clamp(0, hp.attackerHp, ret.counterDamage);
          hp.attackerHp -= ret.counterDamage;
        }
        else
        {
          ret.counterDamage = NC.Clamp(0, hp.defenderHp, ret.counterDamage);
          hp.defenderHp -= ret.counterDamage;
        }
      });
      int attackerHp = hp.attackerHp;
      int defenderHp = hp.defenderHp;
      if (ret.isHit)
      {
        int physicalDefense = attackStatus.duelParameter.defenderUnitParameter.PhysicalDefense;
        int magicDefense = attackStatus.duelParameter.defenderUnitParameter.MagicDefense;
        float num12 = ret.duelSkillProc.defenseDownPhysicalRate * downPhysicalRate;
        float num13 = ret.duelSkillProc.defenseDownMagicRate * defenseDownMagicRate;
        float num14 = 1f - (1f - num12) * (ret.duelSkillProc.defenseDownPhysicalRateRatio * physicalRateRatio);
        float num15 = 1f - (1f - num13) * (ret.duelSkillProc.defenseDownMagicRateRatio * downMagicRateRatio);
        attackStatus.duelParameter.defenderUnitParameter.PhysicalDefense = (int) Mathf.Ceil((float) physicalDefense * num14);
        attackStatus.duelParameter.defenderUnitParameter.MagicDefense = (int) Mathf.Ceil((float) magicDefense * num15);
        float num16 = ret.duelSkillProc.damageRate * num1;
        List<BattleFuncs.SkillParam> skillParams1 = new List<BattleFuncs.SkillParam>();
        Action<BattleskillEffectLogicEnum> action2 = (Action<BattleskillEffectLogicEnum>) (logic =>
        {
          foreach (BL.SkillEffect effect in attack.skillEffects.Where(logic, (Func<BL.SkillEffect, bool>) (x =>
          {
            int num18 = x.effect.GetInt(BattleskillEffectLogicArgumentEnum.attack_phase);
            bool flag = invokedAmbush ? !isAttacker : isAttacker;
            switch (num18)
            {
              case 0:
                if (x.GetCheckInvokeGeneric().DoCheck(attack, defense, colosseumTurn, unitHp: new int?(isAttacker ? hp.attackerHp : hp.defenderHp), targetHp: new int?(isAttacker ? hp.defenderHp : hp.attackerHp), attackType: flag ? 1 : 2, unitPanel: attackPanel, targetPanel: defensePanel, effect: x) && !BattleFuncs.isEffectEnemyRangeAndInvalid(x, attack, defense))
                  return !BattleFuncs.isSkillsAndEffectsInvalid(attack, defense);
                break;
              case 1:
                if (!isOneMoreAttack)
                  goto case 0;
                else
                  goto default;
              default:
                if (!(num18 == 2 & isOneMoreAttack))
                  break;
                goto case 0;
            }
            return false;
          })))
            skillParams1.Add(BattleFuncs.SkillParam.Create(attack.originalUnit, effect));
        });
        action2(BattleskillEffectLogicEnum.damage_rate_one_more_attack);
        action2(BattleskillEffectLogicEnum.damage_rate_one_more_attack2);
        foreach (IGrouping<int, BattleFuncs.SkillParam> grouping in BattleFuncs.gearSkillParamFilter(skillParams1).GroupBy<BattleFuncs.SkillParam, int>((Func<BattleFuncs.SkillParam, int>) (x => x.effect.effectId)))
        {
          Decimal num19 = 1.0M;
          bool flag = true;
          Decimal? nullable19 = new Decimal?();
          foreach (BattleFuncs.SkillParam skillParam in (IEnumerable<BattleFuncs.SkillParam>) grouping)
          {
            BL.SkillEffect effect = skillParam.effect;
            BL.ISkillEffectListUnit invoke = attack;
            BL.ISkillEffectListUnit target = defense;
            Judgement.BeforeDuelUnitParameter attackerUnitParameter = attackStatus.duelParameter.attackerUnitParameter;
            Judgement.BeforeDuelUnitParameter defenderUnitParameter = attackStatus.duelParameter.defenderUnitParameter;
            AttackStatus invokeAS = attackStatus;
            AttackStatus targetAS = defenseStatus;
            int baseSkillLevel = effect.baseSkillLevel;
            double percentage_invocation = (double) effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invocation);
            XorShift random1 = random;
            int invokeHp = isAttacker ? hp.attackerHp : hp.defenderHp;
            int targetHp = isAttacker ? hp.defenderHp : hp.attackerHp;
            int? colosseumTurn1 = colosseumTurn;
            nullable7 = new float?();
            float? invokeRate1 = nullable7;
            if (BattleFuncs.isInvoke(invoke, target, attackerUnitParameter, defenderUnitParameter, invokeAS, targetAS, baseSkillLevel, (float) percentage_invocation, random1, false, invokeHp, targetHp, colosseumTurn1, invokeRate1))
            {
              Decimal num20 = (Decimal) effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.damage_percentage) + (Decimal) effect.baseSkillLevel * (Decimal) effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio);
              if (num20 < 0M)
                num20 = 0M;
              num19 *= num20;
              if (flag)
              {
                flag = false;
              }
              else
              {
                if (!nullable19.HasValue)
                {
                  BattleFuncs.PackedSkillEffect packedSkillEffect = BattleFuncs.PackedSkillEffect.Create(effect);
                  nullable19 = new Decimal?(!packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.max_percentage) ? Decimal.MaxValue : (Decimal) packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.max_percentage));
                }
                Decimal num21 = num19;
                Decimal? nullable20 = nullable19;
                Decimal valueOrDefault = nullable20.GetValueOrDefault();
                if (num21 > valueOrDefault & nullable20.HasValue)
                {
                  num19 = nullable19.Value;
                  break;
                }
              }
            }
          }
          num16 *= (float) num19;
        }
        if ((baseDuelSkill.attackerSkills.Length >= 1 ? 1 : (ret.duelSkillProc.attackerSkills.Length >= 1 ? 1 : 0)) != 0)
        {
          List<BattleFuncs.SkillParam> skillParams2 = new List<BattleFuncs.SkillParam>();
          Action<BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, int> action3 = (Action<BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, int>) ((effectUnit, targetUnit, effect_target) =>
          {
            if (BattleFuncs.isSkillsAndEffectsInvalid(effectUnit, targetUnit))
              return;
            foreach (BL.SkillEffect effect in effectUnit.enabledSkillEffect(BattleskillEffectLogicEnum.duel_skill_damage_fluctuate_ratio).Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x =>
            {
              BL.Panel panel = effect_target == 0 ? attackPanel : defensePanel;
              BattleskillEffect effect = x.effect;
              return effect.GetInt(BattleskillEffectLogicArgumentEnum.effect_target) == effect_target && (!effect.HasKey(BattleskillEffectLogicArgumentEnum.skill_group_id) || effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id) == 0 || effectUnit.originalUnit.unit.HasSkillGroupId(effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id))) && (!effect.HasKey(BattleskillEffectLogicArgumentEnum.exclude_skill_group_id) || effect.GetInt(BattleskillEffectLogicArgumentEnum.exclude_skill_group_id) == 0 || !effectUnit.originalUnit.unit.HasSkillGroupId(effect.GetInt(BattleskillEffectLogicArgumentEnum.exclude_skill_group_id))) && effect.GetPackedSkillEffect().CheckLandTag(panel, isAI);
            })))
            {
              if (!isBiattack)
                skillParams2.Add(BattleFuncs.SkillParam.CreateMul(effectUnit.originalUnit, effect, (float) ((Decimal) effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.damage_rate) + (Decimal) effect.baseSkillLevel * (Decimal) effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio))));
              else
                skillParams2.Add(BattleFuncs.SkillParam.CreateMul(effectUnit.originalUnit, effect, (float) ((Decimal) effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.damage_rate_suisei) + (Decimal) effect.baseSkillLevel * (Decimal) effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio_suisei))));
            }
          });
          action3(attack, defense, 0);
          action3(defense, attack, 1);
          num16 *= BattleFuncs.calcSkillParamMul(skillParams2);
        }
        if (ret.duelSkillProc.isInvokeCounterAttack)
          num16 *= ret.duelSkillProc.counterDamageRate;
        int? fixDamage2 = attackStatus.duelParameter.FixDamage;
        int? nullable21 = ret.duelSkillProc.FixDamage;
        if (nullable21.HasValue)
          attackStatus.duelParameter.FixDamage = ret.duelSkillProc.FixDamage;
        else if (fixDamage1.HasValue)
          attackStatus.duelParameter.FixDamage = fixDamage1;
        if (nullable5.HasValue)
        {
          int preHp = isAttacker ? hp.defenderHp : hp.attackerHp;
          attackStatus.duelParameter.FixDamage = new int?(BattleFuncs.calcPercentageDamage(preHp, nullable5.Value, maxDamage1));
        }
        int physicalAttack = attackStatus.duelParameter.attackerUnitParameter.PhysicalAttack;
        attackStatus.duelParameter.attackerUnitParameter.PhysicalAttack += (int) ((Decimal) ret.duelSkillProc.damageValue + (Decimal) damageValue);
        int magicAttack = attackStatus.duelParameter.attackerUnitParameter.MagicAttack;
        attackStatus.duelParameter.attackerUnitParameter.MagicAttack += (int) ((Decimal) ret.duelSkillProc.damageValue + (Decimal) damageValue);
        bool isMagic = attackStatus.isMagic;
        attackStatus.isMagic = ret.duelSkillProc.isChageAttackType | isChageAttackType ? !isMagic : isMagic;
        float attackRate2 = attackStatus.attackRate;
        attackStatus.attackRate = ret.duelSkillProc.attackRate * attackRate1;
        float damageRate = attackStatus.duelParameter.DamageRate;
        attackStatus.duelParameter.DamageRate *= BattleFuncs.calcAttackClassificationRate(attackStatus, attack, defense, attackPanel, defensePanel);
        attackStatus.duelParameter.DamageRate *= attack.originalUnit.playerUnit.normalAttackDamageRate;
        Decimal num22 = 1.0M;
        if (ret.isCritical)
        {
          bool flag1 = false;
          for (int effectTarget = 0; effectTarget <= 1; effectTarget++)
          {
            Judgement.BeforeDuelUnitParameter invokeParameter;
            Judgement.BeforeDuelUnitParameter targetParameter;
            AttackStatus invokeAS;
            AttackStatus targetAS;
            BL.ISkillEffectListUnit effectUnit;
            BL.ISkillEffectListUnit targetUnit;
            int effectHp;
            int targetHp;
            BL.Panel effectPanel;
            if (effectTarget == 0)
            {
              effectUnit = attack;
              targetUnit = defense;
              invokeParameter = attackStatus.duelParameter.attackerUnitParameter;
              targetParameter = attackStatus.duelParameter.defenderUnitParameter;
              invokeAS = attackStatus;
              targetAS = defenseStatus;
              effectHp = isAttacker ? hp.attackerHp : hp.defenderHp;
              targetHp = isAttacker ? hp.defenderHp : hp.attackerHp;
              effectPanel = attackPanel;
            }
            else
            {
              effectUnit = defense;
              targetUnit = attack;
              invokeParameter = attackStatus.duelParameter.defenderUnitParameter;
              targetParameter = attackStatus.duelParameter.attackerUnitParameter;
              invokeAS = defenseStatus;
              targetAS = attackStatus;
              effectHp = isAttacker ? hp.defenderHp : hp.attackerHp;
              targetHp = isAttacker ? hp.attackerHp : hp.defenderHp;
              effectPanel = defensePanel;
            }
            List<BattleFuncs.SkillParam> skillParams = new List<BattleFuncs.SkillParam>();
            foreach (BL.SkillEffect effect in effectUnit.skillEffects.Where(BattleskillEffectLogicEnum.damage_rate_critical, (Func<BL.SkillEffect, bool>) (x =>
            {
              BattleFuncs.PackedSkillEffect pse = BattleFuncs.PackedSkillEffect.Create(x);
              int num23 = pse.HasKey(BattleskillEffectLogicArgumentEnum.is_attack) ? pse.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) : 0;
              bool flag2 = effectTarget != 0 ? (invokedAmbush ? isAttacker : !isAttacker) : (invokedAmbush ? !isAttacker : isAttacker);
              return (num23 != 1 || flag2) && !(num23 == 2 & flag2) && x.effect.GetInt(BattleskillEffectLogicArgumentEnum.effect_target) == effectTarget && !BattleFuncs.isSealedSkillEffect(effectUnit, x) && BattleFuncs.checkInvokeSkillEffect(pse, effectUnit, targetUnit, colosseumTurn, unitHp: new int?(effectHp), targetHp: new int?(targetHp)) && pse.CheckLandTag(effectPanel, isAI) && (effectTarget == 0 || !BattleFuncs.isSkillsAndEffectsInvalid(defense, attack)) && !BattleFuncs.isEffectEnemyRangeAndInvalid(x, attack, defense) && !BattleFuncs.isSkillsAndEffectsInvalid(attack, defense);
            })))
              skillParams.Add(BattleFuncs.SkillParam.Create(effectUnit.originalUnit, effect));
            foreach (IGrouping<int, BattleFuncs.SkillParam> grouping in BattleFuncs.gearSkillParamFilter(skillParams).GroupBy<BattleFuncs.SkillParam, int>((Func<BattleFuncs.SkillParam, int>) (x => x.effect.effectId)))
            {
              Decimal num24 = 1.0M;
              bool flag3 = true;
              Decimal? nullable22 = new Decimal?();
              foreach (BattleFuncs.SkillParam skillParam in (IEnumerable<BattleFuncs.SkillParam>) grouping)
              {
                BL.SkillEffect effect = skillParam.effect;
                if (BattleFuncs.isInvoke(effectUnit, targetUnit, invokeParameter, targetParameter, invokeAS, targetAS, effect.baseSkillLevel, effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invocation), random, false, effectHp, targetHp, colosseumTurn, base_invocation: effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.base_invocation), invocation_skill_ratio: effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.invocation_skill_ratio), invocation_luck_ratio: effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.invocation_luck_ratio)))
                {
                  Decimal num25 = (Decimal) effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.damage_percentage) + (Decimal) effect.baseSkillLevel * (Decimal) effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio);
                  if (num25 < 0M)
                    num25 = 0M;
                  num24 *= num25;
                  if (flag3)
                  {
                    flag1 = true;
                    flag3 = false;
                  }
                  else
                  {
                    if (!nullable22.HasValue)
                    {
                      float num26 = effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.max_percentage);
                      nullable22 = new Decimal?((double) num26 == 0.0 ? Decimal.MaxValue : (Decimal) num26);
                    }
                    Decimal num27 = num24;
                    Decimal? nullable23 = nullable22;
                    Decimal valueOrDefault = nullable23.GetValueOrDefault();
                    if (num27 > valueOrDefault & nullable23.HasValue)
                    {
                      num24 = nullable22.Value;
                      break;
                    }
                  }
                }
              }
              num22 *= num24;
            }
          }
          if (!flag1)
            num22 = 3M;
        }
        float num28 = Judgement.CalcMaximumFloatValue(((Decimal) Mathf.Max(1f, attackStatus.originalAttackExcludeBaseDamage) + (Decimal) ret.duelSkillProc.additionalDamage + (Decimal) baseDuelSkill.additionalDamage) * (Decimal) num16 * num22);
        if ((double) num28 < 1.0)
          num28 = 1f;
        float damage = Judgement.CalcMaximumFloatValue((Decimal) num28 + (Decimal) attackStatus.duelParameter.BaseDamage);
        attackStatus.duelParameter.attackerUnitParameter.PhysicalAttack = physicalAttack;
        attackStatus.duelParameter.attackerUnitParameter.MagicAttack = magicAttack;
        attackStatus.duelParameter.FixDamage = fixDamage2;
        attackStatus.duelParameter.defenderUnitParameter.PhysicalDefense = physicalDefense;
        attackStatus.duelParameter.defenderUnitParameter.MagicDefense = magicDefense;
        attackStatus.isMagic = isMagic;
        attackStatus.attackRate = attackRate2;
        attackStatus.duelParameter.DamageRate = damageRate;
        if ((double) damage < 1.0)
          damage = 1f;
        if (ret.duelSkillProc.isAbsoluteDefense || baseDuelSkill.isAbsoluteDefense)
        {
          ret.invokeSkillExtraInfo.Add("absolute_defense");
          damage = 0.0f;
        }
        else
        {
          IEnumerable<BL.SkillEffect> skillEffects = defense.skillEffects.Where(BattleskillEffectLogicEnum.absolute_defense, (Func<BL.SkillEffect, bool>) (x =>
          {
            BattleskillEffect effect = x.effect;
            BattleFuncs.PackedSkillEffect pse = BattleFuncs.PackedSkillEffect.Create(x);
            pse.SetIgnoreHeader(true);
            return (!x.useRemain.HasValue || x.useRemain.Value >= 1) && !BattleFuncs.isSealedSkillEffect(defense, x) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == attack.originalUnit.unit.kind.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == defense.originalUnit.playerUnit.GetElement()) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == attack.originalUnit.playerUnit.GetElement()) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == attack.originalUnit.job.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) == 0 || attack.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id))) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.character_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.character_id) == defense.originalUnit.unit.character.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_unit_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_unit_id) == attack.originalUnit.unit.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.unit_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.unit_id) == defense.originalUnit.unit.ID) && BattleFuncs.checkInvokeSkillEffect(pse, defense, attack, colosseumTurn, unitHp: new int?(isAttacker ? hp.defenderHp : hp.attackerHp), targetHp: new int?(isAttacker ? hp.attackerHp : hp.defenderHp)) && pse.CheckLandTag(defensePanel, isAI) && !BattleFuncs.isEffectEnemyRangeAndInvalid(x, defense, attack) && !BattleFuncs.isSkillsAndEffectsInvalid(defense, attack) && !BattleFuncs.checkInvalidEffect(x, invalidSkillLogics);
          }));
          IEnumerable<BL.SkillEffect> array = (IEnumerable<BL.SkillEffect>) BattleFuncs.gearSkillEffectFilter(defense.originalUnit, skillEffects).ToArray<BL.SkillEffect>();
          if (array.Any<BL.SkillEffect>())
          {
            foreach (BL.SkillEffect effect in array)
            {
              if (BattleFuncs.isInvoke(defense, attack, attackStatus.duelParameter.defenderUnitParameter, attackStatus.duelParameter.attackerUnitParameter, defenseStatus, attackStatus, effect.baseSkillLevel, effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invocation), random, false, isAttacker ? hp.defenderHp : hp.attackerHp, isAttacker ? hp.attackerHp : hp.defenderHp, colosseumTurn))
              {
                ret.defenderUseSkillEffects.Add(BL.UseSkillEffect.Create(effect, BL.UseSkillEffect.Type.Decrement));
                ret.invokeSkillExtraInfo.Add("absolute_defense");
                nullable21 = effect.useRemain;
                if (nullable21.HasValue)
                {
                  nullable21 = effect.useRemain;
                  if (nullable21.Value >= 1)
                  {
                    BL.SkillEffect skillEffect = effect;
                    nullable21 = effect.useRemain;
                    int? nullable24 = nullable21.HasValue ? new int?(nullable21.GetValueOrDefault() - 1) : new int?();
                    skillEffect.useRemain = nullable24;
                  }
                }
                damage = 0.0f;
                break;
              }
            }
          }
        }
        IEnumerable<BL.SkillEffect> usedEffects = (IEnumerable<BL.SkillEffect>) null;
        if (hp.otherHp.ContainsKey(defense))
          usedEffects = hp.otherHp[defense].consumeSkillEffect.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.effect.EffectLogic.Enum == BattleskillEffectLogicEnum.damage_cut3_fix_duel));
        List<BL.SkillEffect> newUseEffects = new List<BL.SkillEffect>();
        float finalAttack = (float) BattleFuncs.applyDamageCut(0, (int) damage, defense, attack, attackStatus.duelParameter.defenderUnitParameter, attackStatus.duelParameter.attackerUnitParameter, defenseStatus, attackStatus, random, isAttacker ? hp.defenderHp : hp.attackerHp, isAttacker ? hp.attackerHp : hp.defenderHp, colosseumTurn, defensePanel, usedEffects, newUseEffects);
        foreach (BL.SkillEffect effect in newUseEffects)
        {
          if (!ret.hp.otherHp.ContainsKey(defense))
            ret.hp.otherHp[defense] = new TurnOtherHp(defense.hp);
          ret.hp.otherHp[defense].consumeSkillEffect.Add(effect);
          ret.defenderUseSkillEffects.Add(BL.UseSkillEffect.Create(effect, BL.UseSkillEffect.Type.Decrement));
          nullable21 = effect.useRemain;
          if (nullable21.HasValue)
          {
            nullable21 = effect.useRemain;
            if (nullable21.Value >= 1)
            {
              BL.SkillEffect skillEffect = effect;
              nullable21 = effect.useRemain;
              int? nullable25 = nullable21.HasValue ? new int?(nullable21.GetValueOrDefault() - 1) : new int?();
              skillEffect.useRemain = nullable25;
            }
          }
        }
        if ((double) finalAttack >= 1.0)
        {
          IEnumerable<BL.SkillEffect> skillEffects = defense.skillEffects.Where(BattleskillEffectLogicEnum.damage_shield, (Func<BL.SkillEffect, bool>) (x =>
          {
            BattleskillEffect effect = x.effect;
            BattleFuncs.PackedSkillEffect pse = BattleFuncs.PackedSkillEffect.Create(x);
            return (x.work == null || BattleFuncs.getShieldRemain(x, defense) >= 1) && !BattleFuncs.isSealedSkillEffect(defense, x) && BattleFuncs.checkInvokeSkillEffect(pse, defense, attack, colosseumTurn, unitHp: new int?(isAttacker ? hp.defenderHp : hp.attackerHp), targetHp: new int?(isAttacker ? hp.attackerHp : hp.defenderHp)) && pse.CheckLandTag(defensePanel, isAI) && !BattleFuncs.isEffectEnemyRangeAndInvalid(x, defense, attack) && !BattleFuncs.isSkillsAndEffectsInvalid(defense, attack) && !BattleFuncs.checkInvalidEffect(x, invalidSkillLogics);
          }));
          IEnumerable<BL.SkillEffect> array = (IEnumerable<BL.SkillEffect>) BattleFuncs.gearSkillEffectFilter(defense.originalUnit, skillEffects).ToArray<BL.SkillEffect>();
          if (array.Any<BL.SkillEffect>())
          {
            foreach (BL.SkillEffect skillEffect in (IEnumerable<BL.SkillEffect>) array.OrderByDescending<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (x => BattleFuncs.getShieldRemain(x, defense))).ThenBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (x => x.effectId)))
            {
              BL.UseSkillEffect useSkillEffect = BL.UseSkillEffect.Create(skillEffect);
              if (skillEffect.work == null)
                skillEffect.work = new float[1];
              int num29 = (int) skillEffect.work[0];
              int shieldHp = BattleFuncs.getShieldHp(skillEffect, defense);
              int num30 = num29 + (int) finalAttack;
              if (num30 > shieldHp)
              {
                finalAttack = (float) (num30 - shieldHp);
                num30 = shieldHp;
              }
              else
                finalAttack = 0.0f;
              skillEffect.work[0] = (float) num30;
              useSkillEffect.type = num30 >= shieldHp ? BL.UseSkillEffect.Type.Remove : BL.UseSkillEffect.Type.SetWork;
              useSkillEffect.work = skillEffect.work[0];
              ret.defenderUseSkillEffects.Add(useSkillEffect);
              if ((double) finalAttack <= 0.0)
              {
                ret.invokeSkillExtraInfo.Add("absolute_defense");
                break;
              }
            }
          }
        }
        ret.duelSkillProc.InvokeDamageSkill((int) finalAttack);
        if (!invokedPrayer)
          invokedPrayer = ((IEnumerable<BL.Skill>) ret.duelSkillProc.defenderSkills).Any<BL.Skill>((Func<BL.Skill, bool>) (x => ((IEnumerable<BattleskillEffect>) x.skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (y => y.EffectLogic.Enum == BattleskillEffectLogicEnum.prayer))));
        if (!invokedPrayer)
        {
          int num31 = isAttacker ? hp.defenderHp : hp.attackerHp;
          if ((double) finalAttack >= (double) num31 && defenderDuelBeginHp >= 2)
          {
            IEnumerable<BL.SkillEffect> skillEffects = defense.skillEffects.Where(BattleskillEffectLogicEnum.passive_prayer, (Func<BL.SkillEffect, bool>) (x =>
            {
              BattleskillEffect effect = x.effect;
              return (!x.useRemain.HasValue || x.useRemain.Value >= 1) && !BattleFuncs.isSealedSkillEffect(defense, x) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == defense.originalUnit.unit.kind.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == attack.originalUnit.unit.kind.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == defense.originalUnit.playerUnit.GetElement()) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == attack.originalUnit.playerUnit.GetElement()) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == defense.originalUnit.job.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == attack.originalUnit.job.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || defense.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id))) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) == 0 || attack.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id))) && (!effect.HasKey(BattleskillEffectLogicArgumentEnum.invoke_gamemode) || BattleFuncs.checkInvokeGamemode(effect.GetInt(BattleskillEffectLogicArgumentEnum.invoke_gamemode), isColosseum, defense)) && effect.GetPackedSkillEffect().CheckLandTag(defensePanel, isAI) && !BattleFuncs.isEffectEnemyRangeAndInvalid(x, defense, attack) && !BattleFuncs.isSkillsAndEffectsInvalid(defense, attack) && !BattleFuncs.checkInvalidEffect(x, invalidSkillLogics);
            }));
            IEnumerable<BL.SkillEffect> array = (IEnumerable<BL.SkillEffect>) BattleFuncs.gearSkillEffectFilter(defense.originalUnit, skillEffects).ToArray<BL.SkillEffect>();
            if (array.Any<BL.SkillEffect>())
            {
              foreach (BL.SkillEffect skillEffect in (IEnumerable<BL.SkillEffect>) array.OrderByDescending<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (x => x.baseSkill.weight)).ThenBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (x => x.baseSkillId)).ThenByDescending<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (x => x.baseSkillLevel)))
              {
                BattleFuncs.PackedSkillEffect packedSkillEffect = BattleFuncs.PackedSkillEffect.Create(skillEffect);
                if (BattleFuncs.isInvoke(defense, attack, attackStatus.duelParameter.defenderUnitParameter, attackStatus.duelParameter.attackerUnitParameter, defenseStatus, attackStatus, skillEffect.baseSkillLevel, skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invocation), random, false, isAttacker ? hp.defenderHp : hp.attackerHp, isAttacker ? hp.attackerHp : hp.defenderHp, colosseumTurn, effect: skillEffect.effect, base_invocation: packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.base_invocation) ? packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.base_invocation) : 0.0f, invocation_skill_ratio: packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.invocation_skill_ratio) ? packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.invocation_skill_ratio) : 1f, invocation_luck_ratio: packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.invocation_luck_ratio) ? packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.invocation_luck_ratio) : 1f))
                {
                  ret.defenderUseSkillEffects.Add(BL.UseSkillEffect.Create(skillEffect, BL.UseSkillEffect.Type.Decrement));
                  invokedPrayer = true;
                  break;
                }
              }
            }
          }
        }
        int val = (int) finalAttack;
        if (invokedPrayer)
        {
          int num32 = isAttacker ? hp.defenderHp : hp.attackerHp;
          if ((double) finalAttack >= (double) num32)
            finalAttack = (float) (num32 - 1);
          if (val >= num32)
            val = num32 - 1;
        }
        if ((double) finalAttack >= 1.0 && !isColosseum)
        {
          BL.ISkillEffectListUnit[] array1 = BattleFuncs.getForceUnits(BattleFuncs.env.getForceID(defense.originalUnit), isAI, true).ToArray<BL.ISkillEffectListUnit>();
          Dictionary<BL.ISkillEffectListUnit, Tuple<BL.SkillEffect, int>> source = new Dictionary<BL.ISkillEffectListUnit, Tuple<BL.SkillEffect, int>>();
          foreach (BL.ISkillEffectListUnit skillEffectListUnit in array1)
          {
            BL.ISkillEffectListUnit unit = skillEffectListUnit;
            if (unit != defense)
            {
              if (!ret.hp.otherHp.ContainsKey(unit))
                ret.hp.otherHp[unit] = new TurnOtherHp(unit.hp);
              if (ret.hp.otherHp[unit].hp > 0)
              {
                List<BL.SkillEffect> list1 = unit.skillEffects.Where(BattleskillEffectLogicEnum.damage_share, (Func<BL.SkillEffect, bool>) (x =>
                {
                  if (x.useRemain.HasValue)
                  {
                    int num33 = ret.hp.otherHp[unit].consumeSkillEffect.Count<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (y => y == x));
                    if (x.useRemain.Value <= num33)
                      return false;
                  }
                  BattleskillEffect effect = x.effect;
                  Tuple<int, int> unitCell1 = BattleFuncs.getUnitCell(unit.originalUnit, isAI);
                  BL.Panel panel = BattleFuncs.getPanel(unitCell1.Item1, unitCell1.Item2);
                  Tuple<int, int> unitCell2 = BattleFuncs.getUnitCell(defense.originalUnit, isAI);
                  int num34 = BL.fieldDistance(unitCell2.Item1, unitCell2.Item2, unitCell1.Item1, unitCell1.Item2);
                  return num34 >= effect.GetInt(BattleskillEffectLogicArgumentEnum.min_range) && num34 <= effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range) && !BattleFuncs.isSealedSkillEffect(unit, x) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == defense.originalUnit.unit.kind.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) == 0 || defense.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id))) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == defense.originalUnit.playerUnit.GetElement()) && effect.GetPackedSkillEffect().CheckLandTag(panel, isAI);
                })).ToList<BL.SkillEffect>();
                List<BL.SkillEffect> list2 = BattleFuncs.gearSkillEffectFilter(unit.originalUnit, (IEnumerable<BL.SkillEffect>) list1).ToList<BL.SkillEffect>();
                if (list2.Count > 0)
                {
                  BL.SkillEffect skillEffect1 = (BL.SkillEffect) null;
                  int num35 = int.MinValue;
                  foreach (BL.SkillEffect skillEffect2 in list2)
                  {
                    int num36 = Judgement.CalcMaximumLongToInt(0L + (long) (skillEffect2.effect.GetInt(BattleskillEffectLogicArgumentEnum.value) + skillEffect2.baseSkillLevel * skillEffect2.effect.GetInt(BattleskillEffectLogicArgumentEnum.value_skill_ratio)) + (long) Judgement.CalcMaximumCeilToIntValue((Decimal) finalAttack * (Decimal) (skillEffect2.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) + (float) skillEffect2.baseSkillLevel * skillEffect2.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_skill_ratio))));
                    if (num36 > num35)
                    {
                      num35 = num36;
                      skillEffect1 = skillEffect2;
                    }
                  }
                  source[unit] = Tuple.Create<BL.SkillEffect, int>(skillEffect1, num35);
                }
              }
            }
          }
          if (source.Count > 0)
          {
            int maxDamage = source.Values.Max<Tuple<BL.SkillEffect, int>>((Func<Tuple<BL.SkillEffect, int>, int>) (x => x.Item2));
            KeyValuePair<BL.ISkillEffectListUnit, Tuple<BL.SkillEffect, int>>[] array2 = source.Where<KeyValuePair<BL.ISkillEffectListUnit, Tuple<BL.SkillEffect, int>>>((Func<KeyValuePair<BL.ISkillEffectListUnit, Tuple<BL.SkillEffect, int>>, bool>) (x => x.Value.Item2 == maxDamage)).ToArray<KeyValuePair<BL.ISkillEffectListUnit, Tuple<BL.SkillEffect, int>>>();
            BL.ISkillEffectListUnit key;
            BL.SkillEffect skillEffect;
            int num37;
            if (array2.Length == 1)
            {
              key = array2[0].Key;
              skillEffect = array2[0].Value.Item1;
              num37 = array2[0].Value.Item2;
            }
            else
            {
              Tuple<int, int> unitCell = BattleFuncs.getUnitCell(defense.originalUnit, isAI);
              List<Tuple<int, int>> list = ((IEnumerable<KeyValuePair<BL.ISkillEffectListUnit, Tuple<BL.SkillEffect, int>>>) array2).Select<KeyValuePair<BL.ISkillEffectListUnit, Tuple<BL.SkillEffect, int>>, Tuple<int, int>>((Func<KeyValuePair<BL.ISkillEffectListUnit, Tuple<BL.SkillEffect, int>>, Tuple<int, int>>) (x => BattleFuncs.getUnitCell(x.Key.originalUnit, isAI))).ToList<Tuple<int, int>>();
              int num38 = unitCell.Item2;
              int num39 = unitCell.Item1;
              int index1 = -1;
              for (int index2 = 1; index2 <= 50; ++index2)
              {
                int num40 = num39 + index2;
                int num41 = num38;
                for (int index3 = 0; index3 < index2; ++index3)
                {
                  index1 = list.IndexOf(Tuple.Create<int, int>(num40, num41));
                  if (index1 == -1)
                  {
                    --num40;
                    ++num41;
                  }
                  else
                    break;
                }
                if (index1 == -1)
                {
                  for (int index4 = 0; index4 < index2; ++index4)
                  {
                    index1 = list.IndexOf(Tuple.Create<int, int>(num40, num41));
                    if (index1 == -1)
                    {
                      --num40;
                      --num41;
                    }
                    else
                      break;
                  }
                  if (index1 == -1)
                  {
                    for (int index5 = 0; index5 < index2; ++index5)
                    {
                      index1 = list.IndexOf(Tuple.Create<int, int>(num40, num41));
                      if (index1 == -1)
                      {
                        ++num40;
                        --num41;
                      }
                      else
                        break;
                    }
                    if (index1 == -1)
                    {
                      for (int index6 = 0; index6 < index2; ++index6)
                      {
                        index1 = list.IndexOf(Tuple.Create<int, int>(num40, num41));
                        if (index1 == -1)
                        {
                          ++num40;
                          ++num41;
                        }
                        else
                          break;
                      }
                      if (index1 != -1)
                        break;
                    }
                    else
                      break;
                  }
                  else
                    break;
                }
                else
                  break;
              }
              if (index1 == -1)
                index1 = 0;
              key = array2[index1].Key;
              skillEffect = array2[index1].Value.Item1;
              num37 = array2[index1].Value.Item2;
            }
            BattleFuncs.PackedSkillEffect packedSkillEffect = BattleFuncs.PackedSkillEffect.Create(skillEffect);
            if (num37 < 0)
              num37 = 0;
            else if (num37 > val)
              num37 = val;
            int num42 = num37;
            int num43 = val - num37;
            float num44 = !packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.damage_percentage) ? 1f : packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.damage_percentage);
            float num45 = !packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.target_damage_percentage) ? 1f : packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.target_damage_percentage);
            int num46 = Judgement.CalcMaximumCeilToIntValue((Decimal) num42 * (Decimal) num44);
            int num47 = Judgement.CalcMaximumCeilToIntValue((Decimal) num43 * (Decimal) num45);
            if (num46 < 0)
              num46 = 0;
            if (num47 < 0)
              num47 = 0;
            ret.hp.otherHp[key].hp -= num46;
            finalAttack = (float) num47;
            val = num47;
            ret.hp.otherHp[key].consumeSkillEffect.Add(skillEffect);
            ret.damageShareUnit.Add(key);
            ret.damageShareDamage.Add(num46);
            ret.damageShareSkillEffect.Add(BL.UseSkillEffect.Create(skillEffect));
          }
        }
        ret.damage = NC.Clamp(0, isAttacker ? hp.defenderHp : hp.attackerHp, val);
        ret.dispDamage = NC.Clamp(0, 99999999, val);
        ret.realDamage = val;
        if (ret.realDamage < 0)
          ret.realDamage = 0;
        if (ret.duelSkillProc.isInvokeCounterAttack)
        {
          float num48 = finalAttack * ret.duelSkillProc.counterAttackRate;
          ret.counterDamage += (int) num48;
        }
        action1();
        if ((isAttacker ? (!hp.defenderIsDontAction ? 1 : 0) : (!hp.attackerIsDontAction ? 1 : 0)) != 0)
        {
          Func<BL.SkillEffect, bool> f = (Func<BL.SkillEffect, bool>) (x =>
          {
            BattleskillEffect effect = x.effect;
            BattleFuncs.PackedSkillEffect pse = BattleFuncs.PackedSkillEffect.Create(x);
            pse.SetIgnoreHeader(true);
            if (!BattleFuncs.isSealedSkillEffect(defense, x) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == defense.originalUnit.unit.kind.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == attack.originalUnit.unit.kind.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == defense.originalUnit.playerUnit.GetElement()) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == attack.originalUnit.playerUnit.GetElement()) && (!effect.HasKey(BattleskillEffectLogicArgumentEnum.job_id) || effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == defense.originalUnit.job.ID) && (!effect.HasKey(BattleskillEffectLogicArgumentEnum.target_job_id) || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == attack.originalUnit.job.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || defense.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id))) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) == 0 || attack.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id))) && (!effect.HasKey(BattleskillEffectLogicArgumentEnum.skill_group_id) || effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id) == 0 || defense.originalUnit.unit.HasSkillGroupId(effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id))) && (!effect.HasKey(BattleskillEffectLogicArgumentEnum.exclude_skill_group_id) || effect.GetInt(BattleskillEffectLogicArgumentEnum.exclude_skill_group_id) == 0 || !defense.originalUnit.unit.HasSkillGroupId(effect.GetInt(BattleskillEffectLogicArgumentEnum.exclude_skill_group_id))) && BattleFuncs.checkInvokeSkillEffect(pse, defense, attack, colosseumTurn, unitHp: new int?(isAttacker ? hp.defenderHp : hp.attackerHp), targetHp: new int?(isAttacker ? hp.attackerHp : hp.defenderHp)) && pse.CheckLandTag(defensePanel, isAI) && !BattleFuncs.isEffectEnemyRangeAndInvalid(x, defense, attack) && !BattleFuncs.isSkillsAndEffectsInvalid(defense, attack))
            {
              bool flag = false;
              float percentage_invocation = 0.0f;
              float base_invocation = 0.0f;
              float invocation_skill_ratio = 1f;
              float invocation_luck_ratio = 1f;
              if (pse.HasKey(BattleskillEffectLogicArgumentEnum.percentage_invocation))
              {
                flag = true;
                percentage_invocation = pse.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invocation);
              }
              if (pse.HasKey(BattleskillEffectLogicArgumentEnum.base_invocation))
              {
                flag = true;
                base_invocation = pse.GetFloat(BattleskillEffectLogicArgumentEnum.base_invocation);
              }
              if (pse.HasKey(BattleskillEffectLogicArgumentEnum.invocation_skill_ratio))
              {
                flag = true;
                invocation_skill_ratio = pse.GetFloat(BattleskillEffectLogicArgumentEnum.invocation_skill_ratio);
              }
              if (pse.HasKey(BattleskillEffectLogicArgumentEnum.invocation_luck_ratio))
              {
                flag = true;
                invocation_luck_ratio = pse.GetFloat(BattleskillEffectLogicArgumentEnum.invocation_luck_ratio);
              }
              if (!flag || BattleFuncs.isInvoke(defense, attack, attackStatus.duelParameter.defenderUnitParameter, attackStatus.duelParameter.attackerUnitParameter, defenseStatus, attackStatus, x.baseSkillLevel, percentage_invocation, random, false, isAttacker ? hp.defenderHp : hp.attackerHp, isAttacker ? hp.attackerHp : hp.defenderHp, colosseumTurn, base_invocation: base_invocation, invocation_skill_ratio: invocation_skill_ratio, invocation_luck_ratio: invocation_luck_ratio))
                return true;
            }
            return false;
          });
          BL.SkillEffect[] array3 = defense.skillEffects.Where(BattleskillEffectLogicEnum.damage_drain, f).ToArray<BL.SkillEffect>();
          BL.SkillEffect[] array4 = defense.skillEffects.Where(BattleskillEffectLogicEnum.damage_drain2, f).ToArray<BL.SkillEffect>();
          if (array3.Length != 0 || array4.Length != 0)
          {
            List<BattleFuncs.SkillParam> skillParams = new List<BattleFuncs.SkillParam>();
            int num49 = (isAttacker ? hp.defenderHp : hp.attackerHp) - ret.damage;
            foreach (BL.SkillEffect effect in array3)
            {
              Decimal num50 = (Decimal) (effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_drain) + (float) effect.baseSkillLevel * effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio));
              int healValue = BattleFuncs.getHealValue(defense, defensePanel, (int) ((Decimal) ret.damage * num50), effect.baseSkill.skill_type, attack);
              skillParams.Add(BattleFuncs.SkillParam.CreateAdd(defense.originalUnit, effect, (float) healValue));
            }
            if (num49 >= 1)
            {
              foreach (BL.SkillEffect effect in array4)
              {
                Decimal num51 = (Decimal) (effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_drain) + (float) effect.baseSkillLevel * effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio));
                int healValue = BattleFuncs.getHealValue(defense, defensePanel, (int) ((Decimal) ret.damage * num51), effect.baseSkill.skill_type, attack);
                skillParams.Add(BattleFuncs.SkillParam.CreateAdd(defense.originalUnit, effect, (float) healValue));
              }
            }
            ret.defenderDispDrainDamage = 0;
            ret.defenderSwapHealDamage = 0;
            foreach (BattleFuncs.SkillParam skillParam in BattleFuncs.gearSkillParamFilter(skillParams))
            {
              if (skillParam.addParam.HasValue)
              {
                int num52 = (int) skillParam.addParam.Value;
                if (num52 > 0)
                  ret.defenderDispDrainDamage += num52;
                else if (num52 < 0)
                  ret.defenderSwapHealDamage -= num52;
              }
            }
            if (ret.defenderDispDrainDamage > 0)
              ret.defenderDrainDamage = Mathf.Min(defense.originalUnit.parameter.Hp, num49 + ret.defenderDispDrainDamage) - num49;
          }
        }
        ret.dispDrainDamage = 0;
        ret.drainDamage = 0;
        int hp2 = attack.originalUnit.parameter.Hp;
        float num53 = ret.duelSkillProc.attackerSkills.Length >= 1 || baseDuelSkill.attackerSkills.Length >= 1 ? ret.duelSkillProc.drainRateRatio * drainRateRatio : 1f;
        int num54 = 0;
        if ((double) ret.duelSkillProc.drainRate > 0.0)
          num54 = BattleFuncs.getHealValue(attack, attackPanel, Mathf.CeilToInt((float) ret.damage * ret.duelSkillProc.drainRate * num53), BattleskillSkillType.duel, defense);
        else if ((double) drainRate > 0.0)
          num54 = BattleFuncs.getHealValue(attack, attackPanel, Mathf.CeilToInt((float) ret.damage * drainRate * num53), BattleskillSkillType.duel, defense);
        else if (attackStatus.isDrain)
          num54 = BattleFuncs.getHealValue(attack, attackPanel, Mathf.CeilToInt((float) ret.damage * attackStatus.drainRate * num53), BattleskillSkillType.magic, defense);
        if (num54 > 0)
        {
          ret.dispDrainDamage = num54;
          ret.drainDamage = NC.Clamp(0, hp2, ret.dispDrainDamage);
        }
        else if (num54 < 0)
          ret.attackerSwapHealDamage = -num54;
        if (isAttacker)
        {
          ret.hp.defenderHp -= ret.damage;
          ret.hp.defenderHp += ret.defenderDrainDamage;
          int num55 = Mathf.Min(hp2, hp.attackerHp + ret.drainDamage);
          if (ret.dispDrainDamage > 0)
            ret.drainDamage = Mathf.Max(0, num55 - hp.attackerHp);
          ret.hp.attackerHp = num55;
        }
        else
        {
          ret.hp.attackerHp -= ret.damage;
          ret.hp.attackerHp += ret.defenderDrainDamage;
          int num56 = Mathf.Min(hp2, hp.defenderHp + ret.drainDamage);
          if (ret.dispDrainDamage > 0)
            ret.drainDamage = Mathf.Max(0, num56 - hp.defenderHp);
          hp.defenderHp = num56;
        }
      }
      else
      {
        ret.duelSkillProc.InvokeDamageSkill(0);
        action1();
      }
      Action<IEnumerable<BL.SkillEffect>, BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, int, int, bool> action4 = (Action<IEnumerable<BL.SkillEffect>, BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, int, int, bool>) ((efs, effectUnit, targetUnit, effectUnitHp, targetUnitHp, isDefenseEffect) =>
      {
        foreach (BL.SkillEffect headerEffect in BattleFuncs.gearSkillEffectFilter(effectUnit.originalUnit, efs.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x =>
        {
          BattleskillEffect effect = x.effect;
          int num61 = effect.GetInt(BattleskillEffectLogicArgumentEnum.attack_phase);
          BattleFuncs.PackedSkillEffect pse = BattleFuncs.PackedSkillEffect.Create(x);
          pse.SetIgnoreHeader(true);
          int num62 = pse.HasKey(BattleskillEffectLogicArgumentEnum.is_attack_nc) ? pse.GetInt(BattleskillEffectLogicArgumentEnum.is_attack_nc) : 0;
          bool flag = isDefenseEffect ? (invokedAmbush ? isAttacker : !isAttacker) : (invokedAmbush ? !isAttacker : isAttacker);
          if (num62 == 1 && !flag || num62 == 2 & flag)
            return false;
          BL.Panel panel = !isDefenseEffect ? attackPanel : defensePanel;
          if (!BattleFuncs.isSealedSkillEffect(effectUnit, x))
          {
            switch (num61)
            {
              case 0:
                if ((effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == targetUnit.originalUnit.playerUnit.GetElement()) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == effectUnit.originalUnit.playerUnit.GetElement()) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == targetUnit.originalUnit.unit.kind.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == effectUnit.originalUnit.unit.kind.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) == 0 || targetUnit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id))) && BattleFuncs.checkInvokeSkillEffect(pse, effectUnit, targetUnit, colosseumTurn, unitHp: new int?(effectUnitHp), targetHp: new int?(targetUnitHp)) && pse.CheckLandTag(panel, isAI) && !BattleFuncs.isEffectEnemyRangeAndInvalid(x, effectUnit, targetUnit))
                  return !BattleFuncs.isSkillsAndEffectsInvalid(effectUnit, targetUnit, x);
                break;
              case 1:
                if (!isOneMoreAttack)
                  goto case 0;
                else
                  goto default;
              default:
                if (!(num61 == 2 & isOneMoreAttack))
                  break;
                goto case 0;
            }
          }
          return false;
        }))))
        {
          int num59 = headerEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id);
          bool flag4;
          if (num59 < 0)
          {
            flag4 = true;
            num59 = -num59;
          }
          else
            flag4 = false;
          if (num59 != 0 && MasterData.BattleskillSkill.ContainsKey(num59))
          {
            float lottery = headerEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invest) + headerEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invest_levelup) * (float) headerEffect.baseSkillLevel;
            BattleFuncs.PackedSkillEffect packedSkillEffect = BattleFuncs.PackedSkillEffect.Create(headerEffect);
            int onceInvestFlag = packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.once_invest) ? packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.once_invest) : 0;
            BattleskillSkill battleskillSkill = MasterData.BattleskillSkill[num59];
            foreach (BL.ISkillEffectListUnit skillEffectListUnit in BattleFuncs.getInvestUnit(effectUnit, targetUnit, num59, headerEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.range_type), isAI, isColosseum))
            {
              if (battleskillSkill.skill_type == BattleskillSkillType.ailment)
              {
                int num60 = skillEffectListUnit != effectUnit ? (skillEffectListUnit != targetUnit ? (!ret.hp.otherHp.ContainsKey(skillEffectListUnit) ? skillEffectListUnit.hp : ret.hp.otherHp[skillEffectListUnit].hp) : targetUnitHp) : effectUnitHp;
                List<BL.SkillEffect> useResistEffects;
                bool flag5 = BattleFuncs.isAilmentInvest(lottery, num59, skillEffectListUnit, effectUnit, random, colosseumTurn, out useResistEffects, ret.hp, new int?(num60), new int?(effectUnitHp));
                ret.duelSkillProc.addInvestSkills(skillEffectListUnit, BattleFuncs.ailmentInvest(num59, skillEffectListUnit), new int[1]
                {
                  num59
                }, effectUnit, headerEffect.baseSkillId, true, (flag4 ? 1 : 0) != 0, 0, onceInvestFlag, (flag5 ? 1 : 0) != 0, useResistEffects);
              }
              else if ((double) lottery >= (double) random.NextFloat())
                ret.duelSkillProc.addInvestSkills(skillEffectListUnit, new BL.Skill[1]
                {
                  new BL.Skill() { id = num59, level = 1 }
                }, new int[1]{ num59 }, effectUnit, headerEffect.baseSkillId, false, (flag4 ? 1 : 0) != 0, 0, onceInvestFlag);
            }
          }
        }
      });
      action4(attack.skillEffects.Where(BattleskillEffectLogicEnum.passive_invest_passive).Concat<BL.SkillEffect>(BattleFuncs.getAttackMethodExtraSkillEffects(attackStatus, BattleskillEffectLogicEnum.passive_invest_passive)), attack, defense, isAttacker ? attackerHp : defenderHp, isAttacker ? defenderHp : attackerHp, false);
      action4(defense.skillEffects.Where(BattleskillEffectLogicEnum.passive_invest_passive2), defense, attack, isAttacker ? defenderHp : attackerHp, isAttacker ? attackerHp : defenderHp, true);
      return ret;
    }

    public static void calcSingleAttack(
      List<BL.DuelTurn> turns,
      List<BL.DuelTurn> turnsTemp,
      TurnHp hp,
      bool isAttacker,
      BL.ISkillEffectListUnit attack,
      AttackStatus attackStatus,
      BL.Panel attackPanel,
      BL.ISkillEffectListUnit defense,
      AttackStatus defenseStatus,
      BL.Panel defensePanel,
      int distance,
      XorShift random,
      bool isAI,
      int? colosseumTurn,
      bool invokedAmbush,
      bool invokedPrayer,
      int defenseHp,
      bool isOneMoreAttack,
      int attackedCount,
      bool isInvalidAttackDuelSkill,
      bool isEnableOverKill)
    {
      if (!isEnableOverKill && hp.isDieAttackerOrDefender())
        return;
      BL.MagicBullet magicBullet = attackStatus.magicBullet;
      int cost = attackedCount > 0 || magicBullet == null ? 0 : magicBullet.cost;
      if (magicBullet != null && (isAttacker ? hp.attackerHp : hp.defenderHp) <= cost)
        return;
      if (!invokedPrayer)
      {
        invokedPrayer = BattleFuncs.isInvokedDefenderSkillLogic((IEnumerable<BL.DuelTurn>) turns, BattleskillEffectLogicEnum.prayer, new bool?(isAttacker));
        if (!invokedPrayer)
          invokedPrayer = BattleFuncs.isInvokedDefenderConsumeSkillLogic((IEnumerable<BL.DuelTurn>) turns, BattleskillEffectLogicEnum.passive_prayer, new bool?(isAttacker));
      }
      bool hasValue = colosseumTurn.HasValue;
      IEnumerable<BattleskillSkill> battleskillSkills = defense.skillEffects.Where(BattleskillSkillType.ailment);
      IEnumerable<BattleskillSkill> second1 = attack.skillEffects.Where(BattleskillSkillType.ailment);
      BL.DuelTurn turn = new BL.DuelTurn();
      turn.isAtackker = isAttacker;
      turn.attackStatus = attackStatus;
      turn.attackerStatus = (AttackStatus) null;
      turn.defenderStatus = (AttackStatus) null;
      turn.counterDamage = magicBullet != null ? cost : 0;
      turn.skillIds = ((IEnumerable<BL.Skill>) attack.originalUnit.skills).Select<BL.Skill, int>((Func<BL.Skill, int>) (x => x.id)).ToArray<int>();
      if (isAttacker)
        hp.attackerHp -= turn.counterDamage;
      else
        hp.defenderHp -= turn.counterDamage;
      float? criticalRate = new float?();
      float? invokeRate = new float?();
      List<BL.Skill> first = new List<BL.Skill>();
      if ((isAttacker ? (hp.attackerIsDontUseSkill ? 1 : 0) : (hp.defenderIsDontUseSkill ? 1 : 0)) == 0)
      {
        IEnumerable<Tuple<BattleskillSkill, int, int>> source = ((IEnumerable<Tuple<BattleskillSkill, int, int>>) attack.originalUnit.unitAndGearSkills).Where<Tuple<BattleskillSkill, int, int>>((Func<Tuple<BattleskillSkill, int, int>, bool>) (skill => ((IEnumerable<BattleskillEffect>) skill.Item1.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (effect => effect.EffectLogic.Enum == BattleskillEffectLogicEnum.fierce_rival && !attack.IsDontUseSkill(skill.Item1.ID)))));
        if (source.Any<Tuple<BattleskillSkill, int, int>>() && !BattleFuncs.isSkillsAndEffectsInvalid(attack, defense) && !BattleFuncs.cantInvokeDuelSkill(2, attack, defense, attackPanel, defensePanel))
        {
          BL.DuelTurn duelTurn = turns.LastOrDefault<BL.DuelTurn>();
          foreach (Tuple<BattleskillSkill, int, int> tuple in source)
          {
            bool flag = false;
            foreach (BattleskillEffect battleskillEffect in ((IEnumerable<BattleskillEffect>) tuple.Item1.Effects).Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.fierce_rival)))
            {
              int num1 = battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.invoke_skill);
              int num2 = battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.invoke_critical);
              int num3 = battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.invoke_turn);
              flag = battleskillEffect.checkLevel(tuple.Item2) && (!battleskillEffect.HasKey(BattleskillEffectLogicArgumentEnum.gear_kind_id) || battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == attack.originalUnit.unit.kind.ID) && (!battleskillEffect.HasKey(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) || battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 || battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == defense.originalUnit.unit.kind.ID) && (!battleskillEffect.HasKey(BattleskillEffectLogicArgumentEnum.element) || battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) == attack.originalUnit.playerUnit.GetElement()) && (!battleskillEffect.HasKey(BattleskillEffectLogicArgumentEnum.target_element) || battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 || (CommonElement) battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == defense.originalUnit.playerUnit.GetElement()) && (num3 == 0 || num3 == 1 && duelTurn != null && isAttacker != duelTurn.isAtackker || num3 == 2 && duelTurn != null && isAttacker == duelTurn.isAtackker) && (num1 == 0 || num1 == 1 && duelTurn != null && ((IEnumerable<BL.Skill>) duelTurn.invokeDuelSkills).Any<BL.Skill>((Func<BL.Skill, bool>) (x =>
              {
                BattleskillGenre? genre1 = x.genre1;
                BattleskillGenre battleskillGenre = BattleskillGenre.attack;
                return genre1.GetValueOrDefault() == battleskillGenre & genre1.HasValue;
              })) || num1 == 2 && duelTurn != null && !((IEnumerable<BL.Skill>) duelTurn.invokeDuelSkills).Any<BL.Skill>((Func<BL.Skill, bool>) (x =>
              {
                BattleskillGenre? genre1 = x.genre1;
                BattleskillGenre battleskillGenre = BattleskillGenre.attack;
                return genre1.GetValueOrDefault() == battleskillGenre & genre1.HasValue;
              }))) && (num2 == 0 || num2 == 1 && duelTurn != null && duelTurn.isCritical || num2 == 2 && duelTurn != null && !duelTurn.isCritical) && battleskillEffect.GetPackedSkillEffect().CheckLandTag(attackPanel, isAI) && BattleFuncs.isInvoke(attack, defense, attackStatus.duelParameter.attackerUnitParameter, attackStatus.duelParameter.defenderUnitParameter, attackStatus, defenseStatus, tuple.Item2, battleskillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invocation), random, true, isAttacker ? hp.attackerHp : hp.defenderHp, isAttacker ? hp.defenderHp : hp.attackerHp, colosseumTurn);
              if (flag)
              {
                float num4;
                if ((double) (num4 = battleskillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.counter_critical_rate)) != 0.0 && !BattleFuncs.isCriticalGuardEnable(defense, attack, defensePanel))
                  criticalRate = new float?(num4);
                float num5;
                if ((double) (num5 = battleskillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.counter_skill_rate)) != 0.0)
                  invokeRate = new float?(num5);
                first.Add(new BL.Skill()
                {
                  id = tuple.Item1.ID
                });
                break;
              }
            }
            if (flag)
              break;
          }
        }
      }
      List<BL.ISkillEffectListUnit> skillEffectListUnitList1 = new List<BL.ISkillEffectListUnit>();
      List<int> intList = new List<int>();
      List<BL.ISkillEffectListUnit> skillEffectListUnitList2 = new List<BL.ISkillEffectListUnit>();
      List<int> turnInvestFromSkillIds = new List<int>();
      BL.Skill[] skillArray = (BL.Skill[]) null;
      Dictionary<BL.SkillEffect, BL.ISkillEffectListUnit> useResistEffects = new Dictionary<BL.SkillEffect, BL.ISkillEffectListUnit>();
      BattleDuelSkill battleDuelSkill1 = BattleDuelSkill.invokeBiAttackSkills(attack, attackStatus, attackPanel, defense, defenseStatus, defensePanel, distance, isAttacker ? hp.attackerHp : hp.defenderHp, isAttacker ? hp.defenderHp : hp.attackerHp, isAttacker ? hp.attackerIsDontUseSkill : hp.defenderIsDontUseSkill, isAttacker ? hp.defenderIsDontUseSkill : hp.attackerIsDontUseSkill, random, isAI, colosseumTurn, isAttacker, invokedAmbush, invokeRate, isOneMoreAttack, isInvalidAttackDuelSkill, hp);
      turn.invokeAttackerDuelSkillEffectIds = new List<int>();
      turn.invokeDefenderDuelSkillEffectIds = new List<int>();
      turn.useSkillUnit = new List<BL.ISkillEffectListUnit>();
      turn.useSkillEffect = new List<BL.UseSkillEffect>();
      if (battleDuelSkill1.attackCount > 1)
      {
        List<BL.SuiseiResult> source1 = new List<BL.SuiseiResult>();
        List<BL.Skill> source2 = new List<BL.Skill>();
        List<BL.Skill> source3 = new List<BL.Skill>();
        List<BL.Skill> skillList1 = new List<BL.Skill>();
        List<BL.Skill> skillList2 = new List<BL.Skill>();
        List<BL.Skill> givePassiveSkills = new List<BL.Skill>();
        List<BL.UseSkillEffect> useSkillEffectList1 = new List<BL.UseSkillEffect>();
        List<BL.UseSkillEffect> useSkillEffectList2 = new List<BL.UseSkillEffect>();
        turn.invokeSkillExtraInfo = new List<string>();
        turn.damageShareUnit = new List<BL.ISkillEffectListUnit>();
        turn.damageShareDamage = new List<int>();
        turn.damageShareSkillEffect = new List<BL.UseSkillEffect>();
        for (int index = 0; index < battleDuelSkill1.attackCount; ++index)
        {
          BattleFuncs.AttackDamageData attackDamageData = BattleFuncs.calcAttackDamage(random, isAttacker, hp, attack, attackStatus, attackPanel, defense, defenseStatus, defensePanel, distance, isAI, colosseumTurn, battleDuelSkill1, invokedAmbush, invokedPrayer, invokeRate, criticalRate, defenseHp, isOneMoreAttack, true, index, isInvalidAttackDuelSkill);
          if (!invokedPrayer)
          {
            invokedPrayer = ((IEnumerable<BL.Skill>) attackDamageData.duelSkillProc.defenderSkills).Any<BL.Skill>((Func<BL.Skill, bool>) (x => ((IEnumerable<BattleskillEffect>) x.skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (y => y.EffectLogic.Enum == BattleskillEffectLogicEnum.prayer))));
            if (!invokedPrayer)
              invokedPrayer = attackDamageData.defenderUseSkillEffects.Any<BL.UseSkillEffect>((Func<BL.UseSkillEffect, bool>) (x => MasterData.BattleskillEffect[x.effectEffectId].EffectLogic.Enum == BattleskillEffectLogicEnum.passive_prayer));
          }
          useSkillEffectList1.AddRange((IEnumerable<BL.UseSkillEffect>) attackDamageData.attackerUseSkillEffects);
          useSkillEffectList2.AddRange((IEnumerable<BL.UseSkillEffect>) attackDamageData.defenderUseSkillEffects);
          turn.invokeSkillExtraInfo.AddRange((IEnumerable<string>) attackDamageData.invokeSkillExtraInfo);
          turn.damageShareUnit.AddRange((IEnumerable<BL.ISkillEffectListUnit>) attackDamageData.damageShareUnit);
          turn.damageShareDamage.AddRange((IEnumerable<int>) attackDamageData.damageShareDamage);
          turn.damageShareSkillEffect.AddRange((IEnumerable<BL.UseSkillEffect>) attackDamageData.damageShareSkillEffect);
          BattleFuncs.recordInvokeDuelSkillEffectIds(attack, defense, turn, attackDamageData.isHit, attackDamageData.duelSkillProc.invokeAttackerDuelSkillEffectIds, attackDamageData.duelSkillProc.invokeDefenderDuelSkillEffectIds);
          turn.counterDamage += attackDamageData.counterDamage;
          BL.SuiseiResult suiseiResult = new BL.SuiseiResult();
          suiseiResult.damage = attackDamageData.damage;
          suiseiResult.dispDamage = attackDamageData.dispDamage;
          suiseiResult.realDamage = attackDamageData.realDamage;
          suiseiResult.invokeDuelSkills = ((IEnumerable<BL.Skill>) attackDamageData.duelSkillProc.attackerSkills).Concat<BL.Skill>((IEnumerable<BL.Skill>) attackDamageData.duelSkillProc.attackerElementSkills).ToArray<BL.Skill>();
          suiseiResult.invokeDefenderDuelSkills = ((IEnumerable<BL.Skill>) attackDamageData.duelSkillProc.defenderSkills).Concat<BL.Skill>((IEnumerable<BL.Skill>) attackDamageData.duelSkillProc.defenderElementSkills).ToArray<BL.Skill>();
          suiseiResult.invokeSkillExtraInfo = attackDamageData.invokeSkillExtraInfo;
          suiseiResult.drainDamage = attackDamageData.drainDamage;
          suiseiResult.dispDrainDamage = attackDamageData.dispDrainDamage;
          suiseiResult.isCritical = attackDamageData.isCritical;
          suiseiResult.isHit = attackDamageData.isHit;
          suiseiResult.defenderDrainDamage = attackDamageData.defenderDrainDamage;
          suiseiResult.defenderDispDrainDamage = attackDamageData.defenderDispDrainDamage;
          suiseiResult.attackerSwapHealDamage = attackDamageData.attackerSwapHealDamage;
          suiseiResult.defenderSwapHealDamage = attackDamageData.defenderSwapHealDamage;
          source1.Add(suiseiResult);
          source2.AddRange((IEnumerable<BL.Skill>) suiseiResult.invokeDefenderDuelSkills);
          source3.AddRange((IEnumerable<BL.Skill>) suiseiResult.invokeDuelSkills);
          BattleFuncs.assortOneAttackInvest(attack, defense, battleDuelSkill1, attackDamageData.duelSkillProc, skillEffectListUnitList1, intList, skillEffectListUnitList2, turnInvestFromSkillIds, givePassiveSkills, skillList2, skillList1, suiseiResult.isHit, index, turns, useResistEffects);
        }
        turn.suiseiResults = source1;
        turn.isHit = source1.Any<BL.SuiseiResult>((Func<BL.SuiseiResult, bool>) (x => x.isHit));
        turn.isCritical = source1.Any<BL.SuiseiResult>((Func<BL.SuiseiResult, bool>) (x => x.isCritical));
        turn.invokeDuelSkills = first.Concat<BL.Skill>((IEnumerable<BL.Skill>) battleDuelSkill1.attackerSkills).Concat<BL.Skill>(source3.GroupBy<BL.Skill, int>((Func<BL.Skill, int>) (x => x.id)).Select<IGrouping<int, BL.Skill>, BL.Skill>((Func<IGrouping<int, BL.Skill>, BL.Skill>) (x => x.FirstOrDefault<BL.Skill>()))).ToArray<BL.Skill>();
        turn.invokeDefenderDuelSkills = ((IEnumerable<BL.Skill>) battleDuelSkill1.defenderSkills).Concat<BL.Skill>(source2.GroupBy<BL.Skill, int>((Func<BL.Skill, int>) (x => x.id)).Select<IGrouping<int, BL.Skill>, BL.Skill>((Func<IGrouping<int, BL.Skill>, BL.Skill>) (x => x.FirstOrDefault<BL.Skill>()))).ToArray<BL.Skill>();
        turn.damage = source1.SumMaximumIntValue<BL.SuiseiResult>((Func<BL.SuiseiResult, int>) (x => x.damage), Judgement.MaximumDamageValue);
        turn.dispDamage = source1.SumMaximumIntValue<BL.SuiseiResult>((Func<BL.SuiseiResult, int>) (x => x.dispDamage), Judgement.MaximumDamageValue);
        turn.realDamage = source1.SumMaximumIntValue<BL.SuiseiResult>((Func<BL.SuiseiResult, int>) (x => x.realDamage), Judgement.MaximumDamageValue);
        turn.drainDamage = source1.SumMaximumIntValue<BL.SuiseiResult>((Func<BL.SuiseiResult, int>) (x => x.drainDamage), Judgement.MaximumDamageValue);
        turn.dispDrainDamage = source1.SumMaximumIntValue<BL.SuiseiResult>((Func<BL.SuiseiResult, int>) (x => x.dispDrainDamage), Judgement.MaximumDamageValue);
        turn.defenderDrainDamage = source1.SumMaximumIntValue<BL.SuiseiResult>((Func<BL.SuiseiResult, int>) (x => x.defenderDrainDamage), Judgement.MaximumDamageValue);
        turn.defenderDispDrainDamage = source1.SumMaximumIntValue<BL.SuiseiResult>((Func<BL.SuiseiResult, int>) (x => x.defenderDispDrainDamage), Judgement.MaximumDamageValue);
        turn.attackerSwapHealDamage = source1.SumMaximumIntValue<BL.SuiseiResult>((Func<BL.SuiseiResult, int>) (x => x.attackerSwapHealDamage), Judgement.MaximumDamageValue);
        turn.defenderSwapHealDamage = source1.SumMaximumIntValue<BL.SuiseiResult>((Func<BL.SuiseiResult, int>) (x => x.defenderSwapHealDamage), Judgement.MaximumDamageValue);
        turn.attackerRestHp = hp.attackerHp;
        turn.defenderRestHp = hp.defenderHp;
        turn.attackerUseSkillEffects = useSkillEffectList1;
        turn.defenderUseSkillEffects = useSkillEffectList2;
        if (skillList1.Count > 0)
          turn.invokeAilmentSkills = skillList1.GroupBy<BL.Skill, int>((Func<BL.Skill, int>) (x => x.id)).Select<IGrouping<int, BL.Skill>, BL.Skill>((Func<IGrouping<int, BL.Skill>, BL.Skill>) (x => x.FirstOrDefault<BL.Skill>())).ToArray<BL.Skill>();
        if (skillList2.Count > 0)
          skillArray = skillList2.GroupBy<BL.Skill, int>((Func<BL.Skill, int>) (x => x.id)).Select<IGrouping<int, BL.Skill>, BL.Skill>((Func<IGrouping<int, BL.Skill>, BL.Skill>) (x => x.FirstOrDefault<BL.Skill>())).ToArray<BL.Skill>();
        if (givePassiveSkills.Count > 0)
          turn.invokeGiveSkills = givePassiveSkills.ToArray();
      }
      else
      {
        BattleFuncs.AttackDamageData attackDamageData = BattleFuncs.calcAttackDamage(random, isAttacker, hp, attack, attackStatus, attackPanel, defense, defenseStatus, defensePanel, distance, isAI, colosseumTurn, battleDuelSkill1, invokedAmbush, invokedPrayer, invokeRate, criticalRate, defenseHp, isOneMoreAttack, false, isInvalidAttackDuelSkill: isInvalidAttackDuelSkill);
        turn.counterDamage += attackDamageData.counterDamage;
        turn.isHit = attackDamageData.isHit;
        turn.isCritical = attackDamageData.isCritical;
        turn.invokeDuelSkills = first.Concat<BL.Skill>((IEnumerable<BL.Skill>) battleDuelSkill1.attackerSkills).Concat<BL.Skill>((IEnumerable<BL.Skill>) attackDamageData.duelSkillProc.attackerSkills).Concat<BL.Skill>((IEnumerable<BL.Skill>) attackDamageData.duelSkillProc.attackerElementSkills).ToArray<BL.Skill>();
        turn.invokeDefenderDuelSkills = ((IEnumerable<BL.Skill>) battleDuelSkill1.defenderSkills).Concat<BL.Skill>((IEnumerable<BL.Skill>) attackDamageData.duelSkillProc.defenderSkills).Concat<BL.Skill>((IEnumerable<BL.Skill>) attackDamageData.duelSkillProc.defenderElementSkills).ToArray<BL.Skill>();
        turn.damage = attackDamageData.damage;
        turn.dispDamage = attackDamageData.dispDamage;
        turn.realDamage = attackDamageData.realDamage;
        turn.drainDamage = attackDamageData.drainDamage;
        turn.dispDrainDamage = attackDamageData.dispDrainDamage;
        turn.defenderDrainDamage = attackDamageData.defenderDrainDamage;
        turn.defenderDispDrainDamage = attackDamageData.defenderDispDrainDamage;
        turn.attackerSwapHealDamage = attackDamageData.attackerSwapHealDamage;
        turn.defenderSwapHealDamage = attackDamageData.defenderSwapHealDamage;
        turn.attackerRestHp = attackDamageData.hp.attackerHp;
        turn.defenderRestHp = attackDamageData.hp.defenderHp;
        turn.attackerUseSkillEffects = attackDamageData.attackerUseSkillEffects;
        turn.defenderUseSkillEffects = attackDamageData.defenderUseSkillEffects;
        turn.invokeSkillExtraInfo = attackDamageData.invokeSkillExtraInfo;
        turn.damageShareUnit = attackDamageData.damageShareUnit;
        turn.damageShareDamage = attackDamageData.damageShareDamage;
        turn.damageShareSkillEffect = attackDamageData.damageShareSkillEffect;
        BattleFuncs.recordInvokeDuelSkillEffectIds(attack, defense, turn, turn.isHit, attackDamageData.duelSkillProc.invokeAttackerDuelSkillEffectIds, attackDamageData.duelSkillProc.invokeDefenderDuelSkillEffectIds);
        List<BL.Skill> givePassiveSkills = new List<BL.Skill>();
        List<BL.Skill> giveAttackerAilmentSkills = new List<BL.Skill>();
        List<BL.Skill> giveDefenderAilmentSkills = new List<BL.Skill>();
        BattleFuncs.assortOneAttackInvest(attack, defense, battleDuelSkill1, attackDamageData.duelSkillProc, skillEffectListUnitList1, intList, skillEffectListUnitList2, turnInvestFromSkillIds, givePassiveSkills, giveAttackerAilmentSkills, giveDefenderAilmentSkills, turn.isHit, 0, turns, useResistEffects);
        if (giveDefenderAilmentSkills.Count > 0)
          turn.invokeAilmentSkills = giveDefenderAilmentSkills.ToArray();
        if (giveAttackerAilmentSkills.Count > 0)
          skillArray = giveAttackerAilmentSkills.ToArray();
        if (givePassiveSkills.Count > 0)
          turn.invokeGiveSkills = givePassiveSkills.ToArray();
      }
      BattleFuncs.recordInvokeDuelSkillEffectIds(attack, defense, turn, turn.isHit, battleDuelSkill1.invokeAttackerDuelSkillEffectIds, battleDuelSkill1.invokeDefenderDuelSkillEffectIds);
      turn.attackerCombiUnit = battleDuelSkill1.attackerCombiUnit;
      if (!hasValue)
      {
        BattleDuelSkill battleDuelSkill2 = BattleDuelSkill.invokeAilmentSkills(attack, attackStatus, defense, turn.isHit, isAttacker ? hp.attackerIsDontUseSkill : hp.defenderIsDontUseSkill, random, isAI, colosseumTurn, hp, attackPanel, defensePanel, isAttacker ? hp.attackerHp : hp.defenderHp, isAttacker ? hp.defenderHp : hp.attackerHp);
        List<BL.Skill> skillList3 = new List<BL.Skill>();
        if (turn.isHit)
        {
          BL.Skill[] investSkills = battleDuelSkill2.getInvestSkills(defense, true, false, 0);
          if (investSkills != null)
            skillList3.AddRange((IEnumerable<BL.Skill>) investSkills);
        }
        BL.Skill[] investSkills1 = battleDuelSkill2.getInvestSkills(defense, true, true, 0);
        if (investSkills1 != null)
          skillList3.AddRange((IEnumerable<BL.Skill>) investSkills1);
        if (skillList3.Count > 0)
          turn.invokeAilmentSkills = turn.invokeAilmentSkills != null ? ((IEnumerable<BL.Skill>) turn.invokeAilmentSkills).Concat<BL.Skill>((IEnumerable<BL.Skill>) skillList3.ToArray()).ToArray<BL.Skill>() : skillList3.ToArray();
        List<BL.Skill> skillList4 = new List<BL.Skill>();
        if (turn.isHit)
        {
          BL.Skill[] investSkills2 = battleDuelSkill2.getInvestSkills(attack, true, false, 0);
          if (investSkills2 != null)
            skillList4.AddRange((IEnumerable<BL.Skill>) investSkills2);
        }
        BL.Skill[] investSkills3 = battleDuelSkill2.getInvestSkills(attack, true, true, 0);
        if (investSkills3 != null)
          skillList4.AddRange((IEnumerable<BL.Skill>) investSkills3);
        if (skillList4.Count > 0)
          skillArray = skillArray != null ? ((IEnumerable<BL.Skill>) skillArray).Concat<BL.Skill>((IEnumerable<BL.Skill>) skillList4.ToArray()).ToArray<BL.Skill>() : skillList4.ToArray();
        if (turn.isHit)
        {
          Tuple<BL.ISkillEffectListUnit[], int[], BL.ISkillEffectListUnit[], int[], BL.ISkillEffectListUnit[], BL.SkillEffect[]> allInvestList = battleDuelSkill2.getAllInvestList(true, false, 0);
          if (allInvestList != null)
          {
            skillEffectListUnitList1.AddRange((IEnumerable<BL.ISkillEffectListUnit>) allInvestList.Item1);
            intList.AddRange((IEnumerable<int>) allInvestList.Item2);
            skillEffectListUnitList2.AddRange((IEnumerable<BL.ISkillEffectListUnit>) allInvestList.Item3);
            turnInvestFromSkillIds.AddRange((IEnumerable<int>) allInvestList.Item4);
            for (int index = 0; index < allInvestList.Item6.Length; ++index)
              useResistEffects[allInvestList.Item6[index]] = allInvestList.Item5[index];
          }
        }
        Tuple<BL.ISkillEffectListUnit[], int[], BL.ISkillEffectListUnit[], int[], BL.ISkillEffectListUnit[], BL.SkillEffect[]> allInvestList1 = battleDuelSkill2.getAllInvestList(true, true, 0);
        if (allInvestList1 != null)
        {
          skillEffectListUnitList1.AddRange((IEnumerable<BL.ISkillEffectListUnit>) allInvestList1.Item1);
          intList.AddRange((IEnumerable<int>) allInvestList1.Item2);
          skillEffectListUnitList2.AddRange((IEnumerable<BL.ISkillEffectListUnit>) allInvestList1.Item3);
          turnInvestFromSkillIds.AddRange((IEnumerable<int>) allInvestList1.Item4);
          for (int index = 0; index < allInvestList1.Item6.Length; ++index)
            useResistEffects[allInvestList1.Item6[index]] = allInvestList1.Item5[index];
        }
        List<BL.Skill> second2 = new List<BL.Skill>();
        List<BL.ISkillEffectListUnit> turnInvestUnit = new List<BL.ISkillEffectListUnit>((IEnumerable<BL.ISkillEffectListUnit>) skillEffectListUnitList1);
        List<int> turnInvestSkillIds = new List<int>((IEnumerable<int>) intList);
        List<BL.ISkillEffectListUnit> turnInvestFrom = new List<BL.ISkillEffectListUnit>((IEnumerable<BL.ISkillEffectListUnit>) skillEffectListUnitList2);
        if (turn.isHit)
        {
          BL.Skill[] investSkills4 = battleDuelSkill2.getInvestSkills(attack, false, false, 0, turns, turnInvestUnit, turnInvestSkillIds, turnInvestFrom);
          if (investSkills4 != null)
            second2.AddRange((IEnumerable<BL.Skill>) investSkills4);
          BL.Skill[] investSkills5 = battleDuelSkill2.getInvestSkills(defense, false, false, 0, turns, turnInvestUnit, turnInvestSkillIds, turnInvestFrom);
          if (investSkills5 != null)
            second2.AddRange((IEnumerable<BL.Skill>) investSkills5);
          Tuple<BL.ISkillEffectListUnit[], int[], BL.ISkillEffectListUnit[], int[], BL.ISkillEffectListUnit[], BL.SkillEffect[]> allInvestList2 = battleDuelSkill2.getAllInvestList(false, false, 0, turns, turnInvestUnit, turnInvestSkillIds, turnInvestFrom);
          if (allInvestList2 != null)
          {
            skillEffectListUnitList1.AddRange((IEnumerable<BL.ISkillEffectListUnit>) allInvestList2.Item1);
            intList.AddRange((IEnumerable<int>) allInvestList2.Item2);
            skillEffectListUnitList2.AddRange((IEnumerable<BL.ISkillEffectListUnit>) allInvestList2.Item3);
            turnInvestFromSkillIds.AddRange((IEnumerable<int>) allInvestList2.Item4);
          }
        }
        BL.Skill[] investSkills6 = battleDuelSkill2.getInvestSkills(attack, false, true, 0, turns, turnInvestUnit, turnInvestSkillIds, turnInvestFrom);
        if (investSkills6 != null)
          second2.AddRange((IEnumerable<BL.Skill>) investSkills6);
        BL.Skill[] investSkills7 = battleDuelSkill2.getInvestSkills(defense, false, true, 0, turns, turnInvestUnit, turnInvestSkillIds, turnInvestFrom);
        if (investSkills7 != null)
          second2.AddRange((IEnumerable<BL.Skill>) investSkills7);
        Tuple<BL.ISkillEffectListUnit[], int[], BL.ISkillEffectListUnit[], int[], BL.ISkillEffectListUnit[], BL.SkillEffect[]> allInvestList3 = battleDuelSkill2.getAllInvestList(false, true, 0, turns, turnInvestUnit, turnInvestSkillIds, turnInvestFrom);
        if (allInvestList3 != null)
        {
          skillEffectListUnitList1.AddRange((IEnumerable<BL.ISkillEffectListUnit>) allInvestList3.Item1);
          intList.AddRange((IEnumerable<int>) allInvestList3.Item2);
          skillEffectListUnitList2.AddRange((IEnumerable<BL.ISkillEffectListUnit>) allInvestList3.Item3);
          turnInvestFromSkillIds.AddRange((IEnumerable<int>) allInvestList3.Item4);
        }
        if (second2.Count > 0)
          turn.invokeGiveSkills = turn.invokeGiveSkills == null ? second2.ToArray() : ((IEnumerable<BL.Skill>) turn.invokeGiveSkills).Concat<BL.Skill>((IEnumerable<BL.Skill>) second2).ToArray<BL.Skill>();
        turn.invokeDuelSkills = ((IEnumerable<BL.Skill>) turn.invokeDuelSkills).Concat<BL.Skill>((IEnumerable<BL.Skill>) battleDuelSkill2.attackerSkills).ToArray<BL.Skill>();
        if (turn.invokeAilmentSkills != null)
        {
          if (turn.dispDamage >= 1)
            battleskillSkills = battleskillSkills.Where<BattleskillSkill>((Func<BattleskillSkill, bool>) (x => ((IEnumerable<BattleskillEffect>) x.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (y => y.EffectLogic.Enum != BattleskillEffectLogicEnum.sleep))));
          IEnumerable<BattleskillSkill> source = ((IEnumerable<BL.Skill>) turn.invokeAilmentSkills).Select<BL.Skill, BattleskillSkill>((Func<BL.Skill, BattleskillSkill>) (x => x.skill)).Except<BattleskillSkill>(battleskillSkills);
          turn.ailmentEffects = source.Select<BattleskillSkill, BattleskillAilmentEffect>((Func<BattleskillSkill, BattleskillAilmentEffect>) (x => x.ailment_effect)).ToArray<BattleskillAilmentEffect>();
          if (isAttacker)
          {
            hp.defenderIsDontAction |= BL.Skill.HasDontActionEffect(turn.invokeAilmentSkills);
            hp.defenderIsDontEvasion |= BL.Skill.HasDontEvasionEffect(turn.invokeAilmentSkills);
            hp.defenderIsDontUseSkill |= BL.Skill.HasDontActionEffect(turn.invokeAilmentSkills);
          }
          else
          {
            hp.attackerIsDontAction |= BL.Skill.HasDontActionEffect(turn.invokeAilmentSkills);
            hp.attackerIsDontEvasion |= BL.Skill.HasDontEvasionEffect(turn.invokeAilmentSkills);
            hp.attackerIsDontUseSkill |= BL.Skill.HasDontActionEffect(turn.invokeAilmentSkills);
          }
        }
        if (skillArray != null)
        {
          IEnumerable<BattleskillSkill> source = ((IEnumerable<BL.Skill>) skillArray).Select<BL.Skill, BattleskillSkill>((Func<BL.Skill, BattleskillSkill>) (x => x.skill)).Except<BattleskillSkill>(second1);
          turn.attackerAilmentEffects = source.Select<BattleskillSkill, BattleskillAilmentEffect>((Func<BattleskillSkill, BattleskillAilmentEffect>) (x => x.ailment_effect)).ToArray<BattleskillAilmentEffect>();
          if (!isAttacker)
          {
            hp.defenderIsDontAction |= BL.Skill.HasDontActionEffect(skillArray);
            hp.defenderIsDontEvasion |= BL.Skill.HasDontEvasionEffect(skillArray);
            hp.defenderIsDontUseSkill |= BL.Skill.HasDontActionEffect(skillArray);
          }
          else
          {
            hp.attackerIsDontAction |= BL.Skill.HasDontActionEffect(skillArray);
            hp.attackerIsDontEvasion |= BL.Skill.HasDontEvasionEffect(skillArray);
            hp.attackerIsDontUseSkill |= BL.Skill.HasDontActionEffect(skillArray);
          }
        }
      }
      turn.investUnit = skillEffectListUnitList1.ToArray();
      turn.investSkillIds = intList.ToArray();
      turn.investFrom = skillEffectListUnitList2.ToArray();
      turn.investFromSkillIds = turnInvestFromSkillIds.ToArray();
      foreach (KeyValuePair<BL.SkillEffect, BL.ISkillEffectListUnit> keyValuePair in useResistEffects)
      {
        BL.SkillEffect key1 = keyValuePair.Key;
        BL.ISkillEffectListUnit key2 = keyValuePair.Value;
        if (!hp.otherHp.ContainsKey(key2))
          hp.otherHp[key2] = new TurnOtherHp(key2.hp);
        hp.otherHp[key2].consumeSkillEffect.Add(key1);
        turn.useSkillUnit.Add(key2);
        turn.useSkillEffect.Add(BL.UseSkillEffect.Create(key1, BL.UseSkillEffect.Type.Decrement));
      }
      foreach (var data in ((IEnumerable<BL.ISkillEffectListUnit>) turn.investUnit).Select((unit, index) => new
      {
        unit = unit,
        index = index
      }))
      {
        BattleskillEffectLogicEnum[] source = new BattleskillEffectLogicEnum[3]
        {
          BattleskillEffectLogicEnum.white_night,
          BattleskillEffectLogicEnum.seal,
          BattleskillEffectLogicEnum.heal_impossible
        };
        if (data.unit == attack || data.unit == defense)
        {
          BL.Skill skill = new BL.Skill()
          {
            id = turn.investSkillIds[data.index]
          };
          if (!hasValue || skill.skill.skill_type != BattleskillSkillType.ailment)
          {
            if (skill.skill.target_type == BattleskillTargetType.complex_single || skill.skill.target_type == BattleskillTargetType.complex_range)
            {
              foreach (BattleskillEffect effect in skill.skill.Effects)
              {
                if (((IEnumerable<BattleskillEffectLogicEnum>) source).Contains<BattleskillEffectLogicEnum>(effect.EffectLogic.Enum))
                {
                  if (!effect.is_targer_enemy)
                  {
                    if (data.unit == attack)
                      data.unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, skill.skill, 1));
                  }
                  else if (data.unit == defense)
                    data.unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, skill.skill, 1));
                }
              }
            }
            else
            {
              foreach (BattleskillEffect effect in skill.skill.Effects)
              {
                if (((IEnumerable<BattleskillEffectLogicEnum>) source).Contains<BattleskillEffectLogicEnum>(effect.EffectLogic.Enum))
                  data.unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, skill.skill, 1));
              }
            }
          }
        }
      }
      turnsTemp.Add(turn);
    }

    public static int attackCount(BL.ISkillEffectListUnit attack, BL.ISkillEffectListUnit defense)
    {
      if (BattleFuncs.isSkillsAndEffectsInvalid(attack, defense))
        return 1;
      int num = BattleFuncs.gearSkillEffectFilter(attack.originalUnit, attack.skillEffects.Where(BattleskillEffectLogicEnum.multiple_attack, (Func<BL.SkillEffect, bool>) (x =>
      {
        BattleskillEffect effect = x.effect;
        if (effect.HasKey(BattleskillEffectLogicArgumentEnum.gear_kind_id) && effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != attack.originalUnit.unit.kind.ID || effect.HasKey(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) && effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != defense.originalUnit.unit.kind.ID || effect.HasKey(BattleskillEffectLogicArgumentEnum.element) && effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != attack.originalUnit.playerUnit.GetElement() || effect.HasKey(BattleskillEffectLogicArgumentEnum.target_element) && effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != defense.originalUnit.playerUnit.GetElement() || effect.HasKey(BattleskillEffectLogicArgumentEnum.job_id) && effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) != attack.originalUnit.job.ID || effect.HasKey(BattleskillEffectLogicArgumentEnum.target_job_id) && effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) != defense.originalUnit.job.ID || effect.HasKey(BattleskillEffectLogicArgumentEnum.family_id) && effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) != 0 && !attack.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id)))
          return false;
        return !effect.HasKey(BattleskillEffectLogicArgumentEnum.target_family_id) || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) == 0 || defense.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id));
      }))).Sum<BL.SkillEffect>((Func<BL.SkillEffect, int>) (x => x.effect.GetInt(BattleskillEffectLogicArgumentEnum.value)));
      return num <= 0 ? 1 : num;
    }

    public static bool canOneMore(
      Judgement.BeforeDuelUnitParameter attack,
      Judgement.BeforeDuelUnitParameter defense,
      BL.ISkillEffectListUnit myself,
      BL.ISkillEffectListUnit enemy,
      bool isAttacker,
      bool isInvokedAmbush = false,
      XorShift random = null,
      AttackStatus attackStatus = null,
      AttackStatus defenseStatus = null,
      int myselfHp = 0,
      int enemyHp = 0,
      int? colosseumTurn = null,
      bool colosseumIsSample = false,
      BL.Panel attackPanel = null,
      BL.Panel defensePanel = null)
    {
      bool isAI = myself is BL.AIUnit;
      Func<BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, int, BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, bool> func = (Func<BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, int, BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, bool>) ((effectUnit, targetUnit, effect_target, unit, target) => effectUnit.skillEffects.Where(BattleskillEffectLogicEnum.defensive_formation, (Func<BL.SkillEffect, bool>) (x =>
      {
        BattleskillEffect effect = x.effect;
        if ((double) effect.GetFloat(BattleskillEffectLogicArgumentEnum.effect_target) != (double) effect_target || BattleFuncs.isSealedSkillEffect(effectUnit, x) || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != effectUnit.originalUnit.unit.kind.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != targetUnit.originalUnit.unit.kind.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != effectUnit.originalUnit.playerUnit.GetElement() || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != targetUnit.originalUnit.playerUnit.GetElement() || effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) != effectUnit.originalUnit.job.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) != targetUnit.originalUnit.job.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) != 0 && !effectUnit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id)) || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) != 0 && !targetUnit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id)) || effectUnit == target && BattleFuncs.isSkillsAndEffectsInvalid(target, unit) || BattleFuncs.isEffectEnemyRangeAndInvalid(x, unit, target) || BattleFuncs.isSkillsAndEffectsInvalid(unit, target))
          return false;
        BattleFuncs.PackedSkillEffect packedSkillEffect = BattleFuncs.PackedSkillEffect.Create(x);
        BL.Panel panel = (BL.Panel) null;
        if (effect_target == 0 && attackPanel != null)
          panel = attackPanel;
        else if (effect_target == 1 && defensePanel != null)
          panel = defensePanel;
        if (!packedSkillEffect.CheckLandTag(panel, isAI))
          return false;
        if (!packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.percentage_invocation))
          return true;
        float percentage_invocation = packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invocation);
        if ((double) percentage_invocation >= 200.0)
          return true;
        return random != null && BattleFuncs.isInvoke(effectUnit, targetUnit, effect_target == 0 ? attack : defense, effect_target == 0 ? defense : attack, effect_target == 0 ? attackStatus : defenseStatus, effect_target == 0 ? defenseStatus : attackStatus, x.baseSkillLevel, percentage_invocation, random, false, effect_target == 0 ? myselfHp : enemyHp, effect_target == 0 ? enemyHp : myselfHp, colosseumTurn, base_invocation: packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.base_invocation) ? packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.base_invocation) : 0.0f, invocation_skill_ratio: packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.invocation_skill_ratio) ? packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.invocation_skill_ratio) : 1f, invocation_luck_ratio: packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.invocation_luck_ratio) ? packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.invocation_luck_ratio) : 1f);
      })).Any<BL.SkillEffect>());
      bool flag1 = attack.AttackSpeed - defense.AttackSpeed >= 5;
      if (!flag1)
      {
        IEnumerable<BL.SkillEffect> source = BattleFuncs.gearSkillEffectFilter(myself.originalUnit, myself.skillEffects.Where(BattleskillEffectLogicEnum.absolute_one_more_attack, (Func<BL.SkillEffect, bool>) (x =>
        {
          BattleskillEffect effect = x.effect;
          if (!colosseumIsSample)
          {
            bool flag2 = isInvokedAmbush ? !isAttacker : isAttacker;
            if (effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) == 1 && !flag2 || effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) == 2 & flag2)
              return false;
          }
          else if (effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) != 0)
            return false;
          return !BattleFuncs.isSealedSkillEffect(myself, x) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == enemy.originalUnit.playerUnit.GetElement()) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == myself.originalUnit.playerUnit.GetElement()) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == enemy.originalUnit.unit.kind.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == myself.originalUnit.unit.kind.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) == 0 || enemy.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id))) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || myself.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id))) && BattleFuncs.PackedSkillEffect.Create(x).CheckLandTag(attackPanel, isAI) && !BattleFuncs.isEffectEnemyRangeAndInvalid(x, myself, enemy) && !BattleFuncs.isSkillsAndEffectsInvalid(myself, enemy);
        })));
        flag1 |= source.Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x =>
        {
          BattleskillEffect effect = x.effect;
          float percentage_invocation = x.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invocation);
          if ((double) percentage_invocation >= 200.0)
            return true;
          if (random == null)
            return false;
          BattleFuncs.PackedSkillEffect packedSkillEffect = BattleFuncs.PackedSkillEffect.Create(x);
          return BattleFuncs.isInvoke(myself, enemy, attack, defense, attackStatus, defenseStatus, x.baseSkillLevel, percentage_invocation, random, false, myselfHp, enemyHp, colosseumTurn, base_invocation: packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.base_invocation) ? packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.base_invocation) : 0.0f, invocation_skill_ratio: packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.invocation_skill_ratio) ? packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.invocation_skill_ratio) : 1f, invocation_luck_ratio: packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.invocation_luck_ratio) ? packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.invocation_luck_ratio) : 1f);
        }));
      }
      return flag1 && !func(myself, enemy, 0, myself, enemy) && !func(enemy, myself, 1, myself, enemy);
    }

    public static int calcPercentageDamage(int preHp, float percentageDamageRate, int maxDamage)
    {
      int num;
      if (preHp <= 1)
      {
        num = preHp;
      }
      else
      {
        num = Mathf.CeilToInt((float) ((Decimal) preHp * (Decimal) percentageDamageRate));
        if (maxDamage != 0 && num > maxDamage)
          num = maxDamage;
        if (preHp - num < 1)
          num = preHp - 1;
      }
      return num;
    }

    public static bool isInvokedDefenderSkillLogic(
      IEnumerable<BL.DuelTurn> turns,
      BattleskillEffectLogicEnum skillLogic,
      bool? isAttacker = null)
    {
      return turns.Any<BL.DuelTurn>((Func<BL.DuelTurn, bool>) (x =>
      {
        if (isAttacker.HasValue)
        {
          int num1 = x.isAtackker ? 1 : 0;
          bool? nullable = isAttacker;
          int num2 = nullable.GetValueOrDefault() ? 1 : 0;
          if (!(num1 == num2 & nullable.HasValue))
            return false;
        }
        return ((IEnumerable<BL.Skill>) x.invokeDefenderDuelSkills).Any<BL.Skill>((Func<BL.Skill, bool>) (y => ((IEnumerable<BattleskillEffect>) y.skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (z => z.EffectLogic.Enum == skillLogic))));
      }));
    }

    public static bool isInvokedDefenderConsumeSkillLogic(
      IEnumerable<BL.DuelTurn> turns,
      BattleskillEffectLogicEnum skillLogic,
      bool? isAttacker = null)
    {
      return turns.Any<BL.DuelTurn>((Func<BL.DuelTurn, bool>) (x =>
      {
        if (isAttacker.HasValue)
        {
          int num1 = x.isAtackker ? 1 : 0;
          bool? nullable = isAttacker;
          int num2 = nullable.GetValueOrDefault() ? 1 : 0;
          if (!(num1 == num2 & nullable.HasValue))
            return false;
        }
        return x.defenderUseSkillEffects.Any<BL.UseSkillEffect>((Func<BL.UseSkillEffect, bool>) (y => MasterData.BattleskillEffect[y.effectEffectId].EffectLogic.Enum == skillLogic));
      }));
    }

    private static void assortOneAttackInvest(
      BL.ISkillEffectListUnit attack,
      BL.ISkillEffectListUnit defense,
      BattleDuelSkill biAttackSkills,
      BattleDuelSkill duelSkillProc,
      List<BL.ISkillEffectListUnit> turnInvestUnit,
      List<int> turnInvestSkillIds,
      List<BL.ISkillEffectListUnit> turnInvestFrom,
      List<int> turnInvestFromSkillIds,
      List<BL.Skill> givePassiveSkills,
      List<BL.Skill> giveAttackerAilmentSkills,
      List<BL.Skill> giveDefenderAilmentSkills,
      bool isHit,
      int attackNo,
      List<BL.DuelTurn> turns,
      Dictionary<BL.SkillEffect, BL.ISkillEffectListUnit> useResistEffects)
    {
      List<BL.ISkillEffectListUnit> turnInvestUnitBk = new List<BL.ISkillEffectListUnit>((IEnumerable<BL.ISkillEffectListUnit>) turnInvestUnit);
      List<int> turnInvestSkillIdsBk = new List<int>((IEnumerable<int>) turnInvestSkillIds);
      List<BL.ISkillEffectListUnit> turnInvestFromBk = new List<BL.ISkillEffectListUnit>((IEnumerable<BL.ISkillEffectListUnit>) turnInvestFrom);
      Action<bool> action = (Action<bool>) (isUnconditional =>
      {
        BL.Skill[] investSkills1 = biAttackSkills.getInvestSkills(attack, true, isUnconditional, attackNo, turns, turnInvestUnitBk, turnInvestSkillIdsBk, turnInvestFromBk);
        if (investSkills1 != null)
          giveAttackerAilmentSkills.AddRange((IEnumerable<BL.Skill>) investSkills1);
        BL.Skill[] investSkills2 = duelSkillProc.getInvestSkills(attack, true, isUnconditional, 0, turns, turnInvestUnitBk, turnInvestSkillIdsBk, turnInvestFromBk);
        if (investSkills2 != null)
          giveAttackerAilmentSkills.AddRange((IEnumerable<BL.Skill>) investSkills2);
        BL.Skill[] investSkills3 = biAttackSkills.getInvestSkills(defense, true, isUnconditional, attackNo, turns, turnInvestUnitBk, turnInvestSkillIdsBk, turnInvestFromBk);
        if (investSkills3 != null)
          giveDefenderAilmentSkills.AddRange((IEnumerable<BL.Skill>) investSkills3);
        BL.Skill[] investSkills4 = duelSkillProc.getInvestSkills(defense, true, isUnconditional, 0, turns, turnInvestUnitBk, turnInvestSkillIdsBk, turnInvestFromBk);
        if (investSkills4 != null)
          giveDefenderAilmentSkills.AddRange((IEnumerable<BL.Skill>) investSkills4);
        BL.Skill[] investSkills5 = biAttackSkills.getInvestSkills(attack, false, isUnconditional, attackNo, turns, turnInvestUnitBk, turnInvestSkillIdsBk, turnInvestFromBk);
        if (investSkills5 != null)
          givePassiveSkills.AddRange((IEnumerable<BL.Skill>) investSkills5);
        BL.Skill[] investSkills6 = duelSkillProc.getInvestSkills(attack, false, isUnconditional, 0, turns, turnInvestUnitBk, turnInvestSkillIdsBk, turnInvestFromBk);
        if (investSkills6 != null)
          givePassiveSkills.AddRange((IEnumerable<BL.Skill>) investSkills6);
        BL.Skill[] investSkills7 = biAttackSkills.getInvestSkills(defense, false, isUnconditional, attackNo, turns, turnInvestUnitBk, turnInvestSkillIdsBk, turnInvestFromBk);
        if (investSkills7 != null)
          givePassiveSkills.AddRange((IEnumerable<BL.Skill>) investSkills7);
        BL.Skill[] investSkills8 = duelSkillProc.getInvestSkills(defense, false, isUnconditional, 0, turns, turnInvestUnitBk, turnInvestSkillIdsBk, turnInvestFromBk);
        if (investSkills8 != null)
          givePassiveSkills.AddRange((IEnumerable<BL.Skill>) investSkills8);
        Tuple<BL.ISkillEffectListUnit[], int[], BL.ISkillEffectListUnit[], int[], BL.ISkillEffectListUnit[], BL.SkillEffect[]> allInvestList1 = biAttackSkills.getAllInvestList(true, isUnconditional, attackNo, turns, turnInvestUnitBk, turnInvestSkillIdsBk, turnInvestFromBk);
        if (allInvestList1 != null)
        {
          turnInvestUnit.AddRange((IEnumerable<BL.ISkillEffectListUnit>) allInvestList1.Item1);
          turnInvestSkillIds.AddRange((IEnumerable<int>) allInvestList1.Item2);
          turnInvestFrom.AddRange((IEnumerable<BL.ISkillEffectListUnit>) allInvestList1.Item3);
          turnInvestFromSkillIds.AddRange((IEnumerable<int>) allInvestList1.Item4);
          for (int index = 0; index < allInvestList1.Item6.Length; ++index)
            useResistEffects[allInvestList1.Item6[index]] = allInvestList1.Item5[index];
        }
        Tuple<BL.ISkillEffectListUnit[], int[], BL.ISkillEffectListUnit[], int[], BL.ISkillEffectListUnit[], BL.SkillEffect[]> allInvestList2 = duelSkillProc.getAllInvestList(true, isUnconditional, 0, turns, turnInvestUnitBk, turnInvestSkillIdsBk, turnInvestFromBk);
        if (allInvestList2 != null)
        {
          turnInvestUnit.AddRange((IEnumerable<BL.ISkillEffectListUnit>) allInvestList2.Item1);
          turnInvestSkillIds.AddRange((IEnumerable<int>) allInvestList2.Item2);
          turnInvestFrom.AddRange((IEnumerable<BL.ISkillEffectListUnit>) allInvestList2.Item3);
          turnInvestFromSkillIds.AddRange((IEnumerable<int>) allInvestList2.Item4);
          for (int index = 0; index < allInvestList2.Item6.Length; ++index)
            useResistEffects[allInvestList2.Item6[index]] = allInvestList2.Item5[index];
        }
        Tuple<BL.ISkillEffectListUnit[], int[], BL.ISkillEffectListUnit[], int[], BL.ISkillEffectListUnit[], BL.SkillEffect[]> allInvestList3 = biAttackSkills.getAllInvestList(false, isUnconditional, attackNo, turns, turnInvestUnitBk, turnInvestSkillIdsBk, turnInvestFromBk);
        if (allInvestList3 != null)
        {
          turnInvestUnit.AddRange((IEnumerable<BL.ISkillEffectListUnit>) allInvestList3.Item1);
          turnInvestSkillIds.AddRange((IEnumerable<int>) allInvestList3.Item2);
          turnInvestFrom.AddRange((IEnumerable<BL.ISkillEffectListUnit>) allInvestList3.Item3);
          turnInvestFromSkillIds.AddRange((IEnumerable<int>) allInvestList3.Item4);
        }
        Tuple<BL.ISkillEffectListUnit[], int[], BL.ISkillEffectListUnit[], int[], BL.ISkillEffectListUnit[], BL.SkillEffect[]> allInvestList4 = duelSkillProc.getAllInvestList(false, isUnconditional, 0, turns, turnInvestUnitBk, turnInvestSkillIdsBk, turnInvestFromBk);
        if (allInvestList4 == null)
          return;
        turnInvestUnit.AddRange((IEnumerable<BL.ISkillEffectListUnit>) allInvestList4.Item1);
        turnInvestSkillIds.AddRange((IEnumerable<int>) allInvestList4.Item2);
        turnInvestFrom.AddRange((IEnumerable<BL.ISkillEffectListUnit>) allInvestList4.Item3);
        turnInvestFromSkillIds.AddRange((IEnumerable<int>) allInvestList4.Item4);
      });
      if (isHit)
        action(false);
      action(true);
    }

    private static void recordInvokeDuelSkillEffectIds(
      BL.ISkillEffectListUnit attack,
      BL.ISkillEffectListUnit defense,
      BL.DuelTurn turn,
      bool isHit,
      List<int> invokeAttackerDuelSkillEffectIds,
      List<int> invokeDefenderDuelSkillEffectIds)
    {
      turn.invokeAttackerDuelSkillEffectIds.AddRange((IEnumerable<int>) invokeAttackerDuelSkillEffectIds);
      foreach (int duelSkillEffectId in invokeAttackerDuelSkillEffectIds)
        attack.skillEffects.AddDuelSkillEffectIdInvokeCount(duelSkillEffectId, 1);
      if (isHit)
      {
        turn.invokeDefenderDuelSkillEffectIds.AddRange((IEnumerable<int>) invokeDefenderDuelSkillEffectIds);
        foreach (int duelSkillEffectId in invokeDefenderDuelSkillEffectIds)
          defense.skillEffects.AddDuelSkillEffectIdInvokeCount(duelSkillEffectId, 1);
      }
      else
      {
        foreach (int duelSkillEffectId in invokeDefenderDuelSkillEffectIds)
        {
          if (MasterData.BattleskillEffect.ContainsKey(duelSkillEffectId))
          {
            BattleskillGenre? genre1 = MasterData.BattleskillEffect[duelSkillEffectId].skill.genre1;
            BattleskillGenre battleskillGenre = BattleskillGenre.defense;
            if (!(genre1.GetValueOrDefault() == battleskillGenre & genre1.HasValue))
            {
              turn.invokeDefenderDuelSkillEffectIds.Add(duelSkillEffectId);
              defense.skillEffects.AddDuelSkillEffectIdInvokeCount(duelSkillEffectId, 1);
            }
          }
        }
      }
    }

    private static BL env => BattleFuncs.environment.Get();

    public static BL.ForceID getForceID(BL.Unit unit) => BattleFuncs.env.getForceID(unit);

    public static BL.ForceID[] getForceIDArray(BL.ForceID id)
    {
      switch (id)
      {
        case BL.ForceID.player:
          return BattleFuncs.ForceIDArrayPlayer;
        case BL.ForceID.neutral:
          return BattleFuncs.ForceIDArrayNeutral;
        case BL.ForceID.enemy:
          return BattleFuncs.ForceIDArrayEnemy;
        default:
          return BattleFuncs.ForceIDArrayNone;
      }
    }

    public static BL.ForceID[] getForceIDArray(BL.Unit unit, BL _env)
    {
      return BattleFuncs.getForceIDArray(_env.getForceID(unit));
    }

    public static void setAttributePanels(
      IEnumerable<BL.Panel> panels,
      BL.PanelAttribute attribute,
      bool unset)
    {
      if (panels == null)
        return;
      foreach (BL.Panel panel in panels)
      {
        if (unset)
          panel.unsetAttribute(attribute);
        else
          panel.setAttribute(attribute);
      }
    }

    private static BL.Panel panelAdd(
      BL.Panel panel,
      int movement,
      HashSet<BL.Panel> pwl,
      HashSet<BL.Panel> limit)
    {
      if (limit != null && !limit.Contains(panel))
        return panel;
      panel.workMovement = movement;
      if (!pwl.Contains(panel))
        pwl.Add(panel);
      return panel;
    }

    private static bool checkMoveOK(
      int row,
      int column,
      int movement,
      BL.Unit unit,
      HashSet<BL.Panel> pwl,
      HashSet<BL.Panel> limit,
      bool isAI,
      bool isRebirth,
      bool enabledIgnoreMveCost)
    {
      BL.Panel fieldPanel = BattleFuncs.env.getFieldPanel(row, column);
      if (fieldPanel == null || limit != null && !limit.Contains(fieldPanel))
        return false;
      if (pwl.Contains(fieldPanel))
        return movement > fieldPanel.workMovement;
      return (isAI ? (BattleFuncs.env.isMoveOKAI(fieldPanel, unit, isRebirth, enabledIgnoreMveCost, movement) ? 1 : 0) : (BattleFuncs.env.isMoveOK(fieldPanel, unit, isRebirth, enabledIgnoreMveCost, movement) ? 1 : 0)) != 0;
    }

    private static void createMovePanelsFour(
      int row,
      int column,
      int movement,
      BL.Unit unit,
      HashSet<BL.Panel> pwl,
      HashSet<BL.Panel> limit,
      Queue<Tuple<int, int, int>> queue,
      bool isAI,
      bool isRebirth,
      bool enabledIgnoreMoveCost)
    {
      if (movement < 1)
        return;
      if (BattleFuncs.checkMoveOK(row - 1, column, movement, unit, pwl, limit, isAI, isRebirth, enabledIgnoreMoveCost))
        BattleFuncs.createMovePanelsSub(row - 1, column, movement, unit, pwl, limit, queue, enabledIgnoreMoveCost, isAI);
      if (BattleFuncs.checkMoveOK(row + 1, column, movement, unit, pwl, limit, isAI, isRebirth, enabledIgnoreMoveCost))
        BattleFuncs.createMovePanelsSub(row + 1, column, movement, unit, pwl, limit, queue, enabledIgnoreMoveCost, isAI);
      if (BattleFuncs.checkMoveOK(row, column - 1, movement, unit, pwl, limit, isAI, isRebirth, enabledIgnoreMoveCost))
        BattleFuncs.createMovePanelsSub(row, column - 1, movement, unit, pwl, limit, queue, enabledIgnoreMoveCost, isAI);
      if (!BattleFuncs.checkMoveOK(row, column + 1, movement, unit, pwl, limit, isAI, isRebirth, enabledIgnoreMoveCost))
        return;
      BattleFuncs.createMovePanelsSub(row, column + 1, movement, unit, pwl, limit, queue, enabledIgnoreMoveCost, isAI);
    }

    private static void createMovePanelsSub(
      int row,
      int column,
      int movement,
      BL.Unit unit,
      HashSet<BL.Panel> pwl,
      HashSet<BL.Panel> limit,
      Queue<Tuple<int, int, int>> queue,
      bool enabledIgnoreMoveCost,
      bool isAI)
    {
      BL.Panel fieldPanel = BattleFuncs.env.getFieldPanel(row, column);
      BattleFuncs.panelAdd(fieldPanel, movement, pwl, limit);
      if (fieldPanel.zocCheckp(unit, isAI, BattleFuncs.env))
        return;
      queue.Enqueue(Tuple.Create<int, int, int>(row, column, movement - BattleFuncs.getMoveCost(fieldPanel, unit, enabledIgnoreMoveCost)));
    }

    public static HashSet<BL.Panel> createMovePanels(
      int row,
      int column,
      int movement,
      BL.Unit unit,
      HashSet<BL.Panel> limit = null,
      bool isAI = false,
      bool isRebirth = false)
    {
      if (!isRebirth && unit.HasEnabledSkillEffect(BattleskillEffectLogicEnum.slip_thru))
        isRebirth = true;
      HashSet<BL.Panel> pwl = new HashSet<BL.Panel>();
      Queue<Tuple<int, int, int>> queue = new Queue<Tuple<int, int, int>>();
      bool enabledIgnoreMoveCost = unit.HasEnabledSkillEffect(BattleskillEffectLogicEnum.ignore_move_cost);
      using (BL.Unit unit1 = unit.enableCache())
      {
        BattleFuncs.panelAdd(BattleFuncs.env.getFieldPanel(row, column), movement, pwl, limit);
        BattleFuncs.createMovePanelsFour(row, column, movement, unit1, pwl, limit, queue, isAI, isRebirth, enabledIgnoreMoveCost);
        while (queue.Count != 0)
        {
          Tuple<int, int, int> tuple = queue.Dequeue();
          BattleFuncs.createMovePanelsFour(tuple.Item1, tuple.Item2, tuple.Item3, unit1, pwl, limit, queue, isAI, isRebirth, enabledIgnoreMoveCost);
        }
      }
      return pwl;
    }

    public static HashSet<BL.Panel> createMovePanels(BL.UnitPosition up)
    {
      return BattleFuncs.createMovePanels(up.originalRow, up.originalColumn, up.moveCost, up.unit);
    }

    public static BL.Panel getPanel(BL.UnitPosition up)
    {
      return BattleFuncs.env.getFieldPanel(up.originalRow, up.originalColumn);
    }

    public static BL.Panel getPanel(int row, int column)
    {
      return BattleFuncs.env.getFieldPanel(row, column);
    }

    private static void addEdge(
      int row,
      int column,
      IEnumerable<BL.Panel> panels,
      List<BattleFuncs.AsterEdge> edges,
      BL.Unit unit,
      int cost)
    {
      int to = 0;
      foreach (BL.Panel panel in panels)
      {
        if (row == panel.row && column == panel.column)
          edges.Add(new BattleFuncs.AsterEdge(to, cost));
        ++to;
      }
    }

    private static BattleFuncs.AsterEdge[] createEdges(
      BL.Panel panel,
      IEnumerable<BL.Panel> panels,
      BL.Unit unit,
      int moveCache,
      bool enabledIgnoreMoveCost)
    {
      List<BattleFuncs.AsterEdge> edges = new List<BattleFuncs.AsterEdge>();
      int moveCost = BattleFuncs.getMoveCost(panel, unit, enabledIgnoreMoveCost);
      int cost = moveCost > moveCache ? 10000 : moveCost;
      BattleFuncs.addEdge(panel.row - 1, panel.column, panels, edges, unit, cost);
      BattleFuncs.addEdge(panel.row + 1, panel.column, panels, edges, unit, cost);
      BattleFuncs.addEdge(panel.row, panel.column - 1, panels, edges, unit, cost);
      BattleFuncs.addEdge(panel.row, panel.column + 1, panels, edges, unit, cost);
      return edges.ToArray();
    }

    public static BattleFuncs.AsterNode[] createNodes(
      IEnumerable<BL.Panel> panels,
      BL.Unit unit,
      BL.Panel start,
      BL.Panel goal,
      out int startIdx,
      out int goalIdx,
      bool enabledIgnoreMoveCost)
    {
      List<BattleFuncs.AsterNode> asterNodeList = new List<BattleFuncs.AsterNode>();
      int no = 0;
      startIdx = goalIdx = -1;
      int moveCost = BattleFuncs.env.getUnitPosition(unit).moveCost;
      foreach (BL.Panel panel in panels)
      {
        asterNodeList.Add(new BattleFuncs.AsterNode(no, panel, BattleFuncs.createEdges(panel, panels, unit, moveCost, enabledIgnoreMoveCost)));
        if (panel == start)
          startIdx = no;
        if (panel == goal)
          goalIdx = no;
        ++no;
      }
      return asterNodeList.ToArray();
    }

    private static void aster(BattleFuncs.AsterNode[] nodes, int goal, int start)
    {
      BattleFuncs.AsterNode node1 = nodes[goal];
      BattleFuncs.AsterNode node2 = nodes[start];
      HashSet<BattleFuncs.AsterNode> asterNodeSet1 = new HashSet<BattleFuncs.AsterNode>();
      HashSet<BattleFuncs.AsterNode> asterNodeSet2 = new HashSet<BattleFuncs.AsterNode>();
      asterNodeSet1.Add(node2);
      node2.cost = BattleFuncs.heuristic(node1, node2);
      while (asterNodeSet1.Count != 0)
      {
        BattleFuncs.AsterNode n = (BattleFuncs.AsterNode) null;
        int num1 = 1000000000;
        foreach (BattleFuncs.AsterNode asterNode in asterNodeSet1)
        {
          if (asterNode.cost < num1)
          {
            n = asterNode;
            num1 = n.cost;
          }
        }
        if (n == node1)
          break;
        asterNodeSet2.Add(n);
        asterNodeSet1.Remove(n);
        for (int index = 0; index < n.edges.Length; ++index)
        {
          BattleFuncs.AsterNode node3 = nodes[n.edges[index].to];
          int num2 = n.cost - BattleFuncs.heuristic(node1, n);
          int num3 = BattleFuncs.heuristic(node1, node3);
          int cost = n.edges[index].cost;
          int num4 = num3;
          int num5 = num2 + num4 + cost;
          if (asterNodeSet1.Contains(node3))
          {
            if (num5 < node3.cost)
            {
              node3.cost = num5;
              node3.from = n.no;
            }
          }
          else if (asterNodeSet2.Contains(node3))
          {
            if (num5 < node3.cost)
            {
              node3.cost = num5;
              node3.from = n.no;
              asterNodeSet1.Add(node3);
              asterNodeSet2.Remove(node3);
            }
          }
          else
          {
            node3.cost = num5;
            node3.from = n.no;
            asterNodeSet1.Add(node3);
          }
        }
      }
    }

    private static int heuristic(BattleFuncs.AsterNode goal, BattleFuncs.AsterNode n)
    {
      int row1 = goal.panel.row;
      int column1 = goal.panel.column;
      int row2 = n.panel.row;
      int column2 = n.panel.column;
      return (row1 > row2 ? row1 - row2 : row2 - row1) + (column1 > column2 ? column1 - column2 : column2 - column1);
    }

    public static void getNodesStartAndGoal(
      BattleFuncs.AsterNode[] nodes,
      BL.Panel start,
      BL.Panel goal,
      out int startIdx,
      out int goalIdx)
    {
      startIdx = goalIdx = -1;
      for (int index = 0; index < nodes.Length; ++index)
      {
        if (nodes[index].panel == goal)
          goalIdx = index;
        if (nodes[index].panel == start)
          startIdx = index;
        if (startIdx != -1 && goalIdx != -1)
          break;
      }
    }

    public static List<BL.Panel> createRouteWithCost(
      BL.Unit unit,
      BattleFuncs.AsterNode[] nodes,
      int startIdx,
      int goalIdx,
      out int cost,
      bool enabledIgnoreMoveCost)
    {
      cost = 0;
      if (startIdx == -1 || goalIdx == -1)
        return new List<BL.Panel>();
      BattleFuncs.aster(nodes, goalIdx, startIdx);
      List<BL.Panel> routeWithCost = new List<BL.Panel>();
      BattleFuncs.AsterNode node;
      for (int index = goalIdx; index != startIdx; index = node.from)
      {
        node = nodes[index];
        cost += BattleFuncs.getMoveCost(node.panel, unit, enabledIgnoreMoveCost);
        routeWithCost.Add(node.panel);
      }
      routeWithCost.Add(nodes[startIdx].panel);
      return routeWithCost;
    }

    public static bool checkTargetp(
      BL.ISkillEffectListUnit su,
      BL.ForceID[] forceIds,
      BL.Unit.TargetAttribute ta,
      bool nonFacility = false,
      bool includeJumping = false)
    {
      if (!su.checkTargetAttribute(ta) || nonFacility && su.originalUnit.isFacility || !((IEnumerable<BL.ForceID>) forceIds).Contains<BL.ForceID>(BattleFuncs.env.getForceID(su.originalUnit)))
        return false;
      return includeJumping || !su.IsJumping;
    }

    public static List<BL.UnitPosition> getTargets(
      int r,
      int c,
      int[] range,
      BL.ForceID[] forceIds,
      BL.Unit.TargetAttribute ta,
      bool isAI = false,
      bool originalTarget = false,
      bool isDead = false,
      bool nonFacility = false,
      List<BL.Unit> searchTargets = null,
      bool includeJumping = false)
    {
      List<BL.UnitPosition> targets = new List<BL.UnitPosition>();
      if (range.Length < 1)
        return targets;
      Func<bool, BL.ISkillEffectListUnit, int, int, bool> func = (Func<bool, BL.ISkillEffectListUnit, int, int, bool>) ((dead, su, row, col) =>
      {
        if (isDead != dead || !BattleFuncs.checkTargetp(su, forceIds, ta, nonFacility, includeJumping))
          return false;
        int num = BL.fieldDistance(r, c, row, col);
        return num >= range[0] && num <= range[1] && (searchTargets == null || searchTargets.Contains(su.originalUnit));
      });
      if (isAI)
      {
        foreach (BL.AIUnit aiUnit in BattleFuncs.env.aiUnitPositions.value)
        {
          int num1 = originalTarget ? aiUnit.originalRow : aiUnit.row;
          int num2 = originalTarget ? aiUnit.originalColumn : aiUnit.column;
          if (func(aiUnit.isDead, (BL.ISkillEffectListUnit) aiUnit, num1, num2))
            targets.Add((BL.UnitPosition) aiUnit);
        }
      }
      else
      {
        foreach (BL.UnitPosition unitPosition in BattleFuncs.env.unitPositions.value)
        {
          if (unitPosition.unit.isEnable)
          {
            int num3 = originalTarget ? unitPosition.originalRow : unitPosition.row;
            int num4 = originalTarget ? unitPosition.originalColumn : unitPosition.column;
            if (func(unitPosition.unit.isDead, (BL.ISkillEffectListUnit) unitPosition.unit, num3, num4))
              targets.Add(unitPosition);
          }
        }
      }
      return targets;
    }

    public static Tuple<int, int> getUnitCell(BL.Unit unit, bool isAI = false, bool isOriginal = false)
    {
      int num1;
      int num2;
      if (isAI)
      {
        BL.AIUnit aiUnit = BattleFuncs.env.getAIUnit(unit);
        if (isOriginal)
        {
          num1 = aiUnit.unitPosition.originalRow;
          num2 = aiUnit.unitPosition.originalColumn;
        }
        else
        {
          num1 = aiUnit.row;
          num2 = aiUnit.column;
        }
      }
      else
      {
        BL.UnitPosition unitPosition = BattleFuncs.env.getUnitPosition(unit);
        if (isOriginal)
        {
          num1 = unitPosition.originalRow;
          num2 = unitPosition.originalColumn;
        }
        else
        {
          num1 = unitPosition.row;
          num2 = unitPosition.column;
        }
      }
      return new Tuple<int, int>(num1, num2);
    }

    public static List<BL.UnitPosition> getTargets(
      BL.Unit unit,
      int[] range,
      BL.ForceID[] forceIds,
      BL.Unit.TargetAttribute ta,
      bool isAI = false,
      bool isMineOriginal = false,
      bool isTargetsOriginal = false,
      bool nonFacility = false)
    {
      Tuple<int, int> unitCell = BattleFuncs.getUnitCell(unit, isAI, isMineOriginal);
      return BattleFuncs.getTargets(unitCell.Item1, unitCell.Item2, range, forceIds, ta, isAI, nonFacility: nonFacility);
    }

    public static List<BL.UnitPosition> getAttackTargets(
      BL.UnitPosition up,
      bool isOriginal = false,
      bool isAI = false,
      bool nonFacility = false)
    {
      if (up == null || up.unit == (BL.Unit) null)
        return new List<BL.UnitPosition>();
      int num1 = isOriginal ? up.originalRow : up.row;
      int num2 = isOriginal ? up.originalColumn : up.column;
      BL.Panel fieldPanel = BattleFuncs.env.getFieldPanel(num1, num2);
      BL.ISkillEffectListUnit iskillEffectListUnit = BattleFuncs.unitPositionToISkillEffectListUnit(up);
      return BattleFuncs.getTargets(num1, num2, iskillEffectListUnit.attackRange, BattleFuncs.env.getTargetForce(up.unit, iskillEffectListUnit.IsCharm), BL.Unit.TargetAttribute.attack, isAI, nonFacility: nonFacility, searchTargets: BattleFuncs.getProvokeUnits(iskillEffectListUnit, fieldPanel));
    }

    public static List<BL.UnitPosition> getHealTargets(
      BL.UnitPosition up,
      bool isOriginal = false,
      bool isAI = false,
      bool nonFacility = false)
    {
      if (up == null || up.unit == (BL.Unit) null)
        return new List<BL.UnitPosition>();
      int num1 = isOriginal ? up.originalRow : up.row;
      int num2 = isOriginal ? up.originalColumn : up.column;
      BL.Panel fieldPanel = BattleFuncs.env.getFieldPanel(num1, num2);
      BL.ISkillEffectListUnit iskillEffectListUnit = BattleFuncs.unitPositionToISkillEffectListUnit(up);
      return BattleFuncs.getTargets(num1, num2, iskillEffectListUnit.healRange, BattleFuncs.getForceIDArray(up.unit, BattleFuncs.env), BL.Unit.TargetAttribute.heal, isAI, nonFacility: nonFacility, searchTargets: BattleFuncs.getProvokeUnits(iskillEffectListUnit, fieldPanel));
    }

    public static HashSet<BL.Panel> getTargetPanels(
      int row,
      int column,
      int[] range,
      BL.ForceID[] forceIds,
      BL.Unit.TargetAttribute ta,
      bool isAI = false,
      BL.UnitPosition myUP = null,
      bool nonFacility = false,
      bool isDead = false,
      List<BL.Unit> searchTargets = null)
    {
      bool flag1 = myUP != null;
      int r = row;
      int c = column;
      int[] range1 = range;
      BL.ForceID[] forceIds1 = forceIds;
      int ta1 = (int) ta;
      int num1 = isAI ? 1 : 0;
      int num2 = flag1 ? 1 : 0;
      bool flag2 = nonFacility;
      int num3 = isDead ? 1 : 0;
      int num4 = flag2 ? 1 : 0;
      List<BL.Unit> searchTargets1 = searchTargets;
      List<BL.UnitPosition> targets = BattleFuncs.getTargets(r, c, range1, forceIds1, (BL.Unit.TargetAttribute) ta1, num1 != 0, num2 != 0, num3 != 0, num4 != 0, searchTargets1);
      HashSet<BL.Panel> targetPanels = new HashSet<BL.Panel>();
      foreach (BL.UnitPosition unitPosition in targets)
      {
        if (flag1)
        {
          if (unitPosition != myUP)
            targetPanels.Add(BattleFuncs.env.getFieldPanel(unitPosition.originalRow, unitPosition.originalColumn));
        }
        else
          targetPanels.Add(BattleFuncs.env.getFieldPanel(unitPosition.row, unitPosition.column));
      }
      return targetPanels;
    }

    public static HashSet<BL.Panel> getAttackTargetPanels(
      BL.UnitPosition up,
      bool isOriginal = false,
      bool isAI = false)
    {
      if (up.unit == (BL.Unit) null)
        return new HashSet<BL.Panel>();
      if (up.unit.IsDontAction)
        return new HashSet<BL.Panel>();
      BL.ISkillEffectListUnit iskillEffectListUnit = BattleFuncs.unitPositionToISkillEffectListUnit(up);
      int row = isOriginal ? up.originalRow : up.row;
      int column = isOriginal ? up.originalColumn : up.column;
      BL.Panel fieldPanel = BattleFuncs.env.getFieldPanel(row, column);
      Tuple<int, int> effectsAddRange = fieldPanel.getEffectsAddRange(up.unit);
      int[] attackRange = iskillEffectListUnit.attackRange;
      int[] range = new int[2]
      {
        attackRange[0] + effectsAddRange.Item1,
        attackRange[1] + effectsAddRange.Item2
      };
      return BattleFuncs.getTargetPanels(row, column, range, BattleFuncs.env.getTargetForce(up.unit, iskillEffectListUnit.IsCharm), BL.Unit.TargetAttribute.attack, isAI, searchTargets: BattleFuncs.getProvokeUnits(iskillEffectListUnit, fieldPanel));
    }

    public static HashSet<BL.Panel> getHealTargetPanels(
      BL.UnitPosition up,
      bool isOriginal = false,
      bool isAI = false)
    {
      if (up.unit == (BL.Unit) null)
        return new HashSet<BL.Panel>();
      if (up.unit.IsDontAction)
        return new HashSet<BL.Panel>();
      int row = isOriginal ? up.originalRow : up.row;
      int column = isOriginal ? up.originalColumn : up.column;
      BL.Panel fieldPanel = BattleFuncs.env.getFieldPanel(row, column);
      BL.ISkillEffectListUnit iskillEffectListUnit = BattleFuncs.unitPositionToISkillEffectListUnit(up);
      return BattleFuncs.getTargetPanels(row, column, iskillEffectListUnit.healRange, BattleFuncs.getForceIDArray(up.unit, BattleFuncs.env), BL.Unit.TargetAttribute.heal, isAI, up, searchTargets: BattleFuncs.getProvokeUnits(iskillEffectListUnit, fieldPanel));
    }

    public static List<BL.Panel> getRangePanels(int row, int column, int[] range)
    {
      if (range.Length == 0)
        return new List<BL.Panel>();
      BL env = BattleFuncs.env;
      List<BL.Panel> rangePanels = new List<BL.Panel>();
      for (int r2 = -range[1]; r2 <= range[1]; ++r2)
      {
        for (int c2 = -range[1]; c2 <= range[1]; ++c2)
        {
          BL.Panel fieldPanel = env.getFieldPanel(row + r2, column + c2);
          if (fieldPanel != null)
          {
            int num = BL.fieldDistance(0, 0, r2, c2);
            if (num >= range[0] && num <= range[1])
              rangePanels.Add(fieldPanel);
          }
        }
      }
      return rangePanels;
    }

    public static HashSet<BL.Panel> allMoveActionRangePanels_(
      BL.UnitPosition up,
      HashSet<BL.Panel> completePanels = null,
      bool isAI = false,
      bool isHeal = false,
      BL.Skill skill = null,
      HashSet<BL.Panel> positionPanels = null)
    {
      HashSet<BL.Panel> panelSet = new HashSet<BL.Panel>();
      positionPanels?.Clear();
      if (completePanels == null)
        completePanels = up.completePanels;
      BL.ForceID[] forceIds = (BL.ForceID[]) null;
      int[] range = new int[2];
      int[] numArray = (int[]) null;
      bool isDead = false;
      bool nonFacility = false;
      BL.Unit.TargetAttribute ta;
      if (skill != null)
      {
        if (skill.targetType == BattleskillTargetType.myself)
        {
          foreach (BL.Panel completePanel in completePanels)
          {
            if (!panelSet.Contains(completePanel))
              panelSet.Add(completePanel);
          }
          if (positionPanels != null)
          {
            foreach (BL.Panel completePanel in completePanels)
            {
              if (!positionPanels.Contains(completePanel))
                positionPanels.Add(completePanel);
            }
          }
          return panelSet;
        }
        ta = skill.targetAttribute;
        range = skill.range;
        if (positionPanels != null)
        {
          nonFacility = skill.nonFacility;
          isDead = skill.isDeadTargetOnly;
          forceIds = skill.getTargetForceIDs(BattleFuncs.env, (BL.ISkillEffectListUnit) up.unit);
        }
      }
      else if (isHeal)
      {
        ta = BL.Unit.TargetAttribute.heal;
        range = BattleFuncs.unitPositionToISkillEffectListUnit(up).healRange;
        if (positionPanels != null)
          forceIds = BattleFuncs.getForceIDArray(up.unit, BattleFuncs.env);
      }
      else
      {
        ta = BL.Unit.TargetAttribute.attack;
        BL.ISkillEffectListUnit iskillEffectListUnit = BattleFuncs.unitPositionToISkillEffectListUnit(up);
        numArray = iskillEffectListUnit.attackRange;
        if (positionPanels != null)
          forceIds = BattleFuncs.env.getTargetForce(up.unit, iskillEffectListUnit.IsCharm);
      }
      using (HashSet<BL.Panel>.Enumerator enumerator = completePanels.GetEnumerator())
      {
label_54:
        while (enumerator.MoveNext())
        {
          BL.Panel current = enumerator.Current;
          if (skill == null && ta == BL.Unit.TargetAttribute.attack)
          {
            Tuple<int, int> effectsAddRange = current.getEffectsAddRange(up.unit);
            range[0] = numArray[0] + effectsAddRange.Item1;
            range[1] = numArray[1] + effectsAddRange.Item2;
          }
          List<BL.Panel> rangePanels = BattleFuncs.getRangePanels(current.row, current.column, range);
          foreach (BL.Panel panel in rangePanels)
            panelSet.Add(panel);
          if (positionPanels != null)
          {
            foreach (BL.Panel panel in rangePanels)
            {
              if (isDead)
              {
                BL.UnitPosition[] unitPositionArray = isAI ? BattleFuncs.env.getFieldUnitsAI(panel, isDead: isDead) : BattleFuncs.env.getFieldUnits(panel, isDead: isDead);
                if (unitPositionArray != null)
                {
                  foreach (BL.UnitPosition unitPosition in unitPositionArray)
                  {
                    if (BattleFuncs.checkTargetp(unitPosition is BL.ISkillEffectListUnit ? unitPosition as BL.ISkillEffectListUnit : (BL.ISkillEffectListUnit) unitPosition.unit, forceIds, ta, nonFacility))
                    {
                      positionPanels.Add(current);
                      goto label_54;
                    }
                  }
                }
              }
              else
              {
                BL.UnitPosition[] unitPositionArray = isAI ? BattleFuncs.env.getFieldUnitsAI(panel) : BattleFuncs.env.getFieldUnits(panel);
                if (unitPositionArray != null)
                {
                  foreach (BL.UnitPosition unitPosition in unitPositionArray)
                  {
                    if (BattleFuncs.checkTargetp(isAI ? unitPosition as BL.ISkillEffectListUnit : (BL.ISkillEffectListUnit) unitPosition.unit, forceIds, ta, nonFacility) && (!isHeal || up.id != unitPosition.id))
                    {
                      positionPanels.Add(current);
                      goto label_54;
                    }
                  }
                }
              }
            }
          }
        }
      }
      return panelSet;
    }

    public static HashSet<BL.Panel> createDangerPanels<T>(IEnumerable<T> units) where T : BL.UnitPosition
    {
      HashSet<BL.Panel> dangerPanels = new HashSet<BL.Panel>();
      foreach (T unit in units)
      {
        foreach (BL.Panel actionRangePanel in unit.allMoveActionRangePanels)
          dangerPanels.Add(actionRangePanel);
      }
      return dangerPanels;
    }

    public static HashSet<BL.Panel> createDangerPanels(BL.ForceID byForce)
    {
      return BattleFuncs.createDangerPanels<BL.UnitPosition>((IEnumerable<BL.UnitPosition>) BattleFuncs.env.getActionUnits(byForce).value);
    }

    public static HashSet<BL.Panel> createDangerPanels(BL.ForceID[] byForces)
    {
      HashSet<BL.Panel> dangerPanels = new HashSet<BL.Panel>();
      foreach (BL.ForceID byForce in byForces)
      {
        foreach (BL.Panel dangerPanel in BattleFuncs.createDangerPanels(byForce))
          dangerPanels.Add(dangerPanel);
      }
      return dangerPanels;
    }

    public static HashSet<BL.Panel> moveCompletePanels_(
      HashSet<BL.Panel> panels,
      BL.Unit unit,
      bool isAI = false,
      bool isOriginal = true)
    {
      HashSet<BL.Panel> panelSet = new HashSet<BL.Panel>();
      foreach (BL.Panel panel in panels)
      {
        BL.UnitPosition[] source = isAI ? BattleFuncs.env.getFieldUnitsAI(panel, isOriginal, includeJumping: true) : BattleFuncs.env.getFieldUnits(panel.row, panel.column, isOriginal, includeJumping: true);
        if (source == null)
          panelSet.Add(panel);
        else if (!((IEnumerable<BL.UnitPosition>) source).Any<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (up => up != null && up.unit != unit && !up.unit.isPutOn)))
          panelSet.Add(panel);
      }
      return panelSet;
    }

    public static HashSet<BL.Panel> targetAttackPanels(
      HashSet<BL.Panel> panels,
      BL.Unit unit,
      BL.ForceID[] targetForce,
      bool isAI = false,
      bool nonFacility = false)
    {
      HashSet<BL.Panel> panelSet = new HashSet<BL.Panel>();
      foreach (BL.Panel panel in panels)
      {
        if (BattleFuncs.getTargets(panel.row, panel.column, unit.attackRange, targetForce, BL.Unit.TargetAttribute.attack, isAI, nonFacility: nonFacility).Count > 0)
          panelSet.Add(panel);
      }
      return panelSet;
    }

    public static List<BL.UnitPosition> getFourForceUnits(
      int row,
      int column,
      BL.ForceID[] targetForce,
      bool isAI = false)
    {
      List<BL.UnitPosition> fourForceUnits = new List<BL.UnitPosition>();
      BL.UnitPosition unitPosition1 = BattleFuncs.env.fieldForceUnit(row + 1, column, targetForce, isAI);
      if (unitPosition1 != null)
        fourForceUnits.Add(unitPosition1);
      BL.UnitPosition unitPosition2 = BattleFuncs.env.fieldForceUnit(row - 1, column, targetForce, isAI);
      if (unitPosition2 != null)
        fourForceUnits.Add(unitPosition2);
      BL.UnitPosition unitPosition3 = BattleFuncs.env.fieldForceUnit(row, column - 1, targetForce, isAI);
      if (unitPosition3 != null)
        fourForceUnits.Add(unitPosition3);
      BL.UnitPosition unitPosition4 = BattleFuncs.env.fieldForceUnit(row, column + 1, targetForce, isAI);
      if (unitPosition4 != null)
        fourForceUnits.Add(unitPosition4);
      return fourForceUnits;
    }

    public static List<BL.UnitPosition> getForceUnitsWithinRange(
      int row,
      int column,
      int range,
      BL.ForceID[] targetForce,
      bool isAI = false)
    {
      List<BL.UnitPosition> unitsWithinRange = new List<BL.UnitPosition>();
      BL env = BattleFuncs.env;
      for (int index1 = -range; index1 <= range; ++index1)
      {
        int num1 = row + index1;
        for (int index2 = -range; index2 <= range; ++index2)
        {
          int num2 = column + index2;
          if (BL.fieldDistance(row, column, num1, num2) <= range)
          {
            BL.UnitPosition unitPosition = env.fieldForceUnit(num1, num2, targetForce, isAI);
            if (unitPosition != null)
              unitsWithinRange.Add(unitPosition);
          }
        }
      }
      return unitsWithinRange;
    }

    public static List<BL.UnitPosition> getNeighbors(BL.UnitPosition up, bool isAI = false)
    {
      return BattleFuncs.getFourForceUnits(up.row, up.column, BattleFuncs.getForceIDArray(up.unit, BattleFuncs.env), isAI);
    }

    public static int getMoveCost(
      BL.Panel panel,
      UnitMoveType moveType,
      bool enabledIgnoreMoveCost)
    {
      int moveCost = panel.landform.GetIncr(moveType).move_cost;
      return enabledIgnoreMoveCost && moveCost <= 10 ? 1 : moveCost;
    }

    public static int getMoveCost(BL.Panel panel, BL.Unit unit, bool enabledIgnoreMoveCost)
    {
      return BattleFuncs.getMoveCost(panel, unit.job.move_type, enabledIgnoreMoveCost);
    }

    public static IEnumerable<BL.SkillEffect> getAilmentResistEffects(
      int skillId,
      BL.ISkillEffectListUnit target,
      BL.ISkillEffectListUnit unit,
      int? targetHp = null,
      int? unitHp = null,
      int? colosseumTurn = null)
    {
      if (MasterData.BattleskillSkill.ContainsKey(skillId))
      {
        BattleskillSkill skill = MasterData.BattleskillSkill[skillId];
        IEnumerable<BL.SkillEffect> source = target.skillEffects.Where(BattleskillEffectLogicEnum.change_invest_skilleffect);
        bool isColosseum = colosseumTurn.HasValue;
        foreach (BL.SkillEffect ailmentResistEffect in BattleFuncs.gearSkillEffectFilter(target.originalUnit, source.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x =>
        {
          if (x.useRemain.HasValue)
          {
            int? useRemain = x.useRemain;
            int num = 0;
            if (useRemain.GetValueOrDefault() <= num & useRemain.HasValue)
              return false;
          }
          List<Tuple<int, int>> logics = new List<Tuple<int, int>>(x.effect.GetPackedSkillEffect().GetEffects().Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (y => y.HasKey(BattleskillEffectLogicArgumentEnum.effect_logic))).Select<BattleskillEffect, Tuple<int, int>>((Func<BattleskillEffect, Tuple<int, int>>) (y => Tuple.Create<int, int>(y.GetInt(BattleskillEffectLogicArgumentEnum.effect_logic), y.GetInt(BattleskillEffectLogicArgumentEnum.ailment_group_id)))));
          if (!((IEnumerable<BattleskillEffect>) skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (effect => logics.Any<Tuple<int, int>>((Func<Tuple<int, int>, bool>) (y =>
          {
            if ((BattleskillEffectLogicEnum) y.Item1 != effect.EffectLogic.Enum)
              return false;
            BattleFuncs.PackedSkillEffect packedSkillEffect = BattleFuncs.PackedSkillEffect.Create(effect);
            return (packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.ailment_group_id) ? packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.ailment_group_id) : 0) == y.Item2;
          })))))
            return false;
          BattleFuncs.PackedSkillEffect pse = BattleFuncs.PackedSkillEffect.Create(x);
          if (pse.HasKey(BattleskillEffectLogicArgumentEnum.land_tag1))
          {
            if (isColosseum)
              return false;
            BL.UnitPosition unitPosition = BattleFuncs.iSkillEffectListUnitToUnitPosition(target);
            BL.Panel panel = BattleFuncs.getPanel(unitPosition.row, unitPosition.column);
            if (!pse.CheckLandTag(panel, target is BL.AIUnit))
              return false;
          }
          return BattleFuncs.checkInvokeSkillEffect(pse, target, unit, colosseumTurn, unitHp: targetHp, targetHp: unitHp);
        }))))
          yield return ailmentResistEffect;
      }
    }

    public static BL.SkillEffect getPerfectAilmentResist(IEnumerable<BL.SkillEffect> resistEffects)
    {
      return resistEffects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.effect.HasKey(BattleskillEffectLogicArgumentEnum.is_perfect_guard) && x.effect.GetInt(BattleskillEffectLogicArgumentEnum.is_perfect_guard) == 1)).OrderByDescending<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (x => x.baseSkill.weight)).ThenBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (x => x.baseSkill.ID)).ThenBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (x => x.effectId)).FirstOrDefault<BL.SkillEffect>();
    }

    public static bool isAilmentInvest(
      float lottery,
      int skillID,
      BL.ISkillEffectListUnit target,
      BL.ISkillEffectListUnit unit,
      XorShift random,
      int? colosseumTurn,
      out List<BL.SkillEffect> useResistEffects,
      TurnHp turnHp,
      int? targetHp,
      int? unitHp)
    {
      return BattleFuncs.isAilmentInvest((Decimal) lottery, skillID, target, unit, random, colosseumTurn, out useResistEffects, turnHp, targetHp, unitHp, true);
    }

    public static bool isAilmentInvest(
      Decimal lottery,
      int skillID,
      BL.ISkillEffectListUnit target,
      BL.ISkillEffectListUnit unit,
      XorShift random,
      int? colosseumTurn,
      out List<BL.SkillEffect> useResistEffects,
      TurnHp turnHp,
      int? targetHp,
      int? unitHp,
      bool useFloat)
    {
      useResistEffects = new List<BL.SkillEffect>();
      if (MasterData.BattleskillSkill.ContainsKey(skillID))
      {
        lottery = Math.Min(1.0M, lottery);
        IEnumerable<BL.SkillEffect> source = BattleFuncs.getAilmentResistEffects(skillID, target, unit, targetHp, unitHp, colosseumTurn);
        if (turnHp != null)
          source = source.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x =>
          {
            if (!x.useRemain.HasValue || !turnHp.otherHp.ContainsKey(target))
              return true;
            int num1 = turnHp.otherHp[target].consumeSkillEffect.Count<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (y => y == x));
            int? useRemain = x.useRemain;
            int num2 = num1;
            return useRemain.GetValueOrDefault() > num2 & useRemain.HasValue;
          }));
        BL.SkillEffect[] array = source.ToArray<BL.SkillEffect>();
        BL.SkillEffect perfectAilmentResist = BattleFuncs.getPerfectAilmentResist((IEnumerable<BL.SkillEffect>) array);
        if (perfectAilmentResist != null)
        {
          if (perfectAilmentResist.useRemain.HasValue)
          {
            int? useRemain = perfectAilmentResist.useRemain;
            int num = 1;
            if (useRemain.GetValueOrDefault() >= num & useRemain.HasValue)
              useResistEffects.Add(perfectAilmentResist);
          }
          return false;
        }
        Decimal num3 = ((IEnumerable<BL.SkillEffect>) array).Select<BL.SkillEffect, Decimal>((Func<BL.SkillEffect, Decimal>) (x => (Decimal) x.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_change) + (Decimal) x.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_change_levelup) * (Decimal) x.baseSkillLevel)).Sum();
        lottery *= Math.Max(0.0M, 1.0M - num3);
        useResistEffects.AddRange(((IEnumerable<BL.SkillEffect>) array).Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x =>
        {
          if (!x.useRemain.HasValue)
            return false;
          int? useRemain = x.useRemain;
          int num4 = 1;
          return useRemain.GetValueOrDefault() >= num4 & useRemain.HasValue;
        })));
        if (useFloat)
        {
          if (lottery >= (Decimal) random.NextFloat())
            return true;
        }
        else if (lottery * 1000000M > (Decimal) random.NextFixed(1000000U))
          return true;
      }
      return false;
    }

    public static BL.Skill[] ailmentInvest(int skill_id, BL.ISkillEffectListUnit target)
    {
      if (!MasterData.BattleskillSkill.ContainsKey(skill_id))
        return (BL.Skill[]) null;
      BattleskillSkill battleskillSkill = MasterData.BattleskillSkill[skill_id];
      if (((IEnumerable<BattleskillEffect>) battleskillSkill.Effects).All<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.seal)))
      {
        IEnumerable<int> target_skills = ((IEnumerable<BattleskillEffect>) battleskillSkill.Effects).Select<BattleskillEffect, int>((Func<BattleskillEffect, int>) (x => x.GetInt(BattleskillEffectLogicArgumentEnum.skill_id)));
        if (!target_skills.Contains<int>(0) && !((IEnumerable<Tuple<BattleskillSkill, int, int>>) target.originalUnit.unitAndGearSkills).Any<Tuple<BattleskillSkill, int, int>>((Func<Tuple<BattleskillSkill, int, int>, bool>) (x => target_skills.Contains<int>(x.Item1.ID))))
          return (BL.Skill[]) null;
      }
      return new BL.Skill[1]
      {
        new BL.Skill() { id = skill_id }
      };
    }

    public static BL.ISkillEffectListUnit unitPositionToISkillEffectListUnit(BL.UnitPosition up)
    {
      return !(up is BL.ISkillEffectListUnit) ? (BL.ISkillEffectListUnit) up.unit : up as BL.ISkillEffectListUnit;
    }

    public static BL.UnitPosition iSkillEffectListUnitToUnitPosition(BL.ISkillEffectListUnit unit)
    {
      if (unit is BL.UnitPosition)
        return unit as BL.UnitPosition;
      if (BattleFuncs.env == null)
        return (BL.UnitPosition) null;
      return BattleFuncs.env.unitPositions != null && BattleFuncs.env.unitPositions.value != null ? BattleFuncs.env.getUnitPosition(unit.originalUnit) : (BL.UnitPosition) null;
    }

    public static BL.ISkillEffectListUnit unitToISkillEffectListUnit(BL.Unit unit, bool isAI)
    {
      BL.ISkillEffectListUnit skillEffectListUnit = (BL.ISkillEffectListUnit) null;
      if (isAI)
        skillEffectListUnit = (BL.ISkillEffectListUnit) BattleFuncs.env.getAIUnit(unit);
      return skillEffectListUnit ?? (BL.ISkillEffectListUnit) unit;
    }

    public static bool isSkillsAndEffectsInvalid(
      BL.ISkillEffectListUnit myself,
      BL.ISkillEffectListUnit enemy,
      BL.SkillEffect ef = null)
    {
      if (ef != null && ef.isAttackMethod)
        return false;
      Func<BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, int, bool> func = (Func<BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, int, bool>) ((unit, target, effect_target) => unit.skillEffects.Where(BattleskillEffectLogicEnum.invalid_skills_and_effects, (Func<BL.SkillEffect, bool>) (x =>
      {
        BattleskillEffect effect = x.effect;
        if ((double) effect.GetFloat(BattleskillEffectLogicArgumentEnum.effect_target) != (double) effect_target || BattleFuncs.isSealedSkillEffect(unit, x) || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != target.originalUnit.unit.kind.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != unit.originalUnit.playerUnit.GetElement() || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != target.originalUnit.playerUnit.GetElement() || effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) != unit.originalUnit.job.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) != target.originalUnit.job.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) != 0 && !unit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id)))
          return false;
        return effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) == 0 || target.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id));
      })).Any<BL.SkillEffect>());
      return func(myself, enemy, 0) || func(enemy, myself, 1);
    }

    private static bool checkEnabledSkillsAndEffectsInvalid(
      BattleskillEffect x,
      BL.ISkillEffectListUnit unit)
    {
      BattleskillEffect battleskillEffect = x;
      if (battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID || battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) != unit.originalUnit.playerUnit.GetElement() || battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) != 0 && battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) != unit.originalUnit.job.ID)
        return false;
      return battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || unit.originalUnit.playerUnit.HasFamily((UnitFamily) battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.family_id));
    }

    private static IEnumerable<BL.SkillEffect> GetEnabledLandBlessingBuffDebuff(
      BL.SkillEffectList self,
      BattleskillEffectLogicEnum e,
      BL.ISkillEffectListUnit unit,
      BattleLandform landform)
    {
      return self.Where(e, (Func<BL.SkillEffect, bool>) (effect => !BattleFuncs.isSealedSkillEffect(unit, effect) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.effect_target) == 0 && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.landform_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.landform_id) == landform.baseID) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == unit.originalUnit.job.ID) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == 0));
    }

    private static IEnumerable<BL.SkillEffect> GetEnabledLandBlessingBuffDebuff(
      BL.SkillEffectList self,
      BattleskillEffectLogicEnum e,
      BL.ISkillEffectListUnit effectUnit,
      BL.ISkillEffectListUnit targetUnit,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      BattleLandform landform,
      BattleLandformEffectPhase phase,
      int effectTarget)
    {
      return self.Where(e, (Func<BL.SkillEffect, bool>) (effect =>
      {
        if (BattleFuncs.isSealedSkillEffect(effectUnit, effect) || target != null && effectTarget != effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.effect_target) || phase != (BattleLandformEffectPhase) 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.phase) != 0 && (BattleLandformEffectPhase) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.phase) != phase || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.landform_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.landform_id) != landform.baseID || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != effectUnit.originalUnit.unit.kind.ID || target != null && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != targetUnit.originalUnit.unit.kind.ID || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != effectUnit.originalUnit.playerUnit.GetElement() || target != null && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != 0 && (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != targetUnit.originalUnit.playerUnit.GetElement() || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) != effectUnit.originalUnit.job.ID || target != null && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) != targetUnit.originalUnit.job.ID || target != null && effectUnit == target && BattleFuncs.isSkillsAndEffectsInvalid(target, unit) || target != null && BattleFuncs.isEffectEnemyRangeAndInvalid(effect, unit, target))
          return false;
        return target == null || !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
      }));
    }

    private static bool CheckEnabledLandBlessingBuffDebuff(
      BattleskillEffect effect,
      BL.ISkillEffectListUnit unit)
    {
      if (effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != unit.originalUnit.playerUnit.GetElement())
        return false;
      return effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == unit.originalUnit.job.ID;
    }

    public static void GetLandBlessingSkillAdd(
      List<BattleFuncs.SkillParam> skillParams,
      BL.ISkillEffectListUnit beUnit,
      BattleskillEffectLogicEnum fix_logic,
      BattleLandform landform)
    {
      foreach (BL.SkillEffect effect in BattleFuncs.GetEnabledLandBlessingBuffDebuff(beUnit.skillEffects, fix_logic, beUnit, landform))
        skillParams.Add(BattleFuncs.SkillParam.CreateAdd(beUnit.originalUnit, effect, (float) (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.value) + effect.baseSkillLevel * effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_ratio))));
    }

    public static void GetLandBlessingSkillAdd(
      List<BattleFuncs.SkillParam> skillParams,
      BL.ISkillEffectListUnit beUnit,
      BL.ISkillEffectListUnit beTarget,
      BattleskillEffectLogicEnum fix_logic,
      BattleLandform landform,
      BattleLandformEffectPhase phase)
    {
      foreach (BL.SkillEffect effect in BattleFuncs.GetEnabledLandBlessingBuffDebuff(beUnit.skillEffects, fix_logic, beUnit, beTarget, beUnit, beTarget, landform, phase, 0))
        skillParams.Add(BattleFuncs.SkillParam.CreateAdd(beUnit.originalUnit, effect, (float) (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.value) + effect.baseSkillLevel * effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_ratio))));
      if (beTarget == null || beUnit.originalUnit.isFacility)
        return;
      foreach (BL.SkillEffect effect in BattleFuncs.GetEnabledLandBlessingBuffDebuff(beTarget.skillEffects, fix_logic, beTarget, beUnit, beUnit, beTarget, landform, phase, 1))
        skillParams.Add(BattleFuncs.SkillParam.CreateAdd(beTarget.originalUnit, effect, (float) (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.value) + effect.baseSkillLevel * effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_ratio))));
    }

    public static void GetLandBlessingSkillMul(
      List<BattleFuncs.SkillParam> skillParams,
      BL.ISkillEffectListUnit beUnit,
      BattleskillEffectLogicEnum ratio_logic,
      BattleLandform landform)
    {
      foreach (BL.SkillEffect effect in BattleFuncs.GetEnabledLandBlessingBuffDebuff(beUnit.skillEffects, ratio_logic, beUnit, landform))
        skillParams.Add(BattleFuncs.SkillParam.CreateMul(beUnit.originalUnit, effect, effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) + (float) effect.baseSkillLevel * effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio)));
    }

    public static void GetLandBlessingSkillMul(
      List<BattleFuncs.SkillParam> skillParams,
      BL.ISkillEffectListUnit beUnit,
      BL.ISkillEffectListUnit beTarget,
      BattleskillEffectLogicEnum ratio_logic,
      BattleLandform landform,
      BattleLandformEffectPhase phase)
    {
      foreach (BL.SkillEffect effect in BattleFuncs.GetEnabledLandBlessingBuffDebuff(beUnit.skillEffects, ratio_logic, beUnit, beTarget, beUnit, beTarget, landform, phase, 0))
        skillParams.Add(BattleFuncs.SkillParam.CreateMul(beUnit.originalUnit, effect, effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) + (float) effect.baseSkillLevel * effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio)));
      if (beTarget == null || beUnit.originalUnit.isFacility)
        return;
      foreach (BL.SkillEffect effect in BattleFuncs.GetEnabledLandBlessingBuffDebuff(beTarget.skillEffects, ratio_logic, beTarget, beUnit, beUnit, beTarget, landform, phase, 1))
        skillParams.Add(BattleFuncs.SkillParam.CreateMul(beUnit.originalUnit, effect, effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) + (float) effect.baseSkillLevel * effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio)));
    }

    private static IEnumerable<BL.SkillEffect> GetEnabledDuelSupportBuffDebuff(
      BL.SkillEffectList self,
      BattleskillEffectLogicEnum e,
      BL.ISkillEffectListUnit effectUnit,
      BL.ISkillEffectListUnit targetUnit,
      int effectTarget,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target)
    {
      return self.Where(e, (Func<BL.SkillEffect, bool>) (effect =>
      {
        BattleskillEffect effect1 = effect.effect;
        return !BattleFuncs.isSealedSkillEffect(effectUnit, effect) && effectTarget == effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.effect_target) && (!effect1.HasKey(BattleskillEffectLogicArgumentEnum.gear_kind_id) || effect1.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect1.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == effectUnit.originalUnit.unit.kind.ID) && (!effect1.HasKey(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) || effect1.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 || effect1.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == targetUnit.originalUnit.unit.kind.ID) && (!effect1.HasKey(BattleskillEffectLogicArgumentEnum.element) || effect1.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect1.GetInt(BattleskillEffectLogicArgumentEnum.element) == effectUnit.originalUnit.playerUnit.GetElement()) && (!effect1.HasKey(BattleskillEffectLogicArgumentEnum.target_element) || effect1.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 || (CommonElement) effect1.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == targetUnit.originalUnit.playerUnit.GetElement()) && (effectUnit != target || !BattleFuncs.isSkillsAndEffectsInvalid(target, unit)) && !BattleFuncs.isEffectEnemyRangeAndInvalid(effect, unit, target) && !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
      }));
    }

    private static bool CheckEnabledDuelSupportBuffDebuff(
      BattleskillEffect effect,
      BL.ISkillEffectListUnit effectUnit)
    {
      BattleskillEffect battleskillEffect = effect;
      if (battleskillEffect.HasKey(BattleskillEffectLogicArgumentEnum.gear_kind_id) && battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != effectUnit.originalUnit.unit.kind.ID)
        return false;
      return !battleskillEffect.HasKey(BattleskillEffectLogicArgumentEnum.element) || battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) == effectUnit.originalUnit.playerUnit.GetElement();
    }

    private static IEnumerable<BL.SkillEffect> GetDuelSupportSkillEffects(
      BL.ISkillEffectListUnit effectUnit,
      BL.ISkillEffectListUnit targetUnit,
      BL.ISkillEffectListUnit beUnit,
      BL.ISkillEffectListUnit beTarget,
      BattleskillEffectLogicEnum logic,
      BL.Unit[] neighborUnits,
      int target)
    {
      foreach (BL.SkillEffect skillEffect in BattleFuncs.GetEnabledDuelSupportBuffDebuff(effectUnit.skillEffects, logic, effectUnit, targetUnit, target, beUnit, beTarget))
      {
        BL.SkillEffect effect = skillEffect;
        BattleFuncs.PackedSkillEffect pse = BattleFuncs.PackedSkillEffect.Create(effect);
        int count = ((IEnumerable<BL.Unit>) neighborUnits).Count<BL.Unit>((Func<BL.Unit, bool>) (x =>
        {
          if (effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.character_id) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.character_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.character_id) != x.unit.character.ID || effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.same_character_id) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.same_character_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.same_character_id) != x.unit.same_character_id || effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.unit_id) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.unit_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.unit_id) != x.unit.ID || effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.effect_element) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.effect_element) != 0 && (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.effect_element) != x.playerUnit.GetElement() || effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.effect_gear_kind_id) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.effect_gear_kind_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.effect_gear_kind_id) != x.unit.kind.ID || effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.effect_family_id) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.effect_family_id) != 0 && !x.playerUnit.HasFamily((UnitFamily) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.effect_family_id)))
            return false;
          return !pse.HasKey(BattleskillEffectLogicArgumentEnum.effect_skill_group_id) || pse.GetInt(BattleskillEffectLogicArgumentEnum.effect_skill_group_id) == 0 || x.unit.HasSkillGroupId(pse.GetInt(BattleskillEffectLogicArgumentEnum.effect_skill_group_id));
        }));
        if (count != 0)
        {
          if (count > effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_target_count) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_target_count) != 0)
            count = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_target_count);
          for (int i = 0; i < count; ++i)
            yield return effect;
        }
      }
    }

    public static void GetDuelSupportSkillAdd(
      List<BattleFuncs.SkillParam> skillParams,
      BL.ISkillEffectListUnit beUnit,
      BL.ISkillEffectListUnit beTarget,
      BattleskillEffectLogicEnum fix_logic,
      BL.Unit[] neighborUnits)
    {
      foreach (BL.SkillEffect supportSkillEffect in BattleFuncs.GetDuelSupportSkillEffects(beUnit, beTarget, beUnit, beTarget, fix_logic, neighborUnits, 0))
        skillParams.Add(BattleFuncs.SkillParam.CreateAdd(beUnit.originalUnit, supportSkillEffect, (float) (supportSkillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.value) + supportSkillEffect.baseSkillLevel * supportSkillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_ratio))));
      foreach (BL.SkillEffect supportSkillEffect in BattleFuncs.GetDuelSupportSkillEffects(beTarget, beUnit, beUnit, beTarget, fix_logic, neighborUnits, 1))
        skillParams.Add(BattleFuncs.SkillParam.CreateAdd(beTarget.originalUnit, supportSkillEffect, (float) (supportSkillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.value) + supportSkillEffect.baseSkillLevel * supportSkillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_ratio))));
    }

    public static void GetDuelSupportSkillMul(
      List<BattleFuncs.SkillParam> skillParams,
      BL.ISkillEffectListUnit beUnit,
      BL.ISkillEffectListUnit beTarget,
      BattleskillEffectLogicEnum ratio_logic,
      BL.Unit[] neighborUnits)
    {
      foreach (BL.SkillEffect supportSkillEffect in BattleFuncs.GetDuelSupportSkillEffects(beUnit, beTarget, beUnit, beTarget, ratio_logic, neighborUnits, 0))
        skillParams.Add(BattleFuncs.SkillParam.CreateMul(beUnit.originalUnit, supportSkillEffect, supportSkillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) + (float) supportSkillEffect.baseSkillLevel * supportSkillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio)));
      foreach (BL.SkillEffect supportSkillEffect in BattleFuncs.GetDuelSupportSkillEffects(beTarget, beUnit, beUnit, beTarget, ratio_logic, neighborUnits, 1))
        skillParams.Add(BattleFuncs.SkillParam.CreateMul(beTarget.originalUnit, supportSkillEffect, supportSkillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) + (float) supportSkillEffect.baseSkillLevel * supportSkillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio)));
    }

    public static BattleFuncs.BeforeDuelDuelSupport GetBeforeDuelDuelSupport(
      BL.ISkillEffectListUnit beUnit,
      BL.ISkillEffectListUnit beTarget,
      BL.Unit[] neighborUnitsOrg)
    {
      BattleFuncs.BeforeDuelDuelSupport beforeDuelDuelSupport = new BattleFuncs.BeforeDuelDuelSupport();
      BL.Unit[] array = ((IEnumerable<BL.Unit>) neighborUnitsOrg).Where<BL.Unit>((Func<BL.Unit, bool>) (x => !x.isFacility)).ToArray<BL.Unit>();
      beforeDuelDuelSupport.duelSupport = beUnit.originalUnit.playerUnit.GetIntimateDuelSupport(((IEnumerable<BL.Unit>) array).Select<BL.Unit, PlayerUnit>((Func<BL.Unit, PlayerUnit>) (x => x.playerUnit)).ToArray<PlayerUnit>());
      List<BattleFuncs.SkillParam> skillParams1 = new List<BattleFuncs.SkillParam>();
      List<BattleFuncs.SkillParam> skillParams2 = new List<BattleFuncs.SkillParam>();
      List<BattleFuncs.SkillParam> skillParams3 = new List<BattleFuncs.SkillParam>();
      List<BattleFuncs.SkillParam> skillParams4 = new List<BattleFuncs.SkillParam>();
      BattleFuncs.GetDuelSupportSkillAdd(skillParams1, beUnit, beTarget, BattleskillEffectLogicEnum.duel_support_fix_hit, array);
      BattleFuncs.GetDuelSupportSkillAdd(skillParams1, beUnit, beTarget, BattleskillEffectLogicEnum.duel_support2_fix_hit, array);
      BattleFuncs.GetDuelSupportSkillAdd(skillParams2, beUnit, beTarget, BattleskillEffectLogicEnum.duel_support_fix_evasion, array);
      BattleFuncs.GetDuelSupportSkillAdd(skillParams2, beUnit, beTarget, BattleskillEffectLogicEnum.duel_support2_fix_evasion, array);
      BattleFuncs.GetDuelSupportSkillAdd(skillParams3, beUnit, beTarget, BattleskillEffectLogicEnum.duel_support_fix_critical, array);
      BattleFuncs.GetDuelSupportSkillAdd(skillParams3, beUnit, beTarget, BattleskillEffectLogicEnum.duel_support2_fix_critical, array);
      BattleFuncs.GetDuelSupportSkillAdd(skillParams4, beUnit, beTarget, BattleskillEffectLogicEnum.duel_support_fix_critical_evasion, array);
      BattleFuncs.GetDuelSupportSkillAdd(skillParams4, beUnit, beTarget, BattleskillEffectLogicEnum.duel_support2_fix_critical_evasion, array);
      BattleFuncs.GetDuelSupportSkillMul(skillParams1, beUnit, beTarget, BattleskillEffectLogicEnum.duel_support_ratio_hit, array);
      BattleFuncs.GetDuelSupportSkillMul(skillParams1, beUnit, beTarget, BattleskillEffectLogicEnum.duel_support2_ratio_hit, array);
      BattleFuncs.GetDuelSupportSkillMul(skillParams2, beUnit, beTarget, BattleskillEffectLogicEnum.duel_support_ratio_evasion, array);
      BattleFuncs.GetDuelSupportSkillMul(skillParams2, beUnit, beTarget, BattleskillEffectLogicEnum.duel_support2_ratio_evasion, array);
      BattleFuncs.GetDuelSupportSkillMul(skillParams3, beUnit, beTarget, BattleskillEffectLogicEnum.duel_support_ratio_critical, array);
      BattleFuncs.GetDuelSupportSkillMul(skillParams3, beUnit, beTarget, BattleskillEffectLogicEnum.duel_support2_ratio_critical, array);
      BattleFuncs.GetDuelSupportSkillMul(skillParams4, beUnit, beTarget, BattleskillEffectLogicEnum.duel_support_ratio_critical_evasion, array);
      BattleFuncs.GetDuelSupportSkillMul(skillParams4, beUnit, beTarget, BattleskillEffectLogicEnum.duel_support2_ratio_critical_evasion, array);
      int num1 = BattleFuncs.calcSkillParamAdd(skillParams1);
      int num2 = BattleFuncs.calcSkillParamAdd(skillParams2);
      int num3 = BattleFuncs.calcSkillParamAdd(skillParams3);
      int num4 = BattleFuncs.calcSkillParamAdd(skillParams4);
      float num5 = BattleFuncs.calcSkillParamMul(skillParams1);
      float num6 = BattleFuncs.calcSkillParamMul(skillParams2);
      float num7 = BattleFuncs.calcSkillParamMul(skillParams3);
      float num8 = BattleFuncs.calcSkillParamMul(skillParams4);
      beforeDuelDuelSupport.hit = Mathf.CeilToInt((float) beforeDuelDuelSupport.duelSupport.hit * num5) + num1;
      beforeDuelDuelSupport.evasion = Mathf.CeilToInt((float) beforeDuelDuelSupport.duelSupport.evasion * num6) + num2;
      beforeDuelDuelSupport.critical = Mathf.CeilToInt((float) beforeDuelDuelSupport.duelSupport.critical * num7) + num3;
      beforeDuelDuelSupport.criticalEvasion = Mathf.CeilToInt((float) beforeDuelDuelSupport.duelSupport.critical_evasion * num8) + num4;
      beforeDuelDuelSupport.hitIncr = beforeDuelDuelSupport.hit - beforeDuelDuelSupport.duelSupport.hit;
      beforeDuelDuelSupport.evasionIncr = beforeDuelDuelSupport.evasion - beforeDuelDuelSupport.duelSupport.evasion;
      beforeDuelDuelSupport.criticalIncr = beforeDuelDuelSupport.critical - beforeDuelDuelSupport.duelSupport.critical;
      beforeDuelDuelSupport.criticalEvasionIncr = beforeDuelDuelSupport.criticalEvasion - beforeDuelDuelSupport.duelSupport.critical_evasion;
      return beforeDuelDuelSupport;
    }

    public static BL.PhaseState getPhaseState()
    {
      return BattleFuncs.env != null ? BattleFuncs.env.phaseState : (BL.PhaseState) null;
    }

    public static List<BattleFuncs.InvalidSpecificSkillLogic> GetInvalidSkillsAndLogics(
      IEnumerable<BattleskillEffect> effects)
    {
      List<BattleFuncs.InvalidSpecificSkillLogic> invalidSkillsAndLogics = new List<BattleFuncs.InvalidSpecificSkillLogic>();
      if (effects == null || !effects.Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.HasKey(BattleskillEffectLogicArgumentEnum.gda_percentage_invocation) || x.HasKey(BattleskillEffectLogicArgumentEnum.gdd_percentage_invocation))))
        return invalidSkillsAndLogics;
      foreach (BattleskillEffect effect in effects)
      {
        bool flag1 = effect.HasKey(BattleskillEffectLogicArgumentEnum.invalid_skill_id);
        bool flag2 = effect.HasKey(BattleskillEffectLogicArgumentEnum.invalid_logic_id);
        if (flag1 | flag2)
        {
          int skillId = flag1 ? effect.GetInt(BattleskillEffectLogicArgumentEnum.invalid_skill_id) : 0;
          int logicId = flag2 ? effect.GetInt(BattleskillEffectLogicArgumentEnum.invalid_logic_id) : 0;
          invalidSkillsAndLogics.Add(BattleFuncs.InvalidSpecificSkillLogic.Create(skillId, logicId, (object) effect));
        }
      }
      return invalidSkillsAndLogics;
    }

    public static bool checkInvalidEffect(
      BattleskillEffect effect,
      List<BattleFuncs.InvalidSpecificSkillLogic> invalidSkillLogics,
      Func<BattleFuncs.InvalidSpecificSkillLogic, bool> funcExtraCheck = null)
    {
      return invalidSkillLogics != null && invalidSkillLogics.Any<BattleFuncs.InvalidSpecificSkillLogic>((Func<BattleFuncs.InvalidSpecificSkillLogic, bool>) (x =>
      {
        if (x.skillId != 0 && x.skillId != effect.skill.ID || x.logicId != 0 && (BattleskillEffectLogicEnum) x.logicId != effect.EffectLogic.Enum)
          return false;
        return funcExtraCheck == null || funcExtraCheck(x);
      }));
    }

    public static bool checkInvalidEffect(
      BL.SkillEffect effect,
      List<BattleFuncs.InvalidSpecificSkillLogic> invalidSkillLogics,
      Func<BattleFuncs.InvalidSpecificSkillLogic, bool> funcExtraCheck = null)
    {
      return BattleFuncs.checkInvalidEffect(effect.effect, invalidSkillLogics, funcExtraCheck);
    }

    public static IEnumerable<BattleFuncs.InvalidSpecificSkillLogic> getEnabledInvalidSkillLogics(
      BattleskillEffect effect,
      List<BattleFuncs.InvalidSpecificSkillLogic> invalidSkillLogics,
      Func<BattleFuncs.InvalidSpecificSkillLogic, bool> funcExtraCheck = null)
    {
      return invalidSkillLogics.Where<BattleFuncs.InvalidSpecificSkillLogic>((Func<BattleFuncs.InvalidSpecificSkillLogic, bool>) (x =>
      {
        if (x.skillId != 0 && x.skillId != effect.skill.ID || x.logicId != 0 && (BattleskillEffectLogicEnum) x.logicId != effect.EffectLogic.Enum)
          return false;
        return funcExtraCheck == null || funcExtraCheck(x);
      }));
    }

    public static List<BattleFuncs.InvalidSpecificSkillLogic> getInvalidSkillsAndLogics(
      BL.ISkillEffectListUnit myself,
      BL.ISkillEffectListUnit enemy,
      BL.Panel myselfPanel,
      BL.Panel enemyPanel,
      int attackType,
      bool dontCheckRange,
      bool isAI,
      bool isDuel,
      bool isAfterDuel,
      int? myselfHp = null,
      int? enemyHp = null)
    {
      List<BattleFuncs.InvalidSpecificSkillLogic> invalidSkillsAndLogics = new List<BattleFuncs.InvalidSpecificSkillLogic>();
      foreach (BL.SkillEffect headerEffect in (IEnumerable<BL.SkillEffect>) BattleFuncs.gearSkillEffectFilter(myself.originalUnit, myself.skillEffects.Where(BattleskillEffectLogicEnum.invalid_specific_skills_and_logics, (Func<BL.SkillEffect, bool>) (x =>
      {
        BattleskillEffect effect = x.effect;
        BattleFuncs.PackedSkillEffect pse = BattleFuncs.PackedSkillEffect.Create(x);
        if (x.useRemain.HasValue && x.useRemain.Value < 1 || BattleFuncs.isSealedSkillEffect(myself, x) || effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack_nc) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack_nc) != attackType || !dontCheckRange && effect.GetInt(BattleskillEffectLogicArgumentEnum.range) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.range) != BL.fieldDistance(myselfPanel, enemyPanel) || !BattleFuncs.checkInvokeSkillEffect(pse, myself, enemy, unitHp: myselfHp, targetHp: enemyHp) || !pse.CheckLandTag(myselfPanel, isAI) || isDuel && BattleFuncs.isEffectEnemyRangeAndInvalid(x, myself, enemy))
          return false;
        return !isDuel && !isAfterDuel || !BattleFuncs.isSkillsAndEffectsInvalid(myself, enemy);
      }))).OrderByDescending<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (x => x.baseSkill.weight)).ThenBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (x => x.baseSkillId)).ThenByDescending<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (x => x.baseSkillLevel)))
      {
        foreach (BattleskillEffect effect in BattleFuncs.PackedSkillEffect.Create(headerEffect).GetEffects())
        {
          if (effect.HasKey(BattleskillEffectLogicArgumentEnum.invalid_skill_id))
            invalidSkillsAndLogics.Add(BattleFuncs.InvalidSpecificSkillLogic.Create(effect.GetInt(BattleskillEffectLogicArgumentEnum.invalid_skill_id), effect.GetInt(BattleskillEffectLogicArgumentEnum.invalid_logic_id), (object) headerEffect));
        }
      }
      return invalidSkillsAndLogics;
    }

    public static bool isInvoke(
      BL.ISkillEffectListUnit invoke,
      BL.ISkillEffectListUnit target,
      Judgement.BeforeDuelUnitParameter invokeParameter,
      Judgement.BeforeDuelUnitParameter targetParameter,
      AttackStatus invokeAS,
      AttackStatus targetAS,
      int skill_level,
      float percentage_invocation,
      XorShift random,
      bool isDuelSkill,
      int invokeHp,
      int targetHp,
      int? colosseumTurn,
      float? invokeRate = null,
      BattleskillEffect effect = null,
      float base_invocation = 0.0f,
      float invocation_skill_ratio = 1f,
      float invocation_luck_ratio = 1f,
      List<BattleFuncs.InvalidSpecificSkillLogic> invalidSkillLogics = null)
    {
      return BattleFuncs.isInvoke(out BattleFuncs.InvokeLotteryInfo _, invoke, target, invokeParameter, targetParameter, invokeAS, targetAS, skill_level, percentage_invocation, random, isDuelSkill, invokeHp, targetHp, colosseumTurn, invokeRate, effect, base_invocation, invocation_skill_ratio, invocation_luck_ratio, invalidSkillLogics = (List<BattleFuncs.InvalidSpecificSkillLogic>) null);
    }

    public static bool isInvoke(
      out BattleFuncs.InvokeLotteryInfo info,
      BL.ISkillEffectListUnit invoke,
      BL.ISkillEffectListUnit target,
      Judgement.BeforeDuelUnitParameter invokeParameter,
      Judgement.BeforeDuelUnitParameter targetParameter,
      AttackStatus invokeAS,
      AttackStatus targetAS,
      int skill_level,
      float percentage_invocation,
      XorShift random,
      bool isDuelSkill,
      int invokeHp,
      int targetHp,
      int? colosseumTurn,
      float? invokeRate = null,
      BattleskillEffect effect = null,
      float base_invocation = 0.0f,
      float invocation_skill_ratio = 1f,
      float invocation_luck_ratio = 1f,
      List<BattleFuncs.InvalidSpecificSkillLogic> invalidSkillLogics = null)
    {
      int num1 = invokeParameter.Dexterity;
      if (effect != null)
      {
        switch (effect.EffectLogic.Enum)
        {
          case BattleskillEffectLogicEnum.prayer:
          case BattleskillEffectLogicEnum.passive_prayer:
            num1 = invokeParameter.Luck;
            break;
        }
      }
      int num2 = targetParameter.Luck;
      if (num2 < 0)
        num2 = 0;
      float num3 = (float) ((double) num1 * (double) percentage_invocation + (double) base_invocation + (double) skill_level * (double) invocation_skill_ratio - (double) num2 * (double) invocation_luck_ratio);
      info.Base = num3;
      float? nullable1;
      if (invokeRate.HasValue)
      {
        double num4 = (double) num3;
        float? nullable2 = invokeRate;
        float num5 = 100f;
        nullable1 = nullable2.HasValue ? new float?(nullable2.GetValueOrDefault() * num5) : new float?();
        double valueOrDefault = (double) nullable1.GetValueOrDefault();
        if (num4 < valueOrDefault & nullable1.HasValue)
          num3 = invokeRate.Value * 100f;
      }
      info.External = invokeRate;
      float num6 = BattleFuncs.getWhiteNightRate(invoke, target, invokeAS, targetAS, invalidSkillLogics, isDuelSkill, invokeHp, targetHp, colosseumTurn) * 100f;
      float num7 = num3 + num6;
      info.WhiteNight = num6;
      float? nullable3 = new float?();
      float? nullable4 = new float?();
      Func<BL.SkillEffect, BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, AttackStatus, int, bool> checkConditions = (Func<BL.SkillEffect, BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, AttackStatus, int, bool>) ((skillEffect, effInvoker, effTarget, attackStatus, effect_target) =>
      {
        BattleFuncs.PackedSkillEffect packedSkillEffect = BattleFuncs.PackedSkillEffect.Create(skillEffect);
        if (!packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.effect_target) && effect_target != 0 || packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.effect_target) && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.effect_target) != effect_target)
          return false;
        if (packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.attack_type))
        {
          int num8 = packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.attack_type);
          if (num8 == 1 && (attackStatus == null || attackStatus.isMagic) || num8 == 2 && (attackStatus == null || !attackStatus.isMagic))
            return false;
        }
        BL.Panel fieldPanel = !colosseumTurn.HasValue ? BattleFuncs.env.getFieldPanel(BattleFuncs.iSkillEffectListUnitToUnitPosition(effInvoker)) : (BL.Panel) null;
        return (!packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.gear_kind_id) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == effInvoker.originalUnit.unit.kind.ID) && (!packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == effTarget.originalUnit.unit.kind.ID) && (!packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.element) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) == effInvoker.originalUnit.playerUnit.GetElement()) && (!packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.target_element) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 || (CommonElement) packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == effTarget.originalUnit.playerUnit.GetElement()) && (!packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.job_id) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == 0 || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == effInvoker.originalUnit.job.ID) && (!packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.target_job_id) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == 0 || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == effTarget.originalUnit.job.ID) && packedSkillEffect.CheckLandTag(fieldPanel, invoke is BL.AIUnit);
      });
      foreach (BL.SkillEffect skillEffect in BattleFuncs.gearSkillEffectFilter(invoke.originalUnit, invoke.enabledSkillEffect(BattleskillEffectLogicEnum.clamp_invoke_duel_skill).Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => !BattleFuncs.checkInvalidEffect(x, invalidSkillLogics) && checkConditions(x, invoke, target, invokeAS, 0)))).Concat<BL.SkillEffect>(BattleFuncs.gearSkillEffectFilter(target.originalUnit, target.enabledSkillEffect(BattleskillEffectLogicEnum.clamp_invoke_duel_skill).Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => !BattleFuncs.checkInvalidEffect(x, invalidSkillLogics) && checkConditions(x, target, invoke, targetAS, 1))))))
      {
        float num9 = (float) (((Decimal) skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.min_percentage) + (Decimal) skillEffect.baseSkillLevel * (Decimal) skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.min_skill_ratio)) * 100M);
        if (nullable3.HasValue)
        {
          double num10 = (double) num9;
          nullable1 = nullable3;
          double valueOrDefault = (double) nullable1.GetValueOrDefault();
          if (!(num10 > valueOrDefault & nullable1.HasValue))
            goto label_13;
        }
        nullable3 = new float?(num9);
label_13:
        float num11 = (float) (((Decimal) skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.max_percentage) + (Decimal) skillEffect.baseSkillLevel * (Decimal) skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.max_skill_ratio)) * 100M);
        if (nullable4.HasValue)
        {
          double num12 = (double) num11;
          nullable1 = nullable4;
          double valueOrDefault = (double) nullable1.GetValueOrDefault();
          if (!(num12 < valueOrDefault & nullable1.HasValue))
            continue;
        }
        nullable4 = new float?(num11);
      }
      info.Min = nullable3;
      info.Max = nullable4;
      if (nullable4.HasValue)
      {
        double num13 = (double) num7;
        float? nullable5 = nullable4;
        double valueOrDefault = (double) nullable5.GetValueOrDefault();
        if (num13 > valueOrDefault & nullable5.HasValue)
          num7 = nullable4.Value;
      }
      if (nullable3.HasValue)
      {
        double num14 = (double) num7;
        float? nullable6 = nullable3;
        double valueOrDefault = (double) nullable6.GetValueOrDefault();
        if (num14 < valueOrDefault & nullable6.HasValue)
          num7 = nullable3.Value;
      }
      info.Final = num7;
      return (double) num7 >= (double) random.NextFloat() * 100.0;
    }

    private static float getWhiteNightRate(
      BL.ISkillEffectListUnit myself,
      BL.ISkillEffectListUnit enemy,
      AttackStatus myselfAS,
      AttackStatus enemyAS,
      List<BattleFuncs.InvalidSpecificSkillLogic> invalidSkillLogics,
      bool isDuelSkill,
      int myselfHp,
      int enemyHp,
      int? colosseumTurn,
      BL.Panel myselfPanel = null,
      BL.Panel enemyPanel = null)
    {
      List<BattleFuncs.SkillParam> skillParams = new List<BattleFuncs.SkillParam>();
      Judgement.NonBattleParameter.FromPlayerUnitCache myselfNbpCache = new Judgement.NonBattleParameter.FromPlayerUnitCache(myself.originalUnit.playerUnit);
      Judgement.NonBattleParameter.FromPlayerUnitCache enemyNbpCache = new Judgement.NonBattleParameter.FromPlayerUnitCache(enemy.originalUnit.playerUnit);
      if (!colosseumTurn.HasValue)
      {
        if (myselfPanel == null)
        {
          BL.UnitPosition unitPosition = BattleFuncs.iSkillEffectListUnitToUnitPosition(myself);
          myselfPanel = BattleFuncs.getPanel(unitPosition.row, unitPosition.column);
        }
        if (enemyPanel == null)
        {
          BL.UnitPosition unitPosition = BattleFuncs.iSkillEffectListUnitToUnitPosition(enemy);
          enemyPanel = BattleFuncs.getPanel(unitPosition.row, unitPosition.column);
        }
      }
      Action<BattleskillEffectLogicEnum, BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, int> action = (Action<BattleskillEffectLogicEnum, BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, int>) ((logic, effectUnit, targetUnit, effect_target) =>
      {
        AttackStatus attackStatus;
        int effectHp;
        BL.Panel effectPanel;
        Judgement.NonBattleParameter.FromPlayerUnitCache effectNbpCache;
        Judgement.NonBattleParameter.FromPlayerUnitCache targetNbpCache;
        int targetHp;
        if (effect_target == 0)
        {
          attackStatus = myselfAS;
          effectNbpCache = myselfNbpCache;
          targetNbpCache = enemyNbpCache;
          effectHp = myselfHp;
          targetHp = enemyHp;
          effectPanel = myselfPanel;
        }
        else
        {
          attackStatus = enemyAS;
          effectNbpCache = enemyNbpCache;
          targetNbpCache = myselfNbpCache;
          effectHp = enemyHp;
          targetHp = myselfHp;
          effectPanel = enemyPanel;
        }
        foreach (BL.SkillEffect effect in effectUnit.skillEffects.Where(logic, (Func<BL.SkillEffect, bool>) (x =>
        {
          if (BattleFuncs.checkInvalidEffect(x, invalidSkillLogics))
            return false;
          BattleFuncs.PackedSkillEffect pse = BattleFuncs.PackedSkillEffect.Create(x);
          if (pse.HasKey(BattleskillEffectLogicArgumentEnum.attack_type))
          {
            int num = pse.GetInt(BattleskillEffectLogicArgumentEnum.attack_type);
            if (num == 1 && (attackStatus == null || attackStatus.isMagic) || num == 2 && (attackStatus == null || !attackStatus.isMagic))
              return false;
          }
          if (pse.HasKey(BattleskillEffectLogicArgumentEnum.min_hp_percentage))
          {
            Decimal num4 = (Decimal) effectHp / (Decimal) effectUnit.originalUnit.parameter.Hp;
            float num5 = pse.GetFloat(BattleskillEffectLogicArgumentEnum.min_hp_percentage);
            float num6 = pse.GetFloat(BattleskillEffectLogicArgumentEnum.max_hp_percentage);
            if ((double) num5 != 0.0 && num4 < (Decimal) num5 || (double) num6 != 0.0 && num4 >= (Decimal) num6)
              return false;
          }
          if ((double) pse.GetFloat(BattleskillEffectLogicArgumentEnum.effect_target) != (double) effect_target || !pse.CheckLandTag(effectPanel, myself is BL.AIUnit) || BattleFuncs.isSealedSkillEffect(effectUnit, x))
            return false;
          if (logic == BattleskillEffectLogicEnum.white_night3)
          {
            pse.SetIgnoreHeader(true);
            if (!BattleFuncs.checkInvokeSkillEffect(pse, effectUnit, targetUnit, colosseumTurn, effectNbpCache, targetNbpCache, new int?(effectHp), new int?(targetHp)))
              return false;
            int paramDiffValue = BattleFuncs.GetParamDiffValue(x.effect.GetInt(BattleskillEffectLogicArgumentEnum.param_type), effectNbpCache, effectHp);
            int num = BattleFuncs.GetParamDiffValue(x.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_param_type), targetNbpCache, targetHp) - paramDiffValue;
            if (num < x.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_value) || num > x.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_value))
              return false;
          }
          else if ((pse.HasKey(BattleskillEffectLogicArgumentEnum.gear_kind_id) && pse.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && pse.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != effectUnit.originalUnit.unit.kind.ID || pse.HasKey(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) && pse.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 && pse.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != targetUnit.originalUnit.unit.kind.ID || pse.HasKey(BattleskillEffectLogicArgumentEnum.element) && pse.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) pse.GetInt(BattleskillEffectLogicArgumentEnum.element) != effectUnit.originalUnit.playerUnit.GetElement() || pse.HasKey(BattleskillEffectLogicArgumentEnum.target_element) && pse.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != 0 && (CommonElement) pse.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != targetUnit.originalUnit.playerUnit.GetElement() || pse.HasKey(BattleskillEffectLogicArgumentEnum.job_id) && pse.GetInt(BattleskillEffectLogicArgumentEnum.job_id) != 0 && pse.GetInt(BattleskillEffectLogicArgumentEnum.job_id) != effectUnit.originalUnit.job.ID || pse.HasKey(BattleskillEffectLogicArgumentEnum.target_job_id) && pse.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) != 0 && pse.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) != targetUnit.originalUnit.job.ID || pse.HasKey(BattleskillEffectLogicArgumentEnum.family_id) && pse.GetInt(BattleskillEffectLogicArgumentEnum.family_id) != 0 && !effectUnit.originalUnit.playerUnit.HasFamily((UnitFamily) pse.GetInt(BattleskillEffectLogicArgumentEnum.family_id)) ? 0 : (!pse.HasKey(BattleskillEffectLogicArgumentEnum.target_family_id) || pse.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) == 0 ? 1 : (targetUnit.originalUnit.playerUnit.HasFamily((UnitFamily) pse.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id)) ? 1 : 0))) == 0)
            return false;
          return (effectUnit != enemy || !BattleFuncs.isSkillsAndEffectsInvalid(enemy, myself)) && !BattleFuncs.isEffectEnemyRangeAndInvalid(x, myself, enemy) && !BattleFuncs.isSkillsAndEffectsInvalid(myself, enemy);
        })))
          skillParams.Add(BattleFuncs.SkillParam.CreateAdd(effectUnit.originalUnit, effect, effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) * (float) effect.baseSkillLevel));
      });
      action(BattleskillEffectLogicEnum.white_night, myself, enemy, 0);
      action(BattleskillEffectLogicEnum.white_night, enemy, myself, 1);
      if (isDuelSkill)
      {
        action(BattleskillEffectLogicEnum.white_night2, myself, enemy, 0);
        action(BattleskillEffectLogicEnum.white_night2, enemy, myself, 1);
        action(BattleskillEffectLogicEnum.white_night3, myself, enemy, 0);
        action(BattleskillEffectLogicEnum.white_night3, enemy, myself, 1);
      }
      return BattleFuncs.calcSkillParamAddSingle(skillParams);
    }

    private static bool checkEnabledWhiteNight(
      BattleskillEffect x,
      BL.ISkillEffectListUnit effectUnit)
    {
      BattleskillEffect battleskillEffect = x;
      if (battleskillEffect.HasKey(BattleskillEffectLogicArgumentEnum.gear_kind_id) && battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != effectUnit.originalUnit.unit.kind.ID || battleskillEffect.HasKey(BattleskillEffectLogicArgumentEnum.element) && battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) != effectUnit.originalUnit.playerUnit.GetElement() || battleskillEffect.HasKey(BattleskillEffectLogicArgumentEnum.job_id) && battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) != 0 && battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) != effectUnit.originalUnit.job.ID)
        return false;
      return !battleskillEffect.HasKey(BattleskillEffectLogicArgumentEnum.family_id) || battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || effectUnit.originalUnit.playerUnit.HasFamily((UnitFamily) battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.family_id));
    }

    private static bool checkEnabledWhiteNight3(
      BattleskillEffect x,
      BL.ISkillEffectListUnit effectUnit)
    {
      return BattleFuncs.checkInvokeSkillEffectSelf(BattleFuncs.PackedSkillEffect.Create(x), effectUnit, dontCheckParamDiff: true, checkPassiveEffectEnabled: true);
    }

    public static IEnumerable<Tuple<BattleskillSkill, int, int>> getUnitAndGearSkills(BL.Unit unit)
    {
      bool disableSkills = unit.crippled;
      PlayerAwakeSkill[] source;
      if (unit.playerUnit.equippedExtraSkill == null || disableSkills)
        source = new PlayerAwakeSkill[0];
      else
        source = new PlayerAwakeSkill[1]
        {
          unit.playerUnit.equippedExtraSkill
        };
      IEnumerable<Tuple<BattleskillSkill, int, int>> first = ((IEnumerable<PlayerAwakeSkill>) source).Select<PlayerAwakeSkill, Tuple<BattleskillSkill, int, int>>((Func<PlayerAwakeSkill, Tuple<BattleskillSkill, int, int>>) (x => new Tuple<BattleskillSkill, int, int>(x.masterData, x.level, 0)));
      int[] princessSkills = unit.playerUnit.getPrincessSkillIds(disableSkills).ToArray<int>();
      IEnumerable<Tuple<BattleskillSkill, int, int>> tuples = first.Concat<Tuple<BattleskillSkill, int, int>>(((IEnumerable<PlayerUnitSkills>) unit.playerUnit.skills).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x =>
      {
        if (!disableSkills || BattleskillSkill.InvestElementSkillIds.Contains(x.skill_id))
          return true;
        return x.skill.skill_type == BattleskillSkillType.magic && !((IEnumerable<int>) princessSkills).Contains<int>(x.skill_id);
      })).Select<PlayerUnitSkills, Tuple<BattleskillSkill, int, int>>((Func<PlayerUnitSkills, Tuple<BattleskillSkill, int, int>>) (x => new Tuple<BattleskillSkill, int, int>(x.skill, x.level, 0))));
      if (unit.playerUnit.equippedGear != (PlayerItem) null && !disableSkills)
        tuples = tuples.Concat<Tuple<BattleskillSkill, int, int>>(((IEnumerable<GearGearSkill>) unit.playerUnit.equippedGear.skills).Select<GearGearSkill, Tuple<BattleskillSkill, int, int>>((Func<GearGearSkill, Tuple<BattleskillSkill, int, int>>) (x => new Tuple<BattleskillSkill, int, int>(x.skill, x.skill_level, 1))));
      if (unit.playerUnit.equippedGear2 != (PlayerItem) null && !disableSkills)
        tuples = tuples.Concat<Tuple<BattleskillSkill, int, int>>(((IEnumerable<GearGearSkill>) unit.playerUnit.equippedGear2.skills).Select<GearGearSkill, Tuple<BattleskillSkill, int, int>>((Func<GearGearSkill, Tuple<BattleskillSkill, int, int>>) (x => new Tuple<BattleskillSkill, int, int>(x.skill, x.skill_level, 2))));
      if (unit.playerUnit.equippedGear3 != (PlayerItem) null && !disableSkills)
        tuples = tuples.Concat<Tuple<BattleskillSkill, int, int>>(((IEnumerable<GearGearSkill>) unit.playerUnit.equippedGear3.skills).Select<GearGearSkill, Tuple<BattleskillSkill, int, int>>((Func<GearGearSkill, Tuple<BattleskillSkill, int, int>>) (x => new Tuple<BattleskillSkill, int, int>(x.skill, x.skill_level, 3))));
      if (unit.playerUnit.equippedReisou != (PlayerItem) null && !disableSkills)
        tuples = tuples.Concat<Tuple<BattleskillSkill, int, int>>(((IEnumerable<GearReisouSkill>) unit.playerUnit.equippedReisou.getReisouSkills(unit.playerUnit.equippedGear.entity_id)).Select<GearReisouSkill, Tuple<BattleskillSkill, int, int>>((Func<GearReisouSkill, Tuple<BattleskillSkill, int, int>>) (x => new Tuple<BattleskillSkill, int, int>(x.skill, x.skill_level, 1))));
      if (unit.playerUnit.equippedReisou2 != (PlayerItem) null && !disableSkills)
        tuples = tuples.Concat<Tuple<BattleskillSkill, int, int>>(((IEnumerable<GearReisouSkill>) unit.playerUnit.equippedReisou2.getReisouSkills(unit.playerUnit.equippedGear2.entity_id)).Select<GearReisouSkill, Tuple<BattleskillSkill, int, int>>((Func<GearReisouSkill, Tuple<BattleskillSkill, int, int>>) (x => new Tuple<BattleskillSkill, int, int>(x.skill, x.skill_level, 2))));
      if (unit.playerUnit.equippedReisou3 != (PlayerItem) null && !disableSkills)
        tuples = tuples.Concat<Tuple<BattleskillSkill, int, int>>(((IEnumerable<GearReisouSkill>) unit.playerUnit.equippedReisou3.getReisouSkills(unit.playerUnit.equippedGear3.entity_id)).Select<GearReisouSkill, Tuple<BattleskillSkill, int, int>>((Func<GearReisouSkill, Tuple<BattleskillSkill, int, int>>) (x => new Tuple<BattleskillSkill, int, int>(x.skill, x.skill_level, 3))));
      if (unit.playerUnit.equippedOverkillersSkills != null && !disableSkills)
        tuples = tuples.Concat<Tuple<BattleskillSkill, int, int>>(((IEnumerable<PlayerUnitSkills>) unit.playerUnit.equippedOverkillersSkills).Select<PlayerUnitSkills, Tuple<BattleskillSkill, int, int>>((Func<PlayerUnitSkills, Tuple<BattleskillSkill, int, int>>) (x => new Tuple<BattleskillSkill, int, int>(x.skill, x.level, 0))));
      return (IEnumerable<Tuple<BattleskillSkill, int, int>>) tuples.OrderByDescending<Tuple<BattleskillSkill, int, int>, int>((Func<Tuple<BattleskillSkill, int, int>, int>) (x => x.Item1.weight)).ThenBy<Tuple<BattleskillSkill, int, int>, int>((Func<Tuple<BattleskillSkill, int, int>, int>) (x => x.Item1.ID)).ThenByDescending<Tuple<BattleskillSkill, int, int>, int>((Func<Tuple<BattleskillSkill, int, int>, int>) (x => x.Item2));
    }

    public static bool isCriticalGuardEnable(
      BL.ISkillEffectListUnit myself,
      BL.ISkillEffectListUnit enemy,
      BL.Panel panel = null)
    {
      return myself.enabledSkillEffect(BattleskillEffectLogicEnum.critical_guard).Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x =>
      {
        BattleskillEffect effect = x.effect;
        return (!effect.HasKey(BattleskillEffectLogicArgumentEnum.gear_kind_id) || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == myself.originalUnit.unit.kind.ID) && (!effect.HasKey(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == enemy.originalUnit.unit.kind.ID) && (!effect.HasKey(BattleskillEffectLogicArgumentEnum.element) || effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == myself.originalUnit.playerUnit.GetElement()) && (!effect.HasKey(BattleskillEffectLogicArgumentEnum.target_element) || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == enemy.originalUnit.playerUnit.GetElement()) && (!effect.HasKey(BattleskillEffectLogicArgumentEnum.job_id) || effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == myself.originalUnit.job.ID) && (!effect.HasKey(BattleskillEffectLogicArgumentEnum.target_job_id) || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == enemy.originalUnit.job.ID) && (!effect.HasKey(BattleskillEffectLogicArgumentEnum.family_id) || effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || myself.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id))) && (!effect.HasKey(BattleskillEffectLogicArgumentEnum.target_family_id) || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) == 0 || enemy.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id))) && effect.GetPackedSkillEffect().CheckLandTag(panel, myself is BL.AIUnit);
      })).Any<BL.SkillEffect>() && !BattleFuncs.isSkillsAndEffectsInvalid(myself, enemy);
    }

    private static bool checkEnabledCriticalGuard(
      BattleskillEffect x,
      BL.ISkillEffectListUnit myself)
    {
      BattleskillEffect battleskillEffect = x;
      if (battleskillEffect.HasKey(BattleskillEffectLogicArgumentEnum.gear_kind_id) && battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != myself.originalUnit.unit.kind.ID || battleskillEffect.HasKey(BattleskillEffectLogicArgumentEnum.element) && battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) != myself.originalUnit.playerUnit.GetElement() || battleskillEffect.HasKey(BattleskillEffectLogicArgumentEnum.job_id) && battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) != 0 && battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) != myself.originalUnit.job.ID)
        return false;
      return !battleskillEffect.HasKey(BattleskillEffectLogicArgumentEnum.family_id) || battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || myself.originalUnit.playerUnit.HasFamily((UnitFamily) battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.family_id));
    }

    public static bool isEffectEnemyRangeAndInvalid(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit myself,
      BL.ISkillEffectListUnit enemy)
    {
      return !effect.isAttackMethod && effect.unit != (BL.Unit) null && effect.unit.index == enemy.originalUnit.index && effect.unit.isPlayerForce == enemy.originalUnit.isPlayerForce && (effect.effect.skill.skill_type == BattleskillSkillType.leader || effect.effect.skill.skill_type == BattleskillSkillType.passive && effect.effect.skill.range_effect_passive_skill) && BattleFuncs.isSkillsAndEffectsInvalid(enemy, myself);
    }

    private static void applyUseSkillEffect(
      BL.ISkillEffectListUnit unit,
      BL.UseSkillEffect useEffect,
      bool isAI,
      bool isColosseum,
      Dictionary<BL.ISkillEffectListUnit, List<BL.SkillEffect>> removeSkillEffects = null)
    {
      List<BL.SkillEffect> source1 = new List<BL.SkillEffect>((IEnumerable<BL.SkillEffect>) unit.skillEffects.All());
      if (removeSkillEffects != null && removeSkillEffects.ContainsKey(unit))
        source1.AddRange((IEnumerable<BL.SkillEffect>) removeSkillEffects[unit]);
      IEnumerable<BL.SkillEffect> source2 = source1.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x =>
      {
        if (x.effectId == useEffect.effectEffectId && x.baseSkillLevel == useEffect.effectBaseSkillLevel)
        {
          if (x.turnRemain.HasValue || useEffect.effectTurnRemain != -1)
          {
            int? turnRemain = x.turnRemain;
            int effectTurnRemain = useEffect.effectTurnRemain;
            if (!(turnRemain.GetValueOrDefault() == effectTurnRemain & turnRemain.HasValue))
              goto label_9;
          }
          if (x.unit == (BL.Unit) null && useEffect.effectUnitIndex == -1 || x.unit != (BL.Unit) null && useEffect.effectUnitIndex != -1 && x.unit.index == useEffect.effectUnitIndex && x.unit.isPlayerForce == useEffect.GetEffectUnitIsPlayerControl(isColosseum))
          {
            if (x.work == null && (double) useEffect.effectWork == 0.0)
              return true;
            return x.work != null && (double) useEffect.effectWork != 0.0 && (double) x.work[0] == (double) useEffect.effectWork;
          }
        }
label_9:
        return false;
      }));
      BL.SkillEffect skillEffect1 = useEffect.type != BL.UseSkillEffect.Type.Decrement ? source2.FirstOrDefault<BL.SkillEffect>() : source2.FirstOrDefault<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x =>
      {
        int? useRemain = x.useRemain;
        int num = 1;
        return useRemain.GetValueOrDefault() >= num & useRemain.HasValue;
      }));
      if (skillEffect1 == null)
        return;
      switch (useEffect.type)
      {
        case BL.UseSkillEffect.Type.Remove:
          skillEffect1.useRemain = new int?(0);
          if ((double) useEffect.work != 0.0)
            skillEffect1.work = new float[1]
            {
              useEffect.work
            };
          if (!isAI && skillEffect1.isLandTagEffect)
            unit.skillEffects.LandTagModified.commit();
          unit.skillEffects.commit();
          break;
        case BL.UseSkillEffect.Type.SetWork:
          skillEffect1.work = new float[1]{ useEffect.work };
          break;
        case BL.UseSkillEffect.Type.Decrement:
          if (!skillEffect1.useRemain.HasValue)
            break;
          int? useRemain1 = skillEffect1.useRemain;
          int num1 = 1;
          if (!(useRemain1.GetValueOrDefault() >= num1 & useRemain1.HasValue))
            break;
          BL.SkillEffect skillEffect2 = skillEffect1;
          int? useRemain2 = skillEffect2.useRemain;
          skillEffect2.useRemain = useRemain2.HasValue ? new int?(useRemain2.GetValueOrDefault() - 1) : new int?();
          int? useRemain3 = skillEffect1.useRemain;
          int num2 = 0;
          if (!(useRemain3.GetValueOrDefault() == num2 & useRemain3.HasValue))
            break;
          if (!isAI && skillEffect1.isLandTagEffect)
            unit.skillEffects.LandTagModified.commit();
          unit.skillEffects.commit();
          break;
      }
    }

    public static void consumeSkillEffects(
      BL.DuelTurn[] turns,
      BL.ISkillEffectListUnit atk,
      BL.ISkillEffectListUnit def,
      bool isColosseum)
    {
      bool isAI = atk is BL.AIUnit;
      foreach (BL.DuelTurn turn in turns)
      {
        List<int> duelSkillEffectIds1;
        List<int> duelSkillEffectIds2;
        List<BL.UseSkillEffect> useSkillEffectList1;
        List<BL.UseSkillEffect> useSkillEffectList2;
        if (turn.isAtackker)
        {
          duelSkillEffectIds1 = turn.invokeAttackerDuelSkillEffectIds;
          duelSkillEffectIds2 = turn.invokeDefenderDuelSkillEffectIds;
          useSkillEffectList1 = turn.attackerUseSkillEffects;
          useSkillEffectList2 = turn.defenderUseSkillEffects;
        }
        else
        {
          duelSkillEffectIds1 = turn.invokeDefenderDuelSkillEffectIds;
          duelSkillEffectIds2 = turn.invokeAttackerDuelSkillEffectIds;
          useSkillEffectList1 = turn.defenderUseSkillEffects;
          useSkillEffectList2 = turn.attackerUseSkillEffects;
        }
        if (duelSkillEffectIds1 != null)
        {
          foreach (int skillEffectId in duelSkillEffectIds1)
            atk.skillEffects.AddDuelSkillEffectIdInvokeCount(skillEffectId, 1);
        }
        if (duelSkillEffectIds2 != null)
        {
          foreach (int skillEffectId in duelSkillEffectIds2)
            def.skillEffects.AddDuelSkillEffectIdInvokeCount(skillEffectId, 1);
        }
        if (useSkillEffectList1 != null)
        {
          foreach (BL.UseSkillEffect useEffect in useSkillEffectList1)
            BattleFuncs.applyUseSkillEffect(atk, useEffect, isAI, isColosseum);
        }
        if (useSkillEffectList2 != null)
        {
          foreach (BL.UseSkillEffect useEffect in useSkillEffectList2)
            BattleFuncs.applyUseSkillEffect(def, useEffect, isAI, isColosseum);
        }
        int count = turn.useSkillUnit.Count;
        for (int index = 0; index < count; ++index)
        {
          BL.ISkillEffectListUnit originalUnit = turn.useSkillUnit[index];
          BL.UseSkillEffect useEffect = turn.useSkillEffect[index];
          if (!isAI)
            originalUnit = (BL.ISkillEffectListUnit) originalUnit.originalUnit;
          BattleFuncs.applyUseSkillEffect(originalUnit, useEffect, isAI, isColosseum);
        }
      }
    }

    public static void consumeSkillEffectsLate(
      BL.ISkillEffectListUnit atk,
      BL.ISkillEffectListUnit def,
      AttackStatus atkAttackStatus,
      AttackStatus defAttackStatus,
      bool isColosseum,
      Dictionary<BL.ISkillEffectListUnit, List<BL.SkillEffect>> removeSkillEffects = null)
    {
      bool isAI = atk is BL.AIUnit;
      List<BL.UseSkillEffect> source1 = new List<BL.UseSkillEffect>();
      List<BL.UseSkillEffect> source2 = new List<BL.UseSkillEffect>();
      if (atkAttackStatus != null)
      {
        source1.AddRange((IEnumerable<BL.UseSkillEffect>) atkAttackStatus.duelParameter.attackerUnitParameter.useSkillEffects);
        source2.AddRange((IEnumerable<BL.UseSkillEffect>) atkAttackStatus.duelParameter.defenderUnitParameter.useSkillEffects);
      }
      if (defAttackStatus != null)
      {
        List<BL.UseSkillEffect> collection = new List<BL.UseSkillEffect>();
        foreach (BL.UseSkillEffect useSkillEffect in defAttackStatus.duelParameter.defenderUnitParameter.useSkillEffects)
        {
          BL.UseSkillEffect useEffect = useSkillEffect;
          if (!source1.Any<BL.UseSkillEffect>((Func<BL.UseSkillEffect, bool>) (x => x.effectUniqueId == useEffect.effectUniqueId)))
            collection.Add(useEffect);
        }
        source1.AddRange((IEnumerable<BL.UseSkillEffect>) collection);
        collection.Clear();
        foreach (BL.UseSkillEffect useSkillEffect in defAttackStatus.duelParameter.attackerUnitParameter.useSkillEffects)
        {
          BL.UseSkillEffect useEffect = useSkillEffect;
          if (!source2.Any<BL.UseSkillEffect>((Func<BL.UseSkillEffect, bool>) (x => x.effectUniqueId == useEffect.effectUniqueId)))
            collection.Add(useEffect);
        }
        source2.AddRange((IEnumerable<BL.UseSkillEffect>) collection);
      }
      foreach (BL.UseSkillEffect useEffect in source1)
        BattleFuncs.applyUseSkillEffect(atk, useEffect, isAI, isColosseum, removeSkillEffects);
      foreach (BL.UseSkillEffect useEffect in source2)
        BattleFuncs.applyUseSkillEffect(def, useEffect, isAI, isColosseum, removeSkillEffects);
    }

    public static BL getEnv() => BattleFuncs.env;

    public static BL.ForceID[] getTargetForce(BL.Unit unit, bool isCharm)
    {
      return BattleFuncs.env.getTargetForce(unit, isCharm);
    }

    public static BL.ClassValue<List<BL.Unit>> forceUnits(BL.ForceID forceId)
    {
      return BattleFuncs.env.forceUnits(forceId);
    }

    public static BL.UnitPosition getUnitPosition(BL.ISkillEffectListUnit unit)
    {
      if (BattleFuncs.env == null)
        return (BL.UnitPosition) null;
      return BattleFuncs.env.unitPositions != null && BattleFuncs.env.unitPositions.value != null ? BattleFuncs.env.getUnitPosition(unit.originalUnit) : (BL.UnitPosition) null;
    }

    public static BL.UnitPosition[] getFieldUnitsAI(int row, int column, bool includeJumping = false)
    {
      return BattleFuncs.env.getFieldUnitsAI(row, column, includeJumping: includeJumping);
    }

    public static IEnumerable<BL.ISkillEffectListUnit> getInvestUnit(
      BL.ISkillEffectListUnit myself,
      BL.ISkillEffectListUnit enemy,
      int skillId,
      int rangeType,
      bool isAI,
      bool isColosseum)
    {
      BattleskillSkill battleskillSkill = MasterData.BattleskillSkill[skillId];
      if (isColosseum)
      {
        List<BL.ISkillEffectListUnit> skillEffectListUnitList = new List<BL.ISkillEffectListUnit>();
        switch (rangeType)
        {
          case 0:
            switch (battleskillSkill.target_type)
            {
              case BattleskillTargetType.myself:
              case BattleskillTargetType.player_single:
                skillEffectListUnitList.Add(myself);
                break;
              case BattleskillTargetType.player_range:
                if (battleskillSkill.min_range <= 0)
                {
                  skillEffectListUnitList.Add(myself);
                  break;
                }
                break;
              case BattleskillTargetType.enemy_single:
                skillEffectListUnitList.Add(enemy);
                break;
              case BattleskillTargetType.enemy_range:
                if (battleskillSkill.min_range <= 1 && battleskillSkill.max_range >= 1)
                {
                  skillEffectListUnitList.Add(enemy);
                  break;
                }
                break;
              case BattleskillTargetType.complex_single:
                if (((IEnumerable<BattleskillEffect>) battleskillSkill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => !x.is_targer_enemy)))
                  skillEffectListUnitList.Add(myself);
                if (((IEnumerable<BattleskillEffect>) battleskillSkill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.is_targer_enemy)))
                {
                  skillEffectListUnitList.Add(enemy);
                  break;
                }
                break;
              case BattleskillTargetType.complex_range:
                if (((IEnumerable<BattleskillEffect>) battleskillSkill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => !x.is_targer_enemy)) && battleskillSkill.min_range <= 0)
                  skillEffectListUnitList.Add(myself);
                if (((IEnumerable<BattleskillEffect>) battleskillSkill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.is_targer_enemy)) && battleskillSkill.min_range <= 1 && battleskillSkill.max_range >= 1)
                {
                  skillEffectListUnitList.Add(enemy);
                  break;
                }
                break;
            }
            break;
          case 1:
            switch (battleskillSkill.target_type)
            {
              case BattleskillTargetType.myself:
              case BattleskillTargetType.player_range:
              case BattleskillTargetType.player_single:
                if (battleskillSkill.min_range <= 0)
                {
                  skillEffectListUnitList.Add(myself);
                  break;
                }
                break;
              case BattleskillTargetType.enemy_single:
              case BattleskillTargetType.enemy_range:
                if (battleskillSkill.min_range <= 1 && battleskillSkill.max_range >= 1)
                {
                  skillEffectListUnitList.Add(enemy);
                  break;
                }
                break;
              case BattleskillTargetType.complex_single:
              case BattleskillTargetType.complex_range:
                if (((IEnumerable<BattleskillEffect>) battleskillSkill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => !x.is_targer_enemy)) && battleskillSkill.min_range <= 0)
                  skillEffectListUnitList.Add(myself);
                if (((IEnumerable<BattleskillEffect>) battleskillSkill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.is_targer_enemy)) && battleskillSkill.min_range <= 1 && battleskillSkill.max_range >= 1)
                {
                  skillEffectListUnitList.Add(enemy);
                  break;
                }
                break;
            }
            break;
          case 2:
            switch (battleskillSkill.target_type)
            {
              case BattleskillTargetType.myself:
              case BattleskillTargetType.player_range:
              case BattleskillTargetType.player_single:
                if (battleskillSkill.min_range <= 1 && battleskillSkill.max_range >= 1)
                {
                  skillEffectListUnitList.Add(myself);
                  break;
                }
                break;
              case BattleskillTargetType.enemy_single:
              case BattleskillTargetType.enemy_range:
                if (battleskillSkill.min_range <= 0)
                {
                  skillEffectListUnitList.Add(enemy);
                  break;
                }
                break;
              case BattleskillTargetType.complex_single:
              case BattleskillTargetType.complex_range:
                if (((IEnumerable<BattleskillEffect>) battleskillSkill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => !x.is_targer_enemy)) && battleskillSkill.min_range <= 1 && battleskillSkill.max_range >= 1)
                  skillEffectListUnitList.Add(myself);
                if (((IEnumerable<BattleskillEffect>) battleskillSkill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.is_targer_enemy)) && battleskillSkill.min_range <= 0)
                {
                  skillEffectListUnitList.Add(enemy);
                  break;
                }
                break;
            }
            break;
        }
        return (IEnumerable<BL.ISkillEffectListUnit>) skillEffectListUnitList.ToArray();
      }
      List<BL.UnitPosition> source = new List<BL.UnitPosition>();
      BL.ISkillEffectListUnit skillEffectListUnit;
      switch (rangeType)
      {
        case 0:
          switch (battleskillSkill.target_type)
          {
            case BattleskillTargetType.myself:
            case BattleskillTargetType.player_single:
              source = BattleFuncs.getRangeTargets(myself, new int[2], new BL.ForceID[1]
              {
                BattleFuncs.getForceID(myself.originalUnit)
              }, true).ToList<BL.UnitPosition>();
              goto label_65;
            case BattleskillTargetType.player_range:
              source = BattleFuncs.getRangeTargets(myself, new int[2]
              {
                battleskillSkill.min_range,
                battleskillSkill.max_range
              }, new BL.ForceID[1]
              {
                BattleFuncs.getForceID(myself.originalUnit)
              }, true).ToList<BL.UnitPosition>();
              goto label_65;
            case BattleskillTargetType.enemy_single:
              source = BattleFuncs.getRangeTargets(enemy, new int[2], new BL.ForceID[1]
              {
                BattleFuncs.getForceID(enemy.originalUnit)
              }, true).ToList<BL.UnitPosition>();
              goto label_65;
            case BattleskillTargetType.enemy_range:
              source = BattleFuncs.getRangeTargets(myself, new int[2]
              {
                battleskillSkill.min_range,
                battleskillSkill.max_range
              }, new BL.ForceID[1]
              {
                BattleFuncs.getForceID(enemy.originalUnit)
              }, true).ToList<BL.UnitPosition>();
              goto label_65;
            case BattleskillTargetType.complex_single:
              if (((IEnumerable<BattleskillEffect>) battleskillSkill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => !x.is_targer_enemy)))
                source.AddRange(BattleFuncs.getRangeTargets(myself, new int[2], new BL.ForceID[1]
                {
                  BattleFuncs.getForceID(myself.originalUnit)
                }, true));
              if (((IEnumerable<BattleskillEffect>) battleskillSkill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.is_targer_enemy)))
              {
                source.AddRange(BattleFuncs.getRangeTargets(enemy, new int[2], new BL.ForceID[1]
                {
                  BattleFuncs.getForceID(enemy.originalUnit)
                }, true));
                goto label_65;
              }
              else
                goto label_65;
            case BattleskillTargetType.complex_range:
              if (((IEnumerable<BattleskillEffect>) battleskillSkill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => !x.is_targer_enemy)))
                source.AddRange(BattleFuncs.getRangeTargets(myself, new int[2]
                {
                  battleskillSkill.min_range,
                  battleskillSkill.max_range
                }, new BL.ForceID[1]
                {
                  BattleFuncs.getForceID(myself.originalUnit)
                }, true));
              if (((IEnumerable<BattleskillEffect>) battleskillSkill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.is_targer_enemy)))
              {
                source.AddRange(BattleFuncs.getRangeTargets(myself, new int[2]
                {
                  battleskillSkill.min_range,
                  battleskillSkill.max_range
                }, new BL.ForceID[1]
                {
                  BattleFuncs.getForceID(enemy.originalUnit)
                }, true));
                goto label_65;
              }
              else
                goto label_65;
            default:
              goto label_65;
          }
        case 1:
          skillEffectListUnit = myself;
          break;
        case 2:
          skillEffectListUnit = enemy;
          break;
        default:
label_65:
          return source.Select<BL.UnitPosition, BL.ISkillEffectListUnit>((Func<BL.UnitPosition, BL.ISkillEffectListUnit>) (x => BattleFuncs.unitPositionToISkillEffectListUnit(x)));
      }
      BL.ISkillEffectListUnit unit = skillEffectListUnit;
      int[] range = new int[2]
      {
        battleskillSkill.min_range,
        battleskillSkill.max_range
      };
      switch (battleskillSkill.target_type)
      {
        case BattleskillTargetType.myself:
        case BattleskillTargetType.player_single:
          IEnumerable<BL.UnitPosition> rangeTargets1 = BattleFuncs.getRangeTargets(unit, range, new BL.ForceID[1]
          {
            BattleFuncs.getForceID(myself.originalUnit)
          }, true);
          source.AddRange(rangeTargets1.Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => BattleFuncs.unitPositionToISkillEffectListUnit(x) == myself)));
          goto label_65;
        case BattleskillTargetType.player_range:
          source = BattleFuncs.getRangeTargets(unit, range, new BL.ForceID[1]
          {
            BattleFuncs.getForceID(myself.originalUnit)
          }, true).ToList<BL.UnitPosition>();
          goto label_65;
        case BattleskillTargetType.enemy_single:
          IEnumerable<BL.UnitPosition> rangeTargets2 = BattleFuncs.getRangeTargets(unit, range, new BL.ForceID[1]
          {
            BattleFuncs.getForceID(enemy.originalUnit)
          }, true);
          source.AddRange(rangeTargets2.Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => BattleFuncs.unitPositionToISkillEffectListUnit(x) == enemy)));
          goto label_65;
        case BattleskillTargetType.enemy_range:
          source = BattleFuncs.getRangeTargets(unit, range, new BL.ForceID[1]
          {
            BattleFuncs.getForceID(enemy.originalUnit)
          }, true).ToList<BL.UnitPosition>();
          goto label_65;
        case BattleskillTargetType.complex_single:
          if (((IEnumerable<BattleskillEffect>) battleskillSkill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => !x.is_targer_enemy)))
          {
            IEnumerable<BL.UnitPosition> rangeTargets3 = BattleFuncs.getRangeTargets(unit, range, new BL.ForceID[1]
            {
              BattleFuncs.getForceID(myself.originalUnit)
            }, true);
            source.AddRange(rangeTargets3.Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => BattleFuncs.unitPositionToISkillEffectListUnit(x) == myself)));
          }
          if (((IEnumerable<BattleskillEffect>) battleskillSkill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.is_targer_enemy)))
          {
            IEnumerable<BL.UnitPosition> rangeTargets4 = BattleFuncs.getRangeTargets(unit, range, new BL.ForceID[1]
            {
              BattleFuncs.getForceID(enemy.originalUnit)
            }, true);
            source.AddRange(rangeTargets4.Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => BattleFuncs.unitPositionToISkillEffectListUnit(x) == enemy)));
            goto label_65;
          }
          else
            goto label_65;
        case BattleskillTargetType.complex_range:
          if (((IEnumerable<BattleskillEffect>) battleskillSkill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => !x.is_targer_enemy)))
            source.AddRange(BattleFuncs.getRangeTargets(unit, range, new BL.ForceID[1]
            {
              BattleFuncs.getForceID(myself.originalUnit)
            }, true));
          if (((IEnumerable<BattleskillEffect>) battleskillSkill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.is_targer_enemy)))
          {
            source.AddRange(BattleFuncs.getRangeTargets(unit, range, new BL.ForceID[1]
            {
              BattleFuncs.getForceID(enemy.originalUnit)
            }, true));
            goto label_65;
          }
          else
            goto label_65;
        default:
          goto label_65;
      }
    }

    public static void investSkillEffect(
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      int skillId,
      BL.SkillEffect investEffect,
      bool isAI,
      BattleFuncs.ApplyChangeSkillEffects applyChangeSkillEffects)
    {
      if (skillId == 0 || !MasterData.BattleskillSkill.ContainsKey(skillId))
        return;
      BL.Skill[] skillArray = (BL.Skill[]) null;
      bool flag1 = MasterData.BattleskillSkill[skillId].skill_type == BattleskillSkillType.ailment;
      if (flag1)
      {
        if (unit.originalUnit.isFacility && !unit.originalUnit.facility.isSkillUnit)
        {
          skillArray = BattleFuncs.ailmentInvest(skillId, target);
        }
        else
        {
          BL.SkillEffect perfectAilmentResist = BattleFuncs.getPerfectAilmentResist(BattleFuncs.getAilmentResistEffects(skillId, target, unit));
          if (perfectAilmentResist != null)
          {
            if (perfectAilmentResist.useRemain.HasValue)
            {
              BL.SkillEffect skillEffect = perfectAilmentResist;
              int? useRemain1 = skillEffect.useRemain;
              skillEffect.useRemain = useRemain1.HasValue ? new int?(useRemain1.GetValueOrDefault() - 1) : new int?();
              if (!isAI)
              {
                int? useRemain2 = perfectAilmentResist.useRemain;
                int num = 0;
                if (useRemain2.GetValueOrDefault() == num & useRemain2.HasValue)
                  target.originalUnit.commit();
              }
            }
          }
          else
            skillArray = BattleFuncs.ailmentInvest(skillId, target);
        }
      }
      else
        skillArray = new BL.Skill[1]
        {
          new BL.Skill() { id = skillId }
        };
      BL.SkillEffect[] skillEffectArray = (BL.SkillEffect[]) null;
      if (flag1 && !unit.originalUnit.isFacility)
      {
        BL.UnitPosition unitPosition = BattleFuncs.iSkillEffectListUnitToUnitPosition(target);
        skillEffectArray = BattleFuncs.getAilmentTriggerSkillEffects(skillId, target, unit, BattleFuncs.getPanel(unitPosition.row, unitPosition.column)).ToArray<BL.SkillEffect>();
      }
      if (skillArray != null)
      {
        foreach (BL.Skill skill in skillArray)
        {
          BL.Skill investSkill = skill;
          Action<BattleskillEffect> action = (Action<BattleskillEffect>) (e =>
          {
            target.skillEffects.Add(BL.SkillEffect.FromMasterData(e, investSkill.skill, 1, investUnit: unit.originalUnit, investSkillId: investEffect != null ? investEffect.baseSkillId : 0, investTurn: BattleFuncs.env.phaseState.absoluteTurnCount), checkEnableUnit: target);
            if (!isAI)
              target.originalUnit.commit();
            applyChangeSkillEffects.add(target);
          });
          if (investSkill.skill.target_type == BattleskillTargetType.complex_single || investSkill.skill.target_type == BattleskillTargetType.complex_range)
          {
            bool flag2 = BattleFuncs.getForceID(unit.originalUnit) != BattleFuncs.getForceID(target.originalUnit);
            foreach (BattleskillEffect effect in investSkill.skill.Effects)
            {
              if (effect.is_targer_enemy == flag2)
                action(effect);
            }
          }
          else
          {
            foreach (BattleskillEffect effect in investSkill.skill.Effects)
              action(effect);
          }
        }
      }
      if (skillEffectArray == null)
        return;
      foreach (BL.SkillEffect skillEffect in skillEffectArray)
      {
        int key = skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id);
        if (key != 0 && MasterData.BattleskillSkill.ContainsKey(key))
        {
          BattleskillSkill skill = MasterData.BattleskillSkill[key];
          foreach (BattleskillEffect effect in skill.Effects)
            target.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, skill, 1, investUnit: target.originalUnit, investSkillId: skillEffect.baseSkillId, investTurn: BattleFuncs.env.phaseState.absoluteTurnCount), checkEnableUnit: target);
          if (!isAI)
            target.originalUnit.commit();
          applyChangeSkillEffects.add(target);
        }
      }
    }

    private static IEnumerable<BL.SkillEffect> GetEnabledDeckEveryBuffDebuff(
      BL.SkillEffectList self,
      BattleskillEffectLogicEnum e,
      BL.ISkillEffectListUnit unit,
      IEnumerable<BL.Unit> deckUnits,
      Func<int, int, bool> condFunc)
    {
      return self.WhereAndGroupBy(e, (Func<IEnumerable<BL.SkillEffect>, BL.SkillEffect>) (x => x.OrderBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (effect => effect.effectId)).FirstOrDefault<BL.SkillEffect>()), (Func<IGrouping<int, BL.SkillEffect>, IEnumerable<BL.SkillEffect>>) (skills => skills.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (effect =>
      {
        int num1 = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.type);
        Dictionary<int, int> dictionary = new Dictionary<int, int>();
        for (int index = 1; index <= 7; ++index)
        {
          BattleskillEffectLogicArgumentEnum logicArgumentEnum = BattleskillEffectLogic.GetLogicArgumentEnum("kind_count" + (object) index);
          if (logicArgumentEnum != BattleskillEffectLogicArgumentEnum.none)
          {
            int num2 = effect.effect.GetInt(logicArgumentEnum);
            if (num2 != 0)
            {
              int key = num2 / 10;
              int num3 = num2 % 10;
              dictionary[key] = num3;
            }
          }
        }
        foreach (int key in dictionary.Keys)
        {
          int kind = key;
          int num4 = 0;
          switch (num1)
          {
            case 0:
              num4 = deckUnits.Count<BL.Unit>((Func<BL.Unit, bool>) (x => x.originalUnit.unit.kind.ID == kind));
              break;
            case 1:
              num4 = deckUnits.Count<BL.Unit>((Func<BL.Unit, bool>) (x => x.originalUnit.playerUnit.GetElement() == (CommonElement) kind));
              break;
            case 2:
              num4 = deckUnits.Count<BL.Unit>((Func<BL.Unit, bool>) (x => x.originalUnit.job.ID == kind));
              break;
            case 3:
              num4 = deckUnits.Count<BL.Unit>((Func<BL.Unit, bool>) (x => x.originalUnit.playerUnit.HasFamily((UnitFamily) kind)));
              break;
            case 4:
              num4 = deckUnits.Count<BL.Unit>((Func<BL.Unit, bool>) (x => x.unit.character.ID == kind));
              break;
            case 5:
              num4 = deckUnits.Count<BL.Unit>((Func<BL.Unit, bool>) (x => x.unit.same_character_id == kind));
              break;
            case 6:
              num4 = deckUnits.Count<BL.Unit>((Func<BL.Unit, bool>) (x => x.unit.ID == kind));
              break;
            case 7:
              num4 = deckUnits.Count<BL.Unit>((Func<BL.Unit, bool>) (x => x.originalUnit.unitGroup != null && x.originalUnit.unitGroup.group_large_category_id.ID == kind));
              break;
            case 8:
              num4 = deckUnits.Count<BL.Unit>((Func<BL.Unit, bool>) (x => x.originalUnit.unitGroup != null && x.originalUnit.unitGroup.group_small_category_id.ID == kind));
              break;
            case 9:
              num4 = deckUnits.Count<BL.Unit>((Func<BL.Unit, bool>) (x =>
              {
                if (x.originalUnit.unitGroup == null)
                  return false;
                return x.originalUnit.unitGroup.group_clothing_category_id.ID == kind || x.originalUnit.unitGroup.group_clothing_category_id_2.ID == kind;
              }));
              break;
            case 10:
              num4 = deckUnits.Count<BL.Unit>((Func<BL.Unit, bool>) (x => x.originalUnit.unitGroup != null && x.originalUnit.unitGroup.group_generation_category_id.ID == kind));
              break;
            case 11:
              num4 = deckUnits.Count<BL.Unit>((Func<BL.Unit, bool>) (x => x.originalUnit.unit.HasSkillGroupId(kind)));
              break;
          }
          if (!condFunc(num4, dictionary[kind]))
            return false;
        }
        return true;
      }))));
    }

    private static IEnumerable<BL.SkillEffect> GetEnabledDeckEveryGeBuffDebuff(
      BL.SkillEffectList self,
      BattleskillEffectLogicEnum e,
      BL.ISkillEffectListUnit unit,
      IEnumerable<BL.Unit> deckUnits)
    {
      return BattleFuncs.GetEnabledDeckEveryBuffDebuff(self, e, unit, deckUnits, (Func<int, int, bool>) ((deckCount, condCount) => deckCount >= condCount));
    }

    private static IEnumerable<BL.SkillEffect> GetEnabledDeckEveryLeBuffDebuff(
      BL.SkillEffectList self,
      BattleskillEffectLogicEnum e,
      BL.ISkillEffectListUnit unit,
      IEnumerable<BL.Unit> deckUnits)
    {
      return BattleFuncs.GetEnabledDeckEveryBuffDebuff(self, e, unit, deckUnits, (Func<int, int, bool>) ((deckCount, condCount) => deckCount <= condCount));
    }

    private static IEnumerable<BL.SkillEffect> GetEnabledDeckSameAnotherBuffDebuff(
      BL.SkillEffectList self,
      BattleskillEffectLogicEnum e,
      BL.ISkillEffectListUnit unit,
      IEnumerable<BL.Unit> deckUnits,
      Func<IEnumerable<int>, int, bool> condFunc)
    {
      return self.WhereAndGroupBy(e, (Func<IEnumerable<BL.SkillEffect>, BL.SkillEffect>) (x => x.OrderBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (effect => effect.effectId)).FirstOrDefault<BL.SkillEffect>()), (Func<IGrouping<int, BL.SkillEffect>, IEnumerable<BL.SkillEffect>>) (skills => skills.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (effect =>
      {
        int num1 = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.kind_count);
        IEnumerable<int> source = (IEnumerable<int>) null;
        List<BattleskillEffect> effects = BattleFuncs.PackedSkillEffect.Create(effect).GetEffects();
        if (effects.Skip<BattleskillEffect>(1).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.HasKey(BattleskillEffectLogicArgumentEnum.type))))
        {
          BL.Unit[] array = deckUnits.Where<BL.Unit>((Func<BL.Unit, bool>) (x =>
          {
            foreach (BattleskillEffect battleskillEffect in effects)
            {
              int num2 = battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.type);
              int typeId = battleskillEffect.HasKey(BattleskillEffectLogicArgumentEnum.type_id) ? battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.type_id) : 0;
              bool flag = false;
              switch (num2)
              {
                case 0:
                  if (typeId == 0 || x.originalUnit.unit.kind.ID == typeId)
                  {
                    flag = true;
                    break;
                  }
                  break;
                case 1:
                  if (typeId == 0 || x.originalUnit.playerUnit.GetElement() == (CommonElement) typeId)
                  {
                    flag = true;
                    break;
                  }
                  break;
                case 2:
                  if (typeId == 0 || x.originalUnit.job.ID == typeId)
                  {
                    flag = true;
                    break;
                  }
                  break;
                case 3:
                  if (typeId == 0 || x.originalUnit.playerUnit.HasFamily((UnitFamily) typeId))
                  {
                    flag = true;
                    break;
                  }
                  break;
                case 4:
                  if (typeId == 0 || x.unit.character.ID == typeId)
                  {
                    flag = true;
                    break;
                  }
                  break;
                case 5:
                  if (typeId == 0 || x.unit.same_character_id == typeId)
                  {
                    flag = true;
                    break;
                  }
                  break;
                case 6:
                  if (typeId == 0 || x.unit.ID == typeId)
                  {
                    flag = true;
                    break;
                  }
                  break;
                case 7:
                  if (x.originalUnit.unitGroup != null && (typeId == 0 || x.originalUnit.unitGroup.group_large_category_id.ID == typeId))
                  {
                    flag = true;
                    break;
                  }
                  break;
                case 8:
                  if (x.originalUnit.unitGroup != null && (typeId == 0 || x.originalUnit.unitGroup.group_small_category_id.ID == typeId))
                  {
                    flag = true;
                    break;
                  }
                  break;
                case 9:
                  if (x.originalUnit.unitGroup != null && (typeId == 0 || x.originalUnit.unitGroup.group_clothing_category_id.ID == typeId || x.originalUnit.unitGroup.group_clothing_category_id_2.ID == typeId))
                  {
                    flag = true;
                    break;
                  }
                  break;
                case 10:
                  if (x.originalUnit.unitGroup != null && (typeId == 0 || x.originalUnit.unitGroup.group_generation_category_id.ID == typeId))
                  {
                    flag = true;
                    break;
                  }
                  break;
                case 11:
                  if (typeId == 0 || ((IEnumerable<int>) x.originalUnit.unit.SkillGroupIds).Any<int>((Func<int, bool>) (y => y == typeId)))
                  {
                    flag = true;
                    break;
                  }
                  break;
                case 12:
                  if (typeId == 0 || x.originalUnit.playerUnit.unit_type.ID == typeId)
                  {
                    flag = true;
                    break;
                  }
                  break;
              }
              if (!flag)
                return false;
            }
            return true;
          })).ToArray<BL.Unit>();
          if (array.Length >= 1)
            source = (IEnumerable<int>) new int[1]
            {
              array.Length
            };
        }
        else
        {
          int num3 = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.type);
          int typeId = effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.type_id) ? effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.type_id) : 0;
          switch (num3)
          {
            case 0:
              source = deckUnits.Where<BL.Unit>((Func<BL.Unit, bool>) (x => typeId == 0 || x.originalUnit.unit.kind.ID == typeId)).GroupBy<BL.Unit, int>((Func<BL.Unit, int>) (x => x.originalUnit.unit.kind.ID)).Select<IGrouping<int, BL.Unit>, int>((Func<IGrouping<int, BL.Unit>, int>) (x => x.Count<BL.Unit>()));
              break;
            case 1:
              source = deckUnits.Where<BL.Unit>((Func<BL.Unit, bool>) (x => typeId == 0 || x.originalUnit.playerUnit.GetElement() == (CommonElement) typeId)).GroupBy<BL.Unit, int>((Func<BL.Unit, int>) (x => (int) x.originalUnit.playerUnit.GetElement())).Select<IGrouping<int, BL.Unit>, int>((Func<IGrouping<int, BL.Unit>, int>) (x => x.Count<BL.Unit>()));
              break;
            case 2:
              source = deckUnits.Where<BL.Unit>((Func<BL.Unit, bool>) (x => typeId == 0 || x.originalUnit.job.ID == typeId)).GroupBy<BL.Unit, int>((Func<BL.Unit, int>) (x => x.originalUnit.job.ID)).Select<IGrouping<int, BL.Unit>, int>((Func<IGrouping<int, BL.Unit>, int>) (x => x.Count<BL.Unit>()));
              break;
            case 3:
              source = deckUnits.SelectMany<BL.Unit, UnitFamily>((Func<BL.Unit, IEnumerable<UnitFamily>>) (x => (IEnumerable<UnitFamily>) x.originalUnit.playerUnit.Families)).Where<UnitFamily>((Func<UnitFamily, bool>) (x => typeId == 0 || x == (UnitFamily) typeId)).GroupBy<UnitFamily, UnitFamily>((Func<UnitFamily, UnitFamily>) (x => x)).Select<IGrouping<UnitFamily, UnitFamily>, int>((Func<IGrouping<UnitFamily, UnitFamily>, int>) (x => x.Count<UnitFamily>()));
              break;
            case 4:
              source = deckUnits.Where<BL.Unit>((Func<BL.Unit, bool>) (x => typeId == 0 || x.unit.character.ID == typeId)).GroupBy<BL.Unit, int>((Func<BL.Unit, int>) (x => x.unit.character.ID)).Select<IGrouping<int, BL.Unit>, int>((Func<IGrouping<int, BL.Unit>, int>) (x => x.Count<BL.Unit>()));
              break;
            case 5:
              source = deckUnits.Where<BL.Unit>((Func<BL.Unit, bool>) (x => typeId == 0 || x.unit.same_character_id == typeId)).GroupBy<BL.Unit, int>((Func<BL.Unit, int>) (x => x.unit.same_character_id)).Select<IGrouping<int, BL.Unit>, int>((Func<IGrouping<int, BL.Unit>, int>) (x => x.Count<BL.Unit>()));
              break;
            case 6:
              source = deckUnits.Where<BL.Unit>((Func<BL.Unit, bool>) (x => typeId == 0 || x.unit.ID == typeId)).GroupBy<BL.Unit, int>((Func<BL.Unit, int>) (x => x.unit.ID)).Select<IGrouping<int, BL.Unit>, int>((Func<IGrouping<int, BL.Unit>, int>) (x => x.Count<BL.Unit>()));
              break;
            case 7:
              source = deckUnits.Where<BL.Unit>((Func<BL.Unit, bool>) (x =>
              {
                if (x.originalUnit.unitGroup == null)
                  return false;
                return typeId == 0 || x.originalUnit.unitGroup.group_large_category_id.ID == typeId;
              })).GroupBy<BL.Unit, int>((Func<BL.Unit, int>) (x => x.originalUnit.unitGroup.group_large_category_id.ID)).Select<IGrouping<int, BL.Unit>, int>((Func<IGrouping<int, BL.Unit>, int>) (x => x.Count<BL.Unit>()));
              break;
            case 8:
              source = deckUnits.Where<BL.Unit>((Func<BL.Unit, bool>) (x =>
              {
                if (x.originalUnit.unitGroup == null)
                  return false;
                return typeId == 0 || x.originalUnit.unitGroup.group_small_category_id.ID == typeId;
              })).GroupBy<BL.Unit, int>((Func<BL.Unit, int>) (x => x.originalUnit.unitGroup.group_small_category_id.ID)).Select<IGrouping<int, BL.Unit>, int>((Func<IGrouping<int, BL.Unit>, int>) (x => x.Count<BL.Unit>()));
              break;
            case 9:
              source = deckUnits.Where<BL.Unit>((Func<BL.Unit, bool>) (x =>
              {
                if (x.originalUnit.unitGroup == null)
                  return false;
                return typeId == 0 || x.originalUnit.unitGroup.group_clothing_category_id.ID == typeId || x.originalUnit.unitGroup.group_clothing_category_id_2.ID == typeId;
              })).Select<BL.Unit, List<int>>((Func<BL.Unit, List<int>>) (x =>
              {
                List<int> anotherBuffDebuff = new List<int>();
                if (typeId == 0 || x.originalUnit.unitGroup.group_clothing_category_id.ID == typeId)
                  anotherBuffDebuff.Add(x.originalUnit.unitGroup.group_clothing_category_id.ID);
                if (typeId == 0 || x.originalUnit.unitGroup.group_clothing_category_id_2.ID == typeId)
                  anotherBuffDebuff.Add(x.originalUnit.unitGroup.group_clothing_category_id_2.ID);
                return anotherBuffDebuff;
              })).SelectMany<List<int>, int>((Func<List<int>, IEnumerable<int>>) (x => (IEnumerable<int>) x)).GroupBy<int, int>((Func<int, int>) (x => x)).Select<IGrouping<int, int>, int>((Func<IGrouping<int, int>, int>) (x => x.Count<int>()));
              break;
            case 10:
              source = deckUnits.Where<BL.Unit>((Func<BL.Unit, bool>) (x =>
              {
                if (x.originalUnit.unitGroup == null)
                  return false;
                return typeId == 0 || x.originalUnit.unitGroup.group_generation_category_id.ID == typeId;
              })).GroupBy<BL.Unit, int>((Func<BL.Unit, int>) (x => x.originalUnit.unitGroup.group_generation_category_id.ID)).Select<IGrouping<int, BL.Unit>, int>((Func<IGrouping<int, BL.Unit>, int>) (x => x.Count<BL.Unit>()));
              break;
            case 11:
              source = deckUnits.SelectMany<BL.Unit, int>((Func<BL.Unit, IEnumerable<int>>) (x => (IEnumerable<int>) x.originalUnit.unit.SkillGroupIds)).Where<int>((Func<int, bool>) (x => typeId == 0 || x == typeId)).GroupBy<int, int>((Func<int, int>) (x => x)).Select<IGrouping<int, int>, int>((Func<IGrouping<int, int>, int>) (x => x.Count<int>()));
              break;
            case 12:
              source = deckUnits.Where<BL.Unit>((Func<BL.Unit, bool>) (x => typeId == 0 || x.originalUnit.playerUnit.unit_type.ID == typeId)).GroupBy<BL.Unit, int>((Func<BL.Unit, int>) (x => x.originalUnit.playerUnit.unit_type.ID)).Select<IGrouping<int, BL.Unit>, int>((Func<IGrouping<int, BL.Unit>, int>) (x => x.Count<BL.Unit>()));
              break;
          }
        }
        return source != null && source.Any<int>() && condFunc(source, num1);
      }))));
    }

    private static IEnumerable<BL.SkillEffect> GetEnabledDeckAnotherGeBuffDebuff(
      BL.SkillEffectList self,
      BattleskillEffectLogicEnum e,
      BL.ISkillEffectListUnit unit,
      IEnumerable<BL.Unit> deckUnits)
    {
      return BattleFuncs.GetEnabledDeckSameAnotherBuffDebuff(self, e, unit, deckUnits, (Func<IEnumerable<int>, int, bool>) ((count, kindCount) => count.Count<int>() >= kindCount));
    }

    private static IEnumerable<BL.SkillEffect> GetEnabledDeckAnotherLeBuffDebuff(
      BL.SkillEffectList self,
      BattleskillEffectLogicEnum e,
      BL.ISkillEffectListUnit unit,
      IEnumerable<BL.Unit> deckUnits)
    {
      return BattleFuncs.GetEnabledDeckSameAnotherBuffDebuff(self, e, unit, deckUnits, (Func<IEnumerable<int>, int, bool>) ((count, kindCount) => count.Count<int>() <= kindCount));
    }

    private static IEnumerable<BL.SkillEffect> GetEnabledDeckSameGeBuffDebuff(
      BL.SkillEffectList self,
      BattleskillEffectLogicEnum e,
      BL.ISkillEffectListUnit unit,
      IEnumerable<BL.Unit> deckUnits)
    {
      return BattleFuncs.GetEnabledDeckSameAnotherBuffDebuff(self, e, unit, deckUnits, (Func<IEnumerable<int>, int, bool>) ((count, kindCount) => count.OrderByDescending<int, int>((Func<int, int>) (x => x)).First<int>() >= kindCount));
    }

    private static IEnumerable<BL.SkillEffect> GetEnabledDeckSameLeBuffDebuff(
      BL.SkillEffectList self,
      BattleskillEffectLogicEnum e,
      BL.ISkillEffectListUnit unit,
      IEnumerable<BL.Unit> deckUnits)
    {
      return BattleFuncs.GetEnabledDeckSameAnotherBuffDebuff(self, e, unit, deckUnits, (Func<IEnumerable<int>, int, bool>) ((count, kindCount) => count.OrderByDescending<int, int>((Func<int, int>) (x => x)).First<int>() <= kindCount));
    }

    private static void GetDeckEveryGeSkillAdd(
      BL.ISkillEffectListUnit beUnit,
      BattleskillEffectLogicEnum fix_logic,
      IEnumerable<BL.Unit> deckUnits)
    {
      foreach (BL.SkillEffect effect in BattleFuncs.GetEnabledDeckEveryGeBuffDebuff(beUnit.skillEffects, fix_logic, beUnit, deckUnits))
        beUnit.skillEffects.AddFixEffectParam(fix_logic, effect, effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.value) + effect.baseSkillLevel * effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_ratio));
    }

    private static void GetDeckEveryGeSkillMul(
      BL.ISkillEffectListUnit beUnit,
      BattleskillEffectLogicEnum ratio_logic,
      IEnumerable<BL.Unit> deckUnits)
    {
      foreach (BL.SkillEffect effect in BattleFuncs.GetEnabledDeckEveryGeBuffDebuff(beUnit.skillEffects, ratio_logic, beUnit, deckUnits))
        beUnit.skillEffects.AddRatioEffectParam(ratio_logic, effect, effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) + (float) effect.baseSkillLevel * effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio));
    }

    private static void GetDeckEveryLeSkillAdd(
      BL.ISkillEffectListUnit beUnit,
      BattleskillEffectLogicEnum fix_logic,
      IEnumerable<BL.Unit> deckUnits)
    {
      foreach (BL.SkillEffect effect in BattleFuncs.GetEnabledDeckEveryLeBuffDebuff(beUnit.skillEffects, fix_logic, beUnit, deckUnits))
        beUnit.skillEffects.AddFixEffectParam(fix_logic, effect, effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.value) + effect.baseSkillLevel * effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_ratio));
    }

    private static void GetDeckEveryLeSkillMul(
      BL.ISkillEffectListUnit beUnit,
      BattleskillEffectLogicEnum ratio_logic,
      IEnumerable<BL.Unit> deckUnits)
    {
      foreach (BL.SkillEffect effect in BattleFuncs.GetEnabledDeckEveryLeBuffDebuff(beUnit.skillEffects, ratio_logic, beUnit, deckUnits))
        beUnit.skillEffects.AddRatioEffectParam(ratio_logic, effect, effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) + (float) effect.baseSkillLevel * effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio));
    }

    private static void GetDeckAnotherGeSkillAdd(
      BL.ISkillEffectListUnit beUnit,
      BattleskillEffectLogicEnum fix_logic,
      IEnumerable<BL.Unit> deckUnits)
    {
      foreach (BL.SkillEffect effect in BattleFuncs.GetEnabledDeckAnotherGeBuffDebuff(beUnit.skillEffects, fix_logic, beUnit, deckUnits))
        beUnit.skillEffects.AddFixEffectParam(fix_logic, effect, effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.value) + effect.baseSkillLevel * effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_ratio));
    }

    private static void GetDeckAnotherGeSkillMul(
      BL.ISkillEffectListUnit beUnit,
      BattleskillEffectLogicEnum ratio_logic,
      IEnumerable<BL.Unit> deckUnits)
    {
      foreach (BL.SkillEffect effect in BattleFuncs.GetEnabledDeckAnotherGeBuffDebuff(beUnit.skillEffects, ratio_logic, beUnit, deckUnits))
        beUnit.skillEffects.AddRatioEffectParam(ratio_logic, effect, effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) + (float) effect.baseSkillLevel * effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio));
    }

    private static void GetDeckAnotherLeSkillAdd(
      BL.ISkillEffectListUnit beUnit,
      BattleskillEffectLogicEnum fix_logic,
      IEnumerable<BL.Unit> deckUnits)
    {
      foreach (BL.SkillEffect effect in BattleFuncs.GetEnabledDeckAnotherLeBuffDebuff(beUnit.skillEffects, fix_logic, beUnit, deckUnits))
        beUnit.skillEffects.AddFixEffectParam(fix_logic, effect, effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.value) + effect.baseSkillLevel * effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_ratio));
    }

    private static void GetDeckAnotherLeSkillMul(
      BL.ISkillEffectListUnit beUnit,
      BattleskillEffectLogicEnum ratio_logic,
      IEnumerable<BL.Unit> deckUnits)
    {
      foreach (BL.SkillEffect effect in BattleFuncs.GetEnabledDeckAnotherLeBuffDebuff(beUnit.skillEffects, ratio_logic, beUnit, deckUnits))
        beUnit.skillEffects.AddRatioEffectParam(ratio_logic, effect, effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) + (float) effect.baseSkillLevel * effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio));
    }

    private static void GetDeckSameGeSkillAdd(
      BL.ISkillEffectListUnit beUnit,
      BattleskillEffectLogicEnum fix_logic,
      IEnumerable<BL.Unit> deckUnits)
    {
      foreach (BL.SkillEffect effect in BattleFuncs.GetEnabledDeckSameGeBuffDebuff(beUnit.skillEffects, fix_logic, beUnit, deckUnits))
        beUnit.skillEffects.AddFixEffectParam(fix_logic, effect, effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.value) + effect.baseSkillLevel * effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_ratio));
    }

    private static void GetDeckSameGeSkillMul(
      BL.ISkillEffectListUnit beUnit,
      BattleskillEffectLogicEnum ratio_logic,
      IEnumerable<BL.Unit> deckUnits)
    {
      foreach (BL.SkillEffect effect in BattleFuncs.GetEnabledDeckSameGeBuffDebuff(beUnit.skillEffects, ratio_logic, beUnit, deckUnits))
        beUnit.skillEffects.AddRatioEffectParam(ratio_logic, effect, effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) + (float) effect.baseSkillLevel * effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio));
    }

    private static void GetDeckSameLeSkillAdd(
      BL.ISkillEffectListUnit beUnit,
      BattleskillEffectLogicEnum fix_logic,
      IEnumerable<BL.Unit> deckUnits)
    {
      foreach (BL.SkillEffect effect in BattleFuncs.GetEnabledDeckSameLeBuffDebuff(beUnit.skillEffects, fix_logic, beUnit, deckUnits))
        beUnit.skillEffects.AddFixEffectParam(fix_logic, effect, effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.value) + effect.baseSkillLevel * effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_ratio));
    }

    private static void GetDeckSameLeSkillMul(
      BL.ISkillEffectListUnit beUnit,
      BattleskillEffectLogicEnum ratio_logic,
      IEnumerable<BL.Unit> deckUnits)
    {
      foreach (BL.SkillEffect effect in BattleFuncs.GetEnabledDeckSameLeBuffDebuff(beUnit.skillEffects, ratio_logic, beUnit, deckUnits))
        beUnit.skillEffects.AddRatioEffectParam(ratio_logic, effect, effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) + (float) effect.baseSkillLevel * effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio));
    }

    public static void createBattleSkillEffectParams(
      BL.ISkillEffectListUnit beUnit,
      IEnumerable<BL.Unit> deckUnits,
      IEnumerable<BL.Unit> opponentDeckUnits)
    {
      bool flag = false;
      foreach (BattleskillEffectLogicEnum battleskillEffectLogicEnum in BattleFuncs.DeckEveryGeSkillAddEnum)
      {
        BattleFuncs.GetDeckEveryGeSkillAdd(beUnit, battleskillEffectLogicEnum, deckUnits);
        flag |= beUnit.skillEffects.RemoveEffect(beUnit.skillEffects.Where(battleskillEffectLogicEnum).Except<BL.SkillEffect>(beUnit.skillEffects.GetFixEffectParams(battleskillEffectLogicEnum).Select<Tuple<BL.SkillEffect, int>, BL.SkillEffect>((Func<Tuple<BL.SkillEffect, int>, BL.SkillEffect>) (x => x.Item1))));
      }
      foreach (BattleskillEffectLogicEnum battleskillEffectLogicEnum in BattleFuncs.DeckEveryGeSkillMulEnum)
      {
        BattleFuncs.GetDeckEveryGeSkillMul(beUnit, battleskillEffectLogicEnum, deckUnits);
        flag |= beUnit.skillEffects.RemoveEffect(beUnit.skillEffects.Where(battleskillEffectLogicEnum).Except<BL.SkillEffect>(beUnit.skillEffects.GetRatioEffectParams(battleskillEffectLogicEnum).Select<Tuple<BL.SkillEffect, float>, BL.SkillEffect>((Func<Tuple<BL.SkillEffect, float>, BL.SkillEffect>) (x => x.Item1))));
      }
      foreach (BattleskillEffectLogicEnum battleskillEffectLogicEnum in BattleFuncs.DeckEveryLeSkillAddEnum)
      {
        BattleFuncs.GetDeckEveryLeSkillAdd(beUnit, battleskillEffectLogicEnum, deckUnits);
        flag |= beUnit.skillEffects.RemoveEffect(beUnit.skillEffects.Where(battleskillEffectLogicEnum).Except<BL.SkillEffect>(beUnit.skillEffects.GetFixEffectParams(battleskillEffectLogicEnum).Select<Tuple<BL.SkillEffect, int>, BL.SkillEffect>((Func<Tuple<BL.SkillEffect, int>, BL.SkillEffect>) (x => x.Item1))));
      }
      foreach (BattleskillEffectLogicEnum battleskillEffectLogicEnum in BattleFuncs.DeckEveryLeSkillMulEnum)
      {
        BattleFuncs.GetDeckEveryLeSkillMul(beUnit, battleskillEffectLogicEnum, deckUnits);
        flag |= beUnit.skillEffects.RemoveEffect(beUnit.skillEffects.Where(battleskillEffectLogicEnum).Except<BL.SkillEffect>(beUnit.skillEffects.GetRatioEffectParams(battleskillEffectLogicEnum).Select<Tuple<BL.SkillEffect, float>, BL.SkillEffect>((Func<Tuple<BL.SkillEffect, float>, BL.SkillEffect>) (x => x.Item1))));
      }
      foreach (BattleskillEffectLogicEnum battleskillEffectLogicEnum in BattleFuncs.DeckAnotherGeSkillAddEnum)
      {
        BattleFuncs.GetDeckAnotherGeSkillAdd(beUnit, battleskillEffectLogicEnum, deckUnits);
        flag |= beUnit.skillEffects.RemoveEffect(beUnit.skillEffects.Where(battleskillEffectLogicEnum).Except<BL.SkillEffect>(beUnit.skillEffects.GetFixEffectParams(battleskillEffectLogicEnum).Select<Tuple<BL.SkillEffect, int>, BL.SkillEffect>((Func<Tuple<BL.SkillEffect, int>, BL.SkillEffect>) (x => x.Item1))));
      }
      foreach (BattleskillEffectLogicEnum battleskillEffectLogicEnum in BattleFuncs.DeckAnotherGeSkillMulEnum)
      {
        BattleFuncs.GetDeckAnotherGeSkillMul(beUnit, battleskillEffectLogicEnum, deckUnits);
        flag |= beUnit.skillEffects.RemoveEffect(beUnit.skillEffects.Where(battleskillEffectLogicEnum).Except<BL.SkillEffect>(beUnit.skillEffects.GetRatioEffectParams(battleskillEffectLogicEnum).Select<Tuple<BL.SkillEffect, float>, BL.SkillEffect>((Func<Tuple<BL.SkillEffect, float>, BL.SkillEffect>) (x => x.Item1))));
      }
      foreach (BattleskillEffectLogicEnum battleskillEffectLogicEnum in BattleFuncs.DeckAnotherLeSkillAddEnum)
      {
        BattleFuncs.GetDeckAnotherLeSkillAdd(beUnit, battleskillEffectLogicEnum, deckUnits);
        flag |= beUnit.skillEffects.RemoveEffect(beUnit.skillEffects.Where(battleskillEffectLogicEnum).Except<BL.SkillEffect>(beUnit.skillEffects.GetFixEffectParams(battleskillEffectLogicEnum).Select<Tuple<BL.SkillEffect, int>, BL.SkillEffect>((Func<Tuple<BL.SkillEffect, int>, BL.SkillEffect>) (x => x.Item1))));
      }
      foreach (BattleskillEffectLogicEnum battleskillEffectLogicEnum in BattleFuncs.DeckAnotherLeSkillMulEnum)
      {
        BattleFuncs.GetDeckAnotherLeSkillMul(beUnit, battleskillEffectLogicEnum, deckUnits);
        flag |= beUnit.skillEffects.RemoveEffect(beUnit.skillEffects.Where(battleskillEffectLogicEnum).Except<BL.SkillEffect>(beUnit.skillEffects.GetRatioEffectParams(battleskillEffectLogicEnum).Select<Tuple<BL.SkillEffect, float>, BL.SkillEffect>((Func<Tuple<BL.SkillEffect, float>, BL.SkillEffect>) (x => x.Item1))));
      }
      foreach (BattleskillEffectLogicEnum battleskillEffectLogicEnum in BattleFuncs.DeckSameGeSkillAddEnum)
      {
        BattleFuncs.GetDeckSameGeSkillAdd(beUnit, battleskillEffectLogicEnum, deckUnits);
        flag |= beUnit.skillEffects.RemoveEffect(beUnit.skillEffects.Where(battleskillEffectLogicEnum).Except<BL.SkillEffect>(beUnit.skillEffects.GetFixEffectParams(battleskillEffectLogicEnum).Select<Tuple<BL.SkillEffect, int>, BL.SkillEffect>((Func<Tuple<BL.SkillEffect, int>, BL.SkillEffect>) (x => x.Item1))));
      }
      foreach (BattleskillEffectLogicEnum battleskillEffectLogicEnum in BattleFuncs.DeckSameGeSkillMulEnum)
      {
        BattleFuncs.GetDeckSameGeSkillMul(beUnit, battleskillEffectLogicEnum, deckUnits);
        flag |= beUnit.skillEffects.RemoveEffect(beUnit.skillEffects.Where(battleskillEffectLogicEnum).Except<BL.SkillEffect>(beUnit.skillEffects.GetRatioEffectParams(battleskillEffectLogicEnum).Select<Tuple<BL.SkillEffect, float>, BL.SkillEffect>((Func<Tuple<BL.SkillEffect, float>, BL.SkillEffect>) (x => x.Item1))));
      }
      foreach (BattleskillEffectLogicEnum battleskillEffectLogicEnum in BattleFuncs.DeckSameLeSkillAddEnum)
      {
        BattleFuncs.GetDeckSameLeSkillAdd(beUnit, battleskillEffectLogicEnum, deckUnits);
        flag |= beUnit.skillEffects.RemoveEffect(beUnit.skillEffects.Where(battleskillEffectLogicEnum).Except<BL.SkillEffect>(beUnit.skillEffects.GetFixEffectParams(battleskillEffectLogicEnum).Select<Tuple<BL.SkillEffect, int>, BL.SkillEffect>((Func<Tuple<BL.SkillEffect, int>, BL.SkillEffect>) (x => x.Item1))));
      }
      foreach (BattleskillEffectLogicEnum battleskillEffectLogicEnum in BattleFuncs.DeckSameLeSkillMulEnum)
      {
        BattleFuncs.GetDeckSameLeSkillMul(beUnit, battleskillEffectLogicEnum, deckUnits);
        flag |= beUnit.skillEffects.RemoveEffect(beUnit.skillEffects.Where(battleskillEffectLogicEnum).Except<BL.SkillEffect>(beUnit.skillEffects.GetRatioEffectParams(battleskillEffectLogicEnum).Select<Tuple<BL.SkillEffect, float>, BL.SkillEffect>((Func<Tuple<BL.SkillEffect, float>, BL.SkillEffect>) (x => x.Item1))));
      }
      foreach (BattleskillEffectLogicEnum battleskillEffectLogicEnum in BattleFuncs.DeckOpponentEveryGeSkillAddEnum)
      {
        BattleFuncs.GetDeckEveryGeSkillAdd(beUnit, battleskillEffectLogicEnum, opponentDeckUnits);
        flag |= beUnit.skillEffects.RemoveEffect(beUnit.skillEffects.Where(battleskillEffectLogicEnum).Except<BL.SkillEffect>(beUnit.skillEffects.GetFixEffectParams(battleskillEffectLogicEnum).Select<Tuple<BL.SkillEffect, int>, BL.SkillEffect>((Func<Tuple<BL.SkillEffect, int>, BL.SkillEffect>) (x => x.Item1))));
      }
      foreach (BattleskillEffectLogicEnum battleskillEffectLogicEnum in BattleFuncs.DeckOpponentEveryGeSkillMulEnum)
      {
        BattleFuncs.GetDeckEveryGeSkillMul(beUnit, battleskillEffectLogicEnum, opponentDeckUnits);
        flag |= beUnit.skillEffects.RemoveEffect(beUnit.skillEffects.Where(battleskillEffectLogicEnum).Except<BL.SkillEffect>(beUnit.skillEffects.GetRatioEffectParams(battleskillEffectLogicEnum).Select<Tuple<BL.SkillEffect, float>, BL.SkillEffect>((Func<Tuple<BL.SkillEffect, float>, BL.SkillEffect>) (x => x.Item1))));
      }
      foreach (BattleskillEffectLogicEnum battleskillEffectLogicEnum in BattleFuncs.DeckOpponentEveryLeSkillAddEnum)
      {
        BattleFuncs.GetDeckEveryLeSkillAdd(beUnit, battleskillEffectLogicEnum, opponentDeckUnits);
        flag |= beUnit.skillEffects.RemoveEffect(beUnit.skillEffects.Where(battleskillEffectLogicEnum).Except<BL.SkillEffect>(beUnit.skillEffects.GetFixEffectParams(battleskillEffectLogicEnum).Select<Tuple<BL.SkillEffect, int>, BL.SkillEffect>((Func<Tuple<BL.SkillEffect, int>, BL.SkillEffect>) (x => x.Item1))));
      }
      foreach (BattleskillEffectLogicEnum battleskillEffectLogicEnum in BattleFuncs.DeckOpponentEveryLeSkillMulEnum)
      {
        BattleFuncs.GetDeckEveryLeSkillMul(beUnit, battleskillEffectLogicEnum, opponentDeckUnits);
        flag |= beUnit.skillEffects.RemoveEffect(beUnit.skillEffects.Where(battleskillEffectLogicEnum).Except<BL.SkillEffect>(beUnit.skillEffects.GetRatioEffectParams(battleskillEffectLogicEnum).Select<Tuple<BL.SkillEffect, float>, BL.SkillEffect>((Func<Tuple<BL.SkillEffect, float>, BL.SkillEffect>) (x => x.Item1))));
      }
      foreach (BattleskillEffectLogicEnum battleskillEffectLogicEnum in BattleFuncs.DeckOpponentAnotherGeSkillAddEnum)
      {
        BattleFuncs.GetDeckAnotherGeSkillAdd(beUnit, battleskillEffectLogicEnum, opponentDeckUnits);
        flag |= beUnit.skillEffects.RemoveEffect(beUnit.skillEffects.Where(battleskillEffectLogicEnum).Except<BL.SkillEffect>(beUnit.skillEffects.GetFixEffectParams(battleskillEffectLogicEnum).Select<Tuple<BL.SkillEffect, int>, BL.SkillEffect>((Func<Tuple<BL.SkillEffect, int>, BL.SkillEffect>) (x => x.Item1))));
      }
      foreach (BattleskillEffectLogicEnum battleskillEffectLogicEnum in BattleFuncs.DeckOpponentAnotherGeSkillMulEnum)
      {
        BattleFuncs.GetDeckAnotherGeSkillMul(beUnit, battleskillEffectLogicEnum, opponentDeckUnits);
        flag |= beUnit.skillEffects.RemoveEffect(beUnit.skillEffects.Where(battleskillEffectLogicEnum).Except<BL.SkillEffect>(beUnit.skillEffects.GetRatioEffectParams(battleskillEffectLogicEnum).Select<Tuple<BL.SkillEffect, float>, BL.SkillEffect>((Func<Tuple<BL.SkillEffect, float>, BL.SkillEffect>) (x => x.Item1))));
      }
      foreach (BattleskillEffectLogicEnum battleskillEffectLogicEnum in BattleFuncs.DeckOpponentAnotherLeSkillAddEnum)
      {
        BattleFuncs.GetDeckAnotherLeSkillAdd(beUnit, battleskillEffectLogicEnum, opponentDeckUnits);
        flag |= beUnit.skillEffects.RemoveEffect(beUnit.skillEffects.Where(battleskillEffectLogicEnum).Except<BL.SkillEffect>(beUnit.skillEffects.GetFixEffectParams(battleskillEffectLogicEnum).Select<Tuple<BL.SkillEffect, int>, BL.SkillEffect>((Func<Tuple<BL.SkillEffect, int>, BL.SkillEffect>) (x => x.Item1))));
      }
      foreach (BattleskillEffectLogicEnum battleskillEffectLogicEnum in BattleFuncs.DeckOpponentAnotherLeSkillMulEnum)
      {
        BattleFuncs.GetDeckAnotherLeSkillMul(beUnit, battleskillEffectLogicEnum, opponentDeckUnits);
        flag |= beUnit.skillEffects.RemoveEffect(beUnit.skillEffects.Where(battleskillEffectLogicEnum).Except<BL.SkillEffect>(beUnit.skillEffects.GetRatioEffectParams(battleskillEffectLogicEnum).Select<Tuple<BL.SkillEffect, float>, BL.SkillEffect>((Func<Tuple<BL.SkillEffect, float>, BL.SkillEffect>) (x => x.Item1))));
      }
      foreach (BattleskillEffectLogicEnum battleskillEffectLogicEnum in BattleFuncs.DeckOpponentSameGeSkillAddEnum)
      {
        BattleFuncs.GetDeckSameGeSkillAdd(beUnit, battleskillEffectLogicEnum, opponentDeckUnits);
        flag |= beUnit.skillEffects.RemoveEffect(beUnit.skillEffects.Where(battleskillEffectLogicEnum).Except<BL.SkillEffect>(beUnit.skillEffects.GetFixEffectParams(battleskillEffectLogicEnum).Select<Tuple<BL.SkillEffect, int>, BL.SkillEffect>((Func<Tuple<BL.SkillEffect, int>, BL.SkillEffect>) (x => x.Item1))));
      }
      foreach (BattleskillEffectLogicEnum battleskillEffectLogicEnum in BattleFuncs.DeckOpponentSameGeSkillMulEnum)
      {
        BattleFuncs.GetDeckSameGeSkillMul(beUnit, battleskillEffectLogicEnum, opponentDeckUnits);
        flag |= beUnit.skillEffects.RemoveEffect(beUnit.skillEffects.Where(battleskillEffectLogicEnum).Except<BL.SkillEffect>(beUnit.skillEffects.GetRatioEffectParams(battleskillEffectLogicEnum).Select<Tuple<BL.SkillEffect, float>, BL.SkillEffect>((Func<Tuple<BL.SkillEffect, float>, BL.SkillEffect>) (x => x.Item1))));
      }
      foreach (BattleskillEffectLogicEnum battleskillEffectLogicEnum in BattleFuncs.DeckOpponentSameLeSkillAddEnum)
      {
        BattleFuncs.GetDeckSameLeSkillAdd(beUnit, battleskillEffectLogicEnum, opponentDeckUnits);
        flag |= beUnit.skillEffects.RemoveEffect(beUnit.skillEffects.Where(battleskillEffectLogicEnum).Except<BL.SkillEffect>(beUnit.skillEffects.GetFixEffectParams(battleskillEffectLogicEnum).Select<Tuple<BL.SkillEffect, int>, BL.SkillEffect>((Func<Tuple<BL.SkillEffect, int>, BL.SkillEffect>) (x => x.Item1))));
      }
      foreach (BattleskillEffectLogicEnum battleskillEffectLogicEnum in BattleFuncs.DeckOpponentSameLeSkillMulEnum)
      {
        BattleFuncs.GetDeckSameLeSkillMul(beUnit, battleskillEffectLogicEnum, opponentDeckUnits);
        flag |= beUnit.skillEffects.RemoveEffect(beUnit.skillEffects.Where(battleskillEffectLogicEnum).Except<BL.SkillEffect>(beUnit.skillEffects.GetRatioEffectParams(battleskillEffectLogicEnum).Select<Tuple<BL.SkillEffect, float>, BL.SkillEffect>((Func<Tuple<BL.SkillEffect, float>, BL.SkillEffect>) (x => x.Item1))));
      }
      Func<BattleFuncs.PackedSkillEffect, BattleskillEffectLogicArgumentEnum, BattleskillEffectLogicArgumentEnum, BattleskillEffectLogicArgumentEnum, Func<int, int, bool>, IEnumerable<BL.Unit>, bool> func1 = (Func<BattleFuncs.PackedSkillEffect, BattleskillEffectLogicArgumentEnum, BattleskillEffectLogicArgumentEnum, BattleskillEffectLogicArgumentEnum, Func<int, int, bool>, IEnumerable<BL.Unit>, bool>) ((pse, typeArg, effectTypeArg, kindArg, condFunc, units) =>
      {
        int type = pse.GetInt(typeArg);
        Dictionary<int, int> dictionary = new Dictionary<int, int>();
        for (int index = 1; index <= 7; ++index)
        {
          int num1 = pse.GetInt(kindArg);
          if (num1 != 0)
          {
            int key = num1 / 10;
            int num2 = num1 % 10;
            dictionary[key] = num2;
          }
          ++kindArg;
        }
        Func<BL.Unit, int, bool> isTarget = (Func<BL.Unit, int, bool>) ((unit, kind) =>
        {
          switch (type)
          {
            case 0:
              return unit.originalUnit.unit.kind.ID == kind;
            case 1:
              return unit.originalUnit.playerUnit.GetElement() == (CommonElement) kind;
            case 2:
              return unit.originalUnit.job.ID == kind;
            case 3:
              return unit.originalUnit.playerUnit.HasFamily((UnitFamily) kind);
            case 4:
              return unit.unit.character.ID == kind;
            case 5:
              return unit.unit.same_character_id == kind;
            case 6:
              return unit.unit.ID == kind;
            case 7:
              return unit.originalUnit.unitGroup != null && unit.originalUnit.unitGroup.group_large_category_id.ID == kind;
            case 8:
              return unit.originalUnit.unitGroup != null && unit.originalUnit.unitGroup.group_small_category_id.ID == kind;
            case 9:
              if (unit.originalUnit.unitGroup == null)
                return false;
              return unit.originalUnit.unitGroup.group_clothing_category_id.ID == kind || unit.originalUnit.unitGroup.group_clothing_category_id_2.ID == kind;
            case 10:
              return unit.originalUnit.unitGroup != null && unit.originalUnit.unitGroup.group_generation_category_id.ID == kind;
            case 11:
              return unit.originalUnit.unit.HasSkillGroupId(kind);
            default:
              return false;
          }
        });
        int num3 = pse.GetInt(effectTypeArg);
        bool skillEffectParams = num3 == 0;
        foreach (int key in dictionary.Keys)
        {
          int kind = key;
          if (num3 == 1 && !skillEffectParams)
            skillEffectParams |= isTarget(beUnit.originalUnit, kind);
          int num4 = units.Count<BL.Unit>((Func<BL.Unit, bool>) (x => isTarget(x, kind)));
          if (!condFunc(num4, dictionary[kind]))
            return false;
        }
        return skillEffectParams;
      });
      HashSet<BL.SkillEffect> skillEffectSet1 = new HashSet<BL.SkillEffect>();
      List<BL.SkillEffect> source1 = new List<BL.SkillEffect>();
      foreach (BL.SkillEffect skillEffect in beUnit.skillEffects.All())
      {
        BattleFuncs.PackedSkillEffect packedSkillEffect = skillEffect.effect.GetPackedSkillEffect();
        if (packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.deck_every_ge_type))
        {
          if (!func1(packedSkillEffect, BattleskillEffectLogicArgumentEnum.deck_every_ge_type, BattleskillEffectLogicArgumentEnum.deck_every_ge_effect_type, BattleskillEffectLogicArgumentEnum.deck_every_ge_kind_count1, (Func<int, int, bool>) ((deckCount, condCount) => deckCount >= condCount), deckUnits))
            skillEffectSet1.Add(skillEffect);
          else
            source1.Add(skillEffect);
        }
        if (packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.deck_every_le_type))
        {
          if (!func1(packedSkillEffect, BattleskillEffectLogicArgumentEnum.deck_every_le_type, BattleskillEffectLogicArgumentEnum.deck_every_le_effect_type, BattleskillEffectLogicArgumentEnum.deck_every_le_kind_count1, (Func<int, int, bool>) ((deckCount, condCount) => deckCount <= condCount), deckUnits))
            skillEffectSet1.Add(skillEffect);
          else
            source1.Add(skillEffect);
        }
        if (packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.deck_opponent_every_ge_type))
        {
          if (!func1(packedSkillEffect, BattleskillEffectLogicArgumentEnum.deck_opponent_every_ge_type, BattleskillEffectLogicArgumentEnum.deck_opponent_every_ge_effect_type, BattleskillEffectLogicArgumentEnum.deck_opponent_every_ge_kind_count1, (Func<int, int, bool>) ((deckCount, condCount) => deckCount >= condCount), opponentDeckUnits))
            skillEffectSet1.Add(skillEffect);
          else
            source1.Add(skillEffect);
        }
        if (packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.deck_opponent_every_le_type))
        {
          if (!func1(packedSkillEffect, BattleskillEffectLogicArgumentEnum.deck_opponent_every_le_type, BattleskillEffectLogicArgumentEnum.deck_opponent_every_le_effect_type, BattleskillEffectLogicArgumentEnum.deck_opponent_every_le_kind_count1, (Func<int, int, bool>) ((deckCount, condCount) => deckCount <= condCount), opponentDeckUnits))
            skillEffectSet1.Add(skillEffect);
          else
            source1.Add(skillEffect);
        }
      }
      if (source1.Any<BL.SkillEffect>())
      {
        foreach (IEnumerable<BL.SkillEffect> source2 in source1.GroupBy(x => new
        {
          baseSkillId = x.baseSkillId,
          effectId = x.effectId
        }))
        {
          foreach (BL.SkillEffect skillEffect in source2.Skip<BL.SkillEffect>(1))
            skillEffectSet1.Add(skillEffect);
        }
      }
      if (skillEffectSet1.Any<BL.SkillEffect>())
        flag |= beUnit.skillEffects.RemoveEffect((IEnumerable<BL.SkillEffect>) skillEffectSet1);
      Func<BattleFuncs.PackedSkillEffect, BattleskillEffectLogicArgumentEnum, BattleskillEffectLogicArgumentEnum, Func<IEnumerable<int>, int, bool>, IEnumerable<BL.Unit>, bool> func2 = (Func<BattleFuncs.PackedSkillEffect, BattleskillEffectLogicArgumentEnum, BattleskillEffectLogicArgumentEnum, Func<IEnumerable<int>, int, bool>, IEnumerable<BL.Unit>, bool>) ((pse, typeArg, kindCountArg, condFunc, units) =>
      {
        int num5 = pse.GetInt(kindCountArg);
        IEnumerable<int> source3 = (IEnumerable<int>) null;
        int num6 = pse.GetInt(typeArg);
        int typeId = 0;
        switch (num6)
        {
          case 0:
            source3 = units.Where<BL.Unit>((Func<BL.Unit, bool>) (x => typeId == 0 || x.originalUnit.unit.kind.ID == typeId)).GroupBy<BL.Unit, int>((Func<BL.Unit, int>) (x => x.originalUnit.unit.kind.ID)).Select<IGrouping<int, BL.Unit>, int>((Func<IGrouping<int, BL.Unit>, int>) (x => x.Count<BL.Unit>()));
            break;
          case 1:
            source3 = units.Where<BL.Unit>((Func<BL.Unit, bool>) (x => typeId == 0 || x.originalUnit.playerUnit.GetElement() == (CommonElement) typeId)).GroupBy<BL.Unit, int>((Func<BL.Unit, int>) (x => (int) x.originalUnit.playerUnit.GetElement())).Select<IGrouping<int, BL.Unit>, int>((Func<IGrouping<int, BL.Unit>, int>) (x => x.Count<BL.Unit>()));
            break;
          case 2:
            source3 = units.Where<BL.Unit>((Func<BL.Unit, bool>) (x => typeId == 0 || x.originalUnit.job.ID == typeId)).GroupBy<BL.Unit, int>((Func<BL.Unit, int>) (x => x.originalUnit.job.ID)).Select<IGrouping<int, BL.Unit>, int>((Func<IGrouping<int, BL.Unit>, int>) (x => x.Count<BL.Unit>()));
            break;
          case 3:
            source3 = units.SelectMany<BL.Unit, UnitFamily>((Func<BL.Unit, IEnumerable<UnitFamily>>) (x => (IEnumerable<UnitFamily>) x.originalUnit.playerUnit.Families)).Where<UnitFamily>((Func<UnitFamily, bool>) (x => typeId == 0 || x == (UnitFamily) typeId)).GroupBy<UnitFamily, UnitFamily>((Func<UnitFamily, UnitFamily>) (x => x)).Select<IGrouping<UnitFamily, UnitFamily>, int>((Func<IGrouping<UnitFamily, UnitFamily>, int>) (x => x.Count<UnitFamily>()));
            break;
          case 4:
            source3 = units.Where<BL.Unit>((Func<BL.Unit, bool>) (x => typeId == 0 || x.unit.character.ID == typeId)).GroupBy<BL.Unit, int>((Func<BL.Unit, int>) (x => x.unit.character.ID)).Select<IGrouping<int, BL.Unit>, int>((Func<IGrouping<int, BL.Unit>, int>) (x => x.Count<BL.Unit>()));
            break;
          case 5:
            source3 = units.Where<BL.Unit>((Func<BL.Unit, bool>) (x => typeId == 0 || x.unit.same_character_id == typeId)).GroupBy<BL.Unit, int>((Func<BL.Unit, int>) (x => x.unit.same_character_id)).Select<IGrouping<int, BL.Unit>, int>((Func<IGrouping<int, BL.Unit>, int>) (x => x.Count<BL.Unit>()));
            break;
          case 6:
            source3 = units.Where<BL.Unit>((Func<BL.Unit, bool>) (x => typeId == 0 || x.unit.ID == typeId)).GroupBy<BL.Unit, int>((Func<BL.Unit, int>) (x => x.unit.ID)).Select<IGrouping<int, BL.Unit>, int>((Func<IGrouping<int, BL.Unit>, int>) (x => x.Count<BL.Unit>()));
            break;
          case 7:
            source3 = units.Where<BL.Unit>((Func<BL.Unit, bool>) (x =>
            {
              if (x.originalUnit.unitGroup == null)
                return false;
              return typeId == 0 || x.originalUnit.unitGroup.group_large_category_id.ID == typeId;
            })).GroupBy<BL.Unit, int>((Func<BL.Unit, int>) (x => x.originalUnit.unitGroup.group_large_category_id.ID)).Select<IGrouping<int, BL.Unit>, int>((Func<IGrouping<int, BL.Unit>, int>) (x => x.Count<BL.Unit>()));
            break;
          case 8:
            source3 = units.Where<BL.Unit>((Func<BL.Unit, bool>) (x =>
            {
              if (x.originalUnit.unitGroup == null)
                return false;
              return typeId == 0 || x.originalUnit.unitGroup.group_small_category_id.ID == typeId;
            })).GroupBy<BL.Unit, int>((Func<BL.Unit, int>) (x => x.originalUnit.unitGroup.group_small_category_id.ID)).Select<IGrouping<int, BL.Unit>, int>((Func<IGrouping<int, BL.Unit>, int>) (x => x.Count<BL.Unit>()));
            break;
          case 9:
            source3 = units.Where<BL.Unit>((Func<BL.Unit, bool>) (x =>
            {
              if (x.originalUnit.unitGroup == null)
                return false;
              return typeId == 0 || x.originalUnit.unitGroup.group_clothing_category_id.ID == typeId || x.originalUnit.unitGroup.group_clothing_category_id_2.ID == typeId;
            })).Select<BL.Unit, List<int>>((Func<BL.Unit, List<int>>) (x =>
            {
              List<int> skillEffectParams = new List<int>();
              if (typeId == 0 || x.originalUnit.unitGroup.group_clothing_category_id.ID == typeId)
                skillEffectParams.Add(x.originalUnit.unitGroup.group_clothing_category_id.ID);
              if (typeId == 0 || x.originalUnit.unitGroup.group_clothing_category_id_2.ID == typeId)
                skillEffectParams.Add(x.originalUnit.unitGroup.group_clothing_category_id_2.ID);
              return skillEffectParams;
            })).SelectMany<List<int>, int>((Func<List<int>, IEnumerable<int>>) (x => (IEnumerable<int>) x)).GroupBy<int, int>((Func<int, int>) (x => x)).Select<IGrouping<int, int>, int>((Func<IGrouping<int, int>, int>) (x => x.Count<int>()));
            break;
          case 10:
            source3 = units.Where<BL.Unit>((Func<BL.Unit, bool>) (x =>
            {
              if (x.originalUnit.unitGroup == null)
                return false;
              return typeId == 0 || x.originalUnit.unitGroup.group_generation_category_id.ID == typeId;
            })).GroupBy<BL.Unit, int>((Func<BL.Unit, int>) (x => x.originalUnit.unitGroup.group_generation_category_id.ID)).Select<IGrouping<int, BL.Unit>, int>((Func<IGrouping<int, BL.Unit>, int>) (x => x.Count<BL.Unit>()));
            break;
          case 11:
            source3 = units.SelectMany<BL.Unit, int>((Func<BL.Unit, IEnumerable<int>>) (x => (IEnumerable<int>) x.originalUnit.unit.SkillGroupIds)).Where<int>((Func<int, bool>) (x => typeId == 0 || x == typeId)).GroupBy<int, int>((Func<int, int>) (x => x)).Select<IGrouping<int, int>, int>((Func<IGrouping<int, int>, int>) (x => x.Count<int>()));
            break;
          case 12:
            source3 = units.Where<BL.Unit>((Func<BL.Unit, bool>) (x => typeId == 0 || x.originalUnit.playerUnit.unit_type.ID == typeId)).GroupBy<BL.Unit, int>((Func<BL.Unit, int>) (x => x.originalUnit.playerUnit.unit_type.ID)).Select<IGrouping<int, BL.Unit>, int>((Func<IGrouping<int, BL.Unit>, int>) (x => x.Count<BL.Unit>()));
            break;
        }
        return source3 != null && source3.Any<int>() && condFunc(source3, num5);
      });
      HashSet<BL.SkillEffect> skillEffectSet2 = new HashSet<BL.SkillEffect>();
      List<BL.SkillEffect> source4 = new List<BL.SkillEffect>();
      foreach (BL.SkillEffect skillEffect in beUnit.skillEffects.All())
      {
        BattleFuncs.PackedSkillEffect packedSkillEffect = skillEffect.effect.GetPackedSkillEffect();
        if (packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.deck_another_ge_type))
        {
          if (!func2(packedSkillEffect, BattleskillEffectLogicArgumentEnum.deck_another_ge_type, BattleskillEffectLogicArgumentEnum.deck_another_ge_kind_count, (Func<IEnumerable<int>, int, bool>) ((deckCount, condCount) => deckCount.Count<int>() >= condCount), deckUnits))
            skillEffectSet2.Add(skillEffect);
          else
            source4.Add(skillEffect);
        }
        if (packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.deck_another_le_type))
        {
          if (!func2(packedSkillEffect, BattleskillEffectLogicArgumentEnum.deck_another_le_type, BattleskillEffectLogicArgumentEnum.deck_another_le_kind_count, (Func<IEnumerable<int>, int, bool>) ((deckCount, condCount) => deckCount.Count<int>() <= condCount), deckUnits))
            skillEffectSet2.Add(skillEffect);
          else
            source4.Add(skillEffect);
        }
        if (packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.deck_opponent_another_ge_type))
        {
          if (!func2(packedSkillEffect, BattleskillEffectLogicArgumentEnum.deck_opponent_another_ge_type, BattleskillEffectLogicArgumentEnum.deck_opponent_another_ge_kind_count, (Func<IEnumerable<int>, int, bool>) ((deckCount, condCount) => deckCount.Count<int>() >= condCount), opponentDeckUnits))
            skillEffectSet2.Add(skillEffect);
          else
            source4.Add(skillEffect);
        }
        if (packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.deck_opponent_another_le_type))
        {
          if (!func2(packedSkillEffect, BattleskillEffectLogicArgumentEnum.deck_opponent_another_le_type, BattleskillEffectLogicArgumentEnum.deck_opponent_another_le_kind_count, (Func<IEnumerable<int>, int, bool>) ((deckCount, condCount) => deckCount.Count<int>() <= condCount), opponentDeckUnits))
            skillEffectSet2.Add(skillEffect);
          else
            source4.Add(skillEffect);
        }
      }
      if (source4.Any<BL.SkillEffect>())
      {
        foreach (IEnumerable<BL.SkillEffect> source5 in source4.GroupBy(x => new
        {
          baseSkillId = x.baseSkillId,
          effectId = x.effectId
        }))
        {
          foreach (BL.SkillEffect skillEffect in source5.Skip<BL.SkillEffect>(1))
            skillEffectSet2.Add(skillEffect);
        }
      }
      if (skillEffectSet2.Any<BL.SkillEffect>())
        flag |= beUnit.skillEffects.RemoveEffect((IEnumerable<BL.SkillEffect>) skillEffectSet2);
      if (flag)
        beUnit.skillEffects.ClearCache();
      beUnit.skillEffects.ResetTransformationSkillEffects(0);
    }

    public static bool isSealedSkillEffect(BL.ISkillEffectListUnit unit, BL.SkillEffect effect)
    {
      if (effect.isAttackMethod)
        return false;
      BL.Unit unit1 = effect.unit;
      if (unit1 != (BL.Unit) null)
      {
        BattleskillSkill skill = effect.effect.skill;
        if (skill.skill_type == BattleskillSkillType.leader || skill.skill_type == BattleskillSkillType.passive && skill.range_effect_passive_skill)
        {
          if (unit is BL.AIUnit)
          {
            foreach (BL.AIUnit aiUnit in BattleFuncs.env.aiUnitPositions.value)
            {
              if (aiUnit.originalUnit.index == unit1.index && aiUnit.originalUnit.isPlayerForce == unit1.isPlayerForce)
                return aiUnit.IsDontUseSkillEffect(effect);
            }
          }
          return unit1.IsDontUseSkillEffect(effect);
        }
      }
      return unit.IsDontUseSkillEffect(effect);
    }

    public static IEnumerable<BL.ISkillEffectListUnit> getForceUnits(
      BL.ForceID forceId,
      bool isAI,
      bool isEnableOnly,
      bool includeFacilities = false,
      bool includeJumping = false)
    {
      if (isAI)
      {
        IEnumerable<BL.AIUnit> source = (IEnumerable<BL.AIUnit>) BattleFuncs.env.aiUnitPositions.value;
        if (isEnableOnly)
          source = source.Where<BL.AIUnit>((Func<BL.AIUnit, bool>) (x => !x.isDead));
        foreach (BL.ISkillEffectListUnit forceUnit in source.Where<BL.AIUnit>((Func<BL.AIUnit, bool>) (x =>
        {
          if (BattleFuncs.getForceID(x.originalUnit) != forceId || !includeFacilities && x.originalUnit.isFacility)
            return false;
          return includeJumping || !x.IsJumping;
        })))
          yield return forceUnit;
      }
      else
      {
        IEnumerable<BL.Unit> units = (IEnumerable<BL.Unit>) BattleFuncs.env.getForceUnitList(forceId);
        if (includeFacilities)
          units = units.Concat<BL.Unit>(BattleFuncs.env.facilityUnits.value.Where<BL.Unit>((Func<BL.Unit, bool>) (x => x.facility.thisForce == forceId)));
        if (isEnableOnly)
          units = units.Where<BL.Unit>((Func<BL.Unit, bool>) (x => !x.isDead && x.hp > 0 && x.isEnable));
        foreach (BL.Unit forceUnit in units)
        {
          if (includeJumping || !forceUnit.IsJumping)
            yield return (BL.ISkillEffectListUnit) forceUnit;
        }
      }
    }

    public static IEnumerable<BL.ISkillEffectListUnit> getForceUnitsOrdered(
      BL.ForceID forceId,
      bool isAI,
      bool isEnableOnly,
      bool includeFacilities = false,
      bool includeJumping = false)
    {
      foreach (BL.ISkillEffectListUnit skillEffectListUnit in BattleFuncs.getAllUnitsOrdered(isAI, isEnableOnly, includeFacilities, includeJumping))
      {
        if (BattleFuncs.getForceID(skillEffectListUnit.originalUnit) == forceId)
          yield return skillEffectListUnit;
      }
    }

    public static IEnumerable<BL.ISkillEffectListUnit> getAllUnits(
      bool isAI,
      bool isEnableOnly,
      bool includeFacilities = false,
      bool includeJumping = false)
    {
      return ((IEnumerable<BL.ForceID>) new BL.ForceID[3]
      {
        BL.ForceID.player,
        BL.ForceID.enemy,
        BL.ForceID.neutral
      }).SelectMany<BL.ForceID, BL.ISkillEffectListUnit>((Func<BL.ForceID, IEnumerable<BL.ISkillEffectListUnit>>) (x => BattleFuncs.getForceUnits(x, isAI, isEnableOnly, includeFacilities, includeJumping)));
    }

    public static IEnumerable<BL.ISkillEffectListUnit> getAllUnitsOrdered(
      bool isAI,
      bool isEnableOnly,
      bool includeFacilities = false,
      bool includeJumping = false)
    {
      foreach (BL.UnitPosition unitPosition in BattleFuncs.env.unitPositions.value)
      {
        BL.Unit unit = unitPosition.unit;
        BL.ISkillEffectListUnit skillEffectListUnit;
        if (isAI)
        {
          BL.AIUnit aiUnit = BattleFuncs.env.getAIUnit(unit);
          if (aiUnit != null && (!isEnableOnly || !aiUnit.isDead))
            skillEffectListUnit = (BL.ISkillEffectListUnit) aiUnit;
          else
            continue;
        }
        else if (!isEnableOnly || !unit.isDead && unit.hp > 0 && unit.isEnable)
          skillEffectListUnit = (BL.ISkillEffectListUnit) unit;
        else
          continue;
        if ((includeFacilities || !skillEffectListUnit.originalUnit.isFacility) && (includeJumping || !skillEffectListUnit.IsJumping))
          yield return skillEffectListUnit;
      }
    }

    public static IEnumerable<BL.UnitPosition> getRangeTargets(
      BL.ISkillEffectListUnit unit,
      int[] range,
      BL.ForceID[] forceIds,
      bool nonFacility)
    {
      BL.UnitPosition up = BattleFuncs.iSkillEffectListUnitToUnitPosition(unit);
      bool isAI = up is BL.ISkillEffectListUnit;
      return range[1] == 999 ? ((IEnumerable<BL.ForceID>) forceIds).SelectMany<BL.ForceID, BL.ISkillEffectListUnit>((Func<BL.ForceID, IEnumerable<BL.ISkillEffectListUnit>>) (x => BattleFuncs.getForceUnits(x, isAI, true, !nonFacility))).Select<BL.ISkillEffectListUnit, BL.UnitPosition>((Func<BL.ISkillEffectListUnit, BL.UnitPosition>) (x => BattleFuncs.iSkillEffectListUnitToUnitPosition(x))).Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (u => range[0] == 0 || u != up)) : (IEnumerable<BL.UnitPosition>) BattleFuncs.getTargets(up.row, up.column, range, forceIds, BL.Unit.TargetAttribute.all, isAI, nonFacility: nonFacility);
    }

    private static IEnumerable<BL.SkillEffect> GetEnabledFieldDamageFluctuateBuffDebuff(
      BL.SkillEffectList self,
      BattleskillEffectLogicEnum e,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      int range,
      BL.Panel panel)
    {
      return self.WhereAndGroupBy(e, (Func<IEnumerable<BL.SkillEffect>, BL.SkillEffect>) (x => x.OrderBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (effect => Mathf.Abs(range - effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.range)))).ThenBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (effect => effect.effectId)).FirstOrDefault<BL.SkillEffect>()), (Func<IGrouping<int, BL.SkillEffect>, IEnumerable<BL.SkillEffect>>) (skills => skills.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (effect => !BattleFuncs.isSealedSkillEffect(unit, effect) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.unit_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.unit_id) == unit.originalUnit.unit.ID) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == target.originalUnit.unit.kind.ID) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 || (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == target.originalUnit.playerUnit.GetElement()) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == target.originalUnit.job.ID) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) == 0 || target.originalUnit.playerUnit.HasFamily((UnitFamily) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id))) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_character_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_character_id) == target.originalUnit.unit.character.ID) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_unit_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_unit_id) == target.originalUnit.unit.ID) && effect.effect.GetPackedSkillEffect().CheckLandTag(panel, unit is BL.AIUnit)))));
    }

    private static bool CheckEnabledFieldDamageFluctuateBuffDebuff(
      BattleskillEffect effect,
      BL.ISkillEffectListUnit unit)
    {
      return effect.GetInt(BattleskillEffectLogicArgumentEnum.unit_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.unit_id) == unit.originalUnit.unit.ID;
    }

    private static void GetFieldDamageFluctuateSkillAdd(
      List<BattleFuncs.SkillParam> skillParams,
      BL.ISkillEffectListUnit beUnit,
      BL.ISkillEffectListUnit beTarget,
      int range,
      BL.Panel panel)
    {
      foreach (BL.SkillEffect effect in BattleFuncs.GetEnabledFieldDamageFluctuateBuffDebuff(beUnit.skillEffects, BattleskillEffectLogicEnum.after_duel_damage_fluctuate_fix, beUnit, beTarget, range, panel))
        skillParams.Add(BattleFuncs.SkillParam.CreateAdd(beUnit.originalUnit, effect, (float) (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.damage_value) + effect.baseSkillLevel * effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_ratio))));
    }

    private static void GetFieldDamageFluctuateSkillMul(
      List<BattleFuncs.SkillParam> skillParams,
      BL.ISkillEffectListUnit beUnit,
      BL.ISkillEffectListUnit beTarget,
      int range,
      BL.Panel panel)
    {
      foreach (BL.SkillEffect effect in BattleFuncs.GetEnabledFieldDamageFluctuateBuffDebuff(beUnit.skillEffects, BattleskillEffectLogicEnum.after_duel_damage_fluctuate_ratio, beUnit, beTarget, range, panel))
        skillParams.Add(BattleFuncs.SkillParam.CreateMul(beUnit.originalUnit, effect, (float) ((Decimal) effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.damage_percentage) + (Decimal) effect.baseSkillLevel * (Decimal) effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio))));
    }

    public static int applyFieldDamageFluctuate(
      BL.ISkillEffectListUnit atk,
      BL.ISkillEffectListUnit def,
      BL.ISkillEffectListUnit invokeUnit,
      BL.ISkillEffectListUnit damageUnit,
      BattleskillEffect effect,
      int damage)
    {
      return BattleFuncs.applyFieldDamageFluctuate(atk, def, invokeUnit, damageUnit, effect.GetInt(BattleskillEffectLogicArgumentEnum.is_range_from_enemy) == 0, damage);
    }

    public static int applyFieldDamageFluctuate(
      BL.ISkillEffectListUnit atk,
      BL.ISkillEffectListUnit def,
      BL.ISkillEffectListUnit invokeUnit,
      BL.ISkillEffectListUnit damageUnit,
      bool isRangeFromEnemy,
      int damage,
      BL.ISkillEffectListUnit forceFromUnit = null)
    {
      if (damage < 0)
        return damage;
      BL.ISkillEffectListUnit beTarget = BattleFuncs.env.getForceID(atk.originalUnit) != BattleFuncs.env.getForceID(damageUnit.originalUnit) ? atk : def;
      BL.ISkillEffectListUnit unit = forceFromUnit == null ? (isRangeFromEnemy ? (atk == invokeUnit ? def : atk) : invokeUnit) : forceFromUnit;
      BL.UnitPosition unitPosition = BattleFuncs.iSkillEffectListUnitToUnitPosition(damageUnit);
      int range = BL.fieldDistance(unitPosition, BattleFuncs.iSkillEffectListUnitToUnitPosition(unit));
      BL.Panel panel = BattleFuncs.getPanel(unitPosition.row, unitPosition.column);
      List<BattleFuncs.SkillParam> skillParams = new List<BattleFuncs.SkillParam>();
      BattleFuncs.GetFieldDamageFluctuateSkillAdd(skillParams, damageUnit, beTarget, range, panel);
      BattleFuncs.GetFieldDamageFluctuateSkillMul(skillParams, damageUnit, beTarget, range, panel);
      damage = Mathf.FloorToInt(Judgement.CalcMaximumFloatValue((Decimal) damage * (Decimal) BattleFuncs.calcSkillParamMul(skillParams)));
      damage = Judgement.CalcMaximumLongToInt((long) damage + (long) BattleFuncs.calcSkillParamAdd(skillParams));
      if (damage < 0)
        damage = 0;
      damage = BattleFuncs.applyDamageCut(1, damage, damageUnit, beTarget, invokePanel: panel);
      return damage;
    }

    private static IEnumerable<BL.SkillEffect> GetEnabledDamageCutBuffDebuff(
      BL.SkillEffectList self,
      BattleskillEffectLogicEnum e,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      BL.Panel panel)
    {
      return self.Where(e, (Func<BL.SkillEffect, bool>) (effect =>
      {
        BattleskillEffect effect1 = effect.effect;
        if (effect.useRemain.HasValue && effect.useRemain.Value < 1 || BattleFuncs.isSealedSkillEffect(unit, effect) || target != null && effect1.HasKey(BattleskillEffectLogicArgumentEnum.target_element) && effect1.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != 0 && (CommonElement) effect1.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != target.originalUnit.playerUnit.GetElement() || target != null && effect1.HasKey(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) && effect1.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 && effect1.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != target.originalUnit.unit.kind.ID || target != null && effect1.HasKey(BattleskillEffectLogicArgumentEnum.target_family_id) && effect1.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) != 0 && !target.originalUnit.playerUnit.HasFamily((UnitFamily) effect1.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id)) || !BattleFuncs.PackedSkillEffect.Create(effect).CheckLandTag(panel, unit is BL.AIUnit) || target != null && BattleFuncs.isEffectEnemyRangeAndInvalid(effect, unit, target))
          return false;
        return target == null || !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
      }));
    }

    private static bool CheckEnabledDamageCutBuffDebuff(
      BattleskillEffect effect,
      BL.ISkillEffectListUnit unit)
    {
      return true;
    }

    public static int applyDamageCut(
      int type,
      int damage,
      BL.ISkillEffectListUnit beUnit,
      BL.ISkillEffectListUnit beTarget = null,
      Judgement.BeforeDuelUnitParameter invokeParameter = null,
      Judgement.BeforeDuelUnitParameter targetParameter = null,
      AttackStatus invokeAS = null,
      AttackStatus targetAS = null,
      XorShift random = null,
      int invokeHp = 0,
      int targetHp = 0,
      int? colosseumTurn = null,
      BL.Panel invokePanel = null,
      IEnumerable<BL.SkillEffect> usedEffects = null,
      List<BL.SkillEffect> newUseEffects = null)
    {
      if (damage <= 0)
        return damage;
      bool checkInvoke = type == 0;
      Func<BL.SkillEffect, bool> func = (Func<BL.SkillEffect, bool>) (effect => checkInvoke && !BattleFuncs.isInvoke(beUnit, beTarget, invokeParameter, targetParameter, invokeAS, targetAS, effect.baseSkillLevel, effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invocation), random, false, invokeHp, targetHp, colosseumTurn, base_invocation: effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.base_invocation), invocation_skill_ratio: effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.invocation_skill_ratio), invocation_luck_ratio: effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.invocation_luck_ratio)));
      List<BattleFuncs.SkillParam> skillParams1 = new List<BattleFuncs.SkillParam>();
      foreach (BL.SkillEffect effect in BattleFuncs.GetEnabledDamageCutBuffDebuff(beUnit.skillEffects, BattleFuncs.DamageCutFixEnum[type], beUnit, beTarget, invokePanel))
      {
        int num = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.border_value) + effect.baseSkillLevel * effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_ratio);
        skillParams1.Add(BattleFuncs.SkillParam.CreateParam(beUnit.originalUnit, effect, (object) num));
      }
      foreach (BL.SkillEffect effect in BattleFuncs.GetEnabledDamageCutBuffDebuff(beUnit.skillEffects, BattleFuncs.DamageCutRatioEnum[type], beUnit, beTarget, invokePanel))
      {
        float num1 = effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.border_percentage) + (float) effect.baseSkillLevel * effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio);
        int num2 = Mathf.CeilToInt((float) beUnit.originalUnit.parameter.Hp * num1);
        skillParams1.Add(BattleFuncs.SkillParam.CreateParam(beUnit.originalUnit, effect, (object) num2));
      }
      int num3 = int.MinValue;
      foreach (BattleFuncs.SkillParam skillParam in BattleFuncs.gearSkillParamFilter(skillParams1))
      {
        if (!func(skillParam.effect))
        {
          int num4 = (int) skillParam.param;
          if (num4 > num3)
            num3 = num4;
        }
      }
      if (num3 != int.MinValue && damage < num3)
        damage = 0;
      if (type == 0)
      {
        Func<BL.SkillEffect, bool> checkDamage = (Func<BL.SkillEffect, bool>) (effect =>
        {
          BattleskillEffect effect1 = effect.effect;
          if (damage < effect1.GetInt(BattleskillEffectLogicArgumentEnum.border_min_value))
            return false;
          int num5 = effect1.GetInt(BattleskillEffectLogicArgumentEnum.border_max_value);
          return num5 == 0 || damage <= num5;
        });
        BL.SkillEffect skillEffect = usedEffects != null ? usedEffects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => checkDamage(x))).OrderBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (x => x.effect.GetInt(BattleskillEffectLogicArgumentEnum.damage_value))).FirstOrDefault<BL.SkillEffect>() : (BL.SkillEffect) null;
        if (skillEffect != null)
        {
          damage = skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.damage_value);
        }
        else
        {
          List<BattleFuncs.SkillParam> skillParamList = new List<BattleFuncs.SkillParam>();
          foreach (BL.SkillEffect effect in BattleFuncs.GetEnabledDamageCutBuffDebuff(beUnit.skillEffects, BattleskillEffectLogicEnum.damage_cut3_fix_duel, beUnit, beTarget, invokePanel))
          {
            if (checkDamage(effect))
              skillParamList.Add(BattleFuncs.SkillParam.Create(beUnit.originalUnit, effect));
          }
          if (skillParamList.Any<BattleFuncs.SkillParam>())
          {
            BattleFuncs.SkillParam skillParam = BattleFuncs.gearSkillParamFilter(skillParamList).OrderBy<BattleFuncs.SkillParam, int>((Func<BattleFuncs.SkillParam, int>) (x => x.effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.damage_value))).ThenByDescending<BattleFuncs.SkillParam, int>((Func<BattleFuncs.SkillParam, int>) (x => MasterData.BattleskillSkill[x.effect.baseSkillId].weight)).ThenBy<BattleFuncs.SkillParam, int>((Func<BattleFuncs.SkillParam, int>) (x => x.effect.baseSkillId)).ThenBy<BattleFuncs.SkillParam, int>((Func<BattleFuncs.SkillParam, int>) (x => x.effect.effectId)).FirstOrDefault<BattleFuncs.SkillParam>();
            if (skillParam != null)
            {
              damage = skillParam.effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.damage_value);
              newUseEffects?.Add(skillParam.effect);
            }
          }
        }
      }
      List<BattleFuncs.SkillParam> skillParams2 = new List<BattleFuncs.SkillParam>();
      foreach (BL.SkillEffect effect in BattleFuncs.GetEnabledDamageCutBuffDebuff(beUnit.skillEffects, BattleFuncs.DamageCut2FixEnum[type], beUnit, beTarget, invokePanel))
      {
        if (!func(effect))
          skillParams2.Add(BattleFuncs.SkillParam.CreateAdd(beUnit.originalUnit, effect, (float) (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.border_value) + effect.baseSkillLevel * effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_ratio))));
      }
      foreach (BL.SkillEffect effect in BattleFuncs.GetEnabledDamageCutBuffDebuff(beUnit.skillEffects, BattleFuncs.DamageCut2RatioEnum[type], beUnit, beTarget, invokePanel))
      {
        if (!func(effect))
        {
          float num6 = effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.border_percentage) + (float) effect.baseSkillLevel * effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio);
          skillParams2.Add(BattleFuncs.SkillParam.CreateAdd(beUnit.originalUnit, effect, (float) Mathf.CeilToInt((float) beUnit.originalUnit.parameter.Hp * num6)));
        }
      }
      damage -= BattleFuncs.calcSkillParamAdd(skillParams2);
      if (damage < 0)
        damage = 0;
      if (damage != 0 && type != 0)
      {
        List<BattleFuncs.SkillParam> skillParams3 = new List<BattleFuncs.SkillParam>();
        foreach (BL.SkillEffect effect in BattleFuncs.GetEnabledDamageCutBuffDebuff(beUnit.skillEffects, BattleFuncs.DamageCut4RatioEnum[type], beUnit, beTarget, invokePanel))
        {
          if (!func(effect) && (!effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.include_facilities) || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.include_facilities) != 0 || !beTarget.originalUnit.isFacility))
          {
            Decimal mulParam = (Decimal) effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.damage_percentage) + (Decimal) effect.baseSkillLevel * (Decimal) effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio);
            if (mulParam < 0M)
              mulParam = 0M;
            skillParams3.Add(BattleFuncs.SkillParam.CreateMul(beUnit.originalUnit, effect, (float) mulParam));
          }
        }
        Decimal num7 = (Decimal) BattleFuncs.calcSkillParamMul(skillParams3);
        if (num7 > 1.0M)
          damage = Judgement.CalcMaximumCeilToIntValue((Decimal) damage * num7);
        else if (num7 < 1.0M)
          damage = Judgement.CalcMaximumFloorToIntValue((Decimal) damage * num7);
        if (damage < 0)
          damage = 0;
      }
      return damage;
    }

    public static List<Tuple<int, int, int>> getPenetratePosition(
      int myRow,
      int myColumn,
      int tgtRow,
      int tgtColumn,
      int penetrateCount,
      bool getAllPath = false)
    {
      List<Tuple<int, int, int>> res = new List<Tuple<int, int, int>>();
      int num1 = tgtColumn - myColumn;
      int num2 = tgtRow - myRow;
      int r = penetrateCount;
      int num3 = Mathf.Abs(num1);
      int num4 = Mathf.Abs(num2);
      int distance = num3 + num4;
      if (distance == 0)
        return res;
      int num5 = num1 < 0 ? -1 : 1;
      int num6 = num2 < 0 ? -1 : 1;
      int x = 0;
      int y = 0;
      bool flag = false;
      List<Tuple<int, int>> paths = getAllPath ? new List<Tuple<int, int>>() : (List<Tuple<int, int>>) null;
      Action action = (Action) (() =>
      {
        if (Mathf.Abs(x) + Mathf.Abs(y) == distance)
        {
          flag = true;
          if (!getAllPath)
            return;
          foreach (var data in paths.Select((p, index) => new
          {
            row = p.Item1,
            column = p.Item2,
            index = index
          }))
            res.Add(new Tuple<int, int, int>(data.row, data.column, data.index - paths.Count));
          res.Add(new Tuple<int, int, int>(tgtRow, tgtColumn, 0));
        }
        else if (!flag)
        {
          if (!getAllPath)
            return;
          paths.Add(Tuple.Create<int, int>(y + myRow, x + myColumn));
        }
        else
        {
          --r;
          res.Add(new Tuple<int, int, int>(y + myRow, x + myColumn, penetrateCount - r));
        }
      });
      if (num3 >= num4)
      {
        int num7 = -num3;
        while (!flag || r > 0)
        {
          x += num5;
          num7 += 2 * num4;
          if (num7 >= 0)
          {
            y += num6;
            num7 -= 2 * num3;
          }
          action();
        }
      }
      else
      {
        int num8 = -num4;
        while (!flag || r > 0)
        {
          y += num6;
          num8 += 2 * num3;
          if (num8 >= 0)
          {
            x += num5;
            num8 -= 2 * num4;
          }
          action();
        }
      }
      return res;
    }

    public static HashSet<Tuple<int, int>> getLaserPosition(
      int myRow,
      int myColumn,
      int tgtRow,
      int tgtColumn,
      int minRange,
      int maxRange,
      int radius,
      bool isRangeFromTarget)
    {
      List<Tuple<int, int, int>> penetratePosition = BattleFuncs.getPenetratePosition(myRow, myColumn, tgtRow, tgtColumn, maxRange, true);
      int num1 = tgtColumn - myColumn;
      int num2 = tgtRow - myRow;
      int num3;
      int index1;
      if (!isRangeFromTarget)
      {
        num3 = minRange - 1;
        index1 = maxRange - 1;
      }
      else
      {
        num3 = -1;
        index1 = -1;
        for (int index2 = 0; index2 < penetratePosition.Count; ++index2)
        {
          if (penetratePosition[index2].Item3 == 0)
          {
            num3 = index2 + minRange;
            index1 = index2 + maxRange;
            break;
          }
        }
      }
      if (num3 < 0)
        num3 = 0;
      if (index1 < 0)
        index1 = 0;
      int index3 = num3 - 1;
      int num4;
      int num5;
      if (index3 < 0)
      {
        num4 = myColumn;
        num5 = myRow;
      }
      else
      {
        num4 = penetratePosition[index3].Item2;
        num5 = penetratePosition[index3].Item1;
      }
      int num6 = penetratePosition[index1].Item2;
      int num7 = penetratePosition[index1].Item1;
      HashSet<Tuple<int, int>> laserPosition = new HashSet<Tuple<int, int>>();
      int num8 = radius * radius;
      for (int index4 = num3; index4 <= index1; ++index4)
      {
        Tuple<int, int, int> tuple = penetratePosition[index4];
        int num9 = penetratePosition[index4].Item2;
        int num10 = penetratePosition[index4].Item1;
        for (int index5 = -radius; index5 <= radius; ++index5)
        {
          for (int index6 = -radius; index6 <= radius; ++index6)
          {
            if (index6 * index6 + index5 * index5 <= num8)
            {
              int num11 = num9 + index6;
              int num12 = num10 + index5;
              if (num1 * (num11 - num4) + num2 * (num12 - num5) > 0 && -num1 * (num11 - num6) + -num2 * (num12 - num7) >= 0)
                laserPosition.Add(Tuple.Create<int, int>(num12, num11));
            }
          }
        }
      }
      return laserPosition;
    }

    public static IEnumerable<BattleFuncs.PackedSkillEffect> CreatePackedSkillEffects(
      BattleskillSkill skill,
      int level)
    {
      BattleskillEffect[] battleskillEffectArray = skill.Effects;
      for (int index = 0; index < battleskillEffectArray.Length; ++index)
      {
        BattleskillEffect headerEffect = battleskillEffectArray[index];
        if (!headerEffect.EffectLogic.HasTag(BattleskillEffectTag.ext_arg) && headerEffect.checkLevel(level))
          yield return BattleFuncs.PackedSkillEffect.Create(headerEffect);
      }
      battleskillEffectArray = (BattleskillEffect[]) null;
    }

    public static int calcEquippedGearWeight(
      GearGear initialGear,
      PlayerItem equippedGear,
      PlayerItem equippedGear2,
      PlayerItem equippedGear3)
    {
      if (equippedGear2 != (PlayerItem) null && equippedGear2.gear == null)
        equippedGear2 = (PlayerItem) null;
      if (equippedGear3 != (PlayerItem) null && equippedGear3.gear == null)
        equippedGear3 = (PlayerItem) null;
      if (equippedGear == (PlayerItem) null && equippedGear2 == (PlayerItem) null && equippedGear3 == (PlayerItem) null)
        return initialGear == null ? 0 : initialGear.weight;
      if (equippedGear != (PlayerItem) null && equippedGear2 == (PlayerItem) null && equippedGear3 == (PlayerItem) null)
        return equippedGear.gear.weight;
      if (equippedGear == (PlayerItem) null && equippedGear2 != (PlayerItem) null && equippedGear3 == (PlayerItem) null)
        return equippedGear2.gear.weight;
      if (equippedGear == (PlayerItem) null && equippedGear2 == (PlayerItem) null && equippedGear3 != (PlayerItem) null)
        return equippedGear3.gear.weight;
      if (equippedGear != (PlayerItem) null && equippedGear2 != (PlayerItem) null && equippedGear3 != (PlayerItem) null)
        return Math.Max(equippedGear.gear.weight, equippedGear3.gear.weight) + equippedGear2.gear.weight;
      if (equippedGear != (PlayerItem) null && equippedGear2 != (PlayerItem) null)
        return equippedGear.gear.weight + equippedGear2.gear.weight;
      if (equippedGear != (PlayerItem) null && equippedGear3 != (PlayerItem) null)
        return Math.Max(equippedGear.gear.weight, equippedGear3.gear.weight);
      return equippedGear2 != (PlayerItem) null && equippedGear3 != (PlayerItem) null ? equippedGear2.gear.weight + equippedGear3.gear.weight : 0;
    }

    public static bool isGearEquipped(PlayerUnit playerUnit, int kindId)
    {
      if (kindId == 0 || kindId == playerUnit.equippedGearOrInitial.kind_GearKind)
        return true;
      return playerUnit.equippedGear2 != (PlayerItem) null && kindId == playerUnit.equippedGear2.gear.kind_GearKind;
    }

    public static bool isGearModelEquipped(PlayerUnit playerUnit, int kindId)
    {
      if (kindId == 0 || kindId == playerUnit.equippedGearOrInitial.model_kind_GearModelKind)
        return true;
      return playerUnit.equippedGear2 != (PlayerItem) null && kindId == playerUnit.equippedGear2.gear.model_kind_GearModelKind;
    }

    public static bool isBonusSkillId(int skillId) => skillId >= 900100000 && skillId <= 913999999;

    public static BL.Unit getEffectUnit(BL.SkillEffect effect, BL.Unit hasUnit)
    {
      if (effect.unit != (BL.Unit) null)
      {
        BattleskillSkill skill = effect.effect.skill;
        if (skill.skill_type == BattleskillSkillType.leader || skill.skill_type == BattleskillSkillType.passive && skill.range_effect_passive_skill)
          return effect.unit;
      }
      return effect.parentUnit != (BL.Unit) null ? effect.parentUnit : hasUnit;
    }

    private static int getSkillParamAdd(
      BattleFuncs.SkillParam sp,
      BattleFuncs.BuffDebuffSwapState swapState)
    {
      int skillParamAdd = (int) sp.addParam.Value;
      if (swapState == null || skillParamAdd == 0)
        return skillParamAdd;
      return skillParamAdd > 0 ? (!swapState.isBuffSwap(sp.effect) ? skillParamAdd : -skillParamAdd) : (!swapState.isDebuffSwap(sp.effect) ? skillParamAdd : -skillParamAdd);
    }

    private static float getSkillParamMul(
      BattleFuncs.SkillParam sp,
      BattleFuncs.BuffDebuffSwapState swapState)
    {
      float skillParamMul = sp.mulParam.Value;
      if (swapState == null || (double) skillParamMul == 1.0)
        return skillParamMul;
      if ((double) skillParamMul > 1.0)
        return !swapState.isBuffSwap(sp.effect) ? skillParamMul : (float) (1.0M / (Decimal) skillParamMul);
      if (!swapState.isDebuffSwap(sp.effect))
        return skillParamMul;
      if ((double) skillParamMul < 0.10000000149011612)
        skillParamMul = 0.1f;
      return (float) (1.0M / (Decimal) skillParamMul);
    }

    private static IEnumerable<BattleFuncs.SkillParam> pileSkillParamFilter(
      IEnumerable<BattleFuncs.SkillParam> skillParams,
      double baseParam,
      float baseMul,
      float extraAdd)
    {
      bool exist123 = false;
      bool exist4 = false;
      foreach (BattleFuncs.SkillParam skillParam in skillParams)
      {
        if (skillParam.param == null || (int) skillParam.param == 0)
          yield return skillParam;
        else if ((int) skillParam.param == 4)
          exist4 = true;
        else
          exist123 = true;
      }
      BattleFuncs.SkillParam minSp;
      if (exist4)
      {
        double num1;
        double num2 = num1 = baseParam * 10000.0 * (double) baseMul / 10000.0 + (double) extraAdd;
        BattleFuncs.SkillParam skillParam1 = (BattleFuncs.SkillParam) null;
        minSp = (BattleFuncs.SkillParam) null;
        foreach (BattleFuncs.SkillParam skillParam2 in skillParams.Where<BattleFuncs.SkillParam>((Func<BattleFuncs.SkillParam, bool>) (x => x.param != null && (int) x.param == 4)))
        {
          float num3 = 1f;
          int num4 = 0;
          if (skillParam2.addParam.HasValue)
            num4 += (int) skillParam2.addParam.Value;
          else if (skillParam2.mulParam.HasValue)
            num3 *= skillParam2.mulParam.Value;
          double num5 = baseParam * 10000.0 * (double) baseMul * (double) num3 / 10000.0 + (double) num4 + (double) extraAdd;
          if (num5 > num1)
          {
            skillParam1 = skillParam2;
            num1 = num5;
          }
          else if (num5 < num2)
          {
            minSp = skillParam2;
            num2 = num5;
          }
        }
        if (skillParam1 != null)
          yield return skillParam1;
        if (minSp != null)
          yield return minSp;
        minSp = (BattleFuncs.SkillParam) null;
      }
      if (exist123)
      {
        foreach (IGrouping<BattleskillSkillType, BattleFuncs.SkillParam> spGroup in skillParams.Where<BattleFuncs.SkillParam>((Func<BattleFuncs.SkillParam, bool>) (x => x.param != null && (int) x.param != 0 && (int) x.param != 4)).GroupBy<BattleFuncs.SkillParam, BattleskillSkillType>((Func<BattleFuncs.SkillParam, BattleskillSkillType>) (x => x.effect.baseSkill.skill_type)))
        {
          float num6 = 1f;
          int num7 = 0;
          float num8 = 1f;
          int num9 = 0;
          foreach (BattleFuncs.SkillParam skillParam in (IEnumerable<BattleFuncs.SkillParam>) spGroup)
          {
            if ((int) skillParam.param != 3)
            {
              if (skillParam.addParam.HasValue)
              {
                int num10 = (int) skillParam.addParam.Value;
                if (num10 > 0)
                  num9 += num10;
                else if (num10 < 0)
                  num7 += num10;
              }
              else if (skillParam.mulParam.HasValue)
              {
                float num11 = skillParam.mulParam.Value;
                if ((double) num11 > 1.0)
                  num8 *= num11;
                else if ((double) num11 < 1.0)
                  num6 *= num11;
              }
            }
          }
          double num12 = baseParam * 10000.0 * (double) baseMul * (double) num6 / 10000.0 + (double) num7 + (double) extraAdd;
          double num13 = baseParam * 10000.0 * (double) baseMul * (double) num8 / 10000.0 + (double) num9 + (double) extraAdd;
          BattleFuncs.SkillParam skillParam3 = (BattleFuncs.SkillParam) null;
          minSp = (BattleFuncs.SkillParam) null;
          foreach (BattleFuncs.SkillParam skillParam4 in (IEnumerable<BattleFuncs.SkillParam>) spGroup)
          {
            if ((int) skillParam4.param == 3)
            {
              float num14 = 1f;
              int num15 = 0;
              if (skillParam4.addParam.HasValue)
                num15 += (int) skillParam4.addParam.Value;
              else if (skillParam4.mulParam.HasValue)
                num14 *= skillParam4.mulParam.Value;
              double num16 = baseParam * 10000.0 * (double) baseMul * (double) num14 / 10000.0 + (double) num15 + (double) extraAdd;
              if (num16 > num13)
              {
                skillParam3 = skillParam4;
                num13 = num16;
              }
              else if (num16 < num12)
              {
                minSp = skillParam4;
                num12 = num16;
              }
            }
          }
          if (skillParam3 != null)
          {
            yield return skillParam3;
          }
          else
          {
            foreach (BattleFuncs.SkillParam skillParam5 in (IEnumerable<BattleFuncs.SkillParam>) spGroup)
            {
              if ((int) skillParam5.param != 3)
              {
                if (skillParam5.addParam.HasValue)
                {
                  if ((int) skillParam5.addParam.Value > 0)
                    yield return skillParam5;
                }
                else if (skillParam5.mulParam.HasValue && (double) skillParam5.mulParam.Value > 1.0)
                  yield return skillParam5;
              }
            }
          }
          if (minSp != null)
          {
            yield return minSp;
          }
          else
          {
            foreach (BattleFuncs.SkillParam skillParam6 in (IEnumerable<BattleFuncs.SkillParam>) spGroup)
            {
              if ((int) skillParam6.param != 3)
              {
                if (skillParam6.addParam.HasValue)
                {
                  if ((int) skillParam6.addParam.Value < 0)
                    yield return skillParam6;
                }
                else if (skillParam6.mulParam.HasValue && (double) skillParam6.mulParam.Value < 1.0)
                  yield return skillParam6;
              }
            }
          }
          minSp = (BattleFuncs.SkillParam) null;
        }
      }
    }

    private static double calcSkillParamSub(
      IEnumerable<BattleFuncs.SkillParam> skillParams,
      BattleFuncs.SkillParamClamp skillParamClamp,
      double baseParam,
      float baseMul,
      float extraAdd,
      BattleFuncs.BuffDebuffSwapState swapState = null)
    {
      Decimal num1 = 1.0M;
      Decimal num2 = 1.0M;
      int num3 = 0;
      int num4 = 0;
      List<BattleFuncs.SkillParam> source1 = (List<BattleFuncs.SkillParam>) null;
      foreach (BattleFuncs.SkillParam skillParam in skillParams)
      {
        if (skillParam.effect.gearIndex == 0)
        {
          if (skillParam.addParam.HasValue)
          {
            if (BattleFuncs.isBonusSkillId(skillParam.effect.baseSkillId))
              num4 += BattleFuncs.getSkillParamAdd(skillParam, swapState);
            else
              num3 += BattleFuncs.getSkillParamAdd(skillParam, swapState);
          }
          if (skillParam.mulParam.HasValue)
          {
            if (BattleFuncs.isBonusSkillId(skillParam.effect.baseSkillId))
              num2 *= (Decimal) BattleFuncs.getSkillParamMul(skillParam, swapState);
            else
              num1 *= (Decimal) BattleFuncs.getSkillParamMul(skillParam, swapState);
          }
        }
        else
        {
          if (source1 == null)
            source1 = new List<BattleFuncs.SkillParam>();
          source1.Add(skillParam);
        }
      }
      if (source1 != null)
      {
        foreach (IGrouping<\u003C\u003Ef__AnonymousType3<int, bool>, BattleFuncs.SkillParam> source2 in source1.GroupBy(x => new
        {
          index = x.effectUnit.index,
          isPlayerForce = x.effectUnit.isPlayerForce
        }))
        {
          Decimal num5 = (Decimal) baseParam * (Decimal) baseMul + (Decimal) extraAdd;
          Decimal num6 = Decimal.MaxValue;
          Decimal num7 = 1.0M;
          int num8 = 0;
          Decimal num9 = Decimal.MinValue;
          Decimal num10 = 1.0M;
          int num11 = 0;
          foreach (IGrouping<int, BattleFuncs.SkillParam> grouping in source2.GroupBy<BattleFuncs.SkillParam, int>((Func<BattleFuncs.SkillParam, int>) (x => x.effect.gearIndex)))
          {
            Decimal num12 = 1.0M;
            int num13 = 0;
            foreach (BattleFuncs.SkillParam sp in (IEnumerable<BattleFuncs.SkillParam>) grouping)
            {
              if (sp.addParam.HasValue)
                num13 += BattleFuncs.getSkillParamAdd(sp, swapState);
              if (sp.mulParam.HasValue)
                num12 *= (Decimal) BattleFuncs.getSkillParamMul(sp, swapState);
            }
            Decimal num14 = (Decimal) baseParam * (Decimal) baseMul * num12 + (Decimal) num13 + (Decimal) extraAdd;
            if (num14 > num5)
            {
              if (num14 > num9)
              {
                num9 = num14;
                num10 = num12;
                num11 = num13;
              }
            }
            else if (num14 < num5 && num14 < num6)
            {
              num6 = num14;
              num7 = num12;
              num8 = num13;
            }
          }
          num1 *= num7 * num10;
          num3 += num8 + num11;
        }
      }
      int? nullable1;
      if (skillParamClamp.fixMax.HasValue)
      {
        int num15 = num3;
        nullable1 = skillParamClamp.fixMax;
        int valueOrDefault = nullable1.GetValueOrDefault();
        if (num15 > valueOrDefault & nullable1.HasValue)
          num3 = skillParamClamp.fixMax.Value;
      }
      if (skillParamClamp.fixMin.HasValue)
      {
        int num16 = num3;
        nullable1 = skillParamClamp.fixMin;
        int valueOrDefault = nullable1.GetValueOrDefault();
        if (num16 < valueOrDefault & nullable1.HasValue)
          num3 = skillParamClamp.fixMin.Value;
      }
      Decimal? nullable2;
      if (skillParamClamp.ratioMax.HasValue)
      {
        Decimal num17 = num1;
        nullable2 = skillParamClamp.ratioMax;
        Decimal valueOrDefault = nullable2.GetValueOrDefault();
        if (num17 > valueOrDefault & nullable2.HasValue)
          num1 = skillParamClamp.ratioMax.Value;
      }
      if (skillParamClamp.ratioMin.HasValue)
      {
        Decimal num18 = num1;
        nullable2 = skillParamClamp.ratioMin;
        Decimal valueOrDefault = nullable2.GetValueOrDefault();
        if (num18 < valueOrDefault & nullable2.HasValue)
          num1 = skillParamClamp.ratioMin.Value;
      }
      return (double) ((Decimal) baseParam * (Decimal) baseMul * (num1 * num2) + (Decimal) (num3 + num4) + (Decimal) extraAdd);
    }

    public static double calcSkillParam(
      IEnumerable<BattleFuncs.SkillParam> skillParams,
      BattleFuncs.SkillParamClamp skillParamClamp,
      double baseParam,
      float baseMul = 1f,
      float extraAdd = 0.0f,
      BattleFuncs.BuffDebuffSwapState swapState = null)
    {
      return BattleFuncs.calcSkillParamSub(BattleFuncs.pileSkillParamFilter(skillParams, baseParam, baseMul, extraAdd), skillParamClamp, baseParam, baseMul, extraAdd, swapState);
    }

    public static Tuple<float, float> calcSkillParam2(
      List<BattleFuncs.SkillParam> skillParams,
      BattleFuncs.SkillParamClamp skillParamClamp,
      float baseParam,
      float baseMul,
      float extraAdd,
      List<BattleFuncs.SkillParam> skillParams2,
      BattleFuncs.SkillParamClamp skillParamClamp2,
      float baseParam2,
      float baseMul2,
      float extraAdd2,
      BattleFuncs.BuffDebuffSwapState swapState = null)
    {
      BattleFuncs.SkillParam[] array1 = BattleFuncs.pileSkillParamFilter((IEnumerable<BattleFuncs.SkillParam>) skillParams, (double) baseParam, baseMul, extraAdd).ToArray<BattleFuncs.SkillParam>();
      float num1 = (float) BattleFuncs.calcSkillParamSub((IEnumerable<BattleFuncs.SkillParam>) array1, skillParamClamp, (double) baseParam, baseMul, extraAdd, swapState);
      BattleFuncs.SkillParam[] array2 = BattleFuncs.pileSkillParamFilter((IEnumerable<BattleFuncs.SkillParam>) skillParams2, (double) (int) num1 + (double) baseParam2, baseMul2, extraAdd2).ToArray<BattleFuncs.SkillParam>();
      BattleFuncs.SkillParam[] array3 = ((IEnumerable<BattleFuncs.SkillParam>) array1).Where<BattleFuncs.SkillParam>((Func<BattleFuncs.SkillParam, bool>) (x => x.effect.gearIndex != 0)).ToArray<BattleFuncs.SkillParam>();
      BattleFuncs.SkillParam[] array4 = ((IEnumerable<BattleFuncs.SkillParam>) array2).Where<BattleFuncs.SkillParam>((Func<BattleFuncs.SkillParam, bool>) (x => x.effect.gearIndex != 0)).ToArray<BattleFuncs.SkillParam>();
      float num2;
      float num3;
      if (array3.Length == 0 && array4.Length == 0)
      {
        num2 = num1;
        num3 = (float) BattleFuncs.calcSkillParamSub((IEnumerable<BattleFuncs.SkillParam>) array2, skillParamClamp2, (double) (int) num2 + (double) baseParam2, baseMul2, extraAdd2, swapState);
      }
      else
      {
        Decimal num4 = 1.0M;
        Decimal num5 = 1.0M;
        int num6 = 0;
        int num7 = 0;
        Decimal num8 = 1.0M;
        Decimal num9 = 1.0M;
        int num10 = 0;
        int num11 = 0;
        foreach (BattleFuncs.SkillParam sp in ((IEnumerable<BattleFuncs.SkillParam>) array1).Where<BattleFuncs.SkillParam>((Func<BattleFuncs.SkillParam, bool>) (x => x.effect.gearIndex == 0)))
        {
          if (sp.addParam.HasValue)
          {
            if (BattleFuncs.isBonusSkillId(sp.effect.baseSkillId))
              num7 += BattleFuncs.getSkillParamAdd(sp, swapState);
            else
              num6 += BattleFuncs.getSkillParamAdd(sp, swapState);
          }
          if (sp.mulParam.HasValue)
          {
            if (BattleFuncs.isBonusSkillId(sp.effect.baseSkillId))
              num5 *= (Decimal) BattleFuncs.getSkillParamMul(sp, swapState);
            else
              num4 *= (Decimal) BattleFuncs.getSkillParamMul(sp, swapState);
          }
        }
        foreach (BattleFuncs.SkillParam sp in ((IEnumerable<BattleFuncs.SkillParam>) array2).Where<BattleFuncs.SkillParam>((Func<BattleFuncs.SkillParam, bool>) (x => x.effect.gearIndex == 0)))
        {
          if (sp.addParam.HasValue)
          {
            if (BattleFuncs.isBonusSkillId(sp.effect.baseSkillId))
              num11 += BattleFuncs.getSkillParamAdd(sp, swapState);
            else
              num10 += BattleFuncs.getSkillParamAdd(sp, swapState);
          }
          if (sp.mulParam.HasValue)
          {
            if (BattleFuncs.isBonusSkillId(sp.effect.baseSkillId))
              num9 *= (Decimal) BattleFuncs.getSkillParamMul(sp, swapState);
            else
              num8 *= (Decimal) BattleFuncs.getSkillParamMul(sp, swapState);
          }
        }
        Decimal num12 = ((Decimal) (int) ((Decimal) baseParam * (Decimal) baseMul + (Decimal) extraAdd) + (Decimal) baseParam2) * (Decimal) baseMul2 + (Decimal) extraAdd2;
        foreach (var data in ((IEnumerable<BattleFuncs.SkillParam>) array3).Concat<BattleFuncs.SkillParam>((IEnumerable<BattleFuncs.SkillParam>) array4).Select(x => new
        {
          index = x.effectUnit.index,
          isPlayerForce = x.effectUnit.isPlayerForce
        }).Distinct())
        {
          var unitKey = data;
          BattleFuncs.SkillParam[] array5 = ((IEnumerable<BattleFuncs.SkillParam>) array3).Where<BattleFuncs.SkillParam>((Func<BattleFuncs.SkillParam, bool>) (x => new
          {
            index = x.effectUnit.index,
            isPlayerForce = x.effectUnit.isPlayerForce
          }.Equals((object) unitKey))).ToArray<BattleFuncs.SkillParam>();
          BattleFuncs.SkillParam[] array6 = ((IEnumerable<BattleFuncs.SkillParam>) array4).Where<BattleFuncs.SkillParam>((Func<BattleFuncs.SkillParam, bool>) (x => new
          {
            index = x.effectUnit.index,
            isPlayerForce = x.effectUnit.isPlayerForce
          }.Equals((object) unitKey))).ToArray<BattleFuncs.SkillParam>();
          IEnumerable<int> ints = ((IEnumerable<BattleFuncs.SkillParam>) array5).Concat<BattleFuncs.SkillParam>((IEnumerable<BattleFuncs.SkillParam>) array6).Select<BattleFuncs.SkillParam, int>((Func<BattleFuncs.SkillParam, int>) (x => x.effect.gearIndex)).Distinct<int>();
          Decimal num13 = Decimal.MaxValue;
          Decimal num14 = 1.0M;
          int num15 = 0;
          Decimal num16 = 1.0M;
          int num17 = 0;
          Decimal num18 = Decimal.MinValue;
          Decimal num19 = 1.0M;
          int num20 = 0;
          Decimal num21 = 1.0M;
          int num22 = 0;
          foreach (int num23 in ints)
          {
            int gearIndex = num23;
            BattleFuncs.SkillParam[] array7 = ((IEnumerable<BattleFuncs.SkillParam>) array5).Where<BattleFuncs.SkillParam>((Func<BattleFuncs.SkillParam, bool>) (x => x.effect.gearIndex == gearIndex)).ToArray<BattleFuncs.SkillParam>();
            BattleFuncs.SkillParam[] array8 = ((IEnumerable<BattleFuncs.SkillParam>) array6).Where<BattleFuncs.SkillParam>((Func<BattleFuncs.SkillParam, bool>) (x => x.effect.gearIndex == gearIndex)).ToArray<BattleFuncs.SkillParam>();
            Decimal num24 = 1.0M;
            int num25 = 0;
            foreach (BattleFuncs.SkillParam sp in array7)
            {
              if (sp.addParam.HasValue)
                num25 += BattleFuncs.getSkillParamAdd(sp, swapState);
              if (sp.mulParam.HasValue)
                num24 *= (Decimal) BattleFuncs.getSkillParamMul(sp, swapState);
            }
            Decimal num26 = 1.0M;
            int num27 = 0;
            foreach (BattleFuncs.SkillParam sp in array8)
            {
              if (sp.addParam.HasValue)
                num27 += BattleFuncs.getSkillParamAdd(sp, swapState);
              if (sp.mulParam.HasValue)
                num26 *= (Decimal) BattleFuncs.getSkillParamMul(sp, swapState);
            }
            Decimal num28 = ((Decimal) (int) ((Decimal) baseParam * (Decimal) baseMul * num24 + (Decimal) num25 + (Decimal) extraAdd) + (Decimal) baseParam2) * (Decimal) baseMul2 * num26 + (Decimal) num27 + (Decimal) extraAdd2;
            if (num28 > num12)
            {
              if (num28 > num18)
              {
                num18 = num28;
                num19 = num24;
                num20 = num25;
                num21 = num26;
                num22 = num27;
              }
            }
            else if (num28 < num12 && num28 < num13)
            {
              num13 = num28;
              num14 = num24;
              num15 = num25;
              num21 = num26;
              num22 = num27;
            }
          }
          num4 *= num14 * num19;
          num6 += num15 + num20;
          num8 *= num16 * num21;
          num10 += num17 + num22;
        }
        int? nullable1;
        if (skillParamClamp.fixMax.HasValue)
        {
          int num29 = num6;
          nullable1 = skillParamClamp.fixMax;
          int valueOrDefault = nullable1.GetValueOrDefault();
          if (num29 > valueOrDefault & nullable1.HasValue)
            num6 = skillParamClamp.fixMax.Value;
        }
        if (skillParamClamp.fixMin.HasValue)
        {
          int num30 = num6;
          nullable1 = skillParamClamp.fixMin;
          int valueOrDefault = nullable1.GetValueOrDefault();
          if (num30 < valueOrDefault & nullable1.HasValue)
            num6 = skillParamClamp.fixMin.Value;
        }
        Decimal? nullable2;
        if (skillParamClamp.ratioMax.HasValue)
        {
          Decimal num31 = num4;
          nullable2 = skillParamClamp.ratioMax;
          Decimal valueOrDefault = nullable2.GetValueOrDefault();
          if (num31 > valueOrDefault & nullable2.HasValue)
            num4 = skillParamClamp.ratioMax.Value;
        }
        if (skillParamClamp.ratioMin.HasValue)
        {
          Decimal num32 = num4;
          nullable2 = skillParamClamp.ratioMin;
          Decimal valueOrDefault = nullable2.GetValueOrDefault();
          if (num32 < valueOrDefault & nullable2.HasValue)
            num4 = skillParamClamp.ratioMin.Value;
        }
        num2 = (float) ((Decimal) baseParam * (Decimal) baseMul * (num4 * num5) + (Decimal) (num6 + num7) + (Decimal) extraAdd);
        if (skillParamClamp2.fixMax.HasValue)
        {
          int num33 = num10;
          nullable1 = skillParamClamp2.fixMax;
          int valueOrDefault = nullable1.GetValueOrDefault();
          if (num33 > valueOrDefault & nullable1.HasValue)
            num10 = skillParamClamp2.fixMax.Value;
        }
        if (skillParamClamp2.fixMin.HasValue)
        {
          int num34 = num10;
          nullable1 = skillParamClamp2.fixMin;
          int valueOrDefault = nullable1.GetValueOrDefault();
          if (num34 < valueOrDefault & nullable1.HasValue)
            num10 = skillParamClamp2.fixMin.Value;
        }
        if (skillParamClamp2.ratioMax.HasValue)
        {
          Decimal num35 = num8;
          nullable2 = skillParamClamp2.ratioMax;
          Decimal valueOrDefault = nullable2.GetValueOrDefault();
          if (num35 > valueOrDefault & nullable2.HasValue)
            num8 = skillParamClamp2.ratioMax.Value;
        }
        if (skillParamClamp2.ratioMin.HasValue)
        {
          Decimal num36 = num8;
          nullable2 = skillParamClamp2.ratioMin;
          Decimal valueOrDefault = nullable2.GetValueOrDefault();
          if (num36 < valueOrDefault & nullable2.HasValue)
            num8 = skillParamClamp2.ratioMin.Value;
        }
        num3 = (float) (((Decimal) (int) num2 + (Decimal) baseParam2) * (Decimal) baseMul2 * (num8 * num9) + (Decimal) (num10 + num11) + (Decimal) extraAdd2);
      }
      return Tuple.Create<float, float>(num2, num3);
    }

    public static float calcSkillParamMul(List<BattleFuncs.SkillParam> skillParams, float baseMul = 1f)
    {
      float num = 10000f * baseMul;
      foreach (BattleFuncs.SkillParam skillParam in BattleFuncs.gearSkillParamFilter(skillParams))
      {
        if (skillParam.mulParam.HasValue)
          num *= skillParam.mulParam.Value;
      }
      return num / 10000f;
    }

    public static int calcSkillParamAdd(List<BattleFuncs.SkillParam> skillParams)
    {
      int num = 0;
      foreach (BattleFuncs.SkillParam skillParam in BattleFuncs.gearSkillParamFilter(skillParams))
      {
        if (skillParam.addParam.HasValue)
          num += (int) skillParam.addParam.Value;
      }
      return num;
    }

    public static float calcSkillParamAddSingle(List<BattleFuncs.SkillParam> skillParams)
    {
      Decimal num = 0M;
      foreach (BattleFuncs.SkillParam skillParam in BattleFuncs.gearSkillParamFilter(skillParams))
      {
        if (skillParam.addParam.HasValue)
          num += (Decimal) skillParam.addParam.Value;
      }
      return (float) num;
    }

    public static IEnumerable<BL.SkillEffect> gearSkillEffectFilter(
      BL.Unit hasUnit,
      IEnumerable<BL.SkillEffect> skillEffects)
    {
      HashSet<Tuple<int, bool>> existGearOne = (HashSet<Tuple<int, bool>>) null;
      HashSet<Tuple<int, bool>> existGearTwo = (HashSet<Tuple<int, bool>>) null;
      foreach (BL.SkillEffect skillEffect1 in skillEffects)
      {
        if (skillEffect1.gearIndex == 0)
          yield return skillEffect1;
        else if (skillEffect1.gearIndex == 1)
        {
          if (existGearOne == null)
            existGearOne = new HashSet<Tuple<int, bool>>();
          BL.Unit effectUnit = BattleFuncs.getEffectUnit(skillEffect1, hasUnit);
          existGearOne.Add(Tuple.Create<int, bool>(effectUnit.index, effectUnit.isPlayerForce));
          yield return skillEffect1;
        }
        else if (skillEffect1.gearIndex == 2)
        {
          if (existGearOne == null)
            existGearOne = new HashSet<Tuple<int, bool>>();
          if (existGearTwo == null)
            existGearTwo = new HashSet<Tuple<int, bool>>();
          BL.Unit effectUnit1 = BattleFuncs.getEffectUnit(skillEffect1, hasUnit);
          Tuple<int, bool> unitKey = Tuple.Create<int, bool>(effectUnit1.index, effectUnit1.isPlayerForce);
          if (!existGearOne.Contains(unitKey))
          {
            if (skillEffects.Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x =>
            {
              if (x.gearIndex != 1)
                return false;
              BL.Unit effectUnit2 = BattleFuncs.getEffectUnit(x, hasUnit);
              return Tuple.Create<int, bool>(effectUnit2.index, effectUnit2.isPlayerForce).Equals((object) unitKey);
            })))
            {
              existGearOne.Add(unitKey);
            }
            else
            {
              existGearTwo.Add(unitKey);
              yield return skillEffect1;
            }
          }
        }
        else if (skillEffect1.gearIndex == 3)
        {
          if (existGearOne == null)
            existGearOne = new HashSet<Tuple<int, bool>>();
          if (existGearTwo == null)
            existGearTwo = new HashSet<Tuple<int, bool>>();
          BL.Unit effectUnit3 = BattleFuncs.getEffectUnit(skillEffect1, hasUnit);
          Tuple<int, bool> tuple = Tuple.Create<int, bool>(effectUnit3.index, effectUnit3.isPlayerForce);
          if (!existGearOne.Contains(tuple) && !existGearTwo.Contains(tuple))
          {
            bool flag = false;
            foreach (BL.SkillEffect skillEffect2 in skillEffects)
            {
              if (skillEffect2.gearIndex == 1 || skillEffect2.gearIndex == 2)
              {
                BL.Unit effectUnit4 = BattleFuncs.getEffectUnit(skillEffect2, hasUnit);
                if (Tuple.Create<int, bool>(effectUnit4.index, effectUnit4.isPlayerForce).Equals((object) tuple))
                {
                  if (skillEffect2.gearIndex == 1)
                    existGearOne.Add(tuple);
                  else
                    existGearTwo.Add(tuple);
                  flag = true;
                  break;
                }
              }
            }
            if (!flag)
              yield return skillEffect1;
          }
        }
      }
    }

    public static IEnumerable<BattleFuncs.SkillParam> gearSkillParamFilter(
      List<BattleFuncs.SkillParam> skillParams)
    {
      HashSet<Tuple<int, bool>> existGearOne = (HashSet<Tuple<int, bool>>) null;
      HashSet<Tuple<int, bool>> existGearTwo = (HashSet<Tuple<int, bool>>) null;
      foreach (BattleFuncs.SkillParam skillParam1 in skillParams)
      {
        BL.SkillEffect effect = skillParam1.effect;
        if (effect.gearIndex == 0)
          yield return skillParam1;
        else if (effect.gearIndex == 1)
        {
          if (existGearOne == null)
            existGearOne = new HashSet<Tuple<int, bool>>();
          BL.Unit effectUnit = BattleFuncs.getEffectUnit(effect, skillParam1.hasUnit);
          existGearOne.Add(Tuple.Create<int, bool>(effectUnit.index, effectUnit.isPlayerForce));
          yield return skillParam1;
        }
        else if (effect.gearIndex == 2)
        {
          if (existGearOne == null)
            existGearOne = new HashSet<Tuple<int, bool>>();
          if (existGearTwo == null)
            existGearTwo = new HashSet<Tuple<int, bool>>();
          BL.Unit effectUnit = BattleFuncs.getEffectUnit(effect, skillParam1.hasUnit);
          Tuple<int, bool> unitKey = Tuple.Create<int, bool>(effectUnit.index, effectUnit.isPlayerForce);
          if (!existGearOne.Contains(unitKey))
          {
            if (skillParams.Any<BattleFuncs.SkillParam>((Func<BattleFuncs.SkillParam, bool>) (x => x.effect.gearIndex == 1 && Tuple.Create<int, bool>(x.effectUnit.index, x.effectUnit.isPlayerForce).Equals((object) unitKey))))
            {
              existGearOne.Add(unitKey);
            }
            else
            {
              existGearTwo.Add(unitKey);
              yield return skillParam1;
            }
          }
        }
        else if (effect.gearIndex == 3)
        {
          if (existGearOne == null)
            existGearOne = new HashSet<Tuple<int, bool>>();
          if (existGearTwo == null)
            existGearTwo = new HashSet<Tuple<int, bool>>();
          BL.Unit effectUnit = BattleFuncs.getEffectUnit(effect, skillParam1.hasUnit);
          Tuple<int, bool> tuple = Tuple.Create<int, bool>(effectUnit.index, effectUnit.isPlayerForce);
          if (!existGearOne.Contains(tuple) && !existGearTwo.Contains(tuple))
          {
            bool flag = false;
            foreach (BattleFuncs.SkillParam skillParam2 in skillParams)
            {
              if ((skillParam2.effect.gearIndex == 1 || skillParam2.effect.gearIndex == 2) && Tuple.Create<int, bool>(skillParam2.effectUnit.index, skillParam2.effectUnit.isPlayerForce).Equals((object) tuple))
              {
                if (skillParam2.effect.gearIndex == 1)
                  existGearOne.Add(tuple);
                else
                  existGearTwo.Add(tuple);
                flag = true;
                break;
              }
            }
            if (!flag)
              yield return skillParam1;
          }
        }
      }
    }

    public static IEnumerable<BattleFuncs.SkillParam> gearSkillParamCategoryFilter(
      List<BattleFuncs.SkillParam> skillParams)
    {
      Dictionary<int, HashSet<Tuple<int, bool>>> existGearOne = new Dictionary<int, HashSet<Tuple<int, bool>>>();
      Dictionary<int, HashSet<Tuple<int, bool>>> existGearTwo = new Dictionary<int, HashSet<Tuple<int, bool>>>();
      foreach (BattleFuncs.SkillParam skillParam1 in skillParams)
      {
        BattleFuncs.SkillParam sp = skillParam1;
        BL.SkillEffect effect = sp.effect;
        if (effect.gearIndex == 0)
          yield return sp;
        else if (effect.gearIndex == 1)
        {
          if (!existGearOne.ContainsKey(sp.category))
            existGearOne[sp.category] = new HashSet<Tuple<int, bool>>();
          BL.Unit effectUnit = BattleFuncs.getEffectUnit(effect, sp.hasUnit);
          existGearOne[sp.category].Add(Tuple.Create<int, bool>(effectUnit.index, effectUnit.isPlayerForce));
          yield return sp;
        }
        else if (effect.gearIndex == 2)
        {
          if (!existGearOne.ContainsKey(sp.category))
            existGearOne[sp.category] = new HashSet<Tuple<int, bool>>();
          if (!existGearTwo.ContainsKey(sp.category))
            existGearTwo[sp.category] = new HashSet<Tuple<int, bool>>();
          BL.Unit effectUnit = BattleFuncs.getEffectUnit(effect, sp.hasUnit);
          Tuple<int, bool> unitKey = Tuple.Create<int, bool>(effectUnit.index, effectUnit.isPlayerForce);
          if (!existGearOne[sp.category].Contains(unitKey))
          {
            if (skillParams.Any<BattleFuncs.SkillParam>((Func<BattleFuncs.SkillParam, bool>) (x => x.effect.gearIndex == 1 && x.category == sp.category && Tuple.Create<int, bool>(x.effectUnit.index, x.effectUnit.isPlayerForce).Equals((object) unitKey))))
            {
              existGearOne[sp.category].Add(unitKey);
            }
            else
            {
              existGearTwo[sp.category].Add(unitKey);
              yield return sp;
            }
          }
        }
        else if (effect.gearIndex == 3)
        {
          if (!existGearOne.ContainsKey(sp.category))
            existGearOne[sp.category] = new HashSet<Tuple<int, bool>>();
          if (!existGearTwo.ContainsKey(sp.category))
            existGearTwo[sp.category] = new HashSet<Tuple<int, bool>>();
          BL.Unit effectUnit = BattleFuncs.getEffectUnit(effect, sp.hasUnit);
          Tuple<int, bool> tuple = Tuple.Create<int, bool>(effectUnit.index, effectUnit.isPlayerForce);
          if (!existGearOne[sp.category].Contains(tuple) && !existGearTwo[sp.category].Contains(tuple))
          {
            bool flag = false;
            foreach (BattleFuncs.SkillParam skillParam2 in skillParams)
            {
              if ((skillParam2.effect.gearIndex == 1 || skillParam2.effect.gearIndex == 2) && skillParam2.category == sp.category && Tuple.Create<int, bool>(skillParam2.effectUnit.index, skillParam2.effectUnit.isPlayerForce).Equals((object) tuple))
              {
                if (skillParam2.effect.gearIndex == 1)
                  existGearOne[sp.category].Add(tuple);
                else
                  existGearTwo[sp.category].Add(tuple);
                flag = true;
                break;
              }
            }
            if (!flag)
              yield return sp;
          }
        }
      }
    }

    public static bool isResetPositionOK(
      BL.Unit target,
      int row,
      int column,
      int moveCost,
      IEnumerable<BL.Unit> ignoreUnits,
      bool isAI = false)
    {
      BL.Panel fieldPanel = BattleFuncs.env.getFieldPanel(row, column);
      if (fieldPanel == null)
        return false;
      if (moveCost <= 0)
        moveCost = 1;
      if (!BattleFuncs.env.isMoveOKPanel(fieldPanel, target, false, moveCost))
        return false;
      BL.UnitPosition[] unitPositionArray = isAI ? BattleFuncs.getFieldUnitsAI(row, column, true) : BattleFuncs.env.getFieldUnits(row, column, includeJumping: true);
      if (unitPositionArray != null)
      {
        foreach (BL.UnitPosition unitPosition in unitPositionArray)
        {
          if (!ignoreUnits.Contains<BL.Unit>(unitPosition.unit) && !unitPosition.unit.isPutOn)
            return false;
        }
      }
      return true;
    }

    public static int[] getAttractDelta(BL.UnitPosition up, BL.UnitPosition tup)
    {
      int num1 = tup.row - up.row;
      int num2 = tup.column - up.column;
      int num3 = 0;
      int num4 = 0;
      if (num1 > 0 && num2 >= 0)
      {
        if (Mathf.Abs(num1) >= Mathf.Abs(num2))
          --num4;
        else
          --num3;
      }
      else if (num1 <= 0 && num2 > 0)
      {
        if (Mathf.Abs(num2) >= Mathf.Abs(num1))
          --num3;
        else
          ++num4;
      }
      else if (num1 < 0 && num2 <= 0)
      {
        if (Mathf.Abs(num1) >= Mathf.Abs(num2))
          ++num4;
        else
          ++num3;
      }
      else if (Mathf.Abs(num2) >= Mathf.Abs(num1))
        ++num3;
      else
        --num4;
      return new int[2]{ num4, num3 };
    }

    public static bool getShiftBreakPosition(
      BL.ISkillEffectListUnit useUnit,
      BL.ISkillEffectListUnit targetUnit,
      int range,
      out int posY,
      out int posX)
    {
      BL.UnitPosition unitPosition1 = BattleFuncs.iSkillEffectListUnitToUnitPosition(useUnit);
      BL.UnitPosition unitPosition2 = BattleFuncs.iSkillEffectListUnitToUnitPosition(targetUnit);
      int num1 = unitPosition1.row - unitPosition2.row;
      int num2 = unitPosition1.column - unitPosition2.column;
      int num3 = num1 <= 0 || num2 < 0 ? (num1 > 0 || num2 <= 0 ? (num1 >= 0 || num2 > 0 ? (Mathf.Abs(num2) < Mathf.Abs(num1) ? 0 : 3) : (Mathf.Abs(num1) < Mathf.Abs(num2) ? 3 : 2)) : (Mathf.Abs(num2) < Mathf.Abs(num1) ? 2 : 1)) : (Mathf.Abs(num1) < Mathf.Abs(num2) ? 1 : 0);
      int y = unitPosition2.row;
      int x = unitPosition2.column;
      switch (num3)
      {
        case 0:
          y += range;
          break;
        case 1:
          x += range;
          break;
        case 2:
          y -= range;
          break;
        default:
          x -= range;
          break;
      }
      BL.Unit[] ignoreUnits = new BL.Unit[1]
      {
        useUnit.originalUnit
      };
      int moveCost = unitPosition1.unit.parameter.Move;
      bool found = false;
      Func<bool> func = (Func<bool>) (() =>
      {
        if (!BattleFuncs.isResetPositionOK(useUnit.originalUnit, y, x, moveCost, (IEnumerable<BL.Unit>) ignoreUnits) || !BattleFuncs.canWarpPanel(useUnit, y, x))
          return false;
        found = true;
        return true;
      });
      for (int index1 = 0; index1 < 4; ++index1)
      {
        switch (num3)
        {
          case 0:
            for (int index2 = 0; index2 < range && !func(); ++index2)
            {
              y--;
              x++;
            }
            break;
          case 1:
            for (int index3 = 0; index3 < range && !func(); ++index3)
            {
              y--;
              x--;
            }
            break;
          case 2:
            for (int index4 = 0; index4 < range && !func(); ++index4)
            {
              y++;
              x--;
            }
            break;
          case 3:
            for (int index5 = 0; index5 < range && !func(); ++index5)
            {
              y++;
              x++;
            }
            break;
        }
        if (!found)
          num3 = (num3 + 1) % 4;
        else
          break;
      }
      posY = y;
      posX = x;
      return found;
    }

    public static bool canUseImmediateSkillEffect(
      BattleskillEffect effect,
      BL.ISkillEffectListUnit unit,
      out bool enableAnchorGround,
      BL.ISkillEffectListUnit useUnit = null,
      BL.ISkillEffectListUnit firstTarget = null,
      bool? callIsPlayer = null,
      Judgement.BattleParameter battleParameter = null)
    {
      enableAnchorGround = false;
      switch (effect.EffectLogic.Enum)
      {
        case BattleskillEffectLogicEnum.fix_heal:
        case BattleskillEffectLogicEnum.ratio_heal:
        case BattleskillEffectLogicEnum.fix_lv_heal:
        case BattleskillEffectLogicEnum.ratio_lv_heal:
        case BattleskillEffectLogicEnum.parameter_reference_heal:
          NGBattleManager instance = Singleton<NGBattleManager>.GetInstance();
          if ((useUnit == null || !instance.isRaidBoss(unit.originalUnit) || BattleFuncs.env.getForceID(useUnit.originalUnit) == BattleFuncs.env.getForceID(unit.originalUnit)) && (effect.skill.skill_type != BattleskillSkillType.call || !effect.skill.IsCallTargetEnemy || !instance.isRaidBoss(unit.originalUnit)) && (effect.skill.skill_type != BattleskillSkillType.call || !effect.skill.IsCallTargetComplex || !callIsPlayer.HasValue || !instance.isRaidBoss(unit.originalUnit) || (callIsPlayer.Value ? (IEnumerable<BL.ForceID>) BattleFuncs.ForceIDArrayPlayer : (IEnumerable<BL.ForceID>) BattleFuncs.ForceIDArrayPlayerTarget).Contains<BL.ForceID>(BattleFuncs.env.getForceID(unit.originalUnit))))
          {
            if (effect.skill.skill_type == BattleskillSkillType.magic)
              return true;
            BattleFuncs.PackedSkillEffect pse = BattleFuncs.PackedSkillEffect.Create(effect);
            if (BattleFuncs.checkInvokeSkillEffect(pse, useUnit, unit))
            {
              BL.UnitPosition unitPosition = BattleFuncs.iSkillEffectListUnitToUnitPosition(unit);
              BL.Panel panel = BattleFuncs.getPanel(unitPosition.row, unitPosition.column);
              if (pse.CheckTargetLandTag(panel, unit is BL.AIUnit))
                return true;
              break;
            }
            break;
          }
          break;
        case BattleskillEffectLogicEnum.remove_skilleffect:
          BL.UnitPosition unitPosition1 = BattleFuncs.iSkillEffectListUnitToUnitPosition(unit);
          BL.Panel panel1 = BattleFuncs.getPanel(unitPosition1.row, unitPosition1.column);
          if (effect.GetPackedSkillEffect().CheckTargetLandTag(panel1, unit is BL.AIUnit))
          {
            if (useUnit != null)
            {
              int num1 = effect.GetInt(BattleskillEffectLogicArgumentEnum.logic_id);
              int num2 = effect.HasKey(BattleskillEffectLogicArgumentEnum.skill_id) ? effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id) : 0;
              int num3 = effect.HasKey(BattleskillEffectLogicArgumentEnum.skill_type) ? effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_type) : 0;
              int num4 = effect.HasKey(BattleskillEffectLogicArgumentEnum.invest_type) ? effect.GetInt(BattleskillEffectLogicArgumentEnum.invest_type) : 0;
              int num5 = effect.HasKey(BattleskillEffectLogicArgumentEnum.ailment_group_id) ? effect.GetInt(BattleskillEffectLogicArgumentEnum.ailment_group_id) : 0;
              foreach (BL.SkillEffect invalidRemoveEffect in BattleFuncs.getInvalidRemoveEffects(unit, useUnit, effect, useUnit is BL.AIUnit))
              {
                if (!invalidRemoveEffect.useRemain.HasValue)
                {
                  foreach (Tuple<int, int, int, int, int> effectTargetPattern in BattleFuncs.getInvalidRemoveEffectTargetPatterns(invalidRemoveEffect))
                  {
                    int num6 = effectTargetPattern.Item1;
                    int num7 = effectTargetPattern.Item2;
                    int num8 = effectTargetPattern.Item3;
                    int num9 = effectTargetPattern.Item4;
                    int num10 = effectTargetPattern.Item5;
                    if ((num6 == 0 || num6 == num1) && (num7 == 0 || num7 == num2) && (num8 == 0 || num8 == num3) && (num9 == 0 || num9 == num4) && num10 == num5)
                      return false;
                  }
                }
              }
            }
            return true;
          }
          break;
        case BattleskillEffectLogicEnum.invest_skilleffect_im:
          int num11 = effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id);
          if (num11 != 0 && MasterData.BattleskillSkill.ContainsKey(num11))
          {
            BL.SkillEffect perfectAilmentResist = BattleFuncs.getPerfectAilmentResist(BattleFuncs.getAilmentResistEffects(num11, unit, useUnit));
            if (perfectAilmentResist == null || perfectAilmentResist.useRemain.HasValue)
            {
              BL.UnitPosition unitPosition2 = BattleFuncs.iSkillEffectListUnitToUnitPosition(unit);
              BL.Panel panel2 = BattleFuncs.getPanel(unitPosition2.row, unitPosition2.column);
              if (effect.GetPackedSkillEffect().CheckTargetLandTag(panel2, unit is BL.AIUnit))
              {
                BattleskillSkill battleskillSkill = MasterData.BattleskillSkill[num11];
                List<BL.SkillEffect> rangeSkills = (List<BL.SkillEffect>) null;
                using (IEnumerator<BattleskillEffect> enumerator = ((IEnumerable<BattleskillEffect>) battleskillSkill.Effects).Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => !x.EffectLogic.HasTag(BattleskillEffectTag.ext_arg))).GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    BattleskillEffect current = enumerator.Current;
                    switch (current.EffectLogic.Enum)
                    {
                      case BattleskillEffectLogicEnum.seal:
                        Func<int, bool> func1 = (Func<int, bool>) (sid => ((IEnumerable<BL.Skill>) unit.originalUnit.duelSkills).Any<BL.Skill>((Func<BL.Skill, bool>) (x => (sid == 0 || x.skill.ID == sid) && ((IEnumerable<BattleskillEffect>) x.skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (y => y.EffectLogic.Enum != BattleskillEffectLogicEnum.invest_element && y.EffectLogic.Enum != BattleskillEffectLogicEnum.effect_element)))));
                        Func<int, int, int, int, bool> func2 = (Func<int, int, int, int, bool>) ((sid, stype, sinvest, slogic) =>
                        {
                          if (rangeSkills == null)
                            rangeSkills = BattleFuncs.getAllUnits(unit is BL.AIUnit, false, includeJumping: true).SelectMany<BL.ISkillEffectListUnit, BL.SkillEffect>((Func<BL.ISkillEffectListUnit, IEnumerable<BL.SkillEffect>>) (u => u.skillEffects.All().Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.unit != (BL.Unit) null && (x.baseSkill.skill_type == BattleskillSkillType.leader || x.baseSkill.skill_type == BattleskillSkillType.passive && x.baseSkill.range_effect_passive_skill) && x.unit.index == unit.originalUnit.index && x.unit.isPlayerForce == unit.originalUnit.isPlayerForce)))).ToList<BL.SkillEffect>();
                          return unit.skillEffects.All().Concat<BL.SkillEffect>((IEnumerable<BL.SkillEffect>) rangeSkills).Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x =>
                          {
                            if (x.baseSkill.skill_type == BattleskillSkillType.ailment || x.unit != (BL.Unit) null && (x.baseSkill.skill_type == BattleskillSkillType.leader || x.baseSkill.skill_type == BattleskillSkillType.passive && x.baseSkill.range_effect_passive_skill) && (x.unit.index != unit.originalUnit.index || x.unit.isPlayerForce != unit.originalUnit.isPlayerForce) || sid != 0 && x.baseSkillId != sid || stype != 0 && (BattleskillSkillType) stype != x.baseSkill.skill_type || sinvest != 0 && (sinvest != 1 || x.isBaseSkill) && (sinvest != 2 || !x.isBaseSkill))
                              return false;
                            return slogic == 0 || slogic == x.effect.EffectLogic.ID;
                          }));
                        });
                        int num12 = current.HasKey(BattleskillEffectLogicArgumentEnum.invest_type) ? current.GetInt(BattleskillEffectLogicArgumentEnum.invest_type) : 0;
                        int num13 = current.GetInt(BattleskillEffectLogicArgumentEnum.skill_id);
                        int num14 = current.GetInt(BattleskillEffectLogicArgumentEnum.skill_type);
                        int num15 = current.HasKey(BattleskillEffectLogicArgumentEnum.logic_id) ? current.GetInt(BattleskillEffectLogicArgumentEnum.logic_id) : 0;
                        switch (num14)
                        {
                          case 0:
                            if (func2(num13, 1, 0, num15) || func2(num13, 2, 0, num15) || func2(num13, 3, num12, num15) || func2(num13, 6, 0, num15) || func2(num13, 7, 0, num15) || func2(num13, 14, 0, num15) || num15 == 0 && func1(num13))
                              return true;
                            continue;
                          case 1:
                          case 2:
                          case 3:
                          case 6:
                          case 7:
                          case 14:
                            if (func2(num13, num14, num12, num15))
                              return true;
                            continue;
                          case 4:
                            if (func1(num13))
                              return true;
                            continue;
                          default:
                            continue;
                        }
                      case BattleskillEffectLogicEnum.do_not_use_command:
                        if (unit.skills.Length >= 1)
                        {
                          int sid = current.GetInt(BattleskillEffectLogicArgumentEnum.skill_id);
                          if (sid == 0 || ((IEnumerable<BL.Skill>) unit.skills).Any<BL.Skill>((Func<BL.Skill, bool>) (x => x.id == sid)))
                            return true;
                          continue;
                        }
                        continue;
                      case BattleskillEffectLogicEnum.do_not_use_ougi:
                        if (unit.hasOugi)
                        {
                          int num16 = current.GetInt(BattleskillEffectLogicArgumentEnum.skill_id);
                          if (num16 == 0 || unit.ougi.id == num16)
                            return true;
                          continue;
                        }
                        continue;
                      default:
                        return true;
                    }
                  }
                  break;
                }
              }
              else
                break;
            }
            else
              break;
          }
          else
            break;
        case BattleskillEffectLogicEnum.reduct_release_skill_turn:
        case BattleskillEffectLogicEnum.recovery_command_skill_use:
        case BattleskillEffectLogicEnum.fix_immediate_damage:
        case BattleskillEffectLogicEnum.ratio_immediate_damage:
        case BattleskillEffectLogicEnum.immediate_attack:
          BL.UnitPosition unitPosition3 = BattleFuncs.iSkillEffectListUnitToUnitPosition(unit);
          BL.Panel panel3 = BattleFuncs.getPanel(unitPosition3.row, unitPosition3.column);
          if (effect.GetPackedSkillEffect().CheckTargetLandTag(panel3, unit is BL.AIUnit))
            return true;
          break;
        case BattleskillEffectLogicEnum.fix_rebirth:
        case BattleskillEffectLogicEnum.ratio_rebirth:
          if (effect.skill.skill_type == BattleskillSkillType.item || useUnit != null && BattleFuncs.checkInvokeSkillEffect(BattleFuncs.PackedSkillEffect.Create(effect), useUnit, unit))
            return true;
          break;
        case BattleskillEffectLogicEnum.rescue:
          if (useUnit != null)
          {
            BL.UnitPosition unitPosition4 = BattleFuncs.iSkillEffectListUnitToUnitPosition(unit);
            BL.Panel panel4 = BattleFuncs.getPanel(unitPosition4.row, unitPosition4.column);
            BL.UnitPosition unitPosition5 = BattleFuncs.iSkillEffectListUnitToUnitPosition(useUnit);
            if (BattleFuncs.getAnchorGroundEffects(unit, useUnit, panel4, BattleFuncs.getPanel(unitPosition5.row, unitPosition5.column)).Any<BL.SkillEffect>())
            {
              enableAnchorGround = true;
              break;
            }
            if (effect.GetPackedSkillEffect().CheckTargetLandTag(panel4, unit is BL.AIUnit))
              return true;
            break;
          }
          break;
        case BattleskillEffectLogicEnum.attract:
          if (useUnit != null)
          {
            BL.UnitPosition unitPosition6 = BattleFuncs.iSkillEffectListUnitToUnitPosition(useUnit);
            BL.UnitPosition unitPosition7 = BattleFuncs.iSkillEffectListUnitToUnitPosition(unit);
            if (BattleFuncs.getAnchorGroundEffects(unit, useUnit, BattleFuncs.getPanel(unitPosition7.row, unitPosition7.column), BattleFuncs.getPanel(unitPosition6.row, unitPosition6.column)).Any<BL.SkillEffect>())
            {
              enableAnchorGround = true;
              break;
            }
            if (BattleFuncs.getAnchorGroundEffects(useUnit, useUnit, BattleFuncs.getPanel(unitPosition6.row, unitPosition6.column), BattleFuncs.getPanel(unitPosition6.row, unitPosition6.column)).Any<BL.SkillEffect>())
            {
              enableAnchorGround = true;
              break;
            }
            if ((effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == unit.originalUnit.unit.kind.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) == 0 || unit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id))) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == unit.originalUnit.playerUnit.GetElement()))
            {
              BL.Panel panel5 = BattleFuncs.getPanel(unitPosition7.row, unitPosition7.column);
              if (effect.GetPackedSkillEffect().CheckTargetLandTag(panel5, unit is BL.AIUnit))
              {
                int[] attractDelta = BattleFuncs.getAttractDelta(unitPosition6, unitPosition7);
                BL.Unit[] ignoreUnits = new BL.Unit[2]
                {
                  useUnit.originalUnit,
                  unit.originalUnit
                };
                if (BattleFuncs.isResetPositionOK(useUnit.originalUnit, unitPosition6.row + attractDelta[0], unitPosition6.column + attractDelta[1], unitPosition6.unit.parameter.Move, (IEnumerable<BL.Unit>) ignoreUnits) && BattleFuncs.isResetPositionOK(unit.originalUnit, unitPosition7.row + attractDelta[0], unitPosition7.column + attractDelta[1], unitPosition7.unit.parameter.Move, (IEnumerable<BL.Unit>) ignoreUnits))
                  return true;
                break;
              }
              break;
            }
            break;
          }
          break;
        case BattleskillEffectLogicEnum.shift_break:
          if (useUnit != null)
          {
            BL.UnitPosition unitPosition8 = BattleFuncs.iSkillEffectListUnitToUnitPosition(useUnit);
            if (!BattleFuncs.getAnchorGroundEffects(useUnit, useUnit, BattleFuncs.getPanel(unitPosition8.row, unitPosition8.column), BattleFuncs.getPanel(unitPosition8.row, unitPosition8.column)).Any<BL.SkillEffect>())
            {
              if (firstTarget != null)
                unit = firstTarget;
              if ((effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == unit.originalUnit.unit.kind.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) == 0 || unit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id))) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == unit.originalUnit.playerUnit.GetElement()))
              {
                BL.UnitPosition unitPosition9 = BattleFuncs.iSkillEffectListUnitToUnitPosition(unit);
                BL.Panel panel6 = BattleFuncs.getPanel(unitPosition9.row, unitPosition9.column);
                if (effect.GetPackedSkillEffect().CheckTargetLandTag(panel6, unit is BL.AIUnit) && BattleFuncs.getShiftBreakPosition(useUnit, unit, effect.GetInt(BattleskillEffectLogicArgumentEnum.range), out int _, out int _))
                  return true;
                break;
              }
              break;
            }
            break;
          }
          break;
        case BattleskillEffectLogicEnum.keep_away:
          if (useUnit != null)
          {
            BL.UnitPosition unitPosition10 = BattleFuncs.iSkillEffectListUnitToUnitPosition(useUnit);
            BL.UnitPosition unitPosition11 = BattleFuncs.iSkillEffectListUnitToUnitPosition(unit);
            if (BattleFuncs.getAnchorGroundEffects(unit, useUnit, BattleFuncs.getPanel(unitPosition11.row, unitPosition11.column), BattleFuncs.getPanel(unitPosition10.row, unitPosition10.column)).Any<BL.SkillEffect>())
            {
              enableAnchorGround = true;
              break;
            }
            if ((effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == unit.originalUnit.unit.kind.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) == 0 || unit.originalUnit.unit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id))) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == unit.originalUnit.playerUnit.GetElement()))
            {
              BL.Panel panel7 = BattleFuncs.getPanel(unitPosition11.row, unitPosition11.column);
              if (effect.GetPackedSkillEffect().CheckTargetLandTag(panel7, unit is BL.AIUnit))
              {
                int penetrateCount = effect.GetInt(BattleskillEffectLogicArgumentEnum.range);
                List<Tuple<int, int, int>> penetratePosition = BattleFuncs.getPenetratePosition(unitPosition10.row, unitPosition10.column, unitPosition11.row, unitPosition11.column, penetrateCount);
                BL.Unit[] ignoreUnits = new BL.Unit[0];
                using (List<Tuple<int, int, int>>.Enumerator enumerator = penetratePosition.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    Tuple<int, int, int> current = enumerator.Current;
                    if (BattleFuncs.isResetPositionOK(unit.originalUnit, current.Item1, current.Item2, unitPosition11.unit.parameter.Move, (IEnumerable<BL.Unit>) ignoreUnits))
                      return true;
                  }
                  break;
                }
              }
              else
                break;
            }
            else
              break;
          }
          else
            break;
        case BattleskillEffectLogicEnum.dance:
          if (useUnit != null)
          {
            BattleFuncs.PackedSkillEffect pse = BattleFuncs.PackedSkillEffect.Create(effect);
            if (BattleFuncs.checkInvokeSkillEffect(pse, useUnit, unit))
            {
              BL.UnitPosition unitPosition12 = BattleFuncs.iSkillEffectListUnitToUnitPosition(unit);
              BL.Panel panel8 = BattleFuncs.getPanel(unitPosition12.row, unitPosition12.column);
              if (pse.CheckTargetLandTag(panel8, unit is BL.AIUnit))
              {
                if (!(useUnit is BL.AIUnit))
                {
                  if (BattleFuncs.env.getActionUnits(unitPosition12) == null)
                    return true;
                  break;
                }
                if (!((IEnumerable<BL.ISkillEffectListUnit>) BattleFuncs.env.aiActionUnits.value).Contains<BL.ISkillEffectListUnit>(unit))
                  return true;
                break;
              }
              break;
            }
            break;
          }
          break;
        case BattleskillEffectLogicEnum.changing:
          if (useUnit != null)
          {
            BL.UnitPosition unitPosition13 = BattleFuncs.iSkillEffectListUnitToUnitPosition(useUnit);
            BL.UnitPosition unitPosition14 = BattleFuncs.iSkillEffectListUnitToUnitPosition(unit);
            if (!BattleFuncs.getAnchorGroundEffects(unit, useUnit, BattleFuncs.getPanel(unitPosition14.row, unitPosition14.column), BattleFuncs.getPanel(unitPosition13.row, unitPosition13.column)).Any<BL.SkillEffect>() && !BattleFuncs.getAnchorGroundEffects(useUnit, useUnit, BattleFuncs.getPanel(unitPosition13.row, unitPosition13.column), BattleFuncs.getPanel(unitPosition13.row, unitPosition13.column)).Any<BL.SkillEffect>())
            {
              BattleFuncs.PackedSkillEffect pse = BattleFuncs.PackedSkillEffect.Create(effect);
              if (BattleFuncs.checkInvokeSkillEffect(pse, useUnit, unit))
              {
                BL.Panel panel9 = BattleFuncs.getPanel(unitPosition14.row, unitPosition14.column);
                if (pse.CheckTargetLandTag(panel9, unit is BL.AIUnit))
                {
                  BL.Unit[] ignoreUnits = new BL.Unit[2]
                  {
                    useUnit.originalUnit,
                    unit.originalUnit
                  };
                  if (BattleFuncs.isResetPositionOK(useUnit.originalUnit, unitPosition14.row, unitPosition14.column, unitPosition13.unit.parameter.Move, (IEnumerable<BL.Unit>) ignoreUnits) && BattleFuncs.isResetPositionOK(unit.originalUnit, unitPosition13.row, unitPosition13.column, unitPosition14.unit.parameter.Move, (IEnumerable<BL.Unit>) ignoreUnits))
                    return true;
                  break;
                }
                break;
              }
              break;
            }
            break;
          }
          break;
        case BattleskillEffectLogicEnum.steal:
          if (useUnit != null)
          {
            BattleFuncs.PackedSkillEffect pse = BattleFuncs.PackedSkillEffect.Create(effect);
            if (BattleFuncs.checkInvokeSkillEffect(pse, useUnit, unit))
            {
              BL.UnitPosition unitPosition15 = BattleFuncs.iSkillEffectListUnitToUnitPosition(unit);
              BL.Panel panel10 = BattleFuncs.getPanel(unitPosition15.row, unitPosition15.column);
              if (pse.CheckTargetLandTag(panel10, unit is BL.AIUnit) && BattleFuncs.calcStealEffectParam(unit.parameter, effect, out int _))
                return true;
              break;
            }
            break;
          }
          break;
        case BattleskillEffectLogicEnum.transformation:
          if (useUnit != null)
          {
            BL.UnitPosition unitPosition16 = BattleFuncs.iSkillEffectListUnitToUnitPosition(unit);
            BL.Panel panel11 = BattleFuncs.getPanel(unitPosition16.row, unitPosition16.column);
            if (effect.GetPackedSkillEffect().CheckTargetLandTag(panel11, unit is BL.AIUnit) && useUnit.transformationGroupId != effect.GetInt(BattleskillEffectLogicArgumentEnum.transformation_group_id))
              return true;
            break;
          }
          break;
        case BattleskillEffectLogicEnum.jump:
          if (useUnit != null)
          {
            BL.ISkillEffectListUnit skillEffectListUnit = ((effect.HasKey(BattleskillEffectLogicArgumentEnum.effect_target) ? effect.GetInt(BattleskillEffectLogicArgumentEnum.effect_target) : 0) == 0 ? 1 : (useUnit == unit ? 1 : 0)) != 0 ? useUnit : unit;
            BL.UnitPosition unitPosition17 = BattleFuncs.iSkillEffectListUnitToUnitPosition(useUnit);
            BL.UnitPosition unitPosition18 = BattleFuncs.iSkillEffectListUnitToUnitPosition(skillEffectListUnit);
            if (!BattleFuncs.getAnchorGroundEffects(skillEffectListUnit, useUnit, BattleFuncs.getPanel(unitPosition18.row, unitPosition18.column), BattleFuncs.getPanel(unitPosition17.row, unitPosition17.column)).Any<BL.SkillEffect>() && BattleFuncs.checkInvokeSkillEffect(BattleFuncs.PackedSkillEffect.Create(effect), useUnit, unit))
              return true;
            break;
          }
          break;
        case BattleskillEffectLogicEnum.reduct_command_skill_use:
          if (BattleFuncs.getReductCommandSkillUseTargetSkills(unit, effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id), effect.GetInt(BattleskillEffectLogicArgumentEnum.logic_id), useUnit, effect.skill_BattleskillSkill).Any<BL.Skill>())
            return true;
          break;
        case BattleskillEffectLogicEnum.provide:
          if (useUnit != null)
          {
            BattleFuncs.PackedSkillEffect pse = BattleFuncs.PackedSkillEffect.Create(effect);
            if (BattleFuncs.checkInvokeSkillEffect(pse, useUnit, unit))
            {
              BL.UnitPosition unitPosition19 = BattleFuncs.iSkillEffectListUnitToUnitPosition(unit);
              BL.Panel panel12 = BattleFuncs.getPanel(unitPosition19.row, unitPosition19.column);
              if (pse.CheckTargetLandTag(panel12, unit is BL.AIUnit))
              {
                if (battleParameter == null)
                  battleParameter = useUnit.parameter;
                if (BattleFuncs.calcStealEffectParam(battleParameter, effect, out int _))
                  return true;
                break;
              }
              break;
            }
            break;
          }
          break;
        default:
          return true;
      }
      return false;
    }

    public static bool canUseImmediatePanelSkillEffect(
      BattleskillEffect effect,
      BL.Panel panel,
      BL.ISkillEffectListUnit useUnit)
    {
      switch (effect.EffectLogic.Enum)
      {
        case BattleskillEffectLogicEnum.map_shift:
          if (useUnit == null)
            return false;
          BL.UnitPosition unitPosition1 = BattleFuncs.iSkillEffectListUnitToUnitPosition(useUnit);
          if (BattleFuncs.getAnchorGroundEffects(useUnit, useUnit, BattleFuncs.getPanel(unitPosition1.row, unitPosition1.column), BattleFuncs.getPanel(unitPosition1.row, unitPosition1.column)).Any<BL.SkillEffect>())
            return false;
          return BattleFuncs.isResetPositionOK(useUnit.originalUnit, panel.row, panel.column, unitPosition1.unit.parameter.Move, (IEnumerable<BL.Unit>) new BL.Unit[1]
          {
            useUnit.originalUnit
          }) && BattleFuncs.canWarpPanel(useUnit, panel.row, panel.column) && effect.GetPackedSkillEffect().CheckTargetLandTag(panel, useUnit is BL.AIUnit);
        case BattleskillEffectLogicEnum.invest_land_tag:
          return true;
        case BattleskillEffectLogicEnum.inhale:
          if (useUnit == null)
            return false;
          BattleFuncs.PackedSkillEffect packedSkillEffect = effect.GetPackedSkillEffect();
          BL.ForceID[] forceIds = effect.GetInt(BattleskillEffectLogicArgumentEnum.effect_target) == 0 ? BattleFuncs.getForceIDArray(BattleFuncs.getForceID(useUnit.originalUnit)) : BattleFuncs.getTargetForce(useUnit.originalUnit, useUnit.IsCharm);
          List<BL.UnitPosition> targets = BattleFuncs.getTargets(panel.row, panel.column, new int[2]
          {
            0,
            effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range)
          }, forceIds, BL.Unit.TargetAttribute.all, (useUnit is BL.AIUnit ? 1 : 0) != 0, nonFacility: true);
          BL.UnitPosition unitPosition2 = BattleFuncs.iSkillEffectListUnitToUnitPosition(useUnit);
          BL.Panel panel1 = BattleFuncs.getPanel(unitPosition2.row, unitPosition2.column);
          foreach (BL.UnitPosition up in targets)
          {
            BL.ISkillEffectListUnit iskillEffectListUnit = BattleFuncs.unitPositionToISkillEffectListUnit(up);
            if (!BattleFuncs.getAnchorGroundEffects(iskillEffectListUnit, useUnit, BattleFuncs.getPanel(up.row, up.column), panel1).Any<BL.SkillEffect>() && BattleFuncs.checkInvokeSkillEffect(packedSkillEffect, useUnit, iskillEffectListUnit))
              return true;
          }
          return false;
        case BattleskillEffectLogicEnum.facility_creation:
          if (useUnit == null)
            return false;
          int fid = effect.GetInt(BattleskillEffectLogicArgumentEnum.facility_unit_id);
          BL.Unit unit = BattleFuncs.env.facilityUnits.value.FirstOrDefault<BL.Unit>((Func<BL.Unit, bool>) (x => x.playerUnit._unit == fid));
          if (unit == (BL.Unit) null || panel.landform.GetIncr(unit.job.move_type).move_cost >= 100)
            return false;
          BL.UnitPosition[] unitPositionArray = useUnit is BL.AIUnit ? BattleFuncs.getFieldUnitsAI(panel.row, panel.column, true) : BattleFuncs.env.getFieldUnits(panel.row, panel.column, includeJumping: true);
          if (unitPositionArray != null)
          {
            foreach (BL.UnitPosition unitPosition3 in unitPositionArray)
            {
              if (!unitPosition3.unit.isPutOn)
                return false;
            }
          }
          return true;
        default:
          return false;
      }
    }

    public static bool canUseSkillToPanel(
      BattleskillSkill skill,
      int level,
      BL.Panel panel,
      BL.ISkillEffectListUnit useUnit,
      int nowUseCount)
    {
      BattleskillEffect[] array = ((IEnumerable<BattleskillEffect>) skill.Effects).Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum != BattleskillEffectLogicEnum.hp_consume && x.EffectLogic.Enum != BattleskillEffectLogicEnum.skill_chain && x.EffectLogic.Enum != BattleskillEffectLogicEnum.change_skill_range && x.EffectLogic.Enum != BattleskillEffectLogicEnum.change_skill_use_count && x.EffectLogic.Enum != BattleskillEffectLogicEnum.random_choice && x.EffectLogic.Enum != BattleskillEffectLogicEnum.again_use_skill && x.EffectLogic.Enum != BattleskillEffectLogicEnum.use_skill_count_range_effect && !x.EffectLogic.HasTag(BattleskillEffectTag.ext_arg) && x.checkLevel(level) && x.checkUseSkillCount(nowUseCount))).ToArray<BattleskillEffect>();
      if (array.Length != 0)
      {
        bool flag1 = false;
        bool flag2 = false;
        bool panel1 = false;
        foreach (BattleskillEffect effect in array)
        {
          switch (effect.EffectLogic.Enum)
          {
            case BattleskillEffectLogicEnum.map_shift:
            case BattleskillEffectLogicEnum.facility_creation:
              if (!BattleFuncs.canUseImmediatePanelSkillEffect(effect, panel, useUnit))
                return false;
              flag1 = true;
              continue;
            case BattleskillEffectLogicEnum.invest_land_tag:
              continue;
            case BattleskillEffectLogicEnum.inhale:
              if (BattleFuncs.canUseImmediatePanelSkillEffect(effect, panel, useUnit))
                panel1 = true;
              flag2 = true;
              continue;
            default:
              return false;
          }
        }
        if (flag1)
          return true;
        if (flag2)
          return panel1;
        foreach (BattleskillEffect effect in array)
        {
          if (BattleFuncs.canUseImmediatePanelSkillEffect(effect, panel, useUnit))
            return true;
        }
      }
      foreach (Tuple<BattleskillSkill, IEnumerable<BL.ISkillEffectListUnit>> chainSkillTarget in BattleFuncs.getChainSkillTargets(skill, level, (BL.ISkillEffectListUnit) null, panel, useUnit, nowUseCount))
      {
        Tuple<BattleskillSkill, IEnumerable<BL.ISkillEffectListUnit>> st = chainSkillTarget;
        if (st.Item2.Any<BL.ISkillEffectListUnit>((Func<BL.ISkillEffectListUnit, bool>) (x => x.skillEffects.CanUseSkillOne(st.Item1, level, x, BattleFuncs.env, useUnit, nowUseCount) == 0)))
          return true;
      }
      return false;
    }

    public static IEnumerable<Tuple<BattleskillSkill, IEnumerable<BL.ISkillEffectListUnit>>> getChainSkillTargets(
      BattleskillSkill skill,
      int level,
      BL.ISkillEffectListUnit unit,
      BL.Panel panel,
      BL.ISkillEffectListUnit useUnit,
      int nowUseCount,
      Tuple<int, int> usePosition = null)
    {
      if (useUnit != null)
      {
        bool isAI = useUnit is BL.AIUnit;
        Queue<BattleskillEffect> chainSkillEffect = new Queue<BattleskillEffect>();
        Action<BattleskillSkill> func = (Action<BattleskillSkill>) (s =>
        {
          foreach (BattleskillEffect battleskillEffect in ((IEnumerable<BattleskillEffect>) s.Effects).Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.skill_chain && x.checkLevel(level) && x.checkUseSkillCount(nowUseCount))))
            chainSkillEffect.Enqueue(battleskillEffect);
        });
        func(skill);
        while (chainSkillEffect.Count > 0)
        {
          BattleskillEffect headerEffect = chainSkillEffect.Dequeue();
          if (BattleFuncs.checkInvokeSkillEffect(BattleFuncs.PackedSkillEffect.Create(headerEffect), useUnit, unit))
          {
            int key = headerEffect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id);
            if (key != 0 && MasterData.BattleskillSkill.ContainsKey(key))
            {
              skill = MasterData.BattleskillSkill[key];
              BL.Skill skill1 = new BL.Skill() { id = key };
              int excluding_slanting = headerEffect.GetInt(BattleskillEffectLogicArgumentEnum.excluding_slanting);
              if (excluding_slanting == 3 || excluding_slanting == 4)
              {
                BL.UnitPosition unitPosition1 = BattleFuncs.iSkillEffectListUnitToUnitPosition(useUnit);
                List<Tuple<int, int>> tupleList = new List<Tuple<int, int>>();
                if (excluding_slanting == 3)
                {
                  int row;
                  int column;
                  if (panel != null)
                  {
                    row = panel.row;
                    column = panel.column;
                  }
                  else if (unit != null)
                  {
                    BL.UnitPosition unitPosition2 = BattleFuncs.iSkillEffectListUnitToUnitPosition(unit);
                    row = unitPosition2.row;
                    column = unitPosition2.column;
                  }
                  else
                    continue;
                  int minRange = skill.min_range;
                  int maxRange = skill.max_range;
                  int radius = headerEffect.GetInt(BattleskillEffectLogicArgumentEnum.penetrate_radius);
                  if (radius != 0)
                  {
                    tupleList = BattleFuncs.getLaserPosition(unitPosition1.row, unitPosition1.column, row, column, minRange, maxRange, radius, headerEffect.GetInt(BattleskillEffectLogicArgumentEnum.is_range_from_target) == 1).ToList<Tuple<int, int>>();
                  }
                  else
                  {
                    List<Tuple<int, int, int>> penetratePosition = BattleFuncs.getPenetratePosition(unitPosition1.row, unitPosition1.column, row, column, maxRange, true);
                    if (headerEffect.GetInt(BattleskillEffectLogicArgumentEnum.is_range_from_target) == 0)
                    {
                      for (int index = 0; index < maxRange; ++index)
                        tupleList.Add(Tuple.Create<int, int>(penetratePosition[index].Item1, penetratePosition[index].Item2));
                    }
                    else
                    {
                      foreach (Tuple<int, int, int> tuple in penetratePosition)
                      {
                        if (tuple.Item3 <= maxRange && tuple.Item3 >= minRange)
                          tupleList.Add(Tuple.Create<int, int>(tuple.Item1, tuple.Item2));
                      }
                    }
                  }
                }
                else if (usePosition != null)
                {
                  List<Tuple<int, int, int>> penetratePosition = BattleFuncs.getPenetratePosition(usePosition.Item1, usePosition.Item2, unitPosition1.row, unitPosition1.column, 0, true);
                  for (int index = 0; index < penetratePosition.Count; ++index)
                  {
                    if (penetratePosition[index].Item3 < 0)
                      tupleList.Add(Tuple.Create<int, int>(penetratePosition[index].Item1, penetratePosition[index].Item2));
                  }
                }
                else
                  continue;
                List<BL.ISkillEffectListUnit> skillEffectListUnitList = new List<BL.ISkillEffectListUnit>();
                foreach (Tuple<int, int> tuple in tupleList)
                {
                  BL.UnitPosition[] unitPositionArray = isAI ? BattleFuncs.getFieldUnitsAI(tuple.Item1, tuple.Item2) : BattleFuncs.env.getFieldUnits(tuple.Item1, tuple.Item2);
                  if (unitPositionArray != null)
                  {
                    BL.ForceID[] targetForceIds = skill1.getTargetForceIDs(BattleFuncs.env, (BL.ISkillEffectListUnit) useUnit.originalUnit);
                    foreach (BL.UnitPosition up in unitPositionArray)
                    {
                      BL.ISkillEffectListUnit iskillEffectListUnit = BattleFuncs.unitPositionToISkillEffectListUnit(up);
                      if (BattleFuncs.checkTargetp(iskillEffectListUnit, targetForceIds, skill1.targetAttribute, skill1.nonFacility))
                        skillEffectListUnitList.Add(iskillEffectListUnit);
                    }
                  }
                }
                yield return Tuple.Create<BattleskillSkill, IEnumerable<BL.ISkillEffectListUnit>>(skill, (IEnumerable<BL.ISkillEffectListUnit>) skillEffectListUnitList);
              }
              else
              {
                int row;
                int column;
                if (headerEffect.GetInt(BattleskillEffectLogicArgumentEnum.is_range_from_target) == 0)
                {
                  BL.UnitPosition unitPosition = BattleFuncs.iSkillEffectListUnitToUnitPosition(useUnit);
                  row = unitPosition.row;
                  column = unitPosition.column;
                }
                else if (panel != null)
                {
                  row = panel.row;
                  column = panel.column;
                }
                else if (unit != null)
                {
                  BL.UnitPosition unitPosition = BattleFuncs.iSkillEffectListUnitToUnitPosition(unit);
                  row = unitPosition.row;
                  column = unitPosition.column;
                }
                else
                  continue;
                yield return Tuple.Create<BattleskillSkill, IEnumerable<BL.ISkillEffectListUnit>>(skill, BattleFuncs.env.getSkillTargetUnits(useUnit.originalUnit, row, column, skill1, isAI).Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (tup =>
                {
                  switch (excluding_slanting)
                  {
                    case 1:
                      if (tup.row != row && tup.column != column)
                        return false;
                      break;
                    case 2:
                      if (Mathf.Abs(tup.row - row) != Mathf.Abs(tup.column - column))
                        return false;
                      break;
                  }
                  return true;
                })).Select<BL.UnitPosition, BL.ISkillEffectListUnit>((Func<BL.UnitPosition, BL.ISkillEffectListUnit>) (x => BattleFuncs.unitPositionToISkillEffectListUnit(x))));
              }
              func(skill);
            }
          }
        }
      }
    }

    public static IEnumerable<BattleFuncs.InvestSkill> getInvestSkills(BL.UnitPosition up)
    {
      BL.ISkillEffectListUnit unit = BattleFuncs.unitPositionToISkillEffectListUnit(up);
      IEnumerable<\u003C\u003Ef__AnonymousType4<BL.SkillEffect, BL.Unit, bool>> first = unit.skillEffects.All().Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (effect =>
      {
        if (effect.isDontDisplay)
          return false;
        if (effect.baseSkill.skill_type == BattleskillSkillType.ailment)
          return true;
        return (!effect.isBaseSkill || effect.baseSkill.skill_type != BattleskillSkillType.passive || effect.baseSkill.range_effect_passive_skill) && BattleFuncs.checkEffectLogicEnabled(effect, unit);
      })).Select(x => new
      {
        effect = x,
        fromUnit = x.investUnit,
        isSeal = BattleFuncs.isSealedSkillEffect(unit, x)
      });
      IEnumerable<\u003C\u003Ef__AnonymousType4<BL.SkillEffect, BL.Unit, bool>> datas = unit.skillEffects.GetAllFixEffectParams().Where<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, int>>((Func<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, int>, bool>) (param =>
      {
        BL.SkillEffect skillEffect = param.Item2;
        return !skillEffect.isBaseSkill || skillEffect.baseSkill.skill_type != BattleskillSkillType.passive || skillEffect.baseSkill.range_effect_passive_skill;
      })).Select(x => new
      {
        effect = x.Item2,
        fromUnit = x.Item2.investUnit,
        isSeal = BattleFuncs.isSealedSkillEffect(unit, x.Item2)
      });
      IEnumerable<\u003C\u003Ef__AnonymousType4<BL.SkillEffect, BL.Unit, bool>> second1 = unit.skillEffects.GetAllRatioEffectParams().Where<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>>((Func<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>, bool>) (param =>
      {
        BL.SkillEffect skillEffect = param.Item2;
        return !skillEffect.isBaseSkill || skillEffect.baseSkill.skill_type != BattleskillSkillType.passive || skillEffect.baseSkill.range_effect_passive_skill;
      })).Select(x => new
      {
        effect = x.Item2,
        fromUnit = x.Item2.investUnit,
        isSeal = BattleFuncs.isSealedSkillEffect(unit, x.Item2)
      });
      IEnumerable<\u003C\u003Ef__AnonymousType4<BL.SkillEffect, BL.Unit, bool>> second2 = datas;
      foreach (IGrouping<\u003C\u003Ef__AnonymousType5<int, int, bool, bool>, \u003C\u003Ef__AnonymousType4<BL.SkillEffect, BL.Unit, bool>> grouping in first.Concat(second2).Concat(second1).GroupBy(x =>
      {
        BL.SkillEffect effect = x.effect;
        int num;
        bool flag1;
        if (x.fromUnit != (BL.Unit) null)
        {
          num = x.fromUnit.index;
          flag1 = x.fromUnit.isPlayerForce;
        }
        else
        {
          num = -1;
          flag1 = false;
          BattleskillSkill baseSkill = effect.baseSkill;
          if (baseSkill.skill_type == BattleskillSkillType.call && baseSkill.IsCallTargetComplex && !effect.effect.is_targer_enemy)
            flag1 = true;
        }
        bool flag2 = false;
        int key;
        if (effect.investSkillId != 0)
        {
          key = effect.investSkillId;
          if (MasterData.BattleskillSkill.ContainsKey(key))
          {
            BattleskillSkill battleskillSkill = MasterData.BattleskillSkill[key];
            if (battleskillSkill.skill_type == BattleskillSkillType.leader || battleskillSkill.skill_type == BattleskillSkillType.passive && battleskillSkill.range_effect_passive_skill)
              flag2 = true;
          }
        }
        else
          key = effect.baseSkillId;
        return new
        {
          skillId = key,
          index = num,
          isPlayerControl = flag1,
          invstFromLeaderSkill = flag2
        };
      }))
      {
        BL.SkillEffect skillEffect = (BL.SkillEffect) null;
        BL.Unit unit1 = (BL.Unit) null;
        int? nullable1 = new int?();
        foreach (var data in grouping)
        {
          BL.SkillEffect effect = data.effect;
          if (skillEffect == null)
          {
            skillEffect = effect;
            unit1 = data.fromUnit;
          }
          int? nullable2 = effect.baseSkill.skill_type != BattleskillSkillType.ailment ? effect.turnRemain : effect.executeRemain;
          if (nullable2.HasValue)
          {
            if (nullable1.HasValue)
            {
              int? nullable3 = nullable2;
              int? nullable4 = nullable1;
              if (!(nullable3.GetValueOrDefault() > nullable4.GetValueOrDefault() & nullable3.HasValue & nullable4.HasValue))
                continue;
            }
            nullable1 = nullable2;
          }
        }
        if (skillEffect != null)
        {
          int skillId = grouping.Key.skillId;
          if (MasterData.BattleskillSkill.ContainsKey(skillId))
          {
            BattleskillSkill battleskillSkill = MasterData.BattleskillSkill[skillId];
            yield return new BattleFuncs.InvestSkill()
            {
              skill = battleskillSkill,
              turnRemain = nullable1,
              fromEnemy = battleskillSkill.skill_type == BattleskillSkillType.call && (battleskillSkill.IsCallTargetEnemy || battleskillSkill.IsCallTargetComplex && skillEffect.effect.is_targer_enemy) || unit1 != (BL.Unit) null && BattleFuncs.env.getForceID(unit1) != BattleFuncs.env.getForceID(unit.originalUnit),
              fromFriend = unit1 != (BL.Unit) null && unit1.friend,
              isEnemyIcon = unit1 != (BL.Unit) null && BattleFuncs.env.getForceID(unit1) == BL.ForceID.enemy
            };
          }
        }
      }
    }

    private static bool checkEffectLogicEnabled(BL.SkillEffect effect, BL.ISkillEffectListUnit unit)
    {
      int id = effect.effect.EffectLogic.ID;
      try
      {
        if (effect.useRemain.HasValue && effect.useRemain.Value <= 0)
          return false;
        if (id == 1001338)
        {
          if (effect.work != null)
          {
            if (BattleFuncs.getShieldRemain(effect, unit) <= 0)
              return false;
          }
        }
      }
      catch (Exception ex)
      {
      }
      return BattleFuncs.checkPassiveEffectEnable(effect.effect, unit) == 1;
    }

    public static int checkPassiveEffectEnable(
      BattleskillEffect effect,
      BL.ISkillEffectListUnit unit)
    {
      int id = effect.EffectLogic.ID;
      bool? nullable = new bool?();
      try
      {
        int optTest3 = effect.EffectLogic.opt_test3;
        if (optTest3 <= 1012)
        {
          switch (optTest3)
          {
            case 0:
              if (effect.EffectLogic.opt_test1 != 0)
              {
                nullable = new bool?(Judgement.CheckEnabledBuffDebuff(effect, unit.originalUnit, BattleskillInvokeGameModeEnum.quest));
                goto label_92;
              }
              else
                goto label_92;
            case 1:
              nullable = new bool?(Judgement.CheckEnabledEquipGearBuffDebuff(effect, unit));
              goto label_92;
            case 2:
              nullable = new bool?(Judgement.CheckEnabledRangeBuffDebuff(effect, unit));
              goto label_92;
            case 3:
              nullable = new bool?(Judgement.CheckEnabledRangeBuffDebuff(effect, unit));
              goto label_92;
            case 4:
            case 5:
              nullable = new bool?(Judgement.CheckEnabledHpBuffDebuff(effect, unit));
              goto label_92;
            case 6:
              nullable = new bool?(Judgement.CheckEnabledTargetCountBuffDebuff(effect, unit));
              goto label_92;
            case 7:
              break;
            case 8:
              nullable = new bool?(Judgement.CheckEnabledCavalryRushBuffDebuff(effect, unit));
              goto label_92;
            case 9:
              nullable = new bool?(Judgement.CheckEnabledRaidMissionBuffDebuff(effect, unit));
              goto label_92;
            case 10:
              nullable = new bool?(Judgement.CheckEnabledExtremeOfForceBuffDebuff(effect, unit));
              goto label_92;
            case 11:
              nullable = new bool?(Judgement.CheckEnabledOnemanChargeBuffDebuff(effect, unit));
              goto label_92;
            case 12:
            case 13:
              nullable = new bool?(Judgement.CheckEnabledInOutSideBattleBuffDebuff(effect, unit));
              goto label_92;
            case 14:
              nullable = new bool?(Judgement.CheckEnabledEvenIllusionBuffDebuff(effect, unit));
              goto label_92;
            case 15:
              nullable = new bool?(Judgement.CheckEnabledSpecificUnitBuffDebuff(effect, unit));
              goto label_92;
            case 16:
              nullable = new bool?(Judgement.CheckEnabledUnitRarityBuffDebuff(effect, unit));
              goto label_92;
            case 17:
            case 18:
            case 19:
              nullable = new bool?(Judgement.CheckEnabledDeadCountBuffDebuff(effect, unit, BattleskillInvokeGameModeEnum.quest));
              goto label_92;
            case 20:
              nullable = new bool?(Judgement.CheckEnabledBuffDebuff2(effect, unit.originalUnit, BattleskillInvokeGameModeEnum.quest));
              goto label_92;
            case 21:
            case 22:
              nullable = new bool?(Judgement.CheckEnabledSpecificGroupBuffDebuff(effect, unit));
              goto label_92;
            case 23:
              nullable = new bool?(Judgement.CheckEnabledSpecificSkillGroupBuffDebuff(effect, unit));
              goto label_92;
            case 24:
              nullable = new bool?(Judgement.CheckEnabledEnemyBuffDebuff(effect, unit, BattleskillInvokeGameModeEnum.quest));
              goto label_92;
            case 25:
            case 26:
            case 27:
            case 28:
              nullable = new bool?(Judgement.CheckEnabledParamDiffBuffDebuff(effect, unit));
              goto label_92;
            case 29:
              nullable = new bool?(Judgement.CheckEnabledBuffDebuff3(effect, unit));
              goto label_92;
            case 30:
              nullable = new bool?(Judgement.CheckEnabledBuffDebuff4(effect, unit));
              goto label_92;
            case 31:
              nullable = new bool?(Judgement.CheckEnabledAttackClassBuffDebuff(effect, unit));
              goto label_92;
            case 32:
              nullable = new bool?(Judgement.CheckEnabledAttackElementBuffDebuff(effect, unit));
              goto label_92;
            case 33:
              nullable = new bool?(Judgement.CheckEnabledInvestLogicBuffDebuff(effect, unit));
              goto label_92;
            case 34:
              nullable = new bool?(Judgement.CheckEnabledEnemyInvestLogicBuffDebuff(effect, unit));
              goto label_92;
            case 35:
              nullable = new bool?(Judgement.CheckEnabledEvenIllusion2BuffDebuff(effect, unit, BattleskillInvokeGameModeEnum.quest));
              goto label_92;
            case 36:
              nullable = new bool?(Judgement.CheckEnabledEvenIllusion3BuffDebuff(effect, unit, BattleskillInvokeGameModeEnum.quest));
              goto label_92;
            case 37:
            case 38:
              nullable = new bool?(Judgement.CheckEnabledPeculiarParameterRangeBuffDebuff(effect, unit));
              goto label_92;
            case 39:
              nullable = new bool?(Judgement.CheckEnabledLevelUpStatusBuffDebuff(effect, unit));
              goto label_92;
            case 40:
              nullable = new bool?(Judgement.CheckEnabledUseRemainBuffDebuff(effect, unit));
              goto label_92;
            case 41:
            case 42:
              nullable = new bool?(Judgement.CheckEnabledBuffDebuffClamp(effect, unit));
              goto label_92;
            case 43:
              nullable = new bool?(Judgement.CheckEnabledGenericBuffDebuff(effect, unit));
              goto label_92;
            case 44:
              nullable = new bool?(Judgement.CheckEnabledEnemyGenericBuffDebuff(effect, unit));
              goto label_92;
            default:
              if ((uint) (optTest3 - 1001) <= 11U)
                goto label_92;
              else
                goto label_92;
          }
        }
        else
        {
          switch (optTest3 - 2000)
          {
            case 0:
              nullable = new bool?(BattleFuncs.checkEnabledCriticalGuard(effect, unit));
              goto label_92;
            case 1:
              BattleskillEffect battleskillEffect1 = effect;
              nullable = new bool?((!battleskillEffect1.HasKey(BattleskillEffectLogicArgumentEnum.gear_kind_id) || battleskillEffect1.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || battleskillEffect1.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && (!battleskillEffect1.HasKey(BattleskillEffectLogicArgumentEnum.element) || battleskillEffect1.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) battleskillEffect1.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && (!battleskillEffect1.HasKey(BattleskillEffectLogicArgumentEnum.job_id) || battleskillEffect1.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == 0 || battleskillEffect1.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == unit.originalUnit.job.ID) && (!battleskillEffect1.HasKey(BattleskillEffectLogicArgumentEnum.family_id) || battleskillEffect1.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || unit.originalUnit.playerUnit.HasFamily((UnitFamily) battleskillEffect1.GetInt(BattleskillEffectLogicArgumentEnum.family_id))));
              goto label_92;
            case 2:
              int num1 = effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id);
              nullable = new bool?(num1 == 0 || num1 == unit.originalUnit.unit.kind.ID);
              goto label_92;
            case 3:
            case 4:
              nullable = new bool?(BattleFuncs.checkEnabledWhiteNight(effect, unit));
              goto label_92;
            case 5:
              nullable = new bool?(BattleFuncs.checkEnabledWhiteNight3(effect, unit));
              goto label_92;
            case 6:
              int num2 = !effect.HasKey(BattleskillEffectLogicArgumentEnum.element) ? 0 : effect.GetInt(BattleskillEffectLogicArgumentEnum.element);
              nullable = new bool?(num2 == 0 || (CommonElement) num2 == unit.originalUnit.playerUnit.GetElement());
              goto label_92;
            case 7:
              BattleskillEffect battleskillEffect2 = effect;
              nullable = new bool?((!battleskillEffect2.HasKey(BattleskillEffectLogicArgumentEnum.gear_kind_id) || battleskillEffect2.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || battleskillEffect2.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && (!battleskillEffect2.HasKey(BattleskillEffectLogicArgumentEnum.element) || battleskillEffect2.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) battleskillEffect2.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && (!battleskillEffect2.HasKey(BattleskillEffectLogicArgumentEnum.job_id) || battleskillEffect2.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == 0 || battleskillEffect2.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == unit.originalUnit.job.ID));
              goto label_92;
            case 8:
              BattleskillEffect battleskillEffect3 = effect;
              nullable = new bool?((!battleskillEffect3.HasKey(BattleskillEffectLogicArgumentEnum.gear_kind_id) || battleskillEffect3.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || battleskillEffect3.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && (!battleskillEffect3.HasKey(BattleskillEffectLogicArgumentEnum.element) || battleskillEffect3.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) battleskillEffect3.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && (!battleskillEffect3.HasKey(BattleskillEffectLogicArgumentEnum.job_id) || battleskillEffect3.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == 0 || battleskillEffect3.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == unit.originalUnit.job.ID));
              goto label_92;
            case 9:
              nullable = new bool?(BattleFuncs.checkEnabledSkillsAndEffectsInvalid(effect, unit));
              goto label_92;
            case 10:
              nullable = new bool?(BattleFuncs.CheckEnabledLandBlessingBuffDebuff(effect, unit));
              goto label_92;
            case 11:
            case 12:
              nullable = new bool?(BattleFuncs.CheckEnabledDuelSupportBuffDebuff(effect, unit));
              goto label_92;
            case 13:
              BattleskillEffect battleskillEffect4 = effect;
              nullable = new bool?((battleskillEffect4.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || battleskillEffect4.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && (battleskillEffect4.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) battleskillEffect4.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && (battleskillEffect4.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == 0 || battleskillEffect4.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == unit.originalUnit.job.ID) && (battleskillEffect4.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || unit.originalUnit.playerUnit.HasFamily((UnitFamily) battleskillEffect4.GetInt(BattleskillEffectLogicArgumentEnum.family_id))));
              goto label_92;
            case 14:
            case 15:
              BattleskillEffect battleskillEffect5 = effect;
              BattleFuncs.PackedSkillEffect pse1 = BattleFuncs.PackedSkillEffect.Create(effect);
              pse1.SetIgnoreHeader(true);
              nullable = new bool?((battleskillEffect5.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || battleskillEffect5.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && (battleskillEffect5.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) battleskillEffect5.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && (!battleskillEffect5.HasKey(BattleskillEffectLogicArgumentEnum.job_id) || battleskillEffect5.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == 0 || battleskillEffect5.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == unit.originalUnit.job.ID) && (battleskillEffect5.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || unit.originalUnit.playerUnit.HasFamily((UnitFamily) battleskillEffect5.GetInt(BattleskillEffectLogicArgumentEnum.family_id))) && (!battleskillEffect5.HasKey(BattleskillEffectLogicArgumentEnum.skill_group_id) || battleskillEffect5.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id) == 0 || unit.originalUnit.unit.HasSkillGroupId(battleskillEffect5.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id))) && (!battleskillEffect5.HasKey(BattleskillEffectLogicArgumentEnum.exclude_skill_group_id) || battleskillEffect5.GetInt(BattleskillEffectLogicArgumentEnum.exclude_skill_group_id) == 0 || !unit.originalUnit.unit.HasSkillGroupId(battleskillEffect5.GetInt(BattleskillEffectLogicArgumentEnum.exclude_skill_group_id))) && BattleFuncs.checkInvokeSkillEffectSelf(pse1, unit, dontCheckParamDiff: true, checkPassiveEffectEnabled: true));
              goto label_92;
            case 16:
              BattleskillEffect battleskillEffect6 = effect;
              nullable = new bool?((battleskillEffect6.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || battleskillEffect6.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && (battleskillEffect6.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) battleskillEffect6.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && (battleskillEffect6.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == 0 || battleskillEffect6.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == unit.originalUnit.job.ID) && (battleskillEffect6.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || unit.originalUnit.playerUnit.HasFamily((UnitFamily) battleskillEffect6.GetInt(BattleskillEffectLogicArgumentEnum.family_id))) && (!battleskillEffect6.HasKey(BattleskillEffectLogicArgumentEnum.invoke_gamemode) || BattleFuncs.checkInvokeGamemode(battleskillEffect6.GetInt(BattleskillEffectLogicArgumentEnum.invoke_gamemode), false, unit)));
              goto label_92;
            case 17:
              BattleskillEffect battleskillEffect7 = effect;
              nullable = new bool?((battleskillEffect7.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || battleskillEffect7.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && (battleskillEffect7.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) battleskillEffect7.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && (battleskillEffect7.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == 0 || battleskillEffect7.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == unit.originalUnit.job.ID) && (battleskillEffect7.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || unit.originalUnit.playerUnit.HasFamily((UnitFamily) battleskillEffect7.GetInt(BattleskillEffectLogicArgumentEnum.family_id))));
              goto label_92;
            case 18:
              nullable = new bool?(effect.GetCheckInvokeGeneric().DoCheckFix(unit, false, true));
              goto label_92;
            case 19:
              BattleskillEffect battleskillEffect8 = effect;
              BattleFuncs.PackedSkillEffect pse2 = BattleFuncs.PackedSkillEffect.Create(effect);
              pse2.SetIgnoreHeader(true);
              nullable = new bool?((battleskillEffect8.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || battleskillEffect8.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && (battleskillEffect8.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) battleskillEffect8.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && BattleFuncs.checkInvokeSkillEffectSelf(pse2, unit, dontCheckParamDiff: true, checkPassiveEffectEnabled: true));
              goto label_92;
            case 20:
            case 21:
              nullable = new bool?(effect.GetCheckInvokeGeneric().DoCheckFix(unit, false, true));
              goto label_92;
            case 22:
            case 48:
              BattleskillEffect battleskillEffect9 = effect;
              BattleFuncs.PackedSkillEffect pse3 = BattleFuncs.PackedSkillEffect.Create(effect);
              pse3.SetIgnoreHeader(true);
              nullable = new bool?((battleskillEffect9.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) battleskillEffect9.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && (battleskillEffect9.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || battleskillEffect9.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && BattleFuncs.checkInvokeSkillEffectSelf(pse3, unit, dontCheckParamDiff: true, checkPassiveEffectEnabled: true));
              goto label_92;
            case 23:
            case 24:
              nullable = new bool?(BattleFuncs.CheckEnabledFieldDamageFluctuateBuffDebuff(effect, unit));
              goto label_92;
            case 25:
              BattleskillEffect battleskillEffect10 = effect;
              BattleFuncs.PackedSkillEffect pse4 = BattleFuncs.PackedSkillEffect.Create(effect);
              pse4.SetIgnoreHeader(true);
              nullable = new bool?((battleskillEffect10.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) battleskillEffect10.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && (battleskillEffect10.GetInt(BattleskillEffectLogicArgumentEnum.character_id) == 0 || battleskillEffect10.GetInt(BattleskillEffectLogicArgumentEnum.character_id) == unit.originalUnit.unit.character.ID) && (battleskillEffect10.GetInt(BattleskillEffectLogicArgumentEnum.unit_id) == 0 || battleskillEffect10.GetInt(BattleskillEffectLogicArgumentEnum.unit_id) == unit.originalUnit.unit.ID) && BattleFuncs.checkInvokeSkillEffectSelf(pse4, unit, dontCheckParamDiff: true, checkPassiveEffectEnabled: true));
              goto label_92;
            case 26:
            case 27:
            case 55:
            case 58:
              nullable = new bool?(BattleFuncs.CheckEnabledDamageCutBuffDebuff(effect, unit));
              goto label_92;
            case 28:
              nullable = new bool?(true);
              goto label_92;
            case 29:
              BattleskillEffect battleskillEffect11 = effect;
              nullable = new bool?((!battleskillEffect11.HasKey(BattleskillEffectLogicArgumentEnum.skill_group_id) || battleskillEffect11.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id) == 0 || unit.originalUnit.unit.HasSkillGroupId(battleskillEffect11.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id))) && (!battleskillEffect11.HasKey(BattleskillEffectLogicArgumentEnum.exclude_skill_group_id) || battleskillEffect11.GetInt(BattleskillEffectLogicArgumentEnum.exclude_skill_group_id) == 0 || !unit.originalUnit.unit.HasSkillGroupId(battleskillEffect11.GetInt(BattleskillEffectLogicArgumentEnum.exclude_skill_group_id))));
              goto label_92;
            case 30:
              nullable = new bool?(true);
              goto label_92;
            case 31:
              BattleskillEffect battleskillEffect12 = effect;
              nullable = new bool?((battleskillEffect12.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) battleskillEffect12.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && (battleskillEffect12.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || battleskillEffect12.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && (battleskillEffect12.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || unit.originalUnit.playerUnit.HasFamily((UnitFamily) battleskillEffect12.GetInt(BattleskillEffectLogicArgumentEnum.family_id))));
              goto label_92;
            case 32:
              BattleskillEffect battleskillEffect13 = effect;
              nullable = new bool?((battleskillEffect13.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || battleskillEffect13.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && (battleskillEffect13.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) battleskillEffect13.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && (battleskillEffect13.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || unit.originalUnit.playerUnit.HasFamily((UnitFamily) battleskillEffect13.GetInt(BattleskillEffectLogicArgumentEnum.family_id))) && (battleskillEffect13.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id) == 0 || unit.originalUnit.unit.HasSkillGroupId(battleskillEffect13.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id))) && (battleskillEffect13.GetInt(BattleskillEffectLogicArgumentEnum.exclude_skill_group_id) == 0 || !unit.originalUnit.unit.HasSkillGroupId(battleskillEffect13.GetInt(BattleskillEffectLogicArgumentEnum.exclude_skill_group_id))));
              goto label_92;
            case 33:
            case 34:
            case 47:
              nullable = new bool?(BattleFuncs.checkInvokeSkillEffectSelf(BattleFuncs.PackedSkillEffect.Create(effect), unit, dontCheckParamDiff: true, checkPassiveEffectEnabled: true));
              goto label_92;
            case 35:
              nullable = new bool?(true);
              goto label_92;
            case 36:
              nullable = new bool?(BattleFuncs.checkInvokeSkillEffectSelf(BattleFuncs.PackedSkillEffect.Create(effect), unit, dontCheckParamDiff: true, checkPassiveEffectEnabled: true));
              goto label_92;
            case 37:
              BattleFuncs.PackedSkillEffect pse5 = BattleFuncs.PackedSkillEffect.Create(effect);
              pse5.SetIgnoreHeader(true);
              nullable = new bool?(BattleFuncs.checkInvokeSkillEffectSelf(pse5, unit, dontCheckParamDiff: true, checkPassiveEffectEnabled: true));
              goto label_92;
            case 38:
              BattleskillEffect battleskillEffect14 = effect;
              nullable = new bool?((!battleskillEffect14.HasKey(BattleskillEffectLogicArgumentEnum.gear_kind_id) || battleskillEffect14.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || battleskillEffect14.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && (!battleskillEffect14.HasKey(BattleskillEffectLogicArgumentEnum.element) || battleskillEffect14.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) battleskillEffect14.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()));
              goto label_92;
            case 39:
              nullable = new bool?(BattleFuncs.checkInvokeSkillEffectSelf(BattleFuncs.PackedSkillEffect.Create(effect), unit, dontCheckParamDiff: true, checkPassiveEffectEnabled: true));
              goto label_92;
            case 40:
            case 41:
            case 42:
            case 43:
              BattleskillEffect battleskillEffect15 = effect;
              nullable = new bool?((battleskillEffect15.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || battleskillEffect15.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && (battleskillEffect15.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) battleskillEffect15.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && (battleskillEffect15.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || unit.originalUnit.playerUnit.HasFamily((UnitFamily) battleskillEffect15.GetInt(BattleskillEffectLogicArgumentEnum.family_id))));
              goto label_92;
            case 44:
              break;
            case 45:
            case 46:
              nullable = new bool?(effect.GetCheckInvokeGeneric().DoCheckFix(unit, false, true));
              goto label_92;
            case 49:
              nullable = new bool?(BattleFuncs.checkInvokeSkillEffectSelf(BattleFuncs.PackedSkillEffect.Create(effect), unit, dontCheckParamDiff: true, checkPassiveEffectEnabled: true));
              goto label_92;
            case 50:
              nullable = new bool?(BattleFuncs.checkInvokeSkillEffectSelf(BattleFuncs.PackedSkillEffect.Create(effect), unit, dontCheckParamDiff: true, checkPassiveEffectEnabled: true));
              goto label_92;
            case 51:
              nullable = new bool?(BattleFuncs.checkInvokeSkillEffectSelf(BattleFuncs.PackedSkillEffect.Create(effect), unit, dontCheckParamDiff: true, checkPassiveEffectEnabled: true));
              goto label_92;
            case 52:
              nullable = new bool?(BattleFuncs.checkInvokeSkillEffectSelf(BattleFuncs.PackedSkillEffect.Create(effect), unit, dontCheckParamDiff: true, checkPassiveEffectEnabled: true));
              goto label_92;
            case 53:
              nullable = new bool?(BattleFuncs.checkInvokeSkillEffectSelf(effect.GetPackedSkillEffect(), unit, dontCheckParamDiff: true, checkPassiveEffectEnabled: true));
              goto label_92;
            case 54:
              nullable = new bool?(BattleFuncs.checkInvokeSkillEffectSelf(effect.GetPackedSkillEffect(), unit, dontCheckParamDiff: true, checkPassiveEffectEnabled: true));
              goto label_92;
            case 56:
              nullable = new bool?((effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id) == 0 || unit.originalUnit.unit.HasSkillGroupId(effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id))));
              goto label_92;
            case 57:
              nullable = new bool?(BattleFuncs.checkInvokeSkillEffectSelf(effect.GetPackedSkillEffect(), unit, dontCheckParamDiff: true, checkPassiveEffectEnabled: true));
              goto label_92;
            case 59:
              nullable = new bool?(BattleFuncs.checkInvokeSkillEffectSelf(effect.GetPackedSkillEffect(), unit, dontCheckParamDiff: true, checkPassiveEffectEnabled: true));
              goto label_92;
            case 60:
              nullable = new bool?(BattleFuncs.checkInvokeSkillEffectSelf(effect.GetPackedSkillEffect(), unit, dontCheckParamDiff: true, checkPassiveEffectEnabled: true));
              goto label_92;
            case 61:
              nullable = new bool?(BattleFuncs.checkInvokeSkillEffectSelf(effect.GetPackedSkillEffect(), unit, dontCheckParamDiff: true, checkPassiveEffectEnabled: true));
              goto label_92;
            case 62:
              BattleFuncs.PackedSkillEffect packedSkillEffect = effect.GetPackedSkillEffect();
              nullable = !packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.family_id) || (packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) != 0 && !unit.originalUnit.playerUnit.HasFamily((UnitFamily) packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.family_id)) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID ? 0 : (packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 ? 1 : ((CommonElement) packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement() ? 1 : 0))) != 0 ? (!packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.group_large_id) || (packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_large_id) != 0 && (unit.originalUnit.unitGroup == null || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_large_id) != unit.originalUnit.unitGroup.group_large_category_id.ID) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_small_id) != 0 && (unit.originalUnit.unitGroup == null || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_small_id) != unit.originalUnit.unitGroup.group_small_category_id.ID) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_clothing_id) != 0 && (unit.originalUnit.unitGroup == null || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_clothing_id) != unit.originalUnit.unitGroup.group_clothing_category_id.ID && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_clothing_id) != unit.originalUnit.unitGroup.group_clothing_category_id_2.ID) ? 0 : (packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_generation_id) == 0 ? 1 : (unit.originalUnit.unitGroup == null ? 0 : (packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_generation_id) == unit.originalUnit.unitGroup.group_generation_category_id.ID ? 1 : 0)))) != 0 ? new bool?(true) : new bool?(false)) : new bool?(false);
              goto label_92;
            case 63:
              nullable = new bool?(effect.GetCheckInvokeGeneric().DoCheckFix(unit, false, true));
              goto label_92;
            default:
              if (optTest3 == 9999)
              {
                nullable = new bool?(true);
                goto label_92;
              }
              else
                goto label_92;
          }
        }
        nullable = new bool?(Judgement.CheckEnabledCharismaBuffDebuff(effect, unit));
      }
      catch (Exception ex)
      {
      }
label_92:
      if (!nullable.HasValue)
        return 2;
      return !nullable.Value ? 0 : 1;
    }

    public static bool checkPassiveEffectEnabledElement(
      BL.ISkillEffectListUnit unit,
      int element,
      bool isMatch = true)
    {
      if (element == 0)
        return true;
      return element < 100 ? ((CommonElement) element == unit.originalUnit.playerUnit.GetElement() ? isMatch : !isMatch) : !isMatch || (CommonElement) (element - 100) == unit.originalUnit.playerUnit.GetElement();
    }

    public static bool checkInvokeSkillEffectCommon(
      BattleFuncs.PackedSkillEffect pse,
      int? colosseumTurn = null)
    {
      int absoluteTurnCount;
      if (!colosseumTurn.HasValue)
      {
        BL.PhaseState phaseState = BattleFuncs.getPhaseState();
        if (phaseState == null)
          return false;
        absoluteTurnCount = phaseState.absoluteTurnCount;
      }
      else
        absoluteTurnCount = colosseumTurn.Value;
      int num1 = 0;
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.start_turn))
      {
        num1 = pse.GetInt(BattleskillEffectLogicArgumentEnum.start_turn);
        if (num1 != 0 && absoluteTurnCount < num1)
          return false;
      }
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.end_turn))
      {
        int num2 = pse.GetInt(BattleskillEffectLogicArgumentEnum.end_turn);
        if (num2 != 0 && absoluteTurnCount >= num2)
          return false;
      }
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.turn_cycle))
      {
        int num3 = pse.GetInt(BattleskillEffectLogicArgumentEnum.turn_cycle);
        if (num3 != 0 && (absoluteTurnCount - num1) % num3 != 0)
          return false;
      }
      int num4 = absoluteTurnCount - (pse.skillEffect != null ? pse.skillEffect.investTurn : 0);
      int num5 = 0;
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.start_turn2))
      {
        num5 = pse.GetInt(BattleskillEffectLogicArgumentEnum.start_turn2);
        if (num5 != 0 && num4 < num5)
          return false;
      }
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.turn_cycle2))
      {
        int num6 = pse.GetInt(BattleskillEffectLogicArgumentEnum.turn_cycle2);
        if (num6 != 0 && (num4 - num5) % num6 != 0)
          return false;
      }
      return true;
    }

    public static bool checkInvokeSkillEffectSelf(
      BattleFuncs.PackedSkillEffect pse,
      BL.ISkillEffectListUnit unit,
      Judgement.NonBattleParameter.FromPlayerUnitCache unitNbpCache = null,
      int? unitHp = null,
      bool dontCheckParamDiff = false,
      bool isColosseum = false,
      bool checkPassiveEffectEnabled = false)
    {
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.invoke_gamemode) && !BattleFuncs.checkInvokeGamemode(pse.GetInt(BattleskillEffectLogicArgumentEnum.invoke_gamemode), isColosseum, unit))
        return false;
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.equip_gear_king_id))
      {
        int kindId = pse.GetInt(BattleskillEffectLogicArgumentEnum.equip_gear_king_id);
        if (kindId != 0 && (unit == null || !BattleFuncs.isGearEquipped(unit.originalUnit.playerUnit, kindId)))
          return false;
      }
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.gear_kind_id))
      {
        int num = pse.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id);
        if (num != 0 && (unit == null || num != unit.originalUnit.unit.kind.ID))
          return false;
      }
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.exclude_gear_kind_id))
      {
        int num = pse.GetInt(BattleskillEffectLogicArgumentEnum.exclude_gear_kind_id);
        if (num != 0 && (unit == null || num == unit.originalUnit.unit.kind.ID))
          return false;
      }
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.element))
      {
        if (!checkPassiveEffectEnabled)
        {
          if (!BattleFuncs.checkElement(unit, pse.GetInt(BattleskillEffectLogicArgumentEnum.element)))
            return false;
        }
        else if (!BattleFuncs.checkPassiveEffectEnabledElement(unit, pse.GetInt(BattleskillEffectLogicArgumentEnum.element)))
          return false;
      }
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.exclude_element))
      {
        if (!checkPassiveEffectEnabled)
        {
          if (!BattleFuncs.checkElement(unit, pse.GetInt(BattleskillEffectLogicArgumentEnum.exclude_element), false))
            return false;
        }
        else if (!BattleFuncs.checkPassiveEffectEnabledElement(unit, pse.GetInt(BattleskillEffectLogicArgumentEnum.exclude_element), false))
          return false;
      }
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.job_id))
      {
        int num = pse.GetInt(BattleskillEffectLogicArgumentEnum.job_id);
        if (num != 0 && (unit == null || num != unit.originalUnit.job.ID))
          return false;
      }
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.exclude_job_id))
      {
        int num = pse.GetInt(BattleskillEffectLogicArgumentEnum.exclude_job_id);
        if (num != 0 && (unit == null || num == unit.originalUnit.job.ID))
          return false;
      }
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.family_id))
      {
        int family = pse.GetInt(BattleskillEffectLogicArgumentEnum.family_id);
        if (family != 0 && (unit == null || !unit.originalUnit.playerUnit.HasFamily((UnitFamily) family)))
          return false;
      }
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.exclude_family_id))
      {
        int family = pse.GetInt(BattleskillEffectLogicArgumentEnum.exclude_family_id);
        if (family != 0 && (unit == null || unit.originalUnit.playerUnit.HasFamily((UnitFamily) family)))
          return false;
      }
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.group_large_id))
      {
        int num = pse.GetInt(BattleskillEffectLogicArgumentEnum.group_large_id);
        if (num != 0 && (unit == null || unit.originalUnit.unitGroup == null || num != unit.originalUnit.unitGroup.group_large_category_id.ID))
          return false;
      }
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.group_small_id))
      {
        int num = pse.GetInt(BattleskillEffectLogicArgumentEnum.group_small_id);
        if (num != 0 && (unit == null || unit.originalUnit.unitGroup == null || num != unit.originalUnit.unitGroup.group_small_category_id.ID))
          return false;
      }
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.group_clothing_id))
      {
        int num = pse.GetInt(BattleskillEffectLogicArgumentEnum.group_clothing_id);
        if (num != 0 && (unit == null || unit.originalUnit.unitGroup == null || num != unit.originalUnit.unitGroup.group_clothing_category_id.ID && num != unit.originalUnit.unitGroup.group_clothing_category_id_2.ID))
          return false;
      }
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.group_generation_id))
      {
        int num = pse.GetInt(BattleskillEffectLogicArgumentEnum.group_generation_id);
        if (num != 0 && (unit == null || unit.originalUnit.unitGroup == null || num != unit.originalUnit.unitGroup.group_generation_category_id.ID))
          return false;
      }
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.skill_group_id))
      {
        int skillGroupId = pse.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id);
        if (skillGroupId != 0 && (unit == null || !unit.originalUnit.unit.HasSkillGroupId(skillGroupId)))
          return false;
      }
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.exclude_skill_group_id))
      {
        int skillGroupId = pse.GetInt(BattleskillEffectLogicArgumentEnum.exclude_skill_group_id);
        if (skillGroupId != 0 && (unit == null || unit.originalUnit.unit.HasSkillGroupId(skillGroupId)))
          return false;
      }
      if (!dontCheckParamDiff && pse.HasKey(BattleskillEffectLogicArgumentEnum.param_type))
      {
        int paramType = pse.GetInt(BattleskillEffectLogicArgumentEnum.param_type);
        if (paramType != 0 && pse.GetInt(BattleskillEffectLogicArgumentEnum.target_param_type) == 0)
        {
          if (unit == null)
            return false;
          if (unitNbpCache == null)
            unitNbpCache = new Judgement.NonBattleParameter.FromPlayerUnitCache(unit.originalUnit.playerUnit);
          int num = -BattleFuncs.GetParamDiffValue(paramType, unitNbpCache, unitHp.HasValue ? unitHp.Value : unit.hp);
          if (num < pse.GetInt(BattleskillEffectLogicArgumentEnum.min_value) || num > pse.GetInt(BattleskillEffectLogicArgumentEnum.max_value))
            return false;
        }
      }
      if (!dontCheckParamDiff)
      {
        bool flag1 = pse.HasKey(BattleskillEffectLogicArgumentEnum.oneman_charge_player_min_range);
        bool flag2 = pse.HasKey(BattleskillEffectLogicArgumentEnum.oneman_charge_enemy_min_range);
        bool flag3 = pse.HasKey(BattleskillEffectLogicArgumentEnum.oneman_charge_complex_min_range);
        if (flag1 | flag2 | flag3)
        {
          if (unit == null || isColosseum)
            return false;
          BL.UnitPosition up = BattleFuncs.iSkillEffectListUnitToUnitPosition(unit);
          BattleFuncs.OnemanChargeSearchTargetCheck ocstc = (BattleFuncs.OnemanChargeSearchTargetCheck) null;
          Func<int[], int> func1 = (Func<int[], int>) (range => BattleFuncs.getTargets(up.row, up.column, range, BattleFuncs.getForceIDArray(BattleFuncs.getForceID(unit.originalUnit)), BL.Unit.TargetAttribute.all, unit is BL.AIUnit, nonFacility: true).Count<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x =>
          {
            BL.ISkillEffectListUnit iskillEffectListUnit = BattleFuncs.unitPositionToISkillEffectListUnit(x);
            return x != up && iskillEffectListUnit.hp > 0 && ocstc.DoCheck(iskillEffectListUnit);
          })));
          Func<int[], int> func2 = (Func<int[], int>) (range => BattleFuncs.getTargets(up.row, up.column, range, BattleFuncs.getTargetForce(unit.originalUnit, unit.IsCharm), BL.Unit.TargetAttribute.all, unit is BL.AIUnit, nonFacility: true).Count<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x =>
          {
            BL.ISkillEffectListUnit iskillEffectListUnit = BattleFuncs.unitPositionToISkillEffectListUnit(x);
            return iskillEffectListUnit.hp > 0 && ocstc.DoCheck(iskillEffectListUnit);
          })));
          if (flag1)
          {
            ocstc = (BattleFuncs.OnemanChargeSearchTargetCheck) new BattleFuncs.OnemanChargeSearchTargetCheckPlayer(pse);
            int num = func1(new int[2]
            {
              pse.GetInt(BattleskillEffectLogicArgumentEnum.oneman_charge_player_min_range),
              pse.GetInt(BattleskillEffectLogicArgumentEnum.oneman_charge_player_max_range)
            });
            if (num < pse.GetInt(BattleskillEffectLogicArgumentEnum.oneman_charge_player_min_unit_count) || num > pse.GetInt(BattleskillEffectLogicArgumentEnum.oneman_charge_player_max_unit_count))
              return false;
          }
          if (flag2)
          {
            ocstc = (BattleFuncs.OnemanChargeSearchTargetCheck) new BattleFuncs.OnemanChargeSearchTargetCheckEnemy(pse);
            int num = func2(new int[2]
            {
              pse.GetInt(BattleskillEffectLogicArgumentEnum.oneman_charge_enemy_min_range),
              pse.GetInt(BattleskillEffectLogicArgumentEnum.oneman_charge_enemy_max_range)
            });
            if (num < pse.GetInt(BattleskillEffectLogicArgumentEnum.oneman_charge_enemy_min_unit_count) || num > pse.GetInt(BattleskillEffectLogicArgumentEnum.oneman_charge_enemy_max_unit_count))
              return false;
          }
          if (flag3)
          {
            ocstc = (BattleFuncs.OnemanChargeSearchTargetCheck) new BattleFuncs.OnemanChargeSearchTargetCheckComplex(pse);
            int[] numArray = new int[2]
            {
              pse.GetInt(BattleskillEffectLogicArgumentEnum.oneman_charge_complex_min_range),
              pse.GetInt(BattleskillEffectLogicArgumentEnum.oneman_charge_complex_max_range)
            };
            int num = func1(numArray) + func2(numArray);
            if (num < pse.GetInt(BattleskillEffectLogicArgumentEnum.oneman_charge_complex_min_unit_count) || num > pse.GetInt(BattleskillEffectLogicArgumentEnum.oneman_charge_complex_max_unit_count))
              return false;
          }
        }
      }
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.peculiar_parameter_min))
      {
        int paramType = pse.GetInt(BattleskillEffectLogicArgumentEnum.peculiar_parameter_type);
        if (paramType != 0)
        {
          if (unit == null)
            return false;
          float peculiarParameterValue = BattleFuncs.GetPeculiarParameterValue(unit, paramType);
          if ((double) peculiarParameterValue < (double) pse.GetFloat(BattleskillEffectLogicArgumentEnum.peculiar_parameter_min) || (double) peculiarParameterValue > (double) pse.GetFloat(BattleskillEffectLogicArgumentEnum.peculiar_parameter_max))
            return false;
        }
      }
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.level_up_status_min))
      {
        int statusType = pse.GetInt(BattleskillEffectLogicArgumentEnum.level_up_status_type);
        if (statusType != 0)
        {
          if (unit == null)
            return false;
          Decimal levelUpStatusRatio = BattleFuncs.GetLevelUpStatusRatio(unit, statusType);
          Decimal num1 = (Decimal) pse.GetFloat(BattleskillEffectLogicArgumentEnum.level_up_status_min);
          Decimal num2 = (Decimal) pse.GetFloat(BattleskillEffectLogicArgumentEnum.level_up_status_max);
          if (num1 != 0M && levelUpStatusRatio < num1 || num2 != 0M && levelUpStatusRatio >= num2)
            return false;
        }
      }
      return true;
    }

    public static bool checkInvokeSkillEffectTarget(
      BattleFuncs.PackedSkillEffect pse,
      BL.ISkillEffectListUnit target,
      Judgement.NonBattleParameter.FromPlayerUnitCache targetNbpCache = null,
      int? targetHp = null)
    {
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.target_equip_gear_kind_id))
      {
        int kindId = pse.GetInt(BattleskillEffectLogicArgumentEnum.target_equip_gear_kind_id);
        if (kindId != 0 && (target == null || !BattleFuncs.isGearEquipped(target.originalUnit.playerUnit, kindId)))
          return false;
      }
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.target_gear_kind_id))
      {
        int num = pse.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id);
        if (num != 0 && (target == null || num != target.originalUnit.unit.kind.ID))
          return false;
      }
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.target_exclude_gear_kind_id))
      {
        int num = pse.GetInt(BattleskillEffectLogicArgumentEnum.target_exclude_gear_kind_id);
        if (num != 0 && (target == null || num == target.originalUnit.unit.kind.ID))
          return false;
      }
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.target_element) && !BattleFuncs.checkElement(target, pse.GetInt(BattleskillEffectLogicArgumentEnum.target_element)) || pse.HasKey(BattleskillEffectLogicArgumentEnum.target_exclude_element) && !BattleFuncs.checkElement(target, pse.GetInt(BattleskillEffectLogicArgumentEnum.target_exclude_element), false))
        return false;
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.target_job_id))
      {
        int num = pse.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id);
        if (num != 0 && (target == null || num != target.originalUnit.job.ID))
          return false;
      }
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.target_exclude_job_id))
      {
        int num = pse.GetInt(BattleskillEffectLogicArgumentEnum.target_exclude_job_id);
        if (num != 0 && (target == null || num == target.originalUnit.job.ID))
          return false;
      }
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.target_family_id))
      {
        int family = pse.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id);
        if (family != 0 && (target == null || !target.originalUnit.playerUnit.HasFamily((UnitFamily) family)))
          return false;
      }
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.target_exclude_family_id))
      {
        int family = pse.GetInt(BattleskillEffectLogicArgumentEnum.target_exclude_family_id);
        if (family != 0 && (target == null || target.originalUnit.playerUnit.HasFamily((UnitFamily) family)))
          return false;
      }
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.target_group_large_id))
      {
        int num = pse.GetInt(BattleskillEffectLogicArgumentEnum.target_group_large_id);
        if (num != 0 && (target == null || target.originalUnit.unitGroup == null || num != target.originalUnit.unitGroup.group_large_category_id.ID))
          return false;
      }
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.target_group_small_id))
      {
        int num = pse.GetInt(BattleskillEffectLogicArgumentEnum.target_group_small_id);
        if (num != 0 && (target == null || target.originalUnit.unitGroup == null || num != target.originalUnit.unitGroup.group_small_category_id.ID))
          return false;
      }
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.target_group_clothing_id))
      {
        int num = pse.GetInt(BattleskillEffectLogicArgumentEnum.target_group_clothing_id);
        if (num != 0 && (target == null || target.originalUnit.unitGroup == null || num != target.originalUnit.unitGroup.group_clothing_category_id.ID && num != target.originalUnit.unitGroup.group_clothing_category_id_2.ID))
          return false;
      }
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.target_group_generation_id))
      {
        int num = pse.GetInt(BattleskillEffectLogicArgumentEnum.target_group_generation_id);
        if (num != 0 && (target == null || target.originalUnit.unitGroup == null || num != target.originalUnit.unitGroup.group_generation_category_id.ID))
          return false;
      }
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.target_skill_group_id))
      {
        int skillGroupId = pse.GetInt(BattleskillEffectLogicArgumentEnum.target_skill_group_id);
        if (skillGroupId != 0 && (target == null || !target.originalUnit.unit.HasSkillGroupId(skillGroupId)))
          return false;
      }
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.target_exclude_skill_group_id))
      {
        int skillGroupId = pse.GetInt(BattleskillEffectLogicArgumentEnum.target_exclude_skill_group_id);
        if (skillGroupId != 0 && (target == null || target.originalUnit.unit.HasSkillGroupId(skillGroupId)))
          return false;
      }
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.target_param_type))
      {
        int paramType = pse.GetInt(BattleskillEffectLogicArgumentEnum.target_param_type);
        if (paramType != 0 && pse.GetInt(BattleskillEffectLogicArgumentEnum.param_type) == 0)
        {
          if (target == null)
            return false;
          if (targetNbpCache == null)
            targetNbpCache = new Judgement.NonBattleParameter.FromPlayerUnitCache(target.originalUnit.playerUnit);
          int paramDiffValue = BattleFuncs.GetParamDiffValue(paramType, targetNbpCache, targetHp.HasValue ? targetHp.Value : target.hp);
          if (paramDiffValue < pse.GetInt(BattleskillEffectLogicArgumentEnum.min_value) || paramDiffValue > pse.GetInt(BattleskillEffectLogicArgumentEnum.max_value))
            return false;
        }
      }
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.peculiar_parameter_min))
      {
        int paramType = pse.GetInt(BattleskillEffectLogicArgumentEnum.target_peculiar_parameter_type);
        if (paramType != 0)
        {
          if (target == null)
            return false;
          float peculiarParameterValue = BattleFuncs.GetPeculiarParameterValue(target, paramType);
          if ((double) peculiarParameterValue < (double) pse.GetFloat(BattleskillEffectLogicArgumentEnum.target_peculiar_parameter_min) || (double) peculiarParameterValue > (double) pse.GetFloat(BattleskillEffectLogicArgumentEnum.target_peculiar_parameter_max))
            return false;
        }
      }
      return true;
    }

    public static bool checkInvokeSkillEffectBoth(
      BattleFuncs.PackedSkillEffect pse,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      Judgement.NonBattleParameter.FromPlayerUnitCache unitNbpCache = null,
      Judgement.NonBattleParameter.FromPlayerUnitCache targetNbpCache = null,
      int? unitHp = null,
      int? targetHp = null)
    {
      if (pse.HasKey(BattleskillEffectLogicArgumentEnum.param_type))
      {
        int paramType1 = pse.GetInt(BattleskillEffectLogicArgumentEnum.param_type);
        int paramType2 = pse.GetInt(BattleskillEffectLogicArgumentEnum.target_param_type);
        if (paramType1 != 0 && paramType2 != 0)
        {
          if (unit == null || target == null)
            return false;
          if (unitNbpCache == null)
            unitNbpCache = new Judgement.NonBattleParameter.FromPlayerUnitCache(unit.originalUnit.playerUnit);
          if (targetNbpCache == null)
            targetNbpCache = new Judgement.NonBattleParameter.FromPlayerUnitCache(target.originalUnit.playerUnit);
          int paramDiffValue = BattleFuncs.GetParamDiffValue(paramType1, unitNbpCache, unitHp.HasValue ? unitHp.Value : unit.hp);
          int num = BattleFuncs.GetParamDiffValue(paramType2, targetNbpCache, targetHp.HasValue ? targetHp.Value : target.hp) - paramDiffValue;
          if (num < pse.GetInt(BattleskillEffectLogicArgumentEnum.min_value) || num > pse.GetInt(BattleskillEffectLogicArgumentEnum.max_value))
            return false;
        }
      }
      return true;
    }

    public static bool checkInvokeSkillEffect(
      BattleFuncs.PackedSkillEffect pse,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target = null,
      int? colosseumTurn = null,
      Judgement.NonBattleParameter.FromPlayerUnitCache unitNbpCache = null,
      Judgement.NonBattleParameter.FromPlayerUnitCache targetNbpCache = null,
      int? unitHp = null,
      int? targetHp = null)
    {
      return BattleFuncs.checkInvokeSkillEffectCommon(pse, colosseumTurn) && BattleFuncs.checkInvokeSkillEffectSelf(pse, unit, unitNbpCache, unitHp, isColosseum: colosseumTurn.HasValue) && BattleFuncs.checkInvokeSkillEffectTarget(pse, target, targetNbpCache, targetHp) && BattleFuncs.checkInvokeSkillEffectBoth(pse, unit, target, unitNbpCache, targetNbpCache, unitHp, targetHp);
    }

    private static int getShieldHp(BL.SkillEffect x, BL.ISkillEffectListUnit unit)
    {
      BattleskillEffect effect = x.effect;
      return effect.GetInt(BattleskillEffectLogicArgumentEnum.value) + x.baseSkillLevel * effect.GetInt(BattleskillEffectLogicArgumentEnum.value_skill_ratio) + Mathf.CeilToInt((float) ((Decimal) unit.originalUnit.parameter.Hp * (Decimal) (effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) + (float) x.baseSkillLevel * effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_skill_ratio))));
    }

    private static int getShieldRemain(BL.SkillEffect x, BL.ISkillEffectListUnit unit)
    {
      int shieldRemain = BattleFuncs.getShieldHp(x, unit);
      if (x.work != null)
        shieldRemain -= (int) x.work[0];
      if (shieldRemain < 0)
        shieldRemain = 0;
      return shieldRemain;
    }

    public static int GetParamDiffValue(
      int paramType,
      Judgement.NonBattleParameter.FromPlayerUnitCache nbpCache,
      int hp)
    {
      int paramDiffValue = 0;
      switch (paramType)
      {
        case 1:
          paramDiffValue = hp;
          break;
        case 2:
          paramDiffValue = nbpCache.parameter.Hp;
          break;
        case 3:
          paramDiffValue = nbpCache.parameter.Strength;
          break;
        case 4:
          paramDiffValue = nbpCache.parameter.Intelligence;
          break;
        case 5:
          paramDiffValue = nbpCache.parameter.Vitality;
          break;
        case 6:
          paramDiffValue = nbpCache.parameter.Mind;
          break;
        case 7:
          paramDiffValue = nbpCache.parameter.Agility;
          break;
        case 8:
          paramDiffValue = nbpCache.parameter.Dexterity;
          break;
        case 9:
          paramDiffValue = nbpCache.parameter.Luck;
          break;
        case 10:
          paramDiffValue = nbpCache.parameter.Hit;
          break;
        case 11:
          paramDiffValue = nbpCache.parameter.Evasion;
          break;
        case 12:
          paramDiffValue = nbpCache.parameter.Critical;
          break;
        case 13:
          paramDiffValue = nbpCache.parameter.Move;
          break;
        case 14:
          paramDiffValue = nbpCache.parameter.PhysicalAttack;
          break;
        case 15:
          paramDiffValue = nbpCache.parameter.MagicAttack;
          break;
        case 16:
          paramDiffValue = nbpCache.parameter.PhysicalDefense;
          break;
        case 17:
          paramDiffValue = nbpCache.parameter.MagicDefense;
          break;
        case 18:
          paramDiffValue = nbpCache.parameter.Combat;
          break;
      }
      return paramDiffValue;
    }

    public static float GetPeculiarParameterValue(BL.ISkillEffectListUnit unit, int paramType)
    {
      float peculiarParameterValue = 0.0f;
      switch (paramType)
      {
        case 1:
          peculiarParameterValue = unit.originalUnit.playerUnit.unityTotal;
          break;
        case 2:
          peculiarParameterValue = (float) unit.originalUnit.playerUnit.cost;
          break;
      }
      return peculiarParameterValue;
    }

    public static Decimal GetLevelUpStatusRatio(BL.ISkillEffectListUnit unit, int statusType)
    {
      int level;
      int buildup;
      int levelUpMaxStatus;
      switch (statusType)
      {
        case 2:
          level = unit.originalUnit.playerUnit.hp.level;
          buildup = unit.originalUnit.playerUnit.hp.buildup;
          levelUpMaxStatus = unit.originalUnit.playerUnit.hp.level_up_max_status;
          break;
        case 3:
          level = unit.originalUnit.playerUnit.strength.level;
          buildup = unit.originalUnit.playerUnit.strength.buildup;
          levelUpMaxStatus = unit.originalUnit.playerUnit.strength.level_up_max_status;
          break;
        case 4:
          level = unit.originalUnit.playerUnit.intelligence.level;
          buildup = unit.originalUnit.playerUnit.intelligence.buildup;
          levelUpMaxStatus = unit.originalUnit.playerUnit.intelligence.level_up_max_status;
          break;
        case 5:
          level = unit.originalUnit.playerUnit.vitality.level;
          buildup = unit.originalUnit.playerUnit.vitality.buildup;
          levelUpMaxStatus = unit.originalUnit.playerUnit.vitality.level_up_max_status;
          break;
        case 6:
          level = unit.originalUnit.playerUnit.mind.level;
          buildup = unit.originalUnit.playerUnit.mind.buildup;
          levelUpMaxStatus = unit.originalUnit.playerUnit.mind.level_up_max_status;
          break;
        case 7:
          level = unit.originalUnit.playerUnit.agility.level;
          buildup = unit.originalUnit.playerUnit.agility.buildup;
          levelUpMaxStatus = unit.originalUnit.playerUnit.agility.level_up_max_status;
          break;
        case 8:
          level = unit.originalUnit.playerUnit.dexterity.level;
          buildup = unit.originalUnit.playerUnit.dexterity.buildup;
          levelUpMaxStatus = unit.originalUnit.playerUnit.dexterity.level_up_max_status;
          break;
        case 9:
          level = unit.originalUnit.playerUnit.lucky.level;
          buildup = unit.originalUnit.playerUnit.lucky.buildup;
          levelUpMaxStatus = unit.originalUnit.playerUnit.lucky.level_up_max_status;
          break;
        default:
          return 0M;
      }
      int num = level + buildup;
      return num >= levelUpMaxStatus ? 1M : (Decimal) num / (Decimal) levelUpMaxStatus;
    }

    public static int GetBattleParameterValue(BL.ISkillEffectListUnit unit, int paramType)
    {
      int battleParameterValue = 0;
      switch (paramType)
      {
        case 1:
          battleParameterValue = unit.hp;
          break;
        case 2:
          battleParameterValue = unit.parameter.Hp;
          break;
        case 3:
          battleParameterValue = unit.parameter.Strength;
          break;
        case 4:
          battleParameterValue = unit.parameter.Intelligence;
          break;
        case 5:
          battleParameterValue = unit.parameter.Vitality;
          break;
        case 6:
          battleParameterValue = unit.parameter.Mind;
          break;
        case 7:
          battleParameterValue = unit.parameter.Agility;
          break;
        case 8:
          battleParameterValue = unit.parameter.Dexterity;
          break;
        case 9:
          battleParameterValue = unit.parameter.Luck;
          break;
        case 10:
          battleParameterValue = unit.parameter.Hit;
          break;
        case 11:
          battleParameterValue = unit.parameter.Evasion;
          break;
        case 12:
          battleParameterValue = unit.parameter.Critical;
          break;
        case 13:
          battleParameterValue = unit.parameter.Move;
          break;
        case 14:
          battleParameterValue = unit.parameter.PhysicalAttack;
          break;
        case 15:
          battleParameterValue = unit.parameter.MagicAttack;
          break;
        case 16:
          battleParameterValue = unit.parameter.PhysicalDefense;
          break;
        case 17:
          battleParameterValue = unit.parameter.MagicDefense;
          break;
        case 18:
          battleParameterValue = unit.parameter.Combat;
          break;
      }
      return battleParameterValue;
    }

    public static int getParameterReferenceHealValue(
      BL.ISkillEffectListUnit selfUnit,
      BL.ISkillEffectListUnit targetUnit,
      BattleskillEffect effect)
    {
      Decimal battleParameterValue1 = (Decimal) BattleFuncs.GetBattleParameterValue(selfUnit, effect.GetInt(BattleskillEffectLogicArgumentEnum.param_reference_type));
      Decimal battleParameterValue2 = targetUnit != null ? (Decimal) BattleFuncs.GetBattleParameterValue(targetUnit, effect.GetInt(BattleskillEffectLogicArgumentEnum.target_param_reference_type)) : 0M;
      Decimal num1 = (Decimal) effect.GetFloat(BattleskillEffectLogicArgumentEnum.param_reference_percentage);
      Decimal num2 = (Decimal) effect.GetFloat(BattleskillEffectLogicArgumentEnum.target_param_reference_percentage);
      Decimal num3 = num1;
      int referenceHealValue = (int) (battleParameterValue1 * num3 + battleParameterValue2 * num2);
      if (referenceHealValue < 1)
        referenceHealValue = 1;
      int num4 = effect.GetInt(BattleskillEffectLogicArgumentEnum.limit);
      if (num4 != 0 && referenceHealValue > num4)
        referenceHealValue = num4;
      return referenceHealValue;
    }

    public static int getRunAwayValue(BL.ISkillEffectListUnit unit)
    {
      return unit.enabledSkillEffect(BattleskillEffectLogicEnum.run_away).Sum<BL.SkillEffect>((Func<BL.SkillEffect, int>) (x => x.effect.GetInt(BattleskillEffectLogicArgumentEnum.add_value)));
    }

    public static bool cantRemoveSkillEffect(
      BL.SkillEffect e,
      int investType,
      int ailmentGroupId,
      int rangeEffectRemoveFlag,
      BL.ISkillEffectListUnit rangeFromUnit)
    {
      BattleskillEffect effect = e.effect;
      if (e.baseSkill.skill_type == BattleskillSkillType.ailment)
      {
        BattleFuncs.PackedSkillEffect packedSkillEffect = BattleFuncs.PackedSkillEffect.Create(e);
        if ((packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.ailment_group_id) ? packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.ailment_group_id) : 0) != ailmentGroupId)
          return true;
      }
      return e.baseSkill.skill_type == BattleskillSkillType.leader || e.baseSkill.skill_type == BattleskillSkillType.passive && e.baseSkill.range_effect_passive_skill && ((rangeEffectRemoveFlag & 1) == 0 || e.unit != rangeFromUnit.originalUnit) || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.fix_hp || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.ratio_hp || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.fix_hp2 || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.ratio_hp2 || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.fix_hp4 || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.ratio_hp4 || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.specific_unit_fix_hp || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.specific_unit_ratio_hp || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.specific_group_fix_hp || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.specific_group_ratio_hp || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.specific_group2_fix_hp || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.specific_group2_ratio_hp || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.specific_skill_group_fix_hp || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.specific_skill_group_ratio_hp || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.attack_class_fix_hp || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.attack_class_ratio_hp || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.attack_element_fix_hp || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.attack_element_ratio_hp || effect.EffectLogic.opt_test1 != 0 && effect.EffectLogic.opt_test4 == 0 || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.suppress_duel_skill || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.steal_effect || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.transformation || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.enemy_multi_damage_value_fluctuate || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.aiming || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.enhanced_element || effect.EffectLogic.Enum == BattleskillEffectLogicEnum.ovo_point_interference && effect.GetInt(BattleskillEffectLogicArgumentEnum.cant_remove_skilleffect) == 1 || investType != 0 && (investType == 1 && e.isBaseSkill || investType == 2 && !e.isBaseSkill) || e.isDontDisplay;
    }

    private static IEnumerable<BL.SkillEffect> getInvalidRemoveEffects(
      BL.ISkillEffectListUnit target,
      BL.ISkillEffectListUnit actionUnit,
      BattleskillEffect removeEffect,
      bool isAI)
    {
      BL.UnitPosition unitPosition1 = BattleFuncs.iSkillEffectListUnitToUnitPosition(target);
      BL.UnitPosition unitPosition2 = BattleFuncs.iSkillEffectListUnitToUnitPosition(actionUnit);
      List<BattleFuncs.InvalidSpecificSkillLogic> invalidSkillsAndLogics = BattleFuncs.getInvalidSkillsAndLogics(target, actionUnit, BattleFuncs.getPanel(unitPosition1.row, unitPosition1.column), BattleFuncs.getPanel(unitPosition2.row, unitPosition2.column), 0, true, isAI, false, false);
      return BattleFuncs.getEnabledInvalidSkillLogics(removeEffect, invalidSkillsAndLogics).Select<BattleFuncs.InvalidSpecificSkillLogic, BL.SkillEffect>((Func<BattleFuncs.InvalidSpecificSkillLogic, BL.SkillEffect>) (x => (BL.SkillEffect) x.param)).Distinct<BL.SkillEffect>();
    }

    private static List<Tuple<int, int, int, int, int>> getInvalidRemoveEffectTargetPatterns(
      BL.SkillEffect effect)
    {
      List<Tuple<int, int, int, int, int>> source = new List<Tuple<int, int, int, int, int>>();
      BattleFuncs.PackedSkillEffect packedSkillEffect = BattleFuncs.PackedSkillEffect.Create(effect);
      packedSkillEffect.SetIgnoreHeader(true);
      foreach (BattleskillEffect effect1 in packedSkillEffect.GetEffects())
      {
        if (effect1.HasKey(BattleskillEffectLogicArgumentEnum.logic_id))
          source.Add(Tuple.Create<int, int, int, int, int>(effect1.GetInt(BattleskillEffectLogicArgumentEnum.logic_id), effect1.GetInt(BattleskillEffectLogicArgumentEnum.skill_id), effect1.GetInt(BattleskillEffectLogicArgumentEnum.skill_type), effect1.GetInt(BattleskillEffectLogicArgumentEnum.invest_type), effect1.GetInt(BattleskillEffectLogicArgumentEnum.ailment_group_id)));
      }
      if (!source.Any<Tuple<int, int, int, int, int>>())
        source.Add(Tuple.Create<int, int, int, int, int>(0, 0, 0, 0, 0));
      return source;
    }

    public static BL.SkillEffect[] removeSkillEffect(
      int removeLogicId,
      int removeSkillId,
      int removeSkillType,
      int removeInvestType,
      int removeAilmentGroupId,
      int rangeEffectRemoveFlag,
      BL.ISkillEffectListUnit target,
      BattleFuncs.ApplyChangeSkillEffects applyChangeSkillEffects,
      BL.ISkillEffectListUnit actionUnit,
      BattleskillEffect removeEffect,
      BL.ISkillEffectListUnit moveUnit = null)
    {
      bool isAI = target is BL.AIUnit;
      List<Tuple<int, int, int, int, int>> invalidRemoveSkillEffectPatterns = new List<Tuple<int, int, int, int, int>>();
      if (actionUnit != null && removeEffect != null)
      {
        foreach (IGrouping<IEnumerable<Tuple<int, int, int, int, int>>, BL.SkillEffect> source in BattleFuncs.getInvalidRemoveEffects(target, actionUnit, removeEffect, isAI).GroupBy<BL.SkillEffect, IEnumerable<Tuple<int, int, int, int, int>>>((Func<BL.SkillEffect, IEnumerable<Tuple<int, int, int, int, int>>>) (ef => (IEnumerable<Tuple<int, int, int, int, int>>) BattleFuncs.getInvalidRemoveEffectTargetPatterns(ef).OrderBy<Tuple<int, int, int, int, int>, int>((Func<Tuple<int, int, int, int, int>, int>) (x => x.Item1)).ThenBy<Tuple<int, int, int, int, int>, int>((Func<Tuple<int, int, int, int, int>, int>) (x => x.Item2)).ThenBy<Tuple<int, int, int, int, int>, int>((Func<Tuple<int, int, int, int, int>, int>) (x => x.Item3)).ThenBy<Tuple<int, int, int, int, int>, int>((Func<Tuple<int, int, int, int, int>, int>) (x => x.Item4)).ThenBy<Tuple<int, int, int, int, int>, int>((Func<Tuple<int, int, int, int, int>, int>) (x => x.Item5))), (IEqualityComparer<IEnumerable<Tuple<int, int, int, int, int>>>) new BattleFuncs.EnumerableSequenceEqualityComparer<Tuple<int, int, int, int, int>>()))
        {
          bool flag = false;
          foreach (Tuple<int, int, int, int, int> tuple in source.Key)
          {
            int num1 = tuple.Item1;
            int num2 = tuple.Item2;
            int num3 = tuple.Item3;
            int num4 = tuple.Item4;
            int num5 = tuple.Item5;
            if ((removeLogicId == 0 || num1 == 0 || removeLogicId == num1) && (removeSkillId == 0 || num2 == 0 || removeSkillId == num2) && (removeSkillType == 0 || num3 == 0 || removeSkillType == num3) && (removeInvestType == 0 || num4 == 0 || removeInvestType == num4) && removeAilmentGroupId == num5)
            {
              invalidRemoveSkillEffectPatterns.Add(tuple);
              flag = true;
            }
          }
          if (flag)
          {
            BL.SkillEffect skillEffect1 = source.First<BL.SkillEffect>();
            if (skillEffect1.useRemain.HasValue)
            {
              int? useRemain1 = skillEffect1.useRemain;
              int num6 = 1;
              if (useRemain1.GetValueOrDefault() >= num6 & useRemain1.HasValue)
              {
                BL.SkillEffect skillEffect2 = skillEffect1;
                int? useRemain2 = skillEffect2.useRemain;
                skillEffect2.useRemain = useRemain2.HasValue ? new int?(useRemain2.GetValueOrDefault() - 1) : new int?();
                int? useRemain3 = skillEffect1.useRemain;
                int num7 = 0;
                if (useRemain3.GetValueOrDefault() == num7 & useRemain3.HasValue && !isAI && skillEffect1.isLandTagEffect)
                  target.skillEffects.LandTagModified.commit();
              }
            }
          }
        }
      }
      Func<BL.SkillEffect, bool> checkInvalid = (Func<BL.SkillEffect, bool>) (e => invalidRemoveSkillEffectPatterns.Any<Tuple<int, int, int, int, int>>((Func<Tuple<int, int, int, int, int>, bool>) (pattern =>
      {
        int num8 = pattern.Item1;
        int num9 = pattern.Item2;
        int num10 = pattern.Item3;
        int num11 = pattern.Item4;
        int num12 = pattern.Item5;
        if (e.baseSkill.skill_type == BattleskillSkillType.ailment)
        {
          BattleFuncs.PackedSkillEffect packedSkillEffect = BattleFuncs.PackedSkillEffect.Create(e);
          if ((packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.ailment_group_id) ? packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.ailment_group_id) : 0) != num12)
            return false;
        }
        if (num8 != 0 && e.effect.EffectLogic.Enum != (BattleskillEffectLogicEnum) num8 || num9 != 0 && e.baseSkill.ID != num9 || num10 != 0 && e.baseSkill.skill_type != (BattleskillSkillType) num10)
          return false;
        if (num11 == 0 || num11 == 1 && !e.isBaseSkill)
          return true;
        return num11 == 2 && e.isBaseSkill;
      })));
      BL.SkillEffect[] source1 = target.skillEffects.RemoveEffect(removeLogicId, removeSkillId, removeSkillType, BattleFuncs.env, target, (Func<BL.SkillEffect, bool>) (e => BattleFuncs.cantRemoveSkillEffect(e, removeInvestType, removeAilmentGroupId, rangeEffectRemoveFlag, target) || checkInvalid(e)));
      BL.SkillEffect[] removePassiveRangeEffects = ((IEnumerable<BL.SkillEffect>) source1).Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.baseSkill.skill_type == BattleskillSkillType.passive && x.baseSkill.range_effect_passive_skill)).ToArray<BL.SkillEffect>();
      if (((IEnumerable<BL.SkillEffect>) removePassiveRangeEffects).Any<BL.SkillEffect>())
      {
        removePassiveRangeEffects = ((IEnumerable<BL.SkillEffect>) removePassiveRangeEffects).GroupBy(x => new
        {
          ID = x.baseSkill.ID,
          Enum = x.effect.EffectLogic.Enum
        }).Select<IGrouping<\u003C\u003Ef__AnonymousType6<int, BattleskillEffectLogicEnum>, BL.SkillEffect>, BL.SkillEffect>(x => x.First<BL.SkillEffect>()).ToArray<BL.SkillEffect>();
        foreach (BL.ISkillEffectListUnit skillEffectListUnit in BattleFuncs.getAllUnitsOrdered(isAI, false, includeJumping: true))
        {
          if (skillEffectListUnit != target && skillEffectListUnit.skillEffects.All().Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.baseSkill.range_effect_passive_skill && x.unit == target.originalUnit && ((IEnumerable<BL.SkillEffect>) removePassiveRangeEffects).Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (y => x.baseSkill.ID == y.baseSkill.ID && x.effect.EffectLogic.Enum == y.effect.EffectLogic.Enum)))))
          {
            applyChangeSkillEffects.add(BattleFuncs.iSkillEffectListUnitToUnitPosition(skillEffectListUnit), skillEffectListUnit, moveUnit == skillEffectListUnit);
            foreach (BL.SkillEffect skillEffect in removePassiveRangeEffects)
              skillEffectListUnit.skillEffects.RemoveEffect((int) skillEffect.effect.EffectLogic.Enum, skillEffect.baseSkill.ID, 0, BattleFuncs.env, skillEffectListUnit, (Func<BL.SkillEffect, bool>) (e => BattleFuncs.cantRemoveSkillEffect(e, removeInvestType, removeAilmentGroupId, rangeEffectRemoveFlag, target) || checkInvalid(e)));
            if (!isAI)
              skillEffectListUnit.originalUnit.commit();
          }
        }
      }
      if (!isAI)
        target.originalUnit.commit();
      return source1;
    }

    public static int calcAttackDamage(
      BL.ISkillEffectListUnit attack,
      BL.ISkillEffectListUnit defense,
      float percentageAttack = 1f,
      float percentageDecrease = 1f,
      float percentageDamage = 1f,
      bool disableUseCountSkillEffect = true)
    {
      BL.UnitPosition unitPosition1 = BattleFuncs.iSkillEffectListUnitToUnitPosition(attack);
      BL.UnitPosition unitPosition2 = BattleFuncs.iSkillEffectListUnitToUnitPosition(defense);
      BL.MagicBullet beAttackMagicBullet;
      bool flag;
      if (attack.originalUnit.unit.magic_warrior_flag)
      {
        beAttackMagicBullet = (BL.MagicBullet) null;
        flag = false;
      }
      else
      {
        beAttackMagicBullet = ((IEnumerable<BL.MagicBullet>) attack.originalUnit.magicBullets).Where<BL.MagicBullet>((Func<BL.MagicBullet, bool>) (x => x.isAttack)).OrderBy<BL.MagicBullet, int>((Func<BL.MagicBullet, int>) (x => x.cost)).FirstOrDefault<BL.MagicBullet>();
        flag = beAttackMagicBullet != null;
      }
      Judgement.BeforeDuelParameter duelSkill = Judgement.BeforeDuelParameter.CreateDuelSkill(attack, beAttackMagicBullet, BattleFuncs.getPanel(unitPosition1.row, unitPosition1.column), defense, BattleFuncs.getPanel(unitPosition2.row, unitPosition2.column), defenseHp: defense.hp, disableUseCountSkillEffect: disableUseCountSkillEffect);
      duelSkill.DamageRate *= percentageDamage;
      if (flag)
      {
        duelSkill.attackerUnitParameter.MagicAttack = (int) Mathf.Ceil((float) duelSkill.attackerUnitParameter.MagicAttack * percentageAttack);
        duelSkill.defenderUnitParameter.MagicDefense = (int) Mathf.Ceil((float) duelSkill.defenderUnitParameter.MagicDefense * percentageDecrease);
        return duelSkill.DisplayMagicAttack;
      }
      duelSkill.attackerUnitParameter.PhysicalAttack = (int) Mathf.Ceil((float) duelSkill.attackerUnitParameter.PhysicalAttack * percentageAttack);
      duelSkill.defenderUnitParameter.PhysicalDefense = (int) Mathf.Ceil((float) duelSkill.defenderUnitParameter.PhysicalDefense * percentageDecrease);
      return duelSkill.DisplayPhysicalAttack;
    }

    public static void applyDamage(
      BL.ISkillEffectListUnit defense,
      int damage,
      BL.ISkillEffectListUnit attack = null,
      int minHp = 0)
    {
      if (damage <= 0)
        return;
      bool flag = defense is BL.AIUnit;
      int hp = defense.hp;
      if (defense.hp > minHp)
      {
        defense.hp -= damage;
        if (defense.hp < minHp)
          defense.hp = minHp;
      }
      if (hp > defense.hp)
        defense.skillEffects.RemoveEffect(1000418, BattleFuncs.env, defense);
      if (defense.hp <= 0)
      {
        if (attack != null)
        {
          if (!flag)
          {
            ++attack.originalUnit.killCount;
            defense.originalUnit.killedBy = attack.originalUnit;
          }
          attack.skillEffects.AddKillCount(1);
        }
        if (!flag)
        {
          if (BattleFuncs.env.getForceID(defense.originalUnit) == BL.ForceID.player)
            BattleFuncs.env.updateIntimateByDefense(defense.originalUnit);
          NGBattleManager instance = Singleton<NGBattleManager>.GetInstance();
          if (instance.useGameEngine)
            instance.gameEngine.applyDeadUnit(defense.originalUnit, (BL.Unit) null);
        }
      }
      if (flag || attack == null || hp <= defense.hp || attack.originalUnit.isFacility || defense.originalUnit.isFacility)
        return;
      attack.originalUnit.attackDamage += hp - defense.hp;
    }

    public static bool canHeal(BL.ISkillEffectListUnit unit, BattleskillSkillType skillType = (BattleskillSkillType) 0)
    {
      foreach (BL.SkillEffect skillEffect in unit.skillEffects.Where(BattleskillEffectLogicEnum.heal_impossible))
      {
        int num = skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_type);
        if (num == 0 || (BattleskillSkillType) num == skillType)
          return false;
      }
      return true;
    }

    public static int[] getAttackRange(BL.ISkillEffectListUnit unit)
    {
      int[] attackRange = new int[2]{ 1000000, 0 };
      foreach (BL.MagicBullet magicBullet in unit.originalUnit.magicBullets)
      {
        if (magicBullet.isAttack && unit.hp > magicBullet.cost)
        {
          BL.Unit.MagicRange magicRange = BattleFuncs.getMagicRange(unit, magicBullet);
          if (attackRange[0] > magicRange.Min)
            attackRange[0] = magicRange.Min;
          if (attackRange[1] < magicRange.Max)
            attackRange[1] = magicRange.Max;
        }
      }
      BL.Unit.GearRange gearRange = BattleFuncs.getGearRange(unit);
      if (attackRange[0] > gearRange.Min)
        attackRange[0] = gearRange.Min;
      if (attackRange[1] < gearRange.Max)
        attackRange[1] = gearRange.Max;
      return attackRange;
    }

    public static int[] getHealRange(BL.ISkillEffectListUnit unit)
    {
      int[] numArray = new int[2]{ 1000000, 0 };
      foreach (BL.MagicBullet magicBullet in unit.originalUnit.magicBullets)
      {
        if (magicBullet.isHeal && unit.hp > magicBullet.cost)
        {
          BL.Unit.MagicRange magicRange = BattleFuncs.getMagicRange(unit, magicBullet);
          if (numArray[0] > magicRange.Min)
            numArray[0] = magicRange.Min;
          if (numArray[1] < magicRange.Max)
            numArray[1] = magicRange.Max;
        }
      }
      return numArray[0] == 1000000 ? new int[0] : numArray;
    }

    public static BL.Unit.GearRange getGearRange(BL.ISkillEffectListUnit unit)
    {
      IEnumerable<BL.SkillEffect> skillEffects1 = unit.enabledSkillEffect(BattleskillEffectLogicEnum.fix_range).Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x =>
      {
        int num1 = x.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id);
        if (num1 != 0 && num1 != unit.originalUnit.unit.kind.ID || x.effect.HasKey(BattleskillEffectLogicArgumentEnum.magic_type) && x.effect.GetInt(BattleskillEffectLogicArgumentEnum.magic_type) != 0)
          return false;
        if (x.effect.HasKey(BattleskillEffectLogicArgumentEnum.min_hp_percentage))
        {
          Decimal num2 = (Decimal) unit.hp / (Decimal) unit.originalUnit.parameter.Hp;
          float num3 = x.effect.GetFloat(BattleskillEffectLogicArgumentEnum.min_hp_percentage);
          float num4 = x.effect.GetFloat(BattleskillEffectLogicArgumentEnum.max_hp_percentage);
          if ((double) num3 != 0.0 && num2 < (Decimal) num3 || (double) num4 != 0.0 && num2 >= (Decimal) num4)
            return false;
        }
        return true;
      }));
      IEnumerable<BL.SkillEffect> skillEffects2 = BattleFuncs.gearSkillEffectFilter(unit.originalUnit, skillEffects1);
      BL.Unit.GearRange gearRange = unit.originalUnit.weapon.getGearRange();
      int min = gearRange.Min;
      int max = gearRange.Max;
      foreach (BL.SkillEffect skillEffect in skillEffects2)
      {
        min += skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_add);
        max += skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_add);
      }
      return new BL.Unit.GearRange(min, max);
    }

    public static BL.Unit.MagicRange getMagicRange(BL.ISkillEffectListUnit unit, BL.MagicBullet mb)
    {
      IEnumerable<BL.SkillEffect> skillEffects1 = unit.enabledSkillEffect(BattleskillEffectLogicEnum.fix_range).Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x =>
      {
        int num1 = x.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id);
        if (num1 != 0 && num1 != unit.originalUnit.unit.kind.ID)
          return false;
        if (x.effect.HasKey(BattleskillEffectLogicArgumentEnum.min_hp_percentage))
        {
          Decimal num2 = (Decimal) unit.hp / (Decimal) unit.originalUnit.parameter.Hp;
          float num3 = x.effect.GetFloat(BattleskillEffectLogicArgumentEnum.min_hp_percentage);
          float num4 = x.effect.GetFloat(BattleskillEffectLogicArgumentEnum.max_hp_percentage);
          if ((double) num3 != 0.0 && num2 < (Decimal) num3 || (double) num4 != 0.0 && num2 >= (Decimal) num4)
            return false;
        }
        int num5 = x.effect.HasKey(BattleskillEffectLogicArgumentEnum.magic_type) ? x.effect.GetInt(BattleskillEffectLogicArgumentEnum.magic_type) : 0;
        if (num5 == 0 || num5 == 1 && mb.isAttack)
          return true;
        return num5 == 2 && mb.isHeal;
      }));
      IEnumerable<BL.SkillEffect> skillEffects2 = BattleFuncs.gearSkillEffectFilter(unit.originalUnit, skillEffects1);
      int minRange = mb.minRange;
      int maxRange = mb.maxRange;
      foreach (BL.SkillEffect skillEffect in skillEffects2)
      {
        minRange += skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_add);
        maxRange += skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_add);
      }
      return new BL.Unit.MagicRange(minRange, maxRange);
    }

    public static float calcSpecResistDamageRate(
      BL.ISkillEffectListUnit attack,
      BL.ISkillEffectListUnit defense,
      int argumentCheckValue,
      BattleskillEffectLogicEnum specLogic,
      BattleskillEffectLogicEnum resistLogic,
      BattleskillEffectLogicArgumentEnum specArgument,
      BattleskillEffectLogicArgumentEnum resistArgument,
      BL.Panel attackPanel = null,
      BL.Panel defensePanel = null)
    {
      List<BattleFuncs.SkillParam> skillParams1 = new List<BattleFuncs.SkillParam>();
      List<BattleFuncs.SkillParam> skillParams2 = new List<BattleFuncs.SkillParam>();
      List<BattleFuncs.SkillParam> skillParams3 = new List<BattleFuncs.SkillParam>();
      List<BattleFuncs.SkillParam> skillParams4 = new List<BattleFuncs.SkillParam>();
      Action<BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, BattleskillEffectLogicEnum, BattleskillEffectLogicArgumentEnum, List<BattleFuncs.SkillParam>, List<BattleFuncs.SkillParam>, BL.Panel> action = (Action<BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, BattleskillEffectLogicEnum, BattleskillEffectLogicArgumentEnum, List<BattleFuncs.SkillParam>, List<BattleFuncs.SkillParam>, BL.Panel>) ((effectUnit, targetUnit, logic, argument, buffParams, debuffParams, effectPanel) =>
      {
        foreach (BL.SkillEffect effect in effectUnit.skillEffects.Where(logic, (Func<BL.SkillEffect, bool>) (x =>
        {
          BattleskillEffect effect = x.effect;
          if (BattleFuncs.isSealedSkillEffect(effectUnit, x) || effect.GetInt(argument) != 0 && effect.GetInt(argument) != argumentCheckValue || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != effectUnit.originalUnit.unit.kind.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != targetUnit.originalUnit.unit.kind.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != effectUnit.originalUnit.playerUnit.GetElement() || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != targetUnit.originalUnit.playerUnit.GetElement() || effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) != 0 && !effectUnit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id)) || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) != 0 && !targetUnit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id)) || BattleFuncs.isEffectEnemyRangeAndInvalid(x, effectUnit, targetUnit) || BattleFuncs.isSkillsAndEffectsInvalid(effectUnit, targetUnit))
            return false;
          BattleFuncs.PackedSkillEffect packedSkillEffect = effect.GetPackedSkillEffect();
          if (packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.min_range))
          {
            int num4 = packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.min_range);
            int num5 = packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.max_range);
            if (attackPanel == null)
            {
              if (num4 != 0 || num5 != 0)
                return false;
            }
            else
            {
              int num6 = BL.fieldDistance(attackPanel, defensePanel);
              if (num4 != 0 && num6 < num4 || num5 != 0 && num6 > num5)
                return false;
            }
          }
          return !packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.land_tag1) || attackPanel != null && packedSkillEffect.CheckLandTag(effectPanel, effectUnit is BL.AIUnit);
        })))
        {
          float mulParam = effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.buff_debuff_value) + (float) effect.baseSkillLevel * effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio);
          BattleFuncs.SkillParam mul = BattleFuncs.SkillParam.CreateMul(effectUnit.originalUnit, effect, mulParam);
          if ((double) mulParam > 0.0)
            buffParams.Add(mul);
          else if ((double) mulParam < 0.0)
            debuffParams.Add(mul);
        }
      });
      action(attack, defense, specLogic, specArgument, skillParams1, skillParams2, attackPanel);
      action(defense, attack, resistLogic, resistArgument, skillParams3, skillParams4, defensePanel);
      Decimal num7 = skillParams1.Count > 0 ? (Decimal) BattleFuncs.gearSkillParamFilter(skillParams1).Max<BattleFuncs.SkillParam>((Func<BattleFuncs.SkillParam, float>) (x => x.mulParam.Value)) : 0.0M;
      Decimal num8 = skillParams2.Count > 0 ? (Decimal) BattleFuncs.gearSkillParamFilter(skillParams2).Min<BattleFuncs.SkillParam>((Func<BattleFuncs.SkillParam, float>) (x => x.mulParam.Value)) : 0.0M;
      Decimal num9 = 1.0M + ((skillParams3.Count > 0 ? (Decimal) BattleFuncs.gearSkillParamFilter(skillParams3).Max<BattleFuncs.SkillParam>((Func<BattleFuncs.SkillParam, float>) (x => x.mulParam.Value)) : 0.0M) + (skillParams4.Count > 0 ? (Decimal) BattleFuncs.gearSkillParamFilter(skillParams4).Min<BattleFuncs.SkillParam>((Func<BattleFuncs.SkillParam, float>) (x => x.mulParam.Value)) : 0.0M) - num7 - num8);
      if (num9 < 0.01M)
        num9 = 0.01M;
      Decimal d = 1.0M / num9;
      if (d < 0M)
        d = 0M;
      return (float) Math.Round(d, 4);
    }

    public static float calcAttackClassificationRate(
      AttackStatus attackStatus,
      BL.ISkillEffectListUnit attack,
      BL.ISkillEffectListUnit defense,
      BL.Panel attackPanel,
      BL.Panel defensePanel)
    {
      GearAttackClassification? nullable = new GearAttackClassification?();
      if (attackStatus.isMagic)
        nullable = new GearAttackClassification?(GearAttackClassification.magic);
      else if (attackStatus.weapon != null)
      {
        nullable = new GearAttackClassification?(attackStatus.weapon.attackMethod.attackClass);
      }
      else
      {
        GearGear equippedGearOrInitial = attack.originalUnit.playerUnit.equippedGearOrInitial;
        nullable = new GearAttackClassification?(equippedGearOrInitial.hasAttackClass ? equippedGearOrInitial.gearClassification.attack_classification : attack.originalUnit.playerUnit.initial_gear.gearClassification.attack_classification);
      }
      return nullable.HasValue ? BattleFuncs.calcSpecResistDamageRate(attack, defense, (int) nullable.Value, BattleskillEffectLogicEnum.attack_class_spec_ratio, BattleskillEffectLogicEnum.attack_class_resist_ratio, BattleskillEffectLogicArgumentEnum.attack_classification_id, BattleskillEffectLogicArgumentEnum.target_attack_classification_id, attackPanel, defensePanel) : 1f;
    }

    public static Tuple<int, int> getNextCompleteActionCount(
      BL.ISkillEffectListUnit unit,
      BL.UnitPosition up = null,
      BL.ISkillEffectListUnit attack = null,
      BL.ISkillEffectListUnit defense = null,
      int defenseHp = 0,
      bool isSample = false)
    {
      if (up == null)
        up = BattleFuncs.iSkillEffectListUnitToUnitPosition(unit);
      int num1 = up.completedCount;
      int num2 = up.actionCount;
      if (num1 > 1)
      {
        int num3 = 1;
        int num4 = 1;
        BL.PhaseState phaseState = BattleFuncs.getPhaseState();
        foreach (BL.SkillEffect againEffect in unit.skillEffects.GetAgainEffects())
        {
          if ((!againEffect.effect.HasKey(BattleskillEffectLogicArgumentEnum.condition) || againEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.condition) != 1 || attack != null && defense != null && defenseHp < 1) && BattleFuncs.checkInvokeSkillEffect(BattleFuncs.PackedSkillEffect.Create(againEffect), unit, defense))
          {
            if (!againEffect.againInvoked)
            {
              if (againEffect.useRemain.HasValue)
              {
                int? useRemain = againEffect.useRemain;
                int num5 = 0;
                if (useRemain.GetValueOrDefault() <= num5 & useRemain.HasValue)
                  continue;
              }
              if (againEffect.effect.HasKey(BattleskillEffectLogicArgumentEnum.turn_invoke_count) && againEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.turn_invoke_count) != 0 && againEffect.turnCount >= againEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.turn_invoke_count))
                continue;
            }
            if (!isSample && phaseState != null && phaseState.absoluteTurnCount >= 1 && !againEffect.againInvoked)
            {
              againEffect.againInvoked = true;
              ++againEffect.turnCount;
              if (againEffect.useRemain.HasValue)
              {
                int? useRemain1 = againEffect.useRemain;
                int num6 = 1;
                if (useRemain1.GetValueOrDefault() >= num6 & useRemain1.HasValue)
                {
                  BL.SkillEffect skillEffect = againEffect;
                  int? useRemain2 = skillEffect.useRemain;
                  skillEffect.useRemain = useRemain2.HasValue ? new int?(useRemain2.GetValueOrDefault() - 1) : new int?();
                }
              }
            }
            int num7 = againEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.complete_count);
            if (num7 > num3)
              num3 = num7;
            int num8 = againEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.action_count);
            if (num8 > num4)
              num4 = num8;
          }
        }
        if (num1 > num3)
          num1 = num3;
        if (num2 > num4)
          num2 = num4;
        if (!isSample && num2 > 1 && up.dontUseSkillAgain && phaseState != null && phaseState.absoluteTurnCount >= 1)
        {
          int[] numArray = new int[2]
          {
            300003113,
            300003114
          };
          foreach (int key in numArray)
          {
            BattleskillSkill skill = MasterData.BattleskillSkill[key];
            foreach (BattleskillEffect effect in skill.Effects)
              unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, skill, 1, isDontDisplay: true));
          }
          if (!(unit is BL.AIUnit))
            unit.originalUnit.commit();
        }
      }
      if (num2 > 0)
        --num2;
      return Tuple.Create<int, int>(num1 - 1, num2);
    }

    public static IEnumerable<BattleskillSkill> getAttackMethodExtraSkill(
      BL.Weapon weapon,
      bool isMagic)
    {
      if (!isMagic && weapon != null)
      {
        foreach (BattleskillEffect battleskillEffect in ((IEnumerable<BattleskillEffect>) weapon.attackMethod.skill.Effects).Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.attack_method_extra_skill)))
        {
          int key = battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id);
          if (MasterData.BattleskillSkill.ContainsKey(key))
            yield return MasterData.BattleskillSkill[key];
        }
      }
    }

    public static IEnumerable<BattleskillSkill> getAttackMethodExtraSkill(AttackStatus attackStatus)
    {
      return BattleFuncs.getAttackMethodExtraSkill(attackStatus.weapon, attackStatus.isMagic);
    }

    public static IEnumerable<BL.SkillEffect> getAttackMethodExtraSkillEffects(
      BL.Weapon weapon,
      bool isMagic,
      BattleskillEffectLogicEnum logic)
    {
      foreach (BattleskillSkill skill in BattleFuncs.getAttackMethodExtraSkill(weapon, isMagic))
      {
        foreach (BattleskillEffect effect in ((IEnumerable<BattleskillEffect>) skill.Effects).Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == logic)))
          yield return BL.SkillEffect.FromMasterData(effect, skill, 1, isAttackMethod: true);
      }
    }

    public static IEnumerable<BL.SkillEffect> getAttackMethodExtraSkillEffects(
      AttackStatus attackStatus,
      BattleskillEffectLogicEnum logic)
    {
      return BattleFuncs.getAttackMethodExtraSkillEffects(attackStatus.weapon, attackStatus.isMagic, logic);
    }

    public static bool checkInvokeGamemode(
      int invokeGamemode,
      bool isColosseum,
      BL.ISkillEffectListUnit unit)
    {
      if (invokeGamemode == 0)
        return true;
      if (isColosseum)
        return (invokeGamemode & 2) != 0;
      if ((invokeGamemode & 1) == 0)
        return false;
      if ((invokeGamemode & 508) == 0)
        return true;
      NGBattleManager instance = Singleton<NGBattleManager>.GetInstance();
      if (instance.isPvp || instance.isPvnpc)
        return (invokeGamemode & 8) != 0;
      if (instance.isTower)
        return (invokeGamemode & 64) != 0;
      if (instance.isRaid)
        return (invokeGamemode & 32) != 0;
      if (!instance.isGvg)
        return (invokeGamemode & 4) != 0;
      return (invokeGamemode & 128) != 0 && unit != null && unit.originalUnit.isPlayerForce || (invokeGamemode & 256) != 0 && unit != null && !unit.originalUnit.isPlayerForce || (invokeGamemode & 16) != 0;
    }

    public static bool checkRushInvoke(
      BL.ISkillEffectListUnit myself,
      BL.ISkillEffectListUnit enemy,
      AttackStatus myselfStatus,
      AttackStatus enemyStatus,
      int myselfHp,
      int enemyHp,
      XorShift random,
      int? colosseumTurn = null,
      BL.Panel panel = null)
    {
      return myselfStatus != null && myself.skillEffects.Where(BattleskillEffectLogicEnum.rush, (Func<BL.SkillEffect, bool>) (x =>
      {
        BattleFuncs.PackedSkillEffect pse = BattleFuncs.PackedSkillEffect.Create(x);
        if (BattleFuncs.isSealedSkillEffect(myself, x) || !BattleFuncs.checkInvokeSkillEffect(pse, myself, enemy, colosseumTurn, unitHp: new int?(myselfHp), targetHp: new int?(enemyHp)) || !pse.CheckLandTag(panel, myself is BL.AIUnit) || BattleFuncs.isEffectEnemyRangeAndInvalid(x, myself, enemy) || BattleFuncs.isSkillsAndEffectsInvalid(myself, enemy))
          return false;
        float percentage_invocation = pse.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invocation);
        return (double) percentage_invocation >= 200.0 || BattleFuncs.isInvoke(myself, enemy, myselfStatus.duelParameter.attackerUnitParameter, myselfStatus.duelParameter.defenderUnitParameter, myselfStatus, enemyStatus, x.baseSkillLevel, percentage_invocation, random, false, myselfHp, enemyHp, colosseumTurn, base_invocation: pse.GetFloat(BattleskillEffectLogicArgumentEnum.base_invocation), invocation_skill_ratio: pse.GetFloat(BattleskillEffectLogicArgumentEnum.invocation_skill_ratio), invocation_luck_ratio: pse.GetFloat(BattleskillEffectLogicArgumentEnum.invocation_luck_ratio));
      })).Any<BL.SkillEffect>();
    }

    public static bool calcStealEffectParam(
      Judgement.BattleParameter battleParameter,
      BattleskillEffect effect,
      out int param)
    {
      Judgement.Params @params = (Judgement.Params) effect.GetInt(BattleskillEffectLogicArgumentEnum.param_id);
      if (!battleParameter.GetParamsValue(@params, out param) || @params == Judgement.Params.Hp)
        return false;
      int num1 = Mathf.CeilToInt((float) ((Decimal) param * (Decimal) effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) + (Decimal) effect.GetInt(BattleskillEffectLogicArgumentEnum.value)));
      int num2 = effect.GetInt(BattleskillEffectLogicArgumentEnum.limit);
      if (num1 > param)
        num1 = param;
      if (num2 != 0 && num1 > num2)
        num1 = num2;
      param = num1;
      return param > 0;
    }

    public static bool executeSteal(
      BL.ISkillEffectListUnit myself,
      BL.ISkillEffectListUnit target,
      BattleskillEffect effect,
      BL.UnitPosition myselfUp,
      BL.UnitPosition targetUp,
      bool isAI)
    {
      int num1;
      if (!BattleFuncs.calcStealEffectParam(target.parameter, effect, out num1))
        return false;
      int key = effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id);
      if (!MasterData.BattleskillSkill.ContainsKey(key))
        return false;
      BattleskillSkill skill = MasterData.BattleskillSkill[key];
      if (skill.Effects.Length == 0)
        return false;
      BattleskillEffect effect1 = skill.Effects[0];
      if (effect1.effect_logic.Enum != BattleskillEffectLogicEnum.steal_effect)
        return false;
      int num2 = effect.GetInt(BattleskillEffectLogicArgumentEnum.param_id);
      int num3 = effect.GetInt(BattleskillEffectLogicArgumentEnum.turn);
      BL.SkillEffect effect2 = BL.SkillEffect.FromMasterData(effect1, skill, 1, investUnit: myself.originalUnit);
      myself.skillEffects.Add(effect2);
      effect2.unit = myself.originalUnit;
      if (num3 != 0)
        effect2.turnRemain = new int?(num3);
      effect2.work = new float[2]
      {
        (float) num2,
        (float) num1
      };
      BL.SkillEffect effect3 = BL.SkillEffect.FromMasterData(effect1, skill, 1, investUnit: myself.originalUnit);
      target.skillEffects.Add(effect3);
      effect3.unit = myself.originalUnit;
      if (num3 != 0)
        effect3.turnRemain = new int?(num3);
      effect3.work = new float[2]
      {
        (float) num2,
        (float) -num1
      };
      if (num2 == 8)
      {
        myselfUp.clearMovePanelCache();
        targetUp.clearMovePanelCache();
      }
      if (!isAI)
      {
        myself.originalUnit.commit();
        target.originalUnit.commit();
      }
      return true;
    }

    public static bool executeProvide(
      BL.ISkillEffectListUnit myself,
      BL.ISkillEffectListUnit target,
      BattleskillEffect effect,
      BL.UnitPosition myselfUp,
      BL.UnitPosition targetUp,
      bool isAI,
      int targetCount,
      Judgement.BattleParameter battleParameter,
      bool isFirst)
    {
      int num1;
      if (!BattleFuncs.calcStealEffectParam(battleParameter, effect, out num1))
        return false;
      int key = effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id);
      if (!MasterData.BattleskillSkill.ContainsKey(key))
        return false;
      BattleskillSkill skill = MasterData.BattleskillSkill[key];
      if (skill.Effects.Length == 0)
        return false;
      BattleskillEffect effect1 = skill.Effects[0];
      if (effect1.effect_logic.Enum != BattleskillEffectLogicEnum.steal_effect)
        return false;
      int num2 = effect.GetInt(BattleskillEffectLogicArgumentEnum.param_id);
      int num3 = effect.GetInt(BattleskillEffectLogicArgumentEnum.turn);
      int num4 = (int) Math.Ceiling((Decimal) num1 / (Decimal) targetCount);
      if (isFirst)
      {
        BL.SkillEffect effect2 = BL.SkillEffect.FromMasterData(effect1, skill, 1, investUnit: myself.originalUnit);
        myself.skillEffects.Add(effect2);
        effect2.unit = myself.originalUnit;
        if (num3 != 0)
          effect2.turnRemain = new int?(num3);
        effect2.work = new float[2]
        {
          (float) num2,
          (float) -(num4 * targetCount)
        };
      }
      BL.SkillEffect effect3 = BL.SkillEffect.FromMasterData(effect1, skill, 1, investUnit: myself.originalUnit);
      target.skillEffects.Add(effect3);
      effect3.unit = myself.originalUnit;
      if (num3 != 0)
        effect3.turnRemain = new int?(num3);
      effect3.work = new float[2]
      {
        (float) num2,
        (float) num4
      };
      if (num2 == 8)
      {
        myselfUp.clearMovePanelCache();
        targetUp.clearMovePanelCache();
      }
      if (!isAI)
      {
        myself.originalUnit.commit();
        target.originalUnit.commit();
      }
      return true;
    }

    public static void removeStealEffects(BL.ISkillEffectListUnit deadUnit)
    {
      bool isAI = deadUnit is BL.AIUnit;
      foreach (BL.ISkillEffectListUnit allUnit in BattleFuncs.getAllUnits(isAI, false, includeJumping: true))
      {
        if (allUnit.skillEffects.Where(BattleskillEffectLogicEnum.steal_effect).Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.timeOfDeathDisable && x.unit == deadUnit.originalUnit)))
        {
          allUnit.skillEffects.RemoveEffect(1001676, 0, 0, BattleFuncs.env, allUnit, (Func<BL.SkillEffect, bool>) (x => !x.timeOfDeathDisable || x.unit != deadUnit.originalUnit));
          if (!isAI)
            allUnit.originalUnit.commit();
        }
      }
    }

    public static int getTransformationGroupId(BL.ISkillEffectListUnit unit)
    {
      BL.SkillEffect skillEffect = unit.skillEffects.Where(BattleskillEffectLogicEnum.transformation).FirstOrDefault<BL.SkillEffect>();
      return skillEffect == null ? 0 : skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.transformation_group_id);
    }

    public static SkillMetamorphosis getMetamorphosis(BL.ISkillEffectListUnit unit)
    {
      BL.SkillEffect skillEffect = unit.skillEffects.Where(BattleskillEffectLogicEnum.transformation).FirstOrDefault<BL.SkillEffect>();
      return skillEffect == null ? (SkillMetamorphosis) null : MasterData.FindSkillMetamorphosis(unit.originalUnit.unitId, skillEffect.baseSkillId, skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.transformation_group_id));
    }

    public static bool checkEnableUnitSkill(BL.ISkillEffectListUnit unit, BattleskillSkill skill)
    {
      if (!skill.transformationGroupId.HasValue)
        return true;
      int transformationGroupId1 = BattleFuncs.getTransformationGroupId(unit);
      int? transformationGroupId2 = skill.transformationGroupId;
      int num = transformationGroupId1;
      return transformationGroupId2.GetValueOrDefault() == num & transformationGroupId2.HasValue;
    }

    public static bool checkSkillLogicInvest(
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      int logicId,
      int ailmentGroupId,
      int skillId,
      int skillType,
      int investType,
      int conditionTarget)
    {
      BL.ISkillEffectListUnit checkUnit = conditionTarget == 0 ? unit : target;
      return (logicId == 0 ? (IEnumerable<BL.SkillEffect>) checkUnit.skillEffects.All() : checkUnit.skillEffects.Where((BattleskillEffectLogicEnum) logicId)).Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x =>
      {
        if (x.isDontDisplay || x.useRemain.HasValue && x.useRemain.Value <= 0 || logicId == 1001338 && x.work != null && BattleFuncs.getShieldRemain(x, checkUnit) <= 0 || skillId != 0 && skillId != x.baseSkill.ID || skillType != 0 && (BattleskillSkillType) skillType != x.baseSkill.skill_type || investType != 0 && (investType == 1 && x.isBaseSkill || investType == 2 && !x.isBaseSkill))
          return false;
        if (x.baseSkill.skill_type == BattleskillSkillType.ailment)
        {
          BattleFuncs.PackedSkillEffect packedSkillEffect = BattleFuncs.PackedSkillEffect.Create(x);
          if ((packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.ailment_group_id) ? packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.ailment_group_id) : 0) != ailmentGroupId)
            return false;
        }
        return true;
      }));
    }

    public static List<BL.Unit> getProvokeUnits(BL.ISkillEffectListUnit unit, BL.Panel panel = null)
    {
      if (unit.IsCharm)
        return (List<BL.Unit>) null;
      bool isAI = unit is BL.AIUnit;
      List<Tuple<BL.Unit, BL.SkillEffect>> unitEffects = new List<Tuple<BL.Unit, BL.SkillEffect>>();
      Action<BL.Unit, BL.SkillEffect> action = (Action<BL.Unit, BL.SkillEffect>) ((target, effect) =>
      {
        if (!isAI)
        {
          if (target.isDead || target.IsJumping)
            return;
          unitEffects.Add(Tuple.Create<BL.Unit, BL.SkillEffect>(target, effect));
        }
        else
        {
          BL.AIUnit aiUnit = BattleFuncs.env.getAIUnit(target);
          if (aiUnit == null || aiUnit.isDead || aiUnit.IsJumping)
            return;
          unitEffects.Add(Tuple.Create<BL.Unit, BL.SkillEffect>(target, effect));
        }
      });
      if (panel == null)
      {
        BL.UnitPosition unitPosition = BattleFuncs.iSkillEffectListUnitToUnitPosition(unit);
        panel = BattleFuncs.getPanel(unitPosition.row, unitPosition.column);
      }
      foreach (BL.SkillEffect skillEffect in panel.getSkillEffects(isAI).value.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (effect =>
      {
        if (effect.effect.effect_logic.Enum != BattleskillEffectLogicEnum.charisma_provoke)
          return false;
        BL.ISkillEffectListUnit iskillEffectListUnit = BattleFuncs.unitToISkillEffectListUnit(effect.parentUnit, isAI);
        return BattleFuncs.getForceID(unit.originalUnit) != BattleFuncs.getForceID(iskillEffectListUnit.originalUnit) && !BattleFuncs.isSealedSkillEffect(iskillEffectListUnit, effect) && BattleFuncs.checkInvokeSkillEffect(BattleFuncs.PackedSkillEffect.Create(effect), iskillEffectListUnit, unit);
      })))
        action(skillEffect.parentUnit, skillEffect);
      foreach (BL.SkillEffect skillEffect in unit.skillEffects.Where(BattleskillEffectLogicEnum.provoke))
      {
        BL.Unit investUnit = skillEffect.investUnit;
        if (!(investUnit == (BL.Unit) null))
          action(investUnit, skillEffect);
      }
      if (!unitEffects.Any<Tuple<BL.Unit, BL.SkillEffect>>())
        return (List<BL.Unit>) null;
      int maxWeight = unitEffects.Max<Tuple<BL.Unit, BL.SkillEffect>>((Func<Tuple<BL.Unit, BL.SkillEffect>, int>) (x => x.Item2.baseSkill.weight));
      return unitEffects.Where<Tuple<BL.Unit, BL.SkillEffect>>((Func<Tuple<BL.Unit, BL.SkillEffect>, bool>) (x => x.Item2.baseSkill.weight == maxWeight)).Select<Tuple<BL.Unit, BL.SkillEffect>, BL.Unit>((Func<Tuple<BL.Unit, BL.SkillEffect>, BL.Unit>) (x => x.Item1)).Distinct<BL.Unit>().ToList<BL.Unit>();
    }

    public static IEnumerable<BL.SkillEffect> getImmediateRebirthEffects(
      BL.ISkillEffectListUnit unit)
    {
      return (IEnumerable<BL.SkillEffect>) unit.skillEffects.Where(BattleskillEffectLogicEnum.immediate_rebirth_fix).Concat<BL.SkillEffect>(unit.skillEffects.Where(BattleskillEffectLogicEnum.immediate_rebirth_ratio)).Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x =>
      {
        BL.UnitPosition unitPosition = BattleFuncs.iSkillEffectListUnitToUnitPosition(unit);
        BL.Panel panel = BattleFuncs.getPanel(unitPosition.row, unitPosition.column);
        if (x.effect.GetPackedSkillEffect().CheckLandTag(panel, unit is BL.AIUnit))
        {
          int? useRemain = x.useRemain;
          if (useRemain.HasValue)
          {
            useRemain = x.useRemain;
            if (useRemain.Value < 1)
              goto label_4;
          }
          return !BattleFuncs.isSealedSkillEffect(unit, x);
        }
label_4:
        return false;
      })).OrderBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (x => x.effectId)).OrderByDescending<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (x => x.baseSkill.weight)).ThenBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (x => x.effectId));
    }

    public static void useImmediateRebirthEffect(
      BL.ISkillEffectListUnit unit,
      BL.SkillEffect immediateRebirthEffect)
    {
      BattleskillEffect effect = immediateRebirthEffect.effect;
      if (effect.EffectLogic.Enum == BattleskillEffectLogicEnum.immediate_rebirth_fix)
      {
        unit.hp = effect.GetInt(BattleskillEffectLogicArgumentEnum.value) + immediateRebirthEffect.baseSkillLevel * effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_ratio);
      }
      else
      {
        Decimal num = (Decimal) effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) + (Decimal) immediateRebirthEffect.baseSkillLevel * (Decimal) effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio);
        unit.hp = (int) Math.Ceiling((Decimal) unit.originalUnit.parameter.Hp * num);
      }
      int? useRemain = immediateRebirthEffect.useRemain;
      if (!useRemain.HasValue)
        return;
      BL.SkillEffect skillEffect = immediateRebirthEffect;
      useRemain = skillEffect.useRemain;
      int? nullable = useRemain;
      skillEffect.useRemain = nullable.HasValue ? new int?(nullable.GetValueOrDefault() - 1) : new int?();
      useRemain = immediateRebirthEffect.useRemain;
      int num1 = 0;
      if (!(useRemain.GetValueOrDefault() == num1 & useRemain.HasValue) || unit is BL.AIUnit || !immediateRebirthEffect.isLandTagEffect)
        return;
      unit.skillEffects.LandTagModified.commit();
    }

    public static void removeJumpEffects(
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target = null,
      BL.SkillEffect jumpEffect = null)
    {
      if (jumpEffect == null)
        jumpEffect = unit.skillEffects.Where(BattleskillEffectLogicEnum.jump).FirstOrDefault<BL.SkillEffect>();
      if (jumpEffect == null)
        return;
      if (target == null)
      {
        BL.UnitPosition unitPosition = BattleFuncs.env.unitPositions.value.FirstOrDefault<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => (double) x.id == (double) jumpEffect.work[0]));
        target = !(unit is BL.AIUnit) ? (BL.ISkillEffectListUnit) unitPosition.unit : (BL.ISkillEffectListUnit) BattleFuncs.env.getAIUnit(unitPosition.unit);
      }
      foreach (BL.SkillEffect skillEffect in unit.skillEffects.All().Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.investSkillId == jumpEffect.baseSkillId)))
        unit.skillEffects.RemoveEffect(0, skillEffect.baseSkillId, 0, BattleFuncs.env, unit);
      target?.skillEffects.RemoveEffect(1001837, 0, 0, BattleFuncs.env, target, (Func<BL.SkillEffect, bool>) (x => x.investUnit != unit.originalUnit));
    }

    public static IEnumerable<BL.SkillEffect> getAilmentTriggerSkillEffects(
      int skillId,
      BL.ISkillEffectListUnit myself,
      BL.ISkillEffectListUnit from,
      BL.Panel myPanel,
      int? myselfHp = null,
      int? fromHp = null)
    {
      if (MasterData.BattleskillSkill.ContainsKey(skillId))
      {
        BattleskillSkill skill = MasterData.BattleskillSkill[skillId];
        if (skill.skill_type == BattleskillSkillType.ailment)
        {
          IEnumerable<BL.SkillEffect> skillEffects = myself.skillEffects.Where(BattleskillEffectLogicEnum.ailment_trigger, (Func<BL.SkillEffect, bool>) (effect =>
          {
            Tuple<int, int>[] logics = new Tuple<int, int>[1]
            {
              Tuple.Create<int, int>(effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.effect_logic), effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.ailment_group_id))
            };
            if (!((IEnumerable<BattleskillEffect>) skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (ef => ((IEnumerable<Tuple<int, int>>) logics).Any<Tuple<int, int>>((Func<Tuple<int, int>, bool>) (y =>
            {
              if ((BattleskillEffectLogicEnum) y.Item1 != ef.EffectLogic.Enum)
                return false;
              BattleFuncs.PackedSkillEffect packedSkillEffect = BattleFuncs.PackedSkillEffect.Create(ef);
              return (packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.ailment_group_id) ? packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.ailment_group_id) : 0) == y.Item2;
            })))))
              return false;
            BattleFuncs.PackedSkillEffect pse = BattleFuncs.PackedSkillEffect.Create(effect);
            if (BattleFuncs.isSealedSkillEffect(myself, effect) || !BattleFuncs.checkInvokeSkillEffect(pse, myself, from, unitHp: myselfHp, targetHp: fromHp) || !pse.CheckLandTag(myPanel, myself is BL.AIUnit) || from != null && BattleFuncs.isEffectEnemyRangeAndInvalid(effect, myself, from))
              return false;
            return from == null || !BattleFuncs.isSkillsAndEffectsInvalid(myself, from);
          }));
          foreach (BL.SkillEffect triggerSkillEffect in BattleFuncs.gearSkillEffectFilter(myself.originalUnit, skillEffects))
            yield return triggerSkillEffect;
        }
      }
    }

    public static bool checkUseSkillInvokeGameMode(
      BL.ISkillEffectListUnit unit,
      BL.Skill skill,
      bool isColosseum)
    {
      BattleskillEffect battleskillEffect = ((IEnumerable<BattleskillEffect>) skill.skill.Effects).FirstOrDefault<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.use_skill_ext_check && x.checkLevel(skill.level)));
      return battleskillEffect == null || BattleFuncs.checkInvokeGamemode(battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.use_skill_invoke_gamemode), isColosseum, unit);
    }

    public static bool checkUseCommandSkillMaxCountInDeck(
      BL.ISkillEffectListUnit unit,
      BL.Skill skill)
    {
      BattleskillEffect battleskillEffect = ((IEnumerable<BattleskillEffect>) skill.skill.Effects).FirstOrDefault<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.use_skill_max_count_in_deck));
      if (battleskillEffect != null)
      {
        int num1 = battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.use_skill_max_count_in_deck);
        bool flag = unit is BL.AIUnit;
        List<BL.Unit> unitList = BattleFuncs.forceUnits(BattleFuncs.env.getForceID(unit.originalUnit)).value;
        int num2 = 0;
        foreach (BL.Unit unit1 in unitList)
        {
          if (((IEnumerable<BL.Skill>) unit1.skills).Any<BL.Skill>())
          {
            BL.ISkillEffectListUnit skillEffectListUnit;
            if (!flag)
            {
              skillEffectListUnit = (BL.ISkillEffectListUnit) unit1;
            }
            else
            {
              skillEffectListUnit = (BL.ISkillEffectListUnit) BattleFuncs.env.getAIUnit(unit1);
              if (skillEffectListUnit == null)
                continue;
            }
            foreach (BL.Skill skill1 in skillEffectListUnit.skills)
            {
              if (skill1.id == skill.id)
              {
                num2 += skill1.nowUseCount;
                if (num2 >= num1)
                  return false;
              }
            }
          }
        }
      }
      return true;
    }

    public static bool checkUseOugiSkillMaxCountInDeck(BL.ISkillEffectListUnit unit, BL.Skill skill)
    {
      BattleskillEffect battleskillEffect = ((IEnumerable<BattleskillEffect>) skill.skill.Effects).FirstOrDefault<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.use_skill_max_count_in_deck));
      if (battleskillEffect != null)
      {
        int num1 = battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.use_skill_max_count_in_deck);
        bool flag = unit is BL.AIUnit;
        List<BL.Unit> unitList = BattleFuncs.forceUnits(BattleFuncs.env.getForceID(unit.originalUnit)).value;
        int num2 = 0;
        foreach (BL.Unit unit1 in unitList)
        {
          if (unit1.hasOugi)
          {
            BL.ISkillEffectListUnit skillEffectListUnit;
            if (!flag)
            {
              skillEffectListUnit = (BL.ISkillEffectListUnit) unit1;
            }
            else
            {
              skillEffectListUnit = (BL.ISkillEffectListUnit) BattleFuncs.env.getAIUnit(unit1);
              if (skillEffectListUnit == null)
                continue;
            }
            if (skillEffectListUnit.hasOugi && skillEffectListUnit.ougi.id == skill.id)
            {
              num2 += skillEffectListUnit.ougi.nowUseCount;
              if (num2 >= num1)
                return false;
            }
          }
        }
      }
      return true;
    }

    public static bool checkUseSEASkillMaxCountInDeck(BL.ISkillEffectListUnit unit, BL.Skill skill)
    {
      BattleskillEffect battleskillEffect = ((IEnumerable<BattleskillEffect>) skill.skill.Effects).FirstOrDefault<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.use_skill_max_count_in_deck));
      if (battleskillEffect != null)
      {
        int num1 = battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.use_skill_max_count_in_deck);
        List<BL.Unit> unitList = BattleFuncs.forceUnits(BattleFuncs.env.getForceID(unit.originalUnit)).value;
        int num2 = 0;
        foreach (BL.Unit unit1 in unitList)
        {
          if (unit1.hasSEASkill)
          {
            BL.Unit unit2 = unit1;
            if (unit2.hasSEASkill && unit2.SEASkill.id == skill.id)
            {
              num2 += unit2.SEASkill.nowUseCount;
              if (num2 >= num1)
                return false;
            }
          }
        }
      }
      return true;
    }

    public static bool isDontUseCommand(BL.ISkillEffectListUnit unit, BL.Skill skill)
    {
      int skill_id;
      if (skill == null)
      {
        skill_id = 0;
      }
      else
      {
        if (!BattleFuncs.checkUseSkillInvokeGameMode(unit, skill, false) || !BattleFuncs.checkUseCommandSkillMaxCountInDeck(unit, skill))
          return true;
        skill_id = skill.id;
      }
      return unit.skillEffects.Where(BattleskillEffectLogicEnum.do_not_use_command).Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id) == 0 || x.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id) == skill_id));
    }

    public static bool isDontUseOugi(BL.ISkillEffectListUnit unit, BL.Skill skill)
    {
      int skill_id;
      if (skill == null)
      {
        skill_id = 0;
      }
      else
      {
        if (!BattleFuncs.checkUseSkillInvokeGameMode(unit, skill, false) || !BattleFuncs.checkUseOugiSkillMaxCountInDeck(unit, skill))
          return true;
        skill_id = skill.id;
      }
      return unit.skillEffects.Where(BattleskillEffectLogicEnum.do_not_use_ougi).Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id) == 0 || x.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id) == skill_id));
    }

    public static bool isDontUseSEA(BL.ISkillEffectListUnit unit, BL.Skill skill)
    {
      return skill != null && (!BattleFuncs.checkUseSkillInvokeGameMode(unit, skill, false) || !BattleFuncs.checkUseSEASkillMaxCountInDeck(unit, skill)) || unit.skillEffects.Where(BattleskillEffectLogicEnum.do_not_use_command).Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.baseSkillId == 300001382 || x.baseSkillId == 300003113));
    }

    public static IEnumerable<BL.SkillEffect> getHealSwapEffects(
      BL.ISkillEffectListUnit unit,
      BL.Panel panel,
      BattleskillSkillType skillType = (BattleskillSkillType) 0,
      BL.ISkillEffectListUnit enemy = null)
    {
      return unit.skillEffects.Where(BattleskillEffectLogicEnum.heal_swap, (Func<BL.SkillEffect, bool>) (effect =>
      {
        int num = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_type);
        if (num != 0 && (BattleskillSkillType) num != skillType)
          return false;
        BattleFuncs.PackedSkillEffect packedSkillEffect = BattleFuncs.PackedSkillEffect.Create(effect);
        BattleskillEffect effect1 = effect.effect;
        if (BattleFuncs.isSealedSkillEffect(unit, effect) || effect1.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && effect1.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID || effect1.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) effect1.GetInt(BattleskillEffectLogicArgumentEnum.element) != unit.originalUnit.playerUnit.GetElement() || effect1.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id) != 0 && !unit.originalUnit.unit.HasSkillGroupId(effect1.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id)) || !packedSkillEffect.CheckLandTag(panel, unit is BL.AIUnit) || enemy != null && BattleFuncs.isEffectEnemyRangeAndInvalid(effect, unit, enemy))
          return false;
        return enemy == null || !BattleFuncs.isSkillsAndEffectsInvalid(unit, enemy);
      }));
    }

    public static int getHealValue(
      BL.ISkillEffectListUnit unit,
      BL.Panel panel,
      int healValue,
      BattleskillSkillType skillType = (BattleskillSkillType) 0,
      BL.ISkillEffectListUnit enemy = null)
    {
      if (healValue > 0)
      {
        if (BattleFuncs.getHealSwapEffects(unit, panel, skillType, enemy).Any<BL.SkillEffect>())
          return -healValue;
        if (!unit.CanHeal(skillType))
          return 0;
      }
      return healValue;
    }

    public static bool canWarpPanel(BL.ISkillEffectListUnit useUnit, int row, int column)
    {
      BL.Panel panel = BattleFuncs.getPanel(row, column);
      bool isAI = useUnit is BL.AIUnit;
      BL.UnitPosition up = BattleFuncs.iSkillEffectListUnitToUnitPosition(useUnit);
      return !useUnit.skillEffects.Where(BattleskillEffectLogicEnum.cant_warp_area, (Func<BL.SkillEffect, bool>) (effect =>
      {
        if (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.effect_target) != 0)
          return false;
        int num = BL.fieldDistance(up.row, up.column, row, column);
        return num >= effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_range) && num <= effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range) && !BattleFuncs.isSealedSkillEffect(useUnit, effect) && BattleFuncs.checkInvokeSkillEffect(BattleFuncs.PackedSkillEffect.Create(effect), useUnit, useUnit);
      })).Any<BL.SkillEffect>() && !panel.getSkillEffects(isAI).value.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (effect =>
      {
        if (effect.effect.effect_logic.Enum != BattleskillEffectLogicEnum.cant_warp_area)
          return false;
        BL.ISkillEffectListUnit iskillEffectListUnit = BattleFuncs.unitToISkillEffectListUnit(effect.parentUnit, isAI);
        if (useUnit == iskillEffectListUnit || iskillEffectListUnit.IsJumping)
          return false;
        int num1 = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.effect_target);
        if (num1 == 2)
          num1 = 0;
        int num2 = BattleFuncs.env.getForceID(useUnit.originalUnit) == BattleFuncs.env.getForceID(iskillEffectListUnit.originalUnit) ? 0 : 1;
        return num1 == num2 && !BattleFuncs.isSealedSkillEffect(iskillEffectListUnit, effect) && BattleFuncs.checkInvokeSkillEffect(BattleFuncs.PackedSkillEffect.Create(effect), iskillEffectListUnit, useUnit);
      })).Any<BL.SkillEffect>();
    }

    public static bool checkElement(BL.ISkillEffectListUnit unit, int element, bool isMatch = true)
    {
      if (element == 0)
        return true;
      if (unit == null)
        return false;
      if (element < 100)
      {
        if ((CommonElement) element == unit.originalUnit.playerUnit.GetElement())
          return isMatch;
      }
      else if ((CommonElement) (element - 100) == unit.originalUnit.playerUnit.GetElement() && unit.skillEffects.HasEnhancedElement())
        return isMatch;
      return !isMatch;
    }

    public static int useOvoPointInterference(BL.Unit unit, int point, bool isSample = false)
    {
      IEnumerable<BL.SkillEffect> skillEffects = unit.skillEffects.Where(BattleskillEffectLogicEnum.ovo_point_interference, (Func<BL.SkillEffect, bool>) (x =>
      {
        if (x.useRemain.HasValue && x.useRemain.Value <= 0)
          return false;
        BattleFuncs.PackedSkillEffect pse = BattleFuncs.PackedSkillEffect.Create(x);
        if (!BattleFuncs.checkInvokeSkillEffect(pse, (BL.ISkillEffectListUnit) unit))
          return false;
        Tuple<int, int> unitCell = BattleFuncs.getUnitCell(unit);
        return pse.CheckLandTag(BattleFuncs.getPanel(unitCell.Item1, unitCell.Item2), false) && (x.effect.GetInt(BattleskillEffectLogicArgumentEnum.cant_seal) != 0 || !BattleFuncs.isSealedSkillEffect((BL.ISkillEffectListUnit) unit, x));
      }));
      Decimal num1 = 1.0M;
      Decimal num2 = 1.0M;
      BL.SkillEffect[] skillEffectArray = new BL.SkillEffect[2];
      foreach (BL.SkillEffect skillEffect in BattleFuncs.gearSkillEffectFilter(unit, skillEffects))
      {
        Decimal num3 = (Decimal) skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage);
        if (num3 < 1.0M)
        {
          if (num3 < num1)
          {
            num1 = num3;
            skillEffectArray[0] = skillEffect;
          }
        }
        else if (num3 > 1.0M && num3 > num2)
        {
          num2 = num3;
          skillEffectArray[1] = skillEffect;
        }
      }
      if (!isSample)
      {
        for (int index = 0; index < 2; ++index)
        {
          BL.SkillEffect skillEffect1 = skillEffectArray[index];
          if (skillEffect1 != null && skillEffect1.useRemain.HasValue)
          {
            BL.SkillEffect skillEffect2 = skillEffect1;
            int? useRemain1 = skillEffect2.useRemain;
            skillEffect2.useRemain = useRemain1.HasValue ? new int?(useRemain1.GetValueOrDefault() - 1) : new int?();
            int? useRemain2 = skillEffect1.useRemain;
            int num4 = 0;
            if (useRemain2.GetValueOrDefault() == num4 & useRemain2.HasValue && skillEffect1.isLandTagEffect)
              unit.skillEffects.LandTagModified.commit();
          }
        }
      }
      point = (int) Math.Ceiling((Decimal) point * (num1 * num2));
      return point;
    }

    public static IEnumerable<BL.Skill> getReductCommandSkillUseTargetSkills(
      BL.ISkillEffectListUnit unit,
      int targetSkillId,
      int targetLogicId,
      BL.ISkillEffectListUnit useUnit,
      int useSkillId)
    {
      foreach (BL.Skill skillUseTargetSkill in (IEnumerable<BL.Skill>) ((IEnumerable<BL.Skill>) unit.skills).Where<BL.Skill>((Func<BL.Skill, bool>) (skill =>
      {
        if (skill.remain.HasValue)
        {
          int? remain = skill.remain;
          int num = 1;
          if (remain.GetValueOrDefault() >= num & remain.HasValue && (targetSkillId == 0 || skill.id == targetSkillId) && (targetLogicId == 0 || ((IEnumerable<BattleskillEffect>) skill.skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == (BattleskillEffectLogicEnum) targetLogicId && x.checkLevel(skill.level)))))
            return skill.skill.checkEnableUnit(unit);
        }
        return false;
      })).OrderBy<BL.Skill, int>((Func<BL.Skill, int>) (x => x.id)).ThenBy<BL.Skill, int>((Func<BL.Skill, int>) (x => x.remain.Value)))
      {
        if (useUnit == unit && useSkillId == skillUseTargetSkill.id)
        {
          int? remain = skillUseTargetSkill.remain;
          int num = 1;
          if (remain.GetValueOrDefault() <= num & remain.HasValue)
            continue;
        }
        yield return skillUseTargetSkill;
      }
    }

    public static bool cantInvokeDuelSkill(
      int type,
      BL.ISkillEffectListUnit myself,
      BL.ISkillEffectListUnit enemy,
      BL.Panel myselfPanel,
      BL.Panel enemyPanel)
    {
      bool isAI = myself is BL.AIUnit;
      Func<int, BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, BL.Panel, BL.Panel, bool> func = (Func<int, BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, BL.Panel, BL.Panel, bool>) ((effectTarget, effectUnit, targetUnit, effectPanel, targetPanel) =>
      {
        IEnumerable<BL.SkillEffect> source = effectUnit.skillEffects.Where(BattleFuncs.CantInvokeDuelSkillEnum[type]);
        return source.Any<BL.SkillEffect>() && (effectUnit != enemy || !BattleFuncs.isSkillsAndEffectsInvalid(enemy, myself)) && !BattleFuncs.isSkillsAndEffectsInvalid(myself, enemy) && source.Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x =>
        {
          if (x.effect.GetInt(BattleskillEffectLogicArgumentEnum.effect_target) != effectTarget || x.effect.GetInt(BattleskillEffectLogicArgumentEnum.cant_seal) == 0 && BattleFuncs.isSealedSkillEffect(effectUnit, x))
            return false;
          BattleFuncs.PackedSkillEffect packedSkillEffect = BattleFuncs.PackedSkillEffect.Create(x);
          return (!packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.family_id) || (packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) != 0 && !effectUnit.originalUnit.playerUnit.HasFamily((UnitFamily) packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.family_id)) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) != 0 && !targetUnit.originalUnit.playerUnit.HasFamily((UnitFamily) packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id)) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != effectUnit.originalUnit.unit.kind.ID || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != targetUnit.originalUnit.unit.kind.ID || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) != effectUnit.originalUnit.playerUnit.GetElement() || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != 0 && (CommonElement) packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != targetUnit.originalUnit.playerUnit.GetElement() || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) != 0 && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) != effectUnit.originalUnit.job.ID ? 0 : (packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == 0 ? 1 : (packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == targetUnit.originalUnit.job.ID ? 1 : 0))) != 0) && (!packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.group_large_id) || (packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_large_id) != 0 && (effectUnit.originalUnit.unitGroup == null || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_large_id) != effectUnit.originalUnit.unitGroup.group_large_category_id.ID) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_large_id) != 0 && (targetUnit.originalUnit.unitGroup == null || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_large_id) != targetUnit.originalUnit.unitGroup.group_large_category_id.ID) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_small_id) != 0 && (effectUnit.originalUnit.unitGroup == null || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_small_id) != effectUnit.originalUnit.unitGroup.group_small_category_id.ID) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_small_id) != 0 && (targetUnit.originalUnit.unitGroup == null || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_small_id) != targetUnit.originalUnit.unitGroup.group_small_category_id.ID) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_clothing_id) != 0 && (effectUnit.originalUnit.unitGroup == null || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_clothing_id) != effectUnit.originalUnit.unitGroup.group_clothing_category_id.ID && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_clothing_id) != effectUnit.originalUnit.unitGroup.group_clothing_category_id_2.ID) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_clothing_id) != 0 && (targetUnit.originalUnit.unitGroup == null || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_clothing_id) != targetUnit.originalUnit.unitGroup.group_clothing_category_id.ID && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_clothing_id) != targetUnit.originalUnit.unitGroup.group_clothing_category_id_2.ID) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_generation_id) != 0 && (effectUnit.originalUnit.unitGroup == null || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_generation_id) != effectUnit.originalUnit.unitGroup.group_generation_category_id.ID) ? 0 : (packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_generation_id) == 0 ? 1 : (targetUnit.originalUnit.unitGroup == null ? 0 : (packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_generation_id) == targetUnit.originalUnit.unitGroup.group_generation_category_id.ID ? 1 : 0)))) != 0) && packedSkillEffect.CheckLandTag(effectPanel, isAI) && packedSkillEffect.CheckTargetLandTag(targetPanel, isAI) && !BattleFuncs.isEffectEnemyRangeAndInvalid(x, myself, enemy);
        }));
      });
      return func(0, myself, enemy, myselfPanel, enemyPanel) || func(1, enemy, myself, enemyPanel, myselfPanel);
    }

    public static IEnumerable<BL.SkillEffect> getAnchorGroundEffects(
      BL.ISkillEffectListUnit myself,
      BL.ISkillEffectListUnit useUnit,
      BL.Panel myselfPanel,
      BL.Panel useUnitPanel,
      int attackType = 0)
    {
      return BattleFuncs.gearSkillEffectFilter(myself.originalUnit, myself.skillEffects.Where(BattleskillEffectLogicEnum.anchor_ground, (Func<BL.SkillEffect, bool>) (x =>
      {
        int num = x.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_type);
        if (num != 2)
        {
          if (BattleFuncs.env.getForceID(useUnit.originalUnit) == BattleFuncs.env.getForceID(myself.originalUnit))
          {
            if (num == 1)
              return false;
          }
          else if (num == 0)
            return false;
        }
        return x.GetCheckInvokeGeneric().DoCheck(myself, useUnit, attackType: attackType, unitPanel: myselfPanel, targetPanel: useUnitPanel, effect: x);
      })));
    }

    public static Decimal lotteryCallSkill(BL.CallSkillState param, int turn = 0)
    {
      if (param.isUsedCallSkill || param.skillId == 0)
        return 0M;
      Decimal callSkillPoint = param.callSkillPoint;
      bool sameCharacterJoin = param.isSameCharacterJoin;
      int intimateRank = param.intimateRank;
      int playerRank = param.playerRank;
      int skillId = param.skillId;
      CallGaugeRate[] callGaugeRateList = MasterData.CallGaugeRateList;
      CallIntimateGaugeRate[] intimateGaugeRateList = MasterData.CallIntimateGaugeRateList;
      CallSkillGaugeRate[] skillGaugeRateList = MasterData.CallSkillGaugeRateList;
      Decimal num1 = (Decimal) BattleFuncs.env.random.NextFloat();
      Decimal num2 = 0M;
      Decimal num3 = 0M;
      Decimal num4 = 0M;
      Decimal num5 = 0M;
      Decimal num6 = 0M;
      Decimal num7 = 0M;
      foreach (CallGaugeRate callGaugeRate in callGaugeRateList)
      {
        if (callGaugeRate.key == "base_add_value")
          num2 = (Decimal) callGaugeRate.value;
        else if (callGaugeRate.key == "base_rate")
          num3 = (Decimal) callGaugeRate.value;
        else if (callGaugeRate.key == "turn_count_rate")
          num4 = (Decimal) callGaugeRate.value;
        else if (callGaugeRate.key == "same_character_rate")
        {
          if (sameCharacterJoin)
            num5 = (Decimal) callGaugeRate.value;
        }
        else if (callGaugeRate.key == "player_rank_rate")
          num6 = (Decimal) callGaugeRate.value;
        else if (callGaugeRate.key == "random_rate")
          num7 = (Decimal) callGaugeRate.value;
      }
      Decimal num8 = 0M;
      foreach (CallIntimateGaugeRate intimateGaugeRate in intimateGaugeRateList)
      {
        if (intimateGaugeRate.rank_sum <= intimateRank)
          num8 = (Decimal) intimateGaugeRate.value;
      }
      Decimal num9 = 0M;
      foreach (CallSkillGaugeRate callSkillGaugeRate in skillGaugeRateList)
      {
        if (callSkillGaugeRate.skill_BattleskillSkill == skillId)
        {
          num9 = (Decimal) callSkillGaugeRate.value;
          break;
        }
      }
      Decimal num10 = 0M + num3 + num4 * (Decimal) turn + num5 + num8 + num6 * (Decimal) playerRank + num7 * num1 + num9;
      Decimal num11 = num2 * num10;
      Decimal num12 = callSkillPoint + num11;
      if (num12 > param.gauge_capacity)
        num12 = param.gauge_capacity;
      return num12;
    }

    public static void createAsterNodeCache(BL blenv)
    {
      List<BL.Panel> panelList = new List<BL.Panel>();
      for (int row = 0; row < blenv.getFieldHeight(); ++row)
      {
        for (int column = 0; column < blenv.getFieldWidth(); ++column)
        {
          if (blenv.getFieldPanel(row, column) != null)
            panelList.Add(blenv.getFieldPanel(row, column));
          else
            Debug.LogError((object) (" === ouch! フィールドデータがおかしい(" + (object) row + ", " + (object) column + ")"));
        }
      }
      for (int index = 0; index < 2; ++index)
      {
        Dictionary<UnitMoveType, BattleFuncs.AsterNode[]> dictionary = new Dictionary<UnitMoveType, BattleFuncs.AsterNode[]>();
        foreach (BL.UnitPosition unitPosition in blenv.unitPositions.value)
        {
          if (!unitPosition.unit.isFacility)
          {
            if (unitPosition.asterNodeCache == null)
              unitPosition.asterNodeCache = new BattleFuncs.AsterNode[2][];
            if (dictionary.ContainsKey(unitPosition.unit.job.move_type))
            {
              unitPosition.asterNodeCache[index] = dictionary[unitPosition.unit.job.move_type];
            }
            else
            {
              unitPosition.asterNodeCache[index] = BattleFuncs.createNodes((IEnumerable<BL.Panel>) panelList, unitPosition.unit, (BL.Panel) null, (BL.Panel) null, out int _, out int _, index == 1);
              dictionary[unitPosition.unit.job.move_type] = unitPosition.asterNodeCache[index];
            }
          }
        }
      }
    }

    private static int? GetTurnCount(int? colosseumTurn)
    {
      if (colosseumTurn.HasValue)
        return new int?(colosseumTurn.Value);
      return BattleFuncs.getPhaseState()?.absoluteTurnCount;
    }

    public static void applyDuelResultEffects(
      DuelResult duelResult,
      BL.ISkillEffectListUnit atk,
      BL.ISkillEffectListUnit def,
      BL env,
      Action<BL.Unit, Tuple<int, int>> swapHealDamageFunction = null)
    {
      BattleFuncs.apllyDuelRemoveSleepEffect(duelResult, atk, def);
      BattleFuncs.consumeSkillEffects(duelResult.turns, atk, def, false);
      bool isAI = atk is BL.AIUnit;
      BattleFuncs.ApplyChangeSkillEffects changeSkillEffects = new BattleFuncs.ApplyChangeSkillEffects(isAI);
      BL.ISkillEffectListUnit skillEffectListUnit = isAI ? (BL.ISkillEffectListUnit) env.getAIUnit(duelResult.moveUnit) : (BL.ISkillEffectListUnit) duelResult.moveUnit;
      BL.SkillEffect[] array = skillEffectListUnit.skillEffects.All().ToArray();
      foreach (BL.DuelTurn turn in duelResult.turns)
      {
        for (int index = 0; index < turn.investUnit.Length; ++index)
        {
          BL.ISkillEffectListUnit originalUnit1 = turn.investUnit[index];
          BL.Unit originalUnit2 = turn.investFrom[index].originalUnit;
          BL.Skill skill = new BL.Skill()
          {
            id = turn.investSkillIds[index]
          };
          BL.UnitPosition tup = originalUnit1 is BL.AIUnit ? originalUnit1 as BL.UnitPosition : env.getUnitPosition(originalUnit1.originalUnit);
          if (!isAI)
          {
            originalUnit1 = (BL.ISkillEffectListUnit) originalUnit1.originalUnit;
            tup = env.getUnitPosition(originalUnit1.originalUnit);
            originalUnit1.originalUnit.commit();
          }
          if (!originalUnit1.originalUnit.isDead)
          {
            changeSkillEffects.add(tup, originalUnit1, originalUnit1 == skillEffectListUnit);
            if (skill.skill.target_type == BattleskillTargetType.complex_single || skill.skill.target_type == BattleskillTargetType.complex_range)
            {
              bool flag = env.getForceID(originalUnit1.originalUnit) != env.getForceID(originalUnit2);
              foreach (BattleskillEffect effect in skill.skill.Effects)
              {
                if (effect.EffectLogic.Enum != BattleskillEffectLogicEnum.snake_venom)
                {
                  if (!effect.is_targer_enemy)
                  {
                    if (!flag)
                      originalUnit1.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, skill.skill, 1, investUnit: originalUnit2, investSkillId: turn.investFromSkillIds[index], investTurn: env.phaseState.absoluteTurnCount), checkEnableUnit: originalUnit1);
                  }
                  else if (flag)
                    originalUnit1.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, skill.skill, 1, investUnit: originalUnit2, investSkillId: turn.investFromSkillIds[index], investTurn: env.phaseState.absoluteTurnCount), checkEnableUnit: originalUnit1);
                }
              }
            }
            else
            {
              foreach (BattleskillEffect effect in skill.skill.Effects)
              {
                if (effect.EffectLogic.Enum != BattleskillEffectLogicEnum.snake_venom)
                  originalUnit1.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, skill.skill, 1, investUnit: originalUnit2, investSkillId: turn.investFromSkillIds[index], investTurn: env.phaseState.absoluteTurnCount), checkEnableUnit: originalUnit1);
              }
            }
          }
        }
      }
      changeSkillEffects.execute();
      foreach (BL.SkillEffect skillEffect in skillEffectListUnit.skillEffects.All().Except<BL.SkillEffect>((IEnumerable<BL.SkillEffect>) array))
        skillEffect.moveDistance = new int?();
      int hp1 = atk.hp;
      int hp2 = def.hp;
      int num1 = duelResult.sumSwapHealDamage(true);
      int num2 = duelResult.sumSwapHealDamage(false);
      atk.hp -= num1;
      def.hp -= num2;
      if (swapHealDamageFunction == null)
        return;
      if (num1 > 0)
        swapHealDamageFunction(atk.originalUnit, Tuple.Create<int, int>(hp1, atk.hp));
      if (num2 <= 0)
        return;
      swapHealDamageFunction(def.originalUnit, Tuple.Create<int, int>(hp2, def.hp));
    }

    public static HashSet<BL.ISkillEffectListUnit> applyDuelSkillEffects(
      DuelResult duelResult,
      BL.ISkillEffectListUnit atk,
      BL.ISkillEffectListUnit def,
      BL env,
      Dictionary<BL.ISkillEffectListUnit, List<BL.SkillEffect>> removeSkillEffects,
      Action<BL.ISkillEffectListUnit, int> addHpFunction = null,
      Action<BL.ISkillEffectListUnit, int> subHpFunction = null,
      Action<BL.ISkillEffectListUnit, int> penetrateHpFunction = null,
      Action<BL.ISkillEffectListUnit, int> rangeAttackHpFunction = null,
      Action<BL.ISkillEffectListUnit, int> damageShareHpFunction = null,
      Action<BattleskillSkill, BL.Unit, Dictionary<BL.Unit, Tuple<int, int>>, float?> snakeFunction = null,
      Dictionary<Tuple<BL.Unit, BattleskillSkill>, List<BL.Unit>> lifeAbsorbSkillTarget = null,
      Dictionary<Tuple<BL.Unit, BattleskillSkill>, List<BL.Unit>> curseReflectionSkillTarget = null,
      Dictionary<Tuple<BL.Unit, BattleskillSkill>, Tuple<List<BL.Unit>, float>> penetrateSkillTarget = null,
      Dictionary<Tuple<BL.Unit, BattleskillSkill>, List<BL.Unit>> rangeAttackSkillTarget = null,
      Dictionary<BL.Unit, Dictionary<BattleskillSkill, List<BL.Unit>>> damageShareSkillTarget = null,
      Dictionary<Tuple<BL.Unit, BattleskillSkill>, List<BL.Unit>> stealSkillTarget = null,
      Dictionary<Tuple<BL.Unit, BattleskillSkill>, List<BL.Unit>> removeSkillEffectSkillTarget = null,
      Dictionary<Tuple<BL.Unit, BattleskillSkill>, List<BL.Panel>> afterDuelInvestLandTagSkillTarget = null,
      List<Tuple<BL.Unit, BattleskillSkill>> afterDuelInvestLandTagKeys = null)
    {
      if (duelResult.disableDuelSkillEffects)
        return new HashSet<BL.ISkillEffectListUnit>();
      HashSet<BL.ISkillEffectListUnit> deads = new HashSet<BL.ISkillEffectListUnit>();
      if (!duelResult.isHeal)
      {
        BattleFuncs.applyDuelDamageSkillEffects(duelResult, atk, def, env, removeSkillEffects, deads, addHpFunction, subHpFunction, penetrateHpFunction, rangeAttackHpFunction, damageShareHpFunction, snakeFunction, lifeAbsorbSkillTarget, curseReflectionSkillTarget, penetrateSkillTarget, rangeAttackSkillTarget, damageShareSkillTarget, stealSkillTarget, removeSkillEffectSkillTarget, afterDuelInvestLandTagSkillTarget, afterDuelInvestLandTagKeys);
        HashSet<BL.ISkillEffectListUnit> isInvokedSnakeVenom = new HashSet<BL.ISkillEffectListUnit>();
        Dictionary<BL.ISkillEffectListUnit, BattleskillEffect> snakeVenomUnitEffect = new Dictionary<BL.ISkillEffectListUnit, BattleskillEffect>();
        BL.Skill snakeVenomSkill = (BL.Skill) null;
        BattleFuncs.mapDuelSkillEffects(duelResult, atk, def, env, (Action<Tuple<BL.DuelTurn, BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, BL.Skill>>) null, (Action<Tuple<BL.DuelTurn, BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, BL.Skill, bool, bool>>) (data =>
        {
          foreach (BattleskillEffect effect in data.Item5.skill.Effects)
          {
            if (effect.EffectLogic.Enum == BattleskillEffectLogicEnum.snake_venom && (!data.Item6 || effect.is_targer_enemy == data.Item7))
            {
              snakeVenomUnitEffect[data.Item4] = effect;
              snakeVenomSkill = data.Item5;
            }
          }
        }), (Action<Tuple<BL.DuelTurn, BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, IEnumerable<BL.UnitPosition>, BL.Skill, bool, bool>>) (data =>
        {
          foreach (BL.UnitPosition unitPosition in data.Item4)
          {
            BL.ISkillEffectListUnit key = unitPosition is BL.ISkillEffectListUnit ? unitPosition as BL.ISkillEffectListUnit : (BL.ISkillEffectListUnit) unitPosition.unit;
            foreach (BattleskillEffect effect in data.Item5.skill.Effects)
            {
              if (effect.EffectLogic.Enum == BattleskillEffectLogicEnum.snake_venom && (!data.Item6 || effect.is_targer_enemy == data.Item7))
              {
                if (key != data.Item3)
                  snakeVenomUnitEffect[key] = effect;
                snakeVenomSkill = data.Item5;
              }
            }
          }
        }), (Action<BL.DuelTurn, BL.ISkillEffectListUnit>) ((turn, myself) =>
        {
          if (!isInvokedSnakeVenom.Contains(myself) && snakeVenomSkill != null && snakeVenomUnitEffect.Count >= 1)
          {
            Dictionary<BL.Unit, Tuple<int, int>> dictionary = snakeFunction != null ? new Dictionary<BL.Unit, Tuple<int, int>>() : (Dictionary<BL.Unit, Tuple<int, int>>) null;
            isInvokedSnakeVenom.Add(myself);
            int num = 0;
            foreach (BL.DuelTurn turn1 in duelResult.turns)
            {
              if (turn1.isAtackker == turn.isAtackker && ((IEnumerable<BL.Skill>) turn1.invokeDuelSkills).Any<BL.Skill>((Func<BL.Skill, bool>) (skill => skill == snakeVenomSkill)))
                num = Judgement.CalcMaximumLongToInt((long) num + (long) turn1.realDamage);
            }
            foreach (BL.ISkillEffectListUnit key in snakeVenomUnitEffect.Keys)
            {
              int hp = key.hp;
              BattleskillEffect effect = snakeVenomUnitEffect[key];
              if (hp >= 1)
              {
                int minHp = effect.GetInt(BattleskillEffectLogicArgumentEnum.min_hp);
                int damage1 = Judgement.CalcMaximumLongToInt((long) effect.GetInt(BattleskillEffectLogicArgumentEnum.venom_value) + (long) Mathf.CeilToInt((float) key.originalUnit.parameter.Hp * effect.GetFloat(BattleskillEffectLogicArgumentEnum.venom_hp_percentage)) + (long) Judgement.CalcMaximumCeilToIntValue((Decimal) num * (Decimal) effect.GetFloat(BattleskillEffectLogicArgumentEnum.venom_damage_percentage)));
                int damage2 = BattleFuncs.applyFieldDamageFluctuate(atk, def, myself, key, effect, damage1);
                BattleFuncs.applyHpDamage(atk, def, myself, key, damage2, minHp, deads, (Action<BL.ISkillEffectListUnit, int>) null);
                if (dictionary != null)
                  dictionary[key as BL.Unit] = new Tuple<int, int>(hp, key.hp);
              }
            }
            if (dictionary != null)
              snakeFunction(snakeVenomSkill.skill, myself as BL.Unit, dictionary, new float?());
          }
          snakeVenomUnitEffect.Clear();
          snakeVenomSkill = (BL.Skill) null;
        }), true);
      }
      return deads;
    }

    public static void applyDuelResultEffectsLate(
      DuelResult duelResult,
      BL.ISkillEffectListUnit atk,
      BL.ISkillEffectListUnit def,
      BL env,
      Dictionary<BL.ISkillEffectListUnit, List<BL.SkillEffect>> removeSkillEffects)
    {
      int num = atk is BL.AIUnit ? 1 : 0;
      BattleFuncs.ApplyChangeSkillEffects changeSkillEffects = new BattleFuncs.ApplyChangeSkillEffects(num != 0);
      BL.ISkillEffectListUnit skillEffectListUnit = num != 0 ? (BL.ISkillEffectListUnit) env.getAIUnit(duelResult.moveUnit) : (BL.ISkillEffectListUnit) duelResult.moveUnit;
      changeSkillEffects.add(atk, atk == skillEffectListUnit);
      changeSkillEffects.add(def, def == skillEffectListUnit);
      BattleFuncs.consumeSkillEffectsLate(atk, def, duelResult.attackAttackStatus, duelResult.defenseAttackStatus, false, removeSkillEffects);
      changeSkillEffects.execute();
    }

    private static void apllyDuelRemoveSleepEffect(
      DuelResult duelResult,
      BL.ISkillEffectListUnit atk,
      BL.ISkillEffectListUnit def)
    {
      if (atk != null && ((IEnumerable<BL.DuelTurn>) duelResult.turns).Any<BL.DuelTurn>((Func<BL.DuelTurn, bool>) (x => !x.isAtackker && x.dispDamage >= 1)))
        atk.skillEffects.RemoveEffect(1000418, BattleFuncs.env, atk);
      if (def == null || !((IEnumerable<BL.DuelTurn>) duelResult.turns).Any<BL.DuelTurn>((Func<BL.DuelTurn, bool>) (x => x.isAtackker && x.dispDamage >= 1)))
        return;
      def.skillEffects.RemoveEffect(1000418, BattleFuncs.env, atk);
    }

    private static void mapDuelSkillEffects(
      DuelResult duelResult,
      BL.ISkillEffectListUnit atk,
      BL.ISkillEffectListUnit def,
      BL env,
      Action<Tuple<BL.DuelTurn, BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, BL.Skill>> mapAlimentFunction,
      Action<Tuple<BL.DuelTurn, BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, BL.Skill, bool, bool>> mapGiveOneFunction,
      Action<Tuple<BL.DuelTurn, BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, IEnumerable<BL.UnitPosition>, BL.Skill, bool, bool>> mapGiveMultiFunction,
      Action<BL.DuelTurn, BL.ISkillEffectListUnit> turnFunction,
      bool isCheckSingleRange)
    {
      foreach (BL.DuelTurn turn in duelResult.turns)
      {
        BL.ISkillEffectListUnit myself;
        BL.ISkillEffectListUnit enemy;
        if (turn.isAtackker)
        {
          myself = atk;
          enemy = def;
        }
        else
        {
          myself = def;
          enemy = atk;
        }
        if (mapAlimentFunction != null && turn.invokeAilmentSkills != null)
        {
          BL.ISkillEffectListUnit skillEffectListUnit = turn.isAtackker ? def : atk;
          foreach (BL.Skill invokeAilmentSkill in turn.invokeAilmentSkills)
            mapAlimentFunction(new Tuple<BL.DuelTurn, BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, BL.Skill>(turn, myself, enemy, skillEffectListUnit, invokeAilmentSkill));
        }
        if (mapGiveOneFunction != null && mapGiveMultiFunction != null && turn.invokeGiveSkills != null)
        {
          BL.ForceID[] forceIds1 = new BL.ForceID[1]
          {
            env.getForceID(myself.originalUnit)
          };
          BL.ForceID[] forceIds2 = new BL.ForceID[1]
          {
            env.getForceID(enemy.originalUnit)
          };
          int[] range = new int[2];
          foreach (BL.Skill invokeGiveSkill in turn.invokeGiveSkills)
          {
            range[0] = invokeGiveSkill.skill.min_range;
            range[1] = invokeGiveSkill.skill.max_range;
            BL.ISkillEffectListUnit unit = myself;
            if (((IEnumerable<BattleskillEffect>) invokeGiveSkill.skill.Effects).Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.snake_venom)).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.GetInt(BattleskillEffectLogicArgumentEnum.is_range_from_enemy) != 0)))
              unit = enemy;
            switch (invokeGiveSkill.skill.target_type)
            {
              case BattleskillTargetType.myself:
              case BattleskillTargetType.player_single:
                if (!isCheckSingleRange || range[0] == 0 && range[1] == 0 || BattleFuncs.getRangeTargets(unit, range, forceIds1, true).Any<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (up => (up is BL.ISkillEffectListUnit ? up as BL.ISkillEffectListUnit : (BL.ISkillEffectListUnit) up.unit) == myself)))
                {
                  mapGiveOneFunction(new Tuple<BL.DuelTurn, BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, BL.Skill, bool, bool>(turn, myself, enemy, myself, invokeGiveSkill, false, false));
                  break;
                }
                break;
              case BattleskillTargetType.player_range:
                List<BL.UnitPosition> list1 = BattleFuncs.getRangeTargets(unit, range, forceIds1, true).ToList<BL.UnitPosition>();
                mapGiveMultiFunction(new Tuple<BL.DuelTurn, BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, IEnumerable<BL.UnitPosition>, BL.Skill, bool, bool>(turn, myself, enemy, (IEnumerable<BL.UnitPosition>) list1, invokeGiveSkill, false, false));
                break;
              case BattleskillTargetType.enemy_single:
                if (!isCheckSingleRange || range[0] == 0 && range[1] == 0 || BattleFuncs.getRangeTargets(unit, range, forceIds2, true).Any<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (up => (up is BL.ISkillEffectListUnit ? up as BL.ISkillEffectListUnit : (BL.ISkillEffectListUnit) up.unit) == enemy)))
                {
                  mapGiveOneFunction(new Tuple<BL.DuelTurn, BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, BL.Skill, bool, bool>(turn, myself, enemy, enemy, invokeGiveSkill, false, false));
                  break;
                }
                break;
              case BattleskillTargetType.enemy_range:
                List<BL.UnitPosition> list2 = BattleFuncs.getRangeTargets(unit, range, forceIds2, true).ToList<BL.UnitPosition>();
                mapGiveMultiFunction(new Tuple<BL.DuelTurn, BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, IEnumerable<BL.UnitPosition>, BL.Skill, bool, bool>(turn, myself, enemy, (IEnumerable<BL.UnitPosition>) list2, invokeGiveSkill, false, true));
                break;
              case BattleskillTargetType.complex_single:
                bool flag1 = true;
                bool flag2 = true;
                if (isCheckSingleRange && (range[0] != 0 || range[1] != 0))
                {
                  flag1 = BattleFuncs.getRangeTargets(unit, range, forceIds2, true).Any<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (up => (up is BL.ISkillEffectListUnit ? up as BL.ISkillEffectListUnit : (BL.ISkillEffectListUnit) up.unit) == enemy));
                  flag2 = BattleFuncs.getRangeTargets(unit, range, forceIds1, true).Any<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (up => (up is BL.ISkillEffectListUnit ? up as BL.ISkillEffectListUnit : (BL.ISkillEffectListUnit) up.unit) == myself));
                }
                if (flag1)
                  mapGiveOneFunction(new Tuple<BL.DuelTurn, BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, BL.Skill, bool, bool>(turn, myself, enemy, enemy, invokeGiveSkill, true, true));
                if (flag2)
                {
                  mapGiveOneFunction(new Tuple<BL.DuelTurn, BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, BL.Skill, bool, bool>(turn, myself, enemy, myself, invokeGiveSkill, true, false));
                  break;
                }
                break;
              case BattleskillTargetType.complex_range:
                List<BL.UnitPosition> list3 = BattleFuncs.getRangeTargets(unit, range, forceIds1, true).ToList<BL.UnitPosition>();
                List<BL.UnitPosition> list4 = BattleFuncs.getRangeTargets(unit, range, forceIds2, true).ToList<BL.UnitPosition>();
                mapGiveMultiFunction(new Tuple<BL.DuelTurn, BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, IEnumerable<BL.UnitPosition>, BL.Skill, bool, bool>(turn, myself, enemy, (IEnumerable<BL.UnitPosition>) list4, invokeGiveSkill, true, true));
                mapGiveMultiFunction(new Tuple<BL.DuelTurn, BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, IEnumerable<BL.UnitPosition>, BL.Skill, bool, bool>(turn, myself, enemy, (IEnumerable<BL.UnitPosition>) list3, invokeGiveSkill, true, false));
                break;
            }
          }
        }
        if (turnFunction != null)
          turnFunction(turn, myself);
      }
    }

    private static void applyDuelDamageSkillEffects(
      DuelResult duelResult,
      BL.ISkillEffectListUnit atk,
      BL.ISkillEffectListUnit def,
      BL env,
      Dictionary<BL.ISkillEffectListUnit, List<BL.SkillEffect>> removeSkillEffects,
      HashSet<BL.ISkillEffectListUnit> deads,
      Action<BL.ISkillEffectListUnit, int> addHpFunction,
      Action<BL.ISkillEffectListUnit, int> subHpFunction,
      Action<BL.ISkillEffectListUnit, int> penetrateHpFunction,
      Action<BL.ISkillEffectListUnit, int> rangeAttackHpFunction,
      Action<BL.ISkillEffectListUnit, int> damageShareHpFunction,
      Action<BattleskillSkill, BL.Unit, Dictionary<BL.Unit, Tuple<int, int>>, float?> snakeFunction,
      Dictionary<Tuple<BL.Unit, BattleskillSkill>, List<BL.Unit>> lifeAbsorbSkillTarget,
      Dictionary<Tuple<BL.Unit, BattleskillSkill>, List<BL.Unit>> curseReflectionSkillTarget,
      Dictionary<Tuple<BL.Unit, BattleskillSkill>, Tuple<List<BL.Unit>, float>> penetrateSkillTarget,
      Dictionary<Tuple<BL.Unit, BattleskillSkill>, List<BL.Unit>> rangeAttackSkillTarget,
      Dictionary<BL.Unit, Dictionary<BattleskillSkill, List<BL.Unit>>> damageShareSkillTarget,
      Dictionary<Tuple<BL.Unit, BattleskillSkill>, List<BL.Unit>> stealSkillTarget,
      Dictionary<Tuple<BL.Unit, BattleskillSkill>, List<BL.Unit>> removeSkillEffectSkillTarget,
      Dictionary<Tuple<BL.Unit, BattleskillSkill>, List<BL.Panel>> afterDuelInvestLandTagSkillTarget,
      List<Tuple<BL.Unit, BattleskillSkill>> afterDuelInvestLandTagKeys = null)
    {
      bool isAI = atk is BL.AIUnit;
      bool isInvokedAmbush = duelResult.moveUnit != atk.originalUnit;
      BL.ISkillEffectListUnit myself = !isInvokedAmbush ? atk : def;
      BL.ISkillEffectListUnit enemy = !isInvokedAmbush ? def : atk;
      int is_attack = 1;
      BL.ForceID[] forceIdArray1 = new BL.ForceID[1]
      {
        env.getForceID(myself.originalUnit)
      };
      BL.ForceID[] forceIdArray2 = new BL.ForceID[1]
      {
        env.getForceID(enemy.originalUnit)
      };
      int[] iWork = new int[1];
      Dictionary<BL.ISkillEffectListUnit, int> lifeAbsorbTargetHeal = new Dictionary<BL.ISkillEffectListUnit, int>();
      BL.ISkillEffectListUnit[] curseReflectionInvokeUnit = new BL.ISkillEffectListUnit[2];
      Dictionary<BL.ISkillEffectListUnit, int>[] curseReflectionTargetDamage = new Dictionary<BL.ISkillEffectListUnit, int>[2];
      Dictionary<BL.ISkillEffectListUnit, int>[] curseReflectionTargetSwapHeal = new Dictionary<BL.ISkillEffectListUnit, int>[2];
      List<BattleFuncs.SnakeVenomDamageData> snakeVenomDamage = new List<BattleFuncs.SnakeVenomDamageData>();
      BL.ISkillEffectListUnit[] skillEffectListUnitArray = new BL.ISkillEffectListUnit[2];
      Dictionary<BL.ISkillEffectListUnit, int>[] dictionaryArray = new Dictionary<BL.ISkillEffectListUnit, int>[2];
      int minHp1 = int.MaxValue;
      BL.ISkillEffectListUnit[] rangeAttackInvokeUnit = new BL.ISkillEffectListUnit[2];
      Dictionary<BL.ISkillEffectListUnit, int>[] rangeAttackTargetDamage = new Dictionary<BL.ISkillEffectListUnit, int>[2];
      int rangeAttackMinHp = int.MaxValue;
      BattleFuncs.ApplyChangeSkillEffects applyChangeSkillEffects = (BattleFuncs.ApplyChangeSkillEffects) null;
      BL.ISkillEffectListUnit skillEffectListUnit1 = atk;
      BL.ISkillEffectListUnit enemy1 = def;
      for (int index = 0; index < 2; ++index)
      {
        List<BL.SkillEffect> source = new List<BL.SkillEffect>();
        foreach (BL.SkillEffect effect in skillEffectListUnit1.skillEffects.Where(BattleskillEffectLogicEnum.after_duel_remove_skilleffect))
        {
          if (effect.useRemain.HasValue)
          {
            int? useRemain1 = effect.useRemain;
            int num = 0;
            if (!(useRemain1.GetValueOrDefault() <= num & useRemain1.HasValue))
            {
              BL.SkillEffect skillEffect = effect;
              int? useRemain2 = skillEffect.useRemain;
              skillEffect.useRemain = useRemain2.HasValue ? new int?(useRemain2.GetValueOrDefault() - 1) : new int?();
            }
            else
              continue;
          }
          if (!BattleFuncs.isSkillsAndEffectsInvalid(skillEffectListUnit1, enemy1) && !BattleFuncs.isSealedSkillEffect(skillEffectListUnit1, effect))
            source.Add(effect);
        }
        if (source.Any<BL.SkillEffect>())
        {
          foreach (BL.SkillEffect skillEffect in BattleFuncs.gearSkillEffectFilter(skillEffectListUnit1.originalUnit, (IEnumerable<BL.SkillEffect>) source))
          {
            int[] range = new int[2]
            {
              skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_range),
              skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range)
            };
            BL.Unit unit = skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.is_range_from_enemy) == 0 ? skillEffectListUnit1.originalUnit : enemy1.originalUnit;
            List<BL.UnitPosition> targets = BattleFuncs.getTargets(unit, range, env.getTargetForce(skillEffectListUnit1.originalUnit, false), BL.Unit.TargetAttribute.all, isAI, nonFacility: true);
            int num = skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.range_form);
            foreach (BL.UnitPosition unitPosition in targets)
            {
              switch (num)
              {
                case 1:
                  Tuple<int, int> unitCell1 = BattleFuncs.getUnitCell(unit, isAI);
                  if (unitPosition.row == unitCell1.Item1 || unitPosition.column == unitCell1.Item2)
                    break;
                  continue;
                case 2:
                  Tuple<int, int> unitCell2 = BattleFuncs.getUnitCell(unit, isAI);
                  if (Mathf.Abs(unitPosition.row - unitCell2.Item1) != Mathf.Abs(unitPosition.column - unitCell2.Item2))
                    continue;
                  break;
              }
              BL.ISkillEffectListUnit iskillEffectListUnit = BattleFuncs.unitPositionToISkillEffectListUnit(unitPosition);
              if (applyChangeSkillEffects == null)
                applyChangeSkillEffects = new BattleFuncs.ApplyChangeSkillEffects(isAI);
              applyChangeSkillEffects.add(unitPosition, iskillEffectListUnit, iskillEffectListUnit == myself);
              BL.SkillEffect[] collection = BattleFuncs.removeSkillEffect(skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.logic_id), skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id), skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_type), skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.invest_type), skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.ailment_group_id), skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.range_effect_remove_flag), iskillEffectListUnit, applyChangeSkillEffects, skillEffectListUnit1, skillEffect.effect, myself);
              if (!removeSkillEffects.ContainsKey(iskillEffectListUnit))
                removeSkillEffects[iskillEffectListUnit] = new List<BL.SkillEffect>();
              removeSkillEffects[iskillEffectListUnit].AddRange((IEnumerable<BL.SkillEffect>) collection);
              if (removeSkillEffectSkillTarget != null)
              {
                Tuple<BL.Unit, BattleskillSkill> key = Tuple.Create<BL.Unit, BattleskillSkill>(skillEffectListUnit1 as BL.Unit, skillEffect.baseSkill);
                if (!removeSkillEffectSkillTarget.ContainsKey(key))
                  removeSkillEffectSkillTarget[key] = new List<BL.Unit>();
                if (!removeSkillEffectSkillTarget[key].Contains(iskillEffectListUnit as BL.Unit))
                  removeSkillEffectSkillTarget[key].Add(iskillEffectListUnit as BL.Unit);
              }
            }
          }
        }
        BL.ISkillEffectListUnit skillEffectListUnit2 = skillEffectListUnit1;
        skillEffectListUnit1 = enemy1;
        enemy1 = skillEffectListUnit2;
      }
      applyChangeSkillEffects?.execute();
      BL.UnitPosition unitPosition1 = BattleFuncs.iSkillEffectListUnitToUnitPosition(myself);
      BL.UnitPosition unitPosition2 = BattleFuncs.iSkillEffectListUnitToUnitPosition(enemy);
      for (int index = 0; index < 2; ++index)
      {
        BL.ISkillEffectListUnit skillEffectListUnit3 = myself;
        myself = enemy;
        enemy = skillEffectListUnit3;
        BL.UnitPosition unitPosition3 = unitPosition1;
        unitPosition1 = unitPosition2;
        unitPosition2 = unitPosition3;
        List<BL.SkillEffect> source = new List<BL.SkillEffect>();
        foreach (BL.SkillEffect effect in myself.skillEffects.Where(BattleskillEffectLogicEnum.after_duel_invest_land_tag))
        {
          if (effect.useRemain.HasValue)
          {
            int? useRemain3 = effect.useRemain;
            int num = 0;
            if (!(useRemain3.GetValueOrDefault() <= num & useRemain3.HasValue))
            {
              BL.SkillEffect skillEffect = effect;
              int? useRemain4 = skillEffect.useRemain;
              skillEffect.useRemain = useRemain4.HasValue ? new int?(useRemain4.GetValueOrDefault() - 1) : new int?();
            }
            else
              continue;
          }
          if (!BattleFuncs.isSkillsAndEffectsInvalid(myself, enemy) && !BattleFuncs.isSealedSkillEffect(myself, effect))
            source.Add(effect);
        }
        if (source.Any<BL.SkillEffect>())
        {
          Dictionary<int, List<BL.SkillEffect>> dictionary = new Dictionary<int, List<BL.SkillEffect>>();
          bool flag1 = index == 1;
          if (isInvokedAmbush)
            flag1 = !flag1;
          foreach (BL.SkillEffect skillEffect in BattleFuncs.gearSkillEffectFilter(myself.originalUnit, (IEnumerable<BL.SkillEffect>) source))
          {
            BL.SkillEffect effect = skillEffect;
            bool flag2 = false;
            for (int key = duelResult.turns.Length - 1; key >= 0; --key)
            {
              BL.DuelTurn turn = duelResult.turns[key];
              if (turn.isAtackker == flag1 && turn.invokeGiveSkills != null && ((IEnumerable<BL.Skill>) turn.invokeGiveSkills).Any<BL.Skill>((Func<BL.Skill, bool>) (skill => ((IEnumerable<BattleskillEffect>) skill.skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (e => e.skill.ID == effect.baseSkillId)))))
              {
                if (!dictionary.ContainsKey(key))
                  dictionary[key] = new List<BL.SkillEffect>();
                dictionary[key].Add(effect);
                flag2 = true;
                break;
              }
            }
            if (!flag2)
            {
              if (!dictionary.ContainsKey(-1))
                dictionary[-1] = new List<BL.SkillEffect>();
              dictionary[-1].Add(effect);
            }
          }
          foreach (int key1 in (IEnumerable<int>) dictionary.Keys.OrderBy<int, int>((Func<int, int>) (x => x)))
          {
            foreach (IEnumerable<BL.SkillEffect> skillEffects1 in (IEnumerable<IGrouping<int, BL.SkillEffect>>) dictionary[key1].OrderByDescending<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (x => x.effectId)).GroupBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (x => x.baseSkillId)).OrderBy<IGrouping<int, BL.SkillEffect>, int>((Func<IGrouping<int, BL.SkillEffect>, int>) (x => MasterData.BattleskillSkill[x.Key].weight)).ThenByDescending<IGrouping<int, BL.SkillEffect>, int>((Func<IGrouping<int, BL.SkillEffect>, int>) (x => x.Key)))
            {
              foreach (BL.SkillEffect skillEffect in skillEffects1)
              {
                int num1 = skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_range);
                int num2 = skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range);
                BL.UnitPosition unitPosition4 = skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.is_range_from_enemy) == 0 ? unitPosition1 : unitPosition2;
                int num3 = skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.range_form);
                int key2 = skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id);
                if (key2 != 0 && MasterData.BattleskillSkill.ContainsKey(key2))
                {
                  BL.Skill skill = new BL.Skill()
                  {
                    id = key2
                  };
                  BattleskillEffect effect = ((IEnumerable<BattleskillEffect>) skill.skill.Effects).FirstOrDefault<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.invest_land_tag));
                  if (effect != null)
                  {
                    foreach (BL.Panel rangePanel in BattleFuncs.getRangePanels(unitPosition4.row, unitPosition4.column, new int[2]
                    {
                      num1,
                      num2
                    }))
                    {
                      switch (num3)
                      {
                        case 1:
                          if (unitPosition4.row == rangePanel.row || unitPosition4.column == rangePanel.column)
                            break;
                          continue;
                        case 2:
                          if (Mathf.Abs(unitPosition4.row - rangePanel.row) != Mathf.Abs(unitPosition4.column - rangePanel.column))
                            continue;
                          break;
                      }
                      BL.ClassValue<List<BL.SkillEffect>> skillEffects2 = rangePanel.getSkillEffects(isAI);
                      if (skillEffects2.value.RemoveAll((Predicate<BL.SkillEffect>) (x => x.effect.EffectLogic.Enum == BattleskillEffectLogicEnum.invest_land_tag)) >= 1)
                        skillEffects2.commit();
                      rangePanel.addSkillEffect(BL.SkillEffect.FromMasterData(effect, skill.skill, 1, investUnit: myself.originalUnit, investSkillId: skillEffect.baseSkillId, investTurn: env.phaseState.absoluteTurnCount), myself);
                      if (!isAI)
                      {
                        BL.UnitPosition[] fieldUnits = env.getFieldUnits(rangePanel.row, rangePanel.column);
                        if (fieldUnits != null)
                        {
                          foreach (BL.UnitPosition unitPosition5 in fieldUnits)
                            unitPosition5.unit.skillEffects.LandTagModified.commit();
                        }
                        if (env.fieldCurrent.value == rangePanel)
                          env.fieldCurrent.commit();
                      }
                      if (afterDuelInvestLandTagSkillTarget != null)
                      {
                        Tuple<BL.Unit, BattleskillSkill> key3 = Tuple.Create<BL.Unit, BattleskillSkill>(myself as BL.Unit, skillEffect.baseSkill);
                        if (!afterDuelInvestLandTagSkillTarget.ContainsKey(key3))
                        {
                          afterDuelInvestLandTagSkillTarget[key3] = new List<BL.Panel>();
                          afterDuelInvestLandTagKeys.Add(key3);
                        }
                        if (!afterDuelInvestLandTagSkillTarget[key3].Contains(rangePanel))
                          afterDuelInvestLandTagSkillTarget[key3].Add(rangePanel);
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
      BL.UnitPosition unitPosition6 = BattleFuncs.iSkillEffectListUnitToUnitPosition(myself);
      BL.UnitPosition unitPosition7 = BattleFuncs.iSkillEffectListUnitToUnitPosition(enemy);
      BL.UnitPosition[] unitPositionArray1 = new BL.UnitPosition[2]
      {
        unitPosition6,
        unitPosition7
      };
      int[] numArray1 = new int[2];
      int[] numArray2 = new int[2];
      for (int index = 0; index < 2; ++index)
      {
        List<BL.SkillEffect> skillEffectList = new List<BL.SkillEffect>();
        foreach (BL.SkillEffect skillEffect1 in myself.skillEffects.Where(BattleskillEffectLogicEnum.passive_keep_away))
        {
          if (skillEffect1.useRemain.HasValue)
          {
            int? useRemain5 = skillEffect1.useRemain;
            int num4 = 0;
            if (!(useRemain5.GetValueOrDefault() <= num4 & useRemain5.HasValue))
            {
              BL.SkillEffect skillEffect2 = skillEffect1;
              int? useRemain6 = skillEffect2.useRemain;
              skillEffect2.useRemain = useRemain6.HasValue ? new int?(useRemain6.GetValueOrDefault() - 1) : new int?();
              int? useRemain7 = skillEffect1.useRemain;
              int num5 = 0;
              if (useRemain7.GetValueOrDefault() == num5 & useRemain7.HasValue && !isAI && skillEffect1.isLandTagEffect)
                myself.skillEffects.LandTagModified.commit();
            }
            else
              continue;
          }
          if (!BattleFuncs.isSkillsAndEffectsInvalid(myself, enemy) && !BattleFuncs.isSealedSkillEffect(myself, skillEffect1) && BattleFuncs.PackedSkillEffect.Create(skillEffect1).CheckLandTag(BattleFuncs.getPanel(unitPosition6.row, unitPosition6.column), isAI))
            skillEffectList.Add(skillEffect1);
        }
        if (skillEffectList.Count > 0 && !unitPosition7.unit.isFacility && !BattleFuncs.isSkillsAndEffectsInvalid(myself, enemy) && !BattleFuncs.getAnchorGroundEffects(enemy, myself, BattleFuncs.getPanel(unitPosition7.row, unitPosition7.column), BattleFuncs.getPanel(unitPosition6.row, unitPosition6.column), (index ^ 1) + 1).Any<BL.SkillEffect>())
        {
          int penetrateCount = 0;
          foreach (BL.SkillEffect skillEffect in BattleFuncs.gearSkillEffectFilter(myself.originalUnit, (IEnumerable<BL.SkillEffect>) skillEffectList))
            penetrateCount += skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.range);
          if (penetrateCount >= 1)
          {
            Tuple<int, int, int> tuple = BattleFuncs.getPenetratePosition(unitPosition6.row, unitPosition6.column, unitPosition7.row, unitPosition7.column, penetrateCount).OrderByDescending<Tuple<int, int, int>, int>((Func<Tuple<int, int, int>, int>) (x => x.Item3)).FirstOrDefault<Tuple<int, int, int>>();
            if (tuple != null)
            {
              numArray1[index ^ 1] += tuple.Item1 - unitPosition7.row;
              numArray2[index ^ 1] += tuple.Item2 - unitPosition7.column;
            }
          }
        }
        List<BL.SkillEffect> source1 = new List<BL.SkillEffect>();
        List<BL.SkillEffect> source2 = new List<BL.SkillEffect>();
        foreach (BL.SkillEffect skillEffect3 in myself.skillEffects.Where(BattleskillEffectLogicEnum.slash_back))
        {
          int? nullable;
          if (skillEffect3.useRemain.HasValue)
          {
            int? useRemain8 = skillEffect3.useRemain;
            int num6 = 0;
            if (!(useRemain8.GetValueOrDefault() <= num6 & useRemain8.HasValue))
            {
              BL.SkillEffect skillEffect4 = skillEffect3;
              int? useRemain9 = skillEffect4.useRemain;
              skillEffect4.useRemain = useRemain9.HasValue ? new int?(useRemain9.GetValueOrDefault() - 1) : new int?();
              nullable = skillEffect3.useRemain;
              int num7 = 0;
              if (nullable.GetValueOrDefault() == num7 & nullable.HasValue && !isAI && skillEffect3.isLandTagEffect)
                myself.skillEffects.LandTagModified.commit();
            }
            else
              continue;
          }
          if (!BattleFuncs.isSkillsAndEffectsInvalid(myself, enemy) && !BattleFuncs.isSealedSkillEffect(myself, skillEffect3))
          {
            BattleFuncs.PackedSkillEffect packedSkillEffect = BattleFuncs.PackedSkillEffect.Create(skillEffect3);
            BattleFuncs.PackedSkillEffect pse = packedSkillEffect;
            BL.ISkillEffectListUnit unit = myself;
            BL.ISkillEffectListUnit target = enemy;
            nullable = new int?();
            int? colosseumTurn = nullable;
            nullable = new int?();
            int? unitHp = nullable;
            nullable = new int?();
            int? targetHp = nullable;
            if (BattleFuncs.checkInvokeSkillEffect(pse, unit, target, colosseumTurn, unitHp: unitHp, targetHp: targetHp) && packedSkillEffect.CheckLandTag(BattleFuncs.getPanel(unitPosition6.row, unitPosition6.column), isAI))
            {
              int num = skillEffect3.effect.GetInt(BattleskillEffectLogicArgumentEnum.range);
              if (num > 0)
                source1.Add(skillEffect3);
              else if (num < 0)
                source2.Add(skillEffect3);
            }
          }
        }
        bool flag = false;
        if (source1.Any<BL.SkillEffect>() || source2.Any<BL.SkillEffect>())
          flag = !BattleFuncs.isSkillsAndEffectsInvalid(myself, enemy) && BattleFuncs.getAnchorGroundEffects(myself, myself, BattleFuncs.getPanel(unitPosition6.row, unitPosition6.column), BattleFuncs.getPanel(unitPosition6.row, unitPosition6.column), index + 1).Any<BL.SkillEffect>();
        if (source1.Any<BL.SkillEffect>() && !flag)
        {
          int penetrateCount = 0;
          foreach (BL.SkillEffect skillEffect in BattleFuncs.gearSkillEffectFilter(myself.originalUnit, (IEnumerable<BL.SkillEffect>) source1))
            penetrateCount += skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.range);
          if (penetrateCount >= 1)
          {
            Tuple<int, int, int> tuple = BattleFuncs.getPenetratePosition(unitPosition7.row, unitPosition7.column, unitPosition6.row, unitPosition6.column, penetrateCount).OrderByDescending<Tuple<int, int, int>, int>((Func<Tuple<int, int, int>, int>) (x => x.Item3)).FirstOrDefault<Tuple<int, int, int>>();
            if (tuple != null)
            {
              numArray1[index] += tuple.Item1 - unitPosition6.row;
              numArray2[index] += tuple.Item2 - unitPosition6.column;
            }
          }
        }
        if (source2.Any<BL.SkillEffect>() && !flag)
        {
          int penetrateCount = 0;
          foreach (BL.SkillEffect skillEffect in BattleFuncs.gearSkillEffectFilter(myself.originalUnit, (IEnumerable<BL.SkillEffect>) source2))
            penetrateCount -= skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.range);
          if (penetrateCount >= 1)
          {
            Tuple<int, int, int> tuple = BattleFuncs.getPenetratePosition(unitPosition6.row, unitPosition6.column, unitPosition7.row, unitPosition7.column, penetrateCount).OrderByDescending<Tuple<int, int, int>, int>((Func<Tuple<int, int, int>, int>) (x => x.Item3)).FirstOrDefault<Tuple<int, int, int>>();
            if (tuple != null)
            {
              numArray1[index] += tuple.Item1 - unitPosition6.row;
              numArray2[index] += tuple.Item2 - unitPosition6.column;
            }
          }
        }
        BL.ISkillEffectListUnit skillEffectListUnit4 = myself;
        myself = enemy;
        enemy = skillEffectListUnit4;
        BL.UnitPosition unitPosition8 = unitPosition6;
        unitPosition6 = unitPosition7;
        unitPosition7 = unitPosition8;
      }
      for (int index = 0; index < 2; ++index)
      {
        if (numArray1[index] != 0 || numArray2[index] != 0)
        {
          List<Tuple<int, int, int>> penetratePosition = BattleFuncs.getPenetratePosition(unitPositionArray1[index].row, unitPositionArray1[index].column, unitPositionArray1[index].row + numArray1[index], unitPositionArray1[index].column + numArray2[index], 0, true);
          BL.Unit[] ignoreUnits = new BL.Unit[0];
          foreach (Tuple<int, int, int> tuple in (IEnumerable<Tuple<int, int, int>>) penetratePosition.OrderByDescending<Tuple<int, int, int>, int>((Func<Tuple<int, int, int>, int>) (x => x.Item3)))
          {
            if (BattleFuncs.isResetPositionOK(unitPositionArray1[index].unit, tuple.Item1, tuple.Item2, unitPositionArray1[index].unit.parameter.Move, (IEnumerable<BL.Unit>) ignoreUnits, isAI))
            {
              RecoveryUtility.resetPosition(unitPositionArray1[index], tuple.Item1, tuple.Item2, env, true, index == 0);
              break;
            }
          }
        }
      }
      BL.UnitPosition myselfUp = BattleFuncs.iSkillEffectListUnitToUnitPosition(myself);
      BL.UnitPosition targetUp = BattleFuncs.iSkillEffectListUnitToUnitPosition(enemy);
      for (int index = 0; index < 2; ++index)
      {
        List<BL.SkillEffect> source = new List<BL.SkillEffect>();
        foreach (BL.SkillEffect skillEffect5 in myself.skillEffects.Where(BattleskillEffectLogicEnum.passive_steal))
        {
          int? nullable;
          if (skillEffect5.useRemain.HasValue)
          {
            int? useRemain10 = skillEffect5.useRemain;
            int num8 = 0;
            if (!(useRemain10.GetValueOrDefault() <= num8 & useRemain10.HasValue))
            {
              BL.SkillEffect skillEffect6 = skillEffect5;
              int? useRemain11 = skillEffect6.useRemain;
              skillEffect6.useRemain = useRemain11.HasValue ? new int?(useRemain11.GetValueOrDefault() - 1) : new int?();
              nullable = skillEffect5.useRemain;
              int num9 = 0;
              if (nullable.GetValueOrDefault() == num9 & nullable.HasValue && !isAI && skillEffect5.isLandTagEffect)
                myself.skillEffects.LandTagModified.commit();
            }
            else
              continue;
          }
          if (!BattleFuncs.isSkillsAndEffectsInvalid(myself, enemy) && !BattleFuncs.isSealedSkillEffect(myself, skillEffect5))
          {
            BattleFuncs.PackedSkillEffect packedSkillEffect = BattleFuncs.PackedSkillEffect.Create(skillEffect5);
            BattleFuncs.PackedSkillEffect pse = packedSkillEffect;
            BL.ISkillEffectListUnit unit = myself;
            BL.ISkillEffectListUnit target = enemy;
            nullable = new int?();
            int? colosseumTurn = nullable;
            nullable = new int?();
            int? unitHp = nullable;
            nullable = new int?();
            int? targetHp = nullable;
            if (BattleFuncs.checkInvokeSkillEffect(pse, unit, target, colosseumTurn, unitHp: unitHp, targetHp: targetHp) && packedSkillEffect.CheckLandTag(BattleFuncs.getPanel(myselfUp.row, myselfUp.column), isAI))
              source.Add(skillEffect5);
          }
        }
        if (source.Any<BL.SkillEffect>() && !targetUp.unit.isFacility)
        {
          foreach (BL.SkillEffect skillEffect in BattleFuncs.gearSkillEffectFilter(myself.originalUnit, (IEnumerable<BL.SkillEffect>) source))
          {
            bool flag = BattleFuncs.executeSteal(myself, enemy, skillEffect.effect, myselfUp, targetUp, isAI);
            if (stealSkillTarget != null & flag)
            {
              Tuple<BL.Unit, BattleskillSkill> key = Tuple.Create<BL.Unit, BattleskillSkill>(myself as BL.Unit, skillEffect.baseSkill);
              if (!stealSkillTarget.ContainsKey(key))
                stealSkillTarget[key] = new List<BL.Unit>();
              if (!stealSkillTarget[key].Contains(enemy as BL.Unit))
                stealSkillTarget[key].Add(enemy as BL.Unit);
            }
          }
        }
        BL.ISkillEffectListUnit skillEffectListUnit5 = myself;
        myself = enemy;
        enemy = skillEffectListUnit5;
        BL.UnitPosition unitPosition9 = myselfUp;
        myselfUp = targetUp;
        targetUp = unitPosition9;
      }
      List<Tuple<BL.ISkillEffectListUnit, int, BL.SkillEffect, BL.ISkillEffectListUnit>> source3 = new List<Tuple<BL.ISkillEffectListUnit, int, BL.SkillEffect, BL.ISkillEffectListUnit>>();
      int? useRemain12;
      foreach (BL.DuelTurn turn in duelResult.turns)
      {
        int count = turn.damageShareUnit.Count;
        for (int index = 0; index < count; ++index)
        {
          BL.ISkillEffectListUnit originalUnit = turn.damageShareUnit[index];
          int damage = turn.damageShareDamage[index];
          BL.UseSkillEffect effect = turn.damageShareSkillEffect[index];
          if (!isAI)
            originalUnit = (BL.ISkillEffectListUnit) originalUnit.originalUnit;
          List<BL.SkillEffect> source4 = new List<BL.SkillEffect>((IEnumerable<BL.SkillEffect>) originalUnit.skillEffects.All());
          if (removeSkillEffects.ContainsKey(originalUnit))
            source4.AddRange((IEnumerable<BL.SkillEffect>) removeSkillEffects[originalUnit]);
          BL.SkillEffect[] array = source4.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x =>
          {
            if (x.effectId == effect.effectEffectId && x.baseSkillLevel == effect.effectBaseSkillLevel)
            {
              if (x.turnRemain.HasValue || effect.effectTurnRemain != -1)
              {
                int? turnRemain = x.turnRemain;
                int effectTurnRemain = effect.effectTurnRemain;
                if (!(turnRemain.GetValueOrDefault() == effectTurnRemain & turnRemain.HasValue))
                  goto label_8;
              }
              if (x.unit == (BL.Unit) null && effect.effectUnitIndex == -1)
                return true;
              return x.unit != (BL.Unit) null && effect.effectUnitIndex != -1 && x.unit.index == effect.effectUnitIndex && x.unit.isPlayerForce == effect.GetEffectUnitIsPlayerControl(false);
            }
label_8:
            return false;
          })).ToArray<BL.SkillEffect>();
          BattleFuncs.applyHpDamage(atk, def, turn.isAtackker ? atk : def, originalUnit, damage, 0, deads, damageShareHpFunction);
          BL.SkillEffect skillEffect7 = ((IEnumerable<BL.SkillEffect>) array).FirstOrDefault<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x =>
          {
            int? useRemain13 = x.useRemain;
            int num = 1;
            return useRemain13.GetValueOrDefault() >= num & useRemain13.HasValue;
          }));
          if (skillEffect7 != null)
          {
            BL.SkillEffect skillEffect8 = skillEffect7;
            useRemain12 = skillEffect8.useRemain;
            int? nullable = useRemain12;
            skillEffect8.useRemain = nullable.HasValue ? new int?(nullable.GetValueOrDefault() - 1) : new int?();
            useRemain12 = skillEffect7.useRemain;
            int num = 0;
            if (useRemain12.GetValueOrDefault() == num & useRemain12.HasValue && !isAI && skillEffect7.isLandTagEffect)
              originalUnit.skillEffects.LandTagModified.commit();
          }
          BL.SkillEffect skillEffect9 = ((IEnumerable<BL.SkillEffect>) array).FirstOrDefault<BL.SkillEffect>();
          if (skillEffect9 != null)
          {
            source3.Add(Tuple.Create<BL.ISkillEffectListUnit, int, BL.SkillEffect, BL.ISkillEffectListUnit>(originalUnit, damage, skillEffect9, turn.isAtackker ? atk : def));
            if (damageShareSkillTarget != null)
            {
              BL.Unit key = originalUnit as BL.Unit;
              if (!damageShareSkillTarget.ContainsKey(key))
                damageShareSkillTarget[key] = new Dictionary<BattleskillSkill, List<BL.Unit>>();
              if (!damageShareSkillTarget[key].ContainsKey(skillEffect9.baseSkill))
                damageShareSkillTarget[key][skillEffect9.baseSkill] = new List<BL.Unit>();
            }
          }
        }
      }
      if (source3.Any<Tuple<BL.ISkillEffectListUnit, int, BL.SkillEffect, BL.ISkillEffectListUnit>>())
      {
        Dictionary<BL.ISkillEffectListUnit, int> dictionary = new Dictionary<BL.ISkillEffectListUnit, int>();
        foreach (Tuple<BL.ISkillEffectListUnit, int, BL.SkillEffect, BL.ISkillEffectListUnit> tuple in source3)
        {
          BL.ISkillEffectListUnit unit = tuple.Item1;
          int num10 = tuple.Item2;
          BL.SkillEffect headerEffect = tuple.Item3;
          BL.ISkillEffectListUnit enemyUnit = tuple.Item4;
          if (!removeSkillEffects.ContainsKey(unit) || !removeSkillEffects[unit].Contains(headerEffect))
          {
            BattleFuncs.PackedSkillEffect packedSkillEffect = BattleFuncs.PackedSkillEffect.Create(headerEffect);
            if (packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.reflect_min_range))
            {
              int num11 = packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.reflect_target_gear_kind_id);
              if (num11 == 0 || num11 == enemyUnit.originalUnit.unit.kind.ID)
              {
                int num12 = packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.reflect_target_element);
                if (num12 == 0 || (CommonElement) num12 == enemyUnit.originalUnit.playerUnit.GetElement())
                {
                  if (!dictionary.ContainsKey(unit))
                    dictionary[unit] = unit.hp;
                  switch (packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.reflect_condition))
                  {
                    case 1:
                      if (dictionary[unit] > 0)
                        break;
                      continue;
                    case 2:
                      if (dictionary[unit] >= 1)
                        continue;
                      break;
                  }
                  int num13 = Judgement.CalcMaximumLongToInt((long) packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.reflect_value) + (long) Judgement.CalcMaximumCeilToIntValue((Decimal) num10 * (Decimal) packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.reflect_percentage)));
                  if (num13 != 0)
                  {
                    int[] range = new int[2]
                    {
                      packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.reflect_min_range),
                      packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.reflect_max_range)
                    };
                    int num14 = packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.reflect_is_range_from_enemy);
                    BL.ISkillEffectListUnit forceFromUnit = num14 == 0 ? unit : enemyUnit;
                    int num15 = packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.reflect_effect_target);
                    List<BL.UnitPosition> source5;
                    switch (num15)
                    {
                      case 0:
                      case 2:
                        source5 = BattleFuncs.getTargets(forceFromUnit.originalUnit, range, BattleFuncs.getForceIDArray(env.getForceID(unit.originalUnit)), BL.Unit.TargetAttribute.all, isAI, nonFacility: true);
                        if (num15 == 2)
                        {
                          source5 = source5.Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => BattleFuncs.unitPositionToISkillEffectListUnit(x) == unit)).ToList<BL.UnitPosition>();
                          break;
                        }
                        break;
                      default:
                        source5 = BattleFuncs.getTargets(forceFromUnit.originalUnit, range, BattleFuncs.getForceIDArray(env.getForceID(enemyUnit.originalUnit)), BL.Unit.TargetAttribute.all, isAI, nonFacility: true);
                        if (num15 == 3)
                        {
                          source5 = source5.Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => BattleFuncs.unitPositionToISkillEffectListUnit(x) == enemyUnit)).ToList<BL.UnitPosition>();
                          break;
                        }
                        break;
                    }
                    List<BL.Unit> collection = new List<BL.Unit>();
                    foreach (BL.UnitPosition up in source5)
                    {
                      BL.ISkillEffectListUnit iskillEffectListUnit = BattleFuncs.unitPositionToISkillEffectListUnit(up);
                      if (iskillEffectListUnit.hp > 0)
                      {
                        int damage1 = num13;
                        int num16 = 0;
                        if (damage1 < 0)
                        {
                          num16 = BattleFuncs.getHealValue(iskillEffectListUnit, BattleFuncs.getPanel(up.row, up.column), -damage1, headerEffect.baseSkill.skill_type);
                          if (num16 < 0)
                            damage1 = 0;
                          else
                            num16 = 0;
                        }
                        int damage2 = BattleFuncs.applyFieldDamageFluctuate(atk, def, unit, iskillEffectListUnit, num14 == 0, damage1, forceFromUnit);
                        if (damage2 < 0 && !iskillEffectListUnit.CanHeal(headerEffect.baseSkill.skill_type))
                          damage2 = 0;
                        collection.Add(iskillEffectListUnit.originalUnit);
                        if (damage2 >= 0)
                        {
                          BattleFuncs.applyHpDamage(atk, def, unit, iskillEffectListUnit, damage2, 0, deads, damageShareHpFunction);
                        }
                        else
                        {
                          int hp = iskillEffectListUnit.hp;
                          iskillEffectListUnit.hp -= damage2;
                          if (damageShareHpFunction != null)
                            damageShareHpFunction(iskillEffectListUnit, hp);
                        }
                        if (iskillEffectListUnit.hp >= 1)
                        {
                          int hp = iskillEffectListUnit.hp;
                          iskillEffectListUnit.hp += num16;
                          if (damageShareHpFunction != null)
                            damageShareHpFunction(iskillEffectListUnit, hp);
                          if (iskillEffectListUnit.hp <= 0 && iskillEffectListUnit != atk && iskillEffectListUnit != def)
                            deads.Add(iskillEffectListUnit);
                        }
                      }
                    }
                    damageShareSkillTarget?[unit as BL.Unit][headerEffect.baseSkill].AddRange((IEnumerable<BL.Unit>) collection);
                  }
                }
              }
            }
          }
        }
      }
      AttackStatus attackStatus1 = !isInvokedAmbush ? duelResult.attackAttackStatus : duelResult.defenseAttackStatus;
      AttackStatus attackStatus2 = !isInvokedAmbush ? duelResult.defenseAttackStatus : duelResult.attackAttackStatus;
      for (is_attack = 1; is_attack <= 2; is_attack++)
      {
        List<BL.SkillEffect> skillEffectList = new List<BL.SkillEffect>();
        foreach (BL.SkillEffect effect in myself.skillEffects.Where(BattleskillEffectLogicEnum.penetrate))
        {
          useRemain12 = effect.useRemain;
          if (useRemain12.HasValue)
          {
            useRemain12 = effect.useRemain;
            int num = 0;
            if (!(useRemain12.GetValueOrDefault() <= num & useRemain12.HasValue))
            {
              BL.SkillEffect skillEffect = effect;
              useRemain12 = skillEffect.useRemain;
              int? nullable = useRemain12;
              skillEffect.useRemain = nullable.HasValue ? new int?(nullable.GetValueOrDefault() - 1) : new int?();
            }
            else
              continue;
          }
          if (!BattleFuncs.isSkillsAndEffectsInvalid(myself, enemy) && !BattleFuncs.isSealedSkillEffect(myself, effect))
          {
            int num = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.attack_type);
            if ((num != 1 || attackStatus1 != null && !attackStatus1.isMagic) && (num != 2 || attackStatus1 != null && attackStatus1.isMagic))
              skillEffectList.Add(effect);
          }
        }
        foreach (BL.SkillEffect skillEffect in BattleFuncs.gearSkillEffectFilter(myself.originalUnit, (IEnumerable<BL.SkillEffect>) skillEffectList))
        {
          BL.SkillEffect effect = skillEffect;
          if (skillEffectListUnitArray[is_attack - 1] == null)
          {
            skillEffectListUnitArray[is_attack - 1] = myself;
            dictionaryArray[is_attack - 1] = new Dictionary<BL.ISkillEffectListUnit, int>();
          }
          int num17 = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_hp);
          if (num17 < minHp1)
            minHp1 = num17;
          BL.UnitPosition unitPosition10 = BattleFuncs.iSkillEffectListUnitToUnitPosition(enemy);
          int num18 = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_range);
          int penetrateCount = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range);
          List<BL.UnitPosition> unitPositionList = new List<BL.UnitPosition>();
          unitPositionList.Add(BattleFuncs.iSkillEffectListUnitToUnitPosition(myself));
          if (effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.enable_combi) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.enable_combi) == 1)
          {
            bool flag = is_attack == 1;
            if (isInvokedAmbush)
              flag = !flag;
            for (int index = duelResult.turns.Length - 1; index >= 0; --index)
            {
              BL.DuelTurn turn = duelResult.turns[index];
              if (turn.isAtackker == flag && turn.invokeGiveSkills != null && ((IEnumerable<BL.Skill>) turn.invokeGiveSkills).Any<BL.Skill>((Func<BL.Skill, bool>) (skill => ((IEnumerable<BattleskillEffect>) skill.skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (e => e.skill.ID == effect.baseSkillId)))))
              {
                if (turn.attackerCombiUnit != null)
                {
                  foreach (BL.ISkillEffectListUnit skillEffectListUnit6 in turn.attackerCombiUnit)
                  {
                    if (skillEffectListUnit6 != null)
                    {
                      BL.ISkillEffectListUnit unit = skillEffectListUnit6;
                      if (!isAI)
                        unit = (BL.ISkillEffectListUnit) unit.originalUnit;
                      unitPositionList.Add(BattleFuncs.iSkillEffectListUnitToUnitPosition(unit));
                    }
                  }
                  break;
                }
                break;
              }
            }
          }
          foreach (BL.UnitPosition up1 in unitPositionList)
          {
            List<Tuple<int, int, int>> penetratePosition = BattleFuncs.getPenetratePosition(up1.row, up1.column, unitPosition10.row, unitPosition10.column, penetrateCount);
            BL.ISkillEffectListUnit iskillEffectListUnit1 = BattleFuncs.unitPositionToISkillEffectListUnit(up1);
            foreach (Tuple<int, int, int> tuple in penetratePosition)
            {
              if (tuple.Item3 >= num18)
              {
                BL.UnitPosition[] unitPositionArray2 = isAI ? BattleFuncs.getFieldUnitsAI(tuple.Item1, tuple.Item2) : env.getFieldUnits(tuple.Item1, tuple.Item2);
                if (unitPositionArray2 != null)
                {
                  foreach (BL.UnitPosition up2 in unitPositionArray2)
                  {
                    if (((IEnumerable<BL.ForceID>) forceIdArray2).Contains<BL.ForceID>(env.getForceID(up2.unit)) && !up2.unit.isFacility)
                    {
                      BL.Panel panel = BattleFuncs.getPanel(up2.row, up2.column);
                      if (effect.effect.GetPackedSkillEffect().CheckTargetLandTag(panel, isAI))
                      {
                        BL.ISkillEffectListUnit iskillEffectListUnit2 = BattleFuncs.unitPositionToISkillEffectListUnit(up2);
                        int damage = BattleFuncs.calcAttackDamage(iskillEffectListUnit1, iskillEffectListUnit2, effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_attack), effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_decrease), effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_damage), false);
                        if (damage != 0)
                        {
                          int num19 = BattleFuncs.applyFieldDamageFluctuate(atk, def, myself, iskillEffectListUnit2, true, damage);
                          int index = is_attack - 1;
                          if (!dictionaryArray[index].ContainsKey(iskillEffectListUnit2))
                            dictionaryArray[index][iskillEffectListUnit2] = 0;
                          dictionaryArray[index][iskillEffectListUnit2] = Judgement.CalcMaximumLongToInt((long) dictionaryArray[index][iskillEffectListUnit2] + (long) num19);
                          if (penetrateSkillTarget != null)
                          {
                            Tuple<BL.Unit, BattleskillSkill> key = new Tuple<BL.Unit, BattleskillSkill>(iskillEffectListUnit1 as BL.Unit, effect.baseSkill);
                            if (!penetrateSkillTarget.ContainsKey(key))
                            {
                              float num20 = Mathf.Atan2((float) (unitPosition10.column - up1.column), (float) (unitPosition10.row - up1.row)) * 57.29578f;
                              penetrateSkillTarget[key] = new Tuple<List<BL.Unit>, float>(new List<BL.Unit>(), num20);
                            }
                            if (!penetrateSkillTarget[key].Item1.Contains(iskillEffectListUnit2 as BL.Unit))
                              penetrateSkillTarget[key].Item1.Add(iskillEffectListUnit2 as BL.Unit);
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
        BL.ISkillEffectListUnit skillEffectListUnit7 = myself;
        myself = enemy;
        enemy = skillEffectListUnit7;
        BL.ForceID[] forceIdArray3 = forceIdArray1;
        forceIdArray1 = forceIdArray2;
        forceIdArray2 = forceIdArray3;
        AttackStatus attackStatus3 = attackStatus1;
        attackStatus1 = attackStatus2;
        attackStatus2 = attackStatus3;
      }
      Dictionary<BattleskillEffectLogicEnum, Tuple<Func<BL.SkillEffect, bool>, Func<BL.SkillEffect, bool>, Action<BL.SkillEffect, BL.ISkillEffectListUnit>, Action<BL.SkillEffect>>> dictionary1 = new Dictionary<BattleskillEffectLogicEnum, Tuple<Func<BL.SkillEffect, bool>, Func<BL.SkillEffect, bool>, Action<BL.SkillEffect, BL.ISkillEffectListUnit>, Action<BL.SkillEffect>>>()
      {
        {
          BattleskillEffectLogicEnum.life_absorb,
          new Tuple<Func<BL.SkillEffect, bool>, Func<BL.SkillEffect, bool>, Action<BL.SkillEffect, BL.ISkillEffectListUnit>, Action<BL.SkillEffect>>((Func<BL.SkillEffect, bool>) (effect =>
          {
            if (effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.element))
            {
              int num = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element);
              if (num != 0 && (CommonElement) num != myself.originalUnit.playerUnit.GetElement())
                return false;
            }
            if (effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.target_element))
            {
              int num = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element);
              if (num != 0 && (CommonElement) num != enemy.originalUnit.playerUnit.GetElement())
                return false;
            }
            if (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.condition) == 1)
              return enemy.hp <= 0;
            return effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.condition) != 2 && enemy.hp <= 0 || myself.hp <= 0;
          }), (Func<BL.SkillEffect, bool>) null, (Action<BL.SkillEffect, BL.ISkillEffectListUnit>) ((effect, unit) =>
          {
            if (!lifeAbsorbTargetHeal.ContainsKey(unit))
              lifeAbsorbTargetHeal[unit] = 0;
            int healValue = effect.baseSkillLevel + effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.value) + (int) Math.Ceiling((Decimal) unit.originalUnit.parameter.Hp * (Decimal) effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage));
            BL.UnitPosition unitPosition11 = BattleFuncs.iSkillEffectListUnitToUnitPosition(unit);
            lifeAbsorbTargetHeal[unit] += BattleFuncs.getHealValue(unit, BattleFuncs.getPanel(unitPosition11.row, unitPosition11.column), healValue, effect.baseSkill.skill_type);
            if (lifeAbsorbSkillTarget == null)
              return;
            Tuple<BL.Unit, BattleskillSkill> key = new Tuple<BL.Unit, BattleskillSkill>(myself as BL.Unit, effect.baseSkill);
            if (!lifeAbsorbSkillTarget.ContainsKey(key))
              lifeAbsorbSkillTarget[key] = new List<BL.Unit>();
            if (lifeAbsorbSkillTarget[key].Contains(unit as BL.Unit))
              return;
            lifeAbsorbSkillTarget[key].Add(unit as BL.Unit);
          }), (Action<BL.SkillEffect>) null)
        },
        {
          BattleskillEffectLogicEnum.range_attack,
          new Tuple<Func<BL.SkillEffect, bool>, Func<BL.SkillEffect, bool>, Action<BL.SkillEffect, BL.ISkillEffectListUnit>, Action<BL.SkillEffect>>((Func<BL.SkillEffect, bool>) (effect =>
          {
            if (effect.useRemain.HasValue)
            {
              int? useRemain14 = effect.useRemain;
              int num = 0;
              if (useRemain14.GetValueOrDefault() <= num & useRemain14.HasValue)
                return false;
            }
            return true;
          }), (Func<BL.SkillEffect, bool>) (effect =>
          {
            if (rangeAttackInvokeUnit[is_attack - 1] == null)
            {
              rangeAttackInvokeUnit[is_attack - 1] = myself;
              rangeAttackTargetDamage[is_attack - 1] = new Dictionary<BL.ISkillEffectListUnit, int>();
            }
            int num = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_hp);
            if (num < rangeAttackMinHp)
              rangeAttackMinHp = num;
            return true;
          }), (Action<BL.SkillEffect, BL.ISkillEffectListUnit>) ((effect, unit) =>
          {
            int damage = BattleFuncs.calcAttackDamage(myself, unit, effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_attack), effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_decrease), effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_damage), false);
            if (damage == 0)
              return;
            int num = BattleFuncs.applyFieldDamageFluctuate(atk, def, myself, unit, effect.effect, damage);
            int index = is_attack - 1;
            if (!rangeAttackTargetDamage[index].ContainsKey(unit))
              rangeAttackTargetDamage[index][unit] = 0;
            rangeAttackTargetDamage[index][unit] = Judgement.CalcMaximumLongToInt((long) rangeAttackTargetDamage[index][unit] + (long) num);
            if (rangeAttackSkillTarget == null)
              return;
            Tuple<BL.Unit, BattleskillSkill> key = new Tuple<BL.Unit, BattleskillSkill>(myself as BL.Unit, effect.baseSkill);
            if (!rangeAttackSkillTarget.ContainsKey(key))
              rangeAttackSkillTarget[key] = new List<BL.Unit>();
            if (rangeAttackSkillTarget[key].Contains(unit as BL.Unit))
              return;
            rangeAttackSkillTarget[key].Add(unit as BL.Unit);
          }), (Action<BL.SkillEffect>) (effect =>
          {
            if (!effect.useRemain.HasValue)
              return;
            int? useRemain15 = effect.useRemain;
            int num21 = 1;
            if (!(useRemain15.GetValueOrDefault() >= num21 & useRemain15.HasValue))
              return;
            BL.SkillEffect skillEffect = effect;
            useRemain15 = skillEffect.useRemain;
            int? nullable = useRemain15;
            skillEffect.useRemain = nullable.HasValue ? new int?(nullable.GetValueOrDefault() - 1) : new int?();
            useRemain15 = effect.useRemain;
            int num22 = 0;
            if (!(useRemain15.GetValueOrDefault() == num22 & useRemain15.HasValue) || isAI || !effect.isLandTagEffect)
              return;
            myself.skillEffects.LandTagModified.commit();
          }))
        },
        {
          BattleskillEffectLogicEnum.curse_reflection,
          new Tuple<Func<BL.SkillEffect, bool>, Func<BL.SkillEffect, bool>, Action<BL.SkillEffect, BL.ISkillEffectListUnit>, Action<BL.SkillEffect>>((Func<BL.SkillEffect, bool>) (effect =>
          {
            int num23 = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id);
            if (num23 != 0 && num23 != enemy.originalUnit.unit.kind.ID)
              return false;
            if (effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.target_element))
            {
              int num24 = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element);
              if (num24 != 0 && (CommonElement) num24 != enemy.originalUnit.playerUnit.GetElement())
                return false;
            }
            int num25 = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.condition);
            bool flag = true;
            switch (num25)
            {
              case 1:
                flag = myself.hp >= 1;
                break;
              case 2:
                flag = myself.hp <= 0;
                break;
            }
            return flag;
          }), (Func<BL.SkillEffect, bool>) (effect =>
          {
            if (curseReflectionInvokeUnit[is_attack - 1] == null)
            {
              curseReflectionInvokeUnit[is_attack - 1] = myself;
              curseReflectionTargetDamage[is_attack - 1] = new Dictionary<BL.ISkillEffectListUnit, int>();
              curseReflectionTargetSwapHeal[is_attack - 1] = new Dictionary<BL.ISkillEffectListUnit, int>();
              bool flag = is_attack == 1;
              if (isInvokedAmbush)
                flag = !flag;
              iWork[0] = 0;
              foreach (BL.DuelTurn turn in duelResult.turns)
              {
                if (turn.isAtackker != flag)
                  iWork[0] = Judgement.CalcMaximumLongToInt((long) iWork[0] + (long) turn.realDamage);
              }
            }
            return true;
          }), (Action<BL.SkillEffect, BL.ISkillEffectListUnit>) ((effect, unit) =>
          {
            int damage = Judgement.CalcMaximumLongToInt((long) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.value) + (long) Judgement.CalcMaximumCeilToIntValue((Decimal) iWork[0] * (Decimal) effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage)));
            if (damage == 0)
              return;
            int num26 = 0;
            if (damage < 0)
            {
              BL.UnitPosition unitPosition12 = BattleFuncs.iSkillEffectListUnitToUnitPosition(unit);
              num26 = BattleFuncs.getHealValue(unit, BattleFuncs.getPanel(unitPosition12.row, unitPosition12.column), -damage, effect.baseSkill.skill_type);
              if (num26 < 0)
                damage = 0;
              else
                num26 = 0;
            }
            int num27 = BattleFuncs.applyFieldDamageFluctuate(atk, def, myself, unit, effect.effect, damage);
            if (num27 < 0 && !unit.CanHeal(effect.baseSkill.skill_type))
              num27 = 0;
            int index = is_attack - 1;
            if (!curseReflectionTargetDamage[index].ContainsKey(unit))
              curseReflectionTargetDamage[index][unit] = 0;
            if (!curseReflectionTargetSwapHeal[index].ContainsKey(unit))
              curseReflectionTargetSwapHeal[index][unit] = 0;
            curseReflectionTargetDamage[index][unit] = Judgement.CalcMaximumLongToInt((long) curseReflectionTargetDamage[index][unit] + (long) num27);
            curseReflectionTargetSwapHeal[index][unit] = Judgement.CalcMaximumLongToInt((long) curseReflectionTargetSwapHeal[index][unit] + (long) num26);
            if (curseReflectionSkillTarget == null)
              return;
            Tuple<BL.Unit, BattleskillSkill> key = new Tuple<BL.Unit, BattleskillSkill>(myself as BL.Unit, effect.baseSkill);
            if (!curseReflectionSkillTarget.ContainsKey(key))
              curseReflectionSkillTarget[key] = new List<BL.Unit>();
            if (curseReflectionSkillTarget[key].Contains(unit as BL.Unit))
              return;
            curseReflectionSkillTarget[key].Add(unit as BL.Unit);
          }), (Action<BL.SkillEffect>) null)
        },
        {
          BattleskillEffectLogicEnum.snake_venom_damage,
          new Tuple<Func<BL.SkillEffect, bool>, Func<BL.SkillEffect, bool>, Action<BL.SkillEffect, BL.ISkillEffectListUnit>, Action<BL.SkillEffect>>((Func<BL.SkillEffect, bool>) (effect =>
          {
            if (effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.target_element))
            {
              int num = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element);
              if (num != 0 && (CommonElement) num != enemy.originalUnit.playerUnit.GetElement())
                return false;
            }
            return true;
          }), (Func<BL.SkillEffect, bool>) (effect =>
          {
            bool flag = is_attack == 1;
            if (isInvokedAmbush)
              flag = !flag;
            List<BattleFuncs.SnakeVenomDamageData> collection = (List<BattleFuncs.SnakeVenomDamageData>) null;
            for (int index = duelResult.turns.Length - 1; index >= 0; --index)
            {
              BL.DuelTurn turn = duelResult.turns[index];
              if (turn.isAtackker == flag && turn.invokeGiveSkills != null && ((IEnumerable<BL.Skill>) turn.invokeGiveSkills).Any<BL.Skill>((Func<BL.Skill, bool>) (skill => ((IEnumerable<BattleskillEffect>) skill.skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (e => e.skill.ID == effect.baseSkillId)))))
              {
                if (snakeVenomDamage.Any<BattleFuncs.SnakeVenomDamageData>((Func<BattleFuncs.SnakeVenomDamageData, bool>) (x => x.effect.effectId == effect.effectId)))
                  return false;
                if (collection == null)
                  collection = new List<BattleFuncs.SnakeVenomDamageData>();
                collection.Add(new BattleFuncs.SnakeVenomDamageData()
                {
                  invokeUnit = myself,
                  effect = effect,
                  invokeOrder = index,
                  turnDamage = turn.realDamage
                });
              }
            }
            if (collection != null)
              snakeVenomDamage.AddRange((IEnumerable<BattleFuncs.SnakeVenomDamageData>) collection);
            else
              snakeVenomDamage.Add(new BattleFuncs.SnakeVenomDamageData()
              {
                invokeUnit = myself,
                effect = effect,
                invokeOrder = 10000 + is_attack,
                turnDamage = 0
              });
            return true;
          }), (Action<BL.SkillEffect, BL.ISkillEffectListUnit>) ((effect, unit) =>
          {
            if (unit == enemy && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.effect_target) != 3)
            {
              BattleFuncs.PackedSkillEffect packedSkillEffect = BattleFuncs.PackedSkillEffect.Create(effect);
              if ((!packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.range_form) ? 0 : packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.range_form)) == 3)
              {
                if (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_range) != effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.penetrate_radius) != 0)
                  return;
              }
              else if (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_range) != 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range) != 0)
                return;
            }
            float num28 = effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.venom_hp_percentage);
            float num29 = effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.venom_damage_percentage);
            int num30 = (double) num28 >= 0.0 ? Mathf.CeilToInt((float) unit.originalUnit.parameter.Hp * num28) : Mathf.FloorToInt((float) unit.originalUnit.parameter.Hp * num28);
            foreach (BattleFuncs.SnakeVenomDamageData snakeVenomDamageData in snakeVenomDamage.Where<BattleFuncs.SnakeVenomDamageData>((Func<BattleFuncs.SnakeVenomDamageData, bool>) (x => x.effect == effect)))
            {
              int num31 = (double) num29 >= 0.0 ? Judgement.CalcMaximumCeilToIntValue((Decimal) snakeVenomDamageData.turnDamage * (Decimal) num29) : Judgement.CalcMaximumFloorToIntValue((Decimal) snakeVenomDamageData.turnDamage * (Decimal) num29);
              int damage = Judgement.CalcMaximumLongToInt((long) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.venom_value) + (long) num30 + (long) num31);
              int num32 = 0;
              if (damage < 0)
              {
                BL.UnitPosition unitPosition13 = BattleFuncs.iSkillEffectListUnitToUnitPosition(unit);
                num32 = BattleFuncs.getHealValue(unit, BattleFuncs.getPanel(unitPosition13.row, unitPosition13.column), -damage, effect.baseSkill.skill_type);
                if (num32 < 0)
                  damage = 0;
                else
                  num32 = 0;
              }
              int num33 = BattleFuncs.applyFieldDamageFluctuate(atk, def, myself, unit, effect.effect, damage);
              if (num33 < 0 && !unit.CanHeal(effect.baseSkill.skill_type))
                num33 = 0;
              if (num33 < 0 && unit == myself && myself.hp <= 0)
              {
                BattleFuncs.PackedSkillEffect packedSkillEffect = BattleFuncs.PackedSkillEffect.Create(effect);
                if (!packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.condition) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.condition) != 1)
                  continue;
              }
              if (snakeVenomDamageData.unitDamage == null)
              {
                snakeVenomDamageData.unitDamage = new Dictionary<BL.ISkillEffectListUnit, int>();
                snakeVenomDamageData.unitSwapHealDamage = new Dictionary<BL.ISkillEffectListUnit, int>();
              }
              snakeVenomDamageData.unitDamage[unit] = num33;
              snakeVenomDamageData.unitSwapHealDamage[unit] = num32;
            }
          }), (Action<BL.SkillEffect>) null)
        }
      };
      foreach (BattleskillEffectLogicEnum key in dictionary1.Keys)
      {
        for (is_attack = 1; is_attack <= 2; is_attack++)
        {
          List<BL.SkillEffect> skillEffectList = new List<BL.SkillEffect>();
          foreach (BL.SkillEffect effect in myself.skillEffects.Where(key))
          {
            BL.UnitPosition unitPosition14 = BattleFuncs.iSkillEffectListUnitToUnitPosition(myself);
            if ((effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack_nc) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack_nc) == is_attack) && effect.effect.GetPackedSkillEffect().CheckLandTag(BattleFuncs.getPanel(unitPosition14.row, unitPosition14.column), isAI) && !BattleFuncs.isSkillsAndEffectsInvalid(myself, enemy) && !BattleFuncs.isSealedSkillEffect(myself, effect) && dictionary1[key].Item1(effect))
              skillEffectList.Add(effect);
            if (dictionary1[key].Item4 != null)
              dictionary1[key].Item4(effect);
          }
          foreach (BL.SkillEffect headerEffect in BattleFuncs.gearSkillEffectFilter(myself.originalUnit, (IEnumerable<BL.SkillEffect>) skillEffectList))
          {
            if (dictionary1[key].Item2 == null || dictionary1[key].Item2(headerEffect))
            {
              int[] range = new int[2]
              {
                headerEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_range),
                headerEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range)
              };
              BattleFuncs.PackedSkillEffect packedSkillEffect = BattleFuncs.PackedSkillEffect.Create(headerEffect);
              int range_form = !packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.range_form) ? 0 : packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.range_form);
              int num = headerEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.effect_target);
              IEnumerable<BL.UnitPosition> source6;
              if (range_form == 3)
              {
                BL.UnitPosition unitPosition15 = BattleFuncs.iSkillEffectListUnitToUnitPosition(myself);
                BL.UnitPosition unitPosition16 = BattleFuncs.iSkillEffectListUnitToUnitPosition(enemy);
                HashSet<Tuple<int, int>> laserPosition = BattleFuncs.getLaserPosition(unitPosition15.row, unitPosition15.column, unitPosition16.row, unitPosition16.column, range[0], range[1], packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.penetrate_radius), headerEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.is_range_from_enemy) == 1);
                List<BL.UnitPosition> unitPositionList = new List<BL.UnitPosition>();
                foreach (Tuple<int, int> tuple in laserPosition)
                {
                  BL.UnitPosition[] unitPositionArray3 = isAI ? BattleFuncs.getFieldUnitsAI(tuple.Item1, tuple.Item2) : env.getFieldUnits(tuple.Item1, tuple.Item2);
                  if (unitPositionArray3 != null)
                  {
                    foreach (BL.UnitPosition unitPosition17 in unitPositionArray3)
                    {
                      if (!unitPosition17.unit.isFacility)
                      {
                        if (num == 0 || num == 2)
                        {
                          if (!((IEnumerable<BL.ForceID>) forceIdArray1).Contains<BL.ForceID>(env.getForceID(unitPosition17.unit)))
                            continue;
                        }
                        else if (!((IEnumerable<BL.ForceID>) forceIdArray2).Contains<BL.ForceID>(env.getForceID(unitPosition17.unit)))
                          continue;
                        unitPositionList.Add(unitPosition17);
                      }
                    }
                  }
                }
                source6 = (IEnumerable<BL.UnitPosition>) unitPositionList;
              }
              else
              {
                BL.Unit rangeOriginUnit = headerEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.is_range_from_enemy) == 0 ? myself.originalUnit : enemy.originalUnit;
                source6 = num == 0 || num == 2 ? (IEnumerable<BL.UnitPosition>) BattleFuncs.getTargets(rangeOriginUnit, range, forceIdArray1, BL.Unit.TargetAttribute.all, isAI, nonFacility: true) : (IEnumerable<BL.UnitPosition>) BattleFuncs.getTargets(rangeOriginUnit, range, forceIdArray2, BL.Unit.TargetAttribute.all, isAI, nonFacility: true);
                if (range_form == 1 || range_form == 2)
                  source6 = source6.Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (up =>
                  {
                    switch (range_form)
                    {
                      case 1:
                        Tuple<int, int> unitCell3 = BattleFuncs.getUnitCell(rangeOriginUnit, isAI);
                        if (up.row != unitCell3.Item1 && up.column != unitCell3.Item2)
                          return false;
                        break;
                      case 2:
                        Tuple<int, int> unitCell4 = BattleFuncs.getUnitCell(rangeOriginUnit, isAI);
                        if (Mathf.Abs(up.row - unitCell4.Item1) != Mathf.Abs(up.column - unitCell4.Item2))
                          return false;
                        break;
                    }
                    return true;
                  }));
              }
              switch (num)
              {
                case 2:
                  source6 = source6.Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => BattleFuncs.unitPositionToISkillEffectListUnit(x) == myself));
                  break;
                case 3:
                  source6 = source6.Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => BattleFuncs.unitPositionToISkillEffectListUnit(x) == enemy));
                  break;
              }
              foreach (BL.UnitPosition unitPosition18 in (IEnumerable<BL.UnitPosition>) source6.ToList<BL.UnitPosition>())
              {
                BL.ISkillEffectListUnit skillEffectListUnit8 = unitPosition18 is BL.ISkillEffectListUnit ? unitPosition18 as BL.ISkillEffectListUnit : (BL.ISkillEffectListUnit) unitPosition18.unit;
                if (skillEffectListUnit8 != null)
                  dictionary1[key].Item3(headerEffect, skillEffectListUnit8);
              }
            }
          }
          BL.ISkillEffectListUnit skillEffectListUnit9 = myself;
          myself = enemy;
          enemy = skillEffectListUnit9;
          BL.ForceID[] forceIdArray4 = forceIdArray1;
          forceIdArray1 = forceIdArray2;
          forceIdArray2 = forceIdArray4;
        }
      }
      if (lifeAbsorbTargetHeal.Count >= 1)
      {
        foreach (BL.ISkillEffectListUnit key4 in lifeAbsorbTargetHeal.Keys)
        {
          int hp = key4.hp;
          if (hp >= 1)
          {
            key4.hp += lifeAbsorbTargetHeal[key4];
            if (key4.hp <= 0 && key4 != atk && key4 != def)
              deads.Add(key4);
            if (addHpFunction != null)
              addHpFunction(key4, hp);
          }
          else if (lifeAbsorbSkillTarget != null)
          {
            foreach (Tuple<BL.Unit, BattleskillSkill> key5 in lifeAbsorbSkillTarget.Keys)
              lifeAbsorbSkillTarget[key5].Remove(key4 as BL.Unit);
          }
        }
      }
      if (skillEffectListUnitArray[0] != null || skillEffectListUnitArray[1] != null)
      {
        if (penetrateSkillTarget != null)
        {
          foreach (Tuple<BL.Unit, BattleskillSkill> key in penetrateSkillTarget.Keys)
          {
            List<BL.Unit> unitList = new List<BL.Unit>();
            foreach (BL.Unit unit in penetrateSkillTarget[key].Item1)
            {
              if (unit.hp <= 0)
                unitList.Add(unit);
            }
            foreach (BL.Unit unit in unitList)
              penetrateSkillTarget[key].Item1.Remove(unit);
          }
        }
        for (int index = 0; index < 2; ++index)
        {
          if (dictionaryArray[index] != null && dictionaryArray[index].Count >= 1)
          {
            BL.ISkillEffectListUnit invokeUnit = skillEffectListUnitArray[index];
            foreach (BL.ISkillEffectListUnit key in dictionaryArray[index].Keys)
            {
              if (key.hp >= 1)
                BattleFuncs.applyHpDamage(atk, def, invokeUnit, key, dictionaryArray[index][key], minHp1, deads, penetrateHpFunction);
            }
          }
        }
      }
      if (rangeAttackInvokeUnit[0] != null || rangeAttackInvokeUnit[1] != null)
      {
        if (rangeAttackSkillTarget != null)
        {
          foreach (Tuple<BL.Unit, BattleskillSkill> key in rangeAttackSkillTarget.Keys)
          {
            List<BL.Unit> unitList = new List<BL.Unit>();
            foreach (BL.Unit unit in rangeAttackSkillTarget[key])
            {
              if (unit.hp <= 0)
                unitList.Add(unit);
            }
            foreach (BL.Unit unit in unitList)
              rangeAttackSkillTarget[key].Remove(unit);
          }
        }
        for (int index = 0; index < 2; ++index)
        {
          if (rangeAttackTargetDamage[index] != null && rangeAttackTargetDamage[index].Count >= 1)
          {
            BL.ISkillEffectListUnit invokeUnit = rangeAttackInvokeUnit[index];
            foreach (BL.ISkillEffectListUnit key in rangeAttackTargetDamage[index].Keys)
            {
              if (key.hp >= 1)
                BattleFuncs.applyHpDamage(atk, def, invokeUnit, key, rangeAttackTargetDamage[index][key], rangeAttackMinHp, deads, rangeAttackHpFunction);
            }
          }
        }
      }
      if (curseReflectionInvokeUnit[0] != null || curseReflectionInvokeUnit[1] != null)
      {
        if (curseReflectionSkillTarget != null)
        {
          foreach (Tuple<BL.Unit, BattleskillSkill> key in curseReflectionSkillTarget.Keys)
          {
            List<BL.Unit> unitList = new List<BL.Unit>();
            foreach (BL.Unit unit in curseReflectionSkillTarget[key])
            {
              if (unit.hp <= 0)
                unitList.Add(unit);
            }
            foreach (BL.Unit unit in unitList)
              curseReflectionSkillTarget[key].Remove(unit);
          }
        }
        for (int index = 0; index < 2; ++index)
        {
          if (curseReflectionTargetDamage[index] != null && curseReflectionTargetDamage[index].Count >= 1)
          {
            BL.ISkillEffectListUnit invokeUnit = curseReflectionInvokeUnit[index];
            foreach (BL.ISkillEffectListUnit key in curseReflectionTargetDamage[index].Keys)
            {
              if (key.hp >= 1)
                BattleFuncs.applyHpDamage(atk, def, invokeUnit, key, curseReflectionTargetDamage[index][key], 0, deads, subHpFunction);
            }
          }
          if (curseReflectionTargetSwapHeal[index] != null && curseReflectionTargetSwapHeal[index].Count >= 1)
          {
            foreach (BL.ISkillEffectListUnit key in curseReflectionTargetSwapHeal[index].Keys)
            {
              if (key.hp >= 1)
              {
                int hp = key.hp;
                key.hp += curseReflectionTargetSwapHeal[index][key];
                if (subHpFunction != null)
                  subHpFunction(key, hp);
                if (key.hp <= 0 && key != atk && key != def)
                  deads.Add(key);
              }
            }
          }
        }
      }
      foreach (BattleFuncs.SnakeVenomDamageData snakeVenomDamageData in (IEnumerable<BattleFuncs.SnakeVenomDamageData>) snakeVenomDamage.OrderBy<BattleFuncs.SnakeVenomDamageData, int>((Func<BattleFuncs.SnakeVenomDamageData, int>) (x => x.invokeOrder)).ThenByDescending<BattleFuncs.SnakeVenomDamageData, int>((Func<BattleFuncs.SnakeVenomDamageData, int>) (x => x.effect.baseSkill.weight)).ThenBy<BattleFuncs.SnakeVenomDamageData, int>((Func<BattleFuncs.SnakeVenomDamageData, int>) (x => x.effect.baseSkillId)))
      {
        snakeVenomDamageData.invokeUnit.skillEffects.RemoveEffect(1000413, env, snakeVenomDamageData.invokeUnit);
        if (snakeVenomDamageData.unitDamage != null)
        {
          Dictionary<BL.Unit, Tuple<int, int>> dictionary2 = snakeFunction != null ? new Dictionary<BL.Unit, Tuple<int, int>>() : (Dictionary<BL.Unit, Tuple<int, int>>) null;
          foreach (BL.ISkillEffectListUnit key in snakeVenomDamageData.unitDamage.Keys)
          {
            int minHp2 = snakeVenomDamageData.effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_hp);
            if (key.hp >= minHp2)
            {
              int hp = key.hp;
              int damage = snakeVenomDamageData.unitDamage[key];
              if (damage >= 0)
                BattleFuncs.applyHpDamage(atk, def, snakeVenomDamageData.invokeUnit, key, damage, minHp2, deads, (Action<BL.ISkillEffectListUnit, int>) null);
              else
                key.hp -= damage;
              if (key.hp >= 1)
              {
                key.hp += snakeVenomDamageData.unitSwapHealDamage[key];
                if (key.hp <= 0 && key != atk && key != def)
                  deads.Add(key);
              }
              if (dictionary2 != null && (damage < 0 || hp >= 1))
                dictionary2[key as BL.Unit] = new Tuple<int, int>(hp, key.hp);
            }
          }
          if (dictionary2 != null)
          {
            BattleFuncs.PackedSkillEffect packedSkillEffect = BattleFuncs.PackedSkillEffect.Create(snakeVenomDamageData.effect);
            float? nullable;
            if ((!packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.range_form) ? 0 : packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.range_form)) == 3)
            {
              BL.UnitPosition unitPosition19 = BattleFuncs.iSkillEffectListUnitToUnitPosition(atk);
              BL.UnitPosition unitPosition20 = BattleFuncs.iSkillEffectListUnitToUnitPosition(def);
              nullable = snakeVenomDamageData.invokeUnit != atk ? new float?(Mathf.Atan2((float) (unitPosition19.column - unitPosition20.column), (float) (unitPosition19.row - unitPosition20.row)) * 57.29578f) : new float?(Mathf.Atan2((float) (unitPosition20.column - unitPosition19.column), (float) (unitPosition20.row - unitPosition19.row)) * 57.29578f);
            }
            else
              nullable = new float?();
            snakeFunction(snakeVenomDamageData.effect.baseSkill, snakeVenomDamageData.invokeUnit as BL.Unit, dictionary2, nullable);
          }
        }
      }
    }

    private static void applyHpDamage(
      BL.ISkillEffectListUnit atk,
      BL.ISkillEffectListUnit def,
      BL.ISkillEffectListUnit invokeUnit,
      BL.ISkillEffectListUnit target,
      int damage,
      int minHp,
      HashSet<BL.ISkillEffectListUnit> deads,
      Action<BL.ISkillEffectListUnit, int> hpFunction)
    {
      BL.Unit unit1 = invokeUnit as BL.Unit;
      int hp = target.hp;
      target.hp -= damage;
      if (target.hp < minHp)
        target.hp = minHp;
      if (damage >= 1 && target != atk && target != def)
        target.skillEffects.RemoveEffect(1000418, BattleFuncs.env, target);
      if (hpFunction != null)
        hpFunction(target, hp);
      if (target.hp <= 0 && target != atk && target != def)
      {
        if (unit1 != (BL.Unit) null)
        {
          BL.Unit unit2 = target as BL.Unit;
          ++unit1.killCount;
          unit2.killedBy = unit1;
          if (BattleFuncs.env.getForceID(unit2) == BL.ForceID.player)
            BattleFuncs.env.updateIntimateByDefense(unit2);
        }
        invokeUnit.skillEffects.AddKillCount(1);
        deads.Add(target);
      }
      if (!(unit1 != (BL.Unit) null))
        return;
      int damage1 = hp - target.hp;
      if (damage1 <= 0)
        return;
      if (!unit1.originalUnit.isFacility && !target.originalUnit.isFacility)
        unit1.attackDamage += damage1;
      else
        unit1.originalUnit.addAttackSubDamage(damage1);
      target.originalUnit.addReceivedSubDamage(damage1);
    }

    public static bool applyServantsJoy(BL.ISkillEffectListUnit unit, int healHpTotal)
    {
      if (healHpTotal < 0)
        return false;
      List<BattleFuncs.SkillParam> skillParams = new List<BattleFuncs.SkillParam>();
      bool flag = false;
      BL.UnitPosition unitPosition = BattleFuncs.iSkillEffectListUnitToUnitPosition(unit);
      BL.Panel panel = BattleFuncs.getPanel(unitPosition.row, unitPosition.column);
      foreach (BL.SkillEffect effect in unit.enabledSkillEffect(BattleskillEffectLogicEnum.ratio_servants_joy))
      {
        float num = effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) + effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio) * (float) effect.baseSkillLevel;
        int healValue1 = (int) ((Decimal) healHpTotal * (Decimal) num);
        if ((double) num > 0.0)
        {
          if (healValue1 <= 0)
            healValue1 = 1;
        }
        else if ((double) num < 0.0 && healValue1 >= 0)
          healValue1 = -1;
        int healValue2 = BattleFuncs.getHealValue(unit, panel, healValue1, effect.baseSkill.skill_type);
        skillParams.Add(BattleFuncs.SkillParam.CreateAdd(unit.originalUnit, effect, (float) healValue2));
        flag = true;
      }
      foreach (BL.SkillEffect effect in unit.enabledSkillEffect(BattleskillEffectLogicEnum.fix_servants_joy))
      {
        int healValue3 = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.value) + effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_ratio) * effect.baseSkillLevel;
        int healValue4 = BattleFuncs.getHealValue(unit, panel, healValue3, effect.baseSkill.skill_type);
        skillParams.Add(BattleFuncs.SkillParam.CreateAdd(unit.originalUnit, effect, (float) healValue4));
        flag = true;
      }
      if (flag)
        unit.hp += BattleFuncs.calcSkillParamAdd(skillParams);
      return flag;
    }

    public static bool hasEnabledMoveDistanceEffects(BL.ISkillEffectListUnit unit)
    {
      foreach (BattleskillEffectLogicEnum logic in BattleFuncs.moveDistanceSkillEffectsEnum)
      {
        if (unit.HasEnabledSkillEffect(logic))
          return true;
      }
      return false;
    }

    public static bool hasEnabledRangeEffects(BL.ISkillEffectListUnit unit)
    {
      foreach (BattleskillEffectLogicEnum logic in BattleFuncs.rangeSkillEffectsEnum)
      {
        if (unit.HasEnabledSkillEffect(logic))
          return true;
      }
      return false;
    }

    public static bool hasEnabledOnemanChargeEffects(BL.ISkillEffectListUnit unit)
    {
      foreach (BattleskillEffectLogicEnum logic in BattleFuncs.onemanChargeEffectsEnum)
      {
        if (unit.HasEnabledSkillEffect(logic))
          return true;
      }
      foreach (BL.SkillEffect effect in unit.skillEffects.All())
      {
        if (effect.effect.IsExistOnemanChargeExtArg())
        {
          BattleFuncs.PackedSkillEffect packedSkillEffect = effect.effect.GetPackedSkillEffect();
          if (packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.cant_seal) && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.cant_seal) == 1 || !BattleFuncs.isSealedSkillEffect(unit, effect))
            return true;
        }
      }
      return false;
    }

    public static bool hasEnabledTargetCountEffects(BL.ISkillEffectListUnit unit)
    {
      foreach (BattleskillEffectLogicEnum logic in BattleFuncs.targetCountEffectsEnum)
      {
        if (unit.HasEnabledSkillEffect(logic))
          return true;
      }
      return false;
    }

    public static bool isCharismaEffect(BattleskillEffectLogicEnum e)
    {
      return ((IEnumerable<BattleskillEffectLogicEnum>) BattleFuncs.charismaEffectsEnum).Contains<BattleskillEffectLogicEnum>(e);
    }

    public static IEnumerable<BL.SkillEffect> getEnabledCharismaEffects(BL.ISkillEffectListUnit unit)
    {
      BattleskillEffectLogicEnum[] battleskillEffectLogicEnumArray = BattleFuncs.charismaEffectsEnum;
      for (int index = 0; index < battleskillEffectLogicEnumArray.Length; ++index)
      {
        foreach (BL.SkillEffect enabledEffect in BattleFuncs.getEnabledEffects(unit.skillEffects, battleskillEffectLogicEnumArray[index], unit))
          yield return enabledEffect;
      }
      battleskillEffectLogicEnumArray = (BattleskillEffectLogicEnum[]) null;
    }

    public static bool hasEnabledDeadCountEffects(
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit deadUnit)
    {
      BL.ForceID forceId = BattleFuncs.env.getForceID(deadUnit.originalUnit);
      if (BattleFuncs.env.getForceID(unit.originalUnit) == forceId)
      {
        foreach (BattleskillEffectLogicEnum e in BattleFuncs.deadCountSkillEffectsEnumPlayerForce)
        {
          if (BattleFuncs.getEnabledEffects(unit.skillEffects, e, unit).Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => !x.effect.HasKey(BattleskillEffectLogicArgumentEnum.unit_id) || x.effect.GetInt(BattleskillEffectLogicArgumentEnum.unit_id) == 0 || x.effect.GetInt(BattleskillEffectLogicArgumentEnum.unit_id) == deadUnit.originalUnit.unit.ID)))
            return true;
        }
      }
      else if (((IEnumerable<BL.ForceID>) BattleFuncs.env.getTargetForce(unit.originalUnit, false)).Contains<BL.ForceID>(forceId))
      {
        foreach (BattleskillEffectLogicEnum e in BattleFuncs.deadCountSkillEffectsEnumEnemyForce)
        {
          if (BattleFuncs.getEnabledEffects(unit.skillEffects, e, unit).Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => !x.effect.HasKey(BattleskillEffectLogicArgumentEnum.unit_id) || x.effect.GetInt(BattleskillEffectLogicArgumentEnum.unit_id) == 0 || x.effect.GetInt(BattleskillEffectLogicArgumentEnum.unit_id) == deadUnit.originalUnit.unit.ID)))
            return true;
        }
      }
      return false;
    }

    public static IEnumerable<BL.SkillEffect> getEnabledEffects(
      BL.SkillEffectList self,
      BattleskillEffectLogicEnum e,
      BL.ISkillEffectListUnit effectUnit)
    {
      return self.Where(e, (Func<BL.SkillEffect, bool>) (effect =>
      {
        if (effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.element) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != effectUnit.originalUnit.playerUnit.GetElement())
          return false;
        return !effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.gear_kind_id) || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == effectUnit.originalUnit.unit.kind.ID;
      }));
    }

    public static IEnumerable<BL.SkillEffect> getEnabledOnemanChargeEffects(
      BL.ISkillEffectListUnit unit)
    {
      BattleskillEffectLogicEnum[] battleskillEffectLogicEnumArray = BattleFuncs.onemanChargeEffectsEnum;
      for (int index = 0; index < battleskillEffectLogicEnumArray.Length; ++index)
      {
        foreach (BL.SkillEffect onemanChargeEffect in unit.skillEffects.Where(battleskillEffectLogicEnumArray[index]))
          yield return onemanChargeEffect;
      }
      battleskillEffectLogicEnumArray = (BattleskillEffectLogicEnum[]) null;
    }

    public static IEnumerable<BL.Panel> getSkillZocPanels(
      BL.ISkillEffectListUnit unit,
      int row,
      int column,
      bool useGearFilter,
      bool enableOnly)
    {
      IEnumerable<BL.SkillEffect> skillEffects = !enableOnly ? unit.getSkillEffects(BattleskillEffectLogicEnum.zoc) : unit.enabledSkillEffect(BattleskillEffectLogicEnum.zoc);
      if (useGearFilter)
        skillEffects = BattleFuncs.gearSkillEffectFilter(unit.originalUnit, skillEffects);
      return BattleFuncs.getSkillEffectZocPanels(skillEffects, row, column, unit is BL.AIUnit);
    }

    public static IEnumerable<BL.Panel> getSkillEffectZocPanels(
      IEnumerable<BL.SkillEffect> zocSkillEffects,
      int row,
      int column,
      bool isAI)
    {
      if (zocSkillEffects.Any<BL.SkillEffect>())
      {
        BL.Panel panel = BattleFuncs.getPanel(row, column);
        foreach (BL.SkillEffect zocSkillEffect in zocSkillEffects)
        {
          if (zocSkillEffect.effect.GetPackedSkillEffect().CheckLandTag(panel, isAI))
          {
            int num1 = zocSkillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_range);
            int num2 = zocSkillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range);
            bool includingSlanting = zocSkillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.excluding_slanting) == 0;
            int[] range = new int[2]{ num1, num2 };
            foreach (BL.Panel rangePanel in BattleFuncs.getRangePanels(row, column, range))
            {
              if (includingSlanting || !includingSlanting && (rangePanel.row == row || rangePanel.column == column))
                yield return rangePanel;
            }
          }
        }
      }
    }

    public struct AttackDamageData
    {
      public bool isHit;
      public bool isCritical;
      public int damage;
      public int dispDamage;
      public int realDamage;
      public int drainDamage;
      public int dispDrainDamage;
      public int defenderDispDrainDamage;
      public int defenderDrainDamage;
      public BattleDuelSkill duelSkillProc;
      public TurnHp hp;
      public int counterDamage;
      public List<string> invokeSkillExtraInfo;
      public List<BL.ISkillEffectListUnit> damageShareUnit;
      public List<int> damageShareDamage;
      public List<BL.UseSkillEffect> damageShareSkillEffect;
      public List<BL.UseSkillEffect> attackerUseSkillEffects;
      public List<BL.UseSkillEffect> defenderUseSkillEffects;
      public int attackerSwapHealDamage;
      public int defenderSwapHealDamage;
    }

    public class AsterEdge
    {
      public int to;
      public int cost;

      public AsterEdge(int to, int cost)
      {
        this.to = to;
        this.cost = cost;
      }
    }

    public class AsterNode
    {
      public BL.Panel panel;
      public BattleFuncs.AsterEdge[] edges;
      public int cost;
      public int no;
      public int from;

      public AsterNode(int no, BL.Panel panel, BattleFuncs.AsterEdge[] edges)
      {
        this.panel = panel;
        this.no = no;
        this.edges = edges;
        this.cost = 1000000000;
        this.from = -1;
      }
    }

    public class BeforeDuelDuelSupport
    {
      public IntimateDuelSupport duelSupport;
      public int hit;
      public int evasion;
      public int critical;
      public int criticalEvasion;
      public int hitIncr;
      public int evasionIncr;
      public int criticalIncr;
      public int criticalEvasionIncr;
    }

    public class InvalidSpecificSkillLogic
    {
      public int skillId { get; private set; }

      public int logicId { get; private set; }

      public object param { get; private set; }

      public static BattleFuncs.InvalidSpecificSkillLogic Create(
        int skillId,
        int logicId,
        object param = null)
      {
        return new BattleFuncs.InvalidSpecificSkillLogic()
        {
          skillId = skillId,
          logicId = logicId,
          param = param
        };
      }
    }

    public struct InvokeLotteryInfo
    {
      public float Final;
      public float Base;
      public float WhiteNight;
      public float? Max;
      public float? Min;
      public float? External;

      public override string ToString()
      {
        object[] objArray = new object[6]
        {
          (object) this.Final.ToString("#.##"),
          (object) this.Base.ToString("#.##"),
          null,
          null,
          null,
          null
        };
        float num;
        string str1;
        if (!this.External.HasValue)
        {
          str1 = "null";
        }
        else
        {
          num = this.External.Value * 100f;
          str1 = num.ToString("#.##");
        }
        objArray[2] = (object) str1;
        objArray[3] = (object) this.WhiteNight.ToString("#.##");
        string str2;
        if (!this.Max.HasValue)
        {
          str2 = "null";
        }
        else
        {
          num = this.Max.Value;
          str2 = num.ToString("#.##");
        }
        objArray[4] = (object) str2;
        string str3;
        if (!this.Min.HasValue)
        {
          str3 = "null";
        }
        else
        {
          num = this.Min.Value;
          str3 = num.ToString("#.##");
        }
        objArray[5] = (object) str3;
        return string.Format("{0}% (基={1}, 外={2}, 白={3}, 大={4}, 小={5})", objArray);
      }
    }

    public class PackedSkillEffect
    {
      private List<BattleskillEffect> effects;
      private bool ignoreHeader;
      private List<BattleskillEffect> ignoreHeaderEffects;

      public BL.SkillEffect skillEffect { get; private set; }

      public static BattleFuncs.PackedSkillEffect Create(BL.SkillEffect headerEffect)
      {
        BattleFuncs.PackedSkillEffect packedSkillEffect = new BattleFuncs.PackedSkillEffect();
        packedSkillEffect.skillEffect = headerEffect;
        packedSkillEffect.Init(headerEffect.baseSkill, headerEffect.effect);
        return packedSkillEffect;
      }

      public static BattleFuncs.PackedSkillEffect Create(BattleskillEffect headerEffect)
      {
        BattleFuncs.PackedSkillEffect packedSkillEffect = new BattleFuncs.PackedSkillEffect();
        packedSkillEffect.skillEffect = (BL.SkillEffect) null;
        packedSkillEffect.Init(headerEffect.skill, headerEffect);
        return packedSkillEffect;
      }

      private void Init(BattleskillSkill skill, BattleskillEffect headerEffect)
      {
        int id = headerEffect.ID;
        bool flag = false;
        this.effects = new List<BattleskillEffect>();
        this.effects.Add(headerEffect);
        foreach (BattleskillEffect effect in skill.Effects)
        {
          if (!flag)
          {
            if (effect.ID == id)
              flag = true;
          }
          else
          {
            if (!effect.EffectLogic.HasTag(BattleskillEffectTag.ext_arg))
              break;
            this.effects.Add(effect);
          }
        }
      }

      public bool HasKey(BattleskillEffectLogicArgumentEnum key)
      {
        return this.GetEffects().Find((Predicate<BattleskillEffect>) (x => x.HasKey(key))) != null;
      }

      public int GetInt(BattleskillEffectLogicArgumentEnum key)
      {
        return this.GetEffects().Find((Predicate<BattleskillEffect>) (x => x.HasKey(key))).GetInt(key);
      }

      public float GetFloat(BattleskillEffectLogicArgumentEnum key)
      {
        return this.GetEffects().Find((Predicate<BattleskillEffect>) (x => x.HasKey(key))).GetFloat(key);
      }

      public BattleskillEffectLogicEnum LogicEnum() => this.effects[0].EffectLogic.Enum;

      public BattleskillEffect GetHasKeyEffect(BattleskillEffectLogicArgumentEnum key)
      {
        return this.GetEffects().Find((Predicate<BattleskillEffect>) (x => x.HasKey(key)));
      }

      public void SetIgnoreHeader(bool v)
      {
        this.ignoreHeader = v;
        if (!this.ignoreHeader || this.ignoreHeaderEffects != null)
          return;
        this.ignoreHeaderEffects = this.effects.Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.HasTag(BattleskillEffectTag.ext_arg))).ToList<BattleskillEffect>();
      }

      public List<BattleskillEffect> GetEffects()
      {
        return !this.ignoreHeader ? this.effects : this.ignoreHeaderEffects;
      }

      public bool CheckLandTag(
        BL.Panel panel,
        BattleskillEffectLogicArgumentEnum startLogic,
        BattleskillEffectLogicArgumentEnum endLogic,
        bool isAI)
      {
        if (!this.HasKey(startLogic))
          return true;
        if (panel == null)
          return false;
        BattleLandform landform = panel.landform;
        if (landform == null)
          return false;
        for (BattleskillEffectLogicArgumentEnum key = startLogic; key <= endLogic; ++key)
        {
          int tag = this.GetInt(key);
          if (tag == 0)
            return false;
          if (landform.HasTag(tag))
            return true;
          if (panel.hasLandTag((isAI ? 1 : 0) != 0, tag))
            return true;
        }
        return false;
      }

      public bool CheckLandTag(BL.Panel panel, bool isAI)
      {
        return this.CheckLandTag(panel, BattleskillEffectLogicArgumentEnum.land_tag1, BattleskillEffectLogicArgumentEnum.land_tag10, isAI);
      }

      public bool CheckTargetLandTag(BL.Panel panel, bool isAI)
      {
        return this.CheckLandTag(panel, BattleskillEffectLogicArgumentEnum.target_land_tag1, BattleskillEffectLogicArgumentEnum.target_land_tag10, isAI);
      }
    }

    public class SkillParam
    {
      public BL.Unit hasUnit;
      public BL.SkillEffect effect;
      public float? addParam;
      public float? mulParam;
      public object param;
      public int category;
      private BL.Unit effectUnit_;

      public BL.Unit effectUnit
      {
        get
        {
          if (this.effectUnit_ == (BL.Unit) null)
            this.effectUnit_ = BattleFuncs.getEffectUnit(this.effect, this.hasUnit);
          return this.effectUnit_;
        }
        set => this.effectUnit_ = value;
      }

      public static BattleFuncs.SkillParam Create(BL.Unit hasUnit, BL.SkillEffect effect)
      {
        return new BattleFuncs.SkillParam()
        {
          hasUnit = hasUnit,
          effect = effect
        };
      }

      public static BattleFuncs.SkillParam CreateAdd(
        BL.Unit hasUnit,
        BL.SkillEffect effect,
        float addParam,
        object param = null,
        int category = 0)
      {
        return new BattleFuncs.SkillParam()
        {
          hasUnit = hasUnit,
          effect = effect,
          addParam = new float?(addParam),
          param = param,
          category = category
        };
      }

      public static BattleFuncs.SkillParam CreateMul(
        BL.Unit hasUnit,
        BL.SkillEffect effect,
        float mulParam,
        object param = null,
        int category = 0)
      {
        return new BattleFuncs.SkillParam()
        {
          hasUnit = hasUnit,
          effect = effect,
          mulParam = new float?(mulParam),
          param = param,
          category = category
        };
      }

      public static BattleFuncs.SkillParam CreateParam(
        BL.Unit hasUnit,
        BL.SkillEffect effect,
        object param,
        int category = 0)
      {
        return new BattleFuncs.SkillParam()
        {
          hasUnit = hasUnit,
          effect = effect,
          param = param,
          category = category
        };
      }
    }

    public class SkillParamClamp
    {
      public int? fixMin;
      public int? fixMax;
      public Decimal? ratioMin;
      public Decimal? ratioMax;
    }

    public class BuffDebuffSwapState
    {
      private HashSet<int> flagBuff;
      private HashSet<int> flagDebuff;

      public static BattleFuncs.BuffDebuffSwapState Create(
        BL.ISkillEffectListUnit unit,
        BL.ISkillEffectListUnit target = null,
        BL.Panel panel = null)
      {
        IEnumerable<BL.SkillEffect> skillEffects = unit.skillEffects.Where(BattleskillEffectLogicEnum.buff_debuff_swap, (Func<BL.SkillEffect, bool>) (x => !BattleFuncs.isSealedSkillEffect(unit, x) && (target == null || !BattleFuncs.isEffectEnemyRangeAndInvalid(x, unit, target)) && (target == null || !BattleFuncs.isSkillsAndEffectsInvalid(unit, target)) && x.effect.GetPackedSkillEffect().CheckLandTag(panel, unit is BL.AIUnit)));
        bool flag1 = false;
        HashSet<int> flag2 = (HashSet<int>) null;
        HashSet<int> flag3 = (HashSet<int>) null;
        foreach (BL.SkillEffect skillEffect in skillEffects)
        {
          if (!flag1)
          {
            flag1 = true;
            flag2 = new HashSet<int>();
            flag3 = new HashSet<int>();
          }
          int num = skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.buff_type);
          int skillType = skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_type);
          int investType = skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.invest_type);
          if (num == 0 || num == 1)
            BattleFuncs.BuffDebuffSwapState.setFlag(flag2, skillType, investType);
          if (num == 0 || num == 2)
            BattleFuncs.BuffDebuffSwapState.setFlag(flag3, skillType, investType);
        }
        if (!flag1)
          return (BattleFuncs.BuffDebuffSwapState) null;
        return new BattleFuncs.BuffDebuffSwapState()
        {
          flagBuff = flag2,
          flagDebuff = flag3
        };
      }

      private static void setFlag(HashSet<int> flag, int skillType, int investType)
      {
        switch (skillType)
        {
          case 0:
            flag.Add(1);
            flag.Add(2);
            flag.Add(3);
            flag.Add(0);
            flag.Add(6);
            flag.Add(7);
            flag.Add(14);
            flag.Add(15);
            break;
          case 3:
            if (investType == 0 || investType == 1)
              flag.Add(3);
            if (investType != 0 && investType != 2)
              break;
            flag.Add(0);
            break;
          default:
            flag.Add(skillType);
            break;
        }
      }

      private int getFlag(BL.SkillEffect effect)
      {
        if (effect.baseSkill.skill_type != BattleskillSkillType.passive)
          return (int) effect.baseSkill.skill_type;
        return effect.isBaseSkill ? 0 : 3;
      }

      public bool isBuffSwap(BL.SkillEffect effect)
      {
        return effect.effect.canBuffDebuffSwap() && this.flagBuff.Contains(this.getFlag(effect)) && !BattleFuncs.isBonusSkillId(effect.baseSkillId);
      }

      public bool isDebuffSwap(BL.SkillEffect effect)
      {
        return effect.effect.canBuffDebuffSwap() && this.flagDebuff.Contains(this.getFlag(effect)) && !BattleFuncs.isBonusSkillId(effect.baseSkillId);
      }
    }

    public class InvestSkill
    {
      public BattleskillSkill skill;
      public int? turnRemain;
      public bool fromEnemy;
      public bool fromFriend;
      public bool isEnemyIcon;
    }

    public class OnemanChargeSearchTargetCheck
    {
      private bool existExtArg1;
      private int search_gear_kind_id;
      private int search_element;
      private int search_job_id;
      private int search_family_id;
      private int search_character_id;
      private int search_same_character_id;
      private int search_unit_id;
      private bool existExtArg2;
      private int search_group_large_id;
      private int search_group_small_id;
      private int search_group_clothing_id;
      private int search_group_generation_id;
      private int search_skill_group_id;

      public OnemanChargeSearchTargetCheck(
        BattleFuncs.PackedSkillEffect pse,
        BattleskillEffectLogicArgumentEnum arg_search_gear_kind_id,
        BattleskillEffectLogicArgumentEnum arg_search_element,
        BattleskillEffectLogicArgumentEnum arg_search_job_id,
        BattleskillEffectLogicArgumentEnum arg_search_family_id,
        BattleskillEffectLogicArgumentEnum arg_search_character_id,
        BattleskillEffectLogicArgumentEnum arg_search_same_character_id,
        BattleskillEffectLogicArgumentEnum arg_search_unit_id,
        BattleskillEffectLogicArgumentEnum arg_search_group_large_id,
        BattleskillEffectLogicArgumentEnum arg_search_group_small_id,
        BattleskillEffectLogicArgumentEnum arg_search_group_clothing_id,
        BattleskillEffectLogicArgumentEnum arg_search_group_generation_id,
        BattleskillEffectLogicArgumentEnum arg_search_skill_group_id)
      {
        if (pse.HasKey(arg_search_gear_kind_id))
        {
          this.existExtArg1 = true;
          this.search_gear_kind_id = pse.GetInt(arg_search_gear_kind_id);
          this.search_element = pse.GetInt(arg_search_element);
          this.search_job_id = pse.GetInt(arg_search_job_id);
          this.search_family_id = pse.GetInt(arg_search_family_id);
          this.search_character_id = pse.GetInt(arg_search_character_id);
          this.search_same_character_id = pse.GetInt(arg_search_same_character_id);
          this.search_unit_id = pse.GetInt(arg_search_unit_id);
        }
        if (!pse.HasKey(arg_search_group_large_id))
          return;
        this.existExtArg2 = true;
        this.search_group_large_id = pse.GetInt(arg_search_group_large_id);
        this.search_group_small_id = pse.GetInt(arg_search_group_small_id);
        this.search_group_clothing_id = pse.GetInt(arg_search_group_clothing_id);
        this.search_group_generation_id = pse.GetInt(arg_search_group_generation_id);
        this.search_skill_group_id = pse.GetInt(arg_search_skill_group_id);
      }

      public bool DoCheck(BL.ISkillEffectListUnit u)
      {
        return (!this.existExtArg1 || (this.search_gear_kind_id == 0 || this.search_gear_kind_id == u.originalUnit.unit.kind.ID) && (this.search_element == 0 || (CommonElement) this.search_element == u.originalUnit.playerUnit.GetElement()) && (this.search_job_id == 0 || this.search_job_id == u.originalUnit.job.ID) && (this.search_family_id == 0 || u.originalUnit.playerUnit.HasFamily((UnitFamily) this.search_family_id)) && (this.search_character_id == 0 || this.search_character_id == u.originalUnit.unit.character.ID) && (this.search_same_character_id == 0 || this.search_same_character_id == u.originalUnit.unit.same_character_id) && (this.search_unit_id == 0 || this.search_unit_id == u.originalUnit.unit.ID)) && (!this.existExtArg2 || (this.search_group_large_id == 0 || this.search_group_large_id == u.originalUnit.unitGroup.group_large_category_id.ID) && (this.search_group_small_id == 0 || this.search_group_small_id == u.originalUnit.unitGroup.group_small_category_id.ID) && (this.search_group_clothing_id == 0 || u.originalUnit.unitGroup != null && (this.search_group_clothing_id == u.originalUnit.unitGroup.group_clothing_category_id.ID || this.search_group_clothing_id == u.originalUnit.unitGroup.group_clothing_category_id_2.ID)) && (this.search_group_generation_id == 0 || u.originalUnit.unitGroup != null && this.search_group_generation_id == u.originalUnit.unitGroup.group_generation_category_id.ID) && (this.search_skill_group_id == 0 || u.originalUnit.unit.HasSkillGroupId(this.search_skill_group_id)));
      }
    }

    private class OnemanChargeSearchTargetCheckPlayer : BattleFuncs.OnemanChargeSearchTargetCheck
    {
      public OnemanChargeSearchTargetCheckPlayer(BattleFuncs.PackedSkillEffect pse)
        : base(pse, BattleskillEffectLogicArgumentEnum.oneman_charge_player_search_gear_kind_id, BattleskillEffectLogicArgumentEnum.oneman_charge_player_search_element, BattleskillEffectLogicArgumentEnum.oneman_charge_player_search_job_id, BattleskillEffectLogicArgumentEnum.oneman_charge_player_search_family_id, BattleskillEffectLogicArgumentEnum.oneman_charge_player_search_character_id, BattleskillEffectLogicArgumentEnum.oneman_charge_player_search_same_character_id, BattleskillEffectLogicArgumentEnum.oneman_charge_player_search_unit_id, BattleskillEffectLogicArgumentEnum.oneman_charge_player_search_group_large_id, BattleskillEffectLogicArgumentEnum.oneman_charge_player_search_group_small_id, BattleskillEffectLogicArgumentEnum.oneman_charge_player_search_group_clothing_id, BattleskillEffectLogicArgumentEnum.oneman_charge_player_search_group_generation_id, BattleskillEffectLogicArgumentEnum.oneman_charge_player_search_skill_group_id)
      {
      }
    }

    private class OnemanChargeSearchTargetCheckEnemy : BattleFuncs.OnemanChargeSearchTargetCheck
    {
      public OnemanChargeSearchTargetCheckEnemy(BattleFuncs.PackedSkillEffect pse)
        : base(pse, BattleskillEffectLogicArgumentEnum.oneman_charge_enemy_search_gear_kind_id, BattleskillEffectLogicArgumentEnum.oneman_charge_enemy_search_element, BattleskillEffectLogicArgumentEnum.oneman_charge_enemy_search_job_id, BattleskillEffectLogicArgumentEnum.oneman_charge_enemy_search_family_id, BattleskillEffectLogicArgumentEnum.oneman_charge_enemy_search_character_id, BattleskillEffectLogicArgumentEnum.oneman_charge_enemy_search_same_character_id, BattleskillEffectLogicArgumentEnum.oneman_charge_enemy_search_unit_id, BattleskillEffectLogicArgumentEnum.oneman_charge_enemy_search_group_large_id, BattleskillEffectLogicArgumentEnum.oneman_charge_enemy_search_group_small_id, BattleskillEffectLogicArgumentEnum.oneman_charge_enemy_search_group_clothing_id, BattleskillEffectLogicArgumentEnum.oneman_charge_enemy_search_group_generation_id, BattleskillEffectLogicArgumentEnum.oneman_charge_enemy_search_skill_group_id)
      {
      }
    }

    private class OnemanChargeSearchTargetCheckComplex : BattleFuncs.OnemanChargeSearchTargetCheck
    {
      public OnemanChargeSearchTargetCheckComplex(BattleFuncs.PackedSkillEffect pse)
        : base(pse, BattleskillEffectLogicArgumentEnum.oneman_charge_complex_search_gear_kind_id, BattleskillEffectLogicArgumentEnum.oneman_charge_complex_search_element, BattleskillEffectLogicArgumentEnum.oneman_charge_complex_search_job_id, BattleskillEffectLogicArgumentEnum.oneman_charge_complex_search_family_id, BattleskillEffectLogicArgumentEnum.oneman_charge_complex_search_character_id, BattleskillEffectLogicArgumentEnum.oneman_charge_complex_search_same_character_id, BattleskillEffectLogicArgumentEnum.oneman_charge_complex_search_unit_id, BattleskillEffectLogicArgumentEnum.oneman_charge_complex_search_group_large_id, BattleskillEffectLogicArgumentEnum.oneman_charge_complex_search_group_small_id, BattleskillEffectLogicArgumentEnum.oneman_charge_complex_search_group_clothing_id, BattleskillEffectLogicArgumentEnum.oneman_charge_complex_search_group_generation_id, BattleskillEffectLogicArgumentEnum.oneman_charge_complex_search_skill_group_id)
      {
      }
    }

    public class EnumerableSequenceEqualityComparer<T> : IEqualityComparer<IEnumerable<T>>
    {
      public bool Equals(IEnumerable<T> sequence1, IEnumerable<T> sequence2)
      {
        if (sequence1 == null && sequence2 == null)
          return true;
        return sequence1 != null && sequence2 != null && sequence1.SequenceEqual<T>(sequence2);
      }

      public int GetHashCode(IEnumerable<T> sequence)
      {
        int hashCode = 0;
        foreach (T obj in sequence)
          hashCode ^= obj.GetHashCode();
        return hashCode;
      }
    }

    public class ApplyChangeSkillEffects
    {
      private Dictionary<BL.ISkillEffectListUnit, BattleFuncs.ApplyChangeSkillEffectsOne> dict;
      private bool isAI;
      public bool clearMovePanelCacheAll;
      public bool setMoveUnitDistance;

      public ApplyChangeSkillEffects(bool isAI)
      {
        this.dict = new Dictionary<BL.ISkillEffectListUnit, BattleFuncs.ApplyChangeSkillEffectsOne>();
        this.isAI = isAI;
        this.clearMovePanelCacheAll = false;
        this.setMoveUnitDistance = false;
      }

      public void add(BL.UnitPosition tup, BL.ISkillEffectListUnit target, bool isMoveUnit = false)
      {
        if (this.dict.ContainsKey(target))
          return;
        BattleFuncs.ApplyChangeSkillEffectsOne changeSkillEffectsOne = new BattleFuncs.ApplyChangeSkillEffectsOne(tup, target, this.isAI, isMoveUnit);
        this.dict[target] = changeSkillEffectsOne;
        changeSkillEffectsOne.doBefore();
      }

      public void add(BL.UnitPosition tup, bool isMoveUnit = false)
      {
        this.add(tup, BattleFuncs.unitPositionToISkillEffectListUnit(tup), isMoveUnit);
      }

      public void add(BL.ISkillEffectListUnit target, bool isMoveUnit = false)
      {
        this.add(BattleFuncs.iSkillEffectListUnitToUnitPosition(target), target, isMoveUnit);
      }

      public void execute()
      {
        foreach (BattleFuncs.ApplyChangeSkillEffectsOne changeSkillEffectsOne in this.dict.Values)
        {
          changeSkillEffectsOne.doAfter(false, this.setMoveUnitDistance);
          this.clearMovePanelCacheAll |= changeSkillEffectsOne.clearMovePanelCacheAll;
        }
        if (!this.clearMovePanelCacheAll)
          return;
        BattleFuncs.getAllUnits(this.isAI, true).ForEach<BL.ISkillEffectListUnit>((Action<BL.ISkillEffectListUnit>) (x => BattleFuncs.iSkillEffectListUnitToUnitPosition(x).clearMovePanelCache()));
      }
    }

    public class ApplyChangeSkillEffectsOne
    {
      private BL.UnitPosition tup;
      private BL.ISkillEffectListUnit target;
      private bool isAI;
      private bool isMoveUnit;
      public bool clearMovePanelCacheAll;
      private int backupMove;
      private bool enabledIgnoreMoveCost;
      private bool enabledSlipThru;
      private bool isDontMove;
      private List<BL.SkillEffect> charismaEffect;
      private List<BL.SkillEffect> noSealedCharismaEffect;
      private BL.SkillEffect[] enabledZocEffect;
      private int[] attackRange;
      private int[] healRange;
      private Dictionary<BL.ISkillEffectListUnit, BL.SkillEffect[]> noSealedRangeEffect;
      private BL.SkillEffect[] landTagEffect;
      private HashSet<BL.Panel> movePanels;

      public ApplyChangeSkillEffectsOne(
        BL.UnitPosition tup,
        BL.ISkillEffectListUnit target,
        bool isAI,
        bool isMoveUnit = false)
      {
        this.tup = tup;
        this.target = target;
        this.isAI = isAI;
        this.isMoveUnit = isMoveUnit;
      }

      public ApplyChangeSkillEffectsOne(BL.UnitPosition tup, bool isMoveUnit = false)
        : this(tup, BattleFuncs.unitPositionToISkillEffectListUnit(tup), tup is BL.AIUnit, isMoveUnit)
      {
      }

      public ApplyChangeSkillEffectsOne(BL.ISkillEffectListUnit target, bool isMoveUnit = false)
        : this(BattleFuncs.iSkillEffectListUnitToUnitPosition(target), target, target is BL.AIUnit, isMoveUnit)
      {
      }

      public void doBefore()
      {
        this.clearMovePanelCacheAll = false;
        this.noSealedRangeEffect = new Dictionary<BL.ISkillEffectListUnit, BL.SkillEffect[]>();
        foreach (BL.ISkillEffectListUnit allUnit in BattleFuncs.getAllUnits(this.isAI, true))
        {
          BL.ISkillEffectListUnit unit = allUnit;
          List<BL.SkillEffect> rangeFromEffects = unit.skillEffects.GetRangeFromEffects(this.target.originalUnit);
          this.noSealedRangeEffect[unit] = rangeFromEffects == null ? new BL.SkillEffect[0] : rangeFromEffects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => !BattleFuncs.isSealedSkillEffect(unit, x))).ToArray<BL.SkillEffect>();
        }
        this.backupMove = this.tup.moveCost;
        this.enabledIgnoreMoveCost = this.target.HasEnabledSkillEffect(BattleskillEffectLogicEnum.ignore_move_cost);
        this.enabledSlipThru = this.target.HasEnabledSkillEffect(BattleskillEffectLogicEnum.slip_thru);
        this.isDontMove = this.target.IsDontMove;
        this.charismaEffect = BattleFuncs.getEnabledCharismaEffects(this.target).ToList<BL.SkillEffect>();
        this.noSealedCharismaEffect = this.charismaEffect.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => !BattleFuncs.isSealedSkillEffect(this.target, x))).ToList<BL.SkillEffect>();
        this.enabledZocEffect = this.target.enabledSkillEffect(BattleskillEffectLogicEnum.zoc).ToArray<BL.SkillEffect>();
        if (this.charismaEffect.Count > 0)
          this.tup.removePanelSkillEffects(true);
        this.attackRange = this.target.attackRange;
        this.healRange = this.target.healRange;
        if (!this.isAI)
          this.landTagEffect = this.target.skillEffects.All().Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.effect.GetPackedSkillEffect().HasKey(BattleskillEffectLogicArgumentEnum.land_tag1))).ToArray<BL.SkillEffect>();
        if (!this.isMoveUnit || this.isAI)
          return;
        this.movePanels = new HashSet<BL.Panel>((IEnumerable<BL.Panel>) this.tup.movePanels);
      }

      public void doAfter(bool isClearMovePanelCacheAll = true, bool setMoveUnitDistance = false)
      {
        int moveCost = this.tup.moveCost;
        bool flag = !this.isAI && this.isMoveUnit && moveCost < this.backupMove | setMoveUnitDistance;
        if (flag)
        {
          HashSet<BL.Panel> movePanels = this.tup.movePanels;
          this.tup.movePanels = this.movePanels;
          RecoveryUtility.setMoveDistance(this.tup, this.tup.row, this.tup.column, BattleFuncs.env);
          this.tup.movePanels = movePanels;
        }
        foreach (KeyValuePair<BL.ISkillEffectListUnit, BL.SkillEffect[]> keyValuePair in this.noSealedRangeEffect)
        {
          BL.ISkillEffectListUnit unit = keyValuePair.Key;
          BL.SkillEffect[] first = keyValuePair.Value;
          List<BL.SkillEffect> rangeFromEffects = unit.skillEffects.GetRangeFromEffects(this.target.originalUnit);
          BL.SkillEffect[] second = rangeFromEffects == null ? new BL.SkillEffect[0] : rangeFromEffects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => !BattleFuncs.isSealedSkillEffect(unit, x))).ToArray<BL.SkillEffect>();
          IEnumerable<BL.SkillEffect> source = ((IEnumerable<BL.SkillEffect>) first).Union<BL.SkillEffect>((IEnumerable<BL.SkillEffect>) second).Except<BL.SkillEffect>(((IEnumerable<BL.SkillEffect>) first).Intersect<BL.SkillEffect>((IEnumerable<BL.SkillEffect>) second));
          if (source.Any<BL.SkillEffect>())
          {
            unit.skillEffects.commit();
            if (!this.isAI)
              unit.originalUnit.commit();
            if (source.Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.effect.EffectLogic.Enum == BattleskillEffectLogicEnum.ignore_move_cost || x.effect.EffectLogic.Enum == BattleskillEffectLogicEnum.slip_thru || x.effect.EffectLogic.opt_test4 == 8)))
              BattleFuncs.iSkillEffectListUnitToUnitPosition(unit).clearMovePanelCache();
            if (source.Any<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.effect.EffectLogic.Enum == BattleskillEffectLogicEnum.fix_range)))
            {
              BL.UnitPosition unitPosition = BattleFuncs.iSkillEffectListUnitToUnitPosition(unit);
              unitPosition.clearMoveActionRangePanelCache();
              unitPosition.clearMoveHealRangePanelCache();
            }
          }
        }
        if (moveCost != this.backupMove || this.isDontMove != this.target.IsDontMove || this.enabledIgnoreMoveCost != this.target.HasEnabledSkillEffect(BattleskillEffectLogicEnum.ignore_move_cost) || this.enabledSlipThru != this.target.HasEnabledSkillEffect(BattleskillEffectLogicEnum.slip_thru))
          this.tup.clearMovePanelCache();
        int[] attackRange = this.target.attackRange;
        int[] healRange = this.target.healRange;
        if (this.attackRange[0] != attackRange[0] || this.attackRange[1] != attackRange[1])
          this.tup.clearMoveActionRangePanelCache();
        if (this.healRange.Length != healRange.Length || this.healRange.Length == 2 && (this.healRange[0] != healRange[0] || this.healRange[1] != healRange[1]))
          this.tup.clearMoveHealRangePanelCache();
        List<BL.SkillEffect> list1 = BattleFuncs.getEnabledCharismaEffects(this.target).ToList<BL.SkillEffect>();
        IEnumerable<BL.SkillEffect> source1 = this.charismaEffect.Union<BL.SkillEffect>((IEnumerable<BL.SkillEffect>) list1).Except<BL.SkillEffect>(this.charismaEffect.Intersect<BL.SkillEffect>((IEnumerable<BL.SkillEffect>) list1));
        if (list1.Count > 0)
        {
          if (source1.Any<BL.SkillEffect>())
            this.tup.commitPanelSkillEffects(this.charismaEffect.Except<BL.SkillEffect>((IEnumerable<BL.SkillEffect>) list1));
          this.tup.addPanelSkillEffects(!source1.Any<BL.SkillEffect>());
        }
        else
          this.tup.commitPanelSkillEffects((IEnumerable<BL.SkillEffect>) this.charismaEffect);
        List<BL.SkillEffect> list2 = list1.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => !BattleFuncs.isSealedSkillEffect(this.target, x))).ToList<BL.SkillEffect>();
        this.tup.commitPanelSkillEffects(this.noSealedCharismaEffect.Union<BL.SkillEffect>((IEnumerable<BL.SkillEffect>) list2).Except<BL.SkillEffect>(this.noSealedCharismaEffect.Intersect<BL.SkillEffect>((IEnumerable<BL.SkillEffect>) list2)));
        BL.SkillEffect[] array1 = this.target.enabledSkillEffect(BattleskillEffectLogicEnum.zoc).ToArray<BL.SkillEffect>();
        if (((IEnumerable<BL.SkillEffect>) this.enabledZocEffect).Union<BL.SkillEffect>((IEnumerable<BL.SkillEffect>) array1).Except<BL.SkillEffect>(((IEnumerable<BL.SkillEffect>) this.enabledZocEffect).Intersect<BL.SkillEffect>((IEnumerable<BL.SkillEffect>) array1)).Any<BL.SkillEffect>())
        {
          BL.SkillEffect[] array2 = ((IEnumerable<BL.SkillEffect>) this.enabledZocEffect).Except<BL.SkillEffect>((IEnumerable<BL.SkillEffect>) array1).ToArray<BL.SkillEffect>();
          if (array2.Length != 0)
          {
            foreach (BL.Panel skillEffectZocPanel in BattleFuncs.getSkillEffectZocPanels((IEnumerable<BL.SkillEffect>) array2, this.tup.row, this.tup.column, this.isAI))
              skillEffectZocPanel.removeZocUnit(this.target.originalUnit, this.isAI);
          }
          if (array1.Length != 0)
          {
            foreach (BL.Panel skillEffectZocPanel in BattleFuncs.getSkillEffectZocPanels((IEnumerable<BL.SkillEffect>) array1, this.tup.row, this.tup.column, this.isAI))
              skillEffectZocPanel.addZocUnit(this.target.originalUnit, this.isAI);
          }
          this.clearMovePanelCacheAll = true;
        }
        this.target.skillEffects.ResetHasEnhancedElement();
        if (isClearMovePanelCacheAll && this.clearMovePanelCacheAll)
          BattleFuncs.getAllUnits(this.isAI, true).ForEach<BL.ISkillEffectListUnit>((Action<BL.ISkillEffectListUnit>) (x => BattleFuncs.iSkillEffectListUnitToUnitPosition(x).clearMovePanelCache()));
        if (!this.isAI)
        {
          BL.SkillEffect[] array3 = this.target.skillEffects.All().Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.effect.GetPackedSkillEffect().HasKey(BattleskillEffectLogicArgumentEnum.land_tag1))).ToArray<BL.SkillEffect>();
          if (((IEnumerable<BL.SkillEffect>) this.landTagEffect).Union<BL.SkillEffect>((IEnumerable<BL.SkillEffect>) array3).Except<BL.SkillEffect>(((IEnumerable<BL.SkillEffect>) this.landTagEffect).Intersect<BL.SkillEffect>((IEnumerable<BL.SkillEffect>) array3)).Any<BL.SkillEffect>())
            this.target.skillEffects.LandTagModified.commit();
        }
        if (!flag)
          return;
        RecoveryUtility.resetPosition(this.tup, this.tup.row, this.tup.column, BattleFuncs.env, true);
      }
    }

    public class CheckInvokeGeneric
    {
      private List<BattleFuncs.CheckInvokeGeneric.CheckPattern>[] checkPatterns;
      private bool cantSeal;
      private bool existLandTag;
      private bool existTargetLandTag;
      private BattleFuncs.PackedSkillEffect pse;

      public CheckInvokeGeneric(
        BattleFuncs.PackedSkillEffect pse,
        Func<BattleskillEffectLogicArgumentEnum, BattleFuncs.CheckInvokeGeneric.CheckPattern> extraFunc = null)
      {
        bool flag = pse.skillEffect == null;
        int length = flag ? 1 : 3;
        this.pse = pse;
        this.checkPatterns = new List<BattleFuncs.CheckInvokeGeneric.CheckPattern>[length];
        for (int index = 0; index < length; ++index)
          this.checkPatterns[index] = new List<BattleFuncs.CheckInvokeGeneric.CheckPattern>();
        foreach (BattleskillEffect effect in pse.GetEffects())
        {
          BattleskillEffectLogic effectLogic = effect.EffectLogic;
          for (int index = 0; index < 10; ++index)
          {
            BattleskillEffectLogicArgumentEnum logicArgumentEnum;
            switch (index)
            {
              case 0:
                logicArgumentEnum = effectLogic.arg1;
                break;
              case 1:
                logicArgumentEnum = effectLogic.arg2;
                break;
              case 2:
                logicArgumentEnum = effectLogic.arg3;
                break;
              case 3:
                logicArgumentEnum = effectLogic.arg4;
                break;
              case 4:
                logicArgumentEnum = effectLogic.arg5;
                break;
              case 5:
                logicArgumentEnum = effectLogic.arg6;
                break;
              case 6:
                logicArgumentEnum = effectLogic.arg7;
                break;
              case 7:
                logicArgumentEnum = effectLogic.arg8;
                break;
              case 8:
                logicArgumentEnum = effectLogic.arg9;
                break;
              case 9:
                logicArgumentEnum = effectLogic.arg10;
                break;
              default:
                logicArgumentEnum = BattleskillEffectLogicArgumentEnum.none;
                break;
            }
            if (logicArgumentEnum != BattleskillEffectLogicArgumentEnum.none && logicArgumentEnum != (BattleskillEffectLogicArgumentEnum) 2147483647)
            {
              BattleFuncs.CheckInvokeGeneric.CheckPattern checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) null;
              switch (logicArgumentEnum)
              {
                case BattleskillEffectLogicArgumentEnum.element:
                  checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternElement.Create(effect);
                  break;
                case BattleskillEffectLogicArgumentEnum.end_turn:
                  if (!flag)
                  {
                    checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternEndTurn.Create(effect);
                    break;
                  }
                  break;
                case BattleskillEffectLogicArgumentEnum.equip_gear_king_id:
                  checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternEquipGearKindId.Create(effect, true);
                  break;
                case BattleskillEffectLogicArgumentEnum.family_id:
                  checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternFamilyId.Create(effect);
                  break;
                case BattleskillEffectLogicArgumentEnum.gear_kind_id:
                  checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternGearKindId.Create(effect);
                  break;
                case BattleskillEffectLogicArgumentEnum.invoke_gamemode:
                  checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternInvokeGamemode.Create(effect);
                  break;
                case BattleskillEffectLogicArgumentEnum.is_attack:
                  if (!flag)
                  {
                    checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternIsAttack.Create(effect);
                    break;
                  }
                  break;
                case BattleskillEffectLogicArgumentEnum.is_attack_nc:
                  if (!flag)
                  {
                    checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternIsAttack.Create(effect, true);
                    break;
                  }
                  break;
                case BattleskillEffectLogicArgumentEnum.job_id:
                  checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternJobId.Create(effect);
                  break;
                case BattleskillEffectLogicArgumentEnum.start_turn:
                  if (!flag)
                  {
                    checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternStartTurn.Create(effect);
                    break;
                  }
                  break;
                case BattleskillEffectLogicArgumentEnum.target_element:
                  if (!flag)
                  {
                    checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternTargetElement.Create(effect);
                    break;
                  }
                  break;
                case BattleskillEffectLogicArgumentEnum.target_family_id:
                  if (!flag)
                  {
                    checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternTargetFamilyId.Create(effect);
                    break;
                  }
                  break;
                case BattleskillEffectLogicArgumentEnum.target_gear_kind_id:
                  if (!flag)
                  {
                    checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternTargetGearKindId.Create(effect);
                    break;
                  }
                  break;
                case BattleskillEffectLogicArgumentEnum.target_job_id:
                  if (!flag)
                  {
                    checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternTargetJobId.Create(effect);
                    break;
                  }
                  break;
                case BattleskillEffectLogicArgumentEnum.turn_cycle:
                  if (!flag)
                  {
                    checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternTurnCycle.Create(effect, pse);
                    break;
                  }
                  break;
                case BattleskillEffectLogicArgumentEnum.group_large_id:
                  checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternGroupLargeId.Create(effect);
                  break;
                case BattleskillEffectLogicArgumentEnum.target_group_large_id:
                  if (!flag)
                  {
                    checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternTargetGroupLargeId.Create(effect);
                    break;
                  }
                  break;
                case BattleskillEffectLogicArgumentEnum.group_small_id:
                  checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternGroupSmallId.Create(effect);
                  break;
                case BattleskillEffectLogicArgumentEnum.target_group_small_id:
                  if (!flag)
                  {
                    checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternTargetGroupSmallId.Create(effect);
                    break;
                  }
                  break;
                case BattleskillEffectLogicArgumentEnum.group_clothing_id:
                  checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternGroupClothingId.Create(effect);
                  break;
                case BattleskillEffectLogicArgumentEnum.target_group_clothing_id:
                  if (!flag)
                  {
                    checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternTargetGroupClothingId.Create(effect);
                    break;
                  }
                  break;
                case BattleskillEffectLogicArgumentEnum.group_generation_id:
                  checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternGroupGenerationId.Create(effect);
                  break;
                case BattleskillEffectLogicArgumentEnum.target_group_generation_id:
                  if (!flag)
                  {
                    checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternTargetGroupGenerationId.Create(effect);
                    break;
                  }
                  break;
                case BattleskillEffectLogicArgumentEnum.skill_group_id:
                  checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternSkillGroupId.Create(effect);
                  break;
                case BattleskillEffectLogicArgumentEnum.exclude_skill_group_id:
                  checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternExcludeSkillGroupId.Create(effect);
                  break;
                case BattleskillEffectLogicArgumentEnum.target_skill_group_id:
                  if (!flag)
                  {
                    checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternTargetSkillGroupId.Create(effect);
                    break;
                  }
                  break;
                case BattleskillEffectLogicArgumentEnum.target_exclude_skill_group_id:
                  if (!flag)
                  {
                    checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternTargetExcludeSkillGroupId.Create(effect);
                    break;
                  }
                  break;
                case BattleskillEffectLogicArgumentEnum.target_equip_gear_kind_id:
                  if (!flag)
                  {
                    checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternTargetEquipGearKindId.Create(effect);
                    break;
                  }
                  break;
                case BattleskillEffectLogicArgumentEnum.min_value:
                  if (!flag)
                    checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternParamType.Create(effect);
                  index += 3;
                  break;
                case BattleskillEffectLogicArgumentEnum.land_tag1:
                  if (!flag)
                    this.existLandTag = true;
                  index += 9;
                  break;
                case BattleskillEffectLogicArgumentEnum.exclude_gear_kind_id:
                  checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternExcludeGearKindId.Create(effect);
                  break;
                case BattleskillEffectLogicArgumentEnum.exclude_element:
                  checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternExcludeElement.Create(effect);
                  break;
                case BattleskillEffectLogicArgumentEnum.exclude_job_id:
                  checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternExcludeJobId.Create(effect);
                  break;
                case BattleskillEffectLogicArgumentEnum.exclude_family_id:
                  checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternExcludeFamilyId.Create(effect);
                  break;
                case BattleskillEffectLogicArgumentEnum.target_exclude_gear_kind_id:
                  if (!flag)
                  {
                    checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternTargetExcludeGearKindId.Create(effect);
                    break;
                  }
                  break;
                case BattleskillEffectLogicArgumentEnum.target_exclude_element:
                  if (!flag)
                  {
                    checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternTargetExcludeElement.Create(effect);
                    break;
                  }
                  break;
                case BattleskillEffectLogicArgumentEnum.target_exclude_job_id:
                  if (!flag)
                  {
                    checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternTargetExcludeJobId.Create(effect);
                    break;
                  }
                  break;
                case BattleskillEffectLogicArgumentEnum.target_exclude_family_id:
                  if (!flag)
                  {
                    checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternTargetExcludeFamilyId.Create(effect);
                    break;
                  }
                  break;
                case BattleskillEffectLogicArgumentEnum.start_turn2:
                  if (!flag)
                  {
                    checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternStartTurn2.Create(effect, pse);
                    break;
                  }
                  break;
                case BattleskillEffectLogicArgumentEnum.turn_cycle2:
                  if (!flag)
                  {
                    checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternTurnCycle2.Create(effect, pse);
                    break;
                  }
                  break;
                case BattleskillEffectLogicArgumentEnum.oneman_charge_player_min_range:
                  if (!flag)
                    checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternOnemanChargePlayer.Create(effect, pse);
                  index += 3;
                  break;
                case BattleskillEffectLogicArgumentEnum.oneman_charge_enemy_min_range:
                  if (!flag)
                    checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternOnemanChargeEnemy.Create(effect, pse);
                  index += 3;
                  break;
                case BattleskillEffectLogicArgumentEnum.oneman_charge_complex_min_range:
                  if (!flag)
                    checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternOnemanChargeComplex.Create(effect, pse);
                  index += 3;
                  break;
                case BattleskillEffectLogicArgumentEnum.target_land_tag1:
                  if (!flag)
                    this.existTargetLandTag = true;
                  index += 9;
                  break;
                case BattleskillEffectLogicArgumentEnum.peculiar_parameter_min:
                  checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternPeculiarParameter.Create(effect);
                  ++index;
                  break;
                case BattleskillEffectLogicArgumentEnum.target_peculiar_parameter_min:
                  if (!flag)
                    checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternTargetPeculiarParameter.Create(effect);
                  ++index;
                  break;
                case BattleskillEffectLogicArgumentEnum.level_up_status_min:
                  checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternLevelUpStatus.Create(effect);
                  ++index;
                  break;
                case BattleskillEffectLogicArgumentEnum.target_level_up_status_type:
                  checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternTargetLevelUpStatus.Create(effect);
                  index += 2;
                  break;
                case BattleskillEffectLogicArgumentEnum.cant_seal:
                  if (!flag)
                  {
                    this.cantSeal = effect.GetInt(BattleskillEffectLogicArgumentEnum.cant_seal) == 1;
                    break;
                  }
                  break;
                case BattleskillEffectLogicArgumentEnum.equip_gear_kind_id:
                  checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternEquipGearKindId.Create(effect);
                  break;
                case BattleskillEffectLogicArgumentEnum.exclude_equip_gear_kind_id:
                  checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternExcludeEquipGearKindId.Create(effect);
                  break;
                case BattleskillEffectLogicArgumentEnum.target_exclude_equip_gear_kind_id:
                  if (!flag)
                  {
                    checkPattern = (BattleFuncs.CheckInvokeGeneric.CheckPattern) BattleFuncs.CheckInvokeGeneric.CheckPatternTargetExcludeEquipGearKindId.Create(effect);
                    break;
                  }
                  break;
                default:
                  if (extraFunc != null)
                  {
                    checkPattern = extraFunc(logicArgumentEnum);
                    break;
                  }
                  break;
              }
              if (checkPattern != null)
              {
                int category = (int) checkPattern.GetCategory();
                if (category < length)
                  this.checkPatterns[category].Add(checkPattern);
              }
            }
            else
              break;
          }
        }
      }

      public bool DoCheckFix(
        BL.ISkillEffectListUnit unit,
        bool isColosseum,
        bool isCheckPassiveEffectEnable)
      {
        List<BattleFuncs.CheckInvokeGeneric.CheckPattern> checkPattern1 = this.checkPatterns[0];
        if (!checkPattern1.Any<BattleFuncs.CheckInvokeGeneric.CheckPattern>())
          return true;
        foreach (BattleFuncs.CheckInvokeGeneric.CheckPattern checkPattern2 in checkPattern1)
        {
          switch (checkPattern2.GetPattern())
          {
            case 0:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternInvokeGamemode) checkPattern2).DoCheck(unit, isColosseum))
                return false;
              continue;
            case 1:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternEquipGearKindId) checkPattern2).DoCheck(unit))
                return false;
              continue;
            case 2:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternExcludeEquipGearKindId) checkPattern2).DoCheck(unit))
                return false;
              continue;
            case 3:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternGearKindId) checkPattern2).DoCheck(unit))
                return false;
              continue;
            case 4:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternExcludeGearKindId) checkPattern2).DoCheck(unit))
                return false;
              continue;
            case 5:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternElement) checkPattern2).DoCheck(unit, isCheckPassiveEffectEnable))
                return false;
              continue;
            case 6:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternExcludeElement) checkPattern2).DoCheck(unit, isCheckPassiveEffectEnable))
                return false;
              continue;
            case 7:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternJobId) checkPattern2).DoCheck(unit))
                return false;
              continue;
            case 8:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternExcludeJobId) checkPattern2).DoCheck(unit))
                return false;
              continue;
            case 9:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternFamilyId) checkPattern2).DoCheck(unit))
                return false;
              continue;
            case 10:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternExcludeFamilyId) checkPattern2).DoCheck(unit))
                return false;
              continue;
            case 11:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternGroupLargeId) checkPattern2).DoCheck(unit))
                return false;
              continue;
            case 12:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternGroupSmallId) checkPattern2).DoCheck(unit))
                return false;
              continue;
            case 13:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternGroupClothingId) checkPattern2).DoCheck(unit))
                return false;
              continue;
            case 14:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternGroupGenerationId) checkPattern2).DoCheck(unit))
                return false;
              continue;
            case 15:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternSkillGroupId) checkPattern2).DoCheck(unit))
                return false;
              continue;
            case 16:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternExcludeSkillGroupId) checkPattern2).DoCheck(unit))
                return false;
              continue;
            case 17:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternPeculiarParameter) checkPattern2).DoCheck(unit))
                return false;
              continue;
            case 18:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternLevelUpStatus) checkPattern2).DoCheck(unit))
                return false;
              continue;
            default:
              continue;
          }
        }
        return true;
      }

      public bool DoCheck(
        BL.ISkillEffectListUnit unit,
        BL.ISkillEffectListUnit target = null,
        int? colosseumTurn = null,
        Judgement.NonBattleParameter.FromPlayerUnitCache unitNbpCache = null,
        Judgement.NonBattleParameter.FromPlayerUnitCache targetNbpCache = null,
        int? unitHp = null,
        int? targetHp = null,
        int attackType = 0,
        BL.Panel unitPanel = null,
        BL.Panel targetPanel = null,
        BL.SkillEffect effect = null,
        Func<List<BattleFuncs.CheckInvokeGeneric.CheckPattern>, bool> extraFunc = null)
      {
        bool hasValue = colosseumTurn.HasValue;
        bool isAI = unit is BL.AIUnit;
        foreach (BattleFuncs.CheckInvokeGeneric.CheckPattern checkPattern in this.checkPatterns[1])
        {
          switch (checkPattern.GetPattern())
          {
            case 0:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternIsAttack) checkPattern).DoCheck(attackType))
                return false;
              continue;
            case 1:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternStartTurn) checkPattern).DoCheck(colosseumTurn))
                return false;
              continue;
            case 2:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternEndTurn) checkPattern).DoCheck(colosseumTurn))
                return false;
              continue;
            case 3:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternTurnCycle) checkPattern).DoCheck(colosseumTurn))
                return false;
              continue;
            case 4:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternStartTurn2) checkPattern).DoCheck(colosseumTurn))
                return false;
              continue;
            case 5:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternTurnCycle2) checkPattern).DoCheck(colosseumTurn))
                return false;
              continue;
            case 6:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternTargetEquipGearKindId) checkPattern).DoCheck(target))
                return false;
              continue;
            case 7:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternTargetExcludeEquipGearKindId) checkPattern).DoCheck(target))
                return false;
              continue;
            case 8:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternTargetGearKindId) checkPattern).DoCheck(target))
                return false;
              continue;
            case 9:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternTargetExcludeGearKindId) checkPattern).DoCheck(target))
                return false;
              continue;
            case 10:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternTargetElement) checkPattern).DoCheck(target))
                return false;
              continue;
            case 11:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternTargetExcludeElement) checkPattern).DoCheck(target))
                return false;
              continue;
            case 12:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternTargetJobId) checkPattern).DoCheck(target))
                return false;
              continue;
            case 13:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternTargetExcludeJobId) checkPattern).DoCheck(target))
                return false;
              continue;
            case 14:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternTargetFamilyId) checkPattern).DoCheck(target))
                return false;
              continue;
            case 15:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternTargetExcludeFamilyId) checkPattern).DoCheck(target))
                return false;
              continue;
            case 16:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternTargetGroupLargeId) checkPattern).DoCheck(target))
                return false;
              continue;
            case 17:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternTargetGroupSmallId) checkPattern).DoCheck(target))
                return false;
              continue;
            case 18:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternTargetGroupClothingId) checkPattern).DoCheck(target))
                return false;
              continue;
            case 19:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternTargetGroupGenerationId) checkPattern).DoCheck(target))
                return false;
              continue;
            case 20:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternTargetSkillGroupId) checkPattern).DoCheck(target))
                return false;
              continue;
            case 21:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternTargetExcludeSkillGroupId) checkPattern).DoCheck(target))
                return false;
              continue;
            case 22:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternSelfParamType) checkPattern).DoCheck(unit, unitNbpCache, unitHp))
                return false;
              continue;
            case 23:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternTargetParamType) checkPattern).DoCheck(target, targetNbpCache, targetHp))
                return false;
              continue;
            case 24:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternBothParamType) checkPattern).DoCheck(unit, target, unitNbpCache, targetNbpCache, unitHp, targetHp))
                return false;
              continue;
            case 25:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternOnemanChargePlayer) checkPattern).DoCheck(unit, hasValue))
                return false;
              continue;
            case 26:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternOnemanChargeEnemy) checkPattern).DoCheck(unit, hasValue))
                return false;
              continue;
            case 27:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternOnemanChargeComplex) checkPattern).DoCheck(unit, hasValue))
                return false;
              continue;
            case 28:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternTargetPeculiarParameter) checkPattern).DoCheck(target))
                return false;
              continue;
            case 29:
              if (!((BattleFuncs.CheckInvokeGeneric.CheckPatternTargetLevelUpStatus) checkPattern).DoCheck(target))
                return false;
              continue;
            default:
              continue;
          }
        }
        return (extraFunc == null || extraFunc(this.checkPatterns[2])) && this.DoCheckFix(unit, hasValue, false) && (this.cantSeal || effect == null || !BattleFuncs.isSealedSkillEffect(unit, effect)) && (!this.existLandTag || this.pse.CheckLandTag(unitPanel, isAI)) && (!this.existTargetLandTag || this.pse.CheckTargetLandTag(targetPanel, isAI));
      }

      public abstract class CheckPattern
      {
        public abstract BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory();

        public abstract int GetPattern();

        public enum Category
        {
          Fix,
          Var,
          Extra,
          Kind,
        }
      }

      private enum Pattern
      {
        InvokeGamemode = 0,
        IsAttack = 0,
        EquipGearKindId = 1,
        StartTurn = 1,
        EndTurn = 2,
        ExcludeEquipGearKindId = 2,
        GearKindId = 3,
        TurnCycle = 3,
        ExcludeGearKindId = 4,
        StartTurn2 = 4,
        Element = 5,
        TurnCycle2 = 5,
        ExcludeElement = 6,
        TargetEquipGearKindId = 6,
        JobId = 7,
        TargetExcludeEquipGearKindId = 7,
        ExcludeJobId = 8,
        TargetGearKindId = 8,
        FamilyId = 9,
        TargetExcludeGearKindId = 9,
        ExcludeFamilyId = 10, // 0x0000000A
        TargetElement = 10, // 0x0000000A
        GroupLargeId = 11, // 0x0000000B
        TargetExcludeElement = 11, // 0x0000000B
        GroupSmallId = 12, // 0x0000000C
        TargetJobId = 12, // 0x0000000C
        GroupClothingId = 13, // 0x0000000D
        TargetExcludeJobId = 13, // 0x0000000D
        GroupGenerationId = 14, // 0x0000000E
        TargetFamilyId = 14, // 0x0000000E
        SkillGroupId = 15, // 0x0000000F
        TargetExcludeFamilyId = 15, // 0x0000000F
        ExcludeSkillGroupId = 16, // 0x00000010
        TargetGroupLargeId = 16, // 0x00000010
        PeculiarParameter = 17, // 0x00000011
        TargetGroupSmallId = 17, // 0x00000011
        LevelUpStatus = 18, // 0x00000012
        TargetGroupClothingId = 18, // 0x00000012
        TargetGroupGenerationId = 19, // 0x00000013
        TargetSkillGroupId = 20, // 0x00000014
        TargetExcludeSkillGroupId = 21, // 0x00000015
        SelfParamType = 22, // 0x00000016
        TargetParamType = 23, // 0x00000017
        BothParamType = 24, // 0x00000018
        OnemanChargePlayer = 25, // 0x00000019
        OnemanChargeEnemy = 26, // 0x0000001A
        OnemanChargeComplex = 27, // 0x0000001B
        TargetPeculiarParameter = 28, // 0x0000001C
        TargetLevelUpStatus = 29, // 0x0000001D
      }

      private class CheckPatternIsAttack : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int isAttack;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Var;
        }

        public override int GetPattern() => 0;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternIsAttack Create(
          BattleskillEffect effect,
          bool isNonColosseum = false)
        {
          int num = effect.GetInt(!isNonColosseum ? BattleskillEffectLogicArgumentEnum.is_attack : BattleskillEffectLogicArgumentEnum.is_attack_nc);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternIsAttack) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternIsAttack()
          {
            isAttack = num
          };
        }

        public bool DoCheck(int attackType) => this.isAttack == attackType;
      }

      private class CheckPatternStartTurn : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int startTurn;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Var;
        }

        public override int GetPattern() => 1;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternStartTurn Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.start_turn);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternStartTurn) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternStartTurn()
          {
            startTurn = num
          };
        }

        public bool DoCheck(int? colosseumTurn)
        {
          int? turnCount = BattleFuncs.GetTurnCount(colosseumTurn);
          if (!turnCount.HasValue)
            return false;
          int? nullable = turnCount;
          int startTurn = this.startTurn;
          return !(nullable.GetValueOrDefault() < startTurn & nullable.HasValue);
        }
      }

      private class CheckPatternEndTurn : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int endTurn;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Var;
        }

        public override int GetPattern() => 2;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternEndTurn Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.end_turn);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternEndTurn) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternEndTurn()
          {
            endTurn = num
          };
        }

        public bool DoCheck(int? colosseumTurn)
        {
          int? turnCount = BattleFuncs.GetTurnCount(colosseumTurn);
          if (!turnCount.HasValue)
            return false;
          int? nullable = turnCount;
          int endTurn = this.endTurn;
          return !(nullable.GetValueOrDefault() >= endTurn & nullable.HasValue);
        }
      }

      private class CheckPatternTurnCycle : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int turnCycle;
        private int startTurn;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Var;
        }

        public override int GetPattern() => 3;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternTurnCycle Create(
          BattleskillEffect effect,
          BattleFuncs.PackedSkillEffect pse)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.turn_cycle);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternTurnCycle) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternTurnCycle()
          {
            turnCycle = num,
            startTurn = pse.HasKey(BattleskillEffectLogicArgumentEnum.start_turn) ? pse.GetInt(BattleskillEffectLogicArgumentEnum.start_turn) : 0
          };
        }

        public bool DoCheck(int? colosseumTurn)
        {
          int? turnCount = BattleFuncs.GetTurnCount(colosseumTurn);
          if (!turnCount.HasValue)
            return false;
          int? nullable1 = turnCount;
          int startTurn = this.startTurn;
          int? nullable2 = nullable1.HasValue ? new int?(nullable1.GetValueOrDefault() - startTurn) : new int?();
          int turnCycle = this.turnCycle;
          int? nullable3;
          if (!nullable2.HasValue)
          {
            nullable1 = new int?();
            nullable3 = nullable1;
          }
          else
            nullable3 = new int?(nullable2.GetValueOrDefault() % turnCycle);
          int? nullable4 = nullable3;
          int num = 0;
          return nullable4.GetValueOrDefault() == num & nullable4.HasValue;
        }
      }

      private class CheckPatternStartTurn2 : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int startTurn2;
        private int investTurn;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Var;
        }

        public override int GetPattern() => 4;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternStartTurn2 Create(
          BattleskillEffect effect,
          BattleFuncs.PackedSkillEffect pse)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.start_turn2);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternStartTurn2) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternStartTurn2()
          {
            startTurn2 = num,
            investTurn = pse.skillEffect != null ? pse.skillEffect.investTurn : 0
          };
        }

        public bool DoCheck(int? colosseumTurn)
        {
          int? turnCount = BattleFuncs.GetTurnCount(colosseumTurn);
          if (!turnCount.HasValue)
            return false;
          int? nullable1 = turnCount;
          int investTurn = this.investTurn;
          int? nullable2 = nullable1.HasValue ? new int?(nullable1.GetValueOrDefault() - investTurn) : new int?();
          int startTurn2 = this.startTurn2;
          return !(nullable2.GetValueOrDefault() < startTurn2 & nullable2.HasValue);
        }
      }

      private class CheckPatternTurnCycle2 : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int turnCycle2;
        private int turnOffset;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Var;
        }

        public override int GetPattern() => 5;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternTurnCycle2 Create(
          BattleskillEffect effect,
          BattleFuncs.PackedSkillEffect pse)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.turn_cycle2);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternTurnCycle2) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternTurnCycle2()
          {
            turnCycle2 = num,
            turnOffset = (pse.skillEffect != null ? pse.skillEffect.investTurn : 0) + (pse.HasKey(BattleskillEffectLogicArgumentEnum.start_turn2) ? pse.GetInt(BattleskillEffectLogicArgumentEnum.start_turn2) : 0)
          };
        }

        public bool DoCheck(int? colosseumTurn)
        {
          int? turnCount = BattleFuncs.GetTurnCount(colosseumTurn);
          if (!turnCount.HasValue)
            return false;
          int? nullable1 = turnCount;
          int turnOffset = this.turnOffset;
          int? nullable2 = nullable1.HasValue ? new int?(nullable1.GetValueOrDefault() - turnOffset) : new int?();
          int turnCycle2 = this.turnCycle2;
          int? nullable3;
          if (!nullable2.HasValue)
          {
            nullable1 = new int?();
            nullable3 = nullable1;
          }
          else
            nullable3 = new int?(nullable2.GetValueOrDefault() % turnCycle2);
          int? nullable4 = nullable3;
          int num = 0;
          return nullable4.GetValueOrDefault() == num & nullable4.HasValue;
        }
      }

      private class CheckPatternInvokeGamemode : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int invokeGamemode;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Fix;
        }

        public override int GetPattern() => 0;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternInvokeGamemode Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.invoke_gamemode);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternInvokeGamemode) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternInvokeGamemode()
          {
            invokeGamemode = num
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit unit, bool isColosseum)
        {
          return BattleFuncs.checkInvokeGamemode(this.invokeGamemode, isColosseum, unit);
        }
      }

      private class CheckPatternEquipGearKindId : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int equipGearKindId;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Fix;
        }

        public override int GetPattern() => 1;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternEquipGearKindId Create(
          BattleskillEffect effect,
          bool isKing = false)
        {
          int num = effect.GetInt(isKing ? BattleskillEffectLogicArgumentEnum.equip_gear_king_id : BattleskillEffectLogicArgumentEnum.equip_gear_kind_id);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternEquipGearKindId) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternEquipGearKindId()
          {
            equipGearKindId = num
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit unit)
        {
          return unit != null && BattleFuncs.isGearEquipped(unit.originalUnit.playerUnit, this.equipGearKindId);
        }
      }

      private class CheckPatternTargetEquipGearKindId : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int targetEquipGearKindId;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Var;
        }

        public override int GetPattern() => 6;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternTargetEquipGearKindId Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.target_equip_gear_kind_id);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternTargetEquipGearKindId) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternTargetEquipGearKindId()
          {
            targetEquipGearKindId = num
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit target)
        {
          return target != null && BattleFuncs.isGearEquipped(target.originalUnit.playerUnit, this.targetEquipGearKindId);
        }
      }

      private class CheckPatternExcludeEquipGearKindId : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int excludeEquipGearKindId;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Fix;
        }

        public override int GetPattern() => 2;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternExcludeEquipGearKindId Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.exclude_equip_gear_kind_id);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternExcludeEquipGearKindId) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternExcludeEquipGearKindId()
          {
            excludeEquipGearKindId = num
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit unit)
        {
          return unit != null && !BattleFuncs.isGearEquipped(unit.originalUnit.playerUnit, this.excludeEquipGearKindId);
        }
      }

      private class CheckPatternTargetExcludeEquipGearKindId : 
        BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int targetExcludeEquipGearKindId;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Var;
        }

        public override int GetPattern() => 7;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternTargetExcludeEquipGearKindId Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.target_exclude_equip_gear_kind_id);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternTargetExcludeEquipGearKindId) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternTargetExcludeEquipGearKindId()
          {
            targetExcludeEquipGearKindId = num
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit target)
        {
          return target != null && !BattleFuncs.isGearEquipped(target.originalUnit.playerUnit, this.targetExcludeEquipGearKindId);
        }
      }

      private class CheckPatternGearKindId : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int gearKindId;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Fix;
        }

        public override int GetPattern() => 3;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternGearKindId Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternGearKindId) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternGearKindId()
          {
            gearKindId = num
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit unit)
        {
          return unit != null && this.gearKindId == unit.originalUnit.unit.kind.ID;
        }
      }

      private class CheckPatternTargetGearKindId : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int targetGearKindId;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Var;
        }

        public override int GetPattern() => 8;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternTargetGearKindId Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternTargetGearKindId) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternTargetGearKindId()
          {
            targetGearKindId = num
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit target)
        {
          return target != null && this.targetGearKindId == target.originalUnit.unit.kind.ID;
        }
      }

      private class CheckPatternExcludeGearKindId : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int excludeGearKindId;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Fix;
        }

        public override int GetPattern() => 4;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternExcludeGearKindId Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.exclude_gear_kind_id);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternExcludeGearKindId) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternExcludeGearKindId()
          {
            excludeGearKindId = num
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit unit)
        {
          return unit != null && this.excludeGearKindId != unit.originalUnit.unit.kind.ID;
        }
      }

      private class CheckPatternTargetExcludeGearKindId : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int targetExcludeGearKindId;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Var;
        }

        public override int GetPattern() => 9;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternTargetExcludeGearKindId Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.target_exclude_gear_kind_id);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternTargetExcludeGearKindId) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternTargetExcludeGearKindId()
          {
            targetExcludeGearKindId = num
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit target)
        {
          return target != null && this.targetExcludeGearKindId != target.originalUnit.unit.kind.ID;
        }
      }

      private class CheckPatternElement : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int element;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Fix;
        }

        public override int GetPattern() => 5;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternElement Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.element);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternElement) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternElement()
          {
            element = num
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit unit, bool isCheckPassiveEffectEnable)
        {
          if (!isCheckPassiveEffectEnable)
          {
            if (!BattleFuncs.checkElement(unit, this.element))
              return false;
          }
          else if (!BattleFuncs.checkPassiveEffectEnabledElement(unit, this.element))
            return false;
          return true;
        }
      }

      private class CheckPatternTargetElement : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int targetElement;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Var;
        }

        public override int GetPattern() => 10;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternTargetElement Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternTargetElement) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternTargetElement()
          {
            targetElement = num
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit target)
        {
          return BattleFuncs.checkElement(target, this.targetElement);
        }
      }

      private class CheckPatternExcludeElement : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int excludeElement;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Fix;
        }

        public override int GetPattern() => 6;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternExcludeElement Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.exclude_element);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternExcludeElement) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternExcludeElement()
          {
            excludeElement = num
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit unit, bool isCheckPassiveEffectEnable)
        {
          if (!isCheckPassiveEffectEnable)
          {
            if (!BattleFuncs.checkElement(unit, this.excludeElement, false))
              return false;
          }
          else if (!BattleFuncs.checkPassiveEffectEnabledElement(unit, this.excludeElement, false))
            return false;
          return true;
        }
      }

      private class CheckPatternTargetExcludeElement : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int targetExcludeElement;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Var;
        }

        public override int GetPattern() => 11;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternTargetExcludeElement Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.target_exclude_element);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternTargetExcludeElement) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternTargetExcludeElement()
          {
            targetExcludeElement = num
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit target)
        {
          return BattleFuncs.checkElement(target, this.targetExcludeElement, false);
        }
      }

      private class CheckPatternJobId : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int jobId;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Fix;
        }

        public override int GetPattern() => 7;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternJobId Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternJobId) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternJobId()
          {
            jobId = num
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit unit)
        {
          return unit != null && this.jobId == unit.originalUnit.job.ID;
        }
      }

      private class CheckPatternTargetJobId : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int targetJobId;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Var;
        }

        public override int GetPattern() => 12;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternTargetJobId Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternTargetJobId) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternTargetJobId()
          {
            targetJobId = num
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit target)
        {
          return target != null && this.targetJobId == target.originalUnit.job.ID;
        }
      }

      private class CheckPatternExcludeJobId : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int excludeJobId;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Fix;
        }

        public override int GetPattern() => 8;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternExcludeJobId Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.exclude_job_id);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternExcludeJobId) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternExcludeJobId()
          {
            excludeJobId = num
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit unit)
        {
          return unit != null && this.excludeJobId != unit.originalUnit.job.ID;
        }
      }

      private class CheckPatternTargetExcludeJobId : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int targetExcludeJobId;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Var;
        }

        public override int GetPattern() => 13;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternTargetExcludeJobId Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.target_exclude_job_id);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternTargetExcludeJobId) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternTargetExcludeJobId()
          {
            targetExcludeJobId = num
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit target)
        {
          return target != null && this.targetExcludeJobId != target.originalUnit.job.ID;
        }
      }

      private class CheckPatternFamilyId : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int familyId;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Fix;
        }

        public override int GetPattern() => 9;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternFamilyId Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternFamilyId) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternFamilyId()
          {
            familyId = num
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit unit)
        {
          return unit != null && unit.originalUnit.playerUnit.HasFamily((UnitFamily) this.familyId);
        }
      }

      private class CheckPatternTargetFamilyId : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int targetFamilyId;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Var;
        }

        public override int GetPattern() => 14;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternTargetFamilyId Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternTargetFamilyId) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternTargetFamilyId()
          {
            targetFamilyId = num
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit target)
        {
          return target != null && target.originalUnit.playerUnit.HasFamily((UnitFamily) this.targetFamilyId);
        }
      }

      private class CheckPatternExcludeFamilyId : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int excludeFamilyId;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Fix;
        }

        public override int GetPattern() => 10;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternExcludeFamilyId Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.exclude_family_id);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternExcludeFamilyId) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternExcludeFamilyId()
          {
            excludeFamilyId = num
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit unit)
        {
          return unit != null && !unit.originalUnit.playerUnit.HasFamily((UnitFamily) this.excludeFamilyId);
        }
      }

      private class CheckPatternTargetExcludeFamilyId : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int targetExcludeFamilyId;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Var;
        }

        public override int GetPattern() => 15;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternTargetExcludeFamilyId Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.target_exclude_family_id);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternTargetExcludeFamilyId) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternTargetExcludeFamilyId()
          {
            targetExcludeFamilyId = num
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit target)
        {
          return target != null && !target.originalUnit.playerUnit.HasFamily((UnitFamily) this.targetExcludeFamilyId);
        }
      }

      private class CheckPatternGroupLargeId : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int groupLargeId;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Fix;
        }

        public override int GetPattern() => 11;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternGroupLargeId Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.group_large_id);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternGroupLargeId) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternGroupLargeId()
          {
            groupLargeId = num
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit unit)
        {
          return unit != null && unit.originalUnit.unitGroup != null && this.groupLargeId == unit.originalUnit.unitGroup.group_large_category_id.ID;
        }
      }

      private class CheckPatternTargetGroupLargeId : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int targetGroupLargeId;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Var;
        }

        public override int GetPattern() => 16;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternTargetGroupLargeId Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_large_id);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternTargetGroupLargeId) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternTargetGroupLargeId()
          {
            targetGroupLargeId = num
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit target)
        {
          return target != null && target.originalUnit.unitGroup != null && this.targetGroupLargeId == target.originalUnit.unitGroup.group_large_category_id.ID;
        }
      }

      private class CheckPatternGroupSmallId : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int groupSmallId;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Fix;
        }

        public override int GetPattern() => 12;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternGroupSmallId Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.group_small_id);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternGroupSmallId) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternGroupSmallId()
          {
            groupSmallId = num
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit unit)
        {
          return unit != null && unit.originalUnit.unitGroup != null && this.groupSmallId == unit.originalUnit.unitGroup.group_small_category_id.ID;
        }
      }

      private class CheckPatternTargetGroupSmallId : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int targetGroupSmallId;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Var;
        }

        public override int GetPattern() => 17;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternTargetGroupSmallId Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_small_id);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternTargetGroupSmallId) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternTargetGroupSmallId()
          {
            targetGroupSmallId = num
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit target)
        {
          return target != null && target.originalUnit.unitGroup != null && this.targetGroupSmallId == target.originalUnit.unitGroup.group_small_category_id.ID;
        }
      }

      private class CheckPatternGroupClothingId : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int groupClothingId;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Fix;
        }

        public override int GetPattern() => 13;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternGroupClothingId Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.group_clothing_id);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternGroupClothingId) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternGroupClothingId()
          {
            groupClothingId = num
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit unit)
        {
          return unit != null && unit.originalUnit.unitGroup != null && (this.groupClothingId == unit.originalUnit.unitGroup.group_clothing_category_id.ID || this.groupClothingId == unit.originalUnit.unitGroup.group_clothing_category_id_2.ID);
        }
      }

      private class CheckPatternTargetGroupClothingId : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int targetGroupClothingId;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Var;
        }

        public override int GetPattern() => 18;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternTargetGroupClothingId Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_clothing_id);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternTargetGroupClothingId) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternTargetGroupClothingId()
          {
            targetGroupClothingId = num
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit target)
        {
          return target != null && target.originalUnit.unitGroup != null && (this.targetGroupClothingId == target.originalUnit.unitGroup.group_clothing_category_id.ID || this.targetGroupClothingId == target.originalUnit.unitGroup.group_clothing_category_id_2.ID);
        }
      }

      private class CheckPatternGroupGenerationId : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int groupGenerationId;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Fix;
        }

        public override int GetPattern() => 14;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternGroupGenerationId Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.group_generation_id);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternGroupGenerationId) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternGroupGenerationId()
          {
            groupGenerationId = num
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit unit)
        {
          return unit != null && unit.originalUnit.unitGroup != null && this.groupGenerationId == unit.originalUnit.unitGroup.group_generation_category_id.ID;
        }
      }

      private class CheckPatternTargetGroupGenerationId : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int targetGroupGenerationId;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Var;
        }

        public override int GetPattern() => 19;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternTargetGroupGenerationId Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_generation_id);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternTargetGroupGenerationId) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternTargetGroupGenerationId()
          {
            targetGroupGenerationId = num
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit target)
        {
          return target != null && target.originalUnit.unitGroup != null && this.targetGroupGenerationId == target.originalUnit.unitGroup.group_generation_category_id.ID;
        }
      }

      private class CheckPatternSkillGroupId : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int skillGroupId;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Fix;
        }

        public override int GetPattern() => 15;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternSkillGroupId Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternSkillGroupId) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternSkillGroupId()
          {
            skillGroupId = num
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit unit)
        {
          return unit != null && unit.originalUnit.unit.HasSkillGroupId(this.skillGroupId);
        }
      }

      private class CheckPatternTargetSkillGroupId : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int targetSkillGroupId;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Var;
        }

        public override int GetPattern() => 20;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternTargetSkillGroupId Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.target_skill_group_id);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternTargetSkillGroupId) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternTargetSkillGroupId()
          {
            targetSkillGroupId = num
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit target)
        {
          return target != null && target.originalUnit.unit.HasSkillGroupId(this.targetSkillGroupId);
        }
      }

      private class CheckPatternExcludeSkillGroupId : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int excludeSkillGroupId;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Fix;
        }

        public override int GetPattern() => 16;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternExcludeSkillGroupId Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.exclude_skill_group_id);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternExcludeSkillGroupId) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternExcludeSkillGroupId()
          {
            excludeSkillGroupId = num
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit unit)
        {
          return unit != null && !unit.originalUnit.unit.HasSkillGroupId(this.excludeSkillGroupId);
        }
      }

      private class CheckPatternTargetExcludeSkillGroupId : 
        BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int targetExcludeSkillGroupId;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Var;
        }

        public override int GetPattern() => 21;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternTargetExcludeSkillGroupId Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.target_exclude_skill_group_id);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternTargetExcludeSkillGroupId) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternTargetExcludeSkillGroupId()
          {
            targetExcludeSkillGroupId = num
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit target)
        {
          return target != null && !target.originalUnit.unit.HasSkillGroupId(this.targetExcludeSkillGroupId);
        }
      }

      private class CheckPatternParamType : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        protected int paramType;
        protected int targetParamType;
        protected int minValue;
        protected int maxValue;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Fix;
        }

        public override int GetPattern() => -1;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternParamType Create(
          BattleskillEffect effect)
        {
          int num1 = effect.GetInt(BattleskillEffectLogicArgumentEnum.param_type);
          int num2 = effect.GetInt(BattleskillEffectLogicArgumentEnum.target_param_type);
          BattleFuncs.CheckInvokeGeneric.CheckPatternParamType patternParamType;
          if (num1 != 0 && num2 != 0)
            patternParamType = (BattleFuncs.CheckInvokeGeneric.CheckPatternParamType) new BattleFuncs.CheckInvokeGeneric.CheckPatternBothParamType();
          else if (num1 != 0 && num2 == 0)
          {
            patternParamType = (BattleFuncs.CheckInvokeGeneric.CheckPatternParamType) new BattleFuncs.CheckInvokeGeneric.CheckPatternSelfParamType();
          }
          else
          {
            if (num1 != 0 || num2 == 0)
              return (BattleFuncs.CheckInvokeGeneric.CheckPatternParamType) null;
            patternParamType = (BattleFuncs.CheckInvokeGeneric.CheckPatternParamType) new BattleFuncs.CheckInvokeGeneric.CheckPatternTargetParamType();
          }
          patternParamType.paramType = num1;
          patternParamType.targetParamType = num2;
          patternParamType.minValue = effect.GetInt(BattleskillEffectLogicArgumentEnum.min_value);
          patternParamType.maxValue = effect.GetInt(BattleskillEffectLogicArgumentEnum.max_value);
          return patternParamType;
        }
      }

      private class CheckPatternSelfParamType : BattleFuncs.CheckInvokeGeneric.CheckPatternParamType
      {
        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Var;
        }

        public override int GetPattern() => 22;

        public bool DoCheck(
          BL.ISkillEffectListUnit unit,
          Judgement.NonBattleParameter.FromPlayerUnitCache unitNbpCache,
          int? unitHp)
        {
          if (unit == null)
            return false;
          if (unitNbpCache == null)
            unitNbpCache = new Judgement.NonBattleParameter.FromPlayerUnitCache(unit.originalUnit.playerUnit);
          int num = -BattleFuncs.GetParamDiffValue(this.paramType, unitNbpCache, unitHp.HasValue ? unitHp.Value : unit.hp);
          return num >= this.minValue && num <= this.maxValue;
        }
      }

      private class CheckPatternTargetParamType : 
        BattleFuncs.CheckInvokeGeneric.CheckPatternParamType
      {
        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Var;
        }

        public override int GetPattern() => 23;

        public bool DoCheck(
          BL.ISkillEffectListUnit target,
          Judgement.NonBattleParameter.FromPlayerUnitCache targetNbpCache,
          int? targetHp)
        {
          if (target == null)
            return false;
          if (targetNbpCache == null)
            targetNbpCache = new Judgement.NonBattleParameter.FromPlayerUnitCache(target.originalUnit.playerUnit);
          int paramDiffValue = BattleFuncs.GetParamDiffValue(this.targetParamType, targetNbpCache, targetHp.HasValue ? targetHp.Value : target.hp);
          return paramDiffValue >= this.minValue && paramDiffValue <= this.maxValue;
        }
      }

      private class CheckPatternBothParamType : BattleFuncs.CheckInvokeGeneric.CheckPatternParamType
      {
        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Var;
        }

        public override int GetPattern() => 24;

        public bool DoCheck(
          BL.ISkillEffectListUnit unit,
          BL.ISkillEffectListUnit target,
          Judgement.NonBattleParameter.FromPlayerUnitCache unitNbpCache,
          Judgement.NonBattleParameter.FromPlayerUnitCache targetNbpCache,
          int? unitHp,
          int? targetHp)
        {
          if (unit == null || target == null)
            return false;
          if (unitNbpCache == null)
            unitNbpCache = new Judgement.NonBattleParameter.FromPlayerUnitCache(unit.originalUnit.playerUnit);
          if (targetNbpCache == null)
            targetNbpCache = new Judgement.NonBattleParameter.FromPlayerUnitCache(target.originalUnit.playerUnit);
          int paramDiffValue = BattleFuncs.GetParamDiffValue(this.paramType, unitNbpCache, unitHp.HasValue ? unitHp.Value : unit.hp);
          int num = BattleFuncs.GetParamDiffValue(this.targetParamType, targetNbpCache, targetHp.HasValue ? targetHp.Value : target.hp) - paramDiffValue;
          return num >= this.minValue && num <= this.maxValue;
        }
      }

      private class CheckPatternOnemanCharge : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        protected BattleFuncs.OnemanChargeSearchTargetCheck ocstc;
        protected int[] range;
        protected int minUnitCount;
        protected int maxUnitCount;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Var;
        }

        public override int GetPattern() => -1;

        protected bool DoCheck(int type, BL.ISkillEffectListUnit unit, bool isColosseum)
        {
          if (unit == null || isColosseum)
            return false;
          BL.UnitPosition up = BattleFuncs.iSkillEffectListUnitToUnitPosition(unit);
          int num = 0;
          if (type == 0 || type == 2)
            num += BattleFuncs.getTargets(up.row, up.column, this.range, BattleFuncs.getForceIDArray(BattleFuncs.getForceID(unit.originalUnit)), BL.Unit.TargetAttribute.all, unit is BL.AIUnit, nonFacility: true).Count<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x =>
            {
              BL.ISkillEffectListUnit iskillEffectListUnit = BattleFuncs.unitPositionToISkillEffectListUnit(x);
              return x != up && iskillEffectListUnit.hp > 0 && this.ocstc.DoCheck(iskillEffectListUnit);
            }));
          if (type == 1 || type == 2)
            num += BattleFuncs.getTargets(up.row, up.column, this.range, BattleFuncs.getTargetForce(unit.originalUnit, unit.IsCharm), BL.Unit.TargetAttribute.all, unit is BL.AIUnit, nonFacility: true).Count<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x =>
            {
              BL.ISkillEffectListUnit iskillEffectListUnit = BattleFuncs.unitPositionToISkillEffectListUnit(x);
              return iskillEffectListUnit.hp > 0 && this.ocstc.DoCheck(iskillEffectListUnit);
            }));
          return num >= this.minUnitCount && num <= this.maxUnitCount;
        }
      }

      private class CheckPatternOnemanChargePlayer : 
        BattleFuncs.CheckInvokeGeneric.CheckPatternOnemanCharge
      {
        public override int GetPattern() => 25;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternOnemanChargePlayer Create(
          BattleskillEffect effect,
          BattleFuncs.PackedSkillEffect pse)
        {
          BattleFuncs.CheckInvokeGeneric.CheckPatternOnemanChargePlayer onemanChargePlayer = new BattleFuncs.CheckInvokeGeneric.CheckPatternOnemanChargePlayer();
          onemanChargePlayer.ocstc = (BattleFuncs.OnemanChargeSearchTargetCheck) new BattleFuncs.OnemanChargeSearchTargetCheckPlayer(pse);
          onemanChargePlayer.range = new int[2]
          {
            effect.GetInt(BattleskillEffectLogicArgumentEnum.oneman_charge_player_min_range),
            effect.GetInt(BattleskillEffectLogicArgumentEnum.oneman_charge_player_max_range)
          };
          onemanChargePlayer.minUnitCount = effect.GetInt(BattleskillEffectLogicArgumentEnum.oneman_charge_player_min_unit_count);
          onemanChargePlayer.maxUnitCount = effect.GetInt(BattleskillEffectLogicArgumentEnum.oneman_charge_player_max_unit_count);
          return onemanChargePlayer;
        }

        public bool DoCheck(BL.ISkillEffectListUnit unit, bool isColosseum)
        {
          return this.DoCheck(0, unit, isColosseum);
        }
      }

      private class CheckPatternOnemanChargeEnemy : 
        BattleFuncs.CheckInvokeGeneric.CheckPatternOnemanCharge
      {
        public override int GetPattern() => 26;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternOnemanChargeEnemy Create(
          BattleskillEffect effect,
          BattleFuncs.PackedSkillEffect pse)
        {
          BattleFuncs.CheckInvokeGeneric.CheckPatternOnemanChargeEnemy onemanChargeEnemy = new BattleFuncs.CheckInvokeGeneric.CheckPatternOnemanChargeEnemy();
          onemanChargeEnemy.ocstc = (BattleFuncs.OnemanChargeSearchTargetCheck) new BattleFuncs.OnemanChargeSearchTargetCheckEnemy(pse);
          onemanChargeEnemy.range = new int[2]
          {
            effect.GetInt(BattleskillEffectLogicArgumentEnum.oneman_charge_enemy_min_range),
            effect.GetInt(BattleskillEffectLogicArgumentEnum.oneman_charge_enemy_max_range)
          };
          onemanChargeEnemy.minUnitCount = effect.GetInt(BattleskillEffectLogicArgumentEnum.oneman_charge_enemy_min_unit_count);
          onemanChargeEnemy.maxUnitCount = effect.GetInt(BattleskillEffectLogicArgumentEnum.oneman_charge_enemy_max_unit_count);
          return onemanChargeEnemy;
        }

        public bool DoCheck(BL.ISkillEffectListUnit unit, bool isColosseum)
        {
          return this.DoCheck(1, unit, isColosseum);
        }
      }

      private class CheckPatternOnemanChargeComplex : 
        BattleFuncs.CheckInvokeGeneric.CheckPatternOnemanCharge
      {
        public override int GetPattern() => 27;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternOnemanChargeComplex Create(
          BattleskillEffect effect,
          BattleFuncs.PackedSkillEffect pse)
        {
          BattleFuncs.CheckInvokeGeneric.CheckPatternOnemanChargeComplex onemanChargeComplex = new BattleFuncs.CheckInvokeGeneric.CheckPatternOnemanChargeComplex();
          onemanChargeComplex.ocstc = (BattleFuncs.OnemanChargeSearchTargetCheck) new BattleFuncs.OnemanChargeSearchTargetCheckComplex(pse);
          onemanChargeComplex.range = new int[2]
          {
            effect.GetInt(BattleskillEffectLogicArgumentEnum.oneman_charge_complex_min_range),
            effect.GetInt(BattleskillEffectLogicArgumentEnum.oneman_charge_complex_max_range)
          };
          onemanChargeComplex.minUnitCount = effect.GetInt(BattleskillEffectLogicArgumentEnum.oneman_charge_complex_min_unit_count);
          onemanChargeComplex.maxUnitCount = effect.GetInt(BattleskillEffectLogicArgumentEnum.oneman_charge_complex_max_unit_count);
          return onemanChargeComplex;
        }

        public bool DoCheck(BL.ISkillEffectListUnit unit, bool isColosseum)
        {
          return this.DoCheck(2, unit, isColosseum);
        }
      }

      private class CheckPatternPeculiarParameter : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int paramType;
        private float paramMin;
        private float paramMax;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Fix;
        }

        public override int GetPattern() => 17;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternPeculiarParameter Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.peculiar_parameter_type);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternPeculiarParameter) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternPeculiarParameter()
          {
            paramType = num,
            paramMin = effect.GetFloat(BattleskillEffectLogicArgumentEnum.peculiar_parameter_min),
            paramMax = effect.GetFloat(BattleskillEffectLogicArgumentEnum.peculiar_parameter_max)
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit unit)
        {
          if (unit == null)
            return false;
          float peculiarParameterValue = BattleFuncs.GetPeculiarParameterValue(unit, this.paramType);
          return (double) peculiarParameterValue >= (double) this.paramMin && (double) peculiarParameterValue <= (double) this.paramMax;
        }
      }

      private class CheckPatternTargetPeculiarParameter : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int targetParamType;
        private float paramMin;
        private float paramMax;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Var;
        }

        public override int GetPattern() => 28;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternTargetPeculiarParameter Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.target_peculiar_parameter_type);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternTargetPeculiarParameter) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternTargetPeculiarParameter()
          {
            targetParamType = num,
            paramMin = effect.GetFloat(BattleskillEffectLogicArgumentEnum.target_peculiar_parameter_min),
            paramMax = effect.GetFloat(BattleskillEffectLogicArgumentEnum.target_peculiar_parameter_max)
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit target)
        {
          if (target == null)
            return false;
          float peculiarParameterValue = BattleFuncs.GetPeculiarParameterValue(target, this.targetParamType);
          return (double) peculiarParameterValue >= (double) this.paramMin && (double) peculiarParameterValue <= (double) this.paramMax;
        }
      }

      private class CheckPatternLevelUpStatus : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int statusType;
        private Decimal minRatio;
        private Decimal maxRatio;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Fix;
        }

        public override int GetPattern() => 18;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternLevelUpStatus Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.level_up_status_type);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternLevelUpStatus) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternLevelUpStatus()
          {
            statusType = num,
            minRatio = (Decimal) effect.GetFloat(BattleskillEffectLogicArgumentEnum.level_up_status_min),
            maxRatio = (Decimal) effect.GetFloat(BattleskillEffectLogicArgumentEnum.level_up_status_max)
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit unit)
        {
          if (unit == null)
            return false;
          Decimal levelUpStatusRatio = BattleFuncs.GetLevelUpStatusRatio(unit, this.statusType);
          return (!(this.minRatio != 0M) || !(levelUpStatusRatio < this.minRatio)) && (!(this.maxRatio != 0M) || !(levelUpStatusRatio >= this.maxRatio));
        }
      }

      private class CheckPatternTargetLevelUpStatus : BattleFuncs.CheckInvokeGeneric.CheckPattern
      {
        private int targetStatusType;
        private Decimal minRatio;
        private Decimal maxRatio;

        public override BattleFuncs.CheckInvokeGeneric.CheckPattern.Category GetCategory()
        {
          return BattleFuncs.CheckInvokeGeneric.CheckPattern.Category.Var;
        }

        public override int GetPattern() => 29;

        public static BattleFuncs.CheckInvokeGeneric.CheckPatternTargetLevelUpStatus Create(
          BattleskillEffect effect)
        {
          int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.target_level_up_status_type);
          if (num == 0)
            return (BattleFuncs.CheckInvokeGeneric.CheckPatternTargetLevelUpStatus) null;
          return new BattleFuncs.CheckInvokeGeneric.CheckPatternTargetLevelUpStatus()
          {
            targetStatusType = num,
            minRatio = (Decimal) effect.GetFloat(BattleskillEffectLogicArgumentEnum.target_level_up_status_min),
            maxRatio = (Decimal) effect.GetFloat(BattleskillEffectLogicArgumentEnum.target_level_up_status_max)
          };
        }

        public bool DoCheck(BL.ISkillEffectListUnit target)
        {
          if (target == null)
            return false;
          Decimal levelUpStatusRatio = BattleFuncs.GetLevelUpStatusRatio(target, this.targetStatusType);
          return (!(this.minRatio != 0M) || !(levelUpStatusRatio < this.minRatio)) && (!(this.maxRatio != 0M) || !(levelUpStatusRatio >= this.maxRatio));
        }
      }
    }

    private class SnakeVenomDamageData
    {
      public BL.ISkillEffectListUnit invokeUnit;
      public BL.SkillEffect effect;
      public int invokeOrder;
      public int turnDamage;
      public Dictionary<BL.ISkillEffectListUnit, int> unitDamage;
      public Dictionary<BL.ISkillEffectListUnit, int> unitSwapHealDamage;
    }
  }
}
