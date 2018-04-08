// <copyright file="AdvancedParticleSystemContentCompiler.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace SpecialEffectsExamplesLibrary.Content.Pipeline.AdvancedParticleSystem
{
    using Microsoft.Xna.Framework.Content.Pipeline;
    using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
    using TWrite = AdvancedParticleSystemContent;

    [ContentTypeWriter]
    public class AdvancedParticleSystemContentCompiler : ContentTypeWriter<TWrite>
    {
        /// <summary>
        /// Gets the assembly qualified name of the runtime loader for this type.
        /// </summary>
        /// <param name="targetPlatform">Name of the platform.</param>
        /// <returns>The type string</returns>
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            // class which will be used to load this data.
            return "SpecialEffectsExamplesLibrary.ParticleSystem.AdvancedParticleSystem.AdvancedParticleSystemContentReader, SpecialEffectsExamplesLibrary.ParticleSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        }

        /// <summary>
        /// Return the runtime type information of this assembly
        /// </summary>
        /// <param name="targetPlatform">The target platform.</param>
        /// <returns>the type string</returns>
        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return "SpecialEffectsExamplesLibrary.ParticleSystem.AdvancedParticleSystem.ParticleEmitter, SpecialEffectsExamplesLibrary.ParticleSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        }

        /// <summary>
        /// Writes the specified output.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="value">The value.</param>
        protected override void Write(ContentWriter output, TWrite value)
        {
            output.Write(value.ParticleSystemDescription);
        }
    }
}
