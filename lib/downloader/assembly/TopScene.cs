// Decompiled with JetBrains decompiler
// Type: TopScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using AppSetup;
using DeviceKit;
using GameCore;
using Gsc;
using gu3.Device;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

#nullable disable
public class TopScene : MonoBehaviour
{
  private const string toNextScene = "startup000_6";
  private const string filterSortKey = "FilterSortDataReset";
  [SerializeField]
  private GameObject mainMenu;
  [SerializeField]
  private GameObject eventMenu;
  [SerializeField]
  private string eventDateStart;
  [SerializeField]
  private string eventDateEnd;
  [SerializeField]
  private UIRoot uiRoot;
  [SerializeField]
  private GameObject popupPanel;
  private GameObject menu;
  public GameObject CodeError;
  public GameObject CodeErrorFgGID;
  public GameObject InputBlock;
  public GameObject SameTerminal;
  public GameObject Unknown;
  public GameObject Success;
  private GameObject BackCollider;
  private TermsOfService termsOfService;
  [SerializeField]
  private UniWebViewController m_webview;
  [SerializeField]
  private FGGIDConnectInitializer m_FggIdConnect;
  public Startup0008TopPageMenu userPolicy;
  public Startup00016Menu privacy;
  private bool isInitalizedPrivacyPolicy;
  public Startup00017Menu data_load;
  public Startup00018Menu data_load_fggid;
  public Transfer01281Menu data_load_warning;
  public Transfer01282Menu data_load_select;
  private UILabel txtApplicationVersion;
  private UILabel txtScreenSize;
  [SerializeField]
  private string defaultBgmName;
  [SerializeField]
  private string eventBgmName;
  private GameObject PGSSignInButton;
  public SetupFPSController popupSelectFPS;
  public SetupSoundController popupSelectSound;
  public PopupConfirm popupConfirmFPSAndSound;
  public SetupSpeedPriorityController popupSpeedPriority;
  public GameObject popupAccountManagement;
  public GameObject popupReroll;
  public GameObject popupReroll2;
  public GameObject popupRerollSuccess;
  public GameObject popupAccountDelete;
  public GameObject popupAccountDelete2;
  public GameObject popupAccountDelete3;
  public GameObject popupAccountDeleteSuccess;
  public GameObject AccountDeleteCheckBoxOn;
  public GameObject AccountDeleteCheckBoxOff;
  public GameObject LoadingObj;
  public SpreadColorButton popup008Back;
  public SpreadColorButton popup008Next;
  public UIButton popup016Close;
  public UIButton popup0178OK;
  public SpreadColorButton popup018Decide;
  public SpreadColorButton popup018Back;
  public SpreadColorButton popup01281Back;
  public SpreadColorButton popup01281Next;
  public SpreadColorButton popup01282Back;
  public SpreadColorButton popup01282Fggid;
  public SpreadColorButton popup01282Code;
  public UIButton popup01288OK;
  public UIButton popup01289OK;
  public SpreadColorButton popup019Decide;
  public SpreadColorButton popup019Back;
  public UIButton popup012811OK;
  public UIButton popup012812OK;
  public UIButton popup012813OK;
  public UIButton popupAccountBack;
  public UIButton popupAccountDataLoadBtn;
  public UIButton popupRerollBtn;
  public UIButton popupRerollNext;
  public UIButton popupRerollBack;
  public UIButton popupReroll2Next;
  public UIButton popupReroll2Back;
  public UIButton popupRerollSuccessOk;
  public UIButton popupAccountDeleteBtn;
  public UIButton popupAccountDeleteNext;
  public UIButton popupAccountDeleteBack;
  public UIButton popupAccountDelete2Next;
  public UIButton popupAccountDelete2Back;
  public UIButton popupAccountDelete3Next;
  public UIButton popupAccountDelete3Back;
  public UIButton popupAccountDeleteSuccessOk;
  public NGMessageUI ngMessageUi;
  private TopBackGroundAnimation backGroundAnim;
  private TopPressBtnAnimation btnAnim;
  private UILabel txtUserID;
  private UILabel txtVersion;
  private GameObject nowPopup;
  private bool enablePressStartBtn;
  private bool isIntoTouch = true;
  public TopScene.PopupCondition pCond;
  [SerializeField]
  private CriWareInitializer criWareInitializer;
  [SerializeField]
  private GameObject debugContainer;

  public bool EnablePressStartBtn
  {
    set => this.enablePressStartBtn = value;
    get => this.enablePressStartBtn;
  }

