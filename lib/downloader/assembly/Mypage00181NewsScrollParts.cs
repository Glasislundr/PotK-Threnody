// Decompiled with JetBrains decompiler
// Type: Mypage00181NewsScrollParts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using UnityEngine;

#nullable disable
public class Mypage00181NewsScrollParts : MonoBehaviour
{
  [SerializeField]
  private UILabel title;
  [SerializeField]
  private UILabel date;
  [SerializeField]
  private UILabel time;
  [SerializeField]
  private GameObject newsSprite;
  [SerializeField]
  private GameObject bugSprite;
  [SerializeField]
  private GameObject newSprite;
  private InformationInformation master;

  public void IbtnNewslist()
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("mypage001_8_2", false, (object) this.master);
  }
}
