// Decompiled with JetBrains decompiler
// Type: PrincessSynthesisSoundEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PrincessSynthesisSoundEffect : MonoBehaviour
{
  private int resultSuccess;
  public bool result;

  public void OnStartPrincessSynthesis()
  {
    Debug.Log((object) "[TOUGOU] SE 0002 play");
    Singleton<NGSoundManager>.GetInstance().playSE("SE_0002");
  }

  public void setResult(int rarity) => this.resultSuccess = rarity;

  public void OnPlayResult()
  {
    this.result = true;
    Debug.Log((object) "[TOUGOU] SE 000x play");
    switch (this.resultSuccess)
    {
      case 1:
        this.OnSuccess();
        break;
      case 2:
        this.OnGreatSuccess();
        break;
      case 3:
        this.OnFailuer();
        break;
      default:
        this.OnGreatSuccess();
        break;
    }
  }

  private void OnSuccess() => Singleton<NGSoundManager>.GetInstance().playSE("SE_0003");

  private void OnGreatSuccess() => Singleton<NGSoundManager>.GetInstance().playSE("SE_0004");

  private void OnFailuer() => Singleton<NGSoundManager>.GetInstance().playSE("SE_0005");
}