  private void Awake()
  {
    Singleton<NGSoundManager>.GetInstance().IsTitleScene = true;
    GameObject self = this.isEventPeriod(this.eventDateStart, this.eventDateEnd) ? Object.Instantiate<GameObject>(this.eventMenu) : Object.Instantiate<GameObject>(this.mainMenu);
    self.SetParent(((Component) this.uiRoot).gameObject);
    ((UIRect) self.GetComponent<UIPanel>()).SetAnchor(((Component) this.uiRoot).gameObject, 59, 0, -59, 0);
    if (IOSUtil.IsDeviceGenerationiPhoneX)
      SafeAreaBandRoot.HideSafeAreaBand();
    StartupMenuObject component = self.GetComponent<StartupMenuObject>();
    this.menu = component.menu;
    this.BackCollider = component.BackCollider;
    this.txtApplicationVersion = component.txtApplicationVersion;
    this.txtScreenSize = component.txtScreenSize;
    this.PGSSignInButton = component.PGSSignInButton;
    this.backGroundAnim = component.backGroundAnim;
    this.btnAnim = component.btnAnim;
    this.txtUserID = component.txtUserID;
    this.txtVersion = component.txtVersion;
    this.m_webview.m_followerObjects = new GameObject[1]
    {
      component.dirFggMission
    };
    EventDelegate.Set(component.ibtnFggMissionConnected.onClick, (EventDelegate.Callback) (() =>
    {
      this.openWebview();
      if (!IOSUtil.IsDeviceGenerationiPhoneX)
        return;
      SafeAreaBandRoot.ShowSafeAreaBand();
    }));
    EventDelegate.Set(component.ibtnFggMissionDisconnected.onClick, (EventDelegate.Callback) (() =>
    {
      this.openWebview();
      if (IOSUtil.IsDeviceGenerationiPhoneX)
        SafeAreaBandRoot.ShowSafeAreaBand();
      StatusBarHelper.SetVisibilityForIPhoneX(true);
    }));
    EventDelegate.Set(component.iconGoogle.onClick, (EventDelegate.Callback) (() => this.SignInPGS()));
    EventDelegate.Set(component.fggMissionClose.onClick, (EventDelegate.Callback) (() => ((Component) this.m_webview).GetComponent<FGGIDConnectInitializer>().Initialize()));
    EventDelegate.Set(component.ibtnSight1.onClick, (EventDelegate.Callback) (() => this.AccountManagementOn()));
    EventDelegate.Set(component.ibtnSight2.onClick, (EventDelegate.Callback) (() => this.PrivacyOn()));
    EventDelegate.Set(component.ibtnSight3.onClick, (EventDelegate.Callback) (() => this.DeleteCacheOn()));
    EventDelegate.Set(this.popup008Back.onClick, (EventDelegate.Callback) (() =>
    {
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1004");
      this.UserPolicyDissent();
    }));
    EventDelegate.Set(this.popup008Next.onClick, (EventDelegate.Callback) (() => this.UserPolicyAgree()));
    EventDelegate.Set(this.popup016Close.onClick, (EventDelegate.Callback) (() =>
    {
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1004");
      this.PrivacyOff();
    }));
    EventDelegate.Set(this.popup0178OK.onClick, (EventDelegate.Callback) (() =>
    {
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
      this.SuccessOff();
    }));
    EventDelegate.Set(this.popup018Decide.onClick, (EventDelegate.Callback) (() => this.IbtnPopupFgGIDDecide()));
    EventDelegate.Set(this.popup018Back.onClick, (EventDelegate.Callback) (() =>
    {
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1004");
      this.DataLoadFgGIDOff();
    }));
    EventDelegate.Set(this.popup01281Back.onClick, (EventDelegate.Callback) (() =>
    {
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1004");
      this.DataLoadWarningOff();
    }));
    EventDelegate.Set(this.popup01281Next.onClick, (EventDelegate.Callback) (() => this.IbtnPopupNext()));
    EventDelegate.Set(this.popup01282Back.onClick, (EventDelegate.Callback) (() =>
    {
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1004");
      this.DataLoadSelectOff();
    }));
    EventDelegate.Set(this.popup01282Fggid.onClick, (EventDelegate.Callback) (() => this.IbtnPopupFgGIDMigrate()));
    EventDelegate.Set(this.popup01282Code.onClick, (EventDelegate.Callback) (() => this.IbtnPopupMigrate()));
    EventDelegate.Set(this.popup01288OK.onClick, (EventDelegate.Callback) (() => this.CodeErrorOff()));
    EventDelegate.Set(this.popup01289OK.onClick, (EventDelegate.Callback) (() => this.CodeErrorFgGIDOff()));
    EventDelegate.Set(this.popup019Decide.onClick, (EventDelegate.Callback) (() => this.IbtnPopupDecide()));
    EventDelegate.Set(this.popup019Back.onClick, (EventDelegate.Callback) (() =>
    {
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1004");
      this.DataLoadOff();
    }));
    EventDelegate.Set(this.popup012811OK.onClick, (EventDelegate.Callback) (() => this.InputBlockOff()));
    EventDelegate.Set(this.popup012812OK.onClick, (EventDelegate.Callback) (() => this.SameTerminalOff()));
    EventDelegate.Set(this.popup012813OK.onClick, (EventDelegate.Callback) (() => this.UnknownOff()));
    EventDelegate.Set(this.popupAccountBack.onClick, (EventDelegate.Callback) (() =>
    {
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1004");
      this.IbtnAccountManagementBack();
    }));
    EventDelegate.Set(this.popupAccountDataLoadBtn.onClick, (EventDelegate.Callback) (() =>
    {
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
      this.IbtnDataLoadOn();
    }));
    EventDelegate.Set(this.popupRerollBtn.onClick, (EventDelegate.Callback) (() => this.IbtnDataReroll()));
    EventDelegate.Set(this.popupRerollNext.onClick, (EventDelegate.Callback) (() => this.IbtnRerollNext()));
    EventDelegate.Set(this.popupRerollBack.onClick, (EventDelegate.Callback) (() =>
    {
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1004");
      this.IbtnRerollBack();
    }));
    EventDelegate.Set(this.popupReroll2Next.onClick, (EventDelegate.Callback) (() => this.IbtnReroll2Next()));
    EventDelegate.Set(this.popupReroll2Back.onClick, (EventDelegate.Callback) (() =>
    {
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1004");
      this.IbtnRerollBack();
    }));
    EventDelegate.Set(this.popupRerollSuccessOk.onClick, (EventDelegate.Callback) (() => this.IbtnRerollScccessNext()));
    EventDelegate.Set(this.popupAccountDeleteBtn.onClick, (EventDelegate.Callback) (() => this.IbtnDataDelete()));
    EventDelegate.Set(this.popupAccountDeleteNext.onClick, (EventDelegate.Callback) (() => this.IbtnAccountDeleteNext()));
    EventDelegate.Set(this.popupAccountDeleteBack.onClick, (EventDelegate.Callback) (() =>
    {
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1004");
      this.IbtnAccountDeleteBack();
    }));
    EventDelegate.Set(this.popupAccountDelete2Next.onClick, (EventDelegate.Callback) (() => this.IbtnAccountDelete2Next()));
    EventDelegate.Set(this.popupAccountDelete2Back.onClick, (EventDelegate.Callback) (() =>
    {
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1004");
      this.IbtnAccountDeleteBack();
    }));
    EventDelegate.Set(this.popupAccountDelete3Next.onClick, (EventDelegate.Callback) (() => this.IbtnAccountDelete3Next()));
    EventDelegate.Set(this.popupAccountDelete3Back.onClick, (EventDelegate.Callback) (() =>
    {
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1004");
      this.IbtnAccountDeleteBack();
    }));
    EventDelegate.Set(this.popupAccountDeleteSuccessOk.onClick, (EventDelegate.Callback) (() => this.IbtnAccountDeleteScccessNext()));
    this.FilterSortDataCheck();
    EventDelegate.Set(component.buttonPressStart.onClick, (EventDelegate.Callback) (() => this.OnTouchStartHeaven()));
    ModalWindow.setupRootPanel(this.uiRoot);
    AppSetupFPS.SetDefault();
    if (!Object.op_Inequality((Object) this.debugContainer, (Object) null))
      return;
    Object.Destroy((Object) this.debugContainer);
    this.debugContainer = (GameObject) null;
  }

