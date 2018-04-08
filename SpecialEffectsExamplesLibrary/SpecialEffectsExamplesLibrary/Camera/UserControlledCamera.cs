// <copyright file="UserControlledCamera.cs" company="Urs Müller">
// </copyright>

namespace SpecialEffectsExamplesLibrary.Camera
{
    using System;
    using Microsoft.Xna.Framework;

    public class UserControlledCamera : Camera
    {
        private Vector3 velocity;
        private float yaw;
        private float yawVelocity;
        private float pitch;
        private float pitchVelocity;
        private float roll;
        private float rollVelocity;

        public UserControlledCamera()
        {
            velocity.X = 0.0f;
            velocity.Y = 0.0f;
            velocity.Z = 0.0f;
            yaw = 0.0f;
            yawVelocity = 0.0f;
            pitch = 0.0f;
            pitchVelocity = 0.0f;
            roll = 0.0f;
            rollVelocity = 0.0f;
            Position = new Vector3(0.0f, 0.0f, 0.0f);
            Orientation = Matrix.Identity;
        }

        public void AddToYawPitchRoll(float yaw, float pitch, float roll)
        {
            pitchVelocity += pitch;
            yawVelocity += yaw;
            rollVelocity += roll;
        }

        public void AddToVelocity(Vector3 add)
        {
            velocity += add;
        }

        /// <summary>
        /// Update the camera
        /// </summary>
        /// <param name="elapsedTime">The elapsed time in seconds</param>
        public override void Update(float elapsedTime)
        {
            float time;

            if (elapsedTime > 0.0f)
            {
                time = elapsedTime;
            }
            else
            {
                time = 0.05f;
            }

            float speed = 3.0f * time;
            float angularSpeed = 1.0f * time;

            // Update the position vector
            Vector3 vT = velocity * speed;
            vT = Vector3.TransformNormal(vT, Orientation);
            Position += vT;

            // Update the yaw-pitch-rotation vector
            yaw += angularSpeed * yawVelocity;
            pitch += angularSpeed * pitchVelocity;

            // Set the view matrix
            Quaternion qR = Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll);

            /*
            This is the method according to MS XNA Documentation

            var myRotation = Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll);
            var myPosition = position;
            Matrix myView;

            Vector3 up = Vector3.Transform(Vector3.Up, myRotation);
            Vector3 target = Vector3.Transform(Vector3.Forward, myRotation) + myPosition;
            Matrix.CreateLookAt(ref myPosition, ref target, ref up, out myView);
            */

            Orientation = MatrixAffineTransformation(1.25f, null, qR, Position);
            ViewMatrix = Matrix.Invert(Orientation);

            // Decelerate only the camera's position velocity (for smooth motion)
            velocity *= 0.9f;

            // Deceleating the yaw/pitch/roll velocities works great for keyboard control,
            // but it gives me motion sickness when used with mouselook, so I commented it out
            // and replaced it with multiplication by zero, which disables the velocities.
            // Your mileage may vary.

            /*
            m_fYawVelocity   *= 0.9f;
            m_fPitchVelocity *= 0.9f;
            */

            yawVelocity *= 0.0f;
            pitchVelocity *= 0.0f;
        }

        public Matrix MatrixAffineTransformation(float scaling, Vector3? rotationCenter, Quaternion rotation, Vector3 translation)
        {
            Matrix centerOfRotationMatrix;

            if (rotationCenter.HasValue)
            {
                centerOfRotationMatrix = Matrix.CreateTranslation(rotationCenter.Value);
            }
            else
            {
                centerOfRotationMatrix = Matrix.Identity;
            }

            // from MSDN:
            // Ms * (Mrc)-1 * Mr * Mrc * Mt, where Ms is the scaling matrix, Mrc is the center of rotation matrix, Mr is the rotation matrix, and Mt is the translation matrix
            var pOut = Matrix.CreateScale(scaling) * Matrix.Invert(centerOfRotationMatrix) * Matrix.CreateFromQuaternion(rotation) * centerOfRotationMatrix * Matrix.CreateTranslation(translation);

            return pOut;
        }
    }
}
