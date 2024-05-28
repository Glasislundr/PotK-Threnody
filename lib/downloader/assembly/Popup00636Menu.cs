// Decompiled with JetBrains decompiler
// Type: Popup00636Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using UnityEngine;

#nullable disable
public class Popup00636Menu : Popup00635Menu
{
  [SerializeField]
  private GameObject m_Digit;
  [SerializeField]
  private GameObject m_DigitDouble;
  [SerializeField]
  private UISprite m_DigitNumber;
  [SerializeField]
  private UISprite[] m_DigitNumberDouble;
  [SerializeField]
  private UILabel m_CostMulti;
  [SerializeField]
  private UIButton m_MultiButton;

  public void Init(GachaModuleGacha gachaData, Action[] playGacha, int multiTimes)
  {
    this.Init(gachaData, playGacha[0]);
    this.SetCostNumber(gachaData, multiTimes);
    this.InitMultiButton(this.m_MultiButton, playGacha[1], multiTimes);
  }

  private void InitMultiButton(UIButton button, Action playGacha, int times)
  {
    this.SetExecuteTimes(times);
    button.SetPossibility(times > 1);
    EventDelegate.Set(button.onClick, (EventDelegate.Callback) (() => playGacha()));
  }

  private void SetExecuteTimes(int times)
  {
    if (99 < times || times < 0)
      return;
    times = times > 1 ? times : 10;
    int num1 = times % 10;
    int num2 = times / 10;
    this.m_Digit.gameObject.SetActive(times < 10);
    this.m_DigitDouble.gameObject.SetActive(times >= 10);
    if (times < 10)
    {
      this.m_DigitNumber.ChangeSprite(string.Format("slc_button_text_number_{0}_42pt.png__GUI__006-3_sozai__006-3_sozai_prefab", (object) num1));
    }
    else
    {
      this.m_DigitNumberDouble[0].ChangeSprite(string.Format("slc_button_text_number_{0}_42pt.png__GUI__006-3_sozai__006-3_sozai_prefab", (object) num1));
      this.m_DigitNumberDouble[1].ChangeSprite(string.Format("slc_button_text_number_{0}_42pt.png__GUI__006-3_sozai__006-3_sozai_prefab", (object) num2));
    }
  }

  private void SetCostNumber(GachaModuleGacha gachaData, int times)
  {
    if (times < 2)
      times = 10;
    this.m_CostMulti.SetTextLocalize(gachaData.payment_amount * times);
  }
}
