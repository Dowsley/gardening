extends Draggable
class_name WateringCan

@export var drops_per_second: int = 5

var _drop_spawn_position: Marker2D

var _time_since_last_spawn := 0.0

func _ready() -> void:
	super._ready()
	_drop_spawn_position = get_node("DropSpawnPosition")

func _process(delta: float) -> void:
	_time_since_last_spawn += delta

	if _is_dragging and Input.is_mouse_button_pressed(MOUSE_BUTTON_RIGHT):
		if _time_since_last_spawn >= 1.0 / drops_per_second:
			rotation_degrees = -45 # It will automatically be reverted to 0 due to the way DraggableRigidBody2D works.
			spawn_drop()
			_time_since_last_spawn = 0

func spawn_drop() -> void:
	var drop_instance := preload("res://src/core/Drop.tscn").instantiate() as Node2D
	drop_instance.global_position = _drop_spawn_position.global_position
	drop_instance.global_position.x
	get_parent().add_child(drop_instance)
