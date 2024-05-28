// Decompiled with JetBrains decompiler
// Type: EffectSE
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class EffectSE : MonoBehaviour
{
  public string SoundEffectName;
  public bool playOnStart = true;
  public bool playOnEnable;
  public float Delay;
  [HideInInspector]
  public int UseChannel = -1;
  private NGSoundManager sm;

  private void Start()
  {
    this.sm = Singleton<NGSoundManager>.GetInstance();
    if (!this.playOnStart && !this.playOnEnable)
      return;
    this.StartCoroutine(this.PlaySE(this.Delay));
  }

  private void OnEnable()
  {
    if (this.playOnStart || !this.playOnEnable)
      return;
    this.StartCoroutine(this.PlaySE(this.Delay));
  }

  public void playSe()
  {
    this.sm = Singleton<NGSoundManager>.GetInstance();
    if (this.playOnStart)
      return;
    this.StartCoroutine(this.PlaySE(this.Delay));
  }

  public IEnumerator PlaySE(float delayTime = 0.0f)
  {
    if (!Object.op_Equality((Object) null, (Object) this.sm) && !string.IsNullOrEmpty(this.SoundEffectName))
    {
      if ((double) delayTime > 0.0)
        yield return (object) new WaitForSeconds(delayTime);
      this.UseChannel = this.sm.playSE(this.SoundEffectName);
    }
  }
}
