extends Draggable
class_name Seed

func _ready() -> void:
	super._ready()

func _on_pot_collision_area_area_shape_entered(area_rid, area, area_shape_index, local_shape_index) -> void:
	var body = area.get_parent()
	if body is Pot and not body.has_seed:
		body.seed()
		queue_free()
