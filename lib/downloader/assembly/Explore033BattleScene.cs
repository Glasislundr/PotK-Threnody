// Decompiled with JetBrains decompiler
// Type: Explore033BattleScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Explore033BattleScene : NGSceneBase
{
  [SerializeField]
  private NGDuelManager mDuelManager;
  private Explore033BattleMenu menu;
  private Color mOriginalAmbient;
  private GameObject mNonDuelDirectionalLight;
  private bool is_initial_scene;

  public static void changeScene(bool stack, DuelResult duelResult, DuelEnvironment duelEnv)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("explore033_Encount", (stack ? 1 : 0) != 0, (object) duelResult, (object) duelEnv);
  }

  public override IEnumerator onInitSceneAsync()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Explore033BattleScene explore033BattleScene = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    explore033BattleScene.menu = explore033BattleScene.menuBase as Explore033BattleMenu;
    explore033BattleScene.mOriginalAmbient = RenderSettings.ambientLight;
    explore033BattleScene.mNonDuelDirectionalLight = GameObject.Find("Directional light");
    Singleton<ExploreSceneManager>.GetInstance().Model.SetBattleMapMode(true);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) explore033BattleScene.menu.InitAsync();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator onStartSceneAsync(DuelResult duelResult, DuelEnvironment duelEnv)
  {
    ((Component) this.mDuelManager).gameObject.SetActive(true);
    if (this.is_initial_scene)
    {
      this.mDuelManager.ResetLight();
    }
    else
    {
      RenderSettings.ambientLight = new Color(1f, 1f, 1f);
      if (Object.op_Inequality((Object) this.mNonDuelDirectionalLight, (Object) null))
        this.mNonDuelDirectionalLight.SetActive(false);
      IEnumerator e = this.mDuelManager.Initialize(duelResult, duelEnv, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      yield return (object) this.menu.onStartSceneAsync(duelResult);
      this.is_initial_scene = true;
    }
  }

  public void onStartScene(DuelResult duelResult, DuelEnvironment duelEnv)
  {
    Time.timeScale = 1.5f;
    this.menu.onStartScene();
    this.mDuelManager.StartDuel();
    Singleton<CommonRoot>.GetInstance().SetFooterEnable(true);
  }

  public override void onEndScene()
  {
    Time.timeScale = 1f;
    this.menu.onEndScene();
    this.mDuelManager.mRoot3d.SetActive(false);
    RenderSettings.ambientLight = this.mOriginalAmbient;
    if (Object.op_Inequality((Object) this.mNonDuelDirectionalLight, (Object) null))
      this.mNonDuelDirectionalLight.SetActive(true);
    Singleton<NGSoundManager>.GetInstance().crossFadeCurrentBGM(2.5f, 0.0f);
    Singleton<PopupManager>.GetInstance().dismiss();
    this.is_initial_scene = false;
    Singleton<CommonRoot>.GetInstance().GetNormalHeaderComponent().PlayEntryAnime();
    Singleton<CommonRoot>.GetInstance().SetFooterEnable(true);
    Singleton<CommonRoot>.GetInstance().isActiveBlackBGPanel = false;
    Singleton<ExploreSceneManager>.GetInstance().Pause(true);
  }

  private void OnEnable() => this.mDuelManager.isWait = false;

  private void OnDisable() => this.mDuelManager.isWait = true;

  private void OnApplicationPause(bool pause)
  {
    if (pause)
      return;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    this.StartCoroutine(this.SkipDuel());
  }

  private IEnumerator SkipDuel()
  {
    Explore033BattleScene explore033BattleScene = this;
    if (!explore033BattleScene.mDuelManager.isDuelEnd)
    {
      // ISSUE: reference to a compiler-generated method
      yield return (object) new WaitUntil(new Func<bool>(explore033BattleScene.\u003CSkipDuel\u003Eb__13_0));
    }
  }
}