  private bool isEventPeriod(string timeStart, string timeEnd)
  {
    DateTime dateTime1 = Convert.ToDateTime(timeStart);
    DateTime dateTime2 = Convert.ToDateTime(timeEnd);
    DateTime now = DateTime.Now;
    return DateTime.Compare(now, dateTime1) > 0 && DateTime.Compare(now, dateTime2) < 0;
  }

  private void FilterSortDataCheck()
  {
    if (!PlayerPrefs.HasKey("FilterSortDataReset"))
      PlayerPrefs.SetInt("FilterSortDataReset", 0);
    if (PlayerPrefs.GetInt("FilterSortDataReset") == 1)
      return;
    Persist.unit00410SortAndFilter.Delete();
    Persist.unit00411SortAndFilter.Delete();
    Persist.unit00412SortAndFilter.Delete();
    Persist.unit00468SortAndFilter.Delete();
    Persist.unit0048SortAndFilter.Delete();
    Persist.unit00481SortAndFilter.Delete();
    Persist.unit00491SortAndFilter.Delete();
    Persist.unit004912SortAndFilter.Delete();
    Persist.unit004ReincarnationTypeAndFilter.Delete();
    Persist.unit004431SortAndFilter.Delete();
    Persist.unit00486SortAndFilter.Delete();
    Persist.unit00487SortAndFilter.Delete();
    Persist.unit005411SortAndFilter.Delete();
    Persist.unit005468SortAndFilter.Delete();
    Persist.tower029UnitListSortAndFilter.Delete();
    Persist.unit004ExtraSkillEquipUnitListSortAndFilter.Delete();
    Persist.unit004StorageSortAndFilter.Delete();
    Persist.unit004JobChangeUnitSelectSortAndFilter.Delete();
    Persist.mypageEditorSortAndFilter.Delete();
    Persist.friendSupportSortAndFilter.Delete();
    Persist.unitLumpToutaSortAndFilter.Delete();
    Persist.bugu0052SortAndFilter.Delete();
    Persist.bugu005SupplyListSortAndFilter.Delete();
    Persist.bugu005MaterialListSortAndFilter.Delete();
    Persist.bugu005WeaponMaterialListSortAndFilter.Delete();
    Persist.bugu0052CompositeSortAndFilter.Delete();
    Persist.bugu0052RepairSortAndFilter.Delete();
    Persist.bugu0052SellSortAndFilter.Delete();
    Persist.bugu0052DrillingBaseSortAndFilter.Delete();
    Persist.bugu0052DrillingMaterialSortAndFilter.Delete();
    Persist.bugu0052BuildupBaseSortAndFilter.Delete();
    Persist.bugu0052BuildupMaterialSortAndFilter.Delete();
    Persist.unit0044SortAndFilter.Delete();
    Persist.bugu0552SortAndFilter.Delete();
    Persist.bugu055SellSortAndFilter.Delete();
    Persist.bugu0544SortAndFilter.Delete();
    Persist.unit004ExtraSkillSortAndFilter.Delete();
    Persist.unit004ExtraSkillEquipListSortAndFilter.Delete();
    PlayerPrefs.SetInt("FilterSortDataReset", 1);
  }

  private void Update()
  {
  }

  private void TransPopupCondition()
  {
    switch (this.pCond)
    {
      case TopScene.PopupCondition.NONE:
        this.ibtnBackEnd();
        break;
      case TopScene.PopupCondition.USER_POLICY:
        this.UserPolicyDissent();
        break;
      case TopScene.PopupCondition.USER_POLICY_DISSENT:
        this.UserPolicyCaution();
        break;
      case TopScene.PopupCondition.PLIVACY:
        this.PrivacyOff();
        break;
      case TopScene.PopupCondition.DATA_LOAD:
        this.DataLoadOff();
        break;
      case TopScene.PopupCondition.DATA_LOAD_WARNING:
        this.DataLoadWarningOff();
        break;
      case TopScene.PopupCondition.DATA_LOAD_SUCCESS:
        this.SuccessOff();
        break;
      case TopScene.PopupCondition.CLEAR_CACHE:
        this.popupDelete();
        break;
      case TopScene.PopupCondition.CLEAR_CACHE_WARNING:
        this.popupDelete();
        break;
      case TopScene.PopupCondition.GAME_END:
        this.popupDelete();
        break;
      case TopScene.PopupCondition.WEBVIEW:
        this.CloseWebView();
        break;
      case TopScene.PopupCondition.DATA_LOAD_SELECT:
        this.DataLoadSelectOff();
        break;
      case TopScene.PopupCondition.DATA_LOAD_FGGID:
        this.DataLoadFgGIDOff();
        break;
      case TopScene.PopupCondition.SOCIAL_SIGN_OUT:
        this.popupDelete();
        break;
      case TopScene.PopupCondition.ACCOUNT_MANAGEMENT:
        this.IbtnAccountManagementBack();
        break;
      case TopScene.PopupCondition.ACCOUNT_REROLL:
        this.IbtnRerollBack();
        break;
      case TopScene.PopupCondition.ACCOUNT_DELETE:
        this.IbtnAccountDeleteBack();
        break;
    }
  }

