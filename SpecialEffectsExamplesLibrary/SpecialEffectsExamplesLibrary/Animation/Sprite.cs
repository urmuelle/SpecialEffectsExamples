// <copyright file="Sprite.cs" company="Urs Müller">
// </copyright>

namespace SpecialEffectsExamplesLibrary.Animation
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Sprite
    {
        private float size;
        private Vector3 position;
        private float rotationYaw;
        private float rotationPitch;
        private float rotationRoll;

        private Timer timer;
        private AnimationSequence animationSequence;

        public Sprite()
        {
            animationSequence = null;
            size = 1.0f;
            rotationYaw = rotationPitch = rotationRoll = 0.0f;
            timer = new Timer();
        }

        public Sprite(AnimationSequence animationSequence)
        {
            this.animationSequence = animationSequence;
            size = 1.0f;
            timer = new Timer();
        }

        public Timer Timer
        {
            get { return timer; }
        }

        public AnimationSequence AnimationSequemce
        {
            get { return animationSequence; }
            set { animationSequence = value; }
        }

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public float Size
        {
            get { return size; }
            set { size = value; }
        }

        public float RotationYaw
        {
            get { return rotationYaw; }
            set { rotationYaw = value; }
        }

        public float RotationPitch
        {
            get { return rotationPitch; }
            set { rotationPitch = value; }
        }

        public float RotationRoll
        {
            get { return rotationRoll; }
            set { rotationRoll = value; }
        }

        public void Draw(GraphicsDevice graphicsDevice, Matrix view, Matrix projection)
        {
            // Set up a rotation matrix to orient the explo sprite towards the camera.
            Matrix matBillboardRot = Matrix.Identity;
            Matrix matTranspose = Matrix.Transpose(view);

            matBillboardRot.M11 = matTranspose.M11;
            matBillboardRot.M12 = matTranspose.M12;
            matBillboardRot.M13 = matTranspose.M13;
            matBillboardRot.M21 = matTranspose.M21;
            matBillboardRot.M22 = matTranspose.M22;
            matBillboardRot.M23 = matTranspose.M23;
            matBillboardRot.M31 = matTranspose.M31;
            matBillboardRot.M32 = matTranspose.M32;
            matBillboardRot.M33 = matTranspose.M33;

            var matWorld = Matrix.CreateTranslation(position);
            var matRot = Matrix.CreateFromYawPitchRoll(rotationYaw, rotationPitch, rotationRoll);
            var matScale = Matrix.CreateScale(size, size, 1);

            this.animationSequence.Draw(graphicsDevice, timer, matScale * matRot * matBillboardRot * matWorld, view, projection);
        }
    }
}
