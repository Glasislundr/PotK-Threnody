// Decompiled with JetBrains decompiler
// Type: Unit00483StatusGauge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using UnityEngine;

#nullable disable
public class Unit00483StatusGauge : MonoBehaviour
{
  private const int HP_MAX = 198;
  private const int STATUS_MAX = 99;
  private const int STATUS_MAX_WITH_SPECIALTY = 200;
  public GameObject incBase;
  public UILabel txtUppt;
  public GameObject paramMaxStar;
  public UILabel valueLabel;
  public UISprite greenGaugeL;
  public UISprite yellowGaugeL;
  public UISprite blueGauge;
  public UISprite greenGauge;
  public UISprite yellowGauge;
  public UISprite blueMaxGauge;
  public UISprite whiteGauge;
  public UISprite whiteAddGauge;
  private const int GAUGE_WIDTH = 99;

  private void SetGaugeInfo(UISprite gauge, int width, float posX)
  {
    ((UIWidget) gauge).width = width;
    ((Component) gauge).transform.localPosition = new Vector3(posX, 0.0f, 0.0f);
  }

  private int getGaugeWidth(int w, int v, int max, bool reduction)
  {
    Func<int, int> func = (Func<int, int>) (x => Math.Min(x, max));
    return reduction ? (int) Math.Round((double) w * ((double) func(v != 0 ? v + 1 : v) / (double) max), MidpointRounding.AwayFromZero) : (int) Math.Round((double) w * ((double) func(v) / (double) max));
  }

  private int getGaugeWidthCeil(int w, int v, int max)
  {
    if (v == 0)
      return 0;
    int num = (int) Math.Round((double) (w * Math.Min(v, max)) / (double) max);
    return num == 0 ? 1 : num;
  }

  private void SetGauge(
    int baseValue,
    int buildupValue,
    int composeValue,
    int inc,
    int buildupInc,
    int max,
    bool isComposeMax,
    bool reduction)
  {
    this.valueLabel.SetTextLocalize((baseValue + buildupInc + inc).ToString());
    ((UIWidget) this.valueLabel).color = inc != 0 || buildupInc != 0 ? Color.yellow : Color.white;
    int gaugeWidth1 = this.getGaugeWidth(99, baseValue, max, reduction);
    int gaugeWidth2 = this.getGaugeWidth(99, buildupValue, max, reduction);
    int gaugeWidth3 = this.getGaugeWidth(99, buildupInc, max, reduction);
    int gaugeWidth4 = this.getGaugeWidth(99, composeValue, max, reduction);
    int gaugeWidth5 = this.getGaugeWidth(99, inc, max, reduction);
    float posX1 = ((Component) this.blueGauge).transform.localPosition.x + (float) gaugeWidth1;
    float posX2 = posX1 + (float) gaugeWidth2;
    float posX3 = posX2 + (float) gaugeWidth3;
    float posX4 = posX3 + (float) gaugeWidth4;
    this.SetGaugeInfo(this.blueGauge, gaugeWidth1, ((Component) this.blueGauge).transform.localPosition.x);
    this.SetGaugeInfo(this.whiteGauge, gaugeWidth2, posX1);
    this.SetGaugeInfo(this.whiteAddGauge, gaugeWidth3, posX2);
    this.SetGaugeInfo(this.greenGauge, gaugeWidth4, posX3);
    this.SetGaugeInfo(this.blueMaxGauge, gaugeWidth4, posX3);
    this.SetGaugeInfo(this.yellowGauge, gaugeWidth5, posX4);
    ((Component) this.blueGauge).gameObject.SetActive(gaugeWidth1 != 0);
    ((Component) this.yellowGauge).gameObject.SetActive(gaugeWidth5 != 0);
    ((Component) this.whiteGauge).gameObject.SetActive(gaugeWidth2 != 0);
    ((Component) this.whiteAddGauge).gameObject.SetActive(gaugeWidth3 != 0);
    if (isComposeMax)
    {
      ((Component) this.greenGauge).gameObject.SetActive(false);
      ((Component) this.blueMaxGauge).gameObject.SetActive(true);
    }
    else
    {
      ((Component) this.greenGauge).gameObject.SetActive(gaugeWidth4 != 0);
      ((Component) this.blueMaxGauge).gameObject.SetActive(false);
    }
    this.incBase.SetActive(inc + buildupInc > 0);
    if (!this.incBase.activeSelf)
      return;
    this.txtUppt.SetTextLocalize(inc + buildupInc);
  }

