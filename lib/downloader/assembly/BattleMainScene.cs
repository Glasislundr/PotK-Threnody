// Decompiled with JetBrains decompiler
// Type: BattleMainScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;

#nullable disable
public class BattleMainScene : BattleSceneBase
{
  public override IEnumerator onInitSceneAsync()
  {
    IEnumerator e = Singleton<NGBattleManager>.GetInstance().initBattle();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene()
  {
    Singleton<CommonRoot>.GetInstance().isActiveBackground3DCamera = false;
  }

  public override void onEndScene()
  {
    Singleton<CommonRoot>.GetInstance().isActiveBackground3DCamera = true;
  }

  public override IEnumerator onDestroySceneAsync()
  {
    IEnumerator e = Singleton<NGBattleManager>.GetInstance().cleanupBattle();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
