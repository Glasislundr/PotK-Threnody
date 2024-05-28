// Decompiled with JetBrains decompiler
// Type: BattleUI05BreakDown
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class BattleUI05BreakDown : MonoBehaviour
{
  [SerializeField]
  private UILabel Txt_Point;
  [SerializeField]
  private UILabel Txt_Title;

  public void SetPoint(string title, int point)
  {
    this.Txt_Title.SetTextLocalize(title);
    this.Txt_Point.SetTextLocalize(Consts.Format(Consts.GetInstance().RESULT_RANKING_MENU_POINT, (IDictionary) new Hashtable()
    {
      {
        (object) nameof (point),
        (object) point
      }
    }));
  }

  public void SetSpecialRate(string specialRate)
  {
    this.Txt_Title.SetTextLocalize(Consts.GetInstance().RESULT_RANKING_MENU_RATE_TITLE);
    this.Txt_Point.SetTextLocalize(Consts.Format(Consts.GetInstance().RESULT_RANKING_MENU_RATE, (IDictionary) new Hashtable()
    {
      {
        (object) "rate",
        (object) specialRate
      }
    }));
  }
}
