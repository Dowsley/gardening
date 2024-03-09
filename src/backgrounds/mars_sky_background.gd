extends Node2D

const MOUNTAIN_ASSET_PREFIX = 'res://assets/sprites/backgrounds/milky_way_mountains/moutains/transparent/'
const ASSET_SUFFIX = '.png'

@onready var stars_animated_sprite: AnimatedSprite2D = $Stars
@onready var mountains_animated_sprite: AnimatedSprite2D = $Mountains

var reverse_anim := false

func _ready():
	play_anim(stars_animated_sprite)
	play_anim(mountains_animated_sprite)
	reverse_anim = not reverse_anim


func play_anim(sprite: AnimatedSprite2D):
	if reverse_anim:
		sprite.play_backwards('default')
	else:
		sprite.play('default')


func _on_animation_finished():
	play_anim(stars_animated_sprite)
	play_anim(mountains_animated_sprite)
	reverse_anim = not reverse_anim
