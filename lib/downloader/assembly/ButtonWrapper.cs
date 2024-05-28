// Decompiled with JetBrains decompiler
// Type: ButtonWrapper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class ButtonWrapper : MonoBehaviour
{
  [SerializeField]
  private UIButton button_;
  [SerializeField]
  [Tooltip("isEnabledの初期値")]
  private bool firstEnabled_ = true;
  [SerializeField]
  [Tooltip("ボタンアクティブ時アクティブ")]
  private ButtonWrapper.TweenerControl activeTweenOnEnable_ = new ButtonWrapper.TweenerControl();
  [SerializeField]
  [Tooltip("ボタンアクティブ時アクティブ")]
  private GameObject[] activeObjectOnEnable_;
  [SerializeField]
  [Tooltip("ボタン非アクティブ時アクティブ")]
  private ButtonWrapper.TweenerControl activeTweenOnDisable_ = new ButtonWrapper.TweenerControl();
  [SerializeField]
  [Tooltip("ボタン非アクティブ時アクティブ")]
  private GameObject[] activeObjectOnDisable_;
  private bool initialized_;
  private bool isEnabled_;

  private UIButton button
  {
    get
    {
      return !Object.op_Inequality((Object) this.button_, (Object) null) ? (this.button_ = ((Component) this).GetComponent<UIButton>()) : this.button_;
    }
  }

  public bool isEnabled
  {
    get
    {
      this.initialize();
      return this.isEnabled_;
    }
    set
    {
      this.initialize();
      if (this.isEnabled_ == value)
        return;
      this.setEnable(value);
    }
  }

  private void Awake() => this.initialize();

  private void initialize()
  {
    if (this.initialized_)
      return;
    this.initialized_ = true;
    this.setEnable(this.firstEnabled_);
  }

  private void setEnable(bool bEnable)
  {
    this.isEnabled_ = bEnable;
    UIButton button = this.button;
    if (Object.op_Inequality((Object) button, (Object) null))
      ((UIButtonColor) button).isEnabled = bEnable;
    List<GameObject[]> gameObjectArrayList;
    List<UITweener[]> uiTweenerArrayList;
    if (bEnable)
    {
      gameObjectArrayList = new List<GameObject[]>()
      {
        this.activeObjectOnDisable_,
        this.activeObjectOnEnable_
      };
      uiTweenerArrayList = new List<UITweener[]>()
      {
        this.activeTweenOnDisable_.tweeners,
        this.activeTweenOnEnable_.tweeners
      };
    }
    else
    {
      gameObjectArrayList = new List<GameObject[]>()
      {
        this.activeObjectOnEnable_,
        this.activeObjectOnDisable_
      };
      uiTweenerArrayList = new List<UITweener[]>()
      {
        this.activeTweenOnEnable_.tweeners,
        this.activeTweenOnDisable_.tweeners
      };
    }
    for (int index = 0; index < 2; ++index)
    {
      bool flag = index != 0;
      if (gameObjectArrayList[index] != null)
      {
        foreach (GameObject gameObject in gameObjectArrayList[index])
        {
          if (Object.op_Inequality((Object) gameObject, (Object) null))
            gameObject.SetActive(flag);
        }
      }
      if (uiTweenerArrayList[index] != null)
      {
        foreach (Behaviour behaviour in uiTweenerArrayList[index])
          behaviour.enabled = flag;
      }
    }
  }

  [Serializable]
  private class TweenerControl
  {
    [SerializeField]
    [Tooltip("このtweenGroupを操作する")]
    private int group_;
    [SerializeField]
    [Tooltip("この列からUITweenerを取る")]
    private GameObject[] objects_;
    private UITweener[] tweeners_;

    public UITweener[] tweeners
    {
      get
      {
        return this.tweeners_ ?? (this.tweeners_ = ((IEnumerable<GameObject>) this.objects_).SelectMany<GameObject, UITweener>((Func<GameObject, IEnumerable<UITweener>>) (o => ((IEnumerable<UITweener>) o.GetComponentsInChildren<UITweener>(true)).Where<UITweener>((Func<UITweener, bool>) (t => t.tweenGroup == this.group_)))).ToArray<UITweener>());
      }
    }
  }
}
