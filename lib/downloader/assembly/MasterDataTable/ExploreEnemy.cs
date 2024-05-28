// Decompiled with JetBrains decompiler
// Type: MasterDataTable.ExploreEnemy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Explore;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class ExploreEnemy
  {
    public int ID;
    public int period_id;
    public int floor;
    public int lotteryRatio;
    public int unit_UnitUnit;
    public int? magicBullet_BattleskillSkill;
    public int hp;
    public int strength;
    public int vitality;
    public int intelligence;
    public int mind;
    public int agility;
    public int dexterity;
    public int lucky;
    public string weakPoint_element;
    public string weakPoint_weapon;
    public string weakPoint_unitType;
    public int zeny;
    public float trust_rate;
    public int player_exp;
    public int unit_exp;
    public int drop_ratio;
    public int drop_deck_id;
    [NonSerialized]
    private PlayerUnit _PlayerUnit;
    [NonSerialized]
    private WeakPoint _WeakPoint;
    [NonSerialized]
    private SortedDictionary<int, int> _DropDeck;

    public static ExploreEnemy Parse(MasterDataReader reader)
    {
      return new ExploreEnemy()
      {
        ID = reader.ReadInt(),
        period_id = reader.ReadInt(),
        floor = reader.ReadInt(),
        lotteryRatio = reader.ReadInt(),
        unit_UnitUnit = reader.ReadInt(),
        magicBullet_BattleskillSkill = reader.ReadIntOrNull(),
        hp = reader.ReadInt(),
        strength = reader.ReadInt(),
        vitality = reader.ReadInt(),
        intelligence = reader.ReadInt(),
        mind = reader.ReadInt(),
        agility = reader.ReadInt(),
        dexterity = reader.ReadInt(),
        lucky = reader.ReadInt(),
        weakPoint_element = reader.ReadString(true),
        weakPoint_weapon = reader.ReadString(true),
        weakPoint_unitType = reader.ReadString(true),
        zeny = reader.ReadInt(),
        trust_rate = reader.ReadFloat(),
        player_exp = reader.ReadInt(),
        unit_exp = reader.ReadInt(),
        drop_ratio = reader.ReadInt(),
        drop_deck_id = reader.ReadInt()
      };
    }

    public UnitUnit unit
    {
      get
      {
        UnitUnit unit;
        if (!MasterData.UnitUnit.TryGetValue(this.unit_UnitUnit, out unit))
          Debug.LogError((object) ("Key not Found: MasterData.UnitUnit[" + (object) this.unit_UnitUnit + "]"));
        return unit;
      }
    }

    public BattleskillSkill magicBullet
    {
      get
      {
        if (!this.magicBullet_BattleskillSkill.HasValue)
          return (BattleskillSkill) null;
        BattleskillSkill magicBullet;
        if (!MasterData.BattleskillSkill.TryGetValue(this.magicBullet_BattleskillSkill.Value, out magicBullet))
          Debug.LogError((object) ("Key not Found: MasterData.BattleskillSkill[" + (object) this.magicBullet_BattleskillSkill.Value + "]"));
        return magicBullet;
      }
    }

    public PlayerUnit PlayerUnit
    {
      get
      {
        if (this._PlayerUnit == (PlayerUnit) null)
          this._PlayerUnit = this.createPlayerUnit();
        return this._PlayerUnit;
      }
    }

    public WeakPoint WeakPoint
    {
      get
      {
        if (this._WeakPoint == null)
          this._WeakPoint = this.createWeakPoint();
        return this._WeakPoint;
      }
    }

    public SortedDictionary<int, int> DropDeck
    {
      get
      {
        if (this._DropDeck == null)
          this._DropDeck = this.createDropDeck();
        return this._DropDeck;
      }
    }

    private PlayerUnit createPlayerUnit()
    {
      PlayerUnit playerUnit = new PlayerUnit()
      {
        id = this.ID,
        dexterity = new PlayerUnitDexterity(),
        agility = new PlayerUnitAgility(),
        mind = new PlayerUnitMind(),
        strength = new PlayerUnitStrength(),
        vitality = new PlayerUnitVitality(),
        hp = new PlayerUnitHp(),
        intelligence = new PlayerUnitIntelligence(),
        lucky = new PlayerUnitLucky()
      };
      playerUnit.dexterity.initial = this.dexterity;
      playerUnit.agility.initial = this.agility;
      playerUnit.mind.initial = this.mind;
      playerUnit.strength.initial = this.strength;
      playerUnit.vitality.initial = this.vitality;
      playerUnit.hp.initial = this.hp;
      playerUnit.intelligence.initial = this.intelligence;
      playerUnit.lucky.initial = this.lucky;
      playerUnit.level = 1;
      playerUnit.max_level = 1;
      playerUnit.breakthrough_count = 0;
      playerUnit.favorite = false;
      playerUnit.total_exp = 0;
      playerUnit._unit_type = MasterData.UnitTypeList[0].ID;
      playerUnit._unit = this.unit_UnitUnit;
      playerUnit.equip_gear_ids = (int?[]) null;
      playerUnit.job_id = playerUnit.unit.job_UnitJob;
      playerUnit.move = playerUnit.getJobData().movement;
      if (this.magicBullet != null)
        playerUnit.skills = new PlayerUnitSkills[1]
        {
          new PlayerUnitSkills()
          {
            skill_id = this.magicBullet.ID,
            level = 1
          }
        };
      else
        playerUnit.skills = new PlayerUnitSkills[0];
      playerUnit.leader_skills = new PlayerUnitLeader_skills[0];
      playerUnit.is_enemy_leader = playerUnit.leader_skills.Length != 0;
      playerUnit.primary_equipped_gear = (PlayerItem) null;
      playerUnit.primary_equipped_gear2 = (PlayerItem) null;
      playerUnit.primary_equipped_gear3 = (PlayerItem) null;
      playerUnit.spawn_turn = 0;
      playerUnit.reinforcement = (BattleReinforcement) null;
      playerUnit.gear_proficiencies = (PlayerUnitGearProficiency[]) null;
      return playerUnit;
    }

    private WeakPoint createWeakPoint()
    {
      return new WeakPoint(this.parseEnumStringArray<CommonElement>(this.weakPoint_element), this.parseEnumStringArray<GearKindEnum>(this.weakPoint_weapon), this.parseEnumStringArray<UnitTypeEnum>(this.weakPoint_unitType));
    }

    private SortedDictionary<int, int> createDropDeck()
    {
      SortedDictionary<int, int> dropDeck = new SortedDictionary<int, int>();
      int key = 0;
      foreach (ExploreDropTable exploreDropTable in ((IEnumerable<ExploreDropTable>) MasterData.ExploreDropTableList).Where<ExploreDropTable>((Func<ExploreDropTable, bool>) (x => x.deck_id == this.drop_deck_id)))
      {
        key += exploreDropTable.drop_ratio;
        dropDeck.Add(key, exploreDropTable.drop_reward_id);
      }
      return dropDeck;
    }

    private T[] parseEnumStringArray<T>(string enumStrArray)
    {
      System.Type enumType = typeof (T);
      List<T> objList = new List<T>();
      string str1 = enumStrArray;
      char[] chArray = new char[1]{ ',' };
      foreach (string str2 in str1.Split(chArray))
      {
        double result = 0.0;
        if (double.TryParse(str2.Trim(), out result))
          objList.Add((T) Enum.ToObject(enumType, (int) result));
      }
      return objList.ToArray();
    }
  }
}
