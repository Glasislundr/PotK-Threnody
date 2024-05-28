// Decompiled with JetBrains decompiler
// Type: Setting0101Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Setting0101Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  protected UISlider sldBgm;
  [SerializeField]
  protected UISlider sldSe;
  [SerializeField]
  protected UISlider sldVoice;
  [SerializeField]
  public GameObject[] btnHpRecoverOn;
  [SerializeField]
  public GameObject[] btnHpRecoverOff;
  [SerializeField]
  private BoxCollider bgmCollider;
  [SerializeField]
  private BoxCollider seCollider;
  [SerializeField]
  private BoxCollider voiceCollider;
  [SerializeField]
  public GameObject btnRecordOpen;
  [SerializeField]
  public GameObject btnRecordPrivate;
  [SerializeField]
  public SpreadColorButton btnHighGraphic;
  [SerializeField]
  public SpreadColorButton btnSpeedPriority;
  [SerializeField]
  private UIButton btnNormalSound;
  [SerializeField]
  private UIButton btnHighSound;
  private bool initFlg;
  private string btnRecordOpenBaseTexName;
  private string btnRecordPrivateBaseTexName;
  private string btnRecordOffTexName;
  private bool isRecordOpen;
  private bool isRecordOpenBase;
  private string btnSpeedPriorityONSpriteName = "ibtn_pink_234_70_idle.png__GUI__010-1_sozai__010-1_sozai_prefab";
  private string btnSpeedPriorityOFFSpriteName = "ibtn_black_234_70_idle.png__GUI__010-1_sozai__010-1_sozai_prefab";

  public float bgmVolume => ((UIProgressBar) this.sldBgm).value;

  public float seVolume => ((UIProgressBar) this.sldSe).value;

  public float voiceVolume => ((UIProgressBar) this.sldVoice).value;

  public void Initialize()
  {
    ((UIProgressBar) this.sldBgm).value = Persist.volume.Data.Bgm;
    ((UIProgressBar) this.sldSe).value = Persist.volume.Data.Se;
    ((UIProgressBar) this.sldVoice).value = Persist.volume.Data.Voice;
    this.btnRecordOpenBaseTexName = this.btnRecordOpen.GetComponent<UIButton>().normalSprite;
    this.btnRecordPrivateBaseTexName = this.btnRecordPrivate.GetComponent<UIButton>().normalSprite;
    this.btnRecordOffTexName = this.btnRecordPrivate.GetComponent<UIButton>().disabledSprite;
    this.isRecordOpen = SMManager.Get<DisplayPvPHistory>().display;
    this.isRecordOpenBase = this.isRecordOpen;
    if (this.isRecordOpen)
      this.IbtnRecordOpen();
    else
      this.IbtnRecordPrivate();
    this.initFlg = true;
    if (Persist.normalDLC.Data.IsSound)
    {
      this.btnNormalSound.normalSprite = this.btnRecordOpenBaseTexName;
      this.btnHighSound.normalSprite = this.btnRecordOffTexName;
    }
    else
    {
      this.btnNormalSound.normalSprite = this.btnRecordOffTexName;
      this.btnHighSound.normalSprite = this.btnRecordOpenBaseTexName;
    }
    if (!Persist.speedPriority.Data.IsSpeedPrioritySetup)
      return;
    bool isSpeedPriority = Persist.speedPriority.Data.IsSpeedPriority;
    ((Component) this.btnHighGraphic).GetComponent<UIButton>().normalSprite = isSpeedPriority ? this.btnSpeedPriorityOFFSpriteName : this.btnSpeedPriorityONSpriteName;
    ((Component) this.btnSpeedPriority).GetComponent<UIButton>().normalSprite = isSpeedPriority ? this.btnSpeedPriorityONSpriteName : this.btnSpeedPriorityOFFSpriteName;
  }

  public IEnumerator onEndSceneComm()
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Future<WebAPI.Response.PlayerDisplayPvpHistoryEdit> result = WebAPI.PlayerDisplayPvpHistoryEdit(this.isRecordOpen, (Action<WebAPI.Response.UserError>) (e =>
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      WebAPI.DefaultUserErrorCallback(e);
    }));
    IEnumerator e1 = result.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (result.Result != null)
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    }
  }

  public IEnumerator onEndSceneAsync()
  {
    Setting0101Menu setting0101Menu = this;
    if (setting0101Menu.isRecordOpen != setting0101Menu.isRecordOpenBase)
      yield return (object) setting0101Menu.StartCoroutine(setting0101Menu.onEndSceneComm());
  }

  public override void onBackButton() => this.IbtnBack();

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    ((Collider) this.bgmCollider).enabled = false;
    ((Collider) this.seCollider).enabled = false;
    ((Collider) this.voiceCollider).enabled = false;
    Singleton<NGSceneManager>.GetInstance().backScene();
  }

  public virtual void SldBgmValueChange()
  {
    if (!this.initFlg)
      return;
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    if (!Object.op_Inequality((Object) instance, (Object) null))
      return;
    instance.getVolume().bgmVolume = ((UIProgressBar) this.sldBgm).value;
    instance.UpdateVolumeLevel();
  }

  public virtual void SldSeValueChange()
  {
    if (!this.initFlg)
      return;
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    if (!Object.op_Inequality((Object) instance, (Object) null))
      return;
    instance.getVolume().seVolume = ((UIProgressBar) this.sldSe).value;
    instance.UpdateVolumeLevel();
  }

  public virtual void SldVoiceValueChange()
  {
    if (!this.initFlg)
      return;
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    if (!Object.op_Inequality((Object) instance, (Object) null))
      return;
    instance.getVolume().voiceVolume = ((UIProgressBar) this.sldVoice).value;
    instance.UpdateVolumeLevel();
  }

  public virtual void IbtnNotificationChange()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("setting010_2", true);
  }

  public virtual void IbtnNameChange()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("setting010_1_3", true);
  }

  public virtual void IbtnCommentChange()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("setting010_1_4", true);
  }

  public virtual void IbtnRecordOpen()
  {
    if (this.IsPush)
      return;
    this.btnRecordOpen.GetComponent<UIButton>().normalSprite = this.btnRecordOpenBaseTexName;
    this.btnRecordPrivate.GetComponent<UIButton>().normalSprite = this.btnRecordOffTexName;
    this.isRecordOpen = true;
  }

  public virtual void IbtnRecordPrivate()
  {
    if (this.IsPush)
      return;
    this.btnRecordOpen.GetComponent<UIButton>().normalSprite = this.btnRecordOffTexName;
    this.btnRecordPrivate.GetComponent<UIButton>().normalSprite = this.btnRecordPrivateBaseTexName;
    this.isRecordOpen = false;
  }

  public void IbtnOnNormalSound()
  {
    if (this.IsPush || Persist.normalDLC.Data.IsSound)
      return;
    this.StartCoroutine(this.ShowSoundQualityConfirmPopup(true));
  }

  public void IbtnOnHighSound()
  {
    if (this.IsPush || !Persist.normalDLC.Data.IsSound)
      return;
    this.StartCoroutine(this.ShowSoundQualityConfirmPopup(false));
  }

  public void IbtnHighGraphic()
  {
    ((Component) this.btnHighGraphic).GetComponent<UIButton>().normalSprite = this.btnSpeedPriorityONSpriteName;
    ((Component) this.btnSpeedPriority).GetComponent<UIButton>().normalSprite = this.btnSpeedPriorityOFFSpriteName;
    Singleton<NGGameDataManager>.GetInstance().SetSpeedPriorityMode(false);
  }

  public void IbtnSpeedPriority()
  {
    ((Component) this.btnHighGraphic).GetComponent<UIButton>().normalSprite = this.btnSpeedPriorityOFFSpriteName;
    ((Component) this.btnSpeedPriority).GetComponent<UIButton>().normalSprite = this.btnSpeedPriorityONSpriteName;
    Singleton<NGGameDataManager>.GetInstance().SetSpeedPriorityMode(true);
  }

  private IEnumerator ShowSoundQualityConfirmPopup(bool isNormal)
  {
    Future<GameObject> f = Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/popup/popup_010_AnimQualityChangeConfirm__anim_popup01");
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(f.Result).GetComponent<SoundQualityConfirmPopup>().IsNormal = isNormal;
  }
}