  private void ibtnBackEnd()
  {
    this.pCond = TopScene.PopupCondition.GAME_END;
    Consts instance = Consts.GetInstance();
    this.nowPopup = ((Component) ModalWindow.ShowYesNo(instance.gamequit_title, instance.gamequit_text, new Action(this.Quit), (Action) (() =>
    {
      this.pCond = TopScene.PopupCondition.NONE;
      this.nowPopup = (GameObject) null;
    }))).gameObject;
  }

  private void popupDelete()
  {
    Object.DestroyObject((Object) this.nowPopup);
    this.pCond = TopScene.PopupCondition.NONE;
  }

  private void openWebview()
  {
    if (this.pCond != TopScene.PopupCondition.NONE)
      return;
    this.pCond = TopScene.PopupCondition.WEBVIEW;
    ((Component) this.m_webview).GetComponent<UniWebViewController>().Navigate();
  }

  private void Quit()
  {
  }

  public void SuccessOff()
  {
    this.Success.gameObject.SetActive(false);
    this.BackCollider.gameObject.SetActive(false);
    StartScript.Restart();
  }

  public void AccountManagementOn()
  {
    if (this.isIntoTouch)
      return;
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1006");
    this.BackCollider.gameObject.SetActive(true);
    this.pCond = TopScene.PopupCondition.ACCOUNT_MANAGEMENT;
    this.popupAccountManagement.SetActive(true);
    this.IbtnDataLoadOn();
  }

  public void IbtnDataLoadOn()
  {
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1006");
    this.popupAccountManagement.SetActive(false);
    ((Component) this.data_load_warning).gameObject.SetActive(true);
    this.BackCollider.gameObject.SetActive(true);
    this.pCond = TopScene.PopupCondition.DATA_LOAD_WARNING;
  }

  public void IbtnAccountManagementBack()
  {
    this.popupAccountManagement.SetActive(false);
    this.BackCollider.gameObject.SetActive(false);
    this.pCond = TopScene.PopupCondition.NONE;
  }

  public void IbtnDataReroll()
  {
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
    if (Persist.userInfo.Exists)
    {
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1006");
      this.popupAccountManagement.SetActive(false);
      this.popupReroll.SetActive(true);
      this.BackCollider.gameObject.SetActive(true);
      this.pCond = TopScene.PopupCondition.ACCOUNT_REROLL;
    }
    else
      this.ngMessageUi.SetMessageByPosType("アカウントがないのでデータ初期化はできません");
  }

  public void IbtnRerollNext()
  {
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1006");
    this.popupReroll.SetActive(false);
    this.popupReroll2.SetActive(true);
  }

  public void IbtnReroll2Next()
  {
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
    this.popupReroll2.SetActive(false);
    this.Reroll();
  }

  private void Reroll()
  {
    SMManager.Change<Player>((Player) null);
    SMManager.UpdateList<PlayerCommonTicket>(new PlayerCommonTicket[0], true);
    Persist.DeleteAll();
    foreach (string file in Directory.GetFiles(PersistentPath.Value))
    {
      try
      {
        File.Delete(file);
      }
      catch (Exception ex)
      {
        Debug.Log((object) ex);
      }
    }
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1006");
    this.popupRerollSuccess.SetActive(true);
  }

  public void IbtnRerollBack()
  {
    this.popupReroll.SetActive(false);
    this.popupReroll2.SetActive(false);
    this.BackCollider.gameObject.SetActive(false);
    this.pCond = TopScene.PopupCondition.NONE;
  }

  public void IbtnRerollScccessNext()
  {
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
    this.popupRerollSuccess.SetActive(false);
    this.BackCollider.gameObject.SetActive(false);
    this.pCond = TopScene.PopupCondition.NONE;
    StartScript.Restart();
  }

  public void IbtnDataDelete()
  {
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
    if (Persist.userInfo.Exists)
    {
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1006");
      this.popupAccountManagement.SetActive(false);
      this.popupAccountDelete.SetActive(true);
      this.BackCollider.gameObject.SetActive(true);
      this.pCond = TopScene.PopupCondition.ACCOUNT_DELETE;
    }
    else
      this.ngMessageUi.SetMessageByPosType("アカウントがないのでアカウント削除はできません");
  }

  public void IbtnAccountDeleteNext()
  {
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1006");
    ((UIButtonColor) this.popupAccountDelete2Next).isEnabled = false;
    this.AccountDeleteCheckBoxOn.SetActive(false);
    this.AccountDeleteCheckBoxOff.SetActive(true);
    this.popupAccountDelete.SetActive(false);
    this.popupAccountDelete2.SetActive(true);
  }

  public void IbtnAccountDelete2Next()
  {
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1006");
    this.popupAccountDelete2.SetActive(false);
    this.popupAccountDelete3.SetActive(true);
  }

  public void IbtnAccountDelete3Next()
  {
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
    this.popupAccountDelete3.SetActive(false);
    this.StartCoroutine(this.AccountDeleteApi());
  }

