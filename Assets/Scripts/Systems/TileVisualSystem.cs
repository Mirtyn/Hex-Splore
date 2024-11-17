using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;
using static UnityEditor.PlayerSettings;

[UpdateAfter(typeof(MineSystem))]
partial struct TileVisualSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (!SelectionManager.Instance.ChangeOnMap) return;

        var gameMap = SystemAPI.GetSingleton<GameMap>();
        int2 offset;

        foreach (var (tile, entity) in SystemAPI.Query<RefRO<Tile>>().WithDisabled<Mined>().WithEntityAccess())
        {
            // check up
            offset = int2.zero;
            offset.y = 1;
            if (CheckForTileInDirection(tile.ValueRO.Position, offset, gameMap, ref state))
            {
                SystemAPI.SetComponentEnabled<MaterialMeshInfo>(tile.ValueRO.SideZ, false);
            }
            else
            {
                SystemAPI.SetComponentEnabled<MaterialMeshInfo>(tile.ValueRO.SideZ, true);
            }

            // check down
            offset.y = -1;
            if (CheckForTileInDirection(tile.ValueRO.Position, offset, gameMap, ref state))
            {
                SystemAPI.SetComponentEnabled<MaterialMeshInfo>(tile.ValueRO.SideNZ, false);
            }
            else
            {
                SystemAPI.SetComponentEnabled<MaterialMeshInfo>(tile.ValueRO.SideNZ, true);
            }

            // check right
            offset.y = 0;
            offset.x = 1;
            if (CheckForTileInDirection(tile.ValueRO.Position, offset, gameMap, ref state))
            {
                SystemAPI.SetComponentEnabled<MaterialMeshInfo>(tile.ValueRO.SideX, false);
            }
            else
            {
                SystemAPI.SetComponentEnabled<MaterialMeshInfo>(tile.ValueRO.SideX, true);
            }

            // check left
            offset.x = -1;
            if (CheckForTileInDirection(tile.ValueRO.Position, offset, gameMap, ref state))
            {
                SystemAPI.SetComponentEnabled<MaterialMeshInfo>(tile.ValueRO.SideNX, false);
            }
            else
            {
                SystemAPI.SetComponentEnabled<MaterialMeshInfo>(tile.ValueRO.SideNX, true);
            }
        }
    }

    [BurstCompile]
    public bool CheckForTileInDirection(int2 pos, int2 offset, GameMap gameMap, ref SystemState state)
    {
        int2 position = pos + offset;
        if (position.x < 0 || position.x >= gameMap.TileMap.Value.XSize || position.y < 0 || position.y >= gameMap.TileMap.Value.YSize)
        {
            return false;
        }

        var entity = gameMap.TileMap.Value.Map[position.x + position.y * gameMap.TileMap.Value.XSize];
        if (SystemAPI.IsComponentEnabled<Mined>(entity))
        {
            return false;
        }

        return true;
    }
}
