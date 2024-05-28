// Decompiled with JetBrains decompiler
// Type: MypageInformationSheet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MypageInformationSheet : MonoBehaviour
{
  public GameObject node_;
  public UILabel txtTitle_;
  public UILabel txtMessage_;
  public int outCount_;

  public void onOutFinished()
  {
    if (!((Component) this).gameObject.activeSelf || --this.outCount_ > 0)
      return;
    ((Component) this).gameObject.SetActive(false);
  }
}
