// Decompiled with JetBrains decompiler
// Type: Gacha0063TicketItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Gacha0063TicketItem : MonoBehaviour
{
  [SerializeField]
  private UILabel mTitleLabel;
  [SerializeField]
  private UI2DSprite mThumSprite;
  [SerializeField]
  private UIButton mPullObject;
  [SerializeField]
  private GameObject mPullDisableObject;
  [SerializeField]
  private UIButton mDetailButton;
  [SerializeField]
  private UILabel mLimitDateLabel;
  [SerializeField]
  private UILabel mNonLimitDateLabel;
  [SerializeField]
  private UILabel mTicketNeed;
  [SerializeField]
  private UILabel mTicketProgressLabel;
  [SerializeField]
  private UILabel mTicketProgressLabelRed;
  [SerializeField]
  private UILabel mFixCount;
  [SerializeField]
  private UILabel mFixReward;
  [SerializeField]
  private GameObject mTicketCountRayout;
  [SerializeField]
  private int mFixRerardEmptyRayoutY = 64;
  [SerializeField]
  private GachaButton gachaButton;
  [SerializeField]
  private GameObject gachaButtonLabelNormal;
  [SerializeField]
  private GameObject gachaButtonLabel100Ream;
  private Gacha0063Menu mMenu;
  private GachaModuleGacha targetGacha;
  private static readonly List<string> NumberSpriteNameList = new List<string>()
  {
    "slc_gacha_text_0.png__GUI__006-3_sozai__006-3_sozai_prefab.png",
    "slc_gacha_text_1.png__GUI__006-3_sozai__006-3_sozai_prefab.png",
    "slc_gacha_text_2.png__GUI__006-3_sozai__006-3_sozai_prefab.png",
    "slc_gacha_text_3.png__GUI__006-3_sozai__006-3_sozai_prefab.png",
    "slc_gacha_text_4.png__GUI__006-3_sozai__006-3_sozai_prefab.png",
    "slc_gacha_text_5.png__GUI__006-3_sozai__006-3_sozai_prefab.png",
    "slc_gacha_text_6.png__GUI__006-3_sozai__006-3_sozai_prefab.png",
    "slc_gacha_text_7.png__GUI__006-3_sozai__006-3_sozai_prefab.png",
    "slc_gacha_text_8.png__GUI__006-3_sozai__006-3_sozai_prefab.png",
    "slc_gacha_text_9.png__GUI__006-3_sozai__006-3_sozai_prefab.png"
  };
  private static readonly string DateTimeFormat = "yyyy/M/d H:mm";

  public void Init(GachaModule gachaModule, GachaModuleGacha gacha, Gacha0063Menu menu)
  {
    if (!gacha.payment_id.HasValue)
      return;
    this.targetGacha = gacha;
    GachaTicket gachaTicket = MasterData.GachaTicket[gacha.payment_id.Value];
    this.gachaButtonLabelNormal.SetActive(!gacha.is_one_hundred_ream);
    this.gachaButtonLabel100Ream.SetActive(gacha.is_one_hundred_ream);
    this.mTitleLabel.SetTextLocalize(gachaTicket.name);
    PlayerGachaTicket playerGachaTicket = ((IEnumerable<PlayerGachaTicket>) SMManager.Get<Player>().gacha_tickets).FirstOrDefault<PlayerGachaTicket>((Func<PlayerGachaTicket, bool>) (x => x.ticket_id == gacha.payment_id.Value));
    this.SetAmountCount(gacha.payment_amount, playerGachaTicket != null ? playerGachaTicket.quantity : 0);
    this.SetLimitDate(gacha.end_at);
    bool flag = false;
    foreach (FixedEntity fixedEntity in SMManager.Get<FixedEntity[]>())
    {
      if (fixedEntity.gacha_id == gacha.id)
      {
        if (fixedEntity.single_fix_count > gacha.count)
        {
          this.mFixCount.SetTextLocalize(fixedEntity.single_fix_count - gacha.count);
          this.mFixReward.SetTextLocalize(fixedEntity.fix_reward);
          flag = true;
          break;
        }
        break;
      }
    }
    if (!flag)
    {
      this.mTicketCountRayout.transform.localPosition = new Vector3(this.mTicketCountRayout.transform.localPosition.x, (float) this.mFixRerardEmptyRayoutY, this.mTicketCountRayout.transform.localPosition.z);
      this.mTicketCountRayout.SetActive(false);
    }
    ((UIButtonColor) this.mDetailButton).isEnabled = false;
    this.gachaButton.Init(gachaModule.name, gacha, menu, 4, gachaModule.number, gachaModule);
    if (playerGachaTicket.quantity >= gacha.payment_amount)
    {
      ((UIButtonColor) this.mPullObject).isEnabled = true;
      this.mPullDisableObject.SetActive(false);
    }
    else
    {
      ((UIButtonColor) this.mPullObject).isEnabled = false;
      this.mPullDisableObject.SetActive(true);
    }
    this.mMenu = menu;
  }

  public IEnumerator LoadResource(GameObject detailPopup)
  {
    if (this.targetGacha != null && this.targetGacha.payment_id.HasValue)
    {
      Future<Sprite> thumF = MasterData.GachaTicket[this.targetGacha.payment_id.Value].LoadSpriteOrDefault();
      IEnumerator e = thumF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.mThumSprite.sprite2D = thumF.Result;
      if (this.targetGacha.details == null || string.IsNullOrEmpty(this.targetGacha.details.title))
      {
        ((UIButtonColor) this.mDetailButton).isEnabled = false;
      }
      else
      {
        ((UIButtonColor) this.mDetailButton).isEnabled = true;
        List<Texture2D> images = new List<Texture2D>();
        for (int i = 0; i < this.targetGacha.details.bodies.Length; ++i)
        {
          GachaDescriptionBodies body = this.targetGacha.details.bodies[i];
          Texture2D image = (Texture2D) null;
          if (!string.IsNullOrEmpty(body.image_url))
          {
            e = Singleton<NGGameDataManager>.GetInstance().LoadWebImage(body.image_url);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            if (Singleton<NGGameDataManager>.GetInstance().webImageCache.ContainsKey(body.image_url))
              image = Singleton<NGGameDataManager>.GetInstance().webImageCache[body.image_url];
          }
          images.Add(image);
          body = (GachaDescriptionBodies) null;
          image = (Texture2D) null;
        }
        this.SetDetailButton(this.targetGacha, detailPopup);
        images = (List<Texture2D>) null;
      }
    }
  }

  private void SetAmountCount(int amountCount, int haveCount)
  {
    this.mTicketNeed.SetTextLocalize(amountCount);
    if (haveCount >= amountCount)
    {
      ((Component) this.mTicketProgressLabel).gameObject.SetActive(true);
      ((Component) this.mTicketProgressLabelRed).gameObject.SetActive(false);
      this.mTicketProgressLabel.SetTextLocalize(haveCount);
    }
    else
    {
      ((Component) this.mTicketProgressLabel).gameObject.SetActive(false);
      ((Component) this.mTicketProgressLabelRed).gameObject.SetActive(true);
      this.mTicketProgressLabelRed.SetTextLocalize(haveCount);
    }
  }

  private void SetNumberSprite(int number, UISprite sprite)
  {
    sprite.spriteName = Gacha0063TicketItem.NumberSpriteNameList[number];
  }

  private void SetLimitDate(DateTime? date)
  {
    if (date.HasValue)
    {
      ((Component) this.mNonLimitDateLabel).gameObject.SetActive(false);
      ((Component) this.mLimitDateLabel).gameObject.SetActive(true);
      this.mLimitDateLabel.SetTextLocalize(date.Value.ToString(Gacha0063TicketItem.DateTimeFormat));
    }
    else
    {
      ((Component) this.mNonLimitDateLabel).gameObject.SetActive(true);
      ((Component) this.mLimitDateLabel).gameObject.SetActive(false);
    }
  }

  private void SetDetailButton(GachaModuleGacha gacha, GameObject detailPopup)
  {
    EventDelegate.Add(this.mDetailButton.onClick, (EventDelegate.Callback) (() =>
    {
      if (this.mMenu.IsPushAndSet())
        return;
      Singleton<CommonRoot>.GetInstance().loadingMode = 1;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      this.StartCoroutine(this.OpenDetailPopup(gacha, detailPopup));
    }));
  }

  private IEnumerator OpenDetailPopup(GachaModuleGacha gacha, GameObject detailPopup)
  {
    Gacha0063TicketItem gacha0063TicketItem = this;
    GachaDescription details = gacha.details;
    GameObject gameObject = Singleton<PopupManager>.GetInstance().openAlert(detailPopup);
    if (Object.op_Equality((Object) gameObject, (Object) null))
    {
      Debug.LogError((object) ("Can't get popup object from PopupManager: " + ((Object) ((Component) gacha0063TicketItem).gameObject).name));
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    }
    else
    {
      Popup00631Menu component = gameObject.GetComponent<Popup00631Menu>();
      if (Object.op_Equality((Object) component, (Object) null))
      {
        Debug.LogError((object) ("Can't get menu object from popup object: " + ((Object) ((Component) gacha0063TicketItem).gameObject).name));
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      }
      else
      {
        IEnumerator e = component.InitGachaDetail(details.title, details.bodies);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      }
    }
  }
}
