// Decompiled with JetBrains decompiler
// Type: SM.GvgWholeRewardMaster
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class GvgWholeRewardMaster : KeyCompare
  {
    public int reward_quantity;
    public string reward_message;
    public int _reward_type;
    public string reward_title;
    public int reward_id;

    public MasterDataTable.CommonRewardType reward_type
    {
      get
      {
        if (!Enum.IsDefined(typeof (MasterDataTable.CommonRewardType), (object) this._reward_type))
          Debug.LogError((object) ("Key not Found: MasterDataTable.CommonRewardType[" + (object) this._reward_type + "]"));
        return (MasterDataTable.CommonRewardType) this._reward_type;
      }
    }

    public GvgWholeRewardMaster()
    {
    }

    public GvgWholeRewardMaster(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.reward_quantity = (int) (long) json[nameof (reward_quantity)];
      this.reward_message = (string) json[nameof (reward_message)];
      this._reward_type = (int) (long) json[nameof (reward_type)];
      this.reward_title = (string) json[nameof (reward_title)];
      this.reward_id = (int) (long) json[nameof (reward_id)];
    }
  }
}
