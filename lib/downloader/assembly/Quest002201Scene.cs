// Decompiled with JetBrains decompiler
// Type: Quest002201Scene
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
[AddComponentMenu("Scenes/QuestExtra/UnityValueUpSoloQuestScene")]
public class Quest002201Scene : NGSceneBase
{
  public static readonly string defName = "quest002_20_1";
  private bool isInitialize_ = true;
  private bool isInitializeBG_ = true;
  private int Lid_;
  private int Mid_;
  private Modified<PlayerUnit[]> modifiedPlayerUnits_;
  [SerializeField]
  private QuestExtraHeadline headline_;
  [SerializeField]
  private Quest002201Menu menu_;

  public static void changeScene(bool bStack, int Lid, int Mid)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene(Quest002201Scene.defName, (bStack ? 1 : 0) != 0, (object) Lid, (object) Mid);
  }

  public IEnumerator onStartSceneAsync(int Lid, int Mid, int lastReferenceUnitId)
  {
    if (this.isInitialize_)
    {
      if (lastReferenceUnitId == -1)
        lastReferenceUnitId = Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitID;
      this.menu_.setLastReference(lastReferenceUnitId);
    }
    yield return (object) this.onStartSceneAsync(Lid, Mid);
  }

  public void onStartScene(int Lid, int Mid, int lastReferenceUnitId)
  {
    this.onStartScene(Lid, Mid);
  }

  public IEnumerator onStartSceneAsync(int Lid, int Mid)
  {
    Quest002201Scene quest002201Scene = this;
    bool bReset = false;
    CommonRoot cRoot = Singleton<CommonRoot>.GetInstance();
    quest002201Scene.isInitializeBG_ = true;
    quest002201Scene.StartCoroutine(quest002201Scene.doLoadBackground(Lid));
    if (!WebAPI.IsResponsedAtRecent("QuestProgressExtra") && !WebAPI.IsResponsedAtRecent("QuestProgressLimited"))
    {
      cRoot.ShowLoadingLayer(4);
      Future<WebAPI.Response.QuestProgressLimited> qProgress = WebAPI.QuestProgressLimited((Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      }));
      yield return (object) qProgress.Wait();
      if (qProgress.Result == null)
        yield return (object) quest002201Scene.waitErrorInit();
      quest002201Scene.isInitialize_ = true;
      bReset = true;
      WebAPI.SetLatestResponsedAt("QuestProgressLimited");
      qProgress = (Future<WebAPI.Response.QuestProgressLimited>) null;
    }
    quest002201Scene.checkInitialize(Lid, Mid);
    if (quest002201Scene.isInitialize_)
    {
      quest002201Scene.modifiedPlayerUnits_ = SMManager.Observe<PlayerUnit[]>();
      quest002201Scene.modifiedPlayerUnits_.NotifyChanged();
      if (!bReset)
        cRoot.ShowLoadingLayer(4);
      yield return (object) quest002201Scene.headline_.doInitialize(MasterData.QuestExtraL[Lid]);
      yield return (object) quest002201Scene.menu_.doInitialize(Lid, Mid, bReset);
    }
    else
    {
      if (!bReset)
        cRoot.ShowLoadingLayer(0);
      yield return (object) quest002201Scene.menu_.UpdateInfoAndScroll(quest002201Scene.menu_.getPlayerUnits(quest002201Scene.modifiedPlayerUnits_.IsChangedOnce()), (PlayerMaterialUnit[]) null);
    }
    while (quest002201Scene.isInitializeBG_)
      yield return (object) null;
  }

  public void onStartScene(int Lid, int Mid)
  {
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    this.isInitialize_ = false;
    this.autoOpenPopup();
  }

  private void checkInitialize(int Lid, int Mid)
  {
    if (!this.isInitialize_ && (this.Lid_ != Lid || this.Mid_ != Mid))
      this.isInitialize_ = true;
    this.Lid_ = Lid;
    this.Mid_ = Mid;
  }

  private IEnumerator doLoadBackground(int Lid)
  {
    Quest002201Scene quest002201Scene = this;
    QuestExtraL extraL;
    if (!MasterData.QuestExtraL.TryGetValue(Lid, out extraL))
    {
      quest002201Scene.isInitializeBG_ = false;
    }
    else
    {
      yield return (object) null;
      yield return (object) ((Component) quest002201Scene).gameObject.GetOrAddComponent<BGChange>().ExtraBGprefabCreate(extraL.background_image_name);
      quest002201Scene.isInitializeBG_ = false;
    }
  }

  private IEnumerator waitErrorInit()
  {
    Singleton<NGSceneManager>.GetInstance().ChangeErrorPage();
    while (true)
      yield return (object) null;
  }

  private void autoOpenPopup()
  {
    NGGameDataManager instance = Singleton<NGGameDataManager>.GetInstance();
    if (instance.fromPopup != NGGameDataManager.FromPopup.Quest002201Scene)
      return;
    instance.OnceOpenPopup<Future<GameObject>>(this.menu_.unityDetailPrefabs, (NGMenuBase) this.menu_, new Action(this.menu_.preOpenUnityPopup));
    instance.fromPopup = NGGameDataManager.FromPopup.None;
  }
}
