// <copyright file="AnimationFrame.cs" company="Urs Müller">
// </copyright>

namespace SpecialEffectsExamplesLibrary.Animation
{
  using Microsoft.Xna.Framework.Graphics;

  /// <summary>
  /// Class representing a frame for a textured animation
  /// </summary>
  public class AnimationFrame
  {
    /// <summary>
    /// The texture to be drawn in this frame
    /// </summary>
    private readonly Texture2D texture;

    /// <summary>
    /// The animation time
    /// </summary>
    private readonly float time;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnimationFrame"/> class.
    /// </summary>
    /// <param name="texture">The texture.</param>
    /// <param name="time">The time passed.</param>
    public AnimationFrame(Texture2D texture, float time)
    {
      this.time = time;
      this.texture = texture;
    }

    /// <summary>
    /// Gets the texture.
    /// </summary>
    public Texture2D Texture
    {
      get { return texture; }
    }

    /// <summary>
    /// Gets the time.
    /// </summary>
    public float Time
    {
      get { return time; }
    }
  }
}
