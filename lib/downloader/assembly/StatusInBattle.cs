// Decompiled with JetBrains decompiler
// Type: StatusInBattle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class StatusInBattle
{
  [SerializeField]
  private GameObject dir_inbattle_effect_container;
  [SerializeField]
  private string dir_inbattle_effect_container_prefab_name;
  private string loaded_dir_inbattle_effect_container = string.Empty;
  public List<SpriteNumberSelectDirect> slc_Remain_hours;
  public List<SpriteNumberSelectDirect> slc_Remain_minutes;
  [SerializeField]
  private GuildStatus MyStatus;
  [SerializeField]
  private GuildStatus EnStatus;

  public IEnumerator ResourceLoad(GuildRegistration myData, GuildRegistration enData)
  {
    IEnumerator e = this.MyStatus.ResourceLoad(myData);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.EnStatus.ResourceLoad(enData);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject obj = this.dir_inbattle_effect_container;
    string objName = this.dir_inbattle_effect_container_prefab_name;
    string path = string.Format("Prefabs/guild028_2/{0}", (object) objName);
    if (!(this.loaded_dir_inbattle_effect_container == objName))
    {
      Future<GameObject> f = Singleton<ResourceManager>.GetInstance().Load<GameObject>(path);
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.loaded_dir_inbattle_effect_container = objName;
      f.Result.Clone(obj.transform);
    }
  }

  public void SetStatus(GuildRegistration myData, GuildRegistration enData)
  {
    this.MyStatus.SetStatus(myData);
    this.EnStatus.SetStatus(enData);
  }

  public void UpdateStatus(GuildRegistration myData, GuildRegistration enData)
  {
    this.MyStatus.UpdateStatus(myData);
    this.EnStatus.UpdateStatus(enData);
  }
}
