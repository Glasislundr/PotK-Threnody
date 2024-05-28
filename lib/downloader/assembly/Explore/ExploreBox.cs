// Decompiled with JetBrains decompiler
// Type: Explore.ExploreBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace Explore
{
  public class ExploreBox
  {
    public const int REWARD_COUNT_MAX = 40;
    private List<int> mRewardsId = new List<int>(40);

    public bool IsRewardsMax => this.mRewardsId.Count >= 40;

    public int Zeny { get; private set; }

    public int PlayerExp { get; private set; }

    public int UnitExp { get; private set; }

    public float Trust { get; private set; }

    public bool AddReward(int rewardId)
    {
      if (this.IsRewardsMax)
        return false;
      this.mRewardsId.Add(rewardId);
      return true;
    }

    public bool AddRewards(int[] rewardIds)
    {
      foreach (int rewardId in rewardIds)
      {
        if (!this.AddReward(rewardId))
          return false;
      }
      return true;
    }

    public List<int> GetRewardsId() => this.mRewardsId;

    public void ClearRewardsId() => this.mRewardsId.Clear();

    public void Add(int zeny, float trust, int playerExp, int unitExp)
    {
      this.Zeny += zeny;
      this.Trust += trust;
      this.PlayerExp += playerExp;
      this.UnitExp += unitExp;
    }
  }
}
