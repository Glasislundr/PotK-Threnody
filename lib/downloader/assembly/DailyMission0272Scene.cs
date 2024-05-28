// Decompiled with JetBrains decompiler
// Type: DailyMission0272Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class DailyMission0272Scene : NGSceneBase
{
  [SerializeField]
  private DailyMission0272Menu menu;
  [SerializeField]
  private UIPanel mainPanel;
  private long? verPlayer;
  private long? verGuild;
  private long? verDailyMission;
  private long? verGuildMission;
  private bool isInitByBackScene;

  public static void ChangeScene0272(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("dailymission027_2", stack);
  }

  public IEnumerator onStartSceneAsync()
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    IEnumerator e = this.menu.Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onBackSceneAsync()
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    this.isInitByBackScene = false;
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
    {
      this.isInitByBackScene = true;
      yield return e.Current;
    }
    e = (IEnumerator) null;
    if (!this.isInitByBackScene)
      this.isInitByBackScene = this.isUpdatedData();
    if (this.isInitByBackScene)
    {
      e = this.menu.Init(false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public void updateDataVersion()
  {
    this.verPlayer = new long?(SMManager.Revision<Player>());
    this.verGuild = new long?(SMManager.Revision<PlayerAffiliation>());
    this.verDailyMission = new long?(SMManager.Revision<PlayerDailyMissionAchievement[]>());
    this.verGuildMission = new long?(SMManager.Revision<GuildMissionInfo[]>());
  }

  private bool isUpdatedData()
  {
    if (!this.verPlayer.HasValue || !this.verGuild.HasValue || !this.verDailyMission.HasValue || !this.verGuildMission.HasValue)
      return true;
    long num1 = SMManager.Revision<Player>();
    long? nullable = this.verPlayer;
    long valueOrDefault1 = nullable.GetValueOrDefault();
    if (num1 == valueOrDefault1 & nullable.HasValue)
    {
      long num2 = SMManager.Revision<PlayerAffiliation>();
      nullable = this.verGuild;
      long valueOrDefault2 = nullable.GetValueOrDefault();
      if (num2 == valueOrDefault2 & nullable.HasValue)
      {
        long num3 = SMManager.Revision<PlayerDailyMissionAchievement[]>();
        nullable = this.verDailyMission;
        long valueOrDefault3 = nullable.GetValueOrDefault();
        if (num3 == valueOrDefault3 & nullable.HasValue)
        {
          long num4 = SMManager.Revision<GuildMissionInfo[]>();
          nullable = this.verGuildMission;
          long valueOrDefault4 = nullable.GetValueOrDefault();
          if (num4 == valueOrDefault4 & nullable.HasValue)
            return false;
        }
      }
    }
    return true;
  }

  public void onStartScene() => this.StartCoroutine(this.hideLoading());

  public void onBackScene() => this.onStartScene();

  private IEnumerator hideLoading()
  {
    if (Object.op_Inequality((Object) this.mainPanel, (Object) null))
      ((UIRect) this.mainPanel).alpha = 1f;
    yield return (object) new WaitForEndOfFrame();
    yield return (object) new WaitForEndOfFrame();
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  private void OnEnable()
  {
    if (!Object.op_Inequality((Object) this.mainPanel, (Object) null))
      return;
    ((UIRect) this.mainPanel).alpha = 0.0f;
  }
}
