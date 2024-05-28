// Decompiled with JetBrains decompiler
// Type: Shop00720RewardData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
public class Shop00720RewardData
{
  private int rewardId;
  private MasterDataTable.CommonRewardType rewardType;
  private string description;
  private int quantity;

  public MasterDataTable.CommonRewardType RewardType
  {
    get => this.rewardType;
    set => this.rewardType = value;
  }

  public int RewardId
  {
    get => this.rewardId;
    set => this.rewardId = value;
  }

  public int Quantity
  {
    get => this.quantity;
    set => this.quantity = value;
  }

  public string Description
  {
    get => this.description;
    set => this.description = value;
  }
}