  private IEnumerator AccountDeleteApi()
  {
    this.LoadingObj.SetActive(true);
    IEnumerator e = WebAPI.DeleteAccount((Action<WebAPI.Response.UserError>) (error => WebAPI.DefaultUserErrorCallback(error))).Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    SMManager.Change<Player>((Player) null);
    SMManager.UpdateList<PlayerCommonTicket>(new PlayerCommonTicket[0], true);
    Persist.DeleteAll();
    foreach (string file in Directory.GetFiles(PersistentPath.Value))
    {
      try
      {
        File.Delete(file);
      }
      catch (Exception ex)
      {
        Debug.Log((object) ex);
      }
    }
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1006");
    this.LoadingObj.SetActive(false);
    this.popupAccountDeleteSuccess.SetActive(true);
  }

  public void IbtnAccountDeleteBack()
  {
    this.popupAccountDelete.SetActive(false);
    this.popupAccountDelete2.SetActive(false);
    this.popupAccountDelete3.SetActive(false);
    this.BackCollider.gameObject.SetActive(false);
    this.pCond = TopScene.PopupCondition.NONE;
  }

  public void IbtnAccountDeleteScccessNext()
  {
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
    this.popupAccountDeleteSuccess.SetActive(false);
    this.BackCollider.gameObject.SetActive(false);
    this.pCond = TopScene.PopupCondition.NONE;
    StartScript.Restart();
  }

  public void IbtnPopupNext()
  {
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1006");
    ((Component) this.data_load_warning).gameObject.SetActive(false);
    this.BackCollider.gameObject.SetActive(true);
    ((Component) this.data_load).gameObject.SetActive(true);
    this.pCond = TopScene.PopupCondition.DATA_LOAD;
  }

  public void IbtnPopupMigrate()
  {
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1006");
    ((Component) this.data_load_select).gameObject.SetActive(false);
    ((Component) this.data_load).gameObject.SetActive(true);
    this.data_load.InitDataCode();
    this.BackCollider.gameObject.SetActive(true);
    this.pCond = TopScene.PopupCondition.DATA_LOAD;
  }

  public void IbtnPopupFgGIDMigrate()
  {
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1006");
    ((Component) this.data_load_select).gameObject.SetActive(false);
    ((Component) this.data_load_fggid).gameObject.SetActive(true);
    this.data_load_fggid.InitDataCode();
    this.BackCollider.gameObject.SetActive(true);
    this.pCond = TopScene.PopupCondition.DATA_LOAD_FGGID;
  }

  public void DataLoadWarningOff()
  {
    ((Component) this.data_load_warning).gameObject.SetActive(false);
    this.BackCollider.gameObject.SetActive(false);
    this.pCond = TopScene.PopupCondition.NONE;
  }

  public void IbtnPopupDecide()
  {
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
    this.data_load.MigrateAPI();
    ((Component) this.data_load).gameObject.SetActive(false);
    this.pCond = TopScene.PopupCondition.DATA_LOAD;
  }

  public void IbtnPopupFgGIDDecide()
  {
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
    this.data_load_fggid.FgGIDMigrateAPI();
    ((Component) this.data_load_fggid).gameObject.SetActive(false);
    this.pCond = TopScene.PopupCondition.DATA_LOAD_FGGID;
  }

  public void DataLoadSelectOff()
  {
    ((Component) this.data_load_select).gameObject.SetActive(false);
    this.BackCollider.gameObject.SetActive(false);
    this.pCond = TopScene.PopupCondition.NONE;
  }

  public void DataLoadOff()
  {
    ((Component) this.data_load).gameObject.SetActive(false);
    this.BackCollider.gameObject.SetActive(false);
    if (this.CodeError.activeSelf || this.InputBlock.activeSelf || this.SameTerminal.activeSelf)
      this.pCond = TopScene.PopupCondition.LOCK;
    else if (this.Success.activeSelf)
    {
      this.pCond = TopScene.PopupCondition.DATA_LOAD_SUCCESS;
      this.SuccessOff();
    }
    else
      this.pCond = TopScene.PopupCondition.NONE;
  }

  public void DataLoadFgGIDOff()
  {
    ((Component) this.data_load_fggid).gameObject.SetActive(false);
    this.BackCollider.gameObject.SetActive(false);
    if (this.CodeErrorFgGID.activeSelf || this.InputBlock.activeSelf || this.SameTerminal.activeSelf)
      this.pCond = TopScene.PopupCondition.LOCK;
    else if (this.Success.activeSelf)
    {
      this.pCond = TopScene.PopupCondition.DATA_LOAD_SUCCESS;
      this.SuccessOff();
    }
    else
      this.pCond = TopScene.PopupCondition.NONE;
  }

  public void DeleteCacheOn()
  {
    if (this.isIntoTouch)
      return;
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
    Consts instance = Consts.GetInstance();
    if (Persist.battleEnvironment.Exists || Persist.pvpSuspend.Exists)
    {
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1006");
      ModalWindow.ShowYesNo(instance.cache_clear_warning_title, instance.cache_clear_warning_body, (Action) (() =>
      {
        Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
        this.pCond = TopScene.PopupCondition.NONE;
        Persist.cacheInfo.Data.hasDeleted = true;
        Persist.cacheInfo.Flush();
        this.StartCoroutine(this.startClearCacheWithAutoSleep());
      }), (Action) (() =>
      {
        Singleton<NGSoundManager>.GetInstance().playSE("SE_1004");
        this.pCond = TopScene.PopupCondition.NONE;
      }));
      this.pCond = TopScene.PopupCondition.CLEAR_CACHE_WARNING;
    }
    else
    {
      string cacheTextNoMobile = instance.clear_cache_text_no_mobile;
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1006");
      this.nowPopup = ((Component) ModalWindow.ShowYesNo(instance.clear_cache_title, cacheTextNoMobile, (Action) (() =>
      {
        Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
        this.StartCoroutine(this.startClearCacheWithAutoSleep());
      }), (Action) (() =>
      {
        Singleton<NGSoundManager>.GetInstance().playSE("SE_1004");
        this.pCond = TopScene.PopupCondition.NONE;
      }))).gameObject;
      this.pCond = TopScene.PopupCondition.CLEAR_CACHE;
    }
  }

