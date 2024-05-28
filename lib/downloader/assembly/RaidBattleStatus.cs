// Decompiled with JetBrains decompiler
// Type: RaidBattleStatus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class RaidBattleStatus : MonoBehaviour
{
  public const int RP_MAX_NUM = 6;
  [SerializeField]
  private UIGrid mLampAnchor;
  private GameObject mLampPrefab;
  private RaidBattlePointLamp[] mLamps = new RaidBattlePointLamp[6];
  private int mLestRaidPoint;
  private int mLestRaidPointMax;

  public IEnumerator InitAsync()
  {
    this.mLestRaidPoint = GuildUtil.rp;
    this.mLestRaidPointMax = GuildUtil.rp_max;
    if (Object.op_Equality((Object) this.mLampPrefab, (Object) null))
    {
      Future<GameObject> prefabF = new ResourceObject("Prefabs/raid032_battle/slc_icon_BP_Lamp").Load<GameObject>();
      IEnumerator e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.mLampPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    this.initLamps();
  }

  private void initLamps()
  {
    this.mLamps = new RaidBattlePointLamp[this.mLestRaidPointMax];
    for (int index = 0; index < this.mLestRaidPointMax; ++index)
    {
      GameObject gameObject = this.mLampPrefab.Clone(((Component) this.mLampAnchor).transform);
      this.mLamps[index] = gameObject.GetComponent<RaidBattlePointLamp>();
    }
    for (int index = 1; index <= this.mLestRaidPointMax - this.mLestRaidPoint; ++index)
      this.mLamps[this.mLestRaidPointMax - index].Off();
    this.mLampAnchor.repositionNow = true;
  }

  public void EnableAllLamp()
  {
    foreach (RaidBattlePointLamp mLamp in this.mLamps)
      mLamp.Enable();
  }

  public void DisableAllLamp()
  {
    foreach (RaidBattlePointLamp mLamp in this.mLamps)
      mLamp.Disable();
  }
}
