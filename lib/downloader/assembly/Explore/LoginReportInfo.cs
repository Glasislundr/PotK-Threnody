// Decompiled with JetBrains decompiler
// Type: Explore.LoginReportInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections.Generic;

#nullable disable
namespace Explore
{
  public class LoginReportInfo
  {
    public TimeSpan CalcTimeSpan { get; private set; } = new TimeSpan(0L);

    public int DefeatEnemyCount { get; private set; }

    public int PlayerExp { get; private set; }

    public int Zeny { get; private set; }

    public List<int> RewardsId { get; private set; } = new List<int>();

    public int AfterPlayerLevel { get; private set; }

    public int BeforePlayerLevel { get; private set; }

    public int BeforePlayerApMax { get; private set; }

    public int BeforePlayerMaxCost { get; private set; }

    public int BeforePlayerMaxFriend { get; private set; }

    public float ExpGaugeStartValue { get; private set; }

    public float ExpGaugeFinishValue { get; private set; }

    public int ExpGaugeLoopNum => this.AfterPlayerLevel - this.BeforePlayerLevel;

    public void SetReport(int defeatCount, int playerExp, int zeny)
    {
      this.DefeatEnemyCount = defeatCount;
      this.PlayerExp = playerExp;
      this.Zeny = zeny;
    }

    public void AddReport(TimeSpan timeSpan, int[] rewardsId)
    {
      this.CalcTimeSpan += timeSpan;
      this.RewardsId.AddRange((IEnumerable<int>) rewardsId);
    }

    public void SetBeforePlayerStatus()
    {
      Player player = SMManager.Get<Player>();
      this.BeforePlayerLevel = player.level;
      this.ExpGaugeStartValue = player.exp + player.exp_next <= 0 ? 1f : (float) player.exp / (float) (player.exp + player.exp_next);
      this.BeforePlayerApMax = player.ap_max;
      this.BeforePlayerMaxCost = player.max_cost;
      this.BeforePlayerMaxFriend = player.max_friends;
    }

    public void SetAfterPlayerStatus()
    {
      Player player = SMManager.Get<Player>();
      this.AfterPlayerLevel = player.level;
      if (player.exp + player.exp_next > 0)
        this.ExpGaugeFinishValue = (float) player.exp / (float) (player.exp + player.exp_next);
      else
        this.ExpGaugeFinishValue = 1f;
    }
  }
}
