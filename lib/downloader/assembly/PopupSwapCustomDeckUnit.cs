// Decompiled with JetBrains decompiler
// Type: PopupSwapCustomDeckUnit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/CustomDeck/Popup/SwapCustomDeckUnit")]
public class PopupSwapCustomDeckUnit : BackButtonPopupBase
{
  [SerializeField]
  [Tooltip("位置を同じにしたいオブジェクトをセット")]
  private PopupSwapCustomDeckUnit.MatchPosition[] positions_;
  [SerializeField]
  private Transform[] lnkIcons_;
  [SerializeField]
  private UIButton[] buttons_;
  [SerializeField]
  [Tooltip("選択チェック表示")]
  private GameObject[] marks_;
  [SerializeField]
  [Tooltip("選択音")]
  private string seSelect_;
  [SerializeField]
  [Tooltip("選択解除音")]
  private string seUnselect_;
  [SerializeField]
  [Tooltip("選択出来ない音")]
  private string seNG_;
  private bool[] isBlanks_;
  private Action<int, int> onSwap_;
  private Action onClose_;
  private int lastSelected_ = -1;

  public static GameObject show(
    GameObject prefab,
    GameObject[] icons,
    bool[] bBlanks,
    Action<int, int> eventSwap,
    Action eventClose)
  {
    GameObject gameObject = Singleton<PopupManager>.GetInstance().open(prefab);
    gameObject.GetComponent<PopupSwapCustomDeckUnit>().initialize(icons, bBlanks, eventSwap, eventClose);
    return gameObject;
  }

  private void initialize(
    GameObject[] icons,
    bool[] bBlanks,
    Action<int, int> eventSwap,
    Action eventClose)
  {
    this.setTopObject(((Component) this).gameObject);
    this.isBlanks_ = bBlanks;
    this.onSwap_ = eventSwap;
    this.onClose_ = eventClose;
    for (int no = 0; no < this.lnkIcons_.Length; ++no)
    {
      if (Object.op_Implicit((Object) icons[no]))
        icons[no].Clone(this.lnkIcons_[no]);
      this.setButtonEvent(no);
    }
    ((IEnumerable<GameObject>) this.marks_).SetActives(false);
    ((UIRect) ((Component) this).GetComponent<UIWidget>()).alpha = 0.0f;
  }

  private void setButtonEvent(int no)
  {
    EventDelegate.Set(this.buttons_[no].onClick, (EventDelegate.Callback) (() => this.onSelect(no)));
  }

  private void onSelect(int no)
  {
    if (this.IsPush)
      return;
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    if (this.lastSelected_ < 0)
    {
      this.marks_[no].SetActive(true);
      this.lastSelected_ = no;
      instance.PlaySe(this.seSelect_);
    }
    else if (this.lastSelected_ != no)
    {
      if (this.isBlanks_[this.lastSelected_] && this.isBlanks_[no])
      {
        instance.PlaySe(this.seNG_);
        this.onClose_();
        this.hide();
      }
      else
      {
        this.marks_[no].SetActive(true);
        this.onSwap_(this.lastSelected_, no);
        instance.PlaySe(this.seSelect_);
        this.hide();
      }
    }
    else
    {
      this.marks_[no].SetActive(false);
      this.lastSelected_ = -1;
      instance.PlaySe(this.seUnselect_);
    }
  }

  private void hide()
  {
    this.IsPush = true;
    ((UIRect) ((Component) this).GetComponent<UIWidget>()).alpha = 0.0f;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.onClose_();
    this.hide();
  }

  private void Start()
  {
    this.matchPositions(this.positions_);
    ((UIRect) ((Component) this).GetComponent<UIWidget>()).alpha = 1f;
  }

  private void matchPositions(PopupSwapCustomDeckUnit.MatchPosition[] targets)
  {
    Transform transform1 = ((Component) Singleton<NGSceneManager>.GetInstance().sceneBase).transform.Find("MainPanel");
    for (int index = 0; index < targets.Length; ++index)
    {
      PopupSwapCustomDeckUnit.MatchPosition target = targets[index];
      Transform transform2 = transform1.Find(target.path);
      if (Object.op_Implicit((Object) transform2))
        target.pos.position = transform2.position;
    }
  }

  [Serializable]
  private class MatchPosition
  {
    public string path;
    public Transform pos;
  }
}
