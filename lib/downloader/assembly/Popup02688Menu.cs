// Decompiled with JetBrains decompiler
// Type: Popup02688Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Popup02688Menu : Popup02610Base
{
  [SerializeField]
  private UILabel txtTitle;
  [SerializeField]
  private UILabel txtClassName;
  [SerializeField]
  private UILabel txtDraw;
  [SerializeField]
  private UILabel txtWin;
  [SerializeField]
  private UILabel txtLose;
  [SerializeField]
  private UILabel txtConfirmCheck;
  [SerializeField]
  private UILabel txtConfirmCheck2;
  [SerializeField]
  private UILabel txtBattleCount;
  [SerializeField]
  private UILabel txtRemaining;
  [SerializeField]
  private UISprite slcRankGaugeBlue;
  [SerializeField]
  private UISprite slcRankGaugeGreen;
  [SerializeField]
  private UISprite slcRankGaugeYellow;
  [SerializeField]
  private UISprite slcRankGaugeRed;
  [SerializeField]
  private UISprite slcRankGaugeNow;
  [SerializeField]
  private Transform dirParent;
  [SerializeField]
  private UIButton ExpendButton;
  [SerializeField]
  private UIButton ContinueButton;

  public override IEnumerator InitCoroutine(WebAPI.Response.PvpBoot pvpInfo, Versus02610Menu menu)
  {
    IEnumerator e = base.InitCoroutine(pvpInfo, menu);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.SetRankGauge();
    PvpClassKind c = MasterData.PvpClassKind[pvpInfo.current_class];
    Consts instance = Consts.GetInstance();
    this.txtTitle.SetText(instance.VERSUS_002688POPUP_TITLE);
    this.txtClassName.SetText(c.name);
    this.txtWin.SetText(pvpInfo.pvp_class_record.current_season_win_count.ToLocalizeNumberText());
    this.txtLose.SetText(pvpInfo.pvp_class_record.current_season_loss_count.ToLocalizeNumberText());
    this.txtDraw.SetText(pvpInfo.pvp_class_record.current_season_draw_count.ToLocalizeNumberText());
    this.txtConfirmCheck.SetText(Consts.GetInstance().VERSUS_002688POPUP_CONFIRMCHECK);
    this.txtBattleCount.SetText(pvpInfo.season_remaining_matches.ToLocalizeNumberText());
    this.txtRemaining.SetText(Consts.Format(instance.VERSUS_002688POPUP_REMAINING, (IDictionary) new Hashtable()
    {
      {
        (object) "cnt",
        (object) pvpInfo.remaining_addition_matches.ToLocalizeNumberText()
      },
      {
        (object) "max",
        (object) pvpInfo.max_addition_matches.ToLocalizeNumberText()
      }
    }));
    this.isActiveLeftButton(c);
    e = this.CreateClassChange();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private void isActiveLeftButton(PvpClassKind c)
  {
    int remainingMatches = this.pvpInfo.season_remaining_matches;
    if (remainingMatches > 0)
    {
      ((Component) this.ContinueButton).gameObject.SetActive(true);
      ((Component) this.ExpendButton).gameObject.SetActive(false);
      this.isActiveLeftButton(c, remainingMatches, this.ContinueButton);
      this.txtConfirmCheck2.SetText(((UIButtonColor) this.ContinueButton).isEnabled ? Consts.GetInstance().VERSUS_002688POPUP_CONFIRMCHECK2 : Consts.GetInstance().VERSUS_002688POPUP_CONFIRMCHECK3);
    }
    else
    {
      ((Component) this.ContinueButton).gameObject.SetActive(false);
      ((Component) this.ExpendButton).gameObject.SetActive(true);
      this.isActiveLeftButton(c, remainingMatches, this.ExpendButton);
      this.txtConfirmCheck2.SetText(Consts.GetInstance().VERSUS_002688POPUP_CONFIRMCHECK1);
    }
  }

  private void isActiveLeftButton(PvpClassKind c, int remaing_match, UIButton LeftButton)
  {
    bool flag = false;
    int currentSeasonWinCount = this.pvpInfo.pvp_class_record.current_season_win_count;
    int num = currentSeasonWinCount + this.pvpInfo.remaining_addition_matches + remaing_match;
    PvpClassKind.Condition condition = c.ClassCondition(currentSeasonWinCount);
    if (condition != PvpClassKind.Condition.TITLE_TOPCLASS && condition != PvpClassKind.Condition.TITLE)
    {
      if (condition == PvpClassKind.Condition.UP && num >= c.title_point)
        flag = true;
      else if ((condition == PvpClassKind.Condition.STAY_TOPCLASS || condition == PvpClassKind.Condition.STAY) && num >= c.up_point)
        flag = true;
      else if (condition == PvpClassKind.Condition.DOWN && num >= c.stay_point)
        flag = true;
    }
    ((UIButtonColor) LeftButton).isEnabled = flag;
  }

  public void IbtnConfirm()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGGameDataManager>.GetInstance().StartCoroutine(this.SeasonClose());
  }

  public void IbtnExtend()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGGameDataManager>.GetInstance().StartCoroutine(this.ExtendPopup());
  }

  public void IbtnContinue()
  {
    if (this.IsPushAndSet())
      return;
    Versus02610Menu.IsContinue = true;
    this.menu.isEnableSeasonFinishButton(false);
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  private IEnumerator ExtendPopup()
  {
    Popup02688Menu popup02688Menu = this;
    Singleton<PopupManager>.GetInstance().onDismiss();
    Future<GameObject> pF = Res.Prefabs.popup.popup_026_8_7__anim_popup01.Load<GameObject>();
    IEnumerator e = pF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject p = pF.Result.Clone();
    e = p.GetComponent<Popup02687Menu>().InitCoroutine(popup02688Menu.pvpInfo, popup02688Menu.menu);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject gameObject = Singleton<PopupManager>.GetInstance().open(p, isCloned: true);
    gameObject.SetActive(false);
    gameObject.SetActive(true);
  }

  private IEnumerator SeasonClose()
  {
    Popup02688Menu popup02688Menu = this;
    Singleton<PopupManager>.GetInstance().onDismiss();
    Future<WebAPI.Response.PvpSeasonClose> f = WebAPI.PvpSeasonClose((Action<WebAPI.Response.UserError>) (e =>
    {
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e1 = f.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (f.Result != null)
    {
      Future<GameObject> pF = Res.Prefabs.popup.popup_026_8_9__anim_popup01.Load<GameObject>();
      e1 = pF.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      e1 = pF.Result.Clone(popup02688Menu.menu.GetEffectParent).GetComponent<Popup02689Menu>().Init(popup02688Menu.pvpInfo, f.Result, popup02688Menu.menu);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      popup02688Menu.menu.SetPlayingEffect(true);
    }
  }

  public override void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();

  private void SetRankGauge()
  {
    int width = ((UIWidget) this.slcRankGaugeRed).width;
    int num = 10;
    PvpClassKind pvpClassKind = MasterData.PvpClassKind[this.pvpInfo.current_class];
    ((UIWidget) this.slcRankGaugeBlue).width = (pvpClassKind.stay_point - 1) * width / num;
    ((UIWidget) this.slcRankGaugeGreen).width = (pvpClassKind.up_point - 1) * width / num;
    ((UIWidget) this.slcRankGaugeYellow).width = (pvpClassKind.title_point - 1) * width / num;
    ((Component) this.slcRankGaugeBlue).gameObject.SetActive(pvpClassKind.stay_point > 0);
    ((Component) this.slcRankGaugeGreen).gameObject.SetActive(pvpClassKind.up_point - pvpClassKind.stay_point > 0);
    ((Component) this.slcRankGaugeYellow).gameObject.SetActive(pvpClassKind.title_point - pvpClassKind.up_point > 0);
    ((Component) this.slcRankGaugeRed).gameObject.SetActive(true);
    ((Component) this.slcRankGaugeNow).transform.localPosition = new Vector3(((Component) this.slcRankGaugeRed).transform.localPosition.x + (float) (Mathf.Clamp(this.pvpInfo.pvp_class_record.current_season_win_count, 0, num) * width / num), ((Component) this.slcRankGaugeNow).transform.localPosition.y);
  }

  private IEnumerator CreateClassChange()
  {
    Popup02688Menu popup02688Menu = this;
    Future<GameObject> pF = Res.Prefabs.popup.popup_Multi_Class_Change.Load<GameObject>();
    IEnumerator e = pF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    PopupClassChange component = pF.Result.Clone(popup02688Menu.dirParent).GetComponent<PopupClassChange>();
    PvpClassKind pvpClassKind = MasterData.PvpClassKind[popup02688Menu.pvpInfo.current_class];
    int c = (int) pvpClassKind.ClassCondition(popup02688Menu.pvpInfo.pvp_class_record.current_season_win_count, true);
    int num = pvpClassKind.isLowestClass ? 1 : 0;
    component.ChangeSprite((PvpClassKind.Condition) c, num != 0);
  }
}
