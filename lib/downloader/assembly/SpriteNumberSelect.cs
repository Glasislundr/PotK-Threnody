// Decompiled with JetBrains decompiler
// Type: SpriteNumberSelect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SpriteNumberSelect : MonoBehaviour
{
  private GameObject[] numbers = new GameObject[10];

  private void Awake() => this.initialize();

  private void initialize()
  {
    if (!Object.op_Equality((Object) this.numbers[0], (Object) null))
      return;
    foreach (Transform transform in ((Component) this).transform)
    {
      for (int index = 0; index < this.numbers.Length; ++index)
      {
        if (((Object) transform).name.EndsWith(string.Format("{0}", (object) index)))
        {
          this.numbers[index] = ((Component) transform).gameObject;
          break;
        }
      }
    }
  }

  public void setNumber(int n)
  {
    this.initialize();
    for (int index = 0; index < this.numbers.Length; ++index)
    {
      if (Object.op_Inequality((Object) this.numbers[index], (Object) null))
        this.numbers[index].SetActive(n == index);
    }
  }
}
