// Decompiled with JetBrains decompiler
// Type: GVGSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class GVGSetting
{
  public float point_leader_factor;
  public float point_no_leader_factor;
  public float point_cost_factor;
  public float point_rarity_factor;
  public float point_base_factor;
  public float respawn_base_factor;
  public float respawn_rarity_factor;
  public float respawn_cost_factor;
  public float turns_factor;
  public int turns;
  public int point;
  public int annihilation_point;
  public int timeLimit;
  public string enemyID;
  public string enemyPlayerName;
  public int enemyEmblemID;
  public int enemyGuildPosition;
  public int enemyLevel;
  public int enemyContribution;
  public int enemyTownLevel;
  public int enemyKeepStar;
  public string enemyGuildname;
  public string myGuildName;
  public bool isTestBattle;

  public GVGSetting()
  {
    this.point_leader_factor = this.point_no_leader_factor = this.point_cost_factor = this.point_rarity_factor = this.point_base_factor = this.respawn_base_factor = this.respawn_rarity_factor = this.respawn_cost_factor = this.turns_factor = 1f;
    this.turns = 10;
    this.point = 50;
    this.annihilation_point = 100;
    this.timeLimit = 40;
    this.isTestBattle = false;
  }
}
