// Decompiled with JetBrains decompiler
// Type: Bugu00539Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Bugu00539Scene : NGSceneBase
{
  public List<ItemInfo> gearIdList;
  public bool is_new_;
  private GameObject armorSythesisAnimationPrefab;
  private GameObject sythesisObj;
  private string nowBgmName;
  [SerializeField]
  private Bugu00539Menu menu;

  public ItemInfo sythesisItem { get; set; }

  public ItemInfo baseItem { get; set; }

  public PlayerItem targetReisou { get; set; }

  public PlayerItem baseReisou { get; set; }

  public int addReisouJewel { get; set; }

  public static void ChangeScene(bool stack, Bugu00539ChangeSceneParam param)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("bugu005_3_9", (stack ? 1 : 0) != 0, (object) param);
  }

  public IEnumerator onStartSceneAsync(Bugu00539ChangeSceneParam param)
  {
    ItemInfo num = param.num_list[param.num_list.Count - 1];
    param.num_list.Remove(num);
    this.gearIdList = param.num_list;
    this.sythesisItem = num;
    this.baseItem = param.baseItem;
    if (this.sythesisItem.reisou != (PlayerItem) null)
      this.targetReisou = this.sythesisItem.reisou;
    this.baseReisou = param.baseReisou;
    this.addReisouJewel = param.addReisouJewel;
    IEnumerator e;
    if (Object.op_Equality((Object) this.armorSythesisAnimationPrefab, (Object) null))
    {
      Future<GameObject> armorSythesisAnimationPrefabf = Res.Prefabs.ArmorSythesis.ArmorSythesisAnimation.Load<GameObject>();
      e = armorSythesisAnimationPrefabf.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.armorSythesisAnimationPrefab = armorSythesisAnimationPrefabf.Result;
      armorSythesisAnimationPrefabf = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.menu.effect, (Object) null))
      this.menu.effect = Object.Instantiate<GameObject>(this.armorSythesisAnimationPrefab).GetComponent<EffectControllerArmorSythesis>();
    e = this.menu.SetEffectData(param.num_list, param.is_new, this.sythesisItem, param.anim_pattern, this.baseItem, param.backSceneCallback);
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

  public void onStartScene(Bugu00539ChangeSceneParam param) => this.onStartScene();

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
