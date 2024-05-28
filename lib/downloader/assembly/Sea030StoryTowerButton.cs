// Decompiled with JetBrains decompiler
// Type: Sea030StoryTowerButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Sea030StoryTowerButton : MonoBehaviour
{
  [SerializeField]
  private TweenPosition tweenPosition;
  private bool animReverse;
  private int ButtonMnumber;
  private int ButtonLnumber;
  private int _TweenIndex;

  public int Mnumber
  {
    get => this.ButtonMnumber;
    set => this.ButtonMnumber = value;
  }

  public int Lnumber
  {
    get => this.ButtonLnumber;
    set => this.ButtonLnumber = value;
  }

  public int TweenIndex
  {
    get => this._TweenIndex;
    set => this._TweenIndex = value;
  }

  public void changeAnimDirection()
  {
    if (!Object.op_Inequality((Object) this.tweenPosition, (Object) null))
      return;
    this.tweenPosition.to.y = -this.tweenPosition.to.y;
    ((UITweener) this.tweenPosition).delay = 0.0f;
    this.animReverse = !this.animReverse;
  }

  public void initTween()
  {
    if (!this.animReverse)
      return;
    this.changeAnimDirection();
  }
}
