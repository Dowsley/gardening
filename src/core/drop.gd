extends Node2D
class_name Drop


@export var time_of_existence_after_impact: float = 0.2

var falling_sprite: Sprite2D
var impact_sprite: Sprite2D
var velocity := Vector2.ZERO
var gravity := 0.0
var impacted := false


func _ready() -> void:
	falling_sprite = get_node("FallingSprite") as Sprite2D
	impact_sprite = get_node("ImpactSprite") as Sprite2D

	gravity = Globals.gravity_scale
	velocity.y += gravity


func _process(delta: float) -> void:
	if not impacted:
		velocity.y += float(ProjectSettings.get_setting("physics/2d/default_gravity")) * gravity * delta
		position += velocity * delta


func _on_collision_area_body_entered(body: Node) -> void:
	if body.name == "Floor":
		impacted = true
		falling_sprite.visible = false
		impact_sprite.visible = true

		var color_rect = body.get_node("ColorRect") as ColorRect
		position = Vector2(position.x, color_rect.position.y - 3)

		var timer = get_tree().create_timer(time_of_existence_after_impact)
		timer.connect("timeout", Callable(self, "_on_timer_timeout"))
	elif body is Pot:
		body.add_water(0.01)
		impacted = true
		queue_free()


func _on_timer_timeout() -> void:
	queue_free()
