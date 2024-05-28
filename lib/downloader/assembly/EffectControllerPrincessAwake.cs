// Decompiled with JetBrains decompiler
// Type: EffectControllerPrincessAwake
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class EffectControllerPrincessAwake : EffectControllerPrincessEvolutionBase
{
  [SerializeField]
  private UI2DSprite unitSpriteBefore;
  [Tooltip("Glowing border effect.")]
  [SerializeField]
  private UI2DSprite unitEffectBefore;
  [Tooltip("Transparent character image.")]
  [SerializeField]
  private UI2DSprite unitEffectBefore2;
  [SerializeField]
  private UI2DSprite unitSpriteAfter;
  [Tooltip("Glowing border effect.")]
  [SerializeField]
  private UI2DSprite unitEffectAfter;
  [Tooltip("Transparent character image.")]
  [SerializeField]
  private UI2DSprite unitEffectAfter2;
  [SerializeField]
  private UI2DSprite unitBlack;
  [SerializeField]
  private UI2DSprite unitGod;
  [SerializeField]
  private UI2DSprite textSprite1;
  [SerializeField]
  private UI2DSprite textSprite2;
  [SerializeField]
  private UI2DSprite textSprite3;
  [Header("Glowing Border Effect Parameters")]
  [SerializeField]
  private Texture2D borderColorTexture;
  [SerializeField]
  [Range(0.0f, 50f)]
  private int borderWidth;
  [SerializeField]
  [Range(0.0f, 1f)]
  private float borderAlphaThreshold;
  [Header("Black Image Effect Parameters")]
  [SerializeField]
  private Color blackMainColor;
  [SerializeField]
  [Range(0.0f, 1f)]
  private float blackMainColorStrength;
  [SerializeField]
  [Range(0.0f, 3f)]
  private float blackBrightness;
  [SerializeField]
  [Range(0.0f, 3f)]
  private float blackColorfulness;
  [SerializeField]
  [Range(0.0f, 3f)]
  private float blackContrast;
  [SerializeField]
  [Range(0.0f, 1f)]
  private float blackBlurStrength;
  [SerializeField]
  [Range(0.0f, 1f)]
  private float blackWhiteStrength;
  [Header("God Image Effect Parameters")]
  [SerializeField]
  [Range(0.0f, 3f)]
  private float godBrightness;
  [SerializeField]
  [Range(0.0f, 3f)]
  private float godColorfulness;
  [SerializeField]
  [Range(0.0f, 3f)]
  private float godContrast;
  [SerializeField]
  [Range(0.0f, 1f)]
  private float godBlurStrength;
  [SerializeField]
  [Range(0.0f, 1f)]
  private float godWhiteStrength;
  private NGSoundManager sm;
  private UnitVoicePattern currentVoicePattern;

  public override IEnumerator Initialize(PrincesEvolutionParam param, GameObject backBtn)
  {
    EffectControllerPrincessAwake controllerPrincessAwake = this;
    if (param.baseUnit.unit.ID != 101414)
    {
      Future<Sprite> loader = param.baseUnit.unit.LoadFullSprite();
      IEnumerator e = loader.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      controllerPrincessAwake.unitSpriteBefore.sprite2D = loader.Result;
      controllerPrincessAwake.unitEffectBefore2.sprite2D = loader.Result;
      loader = param.resultUnit.unit.LoadFullSprite();
      e = loader.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      controllerPrincessAwake.unitSpriteAfter.sprite2D = loader.Result;
      controllerPrincessAwake.unitEffectAfter2.sprite2D = loader.Result;
      Texture2D glowBorderTexture1 = GlowingBorderEffectManager.GetGlowBorderTexture(controllerPrincessAwake.unitSpriteBefore.sprite2D.texture, controllerPrincessAwake.borderColorTexture, controllerPrincessAwake.borderWidth, controllerPrincessAwake.borderAlphaThreshold);
      controllerPrincessAwake.unitEffectBefore.sprite2D = Sprite.Create(glowBorderTexture1, new Rect(0.0f, 0.0f, (float) ((Texture) glowBorderTexture1).width, (float) ((Texture) glowBorderTexture1).height), new Vector2(0.5f, 0.5f), 100f, 0U, (SpriteMeshType) 0);
      Texture2D glowBorderTexture2 = GlowingBorderEffectManager.GetGlowBorderTexture(controllerPrincessAwake.unitSpriteAfter.sprite2D.texture, controllerPrincessAwake.borderColorTexture, controllerPrincessAwake.borderWidth, controllerPrincessAwake.borderAlphaThreshold);
      controllerPrincessAwake.unitEffectAfter.sprite2D = Sprite.Create(glowBorderTexture2, new Rect(0.0f, 0.0f, (float) ((Texture) glowBorderTexture2).width, (float) ((Texture) glowBorderTexture2).height), new Vector2(0.5f, 0.5f), 100f, 0U, (SpriteMeshType) 0);
      UnitAwakeningEffect unitAwakeningEffect1 = MasterData.UnitAwakeningEffect[param.baseUnit.unit.resource_reference_unit_id.ID];
      Rect clipRegion1;
      // ISSUE: explicit constructor call
      ((Rect) ref clipRegion1).\u002Ector((float) unitAwakeningEffect1.x, (float) unitAwakeningEffect1.y, (float) unitAwakeningEffect1.width, (float) unitAwakeningEffect1.height);
      Texture2D blackOrGodTexture1 = GodAndBlackEffectManager.GetBlackOrGodTexture(controllerPrincessAwake.unitSpriteBefore.sprite2D.texture, clipRegion1, controllerPrincessAwake.blackBrightness, controllerPrincessAwake.blackColorfulness, controllerPrincessAwake.blackContrast, controllerPrincessAwake.blackBlurStrength, controllerPrincessAwake.blackWhiteStrength, controllerPrincessAwake.blackMainColor, controllerPrincessAwake.blackMainColorStrength);
      controllerPrincessAwake.unitBlack.sprite2D = Sprite.Create(blackOrGodTexture1, new Rect(0.0f, 0.0f, (float) ((Texture) blackOrGodTexture1).width, (float) ((Texture) blackOrGodTexture1).height), new Vector2(0.5f, 0.5f));
      UnitAwakeningEffect unitAwakeningEffect2 = MasterData.UnitAwakeningEffect[param.resultUnit.unit.resource_reference_unit_id.ID];
      Rect clipRegion2;
      // ISSUE: explicit constructor call
      ((Rect) ref clipRegion2).\u002Ector((float) unitAwakeningEffect2.x, (float) unitAwakeningEffect2.y, (float) unitAwakeningEffect2.width, (float) unitAwakeningEffect2.height);
      Color mainColor;
      // ISSUE: explicit constructor call
      ((Color) ref mainColor).\u002Ector(unitAwakeningEffect2.god_color_r, unitAwakeningEffect2.god_color_g, unitAwakeningEffect2.god_color_b);
      float godColorWeight = unitAwakeningEffect2.god_color_weight;
      Texture2D blackOrGodTexture2 = GodAndBlackEffectManager.GetBlackOrGodTexture(controllerPrincessAwake.unitSpriteAfter.sprite2D.texture, clipRegion2, controllerPrincessAwake.godBrightness, controllerPrincessAwake.godColorfulness, controllerPrincessAwake.godContrast, controllerPrincessAwake.godBlurStrength, controllerPrincessAwake.godWhiteStrength, mainColor, godColorWeight);
      controllerPrincessAwake.unitGod.sprite2D = Sprite.Create(blackOrGodTexture2, new Rect(0.0f, 0.0f, (float) ((Texture) blackOrGodTexture2).width, (float) ((Texture) blackOrGodTexture2).height), new Vector2(0.5f, 0.5f));
      loader = param.baseUnit.unit.LoadAwakeningText(1);
      e = loader.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      controllerPrincessAwake.textSprite1.sprite2D = loader.Result;
      loader = param.baseUnit.unit.LoadAwakeningText(2);
      e = loader.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      controllerPrincessAwake.textSprite2.sprite2D = loader.Result;
      loader = param.baseUnit.unit.LoadAwakeningText(3);
      e = loader.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      controllerPrincessAwake.textSprite3.sprite2D = loader.Result;
      controllerPrincessAwake.sm = Singleton<NGSoundManager>.GetInstance();
      controllerPrincessAwake.currentVoicePattern = (UnitVoicePattern) null;
      UnitVoicePattern voicePattern = MasterData.UnitUnit[param.baseUnit.unit.ID].unitVoicePattern;
      if (controllerPrincessAwake.sm.LoadVoiceData(voicePattern.file_name))
      {
        while (!controllerPrincessAwake.sm.LoadedCueSheet(voicePattern.file_name))
          yield return (object) null;
        controllerPrincessAwake.currentVoicePattern = voicePattern;
      }
      loader = (Future<Sprite>) null;
      voicePattern = (UnitVoicePattern) null;
    }
    controllerPrincessAwake.back_button_ = backBtn;
    ((Collider) controllerPrincessAwake.back_button_.GetComponent<BoxCollider>()).enabled = false;
    ((Behaviour) controllerPrincessAwake.back_button_.GetComponent<UIButton>()).enabled = false;
    controllerPrincessAwake.back_button_.SetActive(false);
    controllerPrincessAwake.soundManager.result = false;
    controllerPrincessAwake.isAnimation = true;
  }

  public override void OnAnimationEnd()
  {
    if (Object.op_Inequality((Object) this.back_button_, (Object) null))
    {
      this.back_button_.SetActive(true);
      ((Collider) this.back_button_.GetComponent<BoxCollider>()).enabled = true;
      ((Behaviour) this.back_button_.GetComponent<UIButton>()).enabled = true;
    }
    this.isAnimation = false;
  }

  public void OnPlayAwakeningVoice()
  {
    if (this.currentVoicePattern.file_name == null)
      return;
    if (this.sm.ExistsCueID(this.currentVoicePattern.file_name, 300))
      this.sm.playVoiceByID(this.currentVoicePattern, 300);
    else
      Debug.LogError((object) ("Cue does not exist: " + this.currentVoicePattern.file_name + "/" + (object) 300));
  }
}
