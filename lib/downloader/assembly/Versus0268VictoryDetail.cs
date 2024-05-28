// Decompiled with JetBrains decompiler
// Type: Versus0268VictoryDetail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Versus0268VictoryDetail : MonoBehaviour
{
  [SerializeField]
  private UILabel TxtDraw;
  [SerializeField]
  private UILabel TxtLose;
  [SerializeField]
  private UILabel TxtWin;
  [SerializeField]
  private UILabel TxtRankPoint;
  [SerializeField]
  private UILabel TxtRank;
  [SerializeField]
  private UILabel TxtClass;
  [SerializeField]
  private UILabel TxtMyRank;
  [SerializeField]
  private UILabel TxtZeny;
  [SerializeField]
  private UISprite slcRankGaugeBlue;
  [SerializeField]
  private UISprite slcRankGaugeGreen;
  [SerializeField]
  private UISprite slcRankGaugeYellow;
  [SerializeField]
  private UISprite slcRankGaugeRed;
  [SerializeField]
  private GameObject slcRankGaugeNowParent;
  [SerializeField]
  private UISprite slcRankGaugeNow;
  [SerializeField]
  private UISprite slcClassCondition;
  [SerializeField]
  private Transform dirClassConditionParent;
  [SerializeField]
  private GameObject NextGaugeAnimation;
  private bool arrowAnim;
  private bool nextAnim;
  private float waitTime;

  public float WaitTime => this.waitTime;

  public IEnumerator SetDefault(
    WebAPI.Response.PvpPlayerFinish finish,
    Versus0268Menu.PvpParam param)
  {
    if (finish.matching_type != 6)
    {
      this.TxtWin.SetText(param.win.ToLocalizeNumberText());
      this.TxtDraw.SetText(param.draw.ToLocalizeNumberText());
      this.TxtLose.SetText(param.lose.ToLocalizeNumberText());
      this.waitTime = 0.0f;
    }
    else
    {
      this.TxtWin.SetText(param.season_win.ToLocalizeNumberText());
      this.TxtDraw.SetText(param.season_draw.ToLocalizeNumberText());
      this.TxtLose.SetText(param.season_lose.ToLocalizeNumberText());
      PvpClassKind c = MasterData.PvpClassKind[finish.current_class];
      this.arrowAnim = finish.pvp_finish.battle_result <= 3;
      IEnumerator e = this.SetConditionSprite(c, param.season_win);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.TxtClass.SetText(c.name);
      this.TxtMyRank.SetText(MasterData.PvpRankingKind[finish.current_rank].name);
      this.TxtZeny.SetText(finish.reward_money.ToLocalizeNumberText());
      Consts instance = Consts.GetInstance();
      int num = Player.Current.IsClassMatchRanking() ? 1 : 0;
      string text1 = num == 0 || finish.ranking <= 0 ? instance.COMMON_NOVALUE : finish.ranking.ToLocalizeNumberText();
      string text2 = num != 0 ? finish.ranking_pt.ToLocalizeNumberText() : instance.COMMON_NOVALUE;
      this.TxtRank.SetText(text1);
      this.TxtRankPoint.SetText(text2);
      this.SetRankGauge(c);
      this.SetArrow(param.season_win);
      this.waitTime = 1f;
      c = (PvpClassKind) null;
    }
  }

  private IEnumerator SetConditionSprite(PvpClassKind c, int nowWin)
  {
    PvpClassKind.Condition condition = c.ClassCondition(nowWin);
    this.nextAnim = false;
    if ((c.stay_point != 0 && c.stay_point == nowWin || c.up_point == nowWin || c.title_point == nowWin) && this.arrowAnim)
    {
      this.nextAnim = true;
      c.ClassConditionZone(ref condition);
      IEnumerator e = this.SetConditionEffect(condition);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ((Component) this.slcClassCondition).gameObject.SetActive(false);
    }
    else
    {
      string[] strArray = new string[6]
      {
        "slc_ClassDown_text.png__GUI__battleUI_05__battleUI_05_prefab",
        "slc_EscapeClassDown_text.png__GUI__battleUI_05__battleUI_05_prefab",
        "slc_EscapeClassDown_text.png__GUI__battleUI_05__battleUI_05_prefab",
        "slc_ClassUp_text.png__GUI__battleUI_05__battleUI_05_prefab",
        "slc_TitleGet_text.png__GUI__battleUI_05__battleUI_05_prefab",
        "slc_TitleGet_text.png__GUI__battleUI_05__battleUI_05_prefab"
      };
      string str = "slc_ClassStayed_text.png__GUI__battleUI_05__battleUI_05_prefab";
      int index = (int) condition;
      if (strArray.Length <= index || strArray[index] == "")
      {
        ((Component) this.slcClassCondition).gameObject.SetActive(false);
      }
      else
      {
        this.slcClassCondition.spriteName = !c.isLowestClass || condition != PvpClassKind.Condition.STAY ? strArray[index] : str;
        ((UIWidget) this.slcClassCondition).MakePixelPerfect();
      }
    }
  }

  private IEnumerator SetConditionEffect(PvpClassKind.Condition condition)
  {
    Future<GameObject> pF = Res.Prefabs.versus_result.Class_Change_Anim2.Load<GameObject>();
    IEnumerator e = pF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.NextGaugeAnimation = pF.Result.Clone(this.dirClassConditionParent);
    this.NextGaugeAnimation.SetActive(false);
    ((IEnumerable<PopupClassChange>) this.NextGaugeAnimation.GetComponents<PopupClassChange>()).ForEach<PopupClassChange>((Action<PopupClassChange>) (x => x.ChangeSprite(condition)));
  }

  private void SetRankGauge(PvpClassKind c)
  {
    int width = ((UIWidget) this.slcRankGaugeRed).width;
    int num = 10;
    ((UIWidget) this.slcRankGaugeBlue).width = (c.stay_point - 1) * width / num + 1;
    ((UIWidget) this.slcRankGaugeGreen).width = (c.up_point - 1) * width / num + 1;
    ((UIWidget) this.slcRankGaugeYellow).width = (c.title_point - 1) * width / num + 1;
    ((Component) this.slcRankGaugeBlue).gameObject.SetActive(c.stay_point > 0);
    ((Component) this.slcRankGaugeGreen).gameObject.SetActive(c.up_point - c.stay_point > 0);
    ((Component) this.slcRankGaugeYellow).gameObject.SetActive(c.title_point - c.up_point > 0);
    ((Component) this.slcRankGaugeRed).gameObject.SetActive(true);
  }

  private void SetArrow(int win)
  {
    int num = 10;
    int width = ((UIWidget) this.slcRankGaugeRed).width;
    win = this.arrowAnim ? win - 1 : win;
    this.slcRankGaugeNowParent.transform.localPosition = new Vector3(((Component) this.slcRankGaugeRed).transform.localPosition.x + (float) (Mathf.Clamp(win, 0, num) * width / num), this.slcRankGaugeNowParent.transform.localPosition.y);
  }

  public void StartAnimationArrow()
  {
    if (!this.arrowAnim)
      return;
    this.StartCoroutine(this.StartAnim());
  }

  private IEnumerator StartAnim()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Versus0268VictoryDetail versus0268VictoryDetail = this;
    UITweener tween;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      tween.PlayForward();
      Singleton<NGSoundManager>.GetInstance().playSE("SE_0552");
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    tween = ((Component) versus0268VictoryDetail.slcRankGaugeNow).GetComponent<UITweener>();
    // ISSUE: reference to a compiler-generated method
    EventDelegate.Set(tween.onFinished, new EventDelegate.Callback(versus0268VictoryDetail.\u003CStartAnim\u003Eb__28_0));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.2f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void NextAnimation()
  {
    if (!this.nextAnim)
      return;
    this.NextGaugeAnimation.SetActive(true);
    Singleton<NGSoundManager>.GetInstance().playSE("SE_0553");
  }
}
