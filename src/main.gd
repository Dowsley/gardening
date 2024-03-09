extends Node2D

@onready var blind_sprite_anim: AnimationPlayer = $Environment/BlindAnimationPlayer
@onready var blind_button: ColorRect = $Environment/BlindButton

enum BlindStates {
	OPEN,
	CLOSED
}

var current_blind_state := BlindStates.CLOSED


func _ready() -> void:
	pass


func _process(delta: float) -> void:
		if Input.is_action_just_pressed("show_labels"):
			Globals.water_level_label_visible = not Globals.water_level_label_visible


func _on_blind_button_gui_input(event: InputEvent):
	if event is InputEventMouseButton and event.button_index == MOUSE_BUTTON_LEFT:
		if event.pressed:
			if current_blind_state == BlindStates.CLOSED:
				blind_button.color = Color.GREEN
				blind_sprite_anim.play('open_blind')
				current_blind_state = BlindStates.OPEN
			else:
				blind_button.color = Color.RED
				blind_sprite_anim.play('close_blind')
				current_blind_state = BlindStates.CLOSED
