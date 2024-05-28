// Decompiled with JetBrains decompiler
// Type: SheetGachaComplete
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class SheetGachaComplete : MonoBehaviour
{
  [SerializeField]
  private SheetGachaAnim anim;

  public void Init(Action endAction)
  {
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    if (Object.op_Inequality((Object) instance, (Object) null))
      instance.playSE("SE_0554");
    this.anim.Init(endAction);
  }
}
