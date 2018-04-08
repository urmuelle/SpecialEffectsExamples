// <copyright file="SimpleParticleSystemContentCompiler.cs" company="Urs Müller">
// </copyright>

namespace SpecialEffectsExamplesLibrary.Content.Pipeline.ParticleSystemProperties
{
    using Microsoft.Xna.Framework.Content.Pipeline;
    using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
    using TWrite = ParticleSystemPropertiesContent;

    [ContentTypeWriter]
    public class ParticleSystemPropertiesContentCompiler : ContentTypeWriter<TWrite>
    {
        /// <summary>
        /// Gets the assembly qualified name of the runtime loader for this type.
        /// </summary>
        /// <param name="targetPlatform">Name of the platform.</param>
        /// <returns>The type string</returns>
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            // class which will be used to load this data.
            return "SpecialEffectsExamplesLibrary.ParticleSystem.ParticleProperties.ParticlePropertiesContentReader, SpecialEffectsExamplesLibrary.ParticleSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        }

        /// <summary>
        /// Return the runtime type information of this assembly
        /// </summary>
        /// <param name="targetPlatform">The target platform.</param>
        /// <returns>the type string</returns>
        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return "SpecialEffectsExamplesLibrary.ParticleSystem.ParticleProperties.ParticleEmitter, SpecialEffectsExamplesLibrary.ParticleSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        }

        /// <summary>
        /// Writes the specified output.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="value">The value.</param>
        protected override void Write(ContentWriter output, TWrite value)
        {
            output.Write(value.Position);
            output.Write(value.Gravity);
            output.Write(value.SpawnDir1);
            output.Write(value.SpawnDir2);
            output.Write(value.StartColor1);
            output.Write(value.StartColor2);
            output.Write(value.EndColor1);
            output.Write(value.EndColor2);
            output.Write(value.MinEmitRate);
            output.Write(value.MaxEmitRate);
            output.Write(value.EmitRadius);
            output.Write(value.MinLifetime);
            output.Write(value.MaxLifetime);
            output.Write(value.MinSize);
            output.Write(value.MaxSize);
            output.Write(value.SrcBlend);
            output.Write(value.DestBlend);
            output.Write(value.MaxParticles);
            output.Write(value.Texture);
        }
    }
}
