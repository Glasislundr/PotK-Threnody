// Decompiled with JetBrains decompiler
// Type: Gacha0063KisekiExtention
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using LocaleTimeZone;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Gacha0063KisekiExtention : MonoBehaviour
{
  [SerializeField]
  private UIButton detailButton;
  [SerializeField]
  [Tooltip("設定が省略出来る様に初期値はOFFにしておく")]
  private UIButton statusButton;
  [SerializeField]
  private List<Gacha0063NoCoinObject> NoCoinObject;
  [SerializeField]
  private UILabel stepNowText;
  [SerializeField]
  private UILabel stepMaxText;
  [SerializeField]
  private GameObject periodObject;
  [SerializeField]
  private UILabel periodValueText;
  [SerializeField]
  [Tooltip("設定が省略出来る様に初期値はOFFにしておく")]
  private GameObject selectButton;
  [SerializeField]
  [Tooltip("selectButton の状態表示(0:未選択/1:選択済み)")]
  private GameObject[] selectButtonStatuses;
  [Header("通常輝石ガチャ")]
  [SerializeField]
  private GameObject KisekiObj;
  [SerializeField]
  private GameObject KisekiNormalGacha;
  [SerializeField]
  private GachaButton KisekiNormalGachaButton;
  [SerializeField]
  private UILabel KisekiNormalAmountLabel;
  [SerializeField]
  private UILabel KisekiNormalGachaLabel;
  [SerializeField]
  private GameObject KisekiCompensationGacha;
  [SerializeField]
  private GachaButton KisekiCompensationGachaButton;
  [SerializeField]
  private UILabel KisekiCompensationAmountLabel;
  [SerializeField]
  private UILabel KisekiCompensationGachaLabel;
  [SerializeField]
  private GameObject KisekiCoinButton;
  [SerializeField]
  private UI2DSprite KisekiCoinIcon;
  [SerializeField]
  private GameObject KisekiNoticeObject;
  [SerializeField]
  private UILabel KisekiNoticeText;
  [Header("ステップアップガチャ")]
  [SerializeField]
  private GameObject StepObj;
  [SerializeField]
  private GameObject StepNormalGacha;
  [SerializeField]
  private GachaButton StepNormalGachaButton;
  [SerializeField]
  private UILabel StepNormalAmountLabel;
  [SerializeField]
  private UILabel StepNormalGachaLabel;
  [SerializeField]
  private GameObject StepCompensationGacha;
  [SerializeField]
  private GachaButton StepCompensationGachaButton;
  [SerializeField]
  private UILabel StepCompensationAmountLabel;
  [SerializeField]
  private UILabel StepCompensationGachaLabel;
  [SerializeField]
  private GameObject StepCoinButton;
  [SerializeField]
  private UI2DSprite StepCoinIcon;
  [SerializeField]
  private GameObject StepNoticeObject;
  [SerializeField]
  private UILabel StepNoticeText;
  [Header("シートガチャ")]
  [SerializeField]
  private GameObject SheetObj;
  [SerializeField]
  private GameObject SheetNormalGacha;
  [SerializeField]
  private GachaButton SheetNormalGachaButton;
  [SerializeField]
  private UILabel SheetNormalAmountLabel;
  [SerializeField]
  private UILabel SheetNormalGachaLabel;
  [SerializeField]
  private GameObject SheetCompensationGacha;
  [SerializeField]
  private GachaButton SheetCompensationGachaButton;
  [SerializeField]
  private UILabel SheetCompensationAmountLabel;
  [SerializeField]
  private UILabel SheetCompensationGachaLabel;
  [SerializeField]
  private GameObject SheetCoinButton;
  [SerializeField]
  private UI2DSprite SheetCoinIcon;
  [SerializeField]
  private List<UISprite> SheetIconList;
  [SerializeField]
  private GameObject SheetNoticeObject;
  [SerializeField]
  private UILabel SheetNoticeText;
  public Gacha0063Menu Menu;
  private bool isSheetDetail = true;
  private bool m_TimeOut;
  private GachaModule gachaModule;
  private const int DEADLINE_MIN = 15;
  private bool isAboveDay;

  private void Init()
  {
    this.KisekiObj.SetActive(false);
    this.StepObj.SetActive(false);
    this.SheetObj.SetActive(false);
    this.KisekiNoticeObject.SetActive(false);
    this.StepNoticeObject.SetActive(false);
    this.SheetNoticeObject.SetActive(false);
    this.m_TimeOut = false;
  }

  public void UpdateSheetInfo()
  {
    this.SheetObj.SetActive(true);
    if (this.gachaModule.gacha[0].payment_type_id == Gacha0063Menu.PaymentTypeIDCompensation)
    {
      this.SheetNormalGacha.SetActive(false);
      this.SheetCompensationGacha.SetActive(true);
      this.SheetCompensationGachaButton.Init(this.gachaModule.name, this.gachaModule.gacha[0], this.Menu, this.gachaModule.type, this.gachaModule.number, this.gachaModule);
      this.SheetCompensationGachaButton.ChangeButtonEvent(this.gachaModule);
      if (this.gachaModule.gacha[0].roll_count == 1)
        this.SheetCompensationGachaLabel.text = Consts.Format(Consts.GetInstance().GACHA_0063SINGLE_GACHA_PLAY);
      else
        this.SheetCompensationGachaLabel.text = Consts.Format(Consts.GetInstance().GACHA_0063MULTI_GACHA_PLAY, (IDictionary) new Hashtable()
        {
          {
            (object) "num",
            (object) this.gachaModule.gacha[0].roll_count.ToString()
          }
        });
    }
    else
    {
      this.SheetNormalGacha.SetActive(true);
      this.SheetCompensationGacha.SetActive(false);
      this.SheetNormalGachaButton.Init(this.gachaModule.name, this.gachaModule.gacha[0], this.Menu, this.gachaModule.type, this.gachaModule.number, this.gachaModule);
      this.SheetNormalGachaButton.ChangeButtonEvent(this.gachaModule);
      if (this.gachaModule.gacha[0].roll_count == 1)
        this.SheetNormalGachaLabel.text = Consts.Format(Consts.GetInstance().GACHA_0063SINGLE_GACHA_PLAY);
      else
        this.SheetNormalGachaLabel.text = Consts.Format(Consts.GetInstance().GACHA_0063MULTI_GACHA_PLAY, (IDictionary) new Hashtable()
        {
          {
            (object) "num",
            (object) this.gachaModule.gacha[0].roll_count.ToString()
          }
        });
    }
    GachaG007PlayerPanel[] array = ((IEnumerable<GachaG007PlayerPanel>) SMManager.Get<GachaG007PlayerSheet[]>()[0].player_panels).OrderBy<GachaG007PlayerPanel, int>((Func<GachaG007PlayerPanel, int>) (x => x.position)).ToArray<GachaG007PlayerPanel>();
    for (int index = 0; index < array.Length; ++index)
    {
      if (array[index].is_opened)
        this.ChangeSprite(this.SheetIconList[index], string.Format("slc_Sheet_On.png__GUI__006-3_sozai_new__006-3_sozai_new_prefab"));
      else
        this.ChangeSprite(this.SheetIconList[index], string.Format("slc_Sheet_Off.png__GUI__006-3_sozai_new__006-3_sozai_new_prefab"));
    }
    if (string.IsNullOrEmpty(this.gachaModule.gacha[this.gachaModule.gacha.Length - 1].button_text))
      return;
    this.SheetNoticeObject.SetActive(true);
    this.SheetNoticeText.text = this.gachaModule.gacha[this.gachaModule.gacha.Length - 1].button_text;
  }

  private void InitSheetGacha() => this.UpdateSheetInfo();

  private void SetStepUp(int? now, int? max)
  {
    if (!now.HasValue || !max.HasValue)
      return;
    this.StepObj.SetActive(true);
    if (this.gachaModule.gacha[0].payment_type_id == Gacha0063Menu.PaymentTypeIDCompensation)
    {
      this.StepNormalGacha.SetActive(false);
      this.StepCompensationGacha.SetActive(true);
      this.StepCompensationGachaButton.Init(this.gachaModule.name, this.gachaModule.gacha[0], this.Menu, this.gachaModule.type, this.gachaModule.number, this.gachaModule);
      this.StepCompensationGachaButton.ChangeButtonEvent(this.gachaModule);
    }
    else
    {
      this.StepNormalGacha.SetActive(true);
      this.StepCompensationGacha.SetActive(false);
      this.StepNormalGachaButton.Init(this.gachaModule.name, this.gachaModule.gacha[0], this.Menu, this.gachaModule.type, this.gachaModule.number, this.gachaModule);
      this.StepNormalGachaButton.ChangeButtonEvent(this.gachaModule);
    }
    if (!string.IsNullOrEmpty(this.gachaModule.gacha[this.gachaModule.gacha.Length - 1].button_text))
    {
      this.StepNoticeObject.SetActive(true);
      this.StepNoticeText.text = this.gachaModule.gacha[this.gachaModule.gacha.Length - 1].button_text;
    }
    this.stepNowText.text = now.ToString();
    this.stepMaxText.text = max.ToString();
  }

  private void DispGachaModeEx(int num)
  {
    if (this.gachaModule.gacha[0].payment_type_id == Gacha0063Menu.PaymentTypeIDCompensation)
    {
      this.KisekiNormalGacha.SetActive(false);
      this.KisekiCompensationGacha.SetActive(true);
      this.KisekiCompensationGachaButton.Init(this.gachaModule.name, this.gachaModule.gacha[0], this.Menu, this.gachaModule.type, this.gachaModule.number, this.gachaModule);
      this.KisekiCompensationGachaButton.ChangeButtonEvent(this.gachaModule);
      if (num == 1)
      {
        this.KisekiCompensationGachaLabel.text = Consts.Format(Consts.GetInstance().GACHA_0063SINGLE_GACHA_PLAY);
        this.StepCompensationGachaLabel.text = Consts.Format(Consts.GetInstance().GACHA_0063SINGLE_GACHA_PLAY);
      }
      else
      {
        this.KisekiCompensationGachaLabel.text = Consts.Format(Consts.GetInstance().GACHA_0063MULTI_GACHA_PLAY, (IDictionary) new Hashtable()
        {
          {
            (object) nameof (num),
            (object) num.ToString()
          }
        });
        this.StepCompensationGachaLabel.text = Consts.Format(Consts.GetInstance().GACHA_0063MULTI_GACHA_PLAY, (IDictionary) new Hashtable()
        {
          {
            (object) nameof (num),
            (object) this.gachaModule.gacha[0].roll_count.ToString()
          }
        });
      }
    }
    else
    {
      this.KisekiNormalGacha.SetActive(true);
      this.KisekiCompensationGacha.SetActive(false);
      this.KisekiNormalGachaButton.Init(this.gachaModule.name, this.gachaModule.gacha[0], this.Menu, this.gachaModule.type, this.gachaModule.number, this.gachaModule);
      this.KisekiNormalGachaButton.ChangeButtonEvent(this.gachaModule);
      if (num == 1)
      {
        this.StepNormalGachaLabel.text = Consts.Format(Consts.GetInstance().GACHA_0063SINGLE_GACHA_PLAY);
        this.KisekiNormalGachaLabel.text = Consts.Format(Consts.GetInstance().GACHA_0063SINGLE_GACHA_PLAY);
      }
      else
      {
        this.StepNormalGachaLabel.text = Consts.Format(Consts.GetInstance().GACHA_0063MULTI_GACHA_PLAY, (IDictionary) new Hashtable()
        {
          {
            (object) nameof (num),
            (object) this.gachaModule.gacha[0].roll_count.ToString()
          }
        });
        this.KisekiNormalGachaLabel.text = Consts.Format(Consts.GetInstance().GACHA_0063MULTI_GACHA_PLAY, (IDictionary) new Hashtable()
        {
          {
            (object) nameof (num),
            (object) num.ToString()
          }
        });
      }
    }
  }

  private void SetKisekiSprite(int amount)
  {
    switch (this.GetGachaType())
    {
      case 2:
        if (this.gachaModule.gacha[0].payment_type_id == Gacha0063Menu.PaymentTypeIDCompensation)
        {
          this.StepCompensationAmountLabel.text = amount.ToString();
          break;
        }
        this.StepNormalAmountLabel.text = amount.ToString();
        break;
      case 3:
        if (this.gachaModule.gacha[0].payment_type_id == Gacha0063Menu.PaymentTypeIDCompensation)
        {
          this.SheetCompensationAmountLabel.text = amount.ToString();
          break;
        }
        this.SheetNormalAmountLabel.text = amount.ToString();
        break;
      default:
        if (this.gachaModule.gacha[0].payment_type_id == Gacha0063Menu.PaymentTypeIDCompensation)
        {
          this.KisekiCompensationAmountLabel.text = amount.ToString();
          break;
        }
        this.KisekiNormalAmountLabel.text = amount.ToString();
        break;
    }
  }

  private void SetGacha(int num, int amount)
  {
    this.DispGachaModeEx(num);
    this.SetKisekiSprite(amount);
  }

  private void DispDetailButton(bool canDisp)
  {
    ((Component) this.detailButton).gameObject.SetActive(canDisp);
  }

  private void SetDetailButton(GachaModule module, GameObject detailPopup)
  {
    if (module.description.title == null)
      this.DispDetailButton(false);
    else
      EventDelegate.Add(this.detailButton.onClick, (EventDelegate.Callback) (() =>
      {
        if (this.Menu.IsPushAndSet())
          return;
        Singleton<CommonRoot>.GetInstance().loadingMode = 1;
        Singleton<CommonRoot>.GetInstance().isLoading = true;
        this.StartCoroutine(this.OpenDetailPopup(module, detailPopup));
      }));
  }

  private IEnumerator OpenDetailPopup(GachaModule module, GameObject detailPopup)
  {
    GachaDescription description = module.description;
    IEnumerator e = Singleton<PopupManager>.GetInstance().openAlert(detailPopup).GetComponent<Popup00631Menu>().InitGachaDetail(description.title, description.bodies);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
  }

  public IEnumerator InitDetail(GachaModule module, GameObject detailPopup, Gacha0063Menu menu)
  {
    bool canDisp = !string.IsNullOrEmpty(module.description.title);
    this.DispDetailButton(canDisp);
    if (canDisp)
    {
      this.Menu = menu;
      this.SetDetailButton(module, detailPopup);
      yield break;
    }
  }

  public void onClickedStatus()
  {
    if (Object.op_Implicit((Object) this.Menu) && this.Menu.IsPushAndSet())
      return;
    DateTime dateTime1 = TimeZoneInfo.ConvertTime(ServerTime.NowAppTimeAddDelta(), Japan.CreateTimeZone());
    GachaModuleGacha gmg = ((IEnumerable<GachaModuleGacha>) this.gachaModule.gacha).First<GachaModuleGacha>();
    Action callback = (Action) (() =>
    {
      Singleton<NGGameDataManager>.GetInstance().currentGachaNumber = this.gachaModule.number;
      Unit0042PickupScene.changeScene(true, this.gachaModule.name, gmg);
    });
    TimeSpan timeSpan1 = new TimeSpan(0, 15, 0);
    if (gmg.end_at.HasValue)
    {
      DateTime? endAt = gmg.end_at;
      DateTime dateTime2 = dateTime1;
      TimeSpan? nullable = endAt.HasValue ? new TimeSpan?(endAt.GetValueOrDefault() - dateTime2) : new TimeSpan?();
      TimeSpan timeSpan2 = timeSpan1;
      if ((nullable.HasValue ? (nullable.GetValueOrDefault() > timeSpan2 ? 1 : 0) : 0) == 0)
      {
        Consts instance = Consts.GetInstance();
        this.StartCoroutine(PopupCommon.Show(instance.GACHA_DEADLINE_TITLE, Consts.Format(instance.GACHA_DEADLINE_MESSAGE, (IDictionary) new Hashtable()
        {
          {
            (object) "DateTime",
            (object) gmg.end_at.Value.ToString("MM/dd HH:mm")
          }
        }), callback));
        return;
      }
    }
    callback();
  }

  private void SwitchTimeLimit(bool isAboveDay) => this.isAboveDay = isAboveDay;

  private void SetGachaLimitTime(GachaModule module)
  {
    DateTime? endAt = module.gacha[0].end_at;
    if (!endAt.HasValue)
      return;
    this.periodObject.SetActive(true);
    this.periodValueText.SetText(Consts.Format(Consts.GetInstance().GACHA_0063KISEKI_PERIOD) + endAt.Value.ToString("MM/dd HH:mm"));
  }

  public void SetOldGacha() => this.periodObject.SetActive(false);

  public void UpdateLimitTime(GachaModule module, DateTime serverTime)
  {
    TimeSpan? nullable1 = new TimeSpan?();
    DateTime? endAt = module.period.end_at;
    DateTime dateTime = serverTime;
    TimeSpan? nullable2 = endAt.HasValue ? new TimeSpan?(endAt.GetValueOrDefault() - dateTime) : new TimeSpan?();
    int days = nullable2.Value.Days;
    int hours = nullable2.Value.Hours;
    int minutes = nullable2.Value.Minutes;
    TimeSpan timeSpan = nullable2.Value;
    int seconds = timeSpan.Seconds;
    timeSpan = nullable2.Value;
    if (timeSpan.Milliseconds < 0 && !this.m_TimeOut)
    {
      this.m_TimeOut = true;
      this.UpdateGacha();
    }
    else if (days > 0)
    {
      this.periodValueText.SetText(Consts.Format(Consts.GetInstance().GACHA_0063KISEKI_PERIOD_DAY, (IDictionary) new Hashtable()
      {
        {
          (object) "day",
          (object) days
        }
      }));
    }
    else
    {
      if (this.isAboveDay)
        this.SwitchTimeLimit(false);
      this.periodValueText.SetText(Consts.Format(Consts.GetInstance().GACHA_0063KISEKI_PERIOD_TIME, (IDictionary) new Hashtable()
      {
        {
          (object) "hour",
          (object) hours.ToString("00")
        },
        {
          (object) "min",
          (object) minutes.ToString("00")
        },
        {
          (object) "sec",
          (object) seconds.ToString("00")
        }
      }));
    }
  }

  public void SetTimiLimit(GachaModule module)
  {
    this.periodObject.SetActive(true);
    if (!module.period.display_count_down || !module.period.end_at.HasValue)
      return;
    if (module.period.end_at.Value.Day > 0)
      this.SwitchTimeLimit(true);
    else
      this.SwitchTimeLimit(false);
  }

  public void SetKisekiEx(GachaModule module, Gacha0063Menu menu)
  {
    Gacha0063SheetModel gacha0063SheetModel = new Gacha0063SheetModel(module);
    this.gachaModule = module;
    this.Init();
    this.SetCoin(module.gacha[0].common_ticket_id);
    GachaModuleGacha gachaModuleGacha = module.gacha[0];
    this.SetStatusButton(gachaModuleGacha.is_gacha_pickup && !gachaModuleGacha.is_pickup_select);
    this.SetSelectButton(gachaModuleGacha.is_pickup_select, gachaModuleGacha.is_selected_pickup);
    this.Menu = menu;
    switch (this.GetGachaType())
    {
      case 2:
        this.SetStepUp(module.stepup.current_count, module.stepup.total_count);
        break;
      case 3:
        this.InitSheetGacha();
        break;
      default:
        this.KisekiObj.SetActive(true);
        if (!string.IsNullOrEmpty(this.gachaModule.gacha[this.gachaModule.gacha.Length - 1].button_text))
        {
          this.KisekiNoticeObject.SetActive(true);
          this.KisekiNoticeText.text = this.gachaModule.gacha[this.gachaModule.gacha.Length - 1].button_text;
          break;
        }
        break;
    }
    this.SetGacha(module.gacha[0].roll_count, module.gacha[0].payment_amount);
    this.SetGachaLimitTime(module);
  }

  private void UpdateGacha()
  {
    Singleton<NGSceneManager>.GetInstance().sceneBase.IsPush = true;
    Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
    Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitID = -1;
    Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitIndex = -1;
    Singleton<NGSceneManager>.GetInstance().clearStack();
    Singleton<NGSceneManager>.GetInstance().changeScene("gacha006_3", false);
  }

  private void ChangeSprite(UISprite target, string newName)
  {
    target.spriteName = newName;
    UISpriteData atlasSprite = target.GetAtlasSprite();
    if (atlasSprite == null)
    {
      Debug.LogWarning((object) "Atlas内のSprite取得失敗");
    }
    else
    {
      ((UIWidget) target).width = atlasSprite.width;
      ((UIWidget) target).height = atlasSprite.height;
    }
  }

  public void IbtnProgressSheet()
  {
    if (this.Menu.IsPushAndSet() || !this.isSheetDetail)
      return;
    ((Behaviour) this.Menu.scene.ScrollView).enabled = false;
    this.Menu.isSheetPopup = true;
    this.isSheetDetail = false;
    this.StartCoroutine(this.ShowProgressSheet());
  }

  private IEnumerator ShowProgressSheet()
  {
    Gacha0063KisekiExtention kisekiExtention = this;
    Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
    ((UIButtonColor) kisekiExtention.detailButton).isEnabled = false;
    GameObject popup = kisekiExtention.Menu.popupSheet.Clone();
    popup.SetActive(false);
    Popup0063SheetMenu script = popup.GetComponent<Popup0063SheetMenu>();
    GachaG007PlayerSheet[] gachaG007PlayerSheetArray = SMManager.Get<GachaG007PlayerSheet[]>();
    GachaG007PlayerPanel[] array = ((IEnumerable<GachaG007PlayerPanel>) gachaG007PlayerSheetArray[0].player_panels).OrderBy<GachaG007PlayerPanel, int>((Func<GachaG007PlayerPanel, int>) (x => x.position)).ToArray<GachaG007PlayerPanel>();
    IEnumerator e = script.Init(kisekiExtention, array, gachaG007PlayerSheetArray[0]);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    // ISSUE: reference to a compiler-generated method
    script.SetCallback(new Action(kisekiExtention.\u003CShowProgressSheet\u003Eb__77_1));
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
    popup.SetActive(true);
  }

  private void SetCoin(int? common_ticket_id)
  {
    int ticket_id = !common_ticket_id.HasValue ? 0 : common_ticket_id.Value;
    int gachaType = this.GetGachaType();
    if (ticket_id == 0)
    {
      this.KisekiCoinButton.SetActive(false);
      this.StepCoinButton.SetActive(false);
      this.SheetCoinButton.SetActive(false);
      foreach (Gacha0063NoCoinObject gacha0063NoCoinObject in this.NoCoinObject)
        ((Component) gacha0063NoCoinObject).transform.localPosition = gacha0063NoCoinObject.noCoinPosition;
    }
    else
    {
      this.StartCoroutine(this.SetCoinIcon(ticket_id));
      if (gachaType == 3)
        this.SheetCoinButton.SetActive(true);
      else if (gachaType == 2)
        this.StepCoinButton.SetActive(true);
      else
        this.KisekiCoinButton.SetActive(true);
    }
  }

  private IEnumerator SetCoinIcon(int ticket_id)
  {
    if (!MasterData.CommonTicket.ContainsKey(ticket_id))
    {
      Debug.LogError((object) "id:{0}のcommon_ticketが存在しません。".F((object) ticket_id));
    }
    else
    {
      Future<Sprite> future = MasterData.CommonTicket[ticket_id].LoadIconMSpriteF();
      IEnumerator e = future.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      switch (this.GetGachaType())
      {
        case 2:
          this.StepCoinIcon.sprite2D = future.Result;
          break;
        case 3:
          this.SheetCoinIcon.sprite2D = future.Result;
          break;
        default:
          this.KisekiCoinIcon.sprite2D = future.Result;
          break;
      }
    }
  }

  private int GetGachaType()
  {
    if (new Gacha0063SheetModel(this.gachaModule).IsSheetGachaOpen)
      return 3;
    return this.gachaModule.stepup.current_count.HasValue && this.gachaModule.stepup.total_count.HasValue ? 2 : 1;
  }

  private void SetStatusButton(bool bEnabled)
  {
    if (!Object.op_Implicit((Object) this.statusButton))
      return;
    ((Component) this.statusButton).gameObject.SetActive(bEnabled);
  }

  private void SetSelectButton(bool bEnabled, bool bSelected)
  {
    if (!Object.op_Implicit((Object) this.selectButton))
      return;
    this.selectButton.SetActive(bEnabled);
    ((IEnumerable<GameObject>) this.selectButtonStatuses).ToggleOnce(bSelected ? 1 : 0);
  }

  public void onClickedPickupSelector()
  {
    if (!Object.op_Implicit((Object) this.Menu) || this.Menu.IsPushAndSet() || !this.isActivePickupSelector)
      return;
    Singleton<NGGameDataManager>.GetInstance().currentGachaNumber = this.gachaModule.number;
    GachaPickupSelectScene.changeScene(true, this.gachaModule.gacha[0], (Action) (() => this.Menu.forceApiUpdate()));
  }

  private bool isActivePickupSelector
  {
    get
    {
      return this.gachaModule != null && this.gachaModule.gacha[0].is_pickup_select && this.gachaModule.gacha[0].pickup_select_count.HasValue;
    }
  }

  public enum GachaType
  {
    normal = 1,
    stepup = 2,
    sheet = 3,
  }
}
