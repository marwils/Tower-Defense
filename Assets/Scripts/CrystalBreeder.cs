using UnityEngine.InputSystem;

using UnityEngine;

public class CrystalBreeder : MonoBehaviour
{
    [Header("Breeding")]

    [SerializeField]
    [Tooltip("Initial delay (in seconds) before breeding starts.")]
    private float _delay = 1f;

    [SerializeField]
    [Tooltip("Time interval (in seconds) between consecutive breeding attempts.")]
    [Min(0f)]
    private float _interval = 5f;

    [SerializeField]
    [Tooltip("Number of small crystals to spawn before creating the large one.")]
    private int _smallCrystalAmount = 4;

    [Header("Prefabs")]

    [SerializeField]
    private GameObject _largeCrystalPrefab;

    [SerializeField]
    private GameObject _smallCrystalPrefab;

    [Space]

    [SerializeField]
    [Tooltip("Rotation speed (degrees per frame) of the breeder object.")]
    private float _spinningVelocity = .2f;

    private const float Radius = .25f;

    private int _smallCrystalCount = 0;

    private void Start()
    {
        StartBreeding();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider is BoxCollider)
                {
                    FarmCrystals();
                }
            }
        }
        transform.RotateAround(transform.position, Vector3.up, _spinningVelocity);
    }

    private void Breed()
    {
        if (_smallCrystalCount < _smallCrystalAmount)
        {
            CreateSmallCrystal();
        }
        else
        {
            CreateLargeCrystal();
        }
    }

    private void CreateSmallCrystal()
    {
        float angleStep = 360f / _smallCrystalAmount;

        float angle = _smallCrystalCount++ * angleStep;
        float rad = angle * Mathf.Deg2Rad;

        Vector3 localOffset = new Vector3(
            Mathf.Cos(rad) * Radius,
            0.44375f,
            Mathf.Sin(rad) * Radius
        );

        Vector3 spawnPosition = transform.TransformPoint(localOffset);

        Instantiate(_smallCrystalPrefab, spawnPosition, _smallCrystalPrefab.transform.rotation, transform);
    }

    private void CreateLargeCrystal()
    {
        Instantiate(_largeCrystalPrefab, _largeCrystalPrefab.transform.position, _largeCrystalPrefab.transform.rotation, transform);
        StopBreeding();
    }

    private void FarmCrystals()
    {
        if (_smallCrystalCount == 0)
        {
            return;
        }

        StopBreeding();

        CrystalAnimator[] crystalAnimators = gameObject.GetComponentsInChildren<CrystalAnimator>();
        foreach (CrystalAnimator crystalAnimator in crystalAnimators)
        {
            crystalAnimator.StartDestroy();
            _smallCrystalCount = 0;
        }

        StartBreeding();
    }

    private void StartBreeding()
    {
        InvokeRepeating("Breed", _delay, _interval);
    }

    private void StopBreeding()
    {
        CancelInvoke("Breed");
    }
}
