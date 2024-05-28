// Decompiled with JetBrains decompiler
// Type: Mypage00181EventScrollParts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using UnityEngine;

#nullable disable
public class Mypage00181EventScrollParts : MonoBehaviour
{
  [SerializeField]
  private UI2DSprite sprite;
  [SerializeField]
  private GameObject newSprite;
  [SerializeField]
  private UILabel date;
  [SerializeField]
  private UILabel time;
  private InformationInformation master;
  private DateTime endDate;

  public void IbtnNewslist()
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("mypage001_8_2", false, (object) this.master);
  }
}
