// Decompiled with JetBrains decompiler
// Type: Raid032BattleScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;

#nullable disable
public class Raid032BattleScene : NGSceneBase
{
  private Raid032BattleMenu menu;

  public static void changeScene(
    bool stack,
    int loopCount,
    int raid_id,
    bool isSimulation,
    bool fromBattle)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("raid032_battle", (stack ? 1 : 0) != 0, (object) loopCount, (object) raid_id, (object) isSimulation, (object) fromBattle);
  }

  public override IEnumerator onInitSceneAsync()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Raid032BattleScene raid032BattleScene = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    raid032BattleScene.isActiveHeader = false;
    raid032BattleScene.menu = raid032BattleScene.menuBase as Raid032BattleMenu;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) raid032BattleScene.menu.initAsync();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator onStartSceneAsync(
    int loopCount,
    int raid_id,
    bool isSimulation,
    bool isFromBattle)
  {
    if (this.menu.isInitializeSucceeded)
      yield return (object) this.menu.onBackSceneAsync();
    else
      yield return (object) this.menu.onStartSceneAsync(loopCount, raid_id, isSimulation, isFromBattle);
  }

  public void onStartScene(int loopCount, int raid_id, bool isSimulation, bool isFromBattle)
  {
    if (!this.menu.isInitializeSucceeded)
      return;
    Singleton<CommonRoot>.GetInstance().isActiveBackground = false;
    this.StartCoroutine(this.waitForReposition());
  }

  private IEnumerator waitForReposition()
  {
    yield return (object) null;
    if (this.menu.isInitializeSucceeded)
      Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public IEnumerator onBackSceneAsync(
    int loopCount,
    int raid_id,
    bool isSimulation,
    bool isFromBattle)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    yield return (object) null;
    yield return (object) this.menu.onBackSceneAsync();
  }

  public void onBackScene(int loopCount, int raid_id, bool isSimulation, bool isFromBattle)
  {
    Singleton<CommonRoot>.GetInstance().isActiveBackground = false;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }
}
