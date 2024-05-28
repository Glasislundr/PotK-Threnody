// Decompiled with JetBrains decompiler
// Type: ArmorRepair
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ArmorRepair : MonoBehaviour
{
  public bool mLost = true;

  public void OnSE0018()
  {
    Debug.Log((object) "OnSE0018()");
    Singleton<NGSoundManager>.GetInstance().playSE("SE_0018");
  }

  public void OnSE0019()
  {
    Debug.Log((object) "OnSE0019()");
    if (!this.mLost)
      return;
    Singleton<NGSoundManager>.GetInstance().playSE("SE_0019");
  }

  public void SetLost(bool lost) => this.mLost = lost;
}
