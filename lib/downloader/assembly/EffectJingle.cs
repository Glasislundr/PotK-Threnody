// Decompiled with JetBrains decompiler
// Type: EffectJingle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class EffectJingle : MonoBehaviour
{
  public string bgm;
  private string oldBgm;

  private void Start()
  {
    this.oldBgm = Singleton<NGSoundManager>.GetInstance().GetBgmName();
    Singleton<NGSoundManager>.GetInstance().PlayBgm(this.bgm, fadeInTime: 1f, fadeOutTime: 0.3f);
  }

  private void Update()
  {
    if (Singleton<NGSoundManager>.GetInstance().IsBgmPlaying(0))
      return;
    Singleton<NGSoundManager>.GetInstance().PlayBgm(this.oldBgm);
  }
}
