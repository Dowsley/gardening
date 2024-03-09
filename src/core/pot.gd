extends StaticBody2D
class_name Pot

const TOTAL_WATER_LEVEL_STAGES := 15

@export var water_consumption_per_phase: float = 0.2
@export var growth_particle_effect_scene: PackedScene = null

var has_seed := false

var _plant_sprite: Sprite2D
var _water_level_label: Label
var _water_level_mask: Light2D

var _water_level_mask_pivot: Vector2
var _curr_plant_phase := 0
var _water_level := 0.0

var PLANT_PHASE_TEXTURES := [
	null,
	preload("res://assets/sprites/plants/probe/0.png"),
	preload("res://assets/sprites/plants/probe/1.png"),
	preload("res://assets/sprites/plants/probe/2.png"),
	preload("res://assets/sprites/plants/probe/3.png"),
	preload("res://assets/sprites/plants/probe/4.png"),
]

func _ready() -> void:
	_water_level_label = get_node("WaterLevelLabel") as Label
	_water_level_mask = get_node("WaterLevelMask") as Light2D
	_water_level_mask_pivot = _water_level_mask.position
	_plant_sprite = get_node("PlantSprite") as Sprite2D
	_plant_sprite.texture = null
	_water_level_label.modulate = Color.RED

func _process(delta: float) -> void:
	var stage = _water_level_to_stage()
	_water_level_mask.position = Vector2(_water_level_mask_pivot.x, _water_level_mask_pivot.y - (stage * 1))
	_water_level_label.visible = Globals.water_level_label_visible
	_water_level_label.text = str(_get_fill_percentage())
	if not has_seed or _curr_plant_phase >= len(PLANT_PHASE_TEXTURES) - 1:
		return

	if _water_level >= water_consumption_per_phase:
		_grow_to_next_phase()

func add_water(amount: float) -> void:
	_water_level += amount
	_water_level = min(_water_level, 1.0) # Ensuring water level does not exceed 100%

func seed() -> void:
	_water_level_label.modulate = Color.WHITE
	has_seed = true

func _water_level_to_stage() -> int:
	var index := int(_water_level * TOTAL_WATER_LEVEL_STAGES)
	return index

func _grow_to_next_phase() -> void:
	_curr_plant_phase += 1
	_plant_sprite.texture = PLANT_PHASE_TEXTURES[_curr_plant_phase]
	_water_level -= water_consumption_per_phase

	# Particle effect
	if growth_particle_effect_scene == null:
		return
	var particle := growth_particle_effect_scene.instance() as CPUParticles2D
	particle.position = _plant_sprite.global_position
	particle.rotation = global_rotation
	particle.emitting = true

	get_tree().current_scene.add_child(particle)

func _get_fill_percentage() -> int:
	return int(_water_level * 100)
