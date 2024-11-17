using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    public float MoveSpeed;
    public float RotateSpeed;
    public Vector2Int CurrentTile;

    public class Baker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Player
            {
                MoveSpeed = authoring.MoveSpeed,
                RotateSpeed = authoring.RotateSpeed,
                CurrentTile = new int2(authoring.CurrentTile.x, authoring.CurrentTile.y),
            });
        }
    }
}


public struct Player : IComponentData
{
    public float MoveSpeed;
    public float RotateSpeed;
    public int2 CurrentTile;
}
