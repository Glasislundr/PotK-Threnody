﻿// Decompiled with JetBrains decompiler
// Type: Bugu005ReisouMixerScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Bugu005ReisouMixerScene : NGSceneBase
{
  public Bugu005ReisouMixerMenu menu;

  public static void ChangeScene(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("bugu005_reisou_mixer", stack);
  }

  public IEnumerator onStartSceneAsync()
  {
    if (Singleton<NGGameDataManager>.GetInstance().IsColosseum)
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
    IEnumerator e = this.menu.Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual void onStartScene()
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    Singleton<CommonRoot>.GetInstance().isActiveHeader = true;
    Singleton<CommonRoot>.GetInstance().isActiveFooter = true;
  }

  public override void onEndScene()
  {
    Persist.sortOrder.Flush();
    this.menu.onEndScene();
    ItemIcon.ClearCache();
    ((Component) this).GetComponentInChildren<NGxScroll2>().scrollView.Press(false);
  }
}
