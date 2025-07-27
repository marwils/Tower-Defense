using System.Collections.Generic;

using Helper;

using LevelSystem;

using UnityEngine;

public class CrystalBreeder : MonoBehaviour, ISelectable
{
    [Header("Breeding")]

    [SerializeField]
    private LevelSystem.CrystalBreeder _metadata;

    private int _smallCrystalAmount = -1;
    public int SmallCrystalAmount
    {
        get
        {
            if (_metadata == null)
            {
                Debug.LogError("CrystalBreederMetadata is not set in CrystalBreeder.");
                Destroy(gameObject);
                return 0;
            }

            if (_smallCrystalAmount < 0 && _smallCrystalAmount != _metadata.MaxCrystals - 1)
            {
                _smallCrystalAmount = _metadata.MaxCrystals - 1;
            }
            return _smallCrystalAmount;
        }
        set { _smallCrystalAmount = value < 0 ? 0 : value; }
    }

    [SerializeField]
    [Tooltip("Maximum time offset (in seconds) for the crystal collection animation.")]
    [Range(0f, 0.5f)]
    private float _maxTimeOffset = 0.2f;

    [Header("Prefabs")]

    [SerializeField]
    [Tooltip("Prefab for the large crystal that will be spawned after breeding small crystals.")]
    private GameObject _largeCrystalPrefab;

    [SerializeField]
    [Tooltip("Prefab for the small crystal that will be spawned during the breeding process.")]
    private GameObject _smallCrystalPrefab;

    [Space]

    [SerializeField]
    [Tooltip("Rotation speed (degrees per frame) of the breeder object.")]
    private float _spinningVelocity = .2f;

    private int _crystalCount = -1;

    private int _currentSmallCrystalAmount;

    private List<GameObject> _smallCrystalInstances = new();
    private GameObject _largeCrystalInstance;

    private const float SmallCrystalSpawnRadius = .25f;

    private void CollectCrystals()
    {
        if (_crystalCount == 0)
        {
            return;
        }

        StopBreeding();
        StartCollectAnimations();

        // Note: Instances were disabled when their animation state exits

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
            Mathf.Cos(rad) * SmallCrystalSpawnRadius,
            0.44375f,
            Mathf.Sin(rad) * SmallCrystalSpawnRadius
        );

        Vector3 spawnPosition = transform.TransformPoint(localOffset);

        return Instantiate(_smallCrystalPrefab, spawnPosition, _smallCrystalPrefab.transform.rotation, transform);
    }

    private GameObject InstanciateLargeCrystal()
    {
        return Instantiate(_largeCrystalPrefab, _largeCrystalPrefab.transform.position, _largeCrystalPrefab.transform.rotation, transform);
    }

    private void StartCollectAnimations()
    {
        CrystalAnimator[] crystalAnimators = gameObject.GetComponentsInChildren<CrystalAnimator>();
        for (int i = 0; i < crystalAnimators.Length; i++)
        {
            if (crystalAnimators[i] != null)
            {
                if (i == 0)
                {
                    crystalAnimators[i].StartCollect();
                }
                else
                {
                    crystalAnimators[i].StartCollect(Random.Range(0.0f, _maxTimeOffset));
                }
            }
        }
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
        InvokeRepeating(nameof(Breed), _metadata.BreedingTime, _metadata.BreedingTime);
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