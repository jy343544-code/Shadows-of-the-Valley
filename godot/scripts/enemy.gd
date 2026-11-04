extends CharacterBody3D
class_name ValleyEnemy

@export var max_health := 60.0
@export var move_speed := 2.8
var health := 60.0
var player: Node3D
var attack_timer := 0.0
var knockback := Vector3.ZERO
var gravity := 20.0
var dead := false

func _ready() -> void:
	health = max_health
	add_to_group("enemies")

func _physics_process(delta: float) -> void:
	if dead or not is_instance_valid(player):
		return
	attack_timer = maxf(0.0, attack_timer - delta)
	var to_player := player.global_position - global_position
	to_player.y = 0.0
	var distance := to_player.length()
	if distance > 1.55:
		var direction := to_player.normalized()
		velocity.x = move_toward(velocity.x, direction.x * move_speed, 10.0 * delta)
		velocity.z = move_toward(velocity.z, direction.z * move_speed, 10.0 * delta)
		rotation.y = lerp_angle(rotation.y, atan2(-direction.x, -direction.z), delta * 5.0)
	else:
		velocity.x = move_toward(velocity.x, 0.0, 14.0 * delta)
		velocity.z = move_toward(velocity.z, 0.0, 14.0 * delta)
		if attack_timer <= 0.0 and player.has_method("take_hit"):
			attack_timer = 1.4
			player.take_hit(9.0)
	velocity.x += knockback.x
	velocity.z += knockback.z
	knockback = knockback.move_toward(Vector3.ZERO, 12.0 * delta)
	if not is_on_floor():
		velocity.y -= gravity * delta
	else:
		velocity.y = -0.2
	move_and_slide()

func take_hit(amount: float, impulse: Vector3) -> void:
	if dead:
		return
	health -= amount
	knockback += impulse
	if health <= 0.0:
		dead = true
		queue_free()
