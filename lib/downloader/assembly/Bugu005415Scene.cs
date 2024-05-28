// Decompiled with JetBrains decompiler
// Type: Bugu005415Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Bugu005415Scene : NGSceneBase
{
  [SerializeField]
  private Bugu005415Menu menu;
  private GameObject popUp;
  private string nowBgmName;

  public static void ChangeScene(
    bool stack,
    List<ItemInfo> thum_list,
    List<WebAPI.Response.ItemGearRepairRepair_results> result_list)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("bugu005_4_15", (stack ? 1 : 0) != 0, (object) thum_list, (object) result_list);
  }

  public static void ChangeScene(
    bool stack,
    List<ItemInfo> thum_list,
    List<WebAPI.Response.ItemGearRepairListRepair_results> result_list)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("bugu005_4_15", (stack ? 1 : 0) != 0, (object) thum_list, (object) result_list);
  }

  private IEnumerator onStartSceneAsync()
  {
    if (Object.op_Equality((Object) this.popUp, (Object) null))
    {
      Future<GameObject> fPopUp = Res.Prefabs.ArmorRepair.ArmorRepairAnimation.Load<GameObject>();
      IEnumerator e = fPopUp.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.popUp = fPopUp.Result;
      fPopUp = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.menu.effect, (Object) null))
      this.menu.effect = Object.Instantiate<GameObject>(this.popUp).GetComponent<EffectControllerArmorRepair>();
  }

  public IEnumerator onStartSceneAsync(
    List<ItemInfo> thum_list,
    List<WebAPI.Response.ItemGearRepairRepair_results> result_list)
  {
    IEnumerator e = this.onStartSceneAsync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.menu.SetEffectData(thum_list, result_list);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(
    List<ItemInfo> thum_list,
    List<WebAPI.Response.ItemGearRepairListRepair_results> result_list)
  {
    IEnumerator e = this.onStartSceneAsync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.menu.SetEffectData(thum_list, result_list);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene()
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<PopupManager>.GetInstance().closeAll();
    this.nowBgmName = Singleton<NGSoundManager>.GetInstance().GetBgmName();
    Singleton<NGSoundManager>.GetInstance().StopBgm();
  }

  public void onStartScene(
    List<ItemInfo> thum_list,
    List<WebAPI.Response.ItemGearRepairRepair_results> result_list)
  {
    this.onStartScene();
  }

  public void onStartScene(
    List<ItemInfo> thum_list,
    List<WebAPI.Response.ItemGearRepairListRepair_results> result_list)
  {
    this.onStartScene();
  }

  public override void onEndScene()
  {
    base.onEndScene();
    Singleton<PopupManager>.GetInstance().open((GameObject) null);
    Singleton<NGSoundManager>.GetInstance().PlayBgm(this.nowBgmName);
  }

  public override IEnumerator onEndSceneAsync()
  {
    yield return (object) new WaitForSeconds(0.5f);
    ((Component) this.menu.effect).gameObject.SetActive(false);
  }
}
