// Decompiled with JetBrains decompiler
// Type: InventoryExtraSkill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;

#nullable disable
public class InventoryExtraSkill
{
  public PlayerAwakeSkill skill;
  public int level;
  public int uniqueID;
  public ExtraSkillIcon icon;
  public bool select;
  public int index;
  public bool Gray;
  public bool removeButton;
  public bool forBattle;
  public bool favorite;
  public PlayerUnit equipUnit;

  public InventoryExtraSkill() => this.removeButton = true;

  public InventoryExtraSkill(PlayerAwakeSkill skill) => this.Init(skill);

  public InventoryExtraSkill(PlayerAwakeSkill skill, PlayerUnit equipped)
  {
    this.Init(skill, equipped);
  }

  public void Init(PlayerAwakeSkill awakeSkill)
  {
    this.skill = awakeSkill;
    this.favorite = this.skill.favorite;
    this.level = this.skill.level;
    this.uniqueID = this.skill.id;
    this.equipUnit = this.skill.EqupmentUnit;
    this.forBattle = this.equipUnit != (PlayerUnit) null;
  }

  public void Init(PlayerAwakeSkill awakeSkill, PlayerUnit equipped)
  {
    this.skill = awakeSkill;
    this.favorite = this.skill.favorite;
    this.level = this.skill.level;
    this.uniqueID = this.skill.id;
    this.equipUnit = equipped;
    this.forBattle = this.equipUnit != (PlayerUnit) null;
  }

  public string GetName() => this.skill.masterData.name;

  public string GetDescription() => this.skill.masterData.description;

  public int GetMaxLevel() => this.skill.masterData.upper_level;

  public BattleskillGenre? GetGenre1() => this.skill.masterData.genre1;

  public BattleskillGenre? GetGenre2() => this.skill.masterData.genre2;
}
