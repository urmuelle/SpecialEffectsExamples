// <copyright file="AdvancedParticleSystemContentImporter.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace SpecialEffectsExamplesLibrary.Content.Pipeline.AdvancedParticleSystem
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework.Content.Pipeline;
    using TImport = AdvancedParticleSystemContent;

    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to import a file from disk into the specified type, TImport.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    ///
    /// TODO: change the ContentImporter attribute to specify the correct file
    /// extension, display name, and default processor for this importer.
    /// </summary>
    [ContentImporter(".txt", DisplayName = "Advanced Particle System Importer", DefaultProcessor = "AdvancedParticleSystemProcessor")]
    public class AdvancedParticleSystemContentImporter : ContentImporter<TImport>
    {
        public override TImport Import(string filename, ContentImporterContext context)
        {
            var content = new TImport
            {
                ParticleSystemDescription = System.IO.File.ReadAllText(filename)
            };

            return content;
        }
    }
}
