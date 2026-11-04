extends CharacterBody3D
class_name ValleyPlayer

@export var move_speed := 6.5
@export var acceleration := 18.0
@export var jump_velocity := 7.0
@export var attack_range := 3.2
@export var max_health := 100.0

var health := 100.0
var game: Node3D
var attack_cooldown := 0.0
var dodge_timer := 0.0
var invulnerable_timer := 0.0
var target: Node3D
var facing := Vector3.FORWARD
var gravity := 20.0

func _ready() -> void:
	health = max_health
	floor_snap_length = 0.35

func _physics_process(delta: float) -> void:
	attack_cooldown = maxf(0.0, attack_cooldown - delta)
	dodge_timer = maxf(0.0, dodge_timer - delta)
	invulnerable_timer = maxf(0.0, invulnerable_timer - delta)
	var stick := Vector2.ZERO
	if game != null:
		stick = game.joystick_vector
	var keyboard := Vector2(
		Input.get_action_strength("move_right") - Input.get_action_strength("move_left"),
		Input.get_action_strength("move_back") - Input.get_action_strength("move_forward")
	)
	var input_dir := keyboard if keyboard.length() > 0.1 else stick
	var direction := Vector3(input_dir.x, 0.0, input_dir.y)
	if direction.length() > 1.0:
		direction = direction.normalized()
	if game != null and game.camera_pivot != null:
		var basis := game.camera_pivot.global_basis
		direction = (basis.x * direction.x + basis.z * direction.z)
		direction.y = 0.0
		direction = direction.normalized()
	if direction.length() > 0.08:
		facing = direction
		rotation.y = lerp_angle(rotation.y, atan2(-facing.x, -facing.z), delta * 12.0)
	var desired := direction * move_speed
	if dodge_timer > 0.0:
		desired = facing * move_speed * 2.5
	velocity.x = move_toward(velocity.x, desired.x, acceleration * delta)
	velocity.z = move_toward(velocity.z, desired.z, acceleration * delta)
	if not is_on_floor():
		velocity.y -= gravity * delta
	else:
		if velocity.y < 0.0:
			velocity.y = -0.2
	move_and_slide()

func do_jump() -> void:
	if is_on_floor():
		velocity.y = jump_velocity

func do_dodge() -> void:
	if dodge_timer <= 0.0:
		dodge_timer = 0.24
		invulnerable_timer = 0.35

func do_attack() -> void:
	if attack_cooldown > 0.0:
		return
	attack_cooldown = 0.38
	var victim: Node3D = null
	var best := attack_range
	for enemy in get_tree().get_nodes_in_group("enemies"):
		if not is_instance_valid(enemy):
			continue
		var distance := global_position.distance_to(enemy.global_position)
		var toward := (enemy.global_position - global_position).normalized()
		if distance < best and facing.dot(toward) > 0.05:
			best = distance
			victim = enemy
	if victim != null and victim.has_method("take_hit"):
		victim.take_hit(24.0, facing * 8.0 + Vector3.UP * 3.5)

func do_pull() -> void:
	var victim: Node3D = null
	var best := 12.0
	for enemy in get_tree().get_nodes_in_group("enemies"):
		if not is_instance_valid(enemy):
			continue
		var distance := global_position.distance_to(enemy.global_position)
		var toward := (enemy.global_position - global_position).normalized()
		if distance < best and facing.dot(toward) > 0.0:
			best = distance
			victim = enemy
	if victim != null:
		victim.global_position = global_position + facing * 2.0 + Vector3.UP * 0.2
		if victim.has_method("take_hit"):
			victim.take_hit(8.0, facing * 3.0)

func take_hit(amount: float) -> void:
	if invulnerable_timer > 0.0:
		return
	health = maxf(0.0, health - amount)
	if game != null:
		game.update_health(health / max_health)
	if health <= 0.0 and game != null:
		game.show_game_over()
