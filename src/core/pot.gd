extends StaticBody2D
class_name Pot


const TOTAL_WATER_LEVEL_STAGES := 15
const PLANT_PHASE_TEXTURES := [
	null,
	preload("res://assets/sprites/plants/probe/0.png"),
	preload("res://assets/sprites/plants/probe/1.png"),
	preload("res://assets/sprites/plants/probe/2.png"),
	preload("res://assets/sprites/plants/probe/3.png"),
	preload("res://assets/sprites/plants/probe/4_alt.png"),
]

@export var water_consumption_per_phase: float = 0.2
@export var growth_particle_effect_scene: PackedScene = null
@export var tentacle_activation_radius: int = 35

@onready var plant_sprite : Sprite2D = $PlantSprite
@onready var water_level_label: Label = $WaterLevelLabel
@onready var water_level_mask: Light2D = $WaterLevelMask as Light2D
@onready var tentacle1: Node2D = $Tentacle1 as Node2D
@onready var tentacle2: Node2D = $Tentacle2 as Node2D
@onready var tentacle_origin: Vector2 = $TentacleOriginMarker.position
@onready var tentacle_rest1: Vector2 = $Tentacle1RestMarker.position
@onready var tentacle_rest2: Vector2 = $Tentacle2RestMarker.position
@onready var tentacle_end1: Sprite2D = $TentacleEnd1
@onready var tentacle_end2: Sprite2D = $TentacleEnd2

var is_mouse_near_tentacle_origin := false

var has_seed := false
var water_level_mask_pivot: Vector2
var curr_plant_phase := 0
var water_level := 0.0


func _ready() -> void:
	plant_sprite.texture = null
	water_level_mask_pivot = water_level_mask.position
	water_level_label.modulate = Color.RED
	tentacle1.visible = false
	tentacle2.visible = false
	tentacle_end1.visible = false
	tentacle_end2.visible = false
	tentacle1.set_start(tentacle_origin)
	tentacle1.set_last(tentacle_rest1)
	tentacle2.set_start(tentacle_origin)
	tentacle2.set_last(tentacle_rest2)


func _process(delta: float) -> void:
	# Tentacle 1 logic
	var mouse_position_local := to_local(get_global_mouse_position())
	is_mouse_near_tentacle_origin = mouse_position_local.distance_to(tentacle_origin) < tentacle_activation_radius
	
	if is_mouse_near_tentacle_origin:
		tentacle1.set_last(mouse_position_local)
		tentacle2.set_last(mouse_position_local)
	else:
		tentacle1.set_last(tentacle_rest1)
		tentacle2.set_last(tentacle_rest2)
	# update tentacle end rotation
	var last_point = tentacle1.pos[tentacle1.point_count - 1]
	var second_last_point = tentacle1.pos[tentacle1.point_count - 2]
	var direction = last_point - second_last_point
	var angle = direction.angle() # Get the angle in radians
	tentacle_end1.rotation = angle
	tentacle_end1.position = last_point
	
	last_point = tentacle2.pos[tentacle2.point_count - 1]
	second_last_point = tentacle2.pos[tentacle2.point_count - 2]
	direction = last_point - second_last_point
	angle = direction.angle() # Get the angle in radians
	tentacle_end2.rotation = angle
	tentacle_end2.position = last_point

	# Pot logic
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
	if curr_plant_phase == PLANT_PHASE_TEXTURES.size() - 1:
		tentacle1.visible = true
		tentacle2.visible = true
		tentacle_end1.visible = true
		tentacle_end2.visible = true
	plant_sprite.texture = PLANT_PHASE_TEXTURES[curr_plant_phase]
	water_level -= water_consumption_per_phase


func get_fill_percentage() -> int:
	return int(water_level * 100)
