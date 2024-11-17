using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct PlayerMoverSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var gameMap = SystemAPI.GetSingleton<GameMap>();

        var playerInputActions = PlayerController.Instance.PlayerInput;
        float2 moveDir = new float2(playerInputActions.Movement.x, playerInputActions.Movement.y);

        if (moveDir.x == 0 && moveDir.y == 0) return;

        foreach (var (player, localTransform) in SystemAPI.Query<RefRW<Player>, RefRW<LocalTransform>>())
        {
            float3 movement = new float3(moveDir.x, 0, moveDir.y);
            movement = math.normalize(movement);
            movement *= player.ValueRO.MoveSpeed * SystemAPI.Time.DeltaTime;

            var endPosition = localTransform.ValueRO.Position + movement;
            var endTile = new int2((int)endPosition.x, (int)endPosition.z);

            bool canMove = true;

            var length = gameMap.TileMap.Value.Map.Length;

            for (int i = 0; i < length; i++)
            {
                var currentTileEntity = gameMap.TileMap.Value.Map[i];
                bool2 comparison2 = SystemAPI.GetComponent<Tile>(currentTileEntity).Position == endTile;
                if (comparison2.x && comparison2.y)
                {
                    if (!SystemAPI.IsComponentEnabled<Mined>(currentTileEntity))
                    {
                        canMove = false;
                    }

                    break;
                }
            }

            if (canMove)
            {
                localTransform.ValueRW.Position = endPosition;
                player.ValueRW.CurrentTile = endTile;
            }
        }
    }
}
