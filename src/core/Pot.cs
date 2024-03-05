using System;
using Godot;
using System.Collections.Generic;

public partial class Pot : StaticBody2D
{
	private const int TotalWaterLevelStages = 5;

	[Export] public float WaterConsumptionPerPhase = 0.2f;
	[Export] public PackedScene GrowthParticleEffectScene = null;

	public bool HasSeed = false;

	private Sprite _plantSprite;
	private Label _waterLevelLabel;
	private Light2D _waterLevelMask;

	private Vector2 _waterLevelMaskPivot;
	private int _currPlantPhase = 0;
	private float _waterLevel = 0.0f;

	private static readonly List<Texture> PlantPhaseTextures = new List<Texture>
	{
		null,
		GD.Load<Texture>("res://assets/plants/Probe/probe_1.png"),
		GD.Load<Texture>("res://assets/plants/Probe/probe_2.png"),
		GD.Load<Texture>("res://assets/plants/Probe/probe_3.png"),
		GD.Load<Texture>("res://assets/plants/Probe/probe_4.png"),
		GD.Load<Texture>("res://assets/plants/Probe/probe_5.png"),
	};

	public override void _Ready()
	{
		_waterLevelLabel = GetNode<Label>("WaterLevelLabel");
		_waterLevelMask = GetNode<Light2D>("WaterLevelMask");
		_waterLevelMaskPivot = _waterLevelMask.Position;
		_plantSprite = GetNode<Sprite>("PlantSprite");
		_plantSprite.Texture = null;
		_waterLevelLabel.Modulate = Colors.Red;
	}

	public override void _Process(float delta)
	{
		_waterLevelLabel.Visible = Globals.Instance.WaterLevelLabelVisible;
		_waterLevelLabel.Text = (_getFillPercentage()).ToString();
		if (!HasSeed || _currPlantPhase >= PlantPhaseTextures.Count - 1)
		{
			return;
		}

		if (_waterLevel >= WaterConsumptionPerPhase)
		{
			_growToNextPhase();
		}
	}

	public void AddWater(float amount)
	{
		_waterLevel += amount;
		_waterLevel = Math.Min(_waterLevel, 1.0f); // Ensuring water level does not exceed 100%

		var stage = _waterLevelToStage();
		_waterLevelMask.Position = new Vector2(_waterLevelMaskPivot.x, _waterLevelMaskPivot.y - (stage * 4)); // Each level is 2 pixels
	}

	public void Seed()
	{
		_waterLevelLabel.Modulate = Colors.White;
		HasSeed = true;
	}


	private int _waterLevelToStage()
	{
		var index = (int)(_waterLevel * TotalWaterLevelStages);
		return index;
	}

	private void _growToNextPhase()
	{
		_currPlantPhase += 1;
		_plantSprite.Texture = PlantPhaseTextures[_currPlantPhase];
		_waterLevel -= WaterConsumptionPerPhase;

		/* Particle effect */
		if (GrowthParticleEffectScene == null)
		{
			return;
		}
		var particle = GrowthParticleEffectScene.Instance<CPUParticles2D>();
		particle.Position = _plantSprite.GlobalPosition;
		particle.Rotation = GlobalRotation;
		particle.Emitting = true;

		GetTree().CurrentScene.AddChild(particle);
	}

	private int _getFillPercentage()
	{
		return (int) (_waterLevel * 100);
	}
}
