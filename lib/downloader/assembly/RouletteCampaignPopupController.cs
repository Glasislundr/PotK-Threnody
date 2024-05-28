// Decompiled with JetBrains decompiler
// Type: RouletteCampaignPopupController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using UnityEngine;

#nullable disable
public class RouletteCampaignPopupController : MonoBehaviour
{
  private string campaignURL;

  public void Init(string campaignURL) => this.campaignURL = campaignURL;

  public void OnTapApply() => Application.OpenURL(this.campaignURL);

  public void OnTapReturn()
  {
    ModalWindow.ShowYesNo("確認", Consts.GetInstance().ROULETTE_CLOSE_CAMPAIGN_POPUP_CONTENT, (Action) (() =>
    {
      Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();
      Singleton<NGSceneManager>.GetInstance().backScene();
    }), (Action) (() => ((Component) this).gameObject.SetActive(true)));
    ((Component) this).gameObject.SetActive(false);
  }
}
