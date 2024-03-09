extends Node2D


@export var mouse_debug := false
@export var rope_length := 30.0
@export var constraint := 1.0 # Distance between points
@export var gravity := Vector2(0,9.8)
@export var dampening := 0.9
@export var start_pin := true
@export var end_pin := true
@export var default_color := Color.WHITE

@onready var line2D := $Line2D

var pos: PackedVector2Array
var prev_pos: PackedVector2Array
var point_count: int


func _ready() -> void:
	line2D.default_color = default_color
	point_count = get_point_count(rope_length)
	resize_arrays()
	init_position()


func get_point_count(distance: float) -> int:
	return int(ceil(distance / constraint))


func resize_arrays():
	pos.resize(point_count)
	prev_pos.resize(point_count)


func init_position() -> void:
	for i in range(point_count):
		pos[i] = position + Vector2(constraint * i, 0)
		prev_pos[i] = position + Vector2(constraint * i, 0)
	position = Vector2.ZERO


func _process(delta)->void:
	update_points(delta)
	update_constraint()
	
	#update_constrain()	#Repeat to get tighter rope
	#update_constrain()
	
	# Send positions to Line2D for drawing
	line2D.points = pos


func set_start(p: Vector2) -> void:
	pos[0] = p
	prev_pos[0] = p

func _unhandled_input(event:InputEvent)->void:
	if mouse_debug:
		if event is InputEventMouseMotion:
			if Input.is_action_pressed("click"):
				set_start(get_global_mouse_position())
			if Input.is_action_pressed("right_click"):
				set_last(get_global_mouse_position())
		elif event is InputEventMouseButton && event.is_pressed():
			if event.button_index == 1:
				set_start(get_global_mouse_position())
			elif event.button_index == 2:
				set_last(get_global_mouse_position())



func set_last(p: Vector2) -> void:
	var end_point
	var move_amount
	if not mouse_debug:
		end_point = pos[point_count - 1]
		var move_direction = (p - end_point).normalized()
		move_amount = move_direction * 0.4 # Adjust speed as necessary
	
	# Move towards the target position, but stop if we overshoot
	if not mouse_debug and end_point.distance_to(p) > move_amount.length():
		pos[point_count - 1] += move_amount
		prev_pos[point_count - 1] = pos[point_count - 1]
	else:
		pos[point_count - 1] = p
		prev_pos[point_count - 1] = p


func update_points(delta) -> void:
	for i in range (point_count):
		# not first and last || first if not pinned || last if not pinned
		if (i != 0 && i != point_count - 1) || (i == 0 && !start_pin) || (i == point_count - 1 && !end_pin):
			var velocity = (pos[i] - prev_pos[i]) * dampening
			prev_pos[i] = pos[i]
			pos[i] += velocity + (gravity * delta)


func update_constraint() -> void:
	for i in range(point_count):
		if i == point_count - 1:
			return
		var distance = pos[i].distance_to(pos[i+1])
		var difference = constraint - distance
		var percent = difference / distance
		var vec2 = pos[i+1] - pos[i]
		
		# if first point
		if i == 0:
			if start_pin:
				pos[i+1] += vec2 * percent
			else:
				pos[i] -= vec2 * (percent/2)
				pos[i+1] += vec2 * (percent/2)
		# if last point, skip because no more points after it
		elif i == point_count - 1:
			pass
		# all the rest
		else:
			if i+1 == point_count - 1 && end_pin:
				pos[i] -= vec2 * percent
			else:
				pos[i] -= vec2 * (percent/2)
				pos[i+1] += vec2 * (percent/2)
