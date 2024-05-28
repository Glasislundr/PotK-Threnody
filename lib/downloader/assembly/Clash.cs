// Decompiled with JetBrains decompiler
// Type: Clash
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Clash : MonoBehaviour
{
  public GameObject windowObj;
  public float scale = 2.5f;
  public float scaleTime = 0.2f;
  public float clashTime = 0.2f;
  public float clashWieght = 10f;
  public float wait = 0.3f;
  public Clash.State state;
  private float clash;
  private float waitTime;
  public float posY = -350f;
  private float posX;
  public bool isClash;
  public bool isSkip;
  public bool isloop;
  private float ScaleX = 1f;
  private float ScaleY = 1f;

  private void Start() => this.state = Clash.State.None;

  private void Update()
  {
    switch (this.state)
    {
      case Clash.State.Start:
        this.isClash = true;
        this.ScaleX = ((Component) this).gameObject.transform.localScale.x;
        this.ScaleY = ((Component) this).gameObject.transform.localScale.y;
        this.posX = ((Component) this).gameObject.transform.localPosition.x;
        if (this.isSkip)
        {
          TweenPosition.Begin(((Component) this).gameObject, 0.02f, new Vector3(0.0f, this.posY, 0.0f));
          TweenScale.Begin(((Component) this).gameObject, 0.02f, new Vector3(this.scale, this.scale, 0.0f));
        }
        TweenPosition.Begin(((Component) this).gameObject, this.scaleTime, new Vector3(0.0f, this.posY, 0.0f));
        TweenScale.Begin(((Component) this).gameObject, this.scaleTime, new Vector3(this.scale, this.scale, 0.0f));
        this.state = Clash.State.ScaleUp;
        break;
      case Clash.State.ScaleUp:
        if ((double) ((Component) this).gameObject.transform.localScale.x != (double) this.scale)
          break;
        this.state = Clash.State.Clash;
        break;
      case Clash.State.Clash:
        this.clash += Time.deltaTime;
        if (this.isSkip)
          this.clashTime = 0.1f;
        if ((double) this.clash >= (double) this.clashTime)
        {
          this.clash = 0.0f;
          this.windowObj.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
          this.state = Clash.State.Wait;
          break;
        }
        this.windowObj.transform.localPosition = new Vector3(Random.Range(-this.clashWieght, this.clashWieght), Random.Range((float) (-(double) this.clashWieght * 2.0), this.clashWieght * 2f), 0.0f);
        break;
      case Clash.State.Wait:
        if (this.isSkip)
        {
          this.state = Clash.State.ScaleDown;
          break;
        }
        this.waitTime += Time.deltaTime;
        if ((double) this.waitTime < (double) this.wait)
          break;
        this.waitTime = 0.0f;
        this.state = Clash.State.ScaleDown;
        break;
      case Clash.State.ScaleDown:
        if (this.isSkip)
        {
          TweenPosition.Begin(((Component) this).gameObject, 0.04f, new Vector3(this.posX, 0.0f, 0.0f));
          TweenScale.Begin(((Component) this).gameObject, 0.04f, new Vector3(this.ScaleX, this.ScaleY, 0.0f));
        }
        else
        {
          TweenPosition.Begin(((Component) this).gameObject, this.scaleTime * 3f, new Vector3(this.posX, 0.0f, 0.0f));
          TweenScale.Begin(((Component) this).gameObject, this.scaleTime * 3f, new Vector3(this.ScaleX, this.ScaleY, 0.0f));
        }
        this.state = Clash.State.End;
        break;
      case Clash.State.End:
        if ((double) ((Component) this).gameObject.transform.localScale.x != (double) this.ScaleX)
          break;
        this.isClash = false;
        this.isSkip = false;
        if (this.isloop)
        {
          this.state = Clash.State.Start;
          break;
        }
        this.state = Clash.State.None;
        break;
    }
  }

  public void EndClash()
  {
    if (!this.isClash)
      return;
    this.windowObj.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
    TweenPosition.Begin(((Component) this).gameObject, 0.0f, new Vector3(this.posX, 0.0f, 0.0f));
    TweenScale.Begin(((Component) this).gameObject, 0.0f, new Vector3(this.ScaleX, this.ScaleY, 0.0f));
    this.clash = 0.0f;
    this.waitTime = 0.0f;
    this.isClash = false;
    this.isSkip = false;
    this.state = Clash.State.None;
  }

  public enum State
  {
    None,
    Start,
    ScaleUp,
    Clash,
    Wait,
    ScaleDown,
    End,
  }
}
