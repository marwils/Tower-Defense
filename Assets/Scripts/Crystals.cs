using UnityEngine.InputSystem;

using UnityEngine;

public class Crystals : MonoBehaviour
{

    [SerializeField]
    private GameObject _largeCrystalPrefab;
    [SerializeField]
    private GameObject _smallCrystalPrefab;
    [SerializeField]
    private int _smallCrystalCapacity = 4;

    private float _radius = .25f;

    private int _crystalCount = 0;

    void Start()
    {
        InvokeRepeating("CreateNew", 1f, 5f);
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
        transform.RotateAround(transform.position, Vector3.up, .2f);
    }

    void CreateNew()
    {
        if (_crystalCount < _smallCrystalCapacity)
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
        float angleStep = 360f / _smallCrystalCapacity;

        float angle = _crystalCount++ * angleStep;
        float rad = angle * Mathf.Deg2Rad;

        Vector3 localOffset = new Vector3(
            Mathf.Cos(rad) * _radius,
            0.44375f,
            Mathf.Sin(rad) * _radius
        );

        Vector3 spawnPosition = transform.TransformPoint(localOffset);

        Instantiate(_smallCrystalPrefab, spawnPosition, _smallCrystalPrefab.transform.rotation, transform);
    }

    private void CreateLargeCrystal()
    {
        Instantiate(_largeCrystalPrefab, _largeCrystalPrefab.transform.position, _largeCrystalPrefab.transform.rotation, transform);
        CancelInvoke("CreateNew");
    }

    // Update is called once per frame
    public void FarmCrystals()
    {
        if (_crystalCount == 0)
        {
            return;
        }
        CancelInvoke("CreateNew");
        CrystalAnimator[] crystalAnimators = gameObject.GetComponentsInChildren<CrystalAnimator>();
        foreach (CrystalAnimator crystalAnimator in crystalAnimators)
        {
            crystalAnimator.StartDestroy();
            _crystalCount = 0;
        }
        InvokeRepeating("CreateNew", 5f, 5f);
    }
}
