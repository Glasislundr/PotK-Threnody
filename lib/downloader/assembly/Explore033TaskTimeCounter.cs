// Decompiled with JetBrains decompiler
// Type: Explore033TaskTimeCounter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class Explore033TaskTimeCounter : MonoBehaviour
{
  private const string TIME_FORMAT_M_S = "{0}分{1}秒";
  private const string TIME_FORMAT_S = "{0}秒";
  [SerializeField]
  private GameObject[] mCounterObj;
  [SerializeField]
  private UILabel mCounterMiniSecLbl;
  [SerializeField]
  private UILabel mCounterFullMinLbl;
  [SerializeField]
  private UILabel mCounterFullSecLbl;
  private float mStartTime;
  private long mCount;
  private long mRestCount;

  public void StartCounter(long count)
  {
    this.mStartTime = Time.realtimeSinceStartup;
    this.mRestCount = this.mCount = count;
    ((Component) this).gameObject.SetActive(true);
  }

  private void updateCounterLabel()
  {
    int num1 = (int) (this.mRestCount / 60000L);
    int num2 = Mathf.CeilToInt((float) (this.mRestCount % 60000L) / 1000f);
    if (num1 > 0)
    {
      this.mCounterObj[0].SetActive(false);
      this.mCounterObj[1].SetActive(true);
      this.mCounterFullMinLbl.SetTextLocalize(num1);
      this.mCounterFullSecLbl.SetTextLocalize(num2);
    }
    else
    {
      this.mCounterObj[0].SetActive(true);
      this.mCounterObj[1].SetActive(false);
      this.mCounterMiniSecLbl.SetTextLocalize(num2);
    }
    if (num1 != 0 || num2 != 0)
      return;
    ((Component) this).gameObject.SetActive(false);
  }

  private void Update()
  {
    this.mRestCount = this.mCount - (long) (((double) Time.realtimeSinceStartup - (double) this.mStartTime) * 1000.0);
    this.mRestCount = Math.Max(this.mRestCount, 0L);
    this.updateCounterLabel();
  }

  private void OnEnable() => this.updateCounterLabel();

  private void OnDisable() => this.mRestCount = 0L;
}
