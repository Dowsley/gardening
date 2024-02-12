using Godot;
using System.Collections.Generic;

public partial class Pot : StaticBody2D
{
    public int WaterConsumptionPerPhase = 20;
    public bool HasSeed = false;

    private Sprite _plantSprite;
    private Label _waterLevelLabel;

    private int _waterLevel = 0;
    private int _currPhase = 0;

    private static readonly List<Texture> PlantPhaseTextures = new List<Texture>
    {
        null,
        GD.Load<Texture>("res://assets/plants/Classic/1.png"),
        GD.Load<Texture>("res://assets/plants/Classic/2.png"),
        GD.Load<Texture>("res://assets/plants/Classic/3.png"),
    };


    public override void _Ready()
    {
        _waterLevelLabel = GetNode<Label>("WaterLevelLabel");
        _plantSprite = GetNode<Sprite>("PlantSprite");
        _plantSprite.Texture = null;
        _waterLevelLabel.Modulate = Colors.Red;
    }

    public override void _Process(float delta)
    {
        _waterLevelLabel.Visible = Globals.Instance.WaterLevelLabelVisible;
        _waterLevelLabel.Text = _waterLevel.ToString();
        if (!HasSeed || _currPhase >= PlantPhaseTextures.Count - 1)
        {
            return;
        }

        if (_waterLevel >= WaterConsumptionPerPhase)
        {
            GrowToNextPhase();
        }
    }

    public void GrowToNextPhase()
    {
        _currPhase += 1;
        _plantSprite.Texture = PlantPhaseTextures[_currPhase];
        _waterLevel -= WaterConsumptionPerPhase;
    }

    public void AddWater(int amount)
    {
        _waterLevel += amount;
    }

    public void Seed()
    {
        _waterLevelLabel.Modulate = Colors.White;
        HasSeed = true;
    }
}