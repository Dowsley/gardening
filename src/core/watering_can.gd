extends Draggable
class_name WateringCan


@export var drops_per_second: int = 5
@export var drop_scene: PackedScene

var drop_spawn_position: Marker2D
var time_since_last_spawn := 0.0


func _ready() -> void:
	super._ready()
	drop_spawn_position = get_node("DropSpawnPosition")


func _process(delta: float) -> void:
	time_since_last_spawn += delta
	
	if is_dragging and Input.is_mouse_button_pressed(MOUSE_BUTTON_RIGHT):
		if time_since_last_spawn >= 1.0 / drops_per_second:
			rotation_degrees = -45 # It will automatically be reverted to 0 due to the way DraggableRigidBody2D works.
			spawn_drop()
			time_since_last_spawn = 0


func spawn_drop() -> void:
	var drop_instance := drop_scene.instantiate() as Node2D
	drop_instance.global_position = drop_spawn_position.global_position
	drop_instance.global_position.x
	get_parent().add_child(drop_instance)
