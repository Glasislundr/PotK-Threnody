// Decompiled with JetBrains decompiler
// Type: Battle01PVPSetupTime
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Battle01PVPSetupTime : BattleMonoBehaviour
{
  [SerializeField]
  private SpriteNumber setupTime;
  private BL.BattleModified<BL.StructValue<int>> timeLimitModified;

  protected override IEnumerator Start_Battle()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Battle01PVPSetupTime battle01PvpSetupTime = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (battle01PvpSetupTime.battleManager.gameEngine == null)
      return false;
    battle01PvpSetupTime.timeLimitModified = BL.Observe<BL.StructValue<int>>(battle01PvpSetupTime.battleManager.gameEngine.timeLimit);
    return false;
  }

  protected override void Update_Battle()
  {
    if (this.battleManager.gameEngine == null || !this.timeLimitModified.isChangedOnce())
      return;
    this.setupTime.setNumber(this.timeLimitModified.value.value);
  }
}
