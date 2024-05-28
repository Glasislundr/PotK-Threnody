// Decompiled with JetBrains decompiler
// Type: Unit0542Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Unit0542Scene : NGSceneBase
{
  public Unit0042Menu menu;
  protected static bool limited;
  protected bool init;

  public static void changeScene(bool stack, PlayerUnit playerUnit, PlayerUnit[] playerUnits)
  {
    Unit0542Scene.limited = false;
    Singleton<NGSceneManager>.GetInstance().changeScene("unit054_2", (stack ? 1 : 0) != 0, (object) "", (object) playerUnit, (object) playerUnits, (object) playerUnit._unit);
  }

  public static void changeSceneFriendUnit(bool stack, string player_id)
  {
    Unit0542Scene.limited = true;
    Singleton<NGSceneManager>.GetInstance().changeScene("unit054_2", (stack ? 1 : 0) != 0, (object) player_id, null, null);
  }

  public static void changeSceneEvolutionUnit(
    bool stack,
    PlayerUnit playerUnit,
    PlayerUnit[] playerUnits)
  {
    Unit0542Scene.limited = true;
    Singleton<NGSceneManager>.GetInstance().changeScene("unit054_2", (stack ? 1 : 0) != 0, (object) "", (object) playerUnit, (object) playerUnits, (object) playerUnit._unit);
  }

  public override IEnumerator onInitSceneAsync()
  {
    Unit0542Scene unit0542Scene = this;
    Future<GameObject> bgF = Res.Prefabs.BackGround.UnitBackground_p0.Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit0542Scene.backgroundPrefab = bgF.Result;
  }

  public IEnumerator onStartSceneAsync(
    string player_id,
    PlayerUnit playerUnit,
    PlayerUnit[] playerUnits,
    int unitID)
  {
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    IEnumerator e;
    if (playerUnit == (PlayerUnit) null)
    {
      if (!this.init)
      {
        Future<WebAPI.Response.FriendStatus> futureF = WebAPI.FriendStatus(player_id, unitID, (Action<WebAPI.Response.UserError>) (error =>
        {
          Singleton<CommonRoot>.GetInstance().isLoading = false;
          WebAPI.DefaultUserErrorCallback(error);
        }));
        e = futureF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        WebAPI.Response.FriendStatus result = futureF.Result;
        PlayerItem equippedGear = (PlayerItem) null;
        playerUnit = result.target_leader_unit;
        PlayerUnit[] playerUnits1 = new PlayerUnit[1]
        {
          playerUnit
        };
        foreach (int? equipGearId in playerUnit.equip_gear_ids)
        {
          int? gearId = equipGearId;
          equippedGear = ((IEnumerable<PlayerItem>) result.target_player_items).First<PlayerItem>((Func<PlayerItem, bool>) (x =>
          {
            int id = x.id;
            int? nullable = gearId;
            int valueOrDefault = nullable.GetValueOrDefault();
            return id == valueOrDefault & nullable.HasValue;
          }));
        }
        playerUnit.primary_equipped_gear = equippedGear;
        playerUnit.primary_equipped_gear2 = (PlayerItem) null;
        playerUnit.primary_equipped_gear3 = (PlayerItem) null;
        e = this.menu.Init(playerUnit, playerUnits1, true, Unit0542Scene.limited, false, true, equippedGear: equippedGear);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.init = true;
        futureF = (Future<WebAPI.Response.FriendStatus>) null;
      }
    }
    else
    {
      PlayerUnit[] playerUnitArray;
      if (playerUnits == null)
        playerUnitArray = new PlayerUnit[1]{ playerUnit };
      else
        playerUnitArray = ((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null)).ToArray<PlayerUnit>();
      if (!this.init)
      {
        this.menu.scrollView.Clear();
        ((IEnumerable<PlayerUnit>) playerUnitArray).ForEach<PlayerUnit>((Action<PlayerUnit>) (pu =>
        {
          if (pu.id != playerUnit.id)
            return;
          playerUnit = pu;
        }));
        e = this.menu.Init(playerUnit, playerUnitArray, false, Unit0542Scene.limited, false, true);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.init = true;
      }
      else
      {
        List<PlayerUnit> newData = ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).ToList<PlayerUnit>();
        List<PlayerUnit> newList = new List<PlayerUnit>();
        ((IEnumerable<PlayerUnit>) playerUnitArray).ForEach<PlayerUnit>((Action<PlayerUnit>) (x =>
        {
          PlayerUnit playerUnit1 = newData.FirstOrDefault<PlayerUnit>((Func<PlayerUnit, bool>) (y => y.id == x.id));
          if (!(playerUnit1 != (PlayerUnit) null))
            return;
          newList.Add(playerUnit1);
        }));
        if (newList.Count > 0)
        {
          e = this.menu.UpdateAllPageForEarth(newList.ToArray(), Unit0542Scene.limited);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
        else
        {
          e = this.menu.UpdateAllPageForEarth(((IEnumerable<PlayerUnit>) playerUnitArray).ToArray<PlayerUnit>(), Unit0542Scene.limited);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
      }
    }
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public override void onEndScene()
  {
    if (this.tweens != null)
    {
      foreach (Behaviour tween in this.tweens)
        tween.enabled = false;
      this.tweenTimeoutTime = 10f;
    }
    this.menu.scrollView.scrollView.Press(false);
  }

  public override IEnumerator onEndSceneAsync()
  {
    Unit0542Scene receiver = this;
    IEnumerator e = receiver.menu.onEndSceneAsync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (receiver.tweens != null)
    {
      foreach (Behaviour tween in receiver.tweens)
        tween.enabled = true;
      NGTween.setOnTweenFinished(receiver.tweens, (MonoBehaviour) receiver, "oneFrameWait");
    }
    else
      receiver.isTweenFinished = true;
  }

  protected void oneFrameWait() => this.StartCoroutine(this._oneFrameWait());

  protected IEnumerator _oneFrameWait()
  {
    Unit0542Scene unit0542Scene = this;
    yield return (object) null;
    yield return (object) null;
    yield return (object) null;
    yield return (object) null;
    unit0542Scene.isTweenFinished = true;
  }
}
