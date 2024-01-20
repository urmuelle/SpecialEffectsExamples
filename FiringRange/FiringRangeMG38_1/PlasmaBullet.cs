// <copyright file="PlasmaBullet.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace FiringRangeMG38_1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using SpecialEffectsExamplesLibrary.Animation;

    public class PlasmaBullet : Bullet
    {
        private Sprite sprite;

        public PlasmaBullet()
        {
            sprite = new Sprite();
        }

        public Sprite Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }
    }
}
