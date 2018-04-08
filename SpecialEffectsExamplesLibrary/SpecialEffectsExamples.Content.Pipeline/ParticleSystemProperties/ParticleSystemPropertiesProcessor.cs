// <copyright file="SimpleParticleSystemProcessor.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace SpecialEffectsExamplesLibrary.Content.Pipeline.ParticleSystemProperties
{
    using Microsoft.Xna.Framework.Content.Pipeline;

    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to apply custom processing to content data, converting an object of
    /// type TInput to TOutput. The input and output types may be the same if
    /// the processor wishes to alter data without changing its type.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    ///
    /// TODO: change the ContentProcessor attribute to specify the correct
    /// display name for this processor.
    /// </summary>
    [ContentProcessor(DisplayName = "Particle System Properties Processor")]
    public class ParticleSystemPropertiesProcessor : ContentProcessor<ParticleSystemPropertiesContent, ParticleSystemPropertiesContent>
    {
        public override ParticleSystemPropertiesContent Process(ParticleSystemPropertiesContent input, ContentProcessorContext context)
        {
            var content = input;

            return content;
        }
    }
}