  private IEnumerator startClearCacheWithAutoSleep()
  {
    this.pCond = TopScene.PopupCondition.LOCK;
    App.SetAutoSleep(false);
    IEnumerator e = this.startClearCache();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    App.SetAutoSleep(true);
  }

  private IEnumerator startClearCache()
  {
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1006");
    Consts c = Consts.GetInstance();
    ModalWindow window = ModalWindow.Show(c.clear_cache_title, c.clearCacheProgress(0, 0), (Action) (() =>
    {
      this.pCond = TopScene.PopupCondition.NONE;
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
    }));
    window.DisableOkButton();
    IEnumerator e = ResourceDownloader.CleanCache((Action<int, int>) ((numerator, denominator) => window.SetText(c.clearCacheProgress(numerator, denominator))));
    while (e.MoveNext())
      yield return e.Current;
    window.SetText(c.clear_cache_done);
    window.EnableOkButton();
  }

  public void OpenWebView()
  {
    if (Object.op_Equality((Object) this.m_webview, (Object) null))
      return;
    this.pCond = TopScene.PopupCondition.WEBVIEW;
  }

  public void CloseWebView()
  {
    if (Object.op_Equality((Object) this.m_webview, (Object) null))
      return;
    this.pCond = TopScene.PopupCondition.NONE;
    if (!Object.op_Inequality((Object) this.m_FggIdConnect, (Object) null))
      return;
    this.m_FggIdConnect.Initialize();
  }

  public void PrivacyOn()
  {
    if (this.isIntoTouch)
      return;
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1006");
    if (!this.isInitalizedPrivacyPolicy)
    {
      this.privacy.InitScene();
      this.isInitalizedPrivacyPolicy = true;
    }
    ((Component) this.privacy).gameObject.SetActive(true);
    this.BackCollider.gameObject.SetActive(true);
    this.pCond = TopScene.PopupCondition.PLIVACY;
  }

  public void PrivacyOff()
  {
    ((Component) this.privacy).gameObject.SetActive(false);
    this.BackCollider.gameObject.SetActive(false);
    this.pCond = TopScene.PopupCondition.NONE;
  }

  public void UserPolicyOn()
  {
    this.BackCollider.gameObject.SetActive(true);
    this.pCond = TopScene.PopupCondition.USER_POLICY;
    this.StartCoroutine(this.doWaitPolicyOn());
  }

  private IEnumerator doWaitPolicyOn()
  {
    if (Object.op_Equality((Object) this.termsOfService, (Object) null))
    {
      this.termsOfService = TermsOfService.GetData();
      this.userPolicy.Initialize(this.termsOfService.content.title, this.termsOfService.content.header, "利用規約\n" + this.termsOfService.content.text, "\n\nプライバシーポリシー\n" + this.termsOfService.privacyPolicy);
    }
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1006");
    ((Component) this.userPolicy).gameObject.SetActive(true);
    yield return (object) this.userPolicy.ScrollValue();
  }

  public void UserPolicyAgree()
  {
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
    ((Component) this.userPolicy).gameObject.SetActive(false);
    this.BackCollider.gameObject.SetActive(false);
    Persist.userPolicy.Data.SetUserPolicy(true, TermsOfService.update_date);
    Persist.userPolicy.Flush();
    this.StartCoroutine(this.UserPolicy());
  }

  public void UserPolicyDissent()
  {
    this.pCond = TopScene.PopupCondition.USER_POLICY_DISSENT;
    ((Component) this.userPolicy).gameObject.SetActive(false);
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1006");
    this.StartCoroutine(PopupCommon.ShowUserPolicyDissent(this.termsOfService.content.titleDissent, this.termsOfService.content.textDissent, this.popupPanel.transform, (Action) (() =>
    {
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
      this.BackCollider.gameObject.SetActive(false);
      this.pCond = TopScene.PopupCondition.NONE;
    })));
  }

  public void UserPolicyCaution()
  {
    this.BackCollider.gameObject.SetActive(false);
    this.pCond = TopScene.PopupCondition.NONE;
    PopupCommon componentInChildren = this.popupPanel.GetComponentInChildren<PopupCommon>();
    if (!Object.op_Inequality((Object) componentInChildren, (Object) null))
      return;
    Object.Destroy((Object) ((Component) componentInChildren).gameObject);
  }

  public void SelectFPSOn()
  {
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1006");
    this.BackCollider.gameObject.SetActive(true);
    ((Component) this.popupSelectFPS).gameObject.SetActive(true);
    this.pCond = TopScene.PopupCondition.SELECT_FPS;
  }

  public void SelectFPSOff()
  {
    this.BackCollider.gameObject.SetActive(false);
    ((Component) this.popupSelectFPS).gameObject.SetActive(false);
    this.pCond = TopScene.PopupCondition.NONE;
  }

  public void CodeErrorOff()
  {
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
    this.BackCollider.SetActive(false);
    this.CodeError.SetActive(false);
    this.pCond = TopScene.PopupCondition.NONE;
  }

  public void CodeErrorFgGIDOff()
  {
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
    this.BackCollider.SetActive(false);
    this.CodeErrorFgGID.SetActive(false);
    this.pCond = TopScene.PopupCondition.NONE;
  }

  public void InputBlockOff()
  {
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
    this.InputBlock.SetActive(false);
    this.BackCollider.SetActive(false);
    this.pCond = TopScene.PopupCondition.NONE;
  }

  public void SameTerminalOff()
  {
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
    this.SameTerminal.SetActive(false);
    this.BackCollider.SetActive(false);
    this.pCond = TopScene.PopupCondition.NONE;
  }

  public void UnknownOff()
  {
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
    this.Unknown.SetActive(false);
    this.BackCollider.SetActive(false);
    this.pCond = TopScene.PopupCondition.NONE;
  }

