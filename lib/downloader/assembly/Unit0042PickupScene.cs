// Decompiled with JetBrains decompiler
// Type: Unit0042PickupScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/Unit/GachaPickup/Scene")]
public class Unit0042PickupScene : NGSceneBase
{
  public static readonly string defName = "unit004_2_pickup";
  [SerializeField]
  private Unit0042PickupMenu menu_;

  public static void changeScene(bool bStack, string gacha_name, GachaModuleGacha gacha)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene(Unit0042PickupScene.defName, (bStack ? 1 : 0) != 0, (object) gacha_name, (object) gacha);
  }

  public static void changeScene(bool bStack, int unitID)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene(Unit0042PickupScene.defName, (bStack ? 1 : 0) != 0, (object) unitID);
  }

  public override IEnumerator onInitSceneAsync()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Unit0042PickupScene unit0042PickupScene = this;
    Future<GameObject> ldBG;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      unit0042PickupScene.backgroundPrefab = ldBG.Result;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    ldBG = Res.Prefabs.BackGround.GachaTopBackground.Load<GameObject>();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) ldBG.Wait();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator onStartSceneAsync(string gacha_name, GachaModuleGacha gacha)
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    Dictionary<UnitTypeEnum, PlayerUnit>[] dicUnits = (Dictionary<UnitTypeEnum, PlayerUnit>[]) null;
    if (gacha.is_gacha_pickup)
    {
      Future<WebAPI.Response.GachaGetPickupUnitMaxStatus> wapi = WebAPI.GachaGetPickupUnitMaxStatus(gacha.id, gacha_name, (Action<WebAPI.Response.UserError>) (error => WebAPI.DefaultUserErrorCallback(error)));
      yield return (object) wapi.Wait();
      if (wapi.Result != null)
        dicUnits = PlayerUnit.create_for_pickup(wapi.Result);
      wapi = (Future<WebAPI.Response.GachaGetPickupUnitMaxStatus>) null;
    }
    IEnumerator e;
    if (dicUnits == null || dicUnits.Length == 0)
    {
      e = this.doWaitError();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    e = this.commonStartSceneAsync(dicUnits);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(int unitID)
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    IEnumerator e = this.commonStartSceneAsync(new Dictionary<UnitTypeEnum, PlayerUnit>[1]
    {
      PlayerUnit.create_for_pickup_without_param(unitID)
    });
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator commonStartSceneAsync(Dictionary<UnitTypeEnum, PlayerUnit>[] dicUnits)
  {
    yield return (object) OnDemandDownload.WaitLoadUnitResource(((IEnumerable<Dictionary<UnitTypeEnum, PlayerUnit>>) dicUnits).Select<Dictionary<UnitTypeEnum, PlayerUnit>, PlayerUnit>((Func<Dictionary<UnitTypeEnum, PlayerUnit>, PlayerUnit>) (x => x.Values.First<PlayerUnit>())), false);
    yield return (object) this.menu_.Init(dicUnits);
  }

  public void onStartScene(string gacha_name, GachaModuleGacha gacha)
  {
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  public void onStartScene(int unitID) => this.onStartScene((string) null, (GachaModuleGacha) null);

  public IEnumerator onBackSceneAsync(string gacha_name, GachaModuleGacha gacha)
  {
    this.menu_.onBackScene();
    yield break;
  }

  public IEnumerator onBackSceneAsync(int unitID)
  {
    yield return (object) this.onBackSceneAsync((string) null, (GachaModuleGacha) null);
  }

  public void onBackScene(string gacha_name, GachaModuleGacha gacha)
  {
    this.onStartScene(gacha_name, gacha);
  }

  public void onBackScene(int unitID) => this.onBackScene((string) null, (GachaModuleGacha) null);

  public override IEnumerator onEndSceneAsync()
  {
    yield return (object) this.menu_.onEndSceneAsync();
  }

  private IEnumerator doWaitError()
  {
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    Singleton<NGSceneManager>.GetInstance().ChangeErrorPage();
    while (true)
      yield return (object) null;
  }
}
