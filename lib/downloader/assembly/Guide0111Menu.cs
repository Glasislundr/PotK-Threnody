// Decompiled with JetBrains decompiler
// Type: Guide0111Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
public class Guide0111Menu : BackButtonMenuBase
{
  private Guide0111Scene.UpdateInfo updaterUnit = new Guide0111Scene.UpdateInfo();
  private Guide0111Scene.UpdateInfo updaterEnemy = new Guide0111Scene.UpdateInfo();
  private Guide0111Scene.UpdateInfo updaterBugu = new Guide0111Scene.UpdateInfo();

  public void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    if (Singleton<NGSceneManager>.GetInstance().getSceneStackPath() == "")
      Singleton<NGSceneManager>.GetInstance().changeScene("story001_9_1", false);
    else
      this.backScene();
  }

  public void IbtnBugubook_AnimList3()
  {
    if (this.IsPushAndSet())
      return;
    Guide0114Scene.changeScene(true, this.updaterBugu);
  }

  public void IbtnEnemybook_AnimList2()
  {
    if (this.IsPushAndSet())
      return;
    Guide0113Scene.changeScene(true, this.updaterEnemy);
  }

  public void IbtnUnitbook_AnimList1()
  {
    if (this.IsPushAndSet())
      return;
    Guide0112Scene.changeScene(true, this.updaterUnit);
  }

  public void IbtnStory()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("story009_0", true);
  }

  public void IbtnJukebox()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("guide011_Jukebox", true);
  }

  public void IbtnPledgeHimeList()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("guide011_PledgeHime_List", true);
  }

  public override void onBackButton() => this.IbtnBack();
}
