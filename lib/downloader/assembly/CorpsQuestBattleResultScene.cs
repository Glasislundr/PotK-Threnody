// Decompiled with JetBrains decompiler
// Type: CorpsQuestBattleResultScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class CorpsQuestBattleResultScene : NGSceneBase
{
  [SerializeField]
  private UIButton TouchToNext;
  private Queue<ResultMenuBase> Sequences;
  private Queue<ResultMenuBase> TrushSeq;
  private ResultMenuBase PlayingMenu;
  private PlayerCorps Progress;
  private bool ToNextDirty;
  private bool IsBoss;

  public static void ChangeScene(BattleInfo info, WebAPI.Response.QuestCorpsBattleFinish result)
  {
    NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
    instance.clearStack();
    instance.destroyCurrentScene();
    instance.destroyLoadedScenes();
    instance.changeScene("CorpsQuest_battle_result", false, (object) info, (object) result);
  }

  public IEnumerator onStartSceneAsync(
    BattleInfo info,
    WebAPI.Response.QuestCorpsBattleFinish result)
  {
    ((Component) this.TouchToNext).gameObject.SetActive(false);
    this.Progress = ((IEnumerable<PlayerCorps>) result.player_corps_list).First<PlayerCorps>();
    this.IsBoss = MasterData.CorpsStage[info.stageId].isBoss;
    this.SetupBgm(MasterData.CorpsPeriod[this.Progress.period_id]);
    IEnumerator e = this.SetupBackground(info.stageId);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.SetupResultMenu(info, result);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene(BattleInfo info, WebAPI.Response.QuestCorpsBattleFinish result)
  {
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    this.StartCoroutine(this.RunMenus());
  }

  public override void onEndScene()
  {
    foreach (ResultMenuBase resultMenuBase in this.TrushSeq)
      resultMenuBase.OnRemove();
    Singleton<CommonRoot>.GetInstance().releaseBackground();
  }

  private IEnumerator SetupResultMenu(
    BattleInfo info,
    WebAPI.Response.QuestCorpsBattleFinish result)
  {
    CorpsQuestBattleResultScene battleResultScene = this;
    battleResultScene.Sequences = new Queue<ResultMenuBase>();
    battleResultScene.TrushSeq = new Queue<ResultMenuBase>();
    battleResultScene.Sequences.Enqueue((ResultMenuBase) ((Component) battleResultScene).GetComponent<CorpsQuestResultMenu>());
    battleResultScene.Sequences.Enqueue((ResultMenuBase) ((Component) battleResultScene).GetComponent<BattleUI05RewardMenu>());
    if (result.stage_clear_rewards.Length != 0)
      battleResultScene.Sequences.Enqueue((ResultMenuBase) ((Component) battleResultScene).GetComponent<BattleUI05ClearBonusMenu>());
    foreach (ResultMenuBase sequence in battleResultScene.Sequences)
    {
      IEnumerator e = sequence.Init(info, result);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private IEnumerator RunMenus()
  {
    while (this.Sequences.Count > 0)
    {
      this.PlayingMenu = this.Sequences.Dequeue();
      ((Component) this.TouchToNext).gameObject.SetActive(true);
      IEnumerator e = this.PlayingMenu.Run();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.ToNextDirty = false;
      while (!this.ToNextDirty && !this.PlayingMenu.isSkip)
        yield return (object) null;
      this.ToNextDirty = false;
      e = this.PlayingMenu.OnFinish();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.TrushSeq.Enqueue(this.PlayingMenu);
    }
    this.PlayingMenu = (ResultMenuBase) null;
    ((Component) this.TouchToNext).gameObject.SetActive(true);
    ((UIButtonColor) this.TouchToNext).isEnabled = false;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    CorpsQuestTopScene.ChangeScene(this.Progress.period_id, this.IsBoss);
  }

  private IEnumerator SetupBackground(int stageId)
  {
    CorpsQuestBattleResultScene battleResultScene = this;
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
          battleResultScene.backgroundPrefab = result;
          bgF = (Future<GameObject>) null;
        }
      }
    }
  }

  private void SetupBgm(CorpsPeriod period)
  {
    CorpsSetting setting = period.setting;
    if (string.IsNullOrEmpty(setting.bgm_file))
      return;
    this.bgmFile = setting.bgm_file;
    this.bgmName = setting.bgm_name;
  }

  public void OnTouchToNext()
  {
    if (this.IsPush)
      return;
    this.IsPush = true;
    this.ToNextDirty = true;
    if (Object.op_Inequality((Object) this.PlayingMenu, (Object) null))
      this.PlayingMenu.isSkip = true;
    this.StartCoroutine(this.PushOff());
  }

  private IEnumerator PushOff()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    CorpsQuestBattleResultScene battleResultScene = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      battleResultScene.IsPush = false;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.1f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }
}
