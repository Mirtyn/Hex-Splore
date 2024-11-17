using Unity.Entities;
using UnityEngine;

public class GameMapAuthoring : MonoBehaviour
{
    public class Baker : Baker<GameMapAuthoring>
    {
        public override void Bake(GameMapAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new GameMap());
        }
    }
}


public struct GameMap : IComponentData
{
    public BlobAssetReference<TileMapBlobAsset> TileMap;
}
