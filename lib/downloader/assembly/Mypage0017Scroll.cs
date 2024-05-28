// Decompiled with JetBrains decompiler
// Type: Mypage0017Scroll
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Mypage0017Scroll : MonoBehaviour
{
  [SerializeField]
  public GameObject[] PresentObject;
  [SerializeField]
  public UILabel[] PresentExplanation;
  [SerializeField]
  public UILabel[] PresentDate;
  [SerializeField]
  public UILabel[] PresentTime;
  [SerializeField]
  public UIButton[] PresentReceive;
  [SerializeField]
  public UIButton[] PresentHaveReceive;
  [SerializeField]
  public UISprite[] PresentNew;
  [SerializeField]
  public UISprite[] PresentLock;
  [SerializeField]
  public UILabel PresentName;
  [SerializeField]
  public UISprite Present;
  public Mypage0017Scroll.Mode mode;

  public UIButton GetReceive() => this.PresentReceive[(int) this.mode];

  public UIButton GetHaveReceive() => this.PresentHaveReceive[(int) this.mode];

  public IEnumerator Init(PlayerPresent present)
  {
    this.mode = !present.reward_type_id.HasValue ? Mypage0017Scroll.Mode.WithOutPresent : Mypage0017Scroll.Mode.Present;
    this.mode = Mypage0017Scroll.Mode.Present;
    int mode = (int) this.mode;
    ((IEnumerable<GameObject>) this.PresentObject).ToggleOnce(mode);
    string text1 = present.message;
    if (text1.Length > 32)
      text1 = text1.Substring(0, 29).Replace("\r", "").Replace("\n", "") + "．．．";
    this.PresentExplanation[mode].SetTextLocalize(text1);
    string str1 = string.Format("{0:00}", (object) present.created_at.Month);
    string str2 = string.Format("{0:00}", (object) present.created_at.Day);
    string str3 = string.Format("{0:00}", (object) present.created_at.Hour);
    string str4 = string.Format("{0:00}", (object) present.created_at.Minute);
    this.PresentDate[mode].SetTextLocalize(Consts.Format(Consts.GetInstance().Mypage0017Scroll_MonthDay, (IDictionary) new Hashtable()
    {
      {
        (object) "month",
        (object) str1
      },
      {
        (object) "day",
        (object) str2
      }
    }));
    this.PresentTime[mode].SetTextLocalize(Consts.Format(Consts.GetInstance().Mypage0017Scroll_HourMin, (IDictionary) new Hashtable()
    {
      {
        (object) "hour",
        (object) str3
      },
      {
        (object) "minute",
        (object) str4
      }
    }));
    if (!present.received_at.HasValue)
    {
      ((Component) this.PresentReceive[mode]).gameObject.SetActive(true);
      ((Component) this.PresentHaveReceive[mode]).gameObject.SetActive(false);
      ((Component) this.PresentNew[mode]).gameObject.SetActive(true);
    }
    else
    {
      ((Component) this.PresentReceive[mode]).gameObject.SetActive(false);
      ((Component) this.PresentHaveReceive[mode]).gameObject.SetActive(true);
      ((Component) this.PresentNew[mode]).gameObject.SetActive(false);
    }
    ((Component) this.PresentLock[mode]).gameObject.SetActive(false);
    if (this.mode == Mypage0017Scroll.Mode.Present)
    {
      string text2 = present.title;
      if (text2.Length > 16)
        text2 = text2.Substring(0, 13) + "．．．";
      this.PresentName.SetTextLocalize(text2);
      ((Component) this.Present).gameObject.SetActive(true);
      yield break;
    }
  }

  public enum Mode
  {
    Present,
    WithOutPresent,
  }
}
