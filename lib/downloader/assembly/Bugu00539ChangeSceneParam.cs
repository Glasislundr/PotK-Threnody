// Decompiled with JetBrains decompiler
// Type: Bugu00539ChangeSceneParam
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections.Generic;

#nullable disable
public class Bugu00539ChangeSceneParam
{
  public List<GameCore.ItemInfo> num_list;
  public bool is_new;
  public string[] anim_pattern;
  public GameCore.ItemInfo baseItem;
  public PlayerItem baseReisou;
  public Action backSceneCallback;
  public int addReisouJewel;

  public Bugu00539ChangeSceneParam(
    List<GameCore.ItemInfo> numList,
    bool isNew,
    string[] animPattern,
    GameCore.ItemInfo baseItem,
    PlayerItem baseReisou,
    Action backScene,
    int addReisouJewel)
  {
    this.num_list = numList;
    this.is_new = isNew;
    this.anim_pattern = animPattern;
    this.baseItem = baseItem;
    this.baseReisou = baseReisou;
    this.backSceneCallback = backScene;
    this.addReisouJewel = addReisouJewel;
  }
}
