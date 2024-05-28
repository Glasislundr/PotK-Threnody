// Decompiled with JetBrains decompiler
// Type: GuildButtonFrameLight
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class GuildButtonFrameLight : MonoBehaviour
{
  [SerializeField]
  private UI2DSprite sprFrame;
  [SerializeField]
  private UI2DSprite sprColor;

  public IEnumerator Init(Color color)
  {
    if (Object.op_Inequality((Object) this.sprFrame, (Object) null))
      ((UIWidget) this.sprFrame).color = color;
    if (Object.op_Inequality((Object) this.sprColor, (Object) null))
    {
      ((UIWidget) this.sprColor).color = color;
      yield break;
    }
  }
}
