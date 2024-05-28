// Decompiled with JetBrains decompiler
// Type: Quest0529Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Earth;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Quest0529Scene : NGSceneBase
{
  [SerializeField]
  private Quest0529Menu menu;
  private bool isInit = true;

  public static void ChangeScene(bool stack, int maxNum = -1)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest052_9", (stack ? 1 : 0) != 0, (object) maxNum);
  }

  public override IEnumerator onInitSceneAsync()
  {
    yield break;
  }

  public IEnumerator onStartSceneAsync(int maxNum)
  {
    IEnumerator e = this.menu.LoadResources();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    PlayerUnit[] array = ((IEnumerable<PlayerUnit>) Singleton<EarthDataManager>.GetInstance().GetEnableSortiePlayerUnits()).ToArray<PlayerUnit>();
    PlayerItem[] formationSupplys = ((IEnumerable<PlayerItem>) Singleton<EarthDataManager>.GetInstance().GetPlayerItems()).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.supply != null)).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.box_type_id == 2)).ToArray<PlayerItem>();
    if (this.isInit)
    {
      e = this.menu.Initialize(array, maxNum);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.isInit = false;
    }
    e = this.menu.DispSupplyDeck(formationSupplys);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene(int maxNum)
  {
  }

  public override void onEndScene()
  {
  }
}
