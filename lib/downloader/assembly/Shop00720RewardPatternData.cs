// Decompiled with JetBrains decompiler
// Type: Shop00720RewardPatternData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Shop00720RewardPatternData
{
  private bool isEnabled;
  private List<Sprite> reelPattern = new List<Sprite>();
  private List<Shop00720RewardData> rewardList = new List<Shop00720RewardData>();
  private string description;

  public bool IsEnabled
  {
    get => this.isEnabled;
    set => this.isEnabled = value;
  }

  public List<Sprite> ReelPattern
  {
    get => this.reelPattern;
    set => this.reelPattern = value;
  }

  public List<Shop00720RewardData> RewardList
  {
    get => this.rewardList;
    set => this.rewardList = value;
  }

  public string Description
  {
    get => this.description;
    set => this.description = value;
  }

  public Shop00720RewardPatternData(int[] reelIds)
  {
    this.isEnabled = ((IEnumerable<int>) reelIds).All<int>((Func<int, bool>) (a => a != 0));
  }

  public IEnumerator SetSprite(int[] ids)
  {
    int[] numArray = ids;
    for (int index = 0; index < numArray.Length; ++index)
    {
      Future<Sprite> spriteF = this.LoadSpriteThumbnail(numArray[index]);
      IEnumerator e = spriteF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.reelPattern.Add(spriteF.Result);
      spriteF = (Future<Sprite>) null;
    }
    numArray = (int[]) null;
  }

  private Future<Sprite> LoadSpriteThumbnail(int id)
  {
    return Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format("AssetBundle/Resources/Animations/slot/Texture/lilleImages/slot_icon_{0:D2}f", (object) id));
  }
}