  private void SetGauge(
    int baseInheritance,
    int incInheritance,
    int baseValue,
    int buildupValue,
    int composeValue,
    int inc,
    int buildupInc,
    int max,
    bool isComposeMax)
  {
    int num = incInheritance + buildupInc + inc;
    int v = baseValue + num;
    this.valueLabel.SetTextLocalize(v.ToString());
    ((UIWidget) this.valueLabel).color = num == 0 ? Color.white : Color.yellow;
    int gaugeWidthCeil1 = this.getGaugeWidthCeil(99, baseInheritance, max);
    int gaugeWidthCeil2 = this.getGaugeWidthCeil(99, incInheritance, max);
    int gaugeWidthCeil3 = this.getGaugeWidthCeil(99, buildupValue, max);
    int gaugeWidthCeil4 = this.getGaugeWidthCeil(99, buildupInc, max);
    int gaugeWidthCeil5 = this.getGaugeWidthCeil(99, composeValue, max);
    int gaugeWidthCeil6 = this.getGaugeWidthCeil(99, inc, max);
    int width = this.getGaugeWidthCeil(99, v, max) - (gaugeWidthCeil1 + gaugeWidthCeil2 + gaugeWidthCeil3 + gaugeWidthCeil4 + gaugeWidthCeil5 + gaugeWidthCeil6);
    if (width < 0)
      width = 0;
    float posX1 = (float) gaugeWidthCeil1;
    float posX2 = posX1 + (float) gaugeWidthCeil2;
    float posX3 = posX2 + (float) width;
    float posX4 = posX3 + (float) gaugeWidthCeil3;
    float posX5 = posX4 + (float) gaugeWidthCeil4;
    float posX6 = posX5 + (float) gaugeWidthCeil5;
    this.SetGaugeInfo(this.greenGaugeL, gaugeWidthCeil1, 0.0f);
    this.SetGaugeInfo(this.yellowGaugeL, gaugeWidthCeil2, posX1);
    this.SetGaugeInfo(this.blueGauge, width, posX2);
    this.SetGaugeInfo(this.whiteGauge, gaugeWidthCeil3, posX3);
    this.SetGaugeInfo(this.whiteAddGauge, gaugeWidthCeil4, posX4);
    this.SetGaugeInfo(this.greenGauge, gaugeWidthCeil5, posX5);
    this.SetGaugeInfo(this.blueMaxGauge, gaugeWidthCeil5, posX5);
    this.SetGaugeInfo(this.yellowGauge, gaugeWidthCeil6, posX6);
    ((Component) this.greenGaugeL).gameObject.SetActive(gaugeWidthCeil1 != 0);
    ((Component) this.yellowGaugeL).gameObject.SetActive(gaugeWidthCeil2 != 0);
    ((Component) this.blueGauge).gameObject.SetActive(width != 0);
    ((Component) this.yellowGauge).gameObject.SetActive(gaugeWidthCeil6 != 0);
    ((Component) this.whiteGauge).gameObject.SetActive(gaugeWidthCeil3 != 0);
    ((Component) this.whiteAddGauge).gameObject.SetActive(gaugeWidthCeil4 != 0);
    if (isComposeMax)
    {
      ((Component) this.greenGauge).gameObject.SetActive(false);
      ((Component) this.blueMaxGauge).gameObject.SetActive(true);
    }
    else
    {
      ((Component) this.greenGauge).gameObject.SetActive(gaugeWidthCeil5 != 0);
      ((Component) this.blueMaxGauge).gameObject.SetActive(false);
    }
    this.incBase.SetActive(num > 0);
    if (!this.incBase.activeSelf)
      return;
    this.txtUppt.SetTextLocalize(num);
  }

  public void Init(
    PlayerUnit basePlayerUnit,
    PlayerUnit[] materialPlayerUnits,
    CalcUnitCompose.ComposeType type,
    int tValue,
    int cValuse,
    int bValue,
    bool isMax)
  {
    int composeValue = CalcUnitCompose.getComposeValue(basePlayerUnit, materialPlayerUnits, type);
    int buildupValue = CalcUnitCompose.getBuildupValue(basePlayerUnit, materialPlayerUnits, type);
    if (basePlayerUnit.unit.compose_max_unity_value_setting_id != null)
      this.SetGauge(tValue, bValue, cValuse, composeValue, buildupValue, 200, CalcUnitCompose.isComposeMax(basePlayerUnit, type), true);
    else if (type == CalcUnitCompose.ComposeType.HP)
      this.SetGauge(tValue, bValue, cValuse, composeValue, buildupValue, 198, CalcUnitCompose.isComposeMax(basePlayerUnit, type), true);
    else
      this.SetGauge(tValue, bValue, cValuse, composeValue, buildupValue, 99, CalcUnitCompose.isComposeMax(basePlayerUnit, type), false);
    this.paramMaxStar.SetActive(isMax);
  }

  public void Init(
    PlayerUnit basePlayerUnit,
    PlayerUnit[] materialPlayerUnits,
    CalcUnitCompose.ComposeType type,
    int biValue,
    int iiValue,
    int tValue,
    int cValuse,
    int bValue,
    bool isMax,
    int gaugeMax)
  {
    int composeValue = CalcUnitCompose.getComposeValue(basePlayerUnit, materialPlayerUnits, type);
    int buildupValue = CalcUnitCompose.getBuildupValue(basePlayerUnit, materialPlayerUnits, type);
    this.SetGauge(biValue, iiValue, tValue, bValue, cValuse, composeValue, buildupValue, gaugeMax, CalcUnitCompose.isComposeMax(basePlayerUnit, type));
    this.paramMaxStar.SetActive(isMax);
  }
}
