// Decompiled with JetBrains decompiler
// Type: GachaButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class GachaButton : MonoBehaviour
{
  public MasterDataTable.GachaType gachaType;
  public int gachaNumber;
  public Gacha0063Menu menu_;
  public string gacha_name_;
  public int max_play_num_;
  public Startup00010ScoreDraw use_point_;
  public Startup00010ScoreDraw play_num_;
  public Transform slc_GachaPTNeed;
  public Transform dir_GachaPointUse;
  public Transform san_slc_GachaPTNeedPosition;
  public Transform san_dir_GachaPointUsePosition;
  public Transform yon_slc_GachaPTNeedPosition;
  public Transform yon_dir_GachaPointUsePosition;
  [SerializeField]
  private SpreadColorButton btn;

  public GachaModuleGacha gacha_data_ { get; set; }

  public GachaModule gacha_module_data_ { get; set; }

  public void Init(
    string name,
    GachaModuleGacha data,
    Gacha0063Menu menu,
    int gachaType,
    int gachaNumber,
    GachaModule gachaModule)
  {
    this.gacha_name_ = name;
    this.gacha_data_ = data;
    this.menu_ = menu;
    this.gachaType = (MasterDataTable.GachaType) gachaType;
    this.gachaNumber = gachaNumber;
    if (this.gacha_data_.payment_amount > 999)
    {
      if (Object.op_Implicit((Object) this.slc_GachaPTNeed))
        this.slc_GachaPTNeed.localPosition = this.yon_slc_GachaPTNeedPosition.localPosition;
      if (Object.op_Implicit((Object) this.dir_GachaPointUse))
        this.dir_GachaPointUse.localPosition = this.yon_dir_GachaPointUsePosition.localPosition;
    }
    else
    {
      if (Object.op_Implicit((Object) this.slc_GachaPTNeed))
        this.slc_GachaPTNeed.localPosition = this.san_slc_GachaPTNeedPosition.localPosition;
      if (Object.op_Implicit((Object) this.dir_GachaPointUse))
        this.dir_GachaPointUse.localPosition = this.san_dir_GachaPointUsePosition.localPosition;
    }
    if (!Object.op_Implicit((Object) this.use_point_))
      return;
    this.use_point_.Draw(this.gacha_data_.payment_amount);
  }

  public void SetMaxPlayNum(int max_play_nam)
  {
    this.max_play_num_ = max_play_nam;
    if (max_play_nam * this.gacha_data_.payment_amount > 999)
    {
      if (Object.op_Implicit((Object) this.slc_GachaPTNeed))
        this.slc_GachaPTNeed.localPosition = this.yon_slc_GachaPTNeedPosition.localPosition;
      if (Object.op_Implicit((Object) this.dir_GachaPointUse))
        this.dir_GachaPointUse.localPosition = this.yon_dir_GachaPointUsePosition.localPosition;
    }
    else
    {
      if (Object.op_Implicit((Object) this.slc_GachaPTNeed))
        this.slc_GachaPTNeed.localPosition = this.san_slc_GachaPTNeedPosition.localPosition;
      if (Object.op_Implicit((Object) this.dir_GachaPointUse))
        this.dir_GachaPointUse.localPosition = this.san_dir_GachaPointUsePosition.localPosition;
    }
    if (Object.op_Implicit((Object) this.use_point_))
      this.use_point_.Draw(max_play_nam * this.gacha_data_.payment_amount);
    if (!Object.op_Implicit((Object) this.play_num_))
      return;
    this.play_num_.Draw(max_play_nam);
  }

  public void IbtnGachaCharge()
  {
    if (!this.menu_.CheckGachaCharge(this.gacha_data_, this.gachaNumber) || this.menu_.IsPushAndSet())
      return;
    this.menu_.scene.gachaType = this.gachaType;
    Singleton<NGGameDataManager>.GetInstance().currentGachaNumber = this.gachaNumber;
    Singleton<PopupManager>.GetInstance().open(this.menu_.gachaChargePrefab).GetComponent<Gacha0065Menu>().Init(this.gacha_name_, this.gacha_data_, this.menu_.scene);
  }

  public void IbtnGachaChargeMulti()
  {
    if (!this.menu_.CheckGachaCharge(this.gacha_data_, this.gachaNumber) || this.menu_.IsPushAndSet())
      return;
    this.menu_.scene.gachaType = this.gachaType;
    Singleton<NGGameDataManager>.GetInstance().currentGachaNumber = this.gachaNumber;
    Singleton<PopupManager>.GetInstance().open(this.menu_.gachaChargePrefab).GetComponent<Gacha0065Menu>().Init(this.gacha_name_, this.gacha_data_, this.menu_.scene);
  }

  public void IbtnGachaPt()
  {
    if (!this.menu_.CheckGachaPt(this.gacha_data_) || this.menu_.IsPushAndSet())
      return;
    this.menu_.scene.gachaType = this.gachaType;
    Singleton<NGGameDataManager>.GetInstance().currentGachaNumber = this.gachaNumber;
    this.StartCoroutine(this.Play(gacha_data: this.gacha_data_));
  }

  public void IbtnGachasPt()
  {
    if (!this.menu_.CheckGachaPt(this.gacha_data_) || this.menu_.IsPushAndSet())
      return;
    this.menu_.scene.gachaType = this.gachaType;
    Singleton<NGGameDataManager>.GetInstance().currentGachaNumber = this.gachaNumber;
    this.StartCoroutine(this.Play(this.max_play_num_, this.gacha_data_));
  }

  public void IbtnGachaTicket()
  {
    if (!this.menu_.CheckGachaTicket(this.gacha_data_) || this.menu_.IsPushAndSet())
      return;
    this.menu_.scene.gachaType = MasterDataTable.GachaType.ticket;
    Singleton<NGGameDataManager>.GetInstance().currentGachaNumber = this.gachaNumber;
    this.InitPlayGeneralGachaTicket(this.gacha_data_, this.menu_);
  }

  private void InitPlayGeneralGachaTicket(GachaModuleGacha gachaData, Gacha0063Menu menu)
  {
    Popup006SliderSelectMenu menuPopup = Singleton<PopupManager>.GetInstance().open(menu.gachaTicketSliderSelectPopupPrefab).GetComponent<Popup006SliderSelectMenu>();
    menuPopup.Init(gachaData, (Action) (() => this.StartCoroutine(this.PlayTicket(menuPopup.currentPlayTime, gachaData, menu.gachaTicketSliderSelectPopupPrefab))));
  }

  public void IbtnSheetGachaCharge()
  {
    if (!this.menu_.CheckGachaCharge(this.gacha_data_, this.gachaNumber) || this.menu_.IsPushAndSet())
      return;
    this.menu_.scene.gachaType = MasterDataTable.GachaType.sheet;
    Singleton<NGGameDataManager>.GetInstance().currentGachaNumber = this.gachaNumber;
    Singleton<PopupManager>.GetInstance().open(this.menu_.gachaChargePrefab).GetComponent<Gacha0065Menu>().Init(this.gacha_name_, this.gacha_data_, this.menu_.scene);
  }

  public void IbtnSheetGachaChargeMulti()
  {
    if (!this.menu_.CheckGachaCharge(this.gacha_data_, this.gachaNumber) || this.menu_.IsPushAndSet())
      return;
    this.menu_.scene.gachaType = MasterDataTable.GachaType.sheet;
    Singleton<NGGameDataManager>.GetInstance().currentGachaNumber = this.gachaNumber;
    Singleton<PopupManager>.GetInstance().open(this.menu_.gachaChargePrefab).GetComponent<Gacha0065Menu>().Init(this.gacha_name_, this.gacha_data_, this.menu_.scene);
  }

  public IEnumerator Play(int num = 1, GachaModuleGacha gacha_data = null)
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 4;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    GachaPlay gacha = GachaPlay.GetInstance();
    IEnumerator e = gacha.FriendGacha(this.gacha_name_, num, gacha_data.id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!gacha.isError)
      Singleton<NGSceneManager>.GetInstance().changeScene("gacha006_effect", true);
  }

  public IEnumerator PlayTicket(int num, GachaModuleGacha gacha_data, GameObject popupPrefab)
  {
    Singleton<PopupManager>.GetInstance().dismiss();
    Singleton<CommonRoot>.GetInstance().loadingMode = 4;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    GachaPlay gacha = GachaPlay.GetInstance();
    IEnumerator e = gacha.TicketGacha(this.gacha_name_, num, gacha_data, popupPrefab);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!gacha.isError)
      Singleton<NGSceneManager>.GetInstance().changeScene("gacha006_effect", true);
  }

  public void ChangeButtonEvent(GachaModule gachaModule)
  {
    UIButton component = ((Component) this).gameObject.GetComponent<UIButton>();
    if (Object.op_Equality((Object) component, (Object) null))
      Debug.LogWarning((object) "UIButtonが外れている");
    else if (gachaModule.type == 6)
    {
      if (gachaModule.gacha[0].roll_count == 1)
        EventDelegate.Set(component.onClick, new EventDelegate.Callback(this.IbtnGachaCharge));
      else
        EventDelegate.Set(component.onClick, new EventDelegate.Callback(this.IbtnGachaChargeMulti));
    }
    else if (gachaModule.gacha[0].roll_count == 1)
      EventDelegate.Set(component.onClick, new EventDelegate.Callback(this.IbtnGachaCharge));
    else
      EventDelegate.Set(component.onClick, new EventDelegate.Callback(this.IbtnGachaChargeMulti));
  }

  public void SetGachaButton(int payment_amount, int quantity)
  {
    if (payment_amount <= quantity)
      return;
    ((UIButtonColor) ((Component) this).gameObject.GetComponent<UIButton>()).isEnabled = false;
  }
}
