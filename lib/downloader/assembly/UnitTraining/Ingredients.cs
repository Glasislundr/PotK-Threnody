// Decompiled with JetBrains decompiler
// Type: UnitTraining.Ingredients
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;

#nullable disable
namespace UnitTraining
{
  public class Ingredients
  {
    private object estimates_;

    public TrainingType type { get; private set; }

    public PlayerUnit baseUnit { get; set; }

    public PlayerUnit[] materialPlayerUnits { get; set; }

    public PlayerMaterialUnit[] materialUnits { get; set; }

    public WebAPI.Response.UnitEvolutionParameter estimatesEvolution
    {
      get => this.estimates_ as WebAPI.Response.UnitEvolutionParameter;
      set => this.estimates_ = (object) value;
    }

    public PlayerUnit estimatesReincarnation
    {
      get => this.estimates_ as PlayerUnit;
      set => this.estimates_ = (object) value;
    }

    public Ingredients(TrainingType t) => this.type = t;

    public void resetWithoutBase()
    {
      this.materialPlayerUnits = (PlayerUnit[]) null;
      this.materialUnits = (PlayerMaterialUnit[]) null;
      this.estimates_ = (object) null;
    }
  }
}
