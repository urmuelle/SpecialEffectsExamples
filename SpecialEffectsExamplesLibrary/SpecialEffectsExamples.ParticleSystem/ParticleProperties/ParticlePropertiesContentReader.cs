// <copyright file="SimpleParticleSystemContentReader.cs" company="Urs Müller">
// </copyright>

namespace SpecialEffectsExamplesLibrary.ParticleSystem.ParticleProperties
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework.Content;
    using ParticleProperties;

    public class ParticlePropertiesContentReader : ContentTypeReader<ParticleEmitter>
    {
        protected override ParticleEmitter Read(ContentReader input, ParticleEmitter existingInstance)
        {
            return new ParticleEmitter(input);
        }
    }
}
