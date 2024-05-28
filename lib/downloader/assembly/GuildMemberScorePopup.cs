// Decompiled with JetBrains decompiler
// Type: GuildMemberScorePopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class GuildMemberScorePopup : GuildMemberScoreListBase
{
  private const int Width = 612;
  private const int Height = 160;
  private const int ColumnValue = 1;
  private const int RowValue = 11;
  private const int ScreenValue = 6;
  [SerializeField]
  private UILabel lblPopupTitle;
  [SerializeField]
  private GameObject slc_title_base_one;
  [SerializeField]
  private GameObject slc_title_base_enemy;

  private IEnumerator InitMemberScoreListScroll(GvgPlayerHistory[] scores)
  {
    GuildMemberScorePopup memberScorePopup = this;
    memberScorePopup.allScoreInfo.Clear();
    memberScorePopup.allScoreBar.Clear();
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    DateTime nowTime = ServerTime.NowAppTime();
    memberScorePopup.Initialize(nowTime, 612, 160, 11, 6);
    memberScorePopup.CreateMemberScoreInfo(scores);
    if (memberScorePopup.allScoreInfo.Count > 0)
    {
      Future<GameObject> prefabF = Res.Prefabs.guild.guild_member_battle_records_list.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject result = prefabF.Result;
      e = memberScorePopup.CreateScrollBase(result);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      prefabF = (Future<GameObject>) null;
    }
    memberScorePopup.scroll.ResolvePosition();
    memberScorePopup.scroll.scrollView.UpdatePosition();
    memberScorePopup.InitializeEnd();
  }

  protected override IEnumerator CreateScroll(int info_index, int bar_index)
  {
    GuildMemberScorePopup memberScorePopup = this;
    if (bar_index < memberScorePopup.allScoreBar.Count && info_index < memberScorePopup.allScoreInfo.Count)
    {
      GuildMemberScoreScroll scrollParts = memberScorePopup.allScoreBar[bar_index];
      GuildMemberScoreBarInfo memberScoreBarInfo = memberScorePopup.allScoreInfo[info_index];
      memberScoreBarInfo.scrollParts = scrollParts;
      IEnumerator e = scrollParts.InitializeAsync(memberScoreBarInfo.score);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ((Component) scrollParts).gameObject.SetActive(true);
    }
  }

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }

  public IEnumerator InitializeAsync(bool isEnemy, GvgPlayerHistory[] scores)
  {
    GuildMemberScorePopup memberScorePopup = this;
    if (Object.op_Inequality((Object) ((Component) memberScorePopup).GetComponent<UIWidget>(), (Object) null))
      ((UIRect) ((Component) memberScorePopup).GetComponent<UIWidget>()).alpha = 0.0f;
    memberScorePopup.slc_title_base_one.SetActive(!isEnemy);
    memberScorePopup.slc_title_base_enemy.SetActive(isEnemy);
    memberScorePopup.lblPopupTitle.SetTextLocalize(Consts.GetInstance().POPUP_GUILD_BATTLE_MEMBER_SCORE_TITLE);
    IEnumerator e = memberScorePopup.InitMemberScoreListScroll(scores);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void InitScrollPosition() => this.scroll.scrollView.ResetPosition();

  public override void onBackButton() => Singleton<PopupManager>.GetInstance().dismiss();
}
