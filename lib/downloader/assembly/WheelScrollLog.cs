// Decompiled with JetBrains decompiler
// Type: WheelScrollLog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class WheelScrollLog : MonoBehaviour
{
  [SerializeField]
  [Tooltip("ONの時UIDragScrollVie.scrollViewと同じ場所のWheelScrollLogにスクロール量を渡す")]
  private bool isTerminal;
  [SerializeField]
  [Tooltip("isTerminal=trueの時のスクロール量集計先")]
  private WheelScrollLog wheelLog;

  public float amount { get; private set; }

  public void resetAmount() => this.amount = 0.0f;

  private void OnScroll(float delta)
  {
    if (this.isTerminal)
    {
      if (Object.op_Equality((Object) this.wheelLog, (Object) null))
      {
        UIDragScrollView component = ((Component) this).GetComponent<UIDragScrollView>();
        UIScrollView scrollView = Object.op_Inequality((Object) component, (Object) null) ? component.scrollView : (UIScrollView) null;
        if (Object.op_Inequality((Object) scrollView, (Object) null))
          this.wheelLog = ((Component) scrollView).GetComponent<WheelScrollLog>();
        if (Object.op_Equality((Object) this.wheelLog, (Object) null))
          return;
      }
      if (this.wheelLog.isTerminal)
        return;
      this.wheelLog.OnScroll(delta);
    }
    else
      this.amount += delta;
  }
}
