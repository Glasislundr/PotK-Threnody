// Decompiled with JetBrains decompiler
// Type: IconPrefabBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public abstract class IconPrefabBase : MonoBehaviour
{
  protected bool gray;

  public void SetSize(int width, int height)
  {
    UIWidget component = ((Component) this).GetComponent<UIWidget>();
    ((Component) component).transform.localScale = new Vector3((float) width / (float) component.width, (float) height / (float) component.height);
  }

  public void SetBasedOnHeight(int height)
  {
    UIWidget component = ((Component) this).GetComponent<UIWidget>();
    float num = (float) height / (float) component.height;
    ((Component) component).transform.localScale = new Vector3(num, num);
  }

  public void SetBasedOnWidth(int width)
  {
    UIWidget component = ((Component) this).GetComponent<UIWidget>();
    float num = (float) width / (float) component.width;
    ((Component) component).transform.localScale = new Vector3(num, num);
  }

  public virtual bool Gray
  {
    get => this.gray;
    set => this.gray = value;
  }
}
