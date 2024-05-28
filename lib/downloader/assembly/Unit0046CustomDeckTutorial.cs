// Decompiled with JetBrains decompiler
// Type: Unit0046CustomDeckTutorial
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using CustomDeck;
using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/unit004_6/CustomDeckTutorial")]
public class Unit0046CustomDeckTutorial : MonoBehaviour
{
  [SerializeField]
  private Transform lnkLock_;
  [SerializeField]
  private string prefabLockPath_;
  [SerializeField]
  private UIButton btnEditCustomDeck_;
  [SerializeField]
  private float waitPreUnlock_;
  [SerializeField]
  private string effPlayName_;
  [SerializeField]
  private float waitPostUnlock_;
  [SerializeField]
  private string prefabTutorialPath_;
  private bool isInitializing_ = true;
  private Unit0046CustomDeckTutorial.Status status_ = Unit0046CustomDeckTutorial.Status.Unknown;
  private GameObject prefabTutorial_;
  private GameObject objLock_;
  private Animator animator_;
  private ButtonNoneState btnLock_;

  private IEnumerator Start()
  {
    yield return (object) this.doInitialize();
  }

  private IEnumerator doInitialize(bool bDemoMode = false)
  {
    Unit0046CustomDeckTutorial customDeckTutorial = this;
    Future<GameObject> loader = (Future<GameObject>) null;
    if (!Util.checkUnlockedPlayerLevel(Player.Current.level))
    {
      customDeckTutorial.status_ = Unit0046CustomDeckTutorial.Status.Locked;
      loader = Singleton<ResourceManager>.GetInstance().Load<GameObject>(customDeckTutorial.prefabLockPath_);
    }
    else if (Persist.customDeckTutorial.Data.isUnlocked)
    {
      customDeckTutorial.status_ = Unit0046CustomDeckTutorial.Status.Unlocked;
    }
    else
    {
      customDeckTutorial.status_ = Unit0046CustomDeckTutorial.Status.UnlockReady;
      loader = Singleton<ResourceManager>.GetInstance().Load<GameObject>(customDeckTutorial.prefabLockPath_);
    }
    IEnumerator e;
    if (loader != null)
    {
      e = loader.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      customDeckTutorial.objLock_ = loader.Result.Clone(customDeckTutorial.lnkLock_);
      customDeckTutorial.animator_ = customDeckTutorial.objLock_.GetComponentInChildren<Animator>();
      customDeckTutorial.btnLock_ = customDeckTutorial.objLock_.GetComponent<ButtonNoneState>();
    }
    switch (customDeckTutorial.status_)
    {
      case Unit0046CustomDeckTutorial.Status.Unlocked:
        ((UIButtonColor) customDeckTutorial.btnEditCustomDeck_).isEnabled = true;
        break;
      case Unit0046CustomDeckTutorial.Status.Locked:
        ((UIButtonColor) customDeckTutorial.btnEditCustomDeck_).isEnabled = false;
        ((Behaviour) customDeckTutorial.animator_).enabled = false;
        EventDelegate.Set(customDeckTutorial.btnLock_.onClick, new EventDelegate.Callback(customDeckTutorial.onClickedLock));
        break;
      case Unit0046CustomDeckTutorial.Status.UnlockReady:
        ((UIButtonColor) customDeckTutorial.btnEditCustomDeck_).isEnabled = false;
        customDeckTutorial.btnLock_.isEnabled = false;
        loader = Singleton<ResourceManager>.GetInstance().Load<GameObject>(customDeckTutorial.prefabTutorialPath_);
        e = loader.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        customDeckTutorial.prefabTutorial_ = loader.Result;
        break;
    }
    customDeckTutorial.isInitializing_ = false;
  }

  private void onClickedLock()
  {
    Singleton<NGMessageUI>.GetInstance().SetMessageByPosType(Consts.GetInstance().CUSTOMDECK_UNLOCK_EDITOR, NGMessageUI.PosType.LITTLEUP);
  }

  public bool startUnlockAndTutorial()
  {
    if (this.status_ != Unit0046CustomDeckTutorial.Status.UnlockReady)
      return false;
    this.status_ = Unit0046CustomDeckTutorial.Status.UnlockWait;
    this.StartCoroutine(this.doPlayUnlockAndTutorial());
    return true;
  }

  private IEnumerator doPlayUnlockAndTutorial()
  {
    while (this.isInitializing_)
      yield return (object) null;
    while (Singleton<CommonRoot>.GetInstance().isLoading)
      yield return (object) null;
    PopupManager popupManager = Singleton<PopupManager>.GetInstance();
    popupManager.open((GameObject) null, isViewBack: false, isNonSe: true);
    yield return (object) new WaitForSeconds(this.waitPreUnlock_);
    this.animator_.Play(this.effPlayName_);
    yield return (object) new WaitForAnimation(this.animator_);
    ((UIButtonColor) this.btnEditCustomDeck_).isEnabled = true;
    yield return (object) new WaitForSeconds(this.waitPostUnlock_);
    this.status_ = Unit0046CustomDeckTutorial.Status.Unlocked;
    Object.Destroy((Object) this.objLock_);
    this.objLock_ = (GameObject) null;
    popupManager.dismiss();
    Persist.customDeckTutorial.Data.isUnlocked = true;
    Persist.customDeckTutorial.Flush();
    SimpleScrollContentsPopup popup = popupManager.open(this.prefabTutorial_, isNonSe: true, isNonOpenAnime: true).GetComponent<SimpleScrollContentsPopup>();
    ((Component) popup).GetComponent<UIRect>().alpha = 0.0f;
    IEnumerator e = popup.Initialize((Action) null);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popupManager.startOpenAnime(((Component) popup).gameObject);
  }

  private enum Status
  {
    Unknown = -1, // 0xFFFFFFFF
    Unlocked = 0,
    Locked = 1,
    UnlockReady = 2,
    UnlockWait = 3,
  }
}
