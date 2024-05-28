// Decompiled with JetBrains decompiler
// Type: SEASkillCutin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SEASkillCutin : MonoBehaviour
{
  [SerializeField]
  private Animator animator;
  [SerializeField]
  private SpriteRenderer[] spriteShadow;
  [SerializeField]
  private SpriteRenderer[] spriteCharacter;
  [SerializeField]
  private string[] SoundEffectName;
  [SerializeField]
  private float[] SEDelay;
  [SerializeField]
  private Vector4[] BgTexRoopSpeed;
  [SerializeField]
  private SpriteRenderer[] BG_Tex_Roop;

  public static IEnumerator Show(Transform parent, PlayerUnit unit, int settingSpeed)
  {
    bool isTouchBlockLog = Singleton<CommonRoot>.GetInstance().isTouchBlock;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    if (unit.cache_overkillers_unit_ids != null && unit.cache_overkillers_unit_ids.Length != 0 && unit.cache_overkillers_unit_ids.Length == unit.cache_overkillers_unit_job_ids.Length)
    {
      UnitUnit[] killersArray = new UnitUnit[unit.cache_overkillers_unit_ids.Length];
      for (int index = 0; index < unit.cache_overkillers_unit_ids.Length; ++index)
        MasterData.UnitUnit.TryGetValue(unit.cache_overkillers_unit_ids[index], out killersArray[index]);
      List<Sprite> sprites = new List<Sprite>();
      IEnumerator e;
      for (int i = 0; i < killersArray.Length; ++i)
      {
        Future<Sprite> spriteF = killersArray[i].LoadSpriteLarge(unit.cache_overkillers_unit_job_ids[i], 100f);
        e = spriteF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        sprites.Add(spriteF.Result);
        spriteF = (Future<Sprite>) null;
      }
      Future<GameObject> prefabF = new ResourceObject("Animations/battle_cutin/SEAUnitSkill/battle_cutin_SEA_{0}".F((object) killersArray.Length)).Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject obj = prefabF.Result.Clone(parent);
      SEASkillCutin script = obj.GetComponent<SEASkillCutin>();
      e = script.Initialize(sprites, settingSpeed);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      yield return (object) new WaitForAnimation(script.animator);
      yield return (object) new WaitForSeconds(0.25f * (float) settingSpeed);
      foreach (Object @object in sprites)
        Object.Destroy(@object);
      Object.Destroy((Object) obj);
      if (!isTouchBlockLog)
        Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
    }
  }

  public IEnumerator Initialize(List<Sprite> sprites, int settingSpeed)
  {
    // ISSUE: reference to a compiler-generated field
    int num1 = this.\u003C\u003E1__state;
    SEASkillCutin seaSkillCutin = this;
    if (num1 != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    seaSkillCutin.animator.speed = 0.0f;
    for (int index = 0; index < sprites.Count; ++index)
    {
      seaSkillCutin.spriteShadow[index].sprite = sprites[index];
      seaSkillCutin.spriteCharacter[index].sprite = sprites[index];
    }
    seaSkillCutin.animator.speed = 1f / (float) settingSpeed;
    ParticleSystem[] componentsInChildren = ((Component) seaSkillCutin).GetComponentsInChildren<ParticleSystem>(true);
    float num2;
    switch (settingSpeed)
    {
      case 1:
        num2 = 1f;
        break;
      case 2:
        num2 = 0.8f;
        break;
      case 3:
        num2 = 0.55f;
        break;
      default:
        num2 = 0.5f;
        break;
    }
    foreach (ParticleSystem particleSystem in componentsInChildren)
    {
      ParticleSystem.MainModule main = particleSystem.main;
      string name = ((Object) ((Component) particleSystem).gameObject).name;
      if (name.Equals("eff_Sparks") || name.Equals("eff_sparks01") || name.Equals("eff_sparks02"))
      {
        ref ParticleSystem.MainModule local = ref main;
        ((ParticleSystem.MainModule) ref local).simulationSpeed = ((ParticleSystem.MainModule) ref local).simulationSpeed * num2;
      }
      else
      {
        ref ParticleSystem.MainModule local = ref main;
        ((ParticleSystem.MainModule) ref local).simulationSpeed = ((ParticleSystem.MainModule) ref local).simulationSpeed / (float) settingSpeed;
      }
    }
    for (int index = 0; index < seaSkillCutin.BG_Tex_Roop.Length; ++index)
    {
      Material material = new Material(((Renderer) seaSkillCutin.BG_Tex_Roop[index]).material);
      Vector4 vector4;
      // ISSUE: explicit constructor call
      ((Vector4) ref vector4).\u002Ector(seaSkillCutin.BgTexRoopSpeed[index].x / (float) settingSpeed, seaSkillCutin.BgTexRoopSpeed[index].y / (float) settingSpeed);
      material.SetVector("_UVScroll", vector4);
      ((Renderer) seaSkillCutin.BG_Tex_Roop[index]).material = material;
    }
    seaSkillCutin.StartCoroutine(seaSkillCutin.PlaySE(seaSkillCutin.SoundEffectName[0], seaSkillCutin.SEDelay[0] * (float) settingSpeed));
    seaSkillCutin.StartCoroutine(seaSkillCutin.PlaySE(seaSkillCutin.SoundEffectName[1], seaSkillCutin.SEDelay[1] * (float) settingSpeed));
    return false;
  }

  private IEnumerator PlaySE(string SoundEffectName, float delayTime = 0.0f)
  {
    NGSoundManager sm = Singleton<NGSoundManager>.GetInstance();
    if (!Object.op_Equality((Object) null, (Object) sm) && !string.IsNullOrEmpty(SoundEffectName))
    {
      if ((double) delayTime > 0.0)
        yield return (object) new WaitForSeconds(delayTime);
      sm.playSE(SoundEffectName);
    }
  }
}