  public void SceneChangeOPMovie()
  {
    int num = this.isIntoTouch ? 1 : 0;
  }

  private IEnumerator Start()
  {
    TopScene scene = this;
    while (!SDK.Initialized)
      yield return (object) null;
    scene.txtVersion.SetTextLocalize(Consts.Format(Consts.GetInstance().START_APPLICATION_VERSION, (IDictionary) new Hashtable()
    {
      {
        (object) "version",
        (object) Application.version
      }
    }));
    if (Persist.userInfo.Exists)
    {
      ((Component) scene.txtUserID).gameObject.SetActive(true);
      ((UIButtonColor) scene.popupAccountDeleteBtn).isEnabled = true;
      scene.txtUserID.SetTextLocalize(Consts.Format(Consts.GetInstance().START_USER_ID, (IDictionary) new Hashtable()
      {
        {
          (object) "user_id",
          (object) Persist.userInfo.Data.userId
        }
      }));
    }
    else
      ((Component) scene.txtUserID).gameObject.SetActive(false);
    scene.isIntoTouch = true;
    if (Object.op_Inequality((Object) scene.PGSSignInButton, (Object) null))
      scene.PGSSignInButton.SetActive(false);
    Singleton<SocialManager>.GetInstance().Auth((Action<bool>) (success => { }));
    yield return (object) ServerTime.WaitSync();
    scene.menu.SetActive(true);
    string clip = scene.defaultBgmName;
    if (scene.isEventPeriod(scene.eventDateStart, scene.eventDateEnd))
      clip = scene.eventBgmName;
    if (clip != null || "" != clip)
    {
      Singleton<NGSoundManager>.GetInstance().OpeningStart();
      Singleton<NGSoundManager>.GetInstance().playBGM(clip);
    }
    ((Component) scene.txtApplicationVersion).gameObject.SetActive(false);
    ((Component) scene.txtScreenSize).gameObject.SetActive(false);
    if (NGGameDataManager.UrlSchemePresentId != -1)
      yield return (object) scene.StartCoroutine(URLScheme.Instance.RequestPresentGet());
    scene.isIntoTouch = false;
    scene.BackCollider.SetActive(false);
    scene.backGroundAnim.Init();
    scene.btnAnim.Init(scene);
  }

  private void OnDestroy() => this.criWareInitializer.atomConfig.useRandomSeedWithTime = false;

  public void SignInPGS()
  {
    if (this.pCond != TopScene.PopupCondition.NONE)
      return;
    if (Singleton<SocialManager>.GetInstance().isLogin)
      this.ShowSocialSignOutPopup();
    else
      Singleton<SocialManager>.GetInstance().Auth((Action<bool>) (success => { }));
  }

  private void ShowSocialSignOutPopup()
  {
    this.pCond = TopScene.PopupCondition.SOCIAL_SIGN_OUT;
    this.nowPopup = ((Component) ModalWindow.ShowYesNo(Consts.GetInstance().SOCIAL_PLATFORM_NAME, "サインアウトしますか？", (Action) (() =>
    {
      Singleton<SocialManager>.GetInstance().SignOut();
      this.pCond = TopScene.PopupCondition.NONE;
      this.nowPopup = (GameObject) null;
    }), (Action) (() =>
    {
      this.pCond = TopScene.PopupCondition.NONE;
      this.nowPopup = (GameObject) null;
    }))).gameObject;
  }

  public void OnTouchStart()
  {
    if (!this.EnablePressStartBtn)
      return;
    this.StartGame(true);
  }

  public void OnTouchStartHeaven()
  {
    if (!this.EnablePressStartBtn)
      return;
    this.StartGame(false);
  }

  private void StartGame(bool isSea)
  {
    this.BackCollider.SetActive(true);
    this.isIntoTouch = false;
    this.pCond = TopScene.PopupCondition.LOCK;
    NGGameDataManager.SeaChangeFlag = isSea;
    this.StartCoroutine(this.UserPolicy());
  }

  private void changeNextScene()
  {
    this.pCond = TopScene.PopupCondition.LOCK;
    this.StartCoroutine(this.changeNextSceneLoop());
  }

  private IEnumerator changeNextSceneLoop()
  {
    BootLoader bootLoader = BootLoader.Lunch();
    while (!bootLoader.End)
      yield return (object) null;
    Object.Destroy((Object) bootLoader);
    Singleton<NGSoundManager>.GetInstance().IsTitleScene = false;
    WebAPI.Response.PlayerBootRelease lastPlayerBoot = WebAPI.LastPlayerBoot;
    if (!lastPlayerBoot.latest_application)
    {
      bool waitClick = true;
      ModalWindow.Show(Consts.GetInstance().TOP_SCENE_CHANGE_NEXT_SCENE_LOOP_1, Consts.GetInstance().TOP_SCENE_CHANGE_NEXT_SCENE_LOOP_2_PC, (Action) (() => waitClick = false));
      while (waitClick)
        yield return (object) null;
    }
    else if (!lastPlayerBoot.player_is_create)
      SceneManager.LoadScene("startup000_10_DL");
    else if (ResourceDownloader.IsDlcVersionChange())
    {
      SceneManager.LoadScene("startup000_10_2");
    }
    else
    {
      IEnumerator e = Singleton<ResourceManager>.GetInstance().InitResourceInfo();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ResourceInfo resourceInfo = Singleton<ResourceManager>.GetInstance().ResourceInfo;
      bool isAllMoved = Persist.fileMoved.Data.isAllMoved;
      HashSet<string> stringSet1 = new HashSet<string>();
      HashSet<string> stringSet2 = !isAllMoved ? new HashSet<string>((IEnumerable<string>) FileManager.GetEntries(DLC.ResourceDirectory)) : DLC.GetEntries();
      foreach (ResourceInfo.Resource resource in resourceInfo)
      {
        ResourceInfo.Value obj = resource._value;
        switch (obj._path_type)
        {
          case ResourceInfo.PathType.AssetBundle:
            if (stringSet2.Contains(obj._file_name))
            {
              if (isAllMoved)
              {
                CachedFile.Add(DLC.GetPath(obj._file_name));
                continue;
              }
              CachedFile.Add(DLC.ResourceDirectory + obj._file_name);
              continue;
            }
            continue;
          case ResourceInfo.PathType.StreamingAssets:
            if (stringSet2.Contains(obj._file_name))
            {
              if (isAllMoved)
              {
                CachedFile.Add(DLC.GetPath(obj._file_name));
                continue;
              }
              CachedFile.Add(DLC.ResourceDirectory + obj._file_name);
              continue;
            }
            continue;
          default:
            continue;
        }
      }
      ResourceDownloader.Completed = true;
      SceneManager.LoadScene("main");
    }
  }

