using System.Collections.Generic;

using Helper;

using UnityEngine;

public class CrystalBreeder : EntityBase, ISelectable
{
    [Header("Prefabs")]

    [SerializeField]
    [Tooltip("Prefab for the large crystal that will be spawned after breeding small crystals.")]
    private GameObject _largeCrystalPrefab;

    [SerializeField]
    [Tooltip("Prefab for the small crystal that will be spawned during the breeding process.")]
    private GameObject _smallCrystalPrefab;

    [Header("Animation")]

    [SerializeField]
    [Tooltip("Maximum time offset (in seconds) for the crystal collection animation.")]
    [Range(0f, 0.5f)]
    private float _maxTimeOffset = 0.2f;

    [SerializeField]
    [Tooltip("Rotation speed (degrees per frame) of the breeder object.")]
    private float _spinningVelocity = .2f;

    private const float SmallCrystalSpawnRadius = .25f;

    private float _breedingTime = 1f;

    private int _smallCrystalAmount;

    private int _crystalCount;

    private List<GameObject> _smallCrystalInstances = new();
    private GameObject _largeCrystalInstance;

    protected void Start()
    {
        InstantiateCrystals();
        StartBreeding();
    }

    private void Update()
    {
        SpinAround();
    }

    private void CollectCrystals()
    {
        if (_crystalCount == 0)
        {
            return;
        }

        StopBreeding();
        StartCollectAnimations();

        // FYI: Instances were disabled when their animation state exits

        StartBreeding();
    }

    private void Breed()
    {
        if (_crystalCount < _smallCrystalAmount)
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
        for (int crystalIndex = 0; crystalIndex < _smallCrystalAmount; crystalIndex++)
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
        float angleStep = 360f / _smallCrystalAmount;

        float angle = crystalIndex * angleStep;
        float rad = angle * Mathf.Deg2Rad;

        Vector3 localOffset = new(
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

    public override void OnSelect()
    {
        CollectCrystals();
    }

    private void SpinAround()
    {
        transform.RotateAround(transform.position, Vector3.up, _spinningVelocity);
    }

    private void StartBreeding()
    {
        Debug.Log("Starting crystal breeding");
        _crystalCount = 0;
        InvokeRepeating("Breed", _breedingTime, _breedingTime);
    }

    private void StopBreeding()
    {
        CancelInvoke("Breed");
    }


    private void OnDestroy()
    {
        StopBreeding();
        DestroyCrystals();
    }

    protected override void InitState()
    {
        _breedingTime = GetSettings<LevelSystem.CrystalBreeder>().BreedingTime;
        _smallCrystalAmount = GetSettings<LevelSystem.CrystalBreeder>().MaxCrystals - 1;
    }

}