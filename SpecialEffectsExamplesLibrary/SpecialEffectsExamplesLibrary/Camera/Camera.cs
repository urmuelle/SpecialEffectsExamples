// <copyright file="Camera.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace SpecialEffectsExamplesLibrary.Camera
{
    using Microsoft.Xna.Framework;

    public abstract class Camera
    {
        private Matrix viewMatrix;
        private Matrix orientationMatrix;
        private Vector3 position;

        public Camera()
        {
        }

        public Matrix ViewMatrix
        {
            get { return viewMatrix; }
            set { viewMatrix = value; }
        }

        public Matrix Orientation
        {
            get { return orientationMatrix; }
            set { orientationMatrix = value; }
        }

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public abstract void Update(float fElapsedTime);
    }
}