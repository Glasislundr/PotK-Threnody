// Decompiled with JetBrains decompiler
// Type: Gacha0068Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Gacha0068Scene : NGSceneBase
{
  private Gacha0068Menu menu;
  private RenderTextureRecoveryUtil util;

  public PlayerUnit playerUnit { get; set; }

  public IEnumerator onStartSceneAsync()
  {
    this.playerUnit = (PlayerUnit) null;
    foreach (PlayerUnit playerUnit in SMManager.Get<PlayerUnit[]>())
      this.playerUnit = playerUnit;
    IEnumerator e = this.onStartSceneAsync(this.playerUnit, true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(PlayerUnit playerUnit, bool newFlag)
  {
    IEnumerator e = this.onStartSceneAsync(playerUnit, newFlag, true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(PlayerUnit playerUnit, bool newFlag, bool fixedBG)
  {
    Gacha0068Scene gacha0068Scene = this;
    RenderSettings.ambientLight = Singleton<NGGameDataManager>.GetInstance().baseAmbientLight;
    IEnumerator e = gacha0068Scene.SetBackGround(fixedBG);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Equality((Object) gacha0068Scene.menu, (Object) null))
    {
      Future<GameObject> handler = Res.Prefabs.gacha006_8.MainPanel.Load<GameObject>();
      e = handler.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      gacha0068Scene.menu = handler.Result.Clone(((Component) gacha0068Scene).transform).GetComponent<Gacha0068Menu>();
      ((UIRect) ((Component) gacha0068Scene.menu).GetComponent<UIPanel>()).SetAnchor(((Component) gacha0068Scene).transform);
      handler = (Future<GameObject>) null;
    }
    gacha0068Scene.menuBase = (NGMenuBase) gacha0068Scene.menu;
    gacha0068Scene.menuBase.IsPush = false;
    e = gacha0068Scene.menu.Set(playerUnit, newFlag);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene()
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    this.StartCoroutine(this.ResultEffect());
  }

  public void onStartScene(PlayerUnit playerUnit, bool newFlag)
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    this.StartCoroutine(this.ResultEffect());
    this.util = ((Component) this).GetComponent<RenderTextureRecoveryUtil>();
    if (!Object.op_Inequality((Object) this.util, (Object) null))
      return;
    this.util.SaveRenderTexture();
  }

  public void onStartScene(PlayerUnit playerUnit, bool newFlag, bool fixedBG)
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    this.StartCoroutine(this.ResultEffect());
  }

  public IEnumerator ResultEffect()
  {
    Gacha0068Scene m = this;
    if (GachaResultData.GetInstance().IsPopupEffect())
    {
      ((Component) m.menu.BackSceneButton).gameObject.SetActive(false);
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
      IEnumerator e = m.SheetGachaResult();
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
      ((Component) m.menu.BackSceneButton).gameObject.SetActive(true);
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(true);
    }
    m.menu.EnableBackScene = true;
    if (!GachaResultData.GetInstance().GetData().is_retry && m.IsPickupResult() && m.IsAppReviewEnable() && SMManager.Observe<Player>().Value.level >= 150)
      PopupAppReview.Show((MonoBehaviour) m);
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

  private IEnumerator CoinAcquisition()
  {
    IEnumerator e = GachaResultData.GetInstance().CoinAcquisitionPopup();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator SetBackGround(bool fixedBG)
  {
    Gacha0068Scene gacha0068Scene = this;
    Future<GameObject> fBG = fixedBG ? Res.Prefabs.BackGround.GachaTopBackground.Load<GameObject>() : Res.Prefabs.BackGround.UnitBackground.Load<GameObject>();
    IEnumerator e = fBG.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    gacha0068Scene.backgroundPrefab = fBG.Result;
    ((UIWidget) gacha0068Scene.backgroundPrefab.GetComponent<UI2DSprite>()).color = Consts.GetInstance().GACHA_RESULT_BACKGROUND_COLOR;
  }

  private void Update()
  {
    if (!Object.op_Inequality((Object) this.util, (Object) null))
      return;
    this.util.FixRenderTexture();
  }
}
