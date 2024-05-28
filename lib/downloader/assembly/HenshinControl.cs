// Decompiled with JetBrains decompiler
// Type: HenshinControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UniLinq;
using UnityEngine;

#nullable disable
public class HenshinControl : MonoBehaviour
{
  private GameObject prefab_;
  private bool isPause_ = true;
  private bool isPlaying_;
  private bool isSkip_;
  private HenshinContainer container_;
  private int id_;
  private GameObject unitBefore_;
  private GameObject unitAfter_;

  public bool isPlaying => this.isPlaying_;

  private void setUnit()
  {
    this.insertObject(this.container_.dirBefore.transform, this.unitBefore_.transform);
    this.insertObject(this.container_.dirAfter.transform, this.unitAfter_.transform);
  }

  private void insertObject(Transform forwardTrans, Transform inTrans)
  {
    Transform[] array = forwardTrans.GetChildren().ToArray<Transform>();
    Vector3 localPosition1 = inTrans.localPosition;
    Quaternion localRotation1 = inTrans.localRotation;
    Vector3 localScale1 = inTrans.localScale;
    inTrans.parent = forwardTrans;
    inTrans.localPosition = localPosition1;
    inTrans.localRotation = localRotation1;
    inTrans.localScale = localScale1;
    foreach (Transform transform in array)
    {
      Vector3 localPosition2 = transform.localPosition;
      Quaternion localRotation2 = transform.localRotation;
      Vector3 localScale2 = transform.localScale;
      transform.parent = inTrans;
      transform.localPosition = localPosition2;
      transform.localRotation = localRotation2;
      transform.localScale = localScale2;
    }
  }

  public IEnumerator coSetUnit(int id, GameObject unitBefore, GameObject unitAfter)
  {
    HenshinControl henshinControl = this;
    henshinControl.id_ = id;
    henshinControl.unitBefore_ = unitBefore;
    henshinControl.unitAfter_ = unitAfter;
    if (Object.op_Equality((Object) henshinControl.prefab_, (Object) null))
    {
      Future<GameObject> prefabLoad = Singleton<ResourceManager>.GetInstance().Load<GameObject>(string.Format("StoryEffects/{0}/StoryHenshinPrefab", (object) henshinControl.id_));
      IEnumerator e = prefabLoad.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      henshinControl.prefab_ = prefabLoad.Result;
      prefabLoad = (Future<GameObject>) null;
    }
    GameObject gameObject = henshinControl.prefab_.Clone(((Component) henshinControl).gameObject.transform);
    henshinControl.container_ = gameObject.GetComponent<HenshinContainer>();
    if (Object.op_Inequality((Object) henshinControl.container_, (Object) null))
    {
      henshinControl.setUnit();
      henshinControl.container_.updateHenshin();
    }
  }

  public void startHenshin() => this.isPause_ = false;

  public void skipHenshin() => this.isSkip_ = true;

  private void Update()
  {
    if (this.isPlaying_ && this.isSkip_)
    {
      this.isSkip_ = false;
      this.container_.skipHenshin();
    }
    if (this.isPause_ || !Object.op_Inequality((Object) this.container_, (Object) null))
      return;
    this.isPlaying_ = this.container_.updateHenshin();
  }

  public void replayHenshin()
  {
    if (!Object.op_Inequality((Object) this.container_, (Object) null))
      return;
    this.isPause_ = false;
    this.isPlaying_ = false;
    this.isSkip_ = false;
    this.container_.resetHenshin();
  }
}
