// Decompiled with JetBrains decompiler
// Type: Unit0544Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/Earth/unit054_4/Scene")]
public class Unit0544Scene : NGSceneBase
{
  public Unit0544Menu menu;

  public override IEnumerator onInitSceneAsync()
  {
    yield break;
  }

  public static void ChangeScene(bool stack, PlayerUnit basePlayerUnit, int changeGearIndex)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit054_4", (stack ? 1 : 0) != 0, (object) basePlayerUnit, (object) changeGearIndex);
  }

  public IEnumerator onStartSceneAsync(PlayerUnit basePlayerUnit, int changeGearIndex)
  {
    if (Singleton<NGGameDataManager>.GetInstance().IsColosseum)
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
    this.menu.IsEarthMode = true;
    this.menu.BasePlayerUnit = basePlayerUnit;
    this.menu.ChangeGearIndex = changeGearIndex;
    IEnumerator e = this.menu.Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual void onStartScene(PlayerUnit basePlayerUnit, int changeGearIndex)
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
