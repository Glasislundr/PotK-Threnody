// Decompiled with JetBrains decompiler
// Type: Quest00214Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class Quest00214Scene : NGSceneBase
{
  [SerializeField]
  private Quest00214Menu menu;
  [SerializeField]
  private Quest00214aMenu subMenu;
  private bool isInit = true;

  public static void ChangeScene(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_14", stack);
  }

  public static void ChangeScene(bool stack, int unitId)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_14", (stack ? 1 : 0) != 0, (object) unitId, (object) false, (object) false);
  }

  public static void ChangeScene(bool stack, int unitOrQuestId, bool is_change_combi)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_14", (stack ? 1 : 0) != 0, (object) unitOrQuestId, (object) is_change_combi, (object) false);
  }

  public static void ChangeScene(bool stack, int unitId, bool is_change_combi, bool isSameUnit)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_14", (stack ? 1 : 0) != 0, (object) unitId, (object) is_change_combi, (object) isSameUnit);
  }

  private IEnumerator onStartSceneAsyncForCharacter(
    int? unitId,
    bool is_change_combi,
    bool isSameUnit)
  {
    Quest00214Scene quest00214Scene = this;
    if (quest00214Scene.isInit)
    {
      quest00214Scene.StartCoroutine(quest00214Scene.menu.doLoadResources());
      yield return (object) ServerTime.WaitSync();
      Future<WebAPI.Response.QuestProgressCharacter> apiF = WebAPI.QuestProgressCharacter((Action<WebAPI.Response.UserError>) (error =>
      {
        WebAPI.DefaultUserErrorCallback(error);
        MypageScene.ChangeSceneOnError();
      }));
      yield return (object) apiF.Wait();
      if (apiF.Result != null)
      {
        quest00214Scene.menu.apiResponse = apiF.Result;
        WebAPI.SetLatestResponsedAt("QuestProgressCharacter");
        WebAPI.SetLatestResponsedAt("QuestProgressHarmony");
        LambdaEqualityComparer<UnitUnit> comparer = new LambdaEqualityComparer<UnitUnit>((Func<UnitUnit, UnitUnit, bool>) ((a, b) => a.resource_reference_unit_id_UnitUnit == b.resource_reference_unit_id_UnitUnit));
        List<string> paths = new List<string>();
        QuestCharacterS[] array1 = ((IEnumerable<QuestCharacterS>) MasterData.QuestCharacterSList).Where<QuestCharacterS>((Func<QuestCharacterS, bool>) (x => QuestCharacterS.CheckIsReleased(x.start_at))).ToArray<QuestCharacterS>();
        foreach (UnitUnit unitUnit in ((IEnumerable<QuestCharacterS>) array1).Select<QuestCharacterS, int>((Func<QuestCharacterS, int>) (y => y.unit_UnitUnit)).Distinct<int>().Select<int, UnitUnit>((Func<int, UnitUnit>) (n => MasterData.UnitUnit[n])).Distinct<UnitUnit>((IEqualityComparer<UnitUnit>) comparer))
          paths.AddRange((IEnumerable<string>) unitUnit.GetUIResourcePaths());
        QuestHarmonyS[] array2 = ((IEnumerable<QuestHarmonyS>) MasterData.QuestHarmonySList).Where<QuestHarmonyS>((Func<QuestHarmonyS, bool>) (x => QuestCharacterS.CheckIsReleased(x.start_at))).ToArray<QuestHarmonyS>();
        // ISSUE: reference to a compiler-generated method
        foreach (UnitUnit unitUnit in ((IEnumerable<QuestHarmonyS>) array2).SelectMany<QuestHarmonyS, int>(new Func<QuestHarmonyS, IEnumerable<int>>(quest00214Scene.\u003ConStartSceneAsyncForCharacter\u003Eb__7_11)).Distinct<int>().Select<int, UnitUnit>((Func<int, UnitUnit>) (n => MasterData.UnitUnit[n])).Distinct<UnitUnit>((IEqualityComparer<UnitUnit>) comparer))
          paths.AddRange((IEnumerable<string>) unitUnit.GetUIResourcePaths());
        string background_path = Consts.GetInstance().BACKGROUND_BASE_PATH;
        paths.AddRange(((IEnumerable<QuestCharacterS>) array1).Select<QuestCharacterS, int>((Func<QuestCharacterS, int>) (x => x.quest_m_QuestCharacterM)).Distinct<int>().Select<int, int>((Func<int, int>) (n => MasterData.QuestCharacterM[n].background_QuestCommonBackground)).Concat<int>(((IEnumerable<QuestHarmonyS>) array2).Select<QuestHarmonyS, int>((Func<QuestHarmonyS, int>) (x => x.quest_m_QuestHarmonyM)).Distinct<int>().Select<int, int>((Func<int, int>) (n => MasterData.QuestHarmonyM[n].background_QuestCommonBackground))).Distinct<int>().Select<int, string>((Func<int, string>) (i => string.Format(background_path, (object) MasterData.QuestCommonBackground[i].background_name))));
        yield return (object) OnDemandDownload.waitLoadSomethingResource((IEnumerable<string>) paths, false);
        yield return (object) quest00214Scene.menu.InitCharacterQuestButton(unitId, is_change_combi, isSameUnit);
        apiF = (Future<WebAPI.Response.QuestProgressCharacter>) null;
      }
    }
  }

  private List<int> getUnits(QuestHarmonyS quest)
  {
    List<int> units = new List<int>()
    {
      quest.unit_UnitUnit,
      quest.target_unit_UnitUnit
    };
    if (quest.target_unit2_UnitUnit.HasValue && quest.target_unit2_UnitUnit.Value != 0)
      units.Add(quest.target_unit2_UnitUnit.Value);
    return units;
  }

  public IEnumerator onStartSceneAsync()
  {
    this.preStartScene();
    yield return (object) this.onStartSceneAsyncForCharacter(new int?(), false, false);
  }

  public void onStartScene()
  {
    if (this.isInit)
    {
      this.menu.InitializeEnd();
      this.isInit = false;
    }
    this.postStartScene();
  }

  public IEnumerator onStartSceneAsync(int unitId, bool isCombiQuest, bool isSameUnit)
  {
    this.preStartScene();
    yield return (object) this.onStartSceneAsyncForCharacter(new int?(unitId), isCombiQuest, isSameUnit);
  }

  private void preStartScene()
  {
    if (this.menuBases == null)
      this.menuBases = new NGMenuBase[2]
      {
        (NGMenuBase) this.menu,
        (NGMenuBase) this.subMenu
      };
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(4);
  }

  private void postStartScene()
  {
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    this.StartCoroutine(this.doPostStartScene());
  }

  private IEnumerator doPostStartScene()
  {
    yield return (object) null;
    this.menu.preLoadIcons();
  }

  public void onStartScene(int unitId, bool isCombiQuest, bool isSameUnit)
  {
    if (this.isInit)
    {
      this.menu.InitializeEnd();
      this.isInit = false;
    }
    this.postStartScene();
  }
}
