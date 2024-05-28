// Decompiled with JetBrains decompiler
// Type: Colosseum0234ScrollText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Colosseum0234ScrollText : MonoBehaviour
{
  public GameObject scrollView;
  public GameObject infoLabel;
  public float speed;
  private Vector2 scrollSize;
  private Vector3 labelStartPosition;
  private int textLength;
  private bool isInit;
  private bool isScroll;

  public void StartScroll(string text)
  {
    this.Init();
    this.isScroll = true;
    this.infoLabel.GetComponent<UILabel>().SetText(text);
    this.textLength = this.infoLabel.GetComponent<UIWidget>().width;
    this.infoLabel.transform.localPosition = this.labelStartPosition;
  }

  private void Init()
  {
    if (this.isInit)
      return;
    this.isInit = true;
    this.scrollSize = this.scrollView.GetComponent<UIPanel>().GetViewSize();
    this.labelStartPosition = this.infoLabel.transform.localPosition;
    this.textLength = this.infoLabel.GetComponent<UIWidget>().width;
  }

  private void Start() => this.Init();

  private void Update()
  {
    if (!this.isScroll)
      return;
    this.infoLabel.transform.Translate(-this.speed * Time.deltaTime, 0.0f, 0.0f);
    if ((double) this.infoLabel.transform.localPosition.x >= -((double) this.textLength + (double) this.scrollSize.x / 2.0))
      return;
    Vector3 vector3;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3).\u002Ector(this.scrollSize.x / 2f, this.labelStartPosition.y, 0.0f);
    this.infoLabel.transform.localPosition = vector3;
  }
}
