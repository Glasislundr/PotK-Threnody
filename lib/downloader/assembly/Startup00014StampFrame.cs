// Decompiled with JetBrains decompiler
// Type: Startup00014StampFrame
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Startup00014StampFrame : MonoBehaviour
{
  [SerializeField]
  private List<float> position_x = new List<float>();
  [SerializeField]
  private UITexture frame;
  [SerializeField]
  private UITexture arrow;

  public void ArrowPosition(int num)
  {
    ((Component) this.arrow).transform.localPosition = new Vector3(this.position_x[num], 0.0f, 0.0f);
  }

  public void ChangeFrame(Texture texture)
  {
    ((UIWidget) this.frame).mainTexture = texture;
    ((UIWidget) this.frame).SetDimensions(((UIWidget) this.frame).mainTexture.width, ((UIWidget) this.frame).mainTexture.height);
  }

  public void ArrowEnable(bool flag) => ((Behaviour) this.arrow).enabled = flag;
}
