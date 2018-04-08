// <copyright file="SimpleParticleSystemContent.cs" company="Urs Müller">
// </copyright>

namespace SpecialEffectsExamplesLibrary.Content.Pipeline.ParticleSystemProperties
{
    using Microsoft.Xna.Framework;

    public class ParticleSystemPropertiesContent
    {
        private Vector3 position;

        private Vector3 gravity;

        private Vector3 spawnDir1;

        private Vector3 spawnDir2;

        private Vector4 startColor1;

        private Vector4 startColor2;

        private Vector4 endColor1;

        private Vector4 endColor2;

        private float minEmitRate;

        private float maxEmitRate;

        private Vector3 emitRadius;

        private float minLifetime;

        private float maxLifetime;

        private float minSize;

        private float maxSize;

        private string srcBlend;

        private string destBlend;

        private int maxParticles;

        private string texture;

        public Vector3 Position
        {
            get { return position; }
            set { this.position = value; }
        }

        public Vector3 Gravity
        {
            get { return gravity; }
            set { this.gravity = value; }
        }

        public Vector3 SpawnDir1
        {
            get { return spawnDir1; }
            set { this.spawnDir1 = value; }
        }

        public Vector3 SpawnDir2
        {
            get { return spawnDir2; }
            set { this.spawnDir2 = value; }
        }

        public Vector4 StartColor1
        {
            get { return startColor1; }
            set { this.startColor1 = value; }
        }

        public Vector4 StartColor2
        {
            get { return startColor2; }
            set { this.startColor2 = value; }
        }

        public Vector4 EndColor1
        {
            get { return endColor1; }
            set { this.endColor1 = value; }
        }

        public Vector4 EndColor2
        {
            get { return endColor2; }
            set { this.endColor2 = value; }
        }

        public float MinEmitRate
        {
            get { return minEmitRate; }
            set { this.minEmitRate = value; }
        }

        public float MaxEmitRate
        {
            get { return maxEmitRate; }
            set { this.maxEmitRate = value; }
        }

        public Vector3 EmitRadius
        {
            get { return emitRadius; }
            set { this.emitRadius = value; }
        }

        public float MinLifetime
        {
            get { return minLifetime; }
            set { this.minLifetime = value; }
        }

        public float MaxLifetime
        {
            get { return maxLifetime; }
            set { this.maxLifetime = value; }
        }

        public float MinSize
        {
            get { return minSize; }
            set { this.minSize = value; }
        }

        public float MaxSize
        {
            get { return maxSize; }
            set { this.maxSize = value; }
        }

        public string SrcBlend
        {
            get { return srcBlend; }
            set { this.srcBlend = value; }
        }

        public string DestBlend
        {
            get { return destBlend; }
            set { this.destBlend = value; }
        }

        public int MaxParticles
        {
            get { return maxParticles; }
            set { this.maxParticles = value; }
        }

        public string Texture
        {
            get { return texture; }
            set { this.texture = value; }
        }
    }
}
