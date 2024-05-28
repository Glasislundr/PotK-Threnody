// Decompiled with JetBrains decompiler
// Type: BattleUI04SetHP
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BattleUI04SetHP : MonoBehaviour
{
  [SerializeField]
  public GameObject[] hpNumbers;

  public void setValue(int v)
  {
    this.notDisplay();
    this.hpNumbers[v].SetActive(true);
  }

  public void notDisplay()
  {
    foreach (GameObject hpNumber in this.hpNumbers)
      hpNumber.SetActive(false);
  }
}
