// Decompiled with JetBrains decompiler
// Type: unitInfomation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using UnityEngine;

#nullable disable
public class unitInfomation
{
  public BL.Unit bu;
  public Battle0181CharacterStatus p;
  public NGDuelUnit enemy;
  public bool isplayer = true;
  public bool iscriticalcamera;
  public BL.MagicBullet mb;
  public BL.Weapon weapon;
  public Transform trs;
  public int range;
  public IntimateDuelSupport support;
  public int supportHitIncr;
  public int supportEvasionIncr;
  public int supportCriticalIncr;
  public int supportCriticalEvasionIncr;
  public GameObject root3d;
  public NGDuelManager mng;
  public int[] beforeAilmentEffectIDs;
  public SkillMetamorphosis metamorphosis;
}
