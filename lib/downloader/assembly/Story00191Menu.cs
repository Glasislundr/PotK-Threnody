// Decompiled with JetBrains decompiler
// Type: Story00191Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using DeviceKit;
using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Story00191Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  private GameObject BtnCredit;
  [SerializeField]
  private GameObject FriendBadge;
  [SerializeField]
  private GameObject LoginBonusBadge;
  [SerializeField]
  private GameObject UpdateFile;
  [SerializeField]
  private GameObject LoginBonusMonthly;
  private GameObject modalWindow;
  private GameObject dataDownloadPrefab;
  private GameObject dataDeletePrefab;

  public virtual void Foreground()
  {
  }

  public virtual void IbtnBase()
  {
  }

  public virtual void IbtnBaseWithoutIcon()
  {
  }

  public virtual void VScrollBar()
  {
  }

  private void Awake() => this.UpdateFile.SetActive(false);

  public IEnumerator onInitSceneAsync()
  {
    Future<GameObject> popupF;
    IEnumerator e;
    if (Object.op_Equality((Object) this.dataDownloadPrefab, (Object) null))
    {
      popupF = new ResourceObject("Prefabs/popup/popup_001_9_1_VoiceDL__anim_popup01").Load<GameObject>();
      e = popupF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.dataDownloadPrefab = popupF.Result;
      popupF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.dataDeletePrefab, (Object) null))
    {
      popupF = new ResourceObject("Prefabs/story001_9_1/popup_AccountDelete__anim_popup01").Load<GameObject>();
      e = popupF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.dataDeletePrefab = popupF.Result;
      popupF = (Future<GameObject>) null;
    }
  }

  public void InitAsync()
  {
    this.LoginBonusMonthly.SetActive(SMManager.Get<Player>().IsLoginBonusMonthly());
  }

  public void Init()
  {
    this.FriendBadgeSetting();
    this.LoginBonusBadgeSetting();
  }

  private void FriendBadgeSetting()
  {
    if (Object.op_Equality((Object) this.FriendBadge, (Object) null))
      return;
    this.FriendBadge.SetActive(Singleton<NGGameDataManager>.GetInstance().ReceivedFriendRequestCount > 0);
  }

  private void LoginBonusBadgeSetting()
  {
    if (Object.op_Equality((Object) this.LoginBonusBadge, (Object) null))
      return;
    this.LoginBonusBadge.SetActive(Singleton<NGGameDataManager>.GetInstance().hasFillableLoginbonus);
  }

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    MypageScene.ChangeScene();
  }

  public override void onBackButton() => this.IbtnBack();

  public void IbtnTitle()
  {
    this.modalWindow = ((Component) ModalWindow.ShowYesNo(Consts.GetInstance().titleback_title, Consts.GetInstance().titleback_text, (Action) (() => StartScript.Restart()), (Action) (() => this.DeleteModalWindow()))).gameObject;
  }

  private void DeleteModalWindow() => Object.Destroy((Object) this.modalWindow);

  public void IbtnColosseumTitle()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("title024_1", true, (object) SMManager.Get<Player>().id);
  }

  public void IbtnBook()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<CommonRoot>.GetInstance().loadingMode = 4;
    Singleton<NGSceneManager>.GetInstance().changeScene("guide011_1", true);
  }

  public void IbtnFriend()
  {
    if (this.IsPushAndSet())
      return;
    Friend0081Scene.ChangeScene();
  }

  public void IbtnLoginBonus()
  {
    if (this.IsPushAndSet())
      return;
    Startup000LoginBonusConfirmScene.changeScene(true);
  }

  public void IbtnOption()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("setting010_1", true);
  }

  public void IbtnTransfer()
  {
    if (this.IsPushAndSet())
      return;
    Transfer01272Scene.ChangeScene(true);
  }

  public void IbtnHelp()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("help015_1", true);
  }

  public void IbtnBeginnerNavi()
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("help015_5", true);
  }

  public void IbtnPurchase()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("purchase016_2", true);
  }

  private void ShowAchievementsUI() => Singleton<SocialManager>.GetInstance().ShowAchievementsUI();

  public void IbtnAchievements() => this.ShowAchievementsUI();

  public void IbtnUsepolicy()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("mypage001_17", true, (object) Mypage00117Scene.Rule.TermsOfService);
  }

  public void IbtnTransmissionRule()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("mypage001_17", true, (object) Mypage00117Scene.Rule.TransmissionRule);
  }

  public void IbtnCopyright()
  {
    if (this.IsPushAndSet())
      return;
    Mypage00113Scene.changeScene(true);
  }

  public void IbtnCredit()
  {
  }

  public void IbtnBulkDownload()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.bulkDownLoadCheck());
  }

  public void showBtnCredit(bool isShow) => this.BtnCredit.SetActive(isShow);

  public void IbtnOffisialsite() => App.OpenUrl(Consts.GetInstance().OFFICAL_SITE_URL);

  private IEnumerator bulkDownLoadCheck()
  {
    Story00191Menu story00191Menu = this;
    CommonRoot common = Singleton<CommonRoot>.GetInstance();
    common.isLoading = true;
    yield return (object) new WaitForSeconds(0.5f);
    long requiredSize = OnDemandDownload.SizeOfLoadAllUnits();
    common.isLoading = false;
    Consts instance = Consts.GetInstance();
    if (requiredSize > 0L)
    {
      // ISSUE: reference to a compiler-generated method
      PopupCommonYesNo.Show(instance.bulk_download_title, instance.bulkDownloadText(requiredSize), new Action(story00191Menu.\u003CbulkDownLoadCheck\u003Eb__41_0), (Action) (() => { }));
    }
    else
      story00191Menu.StartCoroutine(PopupCommon.Show(instance.bulk_download_title, instance.bulk_downloaded_text, (Action) (() => { })));
  }

  private IEnumerator doBulkDownload()
  {
    yield return (object) new WaitForSeconds(0.5f);
    CommonRoot common = Singleton<CommonRoot>.GetInstance();
    common.isLoading = true;
    yield return (object) new WaitForSeconds(0.5f);
    IEnumerator e = OnDemandDownload.WaitLoadAllUnits(false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    common.isLoading = false;
  }

  public void IbtnAsct()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(PopupUtility._007_18());
  }

  public void IbtnAs()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(PopupUtility._007_19());
  }

  public void IbtnBlacklist()
  {
  }

  public void IbtnInviteFriend()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("story009_12", true);
  }

  public void IbtnHintsAndTips()
  {
  }

  public void IbtnDataFix()
  {
    if (this.IsPush)
      return;
    Singleton<NGGameDataManager>.GetInstance().isCallHomeUpdateAllData = true;
    Singleton<NGGameDataManager>.GetInstance().refreshHomeHome = true;
    Singleton<CommonRoot>.GetInstance().loadingMode = 4;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Singleton<NGGameDataManager>.GetInstance().StartSceneAsyncProxy((Action<NGGameDataManager.StartSceneProxyResult>) (_ =>
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    }));
  }

  public void IbtnUpdateFile()
  {
    int num = this.IsPush ? 1 : 0;
  }

  public void IbtnDetaDownload()
  {
    if (this.IsPush)
      return;
    GameObject prefab = this.dataDownloadPrefab.Clone(Singleton<CommonRoot>.GetInstance().LoadTmpObj.transform);
    prefab.GetComponent<Popup00191DataDownloadMenu>().Init(this);
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
  }

  public void IbtnAccountDelete()
  {
    if (this.IsPushAndSet())
      return;
    GameObject prefab = this.dataDeletePrefab.Clone(Singleton<CommonRoot>.GetInstance().LoadTmpObj.transform);
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
  }

  public IEnumerator DownloadContents(List<string> paths, bool fileCheckDisable = false)
  {
    IEnumerator e = OnDemandDownload.waitLoadSomethingResource((IEnumerable<string>) paths, false, fileCheckDisable);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator DeleteContents(List<string> fileNames)
  {
    IEnumerator e = ResourceDownloader.DeleteContents(fileNames);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
