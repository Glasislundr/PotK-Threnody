// Decompiled with JetBrains decompiler
// Type: BattleResultBonusInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
public class BattleResultBonusInfo
{
  public int rewardID;
  public MasterDataTable.CommonRewardType rewardType;
  public string rewardMessage;
  public bool isMessage;

  public BattleResultBonusInfo(
    int reward_id,
    MasterDataTable.CommonRewardType type,
    string message,
    bool isMessage = true)
  {
    this.rewardID = reward_id;
    this.rewardType = type;
    this.rewardMessage = message;
    this.isMessage = isMessage;
  }
}
