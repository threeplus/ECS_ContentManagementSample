using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class RotationObjectAuthoring : MonoBehaviour
{
    public float RotationSpeed;

    class Baker : Baker<RotationObjectAuthoring>
    {
        public override void Bake(RotationObjectAuthoring authoring)
        {
            Debug.Log("Baking RotationObject");
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new RotationObject
            {
                RotationSpeed = authoring.RotationSpeed
            });
        }
    }
}


public struct RotationObject : IComponentData{
    public float RotationSpeed;
}