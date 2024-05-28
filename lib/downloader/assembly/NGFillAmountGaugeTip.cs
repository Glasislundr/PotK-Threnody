// Decompiled with JetBrains decompiler
// Type: NGFillAmountGaugeTip
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class NGFillAmountGaugeTip : MonoBehaviour
{
  public float OffSetX;
  [SerializeField]
  private UISprite mParentSprite;

  private void Update()
  {
    float num = (float) ((UIWidget) this.mParentSprite).width * this.mParentSprite.fillAmount + this.OffSetX;
    Vector3 localPosition = ((Component) this).transform.localPosition;
    if ((double) num == (double) localPosition.x)
      return;
    ((Component) this).transform.localPosition = new Vector3(num, localPosition.y, localPosition.z);
  }
}
