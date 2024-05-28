// Decompiled with JetBrains decompiler
// Type: Gacha00613Scene
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
public class Gacha00613Scene : NGSceneBase
{
  public Gacha00613Menu Menu;
  private bool isError;

  public static void ChangeScene(bool isStack, bool is_retry = false)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("gacha006_13", (isStack ? 1 : 0) != 0, (object) is_retry);
  }

  public override IEnumerator onEndSceneAsync()
  {
    IEnumerator e = this.Menu.onEndSceneAsync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(bool is_retry = false)
  {
    Gacha00613Scene gacha00613Scene = this;
    RenderSettings.ambientLight = Singleton<NGGameDataManager>.GetInstance().baseAmbientLight;
    IEnumerator e = gacha00613Scene.SetBackGround();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GachaResultData ggrd = GachaResultData.GetInstance();
    if (ggrd.GetData() == null)
    {
      if (is_retry)
      {
        // ISSUE: reference to a compiler-generated method
        Future<WebAPI.Response.GachaResume> paramF = WebAPI.GachaResume(new Action<WebAPI.Response.UserError>(gacha00613Scene.\u003ConStartSceneAsync\u003Eb__4_0));
        e = paramF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (!gacha00613Scene.isError)
        {
          PlayerCommonTicket[] beforePlayerCommonTicket = SMManager.Get<PlayerCommonTicket[]>();
          WebAPI.Response.GachaResume result_list = paramF.Result;
          e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<PlayerUnit>) result_list.temp_player_units, false);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          GachaModuleGacha gachaModuleGacha = result_list.gacha_module.gacha[0];
          ggrd.SetData(result_list, result_list.gacha_module.name, gachaModuleGacha.id, gachaModuleGacha.roll_count, gachaModuleGacha.payment_amount, beforePlayerCommonTicket);
          beforePlayerCommonTicket = (PlayerCommonTicket[]) null;
          result_list = (WebAPI.Response.GachaResume) null;
        }
        paramF = (Future<WebAPI.Response.GachaResume>) null;
      }
      else
      {
        GachaPlay gacha = GachaPlay.GetInstance();
        e = gacha.FriendGacha("g002_friendpoint", 10, 1001);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        gacha00613Scene.isError = gacha.isError;
        gacha = (GachaPlay) null;
      }
    }
    if (gacha00613Scene.isError)
    {
      ((Component) gacha00613Scene.Menu).gameObject.SetActive(false);
    }
    else
    {
      gacha00613Scene.Menu.SetRetryBtnActive(false);
      e = gacha00613Scene.Menu.CreateGetListAsync(ggrd.GetData());
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    GachaResultData.ResultData data = ggrd.GetData();
    gacha00613Scene.Menu.UpdateButtonStatus(data);
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public void onStartScene(bool is_retry = false)
  {
    if (this.isError)
      return;
    Singleton<PopupManager>.GetInstance().closeAll();
    if (is_retry)
    {
      this.Menu.SetRetryBtnActive(!this.Menu.IsConfirmResult);
      this.Menu.BtnActionEnable(true);
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(this.Menu.isConfirmResult);
    }
    else
    {
      if (GachaResultData.GetInstance().IsPopupEffect())
      {
        this.Menu.BtnActionEnable(false);
        Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
      }
      else
        Singleton<CommonRoot>.GetInstance().SetFooterEnable(true);
      this.StartCoroutine(this.ResultEffect());
    }
  }

  public IEnumerator ResultEffect()
  {
    Gacha00613Scene m = this;
    IEnumerator e = m.Menu.OpenBonusIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (GachaResultData.GetInstance().IsPopupEffect())
    {
      m.Menu.BtnActionEnable(false);
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
      e = m.SheetGachaResult();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = m.CharacterStory();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = m.CoinAcquisition();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    m.Menu.BtnActionEnable(true);
    if (m.IsPickupResult() && m.IsAppReviewEnable() && SMManager.Observe<Player>().Value.level >= 150)
      PopupAppReview.Show((MonoBehaviour) m);
    Singleton<CommonRoot>.GetInstance().SetFooterEnable(Singleton<TutorialRoot>.GetInstance().IsTutorialFinish());
    if (!Singleton<TutorialRoot>.GetInstance().IsTutorialFinish())
    {
      Persist.newTutorialGacha.Data.tutorialGacha = true;
      Persist.newTutorialGacha.Flush();
    }
  }

  private bool IsPickupResult()
  {
    bool flag = false;
    foreach (GachaResultData.Result result in GachaResultData.GetInstance().GetData().GetResultData())
    {
      if (result.directionType == GachaDirectionType.pickup)
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  private bool IsAppReviewEnable()
  {
    return Singleton<NGGameDataManager>.GetInstance().isReviewPopupCurrentGacha;
  }

  public IEnumerator SheetGachaResult()
  {
    IEnumerator e = GachaResultData.GetInstance().SheetGachaResultPopup();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator CharacterStory()
  {
    IEnumerator e = GachaResultData.GetInstance().CharacterStoryPopup();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator SetBackGround()
  {
    Gacha00613Scene gacha00613Scene = this;
    Future<GameObject> fBG = Res.Prefabs.BackGround.GachaTopBackground.Load<GameObject>();
    IEnumerator e = fBG.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    gacha00613Scene.backgroundPrefab = fBG.Result;
    ((UIWidget) gacha00613Scene.backgroundPrefab.GetComponent<UI2DSprite>()).color = Color.white;
  }

  private IEnumerator CoinAcquisition()
  {
    IEnumerator e = GachaResultData.GetInstance().CoinAcquisitionPopup();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
