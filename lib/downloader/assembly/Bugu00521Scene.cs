// Decompiled with JetBrains decompiler
// Type: Bugu00521Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Bugu00521Scene : NGSceneBase
{
  public Bugu00521Menu menu;

  public override IEnumerator onInitSceneAsync()
  {
    yield break;
  }

  public static void ChangeScene(bool stack, List<ItemInfo> select)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("bugu005_composite", (stack ? 1 : 0) != 0, (object) select);
  }

  public IEnumerator onStartSceneAsync(List<ItemInfo> select)
  {
    if (Singleton<NGGameDataManager>.GetInstance().IsColosseum)
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
    this.menu.SetFirstSelectItem(select);
    IEnumerator e = this.menu.Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual void onStartScene(List<ItemInfo> select)
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
