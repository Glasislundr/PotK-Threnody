// Decompiled with JetBrains decompiler
// Type: GrowthParameter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GrowthParameter : MonoBehaviour
{
  public static int[] MaxParameters = new int[8]
  {
    198,
    99,
    99,
    99,
    99,
    99,
    99,
    99
  };
  public UILabel[] resultParameters;
  public UISprite[] gaugeBlue;
  public UISprite[] gaugeYellow;
  public UISprite[] gaugeWhite;
  public GameObject[] dirUppt;
  public GameObject[] slcParamMaxStars;
  private List<GameObject> upptParameters = new List<GameObject>();
  private List<UISprite[]> gaugeSprites = new List<UISprite[]>();
  private List<TweenScale> tweenObject = new List<TweenScale>();

  public static Future<GameObject> LoadPrefab()
  {
    return Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/unit004_8_13/GrowthParameter");
  }

  public static GrowthParameter GetInstance(GameObject prefab, Transform parent)
  {
    GameObject gameObject = Object.Instantiate<GameObject>(prefab);
    gameObject.transform.parent = parent;
    gameObject.transform.localPosition = Vector3.zero;
    gameObject.transform.localScale = Vector3.one;
    gameObject.transform.localRotation = Quaternion.identity;
    return gameObject.GetComponent<GrowthParameter>();
  }

  private void SetGauge(GameObject gauge, float rate, float delay)
  {
    UISprite component = gauge.GetComponent<UISprite>();
    ((UIWidget) component).width = (int) ((double) ((UIWidget) component).width * (double) rate);
    TweenScale tweenScale = gauge.AddComponent<TweenScale>();
    tweenScale.from = new Vector3() { x = 0.0f, y = 1f };
    tweenScale.to = new Vector3() { x = 1f, y = 1f };
    ((UITweener) tweenScale).style = (UITweener.Style) 0;
    ((UITweener) tweenScale).duration = 1f;
    ((UITweener) tweenScale).delay = 0.2f + delay;
  }

  public void SetParameter(
    GrowthParameter.ParameterType type,
    int beforePt,
    int afterPt,
    bool isBuildup)
  {
    this.SetParameter(type, beforePt, afterPt, isBuildup, GrowthParameter.MaxParameters[(int) type]);
  }

  public void SetParameter(
    GrowthParameter.ParameterType type,
    int growthValue,
    int beforePt,
    int afterPt,
    bool isBuildup)
  {
    this.SetParameter(type, growthValue, beforePt, afterPt, isBuildup, GrowthParameter.MaxParameters[(int) type]);
  }

  private void SetParameter(
    GrowthParameter.ParameterType type,
    int growthValue,
    int beforePt,
    int afterPt,
    bool isBuildup,
    int maxPt)
  {
    Func<int, float> func = (Func<int, float>) (v => v <= 0 ? 0.0f : (float) v / (float) maxPt);
    UISprite[] self = new UISprite[2];
    float[] gaugeRate = new float[2]
    {
      func(beforePt),
      func(afterPt)
    };
    this.resultParameters[(int) type].SetTextLocalize(afterPt);
    ((UIWidget) this.resultParameters[(int) type]).color = growthValue > 0 ? Color.yellow : Color.white;
    self[0] = this.gaugeBlue[(int) type];
    if (!isBuildup)
    {
      self[1] = this.gaugeYellow[(int) type];
      ((Component) this.gaugeYellow[(int) type]).gameObject.SetActive(true);
      ((Component) this.gaugeWhite[(int) type]).gameObject.SetActive(false);
    }
    else
    {
      self[1] = this.gaugeWhite[(int) type];
      ((Component) this.gaugeYellow[(int) type]).gameObject.SetActive(false);
      ((Component) this.gaugeWhite[(int) type]).gameObject.SetActive(true);
    }
    if (growthValue <= 0)
    {
      this.dirUppt[(int) type].gameObject.SetActive(false);
    }
    else
    {
      this.dirUppt[(int) type].gameObject.SetActive(true);
      ((Component) this.dirUppt[(int) type].transform.Find("txt_Uppt")).GetComponent<UILabel>().SetTextLocalize(growthValue);
    }
    ((IEnumerable<UISprite>) self).ForEachIndex<UISprite>((Action<UISprite, int>) ((gauge, index) => ((UIWidget) gauge).width = (int) ((double) ((UIWidget) gauge).width * (double) gaugeRate[index])));
    this.gaugeSprites.Add(self);
  }

  public void SetParameter(
    GrowthParameter.ParameterType type,
    int beforePt,
    int afterPt,
    bool isBuildup,
    int maxPt)
  {
    this.SetParameter(type, afterPt - beforePt, beforePt, afterPt, isBuildup, maxPt);
  }

  public void GaugesScaleZero()
  {
    foreach (UISprite[] gaugeSprite in this.gaugeSprites)
    {
      foreach (Component component in gaugeSprite)
        component.transform.localScale = Vector3.zero;
    }
  }

  public void SetParameterEffects()
  {
    this.tweenObject.Clear();
    this.gaugeSprites.ForEachIndex<UISprite[]>((Action<UISprite[], int>) ((gauges, index) =>
    {
      float delay = (float) (0.20000000298023224 + (double) index * 0.05000000074505806);
      ((IEnumerable<UISprite>) gauges).ForEach<UISprite>((Action<UISprite>) (gauge =>
      {
        TweenScale tweenScale = ((Component) gauge).gameObject.AddComponent<TweenScale>();
        ((UITweener) tweenScale).style = (UITweener.Style) 0;
        ((UITweener) tweenScale).duration = 0.5f;
        ((UITweener) tweenScale).delay = delay;
        ((UITweener) tweenScale).tweenGroup = 11;
        ((UITweener) tweenScale).ignoreTimeScale = true;
        tweenScale.from = new Vector3() { x = 0.0f, y = 1f };
        tweenScale.to = new Vector3() { x = 1f, y = 1f };
        this.tweenObject.Add(tweenScale);
      }));
    }));
    this.gaugeSprites.Clear();
    this.upptParameters.ForEachIndex<GameObject>((Action<GameObject, int>) ((gameObject, index) =>
    {
      GameObject effObj = gameObject.Clone();
      effObj.transform.position = gameObject.transform.position;
      effObj.transform.parent = gameObject.transform.parent;
      float num = (float) (0.30000001192092896 + (double) index * 0.30000001192092896);
      TweenScale tweenScale = effObj.AddComponent<TweenScale>();
      ((UITweener) tweenScale).style = (UITweener.Style) 0;
      ((UITweener) tweenScale).duration = 0.5f;
      ((UITweener) tweenScale).delay = num;
      ((UITweener) tweenScale).tweenGroup = 11;
      tweenScale.to = new Vector3() { x = 1.5f, y = 1.5f };
      ((UITweener) tweenScale).ignoreTimeScale = true;
      TweenAlpha tweenAlpha = effObj.AddComponent<TweenAlpha>();
      ((UITweener) tweenAlpha).style = (UITweener.Style) 0;
      tweenAlpha.from = 0.87f;
      tweenAlpha.to = 0.0f;
      ((UITweener) tweenAlpha).duration = 0.5f;
      ((UITweener) tweenAlpha).delay = num;
      ((UITweener) tweenAlpha).tweenGroup = 11;
      ((UITweener) tweenAlpha).ignoreTimeScale = true;
      EventDelegate.Add(((UITweener) tweenAlpha).onFinished, (EventDelegate.Callback) (() => Object.Destroy((Object) effObj)));
    }));
    this.upptParameters.Clear();
  }

  public void RemoveTween()
  {
    this.tweenObject.ForEach((Action<TweenScale>) (v => Object.DestroyImmediate((Object) v)));
  }

  public void SetParameterMaxStar(GrowthParameter.ParameterType type, bool isDisp)
  {
    this.slcParamMaxStars[(int) type].SetActive(isDisp);
  }

  public void DisableParameter(GrowthParameter.ParameterType type)
  {
    this.resultParameters[(int) type].SetTextLocalize("---");
    ((UIWidget) this.resultParameters[(int) type]).color = Color.white;
    ((Component) this.gaugeBlue[(int) type]).gameObject.SetActive(false);
    ((Component) this.gaugeYellow[(int) type]).gameObject.SetActive(false);
    ((Component) this.gaugeWhite[(int) type]).gameObject.SetActive(false);
    this.dirUppt[(int) type].gameObject.SetActive(false);
  }

  public enum ParameterType
  {
    HP,
    STR,
    INT,
    VIT,
    MND,
    AGI,
    DEX,
    LUK,
  }
}
