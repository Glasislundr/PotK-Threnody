// Decompiled with JetBrains decompiler
// Type: Unit0044Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using CustomDeck;
using MasterDataTable;
using SM;
using System.Collections;
using System.Collections.Generic;

#nullable disable
public class Unit0044Scene : NGSceneBase
{
  public Unit0044Menu menu;
  private const int EQUIPPED_WEAPON_SLOT = 1;

  public override IEnumerator onInitSceneAsync()
  {
    Unit0044Scene unit0044Scene = this;
    if (Singleton<CommonRoot>.GetInstance().headerType == CommonRoot.HeaderType.Tower)
    {
      unit0044Scene.bgmFile = TowerUtil.BgmFile;
      unit0044Scene.bgmName = TowerUtil.BgmName;
    }
    else if (Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      IEnumerator e = unit0044Scene.SetSeaBgm();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public static void changeScene(bool bStack, EditGearParam param)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_4", (bStack ? 1 : 0) != 0, (object) param);
  }

  public static void ChangeScene(bool stack, PlayerUnit basePlayerUnit, int changeGearIndex)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_4", (stack ? 1 : 0) != 0, (object) basePlayerUnit, (object) changeGearIndex);
  }

  public static void ChangeSceneRecommend(
    bool stack,
    PlayerUnit basePlayerUnit,
    GearGear specialGear)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_4_recommend", (stack ? 1 : 0) != 0, (object) basePlayerUnit, (object) specialGear);
  }

  public IEnumerator onStartSceneAsync(EditGearParam param)
  {
    this.menu.EditParam = param;
    yield return (object) this.doCommonInit(param.baseUnit, param.slotNo + 1, (GearGear) null);
  }

  public void onStartScene(EditGearParam param)
  {
    this.onStartScene(param.baseUnit, param.slotNo + 1);
  }

  public virtual IEnumerator onStartSceneAsync(PlayerUnit basePlayerUnit, int changeGearIndex)
  {
    yield return (object) this.doCommonInit(basePlayerUnit, changeGearIndex, (GearGear) null);
  }

  private IEnumerator doCommonInit(
    PlayerUnit basePlayerUnit,
    int changeGearIndex,
    GearGear specialGear)
  {
    if (Singleton<NGGameDataManager>.GetInstance().IsColosseum)
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
    this.menu.BasePlayerUnit = basePlayerUnit;
    this.menu.ChangeGearIndex = changeGearIndex;
    this.menu.SpecialGear = specialGear;
    yield return (object) this.menu.Init();
  }

  public virtual void onStartScene(PlayerUnit basePlayerUnit, int changeGearIndex)
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    if (Singleton<NGGameDataManager>.GetInstance().IsColosseum)
      return;
    Singleton<CommonRoot>.GetInstance().isActiveHeader = true;
    Singleton<CommonRoot>.GetInstance().isActiveFooter = true;
  }

  public IEnumerator onStartSceneAsync(PlayerUnit basePlayerUnit, GearGear specialGear)
  {
    yield return (object) this.doCommonInit(basePlayerUnit, 1, specialGear);
  }

  public void onStartScene(PlayerUnit basePlayerUnit, GearGear specialGgear)
  {
    this.onStartScene(basePlayerUnit, 1);
  }

  public override void onEndScene()
  {
    if (!this.menu.isDisabledSort)
      Persist.sortOrder.Flush();
    this.menu.onEndScene();
    ItemIcon.ClearCache();
  }

  private IEnumerator SetSeaBgm()
  {
    Unit0044Scene unit0044Scene = this;
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    SeaHomeMap seaHomeMap = ((IEnumerable<SeaHomeMap>) MasterData.SeaHomeMapList).ActiveSeaHomeMap(ServerTime.NowAppTimeAddDelta());
    if (seaHomeMap != null && !string.IsNullOrEmpty(seaHomeMap.bgm_cuesheet_name) && !string.IsNullOrEmpty(seaHomeMap.bgm_cue_name))
    {
      unit0044Scene.bgmFile = seaHomeMap.bgm_cuesheet_name;
      unit0044Scene.bgmName = seaHomeMap.bgm_cue_name;
    }
  }
}
