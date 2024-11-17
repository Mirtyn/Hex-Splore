using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; private set; }
    public bool ChangeOnMap { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        ChangeOnMap = false;

        if (Input.GetMouseButtonDown(0))
        {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            //var entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<Tile>().WithDisabled<Mined>().Build(entityManager);

            var physicsQuery = entityManager.CreateEntityQuery(typeof(PhysicsWorldSingleton));
            PhysicsWorldSingleton physicsWorldSingleton = physicsQuery.GetSingleton<PhysicsWorldSingleton>();
            CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;
            UnityEngine.Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastInput raycastInput = new RaycastInput
            {
                Start = cameraRay.GetPoint(0f),
                End = cameraRay.GetPoint(9999f),
                Filter = new CollisionFilter
                {
                    BelongsTo = ~0u,
                    CollidesWith = Layers.TileLayer,
                    GroupIndex = 0,
                }

            };

            if (collisionWorld.CastRay(raycastInput, out Unity.Physics.RaycastHit raycastHit))
            {
                if (entityManager.HasComponent<Tile>(raycastHit.Entity) && entityManager.HasComponent<Mined>(raycastHit.Entity) && entityManager.HasComponent<ToBeMined>(raycastHit.Entity))
                {
                    entityManager.SetComponentEnabled<ToBeMined>(raycastHit.Entity, true);
                    ChangeOnMap = true;
                }
            }
        }
    }
}
