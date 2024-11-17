using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class TileAuthoring : MonoBehaviour
{
    public TileType TileType;
    public Vector2Int Position;
    public GameObject TopSide;
    public GameObject SideX;
    public GameObject SideZ;
    public GameObject SideNX;
    public GameObject SideNZ;

    public class Baker : Baker<TileAuthoring>
    {
        public override void Bake(TileAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new Tile{
                TileType = authoring.TileType,
                Position = new int2(authoring.Position.x, authoring.Position.y),
                TopSide = GetEntity(authoring.TopSide, TransformUsageFlags.Dynamic),
                SideX = GetEntity(authoring.SideX, TransformUsageFlags.Dynamic),
                SideZ = GetEntity(authoring.SideZ, TransformUsageFlags.Dynamic),
                SideNX = GetEntity(authoring.SideNX, TransformUsageFlags.Dynamic),
                SideNZ = GetEntity(authoring.SideNZ, TransformUsageFlags.Dynamic),
            });
        }
    }
}


public struct Tile : IComponentData
{
    public TileType TileType;
    public int2 Position;
    public Entity TopSide;
    public Entity SideX;
    public Entity SideZ;
    public Entity SideNX;
    public Entity SideNZ;
}
