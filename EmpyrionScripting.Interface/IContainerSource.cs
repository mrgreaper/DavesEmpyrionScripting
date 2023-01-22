﻿using Eleon.Modding;

namespace EmpyrionScripting.Interface
{
    public interface IContainerSource
    {
        IContainer Container { get; set; }
        string CustomName { get; set; }
        IEntityData E { get; set; }
        VectorInt3 Position { get; set; }
        float VolumeCapacity { get; }
        float DecayFactor { get; }
    }
}