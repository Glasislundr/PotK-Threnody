// Decompiled with JetBrains decompiler
// Type: BannerLimitEmphasie
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class BannerLimitEmphasie : MonoBehaviour
{
  [SerializeField]
  private GameObject slcBase;
  [SerializeField]
  private GameObject slcFor;
  [SerializeField]
  private GameObject dirDays;
  [SerializeField]
  private GameObject dirHours;
  [SerializeField]
  private GameObject dirHoursEff;
  [SerializeField]
  private GameObject dirMinutes;
  [SerializeField]
  private GameObject dirMinutesEff;
  [SerializeField]
  private UISprite digit01Day;
  [SerializeField]
  private UISprite digit10Day;
  [SerializeField]
  private UISprite digitCenterDay;
  [SerializeField]
  private UISprite digit01Hours;
  [SerializeField]
  private UISprite digit10Hours;
  [SerializeField]
  private UISprite digitCenterHours;
  [SerializeField]
  private UISprite digit01HoursEff;
  [SerializeField]
  private UISprite digit10HoursEff;
  [SerializeField]
  private UISprite digitCenterHoursEff;
  [SerializeField]
  private UISprite digit01Minutes;
  [SerializeField]
  private UISprite digit10Minutes;
  [SerializeField]
  private UISprite digitCenterMinutes;
  [SerializeField]
  private UISprite digit01MinutesEff;
  [SerializeField]
  private UISprite digit10MinutesEff;
  [SerializeField]
  private UISprite digitCenterMinutesEff;

  public void Init(DateTime serverTime, DateTime? EndTime)
  {
    this.slcBase.SetActive(false);
    this.slcFor.SetActive(false);
    this.dirDays.SetActive(false);
    this.dirHours.SetActive(false);
    this.dirMinutes.SetActive(false);
    this.dirHoursEff.SetActive(false);
    this.dirMinutesEff.SetActive(false);
    if (!EndTime.HasValue)
      return;
    TimeSpan timeSpan = EndTime.Value - serverTime;
    if (timeSpan.TotalDays >= 100.0)
      return;
    this.slcBase.SetActive(true);
    this.slcFor.SetActive(true);
    int num1;
    int num2;
    int num3;
    if (timeSpan.TotalDays >= 1.0)
    {
      num1 = (int) (timeSpan.TotalDays / 10.0);
      num2 = (int) (timeSpan.TotalDays % 10.0);
      num3 = 0;
    }
    else if (timeSpan.TotalHours >= 1.0)
    {
      int num4 = (int) (timeSpan.TotalMinutes / 60.0) + (timeSpan.TotalMinutes % 60.0 != 0.0 ? 1 : 0);
      num1 = num4 / 10;
      num2 = num4 % 10;
      num3 = 1;
    }
    else
    {
      if (timeSpan.TotalSeconds <= 0.0)
      {
        num1 = 0;
        num2 = 0;
      }
      else
      {
        int num5 = (int) (timeSpan.TotalSeconds / 60.0) + (timeSpan.TotalSeconds % 60.0 != 0.0 ? 1 : 0);
        num1 = num5 / 10;
        num2 = num5 % 10;
      }
      num3 = 2;
    }
    GameObject gameObject1 = (GameObject) null;
    UISprite uiSprite1 = (UISprite) null;
    UISprite uiSprite2 = (UISprite) null;
    UISprite uiSprite3 = (UISprite) null;
    string str1 = string.Format("slc_number_eff_{0}.png__GUI__banner_limit__banner_limit_prefab", (object) num2);
    string str2 = string.Format("slc_number_eff_{0}.png__GUI__banner_limit__banner_limit_prefab", (object) num1);
    GameObject gameObject2;
    UISprite uiSprite4;
    UISprite uiSprite5;
    UISprite uiSprite6;
    string str3;
    string str4;
    switch (num3)
    {
      case 0:
        gameObject2 = this.dirDays;
        uiSprite4 = this.digitCenterDay;
        uiSprite5 = this.digit01Day;
        uiSprite6 = this.digit10Day;
        str3 = string.Format("slc_number_days_{0}.png__GUI__banner_limit__banner_limit_prefab", (object) num2);
        str4 = string.Format("slc_number_days_{0}.png__GUI__banner_limit__banner_limit_prefab", (object) num1);
        break;
      case 1:
        gameObject2 = this.dirHours;
        gameObject1 = this.dirHoursEff;
        uiSprite4 = this.digitCenterHours;
        uiSprite1 = this.digitCenterHoursEff;
        uiSprite5 = this.digit01Hours;
        uiSprite2 = this.digit01HoursEff;
        uiSprite6 = this.digit10Hours;
        uiSprite3 = this.digit10HoursEff;
        str3 = string.Format("slc_number_hours_{0}.png__GUI__banner_limit__banner_limit_prefab", (object) num2);
        str4 = string.Format("slc_number_hours_{0}.png__GUI__banner_limit__banner_limit_prefab", (object) num1);
        break;
      default:
        gameObject2 = this.dirMinutes;
        gameObject1 = this.dirMinutesEff;
        uiSprite4 = this.digitCenterMinutes;
        uiSprite1 = this.digitCenterMinutesEff;
        uiSprite5 = this.digit01Minutes;
        uiSprite2 = this.digit01MinutesEff;
        uiSprite6 = this.digit10Minutes;
        uiSprite3 = this.digit10MinutesEff;
        str3 = string.Format("slc_number_minutes_{0}.png__GUI__banner_limit__banner_limit_prefab", (object) num2);
        str4 = string.Format("slc_number_minutes_{0}.png__GUI__banner_limit__banner_limit_prefab", (object) num1);
        break;
    }
    gameObject2.SetActive(true);
    ((Component) uiSprite4).gameObject.SetActive(false);
    ((Component) uiSprite5).gameObject.SetActive(false);
    ((Component) uiSprite6).gameObject.SetActive(false);
    if (Object.op_Inequality((Object) uiSprite1, (Object) null))
      ((Component) uiSprite1).gameObject.SetActive(false);
    if (Object.op_Inequality((Object) uiSprite2, (Object) null))
      ((Component) uiSprite2).gameObject.SetActive(false);
    if (Object.op_Inequality((Object) uiSprite3, (Object) null))
      ((Component) uiSprite3).gameObject.SetActive(false);
    if (Object.op_Inequality((Object) gameObject1, (Object) null))
      gameObject1.SetActive(true);
    if (num1 == 0)
    {
      ((Component) uiSprite4).gameObject.SetActive(true);
      uiSprite4.spriteName = str3;
      if (!Object.op_Inequality((Object) uiSprite1, (Object) null))
        return;
      ((Component) uiSprite1).gameObject.SetActive(true);
      uiSprite1.spriteName = str1;
    }
    else
    {
      ((Component) uiSprite5).gameObject.SetActive(true);
      ((Component) uiSprite6).gameObject.SetActive(true);
      uiSprite5.spriteName = str3;
      uiSprite6.spriteName = str4;
      if (Object.op_Inequality((Object) uiSprite2, (Object) null))
      {
        ((Component) uiSprite2).gameObject.SetActive(true);
        uiSprite2.spriteName = str1;
      }
      if (!Object.op_Inequality((Object) uiSprite3, (Object) null))
        return;
      ((Component) uiSprite3).gameObject.SetActive(true);
      uiSprite3.spriteName = str2;
    }
  }
}
