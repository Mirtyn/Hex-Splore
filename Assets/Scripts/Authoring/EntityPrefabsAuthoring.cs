using Unity.Entities;
using UnityEngine;

public class EntityPrefabsAuthoring : MonoBehaviour
{
    public GameObject TestTilePrefab;

    public class Baker : Baker<EntityPrefabsAuthoring>
    {
        public override void Bake(EntityPrefabsAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EntityPrefabs
            {
                TestTilePrefab = GetEntity(authoring.TestTilePrefab, TransformUsageFlags.Dynamic),
            });
        }
    }
}


public struct EntityPrefabs : IComponentData
{
    public Entity TestTilePrefab;
}
