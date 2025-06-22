class_name AudioButton
extends Button

@export var hoverAudio: AudioStream
@export var clickAudio: AudioStream

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	var click: AudioStreamPlayer = AudioStreamPlayer.new()
	click.stream = clickAudio
	click.set_name("Click")
	add_child(click)

	var hover: AudioStreamPlayer = AudioStreamPlayer.new()
	hover.stream = hoverAudio
	hover.set_name("Hover")
	add_child(hover);

	self.pressed.connect(_on_click)
	self.mouse_entered.connect(_on_hover)


func _on_click():
	$Click.play()

func _on_hover():
	$Hover.play()
