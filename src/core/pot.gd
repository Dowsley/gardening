extends StaticBody2D
class_name Pot


const TOTAL_WATER_LEVEL_STAGES := 15
const PLANT_PHASE_TEXTURES := [
	null,
	preload("res://assets/sprites/plants/probe/0.png"),
	preload("res://assets/sprites/plants/probe/1.png"),
	preload("res://assets/sprites/plants/probe/2.png"),
	preload("res://assets/sprites/plants/probe/3.png"),
	preload("res://assets/sprites/plants/probe/4.png"),
]

@export var water_consumption_per_phase: float = 0.2
@export var growth_particle_effect_scene: PackedScene = null

var has_seed := false
@onready var plant_sprite : Sprite2D = get_node("PlantSprite")
@onready var water_level_label: Label = get_node("WaterLevelLabel")
@onready var water_level_mask: Light2D = get_node("WaterLevelMask") as Light2D

var water_level_mask_pivot: Vector2
var curr_plant_phase := 0
var water_level := 0.0


func _ready() -> void:
	plant_sprite.texture = null
	water_level_mask_pivot = water_level_mask.position
	water_level_label.modulate = Color.RED


func _process(delta: float) -> void:
	var stage = water_level_to_stage()
	water_level_mask.position = Vector2(water_level_mask_pivot.x, water_level_mask_pivot.y - (stage * 1))
	water_level_label.visible = Globals.water_level_label_visible
	water_level_label.text = str(get_fill_percentage())
	if not has_seed or curr_plant_phase >= len(PLANT_PHASE_TEXTURES) - 1:
		return

	if water_level >= water_consumption_per_phase:
		grow_to_next_phase()


func add_water(amount: float) -> void:
	water_level += amount
	water_level = min(water_level, 1.0) # Ensuring water level does not exceed 100%


func seed() -> void:
	water_level_label.modulate = Color.WHITE
	has_seed = true


func water_level_to_stage() -> int:
	var index := int(water_level * TOTAL_WATER_LEVEL_STAGES)
	return index


func grow_to_next_phase() -> void:
	curr_plant_phase += 1
	plant_sprite.texture = PLANT_PHASE_TEXTURES[curr_plant_phase]
	water_level -= water_consumption_per_phase

	# Particle effect
	if growth_particle_effect_scene == null:
		return
	var particle := growth_particle_effect_scene.instance() as CPUParticles2D
	particle.position = plant_sprite.global_position
	particle.rotation = global_rotation
	particle.emitting = true

	get_tree().current_scene.add_child(particle)


func get_fill_percentage() -> int:
	return int(water_level * 100)
