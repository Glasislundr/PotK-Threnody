// Decompiled with JetBrains decompiler
// Type: CutinNameManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CutinNameManager : MonoBehaviour
{
  public CutinNameManager.LeftState left;
  public CutinNameManager.CenterState center;
  public CutinNameManager.RightState right;
  public GameObject[] obj;
  public string characterName = "てーすーとー";
  public UILabel[] label;
  public float time = 0.3f;
  public float wait = 2.5f;
  public float startWait = 1f;
  private float startLeftWait;
  private float startCenterWait;
  private float startRightWait;
  private float leftWait;
  private float centerWait;
  private float rightWait;
  private float alpha = 1f;
  public Vector3[] beforePos;
  private Vector3 pos1 = new Vector3(-184f, 0.0f, 0.0f);
  private Vector3 pos2 = new Vector3(-2.5f, 30f, 0.0f);
  private Vector3 pos3 = new Vector3(184f, 0.0f, 0.0f);

  private void startCutinName(GameObject o, float time, Vector3 pos, float a)
  {
    TweenPosition.Begin(o, time, pos);
    TweenAlpha.Begin(o, time, a);
  }

  private void Start()
  {
    this.leftWait = this.wait;
    this.centerWait = this.wait;
    this.rightWait = this.wait;
  }

  private void Update()
  {
    if (this.left == CutinNameManager.LeftState.Start)
    {
      this.obj[0].SetActive(true);
      this.label[0].SetText(this.characterName);
      this.obj[0].SetActive(false);
      this.startLeftWait = this.startWait;
      this.left = CutinNameManager.LeftState.StartWait;
    }
    else if (this.left == CutinNameManager.LeftState.StartWait)
    {
      this.startLeftWait -= Time.deltaTime;
      if ((double) this.startLeftWait < 0.0)
      {
        this.startLeftWait = this.startWait;
        this.obj[0].SetActive(true);
        this.startCutinName(this.obj[0], this.time, this.pos1, this.alpha);
        this.left = CutinNameManager.LeftState.Wait;
      }
    }
    else if (this.left == CutinNameManager.LeftState.Wait)
    {
      this.leftWait -= Time.deltaTime;
      if ((double) this.leftWait < 0.0)
      {
        this.leftWait = this.wait;
        this.left = CutinNameManager.LeftState.Close;
      }
    }
    else if (this.left == CutinNameManager.LeftState.Close)
    {
      this.startCutinName(this.obj[0], this.time, this.beforePos[0], 0.0f);
      this.left = CutinNameManager.LeftState.Del;
    }
    else if (this.left == CutinNameManager.LeftState.Del)
    {
      if ((double) this.obj[0].GetComponent<UIWidget>().color.a == 0.0)
        this.left = CutinNameManager.LeftState.End;
    }
    else if (this.left == CutinNameManager.LeftState.End)
    {
      this.obj[0].SetActive(false);
      this.left = CutinNameManager.LeftState.None;
    }
    if (this.center == CutinNameManager.CenterState.Start)
    {
      this.obj[1].SetActive(true);
      this.label[1].SetText(this.characterName);
      this.obj[1].SetActive(false);
      this.startCenterWait = this.startWait;
      this.center = CutinNameManager.CenterState.StartWait;
    }
    else if (this.center == CutinNameManager.CenterState.StartWait)
    {
      this.startCenterWait -= Time.deltaTime;
      if ((double) this.startCenterWait <= 0.0)
      {
        this.obj[1].SetActive(true);
        this.startCenterWait = this.startWait;
        this.startCutinName(this.obj[1], this.time, this.pos2, this.alpha);
        this.center = CutinNameManager.CenterState.Wait;
      }
    }
    else if (this.center == CutinNameManager.CenterState.Wait)
    {
      this.centerWait -= Time.deltaTime;
      if ((double) this.centerWait < 0.0)
      {
        this.centerWait = this.wait;
        this.center = CutinNameManager.CenterState.Close;
      }
    }
    else if (this.center == CutinNameManager.CenterState.Close)
    {
      this.startCutinName(this.obj[1], this.time, this.beforePos[1], 0.0f);
      this.center = CutinNameManager.CenterState.Del;
    }
    else if (this.center == CutinNameManager.CenterState.Del)
    {
      if ((double) this.obj[1].GetComponent<UIWidget>().color.a == 0.0)
        this.center = CutinNameManager.CenterState.End;
    }
    else if (this.center == CutinNameManager.CenterState.End)
    {
      this.obj[1].SetActive(false);
      this.center = CutinNameManager.CenterState.None;
    }
    if (this.right == CutinNameManager.RightState.Start)
    {
      this.obj[2].SetActive(true);
      this.label[2].SetText(this.characterName);
      this.obj[2].SetActive(false);
      this.startRightWait = this.startWait;
      this.right = CutinNameManager.RightState.StartWait;
    }
    else if (this.right == CutinNameManager.RightState.StartWait)
    {
      this.startRightWait -= Time.deltaTime;
      if ((double) this.startRightWait > 0.0)
        return;
      this.obj[2].SetActive(true);
      this.startRightWait = this.startWait;
      this.startCutinName(this.obj[2], this.time, this.pos3, this.alpha);
      this.right = CutinNameManager.RightState.Wait;
    }
    else if (this.right == CutinNameManager.RightState.Wait)
    {
      this.rightWait -= Time.deltaTime;
      if ((double) this.rightWait >= 0.0)
        return;
      this.rightWait = this.wait;
      this.right = CutinNameManager.RightState.Close;
    }
    else if (this.right == CutinNameManager.RightState.Close)
    {
      this.startCutinName(this.obj[2], this.time, this.beforePos[2], 0.0f);
      this.right = CutinNameManager.RightState.Del;
    }
    else if (this.right == CutinNameManager.RightState.Del)
    {
      if ((double) this.obj[2].GetComponent<UIWidget>().color.a != 0.0)
        return;
      this.right = CutinNameManager.RightState.End;
    }
    else
    {
      if (this.right != CutinNameManager.RightState.End)
        return;
      this.obj[2].SetActive(false);
      this.right = CutinNameManager.RightState.None;
    }
  }

  private void StartLeft() => this.left = CutinNameManager.LeftState.Start;

  private void StartCenter() => this.center = CutinNameManager.CenterState.Start;

  private void StartRight() => this.right = CutinNameManager.RightState.Start;

  public enum LeftState
  {
    None,
    Start,
    StartWait,
    Wait,
    Close,
    Del,
    End,
  }

  public enum CenterState
  {
    None,
    Start,
    StartWait,
    Wait,
    Close,
    Del,
    End,
  }

  public enum RightState
  {
    None,
    Start,
    StartWait,
    Wait,
    Close,
    Del,
    End,
  }
}
