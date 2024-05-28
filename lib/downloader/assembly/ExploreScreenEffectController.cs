// Decompiled with JetBrains decompiler
// Type: ExploreScreenEffectController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (ExploreSceneManager))]
public class ExploreScreenEffectController : MonoBehaviour
{
  [SerializeField]
  private Explore033TaskTimeCounter mWaitTimeCounter;
  [SerializeField]
  private Explore033TaskTimeCounter mLostWaitTimeCounter;
  private Animator mTransition2d;
  private Animator mTransitionFull;
  private GameObject mItemGetEffectPrefab;
  private Explore033TreasureEffect mItemGetEffectObj;
  private GameObject mNextFloorPrefab;

  public void OpenWaitTimeCounter(long millisec) => this.mWaitTimeCounter.StartCounter(millisec);

  public void OpenLostWaitTimeCounter(long millisec)
  {
    this.mLostWaitTimeCounter.StartCounter(millisec);
  }

  public void CloseTimeCounter()
  {
    ((Component) this.mWaitTimeCounter).gameObject.SetActive(false);
    ((Component) this.mLostWaitTimeCounter).gameObject.SetActive(false);
  }

  public void SetTransitionObject(GameObject transition)
  {
    this.mTransition2d = transition.GetComponent<Animator>();
    ((Component) this.mTransition2d).GetComponent<ExploreTransitionAnimation>().IsExploreOnly = true;
  }

  public void Transition2dIn() => this.mTransition2d.Play("explore_transition_in_anim");

  public void Transition2dOut() => this.mTransition2d.Play("explore_transition_out_anim");

  public IEnumerator WaitForTransition2d()
  {
    AnimatorStateInfo animatorStateInfo;
    do
    {
      yield return (object) null;
      animatorStateInfo = this.mTransition2d.GetCurrentAnimatorStateInfo(0);
    }
    while ((double) ((AnimatorStateInfo) ref animatorStateInfo).normalizedTime < 1.0);
  }

  public void SetTransitionFullObject(GameObject transition)
  {
    this.mTransitionFull = transition.GetComponent<Animator>();
    ((Component) this.mTransitionFull).GetComponent<ExploreTransitionAnimation>().IsExploreOnly = true;
  }

  public void TransitionFullIn() => this.mTransitionFull.Play("explore_transition_in_anim");

  public void TransitionFullOut() => this.mTransitionFull.Play("explore_transition_out_anim");

  public IEnumerator WaitForTransitionFull()
  {
    AnimatorStateInfo animatorStateInfo;
    do
    {
      yield return (object) null;
      animatorStateInfo = this.mTransitionFull.GetCurrentAnimatorStateInfo(0);
    }
    while ((double) ((AnimatorStateInfo) ref animatorStateInfo).normalizedTime < 1.0);
  }

  public IEnumerator LoadItemGetEffect()
  {
    if (Object.op_Equality((Object) this.mItemGetEffectPrefab, (Object) null))
    {
      Future<GameObject> loader = new ResourceObject("Prefabs/explore033_Top/explore_ItemGet").Load<GameObject>();
      IEnumerator e = loader.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.mItemGetEffectPrefab = loader.Result;
      loader = (Future<GameObject>) null;
    }
  }

  public IEnumerator OpenItemGetEffect(ExploreDropReward reward)
  {
    this.mItemGetEffectObj = Singleton<PopupManager>.GetInstance().open(this.mItemGetEffectPrefab, isNonOpenAnime: true).GetComponent<Explore033TreasureEffect>();
    Vector3 boxButtonWorldPos = Singleton<ExploreSceneManager>.GetInstance().Footer.GetExploreBoxButtonWorldPos();
    yield return (object) this.mItemGetEffectObj.InitAsync(reward, boxButtonWorldPos);
  }

  public void CloseItemGetEffect()
  {
    this.mItemGetEffectObj = (Explore033TreasureEffect) null;
    Singleton<PopupManager>.GetInstance().dismissWithoutAnim();
  }

  public IEnumerator LoadNextFloorEffect()
  {
    if (Object.op_Equality((Object) this.mNextFloorPrefab, (Object) null))
    {
      Future<GameObject> loader = new ResourceObject("Prefabs/explore033_Top/explore_NextFloor").Load<GameObject>();
      IEnumerator e = loader.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.mNextFloorPrefab = loader.Result;
      loader = (Future<GameObject>) null;
    }
  }

  public void StartInfoEffect() => this.StartCoroutine(this.OpenInfoEffect());

  public IEnumerator OpenInfoEffect()
  {
    if (Singleton<ExploreDataManager>.GetInstance().IsNextFloor)
    {
      Singleton<ExploreDataManager>.GetInstance().IsNextFloor = false;
      yield return (object) this.OpenNextFloorEffect();
    }
  }

  public IEnumerator OpenNextFloorEffect()
  {
    GameObject prefab = this.mNextFloorPrefab.Clone();
    prefab.SetActive(false);
    ExploreNextFloorAnim exploreNextFloorAnim = prefab.GetComponent<ExploreNextFloorAnim>();
    ExploreFloor floorData = Singleton<ExploreDataManager>.GetInstance().FloorData;
    exploreNextFloorAnim.Initialize(floorData.name, floorData.floor);
    prefab.SetActive(true);
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
    while (!exploreNextFloorAnim.animEnd)
      yield return (object) null;
    exploreNextFloorAnim.Close();
    ExploreFloorRewardPopupSequence floorRewardPopupSeq = new ExploreFloorRewardPopupSequence(true);
    yield return (object) floorRewardPopupSeq.Init(floorData);
    yield return (object) floorRewardPopupSeq.Run();
  }
}
