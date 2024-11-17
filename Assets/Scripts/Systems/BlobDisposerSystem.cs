using Unity.Burst;
using Unity.Entities;

partial struct BlobDisposerSystem : ISystem
{
    public static BlobAssetReference<TileMapBlobAsset> BlobRef;

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        BlobRef.Dispose();
    }
}
