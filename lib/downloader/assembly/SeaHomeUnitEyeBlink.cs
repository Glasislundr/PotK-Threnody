// Decompiled with JetBrains decompiler
// Type: SeaHomeUnitEyeBlink
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using UnityEngine;

#nullable disable
public class SeaHomeUnitEyeBlink : MonoBehaviour
{
  private NGxFaceSprite face;
  private NGxEyeSprite eye;
  private UnitUnit unit;
  private float randomWaitTime;
  private float waitTime;
  private bool isInit;

  public void Init(UnitUnit unit, NGxFaceSprite face, NGxEyeSprite eye)
  {
    this.face = face;
    ((Behaviour) face).enabled = true;
    this.eye = eye;
    ((Behaviour) eye).enabled = true;
    ((Component) eye.EyeUI2DSprite).gameObject.SetActive(true);
    this.unit = unit;
    ((Behaviour) this).enabled = false;
  }

  public void StopBlink() => ((Behaviour) this).enabled = false;

  public void StartBlink(float delay)
  {
    if (!this.unit.HasSpriteEye("close"))
      return;
    ((Behaviour) this).enabled = true;
    this.isInit = false;
    this.waitTime = 0.0f;
    this.randomWaitTime = delay;
  }

  private void Update()
  {
    this.waitTime += Time.deltaTime;
    if ((double) this.waitTime < (double) this.randomWaitTime)
      return;
    if (!this.isInit)
    {
      this.isInit = true;
      this.randomWaitTime = Random.Range(2f, 3f);
      if (this.unit.HasSpriteFace("usual"))
        this.StartCoroutine(this.face.ChangeFace("usual"));
      else
        this.StartCoroutine(this.face.ChangeFace("normal"));
    }
    else if (this.eye.EyeName == "normal")
    {
      this.StartCoroutine(this.eye.ChangeEye("close"));
      this.randomWaitTime = Random.Range(0.1f, 0.2f);
    }
    else if (this.eye.EyeName == "close")
    {
      this.StartCoroutine(this.eye.ChangeEye("normal"));
      this.randomWaitTime = Random.Range(2f, 3f);
    }
    this.waitTime = 0.0f;
  }
}
