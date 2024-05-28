// Decompiled with JetBrains decompiler
// Type: Quest0022Bonus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Quest0022Bonus : MonoBehaviour
{
  [SerializeField]
  private GameObject container;
  [SerializeField]
  private UISprite category;
  [SerializeField]
  private UISprite limitNumberLeft;
  [SerializeField]
  private UISprite limitNumberCenter;
  [SerializeField]
  private UISprite limitNumberRight;
  [SerializeField]
  private List<GameObject> timeUnit;

  public void SetBonusCategory(int bonusCategory)
  {
    if (bonusCategory == 0)
    {
      Debug.Log((object) "＋＋＋＋＋＋＋　ボーナスなし　＋＋＋＋＋＋＋");
      this.container.SetActive(false);
    }
    else
    {
      string str = string.Format("slc_Bonus_{0}.png__GUI__002-2_sozai__002-2_sozai_prefab", (object) bonusCategory);
      if (Singleton<NGGameDataManager>.GetInstance().IsSea)
        str = str.Replace("002-2_sozai_sea", "002-2_sozai_sea");
      Debug.Log((object) string.Format("＋＋＋＋＋＋＋　spriteName : {0}  ＋＋＋＋＋＋＋", (object) str));
      UISpriteData sprite = this.category.atlas.GetSprite(str);
      if (sprite != null)
      {
        this.container.SetActive(true);
        this.category.spriteName = str;
        UIWidget component = ((Component) this.category).GetComponent<UIWidget>();
        Vector3 localPosition = ((Component) component).transform.localPosition;
        component.SetRect(0.0f, 0.0f, (float) sprite.width, (float) sprite.height);
        ((Component) component).transform.localPosition = localPosition;
      }
      else
      {
        Debug.LogWarning((object) string.Format("＋＋＋＋＋＋＋　{0}がありません　＋＋＋＋＋＋＋", (object) str));
        this.container.SetActive(false);
      }
    }
  }

  public void SetLimitTimeNumber(DateTime targetDateTime, DateTime now)
  {
    double limit1 = Math.Floor((targetDateTime - now).TotalMinutes);
    if (limit1 > 0.0)
    {
      if (limit1 > 1440.0)
      {
        double limit2 = Math.Floor(limit1 / 1440.0);
        if (limit2 > 99.0)
        {
          this.container.SetActive(false);
        }
        else
        {
          this.SetSpriteLimitTime((int) limit2);
          this.timeUnit[0].SetActive(true);
          this.timeUnit[1].SetActive(false);
          this.timeUnit[2].SetActive(false);
        }
      }
      else if (limit1 > 60.0)
      {
        this.SetSpriteLimitTime((int) Math.Floor(limit1 / 60.0));
        this.timeUnit[0].SetActive(false);
        this.timeUnit[1].SetActive(true);
        this.timeUnit[2].SetActive(false);
      }
      else
      {
        this.SetSpriteLimitTime((int) limit1);
        this.timeUnit[0].SetActive(false);
        this.timeUnit[1].SetActive(false);
        this.timeUnit[2].SetActive(true);
      }
    }
    else
      this.container.SetActive(false);
  }

  private void SetSpriteLimitTime(int limit)
  {
    if (limit >= 10)
    {
      this.SetSpriteNum((int) Math.Floor((Decimal) limit / 10M), this.limitNumberLeft);
      this.SetSpriteNum((int) Math.Floor((Decimal) limit % 10M), this.limitNumberRight);
      ((Component) this.limitNumberLeft).gameObject.SetActive(true);
      ((Component) this.limitNumberRight).gameObject.SetActive(true);
      ((Component) this.limitNumberCenter).gameObject.SetActive(false);
    }
    else
    {
      this.SetSpriteNum(limit, this.limitNumberCenter);
      ((Component) this.limitNumberLeft).gameObject.SetActive(false);
      ((Component) this.limitNumberRight).gameObject.SetActive(false);
      ((Component) this.limitNumberCenter).gameObject.SetActive(true);
    }
  }

  private void SetSpriteNum(int num, UISprite sprite)
  {
    string str = string.Format("slc_number_{0}.png__GUI__002-2_sozai__002-2_sozai_prefab", (object) num);
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
      str = str.Replace("002-2_sozai_sea", "002-2_sozai_sea");
    UISpriteData sprite1 = sprite.atlas.GetSprite(str);
    ((Component) sprite).gameObject.SetActive(true);
    sprite.spriteName = str;
    UIWidget component = ((Component) sprite).GetComponent<UIWidget>();
    Vector3 localPosition = ((Component) component).transform.localPosition;
    component.SetRect(0.0f, 0.0f, (float) sprite1.width, (float) sprite1.height);
    ((Component) component).transform.localPosition = localPosition;
  }
}
