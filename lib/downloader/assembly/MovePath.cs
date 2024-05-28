// Decompiled with JetBrains decompiler
// Type: MovePath
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MovePath : MonoBehaviour
{
  private MovePathCore core;
  [SerializeField]
  private float editorPointSize = 10f;
  [SerializeField]
  private List<MovePathPoint> points;
  [SerializeField]
  private float time = 1f;
  private float addTime;
  [SerializeField]
  private bool autoPlay;
  [SerializeField]
  private bool loopPlay;
  private MovePath.State state;

  public MovePathCore Core
  {
    get
    {
      if (this.core == null)
        this.core = new MovePathCore();
      return this.core;
    }
  }

  public float EditorPointSize => this.editorPointSize;

  public List<MovePathPoint> Points
  {
    get
    {
      if (this.points == null)
        this.points = new List<MovePathPoint>();
      return this.points;
    }
  }

  public bool IsPlay => this.Core.IsPlay;

  public MovePath.State CurrentState => this.state;

  private void Start()
  {
    if (!this.autoPlay)
      return;
    this.Play();
  }

  private void OnEnable()
  {
    if (!this.autoPlay)
      return;
    this.Play();
  }

  private void Update()
  {
    switch (this.state)
    {
      case MovePath.State.Stop:
        ((Component) this).transform.localPosition = this.Core.GetPointsLocalPosition(0, ((Component) this).transform.lossyScale);
        break;
      case MovePath.State.Play:
        ((Component) this).transform.localPosition = this.Core.UpdateCurve(this.addTime * Time.deltaTime, Vector3.one);
        if (!this.loopPlay || this.IsPlay)
          break;
        this.Play();
        break;
    }
  }

  public void Play()
  {
    if (this.Points.Count <= 2)
      return;
    this.Core.WriteMovePositions(this.GetPointPositions());
    this.Core.Play();
    this.state = MovePath.State.Play;
    this.addTime = (float) (1.0 / ((double) this.time / (double) (this.Points.Count / 2)));
  }

  public void Pause() => this.state = MovePath.State.Pause;

  public void Stop() => this.state = MovePath.State.Stop;

  public Vector3[] GetPointPositions()
  {
    Vector3[] pointPositions = new Vector3[this.points.Count];
    for (int index = 0; index < this.points.Count; ++index)
      pointPositions[index] = this.points[index].LocalPosition;
    return pointPositions;
  }

  public enum State
  {
    Stop,
    Play,
    Pause,
  }
}
