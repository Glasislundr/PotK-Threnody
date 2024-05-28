// Decompiled with JetBrains decompiler
// Type: ExploreRankingResultAnim
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ExploreRankingResultAnim : MonoBehaviour
{
  private const string rankingNumSpriteFormat = "slc_text_{0}.png__GUI__battleUI_05__battleUI_05_prefab";
  [SerializeField]
  private UILabel txtPeriod;
  [SerializeField]
  private UILabel txtDefeatNum;
  [SerializeField]
  private UILabel txtExploreFloor;
  [SerializeField]
  private UISprite[] sprRankingNumber;
  [SerializeField]
  private UIButton btnTouchToNext;
  [SerializeField]
  private Animator animator;

  public void Initialize(ExploreRankingPeriod period_data, int rank, int defeat, int floor)
  {
    ((Behaviour) this.btnTouchToNext).enabled = false;
    this.txtPeriod.SetTextLocalize(Consts.GetInstance().EXPLORE_RANKING_RESULT_PERIOD_FORMAT.F((object) period_data.start_at.Value.Year, (object) period_data.start_at.Value.Month, (object) period_data.start_at.Value.Day, (object) period_data.end_at.Value.Month, (object) period_data.end_at.Value.Day));
    this.txtDefeatNum.SetTextLocalize(defeat);
    this.txtExploreFloor.SetTextLocalize(floor);
    int num1 = 1;
    for (int index = 0; index < this.sprRankingNumber.Length; ++index)
      num1 *= 10;
    int num2 = num1 - 1;
    rank = Mathf.Min(rank, num2);
    int num3 = rank;
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
    ((Behaviour) this.btnTouchToNext).enabled = true;
  }

  public void OnNext()
  {
    ((Behaviour) this.btnTouchToNext).enabled = false;
    this.StartCoroutine(this.CloseWait());
  }

  public IEnumerator CloseWait()
  {
    this.animator.SetTrigger("on_pressed");
    yield return (object) new WaitForSecondsRealtime(0.5f);
    Singleton<PopupManager>.GetInstance().dismiss();
  }
}
