// Decompiled with JetBrains decompiler
// Type: ButtonSE
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ButtonSE : MonoBehaviour
{
  private NGSoundManager sm;
  private int channel;
  public string SE_name = "SE_1002";

  private void Start() => this.sm = Singleton<NGSoundManager>.GetInstance();

  public void playSound()
  {
    this.sm.playSE(this.SE_name);
    Debug.Log((object) "[CRI] ButtonSE playSound");
  }
}
