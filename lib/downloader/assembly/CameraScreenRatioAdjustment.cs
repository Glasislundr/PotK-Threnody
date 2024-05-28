// Decompiled with JetBrains decompiler
// Type: CameraScreenRatioAdjustment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class CameraScreenRatioAdjustment : MonoBehaviour
{
  [SerializeField]
  private Camera viewCamera;
  [Header("サイズ(間のサイズなら線形補完する)")]
  [SerializeField]
  private CameraScreenRatioAdjustment.ratioData ratioSize;
  [Header("画面サイズが可変した場合、自動で計算しなおすか")]
  [SerializeField]
  private bool isAutoUpdate = true;
  private float adjustSize;
  private float[] ratioTable = new float[4];
  private float[] ratioSizeTable = new float[4];

  private void Awake()
  {
    this.ratioTable[0] = 1.33333337f;
    this.ratioTable[1] = 1.77777779f;
    this.ratioTable[2] = 2.11111116f;
    this.ratioTable[3] = 2.33333325f;
    this.ratioSizeTable[0] = this.ratioSize.ratio3_4;
    this.ratioSizeTable[1] = this.ratioSize.ratio9_16;
    this.ratioSizeTable[2] = this.ratioSize.ratio9_19;
    this.ratioSizeTable[3] = this.ratioSize.ratio9_21;
    for (int index = 0; index < this.ratioSizeTable.Length; ++index)
    {
      if (Mathf.Approximately(this.ratioSizeTable[index], 0.0f))
        this.ratioSizeTable[index] = !this.viewCamera.orthographic ? this.viewCamera.fieldOfView : this.viewCamera.orthographicSize;
    }
  }

  private void Start()
  {
    this.adjustSize = this.GetAdjustSize();
    if (Mathf.Approximately(this.adjustSize, 0.0f))
      return;
    this.Adjust(this.adjustSize);
  }

  private void Update()
  {
    if (!this.isAutoUpdate)
      return;
    float adjustSize = this.GetAdjustSize();
    if (Mathf.Approximately(adjustSize, this.adjustSize))
      return;
    this.Adjust(adjustSize);
    this.adjustSize = adjustSize;
  }

  private float GetAdjustSize()
  {
    float num = (float) Screen.height / (float) Screen.width;
    float adjustSize = 0.0f;
    if ((double) num <= (double) this.ratioTable[0])
      adjustSize = this.ratioSizeTable[0];
    else if (Mathf.Approximately(num, this.ratioTable[1]))
      adjustSize = this.ratioSizeTable[1];
    else if (Mathf.Approximately(num, this.ratioTable[2]))
      adjustSize = this.ratioSizeTable[2];
    else if ((double) num >= (double) this.ratioTable[3])
      adjustSize = this.ratioSizeTable[3];
    else if ((double) num >= (double) this.ratioTable[2] && (double) num <= (double) this.ratioTable[3])
      adjustSize = (float) (((double) this.ratioSizeTable[3] - (double) this.ratioSizeTable[2]) * ((double) num - (double) this.ratioTable[2]) / ((double) this.ratioTable[3] - (double) this.ratioTable[2])) + this.ratioSizeTable[2];
    else if ((double) num >= (double) this.ratioTable[1] && (double) num <= (double) this.ratioTable[2])
      adjustSize = (float) (((double) this.ratioSizeTable[2] - (double) this.ratioSizeTable[1]) * ((double) num - (double) this.ratioTable[1]) / ((double) this.ratioTable[2] - (double) this.ratioTable[1])) + this.ratioSizeTable[1];
    else if ((double) num >= (double) this.ratioTable[0] && (double) num <= (double) this.ratioTable[1])
      adjustSize = (float) (((double) this.ratioSizeTable[1] - (double) this.ratioSizeTable[0]) * ((double) num - (double) this.ratioTable[0]) / ((double) this.ratioTable[1] - (double) this.ratioTable[0])) + this.ratioSizeTable[0];
    return adjustSize;
  }

  private void Adjust(float size)
  {
    if (Object.op_Equality((Object) this.viewCamera, (Object) null))
      return;
    if (this.viewCamera.orthographic)
      this.viewCamera.orthographicSize = size;
    else
      this.viewCamera.fieldOfView = size;
  }

  [Serializable]
  public class ratioData
  {
    [Header("画面比率9:21のサイズ")]
    public float ratio9_21;
    [Header("画面比率9:19のサイズ")]
    public float ratio9_19;
    [Header("画面比率9:16のサイズ")]
    public float ratio9_16;
    [Header("画面比率3:4のサイズ")]
    public float ratio3_4;
  }
}
