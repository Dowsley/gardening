using Godot;

public class MouseInputUtil : Godot.Node
{
    public static bool IsLeftMouseButtonPressed(InputEventMouseButton eventMouseButton)
    {
        return eventMouseButton.Pressed && eventMouseButton.ButtonIndex == (int)ButtonList.Left;
    }

    public static bool IsRightMouseButtonPressed(InputEventMouseButton eventMouseButton)
    {
        return eventMouseButton.Pressed && eventMouseButton.ButtonIndex == (int)ButtonList.Right;
    }

    public static bool IsMiddleMouseButtonPressed(InputEventMouseButton eventMouseButton)
    {
        return eventMouseButton.Pressed && eventMouseButton.ButtonIndex == (int)ButtonList.Middle;
    }
    
    public static bool IsLeftMouseButtonReleased(InputEventMouseButton eventMouseButton)
    {
        return !eventMouseButton.Pressed && eventMouseButton.ButtonIndex == (int)ButtonList.Left;
    }

    public static bool IsRightMouseButtonReleased(InputEventMouseButton eventMouseButton)
    {
        return !eventMouseButton.Pressed && eventMouseButton.ButtonIndex == (int)ButtonList.Right;
    }

    public static bool IsMiddleMouseButtonReleased(InputEventMouseButton eventMouseButton)
    {
        return !eventMouseButton.Pressed && eventMouseButton.ButtonIndex == (int)ButtonList.Middle;
    }
}