  public IEnumerator UserPolicy()
  {
    if (!this.isIntoTouch)
    {
      if (!Persist.userPolicy.Data.GetUserPolicy(TermsOfService.update_date))
      {
        this.UserPolicyOn();
        while (true)
          yield return (object) null;
      }
      else
      {
        bool isFpsConfirm = false;
        if (!Persist.appFPS.Data.IsSetup && Object.op_Inequality((Object) this.popupSelectFPS, (Object) null))
        {
          this.SelectFPSOn();
          while (!Persist.appFPS.Data.IsSetup)
            yield return (object) null;
          this.SelectFPSOff();
          isFpsConfirm = true;
        }
        bool isSoundConfirm = false;
        bool flag = false;
        if (Object.op_Inequality((Object) this.popupSpeedPriority, (Object) null))
        {
          if (!Persist.speedPriority.Data.IsSpeedPrioritySetup)
          {
            Singleton<NGSoundManager>.GetInstance().playSE("SE_1006");
            this.BackCollider.gameObject.SetActive(true);
            this.pCond = TopScene.PopupCondition.SELECT_GRAPHIC;
            ((Component) this.popupSpeedPriority).gameObject.SetActive(true);
            while (!Persist.speedPriority.Data.IsSpeedPrioritySetup)
              yield return (object) null;
            ((Component) this.popupSpeedPriority).gameObject.SetActive(false);
            this.pCond = TopScene.PopupCondition.NONE;
            this.BackCollider.gameObject.SetActive(false);
            flag = true;
          }
          else
            PerformanceConfig.GetInstance().IsSpeedPriority = Persist.speedPriority.Data.IsSpeedPriority;
          ScreenUtil.RefreshPerformanceResolution();
        }
        if (Object.op_Inequality((Object) this.popupConfirmFPSAndSound, (Object) null) && isFpsConfirm | isSoundConfirm | flag)
        {
          Singleton<NGSoundManager>.GetInstance().playSE("SE_1006");
          this.BackCollider.gameObject.SetActive(true);
          this.pCond = TopScene.PopupCondition.SELECT_FPS_AND_SOUND_CONFIRM;
          ((Component) this.popupConfirmFPSAndSound).gameObject.SetActive(true);
          string str1 = "";
          string str2 = Persist.appFPS.Data.MaxFPS != 30 ? str1 + "アニメーション品質を「高 (60fps)」" : str1 + "アニメーション品質を「低 (30fps)」";
          if (str2 != "")
            str2 += "\n";
          string str3 = !Persist.normalDLC.Data.IsSound ? str2 + "サウンド品質を「高音質版」" : str2 + "サウンド品質を「通常版」";
          if (str3 != "")
            str3 += "\n";
          this.popupConfirmFPSAndSound.SelectText((!Persist.speedPriority.Data.IsSpeedPriority ? str3 + "グラフィック品質を「高品質」" : str3 + "グラフィック品質を「速度優先」") + "\nに設定しました。");
          while (!this.popupConfirmFPSAndSound.IsDecide)
            yield return (object) null;
          ((Component) this.popupConfirmFPSAndSound).gameObject.SetActive(false);
          this.pCond = TopScene.PopupCondition.NONE;
          this.BackCollider.gameObject.SetActive(false);
        }
        this.isIntoTouch = true;
        Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
        Singleton<NGSoundManager>.GetInstance().playSE("SE_1001");
        this.changeNextSceneAfterAnim();
      }
    }
  }

  public void OnCheckBox()
  {
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
    this.AccountDeleteCheckBoxOn.SetActive(!this.AccountDeleteCheckBoxOn.activeSelf);
    this.AccountDeleteCheckBoxOff.SetActive(!this.AccountDeleteCheckBoxOff.activeSelf);
    ((UIButtonColor) this.popupAccountDelete2Next).isEnabled = this.AccountDeleteCheckBoxOn.activeSelf;
  }

  public void changeNextSceneAfterAnim()
  {
    if (Object.op_Inequality((Object) this.backGroundAnim, (Object) null))
      this.backGroundAnim.StartFinishAnim(new Action(this.changeNextScene));
    else
      this.changeNextScene();
  }

  public enum PopupCondition
  {
    NONE,
    USER_POLICY,
    USER_POLICY_DISSENT,
    PLIVACY,
    DATA_LOAD,
    DATA_LOAD_WARNING,
    DATA_LOAD_SUCCESS,
    CLEAR_CACHE,
    CLEAR_CACHE_WARNING,
    GAME_END,
    LOCK,
    WEBVIEW,
    DATA_LOAD_SELECT,
    DATA_LOAD_FGGID,
    SELECT_FPS,
    SELECT_SOUND,
    SELECT_FPS_AND_SOUND_CONFIRM,
    SOCIAL_SIGN_OUT,
    SELECT_GRAPHIC,
    ACCOUNT_MANAGEMENT,
    ACCOUNT_REROLL,
    ACCOUNT_DELETE,
  }
}
