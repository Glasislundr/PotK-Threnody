// Decompiled with JetBrains decompiler
// Type: Modified`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
public class Modified<T> where T : class
{
  private long revision;

  public Modified(long revision) => this.revision = revision;

  public bool Loaded => SMManager.Contains<T>();

  public bool Changed => this.revision != SMManager.Revision<T>();

  public void Commit() => this.revision = SMManager.Revision<T>();

  public void NotifyChanged()
  {
    if (!this.Loaded)
      return;
    SMManager.Change<T>(SMManager.Get<T>());
  }

  public bool IsChangedOnce()
  {
    int num = this.Changed ? 1 : 0;
    this.revision = SMManager.Revision<T>();
    return num != 0;
  }

  public T Value => SMManager.Get<T>();
}
