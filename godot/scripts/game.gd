extends Node3D

const PlayerScript = preload("res://scripts/player.gd")
const EnemyScript = preload("res://scripts/enemy.gd")

var player: ValleyPlayer
var camera_pivot: Node3D
var joystick_vector := Vector2.ZERO
var health_bar: ProgressBar
var status_label: Label
var game_over_label: Label
var camera_dragging := false
var last_touch := Vector2.ZERO
var camera_yaw := 0.0
var camera_pitch := -0.22

func _ready() -> void:
	_build_world()
	_spawn_player()
	_spawn_enemies()
	_build_hud()

func _process(delta: float) -> void:
	if is_instance_valid(player) and is_instance_valid(camera_pivot):
		camera_pivot.global_position = camera_pivot.global_position.lerp(player.global_position + Vector3.UP * 1.35, delta * 8.0)
		camera_pivot.rotation.y = camera_yaw
		camera_pivot.rotation.x = camera_pitch

func _build_world() -> void:
	var world := WorldEnvironment.new()
	var environment := Environment.new()
	environment.background_mode = Environment.BG_COLOR
	environment.background_color = Color(0.12, 0.16, 0.22)
	environment.ambient_light_source = Environment.AMBIENT_SOURCE_COLOR
	environment.ambient_light_color = Color(0.55, 0.48, 0.39)
	environment.ambient_light_energy = 0.7
	world.environment = environment
	add_child(world)
	var sun := DirectionalLight3D.new()
	sun.rotation_degrees = Vector3(-48, -32, 0)
	sun.light_energy = 1.35
	sun.shadow_enabled = true
	add_child(sun)
	_make_box("Sand Arena", Vector3(0, -0.35, 0), Vector3(42, 0.7, 42), Color(0.55, 0.38, 0.22), true)
	for x in [-13.0, 13.0]:
		for z in [-13.0, 13.0]:
			_make_box("Ancient Pillar", Vector3(x, 2.2, z), Vector3(1.5, 4.4, 1.5), Color(0.63, 0.51, 0.35), true)
	_make_box("Black Gate Left", Vector3(-5.2, 3.4, -17.0), Vector3(2.2, 6.8, 1.6), Color(0.19, 0.17, 0.16), true)
	_make_box("Black Gate Right", Vector3(5.2, 3.4, -17.0), Vector3(2.2, 6.8, 1.6), Color(0.19, 0.17, 0.16), true)
	_make_box("Black Gate Top", Vector3(0, 6.4, -17.0), Vector3(12.5, 1.8, 1.8), Color(0.22, 0.18, 0.14), true)
	for i in range(7):
		_make_box("Broken Stone", Vector3(-8.0 + i * 2.5, 0.35, -8.0 - float(i % 2) * 2.0), Vector3(1.0, 0.7, 0.8), Color(0.39, 0.31, 0.24), true)

func _make_box(label: String, pos: Vector3, size: Vector3, color: Color, solid: bool) -> void:
	var body := StaticBody3D.new() if solid else Node3D.new()
	body.name = label
	body.position = pos
	var mesh := MeshInstance3D.new()
	var box := BoxMesh.new()
	box.size = size
	mesh.mesh = box
	var material := StandardMaterial3D.new()
	material.albedo_color = color
	material.roughness = 0.95
	mesh.material_override = material
	body.add_child(mesh)
	if solid:
		var collision := CollisionShape3D.new()
		var shape := BoxShape3D.new()
		shape.size = size
		collision.shape = shape
		body.add_child(collision)
	add_child(body)

func _make_actor_mesh(actor: CharacterBody3D, color: Color, height: float) -> void:
	var mesh := MeshInstance3D.new()
	var capsule := CapsuleMesh.new()
	capsule.radius = 0.48
	capsule.height = height
	mesh.mesh = capsule
	var material := StandardMaterial3D.new()
	material.albedo_color = color
	material.metallic = 0.12
	material.roughness = 0.7
	mesh.material_override = material
	mesh.position.y = height * 0.5
	actor.add_child(mesh)
	var collision := CollisionShape3D.new()
	var shape := CapsuleShape3D.new()
	shape.radius = 0.48
	shape.height = height
	collision.shape = shape
	collision.position.y = height * 0.5
	actor.add_child(collision)

func _spawn_player() -> void:
	player = ValleyPlayer.new()
	player.name = "Sekhem"
	player.game = self
	player.position = Vector3(0, 0.2, 5)
	_make_actor_mesh(player, Color(0.12, 0.39, 0.42), 1.9)
	add_child(player)
	camera_pivot = Node3D.new()
	camera_pivot.name = "CameraPivot"
	camera_pivot.position = player.position + Vector3.UP * 1.35
	add_child(camera_pivot)
	var camera := Camera3D.new()
	camera.position = Vector3(0, 2.0, 5.2)
	camera.rotation.x = -0.34
	camera_pivot.add_child(camera)
	camera.make_current()

func _spawn_enemies() -> void:
	for i in range(4):
		var enemy := ValleyEnemy.new()
		enemy.name = "Shadow Raider %d" % (i + 1)
		enemy.player = player
		enemy.position = Vector3(-6.0 + float(i) * 4.0, 0.2, -5.0 - float(i % 2) * 2.0)
		_make_actor_mesh(enemy, Color(0.25, 0.19, 0.24), 1.8)
		add_child(enemy)

