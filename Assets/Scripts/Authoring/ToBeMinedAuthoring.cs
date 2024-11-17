using Unity.Entities;
using UnityEngine;

public class ToBeMinedAuthoring : MonoBehaviour
{
    public class Baker : Baker<ToBeMinedAuthoring>
    {
        public override void Bake(ToBeMinedAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ToBeMined());
            SetComponentEnabled<ToBeMined>(entity, false);
        }
    }
}


public struct ToBeMined : IComponentData, IEnableableComponent
{

}
