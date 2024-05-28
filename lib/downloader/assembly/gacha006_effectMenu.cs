// Decompiled with JetBrains decompiler
// Type: gacha006_effectMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class gacha006_effectMenu : BackButtonMenuBase
{
  public GameObject backButton;
  public GameObject skipButton;
  public bool isPreview;
  [SerializeField]
  private GameObject effectFade;
  private GachaResultData.Result[] resultList;
  private readonly int LOADING_DISPLAY_COUNT = 10;

  public EffectControllerGacha Effect { get; set; }

  public void IbtnSkip()
  {
    this.skipButton.SetActive(false);
    this.Effect.Skip();
  }

  public void IbtnBack() => this.Effect.Next();

  public override void onBackButton()
  {
    if (this.Effect.State != EffectControllerGacha.STATE.WAIT)
      ToastMessage.showBackKeyToast();
    this.IbtnBack();
  }

  public IEnumerator SetEffectData(GachaResultData.Result[] resultList)
  {
    this.resultList = resultList;
    ((Component) this.Effect).gameObject.SetActive(true);
    this.skipButton.SetActive(!this.isPreview);
    IEnumerator e = this.Effect.SetNeedData(resultList, this.backButton, this.isPreview);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void ShowResult()
  {
    Singleton<NGSoundManager>.GetInstance().StopBgm(time: 1f);
    Singleton<NGSoundManager>.GetInstance().StopSe(time: 1f);
    Singleton<NGSoundManager>.GetInstance().StopVoice(time: 2f);
    if (this.isPreview)
    {
      Singleton<NGSceneManager>.GetInstance().backScene();
      Singleton<PopupManager>.GetInstance().dismiss();
    }
    else
    {
      GachaResultData.ResultData data = GachaResultData.GetInstance().GetData();
      if (this.resultList.Length != 1 || data.is_retry || data.is_ticket && this.resultList.Length == 1 && this.resultList[0].reward_result_quantity > 1)
      {
        if (data.is_ticket && data.resultList.Length > this.LOADING_DISPLAY_COUNT)
        {
          Singleton<CommonRoot>.GetInstance().isLoading = false;
          Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(4, true);
        }
        Gacha00613Scene.ChangeScene(false, GachaResultData.GetInstance().GetData().is_retry);
      }
      else
      {
        GachaResultData.Result result = this.resultList[0];
        CommonRewardType commonRewardType = new CommonRewardType(result.reward_type_id, result.reward_result_id, result.reward_result_quantity, result.is_new, result.is_reserves);
        if (commonRewardType.isUnit)
        {
          PlayerUnit unit = commonRewardType.unit;
          Singleton<NGSceneManager>.GetInstance().changeScene("gacha006_8", false, (object) unit, (object) result.is_new);
        }
        else if (commonRewardType.isMaterialUnit)
          Unit00493Scene.changeScene(false, commonRewardType.materialUnit.unit, result.is_new, true);
        else if (commonRewardType.isGear)
        {
          PlayerItem gear = commonRewardType.gear;
          if (commonRewardType.gear.gear.kind.isEquip)
            Gacha00611Scene.changeScene(false, result.is_new, 0, new ItemInfo(gear), 0);
          else
            Bugu00561Scene.changeScene(false, new ItemInfo(gear), result.is_new, true);
        }
        else
          Bugu00561Scene.changeScene(false, new ItemInfo(commonRewardType.materialGear), result.is_new, true);
      }
    }
  }

  public void PlayEffectWhiteFadeIn()
  {
    if (Object.op_Equality((Object) this.effectFade, (Object) null))
      return;
    this.SetEffectWhiteFadeActive(true);
    ((UITweener) this.effectFade.GetComponent<TweenAlpha>()).PlayForward();
  }

  public void SetEffectWhiteFadeActive(bool active)
  {
    if (Object.op_Equality((Object) this.effectFade, (Object) null))
      return;
    this.effectFade.SetActive(active);
    ((UIRect) this.effectFade.GetComponent<UI2DSprite>()).alpha = 1f;
  }
}
