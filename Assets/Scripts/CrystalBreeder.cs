using System.Collections.Generic;

using UnityEngine;

public class CrystalBreeder : TowerNode, ISelectable
{
    [Header("Crystal Breeding")]

    [SerializeField]
    private float _breedingTime = 5f;
    public float BreedingTime => _breedingTime;

    [SerializeField]
    [Range(1, 8)]
    private int _maxCrystals = 4;
    public int MaxCrystals => _maxCrystals;

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
    private float _spinningVelocity = 0.2f;

    [SerializeField]
    private CrystalAnimator[] _crystalAnimators;

    private const float SmallCrystalSpawnRadius = 0.25f;

    private int _smallCrystalAmount;
    private int _crystalCount;
    private List<GameObject> _smallCrystalInstances = new();
    private GameObject _largeCrystalInstance;

    protected override void Awake()
    {
        base.Awake();

        if (_largeCrystalPrefab == null || _smallCrystalPrefab == null)
        {
            Debug.LogWarning($"Crystal prefabs are not assigned in <{gameObject.name}>.");
            Destroy(this);
            return;
        }

        _smallCrystalAmount = _maxCrystals - 1;

        InstantiateCrystals();

        if (_crystalAnimators == null || _crystalAnimators.Length == 0)
        {
            _crystalAnimators = GetComponentsInChildren<CrystalAnimator>(true);
        }
        if (_crystalAnimators == null || _crystalAnimators.Length == 0)
        {
            Debug.LogWarning($"Crystal animators are not assigned in <{gameObject.name}>.");
            Destroy(this);
            return;
        }
    }

    private void Start()
    {
        StartBreeding();
    }

    private void Update() => SpinAround();

    public void OnSelect() => CollectCrystals();

    private void CollectCrystals()
    {
        if (_crystalCount == 0) return;

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

    private void BreedSmallCrystal(int index)
    {
        ResetSmallCrystal(index);
        _smallCrystalInstances[index].SetActive(true);
    }

    private void BreedLargeCrystal()
    {
        ResetLargeCrystal();
        _largeCrystalInstance.SetActive(true);
    }

    private void InstantiateCrystals()
    {
        for (int i = 0; i < _smallCrystalAmount; i++)
        {
            GameObject smallCrystal = InstantiateSmallCrystal(i);
            _smallCrystalInstances.Add(smallCrystal);
            smallCrystal.SetActive(false);
        }
        _largeCrystalInstance = InstantiateLargeCrystal();
        _largeCrystalInstance.SetActive(false);
    }

    private GameObject InstantiateSmallCrystal(int index)
    {
        float angle = index * (360f / _smallCrystalAmount) * Mathf.Deg2Rad;
        Vector3 localOffset = new(
            Mathf.Cos(angle) * SmallCrystalSpawnRadius,
            0.44375f,
            Mathf.Sin(angle) * SmallCrystalSpawnRadius
        );
        return Instantiate(_smallCrystalPrefab, transform.TransformPoint(localOffset), _smallCrystalPrefab.transform.rotation, transform);
    }

    private GameObject InstantiateLargeCrystal()
    {
        return Instantiate(_largeCrystalPrefab, transform.position, _largeCrystalPrefab.transform.rotation, transform);
    }

    private void StartCollectAnimations()
    {
        if (_crystalAnimators == null || _crystalAnimators.Length == 0)
        {
            foreach (GameObject crystalInstance in _smallCrystalInstances)
            {
                crystalInstance.SetActive(false);
            }
            _largeCrystalInstance.SetActive(false);
            return;
        }

        for (int i = 0; i < _crystalAnimators.Length; i++)
        {
            if (i == 0) _crystalAnimators[i].StartCollect();
            else _crystalAnimators[i].StartCollect(Random.Range(0f, _maxTimeOffset));
        }
    }

    private void DestroyCrystals()
    {
        _smallCrystalInstances.ForEach(Destroy);
        _smallCrystalInstances.Clear();
        if (_largeCrystalInstance != null) Destroy(_largeCrystalInstance);
    }

    private void SpinAround() => transform.Rotate(Vector3.up, _spinningVelocity);

    private void StartBreeding()
    {
        _crystalCount = 0;
        InvokeRepeating(nameof(Breed), _breedingTime, _breedingTime);
    }

    private void StopBreeding() => CancelInvoke(nameof(Breed));

    private void ResetSmallCrystal(int index)
    {
        GameObject smallCrystal = _smallCrystalInstances[index];
        TransformHelper.SetTransformation(smallCrystal.transform, _smallCrystalPrefab.transform, TransformHelper.TransformationType.Rotation, TransformHelper.TransformationType.Scale);
        for (int i = 0; i < smallCrystal.transform.childCount; i++)
        {
            TransformHelper.SetTransformation(smallCrystal.transform.GetChild(i), _smallCrystalPrefab.transform.GetChild(i), TransformHelper.TransformationType.Rotation, TransformHelper.TransformationType.Scale);
        }
    }

    private void ResetLargeCrystal()
    {
        GameObject largeCrystal = _largeCrystalInstance;
        TransformHelper.SetTransformation(largeCrystal.transform, _largeCrystalPrefab.transform);
    }

    private void OnDestroy()
    {
        StopBreeding();
        DestroyCrystals();
    }
}