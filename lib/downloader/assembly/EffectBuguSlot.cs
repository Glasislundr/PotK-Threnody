// Decompiled with JetBrains decompiler
// Type: EffectBuguSlot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class EffectBuguSlot : MonoBehaviour
{
  [SerializeField]
  private GameObject objUnity_;
  [SerializeField]
  private UILabel txtUnity_;

  public void slotActive(bool active)
  {
    if (Object.op_Implicit((Object) this.objUnity_))
      this.objUnity_.SetActive(active);
    if (!Object.op_Implicit((Object) this.txtUnity_))
      return;
    ((Component) this.txtUnity_).gameObject.SetActive(active);
  }

  public void setUnity(string unity) => this.txtUnity_.SetTextLocalize(unity);
}
