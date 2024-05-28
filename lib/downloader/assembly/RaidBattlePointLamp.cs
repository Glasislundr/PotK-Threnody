// Decompiled with JetBrains decompiler
// Type: RaidBattlePointLamp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class RaidBattlePointLamp : MonoBehaviour
{
  [SerializeField]
  private GameObject mLamp;

  public void On() => this.mLamp.SetActive(true);

  public void Off() => this.mLamp.SetActive(false);

  public void Enable()
  {
    ((Component) this).GetComponent<UIWidget>().color = Color.white;
    this.mLamp.GetComponent<UIWidget>().color = Color.white;
  }

  public void Disable()
  {
    ((Component) this).GetComponent<UIWidget>().color = Color.gray;
    this.mLamp.GetComponent<UIWidget>().color = Color.gray;
  }
}
