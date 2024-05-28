// Decompiled with JetBrains decompiler
// Type: StoryEffectMulti
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class StoryEffectMulti : StoryEffectBase
{
  [SerializeField]
  [Tooltip("patterns_を使う")]
  private bool isMultiPattern_;
  [SerializeField]
  [Tooltip("初期設定パターン")]
  private int currentPattern_;
  [SerializeField]
  protected StoryEffectMulti.Pattern[] patterns_;

  protected bool isMultiPattern => this.isMultiPattern_;

  protected int currentPatternIndex => this.currentPattern_ - 1;

  protected Dictionary<string, ParticleSystem> defaultParticles_ { get; private set; }

  protected Dictionary<string, StoryEffectBase.ParticleControl> dicParticles_ { get; private set; }

  protected override void awakeLocal()
  {
    base.awakeLocal();
    if (this.patterns_ == null || this.patterns_.Length == 0)
      this.isMultiPattern_ = false;
    if (!this.isMultiPattern || this.particles_ == null || this.particles_.Length == 0)
      return;
    this.defaultParticles_ = new Dictionary<string, ParticleSystem>();
    this.dicParticles_ = new Dictionary<string, StoryEffectBase.ParticleControl>();
    foreach (StoryEffectBase.ParticleControl particle in this.particles_)
    {
      if (!string.IsNullOrEmpty(particle.name_))
      {
        if (this.defaultParticles_.ContainsKey(particle.name_))
        {
          Debug.LogError((object) string.Format("StoryEffectMulti.particles_[].name_=\"{0}\" is aleady", (object) particle.name_), (Object) this);
        }
        else
        {
          this.defaultParticles_.Add(particle.name_, particle.particle_);
          this.dicParticles_.Add(particle.name_, particle);
        }
      }
    }
  }

  private void OnDestroy()
  {
    this.defaultParticles_ = (Dictionary<string, ParticleSystem>) null;
    this.dicParticles_ = (Dictionary<string, StoryEffectBase.ParticleControl>) null;
  }

  public void resetParticlePattern()
  {
    if (!this.isMultiPattern_ || this.defaultParticles_ == null || !this.defaultParticles_.Any<KeyValuePair<string, ParticleSystem>>())
      return;
    foreach (KeyValuePair<string, ParticleSystem> defaultParticle in this.defaultParticles_)
      this.dicParticles_[defaultParticle.Key].particle_ = defaultParticle.Value;
  }

  public void setPattern(int noPattern, int noColor)
  {
    if (!this.isMultiPattern_)
      return;
    this.currentPattern_ = noPattern;
    int currentPatternIndex = this.currentPatternIndex;
    if (currentPatternIndex < 0 || currentPatternIndex >= this.patterns_.Length)
      return;
    ((IEnumerable<StoryEffectMulti.Pattern>) this.patterns_).Select<StoryEffectMulti.Pattern, GameObject>((Func<StoryEffectMulti.Pattern, GameObject>) (p => p.topObject_)).ToggleOnce(currentPatternIndex);
    StoryEffectMulti.Pattern pattern = this.patterns_[currentPatternIndex];
    if (pattern.particles_ == null || pattern.particles_.Length == 0)
      return;
    foreach (StoryEffectMulti.KeyParticle particle in pattern.particles_)
    {
      if (!Object.op_Equality((Object) particle.particle_, (Object) null) && !string.IsNullOrEmpty(particle.key_) && this.dicParticles_.ContainsKey(particle.key_))
      {
        StoryEffectBase.ParticleControl dicParticle = this.dicParticles_[particle.key_];
        dicParticle.particle_ = particle.particle_;
        dicParticle.setColor(noColor);
      }
    }
  }

  [Serializable]
  protected class KeyParticle
  {
    public string key_;
    public ParticleSystem particle_;
  }

  [Serializable]
  protected class Pattern
  {
    public GameObject topObject_;
    public StoryEffectMulti.KeyParticle[] particles_;
  }
}