func _build_hud() -> void:
	var layer := CanvasLayer.new()
	add_child(layer)
	var root := Control.new()
	root.set_anchors_and_offsets_preset(Control.PRESET_FULL_RECT)
	layer.add_child(root)
	health_bar = ProgressBar.new()
	health_bar.position = Vector2(28, 26)
	health_bar.size = Vector2(245, 24)
	health_bar.max_value = 1.0
	health_bar.value = 1.0
	root.add_child(health_bar)
	var title := Label.new()
	title.text = "SEKHEM  |  THE BLACK GATE"
	title.position = Vector2(28, 55)
	title.add_theme_font_size_override("font_size", 17)
	root.add_child(title)
	status_label = Label.new()
	status_label.text = "Defeat the Shadow Raiders"
	status_label.position = Vector2(28, 82)
	root.add_child(status_label)
	game_over_label = Label.new()
	game_over_label.text = ""
	game_over_label.set_anchors_and_offsets_preset(Control.PRESET_CENTER)
	game_over_label.add_theme_font_size_override("font_size", 36)
	root.add_child(game_over_label)
	_add_joystick(root)
	_add_action_button(root, "ATTACK", Vector2(-105, -105), Vector2(92, 92), "attack")
	_add_action_button(root, "JUMP", Vector2(-215, -80), Vector2(72, 72), "jump")
	_add_action_button(root, "DODGE", Vector2(-205, -175), Vector2(72, 72), "dodge")
	_add_action_button(root, "PULL", Vector2(-315, -115), Vector2(68, 68), "pull")
	var hint := Label.new()
	hint.text = "DRAG RIGHT SIDE TO LOOK"
	hint.set_anchors_and_offsets_preset(Control.PRESET_BOTTOM_WIDE)
	hint.position = Vector2(0, -32)
	hint.horizontal_alignment = HORIZONTAL_ALIGNMENT_CENTER
	hint.add_theme_font_size_override("font_size", 13)
	root.add_child(hint)

func _add_joystick(root: Control) -> void:
	var pad := Control.new()
	pad.position = Vector2(36, -205)
	pad.set_anchors_preset(Control.PRESET_BOTTOM_LEFT)
	pad.size = Vector2(160, 160)
	pad.gui_input.connect(_on_joystick_input)
	root.add_child(pad)
	var ring := ColorRect.new()
	ring.color = Color(0.08, 0.1, 0.12, 0.38)
	ring.size = pad.size
	ring.mouse_filter = Control.MOUSE_FILTER_IGNORE
	pad.add_child(ring)
	var label := Label.new()
	label.text = "MOVE"
	label.set_anchors_and_offsets_preset(Control.PRESET_FULL_RECT)
	label.horizontal_alignment = HORIZONTAL_ALIGNMENT_CENTER
	label.vertical_alignment = VERTICAL_ALIGNMENT_CENTER
	label.add_theme_font_size_override("font_size", 18)
	label.mouse_filter = Control.MOUSE_FILTER_IGNORE
	pad.add_child(label)

func _on_joystick_input(event: InputEvent) -> void:
	if event is InputEventScreenTouch:
		if event.pressed:
			joystick_vector = Vector2.ZERO
		else:
			joystick_vector = Vector2.ZERO
	elif event is InputEventScreenDrag:
		var center := Vector2(80, 80)
		joystick_vector = ((event.position - center) / 65.0).limit_length(1.0)

func _add_action_button(root: Control, caption: String, offset: Vector2, button_size: Vector2, action: String) -> void:
	var button := Button.new()
	button.text = caption
	button.size = button_size
	button.anchor_left = 1.0
	button.anchor_right = 1.0
	button.anchor_top = 1.0
	button.anchor_bottom = 1.0
	button.offset_left = offset.x
	button.offset_right = offset.x + button_size.x
	button.offset_top = offset.y
	button.offset_bottom = offset.y + button_size.y
	button.add_theme_font_size_override("font_size", 15)
	button.modulate = Color(0.8, 0.9, 0.92, 0.9)
	button.pressed.connect(_on_action_pressed.bind(action))
	root.add_child(button)

func _on_action_pressed(action: String) -> void:
	if not is_instance_valid(player):
		return
	match action:
		"attack":
			player.do_attack()
		"jump":
			player.do_jump()
		"dodge":
			player.do_dodge()
		"pull":
			player.do_pull()

func _input(event: InputEvent) -> void:
	if event is InputEventScreenTouch:
		if event.position.x > get_viewport().get_visible_rect().size.x * 0.42:
			camera_dragging = event.pressed
			last_touch = event.position
	elif event is InputEventScreenDrag and camera_dragging:
		var delta := event.position - last_touch
		camera_yaw -= delta.x * 0.006
		camera_pitch = clampf(camera_pitch - delta.y * 0.003, -0.65, 0.12)
		last_touch = event.position

func update_health(value: float) -> void:
	if is_instance_valid(health_bar):
		health_bar.value = value

func show_game_over() -> void:
	if is_instance_valid(game_over_label):
		game_over_label.text = "SEKHEM HAS FALLEN"
