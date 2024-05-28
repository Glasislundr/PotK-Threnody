// Decompiled with JetBrains decompiler
// Type: MovePathCore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MovePathCore
{
  private Vector3[] movePositions;
  private bool isPlay;
  private float weight;
  private int index;

  public Vector3[] MovePositions => this.movePositions;

  public bool IsPlay => this.isPlay;

  public MovePathCore()
  {
  }

  public MovePathCore(MovePathCore core)
  {
    this.WriteMovePositions(core.movePositions);
    this.isPlay = core.isPlay;
    this.weight = core.weight;
    this.index = core.index;
  }

  public void WriteMovePositions(Vector3[] points)
  {
    if (points == null)
      return;
    this.movePositions = new Vector3[points.Length];
    for (int index = 0; index < points.Length; ++index)
    {
      ref Vector3 local = ref points[index];
      this.movePositions[index] = points[index];
    }
  }

  public void Play()
  {
    if (this.movePositions == null || this.movePositions.Length <= 2)
      return;
    this.isPlay = true;
    this.index = 0;
    this.weight = 0.0f;
  }

  public Vector3 UpdateCurve(float addWeight, Vector3 scale)
  {
    if (this.movePositions == null || this.movePositions.Length == 0)
      return Vector3.zero;
    if (this.index >= this.movePositions.Length - 2)
    {
      this.isPlay = false;
      return this.movePositions.Length % 2 != 0 ? this.GetPointsLocalPosition(this.movePositions.Length - 1, scale) : this.GetPointsLocalPosition(this.movePositions.Length - 2, scale);
    }
    Vector3 vector3 = this.B_SplineCurve(this.GetPointsLocalPosition(this.index, scale), this.GetPointsLocalPosition(this.index + 1, scale), this.GetPointsLocalPosition(this.index + 2, scale), this.weight);
    this.weight += addWeight;
    while ((double) this.weight > 1.0)
    {
      --this.weight;
      this.index += 2;
    }
    return vector3;
  }

  public Vector3 GetPointsLocalPosition(int index, Vector3 scale)
  {
    if (this.movePositions == null || index < 0 || this.movePositions.Length <= index)
      return Vector3.zero;
    ref Vector3 local = ref this.movePositions[index];
    return new Vector3(this.movePositions[index].x * scale.x, this.movePositions[index].y * scale.y, this.movePositions[index].z * scale.z);
  }

  private float B_SplineCurve(float x1, float x2, float x3, float t)
  {
    return (float) ((1.0 - (double) t) * (1.0 - (double) t) * (double) x1 + 2.0 * (double) t * (1.0 - (double) t) * (double) x2 + (double) t * (double) t * (double) x3);
  }

  private Vector3 B_SplineCurve(Vector3 p1, Vector3 p2, Vector3 p3, float t)
  {
    return new Vector3(this.B_SplineCurve(p1.x, p2.x, p3.x, t), this.B_SplineCurve(p1.y, p2.y, p3.y, t), this.B_SplineCurve(p1.z, p2.z, p3.z, t));
  }
}
