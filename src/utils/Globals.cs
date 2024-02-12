using Godot;

public class Globals : Godot.Node
{
    
    private static Globals _instance;
    public static Globals Instance => _instance;
    
    public override void _EnterTree(){
        if(_instance != null){
            this.QueueFree();
        }
        _instance = this;
    }
    
    public const float GravityScale = 7.0f;
    public const float AirResistance = 1.0f;
    public bool WaterLevelLabelVisible = false;
}
