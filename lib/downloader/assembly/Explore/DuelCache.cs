// Decompiled with JetBrains decompiler
// Type: Explore.DuelCache
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;

#nullable disable
namespace Explore
{
  public struct DuelCache
  {
    public ExploreEnemy Enemy;
    public int Damage;

    public DuelCache(ExploreEnemy eEnemy, int damage)
    {
      this.Enemy = eEnemy;
      this.Damage = damage;
    }
  }
}
