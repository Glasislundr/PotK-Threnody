// Decompiled with JetBrains decompiler
// Type: NGTweenParts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class NGTweenParts : MonoBehaviour
{
  public bool includeTweenChildren = true;
  public bool autoSetOnTweenFinished;
  public int findTweensTimeOutFrame = 5;
  private UITweener[] tweens;
  [SerializeField]
  protected bool mIsActive;
  public float timeOutTime = 0.5f;
  private bool isRunning;
  private Queue<bool> runQueue = new Queue<bool>();
  private bool isDestroy;

  private void Awake() => this.resetActive(((Component) this).gameObject.activeSelf);

  public bool isActive
  {
    get => this.mIsActive;
    set
    {
      if (this.mIsActive == value)
        return;
      this.forceActive(value);
    }
  }

  public void resetActive(bool v)
  {
    if (((Component) this).gameObject.activeSelf != v)
      ((Component) this).gameObject.SetActive(v);
    this.mIsActive = v;
  }

  public void forceActive(bool v)
  {
    this.mIsActive = v;
    if (this.runQueue.Count > 0 || this.isRunning)
    {
      this.runQueue.Enqueue(v);
    }
    else
    {
      this.isRunning = true;
      ((Component) this).gameObject.SetActive(true);
      this.playTweensDelay(v);
    }
  }

  private IEnumerator doPlayTweens(bool v)
  {
    NGTweenParts receiver = this;
    int count = receiver.findTweensTimeOutFrame;
    while (receiver.tweens == null || receiver.tweens.Length == 0)
    {
      if (receiver.isDestroy)
      {
        yield break;
      }
      else
      {
        try
        {
          receiver.tweens = NGTween.findTweeners(((Component) receiver).gameObject, receiver.includeTweenChildren);
        }
        catch (Exception ex)
        {
          receiver.onTweenFinished();
          yield break;
        }
        if (receiver.autoSetOnTweenFinished)
          NGTween.setOnTweenFinished(receiver.tweens, (MonoBehaviour) receiver);
        if (--count >= 0)
          yield return (object) 0;
        else
          break;
      }
    }
    receiver.playTweens(v);
    int nbQ = receiver.runQueue.Count;
    yield return (object) new WaitForSeconds(receiver.timeOutTime);
    if (receiver.runQueue.Count >= nbQ)
      receiver.onTweenFinished();
  }

  private void playTweensDelay(bool v)
  {
    NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
    if (!Object.op_Implicit((Object) instance))
      return;
    instance.StartCoroutine(this.doPlayTweens(v));
  }

  private void playTweens(bool v)
  {
    if (this.tweens == null)
    {
      this.tweens = NGTween.findTweeners(((Component) this).gameObject, this.includeTweenChildren);
      if (this.autoSetOnTweenFinished)
        NGTween.setOnTweenFinished(this.tweens, (MonoBehaviour) this);
    }
    bool flag1 = NGTween.playTweens(this.tweens, NGTween.Kind.START_END, !v);
    bool isTweensError = NGTween.isTweensError;
    bool flag2;
    bool flag3;
    if (v)
    {
      flag2 = NGTween.playTweens(this.tweens, NGTween.Kind.START) | flag1;
      flag3 = NGTween.isTweensError | isTweensError;
    }
    else
    {
      flag2 = NGTween.playTweens(this.tweens, NGTween.Kind.END) | flag1;
      flag3 = NGTween.isTweensError | isTweensError;
    }
    if (!flag2 && !this.mIsActive)
      ((Component) this).gameObject.SetActive(this.mIsActive);
    if (!flag3)
      return;
    this.tweens = (UITweener[]) null;
  }

  public void onTweenFinished()
  {
    if (this.isDestroy)
      return;
    if (this.runQueue.Count > 0)
    {
      this.playTweensDelay(this.runQueue.Dequeue());
    }
    else
    {
      BattleMonoBehaviour[] componentsInChildren1 = ((Component) this).GetComponentsInChildren<BattleMonoBehaviour>();
      NGBattleMenuBase[] componentsInChildren2 = ((Component) this).GetComponentsInChildren<NGBattleMenuBase>();
      if (this.mIsActive || this.checkBattleMonoBehaviour(componentsInChildren1, componentsInChildren2))
      {
        ((Component) this).gameObject.SetActive(this.mIsActive);
        this.isRunning = false;
      }
      else
        this.StartCoroutine(this.waitBattleMonoBehaviour(componentsInChildren1, componentsInChildren2));
    }
  }

  private void OnDestroy() => this.isDestroy = true;

  private IEnumerator waitBattleMonoBehaviour(BattleMonoBehaviour[] bml, NGBattleMenuBase[] mbl)
  {
    do
    {
      yield return (object) null;
    }
    while (!this.checkBattleMonoBehaviour(bml, mbl));
    this.onTweenFinished();
  }

  private bool checkBattleMonoBehaviour(BattleMonoBehaviour[] bml, NGBattleMenuBase[] mbl)
  {
    foreach (BattleMonoBehaviour battleMonoBehaviour in bml)
    {
      if (!battleMonoBehaviour.isStartInitialized())
        return false;
    }
    foreach (NGBattleMenuBase ngBattleMenuBase in mbl)
    {
      if (!ngBattleMenuBase.isStartInitialized())
        return false;
    }
    return true;
  }
}
