// <copyright file="SimpleParticleSystemContentReader.cs" company="Urs Müller">
// </copyright>

namespace SpecialEffectsExamplesLibrary.ParticleSystem.AdvancedParticleSystem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework.Content;

    class AdvancedParticleSystemContentReader : ContentTypeReader<ParticleEmitter>
    {
        protected override ParticleEmitter Read(ContentReader input, ParticleEmitter existingInstance)
        {
            return new ParticleEmitter(input);
        }
    }
}
