// Decompiled with JetBrains decompiler
// Type: EffectActiveContorler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class EffectActiveContorler : MonoBehaviour
{
  [SerializeField]
  private float activeOffTime;

  private void OnEnable()
  {
    if ((double) this.activeOffTime <= 0.0)
      return;
    this.StartCoroutine(this.activeOffTimer());
  }

  private IEnumerator activeOffTimer()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EffectActiveContorler effectActiveContorler = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      ((Component) effectActiveContorler).gameObject.SetActive(false);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(effectActiveContorler.activeOffTime);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }
}
