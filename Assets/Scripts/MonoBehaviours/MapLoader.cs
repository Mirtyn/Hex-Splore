using Unity.Entities;
using Unity.Collections;
using UnityEngine;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Rendering;

public class MapLoader : MonoBehaviour
{
    private int XSize = 6;
    private int YSize = 6;

    private void Start()
    {
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<EntityPrefabs>().Build(entityManager);
        var entityPrefabs = entityQuery.GetSingleton<EntityPrefabs>();
        var gameMapEntity = new EntityQueryBuilder(Allocator.Temp).WithAll<GameMap>().Build(entityManager).GetSingletonEntity();

        var tileEntities = new NativeArray<Entity>(XSize * YSize, Allocator.Temp);

        for (int y = 0; y < YSize; y++)
        {
            for (int x = 0; x < XSize; x++)
            {
                var tileEntity = entityManager.Instantiate(entityPrefabs.TestTilePrefab);
                var localTransform = entityManager.GetComponentData<LocalTransform>(tileEntity);
                localTransform.Position = new float3(x, 0, y);
                entityManager.SetComponentData(tileEntity, localTransform);

                var tile = entityManager.GetComponentData<Tile>(tileEntity);
                tile.Position = new int2(x, y);
                entityManager.SetComponentData(tileEntity, tile);

                tileEntities[x + y * XSize] = tileEntity;
            }
        }

        using (BlobBuilder blobBuilder = new BlobBuilder(Allocator.Temp))
        {
            ref TileMapBlobAsset tileMapAsset = ref blobBuilder.ConstructRoot<TileMapBlobAsset>();
            tileMapAsset.XSize = XSize;
            tileMapAsset.YSize = YSize;
            BlobBuilderArray<Entity> tileArray = blobBuilder.Allocate(ref tileMapAsset.Map, XSize * YSize);
            //copy MapTile entities to blob array
            for (int t = 0; t < XSize * YSize; t++)
            {
                tileArray[t] = tileEntities[t];
            }
            // create immutable BlobAssetReference
            var assetReference = blobBuilder.CreateBlobAssetReference<TileMapBlobAsset>(Allocator.Persistent);
            // assign BlobAssetReference to GameMap
            entityManager.SetComponentData(gameMapEntity, new GameMap { TileMap = assetReference });
            BlobDisposerSystem.BlobRef = assetReference;
        }

        UpdateTileSides();
    }

    public void UpdateTileSides()
    {
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<GameMap>().Build(entityManager);
        var gameMap = entityQuery.GetSingleton<GameMap>();

        entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<Tile>().WithDisabled<Mined>().Build(entityManager);
        var tileComponents = entityQuery.ToComponentDataArray<Tile>(Allocator.Temp);
        var entities = entityQuery.ToEntityArray(Allocator.Temp);
        int2 offset;

        foreach (var tile in tileComponents)
        {
            // check up
            offset = int2.zero;
            offset.y = 1;
            if (CheckForTileInDirection(tile.Position, offset, gameMap, entityManager))
            {
                entityManager.SetComponentEnabled<MaterialMeshInfo>(tile.SideZ, false);
            }

            // check down
            offset.y = -1;
            if (CheckForTileInDirection(tile.Position, offset, gameMap, entityManager))
            {
                entityManager.SetComponentEnabled<MaterialMeshInfo>(tile.SideNZ, false);

            }

            // check right
            offset.y = 0;
            offset.x = 1;
            if (CheckForTileInDirection(tile.Position, offset, gameMap, entityManager))
            {
                entityManager.SetComponentEnabled<MaterialMeshInfo>(tile.SideX, false);

            }

            // check left
            offset.x = -1;
            if (CheckForTileInDirection(tile.Position, offset, gameMap, entityManager))
            {
                entityManager.SetComponentEnabled<MaterialMeshInfo>(tile.SideNX, false);

            }
        }
    }

    public bool CheckForTileInDirection(int2 pos, int2 offset, GameMap gameMap, EntityManager entityManager)
    {
        int2 position = pos + offset;
        if (position.x < 0 || position.x >= gameMap.TileMap.Value.XSize || position.y < 0 || position.y >= gameMap.TileMap.Value.YSize)
        {
            return false;
        }

        var entity = gameMap.TileMap.Value.Map[position.x + position.y * gameMap.TileMap.Value.XSize];
        if (entityManager.IsComponentEnabled<Mined>(entity))
        {
            return false;
        }

        return true;
    }
}


public struct TileMapBlobAsset
{
    public int XSize;
    public int YSize;
    public BlobArray<Entity> Map;
}
