// Decompiled with JetBrains decompiler
// Type: SlotDebug
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class SlotDebug : MonoBehaviour
{
  private NGSoundManager SM;
  public bool isReady;
  public bool isEnd;
  public bool isSkip;
  private Animator animator;
  [SerializeField]
  private GameObject lille_1;
  [SerializeField]
  private GameObject lille_2;
  [SerializeField]
  private GameObject lille_3;
  [SerializeField]
  private GameObject prize_1;
  [SerializeField]
  private GameObject prize_2;
  [SerializeField]
  private GameObject prize_3;
  [SerializeField]
  private GameObject slotMain;
  [SerializeField]
  private List<string> ReelTextureNames;
  private Shop00720EffectController main_script;
  public int state_index;
  private string lilleTexturePath = "AssetBundle/Resources/Animations/slot/Texture/lilleImages/";
  private const int lilleTextureNum = 7;
  private const int lilleLookingIndex = 6;
  private List<Texture2D> textures = new List<Texture2D>();
  private SpriteRenderer spriteRenderer_p1;
  private SpriteRenderer spriteRenderer_p2;
  private SpriteRenderer spriteRenderer_p3;
  private Sprite sprite_p1;
  private Sprite sprite_p2;
  private Sprite sprite_p3;
  [SerializeField]
  private GameObject lamp_blue;
  [SerializeField]
  private GameObject lamp_lightblue;
  [SerializeField]
  private GameObject lamp_red;
  [SerializeField]
  private ParticleSystem ps_light_lightblue;
  [SerializeField]
  private ParticleSystem ps_light_red;
  [SerializeField]
  private ParticleSystem ps_lightblue_1;
  [SerializeField]
  private ParticleSystem ps_lightblue_2;
  [SerializeField]
  private ParticleSystem ps_lightblue_3;
  [SerializeField]
  private ParticleSystem ps_lightblue_4;
  [SerializeField]
  private ParticleSystem ps_red_1;
  [SerializeField]
  private ParticleSystem ps_red_2;
  [SerializeField]
  private ParticleSystem ps_red_3;
  [SerializeField]
  private ParticleSystem ps_red_4;
  [SerializeField]
  private MeshRenderer black_renderer;
  public bool slot100;

  public IEnumerator Init()
  {
    SlotDebug slotDebug = this;
    slotDebug.SM = Singleton<NGSoundManager>.GetInstance();
    slotDebug.animator = ((Component) slotDebug).GetComponent<Animator>();
    slotDebug.main_script = slotDebug.slotMain.GetComponent<Shop00720EffectController>();
    foreach (string reelTextureName in slotDebug.ReelTextureNames)
    {
      IEnumerator e = slotDebug.GetTexture(reelTextureName);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    Debug.Log((object) ("textures:" + (object) slotDebug.textures.Count));
    for (int setid = 0; setid < 7; ++setid)
    {
      Debug.Log((object) ("textureNameList_1:" + slotDebug.main_script.textureNameList_1[setid]));
      slotDebug.SetTextureByName(slotDebug.main_script.textureNameList_1[setid], slotDebug.lille_1, setid);
      slotDebug.SetTextureByName(slotDebug.main_script.textureNameList_2[setid], slotDebug.lille_2, setid);
      slotDebug.SetTextureByName(slotDebug.main_script.textureNameList_3[setid], slotDebug.lille_3, setid);
    }
    slotDebug.spriteRenderer_p1 = slotDebug.prize_1.GetComponent<SpriteRenderer>();
    slotDebug.spriteRenderer_p2 = slotDebug.prize_2.GetComponent<SpriteRenderer>();
    slotDebug.spriteRenderer_p3 = slotDebug.prize_3.GetComponent<SpriteRenderer>();
  }

  private void Update()
  {
    AnimatorStateInfo animatorStateInfo1 = this.animator.GetCurrentAnimatorStateInfo(0);
    if (!((AnimatorStateInfo) ref animatorStateInfo1).IsName("result") || !this.slot100)
      return;
    AnimatorStateInfo animatorStateInfo2 = this.animator.GetCurrentAnimatorStateInfo(0);
    if ((double) ((AnimatorStateInfo) ref animatorStateInfo2).normalizedTime < 1.0)
      return;
    this.slot100 = false;
    this.ChangeState(3000);
    this.StartCoroutine(this.main_script.ResultView100ren());
  }

  private IEnumerator GetTexture(string filename)
  {
    Debug.Log((object) (this.lilleTexturePath + filename));
    Future<Texture2D> feature = Singleton<ResourceManager>.GetInstance().Load<Texture2D>(this.lilleTexturePath + filename);
    IEnumerator e = feature.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Texture2D result = feature.Result;
    ((Object) result).name = filename;
    this.textures.Add(result);
  }

  private void LilleResultTextureChange()
  {
    this.SetTextureByName(this.main_script.textureNameList_1[this.main_script.stopTextureId_1 - 1], this.lille_1, 6);
    this.SetTextureByName(this.main_script.textureNameList_2[this.main_script.stopTextureId_2 - 1], this.lille_2, 6);
    this.SetTextureByName(this.main_script.textureNameList_3[this.main_script.stopTextureId_3 - 1], this.lille_3, 6);
    this.sprite_p1 = Sprite.Create(this.GetTextureByName(this.main_script.textureNameList_1[this.main_script.stopTextureId_1 - 1]), new Rect(0.0f, 0.0f, 128f, 128f), new Vector2(0.5f, 0.5f), 100f, 0U, (SpriteMeshType) 0);
    this.sprite_p2 = Sprite.Create(this.GetTextureByName(this.main_script.textureNameList_2[this.main_script.stopTextureId_2 - 1]), new Rect(0.0f, 0.0f, 128f, 128f), new Vector2(0.5f, 0.5f), 100f, 0U, (SpriteMeshType) 0);
    this.sprite_p3 = Sprite.Create(this.GetTextureByName(this.main_script.textureNameList_3[this.main_script.stopTextureId_3 - 1]), new Rect(0.0f, 0.0f, 128f, 128f), new Vector2(0.5f, 0.5f), 100f, 0U, (SpriteMeshType) 0);
    this.spriteRenderer_p1.sprite = this.sprite_p1;
    this.spriteRenderer_p2.sprite = this.sprite_p2;
    this.spriteRenderer_p3.sprite = this.sprite_p3;
    Debug.Log((object) ("テクスチャ数:" + (object) this.main_script.textureNameList_1.Length));
    this.SetTextureByName(this.main_script.textureNameList_1[this.GetReelTextureNameId(this.main_script.textureNameList_1, this.main_script.stopTextureId_1)], this.lille_1, 0);
    this.SetTextureByName(this.main_script.textureNameList_2[this.GetReelTextureNameId(this.main_script.textureNameList_2, this.main_script.stopTextureId_2)], this.lille_2, 0);
    this.SetTextureByName(this.main_script.textureNameList_3[this.GetReelTextureNameId(this.main_script.textureNameList_3, this.main_script.stopTextureId_3)], this.lille_3, 0);
    this.SetTextureByName(this.main_script.textureNameList_1[this.GetReelTextureNameId(this.main_script.textureNameList_1, this.main_script.stopTextureId_1 - 2)], this.lille_1, 5);
    this.SetTextureByName(this.main_script.textureNameList_2[this.GetReelTextureNameId(this.main_script.textureNameList_2, this.main_script.stopTextureId_2 - 2)], this.lille_2, 5);
    this.SetTextureByName(this.main_script.textureNameList_3[this.GetReelTextureNameId(this.main_script.textureNameList_3, this.main_script.stopTextureId_3 - 2)], this.lille_3, 5);
  }

  private int GetReelTextureNameId(string[] list, int id)
  {
    int length = list.Length;
    return id <= length - 1 ? (id >= 0 ? id : length - 1) : 0;
  }

  private void SetTextureByName(string texture_name, GameObject target_gb, int setid)
  {
    target_gb.GetComponent<Renderer>().materials[setid].mainTexture = (Texture) this.textures.SingleOrDefault<Texture2D>((Func<Texture2D, bool>) (f => ((Object) f).name == texture_name));
  }

  private Texture2D GetTextureByName(string texture_name)
  {
    return this.textures.SingleOrDefault<Texture2D>((Func<Texture2D, bool>) (f => ((Object) f).name == texture_name));
  }

  public void SlotInit()
  {
    Debug.Log((object) nameof (SlotInit));
    this.ChangeState(3000);
    this.isEnd = false;
    this.isReady = true;
    this.isSkip = false;
    this.slot100 = false;
    this.Lamp_switch("blue");
  }

  public void SlotStart()
  {
    Debug.Log((object) nameof (SlotStart));
    this.isReady = false;
    this.isSkip = true;
    this.state_index = 0;
    this.ChangeState(1000);
  }

  public void SlotEnd()
  {
    Debug.Log((object) nameof (SlotEnd));
    this.isEnd = true;
    this.isSkip = false;
  }

  private void NextState()
  {
    Debug.Log((object) nameof (NextState));
    if (this.state_index < this.main_script.transitionPlanList.Length)
    {
      Debug.Log((object) "1");
      this.ChangeState(this.main_script.transitionPlanList[this.state_index++]);
    }
    else
    {
      Debug.Log((object) "2");
      ((Renderer) this.black_renderer).enabled = !this.slot100;
      this.ChangeState(2000);
    }
  }

  private void ChangeState(int id) => this.animator.SetInteger("transition_id", id);

  public void Skip()
  {
    if (!this.isSkip || this.state_index <= 1 || this.state_index >= 4)
      return;
    Debug.Log((object) nameof (Skip));
    this.isSkip = false;
    this.state_index = 3;
    this.ChangeState(this.main_script.transitionPlanList[this.state_index++]);
  }

  public IEnumerator CheckLoadState()
  {
    while (true)
    {
      Debug.Log((object) ("main_script.loadState:" + this.main_script.loadState.ToString()));
      if (!this.main_script.loadState)
        yield return (object) new WaitForSeconds(0.5f);
      else
        break;
    }
    Debug.Log((object) "LOAD OK!");
    this.NextState();
  }

  public void Lamp_switch(string color)
  {
    switch (color)
    {
      case "blue":
        this.lamp_blue.SetActive(true);
        this.lamp_lightblue.SetActive(false);
        this.lamp_red.SetActive(false);
        break;
      case "lightblue":
        this.lamp_blue.SetActive(false);
        this.lamp_lightblue.SetActive(true);
        this.lamp_red.SetActive(false);
        this.ps_light_lightblue.Play();
        this.ps_lightblue_1.Play();
        this.ps_lightblue_2.Play();
        this.ps_lightblue_3.Play();
        this.ps_lightblue_4.Play();
        break;
      case "red":
        this.lamp_blue.SetActive(false);
        this.lamp_lightblue.SetActive(false);
        this.lamp_red.SetActive(true);
        this.ps_light_red.Play();
        this.ps_red_1.Play();
        this.ps_red_2.Play();
        this.ps_red_3.Play();
        this.ps_red_4.Play();
        break;
    }
  }

  public void PlayShowCutin() => this.main_script.PlayCutin();

  public void HideCutin() => this.main_script.HideCutin();

  public void SoundPlay(string id) => this.SM.playSE(id);

  public void SoundPlayLoop(string id) => this.SM.playSE(id, true);

  public void SoundStop() => this.SM.StopSe();
}
