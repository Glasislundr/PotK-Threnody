// Decompiled with JetBrains decompiler
// Type: Story0593Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Story0593Scene : NGSceneBase
{
  private Story0593Menu menu;
  private bool isInit;

  public static void ChangeScene(
    bool stack,
    EarthQuestChapter chapter,
    List<Story059ItemData> itemList)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("story059_3", (stack ? 1 : 0) != 0, (object) chapter, (object) itemList);
  }

  public override IEnumerator onInitSceneAsync()
  {
    Story0593Scene story0593Scene = this;
    story0593Scene.menu = story0593Scene.menuBase as Story0593Menu;
    if (!Object.op_Equality((Object) story0593Scene.menu, (Object) null))
    {
      story0593Scene.isInit = false;
      IEnumerator e = story0593Scene.menu.InitAsync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public IEnumerator onStartSceneAsync(EarthQuestChapter chapter, List<Story059ItemData> itemList)
  {
    if (!this.isInit && !Object.op_Equality((Object) this.menu, (Object) null))
    {
      IEnumerator e = this.menu.StartAsync(chapter, itemList);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.isInit = true;
    }
  }
}
