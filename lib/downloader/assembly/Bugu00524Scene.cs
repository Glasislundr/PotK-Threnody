// Decompiled with JetBrains decompiler
// Type: Bugu00524Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Bugu00524Scene : NGSceneBase
{
  public Bugu00524Menu menu;

  public override IEnumerator onInitSceneAsync()
  {
    yield break;
  }

  public static void ChangeScene(bool stack)
  {
    bool flag = false;
    Singleton<NGSceneManager>.GetInstance().changeScene("bugu005_repair", (stack ? 1 : 0) != 0, (object) flag);
  }

  public static void ChangeSceneFromExplore(bool stack)
  {
    bool flag = true;
    Singleton<NGSceneManager>.GetInstance().changeScene("bugu005_repair", (stack ? 1 : 0) != 0, (object) flag);
  }

  public virtual IEnumerator onStartSceneAsync(bool isFromExplore)
  {
    if (Singleton<NGGameDataManager>.GetInstance().IsColosseum)
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
    IEnumerator e = this.menu.Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.menu.isFromExplore = isFromExplore;
  }

  public virtual void onStartScene(bool isFromExplore)
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    Singleton<CommonRoot>.GetInstance().isActiveHeader = true;
    Singleton<CommonRoot>.GetInstance().isActiveFooter = true;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public override void onEndScene()
  {
    Persist.sortOrder.Flush();
    this.menu.onEndScene();
    ItemIcon.ClearCache();
    ((Component) this).GetComponentInChildren<NGxScroll2>().scrollView.Press(false);
  }
}
