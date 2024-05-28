// Decompiled with JetBrains decompiler
// Type: Battle01PVPBattleHeader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Battle01PVPBattleHeader : NGBattleMenuBase
{
  [SerializeField]
  private UILabel playerName;
  [SerializeField]
  private UILabel enemyName;

  protected override IEnumerator Start_Battle()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Battle01PVPBattleHeader battle01PvpBattleHeader = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (battle01PvpBattleHeader.battleManager.gameEngine != null)
    {
      battle01PvpBattleHeader.setText(battle01PvpBattleHeader.playerName, battle01PvpBattleHeader.battleManager.gameEngine.playerName);
      battle01PvpBattleHeader.setText(battle01PvpBattleHeader.enemyName, battle01PvpBattleHeader.battleManager.gameEngine.enemyName);
    }
    return false;
  }
}
