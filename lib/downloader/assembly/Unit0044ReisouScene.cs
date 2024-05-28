// Decompiled with JetBrains decompiler
// Type: Unit0044ReisouScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using CustomDeck;
using GameCore;
using MasterDataTable;
using SM;
using System.Collections;
using System.Collections.Generic;

#nullable disable
public class Unit0044ReisouScene : NGSceneBase
{
  public Unit0044ReisouMenu menu;

  public override IEnumerator onInitSceneAsync()
  {
    Unit0044ReisouScene unit0044ReisouScene = this;
    if (Singleton<CommonRoot>.GetInstance().headerType == CommonRoot.HeaderType.Tower)
    {
      unit0044ReisouScene.bgmFile = TowerUtil.BgmFile;
      unit0044ReisouScene.bgmName = TowerUtil.BgmName;
    }
    else if (Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      IEnumerator e = unit0044ReisouScene.SetSeaBgm();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public static void changeScene(bool bStack, EditReisouParam param)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_4_reisou", (bStack ? 1 : 0) != 0, (object) param);
  }

  public static void ChangeScene(bool stack, ItemInfo gearInfo, ItemInfo reisouInfo)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_4_reisou", (stack ? 1 : 0) != 0, (object) gearInfo, (object) reisouInfo);
  }

  public IEnumerator onStartSceneAsync(EditReisouParam param)
  {
    ItemInfo gearInfo = new ItemInfo(param.baseGear);
    PlayerItem reisou = param.deck.unit_parameter_list[param.index].getReisou(param.reisous, param.slotNo);
    ItemInfo reisouInfo = reisou != (PlayerItem) null ? new ItemInfo(reisou) : (ItemInfo) null;
    this.menu.EditParam = param;
    IEnumerator e = this.onStartSceneAsync(gearInfo, reisouInfo);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene(EditReisouParam param)
  {
    this.onStartScene((ItemInfo) null, (ItemInfo) null);
  }

  public virtual IEnumerator onStartSceneAsync(ItemInfo gearInfo, ItemInfo reisouInfo)
  {
    if (Singleton<NGGameDataManager>.GetInstance().IsColosseum)
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
    this.menu.GearInfo = gearInfo;
    this.menu.ReisouInfo = reisouInfo;
    IEnumerator e = this.menu.Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual void onStartScene(ItemInfo gearInfo, ItemInfo reisouInfo)
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    if (Singleton<NGGameDataManager>.GetInstance().IsColosseum)
      return;
    Singleton<CommonRoot>.GetInstance().isActiveHeader = true;
    Singleton<CommonRoot>.GetInstance().isActiveFooter = true;
  }

  public override void onEndScene()
  {
    Persist.sortOrder.Flush();
    this.menu.onEndScene();
    ItemIcon.ClearCache();
  }

  private IEnumerator SetSeaBgm()
  {
    Unit0044ReisouScene unit0044ReisouScene = this;
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    SeaHomeMap seaHomeMap = ((IEnumerable<SeaHomeMap>) MasterData.SeaHomeMapList).ActiveSeaHomeMap(ServerTime.NowAppTimeAddDelta());
    if (seaHomeMap != null && !string.IsNullOrEmpty(seaHomeMap.bgm_cuesheet_name) && !string.IsNullOrEmpty(seaHomeMap.bgm_cue_name))
    {
      unit0044ReisouScene.bgmFile = seaHomeMap.bgm_cuesheet_name;
      unit0044ReisouScene.bgmName = seaHomeMap.bgm_cue_name;
    }
  }
}
