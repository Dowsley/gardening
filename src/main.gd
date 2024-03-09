extends Node2D

var _available_sky_backgrounds: Array = []
var _curr_sky_background: int = 0

func _ready() -> void:
	for child in get_node("Environment/BG").get_children():
		_available_sky_backgrounds.append(child)

func _process(delta: float) -> void:
	if Input.is_action_just_pressed("ui_accept"):
		set_next_sky_background()
	if Input.is_action_just_pressed("show_labels"):
		Globals.water_level_label_visible = not Globals.water_level_label_visible

func set_next_sky_background() -> void:
	_available_sky_backgrounds[_curr_sky_background].visible = false
	_curr_sky_background += 1
	if _curr_sky_background >= _available_sky_backgrounds.size():
		_curr_sky_background = 0
	_available_sky_backgrounds[_curr_sky_background].visible = true
