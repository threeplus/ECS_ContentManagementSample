using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct RotationObjectSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<RotationObject>();
    }

    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        foreach (var (transform, speed) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotationObject>>())
        {
            transform.ValueRW = transform.ValueRO.RotateY(speed.ValueRO.RotationSpeed * deltaTime);
        }
    }
}
