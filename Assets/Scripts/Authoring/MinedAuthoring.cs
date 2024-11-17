using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class MinedAuthoring : MonoBehaviour
{
    public float3 FullTileColliderScale;
    public float3 FullTileColliderOffset;
    public float3 MinedTileColliderScale;
    public float3 MinedTileColliderOffset;

    public class Baker : Baker<MinedAuthoring>
    {
        public override void Bake(MinedAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Mined
            {
                FullTileColliderScale = authoring.FullTileColliderScale,
                FullTileColliderOffset = authoring.FullTileColliderOffset,
                MinedTileColliderScale = authoring.MinedTileColliderScale,
                MinedTileColliderOffset = authoring.MinedTileColliderOffset,
            });
            SetComponentEnabled<Mined>(entity, false);
        }
    }
}


public struct Mined : IComponentData, IEnableableComponent
{
    public float3 FullTileColliderScale;
    public float3 FullTileColliderOffset;
    public float3 MinedTileColliderScale;
    public float3 MinedTileColliderOffset;
}