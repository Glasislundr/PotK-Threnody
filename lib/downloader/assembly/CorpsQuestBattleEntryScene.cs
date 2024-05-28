// Decompiled with JetBrains decompiler
// Type: CorpsQuestBattleEntryScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/CorpsQuest/BattleEntryScene")]
public class CorpsQuestBattleEntryScene : NGSceneBase
{
  [SerializeField]
  private GameObject bg;
  private CorpsQuestBattleEntryMenu menu;
  private CorpsSetting setting;
  private Modified<PlayerUnit[]> mPlayerUnitObserver;

  public static void ChangeScene(int periodId, int stageId)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("CorpsQuest_battle_entry", true, (object) periodId, (object) stageId);
  }

  public override IEnumerator onInitSceneAsync()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    CorpsQuestBattleEntryScene battleEntryScene = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    battleEntryScene.menu = ((Component) battleEntryScene.menuBase).GetComponent<CorpsQuestBattleEntryMenu>();
    battleEntryScene.mPlayerUnitObserver = new Modified<PlayerUnit[]>(0L);
    battleEntryScene.mPlayerUnitObserver.Commit();
    return false;
  }

  public IEnumerator onStartSceneAsync(int periodId, int stageId)
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    yield return (object) null;
    IEnumerator e = this.Initialize(periodId, stageId);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onBackSceneAsync(int periodId, int stageId)
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    yield return (object) null;
    IEnumerator e1;
    if (this.mPlayerUnitObserver.IsChangedOnce())
    {
      Future<WebAPI.Response.QuestCorpsTop> f = WebAPI.QuestCorpsTop(periodId, (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
      e1 = f.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (f.Result == null)
      {
        Singleton<NGSceneManager>.GetInstance().ChangeErrorPage();
        yield return (object) null;
      }
      f = (Future<WebAPI.Response.QuestCorpsTop>) null;
    }
    e1 = this.Initialize(periodId, stageId);
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
  }

  private IEnumerator Initialize(int periodId, int stageId)
  {
    CorpsQuestBattleEntryScene battleEntryScene = this;
    CorpsPeriod corpsPeriod;
    if (MasterData.CorpsPeriod.TryGetValue(periodId, out corpsPeriod))
      battleEntryScene.setting = corpsPeriod.setting;
    IEnumerator e = battleEntryScene.SetupBackground(stageId);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = battleEntryScene.menu.InitializeAsync(periodId, stageId);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (battleEntryScene.setting != null && !string.IsNullOrEmpty(battleEntryScene.setting.bgm_file))
    {
      battleEntryScene.bgmFile = battleEntryScene.setting.bgm_file;
      battleEntryScene.bgmName = battleEntryScene.setting.bgm_name;
    }
  }

  public void onStartScene(int periodId, int stageId)
  {
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  public override void onEndScene() => Singleton<CommonRoot>.GetInstance().releaseBackground();

  private IEnumerator SetupBackground(int stageId)
  {
    CorpsQuestBattleEntryScene battleEntryScene = this;
    CorpsStage corpsStage;
    if (MasterData.CorpsStage.TryGetValue(stageId, out corpsStage))
    {
      CorpsStageBackground backGround = corpsStage.back_ground;
      if (backGround != null)
      {
        string path = Consts.GetInstance().DEFULAT_BACKGROUND;
        if (!string.IsNullOrEmpty(backGround.background_name))
          path = string.Format(Consts.GetInstance().BACKGROUND_BASE_PATH, (object) backGround.background_name);
        Future<Sprite> bgSpriteF = Singleton<ResourceManager>.GetInstance().LoadOrNull<Sprite>(path);
        IEnumerator e = bgSpriteF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (Object.op_Inequality((Object) bgSpriteF.Result, (Object) null))
        {
          Future<GameObject> bgF = Res.Prefabs.BackGround.SortieBackGround.Load<GameObject>();
          e = bgF.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          GameObject result = bgF.Result;
          UI2DSprite component = result.GetComponent<UI2DSprite>();
          component.sprite2D = bgSpriteF.Result;
          Rect textureRect = bgSpriteF.Result.textureRect;
          ((UIWidget) component).width = Mathf.FloorToInt(((Rect) ref textureRect).width);
          textureRect = bgSpriteF.Result.textureRect;
          ((UIWidget) component).height = Mathf.FloorToInt(((Rect) ref textureRect).height);
          battleEntryScene.backgroundPrefab = result;
          bgF = (Future<GameObject>) null;
        }
      }
    }
  }
}
