extends Control
@onready var onpress = $TextureRect/AnimationPlayer
# Called when the node enters the scene tree for the first time.
func _ready():
	pass


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta):
	if Input.is_action_just_pressed("start"):
		onpress.play("onpress")

func _on_animation_player_animation_finished(anim_name):
	if anim_name == "onpress":
		get_tree().change_scene_to_file("res://scenes/node_2d.tscn")
	pass # Replace with function body.
