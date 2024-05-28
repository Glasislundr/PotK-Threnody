// Decompiled with JetBrains decompiler
// Type: Unit004ReincarnationTypeCompleteScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Unit004ReincarnationTypeCompleteScene : NGSceneBase
{
  [SerializeField]
  private Unit004ReincarnationTypeCompleteMenu menu;

  public static void changeScene(
    bool stack,
    PlayerUnit basePlayerUnit,
    PlayerUnit resultPlayerUnit)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_Reincarnation_Type_Complete", (stack ? 1 : 0) != 0, (object) basePlayerUnit, (object) resultPlayerUnit);
  }

  public override IEnumerator onInitSceneAsync()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Unit004ReincarnationTypeCompleteScene typeCompleteScene = this;
    Future<GameObject> bgF;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      typeCompleteScene.backgroundPrefab = bgF.Result;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    bgF = Res.Prefabs.BackGround.UnitBackground_60.Load<GameObject>();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) bgF.Wait();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator onStartSceneAsync(PlayerUnit basePlayerUnit, PlayerUnit resultPlayerUnit)
  {
    yield return (object) this.menu.setCharacter(basePlayerUnit, resultPlayerUnit);
    Singleton<PopupManager>.GetInstance().closeAll();
  }
}
