using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Weapon", order = 0)]
public class WeaponConfigSO : ItemSO, IDestroyableItem, IItemAction, INetworkSerializable
{
    //public WeaponType Type;
    //public string Name;
    //public Sprite Image;
    public GameObject ModelPrefab;
    public Vector2 SpawnPoint;
    public Vector2 RotationPoint;

    public ProjectileConfigSO projectile;
    public TrailConfigSO trail;
    public DamageSO damage;
    public AmmoSO ammo;

    private MonoBehaviour ActiveMonoBehaviour;
    //private AudioSource ShootingAudioSource;
    private GameObject Model;
    private float LastShootTime;
    private float InitialClickTime;
    private float StopShootingTime;
    private bool LastFrameWantedToShoot;
    private ParticleSystem ShootSystem;
    //private Transform Muzzle;
    private ObjectPool<TrailRenderer> TrailPool;

    [field: SerializeField]
    public AudioClip actionSFX { get; private set; }
    public string ActionName => "Equip";

    public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
    {
        WeaponAgent weaponSystem = character.GetComponent<WeaponAgent>();
        if (weaponSystem != null)
        {
            weaponSystem.SetWeapon(this, itemState == null ?
                DefaultParametersList : itemState);
            return true;
        }
        return false;
    }

    public void Spawn(Transform parent, MonoBehaviour ActiveMonoBehaviour)
    {
        this.ActiveMonoBehaviour = ActiveMonoBehaviour;
        LastShootTime = 0;
        TrailPool = new ObjectPool<TrailRenderer>(CreateTrail);

        Model = Instantiate(ModelPrefab);
        Model.transform.SetParent(parent, false);
        Model.transform.localPosition = SpawnPoint;
        Model.transform.localRotation = Quaternion.Euler(RotationPoint);

        //Muzzle = Model.GetComponentInChildren<Transform>();
        ShootSystem = Model.GetComponentInChildren<ParticleSystem>();
    }

    public void Despawn(Transform parent)
    {
        if (parent.childCount > 0)
        {
            Destroy(parent.GetChild(0).gameObject);
        }
    }

    public void Shoot()
    {
        if (Time.time - LastShootTime - projectile.FireRate > Time.deltaTime)
        {
            float lastDuration = Mathf.Clamp(
                0,
                (StopShootingTime - InitialClickTime),
                projectile.MaxSpreadTime
            );
            float lerpTime = (projectile.RecoilRecoverySpeed - (Time.time - StopShootingTime))
                            / projectile.RecoilRecoverySpeed;

            InitialClickTime = Time.time - Mathf.Lerp(0, lastDuration, Mathf.Clamp01(lerpTime));
        }

        if (Time.time > projectile.FireRate + LastShootTime)
        {
            ShootSystem.Play();
            LastShootTime = Time.time;

            Vector3 spreadAmout = projectile.GetSpread(Time.time - InitialClickTime);
            Model.transform.right += Model.transform.TransformDirection(spreadAmout);
            Vector3 shootDirection = Model.transform.right;

            ammo.CurrentClipAmmo--;

            if (Physics2D.Raycast(
                ShootSystem.transform.position,
                shootDirection,
                float.MaxValue,
                 projectile.HitMask
                ))
            {
                RaycastHit2D hit = Physics2D.Raycast(ShootSystem.transform.position, shootDirection, float.MaxValue, projectile.HitMask);
                ActiveMonoBehaviour.StartCoroutine(
                   PlayTrail(
                    ShootSystem.transform.position,
                    hit.point,
                    hit
                ));
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(ShootSystem.transform.position, shootDirection, float.MaxValue, projectile.HitMask);
                ActiveMonoBehaviour.StartCoroutine(
                   PlayTrail(
                    ShootSystem.transform.position,
                    ShootSystem.transform.position + (shootDirection * trail.MissDistance),
                    new RaycastHit2D()
                ));
            }
        }
    }

    public bool CanReload()
    {
        return ammo.CanReload();
    }

    public void Tick(bool WantsToShoot)
    {
        Model.transform.localRotation = Quaternion.Lerp(
            Model.transform.localRotation,
            Quaternion.Euler(RotationPoint),
            Time.deltaTime * projectile.RecoilRecoverySpeed
        );

        if (WantsToShoot)
        {
            LastFrameWantedToShoot = true;
            if(ammo.CurrentClipAmmo > 0)
            {
                Shoot();
            }
        }
        else if (!WantsToShoot && LastFrameWantedToShoot)
        {
            StopShootingTime = Time.time;
            LastFrameWantedToShoot = false;
        }
    }

    private IEnumerator PlayTrail(Vector3 StartPoint, Vector3 EndPoint, RaycastHit2D Hit)
    {
        TrailRenderer instance = TrailPool.Get();
        instance.gameObject.SetActive(true);
        instance.transform.position = StartPoint;
        yield return null;

        instance.emitting = true;

        float distance = Vector3.Distance(StartPoint, EndPoint);
        float remainingDistance = distance;
        while (remainingDistance > 0)
        {
            instance.transform.position = Vector3.Lerp(
                StartPoint,
                EndPoint,
                Mathf.Clamp01(1 - (remainingDistance / distance))
            );
            remainingDistance -= trail.Speed * Time.deltaTime;

            yield return null;
        }

        instance.transform.position = EndPoint;

        if (Hit.collider != null)
        {
            /*SurfaceManager.Instance.HandleImpact(
                HitCollider.gameObject,
                HitLocation,
                HitNormal,
                ImpactType,
                0
            );*/

            if (Hit.collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage.GetDamage());
            }
        }

        yield return new WaitForSeconds(trail.Duration);
        yield return null;
        instance.emitting = false;
        instance.gameObject.SetActive(false);
        instance.Clear();
        TrailPool.Release(instance);
    }

    private TrailRenderer CreateTrail()
    {
        GameObject instance = new GameObject("Bullet Trail");
        TrailRenderer trailRender = instance.AddComponent<TrailRenderer>();
        trailRender.material = trail.Trail;
        trailRender.widthCurve = trail.WidthCurve;
        trailRender.colorGradient = trail.Color;
        trailRender.time = trail.Duration;

        trailRender.emitting = false;
        trailRender.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        return trailRender;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        /*
        public GameObject ModelPrefab;
        public Vector2 SpawnPoint;
        public Vector2 RotationPoint;

        public ProjectileConfigSO projectile;
        public TrailConfigSO trail;
        public DamageSO damage;

        private MonoBehaviour ActiveMonoBehaviour;
        private GameObject Model;
        private float LastShootTime;
        private float InitialClickTime;
        private float StopShootingTime;
        private bool LastFrameWantedToShoot;
        private ParticleSystem ShootSystem;
        private ObjectPool<TrailRenderer> TrailPool;

        [field: SerializeField]
        public AudioClip actionSFX { get; private set; }
        public string ActionName => "Equip";*/
    }
}
