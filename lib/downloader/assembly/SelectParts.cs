// Decompiled with JetBrains decompiler
// Type: SelectParts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SelectParts : MonoBehaviour
{
  public NGTweenParts[] objects;
  [SerializeField]
  protected int value = -1;

  public int setValueNonTween(int v)
  {
    for (int index = 0; index < this.objects.Length; ++index)
      this.objects[index].resetActive(index == v);
    return this.value = v;
  }

  public int setValue(int v)
  {
    for (int index = 0; index < this.objects.Length; ++index)
      this.objects[index].isActive = index == v;
    return this.value = v;
  }

  public int setFirstValue(int v)
  {
    for (int index = 0; index < this.objects.Length; ++index)
    {
      if (index == v)
        this.objects[index].isActive = true;
      else
        this.objects[index].resetActive(false);
    }
    return this.value = v;
  }

  public int getValue() => this.value;

  public int inclement()
  {
    int v = this.value + 1;
    if (v >= this.objects.Length)
      v = this.objects.Length - 1;
    return this.setValue(v);
  }

  public int inclementLoop() => this.setValue((this.value + 1) % this.objects.Length);

  public int inclementLoopNonTween()
  {
    return this.setValueNonTween((this.value + 1) % this.objects.Length);
  }

  public int decrement()
  {
    int v = this.value - 1;
    if (v < 0)
      v = 0;
    return this.setValue(v);
  }

  public int decrementLoop()
  {
    int v = this.value - 1;
    if (v < 0)
      v = this.objects.Length - 1;
    return this.setValue(v);
  }
}
