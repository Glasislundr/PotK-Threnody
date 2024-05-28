// Decompiled with JetBrains decompiler
// Type: Mypage051Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using DeviceKit;
using Earth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Mypage051Scene : NGSceneBase
{
  [SerializeField]
  private Mypage051Menu menu;
  [SerializeField]
  private Mypage051Transition transition;

  public static void ChangeScene(bool stack)
  {
    Singleton<NGGameDataManager>.GetInstance().IsEarth = true;
    Singleton<NGSceneManager>.GetInstance().changeScene("mypage051", stack);
  }

  public bool isAnimePlaying
  {
    get => Object.op_Inequality((Object) this.menu, (Object) null) && this.menu.isAnimePlaying;
  }

  public override List<string> createResourceLoadList()
  {
    EarthDataManager instanceOrNull = Singleton<EarthDataManager>.GetInstanceOrNull();
    return Object.op_Equality((Object) instanceOrNull, (Object) null) ? new List<string>() : instanceOrNull.createResourceLoadList();
  }

  public override IEnumerator onInitSceneAsync()
  {
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    IEnumerator e = this.menu.InitSceneAsync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync()
  {
    IEnumerator e = this.onStartSceneAsync(false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(bool isCloudAnim)
  {
    IEnumerator e = this.LoadBackGround();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.menu.onStartSceneAsync(isCloudAnim, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene()
  {
    App.SetAutoSleep(true);
    CommonRoot instance = Singleton<CommonRoot>.GetInstance();
    instance.isLoading = false;
    this.menu.CharaAnimProc();
    if (instance.getCloudAnimEnabled())
    {
      this.menu.SetJogDialActive(false);
      this.menu.PlayTween(MypageMenuBase.START_TWEENGROUP_TOP);
      instance.StartCloudAnimEnd((Action) (() =>
      {
        this.menu.SetJogDialActive(true);
        this.menu.PlayTween(MypageMenuBase.START_TWEEN_GROUP_JOGDIAL);
        this.menu.SetFirstTweenDelegate();
      }));
    }
    else
      this.menu.PlayTween(MypageMenuBase.START_TWEENGROUP);
  }

  public override void onSceneInitialized()
  {
    base.onSceneInitialized();
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
  }

  public override void onEndScene() => this.menu.onEndScene();

  private IEnumerator LoadBackGround()
  {
    IEnumerator e = ((Component) this).GetComponent<BGChange>().EarthBGprefabCreate(Singleton<EarthDataManager>.GetInstance().questProgress.currentEpisode.background_name);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
