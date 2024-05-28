// Decompiled with JetBrains decompiler
// Type: Unit00487Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public class Unit00487Scene : NGSceneBase
{
  public Unit00487Menu menu487;
  protected bool isInit;

  public override IEnumerator onInitSceneAsync()
  {
    this.isInit = true;
    yield break;
  }

  public static void changeScene(
    bool stack,
    PlayerUnit basePlayerUnit,
    PlayerUnit[] materialUnits,
    Action exceptionBackScene = null)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_8_7", (stack ? 1 : 0) != 0, (object) basePlayerUnit, (object) materialUnits, (object) exceptionBackScene);
  }

  public IEnumerator onStartSceneAsync()
  {
    IEnumerator e = this.onStartSceneAsync(SMManager.Get<PlayerUnit[]>()[0], new PlayerUnit[5], (Action) null);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(
    PlayerUnit basePlayerUnit,
    PlayerUnit[] materialUnits,
    Action exceptionBackScene)
  {
    Player player = SMManager.Get<Player>();
    PlayerUnit[] playerUnits = SMManager.Get<PlayerUnit[]>();
    PlayerDeck[] playerDeck = SMManager.Get<PlayerDeck[]>();
    PlayerMaterialUnit[] playerMaterialUnitArray = SMManager.Get<PlayerMaterialUnit[]>();
    this.menu487.SetIconType(UnitMenuBase.IconType.Detail);
    this.menu487.exceptionBackScene = exceptionBackScene;
    IEnumerator e;
    if (this.isInit)
    {
      e = this.menu487.Init(player, basePlayerUnit, playerUnits, playerMaterialUnitArray, materialUnits, playerDeck, false, 30);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.isInit = false;
    }
    else
    {
      PlayerUnit[] array = this.menu487.SelectedUnitIcons.Select<UnitIconInfo, PlayerUnit>((Func<UnitIconInfo, PlayerUnit>) (x => x.playerUnit)).ToArray<PlayerUnit>();
      e = this.menu487.UpdateInfoAndScrollExtend((PlayerUnit[]) null, ((IEnumerable<PlayerMaterialUnit>) playerMaterialUnitArray).Where<PlayerMaterialUnit>((Func<PlayerMaterialUnit, bool>) (x => this.menu487.isComposeUnit(basePlayerUnit, x))).ToArray<PlayerMaterialUnit>(), array);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public override void onEndScene()
  {
    Persist.sortOrder.Flush();
    UnitDetailIcon.ClearCache();
  }
}
