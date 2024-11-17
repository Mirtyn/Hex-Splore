using System.Drawing;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Rendering;

partial struct MineSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (!SelectionManager.Instance.ChangeOnMap) return;
        foreach (var (tile, mined, toBeMined, entity) in SystemAPI.Query<RefRO<Tile>, RefRO<Mined>, RefRO<ToBeMined>>().WithDisabled<Mined>().WithEntityAccess())
        {
            SystemAPI.SetComponentEnabled<Mined>(entity, true);
            SystemAPI.SetComponentEnabled<ToBeMined>(entity, false);


            SystemAPI.SetComponentEnabled<MaterialMeshInfo>(tile.ValueRO.SideNX, false);
            SystemAPI.SetComponentEnabled<MaterialMeshInfo>(tile.ValueRO.SideNZ, false);
            SystemAPI.SetComponentEnabled<MaterialMeshInfo>(tile.ValueRO.SideX, false);
            SystemAPI.SetComponentEnabled<MaterialMeshInfo>(tile.ValueRO.SideZ, false);
            SystemAPI.SetComponentEnabled<MaterialMeshInfo>(tile.ValueRO.TopSide, false);

            //var physicsCollider = SystemAPI.GetComponentRW<PhysicsCollider>(entity);

            //unsafe
            //{
            //    BoxCollider* BoxColliderPtr = (BoxCollider*)physicsCollider.ValueRO.ColliderPtr;
            //    BoxGeometry boxGeom = BoxColliderPtr->Geometry;
            //    boxGeom.Size = mined.ValueRO.MinedTileColliderScale;
            //    boxGeom.Center = mined.ValueRO.MinedTileColliderOffset;
            //    BoxColliderPtr->Geometry = boxGeom;
            //}
        }
    }
}
