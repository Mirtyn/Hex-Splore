using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
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
            var entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<GameMap>().Build(entityManager);

            //var physicsQuery = entityManager.CreateEntityQuery(typeof(PhysicsWorldSingleton));
            GameMap gameMap = entityQuery.GetSingleton<GameMap>();
            //CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;
            Vector3 pos = Vector3.zero;
            pos += Input.mousePosition;
            var worldPos = Camera.main.ScreenToWorldPoint(pos);
            int2 int2Pos = new int2((int)worldPos.x, (int)worldPos.z);

            //RaycastInput raycastInput = new RaycastInput
            //{
            //    Start = cameraRay.GetPoint(0f),
            //    End = cameraRay.GetPoint(9999f),
            //    Filter = new CollisionFilter
            //    {
            //        BelongsTo = ~0u,
            //        CollidesWith = Layers.TileLayer,
            //        GroupIndex = 0,
            //    }

            //};

            var length = gameMap.TileMap.Value.Map.Length;
            bool foundEntity = false;
            Entity tileEntity = Entity.Null;

            for (int i = 0; i < length; i++)
            {
                var currentTileEntity = gameMap.TileMap.Value.Map[i];
                bool2 comparison2 = entityManager.GetComponentData<Tile>(currentTileEntity).Position == int2Pos;
                if (comparison2.x && comparison2.y)
                {
                    tileEntity = currentTileEntity;
                    foundEntity = true;
                    break;
                }
            }

            if (foundEntity)
            {
                if (entityManager.HasComponent<Tile>(tileEntity) && entityManager.HasComponent<Mined>(tileEntity) && entityManager.HasComponent<ToBeMined>(tileEntity))
                {
                    entityManager.SetComponentEnabled<ToBeMined>(tileEntity, true);
                    ChangeOnMap = true;
                }
            }
        }
    }
}
