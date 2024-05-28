// Decompiled with JetBrains decompiler
// Type: CommonHeaderBP
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CommonHeaderBP : MonoBehaviour
{
  public GameObject[] objects;
  [SerializeField]
  protected int point;
  public UILabel timeText;
  public NGTweenParts timeContainer;

  public int Value => this.point;

  public int setValue(int v)
  {
    for (int index = 0; index < this.objects.Length; ++index)
      this.objects[index].SetActive(index < v);
    return this.point = v;
  }

  public void setTime(float time)
  {
    if (Object.op_Equality((Object) this.timeContainer, (Object) null))
      return;
    bool flag = (double) time > 0.0;
    this.timeContainer.isActive = flag;
    if (!flag)
      return;
    this.timeText.SetTextLocalize(string.Format("{0:00}:{1:00}", (object) Mathf.FloorToInt(time / 60f), (object) Mathf.FloorToInt(time % 60f)));
  }
}
