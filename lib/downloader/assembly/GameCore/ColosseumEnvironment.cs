// Decompiled with JetBrains decompiler
// Type: GameCore.ColosseumEnvironment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections.Generic;

#nullable disable
namespace GameCore
{
  public class ColosseumEnvironment
  {
    public string colosseumTransactionID;
    public Dictionary<int, BL.Unit> playerUnitDict;
    public Dictionary<int, BL.Unit> opponentUnitDict;
    public Dictionary<int, PlayerItem> playerGearDict;
    public Dictionary<int, PlayerItem> playerGearDict2;
    public Dictionary<int, PlayerItem> playerGearDict3;
    public Dictionary<int, PlayerItem> playerReisouDict;
    public Dictionary<int, PlayerItem> playerReisouDict2;
    public Dictionary<int, PlayerItem> playerReisouDict3;
    public Dictionary<int, PlayerItem> opponentGearDict;
    public Dictionary<int, PlayerItem> opponentGearDict2;
    public Dictionary<int, PlayerItem> opponentGearDict3;
    public Dictionary<int, PlayerItem> opponentReisouDict;
    public Dictionary<int, PlayerItem> opponentReisouDict2;
    public Dictionary<int, PlayerItem> opponentReisouDict3;
    public Bonus[] bonusList;
    public string today;
  }
}
