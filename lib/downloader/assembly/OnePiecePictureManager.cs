// Decompiled with JetBrains decompiler
// Type: OnePiecePictureManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class OnePiecePictureManager : MonoBehaviour
{
  public OnePiecePictureManager.State state;
  public GameObject obj;
  public GameObject container;
  public UI2DSprite picture;
  public float fromPos;
  public float toPos = 1000f;
  public float size = 1.3f;
  public float fromAlpha = 1f;
  public float toAlpha;
  public float time = 0.3f;

  private void Start()
  {
  }

  private void Update()
  {
    switch (this.state)
    {
      case OnePiecePictureManager.State.MoveIn:
        this.container.SetActive(true);
        this.obj.transform.localPosition = new Vector3(this.fromPos, 0.0f, 0.0f);
        TweenPosition.Begin(this.obj, this.time, new Vector3(this.toPos, 0.0f, 0.0f));
        this.state = OnePiecePictureManager.State.Wait;
        break;
      case OnePiecePictureManager.State.MoveOut:
        this.container.SetActive(true);
        TweenPosition.Begin(this.obj, this.time, new Vector3(this.toPos, 0.0f, 0.0f));
        this.state = OnePiecePictureManager.State.Wait;
        break;
      case OnePiecePictureManager.State.Scale:
        this.container.SetActive(true);
        TweenScale.Begin(this.obj, this.time, new Vector3(this.size, this.size, 1f));
        this.state = OnePiecePictureManager.State.Wait;
        break;
      case OnePiecePictureManager.State.Alpha:
        this.container.SetActive(true);
        ((UIWidget) this.picture).color = new Color(1f, 1f, 1f, this.fromAlpha);
        TweenAlpha.Begin(this.obj, this.time, this.toAlpha);
        this.state = OnePiecePictureManager.State.Wait;
        break;
      case OnePiecePictureManager.State.Wait:
        if ((double) this.obj.transform.localPosition.x <= -1000.0 || (double) this.obj.transform.localPosition.x >= 1000.0)
        {
          this.container.SetActive(false);
          this.state = OnePiecePictureManager.State.None;
        }
        else if ((double) this.obj.transform.localPosition.x == 0.0)
          this.state = OnePiecePictureManager.State.None;
        if ((double) ((UIWidget) this.picture).color.a != 0.0)
          break;
        this.container.SetActive(false);
        this.state = OnePiecePictureManager.State.None;
        break;
    }
  }

  private void startMoveIn() => this.state = OnePiecePictureManager.State.MoveIn;

  private void startMoveOut() => this.state = OnePiecePictureManager.State.MoveOut;

  private void startScale() => this.state = OnePiecePictureManager.State.Scale;

  private void startAlpha() => this.state = OnePiecePictureManager.State.Alpha;

  public enum State
  {
    None,
    Stack,
    MoveIn,
    MoveOut,
    Scale,
    Alpha,
    Wait,
  }
}
