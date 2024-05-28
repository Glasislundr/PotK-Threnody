// Decompiled with JetBrains decompiler
// Type: Battle01StopAutoBattleButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Battle01StopAutoBattleButton : NGBattleMenuBase
{
  private GameObject popupPrefab;

  protected override IEnumerator Start_Original()
  {
    Future<GameObject> f = Res.Prefabs.popup.popup_017_19_2__anim_popup01.Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.popupPrefab = f.Result;
  }

  public void onClick()
  {
    if (this.battleManager.isOvo || !this.battleManager.isBattleEnable || this.env.core.phaseState.state != BL.Phase.player && this.env.core.phaseState.state != BL.Phase.enemy && this.env.core.phaseState.state != BL.Phase.neutral)
      return;
    this.env.core.isAutoBattle.value = false;
  }
}
