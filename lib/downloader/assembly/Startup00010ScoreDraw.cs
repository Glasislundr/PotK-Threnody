// Decompiled with JetBrains decompiler
// Type: Startup00010ScoreDraw
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Startup00010ScoreDraw : MonoBehaviour
{
  public List<Startup00010Score> score_sprite_list;
  public bool zero_draw = true;

  public void Draw(int score)
  {
    foreach (Startup00010Score scoreSprite in this.score_sprite_list)
    {
      if (score == 0)
      {
        if (!this.zero_draw)
          ((Component) scoreSprite).gameObject.SetActive(false);
      }
      else
        ((Component) scoreSprite).gameObject.SetActive(true);
      scoreSprite.SetActive(score % 10);
      score /= 10;
    }
  }
}
