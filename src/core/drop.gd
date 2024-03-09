extends Node2D
class_name Drop

@export var time_of_existence_after_impact: float = 0.2

var _falling_sprite: Sprite2D
var _impact_sprite: Sprite2D

var _velocity := Vector2.ZERO
var _gravity := 0.0

var _impacted := false

func _ready() -> void:
	_falling_sprite = get_node("FallingSprite") as Sprite2D
	_impact_sprite = get_node("ImpactSprite") as Sprite2D

	_gravity = Globals.gravity_scale
	_velocity.y += _gravity

func _process(delta: float) -> void:
	if not _impacted:
		_velocity.y += float(ProjectSettings.get_setting("physics/2d/default_gravity")) * _gravity * delta
		position += _velocity * delta

func _on_CollisionArea_body_entered(body: Node) -> void:
	if body.name == "Floor":
		_impacted = true
		_falling_sprite.visible = false
		_impact_sprite.visible = true

		var color_rect = body.get_node("ColorRect") as ColorRect
		position = Vector2(position.x, color_rect.position.y - 3)

		var timer = get_tree().create_timer(time_of_existence_after_impact)
		timer.connect("timeout", Callable(self, "_on_Timer_timeout"))
	elif body is Pot:
		body.add_water(0.01)

		_impacted = true
		queue_free()

func _on_Timer_timeout() -> void:
	queue_free()
