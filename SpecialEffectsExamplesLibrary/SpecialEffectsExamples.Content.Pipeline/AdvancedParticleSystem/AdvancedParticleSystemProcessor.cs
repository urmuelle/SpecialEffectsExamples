// <copyright file="AdvancedParticleSystemProcessor.cs" company="Urs Müller">
// </copyright>

namespace SpecialEffectsExamplesLibrary.Content.Pipeline.AdvancedParticleSystem
{
    using System;
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
    [ContentProcessor(DisplayName = "Advanced Particle System Processor")]
    public class AdvancedParticleSystemProcessor : ContentProcessor<AdvancedParticleSystemContent, AdvancedParticleSystemContent>
    {
        public override AdvancedParticleSystemContent Process(AdvancedParticleSystemContent input, ContentProcessorContext context)
        {
            var content = input;

            return content;
        }
    }
}
