// Decompiled with JetBrains decompiler
// Type: TitleSoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TitleSoundEvent : MonoBehaviour
{
  public void playSE(string cue_name)
  {
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    if (!Object.op_Inequality((Object) instance, (Object) null))
      return;
    instance.PlaySe(cue_name);
  }
}
