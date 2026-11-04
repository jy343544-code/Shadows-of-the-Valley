using UnityEngine;
using UnityEngine.UI;

// Wires the generated on-screen controls to the gameplay components.
public class MobileCombatHUD : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private PlayerCombat combat;
    [SerializeField] private ShadowTether tether;
    [SerializeField] private AirComboCombat airCombat;
    [SerializeField] private TargetLock targetLock;
    [SerializeField] private Button jumpButton;
    [SerializeField] private Button dodgeButton;
    [SerializeField] private Button attackButton;
    [SerializeField] private Button tetherButton;
    [SerializeField] private Button launcherButton;
    [SerializeField] private Button airAttackButton;
    [SerializeField] private Button slamButton;
    [SerializeField] private Button lockButton;

    private void Awake()
    {
        if (jumpButton != null && player != null) jumpButton.onClick.AddListener(player.Jump);
        if (dodgeButton != null && player != null) dodgeButton.onClick.AddListener(player.Dodge);
        if (attackButton != null && combat != null) attackButton.onClick.AddListener(combat.Attack);
        if (tetherButton != null && tether != null) tetherButton.onClick.AddListener(tether.Cast);
        if (launcherButton != null && airCombat != null) launcherButton.onClick.AddListener(airCombat.LaunchTarget);
        if (airAttackButton != null && airCombat != null) airAttackButton.onClick.AddListener(airCombat.AirAttack);
        if (slamButton != null && airCombat != null) slamButton.onClick.AddListener(airCombat.GroundSlam);
        if (lockButton != null && targetLock != null) lockButton.onClick.AddListener(targetLock.ToggleLock);
    }
}