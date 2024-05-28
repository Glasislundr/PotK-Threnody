// Decompiled with JetBrains decompiler
// Type: ModalDownloadWindow
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
public class ModalDownloadWindow : MonoBehaviour
{
  [SerializeField]
  private UILabel txtDownLoadSize_;
  [SerializeField]
  private UILabel txtDesc1;
  [SerializeField]
  private GameObject objGuide_;
  [SerializeField]
  private GameObject startEffect_;
  private UITweener[] tweens_;
  private long requireSize_;
  private bool isEnding_;
  private int countEnding_;
  private bool isCancel_;
  private Action onEnd_;
  private Action onCancel_;

  public static IEnumerator Show(IEnumerable<DLC> loaders, Action gotoNext, string strDesc1)
  {
    long requireSize = loaders.Sum<DLC>((Func<DLC, long>) (x => x.GetStoreSize()));
    if (requireSize > 0L)
    {
      GameObject prefab = Resources.Load<GameObject>("Prefabs/ModalDownloadWindow");
      while (true)
      {
        GameObject gameObject = Object.Instantiate<GameObject>(prefab);
        bool bWait = true;
        bool bCancel = false;
        gameObject.GetComponent<ModalDownloadWindow>().initialize(requireSize, strDesc1, (Action) (() => bWait = false), (Action) (() =>
        {
          bWait = false;
          bCancel = true;
        }));
        while (bWait)
          yield return (object) null;
        if (bCancel)
        {
          bWait = true;
          bCancel = false;
          ModalWindow.ShowYesNo(Consts.GetInstance().titleback_title, Consts.GetInstance().titleback_text, (Action) (() =>
          {
            bCancel = true;
            bWait = false;
          }), (Action) (() => bWait = false));
          while (bWait)
            yield return (object) null;
          if (!bCancel)
            ;
          else
            goto label_12;
        }
        else
          break;
      }
      gotoNext();
label_12:
      prefab = (GameObject) null;
    }
    else
      gotoNext();
  }

  private void initialize(long requireSize, string desc, Action eventEnd, Action eventCancel)
  {
    this.requireSize_ = requireSize;
    this.onEnd_ = eventEnd;
    this.onCancel_ = eventCancel;
    if (!string.IsNullOrEmpty(desc))
      this.txtDesc1.SetTextLocalize(desc);
    if (Application.platform != 2)
      return;
    this.objGuide_.SetActive(false);
  }

  private void Awake()
  {
    ModalWindow.setupRootPanel(((Component) this).GetComponent<UIRoot>());
    this.tweens_ = NGTween.findTweeners(this.startEffect_, false);
    NGTween.setOnTweenFinished(this.tweens_, (MonoBehaviour) this);
  }

  private void Start()
  {
    this.txtDownLoadSize_.SetTextLocalize((Math.Ceiling((Decimal) this.requireSize_ / 1024M / 1024M * 100M) / 100M).ToString("F1") + " MB");
    NGTween.playTweens(this.tweens_, NGTween.Kind.START_END);
  }

  private void Update()
  {
  }

  public void onClickedOk()
  {
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
    this.close(false);
  }

  private void close(bool bCancel)
  {
    if (this.isEnding_)
      return;
    this.isEnding_ = true;
    this.countEnding_ = 0;
    this.isCancel_ = bCancel;
    NGTween.playTweens(this.tweens_, NGTween.Kind.START_END, true);
  }

  private void onTweenFinished()
  {
    if (!this.isEnding_ || ++this.countEnding_ != this.tweens_.Length)
      return;
    if (this.isCancel_)
      this.onCancel_();
    else
      this.onEnd_();
    Object.Destroy((Object) ((Component) this).gameObject);
  }
}
