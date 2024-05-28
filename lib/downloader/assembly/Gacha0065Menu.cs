// Decompiled with JetBrains decompiler
// Type: Gacha0065Menu
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
public class Gacha0065Menu : BackButtonMenuBase
{
  [SerializeField]
  private string gacha_name_;
  [SerializeField]
  private int play_num_;
  [SerializeField]
  private UILabel TxtBonus;
  [SerializeField]
  private UILabel TxtDescription;
  [SerializeField]
  private UILabel TxtBonusDescription;
  [SerializeField]
  private UILabel TxtManaNumber;
  [SerializeField]
  private UILabel TxtNumber;
  [SerializeField]
  private UILabel TxtPopuptitle;
  [SerializeField]
  private GameObject bonusObj;
  [SerializeField]
  private GameObject normalObj;
  [SerializeField]
  private UI2DSprite paymentIcon;
  [SerializeField]
  private UILabel shopLimitTime;
  [SerializeField]
  private GameObject shopLimitObj;
  [SerializeField]
  private UILabel commercialText;
  [SerializeField]
  private GameObject commercialObj;
  [SerializeField]
  private UISprite popupBaseSprite;
  private Gacha0063Scene scene;
  private bool is_new;

  public GachaModuleGacha gacha_data_ { get; set; }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public override void onBackButton() => this.IbtnNo();

