// Decompiled with JetBrains decompiler
// Type: LumpToutaIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/Unit/LumpTouta/Icon")]
public class LumpToutaIcon : UnitIcon
{
  [SerializeField]
  private UILabel txtUnityValue;
  [SerializeField]
  private UILabel txtBuildupUnityValue;
  [SerializeField]
  private BlinkSync blinkSync;
  [SerializeField]
  private GameObject unityObj;
  [SerializeField]
  private GameObject buildupUnityObj;

  public void setUnityTotal(int unity, float buildupUnity)
  {
    this.unityObj.SetActive(unity > 0);
    this.buildupUnityObj.SetActive((double) buildupUnity > 0.0);
    ((Behaviour) this.blinkSync).enabled = false;
    if (this.unityObj.activeSelf && this.buildupUnityObj.activeSelf)
    {
      this.blinkSync.resetBlinks((IEnumerable<GameObject>) new GameObject[2]
      {
        this.unityObj,
        this.buildupUnityObj
      });
      ((Behaviour) this.blinkSync).enabled = true;
    }
    this.txtUnityValue.SetTextLocalize(string.Format(Consts.GetInstance().unit_004_8_4_plus_unity_value, (object) unity));
    this.txtBuildupUnityValue.SetTextLocalize(string.Format(Consts.GetInstance().unit_004_8_4_plus_buildup_unity_value, (object) PlayerUnit.UnityToPercent(buildupUnity)));
  }
}
