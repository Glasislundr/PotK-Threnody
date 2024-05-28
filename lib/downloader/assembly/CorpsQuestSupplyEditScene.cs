// Decompiled with JetBrains decompiler
// Type: CorpsQuestSupplyEditScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/CorpsQuest/SupplyEditScene")]
public class CorpsQuestSupplyEditScene : NGSceneBase
{
  private CorpsQuestSupplyEditMenu menu;
  private CorpsSetting setting;

  private static void changeScene(
    bool bStack,
    PlayerCorps corps,
    int[] selectedUnitIds,
    SupplyItem[] supplyItems,
    CorpsUtil.SequenceType type)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("CorpsQuest_supplies_edit", (bStack ? 1 : 0) != 0, (object) corps, (object) selectedUnitIds, (object) supplyItems, (object) type);
  }

  public static void ChangeScene(
    PlayerCorps corps,
    int[] selectedUnitIds,
    CorpsUtil.SequenceType type)
  {
    CorpsQuestSupplyEditScene.changeScene(true, corps, selectedUnitIds, (SupplyItem[]) null, type);
  }

  public static object[] toArgs(
    PlayerCorps corps,
    int[] selectedUnitIds,
    CorpsUtil.SequenceType type)
  {
    return new object[3]
    {
      (object) corps,
      (object) selectedUnitIds,
      (object) type
    };
  }

  public static void ChangeScene(object[] args, SupplyItem[] supplyItems)
  {
    CorpsQuestSupplyEditScene.changeScene(false, (PlayerCorps) args[0], (int[]) args[1], supplyItems, (CorpsUtil.SequenceType) args[2]);
  }

  public override IEnumerator onInitSceneAsync()
  {
    CorpsQuestSupplyEditScene questSupplyEditScene = this;
    questSupplyEditScene.menu = questSupplyEditScene.menuBase as CorpsQuestSupplyEditMenu;
    IEnumerator e = questSupplyEditScene.menu.ResourceLoad();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(
    PlayerCorps corps,
    int[] selectedUnitIds,
    SupplyItem[] supplyItems,
    CorpsUtil.SequenceType type)
  {
    CorpsQuestSupplyEditScene questSupplyEditScene = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    yield return (object) null;
    MasterData.CorpsSetting.TryGetValue(corps.corps_id, out questSupplyEditScene.setting);
    if (supplyItems == null)
      supplyItems = SupplyItem.Merge(SMManager.Get<PlayerItem[]>().AllSupplies(), corps.supplies);
    IEnumerator e = questSupplyEditScene.menu.InitializeAsync(corps, selectedUnitIds, supplyItems, type);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (questSupplyEditScene.setting != null && !string.IsNullOrEmpty(questSupplyEditScene.setting.bgm_file))
    {
      questSupplyEditScene.bgmFile = questSupplyEditScene.setting.bgm_file;
      questSupplyEditScene.bgmName = questSupplyEditScene.setting.bgm_name;
    }
  }

  public void onStartScene(
    PlayerCorps corps,
    int[] selectedUnitIds,
    SupplyItem[] supplyItems,
    CorpsUtil.SequenceType type)
  {
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }
}
