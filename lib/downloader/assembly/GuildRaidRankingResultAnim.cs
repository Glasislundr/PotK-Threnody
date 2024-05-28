// Decompiled with JetBrains decompiler
// Type: GuildRaidRankingResultAnim
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GuildRaidRankingResultAnim : MonoBehaviour
{
  private const string rankingNumSpriteFormat = "slc_text_{0}.png__GUI__battleUI_05__battleUI_05_prefab";
  [SerializeField]
  private UILabel txtTitle;
  [SerializeField]
  private UISprite[] sprRankingNumber;
  [SerializeField]
  private UIGrid gridDirRanking;
  [SerializeField]
  private UIButton btnTouchToNext;

  public void Initialize(string event_name, int ranking)
  {
    this.setBtnNextEnable(false);
    this.txtTitle.SetTextLocalize(event_name);
    int num1 = 1;
    for (int index = 0; index < this.sprRankingNumber.Length; ++index)
      num1 *= 10;
    int num2 = num1 - 1;
    ranking = Mathf.Min(ranking, num2);
    int num3 = ranking;
    ((IEnumerable<UISprite>) this.sprRankingNumber).ForEach<UISprite>((Action<UISprite>) (x => ((Component) x).gameObject.SetActive(false)));
    for (int index = 0; index < this.sprRankingNumber.Length; ++index)
    {
      ((Component) this.sprRankingNumber[index]).gameObject.SetActive(true);
      this.sprRankingNumber[index].spriteName = "slc_text_{0}.png__GUI__battleUI_05__battleUI_05_prefab".F((object) (num3 % 10));
      num3 /= 10;
      if (num3 <= 0)
        break;
    }
    ((IEnumerable<UISprite>) this.sprRankingNumber).ForEach<UISprite>((Action<UISprite>) (x =>
    {
      if (((Component) x).gameObject.activeSelf)
        return;
      ((Component) x).gameObject.SingletonDestory();
      x = (UISprite) null;
    }));
    this.gridDirRanking.Reposition();
  }

  public void setBtnNextEnable(bool flag) => ((Behaviour) this.btnTouchToNext).enabled = flag;

  public void Close() => Singleton<PopupManager>.GetInstance().dismiss();
}
