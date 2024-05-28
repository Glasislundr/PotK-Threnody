// Decompiled with JetBrains decompiler
// Type: StoryBG
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class StoryBG : MonoBehaviour
{
  public string namePrefab;
  private UIWidget widget;

  private void Start()
  {
    this.widget = ((Component) this).gameObject.GetComponent<UIWidget>();
    this.setWidget();
  }

  private void setWidget() => this.widget.depth = 0;
}