  public void IbtnGacha()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.Shot());
  }

  public void Init(string name, GachaModuleGacha gacha_data, Gacha0063Scene scene = null)
  {
    this.gacha_name_ = name;
    this.gacha_data_ = gacha_data;
    this.play_num_ = this.gacha_data_.roll_count;
    this.scene = scene;
    if (Object.op_Inequality((Object) this.scene, (Object) null))
    {
      if (scene.gachaType == MasterDataTable.GachaType.retry)
        this.commercialText.SetTextLocalize(Consts.GetInstance().GACHA_0065MENU_COMMERCIAL2);
      else if (scene.gachaType == MasterDataTable.GachaType.normal)
        this.shopLimitObj.SetActive(false);
    }
    DateTime? endAt = gacha_data.end_at;
    if (endAt.HasValue)
      this.shopLimitTime.SetTextLocalize(string.Format(Consts.GetInstance().GACHA_0065MENU_COMMERCIAL_LIMIT, (object) endAt.Value.ToString("MM/dd HH:mm")));
    this.ChangeBonusMode(gacha_data.remain_count_for_reward, this.gacha_data_.payment_amount, gacha_data.payment_type_id, gacha_data.payment_id);
    if (gacha_data.payment_type_id == Gacha0063Menu.PaymentTypeIDCompensation)
    {
      this.TxtManaNumber.SetText(Consts.Format(Consts.GetInstance().GACHA_99931MENU_DESCRIPTION03_COMPENSATION));
      this.TxtNumber.SetText(Consts.Format(Consts.GetInstance().GACHA_0065MENU_DESCRIPTION03, (IDictionary) new Hashtable()
      {
        {
          (object) "nowcrystal",
          (object) SMManager.Get<Player>().paid_coin.ToString().ToConverter()
        }
      }));
      this.TxtPopuptitle.SetText(Consts.Format(Consts.GetInstance().GACHA_0065MENU_DESCRIPTION04_COMPENSATION));
      if (this.gacha_data_.payment_amount > 0)
        return;
      this.commercialObj.SetActive(false);
      ((UIWidget) this.popupBaseSprite).SetDimensions(((UIWidget) this.popupBaseSprite).width, 560);
    }
    else if (gacha_data.payment_type_id == 5)
    {
      if (gacha_data.payment_id.HasValue)
      {
        int id = gacha_data.payment_id.Value;
        PlayerGachaTicket playerGachaTicket = ((IEnumerable<PlayerGachaTicket>) SMManager.Get<Player>().gacha_tickets).Where<PlayerGachaTicket>((Func<PlayerGachaTicket, bool>) (x => x.ticket.ID == id)).FirstOrDefault<PlayerGachaTicket>();
        int quantity = playerGachaTicket == null ? 0 : playerGachaTicket.quantity;
        this.TxtManaNumber.SetText(Consts.Format(Consts.GetInstance().GACHA_0065MENU_DESCRIPTION02));
        this.TxtNumber.SetText(Consts.Format(Consts.GetInstance().GACHA_0065MENU_DESCRIPTION03, (IDictionary) new Hashtable()
        {
          {
            (object) "nowcrystal",
            (object) quantity.ToString().ToConverter()
          }
        }));
        this.TxtPopuptitle.SetText(Consts.Format(Consts.GetInstance().GACHA_0065MENU_DESCRIPTION04_GACHA_TICKET));
      }
      else
        Debug.LogError((object) "ガチャチケットのIDが取得できません");
      this.commercialObj.SetActive(false);
      ((UIWidget) this.popupBaseSprite).SetDimensions(((UIWidget) this.popupBaseSprite).width, 560);
      this.StartCoroutine(Gacha0063Kiseki.createPaymentIcon(this.paymentIcon, gacha_data.payment_type_id));
    }
    else
    {
      this.TxtManaNumber.SetText(Consts.Format(Consts.GetInstance().GACHA_0065MENU_DESCRIPTION02));
      this.TxtNumber.SetText(Consts.Format(Consts.GetInstance().GACHA_0065MENU_DESCRIPTION03, (IDictionary) new Hashtable()
      {
        {
          (object) "nowcrystal",
          (object) SMManager.Get<Player>().coin.ToString().ToConverter()
        }
      }));
      this.TxtPopuptitle.SetText(Consts.Format(Consts.GetInstance().GACHA_0065MENU_DESCRIPTION04));
      if (this.gacha_data_.payment_amount > 0)
        return;
      this.commercialObj.SetActive(false);
      ((UIWidget) this.popupBaseSprite).SetDimensions(((UIWidget) this.popupBaseSprite).width, 560);
    }
  }

  public IEnumerator Shot()
  {
    Singleton<PopupManager>.GetInstance().dismiss();
    Singleton<CommonRoot>.GetInstance().loadingMode = 4;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    this.scene.apiUpdate = this.IsGachaEX();
    GachaPlay gacha = GachaPlay.GetInstance();
    IEnumerator e = gacha.ChargeGacha(this.gacha_name_, this.play_num_, this.gacha_data_.id, this.scene.gachaType, this.gacha_data_.payment_amount);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!gacha.isError)
    {
      GachaResultData.GetInstance();
      Singleton<NGSceneManager>.GetInstance().changeScene("gacha006_effect", true);
    }
  }

  private bool IsGachaEX()
  {
    return !Object.op_Equality((Object) this.scene, (Object) null) && this.scene.gachaType != MasterDataTable.GachaType.normal && this.scene.gachaType != MasterDataTable.GachaType.friend && this.scene.gachaType != MasterDataTable.GachaType.ticket && this.scene.gachaType != MasterDataTable.GachaType.sheet;
  }

  private void ChangeBonusMode(
    int? bonusCountDown,
    int payment_amount,
    int payment_type_id,
    int? payment_id)
  {
    int? nullable;
    int num1;
    if (bonusCountDown.HasValue)
    {
      nullable = bonusCountDown;
      int num2 = 1;
      num1 = nullable.GetValueOrDefault() == num2 & nullable.HasValue ? 1 : 0;
    }
    else
      num1 = 0;
    bool flag1 = num1 != 0;
    int num3;
    if (bonusCountDown.HasValue)
    {
      nullable = bonusCountDown;
      int num4 = 0;
      num3 = nullable.GetValueOrDefault() > num4 & nullable.HasValue ? 1 : 0;
    }
    else
      num3 = 0;
    bool flag2 = num3 != 0;
    this.bonusObj.SetActive(flag2);
    this.normalObj.SetActive(!flag2);
    string text1 = string.Empty;
    if (payment_type_id == Gacha0063Menu.PaymentTypeIDCompensation)
      text1 = Consts.Format(Consts.GetInstance().GACHA_0065MENU_DESCRIPTION01_COMPENSATION, (IDictionary) new Hashtable()
      {
        {
          (object) "lostcrystal",
          (object) payment_amount.ToString().ToConverter()
        }
      });
    else if (payment_type_id == 5)
    {
      if (payment_id.HasValue)
      {
        GachaTicket gachaTicket = MasterData.GachaTicket[payment_id.Value];
        text1 = Consts.Format(Consts.GetInstance().GACHA_0065MENU_DESCRIPTION01_GACHA_TICKET, (IDictionary) new Hashtable()
        {
          {
            (object) "name",
            (object) gachaTicket.name
          },
          {
            (object) "lostticket",
            (object) payment_amount.ToString().ToConverter()
          }
        });
      }
      else
        Debug.LogError((object) "ガチャチケットのIDが取得できません");
    }
    else
      text1 = Consts.Format(Consts.GetInstance().GACHA_0065MENU_DESCRIPTION01, (IDictionary) new Hashtable()
      {
        {
          (object) "lostcrystal",
          (object) payment_amount.ToString().ToConverter()
        }
      });
    if (flag2)
    {
      string text2;
      if (flag1)
        text2 = Consts.Format(Consts.GetInstance().GACHA_0065MENU_DESCRIPTION06);
      else
        text2 = Consts.Format(Consts.GetInstance().GACHA_0065MENU_DESCRIPTION05, (IDictionary) new Hashtable()
        {
          {
            (object) nameof (bonusCountDown),
            (object) bonusCountDown.ToString().ToConverter()
          }
        });
      this.TxtBonus.SetText(text2);
      this.TxtBonusDescription.SetText(text1);
    }
    else
      this.TxtDescription.SetText(text1);
  }
}
