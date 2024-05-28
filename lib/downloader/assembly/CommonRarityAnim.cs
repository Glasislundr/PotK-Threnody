// Decompiled with JetBrains decompiler
// Type: CommonRarityAnim
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CommonRarityAnim : MonoBehaviour
{
  public MeshRenderer image_;
  public MeshRenderer image400_;
  public MeshRenderer image_blur_;
  public MeshRenderer image400_blur_;
  public List<GameObject> rarity_obj_list_;
  public List<GameObject> gacha_rarity_obj_list_;
  public List<CommonRarityAnim.RarityStart> rarity_list;

  [Serializable]
  public class RarityStart
  {
    public GameObject commonRariry;
    public List<GameObject> rarity_list;
    public GameObject rariryText2;
    public GameObject rariryText3;
    public GameObject rariryText4;
    public GameObject rariryText5;
    public GameObject rariryTextBlue3;
    public GameObject rariryTextBlue4;
    public GameObject rariryTextBlue5;
  }
}
