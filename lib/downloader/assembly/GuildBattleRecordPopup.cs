// Decompiled with JetBrains decompiler
// Type: GuildBattleRecordPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class GuildBattleRecordPopup : GuildBattleRecordListBase
{
  private const int Width = 618;
  private const int Height = 169;
  private const int ColumnValue = 1;
  private const int RowValue = 12;
  private const int ScreenValue = 6;
  private bool isEnemy;
  [SerializeField]
  private UILabel lblPopupTitle;
  [SerializeField]
  private GameObject slc_title_base_one;
  [SerializeField]
  private GameObject slc_title_base_enemy;
  private GameObject memberScorePopup;

  private IEnumerator InitRecordListScroll(GvgHistory[] records)
  {
    GuildBattleRecordPopup battleRecordPopup = this;
    battleRecordPopup.allRecordInfo.Clear();
    battleRecordPopup.allRecordBar.Clear();
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    DateTime nowTime = ServerTime.NowAppTime();
    battleRecordPopup.Initialize(nowTime, 618, 169, 12, 6);
    battleRecordPopup.CreateRecordInfo(records);
    if (battleRecordPopup.allRecordInfo.Count > 0)
    {
      Future<GameObject> prefabF = Res.Prefabs.guild.guild_battle_records_list.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject result = prefabF.Result;
      e = battleRecordPopup.CreateScrollBase(result);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      prefabF = (Future<GameObject>) null;
    }
    battleRecordPopup.scroll.ResolvePosition();
    battleRecordPopup.scroll.scrollView.UpdatePosition();
    battleRecordPopup.InitializeEnd();
  }

  private void SetLabel()
  {
    this.lblPopupTitle.SetTextLocalize(Consts.GetInstance().POPUP_GUILD_BATTLE_RECORD_TITLE);
  }

  protected override IEnumerator CreateScroll(int info_index, int bar_index)
  {
    GuildBattleRecordPopup battleRecordPopup = this;
    if (bar_index < battleRecordPopup.allRecordBar.Count && info_index < battleRecordPopup.allRecordInfo.Count)
    {
      GuildBattleRecordScroll scrollParts = battleRecordPopup.allRecordBar[bar_index];
      GuildBattleRecordBarInfo battleRecordBarInfo = battleRecordPopup.allRecordInfo[info_index];
      battleRecordBarInfo.scrollParts = scrollParts;
      IEnumerator e = scrollParts.InitializeAsync(battleRecordPopup.isEnemy, battleRecordBarInfo.record, battleRecordPopup.memberScorePopup);
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

  public IEnumerator InitializeAsync(
    bool isEnemy,
    string guild_id,
    GameObject memberScorePopup,
    Action success = null)
  {
    GuildBattleRecordPopup battleRecordPopup = this;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    battleRecordPopup.isEnemy = isEnemy;
    bool maintenance = false;
    Future<WebAPI.Response.GvgHistoryHistoryGet> ft = WebAPI.GvgHistoryHistoryGet(true, guild_id, (Action<WebAPI.Response.UserError>) (e =>
    {
      if (e.Code.Equals("GLD014"))
        maintenance = true;
      WebAPI.DefaultUserErrorCallback(e);
    }));
    IEnumerator e1 = ft.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (maintenance)
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      yield return (object) null;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      MypageScene.ChangeSceneOnError();
    }
    else if (ft.Result != null)
    {
      if (Object.op_Inequality((Object) ((Component) battleRecordPopup).GetComponent<UIWidget>(), (Object) null))
        ((UIRect) ((Component) battleRecordPopup).GetComponent<UIWidget>()).alpha = 0.0f;
      battleRecordPopup.memberScorePopup = memberScorePopup;
      battleRecordPopup.slc_title_base_one.SetActive(!isEnemy);
      battleRecordPopup.slc_title_base_enemy.SetActive(isEnemy);
      battleRecordPopup.SetLabel();
      GvgHistory[] records = SMManager.Get<GvgHistory[]>();
      if (records != null)
      {
        e1 = battleRecordPopup.InitRecordListScroll(records);
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        if (success != null)
          success();
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      }
    }
  }

  public void InitScrollPosition() => this.scroll.scrollView.ResetPosition();

  public override void onBackButton()
  {
    if (Singleton<NGSceneManager>.GetInstance().sceneName.Equals("guild028_2") && Object.op_Inequality((Object) Singleton<NGSceneManager>.GetInstance().sceneBase, (Object) null) && Object.op_Inequality((Object) ((Component) Singleton<NGSceneManager>.GetInstance().sceneBase).GetComponent<Guild0282Menu>(), (Object) null))
      ((Component) Singleton<NGSceneManager>.GetInstance().sceneBase).GetComponent<Guild0282Menu>().isClosePopupByBackBtn = true;
    Singleton<PopupManager>.GetInstance().dismiss();
  }
}
