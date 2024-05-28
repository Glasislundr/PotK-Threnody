// Decompiled with JetBrains decompiler
// Type: PopupManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class PopupManager : Singleton<PopupManager>
{
  [SerializeField]
  private GameObject popupPanel;
  [SerializeField]
  private int defaultDepth = 20;
  private Stack<PopupManager.PopupParts> stack = new Stack<PopupManager.PopupParts>();
  private bool isFinished = true;
  private Queue<IEnumerator> dismissQueue = new Queue<IEnumerator>();
  public bool ModalWindowIsOpen;
  private LinkedList<IEnumerator> lstCoroutine = new LinkedList<IEnumerator>();

  protected override void Initialize()
  {
  }

  private bool startTween(GameObject o, bool active)
  {
    bool flag = false;
    if (Object.op_Equality((Object) o, (Object) null))
      return false;
    NGTweenParts component = o.GetComponent<NGTweenParts>();
    if (Object.op_Inequality((Object) component, (Object) null))
    {
      flag = true;
      component.forceActive(active);
    }
    return flag;
  }

  public GameObject openAlert(
    GameObject prefab,
    bool unmask = false,
    bool ispile = false,
    EventDelegate ed = null,
    bool isCloned = false,
    bool isViewBack = true,
    bool isNonSe = false,
    bool ignoreButtonControl = true,
    bool isCreateButtonNoneState = false)
  {
    this.StartCoroutine(this.playBackground(0.1f, (Action) (() => this.SetCurrentScenePush(true))));
    if (ed == null)
      ed = new EventDelegate((MonoBehaviour) this, "onDismiss");
    GameObject gameObject = this.open(prefab, unmask, ispile, isCloned, isViewBack, isNonSe);
    bool flag = true;
    if (Object.op_Inequality((Object) gameObject, (Object) null))
    {
      UIButton[] componentsInChildren = gameObject.GetComponentsInChildren<UIButton>(true);
      if (componentsInChildren.Length != 0 && ignoreButtonControl)
      {
        flag = false;
        foreach (UIButton uiButton in componentsInChildren)
          uiButton.onClick.Add(ed);
      }
    }
    if (flag)
    {
      PopupManager.PopupParts popupParts = this.stack.Last<PopupManager.PopupParts>();
      if (!isCreateButtonNoneState)
      {
        UIButton uiButton = ((Component) popupParts.maskTween).gameObject.AddComponent<UIButton>();
        ((UIButtonColor) uiButton).pressed = Color.black;
        uiButton.onClick.Add(ed);
      }
      else
        ((Component) popupParts.maskTween).gameObject.AddComponent<ButtonNoneState>().onClick.Add(ed);
    }
    return gameObject;
  }

  private int thisDepth()
  {
    return !this.stack.Any<PopupManager.PopupParts>() ? this.defaultDepth : this.stack.Peek().depth;
  }

  public GameObject open(
    GameObject prefab,
    bool unmask = false,
    bool ispile = false,
    bool isCloned = false,
    bool isViewBack = true,
    bool isNonSe = false,
    bool isNonOpenAnime = false,
    string clip = "SE_1006",
    Action openAnim = null,
    Action closeAnim = null,
    float maskAlpha = -1f,
    bool fadeOutFlag = false)
  {
    this.StartCoroutine(this.playBackground(0.1f, (Action) (() => this.SetCurrentScenePush(true))));
    if (!ispile && this.stack.Any<PopupManager.PopupParts>())
    {
      PopupManager.PopupParts popupParts = this.stack.Peek();
      if (Object.op_Inequality((Object) popupParts.maskTween, (Object) null))
        popupParts.maskTween.isActive = false;
      this.startTween(popupParts.popup, false);
      if (popupParts.closeAnim != null)
        popupParts.closeAnim();
    }
    PopupManager.PopupParts popupParts1 = new PopupManager.PopupParts();
    popupParts1.openAnim = openAnim;
    popupParts1.closeAnim = closeAnim;
    int num1 = this.thisDepth();
    popupParts1.ispile = ispile;
    popupParts1.panel = this.popupPanel.Clone(((Component) this).transform);
    popupParts1.fadeOutFlag = fadeOutFlag;
    UIPanel component1 = popupParts1.panel.GetComponent<UIPanel>();
    ((UIRect) component1).SetAnchor(((Component) this).transform);
    ((UIRect) component1).topAnchor.absolute = 0;
    ((UIRect) component1).bottomAnchor.absolute = 0;
    ((UIRect) component1).leftAnchor.absolute = 0;
    ((UIRect) component1).rightAnchor.absolute = 0;
    int num2 = num1;
    int num3 = num2 + 1;
    component1.depth = num2;
    if (!unmask)
    {
      foreach (Component component2 in popupParts1.panel.transform)
      {
        NGTweenParts component3 = component2.GetComponent<NGTweenParts>();
        if (Object.op_Inequality((Object) component3, (Object) null))
        {
          UI2DSprite component4 = ((Component) component3).gameObject.GetComponent<UI2DSprite>();
          if (isViewBack)
          {
            popupParts1.maskTween = component3;
            if (Object.op_Inequality((Object) component4, (Object) null) && (double) maskAlpha > 0.0)
              ((UIWidget) component4).color = new Color(((UIWidget) component4).color.r, ((UIWidget) component4).color.g, ((UIWidget) component4).color.r, maskAlpha);
          }
          else
          {
            if (Object.op_Inequality((Object) component4, (Object) null))
              ((UIWidget) component4).color = new Color(0.0f, 0.0f, 0.0f, 0.01f);
            popupParts1.maskTween = (NGTweenParts) null;
          }
        }
      }
    }
    if (Object.op_Inequality((Object) prefab, (Object) null))
    {
      if (isCloned)
      {
        prefab.SetParent(popupParts1.panel);
        popupParts1.popup = prefab;
      }
      else
        popupParts1.popup = prefab.Clone(popupParts1.panel.transform);
      UIPanel component5 = popupParts1.popup.GetComponent<UIPanel>();
      if (Object.op_Inequality((Object) component5, (Object) null))
      {
        ((UIRect) component5).SetAnchor(((Component) this).transform);
        component5.depth = num3++;
      }
      UIPanel[] componentsInChildren = popupParts1.popup.GetComponentsInChildren<UIPanel>(true);
      int num4 = num3;
      foreach (UIPanel uiPanel in componentsInChildren)
      {
        uiPanel.depth += num3;
        if (num4 < uiPanel.depth)
          num4 = uiPanel.depth;
      }
      num3 = num4 + 1;
      if (Object.op_Inequality((Object) popupParts1.maskTween, (Object) null))
        popupParts1.maskTween.isActive = true;
      if (!isNonOpenAnime)
        this.startTween(popupParts1.popup, true);
      if (popupParts1.openAnim != null)
        popupParts1.openAnim();
    }
    else
      popupParts1.popup = (GameObject) null;
    popupParts1.depth = num3;
    this.stack.Push(popupParts1);
    if (!isNonSe)
      this.StartCoroutine(this.playBackground(0.1f, (Action) (() => Singleton<NGSoundManager>.GetInstance().playSE(clip))));
    CommonRoot instance = Singleton<CommonRoot>.GetInstance();
    if (Object.op_Inequality((Object) instance, (Object) null))
      instance.resetTouchBlockActive();
    return popupParts1.popup;
  }

  public void startOpenAnime(GameObject go, bool isNonSe = false)
  {
    this.startTween(go, true);
    if (isNonSe)
      return;
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1006");
  }

  public GameObject unMaskOpen(GameObject prefab) => this.open(prefab, true);

  public void SetCurrentScenePush(bool currentScenePush)
  {
    if (!Object.op_Inequality((Object) Singleton<NGSceneManager>.GetInstance().sceneBase, (Object) null))
      return;
    Singleton<NGSceneManager>.GetInstance().sceneBase.IsPush = currentScenePush;
  }

  public bool dismiss(bool currentScenePush = false)
  {
    if (!this.stack.Any<PopupManager.PopupParts>())
      return false;
    PopupManager.PopupParts parts = this.stack.Pop();
    if (this.stack.Any<PopupManager.PopupParts>())
    {
      PopupManager.PopupParts popupParts = this.stack.Peek();
      if (Object.op_Inequality((Object) popupParts.maskTween, (Object) null))
        popupParts.maskTween.isActive = true;
      if (!parts.ispile && Object.op_Inequality((Object) parts.popup, (Object) null))
      {
        this.startTween(popupParts.popup, true);
        if (popupParts.openAnim != null)
          popupParts.openAnim();
      }
    }
    else
      this.StartCoroutine(this.playBackground(0.1f, (Action) (() => this.SetCurrentScenePush(currentScenePush))));
    this.dismissQueue.Enqueue(this.dismissWait(parts));
    if (this.dismissQueue.Count == 1)
      this.StartCoroutine(this.doDismissQueue());
    return true;
  }

  public void closeAll(bool currentScenePush = false)
  {
    do
      ;
    while (this.dismiss(currentScenePush));
  }

  public bool dismissWithoutAnim(bool currentScenePush = false, bool animBehindPopup = true)
  {
    if (!this.stack.Any<PopupManager.PopupParts>())
      return false;
    PopupManager.PopupParts parts = this.stack.Pop();
    if (Object.op_Inequality((Object) parts.maskTween, (Object) null))
      parts.maskTween.isActive = false;
    Object.Destroy((Object) parts.popup);
    parts.popup = (GameObject) null;
    if (this.stack.Any<PopupManager.PopupParts>())
    {
      if (animBehindPopup)
      {
        PopupManager.PopupParts popupParts = this.stack.Peek();
        if (Object.op_Inequality((Object) popupParts.maskTween, (Object) null))
          popupParts.maskTween.isActive = true;
        if (!parts.ispile && Object.op_Inequality((Object) parts.popup, (Object) null))
        {
          this.startTween(popupParts.popup, true);
          if (popupParts.openAnim != null)
            popupParts.openAnim();
        }
      }
    }
    else
      this.StartCoroutine(this.playBackground(0.1f, (Action) (() => this.SetCurrentScenePush(currentScenePush))));
    this.dismissQueue.Enqueue(this.dismissWait(parts));
    if (this.dismissQueue.Count == 1)
      this.StartCoroutine(this.doDismissQueue());
    return true;
  }

  public void closeAllWithoutAnim(bool currentScenePush = false)
  {
    if (!this.stack.Any<PopupManager.PopupParts>())
      return;
    PopupManager.PopupParts parts = this.stack.Pop();
    if (this.stack.Any<PopupManager.PopupParts>())
    {
      while (this.dismissWithoutAnim(currentScenePush, false))
        ;
    }
    else
      this.StartCoroutine(this.playBackground(0.1f, (Action) (() => this.SetCurrentScenePush(currentScenePush))));
    this.dismissQueue.Enqueue(this.dismissWait(parts));
    if (this.dismissQueue.Count != 1)
      return;
    this.StartCoroutine(this.doDismissQueue());
  }

  public GameObject currentObject
  {
    get => !this.stack.Any<PopupManager.PopupParts>() ? (GameObject) null : this.stack.Peek().popup;
  }

  public NGTweenParts currentMaskTween
  {
    get
    {
      return !this.stack.Any<PopupManager.PopupParts>() ? (NGTweenParts) null : this.stack.Peek().maskTween;
    }
  }

  private IEnumerator doDismissQueue()
  {
    while (this.dismissQueue.Any<IEnumerator>())
    {
      IEnumerator e = this.dismissQueue.Peek();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.dismissQueue.Dequeue();
    }
  }

  private IEnumerator dismissWait(PopupManager.PopupParts parts)
  {
    this.isFinished = true;
    if (Object.op_Inequality((Object) parts.maskTween, (Object) null))
    {
      this.isFinished = !parts.maskTween.isActive;
      if (parts.fadeOutFlag)
      {
        if (Object.op_Inequality((Object) parts.popup, (Object) null))
        {
          TweenAlpha component1 = parts.popup.GetComponent<TweenAlpha>();
          TweenAlpha component2 = ((Component) parts.maskTween).gameObject.GetComponent<TweenAlpha>();
          UI2DSprite component3 = ((Component) parts.maskTween).gameObject.GetComponent<UI2DSprite>();
          if (Object.op_Inequality((Object) component1, (Object) null) && Object.op_Inequality((Object) component2, (Object) null))
          {
            component2.from = ((UIWidget) component3).color.a;
            component2.to = 0.0f;
            ((UITweener) component2).duration = ((UITweener) component1).duration;
            ((UITweener) component2).delay = ((UITweener) component1).duration - ((UITweener) component1).duration * ((UIWidget) component3).color.a;
            ((UITweener) component2).tweenGroup = ((UITweener) component1).tweenGroup;
            ((UITweener) component2).ignoreTimeScale = ((UITweener) component1).ignoreTimeScale;
            this.startTween(((Component) parts.maskTween).gameObject, true);
          }
        }
      }
      else
        parts.maskTween.isActive = false;
    }
    if (Object.op_Inequality((Object) parts.popup, (Object) null))
    {
      if (this.startTween(parts.popup, false))
        this.isFinished = false;
      if (parts.closeAnim != null)
        parts.closeAnim();
    }
    if (!this.isFinished)
    {
      yield return (object) new WaitForSeconds(0.3f);
      this.isFinished = true;
    }
    Object.Destroy((Object) parts.panel);
  }

  public void onDismiss() => this.dismiss();

  public void onDismiss(bool currentScenePush) => this.dismiss(currentScenePush);

  public bool isOpen => this.stack.Any<PopupManager.PopupParts>() || !this.isFinished;

  public bool isOpenNoFinish => this.stack.Any<PopupManager.PopupParts>();

  private IEnumerator playBackground(float seconds, Action play)
  {
    yield return (object) new WaitForSeconds(seconds);
    play();
  }

  public static void Show(string title, string message, Action callback = null)
  {
    Singleton<PopupManager>.GetInstance().StartCoroutine(PopupCommon.Show(title, message, callback));
  }

  public static void ShowYesNo(string title, string message, Action yes = null, Action no = null)
  {
    PopupCommonOkTitle.Show(title, message, yes, no);
  }

  public static IEnumerator StartTowerEntryUnitWarningPopupProc(Action<bool> callback, bool isTrans = false)
  {
    bool popupViewing = true;
    bool isSelectYes = false;
    string empty = string.Empty;
    string message;
    if (isTrans)
      message = Consts.Format(Consts.GetInstance().TOWER_ENTRY_UNIT_TRANS_WARNING_POPUP_MSG, (IDictionary) new Hashtable()
      {
        {
          (object) "level",
          (object) TowerUtil.BorderLevel
        }
      });
    else
      message = Consts.GetInstance().TOWER_ENTRY_UNIT_WARNING_POPUP_MSG;
    PopupCommonNoYes.Show(Consts.GetInstance().TOWER_ENTRY_UNIT_WARNING_POPUP_TITLE, message, (Action) (() =>
    {
      isSelectYes = true;
      popupViewing = false;
    }), (Action) (() =>
    {
      isSelectYes = false;
      popupViewing = false;
    }), (NGUIText.Alignment) 1);
    yield return (object) new WaitWhile((Func<bool>) (() => popupViewing));
    callback(isSelectYes);
  }

  public bool isRunningCoroutine => this.lstCoroutine.Any<IEnumerator>();

  public Coroutine monitorCoroutine(IEnumerator e)
  {
    return e != null ? Singleton<NGSceneManager>.GetInstance().StartCoroutine(this.runCoroutine(e)) : (Coroutine) null;
  }

  private IEnumerator runCoroutine(IEnumerator e)
  {
    this.lstCoroutine.AddLast(e);
    while (e.MoveNext())
      yield return e.Current;
    this.lstCoroutine.Remove(e);
  }

  private class PopupParts
  {
    public GameObject panel;
    public GameObject popup;
    public NGTweenParts maskTween;
    public bool ispile;
    public int depth;
    public Action openAnim;
    public Action closeAnim;
    public bool fadeOutFlag;
  }
}
