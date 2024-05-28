// Decompiled with JetBrains decompiler
// Type: GUISelectBox`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class GUISelectBox<T>
{
  private readonly string title;
  private readonly T[] orgOptions;
  private T[] options;
  private string[] labels;
  private Vector2 scrollPosition;
  private bool toggle;
  private int selectedIndex;
  private Func<T, string> toLabel;
  private string finder = string.Empty;

  public GUISelectBox(string title_, T[] options_, Func<T, string> toLabel_)
  {
    this.title = title_;
    this.toggle = false;
    this.selectedIndex = 0;
    if (options_.Length == 0)
      Debug.LogError((object) "option is empty");
    this.orgOptions = options_;
    this.options = this.orgOptions;
    this.toLabel = toLabel_;
    this.labels = ((IEnumerable<T>) this.options).Select<T, string>((Func<T, string>) (x => this.toLabel(x))).ToArray<string>();
  }

  public T Render(Action<T> changeCallback = null)
  {
    return changeCallback == null ? this.RenderWithIndex() : this.RenderWithIndex((Action<int, T>) ((_, val) => changeCallback(val)));
  }

  public T RenderWithIndex(Action<int, T> changeCallback = null)
  {
    if (this.toggle)
    {
      GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
      if (GUILayout.Button("back", new GUILayoutOption[1]
      {
        GUILayout.Width(120f)
      }))
        this.toggle = false;
      string str = GUILayout.TextArea(this.finder, 10, Array.Empty<GUILayoutOption>());
      if (str != this.finder)
      {
        this.finder = str;
        this.selectedIndex = 0;
        this.options = ((IEnumerable<T>) this.orgOptions).Where<T>((Func<T, bool>) (x => this.toLabel(x).Contains(this.finder))).ToArray<T>();
        if (this.options.Length != 0)
          this.labels = ((IEnumerable<T>) this.options).Select<T, string>((Func<T, string>) (x => this.toLabel(x))).ToArray<string>();
      }
      GUILayout.EndHorizontal();
      if (this.options.Length < 1)
        return default (T);
      int num1 = Screen.width / 10;
      int num2 = Screen.width - num1;
      this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition, false, false, new GUILayoutOption[2]
      {
        GUILayout.Width((float) num2),
        GUILayout.Height((float) (Screen.height - Screen.height / 10))
      });
      int num3 = GUILayout.SelectionGrid(this.selectedIndex, this.labels, 1, new GUILayoutOption[1]
      {
        GUILayout.Width((float) (num2 - num1))
      });
      GUILayout.EndScrollView();
      if (num3 != this.selectedIndex)
      {
        this.selectedIndex = num3;
        this.toggle = false;
        if (changeCallback != null)
          changeCallback(this.selectedIndex, this.Value());
      }
    }
    else
      this.toggle = GUILayout.Toggle(this.toggle, this.title + " " + this.labels[this.selectedIndex], Array.Empty<GUILayoutOption>());
    return this.Value();
  }

  public T Value()
  {
    return this.options.Length < 1 ? this.orgOptions[0] : this.options[this.selectedIndex];
  }

  public void setSelected(int i) => this.selectedIndex = i;

  public void setSelected(Func<T, bool> match)
  {
    for (int index = 0; index < this.options.Length; ++index)
    {
      if (match(this.options[index]))
      {
        this.selectedIndex = index;
        return;
      }
    }
    this.selectedIndex = 0;
  }
}
