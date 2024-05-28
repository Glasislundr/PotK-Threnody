// Decompiled with JetBrains decompiler
// Type: Startup000102Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#nullable disable
public class Startup000102Menu : MonoBehaviour
{
  [SerializeField]
  private UILabel TxtDiscription;
  [SerializeField]
  private UILabel TxtPercentage;
  [SerializeField]
  private float interval = 5f;
  [SerializeField]
  private List<TweenAlpha> tweenList;
  [SerializeField]
  private NGTweenGaugeScale gaugeScale;
  [SerializeField]
  private TweenAlpha fade;
  [SerializeField]
  private UIRoot uiRoot;
  private List<GameObject> prefabList;
  private GameObject ImgObj;
  private GameObject ShadowImgObj;
  private bool showCharacter = true;
  private int naviSerihuNumber;
  public GameObject DownLoadVar;
  private StartupDownLoad.State state;

  private void Awake() => ModalWindow.setupRootPanel(this.uiRoot);

  public void Start()
  {
    ((UITweener) this.fade).PlayForward();
    this.naviSerihuNumber = 0;
    this.gaugeScale.setValue(0, 100, false);
  }

  public void Update()
  {
    float num = Mathf.Clamp((float) StartupDownLoad.progressNum() / (float) StartupDownLoad.progressDem() * 100f, 0.0f, 100f);
    this.TxtPercentage.SetTextLocalize(((int) num).ToString() + "％");
    this.gaugeScale.setValue(Mathf.FloorToInt(num), 100, false);
    this.state = StartupDownLoad.state;
    switch (this.state)
    {
      case StartupDownLoad.State.Processing:
        if ((int) StartupDownLoad.progressNum() == 0)
          break;
        this.DLStart();
        break;
      case StartupDownLoad.State.Completed:
        this.DLComplete();
        break;
    }
  }

  private float FadePlay(bool reverse = false)
  {
    float duration = 0.0f;
    if (reverse)
      this.tweenList.ForEach((Action<TweenAlpha>) (x =>
      {
        ((UITweener) x).PlayReverse();
        if ((double) ((UITweener) x).duration <= (double) duration)
          return;
        duration = ((UITweener) x).duration;
      }));
    else
      this.tweenList.ForEach((Action<TweenAlpha>) (x =>
      {
        ((UITweener) x).Play(true);
        if ((double) ((UITweener) x).duration <= (double) duration)
          return;
        duration = ((UITweener) x).duration;
      }));
    return duration;
  }

  private void DLStart() => this.DownLoadVar.SetActive(true);

  private void DLComplete() => this.StartCoroutine(this.SceneChange());

  private IEnumerator SceneChange()
  {
    yield return (object) new WaitForSeconds(0.1f);
    SceneManager.LoadScene("main");
  }
}
