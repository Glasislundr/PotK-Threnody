// Decompiled with JetBrains decompiler
// Type: Bugu00523Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using System.Collections.Generic;

#nullable disable
public class Bugu00523Scene : NGSceneBase
{
  public Bugu00523Menu menu;

  public override IEnumerator onInitSceneAsync()
  {
    yield break;
  }

  public static void ChangeScene(bool stack, List<ItemInfo> select, ItemInfo target)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("bugu005_buildup_material", (stack ? 1 : 0) != 0, (object) select, (object) target);
  }

  public IEnumerator onStartSceneAsync(List<ItemInfo> select, ItemInfo target)
  {
    if (Singleton<NGGameDataManager>.GetInstance().IsColosseum)
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
    this.menu.SetFirstSelectItem(select, target);
    IEnumerator e = this.menu.Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual void onStartScene(List<ItemInfo> select, ItemInfo target)
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
  }
}
