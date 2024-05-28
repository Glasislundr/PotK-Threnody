// Decompiled with JetBrains decompiler
// Type: Quest00210Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public class Quest00210Scene : NGSceneBase
{
  public Quest00210Menu menu;

  public static void changeScene(bool stack)
  {
    Quest00210Scene.Mode mode = Singleton<NGGameDataManager>.GetInstance().IsEarth ? Quest00210Scene.Mode.Earth : Quest00210Scene.Mode.Quest;
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_10", (stack ? 1 : 0) != 0, (object) mode);
    Singleton<NGSceneManager>.GetInstance().destroyScene("quest002_10a");
  }

  public static void changeScene(bool stack, List<SupplyItem> SupplyItems)
  {
    Quest00210Scene.Mode mode = Singleton<NGGameDataManager>.GetInstance().IsEarth ? Quest00210Scene.Mode.Earth : Quest00210Scene.Mode.Quest;
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_10", (stack ? 1 : 0) != 0, (object) mode, (object) SupplyItems);
    Singleton<NGSceneManager>.GetInstance().destroyScene("quest002_10a");
  }

  public static void changeScene(bool stack, Quest00210Scene.Mode mode)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_10", (stack ? 1 : 0) != 0, (object) mode);
    Singleton<NGSceneManager>.GetInstance().destroyScene("quest002_10a");
  }

  public static void changeScene(
    bool stack,
    Quest00210Scene.Mode mode,
    List<SupplyItem> SupplyItems)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_10", (stack ? 1 : 0) != 0, (object) mode, (object) SupplyItems);
    Singleton<NGSceneManager>.GetInstance().destroyScene("quest002_10a");
  }

  public override IEnumerator onInitSceneAsync()
  {
    yield break;
  }

  public IEnumerator onStartSceneAsync(Quest00210Scene.Mode mode)
  {
    Quest00210Scene quest00210Scene = this;
    if (mode == Quest00210Scene.Mode.Earth)
      quest00210Scene.bgmName = "bgm104";
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
      Singleton<CommonRoot>.GetInstance().headerType = CommonRoot.HeaderType.Normal;
    List<PlayerItem> playerItemList = mode != Quest00210Scene.Mode.Raid ? ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>().AllBattleSupplies()).ToList<PlayerItem>() : ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>().AllRaidSupplies()).ToList<PlayerItem>();
    List<PlayerItem> list = ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>().AllSupplies()).ToList<PlayerItem>();
    IEnumerator e = quest00210Scene.menu.Init(((IEnumerable<SupplyItem>) SupplyItem.Merge(list.ToArray(), playerItemList.ToArray())).ToList<SupplyItem>(), mode);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(Quest00210Scene.Mode mode, List<SupplyItem> SupplyItems)
  {
    Quest00210Scene quest00210Scene = this;
    if (mode == Quest00210Scene.Mode.Earth)
      quest00210Scene.bgmName = "bgm104";
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
      Singleton<CommonRoot>.GetInstance().headerType = CommonRoot.HeaderType.Normal;
    IEnumerator e = quest00210Scene.menu.Init(SupplyItems, mode);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene(Quest00210Scene.Mode mode)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public void onStartScene(Quest00210Scene.Mode mode, List<SupplyItem> SupplyItems)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public enum Mode
  {
    Quest,
    Earth,
    Tower,
    Raid,
    Corps,
  }
}
