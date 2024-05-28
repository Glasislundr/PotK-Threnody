// Decompiled with JetBrains decompiler
// Type: LabelCountUpper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (UILabel))]
public class LabelCountUpper : MonoBehaviour
{
  [SerializeField]
  private float Span = 0.1f;
  private UILabel mLabel;
  private int mNowNumber;
  private int mEndNumber;
  private float mSurplus;
  private bool mNeedSkip;

  public bool isAnime { get; private set; }

  public bool isFinishCount => this.mNowNumber == this.mEndNumber;

  public void Initialize(int start, int end)
  {
    this.mLabel = ((Component) this).GetComponent<UILabel>();
    this.mNowNumber = start;
    this.mEndNumber = end;
    this.mLabel.SetTextLocalize(this.mNowNumber);
  }

  public void StartCountup(Action onEndCallback = null)
  {
    this.StartCoroutine(this.Countup(onEndCallback));
  }

  public void Skip() => this.mNeedSkip = true;

  private IEnumerator Countup(Action onEndCallback = null)
  {
    this.isAnime = true;
    do
    {
      yield return (object) null;
      if (this.mNeedSkip)
      {
        this.mLabel.SetTextLocalize(this.mEndNumber);
        break;
      }
      this.mSurplus += Time.fixedDeltaTime;
      int num = Math.Max(0, (int) Math.Floor((double) this.mSurplus / (double) this.Span));
      for (int index = 0; index < num; ++index)
      {
        if (this.mNowNumber > this.mEndNumber)
          --this.mNowNumber;
        else if (this.mNowNumber < this.mEndNumber)
          ++this.mNowNumber;
        if (this.mNowNumber == this.mEndNumber)
          break;
      }
      this.mSurplus -= (float) num * this.Span;
      this.mLabel.SetTextLocalize(this.mNowNumber);
    }
    while (this.mNowNumber != this.mEndNumber);
    if (onEndCallback != null)
      onEndCallback();
    this.mNeedSkip = false;
    this.isAnime = false;
  }
}
