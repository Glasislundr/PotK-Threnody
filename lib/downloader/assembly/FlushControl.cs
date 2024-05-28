// Decompiled with JetBrains decompiler
// Type: FlushControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class FlushControl : MonoBehaviour
{
  private Color color;
  private float time;
  private int value;
  private bool isEnd = true;
  [SerializeField]
  private UIWidget widget;

  private void Start()
  {
  }

  private void Update()
  {
  }

  public void Start(Color color, float time, int value)
  {
    this.widget.color = color;
    this.time = time;
    this.value = value;
    this.isEnd = false;
    this.StartCoroutine(this.FlushUpdate());
  }

  public void SetEnd()
  {
    ((UIRect) this.widget).alpha = 0.0f;
    this.isEnd = true;
  }

  public bool IsEnd() => this.isEnd;

  private IEnumerator FlushUpdate()
  {
    for (int count = 0; count < this.value && !this.isEnd; ++count)
    {
      ((UIRect) this.widget).alpha = (float) byte.MaxValue;
      yield return (object) new WaitForSeconds(this.time);
      ((UIRect) this.widget).alpha = 0.0f;
      yield return (object) new WaitForSeconds(this.time);
    }
    this.isEnd = true;
  }
}
