// <copyright file="SimpleParticleSystemContentImporter.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace SpecialEffectsExamplesLibrary.Content.Pipeline.ParticleSystemProperties
{
    using System.Globalization;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content.Pipeline;
    using TImport = ParticleSystemPropertiesContent;

    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to import a file from disk into the specified type, TImport.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    ///
    /// TODO: change the ContentImporter attribute to specify the correct file
    /// extension, display name, and default processor for this importer.
    /// </summary>
    [ContentImporter(".xml", DisplayName = "Particle System Properties Importer", DefaultProcessor = "ParticleSystemPropertiesProcessor")]
    public class ParticleSystemPropertiesContentImporter : ContentImporter<TImport>
    {
        public override TImport Import(string filename, ContentImporterContext context)
        {
            var content = new ParticleSystemPropertiesContent();

            XElement particleSystem = XElement.Load(filename);

            var position = particleSystem.Element("Position");
            content.Position = new Vector3(
                float.Parse(position.Attribute("x").Value, CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(position.Attribute("y").Value, CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(position.Attribute("z").Value, CultureInfo.InvariantCulture.NumberFormat));

            var gravity = particleSystem.Element("Gravity");
            content.Gravity = new Vector3(
                float.Parse(gravity.Attribute("x").Value, CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(gravity.Attribute("y").Value, CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(gravity.Attribute("z").Value, CultureInfo.InvariantCulture.NumberFormat));

            var spawnDir1 = particleSystem.Element("SpawnDir1");
            content.SpawnDir1 = new Vector3(
                float.Parse(spawnDir1.Attribute("x").Value, CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(spawnDir1.Attribute("y").Value, CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(spawnDir1.Attribute("z").Value, CultureInfo.InvariantCulture.NumberFormat));

            var spawnDir2 = particleSystem.Element("SpawnDir2");
            content.SpawnDir2 = new Vector3(
                float.Parse(spawnDir2.Attribute("x").Value, CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(spawnDir2.Attribute("y").Value, CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(spawnDir2.Attribute("z").Value, CultureInfo.InvariantCulture.NumberFormat));

            var startColor1 = particleSystem.Element("StartColor1");
            content.StartColor1 = new Vector4(
                byte.Parse(startColor1.Attribute("r").Value, CultureInfo.InvariantCulture.NumberFormat),
                byte.Parse(startColor1.Attribute("g").Value, CultureInfo.InvariantCulture.NumberFormat),
                byte.Parse(startColor1.Attribute("b").Value, CultureInfo.InvariantCulture.NumberFormat),
                byte.Parse(startColor1.Attribute("a").Value, CultureInfo.InvariantCulture.NumberFormat));

            var startColor2 = particleSystem.Element("StartColor2");
            content.StartColor2 = new Vector4(
                byte.Parse(startColor2.Attribute("r").Value, CultureInfo.InvariantCulture.NumberFormat),
                byte.Parse(startColor2.Attribute("g").Value, CultureInfo.InvariantCulture.NumberFormat),
                byte.Parse(startColor2.Attribute("b").Value, CultureInfo.InvariantCulture.NumberFormat),
                byte.Parse(startColor2.Attribute("a").Value, CultureInfo.InvariantCulture.NumberFormat));

            var endColor1 = particleSystem.Element("EndColor1");
            content.EndColor1 = new Vector4(
                byte.Parse(endColor1.Attribute("r").Value, CultureInfo.InvariantCulture.NumberFormat),
                byte.Parse(endColor1.Attribute("g").Value, CultureInfo.InvariantCulture.NumberFormat),
                byte.Parse(endColor1.Attribute("b").Value, CultureInfo.InvariantCulture.NumberFormat),
                byte.Parse(endColor1.Attribute("a").Value, CultureInfo.InvariantCulture.NumberFormat));

            var endColor2 = particleSystem.Element("EndColor2");
            content.EndColor2 = new Vector4(
                byte.Parse(endColor2.Attribute("r").Value, CultureInfo.InvariantCulture.NumberFormat),
                byte.Parse(endColor2.Attribute("g").Value, CultureInfo.InvariantCulture.NumberFormat),
                byte.Parse(endColor2.Attribute("b").Value, CultureInfo.InvariantCulture.NumberFormat),
                byte.Parse(endColor2.Attribute("a").Value, CultureInfo.InvariantCulture.NumberFormat));

            var emitRate = particleSystem.Element("EmitRate");
            content.MinEmitRate = float.Parse(emitRate.Attribute("min").Value, CultureInfo.InvariantCulture.NumberFormat);
            content.MaxEmitRate = float.Parse(emitRate.Attribute("max").Value, CultureInfo.InvariantCulture.NumberFormat);

            var emitRadius = particleSystem.Element("EmitRadius");
            content.EmitRadius = new Vector3(
                float.Parse(emitRadius.Attribute("x").Value, CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(emitRadius.Attribute("y").Value, CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(emitRadius.Attribute("z").Value, CultureInfo.InvariantCulture.NumberFormat));

            var lifeTime = particleSystem.Element("LifeTime");
            content.MinLifetime = float.Parse(lifeTime.Attribute("min").Value, CultureInfo.InvariantCulture.NumberFormat);
            content.MaxLifetime = float.Parse(lifeTime.Attribute("max").Value, CultureInfo.InvariantCulture.NumberFormat);

            var size = particleSystem.Element("Size");
            content.MinSize = float.Parse(size.Attribute("min").Value, CultureInfo.InvariantCulture.NumberFormat);
            content.MaxSize = float.Parse(size.Attribute("max").Value, CultureInfo.InvariantCulture.NumberFormat);

            content.SrcBlend = particleSystem.Element("SrcBlend").Value;
            content.DestBlend = particleSystem.Element("DestBlend").Value;

            var maxParticles = particleSystem.Element("MaxParticles");
            content.MaxParticles = int.Parse(maxParticles.Value, CultureInfo.InvariantCulture.NumberFormat);

            // TODO: Texture is not used yet.
            content.Texture = particleSystem.Element("Texture").Value;

            return content;
        }
    }
}
