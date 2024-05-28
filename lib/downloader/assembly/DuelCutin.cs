// Decompiled with JetBrains decompiler
// Type: DuelCutin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class DuelCutin : MonoBehaviour
{
  private UnitUnit unitData;
  private int metamor_id;
  private UnitVoicePattern voicePattern;
  private NGSoundManager sm;
  private Animator am;
  public float criticalVoiceDelay;
  public float skillVoiceDelay;
  public bool isTexExist;
  public EffectSE sePlayer;
  private bool combineCutinError;
  private float spent;
  [SerializeField]
  private GameObject cameraCutin;
  [SerializeField]
  private MeshRenderer[] cutinObjects;
  private Texture cutinTexture;

  public GameObject CameraCutin => this.cameraCutin;

  public IEnumerator Initialize<T>(T unit)
  {
    DuelCutin duelCutin = this;
    duelCutin.isTexExist = false;
    duelCutin.combineCutinError = false;
    duelCutin.GetUnitUnit<T>(unit);
    duelCutin.sm = Singleton<NGSoundManager>.GetInstance();
    duelCutin.am = ((Component) duelCutin).gameObject.GetComponent<Animator>();
    if (Object.op_Inequality((Object) duelCutin.cutinObjects[0], (Object) null))
    {
      Future<Sprite> fs = duelCutin.unitData.LoadCutin(duelCutin.metamor_id);
      IEnumerator e = fs.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Texture texture = (Texture) fs.Result.texture;
      if (Object.op_Inequality((Object) texture, (Object) null))
      {
        duelCutin.cutinTexture = texture;
        ((Renderer) duelCutin.cutinObjects[0]).materials[0].SetTexture("_MainTex", duelCutin.cutinTexture);
        duelCutin.isTexExist = true;
      }
      fs = (Future<Sprite>) null;
    }
  }

  public IEnumerator InitializeForRoulette(Future<Sprite> cutInSprite)
  {
    DuelCutin duelCutin = this;
    duelCutin.isTexExist = false;
    duelCutin.combineCutinError = false;
    if (Object.op_Inequality((Object) duelCutin.cutinObjects[0], (Object) null))
    {
      IEnumerator e = cutInSprite.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Texture texture = (Texture) cutInSprite.Result.texture;
      if (Object.op_Inequality((Object) texture, (Object) null))
      {
        duelCutin.cutinTexture = texture;
        ((Renderer) duelCutin.cutinObjects[0]).materials[0].SetTexture("_MainTex", duelCutin.cutinTexture);
        duelCutin.isTexExist = true;
      }
    }
    duelCutin.sm = Singleton<NGSoundManager>.GetInstance();
    duelCutin.am = ((Component) duelCutin).gameObject.GetComponent<Animator>();
  }

  public void SetCutinTexture(Texture tex, DuelCutin.CUTINPOS pos)
  {
    if ((DuelCutin.CUTINPOS) this.cutinObjects.Length <= pos)
    {
      Debug.LogError((object) "[SetCutinTexture]Index Error");
      this.combineCutinError = true;
    }
    else if (Object.op_Inequality((Object) this.cutinObjects[(int) pos], (Object) null) && Object.op_Inequality((Object) tex, (Object) null))
      ((Renderer) this.cutinObjects[(int) pos]).materials[0].SetTexture("_MainTex", tex);
    else
      this.combineCutinError = true;
  }

  public IEnumerator LoadAndSetTexture(int unitId, DuelCutin.CUTINPOS pos)
  {
    UnitUnit unitUnit;
    if (!MasterData.UnitUnit.TryGetValue(unitId, out unitUnit))
    {
      Debug.LogError((object) "[LoadAndSetTexture] UnitId Not Found");
      this.combineCutinError = true;
    }
    else
    {
      Future<Sprite> fs = unitUnit.LoadCutin();
      IEnumerator e = fs.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Texture texture = (Texture) fs.Result.texture;
      if (Object.op_Inequality((Object) texture, (Object) null))
        this.SetCutinTexture(texture, pos);
    }
  }

  private void GetUnitUnit<T>(T unit)
  {
    if ((object) ((object) unit as BL.Unit) != null)
    {
      BL.Unit unit1 = (object) unit as BL.Unit;
      this.unitData = unit1.unit;
      SkillMetamorphosis metamorphosis = unit1.metamorphosis;
      this.metamor_id = metamorphosis != null ? metamorphosis.metamorphosis_id : 0;
      this.voicePattern = this.unitData.getVoicePattern(this.metamor_id);
    }
    else if ((object) ((object) unit as PlayerUnit) != null)
    {
      this.unitData = ((object) unit as PlayerUnit).unit;
      this.metamor_id = 0;
      this.voicePattern = this.unitData.unitVoicePattern;
    }
    else
    {
      this.unitData = (UnitUnit) null;
      this.voicePattern = (UnitVoicePattern) null;
      Debug.LogError((object) "CLASS IS UNEXPECTED!!");
    }
  }

  public void PlayCriticalCutin()
  {
    this.am.SetTrigger("cutin_start");
    ((Renderer) this.cutinObjects[0]).materials[0].SetTexture("_MainTex", this.cutinTexture);
    if (this.voicePattern != null)
      this.sm.playVoiceByID(this.voicePattern, 74, delay: this.criticalVoiceDelay);
    if (!Object.op_Inequality((Object) this.sePlayer, (Object) null))
      return;
    this.sePlayer.playSe();
  }

  public void PlaySkillCutin(DuelCutin.PlayMode mode = DuelCutin.PlayMode.FIRST_PERSON)
  {
    if (this.combineCutinError)
      mode = DuelCutin.PlayMode.NONE;
    switch (mode)
    {
      case DuelCutin.PlayMode.FIRST_PERSON:
        this.am.SetTrigger("cutin_start");
        ((Renderer) this.cutinObjects[0]).materials[0].SetTexture("_MainTex", this.cutinTexture);
        break;
      case DuelCutin.PlayMode.SECOND_PERSON:
        this.am.SetTrigger("cutin_2start");
        break;
      case DuelCutin.PlayMode.THIRD_PERSON:
        this.am.SetTrigger("cutin_3start");
        break;
      default:
        this.am.SetTrigger("cutin_start");
        break;
    }
    if (this.voicePattern != null)
      this.sm.playVoiceByID(this.voicePattern, 73, delay: this.skillVoiceDelay);
    if (!Object.op_Inequality((Object) this.sePlayer, (Object) null))
      return;
    this.sePlayer.playSe();
  }

  public void PlaySkillCutInForRoulette(int clipID)
  {
    this.am.SetTrigger("cutin_start");
    this.sm.playVoiceByID("VO_9001", clipID);
    if (!Object.op_Inequality((Object) this.sePlayer, (Object) null))
      return;
    this.sePlayer.playSe();
  }

  public enum PlayMode
  {
    NONE,
    FIRST_PERSON,
    SECOND_PERSON,
    THIRD_PERSON,
  }

  public enum CUTINPOS
  {
    TOP,
    CENTER,
    BOTTOM,
  }
}
