// Decompiled with JetBrains decompiler
// Type: unit00497Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class unit00497Menu : BackButtonMenuBase
{
  public EffectControllerPrincessEvolutionBase effect;
  [SerializeField]
  private GameObject back_button_;
  private PrincesEvolutionParam sceneParam;
  private GameObject effectPrefab;

  private IEnumerator prefabLoad()
  {
    if (Object.op_Equality((Object) this.effectPrefab, (Object) null))
    {
      Future<GameObject> prefabf = (Future<GameObject>) null;
      switch (this.sceneParam.mode)
      {
        case Unit00499Scene.Mode.AwakeUnit:
          prefabf = new ResourceObject("Prefabs/PrincessEvolution/Awaking_Prefab").Load<GameObject>();
          break;
        case Unit00499Scene.Mode.CommonAwakeUnit:
          prefabf = new ResourceObject("Prefabs/PrincessEvolution/Common_Awaking_Prefab").Load<GameObject>();
          break;
        case Unit00499Scene.Mode.ReincarnationType:
          prefabf = new ResourceObject("Prefabs/PrincessEvolution/Princess_Reincarnation_Type_prefab").Load<GameObject>();
          break;
        case Unit00499Scene.Mode.JobChange:
          prefabf = new ResourceObject("Prefabs/PrincessJobChange/PrincessJobChange").Load<GameObject>();
          break;
        default:
          prefabf = Res.Prefabs.PrincessEvolution.Princess_Evolution_prefab.Load<GameObject>();
          break;
      }
      IEnumerator e = prefabf.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.effectPrefab = prefabf.Result;
      prefabf = (Future<GameObject>) null;
    }
  }

  public IEnumerator Init(PrincesEvolutionParam param)
  {
    this.sceneParam = param;
    IEnumerator e = this.prefabLoad();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.effect = Object.Instantiate<GameObject>(this.effectPrefab).GetComponent<EffectControllerPrincessEvolutionBase>();
    e = this.effect.Initialize(param, this.back_button_);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((Component) this.effect).gameObject.SetActive(true);
  }

  private IEnumerator SkipCurrentAnimation()
  {
    PrincessEvolutionSoundEffect soundManager = this.effect.SoundManager;
    if ((soundManager != null ? (soundManager.result ? 1 : 0) : 0) == 0)
    {
      Singleton<NGSoundManager>.GetInstance().stopSE();
      Time.timeScale = 100f;
      float tempFixedDeltaTime = Time.fixedDeltaTime;
      Time.fixedDeltaTime = tempFixedDeltaTime * 100f;
      while (this.effect.isAnimation)
        yield return (object) this.effect.isAnimation;
      Time.timeScale = 1f;
      Time.fixedDeltaTime = tempFixedDeltaTime;
    }
  }

  public virtual void IbtnBack()
  {
    if (this.sceneParam.mode == Unit00499Scene.Mode.AwakeUnit && this.effect.isAnimation || this.sceneParam.mode == Unit00499Scene.Mode.CommonAwakeUnit && this.effect.isAnimation)
      return;
    if (this.effect.isAnimation)
    {
      this.StartCoroutine(this.SkipCurrentAnimation());
    }
    else
    {
      Singleton<NGSoundManager>.GetInstance().stopSE();
      if (this.sceneParam.isPreview)
      {
        Singleton<NGSceneManager>.GetInstance().backScene();
        Singleton<PopupManager>.GetInstance().dismiss();
      }
      else
      {
        switch (this.sceneParam.mode)
        {
          case Unit00499Scene.Mode.ReincarnationType:
            Unit004ReincarnationTypeCompleteScene.changeScene(false, this.sceneParam.baseUnit, this.sceneParam.resultUnit);
            break;
          case Unit00499Scene.Mode.JobChange:
            Unit00498Scene.changeSceneByJobChange(false, this.sceneParam.baseUnit, this.sceneParam.resultUnit);
            break;
          default:
            Singleton<NGSceneManager>.GetInstance().changeScene("unit004_9_8", false, (object) this.sceneParam.baseUnit, (object) this.sceneParam.resultUnit, (object) this.sceneParam.mode, (object) this.sceneParam.fromEarth);
            break;
        }
      }
    }
  }

  public override void onBackButton() => this.IbtnBack();
}
