// Decompiled with JetBrains decompiler
// Type: LumpToutaMaterialConfirmScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/Unit/LumpTouta/MaterialConfirmScene")]
public class LumpToutaMaterialConfirmScene : NGSceneBase
{
  [SerializeField]
  private LumpToutaMaterialConfirmMenu menu;
  private bool isInit;

  public IEnumerator onStartSceneAsync(
    List<PlayerUnit> selectedBasePlayerUnits,
    Dictionary<PlayerUnit, List<PlayerUnit>> allSamePlayerUnits)
  {
    if (!this.isInit)
    {
      yield return (object) this.menu.StartAsync(selectedBasePlayerUnits, allSamePlayerUnits);
      this.isInit = true;
    }
  }
}
