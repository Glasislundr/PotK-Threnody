// Decompiled with JetBrains decompiler
// Type: StoryEffectControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class StoryEffectControl : MonoBehaviour
{
  private GameObject prefab_;
  private int id_;
  private StoryEffectBase effect_;
  private bool isStart_;
  private bool isSkip_;
  private bool isChange_;
  private int noPattern_;
  private int noColor_;

  private IEnumerator coInitializeLocal(int id, string namePrefab)
  {
    this.id_ = id;
    this.isStart_ = false;
    this.isSkip_ = false;
    if (Object.op_Equality((Object) this.prefab_, (Object) null))
    {
      Future<GameObject> prefabLoad = Singleton<ResourceManager>.GetInstance().Load<GameObject>(string.Format("StoryEffects/{0}/" + namePrefab, (object) this.id_));
      IEnumerator e = prefabLoad.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.prefab_ = prefabLoad.Result;
      prefabLoad = (Future<GameObject>) null;
    }
  }

  public IEnumerator coInitializeEmotion(int id, int noColor)
  {
    StoryEffectControl storyEffectControl = this;
    IEnumerator e = storyEffectControl.coInitializeLocal(id, "StoryEmotionPrefab");
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    StoryEffectEmotion component = storyEffectControl.prefab_.Clone(((Component) storyEffectControl).gameObject.transform).GetComponent<StoryEffectEmotion>();
    component.setColor(noColor);
    storyEffectControl.effect_ = (StoryEffectBase) component;
  }

  public IEnumerator coInitializeEnvironment(int id, int noColor)
  {
    StoryEffectControl storyEffectControl = this;
    IEnumerator e = storyEffectControl.coInitializeLocal(id, "StoryEnvPrefab");
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject gameObject = storyEffectControl.prefab_.Clone(((Component) storyEffectControl).gameObject.transform);
    storyEffectControl.effect_ = (StoryEffectBase) gameObject.GetComponent<StoryEffectEnvironment>();
    storyEffectControl.effect_.setColor(noColor);
  }

  public IEnumerator coInitializeEffect(int id, int noColor)
  {
    StoryEffectControl storyEffectControl = this;
    IEnumerator e = storyEffectControl.coInitializeLocal(id, "StoryEffectPrefab");
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject gameObject = storyEffectControl.prefab_.Clone(((Component) storyEffectControl).gameObject.transform);
    storyEffectControl.effect_ = (StoryEffectBase) gameObject.GetComponent<StoryEffectEffect>();
    storyEffectControl.effect_.setColor(noColor);
  }

  public void startEffect() => this.isStart_ = true;

  public void skipEffect() => this.isSkip_ = true;

  public void changeEffect(int noPattern, int noColor)
  {
    this.isChange_ = true;
    this.noPattern_ = noPattern;
    this.noColor_ = noColor;
  }

  private void Update()
  {
    if (!Object.op_Inequality((Object) this.effect_, (Object) null))
      return;
    if (this.isChange_)
    {
      this.isChange_ = false;
      StoryEffectMulti effect = this.effect_ as StoryEffectMulti;
      if (Object.op_Inequality((Object) effect, (Object) null))
        effect.setPattern(this.noPattern_, this.noColor_);
    }
    if (this.isStart_)
    {
      this.isStart_ = false;
      this.effect_.startEffect();
    }
    if (!this.isSkip_)
      return;
    this.isSkip_ = false;
    this.effect_.skipEffect();
  }
}
