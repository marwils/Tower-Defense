using System.Collections.Generic;

using Helper;

using UnityEngine;

public class CrystalBreeder : MonoBehaviour, ISelectable
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

    private int _crystalCount = -1;

    private int _currentSmallCrystalAmount;

    private List<GameObject> _smallCrystalInstances = new();
    private GameObject _largeCrystalInstance;

    private void CollectCrystals()
    {
        if (_crystalCount == 0)
        {
            return;
        }

        StopBreeding();

        CrystalAnimator[] crystalAnimators = gameObject.GetComponentsInChildren<CrystalAnimator>();
        foreach (CrystalAnimator crystalAnimator in crystalAnimators)
        {
            if (crystalAnimator != null)
            {
                crystalAnimator.StartDestroy();
            }
        }

        // Note: Instances are disabled when their animation state exits.

        _crystalCount = 0;

        StartBreeding();
    }

    private void Breed()
    {
        if (_crystalCount < _currentSmallCrystalAmount)
        {
            BreedSmallCrystal(_crystalCount);
        }
        else
        {
            BreedLargeCrystal();
            StopBreeding();
        }
        _crystalCount++;
    }

    private void BreedSmallCrystal(int crystalCount)
    {
        GameObject smallCrystal = _smallCrystalInstances[crystalCount];
        TransformHelper.SetTransformation(smallCrystal.transform, _smallCrystalPrefab.transform, TransformHelper.TransformationType.Rotation, TransformHelper.TransformationType.Scale);
        smallCrystal.SetActive(true);
    }

    private void BreedLargeCrystal()
    {
        GameObject largeCrystal = _largeCrystalInstance;
        TransformHelper.SetTransformation(largeCrystal.transform, _largeCrystalPrefab.transform);
        largeCrystal.SetActive(true);
    }

    private void InstantiateCrystals()
    {
        for (int crystalIndex = 0; crystalIndex < _currentSmallCrystalAmount; crystalIndex++)
        {
            GameObject smallCrystal = InstanciateSmallCrystal(crystalIndex);
            _smallCrystalInstances.Add(smallCrystal);
            smallCrystal.SetActive(false);
        }
        _largeCrystalInstance = InstanciateLargeCrystal();
        _largeCrystalInstance.SetActive(false);
    }

    private GameObject InstanciateSmallCrystal(int crystalIndex)
    {
        float angleStep = 360f / _currentSmallCrystalAmount;

        float angle = crystalIndex * angleStep;
        float rad = angle * Mathf.Deg2Rad;

        Vector3 localOffset = new Vector3(
            Mathf.Cos(rad) * Radius,
            0.44375f,
            Mathf.Sin(rad) * Radius
        );

        Vector3 spawnPosition = transform.TransformPoint(localOffset);

        return Instantiate(_smallCrystalPrefab, spawnPosition, _smallCrystalPrefab.transform.rotation, transform);
    }

    private GameObject InstanciateLargeCrystal()
    {
        return Instantiate(_largeCrystalPrefab, _largeCrystalPrefab.transform.position, _largeCrystalPrefab.transform.rotation, transform);
    }

    private void DestroyCrystals()
    {
        foreach (GameObject smallCrystal in _smallCrystalInstances)
        {
            if (smallCrystal != null)
            {
                Destroy(smallCrystal);
            }
        }
        if (_largeCrystalInstance != null)
        {
            Destroy(_largeCrystalInstance);
        }
    }

    public void OnSelect()
    {
        CollectCrystals();
    }

    private void Update()
    {
        CheckCrystalAmountChange();
        SpinAround();
    }

    private void CheckCrystalAmountChange()
    {
        if (HasCurrentAmountChanged || IsInitialState)
        {
            _currentSmallCrystalAmount = _smallCrystalAmount < 0 ? 0 : _smallCrystalAmount;
            StopBreeding();
            DestroyCrystals();
            _crystalCount = 0;
            _smallCrystalInstances.Clear();
            InstantiateCrystals();
            StartBreeding();
        }
    }

    private bool HasCurrentAmountChanged { get { return _currentSmallCrystalAmount != _smallCrystalAmount; } }
    private bool IsInitialState { get { return _crystalCount == -1; } }

    private void SpinAround()
    {
        transform.RotateAround(transform.position, Vector3.up, _spinningVelocity);
    }

    private void StartBreeding()
    {
        var animator = _largeCrystalInstance.GetComponent<Animator>();
        InvokeRepeating(nameof(Breed), _delay, _interval);
    }

    private void StopBreeding()
    {
        CancelInvoke(nameof(Breed));
    }


    private void OnDestroy()
    {
        StopBreeding();
        DestroyCrystals();
    }
}