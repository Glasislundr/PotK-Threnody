// Decompiled with JetBrains decompiler
// Type: ShakeControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ShakeControl : MonoBehaviour
{
  public ShakeControl.ShakeState shakeState;
  public float wieght = 3f;
  public float wieghtTime = 0.04f;
  public float waitTime;
  private float elapsedTime;

  private void Start()
  {
    this.shakeState = ShakeControl.ShakeState.None;
    ((Component) this).gameObject.AddComponent<TweenPosition>();
  }

  private void Setshake(float wieght, float time, float wait)
  {
    ((Component) this).gameObject.transform.localPosition = new Vector3(Random.Range(-wieght, wieght), Random.Range(-wieght, wieght), 0.0f);
    this.elapsedTime += Time.deltaTime;
    if ((double) wait == 0.0 || (double) this.elapsedTime <= (double) wait)
      return;
    this.elapsedTime = 0.0f;
    this.shakeState = ShakeControl.ShakeState.Stop;
  }

  private void Update()
  {
    if (this.shakeState == ShakeControl.ShakeState.Start)
    {
      this.Setshake(this.wieght, this.wieghtTime, this.waitTime);
    }
    else
    {
      if (this.shakeState != ShakeControl.ShakeState.Stop)
        return;
      ((Component) this).gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
      this.shakeState = ShakeControl.ShakeState.None;
    }
  }

  private void StopShake() => this.shakeState = ShakeControl.ShakeState.Stop;

  private void StartShake() => this.shakeState = ShakeControl.ShakeState.Start;

  public enum ShakeState
  {
    None,
    Wait,
    Start,
    Stop,
  }
}
