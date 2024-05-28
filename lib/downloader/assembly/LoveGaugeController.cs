// Decompiled with JetBrains decompiler
// Type: LoveGaugeController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class LoveGaugeController : MonoBehaviour
{
  [SerializeField]
  private NGTweenGaugeFillAmount gauge;
  [SerializeField]
  private NGTweenGaugeFillAmount gaugeCap;
  [SerializeField]
  private GameObject heartObject;
  [SerializeField]
  private TweenPosition positionTween;
  [SerializeField]
  private UISprite slcDearDegree;
  [SerializeField]
  private float minPositionX;
  [SerializeField]
  private float maxPositionX;
  private int seChannel = -1;
  private bool isGaugeAnimationStopped;
  private bool isAutoStopSE;

  public void setValue(int value, int cap, int max, bool doTween, bool isSe = false)
  {
    this.gaugeCap.setValue(cap, max, false, -1f, -1f);
    if (!this.gauge.setValue(value, max, doTween, -1f, -1f))
      return;
    float num = (this.maxPositionX - this.minPositionX) * ((float) value / (float) max) + this.minPositionX;
    Vector3 localPosition = this.heartObject.transform.localPosition;
    Vector3 vector3;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3).\u002Ector(num, localPosition.y, localPosition.z);
    if (doTween && Object.op_Inequality((Object) this.positionTween, (Object) null))
    {
      this.positionTween.worldSpace = false;
      this.positionTween.from = localPosition;
      this.positionTween.to = vector3;
      if (isSe)
      {
        this.seChannel = Singleton<NGSoundManager>.GetInstance().playSE("SE_1045");
        this.isAutoStopSE = true;
      }
      NGTween.playTween((UITweener) this.positionTween);
    }
    else
      this.heartObject.transform.localPosition = vector3;
  }

  public IEnumerator setValue(
    int start,
    int end,
    int max,
    int interval,
    bool doTween,
    bool isSe = false)
  {
    LoveGaugeController loveGaugeController = this;
    loveGaugeController.isGaugeAnimationStopped = false;
    loveGaugeController.gaugeCap.setValue(max, max, false, -1f, -1f);
    Vector3 heartPositionMin = new Vector3(loveGaugeController.minPositionX, loveGaugeController.heartObject.transform.localPosition.y, loveGaugeController.heartObject.transform.localPosition.z);
    Vector3 heartPositionMax = new Vector3(loveGaugeController.maxPositionX, loveGaugeController.heartObject.transform.localPosition.y, loveGaugeController.heartObject.transform.localPosition.z);
    if (start != end)
    {
      bool loopFinish = false;
      int num1 = start;
      bool flag = true;
      while (!loopFinish)
      {
        int num2 = num1 / interval * interval;
        int num3 = (num1 / interval + 1) * interval;
        if (num3 >= max)
          num3 = max;
        int num4 = num3 - num2;
        int toValue = end;
        if (toValue > num3)
          toValue = num3;
        else
          loopFinish = true;
        float fromPercent = (float) (num1 - num2) / (float) num4;
        float toPercent = (float) (toValue - num2) / (float) num4;
        if (doTween && Object.op_Inequality((Object) loveGaugeController.positionTween, (Object) null))
        {
          loveGaugeController.positionTween.worldSpace = false;
          loveGaugeController.positionTween.from = Vector3.Lerp(heartPositionMin, heartPositionMax, fromPercent);
          loveGaugeController.positionTween.to = Vector3.Lerp(heartPositionMin, heartPositionMax, toPercent);
          NGTween.playTween((UITweener) loveGaugeController.positionTween);
        }
        else
          loveGaugeController.heartObject.transform.localPosition = Vector3.Lerp(heartPositionMin, heartPositionMax, toPercent);
        if (flag)
        {
          loveGaugeController.gauge.setValue(fromPercent, toPercent, doTween, 0.75f);
          if (((!doTween ? 0 : (Object.op_Inequality((Object) loveGaugeController.positionTween, (Object) null) ? 1 : 0)) & (isSe ? 1 : 0)) != 0)
          {
            yield return (object) new WaitForSeconds(1f);
            loveGaugeController.seChannel = Singleton<NGSoundManager>.GetInstance().playSE("SE_1045");
          }
        }
        else
          loveGaugeController.gauge.setValue(fromPercent, toPercent, doTween, 0.0f);
        while (loveGaugeController.gauge.isAnimationPlaying || ((Behaviour) loveGaugeController.positionTween).isActiveAndEnabled)
        {
          if (loveGaugeController.isGaugeAnimationStopped)
          {
            loveGaugeController.StartCoroutine(loveGaugeController.setValue(end, end, max, interval, false));
            loveGaugeController.StopSE();
            yield break;
          }
          else
            yield return (object) null;
        }
        num1 = toValue;
        flag = false;
      }
      loveGaugeController.StopSE();
    }
    else
    {
      float num = (float) (end % interval) / (float) interval;
      if (end != 0 && (double) num == 0.0 || end == max)
        num = 100f;
      loveGaugeController.heartObject.transform.localPosition = Vector3.Lerp(heartPositionMin, heartPositionMax, num);
      loveGaugeController.gauge.setValue(num, num, false);
    }
  }

  public void SetGaugeText(UnitUnit unit, string dearSpriteName, string relevanceSpriteName)
  {
    if (unit.IsSea)
    {
      this.slcDearDegree.spriteName = dearSpriteName;
    }
    else
    {
      if (!unit.IsResonanceUnit)
        return;
      this.slcDearDegree.spriteName = relevanceSpriteName;
    }
  }

  public void endPositionTween()
  {
    if (!this.isAutoStopSE)
      return;
    this.StopSE();
  }

  public void StopSE()
  {
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    if (Object.op_Inequality((Object) instance, (Object) null) && this.seChannel != -1)
      instance.stopSE(this.seChannel);
    this.seChannel = -1;
  }

  public void StopGaugeAnimation() => this.isGaugeAnimationStopped = true;
}
