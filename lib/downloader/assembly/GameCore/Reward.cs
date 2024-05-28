// Decompiled with JetBrains decompiler
// Type: GameCore.Reward
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using UnityEngine;

#nullable disable
namespace GameCore
{
  [Serializable]
  public class Reward
  {
    [SerializeField]
    private CommonRewardType type;
    [SerializeField]
    private int id;
    [SerializeField]
    private int quantity;

    public Reward(CommonRewardType type, int id, int quantity)
    {
      this.type = type;
      this.id = id;
      this.quantity = quantity;
    }

    public CommonRewardType Type => this.type;

    public int Id => this.id;

    public int Quantity => this.quantity;
  }
}
