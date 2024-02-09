using Godot;
using System.Collections.Generic;

public partial class Pot : StaticBody2D
{
    private static readonly List<Texture> PlantPhaseTextures = new List<Texture>
    {
        GD.Load<Texture>("res://assets/plants/Classic/1.png"),
        GD.Load<Texture>("res://assets/plants/Classic/2.png"),
        GD.Load<Texture>("res://assets/plants/Classic/3.png"),
    };

    private int _currPlantTexture = 0;
    private Sprite _plantSprite;
    
    public override void _Ready()
    {
        _plantSprite = GetNode<Sprite>("PlantSprite");
        _plantSprite.Texture = PlantPhaseTextures[_currPlantTexture];
    }

    public override void _Process(float delta)
    {
        
    }
    
    private void _on_ClickableArea_input_event(Viewport viewport, InputEvent @event, int shapeIdx)
    {
        if (@event is InputEventMouseButton mouseButtonEvent)
        {
            if (mouseButtonEvent.Pressed && mouseButtonEvent.ButtonIndex == (int)ButtonList.Left && _currPlantTexture != PlantPhaseTextures.Count - 1)
            {
                _currPlantTexture += 1;
            } else if (mouseButtonEvent.Pressed && mouseButtonEvent.ButtonIndex == (int)ButtonList.Right && _currPlantTexture != 0)
            {
                _currPlantTexture -= 1;
            }
            _plantSprite.Texture = PlantPhaseTextures[_currPlantTexture];
        }
    }
}