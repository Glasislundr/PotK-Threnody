// Decompiled with JetBrains decompiler
// Type: RouletteAwardResultPopupController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class RouletteAwardResultPopupController : MonoBehaviour
{
  [SerializeField]
  private GameObject awardIconContainer;
  [SerializeField]
  private UILabel awardDescription;
  private GameObject campaignPopupPrefab;
  private bool shouldShowCampaign;
  private string campaignURL;
  private bool isDisplayingCampaignPopup;
  private NGSoundManager soundManager;

  public IEnumerator Init(
    RouletteR001FreeDeckEntity resultDeckEntity,
    bool shouldShowCampaign,
    GameObject campaignPopupPrefab,
    string campaignURL)
  {
    this.soundManager = Singleton<NGSoundManager>.GetInstance();
    this.isDisplayingCampaignPopup = false;
    this.shouldShowCampaign = shouldShowCampaign;
    this.campaignPopupPrefab = campaignPopupPrefab;
    this.campaignURL = campaignURL;
    CreateIconObject target = this.awardIconContainer.GetOrAddComponent<CreateIconObject>();
    IEnumerator e = target.CreateThumbnail(resultDeckEntity.reward_type_id, resultDeckEntity.reward_id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    target.GetIcon().GetComponent<UniqueIcons>();
    this.awardDescription.SetTextLocalize(resultDeckEntity.reward_message);
  }

  public void OnTouchToNext()
  {
    if (this.shouldShowCampaign)
    {
      if (this.isDisplayingCampaignPopup)
        return;
      this.isDisplayingCampaignPopup = true;
      this.soundManager.playSE("SE_1002");
      this.ShowCampaignPopup();
    }
    else
    {
      this.soundManager.playSE("SE_1002");
      Singleton<PopupManager>.GetInstance().dismiss();
      Singleton<NGSceneManager>.GetInstance().backScene();
    }
  }

  private void ShowCampaignPopup()
  {
    Singleton<PopupManager>.GetInstance().open(this.campaignPopupPrefab).GetComponent<RouletteCampaignPopupController>().Init(this.campaignURL);
  }
}
